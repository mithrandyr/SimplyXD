Public Class XDPItemIsNotEmpty
    Inherits baseXDPException

    Public ReadOnly Property ItemType As String
    Public ReadOnly Property ItemValue As String
    Public ReadOnly Property ItemCount As Long

    Public Overrides ReadOnly Property Message As String
        Get
            If ItemCount > 0 Then
                Return $"The {ItemType} '{ItemValue}' is not empty, has {ItemCount} items."
            Else
                Return $"The {ItemType} '{ItemValue}' is not empty."
            End If

        End Get
    End Property

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(itemType As String, itemValue As String, Optional iCount As Long = 0)
        Me.ItemType = itemType
        Me.ItemValue = itemValue
        Me.ItemCount = iCount
    End Sub

    Public Sub New(itemType As String, itemValue As String, Optional iCount As Long = 0, Optional inner As Exception = Nothing)
        MyBase.New(Nothing, inner)
        Me.ItemType = itemType
        Me.ItemValue = itemValue
        Me.ItemCount = iCount
    End Sub
End Class