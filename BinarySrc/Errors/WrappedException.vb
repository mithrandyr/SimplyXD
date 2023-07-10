<Serializable>
Public Class WrappedException
    Inherits Exception

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(message As String, inner As Exception)
        MyBase.New(message, inner)
    End Sub

    Protected Sub New(info As Runtime.Serialization.SerializationInfo, context As Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub

    Friend Shared Function GenerateMessageFromInnermost(ex As Exception) As WrappedException
        Dim innerEx = StandardErrors.GetInnermostException(ex)
        Dim message = StandardErrors.GetProviderMessage(innerEx)
        If message Is Nothing Then message = innerEx.Message
        Return New WrappedException(message, innerEx)
    End Function

    Public Shared Function CreateErrorRecord(ex As Exception, errId As String, Optional errObject As Object = Nothing) As ErrorRecord
        Return CreateErrorRecord(ex, errId, ErrorCategory.NotSpecified, errObject)
    End Function
    Public Shared Function CreateErrorRecord(ex As Exception, errId As String, errCategory As ErrorCategory, Optional errObject As Object = Nothing) As ErrorRecord
        Dim nEx = GenerateMessageFromInnermost(ex)
        Return New ErrorRecord(nEx, errId, errCategory, errObject)
    End Function

End Class