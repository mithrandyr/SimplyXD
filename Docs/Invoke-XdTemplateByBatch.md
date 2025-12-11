---
external help file: SimplyXD.dll-Help.xml
Module Name: SimplyXD
online version:
schema: 2.0.0
---

# Invoke-XdTemplateByBatch

## SYNOPSIS
Executes a Template by using Document Services.

## SYNTAX

```
Invoke-XdTemplateByBatch -TemplateLibraryName <String> -TemplateGroupName <String> -TemplateName <String>
 -BatchGroupName <String> -XmlData <String> [-ConvertToPDF] [-ReturnDocument] [-NoDelete] [-TimeOut <Int32>]
 [<CommonParameters>]
```

## DESCRIPTION
Executes a Template by using Document Services. This involves creating a batch,
attaching a document, configuring a document provider that includes the references
to the template and data.  Executes this batch, waits for it to complete, and
then deletes the batch.  Will return error if batch fails.  Can return document if
-ReturnDocument switch is used.

## EXAMPLES

### Example 1
```powershell
PS C:\> {{ Add example code here }}
```

Invoke-XdTemplateByBatch -TemplateLibraryName "library" -TemplateGroupName "group" -TemplateName "test" -BatchGroupName "batchgroup" -xmlData "someXML"

## PARAMETERS

### -BatchGroupName
Name of the BatchGroup where the batch will be created.

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

### -ConvertToPDF
Will add a documentOperation to the batch to convert the result to PDF.

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

### -NoDelete
Skips deleting the Batch after it completes execution.

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

### -ReturnDocument
Returns the byte array of the generated document

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
Type: String
Parameter Sets: (All)
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

### System.Int32

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
