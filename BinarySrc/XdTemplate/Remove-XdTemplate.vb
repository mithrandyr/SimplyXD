<Cmdlet(VerbsCommon.Remove, "XdTemplate")>
<CmdletBinding(DefaultParameterSetName:="id")>
Public Class Remove_XdTemplate
    Inherits baseCmdlet

#Region "PowerShell Properties"
    <Parameter(Mandatory:=True, ParameterSetName:="obj", ValueFromPipeline:=True)>
    Property InputObject As Template

    <[Alias]("TemplateId")>
    <Parameter(Mandatory:=True, ParameterSetName:="id")>
    Property Id As Guid

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property TemplateLibrary As String

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property TemplateGroup As String

    <ValidateNotNullOrEmpty>
    <[Alias]("TemplateName")>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Property Name As String
#End Region

    Protected Overrides Sub ProcessRecord()
        If ParameterSetName = "name" Then
            WriteVerbose("Looking up TemplateName")
            Dim result = ExecuteWithTimeout(xdp.Templates.Where(Function(x) x.Name.ToUpper.Equals(Name.ToUpper)).FirstOrDefaultAsync)?.TemplateId
            If result Is Nothing Then
                WriteError(StandardErrors.XDPMissing("Template", Name))
                Exit Sub
            End If
            Id = result
        ElseIf ParameterSetName = "obj" Then
            Id = InputObject.TemplateId
        End If

        Dim dTemplate = New Template With {.TemplateId = Id}
        xdp.AttachTo("Templates", dTemplate)

        xdp.DeleteObject(dTemplate)
        WriteVerbose(String.Format("Deleting Template = {0}", Id.ToString))
        SaveChanges()
    End Sub
End Class
