---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Remove-XdTemplate

## SYNOPSIS
Removes the template.

## SYNTAX

### obj
```
Remove-XdTemplate -InputObject <Template> [-TimeOut <Int32>] [<CommonParameters>]
```

### id
```
Remove-XdTemplate -Id <Guid> [-TimeOut <Int32>] [<CommonParameters>]
```

### name
```
Remove-XdTemplate -TemplateLibrary <String> -TemplateGroup <String> -Name <String> [-TimeOut <Int32>]
 [<CommonParameters>]
```

## DESCRIPTION
Removes the template from the Xpertdoc Portal.

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -Id
GUID of the template

```yaml
Type: Guid
Parameter Sets: id
Aliases: TemplateId

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -InputObject
Template to remove

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
Name of the Template

```yaml
Type: String
Parameter Sets: name
Aliases: TemplateName

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateGroup
TemplateGroup in which to find the Template

```yaml
Type: String
Parameter Sets: name
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateLibrary
Library in which to find the TemplateGroup

```yaml
Type: String
Parameter Sets: name
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### Xpertdoc.Portal.SdkCore.Template

### System.Int32

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
