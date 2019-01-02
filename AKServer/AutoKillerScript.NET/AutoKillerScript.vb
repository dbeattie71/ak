#Region "Imports"
Imports System.Reflection
Imports Microsoft.Win32
Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Threading
Imports System.Net
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Imports AKServer.DLL.DAoCServer
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Ipc
Imports System.Collections.Generic
#End Region
Public NotInheritable Class clsAutoKillerScript

#Region "Declares"
    Private Declare Function SendMessage Lib "user32.dll" Alias "SendMessageA" (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Private Declare Function PostMessage Lib "user32.dll" Alias "PostMessageA" (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Private Declare Function TerminateProcess Lib "kernel32" (ByVal hProcess As Integer, ByVal uExitCode As Integer) As Integer
    Private Declare Function ClientToScreen Lib "user32" Alias "ClientToScreen" (ByVal hwnd As Integer, ByRef lpPoint As Point) As Integer
    Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
    Private Declare Function GetWindowThreadProcessId Lib "user32" (ByVal hwnd As Integer, ByRef lpdwProcessId As Integer) As Integer
    Private Declare Function WriteProcessMemory Lib "kernel32" (ByVal hProcess As Integer, ByVal lpBaseAddress As Integer, ByVal lpBuffer As IntPtr, ByVal nSize As Integer, ByRef lpNumberOfBytesWritten As Integer) As Integer
    Private Declare Sub mouse_event Lib "user32" (ByVal dwFlags As Integer, ByVal dx As Integer, ByVal dy As Integer, ByVal cbuttons As Integer, ByVal dwExtraInfo As Integer)
    Private Declare Function DeclareBeep Lib "kernel32" Alias "Beep" (ByVal dwFreq As Integer, ByVal dwDuration As Integer) As Integer
    Private Declare Function OpenProcess Lib "kernel32" (ByVal dwDesiredAccess As Integer, ByVal blnheritHandle As Integer, ByVal dwAppProcessId As Integer) As Integer
    Private Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Integer) As Integer
    Private Declare Function ReadProcessMemory Lib "kernel32" (ByVal hProcess As Integer, ByVal lpBaseAddress As Integer, ByVal lpBuffer As IntPtr, ByVal nSize As Integer, ByRef lpNumberOfBytesWritten As Integer) As Integer
#End Region

#Region "Constants"
    ' Other constants
    Private Const LogFile As String = "AutoKillerScript.log"
    ' Global Memory Flags
    Private Const MOUSEEVENTF_LEFTDOWN As Short = &H2S
    Private Const MOUSEEVENTF_LEFTUP As Short = &H4S
    Private Const MOUSEEVENTF_RIGHTDOWN As Short = &H8S
    Private Const MOUSEEVENTF_RIGHTUP As Short = &H10S
    Private Const PROCESS_ALL_ACCESS As Integer = &H1F0FFF

#End Region

#Region "Memory Variables"
    Private LocalPlayerInfo As Integer
    Private RunningAddress As Integer
    Private TargetIndexAddress As Integer
#End Region

#Region "Classes"
    <Serializable()> _
    Public NotInheritable Class MobListMob
        Private _MobName As String = Nothing
        Private _Aggro As Boolean = False
        Public Sub New()
        End Sub

        Public Sub New(ByVal name As String, ByVal aggro As Boolean)
            _MobName = name
            _Aggro = aggro
        End Sub

        Public Property MobName() As String
            Get
                Return _MobName
            End Get
            Set(ByVal Value As String)
                _MobName = Value
            End Set
        End Property

        Public Property Aggro() As Boolean
            Get
                Return _Aggro
            End Get
            Set(ByVal Value As Boolean)
                _Aggro = Value
            End Set
        End Property
    End Class

    <Serializable()> _
    Public NotInheritable Class MobListCollection
        Implements ICollection
        Private _moblist As List(Of MobListMob)
        Public Sub New()
            _moblist = New List(Of MobListMob)
        End Sub

        Public Sub Add(ByVal value As MobListMob)
            If value Is Nothing Then
                Throw New ArgumentException("value can not be nothing")
            End If

            _moblist.Add(value)

        End Sub

        Public Function Add(ByVal name As String, ByVal aggro As Boolean) As MobListMob
            If name Is Nothing OrElse name.Length = 0 Then
                Throw New ArgumentException("MustHaveValidName")
            End If
            If (Me(name) IsNot Nothing) Then
                Throw New ArgumentException(String.Format("NameAlreadyExists"), name)
            End If

            Dim mob As MobListMob = New MobListMob(name, aggro)
            _moblist.Add(mob)
            Return mob
        End Function

        Public Sub Clear()
            _moblist.Clear()
        End Sub

        Public Sub Remove(ByVal obj As MobListMob)
            _moblist.Remove(obj)
        End Sub

        Public Sub Remove(ByVal Index As Integer)
            Remove(_moblist(Index))
        End Sub

        Public Sub Remove(ByVal Name As String)
            For Each m As MobListMob In _moblist
                If m.MobName.ToLower = Name.ToLower Then
                    Remove(m)
                    Exit For
                End If
            Next
        End Sub

        Default Public ReadOnly Property Item(ByVal index As Integer) As MobListMob
            Get
                If (_moblist IsNot Nothing) Then
                    Return CType((_moblist(index)), MobListMob)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Default Public ReadOnly Property Item(ByVal name As String) As MobListMob
            Get
                If (_moblist IsNot Nothing) Then
                    For i As Integer = 0 To _moblist.Count - 1
                        If name.ToLower = CType(_moblist(i), MobListMob).MobName.ToLower Then
                            Return CType((_moblist(i)), MobListMob)
                        End If
                    Next
                End If
                Return Nothing
            End Get
        End Property

        Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
            Dim e As IEnumerator = Me.GetEnumerator
            While e.MoveNext
                array.SetValue(e.Current, index - 1)
            End While
        End Sub
        Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
            Get
                If (_moblist IsNot Nothing) Then
                    Return _moblist.Count
                Else
                    Return 0
                End If
            End Get
        End Property

        Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
            Get
                Return False
            End Get
        End Property

        Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
            Get
                Return Me
            End Get
        End Property

        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            If _moblist Is Nothing Then
                _moblist = New List(Of MobListMob)
            End If
            Return _moblist.GetEnumerator()
        End Function
    End Class

    Public NotInheritable Class AutoKillerException
        Inherits Exception
        Private _AKException As Exception
        Public Sub New()

        End Sub

        Public Sub New(ByVal ex As Exception)
            _AKException = ex
        End Sub

        Public Property AKException() As Exception
            Get
                Return _AKException
            End Get
            Set(ByVal Value As Exception)
                _AKException = Value
            End Set
        End Property
    End Class

    Public Enum InjectType
        SETINDEX = 1100
        FINDWINDOW = 1101
        SETTEXT = 1102
    End Enum

    'Public Enum NPC_TYPE
    '    Item = 0
    '    NPC = 2
    '    PC = 4
    '    Dead = 7
    '    FILTERED = 99
    'End Enum
    Public Enum oEquipment
        oeRightHand = 0
        oeLeftHand = 1
        oeTwoHanded = 2
        oeRanged = 3
        oeHelm = 11
        oeHands = 12
        oeBoots = 13
        oeJewelry = 14
        oeVest = 15
        oeCloak = 16
        oeLeggings = 17
        oeSleeves = 18
        oeNecklace = 19
        oeBelt = 22
        oeLeftWrist = 23
        oeRightWrist = 24
        oeLeftRing = 25
        oeRightRing = 26
    End Enum

    <StructLayout(LayoutKind.Sequential)> Private NotInheritable Class PlayerInfo
        Private mDirection As Integer
        Private mX As Single
        Private mY As Single
        Private mZ As Single

        Public Property Direction() As Integer
            Get
                Return mDirection
            End Get
            Set(ByVal Value As Integer)
                mDirection = Value
            End Set
        End Property

        Public Property X() As Integer
            Get
                Return CInt(mX)
            End Get
            Set(ByVal Value As Integer)
                mX = Value
            End Set
        End Property

        Public Property Y() As Integer
            Get
                Return CInt(mY)
            End Get
            Set(ByVal Value As Integer)
                mY = Value
            End Set
        End Property

        Public Property Z() As Integer
            Get
                Return CInt(mZ)
            End Get
            Set(ByVal Value As Integer)
                mZ = Value
            End Set
        End Property
    End Class

    <StructLayout(LayoutKind.Sequential)> Private Class RouteStruct
        Public xPos As Single
        Public yPos As Single
        Public Range As Integer
        Public action As Byte
    End Class

    Public Class AutokillerQueryEventParams
        Inherits EventArgs
        Public QueryID As Integer
        Public Logline As String
        Public Sub New(ByVal aQueryID As Integer, ByVal aMessage As String)
            QueryID = aQueryID
            Logline = aMessage
        End Sub
    End Class

    Public Class AutokillerRegExEventParams
        Inherits EventArgs
        Public QueryID As Integer
        Public Logline As String
        Public RegExMatch As Match
        Public Sub New(ByVal aQueryID As Integer, ByVal aMessage As String, ByVal aMatch As Match)
            QueryID = aQueryID
            Logline = aMessage
            RegExMatch = aMatch
        End Sub
    End Class

#End Region

#Region "Variables 2"
    ' Global Private Variables
    Private callback As OurCallBack
    Private packets As DAOCMain
    Private channel As IChannel

    Private FilterItem As New List(Of String)
    Private MobsToKill As New MobListCollection
    Private TerminateThread As Boolean
    Private Running As Boolean
    Private hInst As Integer
    Private mhdl As Integer
    Private AKHwnd As Integer
    Private GamePID As Integer
    Private mlngPlayerIndex As Integer
    Private mFilePos As Long
    Private mFileLastPos As Long
    Private mToA As Boolean
    Private mCAT As Boolean
    Private mDR As Boolean
    Private mAutoQuery As Boolean
    Private mUseRegEx As Boolean
    Private mTriggerStrings() As String
    Private mTriggerTable() As Boolean
    Private mRouteLength As Integer
    Private mRouteTable() As RouteStruct
    Private mEuro As Boolean
    Private mChatLog As String
    Private mGamePath As String
    Private mRegKey As String
    Private pi As New PlayerInfo

    Private ValidClosestMob As Hashtable
    Private ValidInvader As Hashtable
    Private ValidMobInCombat As Hashtable
    Private ValidSearchMob As Hashtable
    Private ValidMobWithPlayerAsTarget As Hashtable
    Private ValidObject As Hashtable

    Private ret As Integer

    Private mLeftTurnKey As Byte
    Private mRightTurnKey As Byte
    Private mConsiderKey As Byte
    Private QueryThread As Thread
    Private mUserProcessID As Integer
#End Region

#Region "Events"
    Public Event OnLog(ByVal LogString As String)
    Public Event OnFinishDoInit(ByVal Success As Boolean)
    Public Event OnQueryStringTrue(ByVal e As AutokillerQueryEventParams)
    Public Event OnRegExTrue(ByVal e As AutokillerRegExEventParams)

    Public Event OnPlayerQuit()
    Public Event OnSpellEffectAnimation(ByVal Sender As Object, ByVal e As DAOCEventArgs)
    Public Event OnSpellCast(ByVal Sender As Object, ByVal e As DAOCEventArgs)
    Public Event OnPetWindowUpdate(ByVal Sender As Object, ByVal e As DAOCEventArgs)
    Public Event OnMobCreation(ByVal Sender As Object, ByVal e As DAOCEventArgs)
    Public Event OnProgressMeter(ByVal Sender As Object, ByVal e As LogUpdateEventArgs)
    Public Event OnDialog(ByVal Sender As Object, ByVal e As LogUpdateEventArgs)
    Public Event OnZoneChange(ByVal ZoneID As Integer)
#End Region

#Region "Properties"
#Region "Merchant"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Page"></param>
    ''' <param name="Item"></param>
    ''' <value></value>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public ReadOnly Property MerchantItem(ByVal Page As Integer, ByVal Item As Integer) As DAOCVendorItem
        Get
            Try
                If Page < 1 OrElse Item < 1 OrElse Item > 30 OrElse hInst = 0 Then
                    Return Nothing
                End If

                Return packets.VendorItems.MerchantItem(Page, Item)

            Catch ex As Exception
                Throw New AutoKillerException(ex)
            Finally

            End Try
        End Get
    End Property

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public ReadOnly Property MerchantItems() As DAOCVendorItemList
        Get
            Return packets.VendorItems
        End Get
    End Property
#End Region

#Region "Inventory"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public WriteOnly Property FilterItems() As List(Of String)
        Set(ByVal Value As List(Of String))
            FilterItem = Value
        End Set
    End Property

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Index"></param>
    ''' <value></value>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public ReadOnly Property InventoryItem(ByVal Index As Byte) As DAOCInventoryItem
        Get
            Try
                If Index < 0 OrElse Index > 39 Then
                    Return Nothing
                End If
                Return packets.Player.Inventory.ItemInSlot(CByte(Index + 40))
            Catch ex As Exception
                Throw New AutoKillerException(ex)
            Finally
            End Try
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property InventoryItems() As DAOCInventory
        Get
            Return packets.Player.Inventory
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Location"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Equipment(ByVal Location As oEquipment) As DAOCInventoryItem
        Get
            Try
                Return packets.Player.Inventory.ItemInSlot(CByte(Location))
            Catch ex As Exception
                Throw New AutoKillerException(ex)
            Finally

            End Try
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="index"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property VaultItem(ByVal index As Integer) As DAOCInventoryItem
        Get
            Try
                If index < 0 OrElse index > 39 Then
                    Return Nothing
                End If
                Return packets.Player.Inventory.ItemInSlot(CByte(index + 110))
            Catch ex As Exception
                Throw New AutoKillerException(ex)
            Finally

            End Try
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property VaultItems() As DAOCInventory
        Get
            Try
                Return packets.Player.Inventory
            Catch ex As Exception
                Throw New AutoKillerException(ex)
            End Try
        End Get
    End Property
#End Region

#Region "Player"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property PlayerName() As String
        Get
            Return packets.Player.Name
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property PlayerClass() As String
        Get
            Return packets.Player.Name 'TODO FIX
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property PlayerRace() As String
        Get
            Return packets.Player.Name 'TODO FIX
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property PlayerBaseClass() As String
        Get
            Return packets.Player.Name 'TODO FIX
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property PlayerGuild() As String
        Get
            Return packets.Player.Name 'TODO FIX
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property PlayerLevel() As Integer
        Get
            Return packets.Player.Level
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="i"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property oCraftSkill(ByVal i As Integer) As DAOCNameValuePair
        Get
            Try
                Return packets.Player.Skills.Item(i)
            Catch ex As Exception
                Throw New AutoKillerException(ex)
            End Try
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property CraftSkills() As DAOCNameValueList
        Get
            Return packets.Player.Skills
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="i"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property oPlayerSkill(ByVal i As Integer) As DAOCNameValuePair
        Get
            Try
                Return packets.Player.Skills(i)
            Catch ex As Exception
                Throw New AutoKillerException(ex)
            End Try
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property PlayerSkills() As DAOCNameValueList
        Get
            Return packets.Player.Skills
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property PlayerCasting() As Short '0 not casting 255 casting
        Get
            Try
                Return 0 'TODO FIX
                'Return packets.Player.
            Catch ex As Exception
                Throw New AutoKillerException(ex)
            End Try
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property gPlayerXCoord() As Integer
        Get
            Try
                'Debug.WriteLine(packets.Player.GetXProjected.ToString)
                pi = DirectCast(ReadMemory(LocalPlayerInfo, pi), PlayerInfo)
                Return pi.X
                'Return packets.Player.GetXProjected
            Catch ex As Exception
                LogF("gPlayerXCoord " & ex.Message)
                Throw New AutoKillerException(ex)
            End Try
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property gPlayerYCoord() As Integer
        Get
            Try
                pi = DirectCast(ReadMemory(LocalPlayerInfo, pi), PlayerInfo)
                Return pi.Y
                'Return packets.Player.Y
            Catch ex As Exception
                LogF("gPlayerYCoord " & ex.Message)
                Throw New AutoKillerException(ex)
            End Try
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property gPlayerZCoord() As Integer
        Get
            Try
                pi = DirectCast(ReadMemory(LocalPlayerInfo, pi), PlayerInfo)
                Return pi.Z
                'Return packets.Player.Z
            Catch ex As Exception
                LogF("gPlayerZCoord " & ex.Message)
                Throw New AutoKillerException(ex)
            End Try
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property PlayerDir() As Short
        Get
            Try
                pi = DirectCast(ReadMemory(LocalPlayerInfo, pi), PlayerInfo)
                Return CShort(((pi.Direction / 4096 * 360) + 180) Mod 360)
                'Return packets.Player.Direction()
            Catch ex As Exception
                LogF(ex.Message)
                Throw New AutoKillerException(ex)
            End Try
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property zPlayerXCoord() As Integer
        Get
            pi = DirectCast(ReadMemory(LocalPlayerInfo, pi), PlayerInfo)
            Return packets.GlobalToZoneX(pi.X)
            'Return packets.Player.X - packets.Zone.BaseLoc.X
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property zPlayerYCoord() As Integer
        Get
            pi = DirectCast(ReadMemory(LocalPlayerInfo, pi), PlayerInfo)
            Return packets.GlobalToZoneY(pi.Y)
            'Return packets.Player.Y
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property zPlayerZCoord() As Integer
        Get
            pi = DirectCast(ReadMemory(LocalPlayerInfo, pi), PlayerInfo)
            Return pi.Z
            'Return packets.Player.Z
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property PlayerHealth() As Byte
        Get
            Return packets.Player.Health
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property PlayerMana() As Byte
        Get
            Return packets.Player.Mana
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property PlayerStamina() As Byte
        Get
            Return packets.Player.Stamina
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property isPlayerSitting() As Boolean
        Get
            Return packets.Player.IsSitting
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property isPlayerInCombat() As Boolean
        Get
            Return packets.Player.Combat
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property isPlayerStealthed() As Boolean
        Get
            Return packets.Player.Stealthed
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsPlayerDead() As Boolean
        Get
            Return packets.Player.ImDead
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property PlayerID() As Integer
        Get
            Return packets.Player.SpawnID
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property gtXCoord() As Integer
        Get
            Return packets.GroundTarget.X
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property gtYCoord() As Integer
        Get
            Return packets.GroundTarget.Y
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property gtZCoord() As Integer
        Get
            Return packets.GroundTarget.Z
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property GetPlayerINI() As String
        Get
            Dim f As New DirectoryInfo(mGamePath)
            Dim fi() As FileInfo = f.GetFiles(packets.Player.Name & "*.ini")
            If fi.Length > 0 Then
                Return fi(0).Name
            Else
                Return String.Empty
            End If
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property GetOKtoCast() As Boolean
        Get

        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LocalBufs() As LocalBuffList
        Get
            Return packets.LocalBufs
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="BufIndex"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LocalBuf(ByVal BufIndex As Integer) As LocalBuff
        Get
            Return packets.LocalBufs.BuffTable(BufIndex)
        End Get
    End Property

    ''' <summary>
    '''  Returns 0 if player is not stuck or following.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property isPlayerStuck() As Boolean
        Get
            Try
                Dim ReadPtr As IntPtr = Marshal.AllocHGlobal(2)
                ReadProcessMemory(hInst, TargetIndexAddress + 16, ReadPtr, 2, 0)

                Dim stuck As Short = Marshal.ReadInt16(ReadPtr, 0)
                If stuck = 0 Then 'if 0 then not stuck or following
                    Return False
                Else
                    Return True
                End If
            Catch ex As Exception
                LogF(ex.Message)
                Throw New AutoKillerException(ex)
            End Try
        End Get
    End Property

    ''' <summary>
    '''  Returns the player object from AKServer
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Player() As DAOCLocalPlayer
        Get
            Return packets.Player
        End Get
    End Property

    ''' <summary>
    ''' id player is casting returns true
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property isPlayerCasting() As Boolean
        Get
            Return packets.Player.IsCasting
        End Get
    End Property

    Public ReadOnly Property PlayerLeftHand() As WeaponSlots
        Get
            Return CType(packets.Player.LeftHand, WeaponSlots)
        End Get
    End Property

    Public ReadOnly Property PlayerRightHand() As WeaponSlots
        Get
            Return CType(packets.Player.RightHand, WeaponSlots)
        End Get
    End Property
#End Region

#Region "Mob"
    ''' <summary>
    ''' Returns the objectlist from AKServer
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property DAOCObjects() As DAOCObjectlist
        Get
            Return packets.DAOCObjects
        End Get
    End Property

    ''' <summary>
    ''' This list is used internally when FindClosestMob is called. It basically will
    '''  search for only the mobs indicated.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MobList() As MobListCollection
        Get
            Try
                Return MobsToKill
            Catch ex As Exception
                LogF(ex.Message)
                Throw New AutoKillerException(ex)
            End Try
        End Get
        Set(ByVal Value As MobListCollection)
            MobsToKill = Value
        End Set
    End Property


    ''' <summary>
    ''' Returns the direction of the mob at the index specified.
    ''' </summary>
    ''' <param name="spawnID"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MobDir(ByVal spawnID As Integer) As Short
        Get
            If packets.DAOCObjects.ObjectTable.ContainsKey(spawnID) Then
                Return CShort(packets.DAOCObjects.ObjectTable.Item(spawnID).Direction)
            End If
        End Get
    End Property

    ''' <summary>
    ''' Returns the object x-coordinate in global format.
    ''' </summary>
    ''' <param name="spawnID"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MobXCoord(ByVal spawnID As Integer) As Integer
        Get 'global x
            If packets.DAOCObjects.ObjectTable.ContainsKey(spawnID) Then
                Dim obj As DAOCObject = packets.DAOCObjects.ObjectTable.Item(spawnID)
                If TypeOf obj Is DAOCMovingObject Then
                    With DirectCast(obj, DAOCMovingObject)
                        Return .GetXProjected
                    End With
                Else
                    Return obj.X
                End If
            End If
        End Get
    End Property

    ''' <summary>
    ''' Returns the object y-coordinate in global format.
    ''' </summary>
    ''' <param name="spawnID"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MobYCoord(ByVal spawnID As Integer) As Integer
        Get 'global y
            If packets.DAOCObjects.ObjectTable.ContainsKey(spawnID) Then
                Dim obj As DAOCObject = packets.DAOCObjects.ObjectTable.Item(spawnID)
                If TypeOf obj Is DAOCMovingObject Then
                    With DirectCast(obj, DAOCMovingObject)
                        Return .GetYProjected
                    End With
                Else
                    Return obj.Y
                End If
            End If
        End Get
    End Property

    ''' <summary>
    ''' Returns the object z-coordinate in global format.
    ''' </summary>
    ''' <param name="spawnID"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MobZCoord(ByVal spawnID As Integer) As Integer
        Get
            If packets.DAOCObjects.ObjectTable.ContainsKey(spawnID) Then
                Return packets.DAOCObjects.ObjectTable.Item(spawnID).Z
            End If
        End Get
    End Property

    ''' <summary>
    ''' Returns the object's health at the index as a percentage from 0 - 100.
    ''' </summary>
    ''' <param name="spawnID"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MobHealth(ByVal spawnID As Integer) As Byte
        Get
            If packets.DAOCObjects.ObjectTable.ContainsKey(spawnID) Then
                Return packets.DAOCObjects.ObjectTable.Item(spawnID).Health()
            End If
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="spawnID"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MobSpeed(ByVal spawnID As Integer) As Integer
        Get
            If packets.DAOCObjects.ObjectTable.ContainsKey(spawnID) Then
                Return TryCast(packets.DAOCObjects.ObjectTable.Item(spawnID), DAOCMovingObject).Speed
            End If
        End Get
    End Property

    ''' <summary>
    ''' Returns true if a player is casting, only works with players
    ''' </summary>
    ''' <param name="spawnID"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property isMobCasting(ByVal spawnID As Integer) As Boolean
        Get
            Dim pDAOCObject As DAOCObject = packets.DAOCObjects.FindBySpawnID(spawnID)

            If (pDAOCObject IsNot Nothing) AndAlso (TypeOf pDAOCObject Is DAOCMovingObject) Then
                With DirectCast(pDAOCObject, DAOCMovingObject)
                    Return .IsCasting
                End With
            End If
        End Get
    End Property

    ''' <summary>
    ''' Returns the object's level at the index.
    ''' </summary>
    ''' <param name="spawnID"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MobLevel(ByVal spawnID As Integer) As Integer
        Get
            If packets.DAOCObjects.ObjectTable.ContainsKey(spawnID) Then
                Return packets.DAOCObjects.ObjectTable.Item(spawnID).Level
            End If
        End Get
    End Property

    ''' <summary>
    ''' Returns 1 if the object at the index is in combat mode, 0 otherwise.
    ''' </summary>
    ''' <param name="spawnID"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property isMobInCombat(ByVal spawnID As Integer) As Boolean
        Get
            If packets.DAOCObjects.ObjectTable.ContainsKey(spawnID) Then
                Return packets.DAOCObjects.ObjectTable.Item(spawnID).Combat
            End If
        End Get
    End Property

    ''' <summary>
    ''' Returns the object's name at the index.
    ''' </summary>
    ''' <param name="spawnID"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MobName(ByVal spawnID As Integer) As String
        Get
            If packets.DAOCObjects.ObjectTable.ContainsKey(spawnID) Then
                Return packets.DAOCObjects.ObjectTable.Item(spawnID).Name()
            End If
            Return String.Empty
        End Get
    End Property

    ''' <summary>
    ''' Returns the object's type.
    ''' Public Enum DAOCObjectClass
    '''    ocUnknown
    '''    ocObject
    '''    ocMob
    '''    ocPlayer
    '''    ocLocalPlayer
    '''    ocVehicle
    ''' End Enum
    ''' </summary>
    ''' <param name="spawnID"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property QueryNPC(ByVal spawnID As Integer) As DAOCObjectClass
        Get
            If packets.DAOCObjects.ObjectTable.ContainsKey(spawnID) Then
                Return packets.DAOCObjects.ObjectTable.Item(spawnID).GetObjectClass
            End If
        End Get
    End Property

    ''' <summary>
    ''' Returns 1 if dead 0 otherwise.
    ''' </summary>
    ''' <param name="spawnID"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsMobDead(ByVal spawnID As Integer) As Boolean
        Get
            If packets.DAOCObjects.ObjectTable.ContainsKey(spawnID) Then
                Return packets.DAOCObjects.ObjectTable.Item(spawnID).Dead
            End If
        End Get
    End Property

    ''' <summary>
    ''' Returns the spawnID of the mobs target. Good way to detect aggro
    ''' </summary>
    ''' <param name="spawnID"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MobTarget(ByVal spawnID As Integer) As Integer
        Get
            If packets.DAOCObjects.ObjectTable.ContainsKey(spawnID) Then
                Return packets.DAOCObjects.ObjectTable.Item(spawnID).TargetID
            End If
        End Get
    End Property

    ''' <summary>
    ''' Mob's x-coordinate localized for the current zone.
    ''' </summary>
    ''' <param name="spawnID"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property zMobXCoord(ByVal spawnID As Integer) As Integer
        Get
            If packets.DAOCObjects.ObjectTable.ContainsKey(spawnID) Then
                Dim obj As DAOCObject = packets.DAOCObjects.ObjectTable.Item(spawnID)
                If TypeOf obj Is DAOCMovingObject Then
                    With DirectCast(obj, DAOCMovingObject)
                        Return packets.GlobalToZoneX(.GetXProjected)
                    End With
                Else
                    Return packets.GlobalToZoneX(obj.X)
                End If
            End If
        End Get
    End Property

    ''' <summary>
    ''' Mob's y-coordinate localized for the current zone.
    ''' </summary>
    ''' <param name="spawnID"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property zMobYCoord(ByVal spawnID As Integer) As Integer
        Get
            If packets.DAOCObjects.ObjectTable.ContainsKey(spawnID) Then
                Dim obj As DAOCObject = packets.DAOCObjects.ObjectTable.Item(spawnID)
                If TypeOf obj Is DAOCMovingObject Then
                    With DirectCast(obj, DAOCMovingObject)
                        Return packets.GlobalToZoneY(.GetYProjected)
                    End With
                Else
                    Return packets.GlobalToZoneY(obj.Y)
                End If
            End If
        End Get
    End Property
#End Region

#Region "Zone"
    ''' <summary>
    '''  Returns the ID number of the player's current zone.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ZoneID() As Integer
        Get
            Return packets.Zone.ZoneNum
        End Get
    End Property

    ''' <summary>
    ''' Returns the name of the player's current zone.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ZoneName() As String
        Get
            Return packets.Zone.Name
        End Get
    End Property

    ''' <summary>
    ''' access to the full zone info.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Zone() As DAOCZoneInfo
        Get
            Return packets.Zone
        End Get
    End Property

    Private Sub ZoneHandler(ByVal ZoneID As Integer)
        RaiseEvent OnZoneChange(ZoneID)
    End Sub

    Public Function GlobalToZoneX(ByVal x As Integer) As Integer
        Return packets.GlobalToZoneX(x)
    End Function

    Public Function GlobalToZoneY(ByVal y As Integer) As Integer
        Return packets.GlobalToZoneY(y)
    End Function
#End Region

#Region "Group"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ConcBufs() As ConcentrationBuffList
        Get
            Return packets.ConcentrationBufs
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="BufIndex"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Buf(ByVal BufIndex As Integer) As LocalBuff
        Get
            Try
                Return packets.LocalBufs.BuffTable(BufIndex)
            Catch ex As Exception
                Throw New AutoKillerException(ex)
            Finally

            End Try
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property GroupMemberInfo() As AKServer.DLL.DAoCServer.Group
        Get 'returns the whole group in an arraylist
            Return packets.Group
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="i"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property PlayerInAGroupClass(ByVal i As Short) As String
        Get
            If i < 1 OrElse i > 8 Then
                LogF(("Error in PlayerInAGroupClass"))
                Return String.Empty
                Exit Property
            End If
            Return GroupMemberInfo.GroupMemberTable(i).Class
        End Get
    End Property

    ''' <summary>
    ''' Returns health of a player in a group works with scripts 1 - 8. 
    ''' </summary>
    ''' <param name="i"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property PlayerInAGroupHealth(ByVal i As Short) As Byte
        Get
            If i < 1 OrElse i > 8 Then
                LogF(("Error in PlayerInAGroupHealth"))
                Exit Property
            End If
            Return GroupMemberInfo.GroupMemberTable(i).Health
        End Get
    End Property

    ''' <summary>
    ''' Returns a player in a group name works with scripts 1-8
    ''' </summary>
    ''' <param name="i"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property PlayerInAGroupName(ByVal i As Short) As String
        Get
            If i < 1 OrElse i > 8 Then
                LogF(("Error in PlayerInAGroupName"))
                Return String.Empty
                Exit Property
            End If
            Return GroupMemberInfo.GroupMemberTable(i).Name
        End Get
    End Property
#End Region

#Region "General"
    ''' <summary>
    ''' Returns if the object is in the ObjectTable.
    ''' </summary>
    ''' <param name="spawnID"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property DoesObjectExist(ByVal spawnID As Integer) As Boolean
        Get
            Return packets.DAOCObjects.ObjectTable.ContainsKey(spawnID)
        End Get
    End Property

    ''' <summary>
    ''' Returns the version of the DLL.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property getVersion() As String
        Get
            Dim Version As String

            Version = (FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FileMajorPart.ToString)
            Version &= "." & (FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FileMinorPart)
            Version &= "." & (FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FileBuildPart)
            Return Version
        End Get
    End Property

    ''' <summary>
    ''' Returns the current target spawnID. 0 if nothing targetted
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property TargetIndex() As Integer
        Get
            Return packets.SelectedID
        End Get
    End Property

    ''' <summary>
    ''' Returns the current target object.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property TargetObject() As DAOCObject
        Get
            Return packets.SelectedObject
        End Get
    End Property

    ''' <summary>
    ''' Returns the number of waypoints in the loaded route.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property RouteLength() As Integer
        Get
            RouteLength = mRouteLength
        End Get
    End Property

    ''' <summary>
    ''' Returns an array of ID's of coins or magic items within ObjDistance of player. Use SetTarget then consider to select the target.
    ''' </summary>
    ''' <param name="ObjDistance"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property GetClosestObject(ByVal ObjDistance As Integer) As ArrayList
        Get 'returns ID's of coins or magic items

            Dim ObjectID As New ArrayList
            Dim TempName As String

            Try
                For Each Obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
                    TempName = Obj.Name.ToLower
                    If Obj.GetObjectClass = DAOCObjectClass.ocObject AndAlso ZDistance(gPlayerXCoord, gPlayerYCoord, gPlayerZCoord, CDbl(Obj.X), CDbl(Obj.Y), CDbl(Obj.Z)) <= ObjDistance Then

                        If Obj.Name = "bag of coins" OrElse Obj.Name = "some copper coins" OrElse Obj.Name < TempName Then
                            ObjectID.Add(Obj.SpawnID)
                        End If
                    End If
                Next

                Return ObjectID
            Catch ex As Exception
                LogF(ex.Message)
                Throw New AutoKillerException(ex)
            End Try
        End Get
    End Property

    ''' <summary>
    ''' Returns an array of slots 0-39 of what not to sell. For example, if a magic item is in slot 5, it will return 5.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property GetInventoryToKeep() As ArrayList
        Get
            Dim InventoryToKeep As New ArrayList

            Try
                For Each obj As DAOCInventoryItem In packets.Player.Inventory
                    If obj.isInBag Then
                        If obj.Description < obj.Description.ToLower Then
                            InventoryToKeep.Add(obj.Description)
                        End If
                    End If
                Next
                Return InventoryToKeep
            Catch ex As Exception
                LogF(ex.Message)
                Throw New AutoKillerException(ex)
            End Try
        End Get
    End Property

    ''' <summary>
    ''' Returns the game.dll process ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property GameProcess() As Integer
        Get
            GameProcess = GamePID
        End Get
    End Property
#End Region

#Region "Write"
    ''' <summary>
    ''' Sets the path to the chat log.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property ChatLog() As String
        Set(ByVal Value As String)
            If Not Value.Length = 0 Then
                mChatLog = Value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Sets the path to the game path. Used for keys.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GamePath() As String
        Get
            Return mGamePath
        End Get
        Set(ByVal Value As String)
            If Not Value.Length = 0 Then
                If Value.EndsWith("\") Then
                    mGamePath = Value
                Else
                    mGamePath = Value & "\"
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Sets your registration key.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property RegKey() As String
        Set(ByVal Value As String)
            mRegKey = Value
        End Set
    End Property

    ''' <summary>
    ''' Set to True to enable Euro version. Otherwise US version is enabled.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property EnableEuro() As Boolean
        Set(ByVal Value As Boolean)
            mEuro = Value
        End Set
    End Property

    ''' <summary>
    ''' Set to true for ToA client.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property EnableToA() As Boolean
        Set(ByVal Value As Boolean)
            mToA = Value
        End Set
    End Property

    ''' <summary>
    ''' Set to true for Catacombs client.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property EnableCatacombs() As Boolean
        Set(ByVal Value As Boolean)
            mCAT = Value
        End Set
    End Property

    ''' <summary>
    ''' Set to true for Darkness Rising client.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property EnableDarknessRising() As Boolean
        Set(ByVal Value As Boolean)
            mDR = Value
        End Set
    End Property

    ''' <summary>
    ''' To automatically set keys.
    ''' Private PlayerKeys As UserKeys
    ''' .GamePath = Settings.GamePath 'set path or keys won't work
    ''' PlayerKeys = New AutoKillerScript.UserKeys(AK) 
    ''' .SetLeftTurnKey = PlayerKeys.TurnLeftKey
    ''' .SetRightTurnKey = PlayerKeys.TurnRightKey
    ''' .SetConsiderKey = PlayerKeys.ConsiderKey
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property SetLeftTurnKey() As Byte
        Set(ByVal Value As Byte)
            mLeftTurnKey = Value
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property SetRightTurnKey() As Byte
        Set(ByVal Value As Byte)
            mRightTurnKey = Value
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property SetConsiderKey() As Byte
        Set(ByVal Value As Byte)
            mConsiderKey = Value
        End Set
    End Property

    ''' <summary>
    ''' Creates a thread that will raise an event OnQueryStringTrue or OnRegExTrue. No need to call QueryString.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property EnableAutoQuery() As Boolean
        Set(ByVal Value As Boolean)
            mAutoQuery = Value
        End Set
    End Property

    ''' <summary>
    ''' Uses regex instead of normal string comparing.
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property UseRegEx() As Boolean
        Set(ByVal Value As Boolean)
            mUseRegEx = Value
        End Set
    End Property

#End Region

#Region "Crafting"
    ''' <summary>
    '''  Returns the name of the item in that spot in the inventory. The inventory is indexed from 0 to 39.
    ''' </summary>
    ''' <param name="x"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property InvName(ByVal x As Byte) As String
        Get
            If x < 0 OrElse x > 39 Then
                Return String.Empty
            End If
            Dim inv As DAOCInventoryItem = InventoryItem(x)
            If Not inv Is Nothing Then
                Return inv.Description
            Else
                Return String.Empty
            End If
        End Get
    End Property
#End Region
#End Region

#Region "File Functions"
    Private Sub LogF(ByVal strString As String)
        Dim myStreamWriter As StreamWriter = File.AppendText(LogFile)

        Try
            Thread.Sleep(1)
            myStreamWriter.WriteLine(TimeString & ": " & strString)
            myStreamWriter.Flush()
        Catch ex As Exception
            Debug.Write(ex.Message)
        Finally
            If myStreamWriter IsNot Nothing Then
                myStreamWriter.Close()
            End If
            RaiseEvent OnLog(strString)
        End Try
    End Sub

    Private Sub ParseLogFile()
        Dim strString As String
        Dim fs As FileStream
        Dim sr As StreamReader

        If mChatLog.Length = 0 Then
            Exit Sub
        End If

        fs = New FileStream(mChatLog, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        If mFilePos = 0 Then mFilePos = 1
        fs.Seek(mFilePos, SeekOrigin.Begin)
        sr = New StreamReader(fs)

        If mAutoQuery Then
            mAutoQuery = False 'set to false so manual query doesn't start a new loop
            While Not TerminateThread
                Try
                    ' Now parse any new lines of data
                    While sr.Peek > -1
                        strString = sr.ReadLine()
                        ' Make sure it's not a blank string, and get rid of time date stamp
                        If strString.Length > 0 Then
                            strString = Mid(strString, 12)
                            ParseString(strString)
                        End If
                    End While
                    mFilePos = fs.Position
                Catch ex As Exception
                    LogF(ex.Message)
                    Throw New AutoKillerException(ex)
                Finally
                    If sr IsNot Nothing Then
                        sr.Close()
                    End If
                End Try
                Thread.Sleep(250)
            End While
        Else
            Try
                ' Now parse any new lines of data
                While sr.Peek > -1
                    strString = sr.ReadLine()
                    ' Make sure it's not a blank string, and get rid of time date stamp
                    If strString.Length > 0 Then
                        strString = Mid(strString, 12)
                        ParseString(strString)
                    End If
                End While
                mFilePos = fs.Position
            Catch ex As Exception
                LogF(ex.Message)
                Throw New AutoKillerException(ex)
            Finally
                If sr IsNot Nothing Then
                    sr.Close()
                End If
            End Try
        End If

    End Sub

    Private Sub ParseString(ByVal strString As String)
        Dim x As Integer

        Try
            ' First we check our added string table
            For x = mTriggerStrings.GetLowerBound(0) To mTriggerStrings.GetUpperBound(0)
                If mTriggerStrings(x) IsNot Nothing AndAlso mTriggerStrings(x).Length <> 0 Then
                    If Not mUseRegEx Then 'can't use regex here
                        If String.Compare(strString, mTriggerStrings(x)) = 0 Then
                            mTriggerTable(x) = True
                            RaiseEvent OnQueryStringTrue(New AutokillerQueryEventParams(x, strString))
                        End If
                    Else
                        Dim aMatch As Match = Regex.Match(strString, mTriggerStrings(x), RegexOptions.Compiled Or RegexOptions.Singleline)
                        If aMatch.Success Then
                            mTriggerTable(x) = True
                            RaiseEvent OnRegExTrue(New AutokillerRegExEventParams(x, strString, aMatch))
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            LogF(ex.Message)
            Throw New AutoKillerException(ex)
        End Try

    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Removes a string from the array.
    ''' </summary>
    ''' <param name="x"></param>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub RemoveString(ByVal x As Short)
        If x < 0 OrElse x > mTriggerStrings.GetUpperBound(0) Then
            Exit Sub
        End If
        If x = UBound(mTriggerStrings) Then
            ReDim Preserve mTriggerStrings(x - 1)
            ReDim Preserve mTriggerTable(x - 1)
        Else
            mTriggerStrings(x) = ""
            mTriggerTable(x) = False
        End If
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Adds a string to the array, the strings that are searched in the chat.log.
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="strString"></param>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub AddString(ByVal x As Short, ByVal strString As String)
        If x > UBound(mTriggerStrings) Then
            ReDim Preserve mTriggerStrings(x)
            ReDim Preserve mTriggerTable(x)
            mTriggerTable(x) = False
        End If

        mTriggerStrings(x) = strString
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Returns true if the string at the index was found in the log since the last check.
    ''' </summary>
    ''' <param name="x"></param>
    ''' <returns></returns>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Function QueryString(ByVal x As Short) As Boolean

        Try
            If x < 0 OrElse x > mTriggerTable.GetUpperBound(0) Then
                LogF("Invalid index in QueryString.")
                Return False
            End If
            ParseLogFile()
            Return mTriggerTable(x)
        Catch ex As Exception
            LogF(ex.Message)
            Throw New AutoKillerException(ex)
        Finally
            mTriggerTable(x) = False
        End Try

    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Each call returns one string from the chat log. It starts at the end of the chat log 
    ''' when the script is ran. Each call moves the position up one-line of the chat log. 
    ''' So this function effectively returns all new lines since the last sequence of calls. 
    ''' If no new lines have been added, this functions returns a null string.
    ''' </summary>
    ''' <returns></returns>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Function GetString() As String
        Dim fs As FileStream
        Dim sr As StreamReader
        Dim strString As String = String.Empty
        Dim Found As Boolean = False

        fs = New FileStream(mChatLog, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        sr = New StreamReader(fs)

        Try
            If mFileLastPos < fs.Length Then
                fs.Seek(mFileLastPos, SeekOrigin.Begin)
                While sr.Peek > -1 AndAlso Not Found
                    strString = sr.ReadLine()
                    If strString.Length > 0 Then
                        mFileLastPos += strString.Length + 2
                        strString = Mid(strString, 12)
                        Found = True
                    End If
                End While
            End If
        Catch ex As Exception
            LogF("GetString error " & ex.Message)
            Throw New AutoKillerException(ex)
        Finally
            If sr IsNot Nothing Then
                sr.Close()
            End If
            If fs IsNot Nothing Then
                fs.Close()
            End If
        End Try

        Return strString

    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Used to load a route file into the DLL. Then you can use GetRoute() to get waypoints.
    ''' </summary>
    ''' <param name="RouteFile"></param>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub LoadRoute(ByVal RouteFile As String)
        Dim strString As String
        Dim count As Integer = 0
        Dim stream As StreamReader = File.OpenText(RouteFile)

        ' Now Load Route Data
        Try
            While stream.Peek > -1
                strString = stream.ReadLine
                Dim sArray() As String = strString.Split(CChar(";"))
                If strString.Length > 0 AndAlso sArray.GetUpperBound(0) = 3 Then
                    ' Add route point
                    ReDim Preserve mRouteTable(count)
                    If mRouteTable(count) Is Nothing Then
                        mRouteTable(count) = New RouteStruct
                    End If
                    If IsNumeric(sArray(0)) AndAlso IsNumeric(sArray(1)) AndAlso IsNumeric(sArray(2)) Then
                        mRouteTable(count).xPos = CSng(sArray(0))
                        mRouteTable(count).yPos = CSng(sArray(1))
                        mRouteTable(count).Range = CInt(sArray(2))
                    Else
                        LogF("Encountered error while loading route '" & RouteFile & "' at line " & count & ".")
                    End If

                    If IsNumeric(sArray(3)) Then
                        mRouteTable(count).action = CByte(sArray(3))
                    Else
                        Select Case sArray(3)
                            Case "hunt"
                                mRouteTable(count).action = 1
                            Case "sell"
                                mRouteTable(count).action = 2
                            Case Else
                                mRouteTable(count).action = 0
                        End Select
                    End If
                    count += 1
                End If
            End While
        Catch e As Exception
            Debug.WriteLine(e.Message)
        Finally
            If stream IsNot Nothing Then
                stream.Close()
            End If
        End Try
        mRouteLength = UBound(mRouteTable)
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Used to save the route data in the DLL to a filename. Can be used to create routes, or can be done manually.
    ''' </summary>
    ''' <param name="RouteFile"></param>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub SaveRoute(ByVal RouteFile As String)
        Dim strString As String
        Dim sArray(3) As String
        Dim x As Integer

        Dim myStreamWriter As StreamWriter = File.AppendText(RouteFile)

        Try
            ' Now Save Route Data
            For x = 0 To mRouteLength - 1
                sArray(0) = CStr(mRouteTable(x).xPos)
                sArray(1) = CStr(mRouteTable(x).yPos)
                sArray(2) = CStr(mRouteTable(x).Range)
                Select Case mRouteTable(x).action
                    Case 1
                        sArray(3) = "hunt"
                    Case 2
                        sArray(3) = "sell"
                    Case Else
                        sArray(3) = "none"
                End Select
                strString = Join(sArray, ";")
                myStreamWriter.WriteLine(strString)
                myStreamWriter.Flush()
            Next
        Catch ex As Exception
            LogF(ex.Message)
            Throw New AutoKillerException(ex)
        Finally
            If myStreamWriter IsNot Nothing Then
                myStreamWriter.Close()
            End If
        End Try
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Used to add a waypoint to the current route. Range is the distance to the player's current x,y coordinates, 
    ''' that the waypoint includes. Action is what to do when reaching that waypoint. 0 or "none" is for nothing, 
    ''' 1 or "hunt" is for hunting mobs, 2 or "sell" is to sell (when this feature is added).
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="action"></param>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub AddRoute(ByVal Range As Integer, ByVal action As String)

        Try
            ReDim Preserve mRouteTable(mRouteLength)
            If mRouteTable(mRouteLength) Is Nothing Then
                mRouteTable(mRouteLength) = New RouteStruct
            End If
            mRouteTable(mRouteLength).xPos = gPlayerXCoord
            mRouteTable(mRouteLength).yPos = gPlayerYCoord
            mRouteTable(mRouteLength).Range = Range
            If IsNumeric(action) Then
                mRouteTable(mRouteLength).action = CByte(action)
            Else
                Select Case action
                    Case "hunt"
                        mRouteTable(mRouteLength).action = 1
                    Case "sell"
                        mRouteTable(mRouteLength).action = 2
                    Case Else
                        mRouteTable(mRouteLength).action = 0
                End Select
            End If

            mRouteLength += 1
        Catch ex As Exception
            LogF(ex.Message)
            Throw New AutoKillerException(ex)
        End Try

    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Returns the waypoint at 'index' in the route file. It returns it as a string in the format of, 
    ''' x:y:range:action.
    ''' </summary>
    ''' <param name="Index"></param>
    ''' <returns></returns>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Function GetRoute(ByVal Index As Short) As String
        Dim sArray(3) As String

        Try
            If Index <= mRouteLength OrElse Index < 0 Then
                sArray(0) = CStr(mRouteTable(Index).xPos)
                sArray(1) = CStr(mRouteTable(Index).yPos)
                sArray(2) = CStr(mRouteTable(Index).Range)
                sArray(3) = CStr(mRouteTable(Index).action)
            Else
                LogF("Tried to load invalid point from route.")
                sArray(0) = CStr(0)
                sArray(1) = CStr(0)
                sArray(2) = CStr(0)
                sArray(3) = CStr(0)
            End If
            GetRoute = Join(sArray, ":")
        Catch ex As Exception
            LogF(ex.Message)
            Throw New AutoKillerException(ex)
        End Try

    End Function
#End Region

#Region "Memory Functions"
    Private Function ReadMemory(ByVal mAddress As Integer, ByVal mClass As Object) As Object
        Dim ptrStruct As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(mClass))
        ReadProcessMemory(hInst, mAddress, ptrStruct, Marshal.SizeOf(mClass), 0)
        Marshal.PtrToStructure(ptrStruct, mClass)
        Marshal.FreeHGlobal(ptrStruct)
        Return mClass
    End Function

    ''' <summary>
    ''' Returns all object in range specified, setting filterjunk to true will return only magic items and coin.
    ''' FilterNamed will not return named items.
    ''' BlackList, by default it's a white list, set true if you want the hunter to filter out what's specified in FilterItem.
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="FilterJunk"></param>
    ''' <param name="FilterNamed"></param>
    ''' <param name="BlackList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllObjects(ByVal Range As Integer, Optional ByVal FilterJunk As Boolean = False, Optional ByVal FilterNamed As Boolean = False, Optional ByVal BlackList As Boolean = False) As ArrayList 'get all objects
        Dim ObjectID As New ArrayList
        Dim TempName As String
        Dim Grave As Match
        Dim FontOfPower As Match
        Dim SphereRej As Match

        Try

            For Each Obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
                Grave = Regex.Match(Obj.Name, "Grave", RegexOptions.Compiled Or RegexOptions.Singleline)
                FontOfPower = Regex.Match(Obj.Name, "Font of Power", RegexOptions.Compiled Or RegexOptions.Singleline)
                SphereRej = Regex.Match(Obj.Name, "Sphere of Rejuvenation", RegexOptions.Compiled Or RegexOptions.Singleline)

                If Obj.GetObjectClass = DAOCObjectClass.ocObject AndAlso Not Grave.Success AndAlso Not FontOfPower.Success AndAlso Not SphereRej.Success AndAlso ZDistance(gPlayerXCoord, gPlayerYCoord, gPlayerZCoord, CDbl(Obj.X), CDbl(Obj.Y), CDbl(Obj.Z)) <= Range Then
                    If FilterJunk AndAlso Not BlackList Then 'white list will only loot items in the list
                        TempName = Obj.Name.ToLower
                        For Each s As String In FilterItem
                            Dim aMatch As Match = Regex.Match(Obj.Name, s, RegexOptions.Compiled Or RegexOptions.Singleline)
                            If aMatch.Success Then
                                ObjectID.Add(Obj.SpawnID)
                            End If
                        Next
                        If Obj.Name < TempName Then
                            If Not FilterNamed Then
                                ObjectID.Add(Obj.SpawnID)
                            End If
                        End If
                    Else
                        ObjectID.Add(Obj.SpawnID)
                    End If
                End If
            Next

            If BlackList Then 'pick up everything except what's in the list
                Dim remove As New ArrayList
                For Each i As Integer In ObjectID
                    For Each s As String In FilterItem
                        Dim tempobj As DAOCObject = packets.DAOCObjects.ObjectTable.Item(i)
                        Dim aMatch As Match = Regex.Match(tempobj.Name, s, RegexOptions.Compiled Or RegexOptions.Singleline)
                        If aMatch.Success Then
                            remove.Add(i)
                        End If
                    Next
                Next
                For Each i As Integer In remove
                    ObjectID.Remove(i)
                Next
            End If

            Return ObjectID
        Catch ex As Exception
            LogF(ex.Message)
            Throw New AutoKillerException(ex)
        End Try
    End Function

    ''' <summary>
    ''' Checks to see if player needs to be buffed. For example, if player should have 4 buffs but 
    ''' 1 has gone away, function will return true.
    ''' </summary>
    ''' <param name="NumOfBuffs"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckBuffs(ByVal NumOfBuffs As Short) As Boolean  'true need to buff, false you don't
        Return packets.LocalBufs.BuffTable.Count = NumOfBuffs
    End Function

    ''' <summary>
    ''' Selects the target at index. It does this by modifying memory, so it is 
    ''' possible to select targets as far away as you want, as long as your client 
    ''' knows the target exists.
    ''' </summary>
    ''' <param name="TargetIndex"></param>
    ''' <remarks></remarks>
    Private Sub SelectTarget(ByVal TargetIndex As Integer)
        'Use SetTarget which will call this
        Try
            Dim WritePtr As IntPtr = Marshal.AllocHGlobal(2)

            Marshal.WriteInt16(WritePtr, CShort(TargetIndex))

            ret = WriteProcessMemory(hInst, TargetIndexAddress, WritePtr, 2, 0)
            SendKeys(mConsiderKey, False)
            Marshal.FreeHGlobal(WritePtr)
        Catch ex As Exception
            LogF(ex.Message)
            Throw New AutoKillerException(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Finds closest invader within the specified range. newx and y can be used to specify search coordinates. Default coordinated are player.
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="newx"></param>
    ''' <param name="newy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindClosestInvader(ByVal Range As Integer, Optional ByVal newx As Integer = -1, Optional ByVal newy As Integer = -1) As Integer

        'Dim x As Short
        'Dim Index As Short = -1
        'Dim old_dist, new_dist As Double

        'Try
        '    For x = 0 To MAX_SPAWNS - 1
        '        ValidInvader(x) = 0
        '    Next

        '    If newx = -1 AndAlso newy = -1 Then
        '        newx = Spawn(mlngPlayerIndex).xPos
        '        newy = Spawn(mlngPlayerIndex).yPos
        '    End If

        '    SyncLock Spawn
        '        For x = Spawn.GetLowerBound(0) To Spawn.GetUpperBound(0)
        '            If (Spawn(x) Isnot Nothing) Then
        '                Try
        '                    Select Case Spawn(x).MobName
        '                        Case "Celt", "Elf", "Firbolg", "Lurikeen", "Sylvan", "Avalonian", "Briton", "Highlander", "Saracen", "Inconnu", _
        '                        "Dwarf", "Kobold", "Norseman", "Troll", "Valkyn", "Elfe", "Celte", "Sylvain", "Sylvaine", "Breton", "Avalonien", _
        '                        "Sarrasin", "Nécrite", "Avalonienne", "Sarasine", "Bretonne", "Nain", "Naine"
        '                            If Spawn(x).spawnId <> 0 AndAlso Spawn(x).NPC = 4 Then
        '                                new_dist = ZDistance(CDbl(newx), CDbl(newy), gPlayerZCoord, CDbl(Spawn(x).xPos), CDbl(Spawn(x).yPos), CDbl(Spawn(x).zPos))
        '                                If (new_dist < old_dist OrElse Index = -1) AndAlso (new_dist < Range) Then
        '                                    Index = x
        '                                    old_dist = new_dist
        '                                End If
        '                            End If
        '                    End Select
        '                Catch ex As Exception
        '                    Debug.WriteLine(ex.Message)
        '                End Try
        '            End If
        '        Next
        '    End SyncLock

        '    ' Set mob to being invalid 
        '    If Index <> -1 Then
        '        ValidInvader(Index) = 1
        '    End If

        '    Return Index
        'Catch ex As Exception
        '    LogF(ex.Message)
        '    Return -1
        'End Try
    End Function

    ''' <summary>
    ''' Finds the next closest invader within the specified range. newx and y can be used to specify search coordinates. Default coordinated are player.
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="newx"></param>
    ''' <param name="newy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindNextClosestInvader(ByVal Range As Integer, Optional ByVal newx As Integer = -1, Optional ByVal newy As Integer = -1) As Integer

        'Dim x As Short
        'Dim Index As Short = -1
        'Dim old_dist, new_dist As Double

        'Try
        '    If newx = -1 AndAlso newy = -1 Then
        '        newx = Spawn(mlngPlayerIndex).xPos
        '        newy = Spawn(mlngPlayerIndex).yPos
        '    End If

        '    SyncLock Spawn
        '        For x = Spawn.GetLowerBound(0) To Spawn.GetUpperBound(0)
        '            If Spawn(x) Isnot Nothing) Then
        '                Try
        '                    Select Case Spawn(x).MobName
        '                        Case "Celt", "Elf", "Firbolg", "Lurikeen", "Sylvan", "Avalonian", "Briton", "Highlander", "Saracen", "Inconnu", _
        '                       "Dwarf", "Kobold", "Norseman", "Troll", "Valkyn", "Elfe", "Celte", "Sylvain", "Sylvaine", "Breton", "Avalonien", _
        '                       "Sarrasin", "Nécrite", "Avalonienne", "Sarasine", "Bretonne", "Nain", "Naine", "Bretone", "Avalonier", "Sarazene", _
        '                       "Nordmann", "Zwerg", "Kelte", "Sylvaner"
        '                            If Spawn(x).spawnId <> 0 AndAlso Spawn(x).NPC = 4 AndAlso ValidInvader(x) = 0 Then
        '                                new_dist = ZDistance(CDbl(newx), CDbl(newy), gPlayerZCoord, CDbl(Spawn(x).xPos), CDbl(Spawn(x).yPos), CDbl(Spawn(x).zPos))
        '                                If (new_dist < old_dist OrElse Index = -1) AndAlso (new_dist < Range) Then
        '                                    Index = x
        '                                    old_dist = new_dist
        '                                End If
        '                            End If
        '                    End Select
        '                Catch ex As Exception
        '                    Debug.WriteLine(ex.Message)
        '                End Try
        '            End If
        '        Next
        '    End SyncLock

        '    ' Set mob to being invalid 
        '    If Index <> -1 Then
        '        ValidInvader(Index) = 1
        '    End If

        '    Return Index
        'Catch ex As Exception
        '    LogF(ex.Message)
        '    Return -1
        'End Try
    End Function

    ''' <summary>
    ''' Finds closest mob in combat within the specified range. newx and y can be used to specify search coordinates. Default coordinated are player. PetID is used to ignore a pet.
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="PetID"></param>
    ''' <param name="newx"></param>
    ''' <param name="newy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindClosestMobInCombat(ByVal Range As Integer, Optional ByVal PetID As Integer = 0, Optional ByVal newx As Integer = -1, Optional ByVal newy As Integer = -1) As Integer

        Dim SpawnID As Integer = -1
        Dim old_dist, new_dist As Double

        If newx = -1 AndAlso newy = -1 Then
            newx = gPlayerXCoord
            newy = gPlayerYCoord
        End If

        ValidMobInCombat.Clear()

        For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
            ' Make sure they're in combat, and are a NPC
            If ((obj.GetObjectClass = DAOCObjectClass.ocMob OrElse obj.GetObjectClass = DAOCObjectClass.ocUnknown) OrElse obj.GetObjectClass = DAOCObjectClass.ocUnknown) AndAlso obj.Combat AndAlso obj.SpawnID <> PetID Then
                new_dist = ZDistance(newx, newy, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))
                If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                    SpawnID = obj.SpawnID
                    old_dist = new_dist
                End If
            End If
        Next

        ' Set mob to being invalid 
        If Not SpawnID = -1 Then
            ValidMobInCombat.Add(SpawnID, Nothing)
            Return SpawnID
        End If

        Return -1

    End Function

    ''' <summary>
    ''' Finds closest mob in combat within the specified range. newx and y can be used to specify search coordinates. Default coordinated are player. PetID is used to ignore a pet. 
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="PetID"></param>
    ''' <param name="newx"></param>
    ''' <param name="newy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindClosestMobInCombat(ByVal Range As Integer, ByVal PetID As ArrayList, Optional ByVal newx As Integer = -1, Optional ByVal newy As Integer = -1) As Integer

        Dim SpawnID As Integer = -1
        Dim old_dist, new_dist As Double
        Dim isPet As Boolean

        If newx = -1 AndAlso newy = -1 Then
            newx = gPlayerXCoord
            newy = gPlayerYCoord
        End If

        ValidMobInCombat.Clear()

        For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
            ' Make sure mob has player targetted, and is an NPC
            For Each i As Integer In PetID
                If obj.SpawnID = i Then
                    isPet = True
                    Exit For
                Else
                    isPet = False
                End If
            Next

            ' Make sure they're in combat, and are a NPC
            If (obj.GetObjectClass = DAOCObjectClass.ocMob OrElse obj.GetObjectClass = DAOCObjectClass.ocUnknown) AndAlso obj.Combat AndAlso Not isPet Then
                new_dist = ZDistance(newx, newy, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))
                If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                    SpawnID = obj.SpawnID
                    old_dist = new_dist
                End If
            End If
        Next

        ' Set mob to being invalid 
        If Not SpawnID = -1 Then
            ValidMobInCombat.Add(SpawnID, Nothing)
            Return SpawnID
        End If

        Return -1

    End Function

    ''' <summary>
    ''' Finds the next closest mob in combat within the specified range. newx and y can be used to specify search coordinates. Default coordinated are player. PetID is used to ignore a pet.
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="PetID"></param>
    ''' <param name="newx"></param>
    ''' <param name="newy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindNextClosestMobInCombat(ByVal Range As Integer, Optional ByVal PetID As Integer = 0, Optional ByVal newx As Integer = -1, Optional ByVal newy As Integer = -1) As Integer

        Dim SpawnID As Integer = -1
        Dim old_dist, new_dist As Double

        If newx = -1 AndAlso newy = -1 Then
            newx = gPlayerXCoord
            newy = gPlayerYCoord
        End If

        For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
            ' Make sure they're in combat, and are a NPC
            If (obj.GetObjectClass = DAOCObjectClass.ocMob OrElse obj.GetObjectClass = DAOCObjectClass.ocUnknown) AndAlso obj.Combat AndAlso obj.SpawnID <> PetID AndAlso Not ValidMobInCombat.ContainsKey(obj.SpawnID) Then
                new_dist = ZDistance(newx, newy, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))
                If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                    SpawnID = obj.SpawnID
                    old_dist = new_dist
                End If
            End If
        Next

        ' Set mob to being invalid 
        If Not SpawnID = -1 Then
            ValidMobInCombat.Add(SpawnID, Nothing)
            Return SpawnID
        End If

        Return -1

    End Function

    ''' <summary>
    ''' Finds the next closest mob in combat within the specified range. newx and y can be used to specify search coordinates. Default coordinated are player. PetID is used to ignore a pet.
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="PetID"></param>
    ''' <param name="newx"></param>
    ''' <param name="newy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindNextClosestMobInCombat(ByVal Range As Integer, ByVal PetID As ArrayList, Optional ByVal newx As Integer = -1, Optional ByVal newy As Integer = -1) As Integer

        Dim SpawnID As Integer = -1
        Dim old_dist, new_dist As Double
        Dim isPet As Boolean

        If newx = -1 AndAlso newy = -1 Then
            newx = gPlayerXCoord
            newy = gPlayerYCoord
        End If

        For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
            ' Make sure mob has player targetted, and is an NPC
            For Each i As Integer In PetID
                If obj.SpawnID = i Then
                    isPet = True
                    Exit For
                Else
                    isPet = False
                End If
            Next

            ' Make sure they're in combat, and are a NPC
            If (obj.GetObjectClass = DAOCObjectClass.ocMob OrElse obj.GetObjectClass = DAOCObjectClass.ocUnknown) AndAlso obj.Combat AndAlso Not isPet AndAlso Not ValidMobInCombat.ContainsKey(obj.SpawnID) Then
                new_dist = ZDistance(newx, newy, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))
                If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                    SpawnID = obj.SpawnID
                    old_dist = new_dist
                End If
            End If
        Next

        ' Set mob to being invalid 
        If Not SpawnID = -1 Then
            ValidMobInCombat.Add(SpawnID, Nothing)
            Return SpawnID
        End If

        Return -1

    End Function

    ''' <summary>
    ''' Finds closest mob that has targeted the player within the specified range. newx and y can be used to specify search coordinates. Default coordinated are player. PetID is used to ignore a pet. 
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="PetID"></param>
    ''' <param name="newx"></param>
    ''' <param name="newy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindClosestMobWithPlayerAsTarget(ByVal Range As Integer, Optional ByVal PetID As Integer = 0, Optional ByVal newx As Integer = -1, Optional ByVal newy As Integer = -1) As Integer

        Dim SpawnID As Integer = -1
        Dim old_dist, new_dist As Double

        If newx = -1 AndAlso newy = -1 Then
            newx = gPlayerXCoord
            newy = gPlayerYCoord
        End If

        ValidMobWithPlayerAsTarget.Clear()

        For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
            ' Make sure mob has player targetted, and are a NPC
            If obj.TargetID = packets.Player.SpawnID AndAlso (obj.GetObjectClass = DAOCObjectClass.ocMob OrElse obj.GetObjectClass = DAOCObjectClass.ocUnknown) AndAlso obj.SpawnID <> PetID Then
                new_dist = ZDistance(newx, newy, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))
                If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                    SpawnID = obj.SpawnID
                    old_dist = new_dist
                End If
            End If
        Next

        ' Set mob to being invalid 
        If Not SpawnID = -1 Then
            ValidMobWithPlayerAsTarget.Add(SpawnID, Nothing)
            Return SpawnID
        End If

        Return -1

    End Function

    ''' <summary>
    ''' Finds the next closest mob that has targeted the player within the specified range. newx and y can be used to specify search coordinates. Default coordinated are player. PetID is used to ignore a pet. 
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="PetID"></param>
    ''' <param name="newx"></param>
    ''' <param name="newy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindNextClosestMobWithPlayerAsTarget(ByVal Range As Integer, Optional ByVal PetID As Integer = 0, Optional ByVal newx As Integer = -1, Optional ByVal newy As Integer = -1) As Integer

        Dim SpawnID As Integer = -1
        Dim old_dist, new_dist As Double

        If newx = -1 AndAlso newy = -1 Then
            newx = gPlayerXCoord
            newy = gPlayerYCoord
        End If

        For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
            ' Make sure mob has player targetted, and are a NPC
            If obj.TargetID = packets.Player.SpawnID AndAlso (obj.GetObjectClass = DAOCObjectClass.ocMob OrElse obj.GetObjectClass = DAOCObjectClass.ocUnknown) AndAlso obj.SpawnID <> PetID AndAlso Not ValidMobWithPlayerAsTarget.ContainsKey(obj.SpawnID) Then
                new_dist = ZDistance(newx, newy, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))
                If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                    SpawnID = obj.SpawnID
                    old_dist = new_dist
                End If
            End If
        Next

        ' Set mob to being invalid 
        If Not SpawnID = -1 Then
            ValidMobWithPlayerAsTarget.Add(SpawnID, Nothing)
            Return SpawnID
        End If

        Return -1

    End Function

    ''' <summary>
    ''' Finds closest mob that has targeted the TargetIndex within the specified range. newx and y can be used to specify search coordinates. Default coordinated are player. PetID is used to ignore a pet. 
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="TargetID"></param>
    ''' <param name="PetID"></param>
    ''' <param name="OverLoadJunk"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindClosestMobWithPlayerAsTarget(ByVal Range As Integer, ByVal TargetID As Integer, ByVal PetID As Integer, ByVal OverLoadJunk As Boolean) As Integer

        Dim SpawnID As Integer = -1
        Dim old_dist, new_dist As Double
        Dim newx As Integer = gPlayerXCoord
        Dim newy As Integer = gPlayerYCoord

        Dim newplayer As DAOCObject = DirectCast(packets.DAOCObjects.ObjectTable.Item(TargetID), DAOCObject)

        If newplayer IsNot Nothing Then

            ValidMobWithPlayerAsTarget.Clear()

            For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
                ' Make sure mob has player targetted, and are a NPC
                If obj.TargetID = newplayer.SpawnID AndAlso (obj.GetObjectClass = DAOCObjectClass.ocMob OrElse obj.GetObjectClass = DAOCObjectClass.ocUnknown) AndAlso obj.SpawnID <> PetID Then
                    new_dist = ZDistance(newx, newy, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))
                    If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                        SpawnID = obj.SpawnID
                        old_dist = new_dist
                    End If
                End If
            Next

            ' Set mob to being invalid 
            If Not SpawnID = -1 Then
                ValidMobWithPlayerAsTarget.Add(SpawnID, Nothing)
                Return SpawnID
            End If

            Return -1
        Else
            Return -1
        End If

    End Function

    ''' <summary>
    ''' Finds the next closest mob that has targeted the TargetIndex within the specified range. newx and y can be used to specify search coordinates. Default coordinated are player. PetID is used to ignore a pet. 
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="TargetID"></param>
    ''' <param name="PetID"></param>
    ''' <param name="OverLoadJunk"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindNextClosestMobWithPlayerAsTarget(ByVal Range As Integer, ByVal TargetID As Integer, ByVal PetID As Integer, ByVal OverLoadJunk As Boolean) As Integer

        Dim SpawnID As Integer = -1
        Dim old_dist, new_dist As Double
        Dim newx As Integer = gPlayerXCoord
        Dim newy As Integer = gPlayerYCoord

        Dim newplayer As DAOCObject = DirectCast(packets.DAOCObjects.ObjectTable.Item(TargetID), DAOCObject)

        If newplayer IsNot Nothing Then

            For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
                ' Make sure mob has player targetted, and are a NPC
                If obj.TargetID = newplayer.SpawnID AndAlso (obj.GetObjectClass = DAOCObjectClass.ocMob OrElse obj.GetObjectClass = DAOCObjectClass.ocUnknown) AndAlso obj.SpawnID <> PetID AndAlso Not ValidMobWithPlayerAsTarget.ContainsKey(obj.SpawnID) Then
                    new_dist = ZDistance(newx, newy, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))
                    If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                        SpawnID = obj.SpawnID
                        old_dist = new_dist
                    End If
                End If
            Next

            ' Set mob to being invalid 
            If Not SpawnID = -1 Then
                ValidMobWithPlayerAsTarget.Add(SpawnID, Nothing)
                Return SpawnID
            End If

            Return -1
        Else
            Return -1
        End If

    End Function

    ''' <summary>
    ''' Finds closest mob that has targeted the TargetIndex within the specified range. newx and y can be used to specify search coordinates. Default coordinated are player. PetID is used to ignore a pet. 
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="TargetID"></param>
    ''' <param name="PetID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindClosestMobWithPlayerAsTarget(ByVal Range As Integer, ByVal TargetID As Integer, ByVal PetID As ArrayList) As Integer

        Dim SpawnID As Integer = -1
        Dim old_dist, new_dist As Double
        Dim newx As Integer = gPlayerXCoord
        Dim newy As Integer = gPlayerYCoord
        Dim isPet As Boolean

        Dim newplayer As DAOCObject = DirectCast(packets.DAOCObjects.ObjectTable.Item(TargetID), DAOCObject)

        If newplayer IsNot Nothing Then

            ValidMobWithPlayerAsTarget.Clear()

            For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
                ' Make sure mob has player targetted, and is an NPC
                For Each i As Integer In PetID
                    If obj.SpawnID = i Then
                        isPet = True
                        Exit For
                    Else
                        isPet = False
                    End If
                Next
                If obj.TargetID = newplayer.SpawnID AndAlso (obj.GetObjectClass = DAOCObjectClass.ocMob OrElse obj.GetObjectClass = DAOCObjectClass.ocUnknown) AndAlso Not isPet Then
                    new_dist = ZDistance(newx, newy, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))
                    If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                        SpawnID = obj.SpawnID
                        old_dist = new_dist
                    End If
                End If
            Next

            ' Set mob to being invalid 
            If Not SpawnID = -1 Then
                ValidMobWithPlayerAsTarget.Add(SpawnID, Nothing)
                Return SpawnID
            End If

            Return -1
        Else
            Return -1
        End If

    End Function

    ''' <summary>
    ''' Finds the next closest mob that has targeted the TargetIndex within the specified range. newx and y can be used to specify search coordinates. Default coordinated are player. PetID is used to ignore a pet. 
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="TargetID"></param>
    ''' <param name="PetID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindNextClosestMobWithPlayerAsTarget(ByVal Range As Integer, ByVal TargetID As Integer, ByVal PetID As ArrayList) As Integer

        Dim SpawnID As Integer = -1
        Dim old_dist, new_dist As Double
        Dim newx As Integer = gPlayerXCoord
        Dim newy As Integer = gPlayerYCoord
        Dim isPet As Boolean

        Dim newplayer As DAOCObject = DirectCast(packets.DAOCObjects.ObjectTable.Item(TargetID), DAOCObject)

        If newplayer IsNot Nothing Then

            For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
                ' Make sure mob has player targetted, and is an NPC
                For Each i As Integer In PetID
                    If obj.SpawnID = i Then
                        isPet = True
                        Exit For
                    Else
                        isPet = False
                    End If
                Next
                If obj.TargetID = newplayer.SpawnID AndAlso (obj.GetObjectClass = DAOCObjectClass.ocMob OrElse obj.GetObjectClass = DAOCObjectClass.ocUnknown) AndAlso Not isPet AndAlso Not ValidMobWithPlayerAsTarget.ContainsKey(obj.SpawnID) Then
                    new_dist = ZDistance(newx, newy, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))
                    If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                        SpawnID = obj.SpawnID
                        old_dist = new_dist
                    End If
                End If
            Next

            ' Set mob to being invalid 
            If Not SpawnID = -1 Then
                ValidMobWithPlayerAsTarget.Add(SpawnID, Nothing)
                Return SpawnID
            End If

            Return -1
        Else
            Return -1
        End If

    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Finds closest mob that has targeted the player within the specified range. PetID is used to ignore a pet. 
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="PetID"></param>
    ''' <returns></returns>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Function FindClosestMobWithPlayerAsTarget(ByVal Range As Integer, ByVal PetID As ArrayList) As Integer

        Dim SpawnID As Integer = -1
        Dim old_dist, new_dist As Double
        Dim newx As Integer = gPlayerXCoord
        Dim newy As Integer = gPlayerYCoord
        Dim isPet As Boolean

        ValidMobWithPlayerAsTarget.Clear()

        For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
            ' Make sure mob has player targetted, and are a NPC
            For Each i As Integer In PetID
                If obj.SpawnID = i Then
                    isPet = True
                    Exit For
                Else
                    isPet = False
                End If
            Next
            If obj.TargetID = packets.Player.SpawnID AndAlso (obj.GetObjectClass = DAOCObjectClass.ocMob OrElse obj.GetObjectClass = DAOCObjectClass.ocUnknown) AndAlso Not isPet Then
                new_dist = ZDistance(newx, newy, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))
                If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                    SpawnID = obj.SpawnID
                    old_dist = new_dist
                End If
            End If
        Next

        ' Set mob to being invalid 
        If Not SpawnID = -1 Then
            ValidMobWithPlayerAsTarget.Add(SpawnID, Nothing)
            Return SpawnID
        End If

        Return -1

    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Finds the next closest mob that has targeted the player within the specified range. PetID is used to ignore a pet.
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="PetID"></param>
    ''' <returns></returns>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Function FindNextClosestMobWithPlayerAsTarget(ByVal Range As Integer, ByVal PetID As ArrayList) As Integer

        Dim SpawnID As Integer = -1
        Dim old_dist, new_dist As Double
        Dim newx As Integer = gPlayerXCoord
        Dim newy As Integer = gPlayerYCoord
        Dim isPet As Boolean

        For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
            ' Make sure mob has player targetted, and are a NPC
            For Each i As Integer In PetID
                If obj.SpawnID = i Then
                    isPet = True
                    Exit For
                Else
                    isPet = False
                End If
            Next
            If obj.TargetID = packets.Player.SpawnID AndAlso (obj.GetObjectClass = DAOCObjectClass.ocMob OrElse obj.GetObjectClass = DAOCObjectClass.ocUnknown) AndAlso Not isPet AndAlso Not ValidMobWithPlayerAsTarget.ContainsKey(obj.SpawnID) Then
                new_dist = ZDistance(newx, newy, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))
                If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                    SpawnID = obj.SpawnID
                    old_dist = new_dist
                End If
            End If
        Next

        ' Set mob to being invalid 
        If Not SpawnID = -1 Then
            ValidMobWithPlayerAsTarget.Add(SpawnID, Nothing)
            Return SpawnID
        End If

        Return -1

    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' This finds the closest mob between the levels specified by MinLevel and
    ''' MaxLevel. It only will return mobs that are within Range of the player. If no
    ''' mobs are found, -1 is returned.
    ''' </summary>
    ''' <param name="MinLevel"></param>
    ''' <param name="MaxLevel"></param>
    ''' <param name="Range"></param>
    ''' <param name="UseMobList"></param>
    ''' <param name="newx"></param>
    ''' <param name="newy"></param>
    ''' <returns></returns>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Function FindClosestMob(ByVal MinLevel As Short, ByVal MaxLevel As Short, ByVal Range As Integer, Optional ByVal UseMobList As Boolean = False, Optional ByVal newx As Integer = -1, Optional ByVal newy As Integer = -1) As Integer

        Dim SpawnID As Integer = -1
        Dim old_dist, new_dist As Double
        Dim AgroMobDist As Integer
        Dim AgroID As Integer = -1

        If newx = -1 AndAlso newy = -1 Then
            newx = gPlayerXCoord
            newy = gPlayerYCoord
        End If

        ValidClosestMob.Clear()

        Select Case True
            Case UseMobList
                For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
                    ' Make sure they're in level range, and are a NPC
                    If (obj.GetObjectClass = DAOCObjectClass.ocMob OrElse obj.GetObjectClass = DAOCObjectClass.ocUnknown) AndAlso obj.Health = 100 AndAlso Not obj.Combat AndAlso obj.Level >= MinLevel AndAlso obj.Level <= MaxLevel Then
                        new_dist = ZDistance(newx, newy, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))

                        For Each sItem As MobListMob In MobsToKill
                            If obj.Name = sItem.MobName Then
                                If sItem.Aggro Then 'TODO need to check this
                                    If (new_dist < AgroMobDist OrElse AgroID = -1) AndAlso (new_dist < Range) Then
                                        AgroID = obj.SpawnID
                                        AgroMobDist = CInt(new_dist)
                                    End If
                                Else
                                    If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                                        SpawnID = obj.SpawnID
                                        old_dist = new_dist
                                    End If
                                End If
                            End If
                        Next
                    End If
                Next
            Case Else
                For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
                    ' Make sure they're in level range, and are a NPC
                    If (obj.GetObjectClass = DAOCObjectClass.ocMob OrElse obj.GetObjectClass = DAOCObjectClass.ocUnknown) AndAlso obj.Health = 100 AndAlso Not obj.Combat AndAlso obj.Level >= MinLevel AndAlso obj.Level <= MaxLevel Then
                        new_dist = ZDistance(newx, newy, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))
                        If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                            SpawnID = obj.SpawnID
                            old_dist = new_dist
                        End If
                    End If
                Next
        End Select

        If AgroID <> -1 Then SpawnID = AgroID

        ' Set mob to being invalid 
        If Not SpawnID = -1 Then
            ValidClosestMob.Add(SpawnID, Nothing)
            Return SpawnID
        End If

        Return -1

    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' This finds the next closest mob between the levels specified by MinLevel and
    ''' MaxLevel. It only will return mobs that are within Range of the player. If no
    ''' mobs are found, -1 is returned.
    ''' </summary>
    ''' <param name="MinLevel"></param>
    ''' <param name="MaxLevel"></param>
    ''' <param name="Range"></param>
    ''' <param name="UseMobList"></param>
    ''' <param name="newx"></param>
    ''' <param name="newy"></param>
    ''' <returns></returns>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Function FindNextClosestMob(ByVal MinLevel As Short, ByVal MaxLevel As Short, ByVal Range As Integer, Optional ByVal UseMobList As Boolean = False, Optional ByVal newx As Integer = -1, Optional ByVal newy As Integer = -1) As Integer

        Dim SpawnID As Integer = -1
        Dim old_dist, new_dist As Double
        Dim AgroMobDist As Integer
        Dim AgroID As Integer = -1

        If newx = -1 AndAlso newy = -1 Then
            newx = gPlayerXCoord
            newy = gPlayerYCoord
        End If

        Select Case True
            Case UseMobList
                For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
                    ' Make sure they're in level range, and are a NPC
                    If (obj.GetObjectClass = DAOCObjectClass.ocMob OrElse obj.GetObjectClass = DAOCObjectClass.ocUnknown) AndAlso obj.Health = 100 AndAlso Not obj.Combat AndAlso obj.Level >= MinLevel AndAlso obj.Level <= MaxLevel AndAlso Not ValidClosestMob.ContainsKey(obj.SpawnID) Then
                        new_dist = ZDistance(newx, newy, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))

                        For Each sItem As MobListMob In MobsToKill
                            If obj.Name = sItem.MobName Then
                                If sItem.Aggro Then 'TODO need to check this
                                    If (new_dist < AgroMobDist OrElse AgroID = -1) AndAlso (new_dist < Range) Then
                                        AgroID = obj.SpawnID
                                        AgroMobDist = CInt(new_dist)
                                    End If
                                Else
                                    If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                                        SpawnID = obj.SpawnID
                                        old_dist = new_dist
                                    End If
                                End If
                            End If
                        Next
                    End If
                Next
            Case Else
                For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
                    ' Make sure they're in level range, and are a NPC
                    If (obj.GetObjectClass = DAOCObjectClass.ocMob OrElse obj.GetObjectClass = DAOCObjectClass.ocUnknown) AndAlso obj.Health = 100 AndAlso Not obj.Combat AndAlso obj.Level >= MinLevel AndAlso obj.Level <= MaxLevel AndAlso Not ValidClosestMob.ContainsKey(obj.SpawnID) Then
                        new_dist = ZDistance(newx, newy, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))
                        If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                            SpawnID = obj.SpawnID
                            old_dist = new_dist
                        End If
                    End If
                Next
        End Select

        If AgroID <> -1 Then SpawnID = AgroID

        ' Set mob to being invalid 
        If Not SpawnID = -1 Then
            ValidClosestMob.Add(SpawnID, Nothing)
            Return SpawnID
        End If

        Return -1

    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' This finds the closest object between the levels specified by MinLevel and 
    ''' MaxLevel. It only will return objects that are within Range of the player. If no 
    ''' objects are found, -1 is returned.
    ''' </summary>
    ''' <param name="MinLevel"></param>
    ''' <param name="MaxLevel"></param>
    ''' <param name="Range"></param>
    ''' <param name="newx"></param>
    ''' <param name="newy"></param>
    ''' <returns></returns>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Function FindClosestObject(ByVal MinLevel As Short, ByVal MaxLevel As Short, ByVal Range As Integer, Optional ByVal newx As Integer = -1, Optional ByVal newy As Integer = -1) As Integer

        Dim SpawnID As Integer = -1
        Dim old_dist, new_dist As Double

        If newx = -1 AndAlso newy = -1 Then
            newx = gPlayerXCoord
            newy = gPlayerYCoord
        End If

        ValidObject.Clear()

        For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
            ' Make sure they're in level range
            If obj.GetObjectClass = DAOCObjectClass.ocObject AndAlso obj.Level >= MinLevel AndAlso obj.Level <= MaxLevel Then
                new_dist = ZDistance(newx, newy, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))
                If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                    SpawnID = obj.SpawnID
                    old_dist = new_dist
                End If
            End If
        Next

        ' Set mob to being invalid 
        If Not SpawnID = -1 Then
            ValidObject.Add(SpawnID, Nothing)
            Return SpawnID
        End If

        Return -1

    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' This finds the next closest object between the levels specified by MinLevel and 
    ''' MaxLevel. It only will return objects that are within Range of the player. If no 
    ''' objects are found, -1 is returned.
    ''' </summary>
    ''' <param name="MinLevel"></param>
    ''' <param name="MaxLevel"></param>
    ''' <param name="Range"></param>
    ''' <param name="newx"></param>
    ''' <param name="newy"></param>
    ''' <returns></returns>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Function FindNextClosestObject(ByVal MinLevel As Short, ByVal MaxLevel As Short, ByVal Range As Integer, Optional ByVal newx As Integer = -1, Optional ByVal newy As Integer = -1) As Integer

        Dim SpawnID As Integer = -1
        Dim old_dist, new_dist As Double

        If newx = -1 AndAlso newy = -1 Then
            newx = gPlayerXCoord
            newy = gPlayerYCoord
        End If

        For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
            ' Make sure they're in level range
            If obj.GetObjectClass = DAOCObjectClass.ocObject AndAlso obj.Level >= MinLevel AndAlso obj.Level <= MaxLevel AndAlso Not ValidObject.ContainsKey(obj.SpawnID) Then
                new_dist = ZDistance(newx, newy, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))
                If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                    SpawnID = obj.SpawnID
                    old_dist = new_dist
                End If
            End If
        Next

        ' Set mob to being invalid 
        If Not SpawnID = -1 Then
            ValidObject.Add(SpawnID, Nothing)
            Return SpawnID
        End If

        Return -1

    End Function

    ''' <summary>
    ''' Sets your target returns true it successful
    ''' </summary>
    ''' <param name="spawnID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetTarget(ByVal spawnID As Integer) As Boolean
        Dim c As Integer = -1

        Try
            c = GetIndex(spawnID)

            If c > 0 Then
                SelectTarget(c)
                Return True
            End If

            Return False

        Catch ex As Exception
            LogF(ex.Message)
            Throw New AutoKillerException(ex)
        End Try
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Sets a target using a name, returns -1 if name is not found or if it is found returns the mob's id. If you do not want to write to memory, SetIndex to false.
    ''' </summary>
    ''' <param name="Name"></param>
    ''' <param name="SetIndex"></param>
    ''' <returns></returns>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Function SetTarget(ByVal Name As String, ByVal SetIndex As Boolean) As Integer
        Dim c As Integer = -1
        Dim FoundIt As Boolean
        Dim obj As DAOCObject = Nothing

        Try
            For Each obj In packets.DAOCObjects.ObjectTable.Values
                If obj.Name = Name Then
                    FoundIt = True
                    Exit For
                End If
            Next

            If FoundIt AndAlso SetIndex Then
                c = GetIndex(obj.SpawnID)
                SelectTarget(c)
                Return obj.SpawnID
            End If

            If FoundIt Then
                Return obj.SpawnID
            Else
                Return -1
            End If

        Catch ex As Exception
            LogF("SetTarget2 " & ex.Message)
            Throw New AutoKillerException(ex)
        End Try
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Finds closest NPC_TYPE with range specified, returns -1 if nothing is found.
    ''' Item = 0
    ''' NPC = 2
    ''' PC = 4
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="NPCType"></param>
    ''' <returns></returns>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Function SearchArea(ByVal Range As Integer, ByVal NPCType As DAOCObjectClass) As Integer

        Dim SpawnID As Integer = -1
        Dim old_dist, new_dist As Double
        Dim obj As DAOCObject

        ValidSearchMob.Clear()

        Try
            For Each obj In packets.DAOCObjects.ObjectTable.Values
                If obj.GetObjectClass = NPCType Then
                    new_dist = ZDistance(gPlayerXCoord, gPlayerYCoord, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))
                    If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                        SpawnID = obj.SpawnID
                        old_dist = new_dist
                    End If
                End If
            Next

            ' Set mob to being invalid 
            If SpawnID <> -1 Then
                ValidSearchMob.Add(SpawnID, Nothing)
            End If

            Return SpawnID
        Catch ex As Exception
            LogF(ex.Message)
            Throw New AutoKillerException(ex)
        End Try

    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Finds the next closest NPC_TYPE with range specified, returns -1 if nothing is found.
    ''' Item = 0
    ''' NPC = 2
    ''' PC = 4
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="NPCType"></param>
    ''' <returns></returns>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Function SearchAreaNext(ByVal Range As Integer, ByVal NPCType As DAOCObjectClass) As Integer

        Dim SpawnID As Integer = -1
        Dim old_dist, new_dist As Double

        Try
            For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values
                If obj.GetObjectClass = NPCType AndAlso Not ValidSearchMob.ContainsKey(obj.SpawnID) Then
                    new_dist = ZDistance(gPlayerXCoord, gPlayerYCoord, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))
                    If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                        SpawnID = obj.SpawnID
                        old_dist = new_dist
                    End If
                End If
            Next

            ' Set mob to being invalid 
            If SpawnID <> -1 Then
                ValidSearchMob.Add(SpawnID, Nothing)
            End If

            Return SpawnID
        Catch ex As Exception
            LogF(ex.Message)
            Throw New AutoKillerException(ex)
        End Try

    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Finds closest NPC_TYPE with range specified, returns -1 if nothing is found, PetID is used to ignore pets.
    ''' Item = 0
    ''' NPC = 2
    ''' PC = 4
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="NPCType"></param>
    ''' <param name="PetID"></param>
    ''' <returns></returns>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Function SearchArea(ByVal Range As Integer, ByVal NPCType As DAOCObjectClass, ByVal PetID As ArrayList) As Integer

        Dim SpawnID As Integer = -1
        Dim old_dist, new_dist As Double
        Dim isPet As Boolean

        ValidSearchMob.Clear()

        Try
            For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values

                ' Make sure mob has player targetted, and are a NPC
                For Each i As Integer In PetID
                    If obj.SpawnID = i Then
                        isPet = True
                        Exit For
                    Else
                        isPet = False
                    End If
                Next

                If obj.GetObjectClass = NPCType AndAlso Not isPet Then
                    new_dist = ZDistance(gPlayerXCoord, gPlayerYCoord, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))
                    If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                        SpawnID = obj.SpawnID
                        old_dist = new_dist
                    End If
                End If
            Next

            ' Set mob to being invalid 
            If SpawnID <> -1 Then
                ValidSearchMob.Add(SpawnID, Nothing)
            End If

            Return SpawnID
        Catch ex As Exception
            LogF(ex.Message)
            Throw New AutoKillerException(ex)
        End Try

    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Finds the next closest NPC_TYPE with range specified, returns -1 if nothing is found, PetID is used to ignore pets.
    ''' Item = 0
    ''' NPC = 2
    ''' PC = 4
    ''' </summary>
    ''' <param name="Range"></param>
    ''' <param name="NPCType"></param>
    ''' <param name="PetID"></param>
    ''' <returns></returns>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Function SearchAreaNext(ByVal Range As Integer, ByVal NPCType As DAOCObjectClass, ByVal PetID As ArrayList) As Integer

        Dim SpawnID As Integer = -1
        Dim old_dist, new_dist As Double
        Dim isPet As Boolean

        Try
            For Each obj As DAOCObject In packets.DAOCObjects.ObjectTable.Values

                ' Make sure mob has player targetted, and are a NPC
                For Each i As Integer In PetID
                    If obj.SpawnID = i Then
                        isPet = True
                        Exit For
                    Else
                        isPet = False
                    End If
                Next

                If obj.GetObjectClass = NPCType AndAlso Not isPet AndAlso Not ValidSearchMob.ContainsKey(obj.SpawnID) Then
                    new_dist = ZDistance(gPlayerXCoord, gPlayerYCoord, gPlayerZCoord, CDbl(obj.X), CDbl(obj.Y), CDbl(obj.Z))
                    If (new_dist < old_dist OrElse SpawnID = -1) AndAlso (new_dist < Range) Then
                        SpawnID = obj.SpawnID
                        old_dist = new_dist
                    End If
                End If
            Next

            ' Set mob to being invalid 
            If SpawnID <> -1 Then
                ValidSearchMob.Add(SpawnID, Nothing)
            End If

            Return SpawnID
        Catch ex As Exception
            LogF(ex.Message)
            Throw New AutoKillerException(ex)
        End Try

    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Starts the player running via memory, can be used instead of send keys.
    ''' </summary>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub StartRunning()
        Dim WritePtr As IntPtr
        Try
            WritePtr = Marshal.AllocHGlobal(1)
            Marshal.WriteByte(WritePtr, 1)
            ret = WriteProcessMemory(hInst, RunningAddress, WritePtr, 1, 0)
            Marshal.FreeHGlobal(WritePtr)
        Catch ex As Exception
            LogF(ex.Message)
            Throw New AutoKillerException(ex)
        End Try
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Stops the player from running via memory, can be used instead of send keys.
    ''' </summary>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub StopRunning()
        Dim WritePtr As IntPtr
        Try
            WritePtr = Marshal.AllocHGlobal(1)
            Marshal.WriteByte(WritePtr, 0)
            ret = WriteProcessMemory(hInst, RunningAddress, WritePtr, 1, 0)
            Marshal.FreeHGlobal(WritePtr)
        Catch ex As Exception
            LogF(ex.Message)
            Throw New AutoKillerException(ex)
        End Try
    End Sub
#End Region

#Region "Key and Mouse"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Simulates a keypress.
    ''' </summary>
    ''' <param name="keyPress"></param>
    ''' <param name="bHold"></param>
    ''' <param name="bRelease"></param>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub SendKeys(ByVal keyPress As Byte, Optional ByVal bHold As Boolean = False, Optional ByVal bRelease As Boolean = False)

        Dim theInput As cKey

        Try
            theInput = New cKey
            theInput.Key = keyPress

            If Not bRelease AndAlso Not bHold Then
                Keyboard.SendKey(theInput, KeyDirection.KeyUpDown)
            ElseIf Not bRelease Then
                Keyboard.SendKey(theInput, KeyDirection.KeyDown)
            ElseIf Not bHold Then
                Keyboard.SendKey(theInput, KeyDirection.KeyUp)
            End If

        Catch ex As Exception
            LogF(ex.Message)
            Throw New AutoKillerException(ex)
        End Try
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Sends a string to the game window. ie /stick
    ''' </summary>
    ''' <param name="strString"></param>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub SendString(ByVal strString As String)
        Keyboard.SendString(strString)
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Moves the mouse to specified coordinates.
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="y"></param>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub MouseMove(ByVal x As Integer, ByVal y As Integer)
        Dim P As Point = New Point(x, y)
        ClientToScreen(mhdl, P)
        Windows.Forms.Cursor.Position = New Point(P.X, P.Y)
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Simulates a left mouse click.
    ''' </summary>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub LeftClick()
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0)
        System.Threading.Thread.Sleep(250)
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0)
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Simulates a right mouse click.
    ''' </summary>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub RightClick()
        Call mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0)
        System.Threading.Thread.Sleep(250)
        Call mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0)
    End Sub
#End Region

#Region "Navigation"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Calculate the distance between two points, factors in height.
    ''' </summary>
    ''' <param name="X1"></param>
    ''' <param name="Y1"></param>
    ''' <param name="Z1"></param>
    ''' <param name="X2"></param>
    ''' <param name="Y2"></param>
    ''' <param name="Z2"></param>
    ''' <returns></returns>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Function ZDistance(ByVal X1 As Double, ByVal Y1 As Double, ByVal Z1 As Double, ByVal X2 As Double, ByVal Y2 As Double, ByVal Z2 As Double) As Double
        Return System.Math.Sqrt((X1 - X2) ^ 2 + (Y1 - Y2) ^ 2 + (Z1 - Z2) ^ 2)
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Find the heading difference between two points.
    ''' </summary>
    ''' <param name="X1"></param>
    ''' <param name="Y1"></param>
    ''' <param name="X2"></param>
    ''' <param name="Y2"></param>
    ''' <returns></returns>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Function FindHeading(ByVal X1 As Double, ByVal Y1 As Double, ByVal X2 As Double, ByVal Y2 As Double) As Short
        Dim Degree As Double
        Dim d_x As Double

        Try
            d_x = X2 - X1
            If d_x = 0 Then d_x = 1
            Degree = System.Math.Atan(System.Math.Abs((Y2 - Y1) / d_x)) * 180 / Math.PI

            If Y2 - Y1 > 0 AndAlso X2 - X1 < 0 Then
                Degree = 180 - Degree
            End If
            If Y2 - Y1 < 0 AndAlso X2 - X1 < 0 Then
                Degree = 180 + Degree
            End If
            If Y2 - Y1 < 0 AndAlso X2 - X1 > 0 Then
                Degree = 360 - Degree
            End If

            Degree = Degree + 90

            Degree = Degree Mod 360

            Return CShort(Degree)
        Catch ex As Exception
            LogF(ex.Message)
            Throw New AutoKillerException(ex)
        End Try
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Turn the player to within 5 degrees of a specified heading.
    ''' </summary>
    ''' <param name="DestHeading"></param>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub TurnToHeading(ByVal DestHeading As Short)
        Dim Direction As String
        Dim TurnTime As Long

        If DestHeading < 0 OrElse DestHeading > 360 Then
            LogF("Invalid heading.")
            Exit Sub
        End If
        Try
            Direction = GetTurnDirection(PlayerDir, DestHeading) 'Returns left or right
            TurnTime = CLng(GetTurnTime(PlayerDir, DestHeading)) 'returns amount of time to turn
            Select Case Direction
                Case "RIGHT"
                    SendKeys(mRightTurnKey, True)
                    Threading.Thread.Sleep(CInt(TurnTime))
                    SendKeys(mRightTurnKey, , True)
                Case "LEFT"
                    SendKeys(mLeftTurnKey, True)
                    Threading.Thread.Sleep(CInt(TurnTime))
                    SendKeys(mLeftTurnKey, , True)
            End Select
        Catch ex As Exception
            LogF(ex.Message)
            Throw New AutoKillerException(ex)
        End Try
    End Sub

    Private Function GetTurnTime(ByVal CurrentHeading As Integer, ByVal DestHeading As Integer) As Double
        Dim x As Integer

        Try
            x = System.Math.Abs(CurrentHeading - DestHeading)
            If x > 180 Then
                x = 360 - x
            End If
            Return (x * 8)
        Catch ex As Exception
            LogF(ex.Message)
            Throw New AutoKillerException(ex)
        End Try
    End Function

    Private Function GetTurnDirection(ByVal CurrentHeading As Integer, ByVal DestHeading As Integer) As String
        Dim x As Integer

        Try
            If CurrentHeading < DestHeading Then
                x = System.Math.Abs(CurrentHeading - DestHeading)
                If x > 180 Then
                    Return "LEFT"
                Else
                    Return "RIGHT"
                End If
            Else
                x = (CurrentHeading - DestHeading)
                If x > 180 Then
                    Return "RIGHT"
                Else
                    Return "LEFT"
                End If
            End If
        Catch ex As Exception
            LogF(ex.Message)
        End Try

        Return String.Empty

    End Function
#End Region

#Region "General"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Terminates the game process.
    ''' </summary>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub KillGame()
        TerminateProcess(hInst, 0)
    End Sub

    Private Sub FindProcess()
        Dim lngPID As Integer

        Try
            If mUserProcessID > 0 Then
                hInst = OpenProcess(PROCESS_ALL_ACCESS, 0, mUserProcessID)
                GamePID = mUserProcessID 'for get process
            Else
                AKHwnd = FindWindow("AKMSGWINDOW", "AKMSGWINDOW")
                mhdl = FindWindow("DAocMWC", Nothing)
                GetWindowThreadProcessId(mhdl, lngPID)
                GamePID = lngPID 'for get process
                hInst = OpenProcess(PROCESS_ALL_ACCESS, 0, lngPID)
            End If
        Catch ex As Exception
            LogF("Error trying to find process ID " & ex.Message)
        End Try

    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub New()
        MyBase.New()

        ' Code to be executed when the component starts,
        '   in response to the first object request.
        Debug.WriteLine("Executing Sub New")

        mLeftTurnKey = CByte(Keys.Left)
        mRightTurnKey = CByte(Keys.Right)
        mConsiderKey = CByte(Keys.N)
        mRegKey = String.Empty
        mEuro = False
        mRouteLength = 0
        mChatLog = String.Empty

        ValidSearchMob = New Hashtable
        ValidClosestMob = New Hashtable
        ValidInvader = New Hashtable
        ValidMobInCombat = New Hashtable
        ValidMobWithPlayerAsTarget = New Hashtable
        ValidObject = New Hashtable

        ReDim mTriggerStrings(0)
        ReDim mTriggerTable(0)
        mTriggerTable(0) = False

        Debug.WriteLine("Starting")
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Game process can be specified if 2 games are running.
    ''' </summary>
    ''' <param name="ProcessID"></param>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub New(ByVal ProcessID As Integer)
        MyBase.New()

        ' Code to be executed when the component starts,
        '   in response to the first object request.
        Debug.WriteLine("Executing Sub New")

        mLeftTurnKey = CByte(Keys.Left)
        mRightTurnKey = CByte(Keys.Right)
        mConsiderKey = CByte(Keys.N)
        mRegKey = String.Empty
        mEuro = False
        mRouteLength = 0
        mChatLog = vbNullString
        mUserProcessID = ProcessID 'if user wants to run multiple games on 1 pc

        ValidSearchMob = New Hashtable
        ValidClosestMob = New Hashtable
        ValidInvader = New Hashtable
        ValidMobInCombat = New Hashtable
        ValidMobWithPlayerAsTarget = New Hashtable
        ValidObject = New Hashtable

        ReDim mTriggerStrings(0)
        ReDim mTriggerTable(0)
        mTriggerTable(0) = False

        Debug.WriteLine("Starting")
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Stops the dll, should be called before shutting down your program or script.
    ''' </summary>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub StopInit()
        LogF("Shutting down dll")
        TerminateThread = True
        If Not hInst = 0 Then
            CloseHandle(hInst)
            hInst = 0
        End If

        If mAutoQuery Then
            If QueryThread IsNot Nothing Then
                QueryThread.Join()
            End If
        End If

        Running = False

        'DGB
        'Dim xml As New Encrypter
        'xml.LogUserOut(mRegKey)

        CleanUpRemoting()

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Used for an alert.
    ''' </summary>
    ''' <param name="dwFreq"></param>
    ''' <param name="dwDuration"></param>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub Sound(ByVal dwFreq As Integer, ByVal dwDuration As Integer)
        DeclareBeep(dwFreq, dwDuration)
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Starts up program. Must be called before anything else other than the 
    ''' Let properties.
    ''' </summary>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub DoInit()

        Dim FoundProcess As Boolean = False

        LogF("Starting dll")
        If Not mChatLog = Nothing Then
            Try
                Dim tempFileInfo As New FileInfo(mChatLog)
                LogF("Opening Chat.log.")
                mFilePos = tempFileInfo.Length
                mFileLastPos = tempFileInfo.Length
            Catch e As Exception
                LogF("Error opening Chat.log")
            End Try
        End If

        If LoadMemLocs() Then
            LoadRemoting()

            Dim i As Integer
            LogF("Finding Process ID...")
            Do Until Not hInst = 0
                i += 1
                FindProcess()
                Thread.Sleep(1)
                If hInst = 0 Then LogF("Trying again.")
                If i = 500 Then Exit Do
            Loop

            If Not hInst = 0 Then
                FoundProcess = True
            End If

            If FoundProcess Then
                LogF("Successfully got Process ID.")

                TerminateThread = False

                If mAutoQuery Then
                    QueryThread = New Thread(AddressOf ParseLogFile)
                    QueryThread.Name = "QueryThread"
                    QueryThread.Start() 'start reading logs
                End If

                LogF("Finished with DoInit")

                RaiseEvent OnFinishDoInit(True)
            Else
                RaiseEvent OnFinishDoInit(False)
            End If

        Else
            RaiseEvent OnFinishDoInit(False)
        End If

    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Starts up program. Must be called before anything else other than the 
    ''' Let properties.
    ''' </summary>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub DoInit(ByVal Instance As DAOCMain)

        Dim FoundProcess As Boolean = False

        LogF("Starting dll")
        If Not mChatLog = Nothing Then
            Try
                Dim tempFileInfo As New FileInfo(mChatLog)
                LogF("Opening Chat.log.")
                mFilePos = tempFileInfo.Length
                mFileLastPos = tempFileInfo.Length
            Catch e As Exception
                LogF("Error opening Chat.log")
            End Try
        End If

        If LoadMemLocs() Then
            LoadEvents(Instance)

            Dim i As Integer
            LogF("Finding Process ID...")
            Do Until Not hInst = 0
                i += 1
                FindProcess()
                Thread.Sleep(1)
                If hInst = 0 Then LogF("Trying again.")
                If i = 500 Then Exit Do
            Loop

            If Not hInst = 0 Then
                FoundProcess = True
            End If

            If FoundProcess Then
                LogF("Successfully got Process ID.")

                TerminateThread = False

                If mAutoQuery Then
                    QueryThread = New Thread(AddressOf ParseLogFile)
                    QueryThread.Name = "QueryThread"
                    QueryThread.Start() 'start reading logs
                End If

                LogF("Finished with DoInit")

                RaiseEvent OnFinishDoInit(True)
            Else
                RaiseEvent OnFinishDoInit(False)
            End If

        Else
            RaiseEvent OnFinishDoInit(False)
        End If

    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Starts up program. Must be called before anything else other than the 
    ''' Let properties.
    ''' </summary>
    ''' <Author>
    ''' 	AutoKiller
    ''' </Author>
    ''' -----------------------------------------------------------------------------
    Public Sub DoInit(ByVal SkipFindProcess As Boolean)

        Dim FoundProcess As Boolean = False

        LogF("Starting dll")
        If Not mChatLog = Nothing Then
            Try
                Dim tempFileInfo As New FileInfo(mChatLog)
                LogF("Opening Chat.log.")
                mFilePos = tempFileInfo.Length
                mFileLastPos = tempFileInfo.Length
            Catch e As Exception
                LogF("Error opening Chat.log")
            End Try
        End If

        If LoadMemLocs() Then
            LoadRemoting()

            If Not SkipFindProcess Then
                Dim i As Integer
                LogF("Finding Process ID...")
                Do Until Not hInst = 0
                    i += 1
                    FindProcess()
                    Thread.Sleep(1)
                    If hInst = 0 Then LogF("Trying again.")
                    If i = 500 Then Exit Do
                Loop
                If Not hInst = 0 Then
                    FoundProcess = True
                End If
            Else
                FoundProcess = True
            End If

            If FoundProcess Then
                LogF("Successfully got Process ID.")

                TerminateThread = False

                If mAutoQuery Then
                    QueryThread = New Thread(AddressOf ParseLogFile)
                    QueryThread.Name = "QueryThread"
                    QueryThread.Start() 'start reading logs
                End If

                LogF("Finished with DoInit")

                RaiseEvent OnFinishDoInit(True)
            Else
                RaiseEvent OnFinishDoInit(False)
            End If

        Else
            RaiseEvent OnFinishDoInit(False)
        End If

    End Sub

    ''' <summary>
    ''' used when noremoting is true
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadEvents(ByVal Instance As DAOCMain)
        packets = Instance

        callback = New OurCallBack

        'IMPORTANT any events registered with OurCallBack must be unregistered when closing the app
        'or the next connection events will not work

        'logs 1.0.2128.10568  1.0.2142.12743
        AddHandler packets.OnLogUpdate, AddressOf callback.LogUpdateMethodCallback
        AddHandler callback.OnLogUpdate, AddressOf ParseString

        'newmob
        AddHandler packets.OnNewDAOCObject, AddressOf callback.NewObjectMethodCallback
        AddHandler callback.OnNewObject, AddressOf OnNewObjectHandler

        'petwindow
        AddHandler packets.OnPetWindowUpdate, AddressOf callback.PetWindowUpdateMethodCallback
        AddHandler callback.OnPetWindowUpdate, AddressOf OnPetWindowUpdateHandler

        'spellcast
        AddHandler packets.OnSpellCast, AddressOf callback.SpellCastMethodCallback
        AddHandler callback.OnSpellCast, AddressOf OnSpellCastHandler

        'progress meter
        AddHandler packets.OnProgressMeter, AddressOf callback.ProgressMeterMethodCallback
        AddHandler callback.OnProgressMeter, AddressOf OnProgressMeterHandler

        'Dialogue Update
        AddHandler packets.OnDialogMessage, AddressOf callback.DialogMessageMethodCallback
        AddHandler callback.OnDialogMessage, AddressOf OnDialogueUpdateHandler

        'spelleffect
        AddHandler packets.OnSpellEffect, AddressOf callback.SpellEffectMethodCallback
        AddHandler callback.OnSpellEffectAnimation, AddressOf OnSpellEffectHandler

        'zone
        AddHandler packets.OnZoneChange, AddressOf callback.ZoneChangeMethodCallback
        AddHandler callback.OnZoneChange, AddressOf ZoneHandler

        'player quit
        AddHandler packets.OnPlayerQuit, AddressOf callback.PlayerQuitMethodCallback
        AddHandler callback.OnPlayerQuit, AddressOf OnPlayerQuitHandler
    End Sub

    ''' <summary>
    ''' used when noremoting is true
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadEvents()
        packets = New DAOCMain

        callback = New OurCallBack

        'IMPORTANT any events registered with OurCallBack must be unregistered when closing the app
        'or the next connection events will not work

        'logs 1.0.2128.10568  1.0.2142.12743
        AddHandler packets.OnLogUpdate, AddressOf callback.LogUpdateMethodCallback
        AddHandler callback.OnLogUpdate, AddressOf ParseString

        'newmob
        AddHandler packets.OnNewDAOCObject, AddressOf callback.NewObjectMethodCallback
        AddHandler callback.OnNewObject, AddressOf OnNewObjectHandler

        'petwindow
        AddHandler packets.OnPetWindowUpdate, AddressOf callback.PetWindowUpdateMethodCallback
        AddHandler callback.OnPetWindowUpdate, AddressOf OnPetWindowUpdateHandler

        'spellcast
        AddHandler packets.OnSpellCast, AddressOf callback.SpellCastMethodCallback
        AddHandler callback.OnSpellCast, AddressOf OnSpellCastHandler

        'progress meter
        AddHandler packets.OnProgressMeter, AddressOf callback.ProgressMeterMethodCallback
        AddHandler callback.OnProgressMeter, AddressOf OnProgressMeterHandler

        'Dialogue Update
        AddHandler packets.OnDialogMessage, AddressOf callback.DialogMessageMethodCallback
        AddHandler callback.OnDialogMessage, AddressOf OnDialogueUpdateHandler

        'spelleffect
        AddHandler packets.OnSpellEffect, AddressOf callback.SpellEffectMethodCallback
        AddHandler callback.OnSpellEffectAnimation, AddressOf OnSpellEffectHandler

        'zone
        AddHandler packets.OnZoneChange, AddressOf callback.ZoneChangeMethodCallback
        AddHandler callback.OnZoneChange, AddressOf ZoneHandler

        'player quit
        AddHandler packets.OnPlayerQuit, AddressOf callback.PlayerQuitMethodCallback
        AddHandler callback.OnPlayerQuit, AddressOf OnPlayerQuitHandler
    End Sub

    Private Sub LoadRemoting()
        Dim clientProvider As BinaryClientFormatterSinkProvider = New BinaryClientFormatterSinkProvider
        Dim serverProvider As BinaryServerFormatterSinkProvider = New BinaryServerFormatterSinkProvider
        serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full

        Dim props As IDictionary = New Hashtable
        Dim s As String = System.Guid.NewGuid().ToString()
        props.Add("portName", s)
        props.Add("typeFilterLevel", System.Runtime.Serialization.Formatters.TypeFilterLevel.Full)
        Dim channel As IChannel = New IpcChannel(props, clientProvider, serverProvider)

        ChannelServices.RegisterChannel(channel, False)
        Dim c() As WellKnownClientTypeEntry = RemotingConfiguration.GetRegisteredWellKnownClientTypes
        'how do I unregister?
        If c.Length = 0 Then
            RemotingConfiguration.RegisterWellKnownClientType(GetType(AKServer.DLL.DAoCServer.DAOCMain), "ipc://akserverport/AKServerRemote")
        End If

        LoadEvents()

    End Sub

    Private Sub CleanUpRemoting()
        If packets IsNot Nothing Then
            RemoveHandler packets.OnLogUpdate, AddressOf callback.LogUpdateMethodCallback
            RemoveHandler packets.OnNewDAOCObject, AddressOf callback.NewObjectMethodCallback
            RemoveHandler packets.OnPetWindowUpdate, AddressOf callback.PetWindowUpdateMethodCallback
            RemoveHandler packets.OnSpellCast, AddressOf callback.SpellCastMethodCallback
            RemoveHandler packets.OnProgressMeter, AddressOf callback.ProgressMeterMethodCallback
            RemoveHandler packets.OnDialogMessage, AddressOf callback.DialogMessageMethodCallback
            RemoveHandler packets.OnSpellEffect, AddressOf callback.SpellEffectMethodCallback
            RemoveHandler packets.OnZoneChange, AddressOf callback.ZoneChangeMethodCallback
            RemoveHandler packets.OnPlayerQuit, AddressOf callback.PlayerQuitMethodCallback
        End If

        packets = Nothing
        callback = Nothing
        'ChannelServices.UnregisterChannel(channel)

        For Each channel As IChannel In ChannelServices.RegisteredChannels
            ChannelServices.UnregisterChannel(channel)
        Next

        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Private Function LoadMemLocs() As Boolean      
        'DGB
        'Dim xml As New Encrypter
        'Dim iFile As New XmlDocument
        'Dim N As XmlElement = Nothing
        'Dim item As XmlElement
        'Dim temp As String

        'Try
        '    iFile.Load(xml.GetXML(mRegKey))
        '    If Not mEuro Then
        '        If mCAT Then
        '            N = DirectCast(iFile.SelectSingleNode("//section[@name='USCATACOMBS']"), XmlElement)
        '        ElseIf mToA Then
        '            N = DirectCast(iFile.SelectSingleNode("//section[@name='USTRIALSOFATLANTIS']"), XmlElement)
        '        ElseIf mDR Then
        '            N = DirectCast(iFile.SelectSingleNode("//section[@name='USDARKNESSRISING']"), XmlElement)
        '        End If
        '    Else
        '        If mCAT Then
        '            N = DirectCast(iFile.SelectSingleNode("//section[@name='EUROCATACOMBS']"), XmlElement)
        '        ElseIf mToA Then
        '            N = DirectCast(iFile.SelectSingleNode("//section[@name='EUROTRIALSOFATLANTIS']"), XmlElement)
        '        ElseIf mDR Then
        '            N = DirectCast(iFile.SelectSingleNode("//section[@name='EURODARKNESSRISING']"), XmlElement)
        '        End If
        '    End If

        '    item = DirectCast(N.SelectSingleNode("item[@key='TargetIndexAddress']"), XmlElement)
        '    If (item IsNot Nothing) Then
        '        temp = item.GetAttribute("newValue")
        '        TargetIndexAddress = CInt(Encrypt(temp))
        '    End If

        '    item = DirectCast(N.SelectSingleNode("item[@key='RunningAddress']"), XmlElement)
        '    If (item IsNot Nothing) Then
        '        temp = item.GetAttribute("newValue")
        '        RunningAddress = CInt(Encrypt(temp))
        '    End If

        '    item = DirectCast(N.SelectSingleNode("item[@key='LocalPlayerInfo']"), XmlElement)
        '    If (item IsNot Nothing) Then
        '        temp = item.GetAttribute("newValue")
        '        LocalPlayerInfo = CInt(Encrypt(temp))
        '    End If

        '    Return True

        'Catch ex As Exception
        '    LogF(ex.Message)
        '    LogF("Error logging in, you need a valid registration code to use.")
        '    Return False
        'End Try

        'TargetIndexAddress = &H1472B37
        TargetIndexAddress = &H1472B38
        'TargetIndexAddress = &H1472B38

        RunningAddress = &H1472B20
        'RunningAddress = &H1472B20

        'LocalPlayerInfo = &H38227
        LocalPlayerInfo = &H2293B68

        return True

    End Function

    'DGB
    'Private Function Encrypt(ByRef xSrc As String) As String
    '    Dim masterString, sChar As String
    '    Dim L, I As Integer
    '    masterString = "WriteProcessMemory" ' use any string you want
    '    L = (masterString).Length
    '    For I = 1 To (xSrc).Length
    '        sChar = (Asc((masterString).Substring((I Mod L) - L * CShort((I Mod L) = 0), 1))).ToString()
    '        Mid(xSrc, I, 1) = Chr(Asc(Mid(xSrc, I, 1)) Xor CInt(sChar))
    '    Next
    '    Encrypt = xSrc
    'End Function

