<Cmdlet(VerbsCommunications.Connect, "XdPortal")>
<CmdletBinding()>
Public Class Connect_XdPortal
    Inherits PSCmdlet

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, Position:=0)>
    Public Property PortalUri As String

    Protected Overrides Sub EndProcessing()
        ConnectXDPortal(PortalUri)
    End Sub
End Class
