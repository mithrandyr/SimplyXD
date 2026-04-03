'Not IN use
Imports Microsoft.OData.Edm

'<Cmdlet(VerbsDiagnostic.Test, "XdPortal")>
'<CmdletBinding()>
Public Class Test_XdPortal
    Inherits baseCmdlet

    Protected Overrides Sub EndProcessing()
        Throw New NotImplementedException("This is not implemented!")

        xdp.MergeOption = Microsoft.OData.Client.MergeOption.OverwriteChanges
        Dim bg = ExecuteWithTimeout(xdp.BatchGroups.Where(Function(x) x.Name.Equals("test")).FirstOrDefaultAsync)
        If bg Is Nothing Then
            bg = New BatchGroup With {.Name = "test"}
            xdp.AddToBatchGroups(bg)
            xdp.SaveChangesWithTimeout(TimeOut)
        End If

        'Create the Batch
        Dim b = New Batch With {
            .BatchGroupId = bg.BatchGroupId,
            .Description = "Created by Test-XdPortal",
            .Name = String.Format("{0}_{1:yyyy-MM-dd_HH:mm:ss.fff}", Environment.UserName, DateTime.Now())
        }
        xdp.AddToBatches(b)
        SaveChanges(b)

        Dim sw = Stopwatch.StartNew
        Dim tor = b.Execute()

        sw.Stop()
        Debug.WriteLine($"elapsed... {sw.ElapsedMilliseconds}ms")
        Dim cnt As Integer = 0
        While cnt < 50
            b = xdp.Batches.First(Function(x) x.BatchId = b.BatchId)
            Debug.WriteLine($"[{cnt}] {b.Status}")
            If (b.Status = BatchStatus.Completed Or b.Status = BatchStatus.Error Or b.Status = BatchStatus.TimedOut) Then Exit While
            cnt += 1
        End While

        xdp.DeleteObject(b)
        xdp.SaveChangesWithTimeout(TimeOut)

    End Sub
End Class
