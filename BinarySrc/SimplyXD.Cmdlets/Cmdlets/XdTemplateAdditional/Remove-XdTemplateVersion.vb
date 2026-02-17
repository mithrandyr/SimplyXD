<Cmdlet(VerbsCommon.Remove, "XdTemplateVersion", SupportsShouldProcess:=True)>
Public Class Remove_XdTemplateVersion
    Inherits baseCmdlet

    <Parameter(Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
    Property TemplateVersionHistoryId As Guid
    Dim tvhList As New List(Of Guid)

    Protected Overrides Sub ProcessRecord()
        tvhList.Add(TemplateVersionHistoryId)
    End Sub
    Protected Overrides Sub EndProcessing()

        Dim counter As Integer = 0
        For Each tvhId In tvhList
            WriteProgress(New ProgressRecord(0, MyInvocation.MyCommand.Name, $"Id:{TemplateVersionHistoryId} | Total: {tvhList.Count}") With {.PercentComplete = counter * 100 / tvhList.Count})

            Dim tv = ExecuteWithTimeout(xdp.
                                    TemplateVersionsHistory.
                                    Expand(Function(x) x.Template.TemplateGroup.TemplateLibrary).
                                    Where(Function(x) x.TemplateVersionHistoryId = TemplateVersionHistoryId).
                                    Take(1).ToListAsync).FirstOrDefault
            If tv Is Nothing Then
                If ShouldProcess(TemplateVersionHistoryId.ToString, "Delete (Missing)") Then
                    WriteErrorMissing("TemplateVersion", TemplateVersionHistoryId.ToString)
                End If
            Else
                Dim tvhName = $"{tv.Template.TemplateGroup.TemplateLibrary.Name}\{tv.Template.TemplateGroup.Name}\{tv.Template.Name} ({tv.Version})"
                If tv.CreatedDate = tv.Template.ModifiedDate Then
                    If ShouldProcess(tvhName, "Delete (InvalidOp)") Then
                        WriteErrorInvalidOp("TemplateVersion", TemplateVersionHistoryId.ToString, "Delete", "it is the latest version")
                    End If
                Else
                    If ShouldProcess(tvhName, "Delete TemplateVersion") Then
                        WriteVerbose($"TemplateVersion: {tvhName}")
                        xdp.AttachTo("TemplateVersionsHistory", tv)
                        xdp.DeleteObject(tv)
                        SaveChanges(tv)
                    End If
                End If
            End If

            counter += 1
        Next
        FinishWriteProgress()
    End Sub
End Class
