<Cmdlet(VerbsCommon.Set, "XdUserProfile")>
<CmdletBinding(DefaultParameterSetName:="id")>
Public Class Set_XdUserProfile
    Inherits baseCmdlet

#Region "PowerShell Properties"
    <Parameter(Mandatory:=True, ParameterSetName:="id", ValueFromPipelineByPropertyName:=True)>
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

    <Parameter>
    Property PassThru As SwitchParameter
#End Region

    Protected Overrides Sub ProcessRecord()
        Try
            xdp.MergeOption = Microsoft.OData.Client.MergeOption.OverwriteChanges

            Dim nUserProfile As UserProfile = ExecuteWithTimeout(xdp.UserProfiles.Where(Function(x) x.UserProfileId = UserProfileId).FirstOrDefaultAsync)
            If nUserProfile Is Nothing Then
                WriteErrorMissing("UserProfile", UserProfileId.ToString)
            Else
                With MyInvocation.BoundParameters
                    If .ContainsKey("UserName") Then nUserProfile.Name = UserName
                    If .ContainsKey("Email") Then nUserProfile.Email = Email
                    If .ContainsKey("FirstName") Then nUserProfile.FirstName = FirstName
                    If .ContainsKey("LastName") Then nUserProfile.LastName = LastName
                End With

                xdp.UpdateObject(nUserProfile)
                If SaveChanges(nUserProfile) AndAlso PassThru.IsPresent Then WriteObject(nUserProfile)
                xdp.Detach(nUserProfile)
            End If
        Finally
            xdp.MergeOption = Microsoft.OData.Client.MergeOption.NoTracking
        End Try
    End Sub
End Class
