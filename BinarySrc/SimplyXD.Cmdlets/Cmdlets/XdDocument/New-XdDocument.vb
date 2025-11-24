<Cmdlet(VerbsCommon.[New], "XdDocument")>
<CmdletBinding()>
Public Class NewXdDocument
    Inherits baseCmdlet

    <Parameter(Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
    Public Property BatchId As String

    <Parameter()>
    Public Property SequenceNumber As Integer = 1

    Protected Overrides Sub EndProcessing()
        Dim doc As New Document With {.BatchId = Guid.Parse(BatchId), .Status = DocumentStatus.Created, .SequenceNumber = SequenceNumber}
        xdp.AddToDocuments(doc)

        If SaveChanges(doc) Then WriteObject(doc)
        xdp.Detach(doc)
    End Sub

End Class
