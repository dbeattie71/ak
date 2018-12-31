'Option Strict On
Option Explicit On 

Imports System.Threading
Imports System.Xml

Public Class AKFarm

#Region " Variables "

    Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
    Private Declare Function GetForegroundWindow Lib "user32" Alias "GetForegroundWindow" () As Integer

    Private Const _ClassName As String = "AKFarm"

    Private _AKFarm As Thread
    Private _TerminateThread As Boolean = False

    Private _GamePath As String
    Private _RegKey As String
    Private _EnableToA As Boolean
    Private _EnableCatacombs As Boolean

    Private _Daoc As AutoKillerScript.clsAutoKillerScript

    Private _MainForm As Form
    Private _LogListBox As ListBox
    Private _MainFormTitle As String

    Private _PatrolPoints As PatrolPoints

    Private _BuffTimer As System.Timers.Timer
    Private _Buff As Boolean

    Private _stickKey As Byte
    Private _faceKey As Byte
    Private _moveBackwardKey As Byte
    Private _moveForwardKey As Byte
    Private _showInventoryKey As Byte
    Private _nearestEnemyKey As Byte

#End Region

    Public Sub New(ByRef MainForm As Form, _
               ByRef LogListbox As ListBox, _
               ByVal RequestedLevel As Integer, _
               ByVal GamePath As String, _
               ByVal RegKey As String, _
               ByVal EnableToA As Boolean, _
               ByVal EnableCatacombs As Boolean)

        Dim methodName As String = "New()"

        _MainForm = MainForm
        _LogListBox = LogListbox
        _GamePath = GamePath
        _RegKey = RegKey
        _EnableToA = EnableToA
        _EnableCatacombs = EnableCatacombs

        Try
            Dim objStart As New ThreadStart(AddressOf Me.Start)

            ' startup the thread...
            _AKFarm = New Thread(objStart)
            _AKFarm.Name = "AKFarm"
            _AKFarm.Start()

        Catch ex As Exception
            LogLineAsync(Thread.CurrentThread.Name & ": " & ex.Message)
        End Try

    End Sub

    Public Sub Terminate()

        Dim methodName As String = "Terminate()"

        Try
            '_AKFarm.StopInit()

            _TerminateThread = True
            _AKFarm.Join()
            _Daoc.StopInit()
            LogLineAsync(_AKFarm.Name & " - stopped.")

        Catch ex As Exception
            LogLineAsync(Thread.CurrentThread.Name & ": " & ex.Message)
        End Try

    End Sub

    Private Sub Start()

        Dim methodName As String = "Start()"

        Dim keys As AutoKillerScript.UserKeys
        Dim keys2 As DAOCKeyboard
        Dim processProfile As Boolean = False

        Dim patrolPointsCount As Integer
        Dim wayPointsCount As Integer
        Dim tempPatrolPoint As patrolPoint
        Dim tempWayPoints As WayPoints
        Dim tempWayPoint As WayPoint

        Try
            _Daoc = New AutoKillerScript.clsAutoKillerScript

            AddHandler _Daoc.OnLog, AddressOf LogLineAsync

            LogLineAsync("Initializing DAOCScript v" & _Daoc.getVersion & "....")

            LogLineAsync("RegKey:" & _RegKey)
            LogLineAsync("GamePath:" & _GamePath)
            LogLineAsync("EnableToA:" & _EnableToA)
            LogLineAsync("EnableCatacombs:" & _EnableCatacombs)

            With _Daoc
                .RegKey = _RegKey
                .GamePath = _GamePath
                .EnableToA = _EnableToA
                .EnableCatacombs = _EnableCatacombs
                .EnableAutoQuery = True
                .UseRegEx = True
            End With

            keys = New AutoKillerScript.UserKeys(_Daoc)
            keys2 = New DAOCKeyboard(_Daoc)

            With _Daoc
                .SetLeftTurnKey = keys.TurnLeftKey
                .SetRightTurnKey = keys.TurnRightKey
                .SetConsiderKey = keys.ConsiderKey

                .DoInit()
            End With

            _stickKey = keys.StickKey
            _faceKey = keys.FaceKey
            _moveForwardKey = keys.MoveForwardKey
            _moveBackwardKey = keys.MoveBackwardKey
            _showInventoryKey = keys2.ShowInventory_Key

            _MainFormTitle = "AKFarm - " & _Daoc.PlayerName

            While Not _TerminateThread
                If IsDAOCActive() Then 'And objDAOC.IsPlayerDead = 0 Then

                    '*********************************************************************************
                    'process profile
                    '*********************************************************************************
                    If Not processProfile Then
                        processProfile = True

                        'add log string
                        _Daoc.AddString(0, "Your target is not visible!")
                        _Daoc.AddString(1, "You can't see your target from here!")
                        _Daoc.AddString(2, "Your target is not visible. The spell fails.")

                        'start buff timer
                        StartBuffTimer()
                        _Buff = True

                        _PatrolPoints = New PatrolPoints
                        _PatrolPoints.AddPatrolPoint(1, False, 28275, 19693, 15986)

                        Dim mywp As New WayPoints
                        mywp.AddWayPoint(1, 27865, 19454, 15995)
                        mywp.AddWayPoint(2, 27434, 19351, 16011)

                        _PatrolPoints.AddPatrolPoint(2, True, 27434, 19822, 16008, mywp)

                    End If
                    
                    'iterate through all the patrol points for the instance
                    LogLineAsync("Iterate all the patrol points")

                    patrolPointsCount = 1
                    While patrolPointsCount <= _PatrolPoints.GetNumberOfPatrolPoints() And Not _TerminateThread

                        'get patrolpoint
                        tempPatrolPoint = _PatrolPoints.GetPatrolPoint(patrolPointsCount)
                        LogLineAsync("PatrolPoint:" & tempPatrolPoint.Number _
                                     & "  x:" & tempPatrolPoint.X _
                                     & "  y:" & tempPatrolPoint.Y _
                                     & "  z:" & tempPatrolPoint.Z)

                        'if no waypoints needed, moved to patrolpoint if needed
                        If tempPatrolPoint.WayPointsNeeded = False Then

                            LogLineAsync("No WayPoints needed for this PatrolPoint")

                            LogLineAsync("Moving to PatrolPoint " & tempPatrolPoint.Number)
                            Sleep(500)
                            MoveToZXY(tempPatrolPoint.X, tempPatrolPoint.Y, tempPatrolPoint.Z, 100)
                            Sleep(500)

                        Else

                            'waypoints are needed to get to patrolpoint
                            LogLineAsync("WayPoints are needed to get to this PatrolPoint")

                            'get waypoints object
                            tempWayPoints = tempPatrolPoint.WayPoints

                            'iterate the waypoints
                            wayPointsCount = 1
                            While wayPointsCount <= tempWayPoints.GetNumberOfWayPoints And Not _TerminateThread

                                'get first waypoint
                                tempWayPoint = tempWayPoints.GetWayPoint(wayPointsCount)
                                LogLineAsync("WayPoint:" & tempWayPoint.Number _
                                             & "  x:" & tempWayPoint.X _
                                             & "  y:" & tempWayPoint.Y _
                                             & "  z:" & tempWayPoint.Z)


                                LogLineAsync("Moving to WayPoint")
                                Sleep(500)
                                MoveToZXY(tempWayPoint.X, tempWayPoint.Y, tempWayPoint.Z, 100)
                                Sleep(500)

                                wayPointsCount = wayPointsCount + 1

                            End While

                            LogLineAsync("Moving to PatrolPoint " & tempPatrolPoint.Number)
                            Sleep(500)
                            MoveToZXY(tempPatrolPoint.X, tempPatrolPoint.Y, tempPatrolPoint.Z, 100)
                            Sleep(500)

                        End If

                        LogLineAsync("Sleep for 5 seconds, then check for agro")
                        Sleep(5000)
                        CheckForAgro()

                        LogLineAsync("No agro, clear mobs")
                        ClearMobs()

                        patrolPointsCount = patrolPointsCount + 1

                    End While

                    'If _Buff Then
                    '    _Buff = False

                    '    LogLineAsync("Buffing")
                    '    System.Threading.Thread.CurrentThread.Sleep(1000)
                    '    _Daoc.SendString("7~")
                    '    System.Threading.Thread.CurrentThread.Sleep(2800)
                    '    _Daoc.SendString("8~")
                    '    System.Threading.Thread.CurrentThread.Sleep(2800)
                    '    _Daoc.SendString("9~")
                    '    System.Threading.Thread.CurrentThread.Sleep(2000)
                    'End If

                    ''invite Inevoth
                    'System.Threading.Thread.CurrentThread.Sleep(1500)
                    'LogLineAsync("Invite Inevoth")
                    '_Daoc.SendString("/invite Inevoth~")
                    'System.Threading.Thread.CurrentThread.Sleep(1500)

                    'LogLineAsync("Sleeping for 5 seconds")
                    'System.Threading.Thread.CurrentThread.Sleep(5000)

                    ''disband
                    'System.Threading.Thread.CurrentThread.Sleep(1500)
                    'LogLineAsync("Disband to clear instance")
                    '_Daoc.SendString("/disband~")
                    'System.Threading.Thread.CurrentThread.Sleep(1500)

                    'LogLineAsync("Sleeping for 10 seconds")
                    'System.Threading.Thread.CurrentThread.Sleep(10000)

                    ''run once for testing
                    _TerminateThread = True

                    ''LogLineAsync("foo")
                    '_AKFarm.CurrentThread.Sleep(500)

                End If 'If IsDAOCActive() AndAlso Not objDAOC.IsPlayerDead Then

                _AKFarm.CurrentThread.Sleep(100)
            End While

        Catch ex As Exception
            LogLineAsync(Thread.CurrentThread.Name & ": " & ex.Message)
            Me.Terminate()
        End Try

    End Sub

    Private Sub CheckForAgro()

        Dim spawnID As Integer

        spawnID = _Daoc.FindClosestMobWithPlayerAsTarget(2000)
        LogLineAsync("FindClosestMobWithPlayerAsTarget: " & spawnID)

        If spawnID > 0 And Not _Daoc.IsMobDead(spawnID) Then

            LogLineAsync("Got aggro")

            _Daoc.SetTarget(spawnID)

            'face mob
            LogLineAsync("Face mob")
            Sleep(1000)
            _Daoc.SendString("/face~")
            Sleep(1000)

            Melee(spawnID)

        End If

    End Sub

    Private Sub ClearMobs()

        Dim spawnID As Integer
        'Dim noMoreMobs As Boolean

        spawnID = _Daoc.FindClosestMob(37, 47, 1000)
        LogLineAsync("FindClosestMob: " & spawnID)
        Sleep(1000)

        'noMoreMobs = False
        While Not spawnID = -1

            'set target
            LogLineAsync("SetTarget")
            _Daoc.SetTarget(spawnID)
            Sleep(1000)

            'face mob
            LogLineAsync("Face mob")
            Sleep(1000)
            _Daoc.SendString("/face~")
            Sleep(1000)

            'pull mob
            LogLineAsync("Attempting to pull " & _Daoc.MobName(spawnID))
            _Daoc.SendString("5~")
            Sleep(3500)

            'see if mob is visible for pull
            If _Daoc.QueryString(0) Or _Daoc.QueryString(1) Or _Daoc.QueryString(2) Then

                LogLineAsync(_Daoc.MobName(spawnID) & " is not visible")

            Else

                LogLineAsync(_Daoc.MobName(spawnID) & " is visible and pulled")

                LogLineAsync("Wait until mob is in range for melee")
                While _Daoc.ZDistance(_Daoc.zPlayerXCoord, _Daoc.zPlayerYCoord, _Daoc.zPlayerZCoord, _
                                 _Daoc.zMobXCoord(spawnID), _Daoc.zMobYCoord(spawnID), _Daoc.zPlayerZCoord) > 200
                    Sleep(100)
                    'can loop forever****
                End While

                LogLineAsync("Mob is in melee range, attack")

                _Daoc.SendString("1~")
                Sleep(2000)

                Melee(spawnID)

                LogLineAsync("Fight over, check to see if player is still in combat")
                If _Daoc.isPlayerInCombat() Then
                    Sleep(1000)
                    LogLineAsync("Get out of combat")
                    _Daoc.SendString("1~")
                    Sleep(1000)
                End If

                LogLineAsync("Sleeping for 2 seconds")
                Sleep(2000)

            End If

            Sleep(1000)
            spawnID = _Daoc.FindNextClosestMob(37, 47, 1000)
            LogLineAsync("FindNextClosestMob: " & spawnID)

            If spawnID = -1 Then
                'noMoreMobs = True
                LogLineAsync("No more mobs, done")
            Else
                _Daoc.SetTarget(spawnID)
            End If

            Sleep(1000)

        End While

    End Sub

    Private Sub Melee(ByVal spawnID As Integer)

        Dim fightOver As Boolean

        fightOver = False

        While Not fightOver And Not _TerminateThread
            LogLineAsync("MobHealth: " & _Daoc.MobHealth(spawnID))
            _Daoc.SendString("3~")

            Sleep(2600)

            Sleep(500)

            LogLineAsync("IsMobDead: " & _Daoc.IsMobDead(spawnID))
            LogLineAsync("IsMobInCombat: " & _Daoc.IsMobDead(spawnID))
            If Not _Daoc.isMobInCombat(spawnID) Then

                LogLineAsync("IsMobDead: " & _Daoc.IsMobDead(spawnID))
                LogLineAsync("IsMobInCombat: " & _Daoc.IsMobDead(spawnID))

                fightOver = True

            End If

        End While

        LogLineAsync("*IsMobDead: " & _Daoc.IsMobDead(spawnID))
        LogLineAsync("*IsMobInCombat: " & _Daoc.IsMobDead(spawnID))

    End Sub

    Private Sub StartBuffTimer()

        _BuffTimer = New System.Timers.Timer

        _Buff = False

        _BuffTimer.Stop()
        _BuffTimer.Interval = 900000

        AddHandler _BuffTimer.Elapsed, AddressOf BuffTimer
        _BuffTimer.Start()

    End Sub

    Private Sub BuffTimer(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)

        _Buff = True

    End Sub

    Private Function ZoneIntoTD() As Boolean

        Dim keepLooping As Boolean

        Dim doorIndex As Integer
        Dim findDoor As Boolean
        Dim getDoorIndexAttemptCount As Integer

        Dim zoned As Boolean
        Dim zoneAttemptCount As Integer

        ZoneIntoTD = False

        'find instance door
        LogLineAsync("Find instance door")

        keepLooping = False
        findDoor = False

        While Not keepLooping
            Sleep(1500)
            doorIndex = _Daoc.SetTarget("door", True)
            LogLineAsync("doorIndex: " & doorIndex)

            If doorIndex = -1 Then
                getDoorIndexAttemptCount = getDoorIndexAttemptCount + 1
                LogLineAsync("Door not found, trying again, getDoorIndexAttemptCount: " & getDoorIndexAttemptCount)
            Else
                keepLooping = True
                findDoor = True
                LogLineAsync("Found door")
            End If

            If getDoorIndexAttemptCount > 4 Then
                keepLooping = True
                findDoor = False
                LogLineAsync("Can't find door")
            End If

        End While

        If findDoor = True Then

            zoned = False

            While Not zoned

                Sleep(1500)

                'face door
                Sleep(2000)
                LogLineAsync("Face door")
                _Daoc.SendString("/face~")
                Sleep(2000)

                'open door
                Sleep(1000)
                LogLineAsync("Open door")
                _Daoc.SendString("o~")
                Sleep(4000)

                'move forward to zone in 
                LogLineAsync("Move to zone in")
                _Daoc.SendKeys(_moveForwardKey, True, False)
                Sleep(1500)
                _Daoc.SendKeys(_moveForwardKey, False, True)

                LogLineAsync("Sleep for 10 seconds")
                Sleep(10000)

                LogLineAsync("Execute kickout logic **COMMENTED OUT FOR TEST")
                'Kickout(Daoc)

                LogLineAsync("Sleeping for 18 seconds")
                Sleep(18000)

                LogLineAsync("Zone: " & _Daoc.ZoneName)

                If _Daoc.ZoneName = "Varulvhamn" Then
                    zoneAttemptCount = zoneAttemptCount + 1
                    LogLineAsync("Zone in failed, zoneAttemptCount: " & zoneAttemptCount)

                    'move backwards for 4 secs
                    LogLineAsync("Move backwards and try again")
                    _Daoc.SendKeys(_moveBackwardKey, True, False)
                    Sleep(1400)
                    _Daoc.SendKeys(_moveBackwardKey, False, True)

                Else
                    LogLineAsync("Zone in ok")
                    zoned = True
                    ZoneIntoTD = True
                End If

                If zoneAttemptCount > 2 Then
                    LogLineAsync("Zone in failed")
                    zoned = True
                    ZoneIntoTD = False
                End If

            End While

        Else
            'couldn't fine door
            ZoneIntoTD = False
        End If

    End Function

    Private Sub Kickout()

        Sleep(1000)
        _Daoc.MouseMove(885, 575)
        Sleep(500)
        _Daoc.LeftClick()
        Sleep(1000)

    End Sub

    Private Sub MoveToGXY(ByVal X As Double, ByVal Y As Double, ByVal Z As Double, ByVal Range As Double)

        'needs stuck logic

        If Not (_Daoc.ZDistance(_Daoc.gPlayerXCoord, _Daoc.gPlayerYCoord, _Daoc.gPlayerZCoord, X, Y, Z)) < Range Then
            _Daoc.StartRunning()

            While _Daoc.ZDistance(_Daoc.gPlayerXCoord, _Daoc.gPlayerYCoord, _Daoc.gPlayerZCoord, X, Y, Z) > Range _
                And Not _Daoc.IsPlayerDead()

                _Daoc.TurnToHeading(_Daoc.FindHeading(_Daoc.gPlayerXCoord, _Daoc.gPlayerYCoord, X, Y))
                _AKFarm.Sleep(100)
            End While

            _Daoc.StopRunning()
        End If

    End Sub

    Private Sub MoveToZXY(ByVal X As Double, ByVal Y As Double, ByVal Z As Double, ByVal Range As Double)

        'needs stuck logic

        If Not (_Daoc.ZDistance(_Daoc.zPlayerXCoord, _Daoc.zPlayerYCoord, _Daoc.zPlayerZCoord, X, Y, Z)) < Range Then
            _Daoc.StartRunning()

            While _Daoc.ZDistance(_Daoc.zPlayerXCoord, _Daoc.zPlayerYCoord, _Daoc.zPlayerZCoord, X, Y, Z) > Range _
                And Not _Daoc.IsPlayerDead() And Not _TerminateThread

                _Daoc.TurnToHeading(_Daoc.FindHeading(_Daoc.zPlayerXCoord, _Daoc.zPlayerYCoord, X, Y))
                _AKFarm.Sleep(100)
            End While

            _Daoc.StopRunning()
        End If

    End Sub

    Private Function FindMob(ByVal MobName As String, _
                             ByVal MinLevel As Integer, ByVal MaxLevel As Integer, _
                             ByVal Range As Integer) As Short

        Dim intTempId As Integer

        Dim blnMobLoop As Boolean = False

        intTempId = _Daoc.FindClosestMob(MinLevel, MaxLevel, Range)

        While Not blnMobLoop
            If intTempId = -1 Then
                FindMob = -1
                blnMobLoop = True
            Else
                If Trim(_Daoc.MobName(intTempId)) = Trim(MobName) Then
                    FindMob = intTempId
                    blnMobLoop = True
                Else
                    intTempId = _Daoc.FindNextClosestMob(MinLevel, MaxLevel, Range)
                End If
            End If
        End While

    End Function

    Private Function FindObject(ByVal ObjectName As String, _
                                ByVal MinLevel As Integer, ByVal MaxLevel As Integer, _
                                ByVal Range As Integer) As Short

        Dim intTempId As Integer

        Dim blnMobLoop As Boolean = False

        intTempId = _Daoc.FindClosestObject(MinLevel, MaxLevel, Range)

        While Not blnMobLoop
            If intTempId = -1 Then
                FindObject = -1
                blnMobLoop = True
            Else
                If Trim(_Daoc.MobName(intTempId)) = Trim(ObjectName) Then
                    FindObject = intTempId
                    blnMobLoop = True
                Else
                    intTempId = _Daoc.FindNextClosestMob(MinLevel, MaxLevel, Range)
                End If
            End If
        End While

    End Function

    Public Sub Sleep(ByVal Duration As Long)
        System.Threading.Thread.CurrentThread.Sleep(Duration)
    End Sub

    Private Function IsDAOCActive() As Boolean

        If GetForegroundWindow = FindWindow("DAocMWC", Nothing) Then
            Dim mi As New UpdateTitle(AddressOf UpdateFormTitle)
            '_MainForm.BeginInvoke(mi, New Object() {"AKRemoteAni (Active)"})
            _MainForm.BeginInvoke(mi, New Object() {_MainFormTitle & " (Active)"})
            'Text = "AKNecro (Active)"

            Return True
        Else
            Dim mi As New UpdateTitle(AddressOf UpdateFormTitle)
            '_MainForm.BeginInvoke(mi, New Object() {"AKRemoteAni (Paused)"})
            _MainForm.BeginInvoke(mi, New Object() {_MainFormTitle & " (Paused)"})
            'Text = "AKNecro (Active)"

            Return False

        End If
    End Function

    Private Sub UpdateTextBox(ByRef aTextBox As Object, ByRef Text As Object)

        If TypeName(aTextBox) = "TextBox" Then
            CType(aTextBox, TextBox).Text = CStr(Text)
        ElseIf TypeName(aTextBox) = "Button" Then
            CType(aTextBox, Button).Text = CStr(Text)
        End If

    End Sub

    Private Delegate Sub UpdateTitle(ByRef Text As Object)

    Private Sub UpdateFormTitle(ByRef Text As Object)

        _MainForm.Text = CStr(Text)

    End Sub

    Private Sub LogLineAsync(ByVal line As String)

        Dim mi As New LogIt(AddressOf LogLine)
        'Me.BeginInvoke(mi, New Object() {line})
        _MainForm.BeginInvoke(mi, New Object() {line})

    End Sub

    Private Delegate Sub LogIt(ByVal Line As String)

    Private Sub LogLine(ByVal line As String)

        line = Format(Year(Now), "0000") & "-" & Format(Month(Now), "00") & "-" & Format(Microsoft.VisualBasic.Day(Now), "00") & "|" & Format(Hour(Now), "00") & ":" & Format(Minute(Now), "00") & ":" & Format(Second(Now), "00") & "| " & line
        _LogListBox.BeginUpdate()
        _LogListBox.Items.Insert(0, line)

        If _LogListBox.Items.Count > 128 Then
            _LogListBox.Items.RemoveAt(127)
        End If
        _LogListBox.EndUpdate()

    End Sub

End Class
