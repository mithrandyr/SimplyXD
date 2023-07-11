[cmdletBinding()]
Param([Parameter(Mandatory)][string]$XDPortalURI)
$modulePath = "$PSScriptRoot\SimplyXD"
$moduleTests = "$PSScriptRoot\Tests\*.tests.ps1"

Start-Job -ScriptBlock {
    Import-Module -Name $using:modulePath

    $testConfig = New-PesterConfiguration
    $testConfig.Output.Verbosity = "Detailed"
    $testConfig.Output.StackTraceVerbosity = "Firstline"
    $testConfig.Run.Container = New-PesterContainer -Path $using:moduleTests -Data @{URI=$using:XDPortalURI}
    
    Invoke-Pester -Configuration $testConfig
} | Receive-Job -AutoRemoveJob -Wait
