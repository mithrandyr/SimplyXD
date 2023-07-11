<Cmdlet(VerbsCommon.[New], "XdTemplateGroup")>
<CmdletBinding(DefaultParameterSetName:="name")>
Public Class New_XdTemplateGroup
    Inherits baseCmdlet

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property TemplateLibrary As String

    <Parameter(Mandatory:=True, ParameterSetName:="id")>
    Public Property TemplateLibraryId As Guid


    <ValidateNotNullOrEmpty>
    <[Alias]("TemplateGroup")>
    <Parameter(Mandatory:=True, ParameterSetName:="name")>
    Public Property Name As String

    <Parameter()>
    Public Property Description As String

    Protected Overrides Sub EndProcessing()
        If ParameterSetName = "name" Then
            Dim result = ExecuteWithTimeout(xdp.TemplateLibraries.Where(Function(x) x.Name.ToUpper.Equals(TemplateLibrary.ToUpper)).FirstOrDefaultAsync)?.TemplateLibraryId
            If result Is Nothing Then
                WriteErrorMissing("TemplateLibrary", TemplateLibrary)
                Exit Sub
            End If
            TemplateLibraryId = result
        End If

        Dim nTemplateGroup As New TemplateGroup
        With nTemplateGroup
            .TemplateLibraryId = TemplateLibraryId
            .Name = Name
            .Description = Description
            .UseParentDocumentOutputPostActions = True
            .AddInContractName = "Xpertdoc.TemplateManager.Handler.TemplateDataProviders.XmlDataProvider"
        End With

        xdp.AddToTemplateGroups(nTemplateGroup)
        If SaveChanges(nTemplateGroup) Then WriteObject(nTemplateGroup)
    End Sub
End Class
