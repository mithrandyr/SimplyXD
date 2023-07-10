<Cmdlet(VerbsLifecycle.Start, "XdBatch")>
<CmdletBinding()>
Public Class StartXdBatch
    Inherits baseCmdlet

    <Parameter(Mandatory:=True, ValueFromPipelineByPropertyName:=True, Position:=0)>
    Public Property BatchId As Guid

    Dim batchList As New List(Of Guid)

    Protected Overrides Sub ProcessRecord()
        Dim nbatch As Batch
        Try
            nbatch = ExecuteWithTimeout(xdp.Batches.Where(Function(x) x.BatchId = BatchId).FirstOrDefaultAsync)
        Catch ex As Exception
            WriteErrorMissing("Batch", BatchId.ToString, ex)
        End Try

        If nbatch IsNot Nothing Then
            If nbatch.Status = BatchStatus.Created Then
                Try
                    xdp.AttachTo("Batches", nbatch)
                    nbatch.Execute.ExecuteAsync()
                    nbatch.Status = BatchStatus.Queued
                    WriteObject(nbatch)
                Catch ex As Exception
                    WriteError(New ErrorRecord(ex, "BatchExecutionFailed", ErrorCategory.InvalidOperation, nbatch))
                End Try
            Else
                Dim ioex As New InvalidOperationException($"Batch '{nbatch.BatchId}' has already been executed, status = {nbatch.Status}")
                WriteError(New ErrorRecord(ioex, "BatchExecutionFailed", ErrorCategory.InvalidOperation, nbatch))
            End If
        End If
    End Sub
End Class

