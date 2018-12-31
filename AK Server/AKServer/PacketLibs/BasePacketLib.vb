Namespace DAoCServer
    Public Enum eClientVersion
        VersionNotChecked = -1
        VersionUnknown = 0
        Version168 = 168
        Version169 = 169
        Version170 = 170
        Version171 = 171
        Version172 = 172
        Version173 = 173
        Version174 = 174
        Version175 = 175
        Version176 = 176
        Version177 = 177
        Version178 = 178
        Version179 = 179
        Version180 = 180
        Version181 = 181
        Version182 = 182
        Version183 = 183
        Version184 = 184
        Version185 = 185
        Version186 = 186
        Version187 = 187
        Version188 = 188
        Version189 = 189
        Version190 = 190
    End Enum

    Public MustInherit Class BasePacketLib
        Implements IPacketLib

#Region "Override Methods"
        Public MustOverride Sub ParseAccountCharacters(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseAccountCharacters

        Public MustOverride Sub ParseAttackModeRequest(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseAttackModeRequest

        Public MustOverride Sub ParseCharacterActivationRequest(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseCharacterActivationRequest

        Public MustOverride Sub ParseCharacterLoginInit(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseCharacterLoginInit

        Public MustOverride Sub ParseConcentrationBuff(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseConcentrationBuff

        Public MustOverride Sub ParseDead(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseDead

        Public MustOverride Sub ParseDialogueUpdate(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseDialogueUpdate

        Public MustOverride Sub ParseGroupMembersUpdate(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseGroupMembersUpdate

        Public MustOverride Sub ParseGroupWindowUpdate(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseGroupWindowUpdate

        Public MustOverride Sub ParseInterruptSpell(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseInterruptSpell

        Public MustOverride Sub ParseInventoryList(ByVal ppacket As DAOCPacket) Implements IPacketLib.ParseInventoryList

        Public MustOverride Sub ParseLocalBuff(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseLocalBuff

        Public MustOverride Sub ParseLocalHeadUpdateFromClient(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseLocalHeadUpdateFromClient

        Public MustOverride Sub ParseLocalHealthUpdate(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseLocalHealthUpdate

        Public MustOverride Sub ParseLocalPlayerSpeedUpdate(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseLocalPlayerSpeedUpdate

        Public MustOverride Sub ParseLocalPosUpdateFromClient(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseLocalPosUpdateFromClient

        Public MustOverride Sub ParseLogUpdate(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseLogUpdate

        Public MustOverride Sub ParseMobUpdate(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseMobUpdate

        Public MustOverride Sub ParseNewObject(ByVal pPacket As DAOCPacket, ByVal theClass As DAOCObjectClass) Implements IPacketLib.ParseNewObject

        Public MustOverride Sub ParseObjectEquipment(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseObjectEquipment

        Public MustOverride Sub ParsePetUpdate(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParsePetUpdate

        Public MustOverride Sub ParsePetWindowUpdate(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParsePetWindowUpdate

        Public MustOverride Sub ParsePlayerCastSpell(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParsePlayerCastSpell

        Public MustOverride Sub ParsePlayerDetails(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParsePlayerDetails

        Public MustOverride Sub ParsePlayerHeadUpdate(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParsePlayerHeadUpdate

        Public MustOverride Sub ParsePlayerModelChange(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParsePlayerModelChange

        Public MustOverride Sub ParsePlayerPosUpdate(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParsePlayerPosUpdate

        Public MustOverride Sub ParsePlayerQuit(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParsePlayerQuit

        Public MustOverride Sub ParsePlayerSkills(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParsePlayerSkills

        Public MustOverride Sub ParsePlayerSpecsSpells(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParsePlayerSpecsSpells

        Public MustOverride Sub ParsePlayerSpecsSpellsAbils(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParsePlayerSpecsSpellsAbils

        Public MustOverride Sub ParsePlayerStatsUpdate(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParsePlayerStatsUpdate

        Public MustOverride Sub ParsePlayerUpdateHitPoints(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParsePlayerUpdateHitPoints

        Public MustOverride Sub ParseProgressMeter(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseProgressMeter

        Public MustOverride Sub ParseRemoveObject(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseRemoveObject

        Public MustOverride Sub ParseRevive(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseRevive

        Public MustOverride Sub ParseSelectedIDUpdate(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseSelectedIDUpdate

        Public MustOverride Sub ParseSetGroundTarget(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseSetGroundTarget

        Public MustOverride Sub ParseSetPlayerRegion(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseSetPlayerRegion

        Public MustOverride Sub ParseSpellEffectAnimation(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseSpellEffectAnimation

        Public MustOverride Sub ParseVendorWindow(ByVal pPacket As DAOCPacket) Implements IPacketLib.ParseVendorWindow

#End Region

        Public Sub New(ByVal client As DAOCMain)

        End Sub

        Public Sub New()
        End Sub

        Public Shared Function CreatePacketLibForVersion(ByVal Version As eClientVersion, ByVal client As DAOCMain) As IPacketLib

            Select Case Version

                Case eClientVersion.Version182, eClientVersion.Version183
                    Return New PacketLib183(client)
                Case Else
                    Return New PacketLib168(client)
            End Select
        End Function

    End Class

End Namespace
