<Cmdlet(VerbsCommon.[New], "XdDocumentProvider")>
<CmdletBinding(DefaultParameterSetName:="long")>
Public Class NewXdDocumentProvider
    Inherits baseCmdlet

    <Parameter(Mandatory:=True)>
    Public Property DocumentId As String

    <Parameter(Mandatory:=True, ParameterSetName:="long")>
    Public Property XmlData As String

    <Parameter(Mandatory:=True, ParameterSetName:="long")>
    Public Property LibraryName As String

    <Parameter(Mandatory:=True, ParameterSetName:="long")>
    Public Property GroupName As String

    <Parameter(Mandatory:=True, ParameterSetName:="long")>
    Public Property TemplateName As String

    <Parameter(Mandatory:=False, ParameterSetName:="long")>
    Public Property DopaName As String

    <Parameter(Mandatory:=True, ParameterSetName:="short")>
    Public Property ContractName As String = "Xpertdoc.DocumentServices.Handler.Operations.ExecuteTemplateDocumentProvider"

    <Parameter(Mandatory:=True, ParameterSetName:="short")>
    Public Property InputMetaData As String

    Private inputPattern As String = "<ExecuteTemplateDocumentProviderMetadata><TemplateLibraryName>{0}</TemplateLibraryName><TemplateGroupName>{1}</TemplateGroupName><TemplateName>{2}</TemplateName><TemplateId></TemplateId><InMemory>true</InMemory><ExecutionData>{3}</ExecutionData><DocumentOutputPostActionName>{4}</DocumentOutputPostActionName></ExecuteTemplateDocumentProviderMetadata>"

    Protected Overrides Sub EndProcessing()
        Dim docProv As New DocumentProvider With {.ContractName = ContractName, .DocumentId = Guid.Parse(DocumentId), .Status = ExecutionStatus.Created}
        If ParameterSetName = "short" Then
            docProv.InputMetadata = InputMetaData
        Else
            docProv.InputMetadata = String.Format(inputPattern, LibraryName, GroupName, TemplateName, Security.SecurityElement.Escape(XmlData), DopaName)
        End If

        xdp.AddToDocumentProviders(docProv)
        SaveChanges(docProv)
        WriteObject(docProv)
    End Sub

End Class
