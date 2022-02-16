<Cmdlet(VerbsCommon.Set, "XdUserProfile")>
<CmdletBinding()>
Public Class Set_XdUserProfile
    Inherits baseCmdlet

#Region "PowerShell Properties"
    <Parameter(Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
    Property UserProfileId As Guid

    <Parameter(ValueFromPipelineByPropertyName:=True)>
    <ValidateNotNullOrEmpty>
    Property UserName As String

    <Parameter(ValueFromPipelineByPropertyName:=True)>
    <ValidateNotNullOrEmpty>
    Property Email As String

    <Parameter(ValueFromPipelineByPropertyName:=True)>
    Property FirstName As String

    <Parameter(ValueFromPipelineByPropertyName:=True)>
    Property LastName As String
#End Region

    Protected Overrides Sub ProcessRecord()
        Dim nUserProfile As UserProfile = ExecuteWithTimeout(xdp.UserProfiles.Where(Function(x) x.UserProfileId = UserProfileId).FirstOrDefaultAsync)
        If nUserProfile Is Nothing Then
            WriteError(New ErrorRecord(New ItemNotFoundException(String.Format("No UserProfile was found for '{0}'.", UserProfileId.ToString)), Nothing, ErrorCategory.ObjectNotFound, UserProfileId))
        Else
            With MyInvocation.BoundParameters
                If .ContainsKey("UserName") Then nUserProfile.Name = UserName
                If .ContainsKey("Email") Then nUserProfile.Email = Email
                If .ContainsKey("FirstName") Then nUserProfile.FirstName = FirstName
                If .ContainsKey("LastName") Then nUserProfile.LastName = LastName
                'nUserProfile.cul
            End With
        End If
    End Sub
End Class
