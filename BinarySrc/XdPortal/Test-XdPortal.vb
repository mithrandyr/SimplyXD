<Cmdlet(VerbsDiagnostic.Test, "XdPortal")>
<CmdletBinding()>
Public Class Test_XdPortal
    Inherits baseCmdlet

    Protected Overrides Sub EndProcessing()
        For Each gr In GenerateResults(xdp.DocumentOutputPostActions, Nothing)
            WriteObject(gr)
        Next
    End Sub
End Class
