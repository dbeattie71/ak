Imports System.Threading
#Region "Window Manager"
Public Enum WINDOW_NAMES
    ChatWindow
    MiniGroup
    MiniConc
    MiniFriends
    MiniPet
    GroupFinder
    TimeDisplay
    HousingIntOptions
    HousingExtOptions
    HousingGarden
    HousingFriendPermissions
    HousingPermissionsList
    HousingIntRotation
    MoneySelector
    PlayerMerchant
    BazaarQuery
    BazaarResults
    LagMeter
    Compass
    TradeWindow
    TradeTimer
    TradeSkillWindow
    CharInfo
    ChannelPicker
    Stats
    Train
    Bank
    NpcTalk
    QuestLog
    Dialog
    MiniCraft
    MiniInfo
    MinorPets
    OptionsDialog
    CommandWindow
    InfoPage
    BigDialog
    Quickbar
    Merchant
    Version
    Alpha
    Help
    Siege
    LFG
    LFGOptions
    LFGLevels
    LFGPurposeFlags
    LFGClass0
    LFGClass1
    LFGClass2
    MasterLevel
    Quiver
    GroupBuffs
    KeepUpgrade
    KeepStatus
    HookpointStore
    Warmap
    WarmapKey
    RealmBonuses
    MapWindow
    KeyConfig
End Enum
Public Class WindowManager
#Region "Variables"
    Private AK As AutoKillerScript.clsAutoKillerScript
    Private mWindowSettings As ArrayList
#End Region
    Public Sub New(ByVal DLL As AutoKillerScript.clsAutoKillerScript, ByVal Window As WINDOW_NAMES)
        MyBase.New()
        AK = DLL
        GetWindowPosition(GetStringFromEnum(Window))
    End Sub
    Protected Overrides Sub Finalize()
        mWindowSettings = Nothing
        MyBase.Finalize()
    End Sub
    Public ReadOnly Property Left() As Integer
        Get
            If mWindowSettings.Count >= 0 Then
                Return CInt(mWindowSettings(0))
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property Top() As Integer
        Get
            If mWindowSettings.Count >= 1 Then
                Return CInt(mWindowSettings(1))
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property Open() As Boolean
        Get
            If mWindowSettings.Count >= 2 Then
                Return CBool(mWindowSettings(2))
            Else
                Return False
            End If
        End Get
    End Property
    Public ReadOnly Property Value(ByVal x As Integer) As Object
        Get
            If mWindowSettings.Count >= x Then
                Return CInt(mWindowSettings(x))
            Else
                Return Nothing
            End If
        End Get
    End Property
    Protected Sub GetWindowPosition(ByVal Window As String)
        Dim s As String

        'get window position
        With New CIniFile
            .Path = AK.GamePath & AK.GetPlayerINI
            s = .Value("Panels", Window, "")
        End With

        mWindowSettings = New ArrayList(Split(s, ","))

    End Sub
    Protected Sub DoLeftClick(ByVal x As Integer, ByVal y As Integer)
        With AK
            .MouseMove(x, y)
            Thread.Sleep(1000)
            .LeftClick()
        End With
    End Sub
    Protected Sub DoRightClick(ByVal x As Integer, ByVal y As Integer)
        With AK
            .MouseMove(x, y)
            Thread.Sleep(1000)
            .RightClick()
        End With
    End Sub
    Private Function GetStringFromEnum(ByVal WindowName As WINDOW_NAMES) As String

        Select Case WindowName
            Case WINDOW_NAMES.Bank
                Return "Bank"
            Case WINDOW_NAMES.BazaarQuery
                Return "BazaarQuery"
            Case WINDOW_NAMES.BazaarResults
                Return "BazaarResults"
            Case WINDOW_NAMES.BigDialog
                Return "BigDialog"
            Case WINDOW_NAMES.BigDialog
                Return "BigDialog"
            Case WINDOW_NAMES.ChannelPicker
                Return "ChannelPicker"
            Case WINDOW_NAMES.CharInfo
                Return "CharInfo"
            Case WINDOW_NAMES.ChatWindow
                Return "ChatWindow"
            Case WINDOW_NAMES.CommandWindow
                Return "CommandWindow"
            Case WINDOW_NAMES.Compass
                Return "Compass"
            Case WINDOW_NAMES.Dialog
                Return "Dialog"
            Case WINDOW_NAMES.GroupFinder
                Return "GroupFinder"
            Case WINDOW_NAMES.HousingExtOptions
                Return "HousingExtOptions"
            Case WINDOW_NAMES.HousingFriendPermissions
                Return "HousingFriendPermissions"
            Case WINDOW_NAMES.HousingGarden
                Return "HousingGarden"
            Case WINDOW_NAMES.HousingIntOptions
                Return "HousingIntOptions"
            Case WINDOW_NAMES.HousingIntRotation
                Return "HousingIntRotation"
            Case WINDOW_NAMES.HousingPermissionsList
                Return "HousingPermissionsList"
            Case WINDOW_NAMES.InfoPage
                Return "InfoPage"
            Case WINDOW_NAMES.LagMeter
                Return "LagMeter"
            Case WINDOW_NAMES.MiniConc
                Return "MiniConc"
            Case WINDOW_NAMES.MiniCraft
                Return "MiniCraft"
            Case WINDOW_NAMES.MiniFriends
                Return "MiniFriends"
            Case WINDOW_NAMES.MiniGroup
                Return "MiniGroup"
            Case WINDOW_NAMES.MiniInfo
                Return "MiniInfo"
            Case WINDOW_NAMES.MiniPet
                Return "MiniPet"
            Case WINDOW_NAMES.MinorPets
                Return "MinorPets"
            Case WINDOW_NAMES.MoneySelector
                Return "MoneySelector"
            Case WINDOW_NAMES.NpcTalk
                Return "NpcTalk"
            Case WINDOW_NAMES.OptionsDialog
                Return "OptionsDialog"
            Case WINDOW_NAMES.PlayerMerchant
                Return "PlayerMerchant"
            Case WINDOW_NAMES.QuestLog
                Return "QuestLog"
            Case WINDOW_NAMES.Quickbar
                Return "Quickbar"
            Case WINDOW_NAMES.Stats
                Return "Stats"
            Case WINDOW_NAMES.TimeDisplay
                Return "TimeDisplay"
            Case WINDOW_NAMES.TradeSkillWindow
                Return "TradeSkillWindow"
            Case WINDOW_NAMES.TradeTimer
                Return "TradeTimer"
            Case WINDOW_NAMES.TradeWindow
                Return "TradeWindow"
            Case WINDOW_NAMES.Train
                Return "Train"
        End Select

        Return String.Empty

    End Function
