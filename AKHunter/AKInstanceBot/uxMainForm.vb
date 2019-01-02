Option Explicit On 

Imports Microsoft.Win32

Imports System.IO
Imports System.Threading
Imports System.Xml

Imports AutoKillerScript

Namespace AKInstanceBot

    Public Class frmMain
        Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        Friend WithEvents uxMainTabControl As System.Windows.Forms.TabControl
        Friend WithEvents uxLogTabPage As System.Windows.Forms.TabPage
        Friend WithEvents lbLog As System.Windows.Forms.ListBox
        Friend WithEvents uxOptionsTabPage As System.Windows.Forms.TabPage
        Friend WithEvents Panel4 As System.Windows.Forms.Panel
        Friend WithEvents uxRegKeyLabel As System.Windows.Forms.Label
        Friend WithEvents uxRegKeyTextBox As System.Windows.Forms.TextBox
        Friend WithEvents uxCatacombsTextBox As System.Windows.Forms.TextBox
        Friend WithEvents uxPathsLabel As System.Windows.Forms.Label
        Friend WithEvents uxCatacombsLabel As System.Windows.Forms.Label
        Friend WithEvents Panel3 As System.Windows.Forms.Panel
        Friend WithEvents uxStartButton As System.Windows.Forms.Button
        Friend WithEvents uxStopButton As System.Windows.Forms.Button
        Friend WithEvents uxPauseButton As System.Windows.Forms.Button
        Friend WithEvents uxInitializeButton As System.Windows.Forms.Button
        Friend WithEvents uxPatrolsTabPage As System.Windows.Forms.TabPage
        Friend WithEvents Button1 As System.Windows.Forms.Button
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.uxMainTabControl = New System.Windows.Forms.TabControl
            Me.uxLogTabPage = New System.Windows.Forms.TabPage
            Me.lbLog = New System.Windows.Forms.ListBox
            Me.uxOptionsTabPage = New System.Windows.Forms.TabPage
            Me.Panel4 = New System.Windows.Forms.Panel
            Me.uxRegKeyLabel = New System.Windows.Forms.Label
            Me.uxRegKeyTextBox = New System.Windows.Forms.TextBox
            Me.uxCatacombsTextBox = New System.Windows.Forms.TextBox
            Me.uxPathsLabel = New System.Windows.Forms.Label
            Me.uxCatacombsLabel = New System.Windows.Forms.Label
            Me.uxPatrolsTabPage = New System.Windows.Forms.TabPage
            Me.Panel3 = New System.Windows.Forms.Panel
            Me.Button1 = New System.Windows.Forms.Button
            Me.uxPauseButton = New System.Windows.Forms.Button
            Me.uxInitializeButton = New System.Windows.Forms.Button
            Me.uxStartButton = New System.Windows.Forms.Button
            Me.uxStopButton = New System.Windows.Forms.Button
            Me.uxMainTabControl.SuspendLayout()
            Me.uxLogTabPage.SuspendLayout()
            Me.uxOptionsTabPage.SuspendLayout()
            Me.Panel4.SuspendLayout()
            Me.Panel3.SuspendLayout()
            Me.SuspendLayout()
            '
            'uxMainTabControl
            '
            Me.uxMainTabControl.Controls.Add(Me.uxLogTabPage)
            Me.uxMainTabControl.Controls.Add(Me.uxOptionsTabPage)
            Me.uxMainTabControl.Controls.Add(Me.uxPatrolsTabPage)
            Me.uxMainTabControl.Location = New System.Drawing.Point(8, 8)
            Me.uxMainTabControl.Name = "uxMainTabControl"
            Me.uxMainTabControl.SelectedIndex = 0
            Me.uxMainTabControl.Size = New System.Drawing.Size(520, 270)
            Me.uxMainTabControl.TabIndex = 1
            '
            'uxLogTabPage
            '
            Me.uxLogTabPage.Controls.Add(Me.lbLog)
            Me.uxLogTabPage.Location = New System.Drawing.Point(4, 22)
            Me.uxLogTabPage.Name = "uxLogTabPage"
            Me.uxLogTabPage.Size = New System.Drawing.Size(512, 244)
            Me.uxLogTabPage.TabIndex = 0
            Me.uxLogTabPage.Text = "Log"
            Me.uxLogTabPage.UseVisualStyleBackColor = True
            '
            'lbLog
            '
            Me.lbLog.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lbLog.HorizontalScrollbar = True
            Me.lbLog.Location = New System.Drawing.Point(0, 0)
            Me.lbLog.Name = "lbLog"
            Me.lbLog.Size = New System.Drawing.Size(512, 238)
            Me.lbLog.TabIndex = 1
            '
            'uxOptionsTabPage
            '
            Me.uxOptionsTabPage.Controls.Add(Me.Panel4)
            Me.uxOptionsTabPage.Location = New System.Drawing.Point(4, 22)
            Me.uxOptionsTabPage.Name = "uxOptionsTabPage"
            Me.uxOptionsTabPage.Size = New System.Drawing.Size(512, 244)
            Me.uxOptionsTabPage.TabIndex = 1
            Me.uxOptionsTabPage.Text = "Options"
            Me.uxOptionsTabPage.UseVisualStyleBackColor = True
            Me.uxOptionsTabPage.Visible = False
            '
            'Panel4
            '
            Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.Panel4.Controls.Add(Me.uxRegKeyLabel)
            Me.Panel4.Controls.Add(Me.uxRegKeyTextBox)
            Me.Panel4.Controls.Add(Me.uxCatacombsTextBox)
            Me.Panel4.Controls.Add(Me.uxPathsLabel)
            Me.Panel4.Controls.Add(Me.uxCatacombsLabel)
            Me.Panel4.Location = New System.Drawing.Point(8, 6)
            Me.Panel4.Name = "Panel4"
            Me.Panel4.Size = New System.Drawing.Size(496, 232)
            Me.Panel4.TabIndex = 2
            '
            'uxRegKeyLabel
            '
            Me.uxRegKeyLabel.Location = New System.Drawing.Point(8, 56)
            Me.uxRegKeyLabel.Name = "uxRegKeyLabel"
            Me.uxRegKeyLabel.Size = New System.Drawing.Size(56, 23)
            Me.uxRegKeyLabel.TabIndex = 8
            Me.uxRegKeyLabel.Text = "Reg Key:"
            '
            'uxRegKeyTextBox
            '
            Me.uxRegKeyTextBox.Location = New System.Drawing.Point(72, 55)
            Me.uxRegKeyTextBox.Name = "uxRegKeyTextBox"
            Me.uxRegKeyTextBox.Size = New System.Drawing.Size(176, 20)
            Me.uxRegKeyTextBox.TabIndex = 7
            '
            'uxCatacombsTextBox
            '
            Me.uxCatacombsTextBox.Location = New System.Drawing.Point(72, 29)
            Me.uxCatacombsTextBox.Name = "uxCatacombsTextBox"
            Me.uxCatacombsTextBox.Size = New System.Drawing.Size(176, 20)
            Me.uxCatacombsTextBox.TabIndex = 2
            '
            'uxPathsLabel
            '
            Me.uxPathsLabel.Location = New System.Drawing.Point(8, 8)
            Me.uxPathsLabel.Name = "uxPathsLabel"
            Me.uxPathsLabel.Size = New System.Drawing.Size(100, 23)
            Me.uxPathsLabel.TabIndex = 1
            Me.uxPathsLabel.Text = "Paths"
            '
            'uxCatacombsLabel
            '
            Me.uxCatacombsLabel.Location = New System.Drawing.Point(8, 32)
            Me.uxCatacombsLabel.Name = "uxCatacombsLabel"
            Me.uxCatacombsLabel.Size = New System.Drawing.Size(72, 23)
            Me.uxCatacombsLabel.TabIndex = 0
            Me.uxCatacombsLabel.Text = "Catacombs:"
            '
            'uxPatrolsTabPage
            '
            Me.uxPatrolsTabPage.Location = New System.Drawing.Point(4, 22)
            Me.uxPatrolsTabPage.Name = "uxPatrolsTabPage"
            Me.uxPatrolsTabPage.Size = New System.Drawing.Size(512, 244)
            Me.uxPatrolsTabPage.TabIndex = 2
            Me.uxPatrolsTabPage.Text = "Patrols"
            Me.uxPatrolsTabPage.UseVisualStyleBackColor = True
            '
            'Panel3
            '
            Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.Panel3.Controls.Add(Me.Button1)
            Me.Panel3.Controls.Add(Me.uxPauseButton)
            Me.Panel3.Controls.Add(Me.uxInitializeButton)
            Me.Panel3.Controls.Add(Me.uxStartButton)
            Me.Panel3.Controls.Add(Me.uxStopButton)
            Me.Panel3.Location = New System.Drawing.Point(8, 288)
            Me.Panel3.Name = "Panel3"
            Me.Panel3.Size = New System.Drawing.Size(520, 40)
            Me.Panel3.TabIndex = 9
            '
            'Button1
            '
            Me.Button1.Location = New System.Drawing.Point(112, 8)
            Me.Button1.Name = "Button1"
            Me.Button1.Size = New System.Drawing.Size(75, 23)
            Me.Button1.TabIndex = 7
            Me.Button1.Text = "Button1"
            '
            'uxPauseButton
            '
            Me.uxPauseButton.Enabled = False
            Me.uxPauseButton.Location = New System.Drawing.Point(440, 8)
            Me.uxPauseButton.Name = "uxPauseButton"
            Me.uxPauseButton.Size = New System.Drawing.Size(72, 24)
            Me.uxPauseButton.TabIndex = 6
            Me.uxPauseButton.Text = "Pause"
            '
            'uxInitializeButton
            '
            Me.uxInitializeButton.Location = New System.Drawing.Point(200, 8)
            Me.uxInitializeButton.Name = "uxInitializeButton"
            Me.uxInitializeButton.Size = New System.Drawing.Size(72, 24)
            Me.uxInitializeButton.TabIndex = 5
            Me.uxInitializeButton.Text = "Initialize"
            '
            'uxStartButton
            '
            Me.uxStartButton.Enabled = False
            Me.uxStartButton.Location = New System.Drawing.Point(280, 8)
            Me.uxStartButton.Name = "uxStartButton"
            Me.uxStartButton.Size = New System.Drawing.Size(72, 24)
            Me.uxStartButton.TabIndex = 2
            Me.uxStartButton.Text = "Start"
            '
            'uxStopButton
            '
            Me.uxStopButton.Enabled = False
            Me.uxStopButton.Location = New System.Drawing.Point(360, 8)
            Me.uxStopButton.Name = "uxStopButton"
            Me.uxStopButton.Size = New System.Drawing.Size(72, 24)
            Me.uxStopButton.TabIndex = 4
            Me.uxStopButton.Text = "Stop"
            '
            'frmMain
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(538, 338)
            Me.Controls.Add(Me.Panel3)
            Me.Controls.Add(Me.uxMainTabControl)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
            Me.Name = "frmMain"
            Me.Text = "AKInstanceBot"
            Me.uxMainTabControl.ResumeLayout(False)
            Me.uxLogTabPage.ResumeLayout(False)
            Me.uxOptionsTabPage.ResumeLayout(False)
            Me.Panel4.ResumeLayout(False)
            Me.Panel4.PerformLayout()
            Me.Panel3.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Variables "

        Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
        Private Declare Function GetForegroundWindow Lib "user32" Alias "GetForegroundWindow" () As Integer

        Private _Engine As System.Threading.Thread
        Private _EngineStarted As Boolean = False
        Private _Running As Boolean

        Private _Ak As AutoKillerScript.clsAutoKillerScript
        Private _DAoC_BaseBot As DAoC_BaseBot

