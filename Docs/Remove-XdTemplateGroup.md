---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Remove-XdTemplateGroup

## SYNOPSIS
Removes a TemplateGroup

## SYNTAX

### name
```
Remove-XdTemplateGroup -TemplateLibrary <String> [-Name] <String> [-Force] [-TimeOut <Int32>]
 [<CommonParameters>]
```

### id
```
Remove-XdTemplateGroup -TemplateGroupId <Guid> [-Force] [-TimeOut <Int32>] [<CommonParameters>]
```

### obj
```
Remove-XdTemplateGroup -InputObject <TemplateGroup> [-Force] [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Removes a TemplateGroup.  Will remove all templates in the group as well.

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -Force
Removes templateGroup even if it contains templates

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

### -InputObject
The TemplateGroup to remove

```yaml
Type: TemplateGroup
Parameter Sets: obj
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Name of the TemplateGroup to remove

```yaml
Type: String
Parameter Sets: name
Aliases: TemplateGroup

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateGroupId
GUID of the TemplateGroup to remove

```yaml
Type: Guid
Parameter Sets: id
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateLibrary
Name of the TemplateLibrary in which to find the TemplateGroup

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

### System.Int32

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
