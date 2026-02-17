---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Get-XdTemplateVersion

## SYNOPSIS
Get Template Versions

## SYNTAX

### search
```
Get-XdTemplateVersion [-Search] <String> [-Limit <Int32>] [-AsCount] [-TimeOut <Int32>] [<CommonParameters>]
```

### obj
```
Get-XdTemplateVersion -TemplateId <Guid> [-Limit <Int32>] [-AsCount] [-TimeOut <Int32>] [<CommonParameters>]
```

### address
```
Get-XdTemplateVersion [-TemplateLibrary <String>] [-TemplateGroup <String>] [-TemplateName <String>]
 [-Limit <Int32>] [-AsCount] [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Get Template Versions

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -AsCount
Returns number of template versions.

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

### -Limit
Limits how many results are returned.

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Search
Returns templates with the templatename containing this value

```yaml
Type: String
Parameter Sets: search
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateGroup
Name of the templategroup to return template versions from

```yaml
Type: String
Parameter Sets: address
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateId
GUID of the Template that you want to return the versions for.

```yaml
Type: Guid
Parameter Sets: obj
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -TemplateLibrary
Name of the templatelibrary to return template versions from

```yaml
Type: String
Parameter Sets: address
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateName
Name of the Template to return versions from

```yaml
Type: String
Parameter Sets: address
Aliases:

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

### System.Guid

### System.Int32

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
