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

    <Parameter(ValueFromPipelineByPropertyName:=True)>
    Public Property IncludeProviders As SwitchParameter

    <Parameter(ValueFromPipelineByPropertyName:=True)>
    Public Property IncludeOperations As SwitchParameter
#End Region

    Protected Overrides Sub ProcessRecord()
        Dim query = xdp.Documents.AsQueryable

        If ParameterSetName = "id" Then
            Try
                Dim d = ExecuteWithTimeout(query.Where(Function(x) x.DocumentId = DocumentId).ExecuteAsync)
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
        If IncludeOutput.IsPresent Or IncludeProviders.IsPresent Or IncludeOperations.IsPresent Then
            Dim pso As New PSObject(doc)
            If IncludeOutput.IsPresent AndAlso doc.Status = DocumentStatus.Completed Then
                pso.Members.Add(New PSNoteProperty("Content", ExecuteWithTimeout(doc.GetOutput.GetValueAsync)))
            End If

            If IncludeProviders.IsPresent Then
                pso.Members.Add(New PSNoteProperty("Providers", ExecuteWithTimeout(xdp.DocumentProviders.Where(Function(x) x.DocumentId = doc.DocumentId).ToListAsync)))
            End If

            If IncludeOperations.IsPresent Then
                pso.Members.Add(New PSNoteProperty("Operations", ExecuteWithTimeout(xdp.DocumentOperations.Where(Function(x) x.DocumentId = doc.DocumentId).ToListAsync)))
            End If

            WriteObject(pso)
        Else
            WriteObject(doc)
        End If
    End Sub

End Class
