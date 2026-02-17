Public Class XDPItemInvalidOperation
    Inherits baseXDPException

    Public ReadOnly Property ItemType As String
    Public ReadOnly Property ItemValue As String
    Public ReadOnly Property Reason As String
    Public ReadOnly Property Operation As String

    Public Overrides ReadOnly Property Message As String
        Get
            Return $"Cannot {Operation} {ItemType} '{ItemValue}' because {Reason}."
        End Get
    End Property

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(itemType As String, itemValue As String, operation As String, reason As String)
        Me.ItemType = itemType
        Me.ItemValue = itemValue
        Me.Operation = operation
        Me.Reason = reason
    End Sub
End Class