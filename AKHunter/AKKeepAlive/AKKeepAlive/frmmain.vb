Option Explicit On 

Imports System.Xml
Imports System.IO
Imports System.Security.Permissions

Public Class frmMain
    Inherits System.Windows.Forms.Form
#Region " TODO "
    'TODO: 
    'TODO: fix send keepalive key
    'TODO: make re-starting work
#End Region
#Region " Variables "
    Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
    Private Declare Function GetForegroundWindow Lib "user32" Alias "GetForegroundWindow" () As Integer

    Private WithEvents objWindowManager As DAOCWindowManager
    Private objDAOC As AutoKillerScript.clsAutoKillerScript
    Shared objAKKeepAlive As Threading.Thread

    Shared blnTerminateThread As Boolean = False
    Shared blnRelease As Boolean = False
    Shared blnKeepAlive As Boolean = False
    Shared blnStopAfterDeath As Boolean = True

    Shared strProfile As String = "AKKeepAlive.xml"

    Shared bytIsTargetDead As Byte = 0

    'profile var's
    Shared bytKeepAliveKey As Byte
    Shared strStopAfterPlayerDeath As String
    Shared strReleaseText As String

    Shared shtStopAfterPlayerDeathIndex As Short
#End Region
#Region " Classes "
#End Region
#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents btnStartStop As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents lbLog As System.Windows.Forms.ListBox
    Friend WithEvents tmeRelease As System.Timers.Timer
    Friend WithEvents tmeKeepAlive As System.Timers.Timer
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnStartStop = New System.Windows.Forms.Button
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.lbLog = New System.Windows.Forms.ListBox
        Me.tmeKeepAlive = New System.Timers.Timer
        Me.tmeRelease = New System.Timers.Timer
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.tmeKeepAlive, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tmeRelease, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnStartStop
        '
        Me.btnStartStop.Location = New System.Drawing.Point(352, 288)
        Me.btnStartStop.Name = "btnStartStop"
        Me.btnStartStop.Size = New System.Drawing.Size(72, 24)
        Me.btnStartStop.TabIndex = 0
        Me.btnStartStop.Text = "Start"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.ItemSize = New System.Drawing.Size(42, 18)
        Me.TabControl1.Location = New System.Drawing.Point(8, 8)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(416, 270)
        Me.TabControl1.TabIndex = 1
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.lbLog)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(408, 244)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Log"
        '
        'lbLog
        '
        Me.lbLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbLog.HorizontalScrollbar = True
        Me.lbLog.Location = New System.Drawing.Point(0, 0)
        Me.lbLog.Name = "lbLog"
        Me.lbLog.Size = New System.Drawing.Size(408, 238)
        Me.lbLog.TabIndex = 0
        '
        'tmeKeepAlive
        '
        Me.tmeKeepAlive.Interval = 540000
        Me.tmeKeepAlive.SynchronizingObject = Me
        '
        'tmeRelease
        '
        Me.tmeRelease.Interval = 660000
        Me.tmeRelease.SynchronizingObject = Me
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(432, 320)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.btnStartStop)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmMain"
        Me.Text = "AKKeepAlive"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.tmeKeepAlive, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tmeRelease, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnStartStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartStop.Click
        If IsNothing(objAKKeepAlive) Then
            blnTerminateThread = False

            objAKKeepAlive = New Threading.Thread(AddressOf AKKeepAlive)
            objAKKeepAlive.Start()

            btnStartStop.Text = "Stop"
        Else
            Text = "AKKeepAlive"

            btnStartStop.Text = "Start"

            blnTerminateThread = True

            objAKKeepAlive.Join()
            objAKKeepAlive = Nothing
        End If
    End Sub

    Private Sub AKKeepAlive()
        Dim objKeys As New DAOCKeyboard(objDAOC, ReadSetting("GamePath", GetType(String)))
        Dim objEncoder As New System.Text.UTF8Encoding

        Dim blnProcessProfile As Boolean = False
        Dim blnRecastPowerDrain As Boolean = True

        Dim intLastPowerTransfer As Integer = 0

        Try
            objDAOC = New AutoKillerScript.clsAutoKillerScript

            AddHandler objDAOC.OnLog, AddressOf LogLine

            LogLineAsync("Initializing DAOCScript v" & objDAOC.getVersion & "....")

            objWindowManager = New DAOCWindowManager
            objWindowManager.CharacterName = ReadSetting("CharacterName", GetType(String))

            ' This section sets up various variables for the DLL.
            objDAOC.ChatLog = ReadSetting("ChatLog", GetType(String))
            objWindowManager.DAOCPath = ReadSetting("GamePath", GetType(String))
            objWindowManager.ServerIP = ReadSetting("ServerIP", GetType(String))
            objDAOC.RegKey = ReadSetting("RegKey", GetType(String))
            objDAOC.EnableEuro = ReadSetting("EnableEuro", GetType(Boolean))
            objDAOC.EnableClassic = ReadSetting("EnableClassic", GetType(Boolean))
            objDAOC.EnableToA = ReadSetting("EnableTOA", GetType(Boolean))

            'keys
            objDAOC.SetLeftTurnKey = objKeys.TurnLeft_Key
            objDAOC.SetRightTurnKey = objKeys.TurnRight_Key
            objDAOC.SetConsiderKey = objKeys.Consider_Key

            objDAOC.DoInit()

            LoadProfile()

            While Not blnTerminateThread
                If IsDAOCActive() And objDAOC.IsPlayerDead = 0 And bytIsTargetDead = 0 Then

                    '*********************************************************************************
                    'process profile
                    '*********************************************************************************
                    If Not blnProcessProfile Then
                        blnProcessProfile = True

                        'get StopAfterPlayerDeath index
                        shtStopAfterPlayerDeathIndex = objDAOC.SetTarget(strStopAfterPlayerDeath, , False)
                        LogLine("Got index of " & shtStopAfterPlayerDeathIndex & " for " & strStopAfterPlayerDeath)

                        If shtStopAfterPlayerDeathIndex = -1 Then
                            LogLine("Can't find player, shutting thread down")
                            blnTerminateThread = True
                        Else
                            LogLine("Starting timer")
                            tmeKeepAlive.Start()
                        End If
                    End If

                    'sleep for 1 sec
                    objAKKeepAlive.Sleep(1000)

                    If objDAOC.MobHealth(shtStopAfterPlayerDeathIndex) = 0 Then
                        bytIsTargetDead = 1
                    End If

                    If blnKeepAlive Then
                        LogLine("Stopping timer")
                        tmeKeepAlive.Stop()

                        blnKeepAlive = False

                        LogLine("Time up, sending keep alive key")
                        objAKKeepAlive.Sleep(2000)
                        'objDAOC.SendKeys(bytKeepAliveKey, 0)
                        objDAOC.SendKeys(objKeys.ShowInventory_Key)
                        objAKKeepAlive.Sleep(2000)

                        LogLine("Starting timer")
                        tmeKeepAlive.Start()
                    End If
                Else
                    If objDAOC.IsPlayerDead = 1 Then
                        'player has died, if blnStopAfterDeath is true, player will release and quit
                        Dim blnTempLoop As Boolean = True

                        If blnStopAfterDeath Then
                            LogLine("Player is dead, starting release timer")
                            tmeRelease.Start()

                            While blnTempLoop
                                If blnRelease Then
                                    'stop timer
                                    tmeRelease.Stop()

                                    blnTempLoop = False

                                    LogLine("Time up, sending release line")
                                    objAKKeepAlive.Sleep(2000)
                                    objDAOC.SendString(strReleaseText & "~")
                                    objAKKeepAlive.Sleep(2000)

                                    LogLine("Sleeping for 1 minute, then sending quit")
                                    objAKKeepAlive.Sleep(60000)

                                    objAKKeepAlive.Sleep(2000)
                                    objDAOC.SendString("/quit~")
                                    objAKKeepAlive.Sleep(2000)

                                    LogLine("Terminating thread")
                                    blnTerminateThread = True
                                End If
                            End While
                        Else
                            'player is dead but stopafterdeath is false
                            blnTerminateThread = True
                        End If
                    Else
                        'if one of the targets, either powerdrain or powertransfer targets dies, then quit
                        If bytIsTargetDead = 1 Then
                            LogLine("A target died, sleep for 2 miniutes to see if player will die")
                            objAKKeepAlive.Sleep(120000)

                            'still alive, quit
                            If objDAOC.IsPlayerDead = 0 Then
                                LogLine("Still alive, quitting")
                                objAKKeepAlive.Sleep(2000)
                                objDAOC.SendString("/quit~")
                                objAKKeepAlive.Sleep(2000)

                                LogLine("Terminating thread")
                                blnTerminateThread = True
                            End If
                        End If
                    End If
                End If 'If IsDAOCActive() AndAlso Not objDAOC.IsPlayerDead Then
            End While
        Catch Ex As Exception
            LogLineAsync("Exception occured! AKKeepAlive stopped!")
            LogLineAsync(Ex.Message)
            LogLineAsync(Ex.Source)
            LogLineAsync(Ex.StackTrace)
        Finally
            objDAOC.StopInit()

            'Update
            Dim mi4 As UpdateText
            mi4 = New UpdateText(AddressOf UpdateTextBox)
            Me.BeginInvoke(mi4, New Object() {btnStartStop, "Start"})
            'btnLoad.Text = "Start"
            objAKKeepAlive = Nothing

            objDAOC = Nothing
        End Try
    End Sub

    Public Sub LoadProfile()
        Dim objEncoder As New System.Text.UTF8Encoding

        If IO.File.Exists(strProfile) Then
            Try
                Dim xmlDoc As Xml.XmlDocument = New Xml.XmlDocument
                xmlDoc.Load(strProfile)

                LogLine("--------------------------------------------------------------------------------")
                LogLine("Running profile: " & strProfile)

                'Settings
                'bytKeepAliveKey = objEncoder.GetBytes(xmlDoc.SelectSingleNode("//Profile/Settings/@KeepAliveKey").Value)(0)
                strStopAfterPlayerDeath = xmlDoc.SelectSingleNode("//Profile/Settings/@StopAfterPlayerDeath").Value
                strReleaseText = xmlDoc.SelectSingleNode("//Profile/Settings/@ReleaseText").Value

                'LogLine("KeepAliveKey: " & xmlDoc.SelectSingleNode("//Profile/Settings/@KeepAliveKey").Value)
                LogLine("StopAfterPlayerDeath: " & strStopAfterPlayerDeath)
                LogLine("ReleaseText: " & strReleaseText)

                LogLine("--------------------------------------------------------------------------------")
            Catch Ex As Exception
                LogLineAsync(Ex.Message)
            End Try
        End If
    End Sub

    Public Delegate Sub UpdateText(ByRef aTextBox As Object, ByRef Text As Object)

    Public Delegate Sub LogIt(ByRef Line As String)

    Public Sub UpdateTextBox(ByRef aTextBox As Object, ByRef Text As Object)
        If TypeName(aTextBox) = "TextBox" Then
            CType(aTextBox, TextBox).Text = CStr(Text)
        ElseIf TypeName(aTextBox) = "Button" Then
            CType(aTextBox, Button).Text = CStr(Text)
        End If
    End Sub

    Sub LogLineAsync(ByRef line As String)
        Dim mi As New LogIt(AddressOf LogLine)
        Me.BeginInvoke(mi, New Object() {line})
    End Sub

    Sub LogLine(ByRef line As String)
        line = Format(Year(Now), "0000") & "-" & Format(Month(Now), "00") & "-" & Format(Microsoft.VisualBasic.Day(Now), "00") & "|" & Format(Hour(Now), "00") & ":" & Format(Minute(Now), "00") & ":" & Format(Second(Now), "00") & "| " & line
        lbLog.BeginUpdate()
        lbLog.Items.Insert(0, line)

        If lbLog.Items.Count > 128 Then
            lbLog.Items.RemoveAt(127)
        End If
        lbLog.EndUpdate()
    End Sub

    Public Function ReadSetting(ByVal key As String, ByVal type As Type)
        Dim configurationAppSettings As System.Configuration.AppSettingsReader

        configurationAppSettings = New System.Configuration.AppSettingsReader

        Return configurationAppSettings.GetValue(key, type)
    End Function

    Public Delegate Sub UpdateTitle(ByRef Text As Object)

    Public Sub UpdateFormTitle(ByRef Text As Object)
        Me.Text = CStr(Text)
    End Sub

    Public Function IsDAOCActive() As Boolean
        If GetForegroundWindow = FindWindow("DAocMWC", Nothing) Then
            Dim mi As New UpdateTitle(AddressOf UpdateFormTitle)
            Me.BeginInvoke(mi, New Object() {"AKKeepAlive (Active)"})
            'Text = "AKKeepAlive (Active)"

            Return True
        Else
            Dim mi As New UpdateTitle(AddressOf UpdateFormTitle)
            Me.BeginInvoke(mi, New Object() {"AKKeepAlive (Paused)"})
            'Text = "AKKeepAlive (Active)"

            Return False
        End If
    End Function

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "AKKeepAlive"

        LogLine("AKKeepAlive v0.01 (c) Version125")
        LogLine("DAOC Packets is not required for this application")

        'comment out for now
        'Try
        '    System.Diagnostics.Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.AboveNormal
        'Catch Ex As Exception
        '    ' On Windows 98 machines you get an error for 'above normal'
        '    System.Diagnostics.Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.High
        'End Try

        Threading.Thread.CurrentThread.Name = "MainThread"
    End Sub

    Private Sub frmMain_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Not IsNothing(objAKKeepAlive) Then
            btnStartStop_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub tmeRelease_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles tmeRelease.Elapsed
        blnRelease = True
    End Sub

    Private Sub tmeKeepAlive_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles tmeKeepAlive.Elapsed
        blnKeepAlive = True
    End Sub
End Class

