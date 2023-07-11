<Cmdlet(VerbsCommon.Remove, "XdBatch")>
<CmdletBinding()>
Public Class RemoveBatch
    Inherits baseCmdlet

    <Parameter(Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
    Public Property BatchId As Guid

    Dim batchList As New List(Of Guid)

    Protected Overrides Sub ProcessRecord()
        batchList.Add(BatchId)
    End Sub

    Protected Overrides Sub EndProcessing()
        Dim s As Integer = 0
        For Each b In batchList
            WriteVerbose($"BatchId: {b}")
            Dim nbatch = New Batch With {.BatchId = b}
            xdp.AttachTo("Batches", nbatch)
            xdp.DeleteObject(nbatch)
            WriteProgress(New ProgressRecord(0, MyInvocation.MyCommand.Name, $"Id:{b} | Total: {batchList.Count}") With {.PercentComplete = s * 100 / batchList.Count})
            SaveChanges(nbatch)

            s += 1
        Next
        FinishWriteProgress()
    End Sub
End Class