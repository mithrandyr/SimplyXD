---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Get-XdBatchError

## SYNOPSIS
Gets Batches that have errored, along with error details.

## SYNTAX

### search
```
Get-XdBatchError [-ErrorText <String>] [-BatchGroupId <Guid>] [-Limit <Int32>] [-TimeOut <Int32>]
 [<CommonParameters>]
```

### batch
```
Get-XdBatchError [-BatchId] <Guid> [-Limit <Int32>] [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Gets Batches that have errored, along with error details.  Searchable by error, returns ExecutionData.

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -BatchGroupId
GUID of the batchgroup to limit results from.

```yaml
Type: Guid
Parameter Sets: search
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -BatchId
GUID of the batch to limit results from.

```yaml
Type: Guid
Parameter Sets: batch
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -ErrorText
Only return errors that contain this value

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

### -Limit
Maximum number of Errored batches to return

```yaml
Type: Int32
Parameter Sets: (All)
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

### System.Guid

### System.Int32

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
