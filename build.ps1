param([string]$ConfigName = "Debug", [switch]$SkipBuild, [switch]$SkipUpdateCmdlets)
$ErrorActionPreference = "stop"
$outputPath = "$PSScriptRoot\SimplyXD\bin"
if(Test-Path $outputPath) { Remove-Item $outputPath -Recurse -Force }
New-Item -Path $outputPath -ItemType Directory | Out-Null

if(-not $SkipBuild) {
    $version = [Version](Import-PowerShellDataFile -Path "$PsScriptRoot\SimplyXD\SimplyXD.psd1")["ModuleVersion"]
    $version = [version]::new($version.Major, $version.Minor, (Get-Date).ToString("yyyy"), $version.Revision + 1).tostring()
    Update-ModuleManifest -Path "$PsScriptRoot\SimplyXD\SimplyXD.psd1" -ModuleVersion $version
    Write-Host "Building Version: $version"
    Push-Location "$PSScriptRoot\BinarySrc"
    dotnet clean
    Invoke-Expression "dotnet build /p:Version=$version /p:AssemblyVersion=$version"
    #dotnet publish
    Pop-Location
}

Write-Host
Write-Host "Copying dlls to '$outputPath'..."
Get-ChildItem -Path (Join-Path $PSScriptRoot "BinarySrc\bin\$ConfigName\netstandard2.0\publish") -Filter *.dll -File | 
    Foreach-Object {
        Write-Host "  $($_.Name)"
        $_ | Copy-Item -Destination $outputPath
    }

if(-not $SkipUpdateCmdlets) {
    Write-Host "Updating Cmdlets in manifest file..."
    $files = Get-ChildItem "$PSScriptRoot\BinarySrc\" -Directory |
        Get-ChildItem  -Filter "*-*.vb" |
        Select-Object -ExpandProperty basename
    
    $cmdletsList = "{0}" -f ($files -join ", ")
    Write-Host "  NEW LIST: $cmdletsList"
    
    Update-ModuleManifest -Path "$PsScriptRoot\SimplyXD\SimplyXD.psd1" -CmdletsToExport $files
    Write-Host
}

Write-Host "Updating Help..."
Import-Module -Name "$PSScriptRoot\SimplyXD"
Update-MarkdownHelpModule -Path .\Docs -RefreshModulePage