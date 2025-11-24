---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Get-XdBatchCount

## SYNOPSIS
Gets count of batches (by status)

## SYNTAX

```
Get-XdBatchCount [-BatchGroupId <Guid>] [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Gets count of batches (by status).  Can be filtered by BatchGroupId.

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

Get-XdBatchCount

## PARAMETERS

### -BatchGroupId
Limit counts to this BatchGroupId.

```yaml
Type: Guid
Parameter Sets: (All)
Aliases:

Required: False
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

### System.Guid

### System.Int32

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
