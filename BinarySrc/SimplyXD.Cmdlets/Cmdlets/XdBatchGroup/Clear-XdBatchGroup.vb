Imports System.Collections.Concurrent
Imports System.Management.Automation.Language

<Cmdlet(VerbsCommon.Clear, "XdBatchGroup")>
<CmdletBinding()>
Public Class ClearBatch
    Inherits baseCmdlet


    <Parameter(Mandatory:=True, ParameterSetName:="id", ValueFromPipelineByPropertyName:=True)>
    Public Property BatchGroupId As Guid

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="name", Position:=0)>
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

    Protected Overrides Sub ProcessRecord()
        Dim bgQuery = xdp.BatchGroups

        If ParameterSetName = "name" Then
            bgQuery = bgQuery.Where(Function(x) x.Name.ToUpper().Equals(BatchGroup.ToUpper()))
        Else
            bgQuery = bgQuery.Where(Function(x) x.BatchGroupId = BatchGroupId)
        End If

        Dim bg = ExecuteWithTimeout(bgQuery.FirstOrDefaultAsync())
        If bg Is Nothing Then
            WriteErrorMissing("BatchGroup", If(BatchGroup, BatchGroupId.ToString))
            Exit Sub
        End If

        Dim query = GenerateQuery(bg.BatchGroupId)
        Dim TotalItems = ExecuteWithTimeout(query.CountAsync())
        If TotalItems = 0 Then Exit Sub
        If DeleteLimit = 0 Then DeleteLimit = TotalItems

        Dim tokenSource = New Threading.CancellationTokenSource
        Dim token = tokenSource.Token
        Dim timer = Stopwatch.StartNew
        Dim stats As New ClearBatchStats With {.TotalItems = TotalItems, .DeleteLimit = DeleteLimit}
        Dim taskList As New List(Of Task)
        Dim deleteQueue As New BlockingCollection(Of Guid)
        Dim found As New HashSet(Of Guid)
        Dim errorList As New HashSet(Of String)

        'Task to query for items to delete
        taskList.Add(Task.Run(
            Sub()
                Dim remainingItems = stats.TotalItems
                Dim queryCount = Math.Max(Concurrency * 2, 10)
                Try
                    While remainingItems > 0
                        If token.IsCancellationRequested() Then Exit Sub
                        'make sure queue has enough items in it
                        If deleteQueue.Count < queryCount Then
                            For Each batchId In GenerateResults(query, Nothing, queryCount).Select(Function(x) x.BatchId).Where(Function(x) Not found.Contains(x))
                                If token.IsCancellationRequested() Then Exit Sub
                                deleteQueue.Add(batchId)
                                found.Add(batchId)
                                stats.Found += 1
                                If stats.Found >= stats.DeleteLimit Then Exit While
                            Next
                        Else
                            Task.Delay(250, token).Wait()
                        End If
                        remainingItems = ExecuteWithTimeout(query.CountAsync())
                    End While
                Catch ex As Exception
                    Dim errString As String = $"Could not retrieve more batches{vbNewLine} -- {ex.Message}"
                    If ex.InnerException IsNot Nothing Then errString += $"{vbNewLine} -- {ex.InnerException.Message}"
                    errorList.Add(errString)
                Finally
                    deleteQueue.CompleteAdding()
                End Try
            End Sub, token))

        'setup threads to run deletes
        For i = 1 To Concurrency
            taskList.Add(Task.Run(Sub()
                                      Dim batchid As Guid
                                      Dim xdp2 = Engine.NewXDP
                                      For Each batchid In deleteQueue.GetConsumingEnumerable()
                                          If token.IsCancellationRequested Then Exit Sub
                                          Dim nbatch = New Batch With {.BatchId = batchid}
                                          xdp2.AttachTo("Batches", nbatch)
                                          xdp2.DeleteObject(nbatch)
                                          Try
                                              xdp2.SaveChangesAsync.Wait(TimeOut)
                                              Threading.Interlocked.Increment(stats.Deleted)
                                          Catch ex As Exception
                                              Dim errString As String = $"Could not remove '{batchid}'{vbNewLine} -- {ex.Message}"
                                              If ex.InnerException IsNot Nothing Then errString += $"{vbNewLine} -- {ex.InnerException.Message}"
                                              errorList.Add(errString)
                                              Threading.Interlocked.Increment(stats.Skipped)
                                          End Try
                                      Next
                                  End Sub, token))
        Next

        Try
            Console.TreatControlCAsInput = True
            While taskList.Any(Function(x) x.Status = TaskStatus.Running)
                Dim pRecord = New ProgressRecord(1, $"Cleaning Batches from {bg.Name}", $"{stats.Deleted} out of {DeleteLimit}")
                Dim percent As Double = (stats.Deleted + stats.Skipped) / DeleteLimit
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
            stats.Finished(timer.Elapsed.TotalSeconds)
            stats.Errors = errorList.ToList
            If ParameterSetName = "name" Then
                WriteObject(stats.AsPSObject.AppendProperty("BatchGroupName", BatchGroup))
            Else
                WriteObject(stats.AsPSObject.AppendProperty("BatchGroupId", BatchGroupId))
            End If
            deleteQueue.Dispose()
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
        'Public BatchGroupId As Guid
        Public Deleted As Integer
        Public TotalItems As Integer
        Public DeleteLimit As Integer
        Public Found As Integer
        Public Skipped As Integer
        Public Errors As List(Of String)
        Public ReadOnly Property ElapsedSeconds As Double
        Public ReadOnly Property ProcessedPerSecond As Double
            Get
                If ElapsedSeconds > 0 Then
                    Return Math.Round((Deleted + Skipped) / ElapsedSeconds, 2)
                Else
                    Return 0
                End If
            End Get
        End Property
        Protected Friend Sub Finished(timeSeconds As Double)
            _ElapsedSeconds = Math.Round(timeSeconds, 2)
        End Sub
    End Class
End Class