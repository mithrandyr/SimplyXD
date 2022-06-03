<Cmdlet(VerbsCommon.Get, "XdTemplateLibrary")>
<CmdletBinding(DefaultParameterSetName:="search")>
Public Class Get_XdTemplateLibrary
    Inherits baseCmdlet

    <Parameter(ParameterSetName:="search", Position:=0)>
    <ValidateNotNull()>
    Public Property Search As String

    <Parameter(Mandatory:=True, ParameterSetName:="name", ValueFromPipelineByPropertyName:=True)>
    Public Property TemplateLibrary As String

    <Parameter(Mandatory:=True, ParameterSetName:="id", ValueFromPipelineByPropertyName:=True)>
    Public Property TemplateLibraryId As Guid

    Protected Overrides Sub EndProcessing()
        Dim query = xdp.TemplateLibraries.AsQueryable

        If ParameterSetName = "search" Then
            query = query.Where(Function(x) x.Name.Contains(Search))

            For Each tl In GenerateResults(query, "TemplateLibrary")
                WriteObject(tl)
            Next
        Else
            Dim tl As TemplateLibrary
            If ParameterSetName = "name" Then
                tl = ExecuteWithTimeout(query.Where(Function(x) x.Name.ToUpper.Equals(TemplateLibrary.ToUpper)).FirstOrDefaultAsync)
            Else
                Try
                    tl = ExecuteWithTimeout(query.Where(Function(x) x.TemplateLibraryId = TemplateLibraryId).FirstOrDefaultAsync)
                Catch
                    'TODO: add error handling.  When selecting using a GUID, if the guid doesn't exist, a NotFound error gets thrown.  Currently we are swallowing this error and returning nothing.
                End Try
            End If

            If tl IsNot Nothing Then WriteObject(tl)
        End If
    End Sub
End Class
