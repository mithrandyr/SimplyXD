<Cmdlet(VerbsCommon.Add, "XdDocumentProvider")>
<CmdletBinding(DefaultParameterSetName:="long")>
Public Class AddXdDocumentProvider
    Inherits baseCmdlet

    <Parameter(Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
    Public Property DocumentId As Guid

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="long")>
    Public Property XmlData As String

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="long")>
    <[Alias]("TemplateLibraryName")>
    Public Property TemplateLibrary As String

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="long")>
    <[Alias]("TemplateGroupName")>
    Public Property TemplateGroup As String

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="long")>
    Public Property TemplateName As String

    <Parameter(Mandatory:=False, ParameterSetName:="long")>
    Public Property DopaName As String

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="short")>
    Public Property ContractName As String = "Xpertdoc.DocumentServices.Handler.Operations.ExecuteTemplateDocumentProvider"

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="short")>
    Public Property InputMetaData As String

    Private inputPattern As String = "<ExecuteTemplateDocumentProviderMetadata><TemplateLibraryName>{0}</TemplateLibraryName><TemplateGroupName>{1}</TemplateGroupName><TemplateName>{2}</TemplateName><TemplateId></TemplateId><InMemory>true</InMemory><ExecutionData>{3}</ExecutionData><DocumentOutputPostActionName>{4}</DocumentOutputPostActionName></ExecuteTemplateDocumentProviderMetadata>"

    Protected Overrides Sub EndProcessing()
        Dim docProv As New DocumentProvider With {.ContractName = ContractName, .DocumentId = DocumentId, .Status = ExecutionStatus.Created}
        If ParameterSetName = "short" Then
            docProv.InputMetadata = InputMetaData
        Else
            docProv.InputMetadata = String.Format(inputPattern, TemplateLibrary, TemplateGroup, TemplateName, $"<![CDATA[{XmlData}]]>", DopaName)
        End If

        xdp.AddToDocumentProviders(docProv)
        If SaveChanges(docProv) Then WriteObject(docProv)
    End Sub
End Class
