Imports System.Runtime.CompilerServices
Imports Microsoft.OData.Client
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Friend Class StandardErrors
    Shared Function TemplateBlobUpdate(ex As Exception, t As Template) As ErrorRecord
        Dim nex = WrappedException.GenerateMessageFromInnermost(ex)
        Return New ErrorRecord(nex, "XDPortal-TemplateContentUpdateFailed", ErrorCategory.InvalidResult, t)
    End Function

    Shared Function GetInnermostException(ex As Exception, Optional recurseLimit As Integer = 15) As Exception
        If ex Is Nothing Then Return Nothing
        Dim depth As Integer = 0
        While ex.InnerException IsNot Nothing And depth < recurseLimit
            ex = ex.InnerException
            depth += 1
        End While
        Return ex
    End Function

    Shared Function GetProviderMessage(ex As Exception) As String
        If TypeOf ex Is DataServiceClientException Then
            Try
                Dim err As JObject = JObject.Parse(ex.Message)("error")
                Return $"[{err("code")}] {err("message")}"
            Catch jex As JsonReaderException
                Return ex.Message
            End Try
        Else
            Return Nothing
        End If
    End Function
End Class

Module ErrorExtensions
    <Extension()>
    Sub WriteErrorMissing(this As baseCmdlet, itemType As String, itemValue As String, Optional innerEx As Exception = Nothing)
        Dim errorId = $"XDPortal-{itemType}ItemNotFound"
        Dim ex As New XDPItemMissingException(itemType, itemValue, StandardErrors.GetInnermostException(innerEx))
        this.WriteError(ex, errorId, ErrorCategory.ObjectNotFound, itemValue)
    End Sub

    <Extension()>
    Sub WriteErrorNotEmpty(this As baseCmdlet, itemType As String, itemValue As String, itemCount As Long, Optional innerEx As Exception = Nothing)
        Dim errorId = $"XDPortal-{itemType}IsNotEmpty"
        Dim ex As New XDPItemIsNotEmpty(itemType, itemValue, itemCount, StandardErrors.GetInnermostException(innerEx))
        this.WriteError(ex, errorId, ErrorCategory.ResourceExists, itemValue)
    End Sub

    <Extension()>
    Sub WriteErrorBatchFailed(this As baseCmdlet, b As Batch, Optional innerEx As Exception = Nothing)
        Dim errorId = $"XDPortal-BatchError"
        Dim ex As New XDPBatchExecutionError(b.ErrorMessage, b.BatchId, StandardErrors.GetInnermostException(innerEx))
        this.WriteError(ex, errorId, ErrorCategory.InvalidOperation, b)
    End Sub

    <Extension()>
    Public Sub WriteErrorWrapped(this As baseCmdlet, ex As Exception, errId As String, Optional errObject As Object = Nothing)
        Dim wex = WrappedException.GenerateMessageFromInnermost(ex)
        this.WriteError(wex, errId, ErrorCategory.NotSpecified, errObject)
    End Sub

    <Extension()>
    Public Sub WriteErrorWrapped(this As baseCmdlet, ex As Exception, errId As String, errCategory As ErrorCategory, Optional errObject As Object = Nothing)
        Dim wex = WrappedException.GenerateMessageFromInnermost(ex)
        this.WriteError(wex, errId, errCategory, errObject)
    End Sub

    <Extension()>
    Public Sub WriteErrorInvalidOp(this As baseCmdlet, itemType As String, itemValue As String, operation As String, reason As String)
        Dim errorId = $"XDPortal-{itemType}InvalidOperation"
        Dim ioex As New XDPItemInvalidOperation(itemType, itemValue, operation, reason)
        this.WriteError(ioex, errorId, ErrorCategory.InvalidOperation, itemValue)
    End Sub
End Module