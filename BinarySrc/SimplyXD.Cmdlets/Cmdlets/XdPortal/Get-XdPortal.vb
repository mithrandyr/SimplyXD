<Cmdlet(VerbsCommon.Get, "XdPortal")>
<CmdletBinding()>
Public Class Get_XdPortal
    Inherits PSCmdlet

    Protected Overrides Sub EndProcessing()
        If Engine.CurrentPortalURI Is Nothing Then
            WriteWarning("No Xpertdoc Portal connection, use 'Connect-XdPortal'.")
        Else
            WriteObject(Engine.CurrentPortalURI)
        End If
    End Sub
End Class
