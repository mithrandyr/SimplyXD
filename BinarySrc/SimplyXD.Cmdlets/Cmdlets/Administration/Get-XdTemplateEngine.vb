Imports System.IO
Imports System.Net.Http
Imports System.Reflection
Imports System.Text.Json

<Cmdlet(VerbsCommon.Get, "XdTemplateEngine")>
<CmdletBinding()>
Public Class Get_XdTemplateEngine
    Inherits baseCmdlet

    Protected Overrides Sub EndProcessing()
        Dim url = XDP.Configurations.AppendRequestUri("GetDesigner12TemplateEngineConfig")

        Using handler As New HttpClientHandler With {.PreAuthenticate = True}
            If XDP.Credentials Is Nothing Then
                handler.UseDefaultCredentials = True
            Else
                handler.Credentials = XDP.Credentials
            End If
            Using httpClient As New HttpClient(handler)
                Using request As New HttpRequestMessage(HttpMethod.Get, url)
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json")
                    Using response = ExecuteWithTimeout(httpClient.GetAsync(url))
                        response.EnsureSuccessStatusCode()
                        Using doc = JsonDocument.Parse(response.Content.ReadAsStringAsync.Result)
                            Dim dependencies As JsonElement, deps As XdEngineDependency()
                            Dim root = doc.RootElement
                            If root.TryGetProperty("Dependencies", dependencies) Then
                                deps = dependencies.
                                    EnumerateArray().
                                    Select(Function(e)
                                               Dim n = e.GetProperty("Name").GetString
                                               Dim c = e.GetProperty("Content").GetBytesFromBase64()
                                               Return New XdEngineDependency(n, GetVersionFromDll(c), c)
                                           End Function).ToArray
                            End If
                            Dim re = root.GetProperty("Enabled").GetBoolean
                            Dim rc = root.GetProperty("RuntimeAssemblyContent").GetBytesFromBase64
                            WriteObject(New XdEngineConfig(re, GetVersionFromDll(rc), rc, deps))
                        End Using
                    End Using
                End Using
            End Using
        End Using
    End Sub

    Private Function GetVersionFromDll(dll As Byte()) As Version
        Dim dllAssembly = Assembly.Load(dll)
        ' Try to get the FileVersion attribute first
        Dim fileVersionAttr = dllAssembly.GetCustomAttribute(Of AssemblyFileVersionAttribute)()
        If fileVersionAttr IsNot Nothing Then
            Return Version.Parse(fileVersionAttr.Version)
        End If
        ' Fall back to assembly version if FileVersion attribute not found
        Dim assemblyVersion = dllAssembly.GetName().Version
        If assemblyVersion IsNot Nothing Then
            Return assemblyVersion
        End If
        Return Nothing
    End Function

    Private Class XdEngineConfig
        Public ReadOnly Property IsCustomEngine As Boolean
        Public ReadOnly Property CustomEngineVersion As Version
        Public ReadOnly Property CustomEngineContent As Byte()
        Public ReadOnly Property Dependencies As New List(Of XdEngineDependency)

        Sub New(isCustom As Boolean, customVersion As Version, customContent As Byte(), deps As XdEngineDependency())
            IsCustomEngine = isCustom
            CustomEngineVersion = customVersion
            CustomEngineContent = customContent
            If deps IsNot Nothing And deps.Length > 0 Then
                Dependencies.AddRange(deps)
            End If
        End Sub

        Sub SaveToDisk(filePath As String)
            If CustomEngineContent Is Nothing OrElse CustomEngineContent.Length = 0 Then
                Throw New InvalidOperationException("There is no Custom Content Engine to save to disk!")
            End If
            If File.Exists(filePath) Then
                Throw New Exception($"File 'filepath' already exists, will not overwrite!")
            End If
            File.WriteAllBytes(filePath, CustomEngineContent)
        End Sub
    End Class

    Private Class XdEngineDependency
        Public ReadOnly Property Name As String
        Public ReadOnly Property Version As Version
        Public ReadOnly Property Content As Byte()
        Sub New(n As String, v As Version, c As Byte())
            Name = n
            Version = v
            Content = c
        End Sub

        Sub SaveToDisk(filePath As String)
            If Content Is Nothing OrElse Content.Length = 0 Then
                Throw New InvalidOperationException("There is no Content to save to disk!")
            End If
            If File.Exists(filePath) Then
                Throw New Exception($"File 'filepath' already exists, will not overwrite!")
            End If
            File.WriteAllBytes(filePath, Content)
        End Sub
    End Class
End Class