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
        Dim ItemsToDelete As New Concurrent.ConcurrentQueue(Of Guid)
        Dim ItemsDeleted As New Concurrent.ConcurrentBag(Of Guid)
        Dim tokenSource = New Threading.CancellationTokenSource
        Dim token = tokenSource.Token

        If TotalItems = 0 Then Exit Sub
        If DeleteLimit = 0 Then DeleteLimit = TotalItems
        Dim tasksDeleteBatch As New List(Of Task)
        Dim taskListBatches = Task.Run(Sub()
                                           Dim foundList As New List(Of Guid)
                                           Dim remainingItems = TotalItems
                                           While remainingItems > 0
                                               If token.IsCancellationRequested() Then Exit Sub
                                               For Each batchId In query.ExecuteAsync.Result.Select(Function(x) x.BatchId)
                                                   If Not foundList.Contains(batchId) Then
                                                       foundList.Add(batchId)
                                                       ItemsToDelete.Enqueue(batchId)
                                                       If foundList.Count >= DeleteLimit Then Exit While
                                                   End If
                                               Next
                                               remainingItems = ExecuteWithTimeout(query.CountAsync())
                                           End While
                                       End Sub, token)

        If DeleteLimit < Concurrency Then Concurrency = DeleteLimit / 2
        For c = 1 To Math.Max(Concurrency, 1)
            tasksDeleteBatch.Add(Task.Run(Sub()
                                              Dim xdp2 = XDPortal()
                                              While Not token.IsCancellationRequested()
                                                  Dim BatchId As Guid
                                                  If ItemsToDelete.TryDequeue(BatchId) Then
                                                      Dim nbatch = New Batch With {.BatchId = BatchId}
                                                      xdp2.AttachTo("Batches", nbatch)
                                                      xdp2.DeleteObject(nbatch)
                                                      xdp2.SaveChangesAsync.Wait()
                                                      xdp2.Detach(nbatch)
                                                      ItemsDeleted.Add(BatchId)
                                                  Else
                                                      Task.Delay(25)
                                                  End If
                                              End While
                                          End Sub, token))
        Next
        Try
            Console.TreatControlCAsInput = True
            Dim IsRunning = True
            While IsRunning
                IsRunning = taskListBatches.Status = TaskStatus.Running OrElse ItemsDeleted.Count < DeleteLimit
                WriteProgress(New ProgressRecord(1, "Cleaning Batches from " & bg.Name, String.Format("{0} out of {1}", ItemsDeleted.Count, DeleteLimit)) With {.PercentComplete = ItemsDeleted.Count * 100 / DeleteLimit})
                Task.Delay(1000)
                While Console.KeyAvailable
                    Dim k = Console.ReadKey(False)
                    If k.Key = ConsoleSpecialKey.ControlC Or (k.Key = ConsoleKey.C And k.Modifiers = ConsoleModifiers.Control) Then
                        tokenSource.Cancel()
                        IsRunning = False
                        Exit While
                    End If
                End While
            End While
            If Not tokenSource.IsCancellationRequested Then tokenSource.Cancel()
        Finally
            Console.TreatControlCAsInput = False
            FinishWriteProgress()
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

End Class