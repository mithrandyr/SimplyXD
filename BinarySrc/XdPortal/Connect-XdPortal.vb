﻿<Cmdlet(VerbsCommunications.Connect, "XdPortal")>
<CmdletBinding()>
Public Class Connect_XdPortal
    Inherits PSCmdlet

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, Position:=0)>
    Public Property PortalUri As String

    <Parameter()>
    <ValidateRange(1, 120)>
    Public Property DefaultTimeout As Integer = 15

    <Parameter()>
    Public Property PortalUriAsIs As SwitchParameter
    Protected Overrides Sub EndProcessing()
        ConnectXDPortal(PortalUri, PortalUriAsIs.IsPresent)
        Common.DefaultTimeout = DefaultTimeout
    End Sub
End Class
