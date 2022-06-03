<Cmdlet(VerbsCommon.Get, "XdTemplateGroup")>
<CmdletBinding(DefaultParameterSetName:="search")>
Public Class Get_XdTemplateGroup
    Inherits baseCmdlet

    <Parameter(ParameterSetName:="search", Position:=0)>
    <ValidateNotNull()>
    Public Property Search As String

    <Parameter(ParameterSetName:="search")>
    <Parameter(Mandatory:=True, ParameterSetName:="name", ValueFromPipelineByPropertyName:=True)>
    Public Property TemplateLibrary As String

    <Parameter(Mandatory:=True, ParameterSetName:="name", ValueFromPipelineByPropertyName:=True)>
    Public Property TemplateGroup As String

    <Parameter(Mandatory:=True, ParameterSetName:="id", ValueFromPipelineByPropertyName:=True)>
    Public Property TemplateGroupId As Guid

    Protected Overrides Sub EndProcessing()
        Dim query = xdp.TemplateGroups.Expand(Function(x) x.TemplateLibrary).AsQueryable

        If Not String.IsNullOrWhiteSpace(TemplateLibrary) Then
            query = query.Where(Function(x) x.TemplateLibrary.Name.ToUpper.Equals(TemplateLibrary.ToUpper))
        End If

        If ParameterSetName = "search" Then
            query = query.Where(Function(x) x.Name.Contains(Search))

            For Each gr In GenerateResults(query, "TemplateGroup")
                WriteObject(gr.AsPSObject)
            Next
        Else
            Dim tg As TemplateGroup
            If ParameterSetName = "name" Then
                tg = ExecuteWithTimeout(query.Where(Function(x) x.Name.ToUpper.Equals(TemplateGroup.ToUpper)).FirstOrDefaultAsync)
            Else
                Try
                    tg = ExecuteWithTimeout(query.Where(Function(x) x.TemplateGroupId = TemplateGroupId).FirstOrDefaultAsync)
                Catch
                    'TODO: add error handling.  When selecting using a GUID, if the guid doesn't exist, a NotFound error gets thrown.  Currently we are swallowing this error and returning nothing.
                End Try
            End If

            If tg IsNot Nothing Then WriteObject(tg.AsPSObject)
        End If
    End Sub
End Class
