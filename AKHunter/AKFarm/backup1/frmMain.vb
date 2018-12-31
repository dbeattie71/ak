Option Explicit On 

Imports Microsoft.Win32

Imports System.Xml
Imports System.IO
Imports System.Security.Permissions

Public Class frmMain
    Inherits System.Windows.Forms.Form

#Region " Variables "

    Dim _AKFarm As AKFarm

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
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents lbLog As System.Windows.Forms.ListBox
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbRegKey As System.Windows.Forms.TextBox
    Friend WithEvents tbCatacombs As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents btnStart As System.Windows.Forms.Button
    Friend WithEvents btnStop As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.lbLog = New System.Windows.Forms.ListBox
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.tbRegKey = New System.Windows.Forms.TextBox
        Me.tbCatacombs = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.btnStart = New System.Windows.Forms.Button
        Me.btnStop = New System.Windows.Forms.Button
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(8, 8)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(520, 270)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.lbLog)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(512, 244)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Log"
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
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Panel4)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(512, 244)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Options"
        '
        'Panel4
        '
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.Label2)
        Me.Panel4.Controls.Add(Me.tbRegKey)
        Me.Panel4.Controls.Add(Me.tbCatacombs)
        Me.Panel4.Controls.Add(Me.Label9)
        Me.Panel4.Controls.Add(Me.Label10)
        Me.Panel4.Location = New System.Drawing.Point(8, 6)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(496, 232)
        Me.Panel4.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 23)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Reg Key:"
        '
        'tbRegKey
        '
        Me.tbRegKey.Location = New System.Drawing.Point(72, 56)
        Me.tbRegKey.Name = "tbRegKey"
        Me.tbRegKey.Size = New System.Drawing.Size(176, 20)
        Me.tbRegKey.TabIndex = 7
        Me.tbRegKey.Text = ""
        '
        'tbCatacombs
        '
        Me.tbCatacombs.Location = New System.Drawing.Point(72, 32)
        Me.tbCatacombs.Name = "tbCatacombs"
        Me.tbCatacombs.Size = New System.Drawing.Size(176, 20)
        Me.tbCatacombs.TabIndex = 2
        Me.tbCatacombs.Text = ""
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(8, 8)
        Me.Label9.Name = "Label9"
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "Paths"
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(8, 32)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(72, 23)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Catacombs:"
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.Button2)
        Me.Panel3.Controls.Add(Me.Button1)
        Me.Panel3.Controls.Add(Me.btnStart)
        Me.Panel3.Controls.Add(Me.btnStop)
        Me.Panel3.Location = New System.Drawing.Point(8, 288)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(520, 40)
        Me.Panel3.TabIndex = 8
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(280, 8)
        Me.Button2.Name = "Button2"
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "Get D to tgt"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(200, 8)
        Me.Button1.Name = "Button1"
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Get XYZ"
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(360, 8)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(72, 24)
        Me.btnStart.TabIndex = 2
        Me.btnStart.Text = "Start"
        '
        'btnStop
        '
        Me.btnStop.Location = New System.Drawing.Point(440, 8)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(72, 24)
        Me.btnStop.TabIndex = 4
        Me.btnStop.Text = "Stop"
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(538, 338)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmMain"
        Me.Text = "AKFarm"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click

        _AKFarm = New AKFarm(Me, Me.lbLog, 10, tbCatacombs.Text, tbRegKey.Text, False, True)
        
        Me.btnStart.Enabled = False
        Me.btnStop.Enabled = True

    End Sub

    Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStop.Click

        _AKFarm.Terminate()

        Me.btnStart.Enabled = True
        Me.btnStop.Enabled = False

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

    Public Sub UpdateFormTitle(ByRef Text As Object)

        Me.Text = CStr(Text)

    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim regKey As RegistryKey

        Try
            UpdateFormTitle("AKFarm")
            LogLine("AKFarm v0.01 (c) Version125")

            'had to comment out for now
            'Try
            '    System.Diagnostics.Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.AboveNormal
            'Catch Ex As Exception
            '    ' On Windows 98 machines you get an error for 'above normal'
            '    System.Diagnostics.Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.High
            'End Try

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
            tbCatacombs.Text = regKey.GetValue("CatacombsPath", "c:\mythic\catacombs")
            tbRegKey.Text = regKey.GetValue("RegKey", "DEREKBEATTIE1249")

            If tbCatacombs.Text = "" Then tbCatacombs.Text = "c:\mythic\catacombs"
            If tbRegKey.Text = "" Then tbRegKey.Text = ""

            Threading.Thread.CurrentThread.Name = "MainThread"

            Me.btnStop.Enabled = False

        Catch ex As Exception
            LogLine(ex.Message)
        End Try

    End Sub

    Private Sub frmMain_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Dim regKey As RegistryKey

        Try
            'If IsNothing(_AKRemoteAni) = False Then
            '    _AKRemoteAni.Terminate()
            'End If

            regKey = Registry.LocalMachine.OpenSubKey("Software\AKFarm", True)

            'form position
            regKey.SetValue("FormTop", Me.Top)
            regKey.SetValue("FormLeft", Me.Left)

            'options
            regKey.SetValue("CatacombsPath", tbCatacombs.Text)
            regKey.SetValue("RegKey", tbRegKey.Text)

            regKey.Close()

        Catch ex As Exception
            LogLine(ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim objDAOC As AutoKillerScript.clsAutoKillerScript
        objDAOC = New AutoKillerScript.clsAutoKillerScript

        Dim intPlayerXCoord As Integer
        Dim intPlayerYCoord As Integer
        Dim intPlayerZCoord As Integer

        Dim strCoords As String

        objDAOC.RegKey = "DEREKBEATTIE1249"
        objDAOC.EnableCatacombs = True
        objDAOC.DoInit()

        intPlayerXCoord = objDAOC.gPlayerXCoord
        intPlayerYCoord = objDAOC.gPlayerYCoord
        intPlayerZCoord = objDAOC.gPlayerZCoord
        strCoords = "BaseCoords X=" & intPlayerXCoord & " Y=" & intPlayerYCoord & " Z=" & intPlayerZCoord
        LogLine(strCoords)

        objDAOC.StopInit()
        objDAOC = Nothing

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim objDAOC As AutoKillerScript.clsAutoKillerScript
        objDAOC = New AutoKillerScript.clsAutoKillerScript

        Dim intPlayerXCoord As Integer
        Dim intPlayerYCoord As Integer
        Dim intPlayerZCoord As Integer

        Dim intMobXCoord As Integer
        Dim intMobYCoord As Integer
        Dim intMobZCoord As Integer

        Dim dblDistance As Double

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

        LogLine("Sleep")
        System.Threading.Thread.Sleep(5000)

        'Dim dlg As AutoKillerScript.WindowManager = _
        '            New AutoKillerScript.WindowManager(objDAOC, AutoKillerScript.WINDOW_NAMES.Dialog)


        LogLine("zx: " & objDAOC.zPlayerXCoord & "  zy: " & objDAOC.zPlayerYCoord & "  zz: " & objDAOC.zPlayerZCoord)
        LogLine("gx: " & objDAOC.gPlayerXCoord & "  gy: " & objDAOC.gPlayerYCoord & "  gz: " & objDAOC.gPlayerZCoord)
        'dlg = Nothing

        objDAOC.StopInit()
        objDAOC = Nothing
    End Sub
End Class

