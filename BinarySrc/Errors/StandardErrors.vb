Imports Microsoft.OData.Client
Imports Newtonsoft.Json.Linq
Friend Class StandardErrors
    Shared Function XDPMissing(itemType As String, itemValue As String, Optional innerException As Exception = Nothing) As ErrorRecord
        Dim errorId = String.Format("XDPortal-{0}ItemNotFound", itemType)
        Dim ex As New XDPItemMissingException(itemType, itemValue, GetInnermostException(innerException))
        Return New ErrorRecord(ex, errorId, ErrorCategory.InvalidResult, itemValue)
    End Function

    Shared Function XDPNotEmpty(itemType As String, itemValue As String, itemCount As Long, Optional innerException As Exception = Nothing) As ErrorRecord
        Dim errorId = $"XDPortal-{itemType}IsNotEmpty"
        Dim ex As New XDPItemIsNotEmpty(itemType, itemValue, GetInnermostException(innerException), itemCount)
        Return New ErrorRecord(ex, errorId, ErrorCategory.ResourceExists, itemValue)
    End Function

    Shared Function GenericWrappedError(ex As Exception, errId As String, Optional errObject As Object = Nothing) As ErrorRecord
        Dim nEx = WrappedException.GenerateMessageFromInnermost(ex)
        Return New ErrorRecord(nEx, errId, ErrorCategory.NotSpecified, errObject)
    End Function

    Shared Function TemplateBlobUpdate(ex As Exception, t As Template) As ErrorRecord
        Dim nex = WrappedException.GenerateMessageFromInnermost(ex)
        Return New ErrorRecord(nex, "XDPortal-TemplateContentUpdateFailed", ErrorCategory.InvalidResult, t)
    End Function

    Shared Function NotImplemented(message As String) As ErrorRecord
        Return New ErrorRecord(New NotImplementedException(message), Nothing, ErrorCategory.NotImplemented, Nothing)
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
            Dim err As JObject = JObject.Parse(ex.Message)("error")
            Return $"[{err("code")}] {err("message")}"
        Else
            Return Nothing
        End If
    End Function

End Class