Imports System.IO

<Cmdlet(VerbsData.Import, "XdTemplate")>
<CmdletBinding()>
Public Class Import_XdTemplate
    Inherits baseCmdlet

    <[Alias]("TemplateLibraryName")>
    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
    Public Property TemplateLibrary As String

    <[Alias]("TemplateGroupName")>
    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
    Public Property TemplateGroup As String

    <[Alias]("TemplateName")>
    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
    Public Property Name As String

    <Parameter(ValueFromPipelineByPropertyName:=True)>
    Public Property Comment As String

    <Parameter(ValueFromPipelineByPropertyName:=True)>
    Public Property ImportPath As String

    <Parameter(ValueFromPipelineByPropertyName:=True)>
    Public Property PassThru As SwitchParameter
    Protected Overrides Sub ProcessRecord()
        If Not IsValid() Then Exit Sub
        Try
            xdp.MergeOption = Microsoft.OData.Client.MergeOption.OverwriteChanges
            Dim t As Template = GetTemplate()
            If t Is Nothing Then
                Dim tgId = GetTemplateGroupId()
                If tgId Is Nothing Then
                    WriteErrorMissing("TemplateGroup", String.Format("{0}\{1}", TemplateLibrary, TemplateGroup))
                    Exit Sub
                Else
                    t = CreateTemplate(tgId)
                    If Not SaveChanges(t) Then Exit Sub
                End If
            End If

            If UploadContent(t) AndAlso PassThru.IsPresent Then WriteObject(t.AsPSObject)
        Finally
            xdp.MergeOption = Microsoft.OData.Client.MergeOption.NoTracking
        End Try
    End Sub

    Private Function IsValid() As Boolean
        If Path.GetFullPath(ImportPath) <> ImportPath Then
            ImportPath = Path.Combine(CurrentProviderLocation("FileSystem").ProviderPath, ImportPath)
        End If
        If Path.GetExtension(ImportPath) <> ".zip" Then
            WriteError(New ErrorRecord(New ArgumentException("File is not a zip archive", "ImportPath"), "FileNotZipArchive", ErrorCategory.InvalidArgument, ImportPath))
            Return False
        End If
        If Not File.Exists(ImportPath) Then
            WriteError(New ErrorRecord(New FileNotFoundException("ZipArchive not found", ImportPath), "FileNotFound", ErrorCategory.ObjectNotFound, ImportPath))
            Return False
        End If

        Using zArchive As Compression.ZipArchive = Compression.ZipFile.Open(ImportPath, Compression.ZipArchiveMode.Read)
            Dim DllCount = zArchive.Entries.Where(Function(x) Path.GetExtension(x.Name) = ".dll").Count
            If DllCount = 0 Then
                WriteError(New ErrorRecord(New FileNotFoundException("Template DLL missing from the archive"), "FileNotFound", ErrorCategory.ObjectNotFound, Nothing))
                Return False
            ElseIf DllCount > 1 Then
                WriteError(New ErrorRecord(New ArgumentOutOfRangeException("There should only be a single Template DLL in the archive"), "TooManyDlls", ErrorCategory.InvalidData, Nothing))
                Return False
            End If

            Dim DocxCount = zArchive.Entries.Where(Function(x) Path.GetExtension(x.Name) = ".docx").Count
            If DocxCount = 0 Then
                WriteError(New ErrorRecord(New FileNotFoundException("Template DOCX missing from the archive"), "FileNotFound", ErrorCategory.ObjectNotFound, Nothing))
                Return False
            ElseIf DocxCount > 1 Then
                WriteError(New ErrorRecord(New ArgumentOutOfRangeException("There should only be a single Template DOCX in the archive"), "TooManyDlls", ErrorCategory.InvalidData, Nothing))
                Return False
            End If
        End Using
        Return True
    End Function

    Private Function GetTemplate() As Template
        WriteVerbose("Getting Template")
        Dim query = xdp.Templates.Expand(Function(x) x.TemplateGroup.TemplateLibrary).AsQueryable
        query = query.Where(Function(x) x.TemplateGroup.TemplateLibrary.Name.ToUpper.Equals(TemplateLibrary.ToUpper))
        query = query.Where(Function(x) x.TemplateGroup.Name.ToUpper.Equals(TemplateGroup.ToUpper))
        Return ExecuteWithTimeout(query.Where(Function(x) x.Name.ToUpper.Equals(Name.ToUpper)).FirstOrDefaultAsync)
    End Function

    Private Function GetTemplateGroupId() As Nullable(Of Guid)
        WriteVerbose("Getting TemplateGroupId")
        Dim query = xdp.TemplateGroups.Expand(Function(x) x.TemplateLibrary)
        query = query.Where(Function(x) x.TemplateLibrary.Name.ToUpper.Equals(TemplateLibrary.ToUpper))
        Return ExecuteWithTimeout(query.Where(Function(x) x.Name.ToUpper.Equals(TemplateGroup.ToUpper)).FirstOrDefaultAsync)?.TemplateGroupId
    End Function

    Private Function CreateTemplate(tgId As Guid) As Template
        WriteVerbose("Creating Template")
        Dim nTemplate As New Template
        With nTemplate
            .Name = Name
            .TemplateGroupId = tgId
            .UseParentDocumentOutputPostActions = True
        End With

        xdp.AddToTemplates(nTemplate)
        Return nTemplate
    End Function

    Private Function UploadContent(t As Template) As Boolean
        WriteVerbose("Uploading Template Content")

        Dim SourceContent As Byte()
        Dim AssemblyContent As Byte()

        Using zArchive As Compression.ZipArchive = Compression.ZipFile.Open(ImportPath, Compression.ZipArchiveMode.Read)
            Using zEntry = zArchive.Entries.First(Function(x) Path.GetExtension(x.Name) = ".dll").Open
                Using memStream As New MemoryStream()
                    zEntry.CopyTo(memStream)
                    AssemblyContent = memStream.GetBuffer
                End Using

            End Using

            Using zEntry = zArchive.Entries.First(Function(x) Path.GetExtension(x.Name) = ".docx").Open
                Using memStream As New MemoryStream()
                    zEntry.CopyTo(memStream)
                    SourceContent = memStream.GetBuffer
                End Using
            End Using
        End Using

        Try
            ExecuteWithTimeout(t.UpdateAssemblyAndSource(AssemblyContent, String.Format("{0}.dll", t.Name), SourceContent, String.Format("{0}.docx", t.Name), Comment).ExecuteAsync)
            Return True
        Catch ex As Exception
            WriteError(StandardErrors.TemplateBlobUpdate(ex, t))
            Return False
        End Try
    End Function

End Class
