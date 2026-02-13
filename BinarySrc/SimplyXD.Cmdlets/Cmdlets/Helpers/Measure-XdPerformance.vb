Imports System.Collections.ObjectModel
Imports System.Threading
Imports Microsoft.OData.Client

<Cmdlet(VerbsDiagnostic.Measure, "XDPerformance")>
<CmdletBinding()>
Public Class Measure_XDPerformance
    Inherits baseCmdlet

#Region "Parameters"
    <Parameter(Mandatory:=True)>
    Public Property TemplateLibraryName As String

    <Parameter(Mandatory:=True)>
    Public Property TemplateGroupName As String

    <Parameter(Mandatory:=True)>
    Public Property TemplateName As String

    <Parameter(Mandatory:=True)>
    Public Property BatchGroupName

    <Parameter(Mandatory:=True, ValueFromPipeline:=True)>
    Public Property XmlData As String()

    <ValidateRange(1, 32)>
    <Parameter()>
    Public Property NumThreads As Integer = 8

    <ValidateRange(1, 100000)>
    <Parameter()>
    Public Property DocsPerThread As Integer = 100

    <Parameter()>
    Public Property ConvertToPDF As SwitchParameter

    <Parameter(Mandatory:=True, ParameterSetName:="KeepErrors")>
    Public Property KeepErrors As SwitchParameter

    <Parameter(ParameterSetName:="NoDelete")>
    Public Property NoDelete As SwitchParameter

    <Parameter(ValueFromPipelineByPropertyName:=True)>
    Public Overrides Property TimeOut As Integer = 31
