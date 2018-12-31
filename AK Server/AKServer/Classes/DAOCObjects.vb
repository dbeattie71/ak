Imports System.Math
Imports System.IO
Imports System.Collections
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization

Namespace DAoCServer
    <Serializable()> _
    Public Enum WeaponType
        RightHand = 0
        LeftHand = 1
        TwoHand = 2
        Range = 3
        None = &HF
    End Enum

    <Serializable()> _
    Public Enum DAOCObjectClass
        ocUnknown
        ocObject
        ocMob
        ocPlayer
        ocLocalPlayer
        ocVehicle
    End Enum

    <Serializable()> _
    Public Enum DAOCRealmRank
        rrUnknown
        rr1
        rr2
        rr3
        rr4
        rr5
        rr6
        rr7
        rr8
        rr9
        rr10
    End Enum

    <Serializable()> _
    Public Class DAOCObjects
        Public Shared Function DAOCObjectClassToStr(ByVal AClass As DAOCObjectClass) As String
            Select Case AClass
                Case DAOCObjectClass.ocLocalPlayer
                    Return "Local Player"
                Case DAOCObjectClass.ocMob
                    Return "MOB"
                Case DAOCObjectClass.ocObject
                    Return "Object"
                Case DAOCObjectClass.ocPlayer
                    Return "Player"
                Case DAOCObjectClass.ocUnknown
                    Return "Unknown"
                Case Else
                    Return "Unknown " & CStr(AClass)
            End Select
        End Function

        Private Shared Function PrependStr(ByRef Result As String, ByVal s As String) As String
            If Result <> "" Then
                Return s & " " & Result
            Else
                Return s
            End If
        End Function
    End Class

    Public Interface DAOCObjectInterface
    End Interface

    <Serializable()> _
    Public Class DAOCObject
        Implements DAOCObjectInterface
#Region "Variables"
        Private Const StealthFlag As Integer = &H10
        Private Const UnderWaterFlag As Integer = &H2
        Private Const DeadFlag As Integer = &H1

        Private mSpawnID As Integer 'the ID of a PC or NPC has in a region 
        Private mPlayerID As Integer 'session ID, global to the player across the regions
        Private mX As Integer   'dword
        Private mY As Integer   'dword
        Private mZ As Integer   'dword
        Private mDestinationX As Integer    'dword
        Private mDestinationY As Integer    'dword
        Private mDestinationZ As Integer
        Private mRealm As AccountCharInfo.DAOCRealm
        Private mName As String
        Protected mLastUpdate As Integer  'dword
        Private mLevel As Integer
        Private mHeadingValue As Integer
        Private mDead As Boolean
        Private mSwimming As Boolean
        Private mStealthed As Boolean
        Private mFlags As Integer 'these flags are for when the object is created
        Private mHealth As Byte
        Private mTargetID As Integer
        Private mIsStale As Boolean = False
        Private mCalculatedText As String
        Private mCalculatedColor As Color
        Private mReadyToDraw As Boolean = False
#End Region

