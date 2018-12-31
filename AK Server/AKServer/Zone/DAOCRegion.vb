Imports System.IO
Imports System.Xml.Serialization
Imports System.ComponentModel
Imports Microsoft.DirectX
Imports Microsoft.DirectX.Direct3D
Imports Microsoft.DirectX.Generic
Namespace DAoCServer
    Public Interface IPoint3D
        Property X() As Integer
        Property Y() As Integer
        Property Z() As Integer
    End Interface

    <Serializable()> _
   Public NotInheritable Class Point3D
        Implements IPoint3D
        Private _X As Integer
        Private _Y As Integer
        Private _Z As Integer

        Public Sub New()
        End Sub

        Public Sub New(ByVal X As Integer, ByVal Y As Integer, ByVal Z As Integer)
            _X = X
            _Y = Y
            _Z = Z
        End Sub

        Public Property X() As Integer Implements IPoint3D.X
            Get
                Return _X
            End Get
            Set(ByVal Value As Integer)
                _X = Value
            End Set
        End Property

        Public Property Y() As Integer Implements IPoint3D.Y
            Get
                Return _Y
            End Get
            Set(ByVal Value As Integer)
                _Y = Value
            End Set
        End Property

        Public Property Z() As Integer Implements IPoint3D.Z
            Get
                Return _Z
            End Get
            Set(ByVal Value As Integer)
                _Z = Value
            End Set
        End Property

        Public Sub Clear()
            X = 0
            Y = 0
            Z = 0
        End Sub
    End Class

    <Serializable()> _
    Public Enum DAOCRealm
        drNeutral
        drAlbion
        drMidgard
        drHibernia
    End Enum

    <Serializable()> _
    Public Enum DAOCZoneType
        dztUnknown
        dztOverworld
        dztCity
        dztDungeon
        dztHousing
    End Enum

    <Serializable()> _
    Public Class DAOCZoneInfo
        Private mZoneType As DAOCZoneType
        Private mRotate As Integer
        Private mZoneNum As Integer
        Private mRegion As Integer
        Private mMapName As String
        Private mZoneName As String
        Private mBaseLoc As Point
        Private mMaxLoc As Point
        Private mRealm As DAOCRealm
        Private mAdjacentZones As DAOCZoneInfoList
        Private mVMap As New List(Of VectorMap)

        Public ReadOnly Property ZoneType() As DAOCZoneType
            Get
                Return mZoneType
            End Get
        End Property

        Public Property AdjacentZones() As DAOCZoneInfoList
            Get
                Return mAdjacentZones
            End Get
            Set(ByVal Value As DAOCZoneInfoList)
                mAdjacentZones = Value
            End Set
        End Property

        Public Sub LoadFromXML(ByVal aZoneInfo As MapInfo)
            mRegion = aZoneInfo.Region
            mBaseLoc = aZoneInfo.BaseLoc
            mMaxLoc = aZoneInfo.MaxLoc
            mZoneType = aZoneInfo.ZoneType
            mRealm = aZoneInfo.Realm

            mZoneName = aZoneInfo.ZoneName
            mMapName = aZoneInfo.MapName
            mZoneNum = aZoneInfo.ZoneNum
            mRotate = aZoneInfo.Rotate
        End Sub

        Public Shadows Function ToString(Optional ByVal Indent As String = "") As String
            Dim sReturn As String = String.Format("Zone Info for zone {0} region {1} [{2}]:" & vbCrLf & _
                "  BaseLoc (x,y): {3},{4}" & vbCrLf & _
                "  MaxLoc (x,y): {5},{6}" & vbCrLf & _
                "  Zone type: {7}" & vbCrLf & _
                "  Rotate: {8}" & vbCrLf, _
                mZoneNum, mRegion, mMapName, mBaseLoc.X, mBaseLoc.Y, mMaxLoc.X, mMaxLoc.Y, mZoneType, mRotate)
            If Indent.Length = 0 Then
                For Each pAZ As DAOCZoneInfo In AdjacentZones.ObjectTable.Values
                    sReturn &= pAZ.ToString(Indent & vbTab)
                Next
            End If
            Return sReturn
        End Function

        Public Function ContainsPoint(ByVal ARegion As Integer, ByVal aX As Integer, ByVal aY As Integer) As Boolean
            Return (Region = ARegion) AndAlso (aX > mBaseLoc.X) AndAlso (aX < mMaxLoc.X) AndAlso (aY > mBaseLoc.Y) AndAlso (aY < mMaxLoc.Y)
        End Function

        Public Function ZoneConvertHead(ByVal AHead As Integer) As Integer
            Dim Result As Integer
            Result = AHead + mRotate + 180
            If Result > 360 Then
                Result -= 360
            End If
            Return Result
        End Function

        Public Function ZoneToWorldX(ByVal AX As Integer) As Integer
            Return AX + mBaseLoc.X
        End Function

        Public Function ZoneToWorldY(ByVal AY As Integer) As Integer
            Return AY + mBaseLoc.Y
        End Function

        Public Function WorldToZoneX(ByVal AX As Integer) As Integer
            Return AX - mBaseLoc.X
        End Function

        Public Function WorldToZoneY(ByVal AY As Integer) As Integer
            Return AY - mBaseLoc.Y
        End Function

        Public Sub LoadVectorMap()
            Dim strString As String
            Dim sr As StreamReader
            Dim i As Integer

            mVMap.Clear()

            sr = New StreamReader("Maps\" & mMapName)

            Try
                While sr.Peek > -1
                    strString = sr.ReadLine()
                    If strString.Length > 0 Then
                        Dim myarray() As String = strString.Split(CChar(","))
                        If Not myarray(0).Length > 1 Then
                            Select Case myarray(0)
                                Case "M", "F"
                                    Dim map As New VectorMap
                                    map.LineType = myarray(0)

                                    Select Case myarray(2).ToLower
                                        Case "black"
                                            map.LineColor = System.Drawing.Color.DarkGray 'can't see black on black
                                        Case "white"
                                            map.LineColor = System.Drawing.Color.White
                                        Case "gray"
                                            map.LineColor = System.Drawing.Color.Gray
                                        Case "green"
                                            map.LineColor = System.Drawing.Color.Green
                                        Case "red"
                                            map.LineColor = System.Drawing.Color.Red
                                        Case "blue"
                                            map.LineColor = System.Drawing.Color.Blue
                                        Case "orange"
                                            map.LineColor = System.Drawing.Color.Orange
                                        Case "pink"
                                            map.LineColor = System.Drawing.Color.Pink
                                        Case "magenta"
                                            map.LineColor = System.Drawing.Color.Magenta
                                        Case "brown"
                                            map.LineColor = System.Drawing.Color.Brown
                                        Case "yellow"
                                            map.LineColor = System.Drawing.Color.Yellow
                                    End Select

                                    map.Nodes = CShort(myarray(3))
                                    Dim Counter As Integer = 4
                                    For i = 1 To map.Nodes
                                        map.x.Add(CInt(myarray(Counter)))
                                        Counter += 1
                                        map.y.Add(CInt(myarray(Counter)))
                                        Counter += 1
                                        map.z.Add(CInt(myarray(Counter)))
                                        Counter += 1
                                    Next

                                    mVMap.Add(map)
                                Case "P"
                                    Dim map As New VectorMap

                                    map.LineType = myarray(0)
                                    map.TextName = myarray(1)

                                    Select Case myarray(2).ToLower
                                        Case "black"
                                            map.LineColor = System.Drawing.Color.DarkGray 'can't see black on black
                                        Case "white"
                                            map.LineColor = System.Drawing.Color.White
                                        Case "gray"
                                            map.LineColor = System.Drawing.Color.Gray
                                        Case "green"
                                            map.LineColor = System.Drawing.Color.Green
                                        Case "red"
                                            map.LineColor = System.Drawing.Color.Red
                                        Case "blue"
                                            map.LineColor = System.Drawing.Color.Blue
                                        Case "orange"
                                            map.LineColor = System.Drawing.Color.Orange
                                        Case "pink"
                                            map.LineColor = System.Drawing.Color.Pink
                                        Case "magenta"
                                            map.LineColor = System.Drawing.Color.Magenta
                                        Case "brown"
                                            map.LineColor = System.Drawing.Color.Brown
                                        Case "yellow"
                                            map.LineColor = System.Drawing.Color.Yellow
                                    End Select

                                    map.x.Add(CInt(myarray(3)))
                                    map.y.Add(CInt(myarray(4)))
                                    map.z.Add(CInt(myarray(5)))

                                    mVMap.Add(map)
                            End Select
                        End If
                    End If
                End While
            Catch ex As Exception
                Debug.WriteLine(ex.Message)
            Finally
                If sr IsNot Nothing Then
                    sr.Close()
                End If
            End Try
        End Sub

        Public ReadOnly Property Vmap() As List(Of VectorMap)
            Get
                Return mVMap
            End Get
        End Property

        Public ReadOnly Property Name() As String
            Get
                Return mZoneName
            End Get
        End Property

        Public ReadOnly Property Region() As Integer
            Get
                Return mRegion
            End Get
        End Property

        Public ReadOnly Property Realm() As DAOCRealm
            Get
                Return mRealm
            End Get
        End Property

        Public ReadOnly Property BaseLoc() As Point
            Get
                Return mBaseLoc
            End Get
        End Property

        Public ReadOnly Property MaxLoc() As Point
            Get
                Return mMaxLoc
            End Get
        End Property

        Public ReadOnly Property Rotate() As Integer
            Get
                Return mRotate
            End Get
        End Property

        Public ReadOnly Property MapName() As String
            Get
                Return mMapName
            End Get
        End Property

        Public ReadOnly Property ZoneNum() As Integer
            Get
                Return mZoneNum
            End Get
        End Property

        Public Sub New()
            mAdjacentZones = New DAOCZoneInfoList
        End Sub

        Protected Overrides Sub Finalize()
            mAdjacentZones = Nothing
            MyBase.Finalize()
        End Sub
    End Class

    <Serializable()> _
    Public Class DAOCZoneInfoList
        Private hash As Dictionary(Of Integer, DAOCZoneInfo)
        Public Sub New()
            hash = New Dictionary(Of Integer, DAOCZoneInfo)
        End Sub

        Public ReadOnly Property ObjectTable() As Dictionary(Of Integer, DAOCZoneInfo)
            Get
                Return hash
            End Get
        End Property

        Public Sub ClearTable()
            hash.Clear()
        End Sub

        Public Function FindZone(ByVal AZoneNum As Integer) As DAOCZoneInfo
            Return Item(AZoneNum)
        End Function

        Public Sub LoadFromFile(ByVal aFName As String)
            Dim maps As MapTable

            maps = DirectCast(MapTable.Load(aFName, GetType(MapTable)), MapTable)

            For Each m As MapInfo In maps.MapItems
                Dim pZI As DAOCZoneInfo
                pZI = New DAOCZoneInfo
                pZI.LoadFromXML(m)
                hash.Add(pZI.ZoneNum, pZI)
            Next
            UpdateAdjacentZones()
        End Sub

        Public Sub LoadFromFile(ByVal aFName As Stream)
            Dim maps As MapTable

            maps = DirectCast(MapTable.Load(aFName, GetType(MapTable)), MapTable)

            For Each m As MapInfo In maps.MapItems
                Dim pZI As DAOCZoneInfo
                pZI = New DAOCZoneInfo
                pZI.LoadFromXML(m)
                hash.Add(pZI.ZoneNum, pZI)
            Next

            UpdateAdjacentZones()
        End Sub

        Private Sub UpdateAdjacentZones()
            Dim pZone As DAOCZoneInfo

            For Each pZone In hash.Values
                pZone.AdjacentZones.ClearTable()

                For Each mZone As DAOCZoneInfo In hash.Values
                    If Not (pZone Is mZone) Then
                        If (pZone.Region = mZone.Region) AndAlso Intersections.RectsIntersect(New Rectangle(pZone.BaseLoc.X, pZone.BaseLoc.Y, pZone.MaxLoc.X - pZone.BaseLoc.X, pZone.MaxLoc.Y - pZone.BaseLoc.Y), New Rectangle(mZone.BaseLoc.X, mZone.BaseLoc.Y, mZone.MaxLoc.X - mZone.BaseLoc.X, mZone.MaxLoc.Y - mZone.BaseLoc.Y)) Then
                            ' we could, in theory, cross-add the zones to each other's adjacency
                            ' list, but I don't because it makes the loop logic cleaner }
                            pZone.AdjacentZones.ObjectTable.Add(mZone.ZoneNum, mZone)
                        End If
                    End If
                Next
            Next
        End Sub

        Public Function FindZoneForPoint(ByVal ARegion As Integer, ByVal aX As Integer, ByVal aY As Integer) As DAOCZoneInfo
            Dim pZI As DAOCZoneInfo

            For Each pZI In hash.Values
                If pZI.ContainsPoint(ARegion, aX, aY) = True Then
                    Return pZI
                End If
            Next
            Return Nothing

        End Function

        Default Public Shadows ReadOnly Property Item(ByVal key As Integer) As DAOCZoneInfo
            Get
                If hash.ContainsKey(key) Then
                    Return hash.Item(key)
                Else
                    Return Nothing
                End If
            End Get
        End Property
    End Class

    Public Class Intersections
        Public Shared Function PointInRect(ByVal ARect As Rectangle, ByVal AX As Integer, ByVal AY As Integer) As Boolean
            '{ point in rect assumes a top down rectangle (that the Top is less than the Bottom }
            Return (AX >= ARect.Left) AndAlso (AX <= ARect.Right) AndAlso (AY >= ARect.Top) AndAlso (AY <= ARect.Bottom)
        End Function

        Public Shared Function RectsIntersect(ByVal A As Rectangle, ByVal B As Rectangle) As Boolean
            '{ a rect intersects if any one of its points is inside the other
            'dude() 's rect.  We need to check both against each other because
            'one might completely contain the other, in which case only
            'one of the two checks will be true }
            Return PointInRect(A, B.Left, B.Top) Or PointInRect(A, B.Right, B.Top) Or _
                PointInRect(A, B.Left, B.Bottom) Or PointInRect(A, B.Right, B.Bottom) Or _
                PointInRect(B, A.Left, A.Top) Or PointInRect(B, A.Right, A.Top) Or _
                PointInRect(B, A.Left, A.Bottom) Or PointInRect(B, A.Right, A.Bottom)
        End Function

    End Class

    Public Class MapTable
        Inherits SerializableData
        <XmlIgnore()> Private MapItem As New List(Of MapInfo)
        Public Function AddMapItem() As MapInfo

            ' create one...
            Dim Map As New MapInfo

            ' add it to the list...
            MapItem.Add(Map)
            ' return the UserSettings...
            Return Map

        End Function

        Public Property MapItems() As MapInfo()
            Get
                ' create a new array...
                Dim MyArray(MapItem.Count - 1) As MapInfo
                MapItem.CopyTo(MyArray)
                Return MyArray
            End Get
            Set(ByVal Value As MapInfo())
                ' reset the arraylist...
                MapItem.Clear()

                ' did we get anything?
                If Value IsNot Nothing Then
                    ' go through the array and populate MapItem...
                    Dim MInfo As MapInfo
                    For Each MInfo In Value
                        MapItem.Add(MInfo)
                    Next
                End If
            End Set
        End Property
    End Class

    Public Class SerializableData
        ' Save - serialize the object to disk...
        Public Sub Save(ByVal filename As String)

            ' make a temporary filename...
            Dim tempFilename As String
            tempFilename = filename & ".tmp"

            ' does the file exist?
            Dim tempFileInfo As New FileInfo(tempFilename)
            If tempFileInfo.Exists = True Then tempFileInfo.Delete()

            ' open the file...
            Dim stream As New FileStream(tempFilename, FileMode.Create)

            ' save the object...
            Save(stream)

            ' close the file...
            stream.Close()

            ' remove the existing data file and 
            ' rename the temp file...
            tempFileInfo.CopyTo(filename, True)
            tempFileInfo.Delete()

        End Sub

        ' Save - actually perform the serialization...
        Public Sub Save(ByVal stream As Stream)
            ' create a serializer...
            Dim serializer As New XmlSerializer(Me.GetType)
            ' save the file...
            serializer.Serialize(stream, Me)
        End Sub

        ' Load - deserialize from disk...
        Public Shared Function Load(ByVal filename As String, ByVal newType As Type) As Object

            ' does the file exist?
            Dim fileInfo As New fileInfo(filename)
            If fileInfo.Exists = False Then

                ' create a blank version of the object and return that...
                Return System.Activator.CreateInstance(newType)

            End If

            ' open the file...
            Dim stream As New FileStream(filename, FileMode.Open)

            ' load the object from the stream...
            Dim newObject As Object = Load(stream, newType)

            ' close the stream...
            stream.Close()

            ' return the object...
            Return newObject

        End Function

        Public Shared Function Load(ByVal stream As Stream, ByVal newType As Type) As Object

            ' create a serializer and load the object....
            Dim serializer As New XmlSerializer(newType)
            Dim newObject As Object = serializer.Deserialize(stream)

            ' return the new object...
            Return newObject

        End Function
    End Class

    Public Class MapInfo
        Inherits SerializableData
        Private mZoneType As DAOCZoneType
        Private mRotate As Integer
        Private mZoneNum As Integer
        Private mRegion As Integer
        Private mMapName As String
        Private mZoneName As String
        Private mBaseLoc As Point
        Private mMaxLoc As Point
        Private mRealm As DAOCRealm

        Public Property ZoneType() As DAOCZoneType
            Get
                Return mZoneType
            End Get
            Set(ByVal Value As DAOCZoneType)
                mZoneType = Value
            End Set
        End Property

        Public Property ZoneName() As String
            Get
                Return mZoneName
            End Get
            Set(ByVal Value As String)
                mZoneName = Value
            End Set
        End Property

        Public Property Region() As Integer
            Get
                Return mRegion
            End Get
            Set(ByVal Value As Integer)
                mRegion = Value
            End Set
        End Property

        Public Property BaseLoc() As Point
            Get
                Return mBaseLoc
            End Get
            Set(ByVal Value As Point)
                mBaseLoc = Value
            End Set
        End Property

        Public Property MaxLoc() As Point
            Get
                Return mMaxLoc
            End Get
            Set(ByVal Value As Point)
                mMaxLoc = Value
            End Set
        End Property

        Public Property Rotate() As Integer
            Get
                Return mRotate
            End Get
            Set(ByVal Value As Integer)
                mRotate = Value
            End Set
        End Property

        Public Property MapName() As String
            Get
                Return mMapName
            End Get
            Set(ByVal Value As String)
                mMapName = Value
            End Set
        End Property

        Public Property ZoneNum() As Integer
            Get
                Return mZoneNum
            End Get
            Set(ByVal Value As Integer)
                mZoneNum = Value
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

    Public Interface IVectorMap
        Property LineColor() As System.Drawing.Color
        Property LineType() As String
        Property Nodes() As Integer
        Property TextName() As String
        Property NodePoints() As Point
        Property x() As List(Of Integer)
        Property y() As List(Of Integer)
        Property z() As List(Of Integer)
    End Interface

    <Serializable()> _
    Public Class VectorMap
        Implements IVectorMap
        Private mLineColor As System.Drawing.Color
        Private mLineType As String
        Private mNodes As Integer
        Private mNodePoints As Point
        Private mTextName As String
        Private mx As New List(Of Integer)
        Private my As New List(Of Integer)
        Private mz As New List(Of Integer)
        Private vbuffer As VertexBuffer
        Public Property LineColor() As System.Drawing.Color Implements IVectorMap.LineColor
            Get
                Return mLineColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                mLineColor = Value
            End Set
        End Property

        Public Property LineType() As String Implements IVectorMap.LineType
            Get
                Return mLineType
            End Get
            Set(ByVal Value As String)
                mLineType = Value
            End Set
        End Property

        Public Property Nodes() As Integer Implements IVectorMap.Nodes
            Get
                Return mNodes
            End Get
            Set(ByVal Value As Integer)
                mNodes = Value
            End Set
        End Property

        Public Property x() As List(Of Integer) Implements IVectorMap.x
            Get
                Return mx
            End Get
            Set(ByVal Value As List(Of Integer))
                mx = Value
            End Set
        End Property

        Public Property y() As List(Of Integer) Implements IVectorMap.y
            Get
                Return my
            End Get
            Set(ByVal Value As List(Of Integer))
                my = Value
            End Set
        End Property

        Public Property z() As List(Of Integer) Implements IVectorMap.z
            Get
                Return mz
            End Get
            Set(ByVal Value As List(Of Integer))
                mz = Value
            End Set
        End Property

        Public Property TextName() As String Implements IVectorMap.TextName
            Get
                Return mTextName
            End Get
            Set(ByVal Value As String)
                mTextName = Value
            End Set
        End Property

        Public Property NodePoints() As System.Drawing.Point Implements IVectorMap.NodePoints
            Get
                Return mNodePoints
            End Get
            Set(ByVal Value As System.Drawing.Point)
                mNodePoints = Value
            End Set
        End Property

        'must call before trying to render 
        Public Sub FillVertexBuffer(ByVal dev As Direct3D.Device)
            If vbuffer Is Nothing AndAlso mNodes > 2 Then
                vbuffer = VertexBuffer.CreateGeneric(Of CustomVertex.PositionColored)(dev, mNodes + 1, Usage.WriteOnly, VertexFormats.Position, Pool.Managed, Nothing)

                Dim verts As GraphicsBuffer(Of CustomVertex.PositionColored) = vbuffer.Lock(Of CustomVertex.PositionColored)(0, mNodes, LockFlags.None)

                For cnt As Integer = 0 To mNodes - 1
                    verts(cnt) = New CustomVertex.PositionColored(CInt(x(cnt)), CInt(y(cnt)), CInt(z(cnt)), mLineColor)
                Next
                vbuffer.Unlock()
            End If
        End Sub

        Public Sub Render(ByVal dev As Direct3D.Device, ByVal font As Direct3D.Font, ByVal Translation As Vector3, ByVal ScaleV As Vector3, ByVal Rotation As Single)
            Dim oldworld As Matrix = dev.Transform.World
            Dim newworld As Matrix = Matrix.Identity

            Matrix.Multiply(newworld, Matrix.Scaling(ScaleV))
            Matrix.Multiply(newworld, Matrix.RotationZ(Geometry.DegreeToRadian(Rotation)))
            Matrix.Multiply(newworld, Matrix.Translation(Translation))

            'newworld.Multiply(Matrix.Scaling(ScaleV))
            'newworld.Multiply(Matrix.RotationZ(Geometry.DegreeToRadian(Rotation)))
            'newworld.Multiply(Matrix.Translation(Translation))

            Select Case LineType
                Case "M", "F"
                    If vbuffer IsNot Nothing Then
                        Try
                            'Drawing our lines
                            dev.Transform.World = newworld
                            dev.SetStreamSource(0, vbuffer, 0)
                            dev.VertexFormat = CustomVertex.PositionColored.Format
                            dev.DrawPrimitives(PrimitiveType.LineStrip, 0, mNodes - 1)
                            dev.Transform.World = oldworld
                        Catch ex As Exception
                            Debug.WriteLine(ex.Message)
                        End Try
                    End If
                Case "P"
                    Dim textposition As Vector3 = New Vector3(x(0), y(0), z(0))
                    'Projecting vector from object space to screen to find actual screen coord for drawing text
                    textposition = Vector3.Project(textposition, _
                                       dev.Viewport, _
                                       dev.Transform.Projection, _
                                       dev.Transform.View, _
                                       newworld)
                    Dim xpos As Single = textposition.X
                    Dim ypos As Single = textposition.Y

                    font.DrawString(Nothing, TextName, CInt(xpos), CInt(ypos), mLineColor)
            End Select
        End Sub
    End Class

End Namespace
