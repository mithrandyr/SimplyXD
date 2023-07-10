Public Class XDPItemMissingException
    Inherits baseXDPException

    Public ReadOnly Property ItemType As String
    Public ReadOnly Property ItemValue As String

    Public Overrides ReadOnly Property Message As String
        Get
            Return $"Cannot find {ItemType} '{ItemValue}'."
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
End Class