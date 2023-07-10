<Cmdlet(VerbsCommon.Get, "XdDocument")>
<CmdletBinding(DefaultParameterSetName:="id")>
Public Class Get_XdDocument
    Inherits baseCmdlet

#Region "PowerShell Parameters"
    <Parameter(Mandatory:=True, ParameterSetName:="id", ValueFromPipeline:=True, ValueFromPipelineByPropertyName:=True)>
    Public Property DocumentId As Guid

    <Parameter(Mandatory:=True, ParameterSetName:="batch", ValueFromPipelineByPropertyName:=True)>
    Public Property BatchId As Guid

    <Parameter(ParameterSetName:="batch", ValueFromPipelineByPropertyName:=True)>
    Public Property SequenceNumber As Integer

    <Parameter(ValueFromPipelineByPropertyName:=True)>
    Public Property IncludeOutput As SwitchParameter

#End Region

    Protected Overrides Sub ProcessRecord()
        Dim query = xdp.Documents.Expand(Function(x) x.Batch.BatchGroup).AsQueryable

        If ParameterSetName = "id" Then
            Try
                Dim d = ExecuteWithTimeout(query.Where(Function(x) x.DocumentId = DocumentId).FirstOrDefaultAsync)
                If d IsNot Nothing Then Output(d)
            Catch ex As Exception
                WriteError(StandardErrors.XDPMissing("Document", DocumentId.ToString, ex))
            End Try
        Else
            query = query.Where(Function(x) x.BatchId = BatchId)
            If SequenceNumber > 0 Then query = query.Where(Function(x) x.SequenceNumber = SequenceNumber)

            For Each gr In GenerateResults(query, "Document")
                Output(gr)
            Next
        End If
    End Sub

    Private Sub Output(doc As Document)
        If IncludeOutput.IsPresent And doc.Status = DocumentStatus.Completed Then
            xdp.AttachTo("Documents", doc)
            Dim pso As New PSObject(doc)
            pso.Members.Add(New PSNoteProperty("Content", ExecuteWithTimeout(doc.GetOutput.GetValueAsync)))
            xdp.Detach(doc)
            WriteObject(pso)
        Else
            WriteObject(doc)
        End If
    End Sub

End Class
