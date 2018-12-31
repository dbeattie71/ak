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

    Private WithEvents objWindowManager As DAOCWindowManager
    Private objDAOC As AutoKillerScript.clsAutoKillerScript
    Shared objAKNecro As Threading.Thread

    Shared blnTerminateThread As Boolean = False
    Shared blnAbsorb As Boolean = False
    Shared blnRelease As Boolean = False

    Shared strProfile As String

    Shared bytSitKey As Byte
    Shared bytFaceKey As Byte
    Shared bytIsTargetDead As Byte = 0

    'profile var's
    Shared blnStopAfterDeath As Boolean
    Shared strReleaseText As String
    Shared blnStopAfterTargetDeath As Boolean
    Shared blnSitToRegainPower As Boolean
    Shared bytPetPassiveKey As Byte

    Shared blnAbsorbEnabled As Boolean
    Shared bytAbsorbKey As Byte
    Shared intAbsorbDelay As Integer
    Shared intAbsorbTimer As Integer

    Shared colAbsorbTargets As Collection = New Collection

    Shared blnPowerTransferEnabled As Boolean
    Shared bytPowerTransferKey As Byte
    Shared intPowerTransferDelay As Integer
    Shared intLowerPowerTransferThreshold As Integer
    Shared intUpperPowerTransferThreshold As Integer

    Shared htPowerTransferTargets As Hashtable = New Hashtable

    Shared blnPowerDrainEnabled As Boolean
    Shared bytPowerDrainKey As Byte
    Shared intPowerDrainDelay As Integer
    Shared strPowerDrainTarget As String
    Shared intPowerDrainThreshold As Integer

    Shared shtPowerDrainTargetIndex As Short
