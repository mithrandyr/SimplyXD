Imports System.IO

<Cmdlet(VerbsData.Export, "XdTemplate")>
<CmdletBinding()>
Public Class Export_XdTemplate
    Inherits baseCmdlet

    <Parameter(Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
    Public Property TemplateLibrary As String

    <Parameter(Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
    Public Property TemplateGroup As String

    <[Alias]("TemplateName")>
    <Parameter(Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
    Public Property Name As String

    <Parameter(ValueFromPipelineByPropertyName:=True)>
    Public Property ExportPath As String

    <Parameter(ValueFromPipelineByPropertyName:=True)>
    Public Property Version As String

    Protected Overrides Sub ProcessRecord()
        If String.IsNullOrWhiteSpace(ExportPath) Then
            ExportPath = CurrentProviderLocation("FileSystem").ProviderPath
        Else
            If Path.GetFullPath(ExportPath) <> ExportPath Then
                ExportPath = Path.Combine(CurrentProviderLocation("FileSystem").ProviderPath, ExportPath)
            End If
        End If

        Dim needsCleanup = False
        Try
            'verify that path exists and can be written to
            If Path.HasExtension(ExportPath) AndAlso File.Exists(ExportPath) Then
                WriteError(New ErrorRecord(New Exception("File already exists"), "FileExists", ErrorCategory.ResourceExists, ExportPath))
                Exit Sub
            End If

            'get template
            xdp.MergeOption = Microsoft.OData.Client.MergeOption.OverwriteChanges
            Dim query = xdp.Templates.Expand(Function(x) x.TemplateGroup.TemplateLibrary).AsQueryable

            Dim t As Template

            query = query.Where(Function(x) x.TemplateGroup.TemplateLibrary.Name.ToUpper.Equals(TemplateLibrary.ToUpper))
            query = query.Where(Function(x) x.TemplateGroup.Name.ToUpper.Equals(TemplateGroup.ToUpper))
            WriteVerbose("Getting Template by Name")
            t = ExecuteWithTimeout(query.Where(Function(x) x.Name.ToUpper.Equals(Name.ToUpper)).FirstOrDefaultAsync)
            If t Is Nothing Then
                WriteError(StandardErrors.XDPMissing("Template", String.Format("{0}\{1}\{2}", TemplateLibrary, TemplateGroup, Name)))
                Exit Sub
            End If

            'Create ZipFile
            If Not Path.HasExtension(ExportPath) Then
                ExportPath = Path.Combine(ExportPath, String.Format("{0}.{1}.{2}.zip", t.TemplateGroup.TemplateLibrary.Name, t.TemplateGroup.Name, t.Name))
            End If
            If Not Directory.Exists(Path.GetDirectoryName(ExportPath)) Then
                Directory.CreateDirectory(Path.GetDirectoryName(ExportPath))
            End If

            If File.Exists(ExportPath) Then
                WriteError(New ErrorRecord(New Exception("File already exists"), "FileExists", ErrorCategory.ResourceExists, ExportPath))
                Exit Sub
            End If

            WriteVerbose("Creating ZipFile")
            Using zArchive As Compression.ZipArchive = Compression.ZipFile.Open(ExportPath, Compression.ZipArchiveMode.Create)
                needsCleanup = True
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
                    Dim bytelist = ExecuteWithTimeout(t.GetAssembly.GetValueAsync)
                    zStream.Write(bytelist, 0, bytelist.Count)
                End Using

                'Create DOCX file in zip
                WriteVerbose("Creating DOCX file in zip")
                Using zStream = zArchive.CreateEntry(String.Format("{0}.docx", t.Name)).Open()
                    Dim bytelist = ExecuteWithTimeout(t.GetSource.GetValueAsync)
                    zStream.Write(bytelist, 0, bytelist.Count)
                End Using

                needsCleanup = False
            End Using
            WriteObject(New FileInfo(ExportPath))
        Finally
            xdp.MergeOption = Microsoft.OData.Client.MergeOption.NoTracking
            If needsCleanup Then
                If Path.HasExtension(ExportPath) AndAlso IO.File.Exists(ExportPath) Then
                    File.Delete(ExportPath)
                End If
            End If
        End Try
    End Sub
End Class
