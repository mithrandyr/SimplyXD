
<Cmdlet(VerbsCommon.Add, "XdGroupMember")>
<CmdletBinding(DefaultParameterSetName:="name-name")>
Public Class Add_XdGroupMember
    Inherits baseCmdlet

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="name-name", ValueFromPipelineByPropertyName:=True)>
    <Parameter(Mandatory:=True, ParameterSetName:="name-id", ValueFromPipelineByPropertyName:=True)>
    Public Property GroupName As String

    <Parameter(Mandatory:=True, ParameterSetName:="id-name", ValueFromPipelineByPropertyName:=True)>
    <Parameter(Mandatory:=True, ParameterSetName:="id-id", ValueFromPipelineByPropertyName:=True)>
    Public Property GroupId As Guid

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ParameterSetName:="name-name", ValueFromPipelineByPropertyName:=True)>
    <Parameter(Mandatory:=True, ParameterSetName:="id-name", ValueFromPipelineByPropertyName:=True)>
    Public Property UserName As String

    <Parameter(Mandatory:=True, ParameterSetName:="name-id", ValueFromPipelineByPropertyName:=True)>
    <Parameter(Mandatory:=True, ParameterSetName:="id-id", ValueFromPipelineByPropertyName:=True)>
    Public Property UserProfileId As Guid

    Protected Overrides Sub ProcessRecord()

        If ParameterSetName.StartsWith("name") Then
            Dim g As Group = ExecuteWithTimeout(xdp.Groups.Where(Function(x) x.Name.Equals(GroupName)).FirstOrDefaultAsync)
            If g Is Nothing Then
                WriteError(StandardErrors.XDPMissing("Group", GroupName))
                Exit Sub
            End If
            GroupId = g.GroupId
        End If

        If ParameterSetName.EndsWith("name") Then
            Dim u As UserProfile = ExecuteWithTimeout(xdp.UserProfiles.Where(Function(x) x.Name.Equals(UserName)).FirstOrDefaultAsync)
            If u Is Nothing Then
                WriteError(StandardErrors.XDPMissing("UserProfile", UserName))
                Exit Sub
            End If
            UserProfileId = u.UserProfileId
        End If

        Dim nUG As New UserGroup With {.GroupId = GroupId, .UserProfileId = UserProfileId}

        xdp.AddToUserGroups(nUG)
        SaveChanges(nUG)
    End Sub
End Class