End Class
#End Region
#Region "QBar"
Public Enum QuickbarOrientation
    qboVertical
    qboHorizontal
    qboClosed
End Enum
Public Class QBar
    Inherits WindowManager
    Private mOrientation As QuickbarOrientation
    Public Sub New(ByVal DLL As AutoKillerScript.clsAutoKillerScript)
        MyBase.New(DLL, WINDOW_NAMES.Quickbar)
        GetStartingOrientation()
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    Private Sub GetStartingOrientation()
        Try
            mOrientation = DirectCast(Value(3), QuickbarOrientation)
        Catch
            mOrientation = QuickbarOrientation.qboClosed
        End Try
    End Sub
    Private Sub SlotXY(ByVal ASlot As Integer, ByRef X As Integer, ByRef Y As Integer)
        Select Case mOrientation
            Case QuickbarOrientation.qboVertical
                X = Left + 25
                Y = Top + 45 + (34 * (ASlot - 1))
            Case QuickbarOrientation.qboHorizontal
                X = Top + 45 + (34 * (ASlot - 1))
                Y = Left + 25
        End Select
    End Sub
    Public Sub ClickSlot(ByVal ASlot As Integer)
        Dim X, Y As Integer
        If ASlot < 1 OrElse ASlot > 10 Then Exit Sub
        SlotXY(ASlot, X, Y)
        DoLeftClick(X, Y)
    End Sub
End Class
#End Region
#Region "Stats"
Public Enum StatsWindowPage
    swpAttributes
    swpInventory
    swpSkills
    swpCombat
    swpSpells
    swpGroup
    swpNone
End Enum
Public Enum WeaponSlots
    RightHand
    LeftHand
    TwoHand
    Ranged
