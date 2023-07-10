Imports System.IO

<Cmdlet(VerbsData.Export, "XdTemplate")>
<CmdletBinding(DefaultParameterSetName:="name")>
Public Class Export_XdTemplate
    Inherits baseCmdlet

    <[Alias]("TemplateLibraryName")>
    <ValidateNotNullOrEmpty>
    <Parameter(ParameterSetName:="name", Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
    Public Property TemplateLibrary As String

    <[Alias]("TemplateGroupName")>
    <ValidateNotNullOrEmpty>
    <Parameter(ParameterSetName:="name", Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
    Public Property TemplateGroup As String

    <[Alias]("TemplateName")>
    <ValidateNotNullOrEmpty>
    <Parameter(ParameterSetName:="name", Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
    Public Property Name As String

    <[Alias]("TemplateId")>
    <Parameter(ParameterSetName:="id", Mandatory:=True, ValueFromPipeline:=True)>
    Public Property Id As Guid

    <Parameter(Mandatory:=True, ParameterSetName:="obj", ValueFromPipeline:=True)>
    Public Property InputObject As Template

    <Parameter(ValueFromPipelineByPropertyName:=True)>
    Public Property ExportPath As String

    <Parameter(ValueFromPipelineByPropertyName:=True)>
    Public Property Version As String

    <Parameter()>
    Public Property Force As SwitchParameter

    Protected Overrides Sub ProcessRecord()
        Dim needsCleanup = False
        Dim currentFilePath As String = ""
        Try
            'get template
            xdp.MergeOption = Microsoft.OData.Client.MergeOption.OverwriteChanges
            Dim t As Template = GetTemplate() 'if no template, error already written in GetTemplate
            If t Is Nothing Then Exit Sub

            currentFilePath = CreateFilePath()
            If Not IsValid(currentFilePath) Then Exit Sub 'errors already written

            'Create ZipFile
            needsCleanup = True
            CreateZip(t, currentFilePath)
            needsCleanup = False

            'output the generated zip
            WriteObject(New FileInfo(currentFilePath))
        Finally
            xdp.MergeOption = Microsoft.OData.Client.MergeOption.NoTracking
            If needsCleanup AndAlso Path.HasExtension(currentFilePath) AndAlso File.Exists(currentFilePath) Then
                File.Delete(currentFilePath)
            End If
        End Try
    End Sub

    Private Function CreateFilePath() As String
        Dim filePath As String
        If String.IsNullOrWhiteSpace(ExportPath) Then
            filePath = CurrentProviderLocation("FileSystem").ProviderPath
        Else
            If Path.GetFullPath(ExportPath) <> ExportPath Then
                filePath = Path.Combine(CurrentProviderLocation("FileSystem").ProviderPath, ExportPath)
            Else
                filePath = ExportPath
            End If
        End If

        If Not Path.HasExtension(filePath) Then
            filePath = Path.Combine(filePath, String.Format("{0}.{1}.{2}.zip", TemplateLibrary, TemplateGroup, Name))
        End If
        Return filePath
    End Function

    Private Function IsValid(filePath) As Boolean
        WriteVerbose("Validating ExportPath")
        If File.Exists(filePath) Then
            If Force.IsPresent Then
                File.Delete(filePath)
            Else
                WriteError(New ErrorRecord(New Exception("File already exists"), "FileExists", ErrorCategory.ResourceExists, filePath))
                Return False
            End If
        End If
        If Not Directory.Exists(Path.GetDirectoryName(filePath)) Then
            WriteVerbose("Creating Directory for ExportPath")
            Directory.CreateDirectory(Path.GetDirectoryName(filePath))
        End If
        Return True
    End Function

    Private Function GetTemplate() As Template
        Dim t As Template
        Dim query = xdp.Templates.Expand(Function(x) x.TemplateGroup.TemplateLibrary).AsQueryable

        If ParameterSetName = "name" Then
            WriteVerbose("Getting Template by Name")
            query = query.Where(Function(x) x.TemplateGroup.TemplateLibrary.Name.ToUpper.Equals(TemplateLibrary.ToUpper))
            query = query.Where(Function(x) x.TemplateGroup.Name.ToUpper.Equals(TemplateGroup.ToUpper))
            t = ExecuteWithTimeout(query.Where(Function(x) x.Name.ToUpper.Equals(Name.ToUpper)).FirstOrDefaultAsync)

            If t Is Nothing Then
                WriteErrorMissing("Template", String.Format("{0}\{1}\{2}", TemplateLibrary, TemplateGroup, Name))
            Else
                Return t
            End If
        Else
            WriteVerbose("Getting Template by Id")
            If InputObject IsNot Nothing Then Id = InputObject.TemplateId
            Try
                t = ExecuteWithTimeout(query.Where(Function(x) x.TemplateId = Id).FirstOrDefaultAsync)
                TemplateLibrary = t.TemplateGroup.TemplateLibrary.Name
                TemplateGroup = t.TemplateGroup.Name
                Name = t.Name
                Return t
            Catch ex As Exception
                WriteErrorMissing("Template", Id.ToString, ex)
            End Try

        End If
    End Function

    Private Sub CreateZip(t As Template, filePath As String)
        WriteVerbose("Creating ZipFile")
        Using zArchive As Compression.ZipArchive = Compression.ZipFile.Open(filePath, Compression.ZipArchiveMode.Create)
            'Create Information file in zip
            WriteVerbose("Creating information file in zip")
            Using zStream = zArchive.CreateEntry("details.txt").Open()
                Using sw As New StreamWriter(zStream)
                    sw.WriteLine(String.Format("Exported: {0}", DateTimeOffset.Now.ToString))
                    sw.WriteLine(String.Format("Version: {0}", Version))
                    sw.WriteLine(String.Format("Portal: {0}", PortalURI))
                    sw.WriteLine(String.Format("TemplateLibrary: {0}", t.TemplateGroup.TemplateLibrary.Name))
                    sw.WriteLine(String.Format("TemplateGroup: {0}", t.TemplateGroup.Name))
                    sw.WriteLine(String.Format("TemplateName: {0}", t.Name))
                End Using
            End Using

            'Create DLL file in zip
            WriteVerbose("Creating DLL file in zip")
            Using zStream = zArchive.CreateEntry(String.Format("{0}.dll", t.Name)).Open()
                WriteVerbose("Querying for DLL Content")
                Dim bytelist = ExecuteWithTimeout(t.GetAssembly.GetValueAsync)
                zStream.Write(bytelist, 0, bytelist.Count)
            End Using

            'Create DOCX file in zip
            WriteVerbose("Creating DOCX file in zip")
            Using zStream = zArchive.CreateEntry(String.Format("{0}.docx", t.Name)).Open()
                WriteVerbose("Querying for DOCX Content")
                Dim bytelist = ExecuteWithTimeout(t.GetSource.GetValueAsync)
                zStream.Write(bytelist, 0, bytelist.Count)
            End Using
        End Using
    End Sub
End Class
