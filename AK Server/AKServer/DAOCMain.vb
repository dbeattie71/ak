#Region "Imports"
Imports System.Threading
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Runtime.Remoting.Messaging
Imports System.Collections
#End Region
Namespace DAoCServer
#Region "Event handlers"
    Public MustInherit Class RemotelyDelegatableObject
        Inherits MarshalByRefObject
        Public Sub LogUpdateMethodCallback(ByVal sender As Object, ByVal submitArgs As LogUpdateEventArgs)
            LogUpdateCallback(sender, submitArgs)
            ' This OurInternalCallback method will call our concrete implementation
            ' of OurInternalCallback (don't be confused below, abstract void OurInternalCallback
            ' is an abstract method! 
        End Sub

        Public Sub ZoneChangeMethodCallback(ByVal sender As Object, ByVal submitArgs As ZoneEventArgs)
            ZoneChangeCallback(sender, submitArgs)
        End Sub

        Public Sub NewObjectMethodCallback(ByVal sender As Object, ByVal submitArgs As DAOCEventArgs)
            NewObjectCallback(sender, submitArgs)
        End Sub

        Public Sub PetWindowUpdateMethodCallback(ByVal sender As Object, ByVal submitArgs As DAOCEventArgs)
            PetWindowUpdateCallback(sender, submitArgs)
        End Sub

        Public Sub SpellCastMethodCallback(ByVal sender As Object, ByVal submitArgs As DAOCEventArgs)
            SpellCastCallback(sender, submitArgs)
        End Sub

        Public Sub DialogMessageMethodCallback(ByVal sender As Object, ByVal submitArgs As LogUpdateEventArgs)
            DialogMessageCallback(sender, submitArgs)
        End Sub

        Public Sub ProgressMeterMethodCallback(ByVal sender As Object, ByVal submitArgs As LogUpdateEventArgs)
            ProgressMeterCallback(sender, submitArgs)
        End Sub

        Public Sub SpellEffectMethodCallback(ByVal sender As Object, ByVal submitArgs As DAOCEventArgs)
            SpellEffectCallback(sender, submitArgs)
        End Sub

        Public Sub PlayerQuitMethodCallback(ByVal sender As Object, ByVal submitArgs As DAOCEventArgs)
            PlayerQuitCallback(sender, submitArgs)
        End Sub

        Protected MustOverride Sub LogUpdateCallback(ByVal sender As Object, ByVal submitArgs As LogUpdateEventArgs)
        Protected MustOverride Sub ZoneChangeCallback(ByVal sender As Object, ByVal submitArgs As ZoneEventArgs)
        Protected MustOverride Sub NewObjectCallback(ByVal sender As Object, ByVal submitArgs As DAOCEventArgs)
        Protected MustOverride Sub PetWindowUpdateCallback(ByVal sender As Object, ByVal submitArgs As DAOCEventArgs)
        Protected MustOverride Sub SpellCastCallback(ByVal sender As Object, ByVal submitArgs As DAOCEventArgs)
        Protected MustOverride Sub DialogMessageCallback(ByVal sender As Object, ByVal submitArgs As LogUpdateEventArgs)
        Protected MustOverride Sub ProgressMeterCallback(ByVal sender As Object, ByVal submitArgs As LogUpdateEventArgs)
        Protected MustOverride Sub SpellEffectCallback(ByVal sender As Object, ByVal submitArgs As DAOCEventArgs)
        Protected MustOverride Sub PlayerQuitCallback(ByVal sender As Object, ByVal submitArgs As DAOCEventArgs)

    End Class

    <Serializable()> _
    Public NotInheritable Class LogUpdateEventArgs
        Inherits EventArgs
        Private _sLine As String
        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal Text As String)
            MyBase.New()
            _sLine = Text
        End Sub

        Public ReadOnly Property sLine() As String
            Get
                Return _sLine
            End Get
        End Property
    End Class

    <Serializable()> _
    Public NotInheritable Class ZoneEventArgs
        Inherits EventArgs
        Private _Zone As Integer
        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal ZoneInfo As Integer)
            MyClass.New()
            _Zone = ZoneInfo
        End Sub

        Public ReadOnly Property Zone() As Integer
            Get
                Return _Zone
            End Get
        End Property
    End Class
    Public Delegate Sub OnLogUpdateEventHandler(ByVal Sender As Object, ByVal e As LogUpdateEventArgs)
    Public Delegate Sub OnZoneChangeEventHandler(ByVal Sender As Object, ByVal e As ZoneEventArgs)
    Public Delegate Sub OnNewObjectEventHandler(ByVal Sender As Object, ByVal e As DAOCEventArgs)
    Public Delegate Sub OnPetWindowUpdateEventHandler(ByVal Sender As Object, ByVal e As DAOCEventArgs)
    Public Delegate Sub OnSpellCastEventHandler(ByVal Sender As Object, ByVal e As DAOCEventArgs)
    Public Delegate Sub OnDialogMessageEventHandler(ByVal Sender As Object, ByVal e As LogUpdateEventArgs)
    Public Delegate Sub OnProgressMeterMessageEventHandler(ByVal Sender As Object, ByVal e As LogUpdateEventArgs)
    Public Delegate Sub OnSpellEffectEventHandler(ByVal Sender As Object, ByVal e As DAOCEventArgs)
    Public Delegate Sub OnPlayerQuitEventHandler(ByVal Sender As Object, ByVal e As DAOCEventArgs)
