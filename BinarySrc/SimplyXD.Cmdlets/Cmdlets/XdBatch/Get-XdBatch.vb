<Cmdlet(VerbsCommon.Get, "XdBatch")>
<CmdletBinding(DefaultParameterSetName:="search")>
Public Class GetBatch
    Inherits baseCmdlet

    <Parameter(ParameterSetName:="search", ValueFromPipelineByPropertyName:=True)>
    Public Property BatchGroupId As Guid

    <Parameter(ParameterSetName:="search")>
    <ValidateRange(0, 1000)>
    Public Property Limit As Integer = 0

    <Parameter(ParameterSetName:="search")>
    <ValidateSet("Ascending", "Descending")>
    Public Property SortByCreation As String

    <Parameter(ParameterSetName:="search")>
    <ValidateSet("Completed", "Created", "Error", "Queued", "Running", "TimedOut")>
    Public Property Status As String

    <Parameter(ParameterSetName:="search", Position:=0)>
    Public Property Search As String

    <Parameter(ParameterSetName:="batch", Position:=0)>
    Public Property BatchId As Guid

    Protected Overrides Sub ProcessRecord()
        Dim query = xdp.Batches.Expand(Function(x) x.BatchGroup).AsQueryable
        If ParameterSetName = "batch" Then
            Try
                Dim b = ExecuteWithTimeout(query.Where(Function(x) x.BatchId = BatchId).FirstOrDefaultAsync)
                WriteObject(b)
            Catch ex As Exception
                WriteErrorMissing("Batch", BatchId.ToString, ex)
            End Try
        Else
            If Not BatchGroupId.Equals(Guid.Empty) Then query = query.Where(Function(x) x.BatchGroupId = BatchGroupId)
            If Not String.IsNullOrWhiteSpace(Search) Then query = query.Where(Function(x) x.Name.Contains(Search))

            If Not String.IsNullOrWhiteSpace(Status) Then
                Dim enumBatchStatus As BatchStatus = [Enum].Parse(GetType(BatchStatus), Status)
                query = query.Where(Function(x) x.Status = enumBatchStatus)
            End If

            If SortByCreation = "Ascending" Then query = query.OrderBy(Function(x) x.CreatedDate)
            If SortByCreation = "Descending" Then query = query.OrderByDescending(Function(x) x.CreatedDate)

            For Each gr In GenerateResults(query, "Batch", Limit)
                WriteObject(gr)
            Next
        End If
    End Sub
End Class