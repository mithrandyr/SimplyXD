---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Add-XdDocumentOperation

## SYNOPSIS
Adds Operation to the specified Document

## SYNTAX

### short
```
Add-XdDocumentOperation -DocumentId <Guid> -ContractName <String> -InputMetaData <String> [-TimeOut <Int32>]
 [<CommonParameters>]
```

### asposepdf
```
Add-XdDocumentOperation -DocumentId <Guid> [-AsposePDF] [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Adds Operation to the specified Document

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -AsposePDF
Uses default configuration for Aspose PDF Conversion

```yaml
Type: SwitchParameter
Parameter Sets: asposepdf
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ContractName
The ContractName for the Document Operation

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
The GUID of the Document to add the Operation to

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

### -InputMetaData
The required Input MetaData for the Operation

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

### System.Guid

### System.Int32

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
