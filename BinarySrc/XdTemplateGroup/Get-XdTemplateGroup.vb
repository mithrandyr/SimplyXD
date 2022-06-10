<Cmdlet(VerbsCommon.Get, "XdTemplateGroup")>
<CmdletBinding(DefaultParameterSetName:="search")>
Public Class Get_XdTemplateGroup
    Inherits baseCmdlet

    <Parameter(ParameterSetName:="search", Position:=0)>
    <ValidateNotNull()>
    Public Property Search As String

    <Parameter(ParameterSetName:="search")>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property TemplateLibrary As String

    <Parameter(Mandatory:=True, ParameterSetName:="obj", ValueFromPipeline:=True)>
    Public Property InputObject As TemplateLibrary

    <[Alias]("TemplateGroup")>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property Name As String

    <Parameter(Mandatory:=True, ParameterSetName:="id", ValueFromPipeline:=True)>
    Public Property TemplateGroupId As Guid



    Protected Overrides Sub EndProcessing()
        Dim query = xdp.TemplateGroups.Expand(Function(x) x.TemplateLibrary).AsQueryable

        Select Case ParameterSetName
            Case "search"
                If Not String.IsNullOrWhiteSpace(TemplateLibrary) Then
                    query = query.Where(Function(x) x.TemplateLibrary.Name.ToUpper.Equals(TemplateLibrary.ToUpper))
                End If
                If Not String.IsNullOrWhiteSpace(Search) Then
                    query = query.Where(Function(x) x.Name.Contains(Search))
                End If

            Case "name"
                query = query.Where(Function(x) x.TemplateLibrary.Name.ToUpper.Equals(TemplateLibrary.ToUpper) And x.Name.ToUpper.Equals(Name.ToUpper))

            Case "obj"
                query = query.Where(Function(x) x.TemplateLibraryId = InputObject.TemplateLibraryId)

            Case "id"
                query = query.Where(Function(x) x.TemplateGroupId = TemplateGroupId)
        End Select


        If ParameterSetName = "search" Or ParameterSetName = "obj" Then
            For Each gr In GenerateResults(query, "TemplateGroup")
                WriteObject(gr.AsPSObject)
            Next
        Else
            Dim tg As TemplateGroup
            Try
                tg = ExecuteWithTimeout(query.FirstOrDefaultAsync)
                If tg IsNot Nothing Then WriteObject(tg.AsPSObject)
            Catch ex As Exception
                WriteError(StandardErrors.XDPMissing("TemplateGroup", If(Name, TemplateGroupId), ex))
            End Try

        End If
    End Sub
End Class
