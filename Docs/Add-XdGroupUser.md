---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Add-XdGroupUser

## SYNOPSIS
Adds UserProfile to Group

## SYNTAX

### name-name
```
Add-XdGroupUser -GroupName <String> -UserName <String> [-TimeOut <Int32>] [<CommonParameters>]
```

### name-id
```
Add-XdGroupUser -GroupName <String> -UserProfileId <Guid> [-TimeOut <Int32>] [<CommonParameters>]
```

### id-name
```
Add-XdGroupUser -GroupId <Guid> -UserName <String> [-TimeOut <Int32>] [<CommonParameters>]
```

### id-id
```
Add-XdGroupUser -GroupId <Guid> -UserProfileId <Guid> [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Adds UserProfile to Group

## EXAMPLES

### Example 1
```powershell
PS C:\> Add-XdGroupUser -GroupName Test -UserName SomeUser
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
Default value: None
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