#End Region

#Region "Handlers"
    Private Sub OnPlayerQuitHandler()
        RaiseEvent OnPlayerQuit()
    End Sub

    Private Sub OnNewObjectHandler(ByVal Sender As Object, ByVal e As DAOCEventArgs)
        RaiseEvent OnMobCreation(Sender, e)
    End Sub

    Private Sub OnDialogueUpdateHandler(ByVal Sender As Object, ByVal e As LogUpdateEventArgs)
        RaiseEvent OnDialog(Sender, e)
    End Sub

    Private Sub OnPetWindowUpdateHandler(ByVal Sender As Object, ByVal e As DAOCEventArgs)
        RaiseEvent OnPetWindowUpdate(Sender, e)
    End Sub

    Private Sub OnSpellCastHandler(ByVal Sender As Object, ByVal e As DAOCEventArgs)
        RaiseEvent OnSpellCast(Sender, e)
    End Sub

    Private Sub OnProgressMeterHandler(ByVal Sender As Object, ByVal e As LogUpdateEventArgs)
        RaiseEvent OnProgressMeter(Sender, e)
    End Sub

    Private Sub OnSpellEffectHandler(ByVal Sender As Object, ByVal e As DAOCEventArgs)
        RaiseEvent OnSpellEffectAnimation(Sender, e)
    End Sub
#End Region

#Region "Inject Functions"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	
    ''' </history>
    ''' -----------------------------------------------------------------------------
    ''' 
    Public Sub InjectFindWindow()
        PostMessage(AKHwnd, InjectType.FINDWINDOW, 0, 0)
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Text"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	
    ''' </history>
    ''' -----------------------------------------------------------------------------
    ''' 
    Public Sub InjectSetCaption(ByVal Text As String)
        Dim buffer As Byte() = Encoding.ASCII.GetBytes(Text)
        MailSlot.WriteMail(buffer)
        PostMessage(AKHwnd, InjectType.SETTEXT, 0, 0)
    End Sub

    Private Function GetIndex(ByVal spawnID As Integer) As Integer
        Return SendMessage(AKHwnd, InjectType.SETINDEX, spawnID, 0)
    End Function
#End Region

End Class
