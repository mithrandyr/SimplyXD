Invoke-Build -DebugOnly

$PSModuleAutoLoadingPreference = "none"
Import-Module .\Output\SimplyXD.dll

#Clear-Host
function prompt { "SimplyXD Testing> " }
