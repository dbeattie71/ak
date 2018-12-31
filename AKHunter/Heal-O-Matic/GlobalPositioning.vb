Public Class GlobalPositioning
    Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
    Private Declare Function GetForegroundWindow Lib "user32" Alias "GetForegroundWindow" () As Integer
    Dim _AK As AutoKillerScript.clsAutoKillerScript
    'Dim _Myself As String
    Dim MyInfo As Info
    Public GroupDistance As Integer = 4000
    Private Class DblPoint
        Public X As Double
        Public Y As Double

        Public Sub New(ByVal newX As Double, ByVal newY As Double)
            X = newX
            Y = newY
        End Sub
    End Class
    Private Class Info
        Public Name As String
        Public XCoord As Single
        Public YCoord As Single
        Public Heading As Short
    End Class
    Public Sub New(ByRef AK As AutoKillerScript.clsAutoKillerScript)
        _AK = AK
        '_Myself = MySelf
    End Sub
    Public Function GetGroupValues() As ArrayList
        'dgb
        'Dim aList As ArrayList = _AK.GroupNames(7)
        'Dim aList As ArrayList = _AK.GroupMemberInfo
        Dim objGroupMemberInfo As AKServer.DLL.DAoCServer.Group = _AK.GroupMemberInfo
        Dim htGroupMemberTable As Hashtable = objGroupMemberInfo.GroupMemberTable

        Dim objGrpMbr As AKServer.DLL.DAoCServer.GroupMember

        Dim ReturnList As New ArrayList
        Dim i As Integer

        'For i = 0 To 7
        For Each objGrpMbr In htGroupMemberTable.Values
            'dgb

            'objGrpMbr = aList(i)

            If CStr(objGrpMbr.Name.Length) <> 0 Then
                'Dim aIndex As Short = _AK.SetTarget(CStr(aList(i)), False)
                Dim aIndex As Short = objGrpMbr.ID

                If aIndex <> _AK.PlayerID AndAlso aIndex <> -1 AndAlso _AK.ZDistance(_AK.gPlayerXCoord, _AK.gPlayerYCoord, _AK.gPlayerZCoord, _AK.MobXCoord(aIndex), _AK.MobYCoord(aIndex), _AK.MobZCoord(aIndex)) < GroupDistance Then
                    Dim aInfo As Info = New Info

                    'dgb

                    'aInfo.Name = CStr(aList(i))
                    'aInfo.XCoord = _AK.MobXCoord(aIndex)
                    'aInfo.YCoord = _AK.MobYCoord(aIndex)
                    'aInfo.Heading = _AK.MobDir(aIndex)

                    aInfo.Name = objGrpMbr.Name
                    aInfo.XCoord = _AK.MobXCoord(objGrpMbr.ID)
                    aInfo.YCoord = _AK.MobYCoord(objGrpMbr.ID)
                    aInfo.Heading = _AK.MobDir(objGrpMbr.ID)

                    ReturnList.Add(aInfo)
                    'Debug.WriteLine("AInfo: " & CStr(aList(i)) & ": " & CStr(aInfo.XCoord) & " - " & CStr(aInfo.YCoord) & " head: " & CStr(aInfo.Heading))
                End If
            End If
        Next
        If MyInfo Is Nothing Then
            MyInfo = New Info
        End If
        MyInfo.XCoord = _AK.gPlayerXCoord
        MyInfo.YCoord = _AK.gPlayerYCoord
        MyInfo.Heading = _AK.PlayerDir
        Debug.WriteLine("MyInfo: " & CStr(MyInfo.XCoord) & " - " & CStr(MyInfo.YCoord) & " head: " & CStr(MyInfo.Heading))

        Return ReturnList
    End Function
    Private Function GetAngle(ByVal ax As Single, ByVal ay As Single, ByVal bx As Single, ByVal by As Single, ByVal cx As Single, ByVal cy As Single) As Single
        Dim side_a As Single
        Dim side_b As Single
        Dim side_c As Single

        Try
            side_a = Math.Sqrt((bx - cx) ^ 2 + (by - cy) ^ 2)
            side_b = Math.Sqrt((ax - cx) ^ 2 + (ay - cy) ^ 2)
            side_c = Math.Sqrt((ax - bx) ^ 2 + (ay - by) ^ 2)

            Return Math.Acos((side_b ^ 2 - side_a ^ 2 - side_c ^ 2) / (-2 * side_a * side_c))
        Catch ex As Exception
            Return 0 'how do I return "Not a number"?
        End Try
    End Function
    Public Function InGroup() As Boolean
        Dim total_angle As Single
        Dim aList As ArrayList = GetGroupValues()
        Dim aFirst As Info
        Dim aLast As Info

        aFirst = CType(aList(0), Info)
        aLast = CType(aList(aList.Count - 1), Info)

        total_angle = GetAngle(aLast.XCoord, aLast.YCoord, MyInfo.XCoord, MyInfo.YCoord, aFirst.XCoord, aFirst.YCoord)

        Dim i As Integer
        For i = 0 To aList.Count - 2
            aFirst = CType(aList(i), Info)
            aLast = CType(aList(i + 1), Info)

            total_angle = total_angle + GetAngle(aFirst.XCoord, aFirst.YCoord, MyInfo.XCoord, MyInfo.YCoord, aLast.XCoord, aLast.YCoord)
        Next

        Return (Math.Abs(total_angle) > 0.00001)
    End Function
    Public Function FindHeading() As Short
        Dim aList As ArrayList = GetGroupValues()
        Dim i As Integer
        Dim total_heading As Integer = 0

        If aList.Count = 0 Then
            Return MyInfo.Heading
        End If

        For i = 0 To aList.Count - 1
            total_heading = total_heading + CType(aList(i), Info).Heading
        Next

        'Debug.WriteLine("Found Heading: " & CStr(Math.Abs(total_heading \ aList.Count) Mod 360))
        Return Math.Abs(total_heading \ aList.Count) Mod 360
    End Function
    Public Function IsDAOCActive() As Boolean
        If GetForegroundWindow = FindWindow(vbNullString, "DAOCMWC") Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub PositionXY(ByVal distance As Double)
        Dim aPoint As Point = FindCentroid()
        Dim counter As Integer = 0
        Dim HasMoved As Boolean = False

        'aPoint = FindCentroid()
        If _AK.ZDistance(_AK.gPlayerXCoord, _AK.gPlayerYCoord, _AK.gPlayerZCoord, aPoint.X, aPoint.Y, _AK.gPlayerZCoord) >= distance Then
            If Not frmMain.StopRunning Then
                _AK.StartRunning()
            Else
                Exit Sub
            End If
            Dim isMoving As Boolean = True
            HasMoved = True
            Do While _AK.ZDistance(_AK.gPlayerXCoord, _AK.gPlayerYCoord, _AK.gPlayerZCoord, aPoint.X, aPoint.Y, _AK.gPlayerZCoord) >= distance OrElse isMoving = True _
            OrElse IsDAOCActive() AndAlso Not _AK.IsPlayerDead AndAlso Not frmMain.StopRunning
                _AK.TurnToHeading(Math.Abs(_AK.FindHeading(_AK.gPlayerXCoord, _AK.gPlayerYCoord, aPoint.X, aPoint.Y)) Mod 360)
                Threading.Thread.CurrentThread.Sleep(200)
                Dim thePoint As Point = FindCentroid()
                If thePoint.X = aPoint.X AndAlso thePoint.Y = aPoint.Y Then
                    isMoving = False
                End If
                aPoint = thePoint
            Loop
            _AK.StopRunning()
            If _AK.ZDistance(_AK.gPlayerXCoord, _AK.gPlayerYCoord, _AK.gPlayerZCoord, aPoint.X, aPoint.Y, _AK.gPlayerZCoord) <= distance AndAlso HasMoved = True Then
                _AK.TurnToHeading(FindHeading)
            End If
        End If
    End Sub
    Public Sub PositionXY(ByVal aPoint As Point, ByVal Distance As Double)
        PositionXY(aPoint.X, aPoint.Y, Distance)
    End Sub
    Public Sub PositionXY(ByVal x As Double, ByVal y As Double, ByVal Distance As Double)
        Dim Counter As Integer = 0
        Dim HasMoved As Boolean = False

        If _AK.ZDistance(_AK.gPlayerXCoord, _AK.gPlayerYCoord, _AK.gPlayerZCoord, x, y, _AK.gPlayerZCoord) >= Distance Then
            _AK.StartRunning()
            'Debug.WriteLine("Going to: " & CStr(x) & " - " & CStr(y))
            Do While _AK.ZDistance(_AK.gPlayerXCoord, _AK.gPlayerYCoord, _AK.gPlayerZCoord, x, y, _AK.gPlayerZCoord) >= Distance
                HasMoved = True
                'Debug.WriteLine("Turning to heading: " & CStr(Math.Abs(_AK.FindHeading(_AK.gPlayerXCoord, _AK.gPlayerYCoord, x, y)) Mod 360))
                _AK.TurnToHeading(Math.Abs(_AK.FindHeading(_AK.gPlayerXCoord, _AK.gPlayerYCoord, x, y)) Mod 360)
                Threading.Thread.CurrentThread.Sleep(100)
                'Application.DoEvents()
                'If Counter > 1000 Then
                '    Exit Do
                'End If
                'Counter += 1
            Loop
            _AK.StopRunning()
            If _AK.ZDistance(_AK.gPlayerXCoord, _AK.gPlayerYCoord, _AK.gPlayerZCoord, x, y, _AK.gPlayerZCoord) <= Distance AndAlso HasMoved = True Then
                _AK.TurnToHeading(FindHeading)
            End If
        End If
    End Sub
    Private Function SignedPolygonArea(ByVal m_Points As ArrayList) As Double
        Dim pt As Integer
        Dim area As Single

        ' Get the areas.
        For pt = 0 To m_Points.Count - 2
            area = area + _
                (m_Points(pt + 1).X - m_Points(pt).X) * _
                (m_Points(pt + 1).Y + m_Points(pt).Y) / 2
        Next pt

        ' Return the result.
        SignedPolygonArea = area
    End Function
    ' Find the polygon's area.
    Public Function PolygonArea(ByVal m_Points As ArrayList) As Double
        ' Return the absolute value of the signed area.
        PolygonArea = Math.Abs(SignedPolygonArea(m_Points))
    End Function
    ' Find the polygon's centroid.
    Private Function FindTheCentroid(ByVal m_Points As ArrayList) As DblPoint
        Dim pt As Integer
        Dim second_factor As Double
        Dim X As Double = 0
        Dim Y As Double = 0

        For pt = 0 To m_Points.Count - 2
            second_factor = m_Points(pt).X * m_Points(pt + 1).Y - m_Points(pt + 1).X * m_Points(pt).Y
            X = X + (m_Points(pt).X + m_Points(pt + 1).X) * second_factor
            Y = Y + (m_Points(pt).Y + m_Points(pt + 1).Y) * second_factor
        Next

        Return New DblPoint(X, Y)
    End Function
    Public Function FindCentroid() As Point
        Dim aList As ArrayList = GetGroupValues()
        Dim i As Integer
        Dim second_factor As Double
        Dim polygon_area As Double
        Dim x As Double
        Dim y As Double

        'add first point to the end
        If aList.Count > 2 Then
            aList.Add(aList(0))
            Dim aPoints As ArrayList = New ArrayList

            For i = 0 To aList.Count - 1
                aPoints.Add(New DblPoint(CType(aList(i), Info).XCoord, CType(aList(i), Info).YCoord))
            Next

            Dim P As DblPoint = FindTheCentroid(aPoints)

            polygon_area = Math.Round(PolygonArea(aPoints))
            Debug.WriteLine("Area: " & polygon_area)
            If polygon_area < 1500 Then
                x = CType(aList(0), Info).XCoord
                y = CType(aList(0), Info).YCoord
            Else
                x = P.X / 6 / polygon_area
                y = P.Y / 6 / polygon_area
            End If

            If x < 0 Then x = -x
            If y < 0 Then y = -y
        ElseIf aList.Count = 2 Then
            ' 2 Members
            x = (CType(aList(0), Info).XCoord + CType(aList(1), Info).XCoord) / 2
            y = (CType(aList(0), Info).YCoord + CType(aList(1), Info).YCoord) / 2
        Else ' 1 Other Member
            x = CType(aList(0), Info).XCoord
            y = CType(aList(0), Info).YCoord
        End If

        Debug.WriteLine("Centroid: " & CStr(x) & " - " & CStr(y))
        Return New Point(Math.Round(x), Math.Round(y))
    End Function
    Protected Overrides Sub Finalize()
        _AK = Nothing
        MyBase.Finalize()
    End Sub
End Class
