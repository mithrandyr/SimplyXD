<Cmdlet(VerbsLifecycle.Start, "XdBatch")>
<CmdletBinding()>
Public Class StartXdBatch
    Inherits baseCmdlet

    <Parameter(Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
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
                Try
                    Dim nbatch As Batch = xdp.Batches.Context.Entities.FirstOrDefault(Function(x) CType(x.Entity, Batch).BatchId = BatchId)?.Entity

                    If nbatch Is Nothing Then
                        nbatch = New Batch With {.BatchId = BatchId}
                        xdp.AttachTo("Batches", nbatch)
                    End If

                    nbatch.Execute.ExecuteAsync()
                    WriteObject(BatchId)
                Catch ex As Exception
                    Throw New InvalidOperationException(String.Format("Failed to Start Batch: ''", BatchId), ex)
                End Try
                s += 1
            Next
            WriteProgress(New ProgressRecord(0, MyInvocation.MyCommand.Name, String.Format("{0} out of {1}", s, batchlistCount)) With {.PercentComplete = s * 100 / batchlistCount})
            SaveChanges("Batch(es) may not be started")
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

