---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Get-XdGroupMember

## SYNOPSIS
Get Members of a group

## SYNTAX

### name
```
Get-XdGroupMember [-GroupName] <String> [-TimeOut <Int32>] [<CommonParameters>]
```

### id
```
Get-XdGroupMember -GroupId <Guid> [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Get Members of a group

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -GroupId
GUID of the group to return members from

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

### -GroupName
Name of the group to return

```yaml
Type: String
Parameter Sets: name
Aliases:

Required: True
Position: 0
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
