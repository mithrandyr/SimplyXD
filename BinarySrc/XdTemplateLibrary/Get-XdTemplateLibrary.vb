<Cmdlet(VerbsCommon.Get, "XdTemplateLibrary")>
<CmdletBinding(DefaultParameterSetName:="search")>
Public Class Get_XdTemplateLibrary
    Inherits baseCmdlet

    <Parameter(ParameterSetName:="search", Position:=0)>
    Public Property Search As String

    <[Alias]("TemplateLibrary")>
    <Parameter(Mandatory:=True, ParameterSetName:="name", ValueFromPipelineByPropertyName:=True)>
    Public Property Name As String

    <Parameter(Mandatory:=True, ParameterSetName:="id", ValueFromPipeline:=True)>
    Public Property TemplateLibraryId As Guid

    Protected Overrides Sub EndProcessing()
        Dim query = xdp.TemplateLibraries.AsQueryable

        If ParameterSetName = "search" Then
            If Not String.IsNullOrWhiteSpace(Search) Then
                query = query.Where(Function(x) x.Name.Contains(Search))
            End If

            For Each tl In GenerateResults(query, "TemplateLibrary")
                    WriteObject(tl)
                Next
            Else
                Dim tl As TemplateLibrary
            If ParameterSetName = "name" Then
                tl = ExecuteWithTimeout(query.Where(Function(x) x.Name.ToUpper.Equals(Name.ToUpper)).FirstOrDefaultAsync)
            Else
                Try
                    tl = ExecuteWithTimeout(query.Where(Function(x) x.TemplateLibraryId = TemplateLibraryId).FirstOrDefaultAsync)
                Catch ex As Exception
                    WriteError(StandardErrors.XDPMissing("TemplateLibrary", TemplateLibraryId.ToString, ex))
                End Try
            End If

            If tl IsNot Nothing Then WriteObject(tl)
        End If
    End Sub
End Class
