Imports AutoKillerScript
Imports System.Threading

Public Class DAOCKeyboard

    Private Class KeyPair
        Public [Shift] As Boolean
        Public Key As Byte

        Public Sub New(ByVal aKey As Byte, ByVal aShift As Boolean)
            Key = aKey
            [Shift] = aShift
        End Sub
    End Class

    'Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Long, ByVal lpFileName As String) As Long
    Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Long
    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
    Private Declare Function MapVirtualKey Lib "user32" Alias "MapVirtualKeyA" (ByVal wCode As Integer, ByVal wMapType As Integer) As Integer

    Private _path As String
    Private _AK As clsAutoKillerScript
    Private _WaitTime As Integer

    Private _Keys As Collections.Hashtable

    Function GetKey(ByVal Key As Short)
        Dim sBuffer As String, lRet As Long
        ' Fill String with 255 spaces
        sBuffer = Strings.StrDup(255, Chr(0))
        ' Call DLL
        Dim sKey As String
        If Key < 10 Then
            sKey = "key0" & CStr(Key)
        Else
            sKey = "key" & CStr(Key)
        End If
        lRet = GetPrivateProfileString("keyboard", sKey, "0", sBuffer, Len(sBuffer), _path & "\user.dat")
        If lRet = 0 Then
            Return "0"
        Else
            ' DLL successful
            Return sBuffer.Substring(0, sBuffer.IndexOf(Chr(0)))
        End If
    End Function

    Function GetShift(ByVal Key As Short)
        Dim sBuffer As String, lRet As Long
        ' Fill String with 255 spaces
        sBuffer = Strings.StrDup(255, Chr(0))
        ' Call DLL
        Dim sKey As String
        If Key < 10 Then
            sKey = "shift0" & CStr(Key)
        Else
            sKey = "shift" & CStr(Key)
        End If
        lRet = GetPrivateProfileString("keyboard", sKey, "0", sBuffer, Len(sBuffer), _path & "\user.dat")
        If lRet = 0 Then
            Return "0"
        Else
            ' DLL successful
            Return sBuffer.Substring(0, sBuffer.IndexOf(Chr(0)))
        End If
    End Function

    Private Enum DAOCKeys As Byte
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

    Public Sub New()
        Me.New(Nothing, "")
    End Sub

    Public Sub New(ByVal AK As clsAutoKillerScript)
        Me.New(AK, "")
    End Sub

    Public Sub New(ByVal path As String)
        Me.New(Nothing, path)
    End Sub

    Public Sub New(ByVal AK As clsAutoKillerScript, ByVal path As String)
        MyBase.new()
        _path = path
        _AK = AK
        _WaitTime = 100
        LoadKeys()
    End Sub

    Public Function FindVirtualKey(ByVal aKey As Byte) As Integer
        Dim i As Integer
        For i = 0 To 256
            If MapVirtualKey(i, 0) = aKey Then
                Return i
            End If
        Next
    End Function

    Public Sub LoadKeys()
        Dim i As Byte

        _Keys = New Hashtable(79)

        For i = 0 To 79
            Dim Key As Byte = FindVirtualKey(CByte(GetKey(i)))
            Dim [Shift] As Boolean = CBool(GetShift(i))
            _Keys.Add(CType(i, DAOCKeys), New KeyPair(Key, [Shift]))
        Next
    End Sub

    Protected Overrides Sub Finalize()
        _AK = Nothing
        _Keys.Clear()
        _Keys = Nothing
        MyBase.Finalize()
    End Sub

    Public Property WaitTime() As Integer
        Get
            Return _WaitTime
        End Get
        Set(ByVal Value As Integer)
            _WaitTime = Value
        End Set
    End Property

    Private Sub SendKey(ByVal k As KeyPair)
        If k.Shift Then
            _AK.SendKeys(Keys.ShiftKey, True, False)
        End If
        Thread.CurrentThread.Sleep(WaitTime)
        'Debug.WriteLine("Sending: " & Hex(k.Key))
        'Debug.WriteLine("Sending Key: " & Chr(k.Key))
        _AK.SendKeys(k.Key)
        Thread.CurrentThread.Sleep(WaitTime)
        If k.Shift Then
            _AK.SendKeys(Keys.ShiftKey, False, True)
        End If
    End Sub

    Public ReadOnly Property MoveForward1_Key() As Byte
        Get
            Return DirectCast(_Keys(DAOCKeys.MoveForward1), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property MoveForward1_Shift() As Boolean
        Get
            Return DirectCast(_Keys(DAOCKeys.MoveForward1), KeyPair).Shift
        End Get
    End Property

    Public Sub MoveForward1()
        SendKey(DirectCast(_Keys(DAOCKeys.MoveForward1), KeyPair))
    End Sub

    Public ReadOnly Property MoveForward2_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.MoveForward2), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property MoveForward2_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.MoveForward2), KeyPair).Shift
        End Get
    End Property

    Public Sub MoveForward2()
        SendKey(DirectCast(_Keys(DAOCKeys.MoveForward2), KeyPair))
    End Sub

    Public ReadOnly Property MoveBackward1_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.MoveBackward1), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property MoveBackward1_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.MoveBackward1), KeyPair).Shift
        End Get
    End Property

    Public Sub MoveBackward1()
        SendKey(DirectCast(_Keys(DAOCKeys.MoveBackward1), KeyPair))
    End Sub

    Public ReadOnly Property MoveBackward2_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.MoveBackward2), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property MoveBackward2_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.MoveBackward2), KeyPair).Shift
        End Get
    End Property

    Public Sub MoveBackward2()
        SendKey(DirectCast(_Keys(DAOCKeys.MoveBackward2), KeyPair))
    End Sub

    Public ReadOnly Property CommandWindow_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.CommandWindow), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property CommandWindow_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.CommandWindow), KeyPair).Shift
        End Get
    End Property

    Public Sub CommandWindow()
        SendKey(DirectCast(_Keys(DAOCKeys.CommandWindow), KeyPair))
    End Sub

    Public ReadOnly Property Sprint_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.Sprint), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property Sprint_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.Sprint), KeyPair).Shift
        End Get
    End Property

    Public Sub Sprint()
        SendKey(DirectCast(_Keys(DAOCKeys.Sprint), KeyPair))
    End Sub

    Public ReadOnly Property Craft_Salvage_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.Craft_Salvage), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property Craft_Salvage_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.Craft_Salvage), KeyPair).Shift
        End Get
    End Property

    Public Sub Craft_Salvage()
        SendKey(DirectCast(_Keys(DAOCKeys.Craft_Salvage), KeyPair))
    End Sub

    Public ReadOnly Property Face_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.Face), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property Face_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.Face), KeyPair).Shift
        End Get
    End Property

    Public Sub Face()
        SendKey(DirectCast(_Keys(DAOCKeys.Face), KeyPair))
    End Sub

    Public ReadOnly Property Stick_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.Stick), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property Stick_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.Stick), KeyPair).Shift
        End Get
    End Property

    Public Sub Stick()
        SendKey(DirectCast(_Keys(DAOCKeys.Stick), KeyPair))
    End Sub

    Public ReadOnly Property Follow_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.Follow), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property _Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.Follow), KeyPair).Shift
        End Get
    End Property

    Public Sub Follow()
        SendKey(DirectCast(_Keys(DAOCKeys.Follow), KeyPair))
    End Sub

    Public ReadOnly Property PanCamera_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.PanCamera), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property PanCamera_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.PanCamera), KeyPair).Shift
        End Get
    End Property

    Public Sub PanCamera()
        SendKey(DirectCast(_Keys(DAOCKeys.PanCamera), KeyPair))
    End Sub

    Public ReadOnly Property Info_Delve_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.Info_Delve), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property Info_Delve_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.Info_Delve), KeyPair).Shift
        End Get
    End Property

    Public Sub Info_Delve()
        SendKey(DirectCast(_Keys(DAOCKeys.Info_Delve), KeyPair))
    End Sub

    Public ReadOnly Property Sit_Stand_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.Sit_Stand), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property Sit_Stand_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.Sit_Stand), KeyPair).Shift
        End Get
    End Property

    Public Sub Sit_Stand()
        SendKey(DirectCast(_Keys(DAOCKeys.Sit_Stand), KeyPair))
    End Sub

    Public ReadOnly Property TargetGroup8_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.TargetGroup8), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property TargetGroup8_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.TargetGroup8), KeyPair).Shift
        End Get
    End Property

    Public Sub TargetGroup8()
        SendKey(DirectCast(_Keys(DAOCKeys.TargetGroup8), KeyPair))
    End Sub

    Public ReadOnly Property TargetGroup7_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.TargetGroup7), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property TargetGroup7_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.TargetGroup7), KeyPair).Shift
        End Get
    End Property

    Public Sub TargetGroup7()
        SendKey(DirectCast(_Keys(DAOCKeys.TargetGroup7), KeyPair))
    End Sub

    Public ReadOnly Property TargetGroup6_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.TargetGroup6), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property TargetGroup6_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.TargetGroup6), KeyPair).Shift
        End Get
    End Property

    Public Sub TargetGroup6()
        SendKey(DirectCast(_Keys(DAOCKeys.TargetGroup6), KeyPair))
    End Sub

    Public ReadOnly Property TargetGroup5_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.TargetGroup5), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property TargetGroup5_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.TargetGroup5), KeyPair).Shift
        End Get
    End Property

    Public Sub TargetGroup5()
        SendKey(DirectCast(_Keys(DAOCKeys.TargetGroup5), KeyPair))
    End Sub

    Public ReadOnly Property TargetGroup4_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.TargetGroup4), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property TargetGroup4_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.TargetGroup4), KeyPair).Shift
        End Get
    End Property

    Public Sub TargetGroup4()
        SendKey(DirectCast(_Keys(DAOCKeys.TargetGroup4), KeyPair))
    End Sub

    Public ReadOnly Property TargetGroup3_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.TargetGroup3), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property TargetGroup3_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.TargetGroup3), KeyPair).Shift
        End Get
    End Property

    Public Sub TargetGroup3()
        SendKey(DirectCast(_Keys(DAOCKeys.TargetGroup3), KeyPair))
    End Sub

    Public ReadOnly Property TargetGroup2_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.TargetGroup2), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property TargetGroup2_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.TargetGroup2), KeyPair).Shift
        End Get
    End Property

    Public Sub TargetGroup2()
        SendKey(DirectCast(_Keys(DAOCKeys.TargetGroup2), KeyPair))
    End Sub

    Public ReadOnly Property TargetGroup1_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.TargetGroup1), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property TargetGroup1_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.TargetGroup1), KeyPair).Shift
        End Get
    End Property

    Public Sub TargetGroup1()
        SendKey(DirectCast(_Keys(DAOCKeys.TargetGroup1), KeyPair))
    End Sub

    Public ReadOnly Property GroundTarget_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.GroundTarget), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property GroundTarget_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.GroundTarget), KeyPair).Shift
        End Get
    End Property

    Public Sub GroundTarget()
        SendKey(DirectCast(_Keys(DAOCKeys.GroundTarget), KeyPair))
    End Sub

    Public ReadOnly Property Consider_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.Consider), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property Consider_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.Consider), KeyPair).Shift
        End Get
    End Property

    Public Sub Consider()
        SendKey(DirectCast(_Keys(DAOCKeys.Consider), KeyPair))
    End Sub

    Public ReadOnly Property RunLock_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.RunLock), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property RunLock_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.RunLock), KeyPair).Shift
        End Get
    End Property

    Public Sub RunLock()
        SendKey(DirectCast(_Keys(DAOCKeys.RunLock), KeyPair))
    End Sub

    Public ReadOnly Property ShowCombat_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.ShowCombat), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property ShowCombat_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.ShowCombat), KeyPair).Shift
        End Get
    End Property

    Public Sub ShowCombat()
        SendKey(DirectCast(_Keys(DAOCKeys.ShowCombat), KeyPair))
    End Sub

    Public ReadOnly Property ShowSkills_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.ShowSkills), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property ShowSkills_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.ShowSkills), KeyPair).Shift
        End Get
    End Property

    Public Sub ShowSkills()
        SendKey(DirectCast(_Keys(DAOCKeys.ShowSkills), KeyPair))
    End Sub

    Public ReadOnly Property Destroy_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.Destroy), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property Destroy_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.Destroy), KeyPair).Shift
        End Get
    End Property

    Public Sub Destroy()
        SendKey(DirectCast(_Keys(DAOCKeys.Destroy), KeyPair))
    End Sub

    Public ReadOnly Property ChatToggle_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.ChatToggle), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property ChatToggle_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.ChatToggle), KeyPair).Shift
        End Get
    End Property

    Public Sub ChatToggle()
        SendKey(DirectCast(_Keys(DAOCKeys.ChatToggle), KeyPair))
    End Sub

    Public ReadOnly Property Reply_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.Reply), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property Reply_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.Reply), KeyPair).Shift
        End Get
    End Property

    Public Sub Reply()
        SendKey(DirectCast(_Keys(DAOCKeys.Reply), KeyPair))
    End Sub

    Public ReadOnly Property LastAttacker_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.LastAttacker), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property LastAttacker_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.LastAttacker), KeyPair).Shift
        End Get
    End Property

    Public Sub LastAttacker()
        SendKey(DirectCast(_Keys(DAOCKeys.LastAttacker), KeyPair))
    End Sub

    Public ReadOnly Property UserItem_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.UseItem), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property UseItem_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.UseItem), KeyPair).Shift
        End Get
    End Property

    Public Sub UseItem()
        SendKey(DirectCast(_Keys(DAOCKeys.UseItem), KeyPair))
    End Sub

    Public ReadOnly Property ShowGroup_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.ShowGroup), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property ShowGroup_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.ShowGroup), KeyPair).Shift
        End Get
    End Property

    Public Sub ShowGroup()
        SendKey(DirectCast(_Keys(DAOCKeys.ShowGroup), KeyPair))
    End Sub

    Public ReadOnly Property ShowInventory_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.ShowInventory), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property ShowInventory_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.ShowInventory), KeyPair).Shift
        End Get
    End Property

    Public Sub ShowInventory()
        SendKey(DirectCast(_Keys(DAOCKeys.ShowInventory), KeyPair))
    End Sub

    Public ReadOnly Property ShowStats_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.ShowStats), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property ShowStats_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.ShowStats), KeyPair).Shift
        End Get
    End Property

    Public Sub ShowStats()
        SendKey(DirectCast(_Keys(DAOCKeys.ShowStats), KeyPair))
    End Sub

    Public ReadOnly Property Sell_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.Sell), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property Sell_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.Sell), KeyPair).Shift
        End Get
    End Property

    Public Sub Sell()
        SendKey(DirectCast(_Keys(DAOCKeys.Sell), KeyPair))
    End Sub

    Public ReadOnly Property ShowSpells_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.ShowSpells), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property ShowSpells_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.ShowSpells), KeyPair).Shift
        End Get
    End Property

    Public Sub ShowSpells()
        SendKey(DirectCast(_Keys(DAOCKeys.ShowSpells), KeyPair))
    End Sub

    Public ReadOnly Property AttackMode_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.AttackMode), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property AttackMode_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.AttackMode), KeyPair).Shift
        End Get
    End Property

    Public Sub AttackMode()
        SendKey(DirectCast(_Keys(DAOCKeys.AttackMode), KeyPair))
    End Sub

    Public ReadOnly Property NearestObject_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.NearestObject), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property NearestObject_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.NearestObject), KeyPair).Shift
        End Get
    End Property

    Public Sub NearestObject()
        SendKey(DirectCast(_Keys(DAOCKeys.NearestObject), KeyPair))
    End Sub

    Public ReadOnly Property NearestFriend_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.NearestFriend), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property NearestFriend_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.NearestFriend), KeyPair).Shift
        End Get
    End Property

    Public Sub NearestFriend()
        SendKey(DirectCast(_Keys(DAOCKeys.NearestFriend), KeyPair))
    End Sub

    Public ReadOnly Property NearestEnemy_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.NearestEnemy), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property NearestEnemy_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.NearestEnemy), KeyPair).Shift
        End Get
    End Property

    Public Sub NearestEnemy()
        SendKey(DirectCast(_Keys(DAOCKeys.NearestEnemy), KeyPair))
    End Sub

    Public ReadOnly Property Torch_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.Torch), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property Torch_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.Torch), KeyPair).Shift
        End Get
    End Property

    Public Sub Torch()
        SendKey(DirectCast(_Keys(DAOCKeys.Torch), KeyPair))
    End Sub

    Public ReadOnly Property GetItem_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.GetItem), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property GetItem_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.GetItem), KeyPair).Shift
        End Get
    End Property

    Public Sub GetItem()
        SendKey(DirectCast(_Keys(DAOCKeys.GetItem), KeyPair))
    End Sub

    Public ReadOnly Property LookDown_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.LookDown), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property LookDown_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.LookDown), KeyPair).Shift
        End Get
    End Property

    Public Sub LookDown()
        SendKey(DirectCast(_Keys(DAOCKeys.LookDown), KeyPair))
    End Sub

    Public ReadOnly Property LookUp_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.Lookup), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property LookUp_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.Lookup), KeyPair).Shift
        End Get
    End Property

    Public Sub LookUp()
        SendKey(DirectCast(_Keys(DAOCKeys.Lookup), KeyPair))
    End Sub

    Public ReadOnly Property Select_Open_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.Select_Open), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property Select_Open_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.Select_Open), KeyPair).Shift
        End Get
    End Property

    Public Sub Select_Open()
        SendKey(DirectCast(_Keys(DAOCKeys.Select_Open), KeyPair))
    End Sub

    Public ReadOnly Property StrafeRight_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.StrafeRight), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property StrafeRight_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.StrafeRight), KeyPair).Shift
        End Get
    End Property

    Public Sub StrafeRight()
        SendKey(DirectCast(_Keys(DAOCKeys.StrafeRight), KeyPair))
    End Sub

    Public ReadOnly Property StrafeLeft_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.StrafeLeft), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property StrafeLeft_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.StrafeLeft), KeyPair).Shift
        End Get
    End Property

    Public Sub StrafeLeft()
        SendKey(DirectCast(_Keys(DAOCKeys.StrafeLeft), KeyPair))
    End Sub

    Public ReadOnly Property Walk_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.Walk), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property Walk_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.Walk), KeyPair).Shift
        End Get
    End Property

    Public Sub Walk()
        SendKey(DirectCast(_Keys(DAOCKeys.Walk), KeyPair))
    End Sub

    Public ReadOnly Property StrafeToggle_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.StrafeToggle), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property StrafeToggle_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.StrafeToggle), KeyPair).Shift
        End Get
    End Property

    Public Sub StrafeToggle()
        SendKey(DirectCast(_Keys(DAOCKeys.StrafeToggle), KeyPair))
    End Sub

    Public ReadOnly Property Down_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.Down), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property Down_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.Down), KeyPair).Shift
        End Get
    End Property

    Public Sub Down()
        SendKey(DirectCast(_Keys(DAOCKeys.Down), KeyPair))
    End Sub

    Public ReadOnly Property Up_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.Up), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property Up_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.Up), KeyPair).Shift
        End Get
    End Property

    Public Sub Up()
        SendKey(DirectCast(_Keys(DAOCKeys.Up), KeyPair))
    End Sub

    Public ReadOnly Property TurnRight_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.TurnRight), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property TurnRight_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.TurnRight), KeyPair).Shift
        End Get
    End Property

    Public Sub TurnRight()
        SendKey(DirectCast(_Keys(DAOCKeys.TurnRight), KeyPair))
    End Sub

    Public ReadOnly Property TurnLeft_Key() As Byte
        Get
            Return CType(_Keys(DAOCKeys.TurnLeft), KeyPair).Key
        End Get
    End Property

    Public ReadOnly Property TurnLeft_Shift() As Boolean
        Get
            Return CType(_Keys(DAOCKeys.TurnLeft), KeyPair).Shift
        End Get
    End Property

    Public Sub TurnLeft()
        SendKey(DirectCast(_Keys(DAOCKeys.TurnLeft), KeyPair))
    End Sub

    Public Sub SelectHotBar(ByVal Hotbar As Integer)
        SendKey(New KeyPair(Asc(CInt(Hotbar)), True))
    End Sub

End Class
