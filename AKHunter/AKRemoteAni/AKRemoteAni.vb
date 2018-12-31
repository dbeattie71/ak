'Option Strict On
Option Explicit On 

Imports System.Threading
Imports System.Xml

Public Class AKRemoteAni
#Region " Variables "
    Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
    Private Declare Function GetForegroundWindow Lib "user32" Alias "GetForegroundWindow" () As Integer

    Private Const _ClassName As String = "AKRemoteAni"

    Private _AKRemoteAniThread As Thread
    Private _TerminateThread As Boolean = False

    Private _GamePath As String
    Private _RegKey As String
    Private _EnableToA As Boolean
    Private _EnableCatacombs As Boolean

    Private _Daoc As AutoKillerScript.clsAutoKillerScript

    Private _AKHelper As AKHelper.clsAkHelper
    Private _SpellTimers As Hashtable

    Private _Profile As String = "Animist.xml"

    Private _MainForm As Form
    Private _LogListBox As ListBox
    Private _MainFormTitle As String

    Private _KeepAliveTimer As System.Timers.Timer
    Private _KeepAlive As Boolean

    Private _stickKey As Byte
    Private _faceKey As Byte
    Private _moveBackwardKey As Byte
    Private _moveForwardKey As Byte
    Private _showInventoryKey As Byte
    
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
            _AKRemoteAniThread = New Thread(objStart)
            _AKRemoteAniThread.Name = "AKRemoteAni"
            _AKRemoteAniThread.Start()

        Catch ex As Exception
            LogLineAsync(Thread.CurrentThread.Name & ": " & ex.Message)
        End Try

    End Sub

    Public Sub Terminate()

        Dim methodName As String = "Terminate()"

        Try
            _AKHelper.StopInit()

            _Daoc.StopInit()
            _TerminateThread = True
            _AKRemoteAniThread.Join()
            LogLineAsync(_AKRemoteAniThread.Name & " - stopped.")

        Catch ex As Exception
            LogLineAsync(Thread.CurrentThread.Name & ": " & ex.Message)
        End Try

    End Sub

    Private Sub Start()
        Dim methodName As String = "Start()"

        Dim keys As AutoKillerScript.UserKeys
        Dim keys2 As DAOCKeyboard
        Dim processProfile As Boolean = False

        Dim spellTimer As AKHelper.SpellTimer

        Try
            _Daoc = New AutoKillerScript.clsAutoKillerScript
            _AKHelper = New AKHelper.clsAkHelper

            AddHandler _Daoc.OnLog, AddressOf LogLineAsync
            AddHandler _Daoc.OnRegExTrue, AddressOf ProcessQuery

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

            AddHandler _AKHelper.OnLog, AddressOf LogLineAsync
            _AKHelper.DoInit()

            _stickKey = keys.StickKey
            _faceKey = keys.FaceKey
            _moveForwardKey = keys.MoveForwardKey
            _moveBackwardKey = keys.MoveBackwardKey
            _showInventoryKey = keys2.ShowInventory_Key

            _MainFormTitle = "AKRemoteAni - " & _Daoc.PlayerName

            LoadProfile()

            While Not _TerminateThread
                If IsDAOCActive() Then 'And objDAOC.IsPlayerDead = 0 Then

                    '*********************************************************************************
                    'process profile
                    '*********************************************************************************
                    If Not processProfile Then
                        processProfile = True

                        'set /effects to none
                        LogLineAsync("Setting /effects to none")
                        _AKHelper.SetEffectsToNone(_Daoc)

                        For Each spellTimer In _SpellTimers.Values
                            spellTimer.Process = True
                            spellTimer.Start()
                        Next

                        'start keepalive timer
                        KeepAlive()

                    End If
                    '************************************************

                    If Not _AKHelper.SpamSpellList = "" Then
                        _AKHelper.Cast(_Daoc, "", "", _AKHelper.SpamSpellList, False)
                    End If

                    For Each spellTimer In _SpellTimers.Values
                        If spellTimer.Process = True Then
                            spellTimer.Process = False

                            _AKHelper.Cast(_Daoc, "", "", spellTimer.SpellList, False)

                            spellTimer.Start()
                        End If

                    Next

                    If _KeepAlive Then

                        _KeepAlive = False
                        _Daoc.SendKeys(_showInventoryKey)

                    End If

                    'LogLineAsync("foo")
                    _AKRemoteAniThread.CurrentThread.Sleep(500)

                End If 'If IsDAOCActive() AndAlso Not objDAOC.IsPlayerDead Then

                _AKRemoteAniThread.CurrentThread.Sleep(100)
            End While

        Catch ex As Exception
            LogLineAsync(Thread.CurrentThread.Name & ": " & ex.Message)
            Me.Terminate()
        End Try

    End Sub

    Private Sub ProcessQuery(ByVal e As AutoKillerScript.clsAutoKillerScript.AutokillerRegExEventParams)

        Dim methodName As String = "ProcessQuery()"

        Dim playerName As String = ""
        Dim message As String = ""

        Dim casterName As String = ""
        Dim targetPlayerNameList As String = ""
        Dim spellList As String = ""

        LogLineAsync(_AKHelper.GetLogNameByIndex(e.QueryID))

        Select Case _AKHelper.GetLogNameByIndex(e.QueryID)
            Case "PassThru"
                playerName = e.RegExMatch.Groups("PlayerName").Value
                message = e.RegExMatch.Groups("Message").Value

                If _AKHelper.ValidateUser(playerName) Then
                    LogLineAsync("Valid player name: " & playerName)
                    LogLineAsync("Message: " & message)
                    _AKHelper.PassThru(_Daoc, message, _AKHelper.GetSetting("PassThruFlag"))
                Else
                    LogLineAsync("INVALID PLAYER NAME: " & playerName)
                End If

            Case "AutoFollow"
                playerName = e.RegExMatch.Groups("PlayerName").Value
                message = e.RegExMatch.Groups("Message").Value

                If _AKHelper.ValidateUser(playerName) Then
                    LogLineAsync("Valid player name: " & playerName)
                    LogLineAsync("Message: " & message)
                    _AKHelper.AutoFollow(_Daoc, playerName, _stickKey)
                Else
                    LogLineAsync("INVALID PLAYER NAME: " & playerName)
                End If

            Case "BreakAutoFollow"
                playerName = e.RegExMatch.Groups("PlayerName").Value
                message = e.RegExMatch.Groups("Message").Value

                If _AKHelper.ValidateUser(playerName) Then
                    LogLineAsync("Valid player name: " & playerName)
                    LogLineAsync("Message: " & message)
                    _AKHelper.BreakAutoFollow(_Daoc, _moveBackwardKey)
                Else
                    LogLineAsync("INVALID PLAYER NAME: " & playerName)
                End If

            Case "Disband"
                playerName = e.RegExMatch.Groups("PlayerName").Value
                message = e.RegExMatch.Groups("Message").Value

                If _AKHelper.ValidateUser(playerName) Then
                    LogLineAsync("Valid player name: " & playerName)
                    LogLineAsync("Message: " & message)
                    _AKHelper.Disband(_Daoc)
                Else
                    LogLineAsync("INVALID PLAYER NAME: " & playerName)
                End If

            Case "AcceptDlg"
                playerName = e.RegExMatch.Groups("PlayerName").Value

                If _AKHelper.ValidateUser(playerName) Then
                    LogLineAsync("Valid player name: " & playerName)
                    _AKHelper.AcceptDialog(_Daoc)
                Else
                    LogLineAsync("INVALID PLAYER NAME: " & playerName)
                End If

            Case "Cast"
                playerName = e.RegExMatch.Groups("PlayerName").Value
                casterName = e.RegExMatch.Groups("CasterName").Value
                spellList = e.RegExMatch.Groups("SpellList").Value
                targetPlayerNameList = e.RegExMatch.Groups("TargetPlayerNameList").Value

                If _AKHelper.ValidateUser(playerName) Then
                    LogLineAsync("Valid player name:" & playerName)
                    LogLineAsync("CasterName:" & casterName)
                    LogLineAsync("TargetPlayerNameList:" & targetPlayerNameList)
                    LogLineAsync("SpellList:" & spellList)

                    If casterName = "" Then
                        _AKHelper.Cast(_Daoc, playerName, targetPlayerNameList, spellList, True)
                    Else
                        If casterName = _Daoc.PlayerName.ToLower Then
                            _AKHelper.Cast(_Daoc, playerName, targetPlayerNameList, spellList, True)
                        End If
                    End If

                Else
                    LogLineAsync("INVALID PLAYER NAME: " & playerName)
                End If

            Case "Spam"
                playerName = e.RegExMatch.Groups("PlayerName").Value
                casterName = e.RegExMatch.Groups("CasterName").Value
                spellList = e.RegExMatch.Groups("SpellList").Value
                targetPlayerNameList = e.RegExMatch.Groups("TargetPlayerNameList").Value

                If _AKHelper.ValidateUser(playerName) Then
                    LogLineAsync("Valid player name:" & playerName)
                    LogLineAsync("CasterName:" & casterName)
                    LogLineAsync("TargetPlayerNameList:" & targetPlayerNameList)
                    LogLineAsync("SpellList:" & spellList)

                    If casterName = "" Then
                        _AKHelper.SetGroundTarget(_Daoc, playerName)
                        _AKHelper.SpamSpellList = spellList
                    Else
                        If casterName = _Daoc.PlayerName.ToLower Then
                            _AKHelper.SetGroundTarget(_Daoc, playerName)
                            _AKHelper.SpamSpellList = spellList
                        End If
                    End If
                Else
                    LogLineAsync("INVALID PLAYER NAME: " & playerName)
                End If

            Case "StopSpam"
                playerName = e.RegExMatch.Groups("PlayerName").Value
                message = e.RegExMatch.Groups("Message").Value

                If _AKHelper.ValidateUser(playerName) Then
                    LogLineAsync("Valid player name: " & playerName)
                    LogLineAsync("Message: " & message)

                    _AKHelper.SpamSpellList = ""
                Else
                    LogLineAsync("INVALID PLAYER NAME: " & playerName)
                End If

            Case "StopSpamSit"
                playerName = e.RegExMatch.Groups("PlayerName").Value
                message = e.RegExMatch.Groups("Message").Value

                If _AKHelper.ValidateUser(playerName) Then
                    LogLineAsync("Valid player name: " & playerName)
                    LogLineAsync("Message: " & message)

                    _AKHelper.SpamSpellList = ""
                    _AKHelper.Sit(_Daoc)

                Else
                    LogLineAsync("INVALID PLAYER NAME: " & playerName)
                End If

            Case "Test"
                playerName = e.RegExMatch.Groups("PlayerName").Value

                If _AKHelper.ValidateUser(playerName) Then
                    LogLineAsync("Valid player name: " & playerName)

                Else
                    LogLineAsync("INVALID PLAYER NAME: " & playerName)
                End If

        End Select

    End Sub

    Private Sub KeepAlive()

        _KeepAliveTimer = New System.Timers.Timer

        _KeepAlive = False

        _KeepAliveTimer.Stop()
        _KeepAliveTimer.Interval = 1020000

        AddHandler _KeepAliveTimer.Elapsed, AddressOf KeepAliveTimer
        _KeepAliveTimer.Start()

    End Sub

    Private Sub KeepAliveTimer(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)

        _KeepAlive = True

    End Sub

    Private Sub LoadProfile()

        Dim methodName As String = "LoadProfile()"

        Dim encoder As New System.Text.UTF8Encoding

        Dim xmlDoc As Xml.XmlDocument = New Xml.XmlDocument
        Dim xmlNode As Xml.XmlNode
        Dim xmlNode2 As Xml.XmlNode

        Dim count As Integer

        If IO.File.Exists(_Profile) Then
            Try

                'xmlDoc.Load(_Profile)
                'LogLineAsync("Running profile: " & _Profile)

                LogLineAsync("--------------------------------------------------------------------------------")

                _AKHelper.LoadChatLogLines("AKHelper.xml")
                count = _AKHelper.AddChatLogLinesStrings(_Daoc, 0)

                _AKHelper.LoadLogLines("AKHelper.xml")
                count = _AKHelper.AddLogLinesStrings(_Daoc, count)

                _AKHelper.LoadUsers("AKHelper.xml")
                _AKHelper.LoadSettings("AKHelper.xml")
                _AKHelper.LoadSpells("AKHelper.xml")

                _AKHelper.LoadSpellTimers("AKHelper.xml")
                _SpellTimers = _AKHelper.GetSpellTimers

                LogLineAsync("--------------------------------------------------------------------------------")

            Catch Ex As Exception
                LogLineAsync(Ex.Message)
            End Try
        End If

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

