---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Get-XdDOPA

## SYNOPSIS
Get DOPAs (Document Output Post Actions) from Xpertdoc Portal.

## SYNTAX

### name
```
Get-XdDOPA [[-Search] <String>] [-TemplateLibrary <String>] [-Exact] [-TimeOut <Int32>] [<CommonParameters>]
```

### id
```
Get-XdDOPA [[-Search] <String>] -TemplateLibraryId <Guid> [-Exact] [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Get DOPAs (Document Output Post Actions) from Xpertdoc Portal.  Returns which Template
Library they are configured for and parses the InputMetaData to retrieve which Document
Operations are part of the DOPA.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-XdDopa
```

Returns list of all DOPAs in the Xpertdoc Portal

## PARAMETERS

### -Exact
Returns only those DOPAs that match the -Search parameter exactly.

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

### -Search
Return DOPAs with Name containing this value OR are exactly (if -Exact) this value.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateLibrary
Name of the Template Library to return DOPAs from.

```yaml
Type: String
Parameter Sets: name
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -TemplateLibraryId
GUID of the Template Library to return DOPAs from.

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

### System.String

### System.Guid

### System.Int32

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
