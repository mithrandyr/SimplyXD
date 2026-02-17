Imports System.Management.Automation.Language

<Cmdlet(VerbsCommon.Get, "XdTemplateExecution")>
<CmdletBinding(DefaultParameterSetName:="search")>
Public Class Get_XdTemplateExecution
    Inherits baseCmdlet

    <Parameter(ParameterSetName:="search", Position:=0)>
    Public Property Search As String

    <Parameter(ParameterSetName:="search")>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property TemplateLibrary As String

    <Parameter(ParameterSetName:="search")>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property TemplateGroup As String

    <ValidateNotNullOrEmpty>
    <[Alias]("TemplateName")>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property Name As String

    <[Alias]("TemplateId")>
    <Parameter(Mandatory:=True, ParameterSetName:="obj", ValueFromPipelineByPropertyName:=True)>
    Public Property InputObject As Guid

    <Parameter()>
    Public Property Limit As Integer = 0

    <Parameter()>
    Public Property AsCount As SwitchParameter

    Protected Overrides Sub ProcessRecord()
        Dim query = xdp.TemplateExecutions.Expand(Function(x) x.Template.TemplateGroup.TemplateLibrary).Expand(Function(x) x.TemplateExecutionData).AsQueryable

        If ParameterSetName = "obj" Then
            'Return all templates for a given templateGroup
            query = query.Where(Function(x) x.TemplateId = InputObject)
        Else
            If Not String.IsNullOrWhiteSpace(TemplateGroup) Then
                query = query.Where(Function(x) x.Template.TemplateGroup.Name.ToUpper.Equals(TemplateGroup.ToUpper))
            End If

            If Not String.IsNullOrWhiteSpace(TemplateLibrary) Then
                query = query.Where(Function(x) x.Template.TemplateGroup.TemplateLibrary.Name.ToUpper.Equals(TemplateLibrary.ToUpper))
            End If

            If Not String.IsNullOrWhiteSpace(Search) Then
                query = query.Where(Function(x) x.Template.Name.Contains(Search))
            End If

            If Not String.IsNullOrWhiteSpace(Name) Then
                query = query.Where(Function(x) x.Template.Name.ToUpper.Equals(Name.ToUpper))
            End If
        End If

        If AsCount Then
            WriteObject(ExecuteWithTimeout(query.CountAsync))
        Else
            For Each te In GenerateResults(query, "TemplateExecution", Limit)
                Output(te)
            Next
        End If
    End Sub

    Private Sub Output(te As TemplateExecution)
        If te Is Nothing Then Exit Sub

        Dim obj As PSObject = te.AsPSObject
        obj.AppendProperty("TemplateLibraryName", te.Template.TemplateGroup.TemplateLibrary.Name)
        obj.AppendProperty("TemplateGroupName", te.Template.TemplateGroup.Name)
        obj.AppendProperty("TemplateName", te.Template.Name)

        WriteObject(obj)
    End Sub
End Class
