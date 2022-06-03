param([string]$ConfigName = "Debug", [switch]$SkipBuild)
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