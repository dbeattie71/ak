Option Explicit On 

Public Class Base

    Public Shared Event OnLog(ByVal LogString As String)

    Private _RequestedLogLevel As LogLevel

    Protected Sub SetLogLevel(ByVal LogLevel As LogLevel)

        _RequestedLogLevel = LogLevel

    End Sub

    Protected Sub Log(ByVal Message As String)

        'Private Sub Log(ByVal Message As String, _
        '            ByVal MessageLevel As LogLevel, _
        '            Optional ByVal ObjectName As String = "", _
        '            Optional ByVal ClassName As String = "", _
        '            Optional ByVal MethodName As String = "")

        Dim enumMessageLevel As LogLevel
        Dim tempMessage As String

        'If MessageLevel <= _RequestedLogLevel Then
        'Dim enumEvenLogEntryType As EventLogEntryType

        'If MessageLevel > LogLevel.WarningMinor Then
        '    enumEvenLogEntryType = EventLogEntryType.Information
        'Else
        '    If MessageLevel > LogLevel.ErrorNormal Then
        '        enumEvenLogEntryType = EventLogEntryType.Warning
        '    Else
        '        enumEvenLogEntryType = EventLogEntryType.Error
        '    End If
        'End If

        'RaiseError scOBJNAME & "." & scCLASSNAME, scMETHOD, lngErrorNumber, strErrorDesc, True
        'tempMessage = ObjectName & "." & ClassName & "." & MethodName & ": " & Message
        RaiseEvent OnLog(Message)

        'End If
    End Sub

    Protected Enum LogLevel

        ErrorFatal = 1     'Errors that prevent all processing
        ErrorNormal = 2    'Errors that prevent some processing
        WarningMajor = 3   'WarningMajor
        WarningMinor = 4   'WarningMinor
        InfoLevel1 = 5     'Start, Stop, Pause, etc...
        InfoLevel2 = 6     'Configuration information, Processing
        InfoLevel3 = 7     'Order level info
        InfoLevel4 = 8     'POA / ASN documents created
        InfoLevel5 = 9     'Line item detail
        InfoLevel6 = 10    'Additional debug info	

    End Enum

End Class
