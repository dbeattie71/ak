Public Class frmMain
    Inherits System.Windows.Forms.Form

    Shared gthdStartXp As Threading.Thread

    Shared gblnTerminateThread As Boolean = False
    Shared gblnBuffs As Boolean

    Shared gshtPetId As Short

    Shared gblnPull As Boolean

    Shared blnRelease As Boolean = False

    Shared bytIsTargetDead As Byte = 0

    Shared shtStopAfterPlayerDeathIndex As Short

    Shared intPBPercent As Integer

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
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents lbLog As System.Windows.Forms.ListBox
    Friend WithEvents btnStartStop As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Timers.Timer
    Friend WithEvents Timer2 As System.Timers.Timer
    Friend WithEvents tmeRelease As System.Timers.Timer
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.lbLog = New System.Windows.Forms.ListBox
        Me.btnStartStop = New System.Windows.Forms.Button
        Me.Timer1 = New System.Timers.Timer
        Me.Timer2 = New System.Timers.Timer
        Me.tmeRelease = New System.Timers.Timer
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.Timer1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Timer2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tmeRelease, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Location = New System.Drawing.Point(8, 8)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(416, 270)
        Me.TabControl1.TabIndex = 0
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
        'btnStartStop
        '
        Me.btnStartStop.Location = New System.Drawing.Point(352, 288)
        Me.btnStartStop.Name = "btnStartStop"
        Me.btnStartStop.Size = New System.Drawing.Size(72, 24)
        Me.btnStartStop.TabIndex = 1
        Me.btnStartStop.Text = "Start"
        '
        'Timer1
        '
        Me.Timer1.Interval = 8000
        Me.Timer1.SynchronizingObject = Me
        '
        'Timer2
        '
        Me.Timer2.Enabled = True
        Me.Timer2.Interval = 1080000
        Me.Timer2.SynchronizingObject = Me
        '
        'tmeRelease
        '
        Me.tmeRelease.Interval = 660000
        Me.tmeRelease.SynchronizingObject = Me
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(240, 288)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.TabIndex = 2
        Me.TextBox1.Text = ""
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(432, 320)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.btnStartStop)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "frmMain"
        Me.Text = "DSPbae"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.Timer1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Timer2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tmeRelease, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnStartStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartStop.Click
        If IsNothing(gthdStartXp) Then
            gblnTerminateThread = False
            gblnBuffs = True

            gthdStartXp = New Threading.Thread(AddressOf StartXp)
            gthdStartXp.Start()

            btnStartStop.Text = "Stop"

            Timer1.Start()

        Else
            Text = "DSPbae"

            btnStartStop.Text = "Start"

            gblnTerminateThread = True

            gthdStartXp.Join()
            gthdStartXp = Nothing
        End If
    End Sub

    Private Sub StartXp()
        Dim objDAOC As AutoKillerScript.clsAutoKillerScript = New AutoKillerScript.clsAutoKillerScript
        AddHandler objDAOC.OnLog, AddressOf LogLine

        Dim shtMobId As Short

        Dim intTempCounter As Integer
        Dim intTempRange As Integer

        Dim strLogLine As String

        Dim blnFightOver As Boolean = False

        Try

            LogLine("Initializing DAOCScript v" & objDAOC.getVersion & "....")

            ' This section sets up various variables for the DLL.
            ' not in a setting file for now
            objDAOC.ChatLog = "c:\mythic\isles\realchat.log"
            objDAOC.RegKey = "DEREKBEATTIE"
            objDAOC.SetLeftTurnKey = &H41 'a
            objDAOC.SetRightTurnKey = &H44 'd
            objDAOC.SetConsiderKey = &H4E 'n

            objDAOC.DoInit()

            'MsgBox(objDAOC.gPlayerXCoord)
            'MsgBox(objDAOC.gPlayerYCoord)
            'MsgBox(objDAOC.gPlayerZCoord)

            'get the pet's id
            gshtPetId = FindMob(objDAOC, "underhill ally", 39, 45, 1000)

            If gshtPetId <> -1 Then
                LogLine("FindMob(), Id: " & gshtPetId & " Name: " & Trim(objDAOC.MobName(gshtPetId)))
            Else
                LogLine("Pet not found, shutting down thread")
                gblnTerminateThread = True
            End If

            'get StopAfterPlayerDeath index
            shtStopAfterPlayerDeathIndex = objDAOC.SetTarget("Thundercast", , False)
            LogLine("Got index of " & shtStopAfterPlayerDeathIndex & " for " & "Ulysse")

            If shtStopAfterPlayerDeathIndex = -1 Then
                LogLine("Can't find player, shutting thread down")
                gblnTerminateThread = True
            End If

            intPBPercent = CInt(TextBox1.Text)

            While Not gblnTerminateThread
                If IsDAOCActive(objDAOC) And gblnPull = True And objDAOC.IsPlayerDead = 0 And bytIsTargetDead = 0 Then
                    gblnPull = False

                    LogLine("Stopping Timer...")
                    Timer1.Stop()

                    If objDAOC.MobHealth(shtStopAfterPlayerDeathIndex) = 0 Then
                        bytIsTargetDead = 1
                    End If

                    If gblnBuffs = True Then
                        gblnBuffs = False

                        'stand
                        objDAOC.SendString("/stand~")
                        gthdStartXp.Sleep(300)

                        'do buffs
                        objDAOC.SendString("7")
                        gthdStartXp.Sleep(3500)

                        objDAOC.SendString("8")
                        gthdStartXp.Sleep(3500)

                        objDAOC.SendString("9")
                        gthdStartXp.Sleep(3500)

                    End If

                    LogLine("Checking for mobs in combat...")
                    shtMobId = objDAOC.FindClosestMobInCombat(1000, gshtPetId)

                    If shtMobId <> -1 And shtMobId <> gshtPetId Then
                        LogLine("Found mobs in combat")

                        While objDAOC.MobHealth(shtMobId) > intPBPercent And objDAOC.IsPlayerDead = 0
                            LogLine("MobName(): " & objDAOC.MobName(shtMobId) & " MobHealth(): " & objDAOC.MobHealth(shtMobId))
                            gthdStartXp.Sleep(300)
                        End While

                        'select target
                        LogLine("Select target")
                        objDAOC.SelectTarget(shtMobId)
                        gthdStartXp.Sleep(300)

                        '/stick to mob
                        LogLine("Stick to mob")
                        objDAOC.SendString("=")
                        gthdStartXp.Sleep(2000)

                        'break /stick
                        LogLine("Break stick")
                        objDAOC.StartRunning()
                        gthdStartXp.Sleep(700)
                        objDAOC.StopRunning()

                        'case str/con debuff
                        LogLine("Cast str/con debuff")
                        objDAOC.SendString("3")
                        gthdStartXp.Sleep(200)

                        'cast 4 pbae's
                        LogLine("Pbaoe")
                        objDAOC.SendString("2")
                        gthdStartXp.Sleep(200)
                        objDAOC.SendString("2")

                        gthdStartXp.Sleep(2500)

                        objDAOC.SendString("2")
                        gthdStartXp.Sleep(200)
                        objDAOC.SendString("2")

                        gthdStartXp.Sleep(2500)

                        objDAOC.SendString("1")
                        gthdStartXp.Sleep(200)
                        objDAOC.SendString("2")

                        gthdStartXp.Sleep(3000)

                        objDAOC.SendString("9")
                        gthdStartXp.Sleep(4000)

                        'mcl
                        If objDAOC.PlayerMana < 51 Then
                            gthdStartXp.Sleep(7000)
                            objDAOC.SendString("5")
                            gthdStartXp.Sleep(200)
                        End If

                        'sit
                        'If objDAOC.PlayerHealth = 100 Then
                        '    objDAOC.SendString("/sit~")
                        '    gthdStartXp.Sleep(3000)
                        'End If

                        LogLine("Put thread to sleep for 25 seconds")
                        gthdStartXp.Sleep(10000)
                    Else
                        LogLine("No mobs found in combat")
                    End If

                    LogLine("Starting Timer...")
                    Timer1.Start()
                Else
                    If objDAOC.IsPlayerDead = 1 Then
                        'player has died, if blnStopAfterDeath is true, player will release and quit
                        Dim blnTempLoop As Boolean = True

                        LogLine("Player is dead, starting release timer")
                        tmeRelease.Start()

                        While blnTempLoop
                            If blnRelease Then
                                'stop timer
                                tmeRelease.Stop()

                                blnTempLoop = False

                                LogLine("Time up, sending release line")
                                gthdStartXp.Sleep(2000)
                                objDAOC.SendString("/release~")
                                gthdStartXp.Sleep(2000)

                                LogLine("Sleeping for 1 minute, then sending quit")
                                gthdStartXp.Sleep(60000)

                                gthdStartXp.Sleep(2000)
                                objDAOC.SendString("/quit~")
                                gthdStartXp.Sleep(2000)

                                LogLine("Terminating thread")
                                gblnTerminateThread = True
                            End If
                        End While
                    Else
                        'if one of the targets, either powerdrain or powertransfer targets dies, then quit
                        If bytIsTargetDead = 1 Then
                            LogLine("A target died, sleep for 2 miniutes to see if player will die")
                            gthdStartXp.Sleep(120000)

                            'still alive, quit
                            If objDAOC.IsPlayerDead = 0 Then
                                LogLine("Still alive, quitting")
                                gthdStartXp.Sleep(2000)
                                objDAOC.SendString("/quit~")
                                gthdStartXp.Sleep(2000)

                                LogLine("Terminating thread")
                                gblnTerminateThread = True
                            End If
                        End If
                    End If
                End If
            End While
        Catch Ex As Exception
            LogLine("Exception occured! DSPbae stopped!")
            LogLine(Ex.Message)
            LogLine(Ex.Source)
            LogLine(Ex.StackTrace)
        Finally
            objDAOC.StopInit()
            LogLine("Stopping DAOCScript v" & objDAOC.getVersion & "....")

            btnStartStop.Text = "Start"
            gthdStartXp = Nothing

            objDAOC = Nothing
        End Try
    End Sub

    Declare Auto Function GetForegroundWindow Lib "user32" () As IntPtr
    Declare Auto Function GetWindowThreadProcessId Lib "user32" (ByVal hWnd As IntPtr, ByRef lpdwProcessId As Int32) As Int32

    Public Function IsDAOCActive(ByVal oDAOC As AutoKillerScript.clsAutoKillerScript) As Boolean
        Dim intProcessID As Integer = GetActiveProcess()
        If intProcessID = oDAOC.GameProcess Then
            Text = "DSPbae (Active)"
            Return True
        End If

        Text = "DSPbae (Paused)"
        Return False
    End Function

    Function GetActiveProcess() As Integer
        Dim ProcessID As Int32

        Try
            GetWindowThreadProcessId(GetForegroundWindow, ProcessID)
            Return ProcessID
        Catch E As Exception
        End Try

        Return 0
    End Function

    Sub LogLine(ByRef Line As String)
        Line = Format(Year(Now), "0000") & "-" & Format(Month(Now), "00") & "-" & Format(Microsoft.VisualBasic.Day(Now), "00") & "|" & Format(Hour(Now), "00") & ":" & Format(Minute(Now), "00") & ":" & Format(Second(Now), "00") & "| " & Line
        lbLog.BeginUpdate()
        lbLog.Items.Insert(0, Line)

        If lbLog.Items.Count > 128 Then
            lbLog.Items.RemoveAt(127)
        End If
        lbLog.EndUpdate()

        '    OLD CODE
        'Line = Format(Year(Now), "0000") & "-" & Format(Month(Now), "00") & "-" & Format(Microsoft.VisualBasic.Day(Now), "00") & "|" & Format(Hour(Now), "00") & ":" & Format(Minute(Now), "00") & ":" & Format(Second(Now), "00") & "|" & Line
        'lbLog.BeginUpdate()
        'lbLog.Items.Add(Line)

        'If lbLog.Items.Count > 128 Then
        '    lbLog.Items.RemoveAt(0)
        'End If
        'lbLog.EndUpdate()
    End Sub
    Private Function FindMob(ByVal objDAOC As AutoKillerScript.clsAutoKillerScript, _
                             ByVal MobName As String, _
                             ByVal MinLevel As Integer, ByVal MaxLevel As Integer, _
                             ByVal Range As Integer)

        Dim intTempId As Integer

        Dim blnMobLoop As Boolean = False

        intTempId = objDAOC.FindClosestMob(MinLevel, MaxLevel, Range)

        While Not blnMobLoop
            If intTempId = -1 Then
                FindMob = -1
                blnMobLoop = True
            Else
                If Trim(objDAOC.MobName(intTempId)) = Trim(MobName) Then
                    FindMob = intTempId
                    blnMobLoop = True
                Else
                    intTempId = objDAOC.FindNextClosestMob(MinLevel, MaxLevel, Range)
                End If
            End If
        End While

    End Function
    Private Sub MoveToGXY(ByVal objDAOC As AutoKillerScript.clsAutoKillerScript, _
                             ByVal X As Double, ByVal Y As Double, ByVal Z As Double, ByVal Range As Double)

        If Not (objDAOC.ZDistance(objDAOC.gPlayerXCoord, objDAOC.gPlayerYCoord, objDAOC.gPlayerZCoord, X, Y, Z)) < Range Then
            'AppActivate(objDAOC.GameProcess)
            objDAOC.StartRunning()
            While objDAOC.ZDistance(objDAOC.gPlayerXCoord, objDAOC.gPlayerYCoord, objDAOC.gPlayerZCoord, X, Y, Z) > Range
                objDAOC.TurnToHeading(objDAOC.FindHeading(objDAOC.gPlayerXCoord, objDAOC.gPlayerYCoord, X, Y))
                'gthdStartXp.Sleep(200)
            End While
            objDAOC.StopRunning()
        End If
    End Sub
    Private Sub MoveToPullSpot(ByVal objDAOC As AutoKillerScript.clsAutoKillerScript, _
                                  ByVal MobId As Short, ByVal PullRange As Integer)

        If Not (objDAOC.ZDistance(objDAOC.gPlayerXCoord, objDAOC.gPlayerYCoord, objDAOC.gPlayerZCoord, _
                objDAOC.MobXCoord(MobId), objDAOC.MobYCoord(MobId), objDAOC.MobZCoord(MobId)) < PullRange) Then

            'AppActivate(objDAOC.GameProcess)
            objDAOC.StartRunning()

            While objDAOC.ZDistance(objDAOC.gPlayerXCoord, objDAOC.gPlayerYCoord, objDAOC.gPlayerZCoord, _
                  objDAOC.MobXCoord(MobId), objDAOC.MobYCoord(MobId), objDAOC.MobZCoord(MobId)) > PullRange

                objDAOC.TurnToHeading(objDAOC.FindHeading(objDAOC.gPlayerXCoord, objDAOC.gPlayerYCoord, _
                                       objDAOC.MobXCoord(MobId), objDAOC.MobYCoord(MobId)))
                gthdStartXp.Sleep(200)
            End While
            objDAOC.StopRunning()
        End If

    End Sub

    Private Sub MoveToBase(ByVal objDAOC As AutoKillerScript.clsAutoKillerScript)
        'put in settings file, have button for setbase, blah blah
        Const dlbBaseX As Double = 369108
        Const dlbBaseY As Double = 694588
        Const dlbBaseZ As Double = 3689

        MoveToGXY(objDAOC, dlbBaseX, dlbBaseY, dlbBaseZ, 100)
    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub frmMain_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Not IsNothing(gthdStartXp) Then
            btnStartStop_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub Timer1_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles Timer1.Elapsed
        gblnPull = True
    End Sub

    Private Sub Timer2_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles Timer2.Elapsed
        gblnBuffs = True
    End Sub

    Private Sub tmeRelease_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles tmeRelease.Elapsed
        blnRelease = True
    End Sub
End Class
