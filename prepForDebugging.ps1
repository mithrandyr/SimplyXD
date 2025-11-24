$parentId = Get-CimInstance win32_process -Filter "processid = $pid" | Select-Object -ExpandProperty ParentProcessId

if((Get-Process -Id $parentId).Name -eq "powershell") {
    if(Get-Module SimplyXD) {
        Write-Warning "Already in a nested PowerShell process!!!"
    } else {
        $rootPath = Split-Path $PSCommandPath -Parent
        Join-Path $rootPath -ChildPath "BinarySrc" | Push-Location
        Invoke-Build -DebugOnly
        Pop-Location
        Import-Module (Join-Path $rootPath .\BinarySrc\output\SimplyXD.dll)
        function prompt { "SimplyXD Testing> " }
        [System.Diagnostics.Debugger]::Launch()
    }    
} else {
    powershell -noexit -noprofile -nologo -file $PSCommandPath
    Write-Warning "Has left nested PowerShell session for SimplyXD testing"
}