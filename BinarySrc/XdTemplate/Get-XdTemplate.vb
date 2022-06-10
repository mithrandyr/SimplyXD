<Cmdlet(VerbsCommon.Get, "XdTemplate")>
<CmdletBinding(DefaultParameterSetName:="search")>
Public Class Get_XdTemplate
    Inherits baseCmdlet

    <Parameter(ParameterSetName:="search")>
    Public Property Search As String

    <Parameter(ParameterSetName:="search")>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property TemplateLibrary As String

    <Parameter(ParameterSetName:="search")>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property TemplateGroup As String

    <[Alias]("TemplateName")>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property Name As String

    <Parameter(Mandatory:=True, ParameterSetName:="id", ValueFromPipelineByPropertyName:=True)>
    Public Property TemplateId As Guid

    <Parameter()>
    Public Property IncludeContent As SwitchParameter

    Private idList As New List(Of Guid)
    Protected Overrides Sub ProcessRecord()
        If ParameterSetName = "id" Then idList.Add(TemplateId)
    End Sub

    Protected Overrides Sub EndProcessing()
        'MergeOption must be changed if the Source/Assemble (DOCX/DLL) will also be returned
        If IncludeContent Then xdp.MergeOption = Microsoft.OData.Client.MergeOption.OverwriteChanges
        Try
            Dim query = xdp.Templates.Expand(Function(x) x.TemplateGroup.TemplateLibrary).AsQueryable

            If Not String.IsNullOrWhiteSpace(TemplateGroup) Then
                query = query.Where(Function(x) x.TemplateGroup.Name.ToUpper.Equals(TemplateGroup.ToUpper))
            End If

            If Not String.IsNullOrWhiteSpace(TemplateLibrary) Then
                query = query.Where(Function(x) x.TemplateGroup.TemplateLibrary.Name.ToUpper.Equals(TemplateLibrary.ToUpper))
            End If

            If ParameterSetName = "search" Then
                If Not String.IsNullOrWhiteSpace(Search) Then
                    query = query.Where(Function(x) x.Name.Contains(Search))
                End If
                For Each t In GenerateResults(query, "Template")
                    Output(t)
                Next
            Else
                Dim t As Template
                If ParameterSetName = "name" Then
                    t = ExecuteWithTimeout(query.Where(Function(x) x.Name.ToUpper.Equals(Name.ToUpper)).FirstOrDefaultAsync)
                Else
                    For Each id In idList
                        Try
                            t = ExecuteWithTimeout(query.Where(Function(x) x.TemplateId = id).FirstOrDefaultAsync)
                        Catch ex As Exception
                            WriteError(StandardErrors.XDPMissing("Template", id.ToString, ex))
                        End Try
                    Next
                End If

                Output(t)
            End If
        Finally
            'Revert the MergeOption if it was changed
            If IncludeContent Then xdp.MergeOption = Microsoft.OData.Client.MergeOption.NoTracking
        End Try
    End Sub

    Private Sub Output(t As Template)
        If t Is Nothing Then Exit Sub

        Dim obj As PSObject = t.AsPSObject
        If IncludeContent.IsPresent Then
            obj.Properties.Add(New PSNoteProperty("ContentAssembly", ExecuteWithTimeout(t.GetAssembly.GetValueAsync)))
            obj.Properties.Add(New PSNoteProperty("ContentSource", ExecuteWithTimeout(t.GetSource.GetValueAsync)))
        End If

        WriteObject(obj)
    End Sub
End Class
