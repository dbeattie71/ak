Imports System.ComponentModel

Namespace DAoCServer
    <Serializable()> _
    Public Class LocalBuff
        Private mName As String
        Private mIndex As Integer
        Private mID1 As Integer
        Private mID2 As Integer
        Private mTime As Integer

        Public Property Index() As Integer
            Get
                Return mIndex
            End Get
            Set(ByVal Value As Integer)
                mIndex = Value
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

        Public Property ID1() As Integer
            Get
                Return mID1
            End Get
            Set(ByVal Value As Integer)
                mID1 = Value
            End Set
        End Property

        Public Property ID2() As Integer
            Get
                Return mID2
            End Get
            Set(ByVal Value As Integer)
                mID2 = Value
            End Set
        End Property

        Public Property Time() As Integer
            Get
                Return mTime
            End Get
            Set(ByVal Value As Integer)
                mTime = Value
            End Set
        End Property

    End Class

    <Serializable()> _
        Public Class LocalBuffList
        Private hash As Generic.Dictionary(Of Integer, LocalBuff)
        Public Sub New()
            hash = New Generic.Dictionary(Of Integer, LocalBuff)
        End Sub

        Public Sub RemoveItem(ByVal Key As Integer)
            SyncLock hash
                hash.Remove(Key)
            End SyncLock
        End Sub

        Public Sub AddOrReplace(ByVal aObject As LocalBuff)
            SyncLock hash
                hash.Remove(aObject.Index)
                hash.Add(aObject.Index, aObject)
            End SyncLock
        End Sub

        Public ReadOnly Property BuffTable() As Generic.Dictionary(Of Integer, LocalBuff)
            Get
                Return hash
            End Get
        End Property

        Public Sub ClearTable()
            hash.Clear()
        End Sub

    End Class

End Namespace
