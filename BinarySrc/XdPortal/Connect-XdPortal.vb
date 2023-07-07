<Cmdlet(VerbsCommunications.Connect, "XdPortal")>
<CmdletBinding()>
Public Class Connect_XdPortal
    Inherits PSCmdlet

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, Position:=0)>
    Public Property PortalUri As String

    <ValidateRange(1, 120)>
    Public Property DefaultTimeout As Integer = 15

    Protected Overrides Sub EndProcessing()
        ConnectXDPortal(PortalUri)
        Common.DefaultTimeout = DefaultTimeout
    End Sub
End Class
