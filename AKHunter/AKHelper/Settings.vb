Option Explicit On 

Friend Class Settings
    Inherits Base

    Private _Settings As Hashtable

    Public Sub New()

        _Settings = New Hashtable

    End Sub

    Public Sub Load(ByVal FileName As String)

        Dim xmlDoc As Xml.XmlDocument = New Xml.XmlDocument
        Dim xmlNode As Xml.XmlNode

        Dim logLine As LogLine

        Try
            xmlDoc.Load(FileName)

            Log("---- Loading Settings data  ----")

            For Each xmlNode In xmlDoc.SelectNodes("//Profile/Settings/Setting")
                _Settings.Add(xmlNode.SelectSingleNode("@Name").Value, xmlNode.SelectSingleNode("@Value").Value)
                Log("Name: " & xmlNode.SelectSingleNode("@Name").Value & "  Value: " & xmlNode.SelectSingleNode("@Value").Value)
            Next

            xmlDoc = Nothing

        Catch ex As Exception
            Log(ex.Message)
        End Try
    End Sub

    Public Sub AddSetting(ByVal SettingName As String, _
                          ByVal SettingValue As String)

        _Settings.Add(SettingName, SettingValue)

    End Sub

    Public Function GetSetting(ByVal SettingName As String) As String

        Return _Settings(SettingName)

    End Function

End Class