#End Region
#Region " Classes "
    Class clsAbsorbTarget
        Public Name As String
        Public Index As Short
    End Class

    Class clsPowerTransferTarget
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
    Friend WithEvents tmeAbsorb As System.Timers.Timer
    Friend WithEvents tmeRelease As System.Timers.Timer
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnStartStop = New System.Windows.Forms.Button
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.lbLog = New System.Windows.Forms.ListBox
        Me.tmeAbsorb = New System.Timers.Timer
        Me.tmeRelease = New System.Timers.Timer
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.tmeAbsorb, System.ComponentModel.ISupportInitialize).BeginInit()
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
        'tmeAbsorb
        '
        Me.tmeAbsorb.SynchronizingObject = Me
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
        Me.Text = "AKNecro"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.tmeAbsorb, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tmeRelease, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnStartStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartStop.Click
        If IsNothing(objAKNecro) Then
            blnTerminateThread = False
            blnAbsorb = True

            objAKNecro = New Threading.Thread(AddressOf AKNecro)
            objAKNecro.Start()

            btnStartStop.Text = "Stop"
        Else
            Text = "AKNecro"

            btnStartStop.Text = "Start"

            blnTerminateThread = True

            objAKNecro.Join()
            objAKNecro = Nothing
        End If
    End Sub

    Private Sub AKNecro()
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

            strProfile = ReadSetting("Profile", GetType(String))

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

            'set timer intervals
            tmeAbsorb.Stop()
            tmeAbsorb.Interval = intAbsorbTimer

            While Not blnTerminateThread
                If IsDAOCActive() And objDAOC.IsPlayerDead = 0 And bytIsTargetDead = 0 Then

                    '*********************************************************************************
                    'process profile
                    '*********************************************************************************
                    If Not blnProcessProfile Then
                        blnProcessProfile = True

                        'check to see if absorbing is enabled
                        If blnAbsorbEnabled Then
                            'iterate collection and get id's
                            Dim objAbsorbTarget As clsAbsorbTarget

                            For Each objAbsorbTarget In colAbsorbTargets
                                objAbsorbTarget.Index = objDAOC.SetTarget(objAbsorbTarget.Name, , False)

                                LogLine("Adding index of " & objAbsorbTarget.Index & " to absorb collection for " & _
                                    objAbsorbTarget.Name)
                            Next
                        End If

                        'check to see if PowerTransfering is enabled
                        If blnPowerTransferEnabled Then
                            'iterate hastable and get id's

                            Dim objPowerTransferTarget As clsPowerTransferTarget

                            For Each objPowerTransferTarget In htPowerTransferTargets.Values
                                objPowerTransferTarget.Index = objDAOC.SetTarget(objPowerTransferTarget.Name, , False)

                                LogLine("Adding index of " & objPowerTransferTarget.Index & " to PowerTransferTarget hastable for " & _
                                    objPowerTransferTarget.Name)
                            Next
                        End If

                        'check to see if PowerDraining is enabled
                        If blnPowerDrainEnabled Then
                            shtPowerDrainTargetIndex = objDAOC.SetTarget(strPowerDrainTarget, , False)
                            objDAOC.AddString(0, strPowerDrainTarget & " resists the effect!*")
                        End If
                    End If

                    '*********************************************************************************
                    'absorb logic
                    '*********************************************************************************
                    If blnAbsorb And blnAbsorbEnabled And objDAOC.IsPlayerDead = 0 And bytIsTargetDead = 0 Then
                        'iterate collection and cast abs
                        Dim objAbsorbTarget As clsAbsorbTarget

                        'see if player is sitting
                        If objDAOC.isPlayerSitting = 1 Then
                            LogLine("Player is sitting, stand")
                            objAKNecro.Sleep(1500)
                            objDAOC.SendKeys(bytSitKey, 0)
                            objAKNecro.Sleep(1500)
                        End If

                        tmeAbsorb.Stop()

                        For Each objAbsorbTarget In colAbsorbTargets
                            If blnStopAfterTargetDeath Then
                                If objDAOC.MobHealth(objabsorbtarget.Index) = 0 Then
                                    bytIsTargetDead = 1
                                End If
                            End If

                            'set pet to passive
                            objDAOC.SendKeys(bytPetPassiveKey, 0)
                            objAKNecro.Sleep(100)

                            LogLine("Casting absorb on " & objAbsorbTarget.Name)
                            objDAOC.SelectTarget(objabsorbtarget.Index)
                            objDAOC.SendKeys(bytAbsorbKey, 0)
                            objAKNecro.Sleep(intAbsorbDelay)
                            objAKNecro.Sleep(100)
                        Next

                        'see if sitting to regain power is enabled
                        If blnSitToRegainPower Then
                            'see if player is standing
                            If objDAOC.isPlayerSitting = 0 Then
                                LogLine("Player is standing, sit")
                                objAKNecro.Sleep(1500)
                                objDAOC.SendKeys(bytSitKey, 0)
                                objAKNecro.Sleep(1500)
                            End If
                        End If

                        blnAbsorb = False
                        tmeAbsorb.Start()
                    End If

                    '*********************************************************************************
                    'powertranfer logic
                    '*********************************************************************************
                    If blnPowerTransferEnabled And objDAOC.PlayerMana > intUpperPowerTransferThreshold And _
                        objDAOC.IsPlayerDead = 0 And bytIsTargetDead = 0 Then
                        Dim objPowerTransferTarget As clsPowerTransferTarget

                        'see if player is sitting
                        If objDAOC.isPlayerSitting = 1 Then
                            LogLine("Player is sitting, stand")
                            objAKNecro.Sleep(1500)
                            objDAOC.SendKeys(bytSitKey, 0)
                            objAKNecro.Sleep(1500)
                        End If

                        If htPowerTransferTargets.Values.Count > 0 Then
                            'set pet to passive
                            objDAOC.SendKeys(bytPetPassiveKey, 0)
                            objAKNecro.Sleep(100)

                            LogLine(objWindowManager.CharacterName & "'s mana is at " & objDAOC.PlayerMana)

                            While objDAOC.PlayerMana > intLowerPowerTransferThreshold And objDAOC.IsPlayerDead = 0 And _
                                bytIsTargetDead = 0
                                objPowertransfertarget = htPowerTransferTargets(intLastPowerTransfer)

                                If blnStopAfterTargetDeath Then
                                    If objDAOC.MobHealth(objPowertransfertarget.Index) = 0 Then
                                        bytIsTargetDead = 1
                                    End If
                                End If

                                LogLine("Casting powertransfer to " & objPowertransfertarget.Name)

                                objDAOC.SelectTarget(objPowertransfertarget.Index)
                                objAKNecro.Sleep(100)
                                objDAOC.SendKeys(bytPowerTransferKey, 0)
                                objAKNecro.Sleep(intPowerTransferDelay)
                                objAKNecro.Sleep(100)

                                LogLine(objWindowManager.CharacterName & "'s mana is at " & objDAOC.PlayerMana)

                                If htPowerTransferTargets.Values.Count = intLastPowerTransfer + 1 Then
                                    intLastPowerTransfer = 0
                                Else
                                    intLastPowerTransfer = intLastPowerTransfer + 1
                                End If
                            End While
                        End If

                        If blnSitToRegainPower Then
                            'see if player is standing
                            If objDAOC.isPlayerSitting = 0 Then
                                LogLine("Player is standing, sit")
                                objAKNecro.Sleep(1500)
                                objDAOC.SendKeys(bytSitKey, 0)
                                objAKNecro.Sleep(1500)
                            End If
                        End If
                    End If

                    '*********************************************************************************
                    'powerdrain logic
                    '*********************************************************************************
                    If blnStopAfterTargetDeath Then
                        If objDAOC.MobHealth(shtPowerDrainTargetIndex) = 0 Then
                            bytIsTargetDead = 1
                        End If
                    End If

                    If blnPowerDrainEnabled And objDAOC.MobHealth(shtPowerDrainTargetIndex) > intPowerDrainThreshold And _
                        objDAOC.PlayerMana < intUpperPowerTransferThreshold And _
                        objDAOC.IsPlayerDead = 0 And bytIsTargetDead = 0 Then

                        'see if player is sitting
                        If objDAOC.isPlayerSitting = 1 Then
                            LogLine("Player is sitting, stand")
                            objAKNecro.Sleep(1500)
                            objDAOC.SendKeys(bytSitKey, 0)
                            objAKNecro.Sleep(1500)
                        End If

                        'set pet to passive
                        objDAOC.SendKeys(bytPetPassiveKey, 0)
                        objAKNecro.Sleep(100)

                        'select target
                        objDAOC.SelectTarget(shtPowerDrainTargetIndex)
                        objAKNecro.Sleep(100)

                        'face target
                        objDAOC.SendKeys(bytPetPassiveKey, 0)
                        objAKNecro.Sleep(100)

                        LogLine(objWindowManager.CharacterName & "'s mana is at " & objDAOC.PlayerMana)

                        While objDAOC.PlayerMana < intUpperPowerTransferThreshold And _
                            objDAOC.MobHealth(shtPowerDrainTargetIndex) > intPowerDrainThreshold And _
                            objDAOC.IsPlayerDead = 0 And _
                            bytIsTargetDead = 0

                            If blnStopAfterTargetDeath Then
                                If objDAOC.MobHealth(shtPowerDrainTargetIndex) = 0 Then
                                    bytIsTargetDead = 1
                                End If
                            End If

                            Do
                                LogLine("Casting powerdrain on " & strPowerDrainTarget)

                                'cast powerdrain
                                objDAOC.SendKeys(bytPowerDrainKey, 0)
                                objAKNecro.Sleep(intPowerDrainDelay)
                                objAKNecro.Sleep(100)

                                LogLine(objWindowManager.CharacterName & "'s mana is at " & objDAOC.PlayerMana)

                                If objDAOC.QueryString(0) = True Then
                                    blnRecastPowerDrain = True
                                    LogLine(strPowerDrainTarget & " resisted powerdrain, trying again")
                                Else
                                    blnRecastPowerDrain = False
                                End If
                            Loop Until blnRecastPowerDrain = False
                        End While
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
                                    objAKNecro.Sleep(2000)
                                    objDAOC.SendString(strReleaseText & "~")
                                    objAKNecro.Sleep(2000)

                                    LogLine("Sleeping for 1 minute, then sending quit")
                                    objAKNecro.Sleep(60000)

                                    objAKNecro.Sleep(2000)
                                    objDAOC.SendString("/quit~")
                                    objAKNecro.Sleep(2000)

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
                            objAKNecro.Sleep(120000)

                            'still alive, quit
                            If objDAOC.IsPlayerDead = 0 Then
                                LogLine("Still alive, quitting")
                                objAKNecro.Sleep(2000)
                                objDAOC.SendString("/quit~")
                                objAKNecro.Sleep(2000)

                                LogLine("Terminating thread")
                                blnTerminateThread = True
                            End If
                        End If
                    End If
                End If 'If IsDAOCActive() AndAlso Not objDAOC.IsPlayerDead Then
            End While
        Catch Ex As Exception
            LogLineAsync("Exception occured! AKNecro stopped!")
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
            objAKNecro = Nothing

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
                blnStopAfterTargetDeath = xmlDoc.SelectSingleNode("//Profile/Settings/@StopAfterTargetDeath").Value
                blnSitToRegainPower = xmlDoc.SelectSingleNode("//Profile/Settings/@SitToRegainPower").Value
                bytPetPassiveKey = objEncoder.GetBytes(xmlDoc.SelectSingleNode("//Profile/Settings/@PetPassiveKey").Value)(0)
                LogLine("StopAfterDeath: " & blnStopAfterDeath)
                LogLine("ReleaseText: " & strReleaseText)
                LogLine("StopAfterTargetDeath: " & blnStopAfterTargetDeath)
                LogLine("SitToRegainPower: " & blnSitToRegainPower)
                LogLine("PetPassiveKey: " & xmlDoc.SelectSingleNode("//Profile/Settings/@PetPassiveKey").Value)

                'Absorb
                blnAbsorbEnabled = xmlDoc.SelectSingleNode("//Profile/Absorb/@Enabled").Value
                bytAbsorbKey = objEncoder.GetBytes(xmlDoc.SelectSingleNode("//Profile/Absorb/@Key").Value)(0)
                intAbsorbDelay = xmlDoc.SelectSingleNode("//Profile/Absorb/@Delay").Value
                intAbsorbTimer = xmlDoc.SelectSingleNode("//Profile/Absorb/@Timer").Value

                LogLine("AbsorbEnabled: " & blnAbsorbEnabled)
                LogLine("AbsorbKey: " & xmlDoc.SelectSingleNode("//Profile/Absorb/@Key").Value)
                LogLine("AbsorbDelay: " & intAbsorbDelay)
                LogLine("AbsorbTimter: " & intAbsorbTimer)

                Dim objAbsorbTarget As clsAbsorbTarget
                Dim xmlNode As Xml.XmlNode

                For Each xmlNode In xmlDoc.SelectNodes("//Profile/AbsorbTargets/AbsorbTarget")
                    objAbsorbTarget = New clsAbsorbTarget
                    objAbsorbTarget.Name = xmlNode.SelectSingleNode("@Name").Value
                    colAbsorbTargets.Add(objAbsorbTarget)

                    LogLine("AbsorbTarget: Name: " & objAbsorbTarget.Name)
                Next

                'PowerTransfer
                blnPowerTransferEnabled = xmlDoc.SelectSingleNode("//Profile/PowerTransfer/@Enabled").Value
                bytPowerTransferKey = objEncoder.GetBytes(xmlDoc.SelectSingleNode("//Profile/PowerTransfer/@Key").Value)(0)
                intPowerTransferDelay = xmlDoc.SelectSingleNode("//Profile/PowerTransfer/@Delay").Value
                intLowerPowerTransferThreshold = xmlDoc.SelectSingleNode("//Profile/PowerTransfer/@LowerThreshold").Value
                intUpperPowerTransferThreshold = xmlDoc.SelectSingleNode("//Profile/PowerTransfer/@UpperThreshold").Value

                LogLine("PowerTransferEnabled: " & blnPowerTransferEnabled)
                LogLine("PowerTransferKey: " & xmlDoc.SelectSingleNode("//Profile/PowerTransfer/@Key").Value)
                LogLine("PowerTransferDelay: " & intPowerTransferDelay)
                LogLine("LowerPowerTransferThreshold: " & intLowerPowerTransferThreshold)
                LogLine("UpperPowerTransferThreshold: " & intUpperPowerTransferThreshold)

                Dim objPowerTransferTarget As clsPowerTransferTarget
                Dim intCount As Integer = 0

                For Each xmlNode In xmlDoc.SelectNodes("//Profile/PowerTransferTargets/PowerTransferTarget")
                    objPowerTransferTarget = New clsPowerTransferTarget
                    objPowerTransferTarget.Name = xmlNode.SelectSingleNode("@Name").Value
                    htPowerTransferTargets.Add(intCount, objPowerTransferTarget)

                    LogLine("PowerTransferTarget: Name: " & objPowerTransferTarget.Name)

                    intCount = intCount + 1
                Next

                blnPowerDrainEnabled = xmlDoc.SelectSingleNode("//Profile/PowerDrain/@Enabled").Value
                bytPowerDrainKey = objEncoder.GetBytes(xmlDoc.SelectSingleNode("//Profile/PowerDrain/@Key").Value)(0)
                intPowerDrainDelay = xmlDoc.SelectSingleNode("//Profile/PowerDrain/@Delay").Value
                intPowerDrainThreshold = xmlDoc.SelectSingleNode("//Profile/PowerDrain/@Threshold").Value

                LogLine("PowerDrainEnabled: " & blnPowerDrainEnabled)
                LogLine("PowerDrainTransferKey: " & xmlDoc.SelectSingleNode("//Profile/PowerDrain/@Key").Value)
                LogLine("PowerDrainDelay: " & intPowerDrainDelay)
                LogLine("PowerDrainThreshold: " & intPowerDrainThreshold)

                strPowerDrainTarget = xmlDoc.SelectSingleNode("//Profile/PowerDrainTarget/@Name").Value
                LogLine("PowerDrainTarget: " & strPowerDrainTarget)

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
            Me.BeginInvoke(mi, New Object() {"AKNecro (Active)"})
            'Text = "AKNecro (Active)"

            Return True
        Else
            Dim mi As New UpdateTitle(AddressOf UpdateFormTitle)
            Me.BeginInvoke(mi, New Object() {"AKNecro (Paused)"})
            'Text = "AKNecro (Active)"

            Return False
        End If
    End Function

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "AKNecro, tap dat azz!"

        LogLine("AKNecro v0.01 (c) Version125, tap dat azz!")

        'had to comment out for now
        'Try
        '    System.Diagnostics.Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.AboveNormal
        'Catch Ex As Exception
        '    ' On Windows 98 machines you get an error for 'above normal'
        '    System.Diagnostics.Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.High
        'End Try

        Threading.Thread.CurrentThread.Name = "MainThread"
    End Sub

    Private Sub frmMain_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Not IsNothing(objAKNecro) Then
            btnStartStop_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub tmeAbsorb_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles tmeAbsorb.Elapsed
        blnAbsorb = True
    End Sub

    Private Sub tmeRelease_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles tmeRelease.Elapsed
        blnRelease = True
    End Sub
End Class

