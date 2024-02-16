Public Class Engine
    Private Shared _XDP As PortalODataContext
    Private Shared _TimeoutS As Integer
    Private Shared PortalConnected As Boolean = False
    Friend Shared ReadOnly Property TimeoutS As Integer
        Get
            Return _TimeoutS
        End Get
    End Property
    Public Shared ReadOnly Property XDP As PortalODataContext
        Get
            TestPortalConnected()
            Return _XDP
        End Get
    End Property

    Public Shared Sub Connect(PortalUri As String, timeoutSeconds As Integer, asIs As Boolean)
        _TimeoutS = timeoutSeconds
        If Not asIs Then
            Dim temp = New Uri(PortalUri)
            If temp.AbsolutePath.Length > 1 Then
                Dim tempBase = temp.AbsoluteUri.Substring(0, temp.AbsoluteUri.LastIndexOf(temp.AbsolutePath))
                PortalUri = $"{tempBase.TrimEnd("/")}/odata4/v18"
            Else
                PortalUri = $"{temp.AbsoluteUri.TrimEnd("/")}/odata4/v18"
            End If
        End If

        _XDP = New PortalODataContext(PortalUri) With {.Credentials = Net.CredentialCache.DefaultCredentials, .MergeOption = Microsoft.OData.Client.MergeOption.NoTracking}
        PortalConnected = True
    End Sub
    Public Shared ReadOnly Property CurrentPortalURI As String
        Get
            Return XDP.BaseUri.ToString()
        End Get
    End Property

    Public Shared Function NewXDP() As PortalODataContext
        TestPortalConnected()
        Return New PortalODataContext(CurrentPortalURI) With {.Credentials = Net.CredentialCache.DefaultCredentials, .MergeOption = Microsoft.OData.Client.MergeOption.NoTracking}
    End Function
    Friend Shared Sub TestPortalConnected()
        If Not PortalConnected Then Throw New InvalidOperationException("No Xpertdoc Portal connected, please use 'Connect-XdPortal'.")
    End Sub
End Class
