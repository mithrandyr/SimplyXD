<Cmdlet(VerbsCommon.Get, "XdDocumentProvider")>
<CmdletBinding()>
Public Class Get_XdDocumentProvider
    Inherits baseCmdlet

    <Parameter(Mandatory:=True, ParameterSetName:="id", ValueFromPipelineByPropertyName:=True)>
    Public Property DocumentId As Guid
    Protected Overrides Sub ProcessRecord()
        Dim query = xdp.DocumentProviders.Where(Function(dp) dp.DocumentId = DocumentId).AsQueryable
        For Each gr In GenerateResults(query, $"DocumentProvider ({DocumentId})")
            WriteObject(gr)
        Next
    End Sub
End Class
