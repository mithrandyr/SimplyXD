Imports System.Management.Automation.Language
Imports Microsoft.OData.UriParser

<Cmdlet(VerbsCommon.Get, "XdDOPA")>
<CmdletBinding(DefaultParameterSetName:="name")>
Public Class Get_XdDOPA
    Inherits baseCmdlet

    <Parameter(Position:=0)>
    Public Property Search As String

    <Parameter(ParameterSetName:="name", ValueFromPipeline:=True)>
    Public Property TemplateLibrary As String

    <Parameter(Mandatory:=True, ParameterSetName:="id", ValueFromPipelineByPropertyName:=True)>
    Public Property TemplateLibraryId As Guid

    <Parameter()>
    Public Property Exact As SwitchParameter

    Protected Overrides Sub ProcessRecord()
        Dim query = XDP.DocumentOutputPostActions.Expand(Function(x) x.TemplateLibrary).AsQueryable

        If ParameterSetName = "id" Then
            query = query.Where(Function(x) x.TemplateLibraryId = TemplateLibraryId)
        ElseIf Not String.IsNullOrWhiteSpace(TemplateLibrary) Then
            query = query.Where(Function(x) x.TemplateLibrary.Name.Equals(TemplateLibrary))
        End If

        If Not String.IsNullOrWhiteSpace(Search) Then
            If Exact Then
                query = query.Where(Function(x) x.Name.Equals(Search))
            Else
                query = query.Where(Function(x) x.Name.Contains(Search))
            End If
        End If

        For Each tl In GenerateResults(query, "DocumentOuputPostAction")
            ParseAndWriteObject(tl)
        Next
    End Sub

    Private Sub ParseAndWriteObject(item As DocumentOutputPostAction)
        If String.IsNullOrWhiteSpace(item.Metadata) Then
            WriteObject(item)
            Exit Sub
        End If
        Try
            Dim metaData = XDocument.Parse(item.Metadata)
            Dim listOfOperations = metaData.
                Descendants("DocumentOperation").
                Select(Function(d) New DopaOperation With {
                       .ContractName = d.Attribute("ContractName").Value,
                       .InputMetaData = XDocument.Parse(d.Value)
                       }).ToList

            WriteObject(item.AsPSObject.AppendProperty("Operations", listOfOperations))
        Catch ex As Exception
            WriteWarning($"Cannot parse 'MetaData' for {item.TemplateLibrary.Name}\{item.Name}")
            WriteObject(item)
            Exit Sub
        End Try
    End Sub

    Private Class DopaOperation
        Public ContractName As String
        Public InputMetaData As XDocument
    End Class
End Class
