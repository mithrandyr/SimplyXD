<Cmdlet(VerbsDiagnostic.Test, "Xd")>
<CmdletBinding()>
Public Class Invoke_XdTest
    Inherits baseCmdlet

    Protected Overrides Sub EndProcessing()
'this is simply for testing and validating that on .NET 8.0, Xpertdoc.Portal.SdkCore doesn't work
        Dim thePortal = New PortalODataContext(xdp.BaseUri) With {.Credentials = Net.CredentialCache.DefaultCredentials, .MergeOption = Microsoft.OData.Client.MergeOption.NoTracking}

        Dim nTemplateLibrary As New TemplateLibrary
        With nTemplateLibrary
            .Name = "smoketest"
            .ExecutionTimeoutInMillis = 30000
        End With

        thePortal.AddToTemplateLibraries(nTemplateLibrary)
        thePortal.SaveChangesAsync().Wait()

        WriteObject(nTemplateLibrary)
    End Sub


End Class
