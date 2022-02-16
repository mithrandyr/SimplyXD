<Serializable>
Public Class WrappedException
    Inherits Exception

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(message As String, inner As Exception)
        MyBase.New(message, inner)
    End Sub

    Protected Sub New(info As Runtime.Serialization.SerializationInfo, context As Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub

    Friend Shared Function GenerateMessageFromInnermost(ex As Exception) As WrappedException
        Dim exSearch = ex, exMessage As String = ex.Message, limit As Integer = 10, iteration As Integer = 0
        While exSearch.InnerException IsNot Nothing
            exSearch = exSearch.InnerException
            If Not String.IsNullOrWhiteSpace(exSearch.Message) Then exMessage = exSearch.Message
            iteration += 1
            If iteration > limit Then Exit While
        End While

        Return New WrappedException(exMessage, ex)
    End Function
End Class
