---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Remove-XdTemplateLibrary

## SYNOPSIS
Removes the TemplateLibrary

## SYNTAX

### name
```
Remove-XdTemplateLibrary -Name <String> [-Force] [-TimeOut <Int32>] [<CommonParameters>]
```

### id
```
Remove-XdTemplateLibrary -TemplateLibraryId <Guid> [-Force] [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Removes the TemplateLibrary

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -Force
Removes TemplateLibrary even if it contains TemplateGroups

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
Name of the TemplateLibrary to remove

```yaml
Type: String
Parameter Sets: name
Aliases: TemplateLibrary

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateLibraryId
GUID of the TemplateLibrary to remove

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
