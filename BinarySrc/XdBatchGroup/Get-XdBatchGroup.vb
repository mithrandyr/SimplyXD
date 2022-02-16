<Cmdlet(VerbsCommon.Get, "XdBatchGroup")>
<CmdletBinding(DefaultParameterSetName:="search")>
Public Class GetBatchGroup
    Inherits baseCmdlet

    <Parameter(Mandatory:=True, ParameterSetName:="one")>
    Public Property BatchGroup As String

    <Parameter(Mandatory:=False, ParameterSetName:="search")>
    Public Property Search As String

    Protected Overrides Sub EndProcessing()
        If ParameterSetName = "search" Then
            Dim query = xdp.BatchGroups.AsQueryable
            If Not String.IsNullOrWhiteSpace(Search) Then
                query = query.Where(Function(x) x.Name.Contains(Search))
            End If
            For Each gr In GenerateResults(query, "BatchGroup")
                WriteObject(gr)
            Next
        Else
            Dim bg = ExecuteWithTimeout(xdp.BatchGroups.Where(Function(x) x.Name.ToUpper().Equals(BatchGroup.ToUpper())).FirstOrDefaultAsync())
            WriteObject(bg)
        End If
    End Sub
End Class
