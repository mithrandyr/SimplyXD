---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Import-XdTemplate

## SYNOPSIS
Imports a template from a zipfile.

## SYNTAX

```
Import-XdTemplate -TemplateLibrary <String> -TemplateGroup <String> -Name <String> [-Comment <String>]
 [-ImportPath <String>] [-PassThru] [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Imports a template from a zipfile.  If template already exists, will updated the WordDocument and DLL.  If tempalte does not exists, will create it.  Use Export-XdTemplate to create the zipfile.

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -Comment
Comment to be added to the template version history

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -ImportPath
path to the zipfile to import

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Name
Name of the Template to update

```yaml
Type: String
Parameter Sets: (All)
Aliases: TemplateName

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -PassThru
If specified, will return the template data

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -TemplateGroup
Name of the TemplateGroup to find the Template

```yaml
Type: String
Parameter Sets: (All)
Aliases: TemplateGroupName

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -TemplateLibrary
Name of the TemplateLibrary to find the TemplateGroup

```yaml
Type: String
Parameter Sets: (All)
Aliases: TemplateLibraryName

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

### System.Management.Automation.SwitchParameter

### System.Int32

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