#Region "Properties"
        Public Property CalculatedText() As String
            Get
                Return mCalculatedText
            End Get
            Set(ByVal value As String)
                mCalculatedText = value
            End Set
        End Property

        Public Property CalculatedColor() As Color
            Get
                Return mCalculatedColor
            End Get
            Set(ByVal value As Color)
                mCalculatedColor = value
            End Set
        End Property

        Public Property ReadyToDraw() As Boolean
            Get
                Return mReadyToDraw
            End Get
            Set(ByVal value As Boolean)
                mReadyToDraw = value
            End Set
        End Property


        Public Property IsStale() As Boolean
            Get
                Return mIsStale
            End Get
            Set(ByVal Value As Boolean)
                mIsStale = Value
            End Set
        End Property

        Public WriteOnly Property Flags() As Integer
            Set(ByVal Value As Integer)
                mFlags = Value
            End Set
        End Property

        Public ReadOnly Property Dead() As Boolean
            Get
                If (mFlags And DeadFlag) = DeadFlag Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property Swimming() As Boolean
            Get
                If (mFlags And UnderWaterFlag) = UnderWaterFlag Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public Property SpawnID() As Integer
            Get
                Return mSpawnID
            End Get
            Set(ByVal Value As Integer)
                mSpawnID = Value
            End Set
        End Property

        Public Property PlayerID() As Integer
            Get
                Return mPlayerID
            End Get
            Set(ByVal Value As Integer)
                mPlayerID = Value
            End Set
        End Property

        Public Property X() As Integer
            Get
                Return mX
            End Get
            Set(ByVal Value As Integer)
                mX = Value
                Touch()
            End Set
        End Property

        Public Property Y() As Integer
            Get
                Return mY
            End Get
            Set(ByVal Value As Integer)
                mY = Value
                Touch()
            End Set
        End Property

        Public Property Z() As Integer
            Get
                Return mZ
            End Get
            Set(ByVal Value As Integer)
                mZ = Value
                Touch()
            End Set
        End Property

        Public Property DestinationX() As Integer
            Get
                Return mDestinationX
            End Get
            Set(ByVal Value As Integer)
                mDestinationX = Value
                Touch()
            End Set
        End Property

        Public Property DestinationY() As Integer
            Get
                Return mDestinationY
            End Get
            Set(ByVal Value As Integer)
                mDestinationY = Value
                Touch()
            End Set
        End Property

        Public Property DestinationZ() As Integer
            Get
                Return mDestinationZ
            End Get
            Set(ByVal Value As Integer)
                mDestinationZ = Value
                Touch()
            End Set
        End Property

        Public Property Name() As String
            Get
                Return mName
            End Get
            Set(ByVal Value As String)
                mName = Value
                Touch()
            End Set
        End Property

        Public ReadOnly Property Realm() As AccountCharInfo.DAOCRealm
            Get
                Select Case (mFlags Or &H3) >> 2
                    Case &H0
                        Return AccountCharInfo.DAOCRealm.drFriend
                    Case &H1
                        Return AccountCharInfo.DAOCRealm.drAlbion
                    Case &H2
                        Return AccountCharInfo.DAOCRealm.drMidgard
                    Case &H3
                        Return AccountCharInfo.DAOCRealm.drHibernia
                End Select
            End Get
        End Property

        Public Property Level() As Integer
            Get
                Return mLevel
            End Get
            Set(ByVal Value As Integer)
                mLevel = Value
                Touch()
            End Set
        End Property

        Public ReadOnly Property Direction() As Integer
            Get
                'daoc coordinate system is top->bottom so we need to adjust 180 degrees
                'Return CInt(((mHeadingValue / 4096 * 360) + 180) Mod 360)
                Return CInt(HeadRad + 180) Mod 360
            End Get
        End Property

        Public WriteOnly Property HeadingValue() As Integer
            Set(ByVal Value As Integer)
                mHeadingValue = Value
            End Set
        End Property

        Public ReadOnly Property HeadRad() As Integer
            Get
                Return CInt((mHeadingValue And &HFFF) * (360 / 4096))
            End Get
        End Property

        Public ReadOnly Property Stealthed() As Boolean
            Get
                If (mFlags And StealthFlag) = StealthFlag Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public Sub Assign(ByVal aSrc As DAOCObject)
            SpawnID = aSrc.SpawnID
            mPlayerID = aSrc.PlayerID
            mX = aSrc.X
            mY = aSrc.Y
            mZ = aSrc.Z
            mDestinationX = aSrc.DestinationX
            mDestinationY = aSrc.DestinationY
            mDestinationZ = aSrc.DestinationZ
            mLastUpdate = aSrc.mLastUpdate
            mName = aSrc.Name
            mRealm = aSrc.Realm
            mLevel = aSrc.Level
            mHeadingValue = aSrc.Direction
        End Sub

        Public Sub Clear()
            mPlayerID = 0
            mX = 0
            mY = 0
            mZ = 0
            mDestinationX = 0
            mDestinationY = 0
            mDestinationZ = 0
            mLastUpdate = 0
            mLevel = 0
            mRealm = AccountCharInfo.DAOCRealm.drFriend
            mName = String.Empty
            mHeadingValue = 0
        End Sub

        Public Sub New()
            MyBase.New()
            Clear()
        End Sub

        Public Function GetConColor(ByVal PlayerLevel As Integer, ByVal Level As Integer) As Color

            Select Case (ConColors.GetConColor(PlayerLevel, Level)) 'player level,mob
                Case ConColors.DAOCConColor.Gray      'grey
                    Return System.Drawing.Color.Gray
                Case ConColors.DAOCConColor.Green       'green
                    Return System.Drawing.Color.Green
                Case ConColors.DAOCConColor.Blue      'blue
                    Return System.Drawing.Color.Blue
                Case ConColors.DAOCConColor.Yellow       'yellow
                    Return System.Drawing.Color.Yellow
                Case ConColors.DAOCConColor.Orange       'orange
                    Return System.Drawing.Color.Orange
                Case ConColors.DAOCConColor.Red       'red
                    Return System.Drawing.Color.Red
                Case ConColors.DAOCConColor.Purple       'purple
                    Return System.Drawing.Color.Purple
            End Select

        End Function

        Public Overridable Function GetObjectClass() As DAOCObjectClass
            Return DAOCObjectClass.ocObject
        End Function

        Public Sub SaveToWriter(ByVal aWriter As System.IO.BinaryWriter)
            aWriter.Write(mX)
            aWriter.Write(mY)
            aWriter.Write(mZ)
            aWriter.Write(mName)
            aWriter.Write(CInt(mRealm))
            aWriter.Write(mLevel)
            aWriter.Write(mHeadingValue)
        End Sub

        Public Sub LoadFromReader(ByVal aReader As System.IO.BinaryReader)
            Clear()
            mX = aReader.ReadInt32
            mY = aReader.ReadInt32
            mZ = aReader.ReadInt32
            mName = aReader.ReadString
            mRealm = CType(aReader.ReadInt32, AccountCharInfo.DAOCRealm)
            mLevel = aReader.ReadInt32
            mHeadingValue = aReader.ReadInt32
        End Sub

        Public Sub Touch()
            mLastUpdate = System.Environment.TickCount
            mIsStale = False
        End Sub

        Public Function LastTouched() As Integer
            Return System.Environment.TickCount - mLastUpdate
        End Function

        Public Property Health() As Byte
            Get
                Return mHealth
            End Get
            Set(ByVal Value As Byte)
                mHealth = Value
            End Set
        End Property

        Public Property TargetID() As Integer
            Get
                Return mTargetID
            End Get
            Set(ByVal Value As Integer)
                mTargetID = Value
            End Set
        End Property

        Public Overridable Property Combat() As Boolean
            Get
                Return Not mTargetID = 0
            End Get
            Set(ByVal Value As Boolean)

            End Set
        End Property
