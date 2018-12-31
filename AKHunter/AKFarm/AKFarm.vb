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
    Private _EnableCatacombs As Boolean

    Private _Ak As AutoKillerScript.clsAutoKillerScript

    Private _MainForm As Form
    Private _LogListBox As ListBox
    Private _MainFormTitle As String

    Private _PatrolPoints As PatrolPoints

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
            _Ak.StopInit()
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
            _Ak = New AutoKillerScript.clsAutoKillerScript

            AddHandler _Ak.OnLog, AddressOf LogLineAsync
            AddHandler _Ak.OnRegExTrue, AddressOf ProcessLog

            LogLineAsync("Initializing DAOCScript v" & _Ak.getVersion & "....")

            LogLineAsync("RegKey:" & _RegKey)
            LogLineAsync("GamePath:" & _GamePath)
            LogLineAsync("EnableCatacombs:" & _EnableCatacombs)


            _Ak.RegKey = _RegKey
            _Ak.GamePath = _GamePath
            _Ak.EnableCatacombs = _EnableCatacombs
            _Ak.EnableAutoQuery = True
            _Ak.UseRegEx = True

            keys = New AutoKillerScript.UserKeys(_Ak)
            keys2 = New DAOCKeyboard(_Ak)

            _Ak.SetLeftTurnKey = keys.TurnLeftKey
            _Ak.SetRightTurnKey = keys.TurnRightKey
            _Ak.SetConsiderKey = keys.ConsiderKey

            _Ak.DoInit()

            'keys
            _stickKey = keys.StickKey
            _faceKey = keys.FaceKey
            _moveForwardKey = keys.MoveForwardKey
            _moveBackwardKey = keys.MoveBackwardKey
            _showInventoryKey = keys2.ShowInventory_Key

            'log strings
            _Ak.AddString(0, "Your target is not visible!")
            _Ak.AddString(1, "You can't see your target from here!")
            _Ak.AddString(2, "Your target is not visible. The spell fails.")

            _MainFormTitle = "AKFarm - " & _Ak.PlayerName

            While Not _TerminateThread
                If IsDAOCActive() Then 'And objDAOC.IsPlayerDead = 0 Then

                    '*********************************************************************************
                    'process profile
                    '*********************************************************************************
                    If Not processProfile Then
                        processProfile = True

                        _PatrolPoints = New PatrolPoints
                        _PatrolPoints.AddPatrolPoint(1, False, 28275, 19693, 15986)

                        Dim mywp As New WayPoints
                        mywp.AddWayPoint(1, 27865, 19454, 15995)
                        mywp.AddWayPoint(2, 27434, 19351, 16011)

                        _PatrolPoints.AddPatrolPoint(2, True, 27434, 19822, 16008, mywp)

                    End If

                End If 'If IsDAOCActive() AndAlso Not objDAOC.IsPlayerDead Then

                _AKFarm.CurrentThread.Sleep(100)
            End While

        Catch ex As Exception
            LogLineAsync(Thread.CurrentThread.Name & ": " & ex.Message)
            Me.Terminate()
        End Try

    End Sub

    Private Sub MoveToGXY(ByVal X As Double, ByVal Y As Double, ByVal Z As Double, ByVal Range As Double)

        'needs stuck logic

        If Not (_Ak.ZDistance(_Ak.gPlayerXCoord, _Ak.gPlayerYCoord, _Ak.gPlayerZCoord, X, Y, Z)) < Range Then
            _Ak.StartRunning()

            While _Ak.ZDistance(_Ak.gPlayerXCoord, _Ak.gPlayerYCoord, _Ak.gPlayerZCoord, X, Y, Z) > Range _
                And Not _Ak.IsPlayerDead()

                _Ak.TurnToHeading(_Ak.FindHeading(_Ak.gPlayerXCoord, _Ak.gPlayerYCoord, X, Y))
                _AKFarm.Sleep(100)
            End While

            _Ak.StopRunning()
        End If

    End Sub

    Private Sub MoveToZXY(ByVal X As Double, ByVal Y As Double, ByVal Z As Double, ByVal Range As Double)

        'needs stuck logic

        If Not (_Ak.ZDistance(_Ak.zPlayerXCoord, _Ak.zPlayerYCoord, _Ak.zPlayerZCoord, X, Y, Z)) < Range Then
            _Ak.StartRunning()

            While _Ak.ZDistance(_Ak.zPlayerXCoord, _Ak.zPlayerYCoord, _Ak.zPlayerZCoord, X, Y, Z) > Range _
                And Not _Ak.IsPlayerDead() And Not _TerminateThread

                _Ak.TurnToHeading(_Ak.FindHeading(_Ak.zPlayerXCoord, _Ak.zPlayerYCoord, X, Y))
                _AKFarm.Sleep(100)
            End While

            _Ak.StopRunning()
        End If

    End Sub

    Private Function FindMob(ByVal MobName As String, _
                             ByVal MinLevel As Integer, ByVal MaxLevel As Integer, _
                             ByVal Range As Integer) As Short

        Dim intTempId As Integer

        Dim blnMobLoop As Boolean = False

        intTempId = _Ak.FindClosestMob(MinLevel, MaxLevel, Range)

        While Not blnMobLoop
            If intTempId = -1 Then
                FindMob = -1
                blnMobLoop = True
            Else
                If Trim(_Ak.MobName(intTempId)) = Trim(MobName) Then
                    FindMob = intTempId
                    blnMobLoop = True
                Else
                    intTempId = _Ak.FindNextClosestMob(MinLevel, MaxLevel, Range)
                End If
            End If
        End While

    End Function

    Private Function FindObject(ByVal ObjectName As String, _
                                ByVal MinLevel As Integer, ByVal MaxLevel As Integer, _
                                ByVal Range As Integer) As Short

        Dim intTempId As Integer

        Dim blnMobLoop As Boolean = False

        intTempId = _Ak.FindClosestObject(MinLevel, MaxLevel, Range)

        While Not blnMobLoop
            If intTempId = -1 Then
                FindObject = -1
                blnMobLoop = True
            Else
                If Trim(_Ak.MobName(intTempId)) = Trim(ObjectName) Then
                    FindObject = intTempId
                    blnMobLoop = True
                Else
                    intTempId = _Ak.FindNextClosestMob(MinLevel, MaxLevel, Range)
                End If
            End If
        End While

    End Function

    Private Sub ProcessLog(ByVal e As AutoKillerScript.clsAutoKillerScript.AutokillerRegExEventParams)

    End Sub

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
