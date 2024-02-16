<Cmdlet(VerbsCommon.Remove, "XdTemplate")>
<CmdletBinding(DefaultParameterSetName:="id")>
Public Class Remove_XdTemplate
    Inherits baseCmdlet

#Region "PowerShell Properties"
    <[Alias]("TemplateId")>
    <Parameter(Mandatory:=True, ParameterSetName:="id", ValueFromPipelineByPropertyName:=True)>
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
                WriteErrorMissing("Template", Name)
                Exit Sub
            End If
            Id = result
        End If

        Dim dTemplate = New Template With {.TemplateId = Id}
        xdp.AttachTo("Templates", dTemplate)

        xdp.DeleteObject(dTemplate)
        WriteVerbose($"Deleting Template = {Id}")
        SaveChanges()
    End Sub
End Class
