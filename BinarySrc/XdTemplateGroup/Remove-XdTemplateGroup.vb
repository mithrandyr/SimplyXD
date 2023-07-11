
<Cmdlet(VerbsCommon.Remove, "XdTemplateGroup")>
<CmdletBinding(DefaultParameterSetName:="name")>
Public Class Remove_XdTemplateGroup
    Inherits baseCmdlet

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property TemplateLibrary As String

    <ValidateNotNullOrEmpty>
    <[Alias]("TemplateGroup")>
    <Parameter(Mandatory:=True, ParameterSetName:="name", Position:=0)>
    Public Property Name As String

    <Parameter(Mandatory:=True, ParameterSetName:="id")>
    Public Property TemplateGroupId As Guid

    <Parameter(Mandatory:=True, ParameterSetName:="obj")>
    Public Property InputObject As TemplateGroup

    <Parameter()>
    Public Property Force As SwitchParameter

    Protected Overrides Sub ProcessRecord()
        If ParameterSetName = "name" Then
            WriteVerbose("Looking up the TemplateGroupId")
            Dim result = ExecuteWithTimeout(xdp.TemplateGroups.Where(Function(x) x.Name.ToUpper.Equals(Name.ToUpper) And x.TemplateLibrary.Name.ToUpper.Equals(TemplateLibrary.ToUpper)).FirstOrDefaultAsync)?.TemplateGroupId
            If result Is Nothing Then
                WriteErrorMissing("TemplateGroup", $"{TemplateLibrary}\{Name}")
                Exit Sub
            End If
            TemplateGroupId = result
        ElseIf ParameterSetName = "obj" Then
            TemplateGroupId = InputObject.TemplateGroupId
        End If

        WriteVerbose("Getting count of Templates in the TemplateGroup")
        Dim templateCount As Integer = ExecuteWithTimeout(xdp.Templates.Where(Function(x) x.TemplateGroupId = TemplateGroupId).CountAsync)
        If templateCount = 0 OrElse Force.IsPresent Then
            Dim dTemplateGroup = New TemplateGroup With {.TemplateGroupId = TemplateGroupId}
            xdp.AttachTo("TemplateGroups", dTemplateGroup)
            xdp.DeleteObject(dTemplateGroup)
            WriteVerbose("Deleting the TemplateGroup")
            SaveChanges()
        Else
            WriteErrorNotEmpty("TemplateGroup", If(Name, TemplateGroupId.ToString), templateCount)
        End If
    End Sub
End Class
