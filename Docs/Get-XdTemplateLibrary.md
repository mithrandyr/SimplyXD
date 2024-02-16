---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Get-XdTemplateLibrary

## SYNOPSIS
Returns templatelibraries

## SYNTAX

### search
```
Get-XdTemplateLibrary [[-Search] <String>] [-IncludeCount] [-TimeOut <Int32>] [<CommonParameters>]
```

### name
```
Get-XdTemplateLibrary -Name <String> [-IncludeCount] [-TimeOut <Int32>] [<CommonParameters>]
```

### id
```
Get-XdTemplateLibrary -TemplateLibraryId <Guid> [-IncludeCount] [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Returns templatelibraries

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -IncludeCount
Results Will include a 'GroupCount' property reports the count of TemplateGroups in the TemplateLibrary.

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
Name of the templatelibrary to return

```yaml
Type: String
Parameter Sets: name
Aliases: TemplateLibrary

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Search
Templatelibraries whose name contain this value will be returned

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

### -TemplateLibraryId
GUID of the templatelibrary to return

```yaml
Type: Guid
Parameter Sets: id
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
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
