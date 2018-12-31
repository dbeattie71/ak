Namespace DAoCServer
    <Serializable()> _
    Public Class DAOCNameValuePair

        Public Event NameValueModifiedNotify(ByVal AItem As DAOCNameValuePair)

        Private mName As String
        Private mValue As Integer
        Private mLastValue As Integer

        Public Property Name() As String
            Get
                Return mName
            End Get
            Set(ByVal Value As String)
                mName = Value
            End Set
        End Property

        ReadOnly Property Modified() As Boolean
            Get
                Return mLastValue <> mValue
            End Get
        End Property

        Property Value() As Integer
            Get
                Return mValue
            End Get
            Set(ByVal Value As Integer)
                mLastValue = mValue
                mValue = Value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("{0} [{1}]", Name, Value)
        End Function
    End Class
    <Serializable()> _
    Public Class DAOCNameValueList
        Inherits List(Of DAOCNameValuePair)

        Public Event OnNameValueModified(ByVal AItem As DAOCNameValuePair)

        Default Public Shadows Property Item(ByVal i As Integer) As DAOCNameValuePair
            Get
                Try
                    Return CType(MyBase.Item(i), DAOCNameValuePair)
                Catch ex As Exception
                    Return Nothing
                End Try
            End Get
            Set(ByVal Value As DAOCNameValuePair)
                MyBase.Item(i) = Value
            End Set
        End Property

        Public Overloads Function Find(ByVal AName As String) As DAOCNameValuePair
            Dim i As Integer
            Dim aItem As DAOCNameValuePair

            For i = 0 To Count - 1
                aItem = Item(i)
                If AName = aItem.Name Then
                    Return aItem
                End If
            Next
            Return Nothing

        End Function

        Public Function FindOrAdd(ByVal AName As String) As DAOCNameValuePair
            Dim aItem As DAOCNameValuePair

            aItem = Find(AName)

            If aItem IsNot Nothing Then
                aItem = New DAOCNameValuePair
                aItem.Name = AName
                Add(aItem)
            End If

            Return aItem

        End Function

    End Class
End Namespace

