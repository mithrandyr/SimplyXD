---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Measure-XDPerformance

## SYNOPSIS
Measures the throughput of Xpertdoc Portal Document Services for a given template/data.

## SYNTAX

```
Measure-XDPerformance -TemplateLibraryName <String> -TemplateGroupName <String> -TemplateName <String>
 -BatchGroupName <Object> -XmlData <String[]> [-NumThreads <Int32>] [-DocsPerThread <Int32>] [-ConvertToPDF]
 [-KeepErrors] [-TimeOut <Int32>] [<CommonParameters>]
```

## DESCRIPTION
Measures the throughput of Xpertdoc Portal Document Services for a given template/data.
Enables specifying the concurrency, the documents per thread, an array of XML (randomly
chosen for each execution).

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -BatchGroupName
Name of the BatchGroup where the batches will be created/executed.

```yaml
Type: Object
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ConvertToPDF
Will add a documentOperation to the batches to convert the result to PDF.

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

### -DocsPerThread
The number of batches/documents to be created/executed per concurrent thread.

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

### -KeepErrors
If set, errored batches will not be deleted.

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

### -NumThreads
The number of concurrent threads making requests against the Xpertdoc Portal.

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

### -TemplateGroupName
Name of the TemplateGroup where the Template is stored.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateLibraryName
Name of the TemplateLibrary where the TemplateGroup exists.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateName
Name of the template to be used.

```yaml
Type: String
Parameter Sets: (All)
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

### -XmlData
The XML that will be used as the data source for the document.

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String[]

### System.Int32

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
