Module SimplyXD
    Private _CurrentXDPortal As String

    Sub ConnectXDPortal(uri As String)
        Dim temp = New Uri(uri)
        Dim tempBase = temp.AbsoluteUri.Substring(0, temp.AbsoluteUri.IndexOf(temp.AbsolutePath))
        _CurrentXDPortal = String.Format("{0}/{1}", tempBase, "odata4/v16")
    End Sub

    ReadOnly Property PortalURI As String
        Get
            Return _CurrentXDPortal
        End Get
    End Property

    Function XDPortal() As PortalODataContext
        If _CurrentXDPortal Is Nothing Then
            Throw New InvalidOperationException("No Xpertdoc Portal connected, please use 'Connect-XdPortal'.")
        Else
            Return New PortalODataContext(_CurrentXDPortal) With {.Credentials = Net.CredentialCache.DefaultCredentials, .MergeOption = Microsoft.OData.Client.MergeOption.NoTracking}
        End If
    End Function

End Module

