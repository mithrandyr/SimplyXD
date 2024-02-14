Invoke-Build -DebugOnly

$PSModuleAutoLoadingPreference = "none"
Import-Module .\Output\SimplyXD.Cmdlets.dll

#Clear-Host
function prompt { "SimplyXD Testing> " }
