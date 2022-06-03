<Cmdlet(VerbsCommon.Get, "XdTemplate")>
<CmdletBinding(DefaultParameterSetName:="search")>
Public Class Get_XdTemplate
    Inherits baseCmdlet

    <Parameter(ParameterSetName:="search")>
    Public Property Search As String

    <Parameter(ParameterSetName:="search")>
    <Parameter(Mandatory:=True, ParameterSetName:="name", ValueFromPipelineByPropertyName:=True)>
    Public Property TemplateLibrary As String

    <Parameter(ParameterSetName:="search")>
    <Parameter(Mandatory:=True, ParameterSetName:="name", ValueFromPipelineByPropertyName:=True)>
    Public Property TemplateGroup As String

    <Parameter(Mandatory:=True, ParameterSetName:="name", ValueFromPipelineByPropertyName:=True)>
    Public Property TemplateName As String

    <Parameter(Mandatory:=True, ParameterSetName:="id", ValueFromPipelineByPropertyName:=True)>
    Public Property TemplateId As Guid

    Protected Overrides Sub EndProcessing()
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
                WriteObject(t.AsPSObject)
            Next
        Else
            Dim t As Template
            If ParameterSetName = "name" Then
                t = ExecuteWithTimeout(query.Where(Function(x) x.Name.ToUpper.Equals(TemplateName.ToUpper)).FirstOrDefaultAsync)
            Else
                Try
                    t = ExecuteWithTimeout(query.Where(Function(x) x.TemplateId = TemplateId).FirstOrDefaultAsync)
                Catch
                    'TODO: add error handling.  When selecting using a GUID, if the guid doesn't exist, a NotFound error gets thrown.  Currently we are swallowing this error and returning nothing.
                End Try
            End If

            If t IsNot Nothing Then WriteObject(t.AsPSObject)
        End If
    End Sub
End Class
