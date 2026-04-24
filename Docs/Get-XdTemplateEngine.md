---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Get-XdTemplateEngine

## SYNOPSIS
Gets Template Engine information.

## SYNTAX

```
Get-XdTemplateEngine [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Gets Template Engine information and dependencies.
Returns whether the custome engine is enabled, the version and the dll.
For each dependency, returns name, version and dll.

On all objects you can use '.SaveToDisk(<filepath>)' to save the DLL to disk.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-XdTemplateEngine
```

IsCustomEngine CustomEngineVersion CustomEngineContent Dependencies
-------------- ------------------- ------------------- ------------
          True 3.11.4.0            {77, 90, 144, 0...} {Custom.TemplateDesigner.dll}

## PARAMETERS

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
