<Cmdlet(VerbsCommon.[New], "XdTemplate")>
<CmdletBinding(DefaultParameterSetName:="name")>
Public Class New_XdTemplate
    Inherits baseCmdlet

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property TemplateLibrary As String

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property TemplateGroup As String

    <Parameter(Mandatory:=True, ParameterSetName:="id")>
    Public Property TemplateGroupId As Guid

    <[Alias]("TemplateName")>
    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True)>
    Public Property Name As String

    <Parameter()>
    Public Property Description As String

    <[Alias]("WordDocument")>
    <Parameter(Mandatory:=True)>
    Public Property Source As Byte()

    <[Alias]("Dll")>
    <Parameter(Mandatory:=True)>
    Public Property Assembly As Byte()

    <Parameter()>
    Public Property Comment As String

    Protected Overrides Sub EndProcessing()
        If ParameterSetName = "name" Then
            Dim tg = ExecuteWithTimeout(xdp.TemplateGroups.Expand(Function(x) x.TemplateLibrary).Where(Function(x) x.TemplateLibrary.Name.ToUpper.Equals(TemplateLibrary.ToUpper) And x.Name.ToUpper.Equals(TemplateGroup.ToUpper)).FirstOrDefaultAsync)
            If tg Is Nothing Then
                Dim errMessage = String.Format("Cannot find the TemplateGroup '{0}' in the TemplateLibrary '{1}'.", TemplateGroup, TemplateLibrary)
                WriteError(StandardErrors.XDPMissing("TemplateGroup", String.Format("{0}\{1}", TemplateLibrary, TemplateGroup)))
                Exit Sub
            End If

            TemplateGroupId = tg.TemplateGroupId
        End If

        Dim nTemplate As New Template
        With nTemplate
            .Name = Name
            .TemplateGroupId = TemplateGroupId
            .Description = Description
            .UseParentDocumentOutputPostActions = True
        End With

        xdp.AddToTemplates(nTemplate)
        If SaveChanges(nTemplate) Then
            Try
                ExecuteWithTimeout(nTemplate.UpdateAssemblyAndSource(Assembly, String.Format("{0}.dll", Name), Source, String.Format("{0}.docx", Name), Comment).ExecuteAsync)
            Catch ex As Exception
                WriteError(StandardErrors.TemplateBlobUpdate(ex, nTemplate))
            End Try
            WriteObject(nTemplate)
        End If
    End Sub
End Class
