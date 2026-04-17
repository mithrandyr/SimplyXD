Public Class baseXDPException
    Inherits Exception
    Public ReadOnly Property ProviderMessage As String

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(message As String, inner As Exception)
        MyBase.New(message, inner)
        ProviderMessage = inner.ExtractXdException?.ExtractXDErrorMessage
    End Sub
End Class
