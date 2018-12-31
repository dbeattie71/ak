Imports System.Threading

Public Class DAOCWindowManager
    Private mDAOCPath As String
    Private mServerIP As String
    Private mCharacterName As String
    Public Event OnNeedLeftClick(ByVal Sender As Object, ByVal X As Integer, ByVal Y As Integer)
    Public Event OnNeedRightClick(ByVal Sender As Object, ByVal X As Integer, ByVal Y As Integer)
    Public Event OnNeedSendKeys(ByVal Sender As Object, ByVal aKeys As String)
    Public Event OnNeedVKUp(ByVal Sender As Object, ByVal vk As Byte)
    Public Event OnNeedVKDown(ByVal Sender As Object, ByVal vk As Byte)

    Private Function GetSettingsFile() As String
        Return mDAOCPath & IIf(mDAOCPath.Chars(mDAOCPath.Length - 1) = "\", "", "\").ToString & mCharacterName & "-" & LastIPOctet() & ".ini"
    End Function

    Protected Friend Function LastIPOctet() As String
        Dim iStartPos As Integer

        iStartPos = 0
        ParseWord(mServerIP, iStartPos)
        ParseWord(mServerIP, iStartPos)
        ParseWord(mServerIP, iStartPos)
        Return ParseWord(mServerIP, iStartPos)

    End Function

    Protected Friend Sub DoLeftClick(ByVal Sender As Object, ByVal X As Integer, ByVal Y As Integer)
        RaiseEvent OnNeedLeftClick(Sender, X, Y)
    End Sub

    Protected Friend Sub DoRightClick(ByVal Sender As Object, ByVal X As Integer, ByVal Y As Integer)
        RaiseEvent OnNeedRightClick(Sender, X, Y)
    End Sub

    Protected Friend Sub DoSendKeys(ByVal Sender As Object, ByVal aKeys As String)
        RaiseEvent OnNeedSendKeys(Sender, aKeys)
    End Sub

    Protected Friend Sub DoVKDown(ByVal Sender As Object, ByVal vk As Byte)
        RaiseEvent OnNeedVKDown(Sender, vk)
    End Sub

    Protected Friend Sub DoVKUp(ByVal Sender As Object, ByVal vk As Byte)
        RaiseEvent OnNeedVKUp(Sender, vk)
    End Sub

    Public ReadOnly Property SettingsFile() As String
        Get
            Return GetSettingsFile()
        End Get
    End Property

    Public Property DAOCPath() As String
        Get
            Return mDAOCPath
        End Get
        Set(ByVal Value As String)
            mDAOCPath = Value
        End Set
    End Property

    Public Property CharacterName() As String
        Get
            Return mCharacterName
        End Get
        Set(ByVal Value As String)
            mCharacterName = Value
        End Set
    End Property

    Public Property ServerIP() As String
        Get
            Return mServerIP
        End Get
        Set(ByVal Value As String)
            mServerIP = Value
        End Set
    End Property

    Public Sub MoveInventoryItem(ByVal AFromBag As Integer, ByVal AFromItem As Integer, ByVal AToBag As Integer, ByVal AToItem As Integer)
        Dim aStatsWindow As StatsWindow = New StatsWindow(Me)
        With aStatsWindow
            .MoveInventoryItem(AFromBag, AFromItem, AToBag, AToItem)
        End With
        aStatsWindow = Nothing
    End Sub

End Class

Public Class DAOCWindow
    Private Function GetLeft() As Integer
        If mSettings.Count > 0 Then
            Try
                Return CInt(mSettings(0))
            Catch
                Return 0
            End Try
        Else
            Return 0
        End If
    End Function

    Private Function GetTop() As Integer
        If mSettings.Count > 1 Then
            Try
                Return CInt(mSettings(1))
            Catch
                Return 0
            End Try
        Else
            Return 0
        End If
    End Function

    Public Function GetVisible() As Boolean
        If mSettings.Count > 2 Then
            Try
                Return CBool(CStr(mSettings(2)) = "1")
            Catch
                Return False
            End Try
        Else
            Return False
        End If
    End Function

    Protected Friend mSettings As ArrayList
    Protected Friend mWndManager As DAOCWindowManager
    Protected Friend mWindowName As String

    Protected Sub LoadSettingsLine()
        Dim s As String
        If mWindowName = "" Then Exit Sub

        With New CIniFile()
            .Path = mWndManager.SettingsFile
            s = .Value("Panels", mWindowName, "")
        End With

        mSettings = New ArrayList(Split(s, ","))
    End Sub

    Protected Sub DoLeftClick(ByVal X As Integer, ByVal Y As Integer)
        mWndManager.DoLeftClick(Me, X, Y)
    End Sub

    Protected Sub DoRightClick(ByVal X As Integer, ByVal Y As Integer)
        mWndManager.DoRightClick(Me, X, Y)
    End Sub

    Protected Sub DoSendKeys(ByVal aKeys As String)
        mWndManager.DoSendKeys(Me, aKeys)
    End Sub

    Protected Sub DoVKDown(ByVal vk As Byte)
        mWndManager.DoVKDown(Me, vk)
    End Sub

    Protected Sub DoVKUp(ByVal vk As Byte)
        mWndManager.DoVKUp(Me, vk)
    End Sub

	Public Sub New(ByVal aWndManager As DAOCWindowManager, ByVal WindowName As String)
		MyBase.new()
		mWindowName = WindowName
		mWndManager = aWndManager
		mSettings = New ArrayList
		LoadSettingsLine()
	End Sub

	Public Sub New(ByVal aWndManager As DAOCWindowManager)
		MyBase.New()
		mWindowName = ""
		mWndManager = aWndManager
		mSettings = New ArrayList
		LoadSettingsLine()
	End Sub

	Protected Overrides Sub Finalize()
		mSettings = Nothing
		MyBase.Finalize()
	End Sub

	Public ReadOnly Property WindowName() As String
		Get
			Return mWindowName
		End Get
	End Property

	Public ReadOnly Property Left() As Integer
		Get
			Return GetLeft()
		End Get
	End Property

	Public ReadOnly Property Top() As Integer
		Get
			Return GetTop()
		End Get
	End Property

	Public ReadOnly Property Visible() As Boolean
		Get
			Return GetVisible()
		End Get
	End Property
End Class

Public Enum StatsWindowPage
    swpAttributes
    swpInventory
    swpSkills
    swpCombat
    swpSpells
    swpGroup
    swpNone
End Enum

Public Class MultipleWindow
    Inherits DAOCWindow

    Private Declare Function ScreenToClient Lib "user32" Alias "ScreenToClient" (ByVal hwnd As Integer, ByRef lpPoint As Point) As Integer
    Public Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer

    Private mOriginPoint As Point
    Private mPageTopOffset As Integer = 40
    Private mPageLeftOffset As Integer = 70
    Private mPageRightOffset As Integer = 110
    Private mButtonLeftOffset As Integer = 100
    Private mAcceptButtonTopOffset As Integer = 60
    Private mCancelButtonTopOffset As Integer = 80

    Public Sub New(ByVal aWndManager As DAOCWindowManager)
        MyBase.New(aWndManager, "Multiple")
        mOriginPoint = Windows.Forms.Cursor.Position
        ScreenToClient(FindWindow("DAoCMWC", vbNullString), mOriginPoint)
        Debug.WriteLine("current position: " & mOriginPoint.X & " " & mOriginPoint.Y)
    End Sub

    Public Sub OneLess()
        doleftclick(mOriginPoint.X + mPageLeftOffset, mOriginPoint.Y + mPageTopOffset)
    End Sub

    Public Sub OneMore()
        doleftclick(mOriginPoint.X + mPageRightOffset, mOriginPoint.Y + mPageTopOffset)
    End Sub

    Public Sub Accept()
        doleftclick(mOriginPoint.X + mButtonLeftOffset, mOriginPoint.Y + mAcceptButtonTopOffset)
    End Sub

    Public Sub Cancel()
        doleftclick(mOriginPoint.X + mButtonLeftOffset, mOriginPoint.Y + mCancelButtonTopOffset)
    End Sub

End Class

Public Class StatsWindow
    Inherits DAOCWindow

    Private Function GetStartingOpenPage() As StatsWindowPage
        If mSettings.Count > 3 Then
            Try
                Return DirectCast(mSettings(3), StatsWindowPage)
            Catch
                Return StatsWindowPage.swpNone
            End Try
        Else
            Return StatsWindowPage.swpNone
        End If
    End Function

    Public Sub New(ByVal aWndManager As DAOCWindowManager)
        MyBase.New(aWndManager, "Stats")
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

        If AItem < 0 Or AItem > 7 Then Exit Sub

        DoLeftClick(Left + INV_LEFT_OFFSET, Top + INV_TOP_OFFSET + (AItem * (INV_HEIGHT)) + (INV_HEIGHT \ 2))
    End Sub

    Public Sub DelveInventoryItem(ByVal AItem As Integer)
        Const INV_LEFT_OFFSET As Integer = 20
        Const INV_TOP_OFFSET As Integer = 278
        Const INV_HEIGHT As Integer = 17

        If AItem < 0 Or AItem > 7 Then Exit Sub

        DoRightClick(Left + INV_LEFT_OFFSET, Top + INV_TOP_OFFSET + (AItem * (INV_HEIGHT)) + (INV_HEIGHT \ 2))
    End Sub

    Public Sub SelectInventoryBag(ByVal aBag As Integer)
        Dim BAG_TOP_OFFSETS() As Integer = {291, 318, 343, 368, 393}
        Const BAG_LEFT_OFFSET As Integer = 180
        If aBag < 0 Or aBag > 4 Then Exit Sub
        doleftclick(Left + BAG_LEFT_OFFSET, Top + BAG_TOP_OFFSETS(aBag))
    End Sub

    Public Sub MoveInventoryItem(ByVal AFromBag As Integer, ByVal AFromItem As Integer, ByVal AToBag As Integer, ByVal AToItem As Integer)
        ClickPage(StatsWindowPage.swpAttributes)
        Thread.CurrentThread.Sleep(500)
        ClickPage(StatsWindowPage.swpInventory)
        Thread.CurrentThread.Sleep(500)

        SelectInventoryBag(AFromBag)
        Thread.CurrentThread.Sleep(500)
        SelectInventoryItem(AFromItem)
        Thread.CurrentThread.Sleep(500)

        If AFromBag <> AToBag Then
            SelectInventoryBag(AToBag)
            Thread.CurrentThread.Sleep(500)
        End If

        SelectInventoryItem(AToItem)
        Thread.CurrentThread.Sleep(1000)

        'ClickPage(StatsWindowPage.swpInventory)
    End Sub

    Public ReadOnly Property StartingOpenPage() As StatsWindowPage
        Get
            Return GetStartingOpenPage()
        End Get
    End Property
End Class


Public Class DialogWindow
    Inherits DAOCWindow


    Public Sub New(ByVal aWndManager As DAOCWindowManager)
        MyBase.New(aWndManager, "Dialog")
    End Sub

    Public Sub ClickOK()
        doleftclick(Left + 125, Top + 85)
    End Sub

    Public Shared Sub CloseDialog(ByVal aWndManager As DAOCWindowManager)
        Dim frm As DialogWindow = New DialogWindow(aWndManager)
        frm.ClickOK()
        frm = Nothing
    End Sub

End Class

Public Class ScrollableListWindow
    Inherits DAOCWindow

    Protected Friend mPage As Integer
    Protected Friend mItem As Integer
    Protected Friend mTopItem As Integer

    Protected Friend mItemsPerPage As Integer
    Protected Friend mScrollLeftOffset As Integer
    Protected Friend mScrollUpTopOffset As Integer
    Protected Friend mScrollDownTopOffset As Integer
    Protected Friend mPageTopOffset As Integer
    Protected Friend mPageLeftLeftOffset As Integer
    Protected Friend mPageRightLeftOffset As Integer
    Protected Friend mItemTopOffset As Integer
    Protected Friend mItemLeftOffset As Integer
    Protected Friend mItemHeight As Integer
    Protected Friend mLastSelectWasIcon As Boolean

    Protected Friend Sub SelectItemCommon(ByVal Value As Integer, ByVal XOff As Integer)
        If mItem = -1 Then mItem = 0
        While Value < mTopItem
            ScrollUp()
            Thread.CurrentThread.Sleep(200)
            mTopItem -= mItemsPerPage
        End While

        If mTopItem < 0 Then mTopItem = 0

        While Value >= (mTopItem + mItemsPerPage)
            ScrollDown()
            Thread.CurrentThread.Sleep(200)
            mTopItem += mItemsPerPage
        End While

        doleftclick(Left + mItemLeftOffset + XOff, Top + mItemTopOffset + ((Value - mTopItem) * mItemHeight))
        mItem = Value
    End Sub

    Public Sub New(ByVal aWndManager As DAOCWindowManager, ByVal WindowName As String)
        MyBase.New(aWndManager, WindowName)
        mPage = -1
        mItem = -1

        mItemsPerPage = 20
        mScrollLeftOffset = 345
        mScrollUpTopOffset = 25
        mScrollDownTopOffset = 370
        mPageTopOffset = 430
        mPageLeftLeftOffset = 258
        mPageRightLeftOffset = 278
        mItemTopOffset = 25
        mItemLeftOffset = 15
        mItemHeight = 18
    End Sub

    Public Sub New(ByVal aWndManager As DAOCWindowManager)
        MyBase.New(aWndManager, "")
        mPage = -1
        mItem = -1

        mItemsPerPage = 20
        mScrollLeftOffset = 345
        mScrollUpTopOffset = 25
        mScrollDownTopOffset = 370
        mPageTopOffset = 430
        mPageLeftLeftOffset = 258
        mPageRightLeftOffset = 278
        mItemTopOffset = 25
        mItemLeftOffset = 15
        mItemHeight = 18
    End Sub

    Public Sub SetPage(ByVal Value As Integer, ByVal AssumePage0 As Boolean)
        If mPage = Value Then Exit Sub

        If mPage = -1 Then
            If AssumePage0 Then
                mPage = 0
            Else
                mPage = 5
            End If
            While mPage > 0
                PageLeft()
                Thread.CurrentThread.Sleep(200)
            End While
        End If

        While mPage <> Value
            If mPage > Value Then
                PageLeft()
            Else
                PageRight()
            End If
            Thread.CurrentThread.Sleep(200)
        End While
    End Sub

    Public Sub SelectItem(ByVal Value As Integer)
        mLastSelectWasIcon = False
        SelectItemCommon(Value, 15)
    End Sub

    Public Sub SelectItemIcon(ByVal Value As Integer)
        mLastSelectWasIcon = True
        SelectItemCommon(Value, 0)
    End Sub

    Public Sub PageLeft()
        doleftclick(Left + mPageLeftLeftOffset, Top + mPageTopOffset)
        mPage -= 1
    End Sub

    Public Sub PageRight()
        doleftclick(Left + mPageRightLeftOffset, Top + mPageTopOffset)
        mPage += 1
    End Sub

    Public Sub ScrollUp()
        doleftclick(Left + mScrollLeftOffset, Top + mScrollUpTopOffset)
    End Sub

    Public Sub ScrollDown()
        doleftclick(Left + mScrollLeftOffset, Top + mScrollDownTopOffset)
    End Sub
End Class

Public Class VendorWindow
    Inherits ScrollableListWindow

    Public Sub New(ByVal aWndManager As DAOCWindowManager)
        MyBase.New(aWndManager, "Train")

        mPage = -1
        mItem = -1
    End Sub

    Public Sub Appraise()
        doleftclick(Left + 180, Top + 430)
    End Sub

    Public Sub Buy()
        DoLeftClick(Left + 41, Top + 430)
    End Sub

    Public Sub BuyMultiple(ByVal AQuantity As Integer)
        If Not mLastSelectWasIcon Then
            SelectItemIcon(mItem)
        End If
        dosendkeys("/mbuy " & CInt(AQuantity) & vbCr)
    End Sub
End Class

Public Enum QuickbarOrientation
    qboVertical
    qboHorizontal
    qboClosed
End Enum

Public Class Quickbar
    Inherits DAOCWindow

    Public Sub New(ByVal aWndManager As DAOCWindowManager)
        MyBase.New(aWndManager, "QuickBar")
        GetStartingOrientation()
    End Sub

    Private mOrientation As QuickbarOrientation

    Private Sub GetStartingOrientation()
        If mSettings.Count > 3 Then
            Try
                mOrientation = DirectCast(mSettings(3), QuickbarOrientation)
            Catch
                mOrientation = QuickbarOrientation.qboClosed
            End Try
        Else
            mOrientation = QuickbarOrientation.qboClosed
        End If
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
        If ASlot < 1 Or ASlot > 10 Then Exit Sub
        SlotXY(ASlot, X, Y)
        DoLeftClick(X, Y)
    End Sub

    Public Sub ClearSlot(ByVal ASlot As Integer)
        Dim X, Y As Integer
        If ASlot < 1 Or ASlot > 10 Then Exit Sub
        SlotXY(ASlot, X, Y)

        'DoVKDown(VK_SHIFT)
        Thread.CurrentThread.Sleep(200)
        DoRightClick(X, Y)
        Thread.CurrentThread.Sleep(200)
        'DoVKUp(VK_SHIFT)
    End Sub
End Class

'Public Class TradeRecipeWindow
'    Inherits ScrollableListWindow

'    Public Sub New(ByVal aWndManager As DAOCWindowManager)
'		MyBase.New(aWndManager, "TradeSkillWindow")

'        mItemsPerPage = 21
'        mScrollLeftOffset = 295
'        mExpandedGroupIdx = -1
'	End Sub

'	Private mExpandedGroupIdx As Integer

'	Public Sub ExpandGroup(ByVal aItem As TradeSkillRecipe, ByVal aCraft As CraftRecipeCollection, ByVal aCurrentSkill As Integer)
'		Dim iGroupIdx As Integer
'		Dim iVisibleItemCount As Integer

'		'iGroupIdx = aCraft.OrdinalOfGroup(aItem.Group)
'		If iGroupIdx <> -1 Then
'			mItemLeftOffset = 0
'			SelectItem(iGroupIdx)
'			mExpandedGroupIdx = iGroupIdx

'			'iVisibleItemCount = aCraft.VisibleRecipesInGroup(aItem.Group, aCurrentSkill)
'			If (mExpandedGroupIdx + mTopItem) + iVisibleItemCount >= mItemsPerPage Then
'				mTopItem = mExpandedGroupIdx + iVisibleItemCount + 1 - mItemsPerPage
'				Thread.CurrentThread.Sleep(200)
'			End If
'		End If
'	End Sub

'	Public Sub ClickRecipe(ByVal aItem As TradeSkillRecipe, ByVal aCraft As CraftRecipeCollection, ByVal aCurrentSkill As Integer)
'		Dim iGroupIdx As Integer
'		Dim iTierIdx As Integer

'		ExpandGroup(aItem, aCraft, aCurrentSkill)
'		Thread.CurrentThread.Sleep(500)

'		'iGroupIdx = acraft.ordinalOfGroup(AItem.group0
'		If iGroupIdx = -1 Then Exit Sub
'		'iTierIdx = aCraft.OrdinalOfTierInGroup(aItem.Group, aItem.Tier)
'		If iTierIdx = -1 Then Exit Sub
'		mItemLeftOffset = 25
'		SelectItem(iGroupIdx + 1 + iTierIdx)
'	End Sub

'End Class