#End Region

    Public NotInheritable Class DAOCMain
        Inherits MarshalByRefObject
#Region "Structs/Variables"
        Private Enum PacketType
            Server
            Client
        End Enum
        Private mAccountCharacters As AccountCharInfoList
        Private mSelectedID As Integer
        Private mPlayer As DAOCLocalPlayer
        Private mZoneList As DAOCZoneInfoList
        Private mVendorItems As DAOCVendorItemList
        Private mMasterVendorList As DAOCMasterVendorList
        Private mGroup As Group
        Private mZone As DAOCZoneInfo
        Private mRegionID As Integer
        Private mDAOCObjs As DAOCObjectlist
        Private mLocalBufs As LocalBuffList
        Private mConcentrationBufs As ConcentrationBuffList
        Private mZoneID As Integer = -1
        Private mGroundTarget As Point3D
        Private mRunning As Boolean
        Private mMailer As MailSlot

        Private ServerPacketQueue As New Queue(Of DAOCPacket) ' = Queue(Of DAOCPacket).Synchronized(New Queue(Of DAOCPacket))
        Private ClientPacketQueue As New Queue(Of DAOCPacket) ' = Queue.Synchronized(New Queue(Of DAOCPacket))

        Private _XOffset As Integer = 0
        Private _YOffset As Integer = 0
        Private _UseDynamicOffsets As Boolean = False

        Private _LogPackets As Boolean = False

        Private plib As IPacketLib

        Private Version As eClientVersion = eClientVersion.VersionNotChecked
#End Region

#Region "Events"
        Public Event OnLogUpdate As OnLogUpdateEventHandler
        Public Event OnZoneChange As OnZoneChangeEventHandler
        Public Event OnNewDAOCObject As OnNewObjectEventHandler
        Public Event OnPetWindowUpdate As OnPetWindowUpdateEventHandler
        Public Event OnSpellCast As OnSpellCastEventHandler
        Public Event OnDialogMessage As OnDialogMessageEventHandler
        Public Event OnProgressMeter As OnProgressMeterMessageEventHandler
        Public Event OnSpellEffect As OnSpellEffectEventHandler
        Public Event OnPlayerQuit As OnPlayerQuitEventHandler
        '****************************************************************
        Public Event OnPlayerPosUpdate(ByVal Sender As DAOCMain)
        Public Event OnSkillLevelChanged(ByVal Sender As DAOCMain, ByVal e As DAOCEventArgs)
        Public Event OnVendorWindow(ByVal Sender As DAOCMain)
        Public Event OnCharacterLogin(ByVal Sender As DAOCMain)
        Public Event OnInventoryChanged(ByVal Sender As DAOCMain)
        Public Event OnMoneyChanged(ByVal Sender As DAOCMain)
        Public Event OnSetGroundTarget(ByVal Sender As DAOCMain)
        Public Event OnDAOCObjectMoved(ByVal Sender As DAOCMain, ByVal e As DAOCEventArgs)
        Public Event OnSelectedObjectChange(ByVal Sender As DAOCMain, ByVal e As DAOCEventArgs)
        Public Event OnDeleteDAOCObject(ByVal Sender As DAOCMain, ByVal e As DAOCEventArgs)
        Public Event OnPetUpdate(ByVal Sender As DAOCMain, ByVal ID As Integer, ByVal ID2 As Integer)
        Public Event OnLocalPlayerDeath(ByVal Sender As DAOCMain)
#End Region

#Region "Constructor"
        Public Sub New()

            mPlayer = New DAOCLocalPlayer
            mGroundTarget = New Point3D
            mZoneList = New DAOCZoneInfoList
            mLocalBufs = New LocalBuffList
            mConcentrationBufs = New ConcentrationBuffList

            Dim s1 As New System.IO.MemoryStream(My.Resources.MapInfo)
            mZoneList.LoadFromFile(s1)

            mAccountCharacters = New AccountCharInfoList
            mVendorItems = New DAOCVendorItemList
            mMasterVendorList = New DAOCMasterVendorList
            mDAOCObjs = New DAOCObjectlist
            mGroup = New Group

            mMailer = New MailSlot

            AddHandler mMailer.OnServerPacketReceived, AddressOf QueueServerPacket
            AddHandler mMailer.OnClientPacketReceived, AddressOf QueueClientPacket

            mRunning = True

            Dim t As Thread = New Thread(AddressOf StaleChecker)
            t.Name = "StaleChecker"
            t.Priority = ThreadPriority.Lowest
            t.Start()

            Dim Dispatcher As Thread = New Thread(AddressOf QueueThread)
            Dispatcher.Name = "Dispatcher"
            Dispatcher.IsBackground = True
            Dispatcher.Start()

        End Sub
