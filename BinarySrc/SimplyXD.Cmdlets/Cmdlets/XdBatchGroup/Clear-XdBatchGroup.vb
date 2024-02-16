<Cmdlet(VerbsCommon.Clear, "XdBatchGroup")>
<CmdletBinding()>
Public Class ClearBatch
    Inherits baseCmdlet


    <ValidateNotNullOrEmpty>
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
    <ValidateRange(1, 50)>
    Public Property Concurrency As Integer = 10

    Protected Overrides Sub EndProcessing()
        Dim bg = ExecuteWithTimeout(xdp.BatchGroups.Where(Function(x) x.Name.ToUpper().Equals(BatchGroup.ToUpper())).FirstOrDefaultAsync())
        If bg Is Nothing Then
            WriteErrorMissing("BatchGroup", BatchGroup)
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
        Dim taskList As New List(Of Task)
        Dim deleteQueue As New Concurrent.ConcurrentQueue(Of Guid)

        'Task to query for items to delete
        taskList.Add(Task.Run(
            Sub()
                Dim foundList As New List(Of Guid)
                Dim remainingItems = stats.TotalItems
                Dim queryCount = Math.Max(Concurrency * 2, 10)
                Dim shouldPause As Boolean = True
                While remainingItems > 0
                    If token.IsCancellationRequested() Then Exit Sub
                    For Each batchId In GenerateResults(query, Nothing, queryCount).Select(Function(x) x.BatchId).Where(Function(x) Not foundList.Contains(x))
                        If token.IsCancellationRequested() Then Exit Sub
                        foundList.Add(batchId)
                        deleteQueue.Enqueue(batchId)
                        Threading.Interlocked.Increment(stats.Found)
                        If stats.Found >= stats.DeleteLimit Then Exit While
                        shouldPause = False
                    Next
                    If foundList.Count > Math.Max(Concurrency * 10, 20) Then
                        foundList.RemoveRange(0, Concurrency * 4)
                    End If
                    If shouldPause Then
                        Task.Delay(250, token).Wait()
                    Else
                        shouldPause = True
                    End If
                    remainingItems = ExecuteWithTimeout(query.CountAsync())
                End While
            End Sub, token))

        'Tasks to delete items
        'setup threads to run deletes
        For i = 1 To Concurrency
            taskList.Add(Task.Run(Sub()
                                      Dim batchid As Guid
                                      Dim xdp2 = Engine.NewXDP
                                      While (stats.Deleted + stats.Skipped) < stats.DeleteLimit
                                          If deleteQueue.TryDequeue(batchid) Then
                                              Dim nbatch = New Batch With {.BatchId = batchid}
                                              xdp2.AttachTo("Batches", nbatch)
                                              xdp2.DeleteObject(nbatch)
                                              If token.IsCancellationRequested Then Exit Sub
                                              Try
                                                  xdp2.SaveChangesAsync.Wait()
                                                  Threading.Interlocked.Increment(stats.Deleted)
                                              Catch
                                                  Threading.Interlocked.Increment(stats.Skipped)
                                              End Try
                                          Else
                                              Task.Delay(250, token).Wait()
                                          End If
                                          If token.IsCancellationRequested Then Exit Sub
                                      End While
                                  End Sub, token))
        Next

        Try
            Console.TreatControlCAsInput = True
            While taskList.Any(Function(x) x.Status = TaskStatus.Running)
                Dim pRecord = New ProgressRecord(1, $"Cleaning Batches from {bg.Name}", $"{stats.Deleted} out of {DeleteLimit}")
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
                Threading.Thread.Sleep(500)
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
        query = query.Where(Function(x) x.BatchGroupId = batchgroupId)
        If MinimumAgeInMinutes > 0 Then query = query.Where(Function(x) x.CreatedDate <= minimumAge)

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
        Public Skipped As Integer
    End Class
End Class