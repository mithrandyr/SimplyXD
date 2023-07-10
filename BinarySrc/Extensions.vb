Imports System.Runtime.CompilerServices
Imports Microsoft.OData.Edm.Vocabularies

Module Extensions

    <Extension()>
    Public Function AsPSObject(tg As TemplateGroup) As PSObject
        Dim x = PSObject.AsPSObject(tg)
        x.Properties.Add(New PSScriptProperty("TemplateLibraryName", ScriptBlock.Create("if($this.TemplateLibrary) { $this.TemplateLibrary.Name }")))
        Return x
    End Function

    <Extension()>
    Public Function AsPSObject(tg As Template) As PSObject
        Dim x = PSObject.AsPSObject(tg)
        x.Properties.Add(New PSScriptProperty("TemplateGroupName", ScriptBlock.Create("if($this.TemplateGroup) { $this.TemplateGroup.Name }")))

        x.Properties.Add(New PSScriptProperty("TemplateLibraryName", ScriptBlock.Create("if($this.TemplateGroup -and $this.TemplateGroup.TemplateLibrary) { $this.TemplateGroup.TemplateLibrary.Name }")))
        Return x
    End Function


#Region "WriteError Extension Methods"
    <Extension()>
    Sub WriteErrorMissing(this As baseCmdlet, itemType As String, itemValue As String, Optional innerEx As Exception = Nothing)
        Dim errorId = String.Format("XDPortal-{0}ItemNotFound", itemType)
        Dim ex As New XDPItemMissingException(itemType, itemValue, StandardErrors.GetInnermostException(innerEx))
        this.WriteError(New ErrorRecord(ex, errorId, ErrorCategory.ObjectNotFound, itemValue))
    End Sub

    <Extension()>
    Sub WriteErrorNotEmpty(this As baseCmdlet)

    End Sub
#End Region

End Module
