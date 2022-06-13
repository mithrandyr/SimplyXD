
<Cmdlet(VerbsCommon.Get, "XdGroupMember")>
<CmdletBinding(DefaultParameterSetName:="name")>
Public Class Get_XdGroupMember
    Inherits baseCmdlet

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="name", ValueFromPipelineByPropertyName:=True, Position:=0)>
    Public Property GroupName As String

    <Parameter(Mandatory:=True, ParameterSetName:="id", ValueFromPipelineByPropertyName:=True)>
    Public Property GroupId As Guid

    Protected Overrides Sub ProcessRecord()
        Dim query = xdp.UserGroups.Expand(Function(x) x.UserProfile).Expand(Function(x) x.Group).AsQueryable

        If ParameterSetName = "name" Then
            query = query.Where(Function(x) x.Group.Name.ToUpper.Equals(GroupName.ToUpper))
        Else
            query = query.Where(Function(x) x.GroupId = GroupId)
        End If

        For Each ug In GenerateResults(query, "UserGroup")
            WriteObject(ug.UserProfile)
        Next

    End Sub
End Class


