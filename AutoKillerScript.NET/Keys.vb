Option Strict Off
Option Explicit On 
Imports System.IO
Imports System.Threading
Imports System.Runtime.InteropServices

Public Enum KeyDirection
    KeyDown
    KeyUp
    KeyUpDown
End Enum

Public Module InputConstants
    Public Const INPUT_MOUSE As Byte = 0
    Public Const INPUT_KEYBOARD As Byte = 1
    Public Const INPUT_HARDWARE As Byte = 2

    Public Const SHIFT_key As Byte = 1
    Public Const CTRL_key As Byte = 2
    Public Const ALT_key As Byte = 4
    Public Const KEYEVENTF_EXTENDEDKEY As Byte = &H1
    Public Const KEYEVENTF_KEYUP As Byte = &H2
    Public Const KEYEVENTF_SCANCODE As Byte = &H8

    Public Declare Sub keybd_event Lib "user32.dll" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As Integer)
    Public Declare Function VkKeyScan Lib "user32" Alias "VkKeyScanA" (ByVal aChar As Byte) As Short
    Public Declare Function CharToOem Lib "user32" Alias "CharToOemA" (ByVal lpszSrc As String, ByVal lpszDst As String) As Integer
    Public Declare Function OemKeyScan Lib "user32" (ByVal wOemChar As Integer) As Integer
    Public Declare Function MapVirtualKey Lib "user32" Alias "MapVirtualKeyA" (ByVal wCode As Integer, ByVal wMapType As Integer) As Integer
    Public Declare Function SendInput Lib "user32.dll" (ByVal nInputs As Integer, ByVal pInput As Input, ByVal cbSize As Integer) As Integer
    Public Declare Function GetLastError Lib "kernel32" Alias "GetLastError" () As Integer
    Public Declare Function OemKeyScan Lib "user32" (ByVal wOemChar As Short) As Integer
    Public Declare Sub keybd_event Lib "user32" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Long, ByVal dwExtraInfo As Long)
    Public Declare Function GetKeyboardState Lib "user32.dll" (ByVal kbArray() As Byte) As Int32
    Public Declare Function ToAscii Lib "user32.dll" (ByVal uVirtKey As Int32, ByVal uScanCode As Int32, ByVal lpbKeyState() As Byte, ByRef lpwTransKey As Short, ByVal fuState As Int32) As Int32
End Module

<StructLayout(LayoutKind.Explicit, pack:=2)> _
Public Class Input
    <FieldOffset(0)> Public dwType As Integer
    ' Mouse
    <FieldOffset(4)> Public mouse_dx As Integer
    <FieldOffset(8)> Public mouse_dy As Integer
    <FieldOffset(12)> Public mouse_Data As Integer
    <FieldOffset(16)> Public mouse_dwFlags As Integer
    <FieldOffset(20)> Public mouse_time As Integer
    <FieldOffset(24)> Public mouse_dwExtraInfo As Integer
    ' Keyboard
    <FieldOffset(4)> Public keyb_wVk As Short
    <FieldOffset(6)> Public keyb_wScan As Short
    <FieldOffset(8)> Public keyb_dwFlags As Integer
    <FieldOffset(12)> Public keyb_time As Integer
    <FieldOffset(16)> Public keyb_dwExtraInfo As Integer
    ' Hardware
    <FieldOffset(4)> Public hw_uMsg As Integer
    <FieldOffset(8)> Public hw_wParamL As Short
    <FieldOffset(10)> Public hw_wParamH As Short
End Class

Public NotInheritable Class cKey
    Public Key As Byte
    Public ScanCode As Byte
    Public Shift As Byte
    Public Alt As Byte
    Public Ctrl As Byte

    Public Overrides Function ToString() As String
        Return "Key: " & Key & vbCrLf & "ScanCode: " & ScanCode & vbCrLf & "ALT: " & Alt & vbCrLf & "SHIFT: " & Shift & vbCrLf & "CTRL: " & Ctrl
    End Function
End Class

