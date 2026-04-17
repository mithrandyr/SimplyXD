Public Class Engine
    Private Shared _XDP As PortalODataContext
    Private Shared _TimeoutS As Integer
    Private Shared _portalConnected As Boolean = False
    Public Shared Property PortalConnected As Boolean
        Get
            Return _portalConnected
        End Get
        Private Set(value As Boolean)
            _portalConnected = value
        End Set
    End Property

    Friend Shared ReadOnly Property TimeoutS As Integer
        Get
            Return _TimeoutS
        End Get
    End Property
    Public Shared ReadOnly Property XDP As PortalODataContext
        Get
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
            If PortalConnected Then
                Return XDP.BaseUri.ToString()
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public Shared Function NewXDP() As PortalODataContext
        If PortalConnected Then
            Return New PortalODataContext(CurrentPortalURI) With {.Credentials = Net.CredentialCache.DefaultCredentials, .MergeOption = Microsoft.OData.Client.MergeOption.NoTracking}
        Else
            Throw New InvalidOperationException("No Xpertdoc Portal connected, please use 'Connect-XdPortal'.")
        End If
    End Function
End Class
