param([version]$Version = "1.0.0", [switch]$DebugOnly)

if(-not [bool](Get-ChildItem alias:\).where({$_.name -eq "hv"})) {
    New-Alias -Name HV -Value (Resolve-Path ..\HandleVerbose.ps1)
}
$Script:envList = @("win-x64")

task Clean { remove output }

task Build {
    $configuration = "Release"
    if($DebugOnly) { $configuration = "Debug"}
    
    exec { dotnet publish -c $Configuration -o "output" -p:Version=$Version -p:AssemblyVersion=$version} | HV "Building SimplyXD ($version)" "."
}

task . Clean, Build