#End Region

    Private listData As List(Of String)
    Private cts As CancellationTokenSource
    Private batchGroupId As Guid
    Private stopCmdlet As Boolean = False

    Protected Overrides Sub BeginProcessing()
        ValidateParameters()
        If stopCmdlet Then Exit Sub

        listData = New List(Of String)
        cts = New CancellationTokenSource
    End Sub
    Protected Overrides Sub ProcessRecord()
        If stopCmdlet Then Exit Sub
        For Each item In XmlData.Where(Function(x) Not String.IsNullOrWhiteSpace(x))
            listData.Add(item)
        Next
    End Sub
    Protected Overrides Sub EndProcessing()
        If stopCmdlet Then Exit Sub
        Dim readOnlyData = New ReadOnlyCollection(Of String)(listData)
        Dim errorBag As New Concurrent.ConcurrentBag(Of String)
        Dim batchExecutionBag As New Concurrent.ConcurrentBag(Of ThreadTiming)
        Dim threadExecutions As Integer
        Dim result = New PerformanceResult With {.NumThreads = NumThreads, .DocsPerThread = DocsPerThread, .StartDT = DateTime.Now()}

        Dim tasks As New List(Of Task)
        For Each taskId In Enumerable.Range(1, NumThreads)
            tasks.Add(Task.Run(
                      Sub()
                          Dim id As Integer
                          Dim threadXDP = Engine.NewXDP
                          threadXDP.MergeOption = Microsoft.OData.Client.MergeOption.OverwriteChanges

                          Dim sw As New Stopwatch, rnd As New Random
                          For Each item In Enumerable.Range(1, DocsPerThread)
                              If cts.IsCancellationRequested Then Exit Sub
                              Dim b As Batch, d As Document, dp As DocumentProvider, dOp As DocumentOperation
                              Try
                                  sw.Restart()
                                  id = Interlocked.Increment(threadExecutions)
                                  'Create the batch
                                  b = New Batch With {.BatchGroupId = batchGroupId,
                                                    .Description = "Created by Measure-XdPerformance",
                                                    .Name = String.Format("Document-{0:d4}-{1:d6}", taskId, item)}
                                  threadXDP.AddToBatches(b)
                                  threadXDP.SaveChangesWithTimeout(TimeOut)

                                  'Create the Document
                                  d = New Document With {.BatchId = b.BatchId, .SequenceNumber = 1}
                                  threadXDP.AddToDocuments(d)
                                  threadXDP.SaveChangesWithTimeout(TimeOut)

                                  'Create and attach Document Provider
                                  dp = New DocumentProvider With {
                                      .ContractName = Constants.DefaultContract,
                                      .DocumentId = d.DocumentId,
                                      .InputMetadata = String.Format(Constants.DefaultMetaData,
                                                                     TemplateLibraryName,
                                                                     TemplateGroupName,
                                                                     TemplateName,
                                                                     $"<![CDATA[{readOnlyData.Item(rnd.Next(readOnlyData.Count))}]]>",
                                                                     Nothing)
                                  }
                                  threadXDP.AddToDocumentProviders(dp)
                                  threadXDP.SaveChangesWithTimeout(TimeOut)

                                  If ConvertToPDF Then
                                      'Create and attach DocumentOperation (convert to pdf)
                                      dOp = New DocumentOperation With {
                                          .DocumentId = d.DocumentId,
                                          .ContractName = Constants.AsposeContract,
                                          .InputMetadata = Constants.AsposeMetaData
                                      }
                                      threadXDP.AddToDocumentOperations(dOp)
                                      threadXDP.SaveChangesWithTimeout(TimeOut)
                                  End If

                                  'Execute batch
                                  Dim batchExecutionResult = b.ExecuteAndWait(TimeOut * 1000).GetValue

                                  If batchExecutionResult.BatchStatus <> BatchStatus.Completed Then
                                      If Not (KeepErrors.IsPresent Or NoDelete.IsPresent) Then
                                          threadXDP.DeleteObject(b)
                                          threadXDP.SaveChangesWithTimeout(TimeOut)
                                      End If
                                  Else
                                      b = threadXDP.Batches.First(Function(x) x.BatchId = b.BatchId)
                                      If Not NoDelete.IsPresent Then
                                          threadXDP.DeleteObject(b)
                                          threadXDP.SaveChangesWithTimeout(TimeOut)
                                      End If
                                  End If
                                  sw.Stop()
                                  batchExecutionBag.Add(New ThreadTiming(id, sw.ElapsedMilliseconds, b.EndTime.Value.Subtract(b.StartTime.Value).TotalMilliseconds))
                              Catch ex As Exception
                                  errorBag.Add($"[{id}] {taskId:d4}-{item:d6} -- {ex.Message}")
                              Finally
                                  For Each entity In {b, d, dp, dOp}
                                      If entity IsNot Nothing Then threadXDP.Detach(entity)
                                  Next
                              End Try
                          Next
                      End Sub, cts.Token)
                      )
        Next

        While True
            Dim completed = errorBag.Count + batchExecutionBag.Count
            WriteProgress(New ProgressRecord(0, "Test-XDPerformance", $"{completed} out of {NumThreads * DocsPerThread}") With {.PercentComplete = completed * 100 / (NumThreads * DocsPerThread)})
            If tasks.All(Function(t) t.IsCompleted) Then
                Exit While
            Else
                Task.Delay(250, cts.Token).Wait()
            End If
        End While

        result.StopDT = DateTime.Now()
        result.WasCanceled = cts.IsCancellationRequested
        result.Errors = errorBag.ToList
        result.Timing = batchExecutionBag.ToList
        FinishWriteProgress()
        cts.Dispose()
        WriteObject(result)
    End Sub

    Protected Overrides Sub StopProcessing()
        cts?.Cancel()
    End Sub

    Private Sub ValidateParameters()
        Dim bgTask = xdp.BatchGroups.Where(Function(x) x.Name.Equals(BatchGroupName)).FirstOrDefaultAsync
        Dim libraryTask = xdp.TemplateLibraries.Where(Function(x) x.Name.Equals(TemplateLibraryName)).FirstOrDefaultAsync
        Dim groupTask = xdp.TemplateGroups.Where(Function(x) x.Name.Equals(TemplateGroupName) And x.TemplateLibrary.Name.Equals(TemplateLibraryName)).FirstOrDefaultAsync
        Dim templateTask = xdp.Templates.Where(Function(x) x.Name.Equals(TemplateName) And x.TemplateGroup.Name.Equals(TemplateGroupName) And x.TemplateGroup.TemplateLibrary.Name.Equals(TemplateLibraryName)).FirstOrDefaultAsync
        Task.WaitAll(bgTask, libraryTask, groupTask, templateTask)

        Dim missingItems As New Dictionary(Of String, String)

        If bgTask.Result Is Nothing Then missingItems.Add("BatchGroup", BatchGroupName)
        If libraryTask.Result Is Nothing Then missingItems.Add("TemplateLibrary", TemplateLibraryName)
        If groupTask.Result Is Nothing Then missingItems.Add("TemplateGroup", TemplateGroupName)
        If templateTask.Result Is Nothing Then missingItems.Add("Template", TemplateName)

        If missingItems.Count = 1 Then
            WriteErrorMissing(missingItems.First.Key, missingItems.First.Value)
            stopCmdlet = True
        ElseIf missingItems.Count > 1 Then
            Dim combinedException As New AggregateException("Multiple items missing from Xpertdoc Portal", missingItems.Select(Function(x) New XDPItemMissingException(x.Key, x.Value)))

            WriteError(combinedException, "XDPortal-MultipleItemsNotFound", ErrorCategory.ObjectNotFound)
            stopCmdlet = True
        Else
            batchGroupId = bgTask.Result.BatchGroupId
        End If
    End Sub

    Private Class PerformanceResult
        Public Property NumThreads As Integer
        Public Property DocsPerThread As Integer
        Public Property StartDT As DateTime
        Public Property StopDT As DateTime
        Public Property WasCanceled As Boolean
        Public Property Timing As List(Of ThreadTiming)
        Public Property Errors As List(Of String)
        Public ReadOnly Property Processed As Integer
            Get
                Return Timing.Count + Errors.Count
            End Get
        End Property

        Public ReadOnly Property AvgRoundTripMs As Integer
            Get
                Return Math.Round(Timing.Average(Function(t) t.RequestTimeMs), 0)
            End Get
        End Property
        Public ReadOnly Property AvgBatchExecutionMs As Integer
            Get
                Return Math.Round(Timing.Average(Function(t) t.BatchExecutionTimeMs), 0)
            End Get
        End Property

        Public ReadOnly Property EstimatedDocsPerMinute As Integer
            Get
                Return Math.Truncate((60000 / AvgRoundTripMs) * NumThreads)
            End Get
        End Property
    End Class
    Private Structure ThreadTiming
        Public Id As Integer
        Public RequestTimeMs As Integer
        Public BatchExecutionTimeMs As Integer
        Public NetworkTimeMs As Integer
        Public Sub New(i As Integer, r As Integer, b As Integer)
            Id = i
            RequestTimeMs = r
            BatchExecutionTimeMs = b
            NetworkTimeMs = r - b
        End Sub
    End Structure
End Class
