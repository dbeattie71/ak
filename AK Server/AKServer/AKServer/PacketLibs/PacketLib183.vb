Namespace DAoCServer
    Public Class PacketLib183
        Inherits PacketLib168

        Private _client As DAOCMain

        Public Sub New(ByVal client As DAOCMain)
            MyBase.New(client)

            _client = client
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

                    If ppacket.Position = ppacket.Size Then

                        _client.Player.Inventory.TakeItem(pTmpItem)

                        Exit While

                    End If

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

    End Class

End Namespace
