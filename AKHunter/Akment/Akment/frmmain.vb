Option Explicit On 

Imports System.Xml
Imports System.IO
Imports System.Security.Permissions

Public Class frmMain
    Inherits System.Windows.Forms.Form
#Region " TODO "
    'TODO: 
    'TODO: 
    'TODO: make re-starting work
#End Region
#Region " Variables "
    Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
    Private Declare Function GetForegroundWindow Lib "user32" Alias "GetForegroundWindow" () As Integer

    'Declare Auto Function GetForegroundWindow Lib "user32" () As IntPtr
    'Declare Auto Function GetWindowThreadProcessId Lib "user32" (ByVal hWnd As IntPtr, ByRef lpdwProcessId As Int32) As Int32

    Private WithEvents objWindowManager As DAOCWindowManager
    Private objDAOC As AutoKillerScript.clsAutoKillerScript
    Shared objAKMent As Threading.Thread

    Shared blnTerminateThread As Boolean = False
    Shared blnPowerRegen As Boolean = False
    Shared blnRelease As Boolean = False

    Shared strProfile As String

    Shared bytSitKey As Byte
    Shared bytFaceKey As Byte
    Shared bytIsTargetDead As Byte = 0

    'profile var's
    Shared blnStopAfterDeath As Boolean
    Shared strStopAfterPlayerDeath As String
    Shared shtStopAfterPlayerDeathIndex As Short
    Shared strReleaseText As String
    Shared blnSitToRegainPower As Boolean
    Shared bytMCLKey As Byte

    Shared blnPowerRegenEnabled As Boolean
    Shared bytPowerRegenKey As Byte
    Shared intPowerRegenDelay As Integer
    Shared intPowerRegenTimer As Integer

    Shared colPowerRegenTargets As Collection = New Collection

    Shared blnHOTEnabled As Boolean
    Shared bytHOTKey As Byte
    Shared intHOTDelay As Integer
    'Shared intHOTThreshold As Integer

    Shared strHOTTarget As String
    Shared shtHOTTargetIndex As Short

