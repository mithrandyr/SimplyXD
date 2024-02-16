param([switch]$CommitRevision, [switch]$RebuildDocs)
New-Alias -Name HV -Value (Resolve-Path HandleVerbose.ps1)
$moduleName = "SimplyXD"
if(-not $version) {
    $version = [Version](Import-PowerShellDataFile -Path "$moduleName\$moduleName.psd1")["ModuleVersion"]
    $version = [version]::new($version.Major, $version.Minor, $version.Build + 1).tostring()
}

task Clean -If (-not $RebuildDocs) {
    remove "$moduleName\bin"
    remove "$moduleName.dll"
}
task Build -If (-not $RebuildDocs) {
    Invoke-Build -File "BinarySrc\source.build.ps1" -Version $Version
    Get-ChildItem "BinarySrc\output" | Copy-Item -Destination "$moduleName" -Recurse
}

task GenerateDocs {
    Write-Host  "$BuildRoot\$moduleName\$moduleName.dll"
    Start-Job -ScriptBlock {
            Set-Location $using:BuildRoot
            Import-Module ".\$using:moduleName\$using:moduleName.dll" -Verbose:$false
            
            if(-not (Test-Path "Docs")) {
                New-MarkdownHelp -Module $using:moduleName -OutputFolder Docs -AlphabeticParamsOrder -WithModulePage -ErrorAction SilentlyContinue
                New-MarkdownAboutHelp -OutputFolder Docs -AboutName $using:moduleName
            }
            else { Update-MarkdownHelpModule -Path "Docs" -AlphabeticParamsOrder -Force -RefreshModulePage -ErrorAction SilentlyContinue }
            
        } |
        Receive-Job -Wait -AutoRemoveJob | write-host
        #ForEach-Object { "  $($_.Name)" } |
        #HV "Generating Module Documentation" "."
}

task updateManifest {
    Import-Module PowerShellGet -Verbose:$false
    $cmdlets = Start-Job -ScriptBlock {
            Set-Location $using:BuildRoot
            Import-Module ".\$using:moduleName\$using:moduleName.dll"    
            Get-Command -Module $using:moduleName
        } |
        Receive-Job -AutoRemoveJob -wait |
        Sort-Object name |
        ForEach-Object name
    
    $cmdlets += Get-ChildItem "$moduleName\Public" -Filter "*-*.ps1" | Select-Object -ExpandProperty basename
    Update-ModuleManifest -Path "$moduleName\$moduleName.psd1" -ModuleVersion $version -CmdletsToExport $cmdlets
} -If (-not $RebuildDocs)

task revisionCommit {
    exec { git commit "$moduleName/$moduleName.psd1" -m "Updating version To $version" } | HV "Incrementing Version ($version) and Git Commit"
} -If ($CommitRevision -and -not $RebuildDocs)


task . Clean, Build, updateManifest, GenerateDocs, revisionCommit