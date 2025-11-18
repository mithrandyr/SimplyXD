---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Set-XdTemplateContent

## SYNOPSIS
Updates the content (DLL & DOCX) of a Template

## SYNTAX

### name
```
Set-XdTemplateContent -TemplateLibrary <String> -TemplateGroup <String> -Name <String> -Source <Byte[]>
 -Assembly <Byte[]> [-Comment <String>] [-TimeOut <Int32>] [<CommonParameters>]
```

### id
```
Set-XdTemplateContent -TemplateId <Guid> -Source <Byte[]> -Assembly <Byte[]> [-Comment <String>]
 [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Updates the content (DLL & DOCX) of a Template

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -Assembly
Binary content representing the template's DLL

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

### -Name
Name of the Template whose content is being updated

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

### -Source
Binary content representing the template's word document

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
Name of the templateGroup in which to find the Template

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

### -TemplateId
GUID of the Template whose content is being updated

```yaml
Type: Guid
Parameter Sets: id
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -TemplateLibrary
TemplateLibrary in which to find the TemplateGroup

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
