<Cmdlet(VerbsCommon.Get, "XdConfig")>
<CmdletBinding()>
Public Class Get_XdConfig
    Inherits baseCmdlet
    Protected Overrides Sub EndProcessing()
        Dim logLevelTask = xdp.Configurations.GetLogLevel.GetValueAsync
        Dim externalUrlTask = xdp.Configurations.GetExternalUrl.GetValueAsync
        Dim archiveTask = xdp.Configurations.GetArchiveConfig.GetValueAsync

        Try
            ExecuteWithTimeout({logLevelTask, externalUrlTask, archiveTask})
        Catch ex As Exception
            WriteError(StandardErrors.GenericWrappedError(ex, Nothing))
            Exit Sub
        End Try

        Dim pso As New PSObject
        With pso.Properties
            .Add(New PSNoteProperty("LogLevel", logLevelTask.Result))
            .Add(New PSNoteProperty("ExternalUrl", externalUrlTask.Result))
            .Add(New PSNoteProperty("ArchiveConfig", archiveTask.Result))
        End With

        WriteObject(pso)
    End Sub
End Class