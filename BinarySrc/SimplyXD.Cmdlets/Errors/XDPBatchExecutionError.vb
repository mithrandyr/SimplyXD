Public Class XDPBatchExecutionError
    Inherits Exception

    Public ReadOnly Property BatchId As Guid
    Public Sub New(batchErrorMessage As String, bId As Guid, Optional ex As Exception = Nothing)
        MyBase.New(batchErrorMessage, ex)
        BatchId = bId
    End Sub

End Class
