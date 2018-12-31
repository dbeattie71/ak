Imports System.Xml
Imports System.IO
Imports System.Security.Permissions

Public Class frmMain
    Inherits System.Windows.Forms.Form

    'Shared gintTotalSleep As Integer

    Shared gthdStartXp As Threading.Thread

    Shared gblnTerminateThread As Boolean = False
    Shared gblnPull As Boolean
    Shared gblnDmgShld As Boolean

    Shared blnRelease As Boolean = False

    Shared gshtPetId As Short

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
    Friend WithEvents lbLog As System.Windows.Forms.ListBox
    Friend WithEvents Timer1 As System.Timers.Timer
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Timer2 As System.Timers.Timer
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents tmeRelease As System.Timers.Timer
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnStartStop = New System.Windows.Forms.Button
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.lbLog = New System.Windows.Forms.ListBox
        Me.Timer1 = New System.Timers.Timer
        Me.Button1 = New System.Windows.Forms.Button
        Me.Timer2 = New System.Timers.Timer
        Me.tmeRelease = New System.Timers.Timer
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.Timer1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Timer2, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.TabControl1.Location = New System.Drawing.Point(8, 8)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(416, 270)
        Me.TabControl1.TabIndex = 2
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.lbLog)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(408, 244)
        Me.TabPage1.TabIndex = 2
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
        'Timer1
        '
        Me.Timer1.Interval = 1000
        Me.Timer1.SynchronizingObject = Me
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(264, 288)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(80, 24)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "Button1"
        '
        'Timer2
        '
        Me.Timer2.Enabled = True
        Me.Timer2.Interval = 480000
        Me.Timer2.SynchronizingObject = Me
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
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.btnStartStop)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "frmMain"
        Me.Text = "DSHunt"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.Timer1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Timer2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tmeRelease, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnStartStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartStop.Click

        If IsNothing(gthdStartXp) Then
            gblnTerminateThread = False

            gthdStartXp = New Threading.Thread(AddressOf StartXp)
            gthdStartXp.Start()

            btnStartStop.Text = "Stop"

            gblnPull = True
        Else
            Text = "DSHunt"

            btnStartStop.Text = "Start"

            gblnTerminateThread = True

            gthdStartXp.Join()
            gthdStartXp = Nothing
        End If
    End Sub

    Private Sub StartXp()
        Dim objDAOC As AutoKillerScript.clsAutoKillerScript = New AutoKillerScript.clsAutoKillerScript
        AddHandler objDAOC.OnLog, AddressOf LogLine

        Dim dblTempRange As Double

        Dim shtMobId As Short
        Dim shtLootId As Short

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

            'get the pet's id
            gshtPetId = FindMob(objDAOC, "underhill ally", 40, 45, 1000)

            If gshtPetId = -1 Then
                gshtPetId = FindMob(objDAOC, "underhill ally", 40, 45, 1000)
            End If

            If gshtPetId <> -1 Then
                LogLine("FindMob(), Id: " & gshtPetId & " Name: " & Trim(objDAOC.MobName(gshtPetId)))
            Else
                LogLine("Pet not found, shutting down thread")
                gblnTerminateThread = True
            End If

            While Not gblnTerminateThread
                If IsDAOCActive(objDAOC) And gblnPull = True And objDAOC.IsPlayerDead = 0 Then
                    gblnPull = False

                    LogLine("Stopping Timer...")
                    Timer1.Stop()

                    'change to quickbar 10 ** CHANGE LATER
                    gthdStartXp.Sleep(2000)
                    LogLine("Change to quickbar 10")
                    objDAOC.SendString("/qbar 10~")
                    gthdStartXp.Sleep(2000)

                    'see if its safe to pull
                    shtMobId = FindSafeMob(objDAOC, "finliath", 45, 66, 2500)

                    If shtMobId = -1 Then
                        shtMobId = FindSafeMob(objDAOC, "finliath", 45, 66, 2500)
                    End If

                    If shtMobId <> -1 And objDAOC.PlayerMana() > 80 Then
                        LogLine("Warshades's X: " & objDAOC.MobXCoord(shtMobId))

                        'set pet to defend
                        LogLine("Setting pet to defend")
                        objDAOC.SendString("1")
                        gthdStartXp.Sleep(2000)

                        'move to pull spot
                        LogLine("Move to pull spot")
                        MoveToPullSpot(objDAOC, shtMobId, 1290)
                        gthdStartXp.Sleep(300)

                        'select target
                        objDAOC.SelectTarget(shtMobId)

                        'send pet to mob
                        LogLine("Setting pet to attack mode")
                        objDAOC.SendString("2")
                        gthdStartXp.Sleep(800)

                        'move back to base
                        LogLine("Move back to base")
                        MoveToBase(objDAOC)
                        gthdStartXp.Sleep(2000)

                        'set pet to passive
                        LogLine("Setting pet to passive")
                        objDAOC.SendString("3")
                        gthdStartXp.Sleep(300)

                        'make sure pet is in ds range
                        dblTempRange = objDAOC.ZDistance(objDAOC.gPlayerXCoord, objDAOC.gPlayerYCoord, objDAOC.gPlayerZCoord, _
                                       objDAOC.MobXCoord(gshtPetId), objDAOC.MobYCoord(gshtPetId), objDAOC.MobZCoord(gshtPetId))
                        LogLine("Pet distance: " & dblTempRange)
                        While dblTempRange > 1350 And objDAOC.IsPlayerDead = 0

                            If objDAOC.IsPlayerDead() Then
                                gblnTerminateThread = True
                                dblTempRange = 0
                            End If

                            dblTempRange = objDAOC.ZDistance(objDAOC.gPlayerXCoord, objDAOC.gPlayerYCoord, objDAOC.gPlayerZCoord, _
                                       objDAOC.MobXCoord(gshtPetId), objDAOC.MobYCoord(gshtPetId), objDAOC.MobZCoord(gshtPetId))

                            LogLine("Pet distance: " & dblTempRange)

                            gthdStartXp.Sleep(100)
                        End While

                        'target pet so shield doesn't drop when a mob dies
                        LogLine("Target pet")

                        'target pet
                        objDAOC.SelectTarget(gshtPetId)

                        'cast ds
                        LogLine("Cast damage shield")
                        objDAOC.SendString("4")
                        gthdStartXp.Sleep(3500)

                        'face mob
                        LogLine("Face pet")
                        objDAOC.SendString("-")
                        gthdStartXp.Sleep(300)

                        'sleep for 7 seconds to give mobs a chance to get to pet
                        gthdStartXp.Sleep(7000)

                        'set pet to defend
                        LogLine("Setting pet to defend")
                        objDAOC.SendString("1")
                        gthdStartXp.Sleep(2000)

                        LogLine("Starting fight logic")

                        blnFightOver = False
                        While Not blnFightOver And objDAOC.IsPlayerDead = 0
                            'strLogLine = objDAOC.GetString()

                            'check for player death
                            If objDAOC.IsPlayerDead() Then
                                blnFightOver = True
                                gblnTerminateThread = True
                                'LogLine("Player dead, sending page")
                                'SendMail()
                            End If

                            'check to see if mobs still in combat
                            shtMobId = objDAOC.FindClosestMobInCombat(1500, gshtPetId)
                            If shtMobId = -1 Then
                                LogLine("No more mobs in combat")

                                blnFightOver = True
                                LogLine("Fight complete")

                                'stop damage shield
                                LogLine("Stop damage shield")
                                objDAOC.SendString("4")
                                gthdStartXp.Sleep(4200)

                            Else
                                'LogLine("Still mobs in combat")
                            End If

                            gthdStartXp.Sleep(300)
                        End While

                        'get loots
                        shtLootId = objDAOC.SearchArea(400, 0)
                        If shtLootId <> -1 Then
                            LogLine("Found loot")
                            MoveToPullSpot(objDAOC, shtLootId, 100)
                        End If

                        GetLoot(objDAOC)
                        gthdStartXp.Sleep(1000)


                        'heal pet 
                        'objDAOC.SendString("5")
                        'gthdStartXp.Sleep(3500)

                        'check to see if dmg shld needs casted
                        If gblnDmgShld = True Then
                            gblnDmgShld = False

                            LogLine("Re-casting dmg shld")

                            'target pet
                            objDAOC.SelectTarget(gshtPetId)

                            'heal pet 
                            objDAOC.SendString("6")
                            gthdStartXp.Sleep(4200)
                        End If

                        'check for mcl
                        LogLine("Mana: " & objDAOC.PlayerMana())
                        If objDAOC.PlayerMana() < 46 Then
                            LogLine("Hitting mcl if up")
                            objDAOC.SendString("7")
                            gthdStartXp.Sleep(4200)
                        End If

                        'sit down
                        'objDAOC.SendString("x")
                        'gthdStartXp.Sleep(2000)

                        'check for remote invite
                        strLogLine = objDAOC.GetString()
                        While Trim(strLogLine) <> "" And objDAOC.IsPlayerDead = 0
                            'LogLine(strLogLine)
                            If strLogLine.IndexOf("invitemebish", 1) > 0 Then
                                LogLine("1: " & strLogLine)

                                '@@Barax sends
                                Dim intAtStart As Integer
                                Dim intSendsStart As Integer
                                Dim intNameLen As Integer
                                Dim strName As String

                                '@@Aarmoz sends, "invitemebish"
                                intAtStart = strLogLine.IndexOf("@")
                                intSendsStart = strLogLine.IndexOf("sends")
                                intNameLen = ((intSendsStart - 1) - (intAtStart + 2))
                                strName = strLogLine.Substring(intAtStart + 2, intNameLen)

                                LogLine("2: " & strName)

                                'send invite
                                objDAOC.SendString("/invite " & strName & "~")
                                gthdStartXp.Sleep(300)
                                'Else
                                '    If strLogLine.IndexOf("quitmebish", 1) > 0 Then
                                '        gblnTerminateThread = True
                                '        LogLine("Quit command sent, quitting")
                                '        LogLine("Sleeping for 1 minute, then sending quit")
                                '        gthdStartXp.Sleep(60000)

                                '        gthdStartXp.Sleep(2000)
                                '        objDAOC.SendString("/quit~")
                                '        gthdStartXp.Sleep(2000)

                                '        LogLine("Terminating thread")
                                '        gblnTerminateThread = True
                                'End If
                            End If

                            strLogLine = objDAOC.GetString()
                        End While

                    Else
                        LogLine("No warshades found or mana to low, waiting timer interval")
                        'gblnTerminateThread = True
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
                    End If
                End If
            End While
        Catch Ex As Exception
            LogLine("Exception occured! DSHunt stopped!")
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
            Text = "DSHunt (Active)"
            Return True
        End If

        Text = "DSHunt (Paused)"
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

    Private Function FindSafeMob(ByVal objDAOC As AutoKillerScript.clsAutoKillerScript, _
                             ByVal MobName As String, _
                             ByVal MinLevel As Integer, ByVal MaxLevel As Integer, _
                             ByVal Range As Integer)

        Dim intTempId As Integer

        Dim blnMobLoop As Boolean = False

        intTempId = objDAOC.FindClosestMob(MinLevel, MaxLevel, Range)

        While Not blnMobLoop
            If intTempId = -1 Then
                FindSafeMob = -1
                blnMobLoop = True
            Else
                If Trim(objDAOC.MobName(intTempId)) = Trim(MobName) And IsMobSafe(objDAOC, intTempId) Then
                    FindSafeMob = intTempId
                    blnMobLoop = True
                Else
                    intTempId = objDAOC.FindNextClosestMob(MinLevel, MaxLevel, Range)
                End If
            End If
        End While

    End Function

    Private Function IsMobSafe(ByVal objdaoc As AutoKillerScript.clsAutoKillerScript, ByVal MobId As Short) As Boolean
        Return True
        'Dim shtMobXCoord As Integer

        'shtMobXCoord = objdaoc.MobXCoord(MobId)

        'If shtMobXCoord = 370822 Or shtMobXCoord = 371021 Or shtMobXCoord = 371012 Then

        '    Return True
        'Else
        '    Return False
        'End If

    End Function

    Private Sub MoveToGXY(ByVal objDAOC As AutoKillerScript.clsAutoKillerScript, _
                             ByVal X As Double, ByVal Y As Double, ByVal Z As Double, ByVal Range As Double)

        If Not (objDAOC.ZDistance(objDAOC.gPlayerXCoord, objDAOC.gPlayerYCoord, objDAOC.gPlayerZCoord, X, Y, Z)) < Range Then
            objDAOC.StartRunning()

            While objDAOC.ZDistance(objDAOC.gPlayerXCoord, objDAOC.gPlayerYCoord, objDAOC.gPlayerZCoord, X, Y, Z) > Range _
                And Not objDAOC.IsPlayerDead()

                objDAOC.TurnToHeading(objDAOC.FindHeading(objDAOC.gPlayerXCoord, objDAOC.gPlayerYCoord, X, Y))
                gthdStartXp.Sleep(100)
            End While

            objDAOC.StopRunning()
        End If
    End Sub

    Private Sub MoveToPullSpot(ByVal objDAOC As AutoKillerScript.clsAutoKillerScript, _
                                  ByVal MobId As Short, ByVal PullRange As Integer)

        If Not (objDAOC.ZDistance(objDAOC.gPlayerXCoord, objDAOC.gPlayerYCoord, objDAOC.gPlayerZCoord, _
                objDAOC.MobXCoord(MobId), objDAOC.MobYCoord(MobId), objDAOC.MobZCoord(MobId)) < PullRange) Then

            objDAOC.StartRunning()

            While objDAOC.ZDistance(objDAOC.gPlayerXCoord, objDAOC.gPlayerYCoord, objDAOC.gPlayerZCoord, _
                  objDAOC.MobXCoord(MobId), objDAOC.MobYCoord(MobId), objDAOC.MobZCoord(MobId)) > PullRange _
                  And Not objDAOC.IsPlayerDead()

                objDAOC.TurnToHeading(objDAOC.FindHeading(objDAOC.gPlayerXCoord, objDAOC.gPlayerYCoord, _
                                       objDAOC.MobXCoord(MobId), objDAOC.MobYCoord(MobId)))
                gthdStartXp.Sleep(75)
            End While

            objDAOC.StopRunning()
        End If

    End Sub

    Private Sub MoveToBase(ByVal objDAOC As AutoKillerScript.clsAutoKillerScript)
        'put in settings file, have button for setbase, blah blah

        Const dlbBaseX As Double = 506879
        Const dlbBaseY As Double = 511883
        Const dlbBaseZ As Double = 5372

        'by tree
        'Const dlbBaseX As Double = 506995
        'Const dlbBaseY As Double = 512245
        'Const dlbBaseZ As Double = 5356

        'Const dlbBaseX As Double = 504588
        'Const dlbBaseY As Double = 509449
        'Const dlbBaseZ As Double = 4981

        MoveToGXY(objDAOC, dlbBaseX, dlbBaseY, dlbBaseZ, 150)
    End Sub

    Private Sub GetLoot(ByVal objDAOC As AutoKillerScript.clsAutoKillerScript)
        Dim keyF7 As Byte = &H76
        Dim keyG As Byte = &H47

        Dim intCount As Integer

        For intCount = 1 To 11
            objDAOC.SendKeys(keyF7, 0)
            gthdStartXp.Sleep(300)
            objDAOC.SendKeys(keyG, 0)
            gthdStartXp.Sleep(300)
        Next intCount
    End Sub

    Private Sub SendMail()
        Dim objMail As CDO.Message
        objMail = New CDO.Message

        objMail.From = "vpr-matrix"
        objMail.To = "4024325408@message.alltel.net"
        objMail.Subject = "Wishingtree croaked!"
        objMail.TextBody = "Wishingtree croaked!  Need restart!"
        objMail.Send()

    End Sub

    Private Sub frmMain_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Not IsNothing(gthdStartXp) Then
            btnStartStop_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub TabPage1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Timer1_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles Timer1.Elapsed
        gblnPull = True
    End Sub

    Private Sub Timer2_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles Timer2.Elapsed
        gblnDmgShld = True
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim objdaoc As AutoKillerScript.clsAutoKillerScript
        objdaoc = New AutoKillerScript.clsAutoKillerScript

        objdaoc.RegKey = "DEREKBEATTIE"
        objdaoc.DoInit()

        'MsgBox(objdaoc.gPlayerXCoord)
        'MsgBox(objdaoc.gPlayerYCoord)
        'MsgBox(objdaoc.gPlayerZCoord)

        Dim shtTempIndex As Short
        shtTempIndex = objdaoc.TargetIndex()
        MsgBox(objdaoc.MobXCoord(shtTempIndex))
        MsgBox(objdaoc.MobYCoord(shtTempIndex))
        MsgBox(objdaoc.MobZCoord(shtTempIndex))


        objdaoc.StopInit()
    End Sub

    Private Sub tmeRelease_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles tmeRelease.Elapsed
        blnRelease = True
    End Sub
End Class
