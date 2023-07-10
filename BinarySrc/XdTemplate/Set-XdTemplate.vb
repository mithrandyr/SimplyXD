<Cmdlet(VerbsCommon.Set, "XdTemplate")>
<CmdletBinding(DefaultParameterSetName:="name")>
Public Class Set_XdTemplate
    Inherits baseCmdlet
    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property TemplateLibrary As String

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property TemplateGroup As String

    <[Alias]("TemplateName")>
    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property Name As String

    <Parameter(Mandatory:=True, ParameterSetName:="id")>
    Public Property TemplateId As Guid

    <Parameter()>
    Public Property Description As String

    <Parameter()>
    Public Property Passthru As SwitchParameter

    Protected Overrides Sub EndProcessing()
        Try
            xdp.MergeOption = Microsoft.OData.Client.MergeOption.OverwriteChanges
            Dim uTemplate As Template
            Dim query = xdp.Templates.Expand(Function(x) x.TemplateGroup.TemplateLibrary).AsQueryable
            Try
                Select Case ParameterSetName
                    Case "name"
                        uTemplate = ExecuteWithTimeout(query.Where(Function(x) x.Name.ToUpper.Equals(Name.ToUpper) And x.TemplateGroup.Name.ToUpper.Equals(TemplateGroup.ToUpper) And x.TemplateGroup.TemplateLibrary.Name.ToUpper.Equals(TemplateLibrary.ToUpper)).FirstOrDefaultAsync)
                        If uTemplate Is Nothing Then
                            WriteErrorMissing("Template", $"{TemplateLibrary}\{TemplateGroup}\{Name}")
                            Exit Sub
                        End If
                    Case "id"
                        uTemplate = ExecuteWithTimeout(query.Where(Function(x) x.TemplateId = TemplateId).FirstOrDefaultAsync)
                End Select
            Catch ex As Exception
                WriteError(WrappedException.CreateErrorRecord(ex, "TemplateRetrievalFailed"))
                Exit Sub
            End Try

            If MyInvocation.BoundParameters.ContainsKey("Description") Then
                uTemplate.Description = Description
                xdp.UpdateObject(uTemplate)

                WriteVerbose("Upating Template details")
                If SaveChanges() AndAlso Passthru.IsPresent Then
                    WriteObject(uTemplate.AsPSObject)
                End If
                xdp.Detach(uTemplate)
            End If
        Finally
            xdp.MergeOption = Microsoft.OData.Client.MergeOption.NoTracking
        End Try
    End Sub
End Class
