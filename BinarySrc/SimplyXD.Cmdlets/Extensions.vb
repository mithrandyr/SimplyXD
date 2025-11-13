Imports System.Runtime.CompilerServices
Imports Microsoft.OData.Client

Module Extensions

    <Extension()>
    Public Function AsPSObject(tg As Template) As PSObject
        Dim x = PSObject.AsPSObject(tg)
        x.Properties.Add(New PSScriptProperty("TemplateGroupName", ScriptBlock.Create("if($this.TemplateGroup) { $this.TemplateGroup.Name }")))

        x.Properties.Add(New PSScriptProperty("TemplateLibraryName", ScriptBlock.Create("if($this.TemplateGroup -and $this.TemplateGroup.TemplateLibrary) { $this.TemplateGroup.TemplateLibrary.Name }")))
        Return x
    End Function

    <Extension()>
    Public Function AsPSObject(x As Object) As PSObject
        If x Is GetType(PSObject) Then
            Return x
        Else
            Return PSObject.AsPSObject(x)
        End If
    End Function

    <Extension()>
    Public Function AppendProperty(obj As PSObject, name As String, value As Object) As PSObject
        obj.Properties.Add(New PSNoteProperty(name, value))
        Return obj
    End Function
End Module
