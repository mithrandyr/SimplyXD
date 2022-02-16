<Cmdlet(VerbsCommon.[New], "XdBatch")>
<CmdletBinding()>
Public Class NewBatch
    Inherits baseCmdlet

    <Parameter(Mandatory:=True)>
    Public Property BatchGroupId As Guid

    <Parameter()>
    Public Property BatchName As String = String.Format("{0}-{1:yyyyMMdd-HHmmss-fff}", Environment.UserName, Date.Now)

    Protected Overrides Sub EndProcessing()
        Dim nBatch As New Batch With {.BatchGroupId = BatchGroupId, .Status = BatchStatus.Created, .Name = BatchName}
        xdp.AddToBatches(nBatch)
        SaveChanges()
        WriteObject(nBatch)
    End Sub
End Class