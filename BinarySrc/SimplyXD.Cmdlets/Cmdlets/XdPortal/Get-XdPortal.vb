<Cmdlet(VerbsCommon.Get, "XdPortal")>
<CmdletBinding()>
Public Class Get_XdPortal
    Inherits PSCmdlet

    Protected Overrides Sub EndProcessing()
        'If PortalURI Is Nothing Then
        '    WriteWarning("No Xpertdoc Portal connection, use 'Connect-XdPortal'.")
        'Else
        '    WriteObject(PortalURI)
        'End If
        WriteObject(Engine.CurrentPortalURI)
    End Sub
End Class
