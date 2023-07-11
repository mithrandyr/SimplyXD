Invoke-Build -DebugOnly

$PSModuleAutoLoadingPreference = "none"
Import-Module .\Output\SimplyXD.dll -Verbose

Clear-Host
function prompt { "SimplyXD Testing> " }
