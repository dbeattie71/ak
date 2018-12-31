Public Class mdlAKFarm

    Public Function ReadSetting(ByVal key As String, ByVal type As Type) As String
        Dim configurationAppSettings As System.Configuration.AppSettingsReader

        configurationAppSettings = New System.Configuration.AppSettingsReader

        Return configurationAppSettings.GetValue(key, type)

    End Function

End Class