#End Region
    End Class

    <Serializable()> _
    Public NotInheritable Class DAOCObjectlist
        Private hash As Generic.Dictionary(Of Integer, DAOCObject)
        Public Sub New()
            hash = New Generic.Dictionary(Of Integer, DAOCObject)
        End Sub

        Public ReadOnly Property ObjectTable() As Generic.Dictionary(Of Integer, DAOCObject)
            Get
                Return hash
            End Get
        End Property

        Public Sub CheckStale()
            Dim removelist As New List(Of Integer)

            'an object goes stale if it has not received an update within the alotted time
            'if it has not had an update within the alotted time after it's stale then delete it

            SyncLock hash
                For Each obj As DAOCObject In hash.Values

                    'only moving objects receive updates
                    If TypeOf obj Is DAOCMovingObject Then
                        If obj.GetObjectClass = DAOCObjectClass.ocPlayer Then
                            If Not obj.IsStale AndAlso obj.LastTouched > 5000 Then 'players get updates every 2-3 seconds 
                                obj.IsStale = True 'don't draw or use
                            ElseIf obj.IsStale AndAlso obj.LastTouched > 60000 Then
                                removelist.Add(obj.SpawnID)
                                Debug.WriteLine("removed " & obj.Name)
                            End If
                        Else
                            If Not obj.IsStale AndAlso obj.LastTouched > 20000 Then 'every 10-12 seconds
                                obj.IsStale = True 'don't draw or use
                            ElseIf obj.IsStale AndAlso obj.LastTouched > 60000 Then
                                removelist.Add(obj.SpawnID)
                            End If
                        End If
                    End If

                Next

                For Each x As Integer In removelist
                    hash.Remove(x)
                Next

            End SyncLock

            removelist = Nothing

        End Sub

        Public Sub ClearTable()
            SyncLock hash
                hash.Clear()
            End SyncLock
        End Sub

        Public Sub AddOrReplace(ByVal aObject As DAOCObject)
            SyncLock hash
                hash.Remove(aObject.SpawnID)
                hash.Add(aObject.SpawnID, aObject)
            End SyncLock
        End Sub

        Public Sub RemoveItem(ByVal SpawnID As Integer)
            SyncLock hash
                hash.Remove(SpawnID)
            End SyncLock
        End Sub

        Public Function FindBySpawnID(ByVal aInfoID As Integer) As DAOCObject

            If hash.ContainsKey(aInfoID) Then
                Return DirectCast(hash.Item(aInfoID), DAOCObject)
            Else
                Return Nothing
            End If

        End Function

        Public Function FindByPlayerID(ByVal aPlayerID As Integer) As DAOCObject

            SyncLock hash
                For Each obj As DAOCObject In hash.Values
                    If obj.PlayerID = aPlayerID Then
                        Return obj
                    End If
                Next
            End SyncLock

            Return Nothing

        End Function
    End Class

    <Serializable()> _
    Public Class DAOCMovingObject
        Inherits DAOCObject
        Private mSpeedWord As Integer
        Private mProjectedX As Integer
        Private mProjectedY As Integer
        Private mProjectedLastUpdate As Integer
        Private mCastTime As DateTime 'in tenths
        Public Overloads Sub Assign(ByVal aSrc As DAOCMovingObject)
            MyBase.Assign(aSrc)
            mSpeedWord = aSrc.Speed
        End Sub

        Public Overloads Sub Clear()
            MyBase.Clear()
            mSpeedWord = 0
        End Sub

        Public Property Speed() As Integer
            Get
                Dim Result As Integer = mSpeedWord And &H1FF
                If (Result And &H200) <> 0 Then
                    Result = -Result
                End If
                Return Result
            End Get
            Set(ByVal Value As Integer)
                mSpeedWord = Value
                Touch()
            End Set
        End Property

        Public Function GetSpeedString() As String
            Dim iSpeed As Integer = (Speed * 100) \ 192
            Return iSpeed.ToString & "%"
        End Function

        Public Sub UpdateLastProjected()
            Dim iSpeed As Integer
            Dim dHyp As Double
            Dim s As Double
            Dim c As Double

            If mProjectedLastUpdate = mLastUpdate Then
                Exit Sub
            End If

            mProjectedLastUpdate = System.Environment.TickCount

            iSpeed = Speed
            If iSpeed = 0 Then
                mProjectedX = X
                mProjectedY = Y
            Else
                dHyp = iSpeed * LastTouched() * (1 / 1000)
                QuickSinCos.sincos_quick(HeadRad, s, c)
                's = Math.Sin(Direction * (180 / Math.PI))
                'c = Math.Cos(Direction * (180 / Math.PI))
                mProjectedX = X - CInt(Math.Round(s * dHyp))
                mProjectedY = Y + CInt(Math.Round(c * dHyp))
            End If

        End Sub

        Public ReadOnly Property GetXProjected() As Integer
            Get
                UpdateLastProjected()
                Return mProjectedX
            End Get
        End Property

        Public ReadOnly Property GetYProjected() As Integer
            Get
                UpdateLastProjected()
                Return mProjectedY
            End Get
        End Property

        Public WriteOnly Property CastTime() As DateTime
            Set(ByVal Value As DateTime)
                mCastTime = Value
            End Set
        End Property

        Public ReadOnly Property IsCasting() As Boolean
            Get
                Return Not Now >= mCastTime
            End Get
        End Property
    End Class

    <Serializable()> _
    Public Class DAOCPlayer
        Inherits DAOCMovingObject
        Private mGuild As String
        Private mLastName As String
        Private mIsInGuild As Boolean
        Private mIsInGroup As Boolean
        Private mCharacterClass As DAOCCharacterClass
        Private mRealmRank As DAOCRealmRank
        Private mRealmRankStr As String
        Private mFlags As Integer
        Private mCombatMode As Boolean = False
        Public Overrides Function GetObjectClass() As DAOCObjectClass
            Return DAOCObjectClass.ocPlayer
        End Function

        Public Overloads Sub Assign(ByVal aSrc As DAOCPlayer)
            MyBase.Assign(aSrc)
            mGuild = aSrc.Guild
            mLastName = aSrc.LastName
        End Sub

        Public Overloads Sub Clear()
            MyBase.Clear()
            mGuild = String.Empty
            mLastName = String.Empty
        End Sub

        Public ReadOnly Property CharacterClass() As DAOCCharacterClass
            Get
                Return mCharacterClass
            End Get
        End Property

        Public Property Guild() As String
            Get
                Return mGuild
            End Get
            Set(ByVal Value As String)
                mGuild = Value
            End Set
        End Property

        Public Property IsInGroup() As Boolean
            Get
                Return mIsInGroup
            End Get
            Set(ByVal Value As Boolean)
                mIsInGroup = Value
            End Set
        End Property

        Public Property LastName() As String
            Get
                Return mLastName
            End Get
            Set(ByVal Value As String)
                mLastName = Value
            End Set
        End Property

        Public ReadOnly Property FullName() As String
            Get
                If mLastName <> "" Then
                    Return Name & " " & mLastName
                Else
                    Return Name
                End If
            End Get
        End Property

        Private Sub UpdateRealmRank()
            Select Case Realm
                Case AccountCharInfo.DAOCRealm.drAlbion
                    If mLastName = "Guardian" Then
                        mRealmRank = DAOCRealmRank.rr1
                    ElseIf mLastName = "Warder" Then
                        mRealmRank = DAOCRealmRank.rr2
                    ElseIf mLastName = "Myrmidon" Then
                        mRealmRank = DAOCRealmRank.rr3
                    ElseIf mLastName = "Gryphon Knight" Then
                        mRealmRank = DAOCRealmRank.rr4
                    ElseIf mLastName = "Eagle Knight" Then
                        mRealmRank = DAOCRealmRank.rr5
                    ElseIf mLastName = "Phoenix Knight" Then
                        mRealmRank = DAOCRealmRank.rr6
                    ElseIf mLastName = "Alerion Knight" Then
                        mRealmRank = DAOCRealmRank.rr7
                    ElseIf mLastName = "Unicorn Knight" Then
                        mRealmRank = DAOCRealmRank.rr8
                    ElseIf mLastName = "Lion Knight" Then
                        mRealmRank = DAOCRealmRank.rr9
                    ElseIf mLastName = "Dragon Knight" Then
                        mRealmRank = DAOCRealmRank.rr10
                    Else
                        mRealmRank = DAOCRealmRank.rrUnknown
                    End If
                Case AccountCharInfo.DAOCRealm.drMidgard
                    If mLastName = "Skiltvakten" Then
                        mRealmRank = DAOCRealmRank.rr1
                    ElseIf mLastName = "Isen Vakten" Then
                        mRealmRank = DAOCRealmRank.rr2
                    ElseIf mLastName = "Flammen Vakten" Then
                        mRealmRank = DAOCRealmRank.rr3
                    ElseIf mLastName = "Elding Vakten" Then
                        mRealmRank = DAOCRealmRank.rr4
                    ElseIf mLastName = "Stormur Vakten" Then
                        mRealmRank = DAOCRealmRank.rr5
                    ElseIf mLastName = "Isen Herra" Then
                        mRealmRank = DAOCRealmRank.rr6
                    ElseIf mLastName = "Flammen Herra" Then
                        mRealmRank = DAOCRealmRank.rr7
                    ElseIf mLastName = "Elding Herra" Then
                        mRealmRank = DAOCRealmRank.rr8
                    ElseIf mLastName = "Stormur Herra" Then
                        mRealmRank = DAOCRealmRank.rr9
                    ElseIf mLastName = "Einherjar" Then
                        mRealmRank = DAOCRealmRank.rr10
                    Else
                        mRealmRank = DAOCRealmRank.rrUnknown
                    End If

                Case AccountCharInfo.DAOCRealm.drHibernia
                    If mLastName = "Savant" Then
                        mRealmRank = DAOCRealmRank.rr1
                    ElseIf mLastName = "Cosantoir" Then
                        mRealmRank = DAOCRealmRank.rr2
                    ElseIf mLastName = "Brehon" Then
                        mRealmRank = DAOCRealmRank.rr3
                    ElseIf mLastName = "Grove Protector" Then
                        mRealmRank = DAOCRealmRank.rr4
                    ElseIf mLastName = "Raven Ardent" Then
                        mRealmRank = DAOCRealmRank.rr5
                    ElseIf mLastName = "Silver Hand" Then
                        mRealmRank = DAOCRealmRank.rr6
                    ElseIf mLastName = "Thunderer" Then
                        mRealmRank = DAOCRealmRank.rr7
                    ElseIf mLastName = "Gilded Spear" Then
                        mRealmRank = DAOCRealmRank.rr8
                    ElseIf mLastName = "Tiarna" Then
                        mRealmRank = DAOCRealmRank.rr9
                    ElseIf mLastName = "Emerald Ridere" Then
                        mRealmRank = DAOCRealmRank.rr10
                    Else
                        mRealmRank = DAOCRealmRank.rrUnknown
                    End If
                Case Else
                    mRealmRank = DAOCRealmRank.rrUnknown
            End Select

            UpdateRealmRankStr()
        End Sub

        Private Sub UpdateRealmRankStr()
            Select Case mRealmRank
                Case DAOCRealmRank.rrUnknown
                    mRealmRankStr = ""
                Case DAOCRealmRank.rr1
                    mRealmRankStr = "RR1"
                Case DAOCRealmRank.rr2
                    mRealmRankStr = "RR2"
                Case DAOCRealmRank.rr3
                    mRealmRankStr = "RR3"
                Case DAOCRealmRank.rr4
                    mRealmRankStr = "RR4"
                Case DAOCRealmRank.rr5
                    mRealmRankStr = "RR5"
                Case DAOCRealmRank.rr6
                    mRealmRankStr = "RR6"
                Case DAOCRealmRank.rr7
                    mRealmRankStr = "RR7"
                Case DAOCRealmRank.rr8
                    mRealmRankStr = "RR8"
                Case DAOCRealmRank.rr9
                    mRealmRankStr = "RR9"
                Case DAOCRealmRank.rr10
                    mRealmRankStr = "RR10"
            End Select
        End Sub

        Public WriteOnly Property UpdateFlags() As Integer 'received in updates
            Set(ByVal Value As Integer)
                mFlags = Value
            End Set
        End Property

        Public ReadOnly Property IsDead() As Boolean 'received in updates
            Get
                If (mFlags And 7168) >> 10 = 5 Then
                    Return True
                Else
                    Return False
                End If
            End Get

            '0 (000b) - Stand
            '1 (001b) - Swim
            '2 (010b) - Jump (landing)
            '3 (011b) - Jump (rising)
            '4 (100b) - Sit
            '5 (101b) - Die
            '6 (110b) - Horse riding (heading entry becomes mob id of horse)
            '7 (111b) - Climbing
        End Property

        Public ReadOnly Property IsRiding() As Boolean 'received in updates
            Get
                If (mFlags And 7168) >> 10 = 6 Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property IsSitting() As Boolean 'received in updates
            Get
                If (mFlags And 7168) >> 10 = 4 Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property IsClimbing() As Boolean 'received in updates
            Get
                If (mFlags And 7168) >> 10 = 7 Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property IsSwimming() As Boolean
            Get
                If (mFlags And 7168) >> 10 = 1 Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public Overrides Property Combat() As Boolean
            Get
                Return mCombatMode
            End Get
            Set(ByVal Value As Boolean)
                mCombatMode = Value
            End Set
        End Property
    End Class

    <Serializable()> _
    Public Class DAOCMob
        Inherits DAOCMovingObject
        Private mTypeTag As String
        Public Property TypeTag() As String
            Get
                Return mTypeTag
            End Get
            Set(ByVal Value As String)
                mTypeTag = Value
            End Set
        End Property

        Public Overrides Function GetObjectClass() As DAOCObjectClass
            Return DAOCObjectClass.ocMob
        End Function

        Public Overloads Sub Assign(ByVal aSrc As DAOCMob)
            MyBase.Assign(aSrc)
            mTypeTag = aSrc.TypeTag
        End Sub
    End Class

    <Serializable()> _
    Public Class DAOCVehicle
        Inherits DAOCMovingObject

        Public Overrides Function GetObjectClass() As DAOCObjectClass
            Return DAOCObjectClass.ocVehicle
        End Function

        Public Overloads Sub Assign(ByVal aSrc As DAOCMob)
            MyBase.Assign(aSrc)
        End Sub
    End Class

    <Serializable()> _
    Public Class DAOCCurrency
        Private mPlatinum As Integer
        Private mSilver As Integer
        Private mMithril As Integer
        Private mGold As Integer
        Private mCopper As Integer
        Public Sub Clear()
            mPlatinum = 0
            mSilver = 0
            mMithril = 0
            mGold = 0
            mCopper = 0
        End Sub

        Public Overloads Function ToString() As String
            Dim Result As String = String.Empty
            If mCopper > 0 Then
                Result &= CStr(mCopper) & " copper"
            End If
            If mSilver > 0 Then
                If Result.Length <> 0 Then
                    Result = ", " & Result
                End If
                Result = CStr(mSilver) & " silver" & Result
            End If
            If mGold > 0 Then
                If Result.Length <> 0 Then
                    Result = ", " & Result
                End If
                Result = CStr(mGold) & " gold" & Result
            End If
            If mPlatinum > 0 Then
                If Result.Length <> 0 Then
                    Result = ", " & Result
                End If
                Result = CStr(mPlatinum) & " platinum" & Result
            End If
            If mMithril > 0 Then
                If Result.Length <> 0 Then
                    Result = ", " & Result
                End If
                Result = CStr(mMithril) & " mithril" & Result
            End If

            Return Result

        End Function

        Public Property Copper() As Integer
            Get
                Return mCopper
            End Get
            Set(ByVal Value As Integer)
                mCopper = Value
            End Set
        End Property

        Public Property Silver() As Integer
            Get
                Return mSilver
            End Get
            Set(ByVal Value As Integer)
                mSilver = Value
            End Set
        End Property

        Public Property Gold() As Integer
            Get
                Return mGold
            End Get
            Set(ByVal Value As Integer)
                mGold = Value
            End Set
        End Property

        Public Property Platinum() As Integer
            Get
                Return mPlatinum
            End Get
            Set(ByVal Value As Integer)
                mPlatinum = Value
            End Set
        End Property

        Public Property Mithril() As Integer
            Get
                Return mMithril
            End Get
            Set(ByVal Value As Integer)
                mMithril = Value
            End Set
        End Property
    End Class

    <Serializable()> _
    Public Class DAOCLocalPlayer
        Inherits DAOCMovingObject
        Private mInventory As DAOCInventory
        Private mSkills As DAOCNameValueList
        Private mSpecializations As DAOCNameValueList
        Private mAbilities As DAOCNameValueList
        Private mSpells As DAOCNameValueList
        Private mStyles As DAOCNameValueList
        Private mCurrency As DAOCCurrency
        Private mRealmAbilities As DAOCNameValueList
        Protected mCharacterClass As DAOCCharacterClass
        Protected mRealmRank As DAOCRealmRank
        Protected mRealmRankStr As String
        Private mMana As Byte
        Private mStamina As Byte
        Private mConcentration As Byte
        Private mImDead As Boolean
        Private mIsSitting As Boolean
        Private mStunned As Boolean
        Private mStealthed As Boolean
        Private mRealm As AccountCharInfo.DAOCRealm
        Private mCombatMode As Boolean = False
        Private mLeftHand As WeaponType
        Private mRightHand As WeaponType

        Public Overrides Function GetObjectClass() As DAOCObjectClass
            Return DAOCObjectClass.ocLocalPlayer
        End Function

        Public Sub New()
            MyBase.New()
            mInventory = New DAOCInventory
            mSkills = New DAOCNameValueList
            mSpecializations = New DAOCNameValueList
            mAbilities = New DAOCNameValueList
            mSpells = New DAOCNameValueList
            mStyles = New DAOCNameValueList
            mRealmAbilities = New DAOCNameValueList
            mCurrency = New DAOCCurrency
        End Sub

        Protected Overrides Sub Finalize()
            mSkills = Nothing
            mStyles = Nothing
            mSpells = Nothing
            mAbilities = Nothing
            mSkills = Nothing
            mRealmAbilities = Nothing
            mSpecializations = Nothing
            mInventory = Nothing
            MyBase.Finalize()
        End Sub

        Public Overloads Sub Clear()
            MyBase.Clear()
            mInventory.Clear()
            mSkills.Clear()
            mSpecializations.Clear()
            mAbilities.Clear()
            mSpells.Clear()
            mStyles.Clear()
            mCurrency.Clear()
            mRealmAbilities.Clear()
        End Sub

        Public ReadOnly Property Inventory() As DAOCInventory
            Get
                Return mInventory
            End Get
        End Property

        Public ReadOnly Property Abilities() As DAOCNameValueList
            Get
                Return mAbilities
            End Get
        End Property

        Public ReadOnly Property RealmAbilities() As DAOCNameValueList
            Get
                Return mRealmAbilities
            End Get
        End Property

        Public ReadOnly Property Currency() As DAOCCurrency
            Get
                Return mCurrency
            End Get
        End Property

        Public ReadOnly Property Skills() As DAOCNameValueList
            Get
                Return mSkills
            End Get
        End Property

        Public ReadOnly Property Specializations() As DAOCNameValueList
            Get
                Return mSpecializations
            End Get
        End Property

        Public ReadOnly Property Spells() As DAOCNameValueList
            Get
                Return mSpells
            End Get
        End Property

        Public ReadOnly Property Styles() As DAOCNameValueList
            Get
                Return mStyles
            End Get
        End Property

        Public Property Stunned() As Boolean
            Get
                Return mStunned
            End Get
            Set(ByVal Value As Boolean)
                mStunned = Value
            End Set
        End Property

        Public Property Mana() As Byte
            Get
                Return mMana
            End Get
            Set(ByVal Value As Byte)
                mMana = Value
            End Set
        End Property

        Public Property Concentration() As Byte
            Get
                Return mConcentration
            End Get
            Set(ByVal Value As Byte)
                mConcentration = Value
            End Set
        End Property

        Public Property Stamina() As Byte
            Get
                Return mStamina
            End Get
            Set(ByVal Value As Byte)
                mStamina = Value
            End Set
        End Property

        Public Property ImDead() As Boolean
            Get
                Return mImDead
            End Get
            Set(ByVal Value As Boolean)
                mImDead = Value
            End Set
        End Property

        Public Property IsSitting() As Boolean
            Get
                Return mIsSitting
            End Get
            Set(ByVal Value As Boolean)
                mIsSitting = Value
            End Set
        End Property

        Public Property PlayerStealthed() As Boolean
            Get
                Return mStealthed
            End Get
            Set(ByVal Value As Boolean)
                mStealthed = Value
            End Set
        End Property

        Public Property PlayerRealm() As AccountCharInfo.DAOCRealm
            Get
                Return mRealm
            End Get
            Set(ByVal Value As AccountCharInfo.DAOCRealm)
                mRealm = Value
            End Set
        End Property

        Public Overrides Property Combat() As Boolean
            Get
                Return mCombatMode
            End Get
            Set(ByVal Value As Boolean)
                mCombatMode = Value
            End Set
        End Property

        Public Property LeftHand() As WeaponType
            Get
                Return mLeftHand
            End Get
            Set(ByVal Value As WeaponType)
                mLeftHand = Value
            End Set
        End Property

        Public Property RightHand() As WeaponType
            Get
                Return mRightHand
            End Get
            Set(ByVal Value As WeaponType)
                mRightHand = Value
            End Set
        End Property
    End Class

    <Serializable()> _
    Public Class DAOCUnknownMovingObject
        Inherits DAOCMovingObject
        Public Function GetName() As String
            Return String.Format("UnknownObject NFO 0x{0} 0x{1}", Hex(SpawnID), Hex(PlayerID))
        End Function

        Public Overrides Function GetObjectClass() As DAOCObjectClass
            Return DAOCObjectClass.ocUnknown
        End Function
    End Class

