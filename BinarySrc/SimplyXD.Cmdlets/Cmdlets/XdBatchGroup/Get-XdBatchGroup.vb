<Cmdlet(VerbsCommon.Get, "XdBatchGroup")>
<CmdletBinding(DefaultParameterSetName:="search")>
Public Class GetBatchGroup
    Inherits baseCmdlet

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="one")>
    Public Property Name As String

    <Parameter(Mandatory:=False, ParameterSetName:="search", Position:=0)>
    Public Property Search As String

    <Parameter(Mandatory:=True, ParameterSetName:="id")>
    Public Property BatchGroupId As Guid

    <Parameter()>
    Public Property IncludeCount As SwitchParameter

    Protected Overrides Sub EndProcessing()
        Select Case ParameterSetName
            Case "search"
                Dim query = xdp.BatchGroups.AsQueryable
                If Not String.IsNullOrWhiteSpace(Search) Then
                    query = query.Where(Function(x) x.Name.Contains(Search))
                End If
                For Each gr In GenerateResults(query, "BatchGroup")
                    SendObject(gr)
                Next
            Case "one"
                Dim bg = ExecuteWithTimeout(xdp.BatchGroups.Where(Function(x) x.Name.ToUpper().Equals(Name.ToUpper())).FirstOrDefaultAsync())
                SendObject(bg)
            Case "id"
                Try
                    Dim bg = ExecuteWithTimeout(xdp.BatchGroups.Where(Function(x) x.BatchGroupId = BatchGroupId).FirstOrDefaultAsync)
                    SendObject(bg)
                Catch ex As Exception
                    WriteErrorMissing("BatchGroup", BatchGroupId.ToString, ex)
                End Try
        End Select
    End Sub

    Private Sub SendObject(x As BatchGroup)
        If IncludeCount Then
            WriteObject(AppendCount(x, xdp.Batches.Where(Function(b) b.BatchGroupId = x.BatchGroupId).CountAsync(), "BatchCount"))
        Else
            WriteObject(x)
        End If
    End Sub
End Class
