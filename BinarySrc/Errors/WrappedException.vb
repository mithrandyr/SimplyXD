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
End Class