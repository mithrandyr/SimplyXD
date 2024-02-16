---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Get-XdBatch

## SYNOPSIS
Gets a Batch

## SYNTAX

### bg
```
Get-XdBatch -BatchGroupId <Guid> [-Limit <Int32>] [-SortByCreation <String>] [-Status <String>]
 [-TimeOut <Int32>] [<CommonParameters>]
```

### search
```
Get-XdBatch [-BatchGroupId <Guid>] [-Limit <Int32>] [-SortByCreation <String>] [-Status <String>]
 -Search <String> [-TimeOut <Int32>] [<CommonParameters>]
```

### batch
```
Get-XdBatch -BatchId <Guid> [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Gets a Batch

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -BatchGroupId
GUID of the BatchGroup to return batches from

```yaml
Type: Guid
Parameter Sets: bg
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

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
GUID of the batch to return

```yaml
Type: Guid
Parameter Sets: batch
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Limit
maximum number of batches to return

```yaml
Type: Int32
Parameter Sets: bg, search
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Search
Batches containing this will be returned

```yaml
Type: String
Parameter Sets: search
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SortByCreation
Sorts the batches by creation data

```yaml
Type: String
Parameter Sets: bg, search
Aliases:
Accepted values: Ascending, Descending

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Status
Batches with this status will be returned, if not specified all will be returned.

```yaml
Type: String
Parameter Sets: bg, search
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
