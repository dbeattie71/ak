Namespace DAoCServer
#Region "AccountCharInfo"
    <Serializable()> _
       Public Class AccountCharInfo
        Public Enum DAOCRealm
            drFriend
            drAlbion
            drMidgard
            drHibernia
        End Enum
        Private mName As String
        Private mRegionID As Integer
        Private mLevel As Integer
        Private mRealm As DAOCRealm

        Public Function AsString() As String
            Return String.Format("{0} level {1} {2} region {3}", mName, mLevel, RealmToStr(mRealm), mRegionID)
        End Function

        Public Shared Function RealmToStr(ByVal aRealm As DAOCRealm) As String
            Select Case aRealm
                Case DAOCRealm.drFriend
                    Return "Friend"
                Case DAOCRealm.drAlbion
                    Return "Albion"
                Case DAOCRealm.drHibernia
                    Return "Hibernia"
                Case DAOCRealm.drMidgard
                    Return "Midgard"
            End Select

            Return String.Empty

        End Function

        Public Property Name() As String
            Get
                Return mName
            End Get
            Set(ByVal Value As String)
                mName = Value
            End Set
        End Property

        Public Property RegionID() As Integer
            Get
                Return mRegionID
            End Get
            Set(ByVal Value As Integer)
                mRegionID = Value
            End Set
        End Property

        Public Property Level() As Integer
            Get
                Return mLevel
            End Get
            Set(ByVal Value As Integer)
                mLevel = Value
            End Set
        End Property

        Public Property Realm() As DAOCRealm
            Get
                Return mRealm
            End Get
            Set(ByVal Value As DAOCRealm)
                mRealm = Value
            End Set
        End Property
    End Class
#End Region

#Region "AccountCharInfoList"
    <Serializable()> _
       Public Class AccountCharInfoList
        Inherits List(Of AccountCharInfo)
        Private mAccountName As String

        Default Public Shadows Property Item(ByVal Index As Integer) As AccountCharInfo
            Get
                Return CType(MyBase.Item(Index), AccountCharInfo)
            End Get
            Set(ByVal Value As AccountCharInfo)
                MyBase.Item(Index) = Value
            End Set
        End Property

        Public Function FindOrAddChar(ByVal AName As String) As AccountCharInfo
            Dim i As Integer
            Dim AChar As AccountCharInfo

            For i = 0 To Count - 1
                AChar = DirectCast(MyBase.Item(i), AccountCharInfo)
                If AChar.Name = AName Then
                    Return AChar
                End If
            Next

            AChar = New AccountCharInfo
            AChar.Name = AName
            Add(AChar)

            Return AChar
        End Function

        Public Shadows Sub Clear()
            MyBase.Clear()
        End Sub

        Public Property AccountName() As String
            Get
                Return mAccountName
            End Get
            Set(ByVal Value As String)
                mAccountName = Value
            End Set
        End Property
    End Class
#End Region

End Namespace