End Enum
Public Class StatsWindow
    Inherits WindowManager
    Private mRangedX As Integer
    Private mRightHandX As Integer
    Private mLeftHandX As Integer
    Private mTwoHandX As Integer
    Private mWeaponY As Integer
    Public Sub New(ByVal DLL As AutoKillerScript.clsAutoKillerScript)
        MyBase.New(DLL, WINDOW_NAMES.Stats)

        mWeaponY = Top + 250
        mRangedX = Left + 158
        mRightHandX = Left + 30
        mLeftHandX = Left + 75
        mTwoHandX = Left + 120

    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    Public Sub ClickPage(ByVal aPage As StatsWindowPage)
        Const BUTTON_WIDTH As Integer = 28
        Const BUTTON_HEIGHT As Integer = 32
        Const BUTTON_TOP_OFFSET As Integer = 1
        Const BUTTON_LEFT_OFFSET As Integer = 14

        DoLeftClick(Left + BUTTON_LEFT_OFFSET + (BUTTON_WIDTH * aPage) + (BUTTON_WIDTH \ 2), Top + BUTTON_TOP_OFFSET + (BUTTON_HEIGHT \ 2))
    End Sub
    Public Sub SelectInventoryItem(ByVal AItem As Integer)
        Const INV_LEFT_OFFSET As Integer = 20
        Const INV_TOP_OFFSET As Integer = 278
        Const INV_HEIGHT As Integer = 17

        If AItem < 0 OrElse AItem > 7 Then Exit Sub

        DoLeftClick(Left + INV_LEFT_OFFSET, Top + INV_TOP_OFFSET + (AItem * (INV_HEIGHT)) + (INV_HEIGHT \ 2))
    End Sub
    Public Sub SelectInventoryBag(ByVal aBag As Integer)
        Dim BAG_TOP_OFFSETS() As Integer = {291, 318, 343, 368, 393}
        Const BAG_LEFT_OFFSET As Integer = 180
        If aBag < 0 OrElse aBag > 4 Then Exit Sub
        DoLeftClick(Left + BAG_LEFT_OFFSET, Top + BAG_TOP_OFFSETS(aBag))
    End Sub
    Public Sub MoveInventoryItem(ByVal AFromBag As Integer, ByVal AFromItem As Integer, ByVal AToBag As Integer, ByVal AToItem As Integer)
        ClickPage(StatsWindowPage.swpAttributes)
        Thread.Sleep(500)
        ClickPage(StatsWindowPage.swpInventory)
        Thread.Sleep(500)

        SelectInventoryBag(AFromBag)
        Thread.Sleep(500)
        SelectInventoryItem(AFromItem)
        Thread.Sleep(500)

        If AFromBag <> AToBag Then
            SelectInventoryBag(AToBag)
            Thread.Sleep(500)
        End If

        SelectInventoryItem(AToItem)
        Thread.Sleep(1000)

        ClickPage(StatsWindowPage.swpInventory)
    End Sub
    Public Sub MoveInventoryItemToWeaponSlot(ByVal AFromBag As Integer, ByVal AFromItem As Integer, ByVal WSlot As WeaponSlots)
        ClickPage(StatsWindowPage.swpAttributes)
        Thread.Sleep(500)
        ClickPage(StatsWindowPage.swpInventory)
        Thread.Sleep(500)

        SelectInventoryBag(AFromBag)
        Thread.Sleep(500)
        SelectInventoryItem(AFromItem)
        Thread.Sleep(500)

        Select Case WSlot
            Case WeaponSlots.LeftHand
                DoLeftClick(mLeftHandX, mWeaponY)
            Case WeaponSlots.RightHand
                DoLeftClick(mRightHandX, mWeaponY)
            Case WeaponSlots.TwoHand
                DoLeftClick(mTwoHandX, mWeaponY)
            Case WeaponSlots.Ranged
                DoLeftClick(mRangedX, mWeaponY)
        End Select

        Thread.Sleep(1000)

        ClickPage(StatsWindowPage.swpInventory)
    End Sub
    Public Sub MoveInventoryItemToQbarSlot(ByVal AFromBag As Integer, ByVal AFromItem As Integer, ByVal Slot As Integer, ByVal Q As QBar)
        ClickPage(StatsWindowPage.swpAttributes)
        Thread.Sleep(500)
        ClickPage(StatsWindowPage.swpInventory)
        Thread.Sleep(500)

        SelectInventoryBag(AFromBag)
        Thread.Sleep(500)
        SelectInventoryItem(AFromItem)
        Thread.Sleep(500)

        Q.ClickSlot(Slot)

        Thread.Sleep(1000)

        ClickPage(StatsWindowPage.swpInventory)
    End Sub
    Public Sub MoveWeaponItemToQbarSlot(ByVal WSlot As WeaponSlots, ByVal Slot As Integer, ByVal Q As QBar)
        ClickPage(StatsWindowPage.swpAttributes)
        Thread.Sleep(500)
        ClickPage(StatsWindowPage.swpInventory)
        Thread.Sleep(500)

        Select Case WSlot
            Case WeaponSlots.LeftHand
                DoLeftClick(mLeftHandX, mWeaponY)
            Case WeaponSlots.RightHand
                DoLeftClick(mRightHandX, mWeaponY)
            Case WeaponSlots.TwoHand
                DoLeftClick(mTwoHandX, mWeaponY)
            Case WeaponSlots.Ranged
                DoLeftClick(mRangedX, mWeaponY)
        End Select

        Thread.Sleep(1000)

        Q.ClickSlot(Slot)

        Thread.Sleep(1000)

        ClickPage(StatsWindowPage.swpInventory)
    End Sub
    Public Sub MoveWeaponSlotItemToInventory(ByVal WSlot As WeaponSlots, ByVal AToBag As Integer, ByVal AToItem As Integer)
        ClickPage(StatsWindowPage.swpAttributes)
        Thread.Sleep(500)
        ClickPage(StatsWindowPage.swpInventory)
        Thread.Sleep(500)

        Select Case WSlot
            Case WeaponSlots.LeftHand
                DoLeftClick(mLeftHandX, mWeaponY)
            Case WeaponSlots.RightHand
                DoLeftClick(mRightHandX, mWeaponY)
            Case WeaponSlots.TwoHand
                DoLeftClick(mTwoHandX, mWeaponY)
            Case WeaponSlots.Ranged
                DoLeftClick(mRangedX, mWeaponY)
        End Select

        Thread.Sleep(1000)

        SelectInventoryBag(AToBag)
        Thread.Sleep(500)
        SelectInventoryItem(AToItem)
        Thread.Sleep(500)

        ClickPage(StatsWindowPage.swpInventory)
    End Sub
    Public ReadOnly Property StartingOpenPage() As StatsWindowPage
        Get
            Return GetStartingOpenPage()
        End Get
    End Property
    Private Function GetStartingOpenPage() As StatsWindowPage
        Try
            Return DirectCast(Value(3), StatsWindowPage)
        Catch
            Return StatsWindowPage.swpNone
        End Try
    End Function
