Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations

<Cmdlet(VerbsCommon.Get, "XdBatchError")>
<CmdletBinding(DefaultParameterSetName:="search")>
Public Class Get_XdBatchError
    Inherits baseCmdlet

    <Parameter(ParameterSetName:="search")>
    Public Property ErrorText As String

    <Parameter(ParameterSetName:="search", ValueFromPipelineByPropertyName:=True)>
    Public Property BatchGroupId As Guid

    <Parameter(Mandatory:=True, ParameterSetName:="batch", Position:=0, ValueFromPipeline:=True)>
    Public Property BatchId As Guid

    <ValidateRange(0, 1000)>
    <Parameter()>
    Public Property Limit As Integer = 0

    Protected Overrides Sub ProcessRecord()
        Dim query = xdp.DocumentProviders.Expand(Function(x) x.Document.Batch.BatchGroup).Where(Function(x) x.Document.Batch.Status = BatchStatus.Error).AsQueryable
        If Not BatchId.Equals(Guid.Empty) Then
            query = query.Where(Function(x) x.Document.Batch.BatchId = BatchId)
        Else
            If Not BatchGroupId.Equals(Guid.Empty) Then
                query = query.Where(Function(x) x.Document.Batch.BatchGroupId = BatchGroupId)
            End If
            If Not String.IsNullOrWhiteSpace(ErrorText) Then
                query = query.Where(Function(x) x.ErrorMessage.Contains(ErrorText))
            End If
        End If

        For Each gr In GenerateResults(query, "BatchErrors", Limit)
            Output(gr)
        Next

    End Sub

    Private Sub Output(dp As DocumentProvider)
        Dim displayProps As String() = {"BatchGroupName", "BatchName", "ErrorMessage", "ErrorDateTime", "TemplateLibraryName", "TemplateGroupName", "TemplateName"}

        WriteObject(BatchErrorInfo.Transform(dp).AsPSObject.AddDefaultPropertySet(displayProps))
    End Sub

    Private Class BatchErrorInfo
        Public Property BatchGroupId As Guid
        Public Property BatchGroupName As String
        Public Property BatchId As Guid
        Public Property BatchName As String
        Public Property DocumentId As Guid
        Public Property ErrorMessage As String
        Public Property ErrorDateTime As DateTimeOffset
        Public Property TemplateLibraryName As String
        Public Property TemplateGroupName As String
        Public Property TemplateName As String
        Public Property XmlData As XDocument

        Shared Function Transform(input As DocumentProvider) As BatchErrorInfo
            Dim bei As New BatchErrorInfo With {
                .BatchGroupId = input.Document.Batch.BatchGroupId,
                .BatchGroupName = input.Document.Batch.BatchGroup.Name,
                .BatchId = input.Document.BatchId,
                .BatchName = input.Document.Batch.Name,
                .DocumentId = input.DocumentId,
                .ErrorMessage = input.ErrorMessage,
                .ErrorDateTime = input.EndTime
            }

            With XDocument.Parse(input.InputMetadata).Root
                bei.TemplateLibraryName = .Element("TemplateLibraryName").Value
                bei.TemplateGroupName = .Element("TemplateGroupName").Value
                bei.TemplateName = .Element("TemplateName").Value
                bei.XmlData = XDocument.Parse(.Element("ExecutionData").Value)
            End With

            Return bei
        End Function
    End Class

End Class
