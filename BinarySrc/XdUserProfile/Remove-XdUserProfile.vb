<Cmdlet(VerbsCommon.Remove, "XdUserProfile")>
<CmdletBinding(DefaultParameterSetName:="id")>
Public Class Remove_XdUserProfile
    Inherits baseCmdlet

#Region "PowerShell Properties"
    <Parameter(Mandatory:=True, ParameterSetName:="id", ValueFromPipelineByPropertyName:=True)>
    Property UserProfileId As Guid

    <Parameter(Mandatory:=True, ParameterSetName:="username", ValueFromPipelineByPropertyName:=True)>
    Property UserName As String
#End Region

    Protected Overrides Sub ProcessRecord()
        Dim dUserProfile As UserProfile
        If ParameterSetName = "username" Then
            dUserProfile = ExecuteWithTimeout(xdp.UserProfiles.Where(Function(x) x.Name = UserName).FirstOrDefaultAsync)
            If dUserProfile Is Nothing Then
                WriteWarning(String.Format("Cannot find user '{0}' to remove.", UserName))
                Exit Sub
            End If
        Else
            dUserProfile = New UserProfile With {.UserProfileId = UserProfileId}
            xdp.AttachTo("UserProfiles", dUserProfile)
        End If

        xdp.DeleteObject(dUserProfile)
        SaveChanges()
    End Sub

End Class