#End Region

#Region "Packet Handlers"
        Private Sub QueueThread()
            While mRunning

                If Not ServerPacketQueue.Count = 0 Then
                    HandleServerPacket(ServerPacketQueue.Dequeue)
                End If

                If Not ClientPacketQueue.Count = 0 Then
                    HandleClientPacket(ClientPacketQueue.Dequeue)
                End If

                Thread.Sleep(10)
            End While
        End Sub

        Private Sub QueueServerPacket(ByVal Packet As DAOCPacket)
            ServerPacketQueue.Enqueue(Packet)
        End Sub

        Private Sub QueueClientPacket(ByVal Packet As DAOCPacket)
            ClientPacketQueue.Enqueue(Packet)
        End Sub

        Private Sub HandleServerPacket(ByVal Packet As DAOCPacket)

            If _LogPackets Then
                LogPackets(Packet, PacketType.Server)
                Packet.Position = 0
            End If

            Dim opcode As Byte = Packet.GetByte

            If Version = eClientVersion.VersionNotChecked Then
                Packet.Seek(2)
                Dim v As Integer = CInt(String.Format("{0}{1}{2}", Packet.GetByte, Packet.GetByte, Packet.GetByte))

                plib = BasePacketLib.CreatePacketLibForVersion(CType(v, eClientVersion), Me)

                Version = CType(v, eClientVersion)
            End If

            Try
                Select Case opcode
                    Case &H2
                        'Debug.WriteLine("ParseInventoryList 0x" & Hex(opcode))
                        plib.ParseInventoryList(Packet)
                    Case &H12
                        'Debug.WriteLine("Create Moving Object 0x" & Hex(opcode))
                        plib.ParseNewObject(Packet, DAOCObjectClass.ocVehicle)
                    Case &H15
                        'Debug.WriteLine("ObjectEquipment 0x" & Hex(opcode))
                        'ParseObjectEquipment(Packet)
                    Case &H16
                        'Debug.WriteLine("ParsePlayerStatsUpdate 0x" & Hex(opcode)) '9 10 11
                        plib.ParsePlayerStatsUpdate(Packet)
                    Case &H17
                        'Debug.WriteLine("Merchant Window 0x" & Hex(opcode))
                        plib.ParseVendorWindow(Packet)
                    Case &H1B
                        'Debug.WriteLine("Spell Effect Animation 0x" & Hex(opcode))
                        plib.ParseSpellEffectAnimation(Packet)
                    Case &H20 'DONE
                        'Debug.WriteLine("ParseCharacterLoginInit/Position Update 0x" & Hex(opcode)) '6
                        plib.ParseCharacterLoginInit(Packet)
                    Case &H22
                        'Debug.WriteLine("ParseSetEncryptionKey 0x" & Hex(opcode)) '1
                    Case &H28
                        'Debug.WriteLine("SessionID 0x" & Hex(opcode)) '1
                    Case &H29
                        'Debug.WriteLine("Ping Reply 0x" & Hex(opcode))
                    Case &H2A
                        'Debug.WriteLine("Login granted 0x" & Hex(opcode)) 'game version
                    Case &H2F
                        'Debug.WriteLine("UDP init reply 0x" & Hex(opcode))
                    Case &H4B
                        'Debug.WriteLine("Player Creation 0x" & Hex(opcode))
                        plib.ParseNewObject(Packet, DAOCObjectClass.ocPlayer)
                    Case &H70
                        'Debug.WriteLine("Player Group Update 0x" & Hex(opcode))
                        plib.ParseGroupWindowUpdate(Packet)
                    Case &H72
                        'Debug.WriteLine("Cast Player Spell 0x" & Hex(opcode))
                        plib.ParsePlayerCastSpell(Packet)
                    Case &H73
                        'Debug.WriteLine("Stop Animation 0x" & Hex(opcode))
                        plib.ParseInterruptSpell(Packet)
                    Case &H74
                        'Debug.WriteLine("Change Attack Mode 0x" & Hex(opcode))
                        plib.ParseAttackModeRequest(Packet)
                    Case &H75
                        'Debug.WriteLine("Display Shared Buffs 0x" & Hex(opcode))
                        plib.ParseConcentrationBuff(Packet)
                    Case &H7F
                        'Debug.WriteLine("Display Self Buffs 0x" & Hex(opcode))
                        plib.ParseLocalBuff(Packet)
                    Case &H81 'DONE
                        'Debug.WriteLine("show dialogue box 0x" & Hex(opcode))
                        plib.ParseDialogueUpdate(Packet)
                    Case &H83
                        'Debug.WriteLine("Journal From Server 0x" & Hex(opcode))
                    Case &H88 'DONE
                        'Debug.WriteLine("Pet Window 0x" & Hex(opcode))
                        plib.ParsePetWindowUpdate(Packet)
                    Case &H89
                        'Debug.WriteLine("Revive 0x" & Hex(opcode))
                        plib.ParseRevive(Packet)
                    Case &H8D
                        'Debug.WriteLine("Player Model Change 0x" & Hex(opcode))
                        plib.ParsePlayerModelChange(Packet)
                    Case &H91
                        'Debug.WriteLine("Experience Update 0x" & Hex(opcode))
                    Case &H99
                        'Debug.WriteLine("Open Door 0x" & Hex(opcode))
                    Case &H9E
                        'Debug.WriteLine("Region init 0x" & Hex(opcode))
                    Case &HA1 'DONE
                        'Debug.WriteLine("MobUpdate 0x" & Hex(opcode))
                        plib.ParseMobUpdate(Packet)
                    Case &HA2 'DONE
                        'Debug.WriteLine("Object removal 0x" & Hex(opcode))
                        plib.ParseRemoveObject(Packet)
                    Case &HA4
                        'Debug.WriteLine("Player Quit 0x" & Hex(opcode))
                        plib.ParsePlayerQuit(Packet)
                    Case &HA9 'DONE
                        'Debug.WriteLine("ParsePlayerPosUpdate 0x" & Hex(opcode))
                        plib.ParsePlayerPosUpdate(Packet)
                    Case &HAC
                        'Debug.WriteLine("Destroy 0x" & Hex(opcode))
                    Case &HAD 'DONE
                        'Debug.WriteLine("Self health Update And Sit Stand  0x" & Hex(opcode)) '8
                        plib.ParseLocalHealthUpdate(Packet)
                    Case &HAE
                        'Debug.WriteLine("Dead 0x" & Hex(opcode))
                        plib.ParseDead(Packet)
                    Case &HAF 'DONE
                        'Debug.WriteLine("Console Message 0x" & Hex(opcode))
                        plib.ParseLogUpdate(Packet)
                    Case &HB1 'DONE
                        'Debug.WriteLine("ParseRegionServerInformation 0x" & Hex(opcode)) '4
                    Case &HB6
                        'Debug.WriteLine("Speed Update 0x" & Hex(opcode))
                        plib.ParseLocalPlayerSpeedUpdate(Packet)
                    Case &HB7 'DONE
                        'Debug.WriteLine("ParseSetPlayerRegion 0x" & Hex(opcode))
                        plib.ParseSetPlayerRegion(Packet)
                    Case &HBA
                        'Debug.WriteLine("ParsePlayerHeadUpdate 0x" & Hex(opcode))
                        plib.ParsePlayerHeadUpdate(Packet)
                    Case &HBC 'DONE
                        'Debug.WriteLine("Combat 0x" & Hex(opcode))
                        plib.ParsePlayerUpdateHitPoints(Packet)
                    Case &HBD
                        'Debug.WriteLine("Encumbrance 0x" & Hex(opcode))
                    Case &HC8
                        'Debug.WriteLine("Ride 0x" & Hex(opcode))
                    Case &HD0 'DONE
                        'Debug.WriteLine("Pet Update 0x" & Hex(opcode))
                        plib.ParsePetUpdate(Packet)
                    Case &HD9 'DONE
                        'Debug.WriteLine("Object Creation 0x" & Hex(opcode))
                        plib.ParseNewObject(Packet, DAOCObjectClass.ocObject)
                    Case &HDA 'DONE
                        'Debug.WriteLine("Mob Creation 0x" & Hex(opcode))
                        plib.ParseNewObject(Packet, DAOCObjectClass.ocMob)
                    Case &HDB
                        'Debug.WriteLine("Model Change 0x" & Hex(opcode))
                    Case &HDF
                        'Debug.WriteLine("Ground Assist Reply 0x" & Hex(opcode))
                    Case &HE1 'stealthed player within 2000 units
                        'Dim wID As Integer = Packet.GetShort()
                        'Debug.WriteLine(wID.ToString & Now)
                        'Debug.WriteLine("SendObjectDelete 0x" & Hex(opcode))
                    Case &HEA
                        'Debug.WriteLine("Trade Window 0x" & Hex(opcode))
                    Case &HED
                        'Debug.WriteLine("Product 0x" & Hex(opcode))
                    Case &HEE
                        'Debug.WriteLine("Player Stats 0x" & Hex(opcode))
                    Case &HEF
                        'Debug.WriteLine("Timer Dialog 0x" & Hex(opcode))
                    Case &HF3
                        'Debug.WriteLine("Timer For Crafting 0x" & Hex(opcode))
                        plib.ParseProgressMeter(Packet)
                    Case &HF9
                        'Debug.WriteLine("Player Animation 0x" & Hex(opcode))
                    Case &HFA
                        'Debug.WriteLine("ParseMoneyUpdate 0x" & Hex(opcode))
                        'ParseMoneyUpdate(Packet)
                    Case &HFB
                        'Debug.WriteLine("character stats update 0x" & Hex(opcode))
                    Case &HFD  'DONE
                        'Debug.WriteLine("ParseAccountCharacters 0x" & Hex(opcode)) '3
                        plib.ParseAccountCharacters(Packet)
                    Case &HFE
                        'Debug.WriteLine("Realm Check 0x" & Hex(opcode))
                        'Debug.WriteLine(Packet.ToString)
                    Case Else
                        'Debug.WriteLine("Other packet from Server 0x" & Hex(opcode))
                        'Debug.WriteLine(Packet.ToString)
                End Select
            Catch ex As Exception
                Debug.WriteLine(ex.Message)
                Debug.WriteLine(ex.StackTrace)
            Finally
                Packet = Nothing
            End Try

        End Sub

        Private Sub HandleClientPacket(ByVal Packet As DAOCPacket)

            If _LogPackets Then
                LogPackets(Packet, PacketType.Client)
                Packet.Position = 0
            End If

            Dim opcode As Byte = Packet.GetByte

            Try
                Select Case opcode
                    Case &H10 'DONE
                        'Debug.WriteLine("Select character 0x" & Hex(opcode)) '2 5 7
                        plib.ParseCharacterActivationRequest(Packet)
                    Case &H74 '&HDC
                        'this is a request only, use the server packet
                        'Debug.WriteLine("Attack Mode Change Request 0x" & Hex(opcode))
                    Case &H78 '&HD0
                        'Debug.WriteLine("ParseRequestBuyItem 0x" & Hex(opcode))
                        'ParseRequestBuyItem(Packet)
                    Case &H7D '&HD5
                        'Debug.WriteLine("Send Cast Spell 0x" & Hex(opcode))
                        'Debug.WriteLine(Packet.ToString)
                    Case &H8A '&H22
                        'Debug.WriteLine("Pet Command 0x" & Hex(opcode))
                    Case &H90
                        'Debug.WriteLine("Zone Change request 0x" & Hex(opcode))
                    Case &HA3 'Ping
                        'Debug.WriteLine("Ping Server 0x" & Hex(opcode))
                    Case &HA7
                        'Debug.WriteLine("Login Request 0x" & Hex(opcode))
                    Case &HA9 'DONE
                        'Debug.WriteLine("ParseLocalPosUpdateFromClient 0x" & Hex(opcode))
                        plib.ParseLocalPosUpdateFromClient(Packet)
                    Case &HAC
                        'Debug.WriteLine("Unknown 0x" & Hex(opcode))
                    Case &HAF '&H7
                        'Debug.WriteLine("ParseCommandFromClient 0x" & Hex(opcode))
                        'ParseCommandFromClient(Packet)
                    Case &HB0 '&H18
                        'Debug.WriteLine("ParseSelectedIDUpdate 0x" & Hex(opcode))
                        plib.ParseSelectedIDUpdate(Packet)
                    Case &HBA 'DONE
                        'Debug.WriteLine("ParseLocalHeadUpdateFromClient 0x" & Hex(opcode))
                        plib.ParseLocalHeadUpdateFromClient(Packet)
                    Case &HC7 '&H6F
                        'Debug.WriteLine("Sit/Stand 0x" & Hex(opcode))
                    Case &HEC '&H44
                        'Debug.WriteLine("ParseSetGroundTarget 0x" & Hex(opcode))
                        plib.ParseSetGroundTarget(Packet)
                    Case &HED '&H45
                        ' Request Crafting begin
                        'Debug.WriteLine("Crafting begin 0x" & Hex(opcode))
                    Case &HF4
                        'Debug.WriteLine("Encryption request 0x" & Hex(opcode))
                    Case &HFC
                        'Debug.WriteLine("Request client realm 0x" & Hex(opcode))
                    Case &HFF
                        'Debug.WriteLine("Create character 0x" & Hex(opcode))
                    Case Else
                        'Debug.WriteLine("Other Packet From Client 0x" & Hex(opcode))
                        'Debug.WriteLine(Packet.ToString)
                End Select
            Catch ex As Exception
                Debug.WriteLine(ex.StackTrace)
            Finally
                Packet = Nothing
            End Try

        End Sub
