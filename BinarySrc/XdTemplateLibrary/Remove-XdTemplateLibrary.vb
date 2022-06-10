
<Cmdlet(VerbsCommon.Remove, "XdTemplateLibrary")>
<CmdletBinding(DefaultParameterSetName:="name", SupportsShouldProcess:=True)>
Public Class Remove_XdTemplateLibrary
    Inherits baseCmdlet

    <[Alias]("TemplateLibrary")>
    <Parameter(Mandatory:=True, ParameterSetName:="name", Position:=0)>
    Public Property Name As String

    <Parameter(Mandatory:=True, ParameterSetName:="id")>
    Public Property TemplateLibraryId As Guid

    <Parameter()>
    Public Property Force As SwitchParameter

    Protected Overrides Sub EndProcessing()
        If ParameterSetName = "name" Then
            WriteVerbose("Looking up the TemplateLibraryId")
            Dim result = ExecuteWithTimeout(xdp.TemplateLibraries.Where(Function(x) x.Name.ToUpper.Equals(Name.ToUpper)).FirstOrDefaultAsync)?.TemplateLibraryId
            If result Is Nothing Then
                WriteError(StandardErrors.XDPMissing("TemplateLibrary", Name))
                Exit Sub
            End If
            TemplateLibraryId = result
        End If

        WriteVerbose("Getting count of TemplateGroups in the TemplateLibrary")
        Dim groupCount As Integer = ExecuteWithTimeout(xdp.TemplateGroups.Where(Function(x) x.TemplateLibraryId = TemplateLibraryId).CountAsync)
        Dim templateCount As Integer = ExecuteWithTimeout(xdp.Templates.Where(Function(x) x.TemplateGroup.TemplateLibraryId = TemplateLibraryId).CountAsync)
        Dim shouldQuery = String.Format("TemplateLibrary '{0}' has {1} TemplateGroups and {2} Templates", If(Name, TemplateLibraryId), groupCount, templateCount)

        If groupCount = 0 OrElse Force.IsPresent OrElse ShouldContinue(shouldQuery, "Remove TemplateLibrary?") Then
            Dim dTemplateLibrary = New TemplateLibrary With {.TemplateLibraryId = TemplateLibraryId}
            xdp.AttachTo("TemplateLibraries", dTemplateLibrary)
            xdp.DeleteObject(dTemplateLibrary)
            WriteVerbose("Deleting the TemplateLibrary")
            SaveChanges()
        End If
    End Sub
End Class
