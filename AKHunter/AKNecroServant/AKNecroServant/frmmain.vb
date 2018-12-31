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
    Shared objAKNecroServant As Threading.Thread

    Shared blnTerminateThread As Boolean = False
    Shared blnRelease As Boolean = False
    Shared blnFightOver As Boolean = False

    Shared strProfile As String = "AKNecroServant.xml"

    Shared bytSitKey As Byte
    Shared bytFaceKey As Byte
    Shared bytIsMobInCombat As Byte

    Shared shtPetId As Short
    Shared shtMobId As Short

    'profile var's
    Shared blnStopAfterDeath As Boolean
    Shared strReleaseText As String

    Shared bytDamageShieldKey As Byte

    Shared dblBaseX As Double
    Shared dblBaseY As Double
    Shared dblBaseZ As Double

    Shared strPetName As String
    Shared intPetLevel As Integer
    Shared bytPetPassiveKey As Byte
    Shared bytPetAttackKey As Byte
    Shared bytPetDefenseKey As Byte


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
    Friend WithEvents btnGetBaseCoords As System.Windows.Forms.Button
    Friend WithEvents btnTargetDistance As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnStartStop = New System.Windows.Forms.Button
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.lbLog = New System.Windows.Forms.ListBox
        Me.tmeRelease = New System.Timers.Timer
        Me.btnGetBaseCoords = New System.Windows.Forms.Button
        Me.btnTargetDistance = New System.Windows.Forms.Button
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
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
        'tmeRelease
        '
        Me.tmeRelease.Interval = 660000
        Me.tmeRelease.SynchronizingObject = Me
        '
        'btnGetBaseCoords
        '
        Me.btnGetBaseCoords.Location = New System.Drawing.Point(240, 288)
        Me.btnGetBaseCoords.Name = "btnGetBaseCoords"
        Me.btnGetBaseCoords.Size = New System.Drawing.Size(104, 23)
        Me.btnGetBaseCoords.TabIndex = 2
        Me.btnGetBaseCoords.Text = "Get Base Coords"
        '
        'btnTargetDistance
        '
        Me.btnTargetDistance.Location = New System.Drawing.Point(136, 288)
        Me.btnTargetDistance.Name = "btnTargetDistance"
        Me.btnTargetDistance.Size = New System.Drawing.Size(96, 23)
        Me.btnTargetDistance.TabIndex = 3
        Me.btnTargetDistance.Text = "Target Distance"
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(432, 320)
        Me.Controls.Add(Me.btnTargetDistance)
        Me.Controls.Add(Me.btnGetBaseCoords)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.btnStartStop)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmMain"
        Me.Text = "AKKeepAlive"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.tmeRelease, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnStartStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartStop.Click
        If IsNothing(objAKNecroServant) Then
            blnTerminateThread = False

            objAKNecroServant = New Threading.Thread(AddressOf AKNecroServant)
            objAKNecroServant.Start()

            btnStartStop.Text = "Stop"
        Else
            Text = "AKNecroServant"

            btnStartStop.Text = "Start"

            blnTerminateThread = True

            objAKNecroServant.Join()
            objAKNecroServant = Nothing
        End If
    End Sub

    Private Sub AKNecroServant()
        Dim objKeys As New DAOCKeyboard(objDAOC, ReadSetting("GamePath", GetType(String)))
        Dim objEncoder As New System.Text.UTF8Encoding

        Dim blnProcessProfile As Boolean = False

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

            'sit key
            bytSitKey = objKeys.Sit_Stand_Key
            bytFaceKey = objKeys.Face_Key

            While Not blnTerminateThread
                If IsDAOCActive() And objDAOC.IsPlayerDead = 0 Then

                    '*********************************************************************************
                    'process profile
                    '*********************************************************************************
                    If Not blnProcessProfile Then
                        blnProcessProfile = True

                        'get the pets id
                        shtPetId = FindMob(objDAOC, strPetName, intPetLevel, intPetLevel, 1000)
                        LogLineAsync("Pet ID:: " & shtPetId)
                        LogLineAsync("Pet combat status: " & objDAOC.isMobInCombat(shtPetId))
                        LogLineAsync("Shade combat status: " & objDAOC.isPlayerInCombat())

                        'set pet to defense
                        objDAOC.SendKeys(bytPetDefenseKey, 0)
                        objAKNecroServant.Sleep(100)
                        LogLineAsync("Setting pet to defense")

                        'add string for ds time out
                        objDAOC.AddString(0, "The bone spikes fade from abomination.*")


                    End If
                    '************************************************

                    'find a mob within a range, might add code for checking to see if anymobsareincombat
                    If objDAOC.isMobInCombat(shtPetId) Then
                        'fight logic
                        LogLineAsync("Starting fight logic")

                        blnFightOver = False

                        'cast ds                 
                        objDAOC.SendKeys(bytDamageShieldKey, 0)
                        objAKNecroServant.Sleep(100)
                        LogLineAsync("Casting damage shield")

                        While Not blnFightOver And objDAOC.IsPlayerDead = 0

                            'check for ds fade msg
                            If objDAOC.QueryString(0) = True Then
                                'cast ds                 
                                objDAOC.SendKeys(bytDamageShieldKey, 0)
                                objAKNecroServant.Sleep(100)
                                LogLineAsync("Casting damage shield")
                            End If

                            'check for player death
                            If objDAOC.IsPlayerDead() Then
                                blnFightOver = True
                                blnTerminateThread = True
                            End If

                            'check to see if pet still in combat
                            'bytIsMobInCombat = objDAOC.isMobInCombat(shtPetId)
                            objAKNecroServant.Sleep(300)
                            shtMobId = objDAOC.FindClosestMobInCombat(1500, shtPetId)
                            'If bytIsMobInCombat = 0 Then
                            If shtMobId = -1 Then
                                LogLineAsync("Pet is not in combat")

                                blnFightOver = True
                                LogLineAsync("Fight complete")
                            Else
                                'LogLine("Still mobs in combat")
                            End If

                            objAKNecroServant.Sleep(300)
                        End While


                    Else
                        'pull
                        shtMobId = FindMob(objDAOC, "essence shredder", 49, 70, 1400)

                        If shtMobId <> -1 Then

                            'select the target
                            objDAOC.SelectTarget(shtMobId)

                            'attack
                            objDAOC.SendKeys(bytPetAttackKey, 0)
                            objAKNecroServant.Sleep(100)
                            LogLineAsync("Sending pet to attack")

                            'check pet range to mob
                            Dim dblTempRange As Double

                            dblTempRange = objDAOC.ZDistance(objDAOC.MobXCoord(shtPetId), objDAOC.MobYCoord(shtPetId), objDAOC.MobZCoord(shtPetId), _
                                           objDAOC.MobXCoord(shtMobId), objDAOC.MobYCoord(shtMobId), objDAOC.MobZCoord(shtMobId))

                            LogLineAsync("Pet distance from mob: " & dblTempRange)

                            While dblTempRange > 200 And objDAOC.IsPlayerDead = 0

                                If objDAOC.IsPlayerDead() Then
                                    blnTerminateThread = True
                                    dblTempRange = 0
                                End If

                                dblTempRange = objDAOC.ZDistance(objDAOC.MobXCoord(shtPetId), objDAOC.MobYCoord(shtPetId), objDAOC.MobZCoord(shtPetId), _
                                           objDAOC.MobXCoord(shtMobId), objDAOC.MobYCoord(shtMobId), objDAOC.MobZCoord(shtMobId))

                                LogLineAsync("Pet distance from mob: " & dblTempRange)

                                objAKNecroServant.Sleep(100)
                            End While

                            'set pet to passive now
                            objDAOC.SendKeys(bytPetPassiveKey, 0)
                            objAKNecroServant.Sleep(100)
                            LogLineAsync("Sending pet to passive")

                            'check pet range to shade
                            dblTempRange = objDAOC.ZDistance(objDAOC.gPlayerXCoord, objDAOC.gPlayerYCoord, objDAOC.gPlayerZCoord, _
                                           objDAOC.MobXCoord(shtMobId), objDAOC.MobYCoord(shtMobId), objDAOC.MobZCoord(shtMobId))

                            LogLineAsync("Pet distance from shade: " & dblTempRange)
                            While dblTempRange > 220 And objDAOC.IsPlayerDead = 0

                                If objDAOC.IsPlayerDead() Then
                                    blnTerminateThread = True
                                    dblTempRange = 0
                                End If

                                dblTempRange = objDAOC.ZDistance(objDAOC.gPlayerXCoord, objDAOC.gPlayerYCoord, objDAOC.gPlayerZCoord, _
                                           objDAOC.MobXCoord(shtPetId), objDAOC.MobYCoord(shtPetId), objDAOC.MobZCoord(shtPetId))

                                LogLineAsync("Pet distance from shade: " & dblTempRange)

                                objAKNecroServant.Sleep(100)
                            End While

                            'sleep for 1 seconds to give mobs a chance to get to pet
                            objAKNecroServant.Sleep(2000)

                            'set pet back to defensive
                            objDAOC.SendKeys(bytPetDefenseKey, 0)
                            objAKNecroServant.Sleep(100)
                            LogLineAsync("Setting pet to defense")

                            objAKNecroServant.Sleep(2000)

                        End If


                    End If



                    '************************************************
                Else
                        If objDAOC.IsPlayerDead = 1 Then
                            'player has died, if blnStopAfterDeath is true, player will release and quit
                            Dim blnTempLoop As Boolean = True

                            If blnStopAfterDeath Then
                            LogLineAsync("Player is dead, starting release timer")
                                tmeRelease.Start()

                            While blnTempLoop
                                If blnRelease Then

                                    'stop timer
                                    tmeRelease.Stop()

                                    blnTempLoop = False

                                    LogLineAsync("Time up, sending release line")
                                    objAKNecroServant.Sleep(2000)
                                    objDAOC.SendString(strReleaseText & "~")
                                    objAKNecroServant.Sleep(2000)

                                    LogLineAsync("Sleeping for 1 minute, then sending quit")
                                    objAKNecroServant.Sleep(60000)

                                    objAKNecroServant.Sleep(2000)
                                    objDAOC.SendString("/quit~")
                                    objAKNecroServant.Sleep(2000)

                                    LogLineAsync("Terminating thread")
                                    blnTerminateThread = True
                                End If
                            End While
                        Else
                                'player is dead but stopafterdeath is false
                                blnTerminateThread = True
                            End If
                        End If
                End If 'If IsDAOCActive() AndAlso Not objDAOC.IsPlayerDead Then
            End While
        Catch Ex As Exception
            LogLineAsync("Exception occured! AKNecroServant stopped!")
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
            objAKNecroServant = Nothing

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
                blnStopAfterDeath = xmlDoc.SelectSingleNode("//Profile/Settings/@StopAfterDeath").Value
                strReleaseText = xmlDoc.SelectSingleNode("//Profile/Settings/@ReleaseText").Value
                bytDamageShieldKey = objEncoder.GetBytes(xmlDoc.SelectSingleNode("//Profile/Settings/@DamageShieldKey").Value)(0)

                LogLine("StopAfterDeath: " & blnStopAfterDeath)
                LogLine("ReleaseText: " & strReleaseText)
                LogLine("DamageShieldKey: " & xmlDoc.SelectSingleNode("//Profile/Settings/@DamageShieldKey").Value)

                'Base Coords
                dblBaseX = xmlDoc.SelectSingleNode("//Profile/BaseCoords/@X").Value
                dblBaseY = xmlDoc.SelectSingleNode("//Profile/BaseCoords/@Y").Value
                dblBaseZ = xmlDoc.SelectSingleNode("//Profile/BaseCoords/@Z").Value

                LogLine("BaseCoords X: " & dblBaseX)
                LogLine("BaseCoords Y: " & dblBaseY)
                LogLine("BaseCoords Z: " & dblBaseZ)

                'Pet
                strPetName = xmlDoc.SelectSingleNode("//Profile/Pet/@Name").Value
                intPetLevel = xmlDoc.SelectSingleNode("//Profile/Pet/@Level").Value
                bytPetPassiveKey = objEncoder.GetBytes(xmlDoc.SelectSingleNode("//Profile/Pet/@PassiveKey").Value)(0)
                bytPetAttackKey = objEncoder.GetBytes(xmlDoc.SelectSingleNode("//Profile/Pet/@AttackKey").Value)(0)
                bytPetDefenseKey = objEncoder.GetBytes(xmlDoc.SelectSingleNode("//Profile/Pet/@DefenseKey").Value)(0)

                LogLine("PetName: " & strPetName)
                LogLine("PetLevel: " & intPetLevel)
                LogLine("PetPassiveKey: " & xmlDoc.SelectSingleNode("//Profile/Pet/@PassiveKey").Value)
                LogLine("PetAttackKey: " & xmlDoc.SelectSingleNode("//Profile/Pet/@AttackKey").Value)
                LogLine("PetDefenseKey: " & xmlDoc.SelectSingleNode("//Profile/Pet/@DefenseKey").Value)

                LogLine("--------------------------------------------------------------------------------")
            Catch Ex As Exception
                LogLineAsync(Ex.Message)
            End Try
        End If
    End Sub

    Public Delegate Sub UpdateText(ByVal aTextBox As Object, ByRef Text As Object)

    Public Delegate Sub LogIt(ByVal Line As String)

    Public Sub UpdateTextBox(ByVal aTextBox As Object, ByRef Text As Object)
        If TypeName(aTextBox) = "TextBox" Then
            CType(aTextBox, TextBox).Text = CStr(Text)
        ElseIf TypeName(aTextBox) = "Button" Then
            CType(aTextBox, Button).Text = CStr(Text)
        End If
    End Sub

    Sub LogLineAsync(ByVal line As String)
        Dim mi As New LogIt(AddressOf LogLine)
        Me.BeginInvoke(mi, New Object() {line})
    End Sub

    Sub LogLine(ByVal Line As String)
        Line = Format(Year(Now), "0000") & "-" & Format(Month(Now), "00") & "-" & Format(Microsoft.VisualBasic.Day(Now), "00") & "|" & Format(Hour(Now), "00") & ":" & Format(Minute(Now), "00") & ":" & Format(Second(Now), "00") & "| " & Line
        lbLog.BeginUpdate()
        lbLog.Items.Insert(0, Line)

        If lbLog.Items.Count > 128 Then
            lbLog.Items.RemoveAt(127)
        End If
        lbLog.EndUpdate()
    End Sub

    Private Function FindMob(ByVal objDAOC As AutoKillerScript.clsAutoKillerScript, _
                             ByVal MobName As String, _
                             ByVal MinLevel As Integer, ByVal MaxLevel As Integer, _
                             ByVal Range As Integer) As Short

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
            Me.BeginInvoke(mi, New Object() {"AKNecroServant (Active)"})
            'Text = "AKNecroServant (Active)"

            Return True
        Else
            Dim mi As New UpdateTitle(AddressOf UpdateFormTitle)
            Me.BeginInvoke(mi, New Object() {"AKNecroServant (Paused)"})
            'Text = "AKNecroServant (Active)"

            Return False
        End If
    End Function

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "AKNecroServant"

        LogLine("AKNecroServant v0.01 (c) Version125")
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
        If Not IsNothing(objAKNecroServant) Then
            btnStartStop_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub tmeRelease_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles tmeRelease.Elapsed
        blnRelease = True
    End Sub

    Private Sub btnGetBaseCoords_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetBaseCoords.Click
        Dim objDAOC As AutoKillerScript.clsAutoKillerScript
        objDAOC = New AutoKillerScript.clsAutoKillerScript

        Dim intPlayerXCoord As Integer
        Dim intPlayerYCoord As Integer
        Dim intPlayerZCoord As Integer

        Dim strCoords As String

        objDAOC.RegKey = "DEREKBEATTIE"
        objDAOC.DoInit()

        intPlayerXCoord = objDAOC.gPlayerXCoord
        intPlayerYCoord = objDAOC.gPlayerYCoord
        intPlayerZCoord = objDAOC.gPlayerZCoord
        strCoords = "BaseCoords X=" & intPlayerXCoord & " Y=" & intPlayerYCoord & " Z=" & intPlayerZCoord
        LogLineAsync(strCoords)

        objDAOC.StopInit()
        objDAOC = Nothing
    End Sub

    Private Sub btnTargetDistance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTargetDistance.Click
        Dim objDAOC As AutoKillerScript.clsAutoKillerScript
        objDAOC = New AutoKillerScript.clsAutoKillerScript

        Dim intPlayerXCoord As Integer
        Dim intPlayerYCoord As Integer
        Dim intPlayerZCoord As Integer

        Dim intMobXCoord As Integer
        Dim intMobYCoord As Integer
        Dim intMobZCoord As Integer

        Dim dblDistance As Double

        objDAOC.RegKey = "DEREKBEATTIE"
        objDAOC.DoInit()

        dblDistance = objDAOC.ZDistance(objDAOC.gPlayerXCoord, objDAOC.gPlayerYCoord, objDAOC.gPlayerZCoord, _
                               objDAOC.MobXCoord(objDAOC.TargetIndex()), objDAOC.MobYCoord(objDAOC.TargetIndex()), objDAOC.MobZCoord(objDAOC.TargetIndex()))

        LogLineAsync(CStr(dblDistance))

        objDAOC.StopInit()
        objDAOC = Nothing
    End Sub
End Class

