<Cmdlet(VerbsCommon.Get, "XdAddInPackage")>
<CmdletBinding(DefaultParameterSetName:="search")>
Public Class Get_XdAddInPackage
    Inherits baseCmdlet

    <Parameter(ParameterSetName:="search", Position:=0)>
    Public Property Search As String

    <Parameter(ParameterSetName:="name", Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
    Public Property Name As String

    Protected Overrides Sub EndProcessing()
        Dim query = xdp.AddInPackages.AsQueryable
        If ParameterSetName = "search" Then
            If Not String.IsNullOrWhiteSpace(Search) Then
                query = query.Where(Function(x) x.Name.Contains(Search))
            End If

            For Each p In GenerateResults(query, "AddInPackages")
                WriteObject(p)
            Next
        Else
            Dim p = ExecuteWithTimeout(query.Where(Function(x) x.Name.ToUpper.Equals(Name.ToUpper)).FirstOrDefaultAsync)
            If p IsNot Nothing Then WriteObject(p)
        End If
    End Sub

End Class
