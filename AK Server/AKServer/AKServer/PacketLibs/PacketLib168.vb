Namespace DAoCServer
    Public Class PacketLib168
        Inherits BasePacketLib

        Private _client As DAOCMain

        Public Sub New(ByVal client As DAOCMain)
            MyBase.New(client)

            _client = client
        End Sub

        Public Overrides Sub ParsePlayerQuit(ByVal pPacket As DAOCPacket)
            Dim killclient As Boolean = CBool(pPacket.GetByte)
            _client.DoOnPlayerQuit(killclient)
        End Sub

        Public Overrides Sub ParsePlayerModelChange(ByVal pPacket As DAOCPacket)
            Dim id As Integer = pPacket.GetShort

            If id = _client.Player.SpawnID Then
                Dim mode As Byte = pPacket.GetByte
                If mode = 2 Then
                    _client.Player.PlayerStealthed = False
                ElseIf mode = 3 Then
                    _client.Player.PlayerStealthed = True
                End If
            End If

            'Wire Mode (0x1 = on, 0x0 = off, 0x3 = transparent)
        End Sub

        Public Overrides Sub ParseRevive(ByVal pPacket As DAOCPacket)
            Dim wID As Integer = pPacket.GetShort

            If wID = _client.Player.SpawnID Then
                _client.ResetPlayer()
            End If

        End Sub

        Public Overrides Sub ParseDead(ByVal pPacket As DAOCPacket)
            Dim wID As Integer = pPacket.GetShort

            If wID = _client.Player.SpawnID Then
                _client.Player.ImDead = True
            End If

        End Sub

        Public Overrides Sub ParseLocalPlayerSpeedUpdate(ByVal pPacket As DAOCPacket)
            _client.Player.Speed = pPacket.GetShort 'speed (in percent %) base speed for players is 191 <=> 100%
            _client.Player.Stunned = CBool(pPacket.GetByte)
        End Sub

        Public Overrides Sub ParseObjectEquipment(ByVal pPacket As DAOCPacket)
            'not using
            Dim infoID As Integer
            Dim objcount As Integer
            Dim slot As Byte
            Dim obj_list As Integer
            Dim obj_index As Integer
            Dim obj_color As Short

            Dim ii As DAOCInventoryItem
            Dim mob As DAOCObjectInterface

            infoID = pPacket.GetShort
            mob = _client.DAOCObjects.FindBySpawnID(infoID)

            If mob Is Nothing Then
                Return
            End If

            DirectCast(mob, DAOCMovingObject).Speed = 0
            pPacket.Seek(2)
            objcount = pPacket.GetByte
            slot = 0

            While objcount <> 0
                slot = pPacket.GetByte
                Select Case slot
                    Case 10, 11, 12, 13, 21, 22, 23, 25, 26, 27, 28
                        pPacket.Seek(2) 'Model + info bits

                        'TODO double check these
                        If CBool(obj_list And &H40) Then
                            obj_color = pPacket.GetByte
                        Else
                            obj_color = 0
                        End If

                        If CBool(obj_list And &H20) Then
                            pPacket.GetShort()
                        End If
                        If CBool(obj_list And &H80) Then
                            pPacket.GetShort()
                        End If
                    Case Else
                        obj_list = 0
                        obj_index = 0
                        obj_color = 0
                End Select
                ii = New DAOCInventoryItem
                ii.Slot = slot
                ii.Color = obj_color
                ' obj_list and obj_index and such are used to identify class
                'mob.Inventory.Add(ii)
                If Not CBool((slot And &H80)) Then
                    objcount -= 1
                End If
            End While

        End Sub

        Public Overrides Sub ParseInventoryList(ByVal ppacket As DAOCPacket)
            Dim iItemCount As Integer
            Dim pTmpItem As DAOCInventoryItem

            iItemCount = ppacket.GetByte
            ppacket.Seek(1)

            '0=right hand, 1=left hand, 2=two-hand, 3=range, F=none
            '0x01 byte  = Left/Right Hand
            '                 bit 4-7 left hand weapon (15 for none)
            '                 bit 3-0 right hand weapon (15 for none) 
            Dim m_visibleActiveWeaponSlots As Integer = ppacket.GetByte

            If m_visibleActiveWeaponSlots > 0 Then
                Dim rightHand As Integer = m_visibleActiveWeaponSlots And &HF
                Dim leftHand As Integer = m_visibleActiveWeaponSlots >> 4
                _client.Player.LeftHand = CType(leftHand, WeaponType)
                _client.Player.RightHand = CType(rightHand, WeaponType)
            End If

            ppacket.Seek(1)

            Try
                While (iItemCount > 0) AndAlso (ppacket.Position < ppacket.Size)
                    pTmpItem = New DAOCInventoryItem
                    pTmpItem.Slot = ppacket.GetByte
                    pTmpItem.Level = ppacket.GetByte
                    ppacket.Seek(6)
                    'ppacket.Seek(1) 'ItemValue1
                    'ppacket.Seek(1) 'ItemValue2
                    'ppacket.Seek(1) 'Itemflag
                    'ppacket.Seek(1) 'Bit Structure
                    'ppacket.Seek(1) 'ItemWeight
                    pTmpItem.Condition = ppacket.GetByte
                    pTmpItem.Durability = ppacket.GetByte
                    pTmpItem.Quality = ppacket.GetByte
                    pTmpItem.Bonus = ppacket.GetByte
                    ppacket.Seek(2) 'ItemDatabaseID
                    pTmpItem.Color = CShort(ppacket.GetShort)
                    ppacket.Seek(3) 'ItemWeaponProc extra byte here
                    pTmpItem.Description = ppacket.GetPascalString

                    _client.Player.Inventory.TakeItem(pTmpItem)
                    iItemCount -= 1
                End While
            Catch ex As Exception
                Debug.WriteLine(ex.Message)
            Finally
                _client.DoOnInventorychanged()
            End Try

        End Sub

        Public Overrides Sub ParseGroupWindowUpdate(ByVal pPacket As DAOCPacket)
            Dim key As Integer = pPacket.GetByte
            Dim aGroupMember As GroupMember = DirectCast(_client.Group.GroupMemberTable(key), GroupMember)
            aGroupMember.Health = pPacket.GetByte
            aGroupMember.Mana = pPacket.GetByte
            aGroupMember.Endurance = pPacket.GetByte
            aGroupMember.Status = CType(pPacket.GetByte, PlayerInGroupStatus)
        End Sub

        Public Overrides Sub ParseGroupMembersUpdate(ByVal pPacket As DAOCPacket)
            Dim Count As Integer = pPacket.GetByte

            _client.Group.ClearTable()
            _client.ResetPlayersInGroup()

            If Count = 0 Then Exit Sub

            pPacket.Seek(2)    ' Don't know what they are .. usually 01 00

            For i As Integer = 1 To Count
                Dim aGroupMember As New GroupMember
                aGroupMember.Index = i
                aGroupMember.Level = pPacket.GetByte
                aGroupMember.Health = pPacket.GetByte
                aGroupMember.Mana = pPacket.GetByte
                aGroupMember.Endurance = pPacket.GetByte
                aGroupMember.Status = CType(pPacket.GetByte, PlayerInGroupStatus)
                aGroupMember.ID = pPacket.GetShort
                Dim pObj As DAOCObject = _client.DAOCObjects.FindBySpawnID(aGroupMember.ID)
                If (pObj IsNot Nothing) AndAlso (pObj.GetObjectClass = DAOCObjectClass.ocPlayer) Then
                    DirectCast(pObj, DAOCPlayer).IsInGroup = True
                ElseIf aGroupMember.ID <> _client.Player.SpawnID Then
                    'Log('GroupMembersUpdate: Can not find player by InfoID 0x' + IntToHex(wID, 2));
                End If
                aGroupMember.Name = pPacket.GetPascalString
                aGroupMember.Class = pPacket.GetPascalString
                _client.Group.AddOrReplace(aGroupMember)
            Next

        End Sub

        Public Overrides Sub ParseVendorWindow(ByVal pPacket As DAOCPacket)
            Dim iItemDescs As Integer
            Dim iPage As Integer
            Dim pItem As DAOCVendorItem

            iItemDescs = pPacket.GetByte
            iPage = pPacket.GetShort
            pPacket.Seek(1)

            If iPage = 0 Then
                _client.MergeVendorItemsToMaster()
                _client.VendorItems.Clear()
            End If

            If (_client.SelectedObject IsNot Nothing) AndAlso (TypeOf _client.SelectedObject Is DAOCMob) Then
                _client.VendorItems.Vendor.Assign(CType(_client.SelectedObject, DAOCMob))
            End If

            While iItemDescs > 0
                pItem = New DAOCVendorItem
                _client.VendorItems.Add(pItem)
                pItem.Page = iPage
                pItem.Position = pPacket.GetByte
                pItem.Quantity = pPacket.GetShort
                pPacket.Seek(6)
                pItem.Cost = pPacket.GetLong
                pPacket.Seek(2)
                pItem.Name = pPacket.GetANullTermString(0)
                iItemDescs -= 1
            End While

            _client.DoOnVendorWindow()
        End Sub

        Public Overrides Sub ParseLogUpdate(ByVal pPacket As DAOCPacket)
            pPacket.Seek(8)
            Dim sLine As String = pPacket.GetANullTermString(0)
            _client.DoOnLogUpdate(sLine)
        End Sub

        Public Overrides Sub ParseProgressMeter(ByVal pPacket As DAOCPacket)
            Dim iDuration As Integer
            Dim sMessage As String
            iDuration = pPacket.GetShort
            pPacket.Seek(2)
            sMessage = pPacket.GetANullTermString(0)
            If iDuration = 0 Then
                _client.DoOnProgressMeterClose()
            Else
                _client.DoOnProgressMeterOpen(sMessage)
            End If
        End Sub

        Public Overrides Sub ParsePetWindowUpdate(ByVal pPacket As DAOCPacket)
            '0x00 - short- Mob id of pet
            '0x02 - unused
            '0x04 - byte - 0-close window, 1-update window state, 2-open window
            '0x05 - byte - 1-aggressive, 2-defensive, 3-passive
            '0x06 - byte - 1-follow, 2-stay, 3-goto, 4-here
            '0x07 - unused
            '0x08 - null-terminated (byte) list of shorts - spell icons on pet

            Dim mPet As New DAOCPet
            Try
                mPet.ID = pPacket.GetShort
                If mPet.ID > 0 Then
                    pPacket.Seek(2)
                    mPet.Type = pPacket.GetByte
                    mPet.PetState = CType(pPacket.GetByte, DAOCPet.psState)
                    mPet.PetPosition = CType(pPacket.GetByte, DAOCPet.ppPosition)
                    pPacket.Seek(1)
                    'While pPacket.PeekByte <> 0
                    '    mPet.Buffs.Add(pPacket.GetShort)
                    'End While

                    Try
                        Dim c As Byte = pPacket.GetByte
                        For i As Integer = 1 To c
                            mPet.Buffs.Add(pPacket.GetShort)
                        Next
                    Catch ex As Exception
                        mPet.Buffs = Nothing
                    End Try

                    _client.DoOnPetWindowUpdate(mPet)
                End If

            Catch ex As Exception
            Finally
                mPet = Nothing
            End Try

        End Sub

        Public Overrides Sub ParsePetUpdate(ByVal pPacket As DAOCPacket)
            '0x02 bytes = ObjectID first additional pet cast 
            '0x02 bytes  = ObjectID of parent pet then object id of next pet cast
            Try
                _client.DoOnPetUpdate(pPacket.GetShort, pPacket.GetShort)
            Catch ex As Exception

            End Try

        End Sub

        Public Overrides Sub ParseDialogueUpdate(ByVal pPacket As DAOCPacket)
            Dim sMessage As String
            pPacket.Seek(12)
            sMessage = pPacket.GetANullTermString(0)
            sMessage = sMessage.Replace(Chr(10), " ")  'replace line break
            _client.DoOnPopupMessage(sMessage)
        End Sub

        Public Overrides Sub ParseInterruptSpell(ByVal pPacket As DAOCPacket)

            Dim CasterID As Integer = pPacket.GetShort

            If CasterID = _client.Player.SpawnID Then
                _client.Player.CastTime = Now
            Else
                Dim pDAOCObject As DAOCObject = _client.DAOCObjects.FindBySpawnID(CasterID)

                If (pDAOCObject IsNot Nothing) AndAlso (TypeOf pDAOCObject Is DAOCMovingObject) Then
                    With DirectCast(pDAOCObject, DAOCMovingObject)
                        .CastTime = Now
                    End With
                End If
            End If

        End Sub

        Public Overrides Sub ParseSpellEffectAnimation(ByVal pPacket As DAOCPacket)
            '0x02 bytes = object id of caster
            '0x02 bytes = spellid
            '0x02 bytes = object id of target
            '0x02 bytes = Time in tenths of a second that bolts should take to hit target (0 for nonbolt spells)
            '0x01 byte  = 1=refreshed spell, don't do sound effects
            '0x01 byte  = 0=failed spell, display resisted effect
            '1=successful spell
            '2=when starting a song
            '0x02 bytes = 0xFFBF//unused

            Dim s As New SpellEffectAnimation
            s.CasterID = pPacket.GetShort
            s.SpellCastID = pPacket.GetShort
            s.TargetID = pPacket.GetShort
            s.TimeToHit = pPacket.GetShort
            pPacket.Seek(1)
            s.SpellStatus = CType(pPacket.GetByte, eSpellStatus)

            _client.DoOnSpellEffectAnimation(s)
        End Sub

        Public Overrides Sub ParsePlayerCastSpell(ByVal pPacket As DAOCPacket)
            '0x02 bytes = object id of caster
            '0x02 bytes = spellid
            '0x02 bytes = time in tenths of a second spell will take to cast
            '0x02 bytes=not used

            Dim s As New SpellCast
            s.CasterID = pPacket.GetShort
            s.SpellCastID = pPacket.GetShort
            s.TimeToCast = pPacket.GetShort

            If s.CasterID = _client.Player.SpawnID Then
                _client.Player.CastTime = Now.AddMilliseconds(s.TimeToCast * 100) 'in tenths
            Else
                Dim pDAOCObject As DAOCObject = _client.DAOCObjects.FindBySpawnID(s.CasterID)

                If (pDAOCObject IsNot Nothing) AndAlso (TypeOf pDAOCObject Is DAOCMovingObject) Then
                    With DirectCast(pDAOCObject, DAOCMovingObject)
                        .CastTime = Now.AddMilliseconds(s.TimeToCast * 100) 'in tenths
                    End With
                End If
            End If

            _client.DoOnSpellCast(s)

        End Sub

        Public Overrides Sub ParseNewObject(ByVal pPacket As DAOCPacket, ByVal theClass As DAOCObjectClass)
            Dim tmpObject As DAOCObject
            Dim pOldObject As DAOCObject

            Select Case theClass
                Case DAOCObjectClass.ocObject
                    tmpObject = New DAOCObject
                    With tmpObject
                        'global
                        .SpawnID = pPacket.GetShort
                        .PlayerID = .SpawnID
                        pPacket.Seek(2) 'emblem
                        .HeadingValue = pPacket.GetShort
                        .Z = pPacket.GetShort
                        .X = pPacket.GetLong
                        .Y = pPacket.GetLong
                        pPacket.Seek(8) 'TODO more info in here
                        .Name = pPacket.GetPascalString
                    End With
                Case DAOCObjectClass.ocMob
                    tmpObject = New DAOCMob
                    With DirectCast(tmpObject, DAOCMob)
                        'global
                        .SpawnID = pPacket.GetShort
                        .PlayerID = .SpawnID
                        .Speed = pPacket.GetShort
                        .HeadingValue = pPacket.GetShort
                        .Z = pPacket.GetShort
                        .X = pPacket.GetLong
                        .Y = pPacket.GetLong
                        pPacket.Seek(5)
                        .Level = pPacket.GetByte
                        pPacket.Seek(6)
                        .Name = pPacket.GetPascalString
                        .TypeTag = pPacket.GetPascalString
                    End With
                Case DAOCObjectClass.ocPlayer
                    tmpObject = New DAOCPlayer
                    With DirectCast(tmpObject, DAOCPlayer)
                        'local
                        .PlayerID = pPacket.GetShort 'session id
                        .SpawnID = pPacket.GetShort 'spawnid
                        pPacket.Seek(2) 'model
                        .Z = pPacket.GetShort

                        Dim id As Integer = pPacket.GetShort 'zone ID

                        .X = _client.ZoneToGlobalX(pPacket.GetShort, id)
                        .Y = _client.ZoneToGlobalY(pPacket.GetShort, id)
                        .HeadingValue = pPacket.GetShort
                        pPacket.Seek(4)
                        .Level = pPacket.GetByte
                        pPacket.Seek(3)
                        .Flags = pPacket.GetByte
                        pPacket.Seek(1)
                        .Name = pPacket.GetPascalString
                        .Guild = pPacket.GetPascalString
                        .LastName = pPacket.GetPascalString
                    End With
                Case DAOCObjectClass.ocVehicle
                    tmpObject = New DAOCVehicle
                    With DirectCast(tmpObject, DAOCVehicle)
                        'global
                        .SpawnID = pPacket.GetShort
                        .PlayerID = .SpawnID
                        pPacket.Seek(2)
                        .HeadingValue = pPacket.GetShort
                        .Z = pPacket.GetShort
                        .X = pPacket.GetLong
                        .Y = pPacket.GetLong
                        pPacket.Seek(12) 'TODO more info in here
                        .Name = pPacket.GetPascalString
                    End With
                Case Else
                    tmpObject = Nothing
            End Select

            If (tmpObject IsNot Nothing) Then
                pOldObject = _client.DAOCObjects.FindBySpawnID(tmpObject.SpawnID)
                If (pOldObject IsNot Nothing) Then
                    _client.DoOnDeleteDAOCObject(pOldObject)
                End If
                _client.DAOCObjects.AddOrReplace(tmpObject)
                _client.DoOnNewDAOCObject(tmpObject)
                Debug.WriteLine("Adding new object " & tmpObject.SpawnID.ToString)
            End If

        End Sub

        Public Overrides Sub ParseMobUpdate(ByVal pPacket As DAOCPacket)
            Dim wID As Integer
            Dim pDAOCObject As DAOCObject
            Dim iIDOffset As Integer = 16
            Dim iZoneBase As Integer = 0
            Dim pZoneBase As DAOCZoneInfo
            Dim bAdded As Boolean = False

            pPacket.Seek(iIDOffset)
            wID = pPacket.GetShort
            pDAOCObject = _client.DAOCObjects.FindBySpawnID(wID)

            If pDAOCObject Is Nothing Then
                Debug.WriteLine("Did not find the ID " & wID.ToString)
                pDAOCObject = New DAOCUnknownMovingObject
                pDAOCObject.SpawnID = wID
                pDAOCObject.PlayerID = wID
                _client.DAOCObjects.AddOrReplace(pDAOCObject)
                bAdded = True
            End If

            'mobs not objects
            If (pDAOCObject IsNot Nothing) AndAlso (TypeOf pDAOCObject Is DAOCMovingObject) Then
                pPacket.Seek(-(iIDOffset + 2))
                With DirectCast(pDAOCObject, DAOCMovingObject)
                    .Speed = pPacket.GetShort
                    .HeadingValue = pPacket.GetShort
                    'all local to zone
                    Dim x As Integer = pPacket.GetShort
                    Dim DestX As Integer = pPacket.GetShort
                    Dim y As Integer = pPacket.GetShort
                    Dim DestY As Integer = pPacket.GetShort
                    .Z = pPacket.GetShort
                    .DestinationZ = pPacket.GetShort
                    pPacket.Seek(2)  ' ID again
                    .TargetID = pPacket.GetShort
                    .Health = pPacket.GetByte
                    Dim flags As Byte = pPacket.GetByte 'flags

                    pZoneBase = Nothing

                    ' adjust X, Y to global coords 
                    iZoneBase = pPacket.GetByte

                    If (flags And 4) = 4 Then
                        iZoneBase += 256
                    End If

                    .X = _client.ZoneToGlobalX(x, iZoneBase)
                    .Y = _client.ZoneToGlobalY(y, iZoneBase)

                    iZoneBase = 0

                    ' adjust DestX, DestY to global coords
                    iZoneBase = pPacket.GetByte

                    If Not iZoneBase = 255 Then
                        If (flags And 8) = 8 Then
                            iZoneBase += 256
                        End If

                        .DestinationX = _client.ZoneToGlobalX(DestX, iZoneBase)
                        .DestinationY = _client.ZoneToGlobalY(DestY, iZoneBase)
                    Else
                        .DestinationX = 0
                        .DestinationY = 0
                    End If

                End With

                If bAdded Then
                    _client.DoOnNewDAOCObject(pDAOCObject)
                Else
                    _client.DoOnDAOCObjectMoved(pDAOCObject)
                End If
            End If

        End Sub

        Public Overrides Sub ParseRemoveObject(ByVal pPacket As DAOCPacket)
            'remove static object or dead NPC's, ie loot
            Dim spawnID As Integer = pPacket.GetShort

            If _client.DAOCObjects.ObjectTable.ContainsKey(spawnID) Then
                _client.DoOnDeleteDAOCObject(_client.DAOCObjects.ObjectTable.Item(spawnID))
                _client.DAOCObjects.RemoveItem(spawnID)
            End If

        End Sub

        Public Overrides Sub ParsePlayerSpecsSpells(ByVal pPacket As DAOCPacket)
            Dim iCnt As Integer
            Dim iLevel As Integer
            Dim bPage As Byte
            Dim sName As String
            Dim pItem As DAOCNameValuePair
            Dim pType As Byte

            iCnt = pPacket.GetByte
            pType = pPacket.GetByte
            If Not (pType = &H2) Then
                Debug.WriteLine("Unexpected SpellsAbils: " & pPacket.ToString)
                Exit Sub
            End If
            pPacket.Seek(1)
            While (iCnt > 0) AndAlso (Not pPacket.EOF)
                iLevel = pPacket.GetByte
                bPage = pPacket.GetByte    ' Buffers
                pPacket.Seek(1)    'iLevelAttained = ppacket.GetAByte
                sName = pPacket.GetANullTermString(0)
                pItem = _client.Player.Spells.FindOrAdd(sName)
                If (pItem IsNot Nothing) Then
                    pItem.Value = iLevel
                End If
                iCnt -= 1
            End While
        End Sub

        Public Overrides Sub ParsePlayerSpecsSpellsAbils(ByVal pPacket As DAOCPacket)
            Dim iCnt As Integer
            Dim iLevel As Integer
            Dim bPage As Byte
            Dim sName As String
            Dim pItem As DAOCNameValuePair
            Dim pType As Byte

            iCnt = pPacket.GetByte
            pType = pPacket.GetByte
            If Not (pType = &H3) Then
                Debug.WriteLine("Unexpected SpellsAbils: " & pPacket.ToString)
                Exit Sub
            End If
            pPacket.Seek(1)
            While (iCnt > 0) AndAlso (Not pPacket.EOF)
                iLevel = pPacket.GetByte
                bPage = pPacket.GetByte
                pPacket.Seek(5)
                sName = pPacket.GetPascalString
                Select Case bPage
                    Case &H0
                        pItem = _client.Player.Specializations.FindOrAdd(sName)
                    Case &H1
                        pItem = _client.Player.Abilities.FindOrAdd(sName)
                    Case &H2
                        pItem = _client.Player.Styles.FindOrAdd(sName)
                    Case &H3
                        pItem = _client.Player.Spells.FindOrAdd(sName)
                    Case &H6
                        pItem = _client.Player.RealmAbilities.FindOrAdd(sName)
                    Case Else
                        Debug.WriteLine("Other Abilities: " & pPacket.ToString)
                        pItem = Nothing
                End Select
                If (pItem IsNot Nothing) Then
                    pItem.Value = iLevel
                End If
                iCnt -= 1
            End While
        End Sub

        Public Overrides Sub ParsePlayerStatsUpdate(ByVal pPacket As DAOCPacket)
            Select Case pPacket.GetByte
                Case &H1
                    ParsePlayerSpecsSpellsAbils(pPacket)
                Case &H2
                    ParsePlayerSpecsSpells(pPacket)
                Case &H3
                    ParsePlayerDetails(pPacket)
                Case &H5
                    'weapon stats
                Case &H6
                    ParseGroupMembersUpdate(pPacket)
                Case &H8
                    ParsePlayerSkills(pPacket)
                Case Else
                    pPacket.Seek(-1)
                    Debug.WriteLine("Other Abilities " & pPacket.GetByte)
                    Debug.WriteLine(pPacket.ToString)
            End Select
        End Sub

        Public Overrides Sub ParsePlayerDetails(ByVal pPacket As DAOCPacket)
            Dim SubType As Byte
            Dim iLevel As Integer
            Dim sName As String
            Dim pAcctChar As AccountCharInfo

            pPacket.Seek(1) ' count of items
            SubType = pPacket.GetByte
            If SubType = 0 Then
                pPacket.Seek(1)
                iLevel = pPacket.GetByte
                sName = pPacket.GetPascalString
                pAcctChar = _client.SetActiveCharacterByName(sName)
                pAcctChar.Level = iLevel
                _client.DoOnSetRegionID(pAcctChar.RegionID)

                _client.DoOnCharacterLogin()
            End If
        End Sub

        Public Overrides Sub ParseCharacterActivationRequest(ByVal pPacket As DAOCPacket)
            Dim CharName As String
            pPacket.Seek(5)
            CharName = pPacket.GetANullTermString(28)
            _client.SetActiveCharacterByName(CharName)
        End Sub

        Public Overrides Sub ParseCharacterLoginInit(ByVal pPacket As DAOCPacket)
            'global coordinates 

            _client.UseDynamicOffsets = False

            _client.Player.Clear()
            _client.LocalBufs.ClearTable()
            _client.Player.SpawnID = pPacket.GetShort
            _client.Player.Z = pPacket.GetShort
            _client.Player.X = pPacket.GetLong
            _client.Player.Y = pPacket.GetLong
            _client.Player.HeadingValue = pPacket.GetShort

            '0x01 byte  = Diving bits:
            '  0x80 = Diving enabled in current region
            '  0x01 = player starting under water
            '0x01 byte  = unknown
            pPacket.Seek(2)

            'these are only for dungeons
            _client.XOffset = pPacket.GetShort * 8192
            _client.YOffset = pPacket.GetShort * 8192

            If _client.XOffset > 0 OrElse _client.YOffset > 0 Then
                _client.UseDynamicOffsets = True
            End If

            _client.RegionID = pPacket.GetShort
            'last bytes are server name 
            _client.ClearDAOCObjectList()
            _client.CheckZoneChanged()
        End Sub

        Public Overrides Sub ParseAccountCharacters(ByVal pPacket As DAOCPacket)
            Dim sName As String
            Dim iCharNum As Integer
            Dim iRegion As Integer
            Dim iRealm As AccountCharInfo.DAOCRealm
            Dim iLevel As Integer
            Dim aAcctChar As AccountCharInfo

            _client.AccountCharacters.AccountName = pPacket.GetANullTermString(24)
            For iCharNum = 0 To 9
                sName = pPacket.GetANullTermString(24)
                pPacket.Seek(96)
                iLevel = pPacket.GetByte
                pPacket.Seek(1) 'Class id
                iRealm = CType(pPacket.GetByte, AccountCharInfo.DAOCRealm)
                pPacket.Seek(3)
                iRegion = pPacket.GetByte 'CORRECT
                pPacket.Seek(57)
                If (iRegion <> 0) AndAlso (sName <> String.Empty) Then
                    aAcctChar = _client.AccountCharacters.FindOrAddChar(sName)
                    aAcctChar.RegionID = iRegion
                    aAcctChar.Realm = iRealm
                    aAcctChar.Level = iLevel
                End If
            Next
        End Sub

        Public Overrides Sub ParseSetPlayerRegion(ByVal pPacket As DAOCPacket)
            _client.DoOnSetRegionID(pPacket.GetShort)
        End Sub

        Public Overrides Sub ParsePlayerUpdateHitPoints(ByVal pPacket As DAOCPacket)
            Dim attackerSpawnID As Integer = pPacket.GetShort
            Dim targetSpawnID As Integer = pPacket.GetShort
            pPacket.Seek(7)
            Dim hp As Byte = pPacket.GetByte
            Dim pDAOCObject As DAOCObject = _client.DAOCObjects.FindBySpawnID(attackerSpawnID)

            If (pDAOCObject IsNot Nothing) AndAlso (TypeOf pDAOCObject Is DAOCMovingObject) Then
                With DirectCast(pDAOCObject, DAOCMovingObject)
                    .TargetID = targetSpawnID
                End With
            End If

        End Sub

        Public Overrides Sub ParsePlayerPosUpdate(ByVal pPacket As DAOCPacket)
            'local to zone
            Dim wID As Integer
            Dim pDAOCObject As DAOCObject

            wID = pPacket.GetShort
            pDAOCObject = _client.DAOCObjects.FindByPlayerID(wID)

            If pDAOCObject Is Nothing Then
                Debug.WriteLine("Did not find " & wID.ToString & " from ParsePlayerPosUpdate")
                Exit Sub
            End If

            If Not TypeOf pDAOCObject Is DAOCPlayer Then
                pDAOCObject = _client.DAOCObjects.FindBySpawnID(wID)
                Exit Sub
            End If

            With DirectCast(pDAOCObject, DAOCPlayer)
                Dim UpdateFlags As Integer = pPacket.GetShort 'riding, dead, speed and sitting
                .UpdateFlags = UpdateFlags
                .Speed = UpdateFlags

                .Z = pPacket.GetShort
                Dim x As Integer = pPacket.GetShort
                Dim y As Integer = pPacket.GetShort

                Dim id As Integer = pPacket.GetShort 'zone ID

                .X = _client.ZoneToGlobalX(x, id)
                .Y = _client.ZoneToGlobalY(y, id)

                .HeadingValue = pPacket.GetShort
                pPacket.Seek(2)
                Dim stealthflag As Integer = pPacket.GetByte()
                Dim flag As Byte = pPacket.GetByte
                .Combat = CBool(flag >> 7) '1st bit
                .Health = flag And CByte(&H7F) 'low order 7 bits

                If (stealthflag And &H7F) >> 1 = 1 Then
                    '.Stealthed = True
                Else
                    '.Stealthed = False
                End If

                ' Attack Flag
                'Target visibility 
                _client.DoOnDAOCObjectMoved(pDAOCObject)
            End With

        End Sub

        Public Overrides Sub ParsePlayerHeadUpdate(ByVal pPacket As DAOCPacket)
            Dim wID As Integer
            Dim pDAOCObject As DAOCObject

            wID = pPacket.GetShort
            pDAOCObject = _client.DAOCObjects.FindByPlayerID(wID)

            If pDAOCObject Is Nothing OrElse (Not TypeOf pDAOCObject Is DAOCMovingObject) Then
                Exit Sub
            End If

            With CType(pDAOCObject, DAOCMovingObject)
                .HeadingValue = pPacket.GetShort
                pPacket.Seek(4) 'TODO
                Dim flag As Byte = pPacket.GetByte
                .Combat = CBool(flag >> 7) '1st bit
                .Health = flag And CByte(&H7F) 'low order 7 bits
                _client.DoOnDAOCObjectMoved(pDAOCObject)
            End With

        End Sub

        Public Overrides Sub ParseLocalHealthUpdate(ByVal pPacket As DAOCPacket)

            _client.Player.Health = pPacket.GetByte
            _client.Player.Mana = pPacket.GetByte
            pPacket.Seek(2) 'DOL says this is dead but it's not now
            If pPacket.GetByte = 0 Then
                _client.Player.IsSitting = False
            Else
                _client.Player.IsSitting = True
            End If

            _client.Player.Stamina = pPacket.GetByte
            _client.Player.Concentration = pPacket.GetByte
        End Sub

        Public Overrides Sub ParseAttackModeRequest(ByVal pPacket As DAOCPacket)
            SyncLock (_client.Player)
                _client.Player.Combat = CBool(pPacket.GetByte)
            End SyncLock
        End Sub

        Public Overrides Sub ParseSetGroundTarget(ByVal pPacket As DAOCPacket)
            'global
            _client.GroundTarget.X = _client.GlobalToZoneX(pPacket.GetLong)
            _client.GroundTarget.Y = _client.GlobalToZoneY(pPacket.GetLong)
            _client.GroundTarget.Z = pPacket.GetLong
            _client.DoOnSetGroundTarget()
        End Sub

        Public Overrides Sub ParseLocalHeadUpdateFromClient(ByVal pPacket As DAOCPacket)
            SyncLock (_client.Player)
                pPacket.Seek(2)
                _client.Player.HeadingValue = pPacket.GetShort
                _client.DoOnPlayerPosUpdate()
            End SyncLock
        End Sub

        Public Overrides Sub ParseSelectedIDUpdate(ByVal pPacket As DAOCPacket)
            _client.SelectedID = pPacket.GetShort
            _client.DoOnSelectedObjectChanged(_client.SelectedObject)
        End Sub

        Public Overrides Sub ParseLocalPosUpdateFromClient(ByVal pPacket As DAOCPacket)
            'local to zone
            SyncLock (_client.Player)
                pPacket.Seek(2)
                _client.Player.Speed = pPacket.GetShort

                'just to be on the safe side do it this way
                _client.Player.Z = pPacket.GetShort 'uses byte 0 then 1 instead of 1 then 0
                Dim x As Integer = pPacket.GetShort
                Dim y As Integer = pPacket.GetShort
                _client.ZoneID = pPacket.GetShort 'zone ID

                If _client.Zone Is Nothing Then
                    _client.Zone = _client.ZoneList.FindZone(_client.ZoneID)
                End If

                _client.Player.X = _client.ZoneToGlobalX(x, _client.ZoneID)
                _client.Player.Y = _client.ZoneToGlobalY(y, _client.ZoneID)

                _client.Player.HeadingValue = pPacket.GetShort '12bit
                'Dim flyingflag As Integer = pPacket.GetShort()
                'flag
                'TODO attack flag here

                _client.CheckZoneChanged()
                _client.DoOnPlayerPosUpdate()
            End SyncLock

        End Sub

        Public Overrides Sub ParsePlayerSkills(ByVal pPacket As DAOCPacket)
            Dim iCnt As Integer
            Dim iLevel As Integer
            Dim sName As String
            Dim pItem As DAOCNameValuePair

            Try
                iCnt = pPacket.GetByte
                If pPacket.GetByte <> &H3 Then
                    Debug.WriteLine("Unexpected PlayerSkills: " & pPacket.ToString)
                    Exit Sub
                End If
                pPacket.Seek(1)
                While (iCnt > 0) AndAlso (Not pPacket.EOF)
                    iLevel = pPacket.GetShort
                    pPacket.Seek(5)
                    sName = pPacket.GetPascalString
                    pItem = _client.Player.Skills.FindOrAdd(sName)
                    pItem.Value = iLevel

                    If pItem.Modified Then
                        _client.DoOnSkillLevelChanged(pItem)
                    End If
                    iCnt -= 1
                End While
            Catch ex As Exception
                Debug.WriteLine("ParsePlayerSkills " & ex.Message)
            End Try

        End Sub

        Public Overrides Sub ParseLocalBuff(ByVal pPacket As DAOCPacket)

            Try
                SyncLock (_client.LocalBufs)
                    Dim aCount As Byte = pPacket.GetByte
                    pPacket.Seek(3)
                    If aCount <> 0 Then
                        Dim i As Integer
                        For i = 1 To aCount
                            Dim NewIndex As Byte = pPacket.GetByte
                            pPacket.Seek(2)
                            Dim aID As Integer = pPacket.GetShort
                            Dim aTime As Integer = pPacket.GetShort
                            Dim aID2 As Integer = pPacket.GetShort
                            Dim aName As String = pPacket.GetPascalString
                            If aID = 0 Then
                                ' have to remove
                                _client.LocalBufs.RemoveItem(NewIndex)
                            Else
                                Dim aBuff As LocalBuff = New LocalBuff
                                aBuff.ID1 = aID
                                aBuff.ID2 = aID2
                                aBuff.Time = aTime
                                aBuff.Index = NewIndex
                                aBuff.Name = aName
                                _client.LocalBufs.AddOrReplace(aBuff)
                            End If
                        Next
                    End If
                End SyncLock
            Catch ex As Exception
                Debug.WriteLine(ex.Message)
            End Try

        End Sub

        Public Overrides Sub ParseConcentrationBuff(ByVal pPacket As DAOCPacket)
            Try
                SyncLock (_client.ConcentrationBufs)
                    _client.ConcentrationBufs.Clear()

                    Dim aCount As Byte = pPacket.GetByte
                    pPacket.Seek(3)
                    If aCount <> 0 Then
                        Dim i As Integer
                        For i = 1 To aCount
                            Dim aIndex As Integer = pPacket.GetShort
                            Dim aCon As Byte = pPacket.GetByte
                            Dim aID As Integer = pPacket.GetShort
                            Dim aName As String = pPacket.GetPascalString
                            Dim aBuff As ConcentrationBuff = _client.ConcentrationBufs.FindOrAddConcentrationBuff(aName)
                            aBuff.ID = aID
                            aBuff.Con = aCon
                            aBuff.Index = aIndex
                            aBuff.Target = pPacket.GetPascalString
                        Next
                    End If
                End SyncLock
            Catch ex As Exception
                Debug.WriteLine(ex.Message)
            End Try
        End Sub

    End Class

End Namespace
