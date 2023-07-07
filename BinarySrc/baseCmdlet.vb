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

    Public Function SaveChanges(x As Object) As Boolean
        Return SaveChanges(0, x)
    End Function


    Public Function SaveChanges(Optional timeoutMS As Integer = 0, Optional x As Object = Nothing) As Boolean
        If timeoutMS <= 0 Then timeoutMS = TimeOut * 1000
        Using dsr = xdp.SaveChangesAsync()
            Try
                If Not dsr.Wait(timeoutMS) Then
                    If x IsNot Nothing Then xdp.Detach(x)
                    WriteError(New ErrorRecord(New TimeoutException(String.Format("SaveChanges timeout of {0}s exceeded.", (timeoutMS / 1000))), Nothing, ErrorCategory.OperationTimeout, x))
                    Return False
                Else
                    Return True
                End If
            Catch ex As Exception
                If x IsNot Nothing Then xdp.Detach(x)
                WriteError(StandardErrors.GenericWrappedError(ex, $"{MyInvocation.MyCommand.Name}[SaveChangesFailed]", x))
                Return False
            End Try
        End Using
    End Function

    Public Function ExecuteWithTimeout(Of t)(cmd As Task(Of t), Optional timeoutMS As Integer = 0) As t
        If timeoutMS = 0 Then timeoutMS = TimeOut * 1000
        Dim result As Boolean

        Try
            result = cmd.Wait(timeoutMS)
        Catch ex As Exception
            Throw WrappedException.GenerateMessageFromInnermost(ex)
        End Try

        If result Then
            Return cmd.Result
        Else
            Throw New TimeoutException(String.Format("Retrieval timeout of {0}s exceeded.", (timeoutMS / 1000)))
        End If
    End Function

    Public Sub ExecuteWithTimeout(group As IList(Of Task), Optional timeoutMS As Integer = 0)
        If timeoutMS = 0 Then timeoutMS = TimeOut * 1000

        Try
            If Not Task.WaitAll(group, timeoutMS) Then
                Throw New TimeoutException(String.Format("Retrieval timeout of {0}s exceeded.", (timeoutMS / 1000)))
            End If
        Catch ex As Exception
            Throw WrappedException.GenerateMessageFromInnermost(ex)
        End Try
    End Sub

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
            If objectName IsNot Nothing Then WriteProgress(New ProgressRecord(0, MyInvocation.MyCommand.Name, activityMessage) With {.CurrentOperation = iteration, .PercentComplete = (iteration * 100) / limit})
            Dim rList = ExecuteWithTimeout(cmd.Skip(iteration).ToListAsync())
            If rList Is Nothing Then Exit While

            For Each r In rList
                Yield r
            Next
            iteration += 10
        End While
        If objectName IsNot Nothing Then FinishWriteProgress()
    End Function

    Public Overloads Sub WriteError(ex As Exception, errId As String, errCategory As ErrorCategory, Optional errObject As Object = Nothing)
        WriteError(New ErrorRecord(ex, errId, errCategory, errObject))
    End Sub

    Public Sub FinishWriteProgress(Optional ActivityId As Integer = 0)
        WriteProgress(New ProgressRecord(ActivityId, "Finished", "Completed") With {.RecordType = ProgressRecordType.Completed})
    End Sub
End Class
