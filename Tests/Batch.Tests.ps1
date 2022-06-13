Param([Parameter(Mandatory)][string]$Uri)
Describe "Xpertdoc Batch Testing" {
    BeforeAll {
        Connect-XDPortal -PortalUri $Uri
        $shared = @{}
    }

    It "Create BatchGroup" {
        
    }

}