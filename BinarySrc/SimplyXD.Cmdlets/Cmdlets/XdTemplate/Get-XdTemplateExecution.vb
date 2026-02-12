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

    <Parameter(Mandatory:=True, ParameterSetName:="id", ValueFromPipelineByPropertyName:=True)>
    Public Property TemplateExecutionId As Guid
    Protected Overrides Sub ProcessRecord()
        Dim query = xdp.TemplateExecutions.Expand(Function(x) x.Template.TemplateGroup.TemplateLibrary).Expand(Function(x) x.TemplateExecutionData).AsQueryable

        If ParameterSetName = "obj" Then
            'Return all templates for a given templateGroup
            For Each te In GenerateResults(query.Where(Function(x) x.TemplateId = InputObject), "TemplateExecution")
                Output(te)
            Next
        ElseIf ParameterSetName = "id" Then
            For Each te In ExecuteWithTimeout(query.Where(Function(x) x.TemplateExecutionId = TemplateExecutionId).Take(1).ToListAsync)
                Output(te)
            Next
        Else
            If Not String.IsNullOrWhiteSpace(TemplateGroup) Then
                query = query.Where(Function(x) x.Template.TemplateGroup.Name.ToUpper.Equals(TemplateGroup.ToUpper))
            End If

            If Not String.IsNullOrWhiteSpace(TemplateLibrary) Then
                query = query.Where(Function(x) x.Template.TemplateGroup.TemplateLibrary.Name.ToUpper.Equals(TemplateLibrary.ToUpper))
            End If

            If ParameterSetName = "search" Then
                If Not String.IsNullOrWhiteSpace(Search) Then
                    query = query.Where(Function(x) x.Template.Name.Contains(Search))
                End If
                For Each te In GenerateResults(query, "TemplateExecution")
                    Output(te)
                Next
            Else
                Dim te As TemplateExecution
                te = ExecuteWithTimeout(query.Where(Function(x) x.Template.Name.ToUpper.Equals(Name.ToUpper)).FirstOrDefaultAsync)
                If te IsNot Nothing Then Output(te)
            End If
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
