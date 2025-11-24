<Cmdlet(VerbsCommon.[New], "XdBatchGroup")>
<CmdletBinding()>
Public Class NewBatchGroup
    Inherits baseCmdlet

    <Parameter(Mandatory:=True, Position:=0)>
    Public Property Name As String

    <Parameter(Position:=1)>
    Public Property Description As String

    Protected Overrides Sub EndProcessing()
        Dim nBatchGroup As New BatchGroup With {.Name = Name, .Description = Description}
        xdp.AddToBatchGroups(nBatchGroup)
        If SaveChanges(nBatchGroup) Then WriteObject(nBatchGroup)
        xdp.Detach(nBatchGroup)
    End Sub
End Class