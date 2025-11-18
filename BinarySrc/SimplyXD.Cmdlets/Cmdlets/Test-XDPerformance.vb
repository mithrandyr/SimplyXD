'This is not complete!
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Management.Automation.Language
Imports System.Threading

<Cmdlet(VerbsDiagnostic.Test, "XDPerformance")>
<CmdletBinding()>
Public Class Test_XDPerformance
    Inherits baseCmdlet

#Region "Parameters"
    <Parameter(Mandatory:=True)>
    Public Property TemplateLibraryName As String

    <Parameter(Mandatory:=True)>
    Public Property TemplateGroupName As String

    <Parameter(Mandatory:=True)>
    Public Property TemplateName As String

    <Parameter()>
    Public Property BatchGroupName As String = "PerformanceTest"

    <Parameter(Mandatory:=True, ValueFromPipeline:=True)>
    Public Property XmlData As String()

    <Parameter()>
    Public Property NumThreads As Integer = 8

    <Parameter()>
    Public Property DocsPerThread As Integer = 100
#End Region

    Private ListData As List(Of String)
    Private cts As CancellationTokenSource

    Protected Overrides Sub BeginProcessing()
        ListData = New List(Of String)
        cts = New CancellationTokenSource
    End Sub
    Protected Overrides Sub ProcessRecord()
        For Each item In XmlData.Where(Function(x) Not String.IsNullOrWhiteSpace(x))
            ListData.Add(item)
        Next
    End Sub
    Protected Overrides Sub EndProcessing()
        Dim readOnlyData = New ReadOnlyCollection(Of String)(ListData)
        Dim batchGroupId = xdp.BatchGroups.Where(Function(x) x.Name = BatchGroupName).First.BatchGroupId
        Dim templateLibraryId = xdp.TemplateLibraries.Where(Function(x) x.Name = TemplateLibraryName).First.TemplateLibraryId
        Dim templateGroupId = xdp.TemplateGroups.Where(Function(x) x.TemplateLibraryId = templateLibraryId And x.Name = TemplateGroupName).First.TemplateGroupId
        Dim templateId = xdp.Templates.Where(Function(x) x.TemplateGroupId = templateGroupId And x.Name = TemplateName).First.TemplateId
        Dim completed As Integer

        Dim tasks As New List(Of Task)

        For Each taskId In Enumerable.Range(0, NumThreads)
            tasks.Add(Task.Run(
                      Sub()
                          Dim xdp2 = Engine.NewXDP
                          For Each item In Enumerable.Range(0, DocsPerThread)
                              If cts.IsCancellationRequested Then Exit Sub

                              'Create the batch
                              Dim b As New Batch With {.BatchGroupId = batchGroupId,
                                                    .Status = BatchStatus.Created,
                                                    .Name = String.Format("Document-{0:d4}-{1:d6}", taskId, item)}
                              xdp2.AddToBatches(b)
                              xdp2.SaveChanges()

                              'Create the document
                              Dim d As New Document With {.BatchId = b.BatchId, .Status = DocumentStatus.Created, .SequenceNumber = 1}
                              xdp2.AddToDocuments(d)
                              xdp2.SaveChanges()


                              Interlocked.Increment(completed)
                          Next
                      End Sub, cts.Token)
                      )
        Next
    End Sub

    Protected Overrides Sub StopProcessing()
        cts?.Cancel()
    End Sub

End Class
