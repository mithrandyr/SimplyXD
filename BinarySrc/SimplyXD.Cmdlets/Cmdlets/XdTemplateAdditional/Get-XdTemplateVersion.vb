Imports System.Management.Automation.Language

<Cmdlet(VerbsCommon.Get, "XdTemplateVersion")>
<CmdletBinding(DefaultParameterSetName:="address")>
Public Class Get_XdTemplateVersion
    Inherits baseCmdlet

    <Parameter(Mandatory:=True, ParameterSetName:="search", Position:=0)>
    Public Property Search As String

    <Parameter(Mandatory:=True, ParameterSetName:="obj", ValueFromPipelineByPropertyName:=True)>
    Public Property TemplateId As Guid

    <Parameter(ParameterSetName:="address")>
    Public Property TemplateLibrary As String
    <Parameter(ParameterSetName:="address")>
    Public Property TemplateGroup As String
    <Parameter(ParameterSetName:="address")>
    Public Property TemplateName As String

    <Parameter()>
    Public Property Limit As Integer = 0

    <Parameter()>
    Public Property AsCount As SwitchParameter

    Protected Overrides Sub ProcessRecord()
        Dim query = xdp.TemplateVersionsHistory.Expand(Function(x) x.Template.TemplateGroup.TemplateLibrary)

        Select Case ParameterSetName
            Case "search"
                query = query.Where(Function(x) x.Template.Name.Contains(Search))

            Case "obj"
                query = query.Where(Function(x) x.TemplateId = TemplateId)

            Case "address"
                If Not String.IsNullOrWhiteSpace(TemplateLibrary) Then
                    query = query.Where(Function(x) x.Template.TemplateGroup.TemplateLibrary.Name.ToUpper.Equals(TemplateLibrary.ToUpper))
                End If

                If Not String.IsNullOrWhiteSpace(TemplateGroup) Then
                    query = query.Where(Function(x) x.Template.TemplateGroup.Name.ToUpper.Equals(TemplateGroup.ToUpper))
                End If

                If Not String.IsNullOrWhiteSpace(TemplateName) Then
                    query = query.Where(Function(x) x.Template.Name.ToUpper.Equals(TemplateName.ToUpper))
                End If
        End Select

        If AsCount Then
            WriteObject(ExecuteWithTimeout(query.CountAsync))
        Else
            For Each tv In GenerateResults(query, "TemplateVersion", Limit)
                Output(tv)
            Next
        End If
    End Sub

    Private Sub Output(tv As TemplateVersionHistory)
        WriteObject(tv.AsPSObject.
            AppendProperty("TemplateLibraryName", tv.Template.TemplateGroup.TemplateLibrary.Name).
            AppendProperty("TemplateGroupName", tv.Template.TemplateGroup.Name).
            AppendProperty("TemplateName", tv.Template.Name)
        )
    End Sub
End Class