#End Region

#Region "Misc functions"
        Private Sub LogPackets(ByVal Packet As DAOCPacket, ByVal p As PacketType)
            Dim myStreamWriter As StreamWriter

            Dim strString As String

            If p = PacketType.Server Then
                strString = String.Format("<RECV Time:{0} Code:0x{1:X2} Len:{2} >", Now.ToLongTimeString, Hex(Packet.GetByte.ToString), Packet.Size.ToString)
            Else
                strString = String.Format("<SEND Time:{0} Code:0x{1:X2} Len:{2} >", Now.ToLongTimeString, Hex(Packet.GetByte.ToString), Packet.Size.ToString)
            End If

            strString &= vbCrLf
            strString &= Packet.ToString

            myStreamWriter = File.AppendText("packets.log")

            Try
                myStreamWriter.WriteLine(strString)
                myStreamWriter.Flush()
            Catch ex As Exception
                Debug.Write(ex.Message)
            Finally
                If Not myStreamWriter Is Nothing Then
                    myStreamWriter.Close()
                End If
            End Try
        End Sub

        Public Sub ResetPlayer()
            mPlayer.Combat = False 'RESET THIS OR IT WILL STAY TRUE
            mPlayer.IsSitting = False
            mPlayer.PlayerStealthed = False
            mPlayer.ImDead = False
        End Sub

        Private Sub StaleChecker()

            While mRunning
                mDAOCObjs.CheckStale()
                Thread.Sleep(250)
            End While

        End Sub

        Protected Overrides Sub Finalize()
            mRunning = False
            mGroup = Nothing
            mVendorItems = Nothing
            mDAOCObjs = Nothing
            mMasterVendorList = Nothing
            mAccountCharacters = Nothing
            mGroundTarget = Nothing
            mZoneList = Nothing
            mPlayer = Nothing
            mConcentrationBufs = Nothing
            mLocalBufs = Nothing
            MyBase.Finalize()
        End Sub

        Public Overrides Function InitializeLifetimeService() As Object
            Return Nothing
        End Function

        Public Sub CleanUp()
            mMailer.CleanUp()

            mRunning = False
            mDAOCObjs = Nothing
            mAccountCharacters = Nothing
            mGroundTarget = Nothing
            mZoneList = Nothing
            mPlayer = Nothing
            mConcentrationBufs = Nothing
            mLocalBufs = Nothing
        End Sub

        Public Sub ClearDAOCObjectList()
            mDAOCObjs.ClearTable()
        End Sub

        Private Sub SetZoneBase(ByRef aZoneBase As DAOCZoneInfo, ByVal iZoneBase As Integer)
            If Not (aZoneBase Is Nothing) AndAlso (aZoneBase.ZoneNum = iZoneBase) Then
                Exit Sub
            ElseIf Not (mZone Is Nothing) AndAlso (mZone.ZoneNum = iZoneBase) Then
                aZoneBase = mZone
            Else
                aZoneBase = mZoneList.FindZone(iZoneBase)
            End If
        End Sub

        Public Sub Clear()
            mDAOCObjs.ClearTable()
            mPlayer.Clear()
            mRegionID = 0
            mSelectedID = 0
            mGroundTarget.Clear()
            mConcentrationBufs.Clear()
            mLocalBufs.ClearTable()
        End Sub

        Private Function GetSelectedObject() As DAOCObject
            If mSelectedID = mPlayer.SpawnID Then
                Return mPlayer
            Else
                Return mDAOCObjs.FindBySpawnID(mSelectedID)
            End If
        End Function

        Private Sub SetSelectedObject(ByVal Value As DAOCObject)
            mSelectedID = Value.SpawnID
            RaiseEvent OnSelectedObjectChange(Me, New DAOCEventArgs(Value))
        End Sub

        Public Function CheckZoneChanged() As Boolean
            Dim bZoneChanged As Boolean = False

            If mZone Is Nothing Then 'zone is set at login by this called from ParseCharacterLoginInit
                Dim x As Integer = mPlayer.X
                Dim y As Integer = mPlayer.Y
                If UseDynamicOffsets Then
                    x -= XOffset
                    y -= YOffset
                End If
                mZone = mZoneList.FindZoneForPoint(mRegionID, x, y)
                If Not (mZone Is Nothing) Then
                    bZoneChanged = True
                    mZoneID = mZone.ZoneNum
                End If
            End If

            If Not (mZone Is Nothing) AndAlso Not mZone.ZoneNum = mZoneID Then  'after that it's set by ParseLocalPosUpdateFromClient
                mZone = mZoneList.FindZone(mZoneID)
                If Not (mZone Is Nothing) Then
                    bZoneChanged = True
                    mZoneID = mZone.ZoneNum
                End If
            End If
            If bZoneChanged Then
                DoOnZoneChange()
            End If
            Return bZoneChanged
        End Function

        Public Function SetActiveCharacterByName(ByVal sName As String) As AccountCharInfo
            Dim pAcctChar As AccountCharInfo
            pAcctChar = mAccountCharacters.FindOrAddChar(sName)
            mPlayer.Level = pAcctChar.Level
            mPlayer.Name = pAcctChar.Name
            mPlayer.PlayerRealm = pAcctChar.Realm
            DoOnSetRegionID(pAcctChar.RegionID)
            Return pAcctChar
        End Function

        Public Sub MergeVendorItemsToMaster()
            mMasterVendorList.AddOrUpdate(mVendorItems)
        End Sub

        Public Sub ResetPlayersInGroup()
            Dim pObj As DAOCObject
            Dim i As Integer

            For i = 0 To mDAOCObjs.ObjectTable.Count - 1
                pObj = CType(mDAOCObjs.ObjectTable(i), DAOCObject)
                If TypeOf pObj Is DAOCPlayer Then
                    DirectCast(pObj, DAOCPlayer).IsInGroup = False
                End If
            Next
        End Sub

        Public Function GlobalToZoneX(ByVal X As Integer) As Integer
            If (UseDynamicOffsets) Then
                Return X - XOffset
            Else
                Return mZone.WorldToZoneX(X)
            End If
        End Function

        Public Function GlobalToZoneY(ByVal Y As Integer) As Integer
            If (UseDynamicOffsets) Then
                Return Y - YOffset
            Else
                Return mZone.WorldToZoneY(Y)
            End If
        End Function

        Public Function ZoneToGlobalX(ByVal X As Integer, ByVal ZoneID As Integer) As Integer
            If (UseDynamicOffsets) Then
                Return X + XOffset
            Else
                Dim Zone As DAOCZoneInfo = mZoneList.FindZone(ZoneID)
                Return Zone.ZoneToWorldX(X)
            End If
        End Function

        Public Function ZoneToGlobalY(ByVal Y As Integer, ByVal ZoneID As Integer) As Integer
            If (UseDynamicOffsets) Then
                Return Y + YOffset
            Else
                Dim Zone As DAOCZoneInfo = mZoneList.FindZone(ZoneID)
                Return Zone.ZoneToWorldY(Y)
            End If
        End Function
