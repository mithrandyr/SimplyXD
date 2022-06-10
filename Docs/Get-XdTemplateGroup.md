---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Get-XdTemplateGroup

## SYNOPSIS
Return TemplateGroups

## SYNTAX

### search
```
Get-XdTemplateGroup [[-Search] <String>] [-TemplateLibrary <String>] [-TimeOut <Int32>] [<CommonParameters>]
```

### name
```
Get-XdTemplateGroup -TemplateLibrary <String> -Name <String> [-TimeOut <Int32>] [<CommonParameters>]
```

### id2
```
Get-XdTemplateGroup -TemplateLibraryId <Guid> [-TimeOut <Int32>] [<CommonParameters>]
```

### id
```
Get-XdTemplateGroup -TemplateGroupId <Guid> [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Return TemplateGroups

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -Search
Return templategroups with name containing this value

```yaml
Type: String
Parameter Sets: search
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateGroupId
GUID of the templategroup to return

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

### -TemplateLibrary
Name of the templatelibrary, whose templategroups will be returned

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

```yaml
Type: String
Parameter Sets: name
Aliases:

Required: True
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

### -Name
Name of the templategroup to return

```yaml
Type: String
Parameter Sets: name
Aliases: TemplateGroup

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateLibraryId
GUID of the templatelibrary whose groups will be returned

```yaml
Type: Guid
Parameter Sets: id2
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
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
