Option Explicit On 

Friend Class Movement

    Public Sub AutoFollow(ByRef Daoc As AutoKillerScript.clsAutoKillerScript, _
                          ByVal Name As String, _
                          ByVal StickKey As Byte)

        Daoc.SetTarget(Name, True)
        System.Threading.Thread.CurrentThread.Sleep(500)

        Daoc.SendKeys(StickKey, 0)
        System.Threading.Thread.CurrentThread.Sleep(500)

    End Sub

    Public Sub AutoFollow(ByRef Daoc As AutoKillerScript.clsAutoKillerScript, _
                          ByVal Name As String)

        Daoc.SetTarget(Name, True)
        System.Threading.Thread.CurrentThread.Sleep(500)

        Daoc.SendString("/stick~")
        System.Threading.Thread.CurrentThread.Sleep(500)

    End Sub

    Public Sub BreakAutoFollow(ByVal Daoc As AutoKillerScript.clsAutoKillerScript, _
                               ByVal MoveBackwardKey As Byte)

        Daoc.SendKeys(MoveBackwardKey, True)
        System.Threading.Thread.CurrentThread.Sleep(250)
        Daoc.SendKeys(MoveBackwardKey, False, True)

    End Sub

    Public Sub Stand(ByVal Daoc As AutoKillerScript.clsAutoKillerScript)

        If Daoc.isPlayerSitting() Then
            System.Threading.Thread.CurrentThread.Sleep(100)
            Daoc.SendString("/stand~")
            System.Threading.Thread.CurrentThread.Sleep(500)
        End If

    End Sub

    Public Sub Sit(ByVal Daoc As AutoKillerScript.clsAutoKillerScript)

        If Not Daoc.isPlayerSitting() Then
            System.Threading.Thread.CurrentThread.Sleep(100)
            Daoc.SendString("/sit~")
            System.Threading.Thread.CurrentThread.Sleep(500)
        End If

    End Sub

End Class