#End Region

#Region "Do Functions"
        <OneWayAttribute()> _
       Public Sub DoOnPlayerQuit(ByVal killclient As Boolean)
            RaiseEvent OnPlayerQuit(Me, New DAOCEventArgs)
        End Sub

        <OneWayAttribute()> _
        Public Sub DoOnLogUpdate(ByVal sLine As String)
            RaiseEvent OnLogUpdate(Me, New LogUpdateEventArgs(sLine))
        End Sub

        <OneWayAttribute()> _
        Public Sub DoOnSkillLevelChanged(ByVal AItem As DAOCNameValuePair)
            RaiseEvent OnSkillLevelChanged(Me, New DAOCEventArgs(AItem))
        End Sub

        <OneWayAttribute()> _
        Public Sub DoOnVendorWindow()
            RaiseEvent OnVendorWindow(Me)
        End Sub

        <OneWayAttribute()> _
        Public Sub DoOnProgressMeterClose()
            RaiseEvent OnProgressMeter(Me, New LogUpdateEventArgs(String.Empty))
        End Sub

        <OneWayAttribute()> _
        Public Sub DoOnInventorychanged()
            RaiseEvent OnInventoryChanged(Me)
        End Sub

        <OneWayAttribute()> _
        Public Sub DoOnProgressMeterOpen(ByVal AMessage As String)
            RaiseEvent OnProgressMeter(Me, New LogUpdateEventArgs(AMessage))
        End Sub

        <OneWayAttribute()> _
       Public Sub DoOnPetWindowUpdate(ByVal Pet As DAOCPet)
            RaiseEvent OnPetWindowUpdate(Me, New DAOCEventArgs(Pet))
        End Sub

        <OneWayAttribute()> _
        Public Sub DoOnPetUpdate(ByVal ID As Integer, ByVal ID2 As Integer)
            RaiseEvent OnPetUpdate(Me, ID, ID2)
        End Sub

        <OneWayAttribute()> _
        Public Sub DoOnPopupMessage(ByVal AMessage As String)
            RaiseEvent OnDialogMessage(Me, New LogUpdateEventArgs(AMessage))
        End Sub

        <OneWayAttribute()> _
        Public Sub DoOnSpellCast(ByVal s As SpellCast)
            RaiseEvent OnSpellCast(Me, New DAOCEventArgs(s))
        End Sub

        <OneWayAttribute()> _
        Public Sub DoOnDAOCObjectMoved(ByVal pDAOCObject As DAOCObject)
            RaiseEvent OnDAOCObjectMoved(Me, New DAOCEventArgs(pDAOCObject))
        End Sub

        <OneWayAttribute()> _
        Public Sub DoOnDeleteDAOCObject(ByVal AObject As DAOCObject)
            RaiseEvent OnDeleteDAOCObject(Me, New DAOCEventArgs(AObject))
        End Sub

        <OneWayAttribute()> _
        Public Sub DoOnNewDAOCObject(ByVal AObject As DAOCObject)
            RaiseEvent OnNewDAOCObject(Me, New DAOCEventArgs(AObject))
        End Sub

        <OneWayAttribute()> _
        Public Sub DoOnCharacterLogin()
            RaiseEvent OnCharacterLogin(Me)
        End Sub

        <OneWayAttribute()> _
        Public Sub DoOnSetRegionID(ByVal ARegion As Integer)
            Dim pAcctChar As AccountCharInfo

            If mRegionID = ARegion Then
                Exit Sub
            End If

            mGroundTarget.Clear()
            mRegionID = ARegion
            If mPlayer.Name <> String.Empty Then
                pAcctChar = mAccountCharacters.FindOrAddChar(mPlayer.Name)
                If Not (pAcctChar Is Nothing) Then
                    pAcctChar.RegionID = mRegionID
                End If
            End If
            ClearDAOCObjectList()
        End Sub

        <OneWayAttribute()> _
        Public Sub DoOnPlayerPosUpdate()
            RaiseEvent OnPlayerPosUpdate(Me)
        End Sub

        <OneWayAttribute()> _
        Public Sub DoOnSetGroundTarget()
            RaiseEvent OnSetGroundTarget(Me)
        End Sub

        <OneWayAttribute()> _
        Public Sub DoOnSelectedObjectChanged(ByVal AObject As DAOCObject)
            RaiseEvent OnSelectedObjectChange(Me, New DAOCEventArgs(AObject))
        End Sub

        <OneWayAttribute()> _
        Public Sub DoOnZoneChange()
            RaiseEvent OnZoneChange(Me, New ZoneEventArgs(mZone.ZoneNum))
        End Sub

        <OneWayAttribute()> _
        Public Sub DoOnSpellEffectAnimation(ByVal s As SpellEffectAnimation)
            RaiseEvent OnSpellEffect(Me, New DAOCEventArgs(s))
        End Sub
