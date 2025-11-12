Imports System.Reflection
Imports System.Diagnostics
'Using System.Management.Automation;


<Cmdlet(VerbsDiagnostic.Test, "Xd")>
<CmdletBinding()>
Public Class Invoke_XdTest
    'Inherits baseCmdlet
    Inherits PSCmdlet

    Protected Overrides Sub EndProcessing()

        Dim theAssembly = Assembly.GetExecutingAssembly()
        WriteObject($"Assembly Location: {theAssembly.Location}")
        WriteObject($"Is Debug Build: {IsDebugBuild()}")
        WriteObject($"Debugger Attached: {Debugger.IsAttached}")


        Dim dllPath = theAssembly.Location
        Dim pdbPath = System.IO.Path.ChangeExtension(dllPath, ".pdb")
        WriteObject($"PDB Exists: {System.IO.File.Exists(pdbPath)} at {pdbPath}")

        System.Diagnostics.Debugger.Launch()


        ''this Is simply For testing And validating that On .NET 8.0, Xpertdoc.Portal.SdkCore doesn't work
        'Dim thePortal = New PortalODataContext(xdp.BaseUri) With {.Credentials = Net.CredentialCache.DefaultCredentials, .MergeOption = Microsoft.OData.Client.MergeOption.NoTracking}

        'Dim nTemplateLibrary As New TemplateLibrary
        'With nTemplateLibrary
        '    .Name = "smoketest"
        '    .ExecutionTimeoutInMillis = 30000
        'End With

        'thePortal.AddToTemplateLibraries(nTemplateLibrary)
        'thePortal.SaveChangesAsync().Wait()

        'WriteObject(nTemplateLibrary)
    End Sub

    Private Function IsDebugBuild() As Boolean
        Dim theAssemblly = Assembly.GetExecutingAssembly()
        Dim DebuggableAttribute = theAssemblly.GetCustomAttribute(Of DebuggableAttribute)()
        Return DebuggableAttribute?.IsJITOptimizerDisabled = False
    End Function


End Class
