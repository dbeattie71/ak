Option Explicit On 

Namespace AKInstanceBot

    Public Class Interaction

        Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
        Private Declare Function GetForegroundWindow Lib "user32" Alias "GetForegroundWindow" () As Integer
        Declare Function ShowWindow Lib "user32" (ByVal hWnd As System.IntPtr, ByVal nCmdShow As Integer) As Boolean
        Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As System.IntPtr) As Integer

        Private Const SW_SHOW As Integer = 5
        Private Const SW_RESTORE As Integer = 9

        Private Shared Sub Activate(ByVal MainWindowHandle As System.IntPtr)

            ShowWindow(MainWindowHandle, SW_RESTORE)
            SetForegroundWindow(MainWindowHandle)

        End Sub

        Public Sub Appactivate(ByVal ProcessId As Integer)

            Dim processToActivate As Process
            Dim mainWindowHandle As System.IntPtr

            processToActivate = Process.GetProcessById(ProcessId)
            If Not processToActivate Is Nothing Then
                mainWindowHandle = processToActivate.MainWindowHandle()
                Interaction.Activate(mainWindowHandle)
            End If

        End Sub

    End Class

End Namespace

