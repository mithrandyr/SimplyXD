param([string]$uri, [int]$count = 500, [int]$throttle = 10)

if($uri) { Connect-XdPortal -PortalUri $uri }

$bg = Get-XDBatchGroup -Name deleteTesting
if(-not $bg) {
    $bg = New-XdBatchGroup -Name deleteTesting
}

$numCnt = $count / $throttle
$status = @{}
$jobList = @()
foreach ($i in 1..$throttle) {
    $jobName = "LoadBatch-$i"
    $status[$jobName] = 0
    $jobList += Start-ThreadJob -Name $jobName -ThrottleLimit $throttle -ScriptBlock {
        param($numCnt, $bgid, $jobName, $status)
        foreach ($i in 1..$numCnt) {
            New-XdBatch -BatchGroupId $bgid -Name "$jobName--$i" | Out-Null
            $status[$jobName] = $i
        }
    } -ArgumentList $numCnt, $bg.BatchGroupId, $jobName, $status
}

while($jobList | Where-Object State -ne Completed) {
    $done = $status.Values | Measure-Object -Sum | ForEach-Object Sum
    Write-Progress -Id 0 -Activity "Loading Batches for Delete testing Performance" -PercentComplete ($done * 100 / $count)
    foreach($j in $jobList) {
        Write-Progress -Id $j.Id -ParentId 0 -Activity $j.Name -PercentComplete ($status[$j.name] * 100 / $numCnt)
    }

    Start-Sleep -Seconds 1
}