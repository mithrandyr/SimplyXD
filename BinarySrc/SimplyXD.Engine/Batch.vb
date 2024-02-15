Imports SimplyXD.Engine
Imports System.Reflection

Partial Public Class Engine
    Public Class Batch
        Public Shared Function GetBatchById(batchid As Guid) As Xpertdoc.Portal.SdkCore.Batch
            'NOTE TO SELF --> need to figure out a different way to account for the logic.. maybe all in one big class? named: BatchGet, BatchNew, etc?
            ' or have classes "Get", "GetByID" "Search" and each one has Batch, BatchGroup, etc?
            Try
                Return ExecuteWithTimeout(XDP.Batches.Expand(Function(x) x.BatchGroup).Where(Function(x) x.BatchId = batchid).FirstOrDefaultAsync)
            Catch ex As Exception
                Throw New XDPItemMissingException("Batch", batchid.ToString(), ex)
            End Try
        End Function

        Public Shared Iterator Function GetBatchByBatchGroupId(batchGroupId As Guid, Optional limit As Integer = 0) As IEnumerable(Of Batch)
        End Function

        'Public Shared Function GetAddInPackage(name As String) As AddInPackage
        '    Dim query = XDP.AddInPackages.Where(Function(x) x.Name.ToUpper.Equals(name.ToUpper)).FirstOrDefaultAsync

        '    Return ExecuteWithTimeout(query)
        'End Function
        'Public Shared Iterator Function SearchAddInPackage(Optional search As String = "") As IEnumerable(Of AddInPackage)
        '    Dim query = XDP.AddInPackages.AsQueryable
        '    If Not String.IsNullOrWhiteSpace(search) Then
        '        query = query.Where(Function(x) x.Name.Contains(search))
        '    End If

        '    For Each p In GenerateResults(query, "AddInPackages")
        '        Yield p
        '    Next
        'End Function
    End Class
End Class