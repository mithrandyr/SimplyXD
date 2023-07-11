<Cmdlet(VerbsCommon.Remove, "XdUserProfile")>
<CmdletBinding(DefaultParameterSetName:="id")>
Public Class Remove_XdUserProfile
    Inherits baseCmdlet

#Region "PowerShell Properties"
    <Parameter(Mandatory:=True, ParameterSetName:="obj", ValueFromPipeline:=True)>
    Property InputObject As UserProfile

    <Parameter(Mandatory:=True, ParameterSetName:="id")>
    Property UserProfileId As Guid

    <ValidateNotNullOrEmpty>
    <[Alias]("UserName")>
    <Parameter(Mandatory:=True, ParameterSetName:="username")>
    Property Name As String
#End Region

    Protected Overrides Sub ProcessRecord()
        If ParameterSetName = "username" Then
            Dim result = ExecuteWithTimeout(xdp.UserProfiles.Where(Function(x) x.Name.ToUpper.Equals(Name.ToUpper)).FirstOrDefaultAsync)?.UserProfileId
            If result Is Nothing Then
                WriteErrorMissing("UserProfile", Name)
                Exit Sub
            End If
            UserProfileId = result
        ElseIf ParameterSetName = "obj" Then
            UserProfileId = InputObject.UserProfileId
        End If

        Dim dUserProfile = New UserProfile With {.UserProfileId = UserProfileId}
        xdp.AttachTo("UserProfiles", dUserProfile)

        xdp.DeleteObject(dUserProfile)
        SaveChanges()
    End Sub

End Class
