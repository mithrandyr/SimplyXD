Public MustInherit Class baseCmdlet
    Inherits PSCmdlet
    Private _xdp As PortalODataContext
    Friend ReadOnly Property xdp As PortalODataContext
        Get
            If _xdp Is Nothing Then _xdp = XDPortal()
            Return _xdp
        End Get
    End Property

    <Parameter(ValueFromPipelineByPropertyName:=True)>
    Public Property TimeOut As Integer = 15

    Public Sub SaveChanges(x As Object)
        SaveChanges(0, x)
    End Sub


    Public Sub SaveChanges(Optional timeoutMS As Integer = 0, Optional x As Object = Nothing)
        If timeoutMS <= 0 Then timeoutMS = TimeOut * 1000
        Using dsr = xdp.SaveChangesAsync()
            Try
                If Not dsr.Wait(timeoutMS) Then
                    If x IsNot Nothing Then xdp.Detach(x)
                    WriteError(New ErrorRecord(New TimeoutException(String.Format("SaveChanges timeout of {0}s exceeded.", (timeoutMS / 1000))), Nothing, ErrorCategory.OperationTimeout, x))
                End If
            Catch ex As Exception
                If x IsNot Nothing Then xdp.Detach(x)
                WriteError(New ErrorRecord(WrappedException.GenerateMessageFromInnermost(ex), Nothing, ErrorCategory.NotSpecified, x))
            End Try
        End Using
    End Sub

    Public Function ExecuteWithTimeout(Of t)(cmd As Task(Of t), Optional timeoutMS As Integer = 0) As t
        If timeoutMS = 0 Then timeoutMS = TimeOut * 1000
        If cmd.Wait(timeoutMS) Then
            Return cmd.Result
        Else
            Throw New TimeoutException(String.Format("Retrieval timeout of {0}s exceeded.", (timeoutMS / 1000)))
        End If
    End Function

    Public Iterator Function GenerateResults(Of t)(cmd As IQueryable(Of t), objectName As String, Optional limit As Integer = 0) As IEnumerable(Of t)
        Dim rCount = ExecuteWithTimeout(cmd.CountAsync())
        Dim activityMessage As String, iteration As Integer = 0

        If limit > 0 AndAlso rCount > limit Then
            activityMessage = String.Format("Getting {0} {1} (out of {2})", limit, objectName, rCount)
        Else
            limit = rCount
            activityMessage = String.Format("Getting {0} {1}", limit, objectName)
        End If

        While iteration < limit
            WriteProgress(New ProgressRecord(0, MyInvocation.MyCommand.Name, activityMessage) With {.CurrentOperation = iteration, .PercentComplete = (iteration * 100) / limit})
            Dim rList = ExecuteWithTimeout(cmd.Skip(iteration).ToListAsync())
            If rList Is Nothing Then Exit While

            For Each r In rList
                Yield r
            Next
            iteration += 10
        End While
        FinishWriteProgress()
    End Function

    Public Sub FinishWriteProgress(Optional ActivityId As Integer = 0)
        WriteProgress(New ProgressRecord(ActivityId, "Finished", "Completed") With {.RecordType = ProgressRecordType.Completed})
    End Sub
End Class
