---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# New-XdTemplate

## SYNOPSIS
Creates a template

## SYNTAX

### name
```
New-XdTemplate -TemplateLibrary <String> -TemplateGroup <String> -Name <String> [-Description <String>]
 -Source <Byte[]> -Assembly <Byte[]> [-Comment <String>] [-TimeOut <Int32>] [<CommonParameters>]
```

### id
```
New-XdTemplate -TemplateGroupId <Guid> -Name <String> [-Description <String>] -Source <Byte[]>
 -Assembly <Byte[]> [-Comment <String>] [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Creates a template

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -Assembly
binary content representing the template's DLL

```yaml
Type: Byte[]
Parameter Sets: (All)
Aliases: Dll

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Comment
Comment to be added to the TemplateVersionHistory

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

### -Description
Description for the Template

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

### -Name
Name of the template (must be unique within a TemplateLibrary\TemplateGroup)

```yaml
Type: String
Parameter Sets: (All)
Aliases: TemplateName

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Source
binary content representing the template's word document

```yaml
Type: Byte[]
Parameter Sets: (All)
Aliases: WordDocument

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateGroup
Name of the templateGroup in which to create the template

```yaml
Type: String
Parameter Sets: name
Aliases: TemplateGroupName

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateGroupId
GUID of the templategroup in which to create the template

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
Aliases: TemplateLibraryName

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
