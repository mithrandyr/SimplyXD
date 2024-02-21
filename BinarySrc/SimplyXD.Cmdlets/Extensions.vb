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
    Public Function AsPSObject(x As IBaseEntityType)


        Return x
    End Function
End Module
