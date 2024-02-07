<Cmdlet(VerbsCommon.Remove, "XdBatchGroup")>
<CmdletBinding(DefaultParameterSetName:="name")>
Public Class RemoveBatchGroup
    Inherits baseCmdlet

    <Parameter(ParameterSetName:="id", Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
    Public Property BatchGroupId As Guid

    <ValidateNotNullOrEmpty>
    <Parameter(ParameterSetName:="name", Mandatory:=True, Position:=0)>
    Public Property Name As String

    <Parameter(Mandatory:=True, ParameterSetName:="obj")>
    Public Property InputObject As BatchGroup

    <Parameter()>
    Public Property Force As SwitchParameter

    Protected Overrides Sub ProcessRecord()
        If ParameterSetName = "name" Then
            WriteVerbose("Looking up the BatchGroupId")
            Dim result = ExecuteWithTimeout(xdp.BatchGroups.Where(Function(x) x.Name.ToUpper.Equals(Name.ToUpper)).FirstOrDefaultAsync)?.BatchGroupId
            If result Is Nothing Then
                WriteErrorMissing("BatchGroup", Name)
                Exit Sub
            End If
            BatchGroupId = result
        ElseIf ParameterSetName = "obj" Then
            BatchGroupId = InputObject.BatchGroupId
        End If

        WriteVerbose("Getting count of Batches in the BatchGroup")
        Dim batchCount As Integer = ExecuteWithTimeout(xdp.Batches.Where(Function(x) x.BatchGroupId = BatchGroupId).CountAsync)
        If batchCount = 0 OrElse Force.IsPresent Then
            Dim dBatchGroup = New BatchGroup With {.BatchGroupId = BatchGroupId}
            xdp.AttachTo("BatchGroups", dBatchGroup)
            xdp.DeleteObject(dBatchGroup)
            WriteVerbose("Deleting the BatchGroup")
            SaveChanges(dBatchGroup)
        Else
            WriteErrorNotEmpty("BatchGroup", If(Name, BatchGroupId.ToString), batchCount)

        End If
    End Sub

End Class