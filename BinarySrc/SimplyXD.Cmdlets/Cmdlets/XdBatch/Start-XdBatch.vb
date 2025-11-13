<Cmdlet(VerbsLifecycle.Start, "XdBatch")>
<CmdletBinding()>
Public Class StartXdBatch
    Inherits baseCmdlet

    <Parameter(Mandatory:=True, ValueFromPipelineByPropertyName:=True, Position:=0)>
    Public Property BatchId As Guid

    Protected Overrides Sub ProcessRecord()
        Dim nbatch As Batch
        Try
            nbatch = ExecuteWithTimeout(xdp.Batches.Where(Function(x) x.BatchId = BatchId).FirstOrDefaultAsync)
        Catch ex As Exception
            WriteErrorMissing("Batch", BatchId.ToString, ex)
            Exit Sub
        End Try

        If nbatch.Status = BatchStatus.Created Then
            Try
                xdp.AttachTo("Batches", nbatch)
                nbatch.Execute.ExecuteAsync()
                nbatch.Status = BatchStatus.Queued
                WriteObject(nbatch)
            Catch ex As Exception
                WriteErrorWrapped(ex, "BatchExecutionFailed", ErrorCategory.InvalidOperation, nbatch)
            End Try
        Else
            Dim ioex As New InvalidOperationException($"Batch '{nbatch.BatchId}' has already been executed, status = {nbatch.Status}")
            WriteError(ioex, "BatchExecutionFailed", ErrorCategory.InvalidOperation, nbatch)
        End If
    End Sub
End Class

