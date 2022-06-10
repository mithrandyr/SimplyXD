---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Get-XdTemplate

## SYNOPSIS
Returns templates

## SYNTAX

### search
```
Get-XdTemplate [-Search <String>] [-TemplateLibrary <String>] [-TemplateGroup <String>] [-IncludeContent]
 [-TimeOut <Int32>] [<CommonParameters>]
```

### name
```
Get-XdTemplate -TemplateLibrary <String> -TemplateGroup <String> -Name <String> [-IncludeContent]
 [-TimeOut <Int32>] [<CommonParameters>]
```

### id
```
Get-XdTemplate -TemplateId <Guid> [-IncludeContent] [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Returns templates

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -IncludeContent
If specified, will return the WordDocument and DLL of the template

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

### -Search
Returns templates with the templatename containing this value

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

### -TemplateGroup
Name of the templategroup to return templates from

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

### -TemplateId
GUID of the template to return

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

### -TemplateLibrary
Name of the templatelibrary to return templates from

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
Name of the template to return

```yaml
Type: String
Parameter Sets: name
Aliases: TemplateName

Required: True
Position: Named
Default value: None
Accept pipeline input: False
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
