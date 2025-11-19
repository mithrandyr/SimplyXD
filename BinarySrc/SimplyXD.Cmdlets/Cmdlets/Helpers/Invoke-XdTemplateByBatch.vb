<Cmdlet(VerbsLifecycle.Invoke, "XdTemplateByBatch")>
<CmdletBinding()>
Public Class Invoke_XdTemplateByBatch
    Inherits baseCmdlet

#Region "Parameters"
    <Parameter(Mandatory:=True)>
    Public Property TemplateLibraryName As String

    <Parameter(Mandatory:=True)>
    Public Property TemplateGroupName As String

    <Parameter(Mandatory:=True)>
    Public Property TemplateName As String

    <Parameter(Mandatory:=True)>
    Public Property BatchGroupName As String

    <Parameter(Mandatory:=True)>
    Public Property XmlData As String
#End Region

    Protected Overrides Sub EndProcessing()
        Dim bg As BatchGroup, b As Batch, d As Document, dp As DocumentProvider, dOp As DocumentOperation
        Try
            xdp.MergeOption = Microsoft.OData.Client.MergeOption.OverwriteChanges
            'Get BatchGroup
            bg = ExecuteWithTimeout(xdp.BatchGroups.Where(Function(x) x.Name.Equals(BatchGroupName)).FirstOrDefaultAsync)
            If bg Is Nothing Then
                WriteErrorMissing("BatchGroup", BatchGroupName)
                Exit Sub
            End If

            'Create the Batch
            b = New Batch With {
                .BatchGroupId = bg.BatchGroupId,
                .Description = "Created by Invoke-XdTemplateByBatch",
                .Name = String.Format("{0}_{1:yyyy-MM-dd_HH:mm:ss.fff}", Environment.UserName, DateTime.Now())
            }
            xdp.AddToBatches(b)
            SaveChanges(b)

            'Create the Document
            d = New Document With {.BatchId = b.BatchId, .SequenceNumber = 1}
            xdp.AddToDocuments(d)
            SaveChanges(d)

            'Create and attach Document Provider
            dp = New DocumentProvider With {
                .ContractName = Constants.DefaultContract,
                .DocumentId = d.DocumentId,
                .InputMetadata = String.Format(Constants.DefaultMetaData,
                                               TemplateLibraryName,
                                               TemplateGroupName,
                                               TemplateName,
                                               $"<![CDATA[{XmlData}]]>",
                                               Nothing)
            }
            xdp.AddToDocumentProviders(dp)
            SaveChanges(dp)

            'Create and attach DocumentOperation (convert to pdf)
            dOp = New DocumentOperation With {
                .DocumentId = d.DocumentId,
                .ContractName = Constants.AsposeContract,
                .InputMetadata = Constants.AsposeMetaData
            }
            xdp.AddToDocumentOperations(dOp)
            SaveChanges(dOp)

            'Execute batch
            Dim batchExecutionResult = ExecuteWithTimeout(b.ExecuteAndWait(TimeOut * 1000).GetValueAsync)

            If batchExecutionResult.BatchStatus <> BatchStatus.Completed Then
                b = ExecuteWithTimeout(xdp.Batches.Where(Function(x) x.BatchId = b.BatchId).FirstOrDefaultAsync)
                WriteErrorBatchFailed(b)
            End If
            xdp.DeleteObject(b)
            SaveChanges(b)
        Finally
            For Each entity In {bg, b, d, dp}
                If entity IsNot Nothing Then xdp.Detach(entity)
            Next
            xdp.MergeOption = Microsoft.OData.Client.MergeOption.NoTracking
        End Try
    End Sub
End Class
