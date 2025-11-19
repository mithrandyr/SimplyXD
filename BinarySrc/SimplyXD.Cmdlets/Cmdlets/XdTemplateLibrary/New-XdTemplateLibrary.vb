<Cmdlet(VerbsCommon.[New], "XdTemplateLibrary")>
<CmdletBinding()>
Public Class New_XdTemplateLibrary
    Inherits baseCmdlet

    <ValidateNotNullOrEmpty>
    <[Alias]("TemplateLibrary")>
    <Parameter(Mandatory:=True, Position:=0)>
    Public Property Name As String

    <Parameter()>
    Public Property Description As String

    <Parameter()>
    Public Property DisableTemplateVersioning As SwitchParameter

    Protected Overrides Sub EndProcessing()
        Dim nTemplateLibrary As New TemplateLibrary
        With nTemplateLibrary
            .Name = Name
            .Description = Description
            .EnableTemplateVersionHistory = Not DisableTemplateVersioning.IsPresent
            .ExecutionTimeoutInMillis = 30000
        End With

        xdp.AddToTemplateLibraries(nTemplateLibrary)
        If SaveChanges(nTemplateLibrary) Then WriteObject(nTemplateLibrary)
        xdp.Detach(nTemplateLibrary)
    End Sub
End Class
