
<Cmdlet(VerbsCommon.Remove, "XdTemplateLibrary")>
<CmdletBinding(DefaultParameterSetName:="name")>
Public Class Remove_XdTemplateLibrary
    Inherits baseCmdlet

    <ValidateNotNullOrEmpty>
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
                WriteErrorMissing("TemplateLibrary", Name)
                Exit Sub
            End If
            TemplateLibraryId = result
        End If

        WriteVerbose("Getting count of TemplateGroups in the TemplateLibrary")
        Dim groupCount As Integer = ExecuteWithTimeout(xdp.TemplateGroups.Where(Function(x) x.TemplateLibraryId = TemplateLibraryId).CountAsync)

        If groupCount = 0 OrElse Force.IsPresent Then
            Dim dTemplateLibrary = New TemplateLibrary With {.TemplateLibraryId = TemplateLibraryId}
            xdp.AttachTo("TemplateLibraries", dTemplateLibrary)
            xdp.DeleteObject(dTemplateLibrary)
            WriteVerbose("Deleting the TemplateLibrary")
            SaveChanges()
        Else
            WriteErrorNotEmpty("TemplateLibrary", If(Name, TemplateLibraryId), groupCount)
        End If
    End Sub
End Class
