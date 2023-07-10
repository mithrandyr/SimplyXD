<Cmdlet(VerbsCommon.[New], "XdBatch")>
<CmdletBinding()>
Public Class NewBatch
    Inherits baseCmdlet

    <Parameter(Mandatory:=True)>
    Public Property BatchGroupId As Guid

    <Parameter()>
    <[Alias]("Name")>
    Public Property BatchName As String = $"{Environment.UserName}-{Date.Now:yyyyMMdd-HHmmss-fff}"

    Protected Overrides Sub EndProcessing()
        Dim nBatch As New Batch With {.BatchGroupId = BatchGroupId, .Status = BatchStatus.Created, .Name = BatchName}
        xdp.AddToBatches(nBatch)
        If SaveChanges(nBatch) Then WriteObject(nBatch)
    End Sub
End Class