#End Region

        Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Dim regKey As RegistryKey

            Try
                LogLine("AKInstanceBot v1.0 (c) Version125")

                'load saved setting from registry
                'get registry info
                regKey = Registry.LocalMachine.OpenSubKey("Software\AKFarm")

                'key doesn't exist, make it
                If regKey Is Nothing Then
                    regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE", True)
                    regKey.CreateSubKey("AKFarm")
                    'regKey.Close()
                End If

                'form position
                Me.Top = regKey.GetValue("FormTop", 22)
                Me.Left = regKey.GetValue("FormLeft", 22)

                'options
                uxCatacombsTextBox.Text = regKey.GetValue("CatacombsPath", "c:\mythic\catacombs")
                uxRegKeyTextBox.Text = regKey.GetValue("RegKey", "DEREKBEATTIE1249")

                If uxCatacombsTextBox.Text = "" Then uxCatacombsTextBox.Text = "c:\mythic\catacombs"
                If uxRegKeyTextBox.Text = "" Then uxRegKeyTextBox.Text = ""

                '''''''''''''''''

                _Ak = New AutoKillerScript.clsAutoKillerScript
                'load profile
                'load patrolpoints

                _Running = False
                _Engine = New Thread(New ThreadStart(AddressOf Engine))
                '_Engine.Priority = ThreadPriority.AboveNormal
                _Engine.Priority = ThreadPriority.Normal
                _EngineStarted = True
                _Engine.Start()
                Thread.Sleep(0)
                'Thread.CurrentThread.Priority = ThreadPriority.BelowNormal

            Catch ex As Exception
                LogLine(ex.Message)
            End Try

        End Sub

        Private Sub frmMain_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

            Dim regKey As RegistryKey

            Try

                regKey = Registry.LocalMachine.OpenSubKey("Software\AKFarm", True)

                'form position
                regKey.SetValue("FormTop", Me.Top)
                regKey.SetValue("FormLeft", Me.Left)

                'options
                regKey.SetValue("CatacombsPath", uxCatacombsTextBox.Text)
                regKey.SetValue("RegKey", uxRegKeyTextBox.Text)

                regKey.Close()

                btnStop_Click(Nothing, Nothing)

            Catch ex As Exception
                LogLine(ex.Message)
            End Try

        End Sub

        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)

            If disposing Then

                If Not (components Is Nothing) Then
                    components.Dispose()
                End If

                If (_EngineStarted) Then

                    _EngineStarted = False
                    _Engine.Join()

                End If

            End If

            MyBase.Dispose(disposing)

        End Sub

        Private Sub btnInitialize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxInitializeButton.Click

            Try
                _DAoC_BaseBot = Nothing

                LogLine("RegKey:" & uxRegKeyTextBox.Text)
                LogLine("GamePath:" & uxCatacombsTextBox.Text)

                AddHandler _Ak.OnLog, AddressOf LogLine

                _Ak.RegKey = uxRegKeyTextBox.Text
                _Ak.GamePath = "D:\Program Files (x86)\Electronic Arts\Dark Age of Camelot Yggdrasil" 'uxCatacombsTextBox.Text
                _Ak.EnableCatacombs = False
                _Ak.EnableAutoQuery = True
                _Ak.UseRegEx = True

                '_Ak.AddString(0, "experience points")
                '_Ak.AddString(1, " kills the ")
                '_Ak.AddString(2, "You have died")
                '_Ak.AddString(3, "You can't see your target")
                '_Ak.AddString(4, "can't see its target!")
                '_Ak.AddString(5, "You can't cast while sitting")
                '_Ak.AddString(6, "\\@\\@")
                '_Ak.AddString(7, "You prepare your shot")
                '_Ak.AddString(8, "You miss")
                '_Ak.AddString(9, "You move and interrupt")
                '_Ak.AddString(10, "You shoot")
                '_Ak.AddString(11, "Your servant is too far away from you ")
                '_Ak.AddString(12, " you block the blow")
                '_Ak.AddString(13, "You hit ")
                '_Ak.AddString(14, "You attack ")
                '_Ak.AddString(15, "You fumble ")
                '_Ak.AddString(16, "You prepare to perform ")

                '_Ak.AddString(0, "Your target is not visible!")
                '_Ak.AddString(1, "Your target is not visible.  The spell fails.")
                '_Ak.AddString(2, "You can't see your target from here!")

                'magic pulling - thane
                _Ak.AddString(0, "You hit the ")

                'thrown wep pulling - warr/zerker
                _Ak.AddString(1, "You shoot the ")
                _Ak.AddString(2, "You miss")

                _Ak.DoInit()

                uxInitializeButton.Enabled = False
                uxStartButton.Enabled = True
                uxStopButton.Enabled = False
                uxPauseButton.Enabled = False

                _Running = True

            Catch ex As Exception
                LogLine(ex.Message)

            End Try

        End Sub

        Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxStartButton.Click

            uxStartButton.Enabled = False
            uxStopButton.Enabled = True
            uxPauseButton.Enabled = True

            'when done and have a derived class, add check to engine
            ' to check if object is not nothing and isdaoc active
            Try

                '_DAoC_BaseBot = New DAoC_ThaneBot(_Ak)
                _DAoC_BaseBot = New DAoC_WarriorBot(_Ak)

                _DAoC_BaseBot.SetLog(AddressOf LogLine)

            Catch ex As Exception

                LogLine(ex.Message)

            End Try

        End Sub

        Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxStopButton.Click

            _Running = False
            _Ak.StopRunning()

            If Not _DAoC_BaseBot Is Nothing Then

                _DAoC_BaseBot.Paused = True

            End If

            _DAoC_BaseBot = Nothing

            If _Ak.GameProcess > 0 Then

                _Ak.StopInit()

            End If

            uxInitializeButton.Enabled = True
            uxStartButton.Enabled = False
            uxStopButton.Enabled = False
            uxPauseButton.Enabled = False

        End Sub

        Public Sub Engine()

            While (_EngineStarted)

                If (_Running) Then

                    'If IsDAOCActive() Then
                    If Not _DAoC_BaseBot Is Nothing Then

                        'LogLine("_Running")
                        _DAoC_BaseBot.DoAction()

                    End If
                    'End If

                End If

                'LogLine("blah")
                Thread.Sleep(100)

            End While

        End Sub

        Private Sub btnPause_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxPauseButton.Click

        End Sub

        Private Function IsDAOCActive() As Boolean

            If GetForegroundWindow = FindWindow("DAocMWC", Nothing) Then

                Dim mi As New UpdateTitle(AddressOf UpdateFormTitle)
                Me.BeginInvoke(mi, New Object() {"AKInstanceBot (Active)"})

                Return True

            Else

                Dim mi As New UpdateTitle(AddressOf UpdateFormTitle)
                Me.BeginInvoke(mi, New Object() {"AKInstanceBot (Paused)"})

                Return False

            End If

        End Function

        Private Delegate Sub UpdateTitle(ByRef Text As Object)

        Private Sub UpdateFormTitle(ByRef Text As Object)

            Me.Text = CStr(Text)

        End Sub

        Sub LogLine(ByVal Line As String)

            'Line = Format(Year(Now), "0000") & "-" & Format(Month(Now), "00") & "-" & Format(Microsoft.VisualBasic.Day(Now), "00") & "|" & Format(Hour(Now), "00") & ":" & Format(Minute(Now), "00") & ":" & Format(Second(Now), "00") & "| " & Line
            'lbLog.BeginUpdate()
            'lbLog.Items.Insert(0, Line)

            'If lbLog.Items.Count > 128 Then

            '    lbLog.Items.RemoveAt(127)

            'End If

            'lbLog.EndUpdate()

        End Sub

        Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
            Dim objDAOC As AutoKillerScript.clsAutoKillerScript
            objDAOC = New AutoKillerScript.clsAutoKillerScript

            'Dim intPlayerXCoord As Integer
            'Dim intPlayerYCoord As Integer
            'Dim intPlayerZCoord As Integer

            'Dim intMobXCoord As Integer
            'Dim intMobYCoord As Integer
            'Dim intMobZCoord As Integer

            'Dim dblDistance As Double

            objDAOC.RegKey = "DEREKBEATTIE1249"
            objDAOC.EnableCatacombs = True
            objDAOC.DoInit()

            'dblDistance = objDAOC.ZDistance(objDAOC.gPlayerXCoord, objDAOC.gPlayerYCoord, objDAOC.gPlayerZCoord, _
            '                       objDAOC.MobXCoord(objDAOC.TargetIndex()), objDAOC.MobYCoord(objDAOC.TargetIndex()), objDAOC.MobZCoord(objDAOC.TargetIndex()))

            'LogLine(objDAOC.TargetIndex())
            'LogLine(objDAOC.MobLevel(objDAOC.TargetIndex()))

            'Dim test As Integer
            'test = objDAOC.FindClosestObject(0, 45, 500)
            'LogLine("test: " & test)
            'LogLine(objDAOC.MobName(test))
            'LogLine(objDAOC.MobLevel(test))

            'LogLine("Sleep")
            'System.Threading.Thread.Sleep(5000)

            'Dim dlg As AutoKillerScript.WindowManager = _
            '            New AutoKillerScript.WindowManager(objDAOC, AutoKillerScript.WINDOW_NAMES.Dialog)


            LogLine("zx: " & objDAOC.zPlayerXCoord & "  zy: " & objDAOC.zPlayerYCoord & "  zz: " & objDAOC.zPlayerZCoord)
            'LogLine("gx: " & objDAOC.gPlayerXCoord & "  gy: " & objDAOC.gPlayerYCoord & "  gz: " & objDAOC.gPlayerZCoord)
            'dlg = Nothing

            'Dim x As New Interaction
            'x.Appactivate(objDAOC.GameProcess)
            'System.Threading.Thread.Sleep(2000)

            objDAOC.StopInit()
            objDAOC = Nothing
        End Sub
    End Class

End Namespace