#Region "Group"
    <Serializable()> _
    Public Enum PlayerInGroupStatus
        Normal = 0
        Dead = 1
        Mezzed = 2
        Diseased = 4
        Poisoned = 8
        LinkDead = 10
        InAnotherRegion = 20
    End Enum

    <Serializable()> _
    Public Enum agEnum
        Added
        Removed
    End Enum

    <Serializable()> _
    Public Class GroupEventArgs
        Private _Member As GroupMember
        Private _Action As agEnum
        Public Sub New(ByVal aMember As GroupMember, ByVal aAction As agEnum)
            _Member = aMember
            _Action = aAction
        End Sub

        Public Property Member() As GroupMember
            Get
                Return _Member
            End Get
            Set(ByVal Value As GroupMember)
                _Member = Value
            End Set
        End Property

        Public Property Action() As agEnum
            Get
                Return _Action
            End Get
            Set(ByVal Value As agEnum)
                _Action = Value
            End Set
        End Property
    End Class

    <Serializable()> _
    Public Class GroupMember
        Private mName As String
        Private mClass As String
        Private mLevel As Byte
        Private mHealth As Byte
        Private mMana As Byte
        Private mEndurance As Byte
        Private mID As Integer
        Private mIndex As Integer
        Private mUpdated As Boolean
        Private mStatus As PlayerInGroupStatus
        Public Property Name() As String
            Get
                Return mName
            End Get
            Set(ByVal Value As String)
                mName = Value
            End Set
        End Property

        Public Property [Class]() As String
            Get
                Return mClass
            End Get
            Set(ByVal Value As String)
                mClass = Value
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

        Public Property Health() As Byte
            Get
                Return mHealth
            End Get
            Set(ByVal Value As Byte)
                mHealth = Value
            End Set
        End Property

        Public Property Mana() As Byte
            Get
                Return mMana
            End Get
            Set(ByVal Value As Byte)
                mMana = Value
            End Set
        End Property

        Public Property Endurance() As Byte
            Get
                Return mEndurance
            End Get
            Set(ByVal Value As Byte)
                mEndurance = Value
            End Set
        End Property

        Public Property Updated() As Boolean
            Get
                Return mUpdated
            End Get
            Set(ByVal Value As Boolean)
                mUpdated = Value
            End Set
        End Property

        Public Property Status() As PlayerInGroupStatus
            Get
                Return mStatus
            End Get
            Set(ByVal Value As PlayerInGroupStatus)
                mStatus = Value
            End Set
        End Property

        Public Property ID() As Integer
            Get
                Return mID
            End Get
            Set(ByVal Value As Integer)
                mID = Value
            End Set
        End Property

        Public Property Index() As Integer
            Get
                Return mIndex
            End Get
            Set(ByVal Value As Integer)
                mIndex = Value
            End Set
        End Property
    End Class

    <Serializable()> _
    Public Class Group
        Private hash As Generic.Dictionary(Of Integer, GroupMember)
        Public Event GroupChanged(ByVal Sender As Group, ByVal e As GroupEventArgs)
        Public Sub New()
            hash = New Generic.Dictionary(Of Integer, GroupMember)
        End Sub

        Public Sub RemoveItem(ByVal Key As Integer)
            SyncLock hash
                hash.Remove(Key)
            End SyncLock
        End Sub

        Public Sub AddOrReplace(ByVal aObject As GroupMember)
            SyncLock hash
                hash.Remove(aObject.Index)
                hash.Add(aObject.Index, aObject)
            End SyncLock
        End Sub

        Public ReadOnly Property GroupMemberTable() As Generic.Dictionary(Of Integer, GroupMember)
            Get
                Return hash
            End Get
        End Property

        Public Sub ClearTable()
            hash.Clear()
        End Sub
    End Class
#End Region
End Namespace

'all these objects should inherit MarshalByRefObject because I want System.Environment.TickCount to run 
'on the server only but it's so slow it's just not worth it. 