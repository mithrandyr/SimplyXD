---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Get-XdBatchGroup

## SYNOPSIS
Gets a batchgroup

## SYNTAX

### one
```
Get-XdBatchGroup -Name <String> [-IncludeCount] [-TimeOut <Int32>] [<CommonParameters>]
```

### search
```
Get-XdBatchGroup [-Search <String>] [-IncludeCount] [-TimeOut <Int32>] [<CommonParameters>]
```

### id
```
Get-XdBatchGroup -BatchGroupId <Guid> [-IncludeCount] [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Gets a batchgroup

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -BatchGroupId
GUID of the batchgroup to return

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

### -IncludeCount
Results Will include a 'BatchCount' property reports the count of Batches in the BatchGroup.

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

### -Name
Name of the batchgroup to return

```yaml
Type: String
Parameter Sets: one
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Search
returns batchgroups containing this value

```yaml
Type: String
Parameter Sets: search
Aliases:

Required: False
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
