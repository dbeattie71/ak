Option Explicit On 

Public Class frmMain
    Inherits System.Windows.Forms.Form
#Region " Variables "
    Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
    Private Declare Function GetForegroundWindow Lib "user32" Alias "GetForegroundWindow" () As Integer
    Shared TerminateThread As Boolean = False
    Shared HealThread As Threading.Thread
    Shared BAFThread As Threading.Thread
    Shared CheckBAF As Boolean
    Shared MezzedList As New Hashtable
    Shared mStopRunning As Boolean

    Shared K_Sit As Integer

    Shared PerfMon As DateTime
    Shared TotalSleep As Integer

    Shared Disease As Boolean
    Shared Poison As Boolean
    Shared Buff2 As Boolean
    Shared TmrCount As Integer
    Shared Buddy As Integer

    Shared FollowGroup As Boolean

    Shared K_Disease As Integer
    Shared K_Poison As Integer
    Shared K_Buff1 As Integer
    Shared K_Buff2 As Integer
    Shared K_BuffTime As Integer

    Dim bytShowInventoryKey As Byte
    Dim bytMoveForwardKey As Byte
    Dim bytMoveBackwardKey As Byte
    Dim bytSitKey As Byte
    Dim bytStickKey As Byte
    Dim bytFaceKey As Byte

    Dim htMembers As Hashtable = New Hashtable
    Private WithEvents WndMngr As DAOCWindowManager
    Private oDAOC As AutoKillerScript.clsAutoKillerScript
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
    Friend WithEvents tpLog As System.Windows.Forms.TabPage
    Friend WithEvents tpMembers As System.Windows.Forms.TabPage
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents tbName As System.Windows.Forms.TextBox
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents tbHealer As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lvMembers As System.Windows.Forms.ListView
    Friend WithEvents lbLog As System.Windows.Forms.ListBox
    Friend WithEvents cbxPriority As System.Windows.Forms.ComboBox
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnChangePrio As System.Windows.Forms.Button
    Friend WithEvents tChkHealth As System.Timers.Timer
    Friend WithEvents tbDebug As System.Windows.Forms.TabPage
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbPerf As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents tbHasMoved As System.Windows.Forms.TextBox
    Friend WithEvents tbCycle As System.Windows.Forms.TextBox
    Friend WithEvents tbSleep As System.Windows.Forms.TextBox
    Friend WithEvents tbPosition As System.Windows.Forms.TextBox
    Friend WithEvents tpProfile As System.Windows.Forms.TabPage
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents btnSaveProfile As System.Windows.Forms.Button
    Friend WithEvents cbxProfiles As System.Windows.Forms.ComboBox
    Friend WithEvents TabControl2 As System.Windows.Forms.TabControl
    Friend WithEvents tpTarget As System.Windows.Forms.TabPage
    Friend WithEvents tpGroup As System.Windows.Forms.TabPage
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents nudHealThreshold As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudHealDelay As System.Windows.Forms.NumericUpDown
    Friend WithEvents cbHealInCombat As System.Windows.Forms.CheckBox
    Friend WithEvents tbHealName As System.Windows.Forms.TextBox
    Friend WithEvents tbHealKey As System.Windows.Forms.TextBox
    Friend WithEvents Panel7 As System.Windows.Forms.Panel
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents lvHeals As System.Windows.Forms.ListView
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents BuffTmr1 As System.Windows.Forms.Timer
    Friend WithEvents ColumnHeader10 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader11 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader12 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader13 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader14 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tpBuff As System.Windows.Forms.TabPage
    Friend WithEvents tpOther As System.Windows.Forms.TabPage
    Friend WithEvents cbSitMode As System.Windows.Forms.ComboBox
    Friend WithEvents cbxStopAfterDeath As System.Windows.Forms.CheckBox
    Friend WithEvents cbxAutoGroup As System.Windows.Forms.CheckBox
    Friend WithEvents BuffTmrProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents tbBuffTime1 As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents tbBuffKey1 As System.Windows.Forms.TextBox
    Friend WithEvents tbBuffKey2 As System.Windows.Forms.TextBox
    Friend WithEvents BuffBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents tpCures As System.Windows.Forms.TabPage
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents DiseaseBox As System.Windows.Forms.CheckBox
    Friend WithEvents PoisonBox As System.Windows.Forms.CheckBox
    Friend WithEvents tbPoisonKey As System.Windows.Forms.TextBox
    Friend WithEvents tbDiseaseKey As System.Windows.Forms.TextBox
    Friend WithEvents cbxFollowGroup As System.Windows.Forms.CheckBox
    Friend WithEvents cbxWaitForRez As System.Windows.Forms.CheckBox
    Friend WithEvents cbxQuiet As System.Windows.Forms.CheckBox
    Friend WithEvents btnLoad As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents gbMezz As System.Windows.Forms.GroupBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents rbRVR As System.Windows.Forms.RadioButton
    Friend WithEvents rbHealerArea As System.Windows.Forms.RadioButton
    Friend WithEvents rbTarget As System.Windows.Forms.RadioButton
    Friend WithEvents rbGT As System.Windows.Forms.RadioButton
    Friend WithEvents lblMezz As System.Windows.Forms.Label
    Friend WithEvents txtMezzKey As System.Windows.Forms.TextBox
    Friend WithEvents txtSearchDistance As System.Windows.Forms.TextBox
    Friend WithEvents chkCheckForBAF As System.Windows.Forms.CheckBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents nudSpreadMembersThreshold As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents tbSpreadKey As System.Windows.Forms.TextBox
    Friend WithEvents tbSpreadName As System.Windows.Forms.TextBox
    Friend WithEvents nudSpreadDelay As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudSpreadThreshold As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents cbxEnableSpread As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Button2 = New System.Windows.Forms.Button
        Me.BuffTmrProgress = New System.Windows.Forms.ProgressBar
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.btnLoad = New System.Windows.Forms.Button
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.tpMembers = New System.Windows.Forms.TabPage
        Me.lvMembers = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.btnChangePrio = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.tbHealer = New System.Windows.Forms.TextBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.cbxPriority = New System.Windows.Forms.ComboBox
        Me.btnAdd = New System.Windows.Forms.Button
        Me.tbName = New System.Windows.Forms.TextBox
        Me.tpProfile = New System.Windows.Forms.TabPage
        Me.TabControl2 = New System.Windows.Forms.TabControl
        Me.tpGroup = New System.Windows.Forms.TabPage
        Me.tpOther = New System.Windows.Forms.TabPage
        Me.cbxQuiet = New System.Windows.Forms.CheckBox
        Me.cbxWaitForRez = New System.Windows.Forms.CheckBox
        Me.cbxFollowGroup = New System.Windows.Forms.CheckBox
        Me.cbSitMode = New System.Windows.Forms.ComboBox
        Me.cbxStopAfterDeath = New System.Windows.Forms.CheckBox
        Me.cbxAutoGroup = New System.Windows.Forms.CheckBox
        Me.tpTarget = New System.Windows.Forms.TabPage
        Me.lvHeals = New System.Windows.Forms.ListView
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.Panel7 = New System.Windows.Forms.Panel
        Me.Button3 = New System.Windows.Forms.Button
        Me.Panel6 = New System.Windows.Forms.Panel
        Me.tbHealKey = New System.Windows.Forms.TextBox
        Me.tbHealName = New System.Windows.Forms.TextBox
        Me.cbHealInCombat = New System.Windows.Forms.CheckBox
        Me.nudHealDelay = New System.Windows.Forms.NumericUpDown
        Me.nudHealThreshold = New System.Windows.Forms.NumericUpDown
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.tpBuff = New System.Windows.Forms.TabPage
        Me.BuffBox2 = New System.Windows.Forms.CheckBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.tbBuffKey2 = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.tbBuffKey1 = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.tbBuffTime1 = New System.Windows.Forms.TextBox
        Me.tpCures = New System.Windows.Forms.TabPage
        Me.PoisonBox = New System.Windows.Forms.CheckBox
        Me.DiseaseBox = New System.Windows.Forms.CheckBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.tbPoisonKey = New System.Windows.Forms.TextBox
        Me.tbDiseaseKey = New System.Windows.Forms.TextBox
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.gbMezz = New System.Windows.Forms.GroupBox
        Me.Label23 = New System.Windows.Forms.Label
        Me.rbRVR = New System.Windows.Forms.RadioButton
        Me.rbHealerArea = New System.Windows.Forms.RadioButton
        Me.rbTarget = New System.Windows.Forms.RadioButton
        Me.rbGT = New System.Windows.Forms.RadioButton
        Me.lblMezz = New System.Windows.Forms.Label
        Me.txtMezzKey = New System.Windows.Forms.TextBox
        Me.txtSearchDistance = New System.Windows.Forms.TextBox
        Me.chkCheckForBAF = New System.Windows.Forms.CheckBox
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.btnSaveProfile = New System.Windows.Forms.Button
        Me.cbxProfiles = New System.Windows.Forms.ComboBox
        Me.tpLog = New System.Windows.Forms.TabPage
        Me.lbLog = New System.Windows.Forms.ListBox
        Me.tbDebug = New System.Windows.Forms.TabPage
        Me.tbPosition = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.tbHasMoved = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.tbSleep = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.tbCycle = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.tbPerf = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.tChkHealth = New System.Timers.Timer
        Me.BuffTmr1 = New System.Windows.Forms.Timer(Me.components)
        Me.ColumnHeader10 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader11 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader12 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader13 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader14 = New System.Windows.Forms.ColumnHeader
        Me.nudSpreadMembersThreshold = New System.Windows.Forms.NumericUpDown
        Me.Label16 = New System.Windows.Forms.Label
        Me.tbSpreadKey = New System.Windows.Forms.TextBox
        Me.tbSpreadName = New System.Windows.Forms.TextBox
        Me.nudSpreadDelay = New System.Windows.Forms.NumericUpDown
        Me.nudSpreadThreshold = New System.Windows.Forms.NumericUpDown
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.cbxEnableSpread = New System.Windows.Forms.CheckBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Panel1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.tpMembers.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.tpProfile.SuspendLayout()
        Me.TabControl2.SuspendLayout()
        Me.tpGroup.SuspendLayout()
        Me.tpOther.SuspendLayout()
        Me.tpTarget.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.Panel6.SuspendLayout()
        CType(Me.nudHealDelay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHealThreshold, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpBuff.SuspendLayout()
        Me.tpCures.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.gbMezz.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.tpLog.SuspendLayout()
        Me.tbDebug.SuspendLayout()
        CType(Me.tChkHealth, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudSpreadMembersThreshold, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudSpreadDelay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudSpreadThreshold, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.BuffTmrProgress)
        Me.Panel1.Controls.Add(Me.CheckBox1)
        Me.Panel1.Controls.Add(Me.btnLoad)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 270)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(304, 40)
        Me.Panel1.TabIndex = 1
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(192, 8)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(24, 24)
        Me.Button2.TabIndex = 4
        Me.Button2.Text = "X"
        '
        'BuffTmrProgress
        '
        Me.BuffTmrProgress.Cursor = System.Windows.Forms.Cursors.Default
        Me.BuffTmrProgress.Location = New System.Drawing.Point(88, 14)
        Me.BuffTmrProgress.Maximum = 61
        Me.BuffTmrProgress.Name = "BuffTmrProgress"
        Me.BuffTmrProgress.Size = New System.Drawing.Size(100, 12)
        Me.BuffTmrProgress.Step = 1
        Me.BuffTmrProgress.TabIndex = 3
        '
        'CheckBox1
        '
        Me.CheckBox1.Location = New System.Drawing.Point(8, 8)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(72, 24)
        Me.CheckBox1.TabIndex = 2
        Me.CheckBox1.Text = "Buff timer"
        '
        'btnLoad
        '
        Me.btnLoad.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLoad.Location = New System.Drawing.Point(220, 8)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.TabIndex = 1
        Me.btnLoad.Text = "Start"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tpMembers)
        Me.TabControl1.Controls.Add(Me.tpProfile)
        Me.TabControl1.Controls.Add(Me.tpLog)
        Me.TabControl1.Controls.Add(Me.tbDebug)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(304, 270)
        Me.TabControl1.TabIndex = 2
        '
        'tpMembers
        '
        Me.tpMembers.Controls.Add(Me.lvMembers)
        Me.tpMembers.Controls.Add(Me.Panel3)
        Me.tpMembers.Controls.Add(Me.Panel4)
        Me.tpMembers.Controls.Add(Me.Panel2)
        Me.tpMembers.Location = New System.Drawing.Point(4, 22)
        Me.tpMembers.Name = "tpMembers"
        Me.tpMembers.Size = New System.Drawing.Size(296, 244)
        Me.tpMembers.TabIndex = 1
        Me.tpMembers.Text = "Group Members"
        '
        'lvMembers
        '
        Me.lvMembers.CheckBoxes = True
        Me.lvMembers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader4, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.lvMembers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvMembers.FullRowSelect = True
        Me.lvMembers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvMembers.HideSelection = False
        Me.lvMembers.Location = New System.Drawing.Point(0, 35)
        Me.lvMembers.MultiSelect = False
        Me.lvMembers.Name = "lvMembers"
        Me.lvMembers.Size = New System.Drawing.Size(256, 173)
        Me.lvMembers.TabIndex = 2
        Me.lvMembers.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 100
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Priority"
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Health"
        Me.ColumnHeader2.Width = 48
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "ID"
        Me.ColumnHeader3.Width = 32
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.btnChangePrio)
        Me.Panel3.Controls.Add(Me.btnDelete)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel3.Location = New System.Drawing.Point(256, 35)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(40, 173)
        Me.Panel3.TabIndex = 1
        '
        'btnChangePrio
        '
        Me.btnChangePrio.Font = New System.Drawing.Font("Symbol", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnChangePrio.Location = New System.Drawing.Point(8, 40)
        Me.btnChangePrio.Name = "btnChangePrio"
        Me.btnChangePrio.Size = New System.Drawing.Size(24, 24)
        Me.btnChangePrio.TabIndex = 6
        Me.btnChangePrio.Text = "«"
        '
        'btnDelete
        '
        Me.btnDelete.Font = New System.Drawing.Font("Symbol", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnDelete.Location = New System.Drawing.Point(8, 8)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(24, 24)
        Me.btnDelete.TabIndex = 5
        Me.btnDelete.Text = "´"
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.Label1)
        Me.Panel4.Controls.Add(Me.tbHealer)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(296, 35)
        Me.Panel4.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 16)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Healer :"
        '
        'tbHealer
        '
        Me.tbHealer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbHealer.Location = New System.Drawing.Point(56, 8)
        Me.tbHealer.Name = "tbHealer"
        Me.tbHealer.Size = New System.Drawing.Size(232, 20)
        Me.tbHealer.TabIndex = 1
        Me.tbHealer.Text = ""
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.cbxPriority)
        Me.Panel2.Controls.Add(Me.btnAdd)
        Me.Panel2.Controls.Add(Me.tbName)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 208)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(296, 36)
        Me.Panel2.TabIndex = 0
        '
        'cbxPriority
        '
        Me.cbxPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxPriority.Items.AddRange(New Object() {"Normal", "High", "Low"})
        Me.cbxPriority.Location = New System.Drawing.Point(160, 8)
        Me.cbxPriority.Name = "cbxPriority"
        Me.cbxPriority.Size = New System.Drawing.Size(72, 21)
        Me.cbxPriority.TabIndex = 8
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.Location = New System.Drawing.Point(240, 9)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(48, 21)
        Me.btnAdd.TabIndex = 9
        Me.btnAdd.Text = "Add"
        '
        'tbName
        '
        Me.tbName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbName.Location = New System.Drawing.Point(0, 8)
        Me.tbName.Name = "tbName"
        Me.tbName.Size = New System.Drawing.Size(152, 20)
        Me.tbName.TabIndex = 7
        Me.tbName.Text = ""
        '
        'tpProfile
        '
        Me.tpProfile.Controls.Add(Me.TabControl2)
        Me.tpProfile.Controls.Add(Me.Panel5)
        Me.tpProfile.Location = New System.Drawing.Point(4, 22)
        Me.tpProfile.Name = "tpProfile"
        Me.tpProfile.Size = New System.Drawing.Size(296, 244)
        Me.tpProfile.TabIndex = 4
        Me.tpProfile.Text = "Profile"
        '
        'TabControl2
        '
        Me.TabControl2.Alignment = System.Windows.Forms.TabAlignment.Bottom
        Me.TabControl2.Controls.Add(Me.tpGroup)
        Me.TabControl2.Controls.Add(Me.tpOther)
        Me.TabControl2.Controls.Add(Me.tpTarget)
        Me.TabControl2.Controls.Add(Me.tpBuff)
        Me.TabControl2.Controls.Add(Me.tpCures)
        Me.TabControl2.Controls.Add(Me.TabPage1)
        Me.TabControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl2.Location = New System.Drawing.Point(0, 36)
        Me.TabControl2.Multiline = True
        Me.TabControl2.Name = "TabControl2"
        Me.TabControl2.SelectedIndex = 0
        Me.TabControl2.Size = New System.Drawing.Size(296, 208)
        Me.TabControl2.TabIndex = 3
        '
        'tpGroup
        '
        Me.tpGroup.Controls.Add(Me.GroupBox1)
        Me.tpGroup.Location = New System.Drawing.Point(4, 4)
        Me.tpGroup.Name = "tpGroup"
        Me.tpGroup.Size = New System.Drawing.Size(288, 164)
        Me.tpGroup.TabIndex = 1
        Me.tpGroup.Text = "Group Heals"
        '
        'tpOther
        '
        Me.tpOther.Controls.Add(Me.cbxQuiet)
        Me.tpOther.Controls.Add(Me.cbxWaitForRez)
        Me.tpOther.Controls.Add(Me.cbxFollowGroup)
        Me.tpOther.Controls.Add(Me.cbSitMode)
        Me.tpOther.Controls.Add(Me.cbxStopAfterDeath)
        Me.tpOther.Controls.Add(Me.cbxAutoGroup)
        Me.tpOther.Location = New System.Drawing.Point(4, 4)
        Me.tpOther.Name = "tpOther"
        Me.tpOther.Size = New System.Drawing.Size(288, 164)
        Me.tpOther.TabIndex = 2
        Me.tpOther.Text = "Settings"
        '
        'cbxQuiet
        '
        Me.cbxQuiet.Checked = True
        Me.cbxQuiet.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxQuiet.Location = New System.Drawing.Point(8, 96)
        Me.cbxQuiet.Name = "cbxQuiet"
        Me.cbxQuiet.Size = New System.Drawing.Size(56, 16)
        Me.cbxQuiet.TabIndex = 14
        Me.cbxQuiet.Text = "Quiet"
        '
        'cbxWaitForRez
        '
        Me.cbxWaitForRez.Checked = True
        Me.cbxWaitForRez.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxWaitForRez.Location = New System.Drawing.Point(144, 64)
        Me.cbxWaitForRez.Name = "cbxWaitForRez"
        Me.cbxWaitForRez.Size = New System.Drawing.Size(128, 16)
        Me.cbxWaitForRez.TabIndex = 11
        Me.cbxWaitForRez.Text = "Wait For Rez"
        '
        'cbxFollowGroup
        '
        Me.cbxFollowGroup.Checked = True
        Me.cbxFollowGroup.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxFollowGroup.Location = New System.Drawing.Point(8, 72)
        Me.cbxFollowGroup.Name = "cbxFollowGroup"
        Me.cbxFollowGroup.Size = New System.Drawing.Size(96, 16)
        Me.cbxFollowGroup.TabIndex = 10
        Me.cbxFollowGroup.Text = "Follow Group"
        '
        'cbSitMode
        '
        Me.cbSitMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSitMode.Items.AddRange(New Object() {"Do not sit to regain power.", "Sit to regain power, when everyone is out of combat.", "Always sit to regain power, only stand to heal."})
        Me.cbSitMode.Location = New System.Drawing.Point(8, 8)
        Me.cbSitMode.Name = "cbSitMode"
        Me.cbSitMode.Size = New System.Drawing.Size(272, 21)
        Me.cbSitMode.TabIndex = 9
        '
        'cbxStopAfterDeath
        '
        Me.cbxStopAfterDeath.Checked = True
        Me.cbxStopAfterDeath.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxStopAfterDeath.Location = New System.Drawing.Point(144, 40)
        Me.cbxStopAfterDeath.Name = "cbxStopAfterDeath"
        Me.cbxStopAfterDeath.Size = New System.Drawing.Size(104, 16)
        Me.cbxStopAfterDeath.TabIndex = 8
        Me.cbxStopAfterDeath.Text = "Stop after death"
        '
        'cbxAutoGroup
        '
        Me.cbxAutoGroup.Checked = True
        Me.cbxAutoGroup.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxAutoGroup.Location = New System.Drawing.Point(8, 32)
        Me.cbxAutoGroup.Name = "cbxAutoGroup"
        Me.cbxAutoGroup.Size = New System.Drawing.Size(144, 32)
        Me.cbxAutoGroup.TabIndex = 6
        Me.cbxAutoGroup.Text = "Auto update group (enables group spells)"
        '
        'tpTarget
        '
        Me.tpTarget.Controls.Add(Me.lvHeals)
        Me.tpTarget.Controls.Add(Me.Panel7)
        Me.tpTarget.Controls.Add(Me.Panel6)
        Me.tpTarget.Location = New System.Drawing.Point(4, 4)
        Me.tpTarget.Name = "tpTarget"
        Me.tpTarget.Size = New System.Drawing.Size(288, 164)
        Me.tpTarget.TabIndex = 0
        Me.tpTarget.Text = "Target Heals"
        '
        'lvHeals
        '
        Me.lvHeals.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8, Me.ColumnHeader9})
        Me.lvHeals.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvHeals.FullRowSelect = True
        Me.lvHeals.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvHeals.Location = New System.Drawing.Point(0, 0)
        Me.lvHeals.MultiSelect = False
        Me.lvHeals.Name = "lvHeals"
        Me.lvHeals.Size = New System.Drawing.Size(256, 84)
        Me.lvHeals.TabIndex = 1
        Me.lvHeals.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Name"
        Me.ColumnHeader5.Width = 63
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Threshold"
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Key"
        Me.ColumnHeader7.Width = 30
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Delay"
        Me.ColumnHeader8.Width = 39
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "In Combat"
        '
        'Panel7
        '
        Me.Panel7.Controls.Add(Me.Button3)
        Me.Panel7.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel7.Location = New System.Drawing.Point(256, 0)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(32, 84)
        Me.Panel7.TabIndex = 2
        '
        'Button3
        '
        Me.Button3.Font = New System.Drawing.Font("Symbol", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.Button3.Location = New System.Drawing.Point(4, 4)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(24, 24)
        Me.Button3.TabIndex = 5
        Me.Button3.Text = "´"
        '
        'Panel6
        '
        Me.Panel6.Controls.Add(Me.tbHealKey)
        Me.Panel6.Controls.Add(Me.tbHealName)
        Me.Panel6.Controls.Add(Me.cbHealInCombat)
        Me.Panel6.Controls.Add(Me.nudHealDelay)
        Me.Panel6.Controls.Add(Me.nudHealThreshold)
        Me.Panel6.Controls.Add(Me.Button1)
        Me.Panel6.Controls.Add(Me.Label11)
        Me.Panel6.Controls.Add(Me.Label10)
        Me.Panel6.Controls.Add(Me.Label9)
        Me.Panel6.Controls.Add(Me.Label8)
        Me.Panel6.Controls.Add(Me.Label7)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel6.Location = New System.Drawing.Point(0, 84)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(288, 80)
        Me.Panel6.TabIndex = 0
        '
        'tbHealKey
        '
        Me.tbHealKey.Location = New System.Drawing.Point(72, 56)
        Me.tbHealKey.MaxLength = 1
        Me.tbHealKey.Name = "tbHealKey"
        Me.tbHealKey.Size = New System.Drawing.Size(16, 20)
        Me.tbHealKey.TabIndex = 16
        Me.tbHealKey.Text = ""
        '
        'tbHealName
        '
        Me.tbHealName.Location = New System.Drawing.Point(72, 10)
        Me.tbHealName.Name = "tbHealName"
        Me.tbHealName.Size = New System.Drawing.Size(72, 20)
        Me.tbHealName.TabIndex = 14
        Me.tbHealName.Text = ""
        '
        'cbHealInCombat
        '
        Me.cbHealInCombat.Location = New System.Drawing.Point(210, 32)
        Me.cbHealInCombat.Name = "cbHealInCombat"
        Me.cbHealInCombat.Size = New System.Drawing.Size(16, 24)
        Me.cbHealInCombat.TabIndex = 18
        '
        'nudHealDelay
        '
        Me.nudHealDelay.Increment = New Decimal(New Integer() {50, 0, 0, 0})
        Me.nudHealDelay.Location = New System.Drawing.Point(210, 10)
        Me.nudHealDelay.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudHealDelay.Name = "nudHealDelay"
        Me.nudHealDelay.Size = New System.Drawing.Size(70, 20)
        Me.nudHealDelay.TabIndex = 17
        '
        'nudHealThreshold
        '
        Me.nudHealThreshold.Location = New System.Drawing.Point(72, 33)
        Me.nudHealThreshold.Name = "nudHealThreshold"
        Me.nudHealThreshold.Size = New System.Drawing.Size(72, 20)
        Me.nudHealThreshold.TabIndex = 15
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(238, 56)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(48, 21)
        Me.Button1.TabIndex = 19
        Me.Button1.Text = "Add"
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(144, 32)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(64, 20)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "In Combat :"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(144, 8)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(64, 20)
        Me.Label10.TabIndex = 3
        Me.Label10.Text = "Delay :"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(8, 56)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(64, 20)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "Key :"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(8, 32)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(64, 20)
        Me.Label8.TabIndex = 1
        Me.Label8.Text = "Threshold :"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(8, 8)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(64, 20)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Name :"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tpBuff
        '
        Me.tpBuff.Controls.Add(Me.BuffBox2)
        Me.tpBuff.Controls.Add(Me.Label20)
        Me.tpBuff.Controls.Add(Me.tbBuffKey2)
        Me.tpBuff.Controls.Add(Me.Label18)
        Me.tpBuff.Controls.Add(Me.tbBuffKey1)
        Me.tpBuff.Controls.Add(Me.Label17)
        Me.tpBuff.Controls.Add(Me.tbBuffTime1)
        Me.tpBuff.Location = New System.Drawing.Point(4, 4)
        Me.tpBuff.Name = "tpBuff"
        Me.tpBuff.Size = New System.Drawing.Size(288, 164)
        Me.tpBuff.TabIndex = 3
        Me.tpBuff.Text = "Buffs"
        '
        'BuffBox2
        '
        Me.BuffBox2.Location = New System.Drawing.Point(96, 48)
        Me.BuffBox2.Name = "BuffBox2"
        Me.BuffBox2.Size = New System.Drawing.Size(56, 24)
        Me.BuffBox2.TabIndex = 8
        Me.BuffBox2.Text = "Buff 2"
        '
        'Label20
        '
        Me.Label20.Location = New System.Drawing.Point(184, 52)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(88, 16)
        Me.Label20.TabIndex = 7
        Me.Label20.Text = "Quickbar key"
        '
        'tbBuffKey2
        '
        Me.tbBuffKey2.Location = New System.Drawing.Point(160, 48)
        Me.tbBuffKey2.Name = "tbBuffKey2"
        Me.tbBuffKey2.Size = New System.Drawing.Size(16, 20)
        Me.tbBuffKey2.TabIndex = 6
        Me.tbBuffKey2.Text = "1"
        '
        'Label18
        '
        Me.Label18.Location = New System.Drawing.Point(184, 20)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(88, 16)
        Me.Label18.TabIndex = 3
        Me.Label18.Text = "Quickbar key"
        '
        'tbBuffKey1
        '
        Me.tbBuffKey1.Location = New System.Drawing.Point(160, 16)
        Me.tbBuffKey1.Name = "tbBuffKey1"
        Me.tbBuffKey1.Size = New System.Drawing.Size(16, 20)
        Me.tbBuffKey1.TabIndex = 2
        Me.tbBuffKey1.Text = "1"
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(50, 19)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(104, 16)
        Me.Label17.TabIndex = 1
        Me.Label17.Text = "Duration in seconds"
        '
        'tbBuffTime1
        '
        Me.tbBuffTime1.Location = New System.Drawing.Point(16, 16)
        Me.tbBuffTime1.Name = "tbBuffTime1"
        Me.tbBuffTime1.Size = New System.Drawing.Size(30, 20)
        Me.tbBuffTime1.TabIndex = 0
        Me.tbBuffTime1.Text = "60"
        '
        'tpCures
        '
        Me.tpCures.Controls.Add(Me.PoisonBox)
        Me.tpCures.Controls.Add(Me.DiseaseBox)
        Me.tpCures.Controls.Add(Me.Label21)
        Me.tpCures.Controls.Add(Me.Label19)
        Me.tpCures.Controls.Add(Me.tbPoisonKey)
        Me.tpCures.Controls.Add(Me.tbDiseaseKey)
        Me.tpCures.Location = New System.Drawing.Point(4, 4)
        Me.tpCures.Name = "tpCures"
        Me.tpCures.Size = New System.Drawing.Size(288, 164)
        Me.tpCures.TabIndex = 4
        Me.tpCures.Text = "Cures"
        '
        'PoisonBox
        '
        Me.PoisonBox.Location = New System.Drawing.Point(16, 36)
        Me.PoisonBox.Name = "PoisonBox"
        Me.PoisonBox.Size = New System.Drawing.Size(96, 16)
        Me.PoisonBox.TabIndex = 12
        Me.PoisonBox.Text = "Cure Poison"
        '
        'DiseaseBox
        '
        Me.DiseaseBox.Location = New System.Drawing.Point(16, 12)
        Me.DiseaseBox.Name = "DiseaseBox"
        Me.DiseaseBox.Size = New System.Drawing.Size(96, 16)
        Me.DiseaseBox.TabIndex = 11
        Me.DiseaseBox.Text = "Cure Disease"
        '
        'Label21
        '
        Me.Label21.Location = New System.Drawing.Point(152, 35)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(96, 16)
        Me.Label21.TabIndex = 10
        Me.Label21.Text = "Poison QuickKey"
        '
        'Label19
        '
        Me.Label19.Location = New System.Drawing.Point(152, 12)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(104, 16)
        Me.Label19.TabIndex = 9
        Me.Label19.Text = "Disease QuickKey"
        '
        'tbPoisonKey
        '
        Me.tbPoisonKey.Location = New System.Drawing.Point(128, 32)
        Me.tbPoisonKey.Name = "tbPoisonKey"
        Me.tbPoisonKey.Size = New System.Drawing.Size(16, 20)
        Me.tbPoisonKey.TabIndex = 8
        Me.tbPoisonKey.Text = "1"
        '
        'tbDiseaseKey
        '
        Me.tbDiseaseKey.Location = New System.Drawing.Point(128, 8)
        Me.tbDiseaseKey.Name = "tbDiseaseKey"
        Me.tbDiseaseKey.Size = New System.Drawing.Size(16, 20)
        Me.tbDiseaseKey.TabIndex = 7
        Me.tbDiseaseKey.Text = "1"
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.gbMezz)
        Me.TabPage1.Controls.Add(Me.chkCheckForBAF)
        Me.TabPage1.Location = New System.Drawing.Point(4, 4)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(288, 164)
        Me.TabPage1.TabIndex = 5
        Me.TabPage1.Text = "Mezz"
        '
        'gbMezz
        '
        Me.gbMezz.Controls.Add(Me.Label23)
        Me.gbMezz.Controls.Add(Me.rbRVR)
        Me.gbMezz.Controls.Add(Me.rbHealerArea)
        Me.gbMezz.Controls.Add(Me.rbTarget)
        Me.gbMezz.Controls.Add(Me.rbGT)
        Me.gbMezz.Controls.Add(Me.lblMezz)
        Me.gbMezz.Controls.Add(Me.txtMezzKey)
        Me.gbMezz.Controls.Add(Me.txtSearchDistance)
        Me.gbMezz.Location = New System.Drawing.Point(8, 24)
        Me.gbMezz.Name = "gbMezz"
        Me.gbMezz.Size = New System.Drawing.Size(256, 88)
        Me.gbMezz.TabIndex = 20
        Me.gbMezz.TabStop = False
        Me.gbMezz.Visible = False
        '
        'Label23
        '
        Me.Label23.Location = New System.Drawing.Point(8, 40)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(100, 16)
        Me.Label23.TabIndex = 18
        Me.Label23.Text = "Distance:"
        '
        'rbRVR
        '
        Me.rbRVR.Location = New System.Drawing.Point(128, 64)
        Me.rbRVR.Name = "rbRVR"
        Me.rbRVR.Size = New System.Drawing.Size(96, 16)
        Me.rbRVR.TabIndex = 16
        Me.rbRVR.Text = "RvR"
        '
        'rbHealerArea
        '
        Me.rbHealerArea.Location = New System.Drawing.Point(128, 32)
        Me.rbHealerArea.Name = "rbHealerArea"
        Me.rbHealerArea.Size = New System.Drawing.Size(120, 16)
        Me.rbHealerArea.TabIndex = 15
        Me.rbHealerArea.Text = "Area around healer"
        '
        'rbTarget
        '
        Me.rbTarget.Location = New System.Drawing.Point(128, 48)
        Me.rbTarget.Name = "rbTarget"
        Me.rbTarget.Size = New System.Drawing.Size(104, 16)
        Me.rbTarget.TabIndex = 14
        Me.rbTarget.Text = "Select Target"
        '
        'rbGT
        '
        Me.rbGT.Location = New System.Drawing.Point(128, 16)
        Me.rbGT.Name = "rbGT"
        Me.rbGT.Size = New System.Drawing.Size(120, 16)
        Me.rbGT.TabIndex = 13
        Me.rbGT.Text = "Use Ground Target"
        '
        'lblMezz
        '
        Me.lblMezz.Location = New System.Drawing.Point(32, 16)
        Me.lblMezz.Name = "lblMezz"
        Me.lblMezz.Size = New System.Drawing.Size(56, 16)
        Me.lblMezz.TabIndex = 12
        Me.lblMezz.Text = "Mezz key"
        '
        'txtMezzKey
        '
        Me.txtMezzKey.Location = New System.Drawing.Point(8, 16)
        Me.txtMezzKey.MaxLength = 1
        Me.txtMezzKey.Name = "txtMezzKey"
        Me.txtMezzKey.Size = New System.Drawing.Size(16, 20)
        Me.txtMezzKey.TabIndex = 11
        Me.txtMezzKey.Text = ""
        '
        'txtSearchDistance
        '
        Me.txtSearchDistance.Location = New System.Drawing.Point(8, 56)
        Me.txtSearchDistance.Name = "txtSearchDistance"
        Me.txtSearchDistance.Size = New System.Drawing.Size(88, 20)
        Me.txtSearchDistance.TabIndex = 17
        Me.txtSearchDistance.Text = ""
        '
        'chkCheckForBAF
        '
        Me.chkCheckForBAF.Location = New System.Drawing.Point(8, 8)
        Me.chkCheckForBAF.Name = "chkCheckForBAF"
        Me.chkCheckForBAF.Size = New System.Drawing.Size(104, 16)
        Me.chkCheckForBAF.TabIndex = 19
        Me.chkCheckForBAF.Text = "Check for BAF"
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.btnSaveProfile)
        Me.Panel5.Controls.Add(Me.cbxProfiles)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel5.Location = New System.Drawing.Point(0, 0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(296, 36)
        Me.Panel5.TabIndex = 2
        '
        'btnSaveProfile
        '
        Me.btnSaveProfile.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveProfile.Location = New System.Drawing.Point(252, 7)
        Me.btnSaveProfile.Name = "btnSaveProfile"
        Me.btnSaveProfile.Size = New System.Drawing.Size(40, 23)
        Me.btnSaveProfile.TabIndex = 3
        Me.btnSaveProfile.Text = "Save"
        '
        'cbxProfiles
        '
        Me.cbxProfiles.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbxProfiles.Location = New System.Drawing.Point(8, 8)
        Me.cbxProfiles.Name = "cbxProfiles"
        Me.cbxProfiles.Size = New System.Drawing.Size(240, 21)
        Me.cbxProfiles.TabIndex = 2
        '
        'tpLog
        '
        Me.tpLog.Controls.Add(Me.lbLog)
        Me.tpLog.Location = New System.Drawing.Point(4, 22)
        Me.tpLog.Name = "tpLog"
        Me.tpLog.Size = New System.Drawing.Size(296, 244)
        Me.tpLog.TabIndex = 0
        Me.tpLog.Text = "Log"
        '
        'lbLog
        '
        Me.lbLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbLog.HorizontalScrollbar = True
        Me.lbLog.Location = New System.Drawing.Point(0, 0)
        Me.lbLog.Name = "lbLog"
        Me.lbLog.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lbLog.Size = New System.Drawing.Size(296, 238)
        Me.lbLog.TabIndex = 0
        '
        'tbDebug
        '
        Me.tbDebug.Controls.Add(Me.tbPosition)
        Me.tbDebug.Controls.Add(Me.Label6)
        Me.tbDebug.Controls.Add(Me.tbHasMoved)
        Me.tbDebug.Controls.Add(Me.Label5)
        Me.tbDebug.Controls.Add(Me.tbSleep)
        Me.tbDebug.Controls.Add(Me.Label4)
        Me.tbDebug.Controls.Add(Me.tbCycle)
        Me.tbDebug.Controls.Add(Me.Label3)
        Me.tbDebug.Controls.Add(Me.tbPerf)
        Me.tbDebug.Controls.Add(Me.Label2)
        Me.tbDebug.Location = New System.Drawing.Point(4, 22)
        Me.tbDebug.Name = "tbDebug"
        Me.tbDebug.Size = New System.Drawing.Size(296, 244)
        Me.tbDebug.TabIndex = 3
        Me.tbDebug.Text = "Debug"
        '
        'tbPosition
        '
        Me.tbPosition.Location = New System.Drawing.Point(88, 104)
        Me.tbPosition.Name = "tbPosition"
        Me.tbPosition.ReadOnly = True
        Me.tbPosition.Size = New System.Drawing.Size(128, 20)
        Me.tbPosition.TabIndex = 9
        Me.tbPosition.Text = ""
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(8, 104)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(80, 20)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "Position :"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tbHasMoved
        '
        Me.tbHasMoved.Location = New System.Drawing.Point(88, 80)
        Me.tbHasMoved.Name = "tbHasMoved"
        Me.tbHasMoved.ReadOnly = True
        Me.tbHasMoved.Size = New System.Drawing.Size(80, 20)
        Me.tbHasMoved.TabIndex = 7
        Me.tbHasMoved.Text = ""
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(8, 80)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(80, 20)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Has Moved :"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tbSleep
        '
        Me.tbSleep.Location = New System.Drawing.Point(88, 56)
        Me.tbSleep.Name = "tbSleep"
        Me.tbSleep.ReadOnly = True
        Me.tbSleep.Size = New System.Drawing.Size(80, 20)
        Me.tbSleep.TabIndex = 5
        Me.tbSleep.Text = ""
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(8, 56)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(80, 20)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Sleep :"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tbCycle
        '
        Me.tbCycle.Location = New System.Drawing.Point(88, 32)
        Me.tbCycle.Name = "tbCycle"
        Me.tbCycle.ReadOnly = True
        Me.tbCycle.Size = New System.Drawing.Size(80, 20)
        Me.tbCycle.TabIndex = 3
        Me.tbCycle.Text = ""
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 32)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 20)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Cycle :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tbPerf
        '
        Me.tbPerf.Location = New System.Drawing.Point(88, 8)
        Me.tbPerf.Name = "tbPerf"
        Me.tbPerf.ReadOnly = True
        Me.tbPerf.Size = New System.Drawing.Size(80, 20)
        Me.tbPerf.TabIndex = 1
        Me.tbPerf.Text = ""
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(80, 20)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Performance :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tChkHealth
        '
        Me.tChkHealth.AutoReset = False
        Me.tChkHealth.Enabled = True
        Me.tChkHealth.Interval = 250
        Me.tChkHealth.SynchronizingObject = Me
        '
        'BuffTmr1
        '
        Me.BuffTmr1.Interval = 1000
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "Name"
        Me.ColumnHeader10.Width = 63
        '
        'ColumnHeader11
        '
        Me.ColumnHeader11.Text = "Threshold"
        '
        'ColumnHeader12
        '
        Me.ColumnHeader12.Text = "Key"
        Me.ColumnHeader12.Width = 30
        '
        'ColumnHeader13
        '
        Me.ColumnHeader13.Text = "Delay"
        Me.ColumnHeader13.Width = 39
        '
        'ColumnHeader14
        '
        Me.ColumnHeader14.Text = "In Combat"
        '
        'nudSpreadMembersThreshold
        '
        Me.nudSpreadMembersThreshold.Location = New System.Drawing.Point(192, 64)
        Me.nudSpreadMembersThreshold.Name = "nudSpreadMembersThreshold"
        Me.nudSpreadMembersThreshold.Size = New System.Drawing.Size(48, 20)
        Me.nudSpreadMembersThreshold.TabIndex = 27
        Me.nudSpreadMembersThreshold.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label16
        '
        Me.Label16.Location = New System.Drawing.Point(20, 64)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(176, 20)
        Me.Label16.TabIndex = 28
        Me.Label16.Text = "Members Threshold :"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tbSpreadKey
        '
        Me.tbSpreadKey.Location = New System.Drawing.Point(192, 16)
        Me.tbSpreadKey.MaxLength = 1
        Me.tbSpreadKey.Name = "tbSpreadKey"
        Me.tbSpreadKey.Size = New System.Drawing.Size(16, 20)
        Me.tbSpreadKey.TabIndex = 24
        Me.tbSpreadKey.Text = ""
        '
        'tbSpreadName
        '
        Me.tbSpreadName.Location = New System.Drawing.Point(56, 16)
        Me.tbSpreadName.Name = "tbSpreadName"
        Me.tbSpreadName.Size = New System.Drawing.Size(72, 20)
        Me.tbSpreadName.TabIndex = 22
        Me.tbSpreadName.Text = "Spread Heal"
        '
        'nudSpreadDelay
        '
        Me.nudSpreadDelay.Increment = New Decimal(New Integer() {50, 0, 0, 0})
        Me.nudSpreadDelay.Location = New System.Drawing.Point(56, 40)
        Me.nudSpreadDelay.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudSpreadDelay.Name = "nudSpreadDelay"
        Me.nudSpreadDelay.Size = New System.Drawing.Size(72, 20)
        Me.nudSpreadDelay.TabIndex = 25
        Me.nudSpreadDelay.Value = New Decimal(New Integer() {3500, 0, 0, 0})
        '
        'nudSpreadThreshold
        '
        Me.nudSpreadThreshold.Location = New System.Drawing.Point(192, 40)
        Me.nudSpreadThreshold.Name = "nudSpreadThreshold"
        Me.nudSpreadThreshold.Size = New System.Drawing.Size(48, 20)
        Me.nudSpreadThreshold.TabIndex = 23
        Me.nudSpreadThreshold.Value = New Decimal(New Integer() {75, 0, 0, 0})
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(16, 40)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(40, 20)
        Me.Label12.TabIndex = 21
        Me.Label12.Text = "Delay :"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(128, 16)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(64, 20)
        Me.Label13.TabIndex = 20
        Me.Label13.Text = "Key :"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(128, 40)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(64, 20)
        Me.Label14.TabIndex = 19
        Me.Label14.Text = "Threshold :"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(8, 16)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(48, 20)
        Me.Label15.TabIndex = 18
        Me.Label15.Text = "Name :"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cbxEnableSpread
        '
        Me.cbxEnableSpread.Location = New System.Drawing.Point(11, -5)
        Me.cbxEnableSpread.Name = "cbxEnableSpread"
        Me.cbxEnableSpread.Size = New System.Drawing.Size(96, 24)
        Me.cbxEnableSpread.TabIndex = 0
        Me.cbxEnableSpread.Text = "Spread Heals"
        Me.cbxEnableSpread.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.nudSpreadMembersThreshold)
        Me.GroupBox1.Controls.Add(Me.Label16)
        Me.GroupBox1.Controls.Add(Me.tbSpreadKey)
        Me.GroupBox1.Controls.Add(Me.tbSpreadName)
        Me.GroupBox1.Controls.Add(Me.nudSpreadDelay)
        Me.GroupBox1.Controls.Add(Me.nudSpreadThreshold)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Controls.Add(Me.Label15)
        Me.GroupBox1.Controls.Add(Me.cbxEnableSpread)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(288, 96)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Spread Heals"
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(304, 310)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "frmMain"
        Me.Text = "Heal-O-Matic"
        Me.Panel1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.tpMembers.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.tpProfile.ResumeLayout(False)
        Me.TabControl2.ResumeLayout(False)
        Me.tpGroup.ResumeLayout(False)
        Me.tpOther.ResumeLayout(False)
        Me.tpTarget.ResumeLayout(False)
        Me.Panel7.ResumeLayout(False)
        Me.Panel6.ResumeLayout(False)
        CType(Me.nudHealDelay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHealThreshold, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpBuff.ResumeLayout(False)
        Me.tpCures.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.gbMezz.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.tpLog.ResumeLayout(False)
        Me.tbDebug.ResumeLayout(False)
        CType(Me.tChkHealth, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudSpreadMembersThreshold, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudSpreadDelay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudSpreadThreshold, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region
#Region " Classes "
    Class clsMember
        Public Name As String
        Public Index As Short

        Public Health As Byte
        Public PreviousHealth As Byte

        Public Priority As Byte
        Public PriorityHealth As Byte

        Public InRange As Boolean

        Public InGroup As Boolean

        Public Heal As Boolean

        Public LastHealed As Boolean

        Public lvItem As ListViewItem
    End Class
    Class clsHeal
        Public Name As String

        Public Threshold As Integer

        Public Key As Byte
        Public Delay As Integer
        Public InCombat As Boolean
    End Class
    Private Class MezzedMob
        Public MezzTimer As DateTime
        Public ID As Integer
    End Class
    Public Class CompareMobs : Implements IComparer
        Private _AK As AutoKillerScript.clsAutoKillerScript
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Return _AK.ZDistance(_AK.gPlayerXCoord, _AK.gPlayerYCoord, _AK.gPlayerZCoord, _AK.MobXCoord(x), _AK.MobYCoord(x), _AK.MobZCoord(x)) - _
                    _AK.ZDistance(_AK.gPlayerXCoord, _AK.gPlayerYCoord, _AK.gPlayerZCoord, _AK.MobXCoord(y), _AK.MobYCoord(y), _AK.MobZCoord(y))
        End Function
        Public Sub New(ByVal AK As AutoKillerScript.clsAutoKillerScript)
            _AK = AK
        End Sub
        Protected Overrides Sub Finalize()
            _AK = Nothing
            MyBase.Finalize()
        End Sub
    End Class
#End Region
    Public Delegate Sub UpdateText(ByRef aTextBox As Object, ByRef Text As Object)
    Shared Property StopRunning() As Boolean
        Get
            Return mStopRunning
        End Get
        Set(ByVal Value As Boolean)

            mStopRunning = Value

        End Set
    End Property
    Private Function CheckForVaders(ByVal oDAOC As AutoKillerScript.clsAutoKillerScript) As ArrayList 'returns index of mobs that BAF, not including mob that was pulled
        '-----------------------------------------------------------------------------
        ' Name: CheckForVaders()
        ' Desc: Checks for vaders, returns an array of indexes
        ' Author: AutoKiller
        '-----------------------------------------------------------------------------
        Dim CombatModeMob As New ArrayList
        Dim NeedToMez As New ArrayList
        Dim index As Short
        Dim i As Short
        Dim old_dist, new_dist As Double
        Dim SearchDist As Integer = txtSearchDistance.Text
        Dim ISlept As Boolean = False

        With oDAOC
            'radius around the healer
            index = (.FindClosestInvader(SearchDist, .gPlayerXCoord, .gPlayerYCoord))
            If Not index = -1 Then
                CombatModeMob.Add(index)
            End If
            Do While Not index = -1
                If Not ISlept Then 'run this once
                    Threading.Thread.CurrentThread.Sleep(1000)
                    ISlept = True
                End If
                'radius around the healer
                index = (.FindNextClosestInvader(SearchDist, .gPlayerXCoord, .gPlayerYCoord))
                If Not index = -1 Then
                    CombatModeMob.Add(index)
                End If
                Threading.Thread.CurrentThread.Sleep(1)
            Loop

            If CombatModeMob.Count > 0 Then 'check if mob is moving closer
                For i = 0 To CombatModeMob.Count - 1
                    new_dist = .ZDistance(.gPlayerXCoord, .gPlayerYCoord, .gPlayerZCoord, .MobXCoord(CombatModeMob.Item(i)), .MobYCoord(CombatModeMob.Item(i)), .MobZCoord(CombatModeMob.Item(i)))
                    Threading.Thread.CurrentThread.Sleep(250)
                    old_dist = .ZDistance(.gPlayerXCoord, .gPlayerYCoord, .gPlayerZCoord, .MobXCoord(CombatModeMob.Item(i)), .MobYCoord(CombatModeMob.Item(i)), .MobZCoord(CombatModeMob.Item(i)))
                    If old_dist < new_dist Then
                        NeedToMez.Add(CombatModeMob.Item(i))
                    End If
                Next
            End If

        End With

        Return NeedToMez

    End Function
    Private Function CheckForBAF(ByVal oDAOC As AutoKillerScript.clsAutoKillerScript) As ArrayList 'returns index of mobs that BAF, not including mob that was pulled
        ''-----------------------------------------------------------------------------
        '' Name: CheckForBAF()
        '' Desc: Checks for adds, returns an array of mob indexes.
        '' Author: AutoKiller
        ''-----------------------------------------------------------------------------
        'Dim CombatModeMob As New ArrayList
        'Dim NeedToMez As New ArrayList
        'Dim index As Short
        'Dim i As Short
        'Dim old_dist, new_dist As Double
        'Dim SearchDist As Integer = txtSearchDistance.Text
        'Dim ISlept As Boolean = False

        'With oDAOC
        '    If rbGT.Checked Then
        '        'set gt just outside your mez range near mobs being pulled
        '        index = (.FindClosestMobInCombat(SearchDist, , .gtXCoord, .gtYCoord)) 'use ground target
        '    End If
        '    If rbHealerArea.Checked Then
        '        'radius around the healer
        '        index = (.FindClosestMobInCombat(SearchDist, , .gPlayerXCoord, .gPlayerYCoord))
        '    End If
        '    If rbTarget.Checked Then
        '        'this line is if the healer has a target
        '        index = (.FindClosestMobInCombat(SearchDist, .spawnId(.TargetIndex), .MobXCoord(.TargetIndex), .MobYCoord(.TargetIndex))) 'finds mobs in war mode within 1000 radius of mob we pulled, also excludes that mob from search
        '    End If
        '    If Not index = -1 Then
        '        CombatModeMob.Add(index)
        '    End If
        '    Do While Not index = -1
        '        If Not ISlept Then 'run this once
        '            Threading.Thread.CurrentThread.Sleep(1000)
        '            ISlept = True
        '        End If
        '        If rbGT.Checked Then
        '            'set gt just outside your mez range near mobs being pulled
        '            index = (.FindNextClosestMobInCombat(SearchDist, .spawnId(index), .gtXCoord, .gtYCoord)) 'use ground target, exclude first mob
        '        End If
        '        If rbHealerArea.Checked Then
        '            'radius around the healer
        '            index = (.FindNextClosestMobInCombat(SearchDist, .spawnId(index), .gPlayerXCoord, .gPlayerYCoord))
        '        End If
        '        If rbTarget.Checked Then
        '            'this line is if the healer has a target
        '            index = (.FindNextClosestMobInCombat(SearchDist, .spawnId(.TargetIndex), .MobXCoord(.TargetIndex), .MobYCoord(.TargetIndex)))
        '        End If
        '        If Not index = -1 Then
        '            CombatModeMob.Add(index)
        '        End If
        '        Threading.Thread.CurrentThread.Sleep(1)
        '    Loop

        '    If CombatModeMob.Count > 0 Then 'check if mob is moving closer
        '        For i = 0 To CombatModeMob.Count - 1
        '            new_dist = .ZDistance(.gPlayerXCoord, .gPlayerYCoord, .gPlayerZCoord, .MobXCoord(CombatModeMob.Item(i)), .MobYCoord(CombatModeMob.Item(i)), .MobZCoord(CombatModeMob.Item(i)))
        '            Threading.Thread.CurrentThread.Sleep(250)
        '            old_dist = .ZDistance(.gPlayerXCoord, .gPlayerYCoord, .gPlayerZCoord, .MobXCoord(CombatModeMob.Item(i)), .MobYCoord(CombatModeMob.Item(i)), .MobZCoord(CombatModeMob.Item(i)))
        '            If old_dist < new_dist Then
        '                NeedToMez.Add(CombatModeMob.Item(i))
        '            End If
        '        Next
        '    End If

        'End With

        'Return NeedToMez

    End Function
    Private Sub HandleBAF()
        ''-----------------------------------------------------------------------------
        '' Name: HandleBAF()
        '' Desc: Mezzes adds from CheckForBAF or CheckForVaders
        '' Author: AutoKiller
        ''-----------------------------------------------------------------------------
        'Dim Adds As ArrayList
        'Dim i As Short
        'Dim MobID As Integer
        'Dim Mezzed As MezzedMob
        'Dim Keys As New DAOCKeyboard(oDAOC, readSetting("GamePath", GetType(String)))

        'While Not TerminateThread
        '    If IsDAOCActive() AndAlso Not oDAOC.IsPlayerDead Then
        '        Try
        '            If rbRVR.Checked Then
        '                Adds = CheckForVaders(oDAOC)
        '            Else
        '                Adds = CheckForBAF(oDAOC)
        '            End If

        '            Debug.WriteLine("Count: " & CStr(Adds.Count))
        '            If Adds.Count > 1 Then 'if there's just 1 no need to mez
        '                LogLine("Count: " & CStr(Adds.Count))
        '                Adds.Sort(New CompareMobs(oDAOC)) 'sort by distance

        '                For i = Adds.Count - 1 To 1 Step -1 'Mezz all but the closest
        '                    Debug.WriteLine(i)
        '                    Try
        '                        For Each Mezzed In MezzedList.Values
        '                            If MezzedList.ContainsKey(Mezzed.ID) Then 'we mezzed already
        '                                If Mezzed.MezzTimer <= DateTime.Now Then 'ok to mez now
        '                                    MezzedList.Remove(Mezzed.ID)
        '                                Else
        '                                    Exit Try
        '                                End If
        '                            End If
        '                        Next

        '                        oDAOC.SelectTarget(Adds.Item(i)) 'select farthest
        '                        Threading.Thread.CurrentThread.Sleep(500)

        '                        'make sure target is in view and < 1500 all realms mezz should be 1500
        '                        'if not try next target
        '                        If Not oDAOC.QueryString(0) AndAlso oDAOC.ZDistance(oDAOC.gPlayerXCoord, oDAOC.gPlayerYCoord, oDAOC.gPlayerZCoord, oDAOC.MobXCoord(oDAOC.TargetIndex), oDAOC.MobYCoord(oDAOC.TargetIndex), oDAOC.MobZCoord(oDAOC.TargetIndex)) <= 1500 Then
        '                            Keys.TurnLeft() 'break stick
        '                            StopRunning = True 'stop globalposition from running
        '                            oDAOC.StopRunning() 'double check it

        '                            'face
        '                            Keys.Face()
        '                            Threading.Thread.CurrentThread.Sleep(500)
        '                            'oDAOC.TurnToHeading(oDAOC.FindHeading(oDAOC.gPlayerXCoord, oDAOC.gPlayerYCoord, oDAOC.MobXCoord(oDAOC.TargetIndex), oDAOC.MobYCoord(oDAOC.TargetIndex)))
        '                            oDAOC.SendString(txtMezzKey.Text) 'send mezz key

        '                            Dim M As New MezzedMob
        '                            M.MezzTimer = DateTime.Now.AddMinutes(1) 'mezz timer
        '                            M.ID = oDAOC.spawnId(Adds.Item(i))
        '                            MezzedList.Add(M.ID, M)

        '                            LogLine("Mezzing " & oDAOC.MobName(oDAOC.TargetIndex))
        '                            Threading.Thread.CurrentThread.Sleep(2000) 'sleep while casting
        '                            Do Until oDAOC.PlayerCasting = 0
        '                                Threading.Thread.CurrentThread.Sleep(1)
        '                            Loop

        '                            StopRunning = False
        '                        End If
        '                    Catch ex As Exception
        '                        Debug.WriteLine(ex.Message)
        '                    End Try
        '                Next
        '            End If
        '            Threading.Thread.CurrentThread.Sleep(500)
        '        Catch ex As Exception
        '            LogLineAsync("Error in HandleBAF")
        '        End Try
        '    End If
        '    Threading.Thread.CurrentThread.Sleep(500)
        'End While
    End Sub
    Public Sub UpdateTextBox(ByRef aTextBox As Object, ByRef Text As Object)
        If TypeName(aTextBox) = "TextBox" Then
            CType(aTextBox, TextBox).Text = CStr(Text)
        ElseIf TypeName(aTextBox) = "Button" Then
            CType(aTextBox, Button).Text = CStr(Text)
        End If
    End Sub
    Public Function readSetting(ByVal key As String, ByVal type As Type)
        Dim configurationAppSettings As System.Configuration.AppSettingsReader = New System.Configuration.AppSettingsReader
        Return configurationAppSettings.GetValue(key, type)
    End Function
    Public Delegate Sub UpdateTitle(ByRef Text As Object)
    Public Sub UpdateFormTitle(ByRef Text As Object)
        Me.Text = CStr(Text)
    End Sub
    Public Function IsDAOCActive() As Boolean
        If GetForegroundWindow = FindWindow("DAocMWC", Nothing) Then
            Dim mi As New UpdateTitle(AddressOf UpdateFormTitle)
            Me.BeginInvoke(mi, New Object() {"Heal-O-Matic (Active)"})
            'Text = "Heal-O-Matic (Active)"

            Return True
        Else
            Dim mi As New UpdateTitle(AddressOf UpdateFormTitle)
            Me.BeginInvoke(mi, New Object() {"Heal-O-Matic (Paused)"})
            'Text = "Heal-O-Matic (Active)"

            Return False
        End If
    End Function
    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        If IsNothing(HealThread) Then
            TerminateThread = False

            HealThread = New Threading.Thread(AddressOf HealThreadSub)
            HealThread.Name = "HealThread"
            HealThread.Start()

            btnLoad.Text = "Stop"

        Else
            Text = "Heal-O-Matic"

            btnLoad.Text = "Start"

            TerminateThread = True

            HealThread.Join()
            HealThread = Nothing
        End If
    End Sub
    Public Delegate Sub LogIt(ByVal Line As String)
    Sub LogLineAsync(ByVal Line As String)
        Dim mi As New LogIt(AddressOf LogLine)
        Me.BeginInvoke(mi, New Object() {Line})
    End Sub
    Sub LogLine(ByVal Line As String)
        Line = Format(Year(Now), "0000") & "-" & Format(Month(Now), "00") & "-" & Format(Microsoft.VisualBasic.Day(Now), "00") & "|" & Format(Hour(Now), "00") & ":" & Format(Minute(Now), "00") & ":" & Format(Second(Now), "00") & "| " & Line
        lbLog.BeginUpdate()
        lbLog.Items.Insert(0, Line)

        If lbLog.Items.Count > 128 Then
            lbLog.Items.RemoveAt(0)
        End If
        lbLog.EndUpdate()
    End Sub
    ' Provide a more flikker free listview (only update if needed)
    Private Sub UpdateLvMembers()
        Dim Member As clsMember
        Dim lvItem As ListViewItem
        Dim Counter As Integer


        While lvMembers.Items.Count > htMembers.Values.Count
            lvMembers.Items.RemoveAt(0)
        End While

        While lvMembers.Items.Count < htMembers.Values.Count
            lvItem = lvMembers.Items.Add("")
            lvItem.SubItems.Add("")
            lvItem.SubItems.Add("")
            lvItem.SubItems.Add("")
        End While

        Counter = 0
        For Each Member In htMembers.Values
            lvItem = lvMembers.Items(Counter)
            If lvItem.Text <> Member.Name Then
                lvItem.Text = Member.Name
            End If

            If lvItem.Checked <> Member.Heal Then
                lvItem.Checked = Member.Heal
            End If

            Select Case Member.Priority
                Case 0
                    If lvItem.SubItems(1).Text <> "Normal" Then
                        lvItem.SubItems(1).Text = "Normal"
                    End If
                Case 1
                    If lvItem.SubItems(1).Text <> "High" Then
                        lvItem.SubItems(1).Text = "High"
                    End If
                Case 2
                    If lvItem.SubItems(1).Text <> "Low" Then
                        lvItem.SubItems(1).Text = "Low"
                    End If
            End Select

            If Member.Index <> -1 Then
                If Member.InRange Then
                    If lvItem.SubItems(2).Text <> Member.Health & "%" Then
                        lvItem.SubItems(2).Text = Member.Health & "%"
                    End If
                Else
                    If lvItem.SubItems(2).Text <> Member.Health & "% *" Then
                        lvItem.SubItems(2).Text = Member.Health & "% *"
                    End If
                End If

                If lvItem.SubItems(3).Text <> Member.Index & "" Then
                    lvItem.SubItems(3).Text = Member.Index
                End If
            Else
                If lvItem.SubItems(2).Text <> "" Then
                    lvItem.SubItems(2).Text = ""
                End If

                If lvItem.SubItems(3).Text <> "" Then
                    lvItem.SubItems(3).Text = ""
                End If
            End If

            If Not lvItem.Tag Is Member Then
                lvItem.Tag = Member
            End If
            Member.lvItem = lvItem

            Counter = Counter + 1
        Next
    End Sub
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If tbName.Text <> "" Then
            Dim Member As clsMember

            ' Check if it wasnt already in the list
            Member = htMembers(tbName.Text)
            If IsNothing(Member) Then
                Member = New clsMember
                Member.Name = tbName.Text
                Member.Heal = True
                Member.InGroup = False
                Member.Index = -1
                Member.Priority = cbxPriority.SelectedIndex

                htMembers.Add(tbName.Text, Member)
            End If

            tbName.Text = ""
        Else
            Dim oDAOC As New AutoKillerScript.clsAutoKillerScript

            oDAOC.ChatLog = readSetting("ChatLog", GetType(String))
            oDAOC.RegKey = readSetting("RegKey", GetType(String))
            oDAOC.EnableEuro = readSetting("EnableEuro", GetType(Boolean))
            oDAOC.EnableCatacombs = readSetting("EnableCatacombs", GetType(Boolean))
            oDAOC.EnableToA = readSetting("EnableToA", GetType(Boolean))
            'oDAOC.EnableClassic = readSetting("EnableClassic", GetType(Boolean))
            oDAOC.DoInit()

            Dim index As Short = oDAOC.TargetIndex
            If index <> -1 Then
                Dim Name As String
                Name = oDAOC.MobName(index)
                Dim Member As clsMember

                ' Check if it wasnt already in the list
                Member = htMembers(Name)
                If IsNothing(Member) OrElse (index <> member.index) Then
                    Member = New clsMember
                    Member.Name = Name
                    Member.Heal = True
                    Member.InGroup = False
                    Member.Index = index
                    Member.Priority = cbxPriority.SelectedIndex

                    htMembers.Add(Name, Member)
                End If
            End If

            oDAOC.StopInit()
        End If

        UpdateLvMembers()
    End Sub
    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim lvItem As ListViewItem
        If lvMembers.SelectedItems.Count > 0 Then
            Dim Member As clsMember
            For Each lvItem In lvMembers.SelectedItems
                Member = lvItem.Tag
                htMembers.Remove(Member.Name)
            Next
        End If

        UpdateLvMembers()
    End Sub
    Sub CastGroupHeal(ByVal oDAOC As AutoKillerScript.clsAutoKillerScript, ByVal Heal As Byte, ByVal Delay As Integer)
        If oDAOC.isPlayerSitting <> 0 Then
            LogLineAsync(tbHealer.Text & " is sitting, standing up to cast.")
            oDAOC.SendKeys(K_Sit, 0)

            ' Delay for standing up
            HealThread.Sleep(100)
            TotalSleep = TotalSleep + 100
        End If

        oDAOC.SendKeys(Heal, 0)

        HealThread.Sleep(Delay)
        TotalSleep = TotalSleep + Delay

        LogLineAsync("Group Healed")
    End Sub
    Sub CastTargetHeal(ByVal oDAOC As AutoKillerScript.clsAutoKillerScript, ByVal Member As clsMember, ByVal Heal As Byte, ByVal Delay As Integer)
        If oDAOC.isPlayerSitting <> 0 Then
            LogLineAsync(tbHealer.Text & " is sitting, standing up to cast.")
            oDAOC.SendKeys(K_Sit, 0)

            ' Delay for standing up
            HealThread.Sleep(100)
            TotalSleep = TotalSleep + 100
        Else
            ' Delay for targeting (done outside this function)
            HealThread.Sleep(100)
            TotalSleep = TotalSleep + 100
        End If

        If oDAOC.TargetIndex = Member.Index Then
            oDAOC.SendKeys(Heal, 0)

            HealThread.Sleep(Delay)
            TotalSleep = TotalSleep + Delay

            LogLineAsync("Healed " & Member.Name & " from " & Member.Health & "% HP to " & oDAOC.MobHealth(Member.Index) & "% HP")

            If oDAOC.QueryString(2) Then
                oDAOC.SendKeys(K_Poison)
                HealThread.Sleep(2500)
                TotalSleep = TotalSleep + 2500
            End If

            If oDAOC.QueryString(1) Then
                oDAOC.SendKeys(K_Disease)
                HealThread.Sleep(2500)
                TotalSleep = TotalSleep + 2500
            End If
        Else
            LogLineAsync("Target index is invalid (" & Member.Index & ")(" & oDAOC.TargetIndex & ")")
        End If
    End Sub
    Sub CastBuff(ByVal oDAOC As AutoKillerScript.clsAutoKillerScript, ByVal target As Integer, ByVal Buff As Byte, ByVal Delay As Integer)
        If oDAOC.isPlayerSitting <> 0 Then
            LogLineAsync(tbHealer.Text & " is sitting, standing up to cast.")
            oDAOC.SendKeys(K_Sit, 0)

            ' Delay for standing up
            HealThread.Sleep(100)
            TotalSleep = TotalSleep + 100
        Else
            ' Delay for targeting (done outside this function)
            HealThread.Sleep(100)
            TotalSleep = TotalSleep + 100
        End If

        If oDAOC.TargetIndex = target Then
            LogLineAsync("Sending Buff key " & Buff)
            oDAOC.SendString(Buff)
            HealThread.Sleep(Delay)
            TotalSleep = TotalSleep + Delay

        Else
            LogLineAsync("Target index is invalid (" & target & ")(" & oDAOC.TargetIndex & ")")
        End If

    End Sub
    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LogLine("Heal-O-Matic v0.92 (c) Da_Teach")

        Try
            System.Diagnostics.Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.AboveNormal
        Catch Ex As Exception
            ' On Windows 98 machines you get an error for 'above normal'
            System.Diagnostics.Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.High
        End Try

        cbxPriority.SelectedIndex = 0

        UpdateProfiles()
        Try
            Dim C As Integer
            For C = 0 To cbxProfiles.Items.Count - 1
                If cbxProfiles.Items.Item(C).ToLower = "default" Then
                    cbxProfiles.SelectedIndex = C
                End If
            Next
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try


        Threading.Thread.CurrentThread.Name = "MainThread"
    End Sub
    Private Sub btnChangePrio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangePrio.Click
        Dim lvItem As ListViewItem
        If lvMembers.SelectedItems.Count > 0 Then
            Dim Member As clsMember
            Member = lvMembers.SelectedItems(0).Tag

            Member.Priority = Member.Priority + 1
            If Member.Priority > 2 Then
                Member.Priority = 0
            End If
        End If

        UpdateLvMembers()
    End Sub
    Private Sub tbName_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbName.KeyUp
        If e.KeyCode = Keys.Enter Then
            btnAdd_Click(Nothing, Nothing)
        End If
    End Sub
    Private Sub HealThreadSub()
        Dim oLastYelled As DateTime = Now
        Dim Heals As Collection = New Collection
        oDAOC = New AutoKillerScript.clsAutoKillerScript
        Dim DeBuffCheck As Boolean
        Dim TempRecord As String
        Dim Keys As New DAOCKeyboard(oDAOC, readSetting("GamePath", GetType(String)))

        'dgb
        Dim blnEffectsFlag As Boolean = False

        AddHandler oDAOC.OnLog, AddressOf LogLine

        Try
            LogLineAsync("Initializing DAOCScript v" & oDAOC.getVersion & "....")

            WndMngr = New DAOCWindowManager
            WndMngr.CharacterName = tbHealer.Text
            ' This section sets up various variables for the DLL.
            oDAOC.ChatLog = readSetting("ChatLog", GetType(String))

            WndMngr.DAOCPath = readSetting("GamePath", GetType(String))
            WndMngr.ServerIP = readSetting("ServerIP", GetType(String))
            oDAOC.RegKey = readSetting("RegKey", GetType(String))
            oDAOC.EnableEuro = readSetting("EnableEuro", GetType(Boolean))
            oDAOC.EnableCatacombs = readSetting("EnableCatacombs", GetType(Boolean))
            oDAOC.EnableToA = readSetting("EnableToA", GetType(Boolean))

            'dgb
            oDAOC.GamePath = readSetting("GamePath", GetType(String))
            'oDAOC.EnableAutoQuery = False
            'oDAOC.UseRegEx = True

            'keys
            oDAOC.SetLeftTurnKey = Keys.TurnLeft_Key
            oDAOC.SetRightTurnKey = Keys.TurnRight_Key
            oDAOC.SetConsiderKey = Keys.Consider_Key

            bytShowInventoryKey = Keys.ShowInventory_Key
            bytMoveForwardKey = Keys.MoveForward1_Key
            bytMoveBackwardKey = Keys.MoveBackward2_Key
            bytSitKey = Keys.Sit_Stand_Key
            bytStickKey = Keys.Stick_Key
            bytFaceKey = Keys.Face_Key

            K_Sit = Keys.Sit_Stand_Key

            TmrCount = 0

            oDAOC.DoInit()

            oDAOC.AddString(0, "Target is not in view*")
            oDAOC.AddString(1, "Your target is diseased*")
            oDAOC.AddString(2, "Your target is poisoned*")
            oDAOC.AddString(3, "*You are hit for*")
            oDAOC.AddString(4, "*not enough power*")

            oDAOC.AddString(5, "@@[Party] Oexon: ""af me noobs""")
            oDAOC.AddString(6, "@@[Party] Oexon: ""ok, wait here""")
            oDAOC.AddString(7, "Oexon has invited you to join a group.")
            oDAOC.AddString(8, "@@[Party] Oexon: ""zone into td""")
            oDAOC.AddString(9, "@@[Party] Oexon: ""buff up""")
            oDAOC.AddString(10, "@@[Party] Oexon: ""zone out of td""")
            oDAOC.AddString(11, "@@Oexon sends, ""disband""")

            LogLineAsync("Strings set...")

            If CheckBAF Then
                BAFThread = New Threading.Thread(AddressOf HandleBAF)
                BAFThread.Start()
            End If

            Dim CurrentX As Integer
            Dim CurrentY As Integer
            Dim CurrentZ As Integer

            Dim LastMove As DateTime = Now
            Dim SendSitKey As DateTime = Now()

            Dim Encoder As New System.Text.UTF8Encoding

            Dim HasSpread As Boolean = cbxEnableSpread.Checked
            Dim K_Spread As Byte = Encoder.GetBytes(tbSpreadKey.Text)(0)
            Dim SpreadThreshold As Integer = nudSpreadThreshold.Value
            Dim SpreadDelay As Integer = nudSpreadDelay.Value
            Dim SpreadCount As Integer = nudSpreadMembersThreshold.Value
            Dim SpreadName As String = tbSpreadName.Text

            ' Probably add different profiles
            Dim Heal As clsHeal
            Dim lvItem As ListViewItem
            For Each lvItem In lvHeals.Items
                Heal = New clsHeal
                Heal.Name = lvItem.SubItems(0).Text
                Heal.Delay = lvItem.SubItems(3).Text
                Heal.Key = Encoder.GetBytes(lvItem.SubItems(2).Text)(0)
                Heal.InCombat = lvItem.SubItems(4).Text = "Yes"
                Heal.Threshold = lvItem.SubItems(1).Text
                Heals.Add(Heal)
            Next

            'Dim objGroupMemberInfo As AKServer.DLL.DAoCServer.Group = oDAOC.GroupMemberInfo
            'Dim htGroupMemberTable As Hashtable = objGroupMemberInfo.GroupMemberTable
            'Dim htGroup As Hashtable = New Hashtable
            'Dim Remove As Collection = New Collection

            While Not TerminateThread
                PerfMon = Now
                TotalSleep = 0

                If IsDAOCActive() AndAlso Not oDAOC.IsPlayerDead Then

                    If Not blnEffectsFlag Then
                        blnEffectsFlag = True

                        LogLineAsync("Setting /effects to none")
                        HealThread.CurrentThread.Sleep(2000)
                        oDAOC.SendString("/effects none~")
                        HealThread.CurrentThread.Sleep(2000)
                    End If

                    Dim InCombat As Boolean = False

                    Dim Found As Integer

                    Buddy = oDAOC.TargetIndex

                    Dim LastHealed As clsMember = Nothing
                    Dim HealMember As clsMember = Nothing
                    Dim Member As clsMember = Nothing
                    Dim LastFailed As String = ""    ' try to heal, if fail, set this and next loop skip this one

                    Dim K_Heal As Byte = 0

                    Dim HealDelay As Integer = 0
                    Dim HealName As String

                    Dim SingleTarget As Boolean = True
                    Dim HasMoved As Boolean = False

                    Dim MembersNeedHeal As Integer = 0 ' Used for SpreadHeal

                    If CurrentX <> oDAOC.gPlayerXCoord Then
                        CurrentX = oDAOC.gPlayerXCoord
                        LastMove = Now
                    End If

                    If CurrentY <> oDAOC.gPlayerYCoord Then
                        CurrentY = oDAOC.gPlayerYCoord
                        LastMove = Now
                    End If
                    CurrentZ = oDAOC.gPlayerZCoord

                    ' Protection for 'casting on the move'
                    Try
                        HasMoved = Now.Subtract(LastMove).TotalMilliseconds < 500
                    Catch Ex As Exception
                        HasMoved = True
                    End Try
                    'Update
                    Dim mi As New UpdateText(AddressOf UpdateTextBox)
                    Me.BeginInvoke(mi, New Object() {tbHasMoved, HasMoved})
                    'tbHasMoved.Text = HasMoved

                    If cbxAutoGroup.Checked Then
                        'DGB
                        'Dim Group As ArrayList = oDAOC.GroupNames(7)
                        'Dim Group As ArrayList = oDAOC.GroupMemberInfo
                        Dim objGroupMemberInfo As AKServer.DLL.DAoCServer.Group = oDAOC.GroupMemberInfo
                        Dim htGroupMemberTable As Hashtable = objGroupMemberInfo.GroupMemberTable

                        Dim newS As AKServer.DLL.DAoCServer.GroupMember
                        Dim htGroup As Hashtable = New Hashtable
                        Dim Remove As Collection = New Collection

                        Dim S As String
                        Dim oldS As String


                        'LogLineAsync("Before For...")
                        For Each newS In htGroupMemberTable.Values
                            'LogLineAsync("After For...")

                            If newS.Name <> "" Then
                                oldS = newS.Name
                                htGroup.Add(oldS.Clone, oldS.Clone)
                                Member = htMembers(CStr(newS.Name))
                                If IsNothing(Member) Then
                                    Member = New clsMember
                                    Member.Name = newS.Name
                                    Member.Heal = True
                                    Member.InGroup = True
                                    Member.Index = -1
                                    'Member.Index = newS.ID
                                    Member.Priority = 0
                                    htMembers.Add(CStr(newS.Name), Member)
                                Else
                                    Member.InGroup = True
                                End If
                            End If
                        Next

                        For Each Member In htMembers.Values
                            If Member.InGroup Then
                                If Not htGroup.ContainsKey(Member.Name) Then
                                    Remove.Add(Member.Name)
                                End If
                            End If
                        Next

                        For Each oldS In Remove
                            htMembers.Remove(oldS)
                        Next
                    End If

                    For Each Member In htMembers.Values
                        If Member.Name = tbHealer.Text Then
                            'dgb
                            'Member.Index = oDAOC.PlayerIndex
                            Member.Index = oDAOC.PlayerID
                        Else
                            'dgb
                            'Member.Index = oDAOC.SetTarget(Member.Name, , False)
                            Member.Index = oDAOC.SetTarget(Member.Name, False)
                        End If

                        If Member.LastHealed Then
                            LastHealed = Member
                            Member.LastHealed = False
                        End If

                        If Member.Index <> -1 Then
                            'LogLine("Name: " & Member.Name)
                            'LogLine("Member.Index: " & Member.Index)
                            'LogLine("Member.Health " & Member.Health)
                            'LogLine("MobHealth: " & CStr(oDAOC.MobHealth(Member.Index)))

                            Member.PreviousHealth = Member.Health
                            'Member.Health = oDAOC.MobHealth(Member.Index)

                            If Member.Name = tbHealer.Text Then
                                Member.Health = oDAOC.PlayerHealth
                            Else
                                Member.Health = oDAOC.MobHealth(Member.Index)

                            End If

                            'LogLine("Name: " & Member.Name)
                            'LogLine("Member.Index: " & Member.Index)
                            'LogLine("Member.Health " & Member.Health)
                            'LogLine("MobHealth: " & CStr(oDAOC.MobHealth(Member.Index)))

                            'dgb
                            'If Member.Index = oDAOC.PlayerIndex Then
                            If Member.Index = oDAOC.PlayerID Then
                                Member.InRange = True
                            Else
                                'FIX THIS ???
                                Member.InRange = oDAOC.ZDistance(CurrentX, CurrentY, CurrentZ, oDAOC.MobXCoord(Member.Index), oDAOC.MobYCoord(Member.Index), oDAOC.MobZCoord(Member.Index)) < 2000
                            End If

                            Member.PriorityHealth = Member.Health
                            If Member.Health <> 100 AndAlso Member.Priority = 1 Then
                                Member.PriorityHealth = Member.Health - Member.Health / 3
                            ElseIf Member.Health <> 100 AndAlso Member.Priority = 2 Then
                                Member.PriorityHealth = Member.Health + ((100 - Member.Health) / 4)
                            End If

                            'If Member.Health > 0 And Member.Health < 100 And Member.InRange And Member.Heal Then
                            If Member.Health < 100 AndAlso Member.InRange AndAlso Member.Heal Then ' Add Rez as a 0 health Heal ???
                                If IsNothing(HealMember) AndAlso (LastFailed <> Member.Name) Then
                                    HealMember = Member
                                ElseIf Member.PriorityHealth < HealMember.PriorityHealth AndAlso (LastFailed <> Member.Name) Then
                                    HealMember = Member
                                End If
                            End If

                            If Member.InGroup AndAlso Member.Health < SpreadThreshold AndAlso Member.Health > 0 AndAlso Member.InRange AndAlso Member.Heal AndAlso (LastFailed <> Member.Name) Then
                                MembersNeedHeal = MembersNeedHeal + 1
                            End If

                            InCombat = InCombat OrElse (oDAOC.isMobInCombat(Member.Index) <> 0)
                        End If
                    Next

                    If BuffTmr1.Enabled = True AndAlso MembersNeedHeal = 0 Then
                        If TmrCount = 0 OrElse TmrCount = K_BuffTime Then
                            'dgb
                            'oDAOC.SelectTarget(Buddy)
                            oDAOC.SetTarget(Buddy)
                            LogLineAsync("Buff Routine Starting")
                            CastBuff(oDAOC, Buddy, K_Buff1, 3000)
                            If Buff2 Then
                                CastBuff(oDAOC, Buddy, K_Buff2, 3000)
                            End If
                        End If
                        If TmrCount >= K_BuffTime Then
                            TmrCount = 1
                        End If
                    End If

                    Dim mi2 As New MethodInvoker(AddressOf UpdateLvMembers)
                    Me.BeginInvoke(mi2)

                    Dim DidHeal As Boolean = False
                    If IsDAOCActive() AndAlso Not IsNothing(HealMember) AndAlso oDAOC.QueryString(4) = False Then
                        ' Simple SpreadHeal 'hack', might revise
                        If MembersNeedHeal > SpreadCount AndAlso HasSpread AndAlso HealMember.InGroup Then
                            SingleTarget = False

                            K_Heal = K_Spread
                            HealDelay = SpreadDelay
                            HealName = SpreadName
                        Else       ' actual healing decided
                            For Each Heal In Heals
                                If HealMember.Health < Heal.Threshold Then
                                    If Heal.InCombat OrElse Not InCombat Then
                                        K_Heal = Heal.Key
                                        HealDelay = Heal.Delay
                                        HealName = Heal.Name
                                    End If
                                ElseIf HealMember.Health = 0 AndAlso Heal.Threshold = 0 Then       ' Dead Member
                                    If Heal.InCombat OrElse Not InCombat Then
                                        K_Heal = Heal.Key
                                        HealDelay = Heal.Delay
                                        HealName = Heal.Name
                                    End If
                                Else
                                    ' Previous heal wasnt good enough since current health is lower then before the heal
                                    ' (Do not do this if the member in question is the healer, since he was most likely interrupted)
                                    If HealMember.Health < HealMember.PreviousHealth AndAlso HealMember Is LastHealed AndAlso HealMember.Name <> tbHealer.Text Then
                                        If Heal.InCombat OrElse Not InCombat Then
                                            K_Heal = Heal.Key
                                            HealDelay = Heal.Delay
                                            HealName = Heal.Name
                                        End If
                                    End If

                                    Exit For
                                End If
                            Next
                        End If

                        If K_Heal <> 0 Then
                            If Not HasMoved Then
                                ' Always target the member that needs a heal the most
                                ' this way you can instant-heal the person on the move
                                ' or when spread heal is done.
                                Dim OrgIndex As Short = HealMember.Index
                                'dgb
                                'oDAOC.SelectTarget(HealMember.Index)
                                oDAOC.SetTarget(HealMember.Index)
                                HealMember.Index = OrgIndex

                                If SingleTarget Then
                                    LogLineAsync("Casting: " & HealName & " on " & HealMember.Name)
                                    HealMember.lvItem.BackColor = Color.Green
                                    Try
                                        CastTargetHeal(oDAOC, HealMember, K_Heal, HealDelay)

                                        HealMember.LastHealed = True
                                    Finally
                                        HealMember.lvItem.BackColor = Color.White
                                    End Try
                                Else
                                    LogLineAsync("Casting: " & HealName)
                                    CastGroupHeal(oDAOC, K_Heal, HealDelay)
                                End If

                                ' added check for not enough power when casting
                                If oDAOC.QueryString(4) Then
                                    If SingleTarget Then
                                        LastFailed = HealMember.Name
                                    Else
                                        LastFailed = ""
                                    End If
                                    DidHeal = False
                                Else
                                    LastFailed = ""
                                    DidHeal = True
                                End If
                            Else
                                If SingleTarget Then
                                    LogLineAsync("Has moved, cant cast " & HealName & " on " & HealMember.Name & "!")
                                Else
                                    LogLineAsync("Has moved, cant cast " & HealName & " on group!")
                                End If
                            End If
                        End If
                    End If

                    If cbSitMode.SelectedIndex > 0 Then
                        Dim iSendSitKey As Integer
                        Try
                            iSendSitKey = Now.Subtract(SendSitKey).TotalMilliseconds
                        Catch Ex As Exception
                            iSendSitKey = 0
                        End Try

                        ' If we are walking do not attempt to sit and do not attempt
                        ' to sit if a sit command has been issued in the last second
                        ' (to avoid sit/stand/sit/stand/sit/stand issues)
                        If Not HasMoved AndAlso iSendSitKey > 1000 Then
                            Dim PlayerMana As Integer = oDAOC.PlayerMana
                            Dim isPlayerSitting As Boolean = (oDAOC.isPlayerSitting <> 0)

                            If cbSitMode.SelectedIndex = 2 OrElse (Not InCombat OrElse PlayerMana < 10) Then
                                If (Not DidHeal AndAlso Not isPlayerSitting AndAlso PlayerMana < 90 AndAlso oDAOC.PlayerHealth > 0) Then
                                    LogLineAsync(tbHealer.Text & " is standing, sitting to regenerate.")
                                    oDAOC.SendKeys(K_Sit, 0)


                                    HealThread.Sleep(250)
                                    TotalSleep = TotalSleep + 250

                                    SendSitKey = Now()
                                End If
                            End If

                            If (PlayerMana > 90 AndAlso isPlayerSitting) OrElse (PlayerMana > 20 AndAlso InCombat AndAlso isPlayerSitting AndAlso cbSitMode.SelectedIndex = 1) Then
                                LogLineAsync(tbHealer.Text & " is sitting, standing up.")
                                oDAOC.SendKeys(K_Sit, 0)

                                HealThread.Sleep(250)
                                TotalSleep = TotalSleep + 250

                                SendSitKey = Now()
                            End If
                        End If
                    End If

                    'StickToGroup(oDAOC)

                    'oDAOC.AddString(5, "@@[Party] Oexon: ""af me noobs""")
                    'oDAOC.AddString(6, "@@[Party] Oexon: ""ok, wait here""")
                    'oDAOC.AddString(7, "Oexon has invited you to join a group.")
                    'oDAOC.AddString(8, "@@[Party] Oexon: ""zone into td""")
                    'oDAOC.AddString(9, "@@[Party] Oexon: ""buff up""")
                    'oDAOC.AddString(10, "@@[Party] Oexon: ""zone out of td""")
                    'oDAOC.AddString(11, "@@Oexon sends, ""disband""")

                    '5: AF target
                    If oDAOC.QueryString(5) = True Then
                        LogLineAsync("AF target")

                        oDAOC.SetTarget("Oexon", True)
                        HealThread.CurrentThread.Sleep(500)

                        oDAOC.SendKeys(bytStickKey, 0)
                        HealThread.CurrentThread.Sleep(500)

                    End If

                    '6: Break AF
                    If oDAOC.QueryString(6) = True Then
                        LogLineAsync("Breaking AF")

                        oDAOC.SendKeys(bytMoveBackwardKey, True)
                        HealThread.CurrentThread.Sleep(500)
                        oDAOC.SendKeys(bytMoveBackwardKey, False, True)
                    End If

                    '7: Group invite
                    If oDAOC.QueryString(7) = True Then
                        LogLineAsync("Group invite")
                        Dim dlg As AutoKillerScript.WindowManager = _
                        New AutoKillerScript.WindowManager(oDAOC, AutoKillerScript.WINDOW_NAMES.Dialog)

                        oDAOC.MouseMove(dlg.Left + 125, dlg.Top + 85)
                        HealThread.CurrentThread.Sleep(500)
                        oDAOC.LeftClick()
                        dlg = Nothing

                        HealThread.CurrentThread.Sleep(5000)
                    End If

                    '8: Zone into td
                    If oDAOC.QueryString(8) = True Then
                        LogLine("Zone into td")
                        ZoneIntoTD("Dark_Cavern", 334724, 587960, 8569, 100, 15022, 39060, 3000, oDAOC)
                        HealThread.Sleep(15000)
                    End If

                    '9: Buffing
                    If oDAOC.QueryString(9) = True Then
                        LogLineAsync("Buff up")

                        Dim objEncoder As New System.Text.UTF8Encoding
                        Dim bytStrConkey As Byte
                        Dim bytDexQuiKey As Byte

                        Dim shtTarget As Short

                        bytStrConkey = objEncoder.GetBytes("5")(0)
                        bytDexQuiKey = objEncoder.GetBytes("6")(0)

                        Dim x As Integer

                        For x = 1 To 4
                            Select Case x
                                Case 1
                                    LogLineAsync("Buffing Oexon")
                                    HealThread.CurrentThread.Sleep(500)
                                    shtTarget = oDAOC.SetTarget("Oexon", True)
                                    HealThread.CurrentThread.Sleep(500)

                                Case 2
                                    LogLineAsync("Buffing Ociz")
                                    HealThread.CurrentThread.Sleep(500)
                                    shtTarget = oDAOC.SetTarget("Ociz", True)
                                    HealThread.CurrentThread.Sleep(500)

                                Case 3
                                    LogLineAsync("Buffing Shroomies")
                                    HealThread.CurrentThread.Sleep(500)
                                    shtTarget = oDAOC.SetTarget("Shroomies", True)
                                    HealThread.CurrentThread.Sleep(500)

                                Case 4
                                    LogLineAsync("Buffing Shroomiez")
                                    HealThread.CurrentThread.Sleep(500)
                                    shtTarget = oDAOC.SetTarget("Shroomiez", True)
                                    HealThread.CurrentThread.Sleep(500)
                            End Select


                            If shtTarget <> -1 Then
                                'str/con
                                LogLineAsync("Casting str/con")
                                HealThread.CurrentThread.Sleep(500)
                                oDAOC.SendKeys(bytStrConkey, False)
                                HealThread.CurrentThread.Sleep(3000)
                                HealThread.CurrentThread.Sleep(1000)

                                'dex/qui
                                LogLineAsync("Casting dex/qui")
                                HealThread.CurrentThread.Sleep(500)
                                oDAOC.SendKeys(bytDexQuiKey, False)
                                HealThread.CurrentThread.Sleep(3000)
                                HealThread.CurrentThread.Sleep(1000)
                            End If
                        Next
                    End If

                    '10: Zone out of td
                    If oDAOC.QueryString(10) = True Then
                        LogLineAsync("Zone out of td")

                        LogLineAsync("/disband to zone out")
                        HealThread.CurrentThread.Sleep(500)
                        oDAOC.SendString("/disband~")
                        HealThread.CurrentThread.Sleep(500)

                        HealThread.CurrentThread.Sleep(1500)
                    End If

                    '11: private tell disband
                    If oDAOC.QueryString(11) = True Then
                        LogLineAsync("Disband")

                        HealThread.CurrentThread.Sleep(500)
                        oDAOC.SendString("/disband~")
                        HealThread.CurrentThread.Sleep(500)

                        HealThread.CurrentThread.Sleep(1500)
                    End If

                ElseIf oDAOC.IsPlayerDead = 1 Then
                    ' accept rez / wait for rez
                    WaitForRez(oDAOC)
                End If

                If oDAOC.QueryString(3) = True And cbxQuiet.Checked = False Then
                    If oLastYelled < Now.AddSeconds(-30) Then
                        oLastYelled = Now
                        HealThread.Sleep(250)
                        oDAOC.SendString("/g Help~")
                        TotalSleep = TotalSleep + 250
                    End If
                End If

                HealThread.Sleep(250)
                TotalSleep = TotalSleep + 250

                'Update
                Dim mi3 As New UpdateText(AddressOf UpdateTextBox)
                Me.BeginInvoke(mi3, New Object() {tbPosition, CurrentX & "," & CurrentY & "," & CurrentZ})
                'tbPosition.Text = CurrentX & "," & CurrentY & "," & CurrentZ
                Try
                    Dim Cycle As Integer = Now.Subtract(PerfMon).TotalMilliseconds

                    'Update
                    Dim mi4 As UpdateText
                    mi4 = New UpdateText(AddressOf UpdateTextBox)
                    Me.BeginInvoke(mi4, New Object() {tbPerf, Cycle - TotalSleep})
                    'tbPerf.Text = Cycle - TotalSleep
                    mi4 = New UpdateText(AddressOf UpdateTextBox)
                    Me.BeginInvoke(mi4, New Object() {tbCycle, Cycle})
                    'tbCycle.Text = Cycle
                    mi4 = New UpdateText(AddressOf UpdateTextBox)
                    Me.BeginInvoke(mi4, New Object() {tbSleep, TotalSleep})
                    'tbSleep.Text = TotalSleep
                Catch Ex As Exception
                    'Update
                    Dim mi4 As UpdateText
                    mi4 = New UpdateText(AddressOf UpdateTextBox)
                    Me.BeginInvoke(mi4, New Object() {tbPerf, ""})
                    'tbPerf.Text = ""
                    mi4 = New UpdateText(AddressOf UpdateTextBox)
                    Me.BeginInvoke(mi4, New Object() {tbCycle, ""})
                    'tbCycle.Text = ""
                    mi4 = New UpdateText(AddressOf UpdateTextBox)
                    Me.BeginInvoke(mi4, New Object() {tbSleep, ""})
                    'tbSleep.Text = ""
                End Try

                If (oDAOC.IsPlayerDead = 1) AndAlso cbxStopAfterDeath.Checked Then
                    TerminateThread = True
                End If

            End While
        Catch Ex As Exception
            LogLineAsync("Exception occured! Heal-O-Matic stopped!")
            LogLineAsync(Ex.Message)
            LogLineAsync(Ex.Source)
            LogLineAsync(ex.StackTrace)
            MsgBox(Ex.Message)
            MsgBox(Ex.Source)
            MsgBox(ex.StackTrace)
        Finally
            oDAOC.StopInit()

            'Update
            Dim mi4 As UpdateText
            mi4 = New UpdateText(AddressOf UpdateTextBox)
            Me.BeginInvoke(mi4, New Object() {btnLoad, "Start"})
            'btnLoad.Text = "Start"
            HealThread = Nothing

            oDAOC = Nothing
        End Try
    End Sub
    Private Sub ZoneIntoTD(ByVal TDZoneName As String, _
                           ByVal XCoord As Double, ByVal YCoord As Double, ByVal ZCoord As Double, ByVal Range As Double, _
                           ByVal FaceLocX As Double, ByVal FaceLocY As Double, ByVal Runtime As Integer, _
                           ByRef objDaoc As AutoKillerScript.clsAutoKillerScript)

        Dim intCount As Integer = 0
        Dim blnStopRunning As Boolean = False

        LogLineAsync("Current zone name: " & objDaoc.ZoneName)
        LogLineAsync("TD zone name: " & TDZoneName)

        If objDaoc.ZoneName <> TDZoneName Then

            'move to zone spot
            If Not (objDaoc.ZDistance(objDaoc.gPlayerXCoord, objDaoc.gPlayerYCoord, objDaoc.gPlayerZCoord, _
                                      XCoord, YCoord, ZCoord)) < Range Then
                objDaoc.StartRunning()

                While objDaoc.ZDistance(objDaoc.gPlayerXCoord, objDaoc.gPlayerYCoord, objDaoc.gPlayerZCoord, _
                                        XCoord, YCoord, ZCoord) > Range And Not objDaoc.IsPlayerDead() And Not blnStopRunning

                    objDaoc.TurnToHeading(objDaoc.FindHeading(objDaoc.gPlayerXCoord, objDaoc.gPlayerYCoord, XCoord, YCoord))
                    System.Threading.Thread.CurrentThread.Sleep(100)

                    intCount = intCount + 1

                    'should of reached spot by now
                    If intCount > 10 Then
                        blnStopRunning = True
                    End If
                End While

                objDaoc.StopRunning()
                System.Threading.Thread.CurrentThread.Sleep(1000)
            End If

            'should be a spot now
            If blnStopRunning = False Then
                'face TD
                LogLineAsync("/faceloc of instance")
                objDaoc.SendString("/faceloc " & FaceLocX & " " & FaceLocY & "~")
                System.Threading.Thread.CurrentThread.Sleep(2000)

                LogLineAsync("Run for " & Runtime / 1000 & " seconds to zone in")
                objDaoc.StartRunning()

                System.Threading.Thread.CurrentThread.Sleep(Runtime)

                LogLineAsync("Stop running")
                objDaoc.StopRunning()

                System.Threading.Thread.CurrentThread.Sleep(5000)
            End If
        End If
    End Sub
    Private Sub MoveToGXY(ByVal X As Double, ByVal Y As Double, ByVal Z As Double, _
                          ByVal Range As Double, _
                          ByRef objDaoc As AutoKillerScript.clsAutoKillerScript)

        If Not (objDaoc.ZDistance(objDaoc.gPlayerXCoord, objDaoc.gPlayerYCoord, objDaoc.gPlayerZCoord, X, Y, Z)) < Range Then
            objDaoc.StartRunning()

            While objDaoc.ZDistance(objDaoc.gPlayerXCoord, objDaoc.gPlayerYCoord, objDaoc.gPlayerZCoord, X, Y, Z) > Range _
                And Not objDaoc.IsPlayerDead()

                objDaoc.TurnToHeading(objDaoc.FindHeading(objDaoc.gPlayerXCoord, objDaoc.gPlayerYCoord, X, Y))
                HealThread.Sleep(100)
            End While

            objDaoc.StopRunning()
        End If
    End Sub

    Private Sub StickToGroup(ByRef aDAOC As AutoKillerScript.clsAutoKillerScript)
        If FollowGroup Then
            Dim aPosition As GlobalPositioning = New GlobalPositioning(aDAOC)
            Threading.Thread.CurrentThread.Sleep(500)
            If aPosition.GetGroupValues.Count = 0 Then
                LogLineAsync("StickToGroup - " & "Quitting")
                Threading.Thread.CurrentThread.Sleep(1000)
                aDAOC.SendString("/quit~")
                TerminateThread = True
                Exit Sub
            End If
            Dim aPoint As Point
            Dim Heading As Short
            'Heading = Math.Abs(aPosition.FindHeading() - 180) Mod 360
            aPosition.PositionXY(300)
        End If
    End Sub
    Private Sub lvMembers_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles lvMembers.ItemCheck
        Dim Member As clsMember = lvMembers.Items(e.Index).Tag
        If Not IsNothing(Member) Then
            Member.Heal = (e.NewValue = CheckState.Checked)
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If tbHealName.Text = "" Then
            MsgBox("No spell name filled in.")
        End If

        If tbHealKey.Text = "" Then
            MsgBox("No key filled in.")
            Return
        End If

        Dim lvItem As ListViewItem = New ListViewItem(tbHealName.Text)
        lvItem.SubItems.Add(nudHealThreshold.Value)
        lvItem.SubItems.Add(tbHealKey.Text)
        lvItem.SubItems.Add(nudHealDelay.Value)
        If cbHealInCombat.Checked Then
            lvItem.SubItems.Add("Yes")
        Else
            lvItem.SubItems.Add("No")
        End If

        Dim Inserted As Boolean = False
        Dim I As Integer
        For I = 0 To lvHeals.Items.Count - 1
            If CInt(lvHeals.Items(I).SubItems(1).Text) < nudHealThreshold.Value Then
                Inserted = True
                lvHeals.Items.Insert(I, lvItem)
                Exit For
            End If
        Next

        If Not Inserted Then
            lvHeals.Items.Add(lvItem)
        End If
    End Sub
    Private Sub cbxProfiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxProfiles.SelectedIndexChanged
        If IO.File.Exists(cbxProfiles.Text & ".hom") Then
            Try
                Dim xmlDoc As Xml.XmlDocument = New Xml.XmlDocument
                xmlDoc.Load(cbxProfiles.Text & ".hom")

                cbxAutoGroup.Checked = xmlDoc.SelectSingleNode("//Profile/Settings/@AutoUpdateGroup").Value
                If Not IsNothing(xmlDoc.SelectSingleNode("//Profile/Settings/@SitMode")) Then
                    cbSitMode.SelectedIndex = xmlDoc.SelectSingleNode("//Profile/Settings/@SitMode").Value
                Else
                    cbSitMode.SelectedIndex = 1
                End If

                If Not IsNothing(xmlDoc.SelectSingleNode("//Profile/Settings/@StopAfterDeath")) Then
                    cbxStopAfterDeath.Checked = xmlDoc.SelectSingleNode("//Profile/Settings/@StopAfterDeath").Value
                Else
                    cbxStopAfterDeath.Checked = True
                End If

                If Not IsNothing(xmlDoc.SelectSingleNode("//Profile/Settings/@WaitForRez")) Then
                    cbxWaitForRez.Checked = xmlDoc.SelectSingleNode("//Profile/Settings/@WaitForRez").Value
                Else
                    cbxWaitForRez.Checked = Not cbxStopAfterDeath.Checked
                End If

                If Not IsNothing(xmlDoc.SelectSingleNode("//Profile/Settings/@FollowGroup")) Then
                    cbxFollowGroup.Checked = xmlDoc.SelectSingleNode("//Profile/Settings/@FollowGroup").Value
                Else
                    cbxFollowGroup.Checked = True
                End If

                If Not IsNothing(xmlDoc.SelectSingleNode("//Profile/Settings/@Quiet")) Then
                    cbxQuiet.Checked = xmlDoc.SelectSingleNode("//Profile/Settings/@Quiet").Value
                Else
                    cbxQuiet.Checked = True
                End If

                If Not IsNothing(xmlDoc.SelectSingleNode("//Profile/Settings/@BAF")) Then
                    chkCheckForBAF.Checked = xmlDoc.SelectSingleNode("//Profile/Settings/@BAF").Value
                    CheckBAF = chkCheckForBAF.Checked
                    Me.gbMezz.Visible = CheckBAF
                Else
                    chkCheckForBAF.Checked = False
                    CheckBAF = False
                    Me.gbMezz.Visible = False
                End If

                If Not IsNothing(xmlDoc.SelectSingleNode("//Profile/Settings/@Poison")) Then
                    PoisonBox.Checked = xmlDoc.SelectSingleNode("//Profile/Settings/@Poison").Value
                    Poison = True
                Else
                    PoisonBox.Checked = False
                    Poison = False
                End If

                If Not IsNothing(xmlDoc.SelectSingleNode("//Profile/Settings/@Disease")) Then
                    DiseaseBox.Checked = xmlDoc.SelectSingleNode("//Profile/Settings/@Disease").Value
                    Disease = True
                Else
                    DiseaseBox.Checked = False
                    Disease = False
                End If

                If Not IsNothing(xmlDoc.SelectSingleNode("//Profile/Settings/@PoisonKey").Value) Then
                    tbPoisonKey.Text = xmlDoc.SelectSingleNode("//Profile/Settings/@PoisonKey").Value
                End If

                If Not IsNothing(xmlDoc.SelectSingleNode("//Profile/Settings/@DiseaseKey").Value) Then
                    tbDiseaseKey.Text = xmlDoc.SelectSingleNode("//Profile/Settings/@DiseaseKey").Value
                End If

                If Not IsNothing(xmlDoc.SelectSingleNode("//Profile/Settings/@MezKey").Value) Then
                    txtMezzKey.Text = xmlDoc.SelectSingleNode("//Profile/Settings/@MezKey").Value
                End If

                If Not IsNothing(xmlDoc.SelectSingleNode("//Profile/Settings/@MezzDistance").Value) Then
                    txtSearchDistance.Text = xmlDoc.SelectSingleNode("//Profile/Settings/@MezzDistance").Value
                End If

                If Not IsNothing(xmlDoc.SelectSingleNode("//Profile/Settings/@UseGT")) Then
                    rbGT.Checked = xmlDoc.SelectSingleNode("//Profile/Settings/@UseGT").Value
                Else
                    rbGT.Checked = False
                End If

                If Not IsNothing(xmlDoc.SelectSingleNode("//Profile/Settings/@HealerArea")) Then
                    rbHealerArea.Checked = xmlDoc.SelectSingleNode("//Profile/Settings/@HealerArea").Value
                Else
                    rbHealerArea.Checked = True
                End If

                If Not IsNothing(xmlDoc.SelectSingleNode("//Profile/Settings/@MezzTarget")) Then
                    rbTarget.Checked = xmlDoc.SelectSingleNode("//Profile/Settings/@MezzTarget").Value
                Else
                    rbTarget.Checked = False
                End If

                If Not IsNothing(xmlDoc.SelectSingleNode("//Profile/Settings/@MezzRvR")) Then
                    rbRVR.Checked = xmlDoc.SelectSingleNode("//Profile/Settings/@MezzRvR").Value
                Else
                    rbRVR.Checked = False
                End If

                If Not IsNothing(xmlDoc.SelectSingleNode("//Profile/Settings/@HealerName").Value) Then
                    tbHealer.Text = xmlDoc.SelectSingleNode("//Profile/Settings/@HealerName").Value
                End If

                cbxEnableSpread.Checked = xmlDoc.SelectSingleNode("//Profile/SpreadHeal/@Enabled").Value
                tbSpreadName.Text = xmlDoc.SelectSingleNode("//Profile/SpreadHeal/@Name").Value
                nudSpreadDelay.Value = xmlDoc.SelectSingleNode("//Profile/SpreadHeal/@Delay").Value
                tbSpreadKey.Text = xmlDoc.SelectSingleNode("//Profile/SpreadHeal/@Key").Value
                nudSpreadThreshold.Value = xmlDoc.SelectSingleNode("//Profile/SpreadHeal/@Threshold").Value
                nudSpreadMembersThreshold.Value = xmlDoc.SelectSingleNode("//Profile/SpreadHeal/@MembersThreshold").Value

                lvHeals.Items.Clear()

                Dim xmlNode As Xml.XmlNode
                For Each xmlNode In xmlDoc.SelectNodes("//Profile/TargetHeals/TargetHeal")
                    Dim lvItem As ListViewItem = New ListViewItem(xmlNode.SelectSingleNode("@Name").Value)
                    lvItem.SubItems.Add(xmlNode.SelectSingleNode("@Threshold").Value)
                    lvItem.SubItems.Add(xmlNode.SelectSingleNode("@Key").Value)
                    lvItem.SubItems.Add(xmlNode.SelectSingleNode("@Delay").Value)
                    If xmlNode.SelectSingleNode("@InCombat").Value Then
                        lvItem.SubItems.Add("Yes")
                    Else
                        lvItem.SubItems.Add("No")
                    End If

                    Dim Inserted As Boolean = False
                    Dim I As Integer
                    For I = 0 To lvHeals.Items.Count - 1
                        If CInt(lvHeals.Items(I).SubItems(1).Text) < CInt(lvItem.SubItems(1).Text) Then
                            Inserted = True
                            lvHeals.Items.Insert(I, lvItem)
                            Exit For
                        End If
                    Next

                    If Not Inserted Then
                        lvHeals.Items.Add(lvItem)
                    End If
                Next
            Catch Ex As Exception
                Debug.WriteLine(Ex.Message)
            End Try
        End If
    End Sub
    Sub UpdateProfiles()
        cbxProfiles.Items.Clear()

        Dim Filename As String
        For Each Filename In IO.Directory.GetFiles(".")
            If IO.Path.GetExtension(Filename).ToUpper = ".HOM" Then
                cbxProfiles.Items.Add(IO.Path.GetFileNameWithoutExtension(Filename))
            End If
        Next
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim lvItem As ListViewItem
        If lvHeals.SelectedItems.Count > 0 Then
            Dim Member As clsMember
            For Each lvItem In lvHeals.SelectedItems
                lvHeals.Items.Remove(lvItem)
            Next
        End If
    End Sub
    Private Sub addAttribute(ByVal xmlDoc As Xml.XmlDocument, ByVal xmlNode As Xml.XmlNode, ByVal Attr As String, ByVal value As String)
        Dim xmlAttr As Xml.XmlAttribute
        xmlAttr = xmlDoc.CreateAttribute(Attr)
        xmlAttr.Value = value
        xmlNode.Attributes.Append(xmlAttr)
    End Sub
    Private Sub btnSaveProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveProfile.Click
        If IO.File.Exists(cbxProfiles.Text & ".hom") Then
            If MsgBox("The profile already exists, do you want to overwrite it?", MsgBoxStyle.OKCancel) = MsgBoxResult.Cancel Then
                Return
            End If
        End If

        Dim xmlDoc As Xml.XmlDocument = New Xml.XmlDocument
        Dim xmlNode As Xml.XmlNode

        xmlDoc.LoadXml("<Profile/>")

        xmlNode = xmlDoc.CreateElement("Settings")
        addAttribute(xmlDoc, xmlNode, "HealerName", tbHealer.Text)

        addAttribute(xmlDoc, xmlNode, "AutoUpdateGroup", cbxAutoGroup.Checked)
        addAttribute(xmlDoc, xmlNode, "SitMode", cbSitMode.SelectedIndex)
        addAttribute(xmlDoc, xmlNode, "StopAfterDeath", cbxStopAfterDeath.Checked)
        addAttribute(xmlDoc, xmlNode, "WaitForRez", cbxWaitForRez.Checked)
        addAttribute(xmlDoc, xmlNode, "FollowGroup", cbxFollowGroup.Checked)
        addAttribute(xmlDoc, xmlNode, "Quiet", cbxQuiet.Checked)

        addAttribute(xmlDoc, xmlNode, "BAF", Me.chkCheckForBAF.Checked)
        addAttribute(xmlDoc, xmlNode, "MezKey", txtMezzKey.Text)
        addAttribute(xmlDoc, xmlNode, "MezzDistance", txtSearchDistance.Text)
        addAttribute(xmlDoc, xmlNode, "UseGT", rbGT.Checked)
        addAttribute(xmlDoc, xmlNode, "HealerArea", rbHealerArea.Checked)
        addAttribute(xmlDoc, xmlNode, "MezzTarget", rbTarget.Checked)
        addAttribute(xmlDoc, xmlNode, "MezzRvR", rbRVR.Checked)

        addAttribute(xmlDoc, xmlNode, "Poison", PoisonBox.Checked)
        addAttribute(xmlDoc, xmlNode, "Disease", DiseaseBox.Checked)
        addAttribute(xmlDoc, xmlNode, "PoisonKey", tbPoisonKey.Text)
        addAttribute(xmlDoc, xmlNode, "DiseaseKey", tbDiseaseKey.Text)

        xmlDoc.DocumentElement.AppendChild(xmlNode)

        xmlNode = xmlDoc.CreateElement("TargetHeals")
        Dim lvItem As ListViewItem
        For Each lvItem In lvHeals.Items
            Dim xmlHeal As Xml.XmlNode
            xmlHeal = xmlDoc.CreateElement("TargetHeal")
            addAttribute(xmlDoc, xmlHeal, "Name", lvItem.SubItems(0).Text)
            addAttribute(xmlDoc, xmlHeal, "Delay", lvItem.SubItems(3).Text)
            addAttribute(xmlDoc, xmlHeal, "Key", lvItem.SubItems(2).Text)
            addAttribute(xmlDoc, xmlHeal, "InCombat", lvItem.SubItems(4).Text = "Yes")
            addAttribute(xmlDoc, xmlHeal, "Threshold", lvItem.SubItems(1).Text)
            xmlNode.AppendChild(xmlHeal)
        Next
        xmlDoc.DocumentElement.AppendChild(xmlNode)

        xmlNode = xmlDoc.CreateElement("SpreadHeal")
        addAttribute(xmlDoc, xmlNode, "Enabled", cbxEnableSpread.Checked)
        addAttribute(xmlDoc, xmlNode, "Name", tbSpreadName.Text)
        addAttribute(xmlDoc, xmlNode, "Delay", nudSpreadDelay.Value)
        addAttribute(xmlDoc, xmlNode, "Key", tbSpreadKey.Text)
        addAttribute(xmlDoc, xmlNode, "Threshold", nudSpreadThreshold.Value)
        addAttribute(xmlDoc, xmlNode, "MembersThreshold", nudSpreadMembersThreshold.Value)
        xmlDoc.DocumentElement.AppendChild(xmlNode)

        xmlDoc.Save(cbxProfiles.Text & ".hom")

        Dim Profile As String = cbxProfiles.Text

        UpdateProfiles()

        Dim C As Integer
        For C = 0 To cbxProfiles.Items.Count - 1
            If cbxProfiles.Items.Item(C) = Profile Then
                cbxProfiles.SelectedIndex = C
            End If
        Next
    End Sub
    Private Sub frmMain_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Not IsNothing(HealThread) Then
            btnLoad_Click(Nothing, Nothing)
        End If
    End Sub
    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            TmrCount = 0
            BuffTmrProgress.Value = 0
            BuffTmrProgress.Maximum = Val(tbBuffTime1.Text) + 1
            K_BuffTime = Val(tbBuffTime1.Text)
            K_Buff1 = Val(tbBuffKey1.Text)
            K_Buff2 = Val(tbBuffKey2.Text)
            BuffTmr1.Enabled = True
            BuffTmr1.Start()

            LogLine("Timer started / Progress bar reset")

        Else
            BuffTmr1.Enabled() = False
            BuffTmr1.Stop()
            BuffTmrProgress.Value = 0

            LogLine("Timer Stopped")
        End If
    End Sub
    Private Sub BuffTmr1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BuffTmr1.Tick
        If TmrCount <= K_BuffTime Then
            TmrCount = TmrCount + 1
            BuffTmrProgress.Value = TmrCount - 1
            BuffTmrProgress.Update()
        Else
            TmrCount = 0
            BuffTmrProgress.Value = 0

            LogLine("Timer reset")
            LogLine("Sending Key " & K_Buff1)
            LogLine("Sending Key " & K_Buff2)
        End If

    End Sub
    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbBuffTime1.TextChanged
        K_BuffTime = Val(tbBuffTime1.Text) + 1
        BuffTmrProgress.Maximum = K_BuffTime
    End Sub
    Private Sub tbBuffKey_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbBuffKey1.TextChanged
        K_Buff1 = Val(tbBuffKey1.Text)
    End Sub
    Private Sub tbBuffKey2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbBuffKey2.TextChanged
        K_Buff2 = Val(tbBuffKey2.Text)
    End Sub
    Private Sub BuffBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BuffBox2.CheckedChanged
        If BuffBox2.Checked Then
            Buff2 = True
        Else
            Buff2 = False
        End If
    End Sub
    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbDiseaseKey.TextChanged
        K_Disease = Val(tbDiseaseKey.Text)
    End Sub
    Private Sub tbPoisonKey_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbPoisonKey.TextChanged
        K_Poison = Val(tbPoisonKey.Text)
    End Sub
    Private Sub DiseaseBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiseaseBox.CheckedChanged
        If DiseaseBox.Checked Then
            Disease = True
        Else
            Disease = False
        End If
    End Sub
    Private Sub PoisonBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PoisonBox.CheckedChanged
        If PoisonBox.Checked Then
            Poison = True
        Else
            Poison = False
        End If
    End Sub
    Private Sub cbxAutoGroup_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxAutoGroup.CheckedChanged

    End Sub
    Private Sub cbxStopAfterDeath_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxStopAfterDeath.CheckedChanged
        If cbxStopAfterDeath.Checked Then
            cbxWaitForRez.Checked = False
        End If
    End Sub
    Private Sub cbxFollowGroup_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxFollowGroup.CheckedChanged
        FollowGroup = cbxFollowGroup.Checked
    End Sub
    Public Function WaitForRez(ByVal oDAOC As AutoKillerScript.clsAutoKillerScript) As Integer
        LogLine("Waiting For Rez")
        While Not oDAOC.QueryString(25) And Not TerminateThread
            LogLine(oDAOC.GetString())
            If oDAOC.QueryString(23) Then    ' Might Be a Rez
                LogLine("Possible Rez")
                AcceptRez()
            End If
            Threading.Thread.CurrentThread.Sleep(1500)
            If oDAOC.QueryString(24) Then    ' It was a rez
                LogLine("Got Rez")
                oDAOC.SendString("/s Thank you kindly!~")
                'mIsPlayerDead = False
                Exit Function
            End If
        End While
        oDAOC.SendString("/quit~")
        TerminateThread = False
    End Function
    Public Function AcceptRez() As Integer
        DialogWindow.CloseDialog(WndMngr)
    End Function
    Private Sub tbHealer_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbHealer.TextChanged
        If Not WndMngr Is Nothing Then
            WndMngr.CharacterName = tbHealer.Text
        End If
    End Sub
    Private Sub cbxWaitForRez_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxWaitForRez.CheckedChanged
        If cbxWaitForRez.Checked Then
            cbxStopAfterDeath.Checked = False
        End If
    End Sub
    Private Sub chkCheckForBAF_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCheckForBAF.CheckedChanged
        If chkCheckForBAF.Checked Then
            Me.gbMezz.Visible = True
            CheckBAF = True
        Else
            Me.gbMezz.Visible = False
            CheckBAF = False
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim objDAOC As New AutoKillerScript.clsAutoKillerScript

        objDAOC.RegKey = readSetting("RegKey", GetType(String))
        objDAOC.GamePath = readSetting("GamePath", GetType(String))

        objDAOC.EnableEuro = readSetting("EnableEuro", GetType(Boolean))
        objDAOC.EnableToA = readSetting("EnableTOA", GetType(Boolean))
        objDAOC.EnableCatacombs = readSetting("EnableCatacombs", GetType(Boolean))


        objDAOC.DoInit()

        LogLine("Name: " & objDAOC.PlayerName)
        LogLine("X: " & objDAOC.gPlayerXCoord)
        LogLine("Y: " & objDAOC.gPlayerYCoord)
        LogLine("Z: " & objDAOC.gPlayerZCoord)

        objDAOC.StopInit()
        objDAOC = Nothing
    End Sub

    Private Sub tChkHealth_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles tChkHealth.Elapsed

    End Sub
End Class