Public NotInheritable Class Keyboard
    Private Shared KeyboardMutex As Mutex = New Mutex
    Private Sub New()

    End Sub
    Private Shared Function BuildInputArray(ByVal aKey As cKey, ByVal Direction As KeyDirection) As Input()
        Dim theInput() As Input = {}
        'Dim aKeys() As cKey = StringToKeys(s)
        Dim j As Integer

        If Direction = KeyDirection.KeyDown OrElse Direction = KeyDirection.KeyUpDown Then
            If aKey.Shift = 1 Then
                ReDim Preserve theInput(j)
                theInput(j) = New Input
                With theInput(j)
                    .dwType = INPUT_KEYBOARD
                    .keyb_dwFlags = KEYEVENTF_EXTENDEDKEY
                    .keyb_wVk = Keys.ShiftKey
                    .keyb_wScan = MapVirtualKey(Keys.ShiftKey, 0)
                    .keyb_time = 0
                    .keyb_dwExtraInfo = 0
                End With
                j += 1
            End If

            ReDim Preserve theInput(j)
            theInput(j) = New Input
            With theInput(j)
                .dwType = INPUT_KEYBOARD
                Dim nScan As Integer = MapVirtualKey(aKey.Key, 2)
                If nScan = 0 Then
                    .keyb_dwFlags = KEYEVENTF_EXTENDEDKEY
                Else
                    .keyb_dwFlags = 0
                End If
                .keyb_wVk = aKey.Key
                .keyb_wScan = MapVirtualKey(aKey.Key, 0)
                .keyb_time = 0
                .keyb_dwExtraInfo = 0
            End With
            j += 1
        End If

        If Direction = KeyDirection.KeyUp OrElse Direction = KeyDirection.KeyUpDown Then
            ReDim Preserve theInput(j)
            theInput(j) = New Input
            With theInput(j)
                .dwType = INPUT_KEYBOARD
                Dim nScan As Integer = MapVirtualKey(aKey.Key, 2)
                If nScan = 0 Then
                    .keyb_dwFlags = KEYEVENTF_KEYUP Or KEYEVENTF_EXTENDEDKEY
                Else
                    .keyb_dwFlags = KEYEVENTF_KEYUP
                End If
                .keyb_wVk = aKey.Key
                .keyb_wScan = MapVirtualKey(aKey.Key, 0)
                .keyb_time = 0
                .keyb_dwExtraInfo = 0
            End With
            j += 1
            If aKey.Shift = 1 Then
                ReDim Preserve theInput(j)
                theInput(j) = New Input
                With theInput(j)
                    .dwType = INPUT_KEYBOARD
                    .keyb_dwFlags = KEYEVENTF_KEYUP Or KEYEVENTF_EXTENDEDKEY
                    .keyb_wVk = Keys.ShiftKey
                    .keyb_wScan = MapVirtualKey(Keys.ShiftKey, 0)
                    .keyb_time = 0
                    .keyb_dwExtraInfo = 0
                End With
            End If
        End If

        Return theInput
    End Function

    Public Shared Function TranslateKey(ByVal vkCode As Integer) As String
        Dim keyboardState(255) As Byte
        Dim temp As Integer

        GetKeyboardState(keyboardState)
        If ToAscii(vkCode, 0, keyboardState, temp, 0) > 0 Then
            Return Chr(temp)
        End If

        Return String.Empty

    End Function

    Private Shared Function BuildInputArray(ByVal aKeys() As cKey) As Input()
        Dim theInput() As Input = {}
        'Dim aKeys() As cKey = StringToKeys(s)

        Dim i As Integer
        Dim j As Integer = 0
        For i = aKeys.GetLowerBound(0) To aKeys.GetUpperBound(0)
            If aKeys(i).Shift = 1 Then
                ReDim Preserve theInput(j)
                theInput(j) = New Input
                With theInput(j)
                    .dwType = INPUT_KEYBOARD
                    .keyb_dwFlags = KEYEVENTF_EXTENDEDKEY
                    .keyb_wVk = Keys.ShiftKey
                    .keyb_wScan = MapVirtualKey(Keys.ShiftKey, 0)
                    .keyb_time = 0
                    .keyb_dwExtraInfo = 0
                End With
                j += 1
            End If

            ReDim Preserve theInput(j)
            theInput(j) = New Input
            With theInput(j)
                .dwType = INPUT_KEYBOARD
                Dim nScan As Integer = MapVirtualKey(aKeys(i).Key, 2)
                If nScan = 0 Then
                    .keyb_dwFlags = KEYEVENTF_EXTENDEDKEY
                Else
                    .keyb_dwFlags = 0
                End If
                .keyb_wVk = aKeys(i).Key
                .keyb_wScan = MapVirtualKey(aKeys(i).Key, 0)
                .keyb_time = 0
                .keyb_dwExtraInfo = 0
            End With
            j += 1

            If aKeys(i).Shift = 1 Then
                ReDim Preserve theInput(j)
                theInput(j) = New Input
                With theInput(j)
                    .dwType = INPUT_KEYBOARD
                    .keyb_dwFlags = KEYEVENTF_KEYUP Or KEYEVENTF_EXTENDEDKEY
                    .keyb_wVk = Keys.ShiftKey
                    .keyb_wScan = MapVirtualKey(Keys.ShiftKey, 0)
                    .keyb_time = 0
                    .keyb_dwExtraInfo = 0
                End With
                j += 1
            End If

            ReDim Preserve theInput(j)
            theInput(j) = New Input
            With theInput(j)
                .dwType = INPUT_KEYBOARD
                Dim nScan As Integer = MapVirtualKey(aKeys(i).Key, 2)
                If nScan = 0 Then
                    .keyb_dwFlags = KEYEVENTF_KEYUP Or KEYEVENTF_EXTENDEDKEY
                Else
                    .keyb_dwFlags = KEYEVENTF_KEYUP
                End If
                .keyb_wVk = aKeys(i).Key
                .keyb_wScan = MapVirtualKey(aKeys(i).Key, 0)
                .keyb_time = 0
                .keyb_dwExtraInfo = 0
            End With
            j += 1
        Next

        Return theInput
    End Function

    Private Shared Function BuildInputArray(ByVal s As String) As Input()
        Dim theInput() As Input = {}
        Dim aKeys() As cKey = StringToKeys(s)

        Dim i As Integer
        Dim j As Integer = 0
        For i = aKeys.GetLowerBound(0) To aKeys.GetUpperBound(0)
            If aKeys(i).Shift = 1 Then
                ReDim Preserve theInput(j)
                theInput(j) = New Input
                With theInput(j)
                    .dwType = INPUT_KEYBOARD
                    .keyb_dwFlags = KEYEVENTF_EXTENDEDKEY
                    .keyb_wVk = Keys.ShiftKey
                    .keyb_wScan = MapVirtualKey(Keys.ShiftKey, 0)
                    .keyb_time = 0
                    .keyb_dwExtraInfo = 0
                End With
                j += 1
            End If

            ReDim Preserve theInput(j)
            theInput(j) = New Input
            With theInput(j)
                .dwType = INPUT_KEYBOARD
                .keyb_dwFlags = 0
                .keyb_wVk = aKeys(i).Key
                .keyb_wScan = aKeys(i).ScanCode
                .keyb_time = 0
                .keyb_dwExtraInfo = 0
            End With
            j += 1

            If aKeys(i).Shift = 1 Then
                ReDim Preserve theInput(j)
                theInput(j) = New Input
                With theInput(j)
                    .dwType = INPUT_KEYBOARD
                    .keyb_dwFlags = KEYEVENTF_KEYUP Or KEYEVENTF_EXTENDEDKEY
                    .keyb_wVk = Keys.ShiftKey
                    .keyb_wScan = MapVirtualKey(Keys.ShiftKey, 0)
                    .keyb_time = 0
                    .keyb_dwExtraInfo = 0
                End With
                j += 1
            End If

            ReDim Preserve theInput(j)
            theInput(j) = New Input
            With theInput(j)
                .dwType = INPUT_KEYBOARD
                .keyb_dwFlags = KEYEVENTF_KEYUP
                .keyb_wVk = aKeys(i).Key
                .keyb_wScan = aKeys(i).ScanCode
                .keyb_time = 0
                .keyb_dwExtraInfo = 0
            End With
            j += 1
        Next

        Return theInput
    End Function

    Public Shared Function InputsToIntPtr(ByVal aInputs() As Input) As IntPtr
        Try
            Dim InputCount As Integer = aInputs.Length
            Dim aPtr As IntPtr = Marshal.AllocHGlobal(InputCount * Marshal.SizeOf(aInputs(0)))
            Dim i As Integer
            For i = 0 To aInputs.Length - 1
                Dim thePtr As IntPtr = New IntPtr(aPtr.ToInt32 + i * Marshal.SizeOf(aInputs(0)))
                Marshal.StructureToPtr(aInputs(i), thePtr, False)
            Next
            Return aPtr
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Sub SendKey(ByVal cKey As Char, ByVal direction As KeyDirection)
        Dim aKey As cKey = GetVkScan(cKey)
        SendKey(aKey, direction)
    End Sub

    Public Shared Sub SendKey(ByVal aKey As cKey, ByVal Direction As KeyDirection)
        Try
            KeyboardMutex.WaitOne()
            Dim theInput() As Input = BuildInputArray(aKey, Direction)
            For i As Integer = theInput.GetLowerBound(0) To theInput.GetUpperBound(0)
                If theInput(i).keyb_wVk = Keys.ShiftKey AndAlso i <> theInput.GetLowerBound(0) Then
                    Thread.Sleep(150)
                End If
                SendInput(1, theInput(i), Marshal.SizeOf(theInput(0)))
                If theInput(i).keyb_wVk = Keys.ShiftKey AndAlso i <> theInput.GetUpperBound(0) Then
                    Thread.Sleep(150)
                End If
            Next
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        Finally
            'Thread.Sleep(100)
            KeyboardMutex.ReleaseMutex()
        End Try
    End Sub

    Public Shared Sub SendKeys(ByVal aKeys() As cKey)
        Try
            KeyboardMutex.WaitOne()
            Dim theInput() As Input = BuildInputArray(aKeys)
            For i As Integer = theInput.GetLowerBound(0) To theInput.GetUpperBound(0)
                If theInput(i).keyb_wVk = Keys.ShiftKey AndAlso i <> theInput.GetLowerBound(0) Then
                    Thread.Sleep(150)
                End If
                SendInput(1, theInput(i), Marshal.SizeOf(theInput(0)))
                If theInput(i).keyb_wVk = Keys.ShiftKey AndAlso i <> theInput.GetUpperBound(0) Then
                    Thread.Sleep(150)
                End If
            Next
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        Finally
            'Thread.Sleep(100)
            KeyboardMutex.ReleaseMutex()
        End Try
    End Sub

    Public Shared Sub SendString(ByVal s As String)
        Try
            KeyboardMutex.WaitOne()
            Dim theInput() As Input = BuildInputArray(s)
            For i As Integer = theInput.GetLowerBound(0) To theInput.GetUpperBound(0)
                If theInput(i).keyb_wVk = Keys.ShiftKey AndAlso i <> theInput.GetLowerBound(0) Then
                    Thread.Sleep(150)
                End If
                SendInput(1, theInput(i), Marshal.SizeOf(theInput(0)))
                If theInput(i).keyb_wVk = Keys.ShiftKey AndAlso i <> theInput.GetUpperBound(0) Then
                    Thread.Sleep(150)
                End If
            Next
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        Finally
            Thread.Sleep(100)
            KeyboardMutex.ReleaseMutex()
        End Try
    End Sub

    Public Shared Function StringToKeys(ByVal s As String) As cKey()
        Dim keys() As cKey
        ReDim keys(s.Length - 1)
        Dim i As Integer
        For i = 0 To s.Length - 1
            keys(i) = GetVkScan(s.Chars(i))
        Next
        Return keys
    End Function

    Public Shared Function GetVkScan(ByVal c As Char) As cKey
        Dim aKey As New cKey
        Dim sKey As Short
        If c = "~"c Then
            c = Chr(Keys.Return)
        End If
        sKey = VkKeyScan(Asc(c))
        Dim aState As Integer
        aState = (sKey >> 8)
        Dim oemchar As String = c
        CharToOem(c, oemchar)
        aKey.ScanCode = CByte(OemKeyScan(Asc(oemchar)) And &HFF)
        If (aState And SHIFT_key) = SHIFT_key Then : aKey.Shift = 1 : Else : aKey.Shift = 0 : End If
        If (aState And CTRL_key) = CTRL_key Then : aKey.Ctrl = 1 : Else : aKey.Ctrl = 0 : End If
        If (aState And ALT_key) = ALT_key Then : aKey.Alt = 1 : Else : aKey.Alt = 0 : End If
        aKey.Key = CByte(sKey And &HFF)
        Debug.WriteLine("GetVkScan: " & aKey.ToString)
        Return aKey
    End Function

