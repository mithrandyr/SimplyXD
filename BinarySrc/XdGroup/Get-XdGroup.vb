<Cmdlet(VerbsCommon.Get, "XdGroup")>
<CmdletBinding(DefaultParameterSetName:="search")>
Public Class Get_XdProfile
    Inherits baseCmdlet

    <Parameter(ParameterSetName:="search")>
    Public Property Search As String

    <Parameter(Mandatory:=True, ParameterSetName:="name", ValueFromPipelineByPropertyName:=True)>
    Public Property GroupName As String

    <Parameter(Mandatory:=True, ParameterSetName:="id", ValueFromPipelineByPropertyName:=True)>
    Public Property GroupId As Guid

    Protected Overrides Sub EndProcessing()
        Dim query = xdp.Groups.AsQueryable

        If ParameterSetName = "search" Then
            If Not String.IsNullOrWhiteSpace(Search) Then
                query = query.Where(Function(x) x.Name.Contains(Search) Or x.Description.Contains(Search))
            End If
            For Each gr In GenerateResults(query, "Group")
                WriteObject(gr)
            Next
        Else
            Dim g As Group
            If ParameterSetName = "name" Then
                g = ExecuteWithTimeout(query.Where(Function(x) x.Name.ToUpper.Equals(GroupName.ToUpper)).FirstOrDefaultAsync)
            Else
                g = ExecuteWithTimeout(query.Where(Function(x) x.GroupId = GroupId).FirstOrDefaultAsync)
                'need to provide error checking -- if the result is a 404... then report that the entity doesn't exist
            End If

            If g IsNot Nothing Then WriteObject(g)
        End If
    End Sub
End Class
