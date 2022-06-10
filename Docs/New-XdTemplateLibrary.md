---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# New-XdTemplateLibrary

## SYNOPSIS
Creates a template library

## SYNTAX

```
New-XdTemplateLibrary [-Name] <String> [-Description <String>] [-DisableTemplateVersioning] [-TimeOut <Int32>]
 [<CommonParameters>]
```

## DESCRIPTION
Creates a template library

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -Description
Description for the TemplateLibrary

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DisableTemplateVersioning
Specify if you want to disable template versioning in this library - should not be used

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
Name of the TemplateLibrary to create

```yaml
Type: String
Parameter Sets: (All)
Aliases: TemplateLibrary

Required: True
Position: 0
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
