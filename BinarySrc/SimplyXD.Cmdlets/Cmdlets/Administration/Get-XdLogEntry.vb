<Cmdlet(VerbsCommon.Get, "XdLogEntry")>
<CmdletBinding()>
Public Class Get_XdLogEntry
    Inherits baseCmdlet

    <Parameter()>
    Public Property AsCount As SwitchParameter

    <Parameter()>
    Public Property TailCount As Integer = 100

    <Parameter()>
    Public Property OnOrAfter As DateTime?
    <Parameter()>
    Public Property Before As DateTime?

    <Parameter()>
    <ValidateSet("Trace", "Debug", "Info", "Warn", "Error", "Fatal")>
    Public Property Level As String()

    Protected Overrides Sub EndProcessing()
        Dim query = xdp.LogEntries.AsQueryable

        If Level?.Count > 0 Then query = query.Where(Function(x) Level.Contains(x.Level))


        If OnOrAfter IsNot Nothing Then query = query.Where(Function(x) x.Date >= OnOrAfter)
        If Before IsNot Nothing Then query = query.Where(Function(x) x.Date < Before)

        If AsCount Then
            WriteObject(ExecuteWithTimeout(query.CountAsync()))
            Exit Sub
        Else
            If TailCount > 0 Then query = query.OrderByDescending(Function(x) x.Date)
            For Each p In GenerateResults(query, "LogEntries", TailCount)
            WriteObject(p)
            Next
        End If
    End Sub

End Class
