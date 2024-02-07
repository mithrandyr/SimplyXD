param([version]$Version = "1.0.0", [switch]$DebugOnly)

if(-not [bool](Get-ChildItem alias:\).where({$_.name -eq "hv"})) {
    New-Alias -Name HV -Value (Resolve-Path ..\HandleVerbose.ps1)
}
$Script:envList = @("win-x64")

task Clean { remove output }

task Build {
    $configuration = "Release"
    if($DebugOnly) { $configuration = "Debug"}
    
    exec { dotnet publish -c $Configuration -o "output\bin" -p:Version=$Version -p:AssemblyVersion=$version} | HV "Building SimplyXD ($version)" "."

    if(-not $DebugOnly) { $Script:envList += @("win-x86", "linux-x64", "osx-x64")}
    foreach($env in $Script:envList) {
        #exec { dotnet publish "SimplySql.Cmdlets" -c $Configuration -r $env -o "output\bin\$env"} | HV "Building PlatformSpecific Dependencies $env" "."
        #Remove-Item "output\bin\$env" -Include "SimplySql.*" -Recurse        
    }
}

task . Clean, Build