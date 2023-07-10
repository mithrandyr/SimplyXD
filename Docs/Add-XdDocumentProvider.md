---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Add-XdDocumentProvider

## SYNOPSIS
Adds a Document Provider to the specified Document

## SYNTAX

### long
```
Add-XdDocumentProvider -DocumentId <Guid> -XmlData <String> -TemplateLibrary <String> -TemplateGroup <String>
 -TemplateName <String> [-DopaName <String>] [-TimeOut <Int32>] [<CommonParameters>]
```

### short
```
Add-XdDocumentProvider -DocumentId <Guid> -ContractName <String> -InputMetaData <String> [-TimeOut <Int32>]
 [<CommonParameters>]
```

## DESCRIPTION
Adds a Document Provider to the specified Document

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -ContractName
The ContractName for the Document Provider

```yaml
Type: String
Parameter Sets: short
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DocumentId
The GUID of the Document to add the Provider to

```yaml
Type: Guid
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -DopaName
The name of the Document Output Post Action (DOPA) to be used.

```yaml
Type: String
Parameter Sets: long
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -InputMetaData
The required Input MetaData for the Provider

```yaml
Type: String
Parameter Sets: short
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateGroup
The name of the TemplateGroup where the Template is located

```yaml
Type: String
Parameter Sets: long
Aliases: TemplateGroupName

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateLibrary
The name of the TemplateLibrary where the TemplateGroup is located

```yaml
Type: String
Parameter Sets: long
Aliases: TemplateLibraryName

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateName
The Name of the template to execute

```yaml
Type: String
Parameter Sets: long
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

### -XmlData
The XML that will be used as the data source for the document.

```yaml
Type: String
Parameter Sets: long
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Guid

### System.Int32

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
