---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# New-XdTemplateGroup

## SYNOPSIS
Creates a template group

## SYNTAX

### name
```
New-XdTemplateGroup -TemplateLibrary <String> -Name <String> [-Description <String>] [-TimeOut <Int32>]
 [<CommonParameters>]
```

### id
```
New-XdTemplateGroup -TemplateLibraryId <Guid> [-Description <String>] [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Creates a template group

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -Name
Name of the template group, must be unique within a library

```yaml
Type: String
Parameter Sets: name
Aliases: TemplateGroup

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateLibrary
Name of the TemplateLibrary in which to create the templateGroup

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

### -TemplateLibraryId
GUID of the TemplateLibrary in which to create the TemplateGroup

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

### -Description
Description for the TemplateGroup

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
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