End Class

Public NotInheritable Class UserKeys
    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
    Private Declare Function MapVirtualKey Lib "user32" Alias "MapVirtualKeyA" (ByVal wCode As Integer, ByVal wMapType As Integer) As Integer
    Private _Keys As Collections.Hashtable
    Private _path As String
    Private AK As AutoKillerScript.clsAutoKillerScript
    Private Enum DAOCKeys As Byte
        NearestLoot = 72
        Use2ndItem = 71
        RangedWeapon = 70
        TwoHandedWeapon = 69
        RightHandWeapon = 68
        CommandWindow = 67
        Sprint = 66
        Craft_Salvage = 65
        Face = 64
        Stick = 63
        Follow = 62
        PanCamera = 61
        Info_Delve = 60
        Compass = 59
        PerfMeter = 58
        Sit_Stand = 57
        SysPageDown = 56
        SysPageUp = 55
        ChatPageDown = 54
        ChatPageUp = 53
        TargetGroup8 = 52
        TargetGroup7 = 51
        TargetGroup6 = 50
        TargetGroup5 = 49
        TargetGroup4 = 48
        TargetGroup3 = 47
        TargetGroup2 = 46
        TargetGroup1 = 45
        GroundTarget = 44
        Consider = 43
        RunLock = 42
        ShowCombat = 41
        ShowSkills = 40
        MouseLook = 39
        Destroy = 38
        ChatToggle = 37
        Reply = 36
        LastAttacker = 35
        UseItem = 34
        MoveBackward1 = 33
        ShowGroup = 32
        ShowInventory = 31
        ShowStats = 30
        Sell = 29
        ShowSpells = 28
        AttackMode = 27
        NearestObject = 26
        NearestFriend = 25
        NearestEnemy = 24

        Torch = 22
        GetItem = 21
        MoveForward1 = 20
        ToggleNames = 19
        MouseLookToggle = 18

        ResetCamera = 14
        LookDown = 13
        Lookup = 12
        Select_Open = 11
        Help = 10
        StrafeRight = 9
        StrafeLeft = 8
        Walk = 7
        StrafeToggle = 6
        Down = 5
        Up = 4
        TurnRight = 3
        TurnLeft = 2
        MoveBackward2 = 1
        MoveForward2 = 0
    End Enum
    Private Class KeyPair
        Public [Shift] As Boolean
        Public Key As Byte
        Public Sub New(ByVal aKey As Byte, ByVal aShift As Boolean)
            Key = aKey
            [Shift] = aShift
        End Sub
    End Class
    Public Sub New(ByVal DLL As AutoKillerScript.clsAutoKillerScript)
        MyBase.new()
        AK = DLL
        _path = AK.GamePath
        LoadKeys()
    End Sub
    Public Sub LoadKeys()
        Dim i As Byte

        _Keys = New Hashtable(79)

        For i = 0 To 79
            Dim Key As Byte = FindVirtualKey(CByte(GetKey(i)))
            Dim [Shift] As Boolean = CBool(GetShift(i))
            _Keys.Add(CType(i, DAOCKeys), New KeyPair(Key, [Shift]))
        Next
    End Sub
    Private Sub SendKey(ByVal k As KeyPair, Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        Dim aKey As cKey = New cKey
        aKey.Key = k.Key
        If k.Shift Then
            aKey.Shift = 1
        Else
            aKey.Shift = 0
        End If
        Keyboard.SendKey(aKey, bHold)
    End Sub
    Function GetKey(ByVal Key As Short) As String
        Dim sBuffer As String, lRet As Long
        ' Fill String with 255 spaces
        sBuffer = Strings.StrDup(255, Chr(0))
        Dim sKey As String
        If Key < 10 Then
            sKey = "key0" & CStr(Key)
        Else
            sKey = "key" & CStr(Key)
        End If

        With New CIniFile
            .Path = _path & "\user.dat"
            Dim s As String = .Value("keyboard", sKey, "")
            If Not s = "" Then
                lRet = GetPrivateProfileString("keyboard", sKey, "0", sBuffer, sBuffer.Length, _path & "\user.dat")
            Else
                lRet = GetPrivateProfileString("keyboard", sKey, "0", sBuffer, sBuffer.Length, _path & "\keyboard.dat")
            End If
        End With

        If lRet = 0 Then
            Return "0"
        Else
            Return sBuffer.Substring(0, sBuffer.IndexOf(Chr(0)))
        End If

    End Function
    Function GetShift(ByVal Key As Short) As String
        Dim sBuffer As String, lRet As Long
        ' Fill String with 255 spaces
        sBuffer = Strings.StrDup(255, Chr(0))
        Dim sKey As String
        If Key < 10 Then
            sKey = "shift0" & CStr(Key)
        Else
            sKey = "shift" & CStr(Key)
        End If

        With New CIniFile
            .Path = _path & "\user.dat"
            Dim s As String = .Value("keyboard", sKey, "")
            If Not s = "" Then
                lRet = GetPrivateProfileString("keyboard", sKey, "0", sBuffer, sBuffer.Length, _path & "\user.dat")
            Else
                lRet = GetPrivateProfileString("keyboard", sKey, "0", sBuffer, sBuffer.Length, _path & "\keyboard.dat")
            End If
        End With

        If lRet = 0 Then
            Return "0"
        Else
            Return sBuffer.Substring(0, sBuffer.IndexOf(Chr(0)))
        End If
    End Function
    Public Function FindVirtualKey(ByVal aKey As Byte) As Integer
        Dim i As Integer
        For i = 0 To 256
            If MapVirtualKey(i, 0) = aKey Then
                Return i
            End If
        Next
    End Function
    Protected Overrides Sub Finalize()
        _Keys.Clear()
        _Keys = Nothing
        MyBase.Finalize()
    End Sub
    Public Sub AttackMode(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.AttackMode), KeyPair), bHold)
    End Sub
    Public ReadOnly Property AttackMode_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.AttackMode), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property AttackModeKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.AttackMode), KeyPair).Key
        End Get
    End Property
    Public Sub Consider(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.Consider), KeyPair), bHold)
    End Sub
    Public ReadOnly Property Consider_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.Consider), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property ConsiderKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.Consider), KeyPair).Key
        End Get
    End Property
    Public Sub Face(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.Face), KeyPair), bHold)
    End Sub
    Public ReadOnly Property Face_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.Face), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property FaceKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.Face), KeyPair).Key
        End Get
    End Property
    Public Sub GetItem(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.GetItem), KeyPair), bHold)
    End Sub
    Public ReadOnly Property GetItem_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.GetItem), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property GetItemKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.GetItem), KeyPair).Key
        End Get
    End Property
    Public Sub MoveBackward(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.MoveBackward2), KeyPair), bHold)
    End Sub
    Public ReadOnly Property MoveBackward_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.MoveBackward2), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property MoveBackwardKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.MoveBackward2), KeyPair).Key
        End Get
    End Property
    Public Sub MoveForward(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.MoveForward2), KeyPair), bHold)
    End Sub
    Public ReadOnly Property MoveForward_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.MoveForward2), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property MoveForwardKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.MoveForward2), KeyPair).Key
        End Get
    End Property
    Public Sub Sell(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.Sell), KeyPair), bHold)
    End Sub
    Public ReadOnly Property Sell_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.Sell), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property SellKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.Sell), KeyPair).Key
        End Get
    End Property
    Public Sub SitStand(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.Sit_Stand), KeyPair), bHold)
    End Sub
    Public ReadOnly Property SitStand_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.Sit_Stand), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property SitStandKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.Sit_Stand), KeyPair).Key
        End Get
    End Property
    Public Sub Sprint(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.Sprint), KeyPair), bHold)
    End Sub
    Public ReadOnly Property Sprint_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.Sprint), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property SprintKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.Sprint), KeyPair).Key
        End Get
    End Property
    Public Sub Stick(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.Stick), KeyPair), bHold)
    End Sub
    Public ReadOnly Property Stick_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.Stick), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property StickKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.Stick), KeyPair).Key
        End Get
    End Property
    Public Sub StrafeLeft(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.StrafeLeft), KeyPair), bHold)
    End Sub
    Public ReadOnly Property StrafeLeft_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.StrafeLeft), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property StrafeLeftKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.StrafeLeft), KeyPair).Key
        End Get
    End Property
    Public Sub StrafeRight(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.StrafeRight), KeyPair), bHold)
    End Sub
    Public ReadOnly Property StrafeRight_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.StrafeRight), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property StrafeRightKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.StrafeRight), KeyPair).Key
        End Get
    End Property
    Public Sub TurnLeft(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.TurnLeft), KeyPair), bHold)
    End Sub
    Public ReadOnly Property TurnLeft_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.TurnLeft), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property TurnLeftKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.TurnLeft), KeyPair).Key
        End Get
    End Property
    Public Sub TurnRight(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.TurnRight), KeyPair), bHold)
    End Sub
    Public ReadOnly Property TurnRight_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.TurnRight), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property TurnRightKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.TurnRight), KeyPair).Key
        End Get
    End Property
    Public Sub UseItem(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.UseItem), KeyPair), bHold)
    End Sub
    Public ReadOnly Property UseItem_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.UseItem), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property UseItemKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.UseItem), KeyPair).Key
        End Get
    End Property
    Public Sub Use2ndItem(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.Use2ndItem), KeyPair), bHold)
    End Sub
    Public ReadOnly Property Use2ndItem_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.Use2ndItem), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property Use2ndItemKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.Use2ndItem), KeyPair).Key
        End Get
    End Property
    Public Sub NearestLoot(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.NearestLoot), KeyPair), bHold)
    End Sub
    Public ReadOnly Property NearestLoot_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.NearestLoot), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property NearestLootKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.NearestLoot), KeyPair).Key
        End Get
    End Property
    Public Sub RightHandWeapon(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.RightHandWeapon), KeyPair), bHold)
    End Sub
    Public ReadOnly Property RightHandWeapon_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.TwoHandedWeapon), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property RightHandWeaponKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.RightHandWeapon), KeyPair).Key
        End Get
    End Property
    Public Sub TwoHandedWeapon(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.TwoHandedWeapon), KeyPair), bHold)
    End Sub
    Public ReadOnly Property TwoHandedWeapon_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.TwoHandedWeapon), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property TwoHandedWeaponKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.TwoHandedWeapon), KeyPair).Key
        End Get
    End Property
    Public Sub RangedWeapon(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.RangedWeapon), KeyPair), bHold)
    End Sub
    Public ReadOnly Property RangedWeapon_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.RangedWeapon), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property RangedWeaponKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.RangedWeapon), KeyPair).Key
        End Get
    End Property
    Public Sub Up(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.Up), KeyPair), bHold)
    End Sub
    Public ReadOnly Property Up_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.Up), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property UpKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.Up), KeyPair).Key
        End Get
    End Property
    Public Sub Down(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.Down), KeyPair), bHold)
    End Sub
    Public ReadOnly Property Down_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.Down), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property DownKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.Down), KeyPair).Key
        End Get
    End Property
    Public Sub CraftSalvage(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.Craft_Salvage), KeyPair), bHold)
    End Sub
    Public ReadOnly Property Craft_Salvage_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.Craft_Salvage), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property Craft_Salvage() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.Craft_Salvage), KeyPair).Key
        End Get
    End Property
    Public Sub Walk(Optional ByVal bHold As KeyDirection = KeyDirection.KeyUpDown)
        SendKey(DirectCast(_Keys(DAOCKeys.Walk), KeyPair), bHold)
    End Sub
    Public ReadOnly Property Walk_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.Walk), KeyPair).Shift
        End Get
    End Property
    Public ReadOnly Property WalkKey() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.Walk), KeyPair).Key
        End Get
    End Property
End Class
