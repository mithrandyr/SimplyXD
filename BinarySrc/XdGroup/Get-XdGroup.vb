<Cmdlet(VerbsCommon.Get, "XdGroup")>
<CmdletBinding(DefaultParameterSetName:="search")>
Public Class Get_XdGroup
    Inherits baseCmdlet

    <Parameter(ParameterSetName:="search")>
    Public Property Search As String

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="name", ValueFromPipelineByPropertyName:=True)>
    Public Property Name As String

    <Parameter(Mandatory:=True, ParameterSetName:="id", ValueFromPipelineByPropertyName:=True)>
    Public Property GroupId As Guid

    Protected Overrides Sub EndProcessing()
        Dim query = xdp.Groups.AsQueryable
        Select Case ParameterSetName
            Case "search"
                If Not String.IsNullOrWhiteSpace(Search) Then
                    query = query.Where(Function(x) x.Name.Contains(Search) Or x.Description.Contains(Search))
                End If
                For Each gr In GenerateResults(query, "Group")
                    WriteObject(gr)
                Next

            Case "name"
                Dim g As Group = ExecuteWithTimeout(query.Where(Function(x) x.Name.ToUpper.Equals(Name.ToUpper)).FirstOrDefaultAsync)
                If g IsNot Nothing Then WriteObject(g)

            Case "id"
                Try
                    Dim g As Group = ExecuteWithTimeout(query.Where(Function(x) x.GroupId = GroupId).FirstOrDefaultAsync)
                Catch ex As Exception
                    WriteErrorMissing("Group", GroupId.ToString, ex)
                End Try
        End Select
    End Sub
End Class
