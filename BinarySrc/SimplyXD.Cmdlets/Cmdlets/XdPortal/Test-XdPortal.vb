'Not IN use
'<Cmdlet(VerbsDiagnostic.Test, "XdPortal")>
'<CmdletBinding()>
Public Class Test_XdPortal
    Inherits baseCmdlet

    Protected Overrides Sub EndProcessing()
        'WriteObject(GenerateResults(XDP.BatchOperations.AsQueryable, "DOPAs", 10), True)

        Throw New NotImplementedException("This is not implemented!")
    End Sub
End Class
