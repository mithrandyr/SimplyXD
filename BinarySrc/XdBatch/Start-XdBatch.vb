Imports Microsoft.OData.Client

<Cmdlet(VerbsLifecycle.Start, "XdBatch")>
<CmdletBinding()>
Public Class StartXdBatch
    Inherits baseCmdlet

    <Parameter(Mandatory:=True, ValueFromPipelineByPropertyName:=True, Position:=0)>
    Public Property BatchId As Guid

    Public Property BatchSize As Integer = 1

    Dim batchList As New List(Of Guid)

    Protected Overrides Sub ProcessRecord()
        batchList.Add(BatchId)
    End Sub
    Protected Overrides Sub EndProcessing()
        Dim s As Integer = 0
        Dim batchlistCount = batchList.Count
        While s < batchlistCount
            For Each b In batchList.Skip(s).Take(BatchSize)
                Dim nbatch As Batch
                Try
                    nbatch = ExecuteWithTimeout(xdp.Batches.Where(Function(x) x.BatchId = BatchId).FirstOrDefaultAsync)
                    xdp.AttachTo("Batches", nbatch)
                Catch ex As Exception
                    WriteError(StandardErrors.XDPMissing("Batch", BatchId.ToString, ex))
                End Try

                If nbatch IsNot Nothing Then
                    If nbatch.Status = BatchStatus.Created Then
                        Try
                            nbatch.Execute.ExecuteAsync()
                            nbatch.Status = BatchStatus.Queued
                            WriteObject(nbatch)
                        Catch ex As Exception
                            WriteError(New ErrorRecord(ex, "BatchExecutionFailed", ErrorCategory.InvalidOperation, nbatch))
                        End Try
                    Else
                        Dim ioex As New InvalidOperationException($"Batch '{b}' has already been executed, status = {nbatch.Status}")
                        WriteError(New ErrorRecord(ioex, "BatchExecutionFailed", ErrorCategory.InvalidOperation, nbatch))
                    End If
                End If
                s += 1
            Next
            WriteProgress(New ProgressRecord(0, MyInvocation.MyCommand.Name, String.Format("{0} out of {1}", s, batchlistCount)) With {.PercentComplete = s * 100 / batchlistCount})
            'SaveChanges("Batch(es) may not be started")
        End While

        FinishWriteProgress()
    End Sub

    ' old HTTP processs
    'Protected Overrides Sub EndProcessing()
    '    Using http As New System.Net.Http.HttpClient(New Net.Http.HttpClientHandler With {.UseDefaultCredentials = True}) With {.Timeout = New TimeSpan(0, 0, 5)}
    '        Dim executeUri = String.Format("{0}/Batches({1})/Execute", PortalUri, BatchId)
    '        Dim r = http.PostAsync(executeUri, Nothing)
    '        If Not r.Wait(5000) Then
    '            WriteWarning("Sending Execute Batch request took more than 5 seconds, batch may not have executed.")
    '        Else
    '            If r.IsFaulted Then
    '                WriteError(New ErrorRecord(r.Exception, Nothing, Nothing, Nothing))
    '            ElseIf Not r.Result.IsSuccessStatusCode Then
    '                WriteError(New ErrorRecord(New InvalidOperationException(String.Format("{0} : {1}", r.Result.StatusCode, r.Result.ReasonPhrase)), Nothing, Nothing, r.Result))
    '            End If
    '        End If
    '    End Using
    'End Sub
End Class

