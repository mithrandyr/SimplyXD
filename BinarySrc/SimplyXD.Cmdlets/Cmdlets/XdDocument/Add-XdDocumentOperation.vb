<Cmdlet(VerbsCommon.Add, "XdDocumentOperation")>
<CmdletBinding(DefaultParameterSetName:="short")>
Public Class Add_XdDocumentOperation
    Inherits baseCmdlet

    <Parameter(Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
    Public Property DocumentId As Guid

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="short")>
    Public Property ContractName As String

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="short")>
    Public Property InputMetaData As String

    <Parameter(Mandatory:=True, ParameterSetName:="asposepdf")>
    Public Property AsposePDF As SwitchParameter

    Protected Overrides Sub ProcessRecord()
        If AsposePDF Then
            ContractName = Constants.AsposeContract
            InputMetaData = Constants.AsposeMetaData
        End If

        Dim docOperation As New DocumentOperation With {
            .ContractName = ContractName,
            .InputMetadata = InputMetaData,
            .DocumentId = DocumentId,
            .Status = ExecutionStatus.Created
        }

        xdp.AddToDocumentOperations(docOperation)
        If SaveChanges(docOperation) Then WriteObject(docOperation)
        xdp.Detach(docOperation)
    End Sub
End Class
