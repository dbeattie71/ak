Imports System.IO
Namespace DAoCServer
    <Serializable()> _
    Public Class DAOCVendorItem
        Private mQuantity As Integer
        Private mPosition As Integer
        Private mCost As Integer
        Private mName As String
        Private mPage As Integer
        Public Sub Assign(ByVal AItem As DAOCVendorItem)
            mQuantity = AItem.Quantity
            mPosition = AItem.Position
            mCost = AItem.Cost
            mName = AItem.Name
            mPage = AItem.Page
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("{0},{1}) {2}x {3} ({4} c)", mPage, mPosition, mQuantity, mName, mCost)
        End Function

        Public Sub LoadFromReader(ByVal AReader As BinaryReader)
            mQuantity = AReader.ReadInt32
            mPosition = AReader.ReadInt32
            mCost = AReader.ReadInt32
            mName = AReader.ReadString
            mPage = AReader.ReadInt32
        End Sub

        Public Sub SaveToWriter(ByVal AWriter As BinaryWriter)
            AWriter.Write(mQuantity)
            AWriter.Write(mPosition)
            AWriter.Write(mCost)
            AWriter.Write(mName)
            AWriter.Write(mPage)
        End Sub

        Public Sub LoadFromStream(ByVal AStream As Stream) ' Stream
            Dim R As BinaryReader = New BinaryReader(AStream)
            Try
                LoadFromReader(R)
            Catch
            Finally
                R = Nothing
            End Try
        End Sub

        Public Sub SaveToStream(ByVal AStream As Stream) ' Stream
            Dim W As BinaryWriter = New BinaryWriter(AStream)
            Try
                SaveToWriter(W)
            Catch
            Finally
                W = Nothing
            End Try
        End Sub

        Public Property Page() As Integer
            Get
                Return mPage
            End Get
            Set(ByVal Value As Integer)
                mPage = Value
            End Set
        End Property

        Public Property Position() As Integer
            Get
                Return mPosition
            End Get
            Set(ByVal Value As Integer)
                mPosition = Value
            End Set
        End Property

        Public Property Quantity() As Integer
            Get
                Return mQuantity
            End Get
            Set(ByVal Value As Integer)
                mQuantity = Value
            End Set
        End Property

        Public Property Cost() As Integer
            Get
                Return mCost
            End Get
            Set(ByVal Value As Integer)
                mCost = Value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return mName
            End Get
            Set(ByVal Value As String)
                mName = Value
            End Set
        End Property
    End Class

    <Serializable()> _
    Public Class DAOCVendorItemList
        Inherits List(Of DAOCVendorItem)

        Private mVendor As DAOCMob
        Public Sub New()
            MyBase.New()
            mVendor = New DAOCMob
        End Sub

        Protected Overrides Sub Finalize()
            mVendor = Nothing
            MyBase.Finalize()
        End Sub

        Public Overloads Function Find(ByVal AName As String) As DAOCVendorItem
            Dim i As Integer
            For i = 0 To Count - 1
                If AName = Item(i).Name Then
                    Return Item(i)
                End If
            Next
            Return Nothing
        End Function

        Public Function FindOrAdd(ByVal AName As String) As DAOCVendorItem
            Dim Result As DAOCVendorItem
            Result = Find(AName)
            If Result Is Nothing Then
                Result = New DAOCVendorItem
                Result.Name = AName
                Add(Result)
            End If
            Return Result
        End Function

        Public Function PageOfItem(ByVal AName As String) As Integer
            Dim pItem As DAOCVendorItem

            pItem = Find(AName)
            If (pItem IsNot Nothing) Then
                Return pItem.Page
            End If
            Return -1

        End Function

        Public Function PositionOfItem(ByVal AName As String) As Integer
            Dim pItem As DAOCVendorItem

            pItem = Find(AName)
            If (pItem IsNot Nothing) Then
                Return pItem.Position
            End If
            Return -1

        End Function

        Public Function CostOfItem(ByVal AName As String) As Integer
            Dim pItem As DAOCVendorItem

            pItem = Find(AName)
            If (pItem IsNot Nothing) Then
                Return pItem.Cost
            End If
            Return 0
        End Function

        Public Function CostOfItemQ1(ByVal AName As String) As Integer
            Dim pItem As DAOCVendorItem

            pItem = Find(AName)
            If (pItem IsNot Nothing) Then
                Return pItem.Cost \ pItem.Quantity
            End If
            Return 0
        End Function

        Public Sub LoadFromReader(ByVal AReader As BinaryReader)
            Dim pItem As DAOCVendorItem

            Clear()
            Vendor.LoadFromReader(AReader)

            Try
                Do While AReader.PeekChar() > -1
                    pItem = New DAOCVendorItem
                    pItem.LoadFromReader(AReader)
                    Add(pItem)
                Loop
            Catch
            End Try
        End Sub

        Public Sub SaveToWriter(ByVal AWriter As BinaryWriter)
            Dim i As Integer

            mVendor.SaveToWriter(AWriter)
            For i = 0 To Count - 1
                Item(i).SaveToWriter(AWriter)
            Next
        End Sub

        Public Sub LoadFromStream(ByVal AStream As Stream)
            Dim R As BinaryReader = New BinaryReader(AStream)
            Try
                LoadFromReader(R)
            Catch
            Finally
                R = Nothing
            End Try
        End Sub

        Public Sub SaveToStream(ByVal AStream As Stream)
            Dim W As BinaryWriter = New BinaryWriter(AStream)
            Try
                SaveToWriter(W)
            Catch
            Finally
                W = Nothing
            End Try
        End Sub

        Public Overrides Function ToString() As String
            Dim Result As String = ""
            Dim i As Integer

            For i = 0 To Count - 1
                Result = Result & Item(i).ToString & vbCrLf
            Next
            Return Result
        End Function

        Public Overloads Sub Clear()
            MyBase.Clear()
            If mVendor IsNot Nothing Then
                mVendor.Clear()
            End If
        End Sub

        Default Public Shadows Property Item(ByVal Index As Integer) As DAOCVendorItem
            Get
                Return CType(MyBase.Item(Index), DAOCVendorItem)
            End Get
            Set(ByVal Value As DAOCVendorItem)
                MyBase.Item(Index) = Value
            End Set
        End Property

        Public ReadOnly Property Vendor() As DAOCMob
            Get
                Return mVendor
            End Get
        End Property

        Public Function MerchantItem(ByVal Page As Integer, ByVal Position As Integer) As DAOCVendorItem
            For Each vitem As DAOCVendorItem In Me
                If vitem.Page = Page AndAlso vitem.Position = Position Then
                    Return vitem
                End If
            Next
            Return Nothing
        End Function
    End Class

    <Serializable()> _
    Public Class DAOCMasterVendorList
        Inherits List(Of DAOCVendorItemList)
        Public Sub LoadFromStream(ByVal AStream As Stream)
            Dim Reader As BinaryReader
            Dim pVendorList As DAOCVendorItemList

            Reader = New BinaryReader(AStream)
            Try
                Do While Reader.PeekChar() > -1
                    pVendorList = New DAOCVendorItemList
                    pVendorList.LoadFromReader(Reader)
                    Add(pVendorList)
                Loop
            Catch
            Finally
                Reader = Nothing
            End Try
        End Sub

        Public Sub SaveToStream(ByVal AStream As Stream)
            Dim Writer As BinaryWriter
            Dim i As Integer

            Writer = New BinaryWriter(AStream)
            Try
                For i = 0 To Count - 1
                    Item(i).SaveToWriter(Writer)
                Next
            Catch
            Finally
                Writer = Nothing
            End Try
        End Sub

        Public Sub LoadFromFile(ByVal AFName As String)
            Dim FS As FileStream = New FileStream(AFName, FileMode.Open)

            Clear()
            Try
                LoadFromStream(FS)
            Catch
            Finally
                FS.Close()
                FS = Nothing
            End Try

        End Sub

        Public Sub SaveToFile(ByVal AFName As String)
            Dim FS As FileStream = New FileStream(AFName, FileMode.Create)
            Try
                SaveToStream(FS)
            Catch
            Finally
                FS.Close()
                FS = Nothing
            End Try
        End Sub

        Public Function FindVendor(ByVal AVendorName As String) As DAOCVendorItemList
            Dim i As Integer

            If AVendorName <> String.Empty Then
                For i = 0 To Count - 1
                    If Item(i).Vendor.Name = AVendorName Then
                        Return Item(i)
                    End If
                Next
            End If
            Return Nothing
        End Function

        Public Sub AddOrUpdate(ByVal AVendorItemList As DAOCVendorItemList)
            Dim pVendorInList As DAOCVendorItemList
            Dim pItem As DAOCVendorItem
            Dim i As Integer

            If AVendorItemList.Vendor.Name = "" Then
                Exit Sub
            End If

            pVendorInList = FindVendor(AVendorItemList.Vendor.Name)
            If pVendorInList Is Nothing Then
                pVendorInList = New DAOCVendorItemList
                pVendorInList.Vendor.Assign(AVendorItemList.Vendor)
                Add(pVendorInList)
            End If

            For i = 0 To AVendorItemList.Count - 1
                pItem = pVendorInList.FindOrAdd(AVendorItemList(i).Name)
                pItem.Assign(AVendorItemList(i))
            Next
        End Sub

        Public Function CostOfItem(ByVal AName As String) As Integer
            Dim Result As Integer
            Dim i As Integer
            For i = 0 To Count - 1
                Result = Item(i).CostOfItem(AName)
                If Result <> 0 Then
                    Return Result
                End If
            Next
            Return 0
        End Function

        Public Function CostOFItemQ1(ByVal AName As String) As Integer
            Dim Result As Integer
            Dim i As Integer
            For i = 0 To Count - 1
                Result = Item(i).CostOfItemQ1(AName)
                If Result <> 0 Then
                    Return Result
                End If
            Next
            Return 0
        End Function

        Public Function FindItemVendor(ByVal AItemName As String) As DAOCVendorItemList
            Dim i As Integer
            For i = 0 To Count - 1
                If (Item(i).Find(AItemName) IsNot Nothing) Then
                    Return Item(i)
                End If
            Next
            Return Nothing
        End Function

        Default Public Shadows Property Item(ByVal Index As Integer) As DAOCVendorItemList
            Get
                Return CType(MyBase.Item(Index), DAOCVendorItemList)
            End Get
            Set(ByVal Value As DAOCVendorItemList)
                MyBase.Item(Index) = Value
            End Set
        End Property

    End Class
End Namespace
