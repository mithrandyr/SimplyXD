Partial Public Class Engine
    Public Class UserProfile
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
