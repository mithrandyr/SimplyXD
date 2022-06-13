---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Export-XdTemplate

## SYNOPSIS
Exports a template into a zipfile

## SYNTAX

### name
```
Export-XdTemplate -TemplateLibrary <String> -TemplateGroup <String> -Name <String> [-ExportPath <String>]
 [-Version <String>] [-Force] [-TimeOut <Int32>] [<CommonParameters>]
```

### id
```
Export-XdTemplate -Id <Guid> [-ExportPath <String>] [-Version <String>] [-Force] [-TimeOut <Int32>]
 [<CommonParameters>]
```

### obj
```
Export-XdTemplate -InputObject <Template> [-ExportPath <String>] [-Version <String>] [-Force]
 [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Exports a template into a zipfile

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -ExportPath
Path to either a zipfile to be created or path to a folder where the zipfile will be created

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Force
If specified, will overwrite an existing archive.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Id
GUID of the Template

```yaml
Type: Guid
Parameter Sets: id
Aliases: TemplateId

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -InputObject
A TemplateGroup, all templates for this group will be returned.

```yaml
Type: Template
Parameter Sets: obj
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -Name
Template name to be exported

```yaml
Type: String
Parameter Sets: name
Aliases: TemplateName

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -TemplateGroup
TemplateGroup to find the template

```yaml
Type: String
Parameter Sets: name
Aliases: TemplateGroupName

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -TemplateLibrary
TemplateLibray to find the TemplateGroup

```yaml
Type: String
Parameter Sets: name
Aliases: TemplateLibraryName

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -TimeOut
Actions against the Xpertdoc Portal will timeout in this many seconds.

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Version
Specifies a version string to be stored inside the zipfile

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

### System.Int32

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
