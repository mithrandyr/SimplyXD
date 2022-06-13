[cmdletBinding()]
Param([Parameter(Mandatory)][string]$XDPortalURI)

Import-Module -Path "$PSScriptRoot\SimplyXD"

$testConfig = New-PesterConfiguration
$testConfig.Output.Verbosity = "Detailed"
$testConfig.Output.StackTraceVerbosity = "Firstline"
$testConfig.Run.Container = New-PesterContainer -Path "$PSScriptRoot\Tests\*.tests.ps1" -Data @{URI=$XDPortalURI}

Invoke-Pester -Configuration $testConfig