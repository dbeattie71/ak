Namespace DAoCServer
    <Serializable()> _
    Public Class DAOCInventoryItem
        Public Const INV_BAGPOS_FIRST As Integer = &H28
        Public Const INV_BAGPOS_LAST As Integer = &H4F
        Public Const INV_VAULTPOS_FIRST As Integer = &H6E
        Public Const INV_VAULTPOS_LAST As Integer = &H95

        Private mSlot As Byte
        Private mCount As Integer
        Private mDurability As Integer
        Private mQuality As Integer
        Private mBonus As Integer
        Private mCondition As Integer
        Private mCountlessDescription As String
        Private mDescription As String
        Private mIcon As Byte
        Private mColor As Short
        Private mLevel As Byte

        Public Sub New()
            mCount = 1
        End Sub

        Public Property Description() As String
            Get
                Return mDescription
            End Get
            Set(ByVal Value As String)
                mDescription = Value
                mCountlessDescription = Value
                Try
                    While mCountlessDescription <> "" AndAlso (Char.IsDigit(mCountlessDescription.Chars(0)) Or mCountlessDescription.Chars(0) = " ")
                        mCountlessDescription = mCountlessDescription.Remove(0, 1)
                    End While
                Catch
                End Try
            End Set
        End Property

        Public ReadOnly Property CountlessDescription() As String
            Get
                Return mCountlessDescription
            End Get
        End Property

        Public Property Slot() As Byte
            Get
                Return mSlot
            End Get
            Set(ByVal Value As Byte)
                mSlot = Value
            End Set
        End Property

        Public ReadOnly Property SlotName() As String
            Get
                Return GetSlotName()
            End Get
        End Property

        Public Property Count() As Integer
            Get
                Return mCount
            End Get
            Set(ByVal Value As Integer)
                mCount = Value
            End Set
        End Property

        Public Property Condition() As Integer
            Get
                Return mCondition
            End Get
            Set(ByVal Value As Integer)
                mCondition = Value
            End Set
        End Property

        Public Property Durability() As Integer
            Get
                Return mDurability
            End Get
            Set(ByVal Value As Integer)
                mDurability = Value
            End Set
        End Property

        Public Property Quality() As Integer
            Get
                Return mQuality
            End Get
            Set(ByVal Value As Integer)
                mQuality = Value
            End Set
        End Property

        Public Property Bonus() As Integer
            Get
                Return mBonus
            End Get
            Set(ByVal Value As Integer)
                mBonus = Value
            End Set
        End Property

        Public Property Color() As Short
            Get
                Return mColor
            End Get
            Set(ByVal Value As Short)
                mColor = Value
            End Set
        End Property

        Public Property Icon() As Byte
            Get
                Return mIcon
            End Get
            Set(ByVal Value As Byte)
                mIcon = Value
            End Set
        End Property

        Public Property Level() As Byte
            Get
                Return mLevel
            End Get
            Set(ByVal Value As Byte)
                mLevel = Value
            End Set
        End Property

        Public ReadOnly Property isInBag() As Boolean
            Get
                Return (mSlot >= INV_BAGPOS_FIRST AndAlso mSlot <= INV_BAGPOS_LAST)
            End Get
        End Property

        Public ReadOnly Property BagPage() As Integer
            Get
                Return SlotAsBag()
            End Get
        End Property

        Public ReadOnly Property BagItemIndex() As Integer
            Get
                Return SlotAsBagPos()
            End Get
        End Property

        Public ReadOnly Property VaultPage() As Integer
            Get
                Return SlotAsVaultPage()
            End Get
        End Property

        Public ReadOnly Property VaultItemIndex() As Integer
            Get
                Return SlotAsVaultPos()
            End Get
        End Property

        Public Function SlotAsBag() As Integer
            If mSlot >= INV_BAGPOS_FIRST AndAlso mSlot <= INV_BAGPOS_LAST Then
                Return (mSlot - INV_BAGPOS_FIRST) \ 8
            Else
                Return -1
            End If
        End Function

        Public Function SlotAsBagPos() As Integer
            If mSlot >= INV_BAGPOS_FIRST AndAlso mSlot <= INV_BAGPOS_LAST Then
                Return (mSlot - INV_BAGPOS_FIRST) Mod 8
            Else
                Return -1
            End If
        End Function

        Public Function SlotAsVaultPage() As Integer
            If mSlot >= INV_VAULTPOS_FIRST AndAlso mSlot <= INV_VAULTPOS_LAST Then
                Return (mSlot - INV_VAULTPOS_FIRST) \ 20
            Else
                Return -1
            End If
        End Function

        Public Function SlotAsVaultPos() As Integer
            If mSlot >= INV_VAULTPOS_FIRST AndAlso mSlot <= INV_VAULTPOS_LAST Then
                Return (mSlot - INV_VAULTPOS_FIRST) Mod 20
            Else
                Return -1
            End If
        End Function

        Public Function GetSlotName() As String
            Select Case mSlot
                Case &HA
                    Return "Left Hand"
                Case &HB
                    Return "Right Hand"
                Case &HC
                    Return "2h"
                Case &HD
                    Return "Ranged"
                Case &HE
                    Return "Thrown"
                Case &H15
                    Return "Head"
                Case &H16
                    Return "Hands"
                Case &H17
                    Return "Feet"
                Case &H18
                    Return "Jewelry"
                Case &H19
                    Return "Chest"
                Case &H1A
                    Return "Cloak"
                Case &H1B
                    Return "Legs"
                Case &H1C
                    Return "Sleeves"
                Case &H1D
                    Return "Necklace"
                Case &H20
                    Return "Belt"
                Case &H21
                    Return "Left wrist"
                Case &H22
                    Return "Right wrist"
                Case &H23
                    Return "Left finger"
                Case &H24
                    Return "Right finger"
                Case INV_BAGPOS_FIRST To INV_BAGPOS_LAST
                    Return "Inventory bag " & CStr(SlotAsBag()) & " pos " & CStr(SlotAsBagPos())
                Case INV_VAULTPOS_FIRST To INV_VAULTPOS_LAST
                    Return "Vault Page " & CStr(SlotAsVaultPage()) & " pos " & CStr(SlotAsVaultPos())
                Case Else
                    Return "slot 0x" & Hex(mSlot)
            End Select
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("  {0} ({1}) quantity {2}" & vbCrLf & "    Con {3} Dur {4} Qual {5} Bon {6}" & vbCrLf & _
            "    Level {7} Color {8} Icon {9}", mDescription, GetSlotName, mCount, mCount, mCondition, mDurability, mQuality, mBonus, mLevel, mColor, mIcon)
        End Function
    End Class

    <Serializable()> _
    Public Class DAOCInventory
        Inherits List(Of DAOCInventoryItem)

        Public Overloads Function Find(ByVal AItem As String) As DAOCInventoryItem
            Dim i As Integer
            For i = 0 To Count - 1
                If AItem = Item(i).Description Then
                    Return Item(i)
                End If
            Next
            Return Nothing
        End Function

        Public Function FindCountless(ByVal AItem As String) As DAOCInventoryItem
            Dim i As Integer
            For i = 0 To Count - 1
                If AItem = Item(i).CountlessDescription Then
                    Return Item(i)
                End If
            Next
            Return Nothing
        End Function

        Public Function HasItem(ByVal AItem As String) As Boolean
            Return (Find(AItem) IsNot Nothing)
        End Function

        Public Function TotalCountOfItem(ByVal AItem As String, Optional ByVal AIncludeVault As Boolean = False) As Integer
            Dim i As Integer
            Dim Result As Integer = 0
            For i = 0 To Count - 1
                If AItem = Item(i).CountlessDescription AndAlso (AIncludeVault Or Item(i).isInBag) Then
                    Result = Result + Item(i).Count
                End If
            Next
            Return Result
        End Function

        Public Sub ClearSlot(ByVal ASlotID As Byte)
            Dim i As Integer
            i = IndexOfSlot(ASlotID)
            If i <> -1 Then
                RemoveAt(i)
            End If
        End Sub

        Public Function ItemInSlot(ByVal ASlotID As Byte) As DAOCInventoryItem
            Dim i As Integer
            i = IndexOfSlot(ASlotID)
            If i <> -1 Then
                Return Item(i)
            Else
                Return Nothing
            End If
        End Function

        Public Function IndexOfSlot(ByVal ASlotID As Integer) As Integer
            Dim Result As Integer
            For Result = 0 To Count - 1
                If ASlotID = Item(Result).Slot Then
                    Return Result
                End If
            Next
            Return -1
        End Function

        Public Overrides Function ToString() As String
            Dim i As Integer
            Dim Result As String = String.Empty
            For i = 0 To Count - 1
                Result = Result & Item(i).ToString
                If i < Count - 1 Then
                    Result = Result & vbCrLf
                End If
            Next
            Return Result
        End Function

        Public Sub TakeItem(ByVal AItem As DAOCInventoryItem)
            ClearSlot(AItem.Slot)
            If AItem.Description = String.Empty Then
                AItem = Nothing
            Else
                Add(AItem)
            End If
        End Sub

        Public Function isFull() As Boolean
            Dim i As Integer
            For i = DAOCInventoryItem.INV_BAGPOS_FIRST To DAOCInventoryItem.INV_BAGPOS_LAST
                If IndexOfSlot(i) = -1 Then
                    Return False
                End If
            Next
            Return True
        End Function

        Default Public Shadows Property Item(ByVal Index As Integer) As DAOCInventoryItem
            Get
                Return DirectCast(MyBase.Item(Index), DAOCInventoryItem)
            End Get
            Set(ByVal Value As DAOCInventoryItem)
                MyBase.Item(Index) = Value
            End Set
        End Property
    End Class
End Namespace
