---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Add-XdGroupMember

## SYNOPSIS
Synopsis test

## SYNTAX

### name-name
```
Add-XdGroupMember -GroupName <String> -UserName <String> [-TimeOut <Int32>] [<CommonParameters>]
```

### name-id
```
Add-XdGroupMember -GroupName <String> -UserProfileId <Guid> [-TimeOut <Int32>] [<CommonParameters>]
```

### id-name
```
Add-XdGroupMember -GroupId <Guid> -UserName <String> [-TimeOut <Int32>] [<CommonParameters>]
```

### id-id
```
Add-XdGroupMember -GroupId <Guid> -UserProfileId <Guid> [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
{{ Description Test }}

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -GroupId
{{ Fill GroupId Description }}

```yaml
Type: Guid
Parameter Sets: id-name, id-id
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -GroupName
{{ Fill GroupName Description }}

```yaml
Type: String
Parameter Sets: name-name, name-id
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -TimeOut
{{ Fill TimeOut Description }}

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

### -UserName
{{ Fill UserName Description }}

```yaml
Type: String
Parameter Sets: name-name, id-name
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -UserProfileId
{{ Fill UserProfileId Description }}

```yaml
Type: Guid
Parameter Sets: name-id, id-id
Aliases:

Required: True
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
