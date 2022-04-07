
<Cmdlet(VerbsCommon.Get, "XdGroupMember")>
<CmdletBinding(DefaultParameterSetName:="name")>
Public Class Get_XdGroupMember
    Inherits baseCmdlet
    <Parameter(Mandatory:=True, ParameterSetName:="name", ValueFromPipelineByPropertyName:=True)>
    Public Property GroupName As String

    <Parameter(Mandatory:=True, ParameterSetName:="id", ValueFromPipelineByPropertyName:=True)>
    Public Property GroupId As Guid

    Protected Overrides Sub ProcessRecord()

        If ParameterSetName = "name" Then
            Dim g As Group = ExecuteWithTimeout(xdp.Groups.Where(Function(x) x.Name.Equals(GroupName)).FirstOrDefaultAsync)
            If g Is Nothing Then
                WriteWarning(String.Format("Cannot find group '{0}'.", GroupName))
                Exit Sub
            End If
            GroupId = g.GroupId
        End If

        Dim qUserGroups = xdp.UserGroups.Where(Function(x) x.GroupId = GroupId)

        For Each up In LookupResultsByGuid(qUserGroups, Function(x) x.UserProfileId, xdp.UserProfiles, Function(x) x.UserProfileId, "UserProfile")
            WriteObject(up)
        Next

    End Sub
End Class


