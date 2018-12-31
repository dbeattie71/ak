Option Explicit On 

Friend Class Misc

    Public Sub PassThru(ByVal Daoc As AutoKillerScript.clsAutoKillerScript, _
                        ByVal Message As String, _
                        ByVal Settings As Settings)

        Dim temp As String = ""

        'strip of quotes
        temp = Mid(Message, 2)
        'LogLineAsync(strTemp)

        temp = Microsoft.VisualBasic.Left(temp, Len(temp) - 1)
        'LogLineAsync(strTemp)

        'strip off flag
        temp = Mid(temp, Len(Settings.GetSetting("PassThruFlag")) + 1)
        'LogLineAsync(strTemp)

        'append ~
        temp = temp & "~"

        System.Threading.Thread.CurrentThread.Sleep(500)
        Daoc.SendString(temp)
        System.Threading.Thread.CurrentThread.Sleep(500)

    End Sub

    Public Sub PassThru(ByVal Daoc As AutoKillerScript.clsAutoKillerScript, _
                        ByVal Message As String, _
                        ByVal PassThruFlag As String)

        Dim temp As String = ""

        'strip of quotes
        temp = Mid(Message, 2)
        'LogLineAsync(strTemp)

        temp = Microsoft.VisualBasic.Left(temp, Len(temp) - 1)
        'LogLineAsync(strTemp)

        'strip off flag
        temp = Mid(temp, Len(PassThruFlag) + 1)
        'LogLineAsync(strTemp)

        'append ~
        temp = temp & "~"

        System.Threading.Thread.CurrentThread.Sleep(500)
        Daoc.SendString(temp)
        System.Threading.Thread.CurrentThread.Sleep(500)

    End Sub

    Public Sub AcceptDialog(ByVal Daoc As AutoKillerScript.clsAutoKillerScript)

        Dim dlg As AutoKillerScript.WindowManager = _
            New AutoKillerScript.WindowManager(Daoc, AutoKillerScript.WINDOW_NAMES.Dialog)

        Daoc.MouseMove(dlg.Left + 125, dlg.Top + 85)
        System.Threading.Thread.CurrentThread.Sleep(500)
        Daoc.LeftClick()

        dlg = Nothing
    End Sub

    Public Sub SetGroundTarget(ByVal Daoc As AutoKillerScript.clsAutoKillerScript, _
                               ByVal PlayerName As String)

        System.Threading.Thread.CurrentThread.Sleep(250)
        Daoc.SendString("/groundassist " & PlayerName & "~")
        System.Threading.Thread.CurrentThread.Sleep(250)

    End Sub

    Public Sub SetEffectsToNone(ByVal Daoc As AutoKillerScript.clsAutoKillerScript)

        System.Threading.Thread.CurrentThread.Sleep(250)
        Daoc.SendString("/effects none~")
        System.Threading.Thread.CurrentThread.Sleep(250)

    End Sub

    Public Sub Disband(ByVal Daoc As AutoKillerScript.clsAutoKillerScript)

        System.Threading.Thread.CurrentThread.Sleep(250)
        Daoc.SendString("/disband~")
        System.Threading.Thread.CurrentThread.Sleep(250)

    End Sub

End Class
