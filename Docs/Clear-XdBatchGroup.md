---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Clear-XdBatchGroup

## SYNOPSIS
Removes all batches from a batchgroup.

## SYNTAX

```
Clear-XdBatchGroup -BatchGroup <String> [-DeleteLimit <Int32>] [-SortByCreation <String>] [-Status <String>]
 [-MinimumAgeInMinutes <Int32>] [-Concurrency <Int32>] [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Removes all batches from a batchgroup.

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -BatchGroup
Name of the batchgroup

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

### -Concurrency
How many batches will be deleted concurrently

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

### -DeleteLimit
The total number of batches to delete. If 0, delete all batches.

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 0
Accept pipeline input: False
Accept wildcard characters: False
```

### -MinimumAgeInMinutes
Batches have to be this many minutes old to be deleted

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 15
Accept pipeline input: False
Accept wildcard characters: False
```

### -SortByCreation
Orders the batches by their creation time (Ascending or Descending); intended to be used with -DeleteLimit

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Ascending, Descending

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Status
Which status is to be deleted, if not specified, then all are deleted

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Completed, Created, Error, Queued, Running, TimedOut

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
