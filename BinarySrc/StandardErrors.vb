Friend Class StandardErrors
    Shared Function XDPMissing(itemType As String, itemValue As String, Optional innerException As Exception = Nothing) As ErrorRecord
        Dim errorId = String.Format("XDPortal-{0}ItemNotFound", itemType)
        Dim ex As New XDPItemMissingException(itemType, itemValue, GetInnermostException(innerException))
        Return New ErrorRecord(ex, errorId, ErrorCategory.InvalidResult, itemValue)
    End Function

    Shared Function GenericWrappedError(ex As Exception, errId As String) As ErrorRecord
        Dim nEx = WrappedException.GenerateMessageFromInnermost(ex)
        Return New ErrorRecord(nEx, errId, ErrorCategory.NotSpecified, Nothing)
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

End Class

<Serializable>
Public Class XDPItemMissingException
    Inherits Exception

    Public ReadOnly Property ItemType As String
    Public ReadOnly Property ItemValue As String

    Public Overrides ReadOnly Property Message As String
        Get
            Return String.Format("Cannot find {0} = '{1}'", ItemType, ItemValue)
        End Get
    End Property

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(itemType As String, itemValue As String)
        Me.ItemType = itemType
        Me.ItemValue = itemValue
    End Sub

    Public Sub New(itemType As String, itemValue As String, inner As Exception)
        MyBase.New(Nothing, inner)
        Me.ItemType = itemType
        Me.ItemValue = itemValue
    End Sub

    Protected Sub New(info As Runtime.Serialization.SerializationInfo, context As Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub

End Class

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
        Dim exMessage As String = StandardErrors.GetInnermostException(ex).Message
        Return New WrappedException(exMessage, ex)
    End Function
End Class
