<Cmdlet(VerbsCommon.Get, "XdUserProfile")>
<CmdletBinding(DefaultParameterSetName:="search")>
Public Class Get_XdUserProfile
    Inherits baseCmdlet

    <Parameter(ParameterSetName:="search")>
    Public Property Search As String

    <Parameter(Mandatory:=True, ParameterSetName:="username", ValueFromPipelineByPropertyName:=True)>
    Public Property UserName As String

    <Parameter(Mandatory:=True, ParameterSetName:="id", ValueFromPipelineByPropertyName:=True)>
    Public Property UserProfileId As Guid

    Protected Overrides Sub EndProcessing()
        Dim query = xdp.UserProfiles.AsQueryable

        If ParameterSetName = "search" Then
            If Not String.IsNullOrWhiteSpace(Search) Then
                query = query.Where(Function(x) x.FirstName.Contains(Search) Or x.LastName.Contains(Search) Or x.Email.Contains(Search) Or x.Name.Contains(Search))
            End If
            For Each gr In GenerateResults(query, "UserProfile")
                WriteObject(gr)
            Next
        Else
            Dim up As UserProfile
            If ParameterSetName = "username" Then
                up = ExecuteWithTimeout(query.Where(Function(x) x.Name.ToUpper.Equals(UserName.ToUpper)).FirstOrDefaultAsync)
            Else
                Try
                    up = ExecuteWithTimeout(query.Where(Function(x) x.UserProfileId = UserProfileId).FirstOrDefaultAsync)
                Catch
                    'TODO: add error handling.  When selecting using a GUID, if the guid doesn't exist, a NotFound error gets thrown.  Currently we are swallowing this error and returning nothing.
                End Try
            End If

            If up IsNot Nothing Then WriteObject(up)
        End If
    End Sub
End Class
