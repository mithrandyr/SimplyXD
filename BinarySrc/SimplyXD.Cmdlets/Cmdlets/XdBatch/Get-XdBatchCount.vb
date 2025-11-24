<Cmdlet(VerbsCommon.Get, "XdBatchCount")>
<CmdletBinding()>
Public Class GetBatchCount
    Inherits baseCmdlet

    <Parameter(ValueFromPipelineByPropertyName:=True)>
    Public Property BatchGroupId As Guid

    Protected Overrides Sub ProcessRecord()
        Dim query = xdp.Batches.AsQueryable
        Dim hasBatchGroupId As Boolean = False

        If Not BatchGroupId.Equals(Guid.Empty) Then
            query = query.Where(Function(x) x.BatchGroupId = BatchGroupId)
            hasBatchGroupId = True
        End If


        Dim totalCountTask = query.CountAsync
        Dim completedCountTask = query.Where(Function(x) x.Status = BatchStatus.Completed).CountAsync
        Dim createdCountTask = query.Where(Function(x) x.Status = BatchStatus.Created).CountAsync
        Dim erroredCountTask = query.Where(Function(x) x.Status = BatchStatus.Error).CountAsync
        Dim queuedCountTask = query.Where(Function(x) x.Status = BatchStatus.Queued).CountAsync
        Dim runningCountTask = query.Where(Function(x) x.Status = BatchStatus.Running).CountAsync
        Dim timedoutCountTask = query.Where(Function(x) x.Status = BatchStatus.TimedOut).CountAsync
        Dim groupNameTask = xdp.BatchGroups.Where(Function(x) x.BatchGroupId = BatchGroupId).ToListAsync

        Try
            ExecuteWithTimeout({
                    totalCountTask,
                    completedCountTask,
                    createdCountTask,
                    erroredCountTask,
                    queuedCountTask,
                    runningCountTask,
                    timedoutCountTask,
                    If(hasBatchGroupId, groupNameTask, Task.CompletedTask)
                })
        Catch ex As Exception
            WriteErrorWrapped(ex, Nothing)
            Exit Sub
        End Try

        If BatchGroupId.Equals(Guid.Empty) Then
            WriteObject(New BatchCount With {
                        .Total = totalCountTask.Result,
                        .Completed = completedCountTask.Result,
                        .Created = createdCountTask.Result,
                        .Errored = erroredCountTask.Result,
                        .Queued = queuedCountTask.Result,
                        .Running = runningCountTask.Result,
                        .TimedOut = timedoutCountTask.Result
                        })
        Else
            WriteObject(New GroupBatchCount With {
                        .Total = totalCountTask.Result,
                        .Completed = completedCountTask.Result,
                        .Created = createdCountTask.Result,
                        .Errored = erroredCountTask.Result,
                        .Queued = queuedCountTask.Result,
                        .Running = runningCountTask.Result,
                        .TimedOut = timedoutCountTask.Result,
                        .BatchGroupId = BatchGroupId,
                        .BatchGroupName = If(groupNameTask.Result.Count = 1, groupNameTask.Result.First.Name, "Invalid BatchGroupID")
                        })
        End If

    End Sub

    Private Class BatchCount
        Public Created As Int32
        Public Queued As Int32
        Public Running As Int32
        Public Completed As Int32
        Public Errored As Int32
        Public TimedOut As Int32
        Public Total As Int32
    End Class

    Private Class GroupBatchCount
        Inherits BatchCount
        Public BatchGroupName As String
        Public BatchGroupId As Guid
    End Class
End Class