#End Region

#Region "Properties"
        Public Property ZoneID() As Integer
            Get
                Return mZoneID
            End Get
            Set(ByVal Value As Integer)
                mZoneID = Value
            End Set
        End Property

        Public Property AccountCharacters() As AccountCharInfoList
            Get
                Return mAccountCharacters
            End Get
            Set(ByVal Value As AccountCharInfoList)
                mAccountCharacters = Value
            End Set
        End Property

        Public Property XOffset() As Integer
            Get
                Return _XOffset
            End Get
            Set(ByVal Value As Integer)
                _XOffset = Value
            End Set
        End Property

        Public Property YOffset() As Integer
            Get
                Return _YOffset
            End Get
            Set(ByVal Value As Integer)
                _YOffset = Value
            End Set
        End Property

        Public Property UseDynamicOffsets() As Boolean
            Get
                Return _UseDynamicOffsets
            End Get
            Set(ByVal Value As Boolean)
                _UseDynamicOffsets = Value
            End Set
        End Property

        Public ReadOnly Property DAOCObjects() As DAOCObjectlist
            Get
                Return mDAOCObjs
            End Get
        End Property

        Public ReadOnly Property Player() As DAOCLocalPlayer
            Get
                Return mPlayer
            End Get
        End Property

        Public Property RegionID() As Integer
            Get
                Return mRegionID
            End Get
            Set(ByVal Value As Integer)
                mRegionID = Value
            End Set
        End Property

        Public Property SelectedID() As Integer
            Get
                Return mSelectedID
            End Get
            Set(ByVal Value As Integer)
                mSelectedID = Value
            End Set
        End Property

        Public Property SelectedObject() As DAOCObject
            Get
                Return GetSelectedObject()
            End Get
            Set(ByVal Value As DAOCObject)
                SetSelectedObject(Value)
            End Set
        End Property

        Public Property Zone() As DAOCZoneInfo
            Get
                Return mZone
            End Get
            Set(ByVal Value As DAOCZoneInfo)
                mZone = Value
            End Set
        End Property

        Public ReadOnly Property ZoneList() As DAOCZoneInfoList
            Get
                Return mZoneList
            End Get
        End Property

        Public ReadOnly Property GroundTarget() As Point3D
            Get
                Return mGroundTarget
            End Get
        End Property

        Public ReadOnly Property VendorItems() As DAOCVendorItemList
            Get
                Return mVendorItems
            End Get
        End Property

        Public ReadOnly Property MasterVendorList() As DAOCMasterVendorList
            Get
                Return mMasterVendorList
            End Get
        End Property

        Public ReadOnly Property ConcentrationBufs() As ConcentrationBuffList
            Get
                Return mConcentrationBufs
            End Get
        End Property

        Public ReadOnly Property LocalBufs() As LocalBuffList
            Get
                Return mLocalBufs
            End Get
        End Property

        Public ReadOnly Property [Group]() As Group
            Get
                Return mGroup
            End Get
        End Property

        Public WriteOnly Property LogPacketsToFile() As Boolean
            Set(ByVal Value As Boolean)
                _LogPackets = Value
            End Set
        End Property

        Public ReadOnly Property MovingObject() As DAOCMovingObject
            Get
                Return mPlayer
            End Get
        End Property
#End Region

    End Class
End Namespace
'//all coordinates are adjusted to global coordinates even if an object is in another zone
'//when it's converted back to local it's converted based on the player's zone so it shows on the radar like it's in the same zone

'return LastAttackTick+100 >= GameTimer.CurrentTick || LastAttackedByEnemyTick+100 >= GameTimer.CurrentTick;