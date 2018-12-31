Option Explicit On 

Imports Microsoft.Win32

Imports System.Xml
Imports System.IO
Imports System.Security.Permissions

Public Class frmMain
    Inherits System.Windows.Forms.Form
#Region " TODO "
    'TODO: 
    'TODO: 
    'TODO: add /qbar when using toolbar
    'TODO: death/rezz checking
    'TODO: set effects to NONE

#End Region
#Region " Variables "
    Dim _AKRemoteAni As AKRemoteAni
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
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents lbLog As System.Windows.Forms.ListBox
    Friend WithEvents btnStop As System.Windows.Forms.Button
    Friend WithEvents btnStart As System.Windows.Forms.Button
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents tbToA As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents tbCatacombs As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents rbToA As System.Windows.Forms.RadioButton
    Friend WithEvents rbCatacombs As System.Windows.Forms.RadioButton
    Friend WithEvents tbRegKey As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.lbLog = New System.Windows.Forms.ListBox
        Me.btnStart = New System.Windows.Forms.Button
        Me.btnStop = New System.Windows.Forms.Button
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.tbToA = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.tbCatacombs = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.rbToA = New System.Windows.Forms.RadioButton
        Me.rbCatacombs = New System.Windows.Forms.RadioButton
        Me.tbRegKey = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
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
        Me.TabControl1.ItemSize = New System.Drawing.Size(42, 18)
        Me.TabControl1.Location = New System.Drawing.Point(8, 8)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(520, 270)
        Me.TabControl1.TabIndex = 1
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
        Me.lbLog.TabIndex = 0
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
        Me.Panel4.Controls.Add(Me.tbToA)
        Me.Panel4.Controls.Add(Me.Label11)
        Me.Panel4.Controls.Add(Me.tbCatacombs)
        Me.Panel4.Controls.Add(Me.Label9)
        Me.Panel4.Controls.Add(Me.Label10)
        Me.Panel4.Location = New System.Drawing.Point(8, 8)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(496, 232)
        Me.Panel4.TabIndex = 1
        '
        'tbToA
        '
        Me.tbToA.Location = New System.Drawing.Point(72, 56)
        Me.tbToA.Name = "tbToA"
        Me.tbToA.Size = New System.Drawing.Size(176, 20)
        Me.tbToA.TabIndex = 4
        Me.tbToA.Text = ""
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(8, 56)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(40, 23)
        Me.Label11.TabIndex = 3
        Me.Label11.Text = "ToA:"
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
        Me.Panel3.Controls.Add(Me.rbToA)
        Me.Panel3.Controls.Add(Me.rbCatacombs)
        Me.Panel3.Controls.Add(Me.btnStart)
        Me.Panel3.Controls.Add(Me.btnStop)
        Me.Panel3.Location = New System.Drawing.Point(8, 288)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(520, 40)
        Me.Panel3.TabIndex = 7
        '
        'rbToA
        '
        Me.rbToA.Location = New System.Drawing.Point(88, 8)
        Me.rbToA.Name = "rbToA"
        Me.rbToA.TabIndex = 1
        Me.rbToA.Text = "ToA"
        '
        'rbCatacombs
        '
        Me.rbCatacombs.Checked = True
        Me.rbCatacombs.Location = New System.Drawing.Point(8, 8)
        Me.rbCatacombs.Name = "rbCatacombs"
        Me.rbCatacombs.TabIndex = 0
        Me.rbCatacombs.TabStop = True
        Me.rbCatacombs.Text = "Catacombs"
        '
        'tbRegKey
        '
        Me.tbRegKey.Location = New System.Drawing.Point(72, 80)
        Me.tbRegKey.Name = "tbRegKey"
        Me.tbRegKey.Size = New System.Drawing.Size(176, 20)
        Me.tbRegKey.TabIndex = 7
        Me.tbRegKey.Text = ""
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 80)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 23)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Reg Key:"
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(538, 338)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmMain"
        Me.Text = "AKRemoteAni"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click

        If rbCatacombs.Checked Then
            _AKRemoteAni = New AKRemoteAni(Me, Me.lbLog, 10, tbCatacombs.Text, tbRegKey.Text, False, True)
        ElseIf rbToA.Checked Then
            _AKRemoteAni = New AKRemoteAni(Me, Me.lbLog, 10, tbToA.Text, tbRegKey.Text, True, False)
        End If

        Me.btnStart.Enabled = False
        Me.btnStop.Enabled = True
    End Sub

    Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStop.Click

        _AKRemoteAni.Terminate()

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
            UpdateFormTitle("AKRemoteAni")
            LogLine("AKRemoteAni v0.01 (c) Version125")

            'had to comment out for now
            'Try
            '    System.Diagnostics.Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.AboveNormal
            'Catch Ex As Exception
            '    ' On Windows 98 machines you get an error for 'above normal'
            '    System.Diagnostics.Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.High
            'End Try

            'load saved setting from registry
            'get registry info
            regKey = Registry.LocalMachine.OpenSubKey("Software\AKRemoteAni")

            'key doesn't exist, make it
            If regKey Is Nothing Then
                regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE", True)
                regKey.CreateSubKey("AKRemoteAni")
                'regKey.Close()
            End If

            'form position
            Me.Top = regKey.GetValue("FormTop", 22)
            Me.Left = regKey.GetValue("FormLeft", 22)

            'options
            rbCatacombs.Checked = regKey.GetValue("Catacombs", "1")
            rbToA.Checked = regKey.GetValue("ToA", "0")

            tbCatacombs.Text = regKey.GetValue("CatacombsPath", "c:\mythic\catacombs")
            tbToA.Text = regKey.GetValue("ToAPath", "c:\mythic\atlantis")
            tbRegKey.Text = regKey.GetValue("RegKey", "DEREKBEATTIE1249")

            If tbCatacombs.Text = "" Then tbCatacombs.Text = "c:\mythic\catacombs"
            If tbToA.Text = "" Then tbToA.Text = "c:\mythic\atlantis"
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

            regKey = Registry.LocalMachine.OpenSubKey("Software\AKRemoteAni", True)

            'form position
            regKey.SetValue("FormTop", Me.Top)
            regKey.SetValue("FormLeft", Me.Left)

            'options
            regKey.SetValue("CatacombsPath", tbCatacombs.Text)
            regKey.SetValue("ToAPath", tbToA.Text)
            regKey.SetValue("RegKey", tbRegKey.Text)

            regKey.SetValue("Catacombs", rbCatacombs.Checked)
            regKey.SetValue("ToA", rbToA.Checked)

            regKey.Close()

        Catch ex As Exception
            LogLine(ex.Message)
        End Try
    End Sub

End Class
