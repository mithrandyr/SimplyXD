
<Cmdlet(VerbsCommon.Remove, "XdTemplateGroup")>
<CmdletBinding(DefaultParameterSetName:="name")>
Public Class Remove_XdTemplateGroup
    Inherits baseCmdlet

    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property TemplateLibrary As String

    <[Alias]("TemplateGroup")>
    <Parameter(Mandatory:=True, ParameterSetName:="name", Position:=0)>
    Public Property Name As String

    <Parameter(Mandatory:=True, ParameterSetName:="id")>
    Public Property TemplateGroupId As Guid

    <Parameter()>
    Public Property Force As SwitchParameter

    Protected Overrides Sub EndProcessing()
        If ParameterSetName = "name" Then
            WriteVerbose("Looking up the TemplateGroupId")
            Dim result = ExecuteWithTimeout(xdp.TemplateGroups.Where(Function(x) x.Name.ToUpper.Equals(Name.ToUpper) And x.TemplateLibrary.Name.ToUpper.Equals(TemplateLibrary.ToUpper)).FirstOrDefaultAsync)?.TemplateGroupId
            If result Is Nothing Then
                WriteError(StandardErrors.XDPMissing("TemplateGroup", String.Format("{0}\{1}", TemplateLibrary, Name)))
                Exit Sub
            End If
            TemplateGroupId = result
        End If

        WriteVerbose("Getting count of Templates in the TemplateGroup")
        Dim templateCount As Integer = ExecuteWithTimeout(xdp.Templates.Where(Function(x) x.TemplateGroupId = TemplateGroupId).CountAsync)
        Dim errMessage = String.Format("TemplateGroup '{0}' has {1} templates", If(Name, TemplateGroupId), templateCount)

        If templateCount = 0 OrElse Force.IsPresent Then
            Dim dTemplateGroup = New TemplateGroup With {.TemplateGroupId = TemplateGroupId}
            xdp.AttachTo("TemplateGroups", dTemplateGroup)
            xdp.DeleteObject(dTemplateGroup)
            WriteVerbose("Deleting the TemplateGroup")
            SaveChanges()
        Else
            WriteError(New ErrorRecord(New InvalidOperationException(errMessage), "TemplateGroupIsNotEmpty", ErrorCategory.ResourceExists, TemplateGroupId))
        End If
    End Sub
End Class
