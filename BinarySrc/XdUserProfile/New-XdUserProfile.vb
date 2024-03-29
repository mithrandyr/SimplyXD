﻿<Cmdlet(VerbsCommon.[New], "XdUserProfile")>
<CmdletBinding()>
Public Class New_XdUserProfile
    Inherits baseCmdlet

#Region "PowerShell Properties"
    <ValidateNotNullOrEmpty>
    <[Alias]("UserName")>
    <Parameter(Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
    Property Name As String

    <ValidateNotNullOrEmpty>
    <Parameter(Mandatory:=True, ValueFromPipelineByPropertyName:=True)>
    Property Email As String

    <Parameter(ValueFromPipelineByPropertyName:=True)>
    Property FirstName As String

    <Parameter(ValueFromPipelineByPropertyName:=True)>
    Property LastName As String
#End Region

    Protected Overrides Sub ProcessRecord()
        Dim nUserProfile As New UserProfile With {.Name = Name, .Email = Email}

        If Not String.IsNullOrWhiteSpace(FirstName) Then nUserProfile.FirstName = FirstName
        If Not String.IsNullOrWhiteSpace(LastName) Then nUserProfile.FirstName = LastName

        xdp.AddToUserProfiles(nUserProfile)
        If SaveChanges(nUserProfile) Then WriteObject(nUserProfile)
    End Sub
End Class
