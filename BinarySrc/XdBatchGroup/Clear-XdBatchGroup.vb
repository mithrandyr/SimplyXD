<Cmdlet(VerbsCommon.Clear, "XdBatchGroup")>
<CmdletBinding()>
Public Class ClearBatch
    Inherits baseCmdlet


    <Parameter(Mandatory:=True)>
    Public Property BatchGroup As String

    <Parameter()>
    <ValidateRange(0, 100000)>
    Public Property DeleteLimit As Integer = 0

    <Parameter()>
    <ValidateSet("Ascending", "Descending")>
    Public Property SortByCreation As String

    <Parameter()>
    <ValidateSet("Completed", "Created", "Error", "Queued", "Running", "TimedOut")>
    Public Property Status As String

    <Parameter()>
    <ValidateRange(0, 527400)>
    Public Property MinimumAgeInMinutes As Integer = 15

    <Parameter()>
    <ValidateRange(1, 32)>
    Public Property Concurrency As Integer = Environment.ProcessorCount

    Protected Overrides Sub EndProcessing()
        Dim bg = ExecuteWithTimeout(xdp.BatchGroups.Where(Function(x) x.Name.ToUpper().Equals(BatchGroup.ToUpper())).FirstOrDefaultAsync())
        If bg Is Nothing Then
            WriteError(New ErrorRecord(New ArgumentException(String.Format("Could not find the batchgroup '{0}' to clear.", BatchGroup), "BatchGroup"), Nothing, ErrorCategory.ObjectNotFound, BatchGroup))
            Exit Sub
        End If

        Dim query = GenerateQuery(bg.BatchGroupId)
        Dim TotalItems = ExecuteWithTimeout(query.CountAsync())
        Dim tokenSource = New Threading.CancellationTokenSource
        Dim token = tokenSource.Token

        If TotalItems = 0 Then Exit Sub
        If DeleteLimit = 0 Then DeleteLimit = TotalItems
        Dim timer = Stopwatch.StartNew

        Dim stats As New ClearBatchStats With {.TotalItems = TotalItems, .DeleteLimit = DeleteLimit}

        Dim backgroundTask = Task.Factory.StartNew(
            Sub(xStats As ClearBatchStats)
                Dim foundList As New List(Of Guid)
                Dim remainingItems = xStats.TotalItems
                While remainingItems > 0
                    If token.IsCancellationRequested() Then Exit Sub
                    For Each batchId In query.ExecuteAsync.Result.Select(Function(x) x.BatchId)
                        If Not foundList.Contains(batchId) Then
                            foundList.Add(batchId)
                            Threading.Interlocked.Increment(xStats.Found)

                            Task.Factory.StartNew(
                                Sub(yStats As ClearBatchStats)
                                    Dim xdp2 = XDPortal()
                                    Dim nbatch = New Batch With {.BatchId = batchId}
                                    xdp2.AttachTo("Batches", nbatch)
                                    xdp2.DeleteObject(nbatch)
                                    xdp2.SaveChangesAsync.Wait()
                                    Threading.Interlocked.Increment(yStats.Deleted)
                                End Sub, xStats, token, TaskCreationOptions.AttachedToParent, TaskScheduler.Current)

                            If xStats.Found >= xStats.DeleteLimit Then Exit While
                        End If
                    Next
                    remainingItems = ExecuteWithTimeout(query.CountAsync())
                End While
            End Sub, stats, token)

        Try
            Console.TreatControlCAsInput = True
            While backgroundTask.Status = TaskStatus.Running
                Dim pRecord = New ProgressRecord(1, "Cleaning Batches from " & bg.Name, String.Format("{0} out of {1}", stats.Deleted, DeleteLimit))
                Dim percent As Double = stats.Deleted / DeleteLimit
                pRecord.PercentComplete = percent * 100
                If percent > 0 Then pRecord.SecondsRemaining = (timer.Elapsed.TotalSeconds / percent) - timer.Elapsed.TotalSeconds

                WriteProgress(pRecord)
                While Console.KeyAvailable
                    Dim k = Console.ReadKey(False)
                    If k.Key = ConsoleSpecialKey.ControlC Or (k.Key = ConsoleKey.C And k.Modifiers = ConsoleModifiers.Control) Then
                        tokenSource.Cancel()
                        Exit While
                    End If
                End While
                Threading.Thread.Sleep(1000)
            End While
            If Not tokenSource.IsCancellationRequested Then tokenSource.Cancel()
        Finally
            Console.TreatControlCAsInput = False
            timer.Stop()
            FinishWriteProgress()
            WriteObject(stats)
        End Try

    End Sub

    Private Function GenerateQuery(batchgroupId As Guid) As IQueryable(Of Batch)
        Dim query = xdp.Batches
        Dim minimumAge = Date.Now.AddMinutes(-1 * MinimumAgeInMinutes)
        query = query.Where(Function(x) x.BatchGroupId = batchgroupId And x.CreatedDate <= minimumAge)
        If Not String.IsNullOrWhiteSpace(Status) Then
            Dim enumBatchStatus As BatchStatus = [Enum].Parse(GetType(BatchStatus), Status)
            query = query.Where(Function(x) x.Status = enumBatchStatus)
        End If

        If SortByCreation = "Ascending" Then query = query.OrderBy(Function(x) x.CreatedDate)
        If SortByCreation = "Descending" Then query = query.OrderByDescending(Function(x) x.CreatedDate)
        Return query
    End Function

    Private Class ClearBatchStats
        Public Deleted As Integer
        Public TotalItems As Integer
        Public DeleteLimit As Integer
        Public Found As Integer
    End Class
End Class