Imports System.Threading
Imports Xpertdoc.Portal.SdkCore

Public Class XDPortal
    Private Shared _XDP As PortalODataContext
    Private Shared _TimeoutS As Integer
    Friend Shared ReadOnly Property TimeoutS As Integer
        Get
            Return _TimeoutS
        End Get
    End Property
    Friend Shared ReadOnly Property XDP As PortalODataContext
        Get
            If _XDP Is Nothing Then
                Throw New InvalidOperationException("No Xpertdoc Portal connected, please use 'Connect-XdPortal'.")
            Else
                Return _XDP
            End If
        End Get
    End Property

    Public Shared Sub Connect(PortalUri As String, Optional timeoutSeconds As Integer = 30)
        _TimeoutS = timeoutSeconds
        _XDP = New PortalODataContext(PortalUri) With {.Credentials = Net.CredentialCache.DefaultCredentials, .MergeOption = Microsoft.OData.Client.MergeOption.NoTracking}
    End Sub

    Friend Shared Function ExecuteWithTimeout(Of t)(cmd As Task(Of t), Optional wait As Integer = 0) As t
        If wait = 0 Then wait = TimeoutS * 1000

        Try
            If cmd.Wait(wait) Then
                Return cmd.Result
            Else
                Throw New TimeoutException($"Retrieval timeout of {wait / 1000}s exceeded.")
            End If
        Catch ex As TimeoutException
            Throw
        Catch ex As Exception
            Throw WrappedException.GenerateMessageFromInnermost(ex)
        End Try
    End Function

    Friend Shared Sub ExecuteWithTimeout(group As IList(Of Task), Optional wait As Integer = 0)
        If wait = 0 Then wait = TimeoutS * 1000

        Try
            If Not Task.WaitAll(group, wait) Then
                Throw New TimeoutException($"Retrieval timeout of {wait / 1000}s exceeded.")
            End If
        Catch ex As Exception
            Throw WrappedException.GenerateMessageFromInnermost(ex)
        End Try
    End Sub

    Friend Shared Iterator Function GenerateResults(Of t)(cmd As IQueryable(Of t), objectName As String, Optional limit As Integer = 0) As IEnumerable(Of t)
        Dim rCount = ExecuteWithTimeout(cmd.CountAsync())
        Dim activityMessage As String, iteration As Integer = 0

        If limit > 0 AndAlso rCount > limit Then
            activityMessage = $"Getting {limit} {objectName} (out of {rCount})"
        Else
            limit = rCount
            activityMessage = $"Getting {limit} {objectName}"
        End If

        While iteration < limit
            'replace this with delegate/action that can be invoked
            'If objectName IsNot Nothing Then WriteProgress(New ProgressRecord(0, MyInvocation.MyCommand.Name, activityMessage) With {.CurrentOperation = iteration, .PercentComplete = (iteration * 100) / limit})
            Dim rList = ExecuteWithTimeout(cmd.Skip(iteration).ToListAsync())
            If rList Is Nothing Then Exit While

            For Each r In rList
                Yield r
            Next
            iteration += 10
        End While
        'If objectName IsNot Nothing Then FinishWriteProgress()
    End Function

    Friend Shared Function SaveChanges(Optional wait As Integer = 0, Optional x As Object = Nothing) As Boolean
        If wait <= 0 Then wait = TimeoutS * 1000
        Using dsr = XDP.SaveChangesAsync()
            Try
                If Not dsr.Wait(wait) Then
                    If x IsNot Nothing Then XDP.Detach(x)
                    Throw New TimeoutException($"SaveChanges timeout of {wait / 1000}s exceeded.")
                Else
                    Return True
                End If
            Catch ex As TimeoutException
                Throw
            Catch ex As Exception
                If x IsNot Nothing Then XDP.Detach(x)
                Throw WrappedException.GenerateMessageFromInnermost(ex)
            End Try
        End Using
    End Function
End Class
