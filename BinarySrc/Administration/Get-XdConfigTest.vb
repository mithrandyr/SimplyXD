<Cmdlet(VerbsCommon.Get, "XdConfig")>
<CmdletBinding()>
Public Class Get_XdConfig
    Inherits baseCmdlet
    Protected Overrides Sub EndProcessing()
        Dim externalUrlTask = xdp.Configurations.GetExternalUrl.GetValueAsync
        Dim templateTimeoutTask = xdp.Configurations.GetTemplateManagerCompareTimeout.GetValueAsync
        Dim contentTimeoutTask = xdp.Configurations.GetContentManagerCompareTimeout.GetValueAsync
        Dim logLevelTask = xdp.Configurations.GetLogLevel.GetValueAsync
        Dim smtpTask = xdp.Configurations.GetSmtpConfig.GetValueAsync
        Dim archiveTask = xdp.Configurations.GetArchiveConfig.GetValueAsync
        Dim searchTask = xdp.Configurations.GetFullTextSearchConfig.GetValueAsync
        Dim indexTask = xdp.Configurations.GetIndexOptimizationConfig.GetValueAsync

        Try
            ExecuteWithTimeout({
                    externalUrlTask,
                    templateTimeoutTask,
                    contentTimeoutTask,
                    logLevelTask,
                    smtpTask,
                    archiveTask,
                    searchTask,
                    indexTask
                })
        Catch ex As Exception
            WriteError(WrappedException.CreateErrorRecord(ex, Nothing))
            Exit Sub
        End Try

        Dim pso As New PSObject
        With pso.Properties
            .Add(New PSNoteProperty("System", New System With {
                                        .ExternalUrl = externalUrlTask.Result,
                                        .DocumentCompareTimeoutMs = contentTimeoutTask.Result,
                                        .TemplateCompareTimeoutMs = templateTimeoutTask.Result,
                                        .LogLevel = logLevelTask.Result
                                    }))
            .Add(New PSNoteProperty("SMTP", smtpTask.Result))
            .Add(New PSNoteProperty("Archiving", archiveTask.Result))
            .Add(New PSNoteProperty("FullTextSearch", searchTask.Result))
            .Add(New PSNoteProperty("IndexOptimization", indexTask.Result))
        End With

        WriteObject(pso)

    End Sub
    Public Class System
        Public Property ExternalUrl As String
        Public Property DocumentCompareTimeoutMs As Integer
        Public Property TemplateCompareTimeoutMs As Integer
        Public Property LogLevel As String
    End Class
End Class