#End Region
#Region " Classes "
    Class clsPowerRegenTarget
        Public Name As String
        Public Index As Short
    End Class
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
    Friend WithEvents tmePowerRegen As System.Timers.Timer
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnStartStop = New System.Windows.Forms.Button
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.lbLog = New System.Windows.Forms.ListBox
        Me.tmePowerRegen = New System.Timers.Timer
        Me.tmeRelease = New System.Timers.Timer
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.tmePowerRegen, System.ComponentModel.ISupportInitialize).BeginInit()
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
        'tmePowerRegen
        '
        Me.tmePowerRegen.SynchronizingObject = Me
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
        Me.Text = "AKMent"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.tmePowerRegen, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tmeRelease, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnStartStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartStop.Click
        If IsNothing(objAKMent) Then
            blnTerminateThread = False
            blnPowerRegen = True

            objAKMent = New Threading.Thread(AddressOf AKMent)
            objAKMent.Start()

            btnStartStop.Text = "Stop"
        Else
            Text = "AKMent"

            btnStartStop.Text = "Start"

            blnTerminateThread = True

            objAKMent.Join()
            objAKMent = Nothing
        End If
    End Sub

    Private Sub AKMent()
        Dim objKeys As New DAOCKeyboard(objDAOC, ReadSetting("GamePath", GetType(String)))
        Dim objEncoder As New System.Text.UTF8Encoding

        Dim blnProcessProfile As Boolean = False
        Dim blnRecastPowerDrain As Boolean = True

        Dim intLastPowerTransfer As Integer = 0

        Dim intRegenCount = 0

        Try
            objDAOC = New AutoKillerScript.clsAutoKillerScript

            AddHandler objDAOC.OnLog, AddressOf LogLine

            LogLineAsync("Initializing DAOCScript v" & objDAOC.getVersion & "....")

            objWindowManager = New DAOCWindowManager
            objWindowManager.CharacterName = ReadSetting("CharacterName", GetType(String))

            strProfile = ReadSetting("Profile", GetType(String))

            ' This section sets up various variables for the DLL.
            'objDAOC.ChatLog = ReadSetting("ChatLog", GetType(String))
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

            'sit key
            bytSitKey = objKeys.Sit_Stand_Key
            bytFaceKey = objKeys.Face_Key

            'set timer intervals
            tmePowerRegen.Stop()
            tmePowerRegen.Interval = intPowerRegenTimer

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
                        End If

                        'check to see if power regen is enabled
                        If blnPowerRegenEnabled Then
                            'iterate collection and get id's
                            Dim objPowerRegenTarget As clsPowerRegenTarget

                            For Each objPowerRegenTarget In colPowerRegenTargets
                                If objPowerRegenTarget.Name = objWindowManager.CharacterName Then
                                    objPowerRegenTarget.Index = objDAOC.PlayerIndex
                                Else
                                    objPowerRegenTarget.Index = objDAOC.SetTarget(objPowerRegenTarget.Name, , False)
                                End If


                                LogLine("Adding index of " & objPowerRegenTarget.Index & " to power regen collection for " & _
                                    objPowerRegenTarget.Name)
                            Next
                        End If

                        'get index for HOT target
                        'shtHOTTargetIndex = objDAOC.SetTarget(strHOTTarget, , False)

                        'temp fix for gala
                        shtHOTTargetIndex = FindClosestMob(objDAOC, "underhill ally", 43, 45, 1000)
                    End If

                    'StopAfterPlayerDeath player dead, set flag
                    If objDAOC.MobHealth(shtStopAfterPlayerDeathIndex) = 0 Then
                        bytIsTargetDead = 1
                    End If


                    '*********************************************************************************
                    'power regen logic
                    '*********************************************************************************
                    If blnPowerRegen And blnPowerRegenEnabled And objDAOC.IsPlayerDead = 0 And bytIsTargetDead = 0 Then
                        'iterate collection and cast abs
                        Dim objPowerRegenTarget As clsPowerRegenTarget

                        'see if player is sitting
                        If objDAOC.isPlayerSitting = 1 Then
                            LogLine("Player is sitting, stand")
                            objAKMent.Sleep(3000)
                            objDAOC.SendKeys(bytSitKey, 0)
                            objAKMent.Sleep(1500)
                        End If

                        tmePowerRegen.Stop()
                        objAKMent.Sleep(2000)

                        For Each objPowerRegenTarget In colPowerRegenTargets
                            LogLine("Casting power regen on " & objPowerRegenTarget.Name)
                            objDAOC.SelectTarget(objPowerRegenTarget.Index)
                            objAKMent.Sleep(1000)
                            objDAOC.SendKeys(bytPowerRegenKey, 0)
                            objAKMent.Sleep(intPowerRegenDelay)
                            objAKMent.Sleep(100)

                            intRegenCount = intRegenCount + 1

                            If intRegenCount = 2 Then
                                intRegenCount = 0

                                'select target
                                objDAOC.SelectTarget(shtHOTTargetIndex)
                                objAKMent.Sleep(100)

                                LogLine("Casting HOT on " & strHOTTarget)

                                'cast HOT
                                objDAOC.SendKeys(bytHOTKey, 0)
                                objAKMent.Sleep(intHOTDelay)
                                objAKMent.Sleep(100)

                            End If
                        Next

                        'cast mcl
                        objAKMent.Sleep(100)
                        objDAOC.SendKeys(bytMCLKey, 0)
                        objAKMent.Sleep(3000)

                        'see if sitting to regain power is enabled
                        If blnSitToRegainPower Then
                            'see if player is standing
                            If objDAOC.isPlayerSitting = 0 Then
                                LogLine("Player is standing, sit")
                                objAKMent.Sleep(1500)
                                objDAOC.SendKeys(bytSitKey, 0)
                                objAKMent.Sleep(1500)
                            End If
                        End If

                        blnPowerRegen = False
                        tmePowerRegen.Start()
                    End If

                    '*********************************************************************************
                    'HOT regen logic
                    '*********************************************************************************
                    If blnHOTEnabled And objDAOC.PlayerMana = 100 And objDAOC.IsPlayerDead = 0 And bytIsTargetDead = 0 Then
                        'see if player is sitting
                        If objDAOC.isPlayerSitting = 1 Then
                            LogLine("Player is sitting, stand")
                            objAKMent.Sleep(1500)
                            objDAOC.SendKeys(bytSitKey, 0)
                            objAKMent.Sleep(1500)
                        End If

                        'select target
                        objDAOC.SelectTarget(shtHOTTargetIndex)
                        objAKMent.Sleep(100)

                        LogLine("Casting HOT on " & strHOTTarget)

                        'cast HOT
                        objDAOC.SendKeys(bytHOTKey, 0)
                        objAKMent.Sleep(intHOTDelay)
                        objAKMent.Sleep(100)

                        If blnSitToRegainPower Then
                            'see if player is standing
                            If objDAOC.isPlayerSitting = 0 Then
                                LogLine("Player is standing, sit")
                                objAKMent.Sleep(1500)
                                objDAOC.SendKeys(bytSitKey, 0)
                                objAKMent.Sleep(1500)
                            End If
                        End If
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
                                    objAKMent.Sleep(2000)
                                    objDAOC.SendString(strReleaseText & "~")
                                    objAKMent.Sleep(2000)

                                    LogLine("Sleeping for 1 minute, then sending quit")
                                    objAKMent.Sleep(60000)

                                    objAKMent.Sleep(2000)
                                    objDAOC.SendString("/quit~")
                                    objAKMent.Sleep(2000)

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
                            objAKMent.Sleep(120000)

                            'still alive, quit
                            If objDAOC.IsPlayerDead = 0 Then
                                LogLine("Still alive, quitting")
                                objAKMent.Sleep(2000)
                                objDAOC.SendString("/quit~")
                                objAKMent.Sleep(2000)

                                LogLine("Terminating thread")
                                blnTerminateThread = True
                            End If
                        End If
                    End If
                End If 'If IsDAOCActive() AndAlso Not objDAOC.IsPlayerDead Then
            End While
        Catch Ex As Exception
            LogLineAsync("Exception occured! AKMent stopped!")
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
            objAKMent = Nothing

            objDAOC = Nothing
        End Try
    End Sub
    Private Function FindClosestMob(ByVal objDAOC As AutoKillerScript.clsAutoKillerScript, _
                                    ByVal MobName As String, _
                                    ByVal MinLevel As Integer, _
                                    ByVal MaxLevel As Integer, _
                                    ByVal Range As Integer) As Short

        Dim intTempId As Integer

        Dim blnMobLoop As Boolean = False

        intTempId = objDAOC.FindClosestMob(MinLevel, MaxLevel, Range)

        While Not blnMobLoop
            If intTempId = -1 Then
                FindClosestMob = -1
                blnMobLoop = True
            Else
                If Trim(objDAOC.MobName(intTempId)) = Trim(MobName) Then
                    FindClosestMob = intTempId
                    blnMobLoop = True
                Else
                    intTempId = objDAOC.FindNextClosestMob(MinLevel, MaxLevel, Range)

                End If
            End If
        End While

    End Function

    Public Sub LoadProfile()
        Dim objEncoder As New System.Text.UTF8Encoding

        If IO.File.Exists(strProfile) Then
            Try
                Dim xmlDoc As Xml.XmlDocument = New Xml.XmlDocument
                xmlDoc.Load(strProfile)

                LogLine("--------------------------------------------------------------------------------")
                LogLine("Running profile: " & strProfile)

                'Settings
                blnStopAfterDeath = xmlDoc.SelectSingleNode("//Profile/Settings/@StopAfterDeath").Value
                strStopAfterPlayerDeath = xmlDoc.SelectSingleNode("//Profile/Settings/@StopAfterPlayerDeath").Value
                strReleaseText = xmlDoc.SelectSingleNode("//Profile/Settings/@ReleaseText").Value
                blnSitToRegainPower = xmlDoc.SelectSingleNode("//Profile/Settings/@SitToRegainPower").Value
                bytMCLKey = objEncoder.GetBytes(xmlDoc.SelectSingleNode("//Profile/Settings/@MCLKey").Value)(0)

                LogLine("StopAfterDeath: " & blnStopAfterDeath)
                LogLine("StopAfterPlayerDeath: " & strStopAfterPlayerDeath)
                LogLine("ReleaseText: " & strReleaseText)
                LogLine("SitToRegainPower: " & blnSitToRegainPower)
                LogLine("MCLKey: " & xmlDoc.SelectSingleNode("//Profile/Settings/@MCLKey").Value)

                'Power regen
                blnPowerRegenEnabled = xmlDoc.SelectSingleNode("//Profile/PowerRegen/@Enabled").Value
                bytPowerRegenKey = objEncoder.GetBytes(xmlDoc.SelectSingleNode("//Profile/PowerRegen/@Key").Value)(0)
                intPowerRegenDelay = xmlDoc.SelectSingleNode("//Profile/PowerRegen/@Delay").Value
                intPowerRegenTimer = xmlDoc.SelectSingleNode("//Profile/PowerRegen/@Timer").Value

                LogLine("PowerRegenEnabled: " & blnPowerRegenEnabled)
                LogLine("PowerRegenKey: " & xmlDoc.SelectSingleNode("//Profile/PowerRegen/@Key").Value)
                LogLine("PowerRegenDelay: " & intPowerRegenDelay)
                LogLine("PowerRegenTimter: " & intPowerRegenTimer)

                Dim objPowerRegenTarget As clsPowerRegenTarget
                Dim xmlNode As Xml.XmlNode

                For Each xmlNode In xmlDoc.SelectNodes("//Profile/PowerRegenTargets/PowerRegenTarget")
                    objPowerRegenTarget = New clsPowerRegenTarget
                    objPowerRegenTarget.Name = xmlNode.SelectSingleNode("@Name").Value
                    colPowerRegenTargets.Add(objPowerRegenTarget)

                    LogLine("PowerRegenTarget: Name: " & objPowerRegenTarget.Name)
                Next

                'HOT
                blnHOTEnabled = xmlDoc.SelectSingleNode("//Profile/HOT/@Enabled").Value
                bytHOTKey = objEncoder.GetBytes(xmlDoc.SelectSingleNode("//Profile/HOT/@Key").Value)(0)
                intHOTDelay = xmlDoc.SelectSingleNode("//Profile/HOT/@Delay").Value

                LogLine("HOTEnabled: " & blnHOTEnabled)
                LogLine("HOTKey: " & xmlDoc.SelectSingleNode("//Profile/HOT/@Key").Value)
                LogLine("HOTDelay: " & intHOTDelay)

                strHOTTarget = xmlDoc.SelectSingleNode("//Profile/HOTTarget/@Name").Value
                LogLine("HOTTarget: " & strHOTTarget)

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
            Me.BeginInvoke(mi, New Object() {"AKMent (Active)"})
            'Text = "AKMent (Active)"

            Return True
        Else
            Dim mi As New UpdateTitle(AddressOf UpdateFormTitle)
            Me.BeginInvoke(mi, New Object() {"AKMent (Paused)"})
            'Text = "AKMent (Active)"

            Return False
        End If
    End Function

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "AKMent"

        LogLine("AKMent v0.01 (c) Version125")
        LogLine("DAOC Packets is not required for this application")

        'had to comment this out
        'Try
        '    System.Diagnostics.Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.AboveNormal
        'Catch Ex As Exception
        '    ' On Windows 98 machines you get an error for 'above normal'
        '    System.Diagnostics.Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.High
        'End Try

        Threading.Thread.CurrentThread.Name = "MainThread"
    End Sub

    Private Sub frmMain_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Not IsNothing(objAKMent) Then
            btnStartStop_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub tmeRelease_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles tmeRelease.Elapsed
        blnRelease = True
    End Sub

    Private Sub tmePowerRegen_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles tmePowerRegen.Elapsed
        blnPowerRegen = True
    End Sub
End Class