End Class
#End Region
Public NotInheritable Class Sell
    Inherits StatsWindow
    Private AK As AutoKillerScript.clsAutoKillerScript
    Private UKeys As AutoKillerScript.UserKeys
    Public Sub New(ByVal DLL As AutoKillerScript.clsAutoKillerScript)
        MyBase.New(DLL)
        UKeys = New AutoKillerScript.UserKeys(DLL)
        AK = DLL
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    Public Sub Sell(ByVal Bag As Short, ByVal ItemsNotToSell As ArrayList)  'sells up to bag number passed

        ClickPage(StatsWindowPage.swpAttributes)
        Thread.Sleep(500)
        ClickPage(StatsWindowPage.swpInventory)
        Thread.Sleep(500)

        Dim c(1) As Integer

        For i As Short = 0 To Bag
            SelectInventoryBag(i)
            Select Case i
                Case 0
                    c(0) = 0
                    c(1) = 7
                    SellItems(ItemsNotToSell, c) 'sell all items in specified bag
                Case 1
                    c(0) = 8
                    c(1) = 15
                    SellItems(ItemsNotToSell, c) 'sell all items in specified bag
                Case 2
                    c(0) = 16
                    c(1) = 23
                    SellItems(ItemsNotToSell, c) 'sell all items in specified bag
                Case 3
                    c(0) = 24
                    c(1) = 31
                    SellItems(ItemsNotToSell, c) 'sell all items in specified bag
                Case 4
                    c(0) = 32
                    c(1) = 39
                    SellItems(ItemsNotToSell, c) 'sell all items in specified bag
            End Select
        Next

        Thread.Sleep(250)

    End Sub
    Private Sub SellItems(ByVal ItemsNotToSell As ArrayList, ByVal counter() As Integer)

        Dim sItem As Short
        Dim Checker As Boolean
        Dim c As Integer

        For i As Integer = counter(0) To counter(1)
            For Each sItem In ItemsNotToSell 'if any item in array is true it won't be sold
                If sItem = i Then Checker = True
            Next
            If Not Checker Then
                SelectInventoryItem(c)
                Thread.Sleep(1000)
                UKeys.Sell()
                Thread.Sleep(1000)
            End If
            Checker = False
            c += 1
        Next

    End Sub
End Class
