Public MustInherit Class baseCmdlet
    Inherits PSCmdlet
    Friend ReadOnly Property xdp As PortalODataContext = Engine.XDP

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
                    WriteError(New TimeoutException($"SaveChanges timeout of {timeoutMS / 1000}s exceeded."), Nothing, ErrorCategory.OperationTimeout, x)
                    Return False
                Else
                    Return True
                End If
            Catch ex As Exception
                If x IsNot Nothing Then xdp.Detach(x)
                WriteErrorWrapped(ex, $"{MyInvocation.MyCommand.Name}[SaveChangesFailed]", x)
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
            Throw New TimeoutException($"Retrieval timeout of {timeoutMS / 1000}s exceeded.")
        End If
    End Function

    Public Sub ExecuteWithTimeout(group As IList(Of Task), Optional timeoutMS As Integer = 0)
        If timeoutMS = 0 Then timeoutMS = TimeOut * 1000

        Try
            If Not Task.WaitAll(group, timeoutMS) Then
                Throw New TimeoutException($"Retrieval timeout of {timeoutMS / 1000}s exceeded.")
            End If
        Catch ex As Exception
            Throw WrappedException.GenerateMessageFromInnermost(ex)
        End Try
    End Sub

    Public Iterator Function GenerateResults(Of t)(cmd As IQueryable(Of t), objectName As String, Optional limit As Integer = 0) As IEnumerable(Of t)
        Dim rCount = ExecuteWithTimeout(cmd.CountAsync())
        Dim activityMessage As String, iteration As Integer = 0

        If limit > 0 AndAlso rCount > limit Then
            activityMessage = $"Getting {limit} {objectName} (out of {rCount})"
        Else
            limit = rCount
            activityMessage = $"Getting {limit} {objectName}"
        End If

        While iteration < limit
            If objectName IsNot Nothing Then WriteProgress(New ProgressRecord(0, MyInvocation.MyCommand.Name, activityMessage) With {.CurrentOperation = iteration, .PercentComplete = (iteration * 100) / limit})
            Dim take = Math.Min(10, limit - iteration)
            Dim rList = ExecuteWithTimeout(cmd.Skip(iteration).Take(take).ToListAsync())
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

    Public Function AppendCount(origObject As Object, cmd As Task(Of Integer), Optional propertyName As String = "Count") As PSObject
        Dim result = ExecuteWithTimeout(cmd)
        Dim newObject = PSObject.AsPSObject(origObject)
        newObject.Properties.Add(New PSNoteProperty(propertyName, result))
        Return newObject
    End Function
End Class
