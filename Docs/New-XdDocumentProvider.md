---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# New-XdDocumentProvider

## SYNOPSIS
Creates a document provider

## SYNTAX

### long
```
New-XdDocumentProvider -DocumentId <String> -XmlData <String> -TemplateLibraryName <String>
 -TemplateGroupName <String> -TemplateName <String> [-DopaName <String>] [-TimeOut <Int32>]
 [<CommonParameters>]
```

### short
```
New-XdDocumentProvider -DocumentId <String> -ContractName <String> -InputMetaData <String> [-TimeOut <Int32>]
 [<CommonParameters>]
```

## DESCRIPTION
Creates a document provider

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -ContractName
ContractName that determines which provider to execute - should not be specified

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
GUID of the document to associate this document provider with

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DopaName
Name of the DOPA that should be used

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
MetaData structure to be used with the Contract - should not be specified

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

### -TemplateName
Name of the template to execute

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
XML data to be used in the execution of the template

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

### -TemplateGroupName
Name of the TemplateGroup to find the template to execute

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

### -TemplateLibraryName
Name of the TemplateLibrary to find the TemplateGroup

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

### System.Int32

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
