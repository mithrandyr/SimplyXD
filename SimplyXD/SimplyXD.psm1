Get-ChildItem "$PSScriptRoot\Public" -Filter "*.ps1" -File | 
    Foreach-Object {
        . $_.FullName
        Export-ModuleMember -Function $_.BaseName
    }