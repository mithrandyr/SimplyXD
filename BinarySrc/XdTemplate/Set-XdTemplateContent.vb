<Cmdlet(VerbsCommon.Set, "XdTemplateContent")>
<CmdletBinding(DefaultParameterSetName:="name")>
Public Class Set_XdTemplateContent
    Inherits baseCmdlet

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property TemplateLibrary As String

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property TemplateGroup As String

    <[Alias]("TemplateName")>
    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property Name As String

    <Parameter(Mandatory:=True, ParameterSetName:="id")>
    Public Property TemplateId As Guid

    <[Alias]("WordDocument")>
    <Parameter(Mandatory:=True)>
    Public Property Source As Byte()

    <[Alias]("Dll")>
    <Parameter(Mandatory:=True)>
    Public Property Assembly As Byte()

    <Parameter()>
    Public Property Comment As String

    Protected Overrides Sub EndProcessing()
        Try
            xdp.MergeOption = Microsoft.OData.Client.MergeOption.OverwriteChanges
            Dim uTemplate As Template
            Dim query = xdp.Templates.Expand(Function(x) x.TemplateGroup.TemplateLibrary).AsQueryable
            Try
                Select Case ParameterSetName
                    Case "name"
                        uTemplate = ExecuteWithTimeout(query.Where(Function(x) x.Name.ToUpper.Equals(Name.ToUpper) And x.TemplateGroup.Name.ToUpper.Equals(TemplateGroup.ToUpper) And x.TemplateGroup.TemplateLibrary.Name.ToUpper.Equals(TemplateLibrary.ToUpper)).FirstOrDefaultAsync)
                        If uTemplate Is Nothing Then
                            WriteError(StandardErrors.XDPMissing("Template", String.Format("{0}\{1}\{2}", TemplateLibrary, TemplateGroup, Name)))
                            Exit Sub
                        End If
                    Case "id"
                        uTemplate = ExecuteWithTimeout(query.Where(Function(x) x.TemplateId = TemplateId).FirstOrDefaultAsync)
                End Select
            Catch ex As Exception
                WriteError(StandardErrors.GenericWrappedError(ex, "TemplateRetrievalFailed"))
                Exit Sub
            End Try

            Try
                WriteVerbose("Updating Template Content")
                ExecuteWithTimeout(uTemplate.UpdateAssemblyAndSource(Assembly, uTemplate.AssemblyBlobFileName, Source, uTemplate.SourceBlobFileName, Comment).ExecuteAsync)
            Catch ex As Exception
                WriteError(StandardErrors.TemplateBlobUpdate(ex, uTemplate))
            End Try

        Finally
            xdp.MergeOption = Microsoft.OData.Client.MergeOption.NoTracking
        End Try

    End Sub

End Class
