param([string]$ConfigName = "Debug", [switch]$SkipBuild)
$outputPath = "$PSScriptRoot\SimplyXD\bin"
if(Test-Path $outputPath) { Remove-Item $outputPath -Recurse -Force }
New-Item -Path $outputPath -ItemType Directory | Out-Null

if(-not $SkipBuild) {
    Push-Location "$PSScriptRoot\BinarySrc"
    dotnet clean
    dotnet build
    dotnet publish
    Pop-Location
}

Write-Host
Write-Host "Copying dlls to '$outputPath'..."
Get-ChildItem -Path (Join-Path $PSScriptRoot "BinarySrc\bin\$ConfigName\netstandard2.0\publish") -Filter *.dll -File | 
    Foreach-Object {
        Write-Host "  $($_.Name)"
        $_ | Copy-Item -Destination $outputPath
    }