param([version]$Version = "1.0.0", [switch]$DebugOnly)

if(-not [bool](Get-ChildItem alias:\).where({$_.name -eq "hv"})) {
    New-Alias -Name HV -Value (Resolve-Path ..\HandleVerbose.ps1)
}
$Script:envList = @("win-x64")

task Clean { remove output }

task Build {
    $configuration = "Release"
    if($DebugOnly) { $configuration = "Debug"}

    exec { dotnet build "SimplyXD.Cmdlets" -c $Configuration -o "output\bin" -p:Version=$Version -p:AssemblyVersion=$version} | HV "Building SimplyXD ($version)" "."
    Move-Item "output\bin\SimplyXD.Cmdlets.dll" -Destination "output"
    
    if(-not $DebugOnly) {
        Remove-Item "output\bin" -Exclude "SimplyXD.Engine.dll" -Recurse
        $Script:envList += @("win-x86", "linux-x64", "osx-x64")
    }

    foreach($env in $Script:envList) {
        exec { dotnet publish "SimplyXD.Cmdlets" -c $Configuration -r $env -o "output\bin\$env"} | HV "Building PlatformSpecific Dependencies $env" "."
        Remove-Item "output\bin\$env" -Include "SimplyXD.*" -Recurse
    }
}

task DeDup {
    if(-not $DebugOnly) {
        $first = $Script:envList[0]
        $safeFiles = @{}

        Get-ChildItem -Path "output\bin\$first" |
            Get-FileHash |
            ForEach-Object {
                $name = $_.Path | Split-Path -Leaf
                $safeFiles[$name] = $_.Hash
            }
        
        Write-Host "  ." -NoNewline
       
        foreach($second in $Script:envList | Select-Object -Skip 1) {
            $retain = $safeFiles.Keys | 
                Where-Object { Test-Path "output\bin\$second\$_" } |
                Where-Object { $safeFiles[$_] -eq (Get-FileHash "output\bin\$second\$_").Hash }
            
            $toRemove = $safeFiles.Keys | Where-Object {$_ -notin $retain}
            $toRemove | ForEach-Object { $safeFiles.remove($_) }
                
            Write-Host "." -NoNewline
        }

        foreach($file in $safeFiles.Keys) {
            Move-Item "output\bin\$first\$file" "output\bin"
            foreach($second in $Script:envList | Select-Object -Skip 1) {
                Remove-Item "output\bin\$second\$file"
            }
        }
        Write-Host
    }
}

task . Clean, Build, DeDup