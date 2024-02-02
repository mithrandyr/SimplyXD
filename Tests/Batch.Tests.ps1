Param([parameter(Mandatory)][string]$Uri)
Describe "Xpertdoc Batch Testing" {
    BeforeAll {
        Connect-XDPortal -PortalUri $Uri
        $shared = @{}
    }
    It "New-XdBatchGroup" {
        $shared.BatchGroup = New-XdBatchGroup -Name "pestertest" -Description "for pester testing, can be removed"
    }
    
    It "Get-XdBatchGroup" {
        Get-XdBatchGroup -Name "pestertest" | Should -Not -BeNullOrEmpty
        Get-XdBatchGroup "pestertest" | Should -Not -BeNullOrEmpty
        Get-XdBatchGroup -BatchGroupId $shared.BatchGroup.BatchGroupId | Should -Not -BeNullOrEmpty
    }
    
    It "New-XdBatch" {
        $shared.Batch = New-XdBatch -BatchGroupId $shared.BatchGroup.BatchGroupId
    }

    It "Get-XdBatch" {
        $shared.BatchGroup | Get-XdBatch | Should -Not -BeNullOrEmpty
        $shared.BatchGroup | Get-XdBatch -Status Created | Should -Not -BeNullOrEmpty
        $shared.BatchGroup | Get-XdBatch -Status Completed | Should -BeNullOrEmpty
        Get-XdBatch -BatchId $shared.Batch.BatchId | Should -Not -BeNullOrEmpty
    }

    It "Start-XdBatch" {
        $shared.Batch | Start-XdBatch | Should -Not -BeNullOrEmpty
    }

    It "Remove-XdBatch" {
        {$shared.Batch | Remove-XdBatch} | Should -Not -Throw
    }

    It "Remove-XdBatchGroup" {
        $shared.Batch = New-XdBatch -BatchGroupId $shared.BatchGroup.BatchGroupId
        { Remove-XdBatchGroup -Name $shared.BatchGroup.Name -ErrorAction Stop} | Should -Throw
        Remove-XdBatchGroup -BatchGroupId $shared.BatchGroup.BatchGroupId -Force
    }
    

}