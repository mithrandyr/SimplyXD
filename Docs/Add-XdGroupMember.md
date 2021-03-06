---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Add-XdGroupMember

## SYNOPSIS
Adds UserProfile to Group

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
Adds UserProfile to Group

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-XdGroupMember -GroupName Test -UserName SomeUser
```

Adding 'SomeUser' to the group 'Test'

## PARAMETERS

### -GroupId
GUID for the Group

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
Name of the Group

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
Actions against the Xpertdoc Portal will timeout in this many seconds.

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 15
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -UserName
Name of the UserProfile to add to the group

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
GUID of the UserProfile to add to the Group

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
