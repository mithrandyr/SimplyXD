<Cmdlet(VerbsCommon.[New], "XdBatch")>
<CmdletBinding()>
Public Class NewBatch
    Inherits baseCmdlet

    <Parameter(Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
    Public Property BatchGroupId As Guid

    <Parameter()>
    <[Alias]("BatchName")>
    Public Property Name As String = $"{Environment.UserName}-{Date.Now:yyyyMMdd-HHmmss-fff}"

    <Parameter()>
    Public Property Description As String

    Protected Overrides Sub EndProcessing()
        Dim nBatch As New Batch With {.BatchGroupId = BatchGroupId, .Status = BatchStatus.Created, .Name = Name, .Description = Description}
        xdp.AddToBatches(nBatch)
        If SaveChanges(nBatch) Then WriteObject(nBatch)
    End Sub
End Class