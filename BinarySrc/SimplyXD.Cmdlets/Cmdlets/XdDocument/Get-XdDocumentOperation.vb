<Cmdlet(VerbsCommon.Get, "XdDocumentOperation")>
<CmdletBinding()>
Public Class Get_XdDocumentOperation
    Inherits baseCmdlet

    <Parameter(Mandatory:=True, ParameterSetName:="id", ValueFromPipeline:=True, ValueFromPipelineByPropertyName:=True)>
    Public Property DocumentId As Guid
    Protected Overrides Sub ProcessRecord()
        Dim query = xdp.DocumentOperations.Where(Function(dp) dp.DocumentId = DocumentId).AsQueryable
        For Each gr In GenerateResults(query, $"DocumentOperation ({DocumentId})")
            WriteObject(gr)
        Next
    End Sub
End Class
