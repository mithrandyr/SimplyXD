Module Extensions

    <Runtime.CompilerServices.Extension()>
    Public Function AsPSObject(tg As TemplateGroup) As PSObject
        Dim x = PSObject.AsPSObject(tg)
        x.Properties.Add(New PSScriptProperty("TemplateLibraryName", ScriptBlock.Create("if($this.TemplateLibrary) { $this.TemplateLibrary.Name }")))
        Return x
    End Function

    <Runtime.CompilerServices.Extension()>
    Public Function AsPSObject(tg As Template) As PSObject
        Dim x = PSObject.AsPSObject(tg)
        x.Properties.Add(New PSScriptProperty("TemplateGroupName", ScriptBlock.Create("if($this.TemplateGroup) { $this.TemplateGroup.Name }")))

        x.Properties.Add(New PSScriptProperty("TemplateLibraryName", ScriptBlock.Create("if($this.TemplateGroup -and $this.TemplateGroup.TemplateLibrary) { $this.TemplateGroup.TemplateLibrary.Name }")))
        Return x
    End Function
End Module
