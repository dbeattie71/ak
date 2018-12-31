Option Explicit On 

Imports Microsoft.Win32

Public Class frmMain
    Inherits System.Windows.Forms.Form
#Region " TODO "
    'TODO: 
    'TODO: 
    'TODO: 
    'TODO: 
    'TODO: 

#End Region
#Region " Variables "
    Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
    Private Declare Function GetForegroundWindow Lib "user32" Alias "GetForegroundWindow" () As Integer

    Private objDAOC As AutoKillerScript.clsAutoKillerScript

    Private _AKHelper As AKHelper.clsAkHelper
    Private _SpellTimers As Hashtable

    Private objEncoder As New System.Text.UTF8Encoding

    Shared objAKHeal As Threading.Thread

    Shared blnTerminateThread As Boolean = False
    Shared blnKeepAlive As Boolean = False

    'keys
    Private bytSitKey As Byte

    Private bytShowInventoryKey As Byte
    Private bytMoveForwardKey As Byte
    Private bytMoveBackwardKey As Byte
    Private bytStickKey As Byte
    Private bytFaceKey As Byte

    Private _MainFormTitle As String

    Dim htMembers As Hashtable = New Hashtable

    Dim blnAddNewMember As Boolean = False
    Dim strNewMember As String

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
    Friend WithEvents tpProfile As System.Windows.Forms.TabPage
    Friend WithEvents tpLog As System.Windows.Forms.TabPage
    Friend WithEvents lbLog As System.Windows.Forms.ListBox
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents btnSaveProfile As System.Windows.Forms.Button
    Friend WithEvents cbxProfiles As System.Windows.Forms.ComboBox
    Friend WithEvents TabControl2 As System.Windows.Forms.TabControl
    Friend WithEvents tpGroupHeals As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
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
    Friend WithEvents tpTargetHeals As System.Windows.Forms.TabPage
    Friend WithEvents lvHeals As System.Windows.Forms.ListView
    Friend WithEvents Panel7 As System.Windows.Forms.Panel
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents tbHealKey As System.Windows.Forms.TextBox
    Friend WithEvents tbHealName As System.Windows.Forms.TextBox
    Friend WithEvents cbHealInCombat As System.Windows.Forms.CheckBox
    Friend WithEvents nudHealDelay As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudHealThreshold As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnRemoveHeal As System.Windows.Forms.Button
    Friend WithEvents btnAddHeal As System.Windows.Forms.Button
    Friend WithEvents tmeKeepAlive As System.Timers.Timer
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents tpOptions As System.Windows.Forms.TabPage
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbRegKey As System.Windows.Forms.TextBox
    Friend WithEvents tbToA As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tbCatacombs As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents rbToA As System.Windows.Forms.RadioButton
    Friend WithEvents rbCatacombs As System.Windows.Forms.RadioButton
    Friend WithEvents Button1 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnStartStop = New System.Windows.Forms.Button
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.tpLog = New System.Windows.Forms.TabPage
        Me.lbLog = New System.Windows.Forms.ListBox
        Me.tpProfile = New System.Windows.Forms.TabPage
        Me.TabControl2 = New System.Windows.Forms.TabControl
        Me.tpGroupHeals = New System.Windows.Forms.TabPage
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
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
        Me.tpTargetHeals = New System.Windows.Forms.TabPage
        Me.lvHeals = New System.Windows.Forms.ListView
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.Panel7 = New System.Windows.Forms.Panel
        Me.btnRemoveHeal = New System.Windows.Forms.Button
        Me.Panel6 = New System.Windows.Forms.Panel
        Me.tbHealKey = New System.Windows.Forms.TextBox
        Me.tbHealName = New System.Windows.Forms.TextBox
        Me.cbHealInCombat = New System.Windows.Forms.CheckBox
        Me.nudHealDelay = New System.Windows.Forms.NumericUpDown
        Me.nudHealThreshold = New System.Windows.Forms.NumericUpDown
        Me.btnAddHeal = New System.Windows.Forms.Button
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.btnSaveProfile = New System.Windows.Forms.Button
        Me.cbxProfiles = New System.Windows.Forms.ComboBox
        Me.tpOptions = New System.Windows.Forms.TabPage
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.tbRegKey = New System.Windows.Forms.TextBox
        Me.tbToA = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.tbCatacombs = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.tmeKeepAlive = New System.Timers.Timer
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rbToA = New System.Windows.Forms.RadioButton
        Me.rbCatacombs = New System.Windows.Forms.RadioButton
        Me.Button1 = New System.Windows.Forms.Button
        Me.TabControl1.SuspendLayout()
        Me.tpLog.SuspendLayout()
        Me.tpProfile.SuspendLayout()
        Me.TabControl2.SuspendLayout()
        Me.tpGroupHeals.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.nudSpreadMembersThreshold, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudSpreadDelay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudSpreadThreshold, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpTargetHeals.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.Panel6.SuspendLayout()
        CType(Me.nudHealDelay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHealThreshold, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel5.SuspendLayout()
        Me.tpOptions.SuspendLayout()
        Me.Panel4.SuspendLayout()
        CType(Me.tmeKeepAlive, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnStartStop
        '
        Me.btnStartStop.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStartStop.Location = New System.Drawing.Point(432, 8)
        Me.btnStartStop.Name = "btnStartStop"
        Me.btnStartStop.TabIndex = 0
        Me.btnStartStop.Text = "Start"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tpLog)
        Me.TabControl1.Controls.Add(Me.tpProfile)
        Me.TabControl1.Controls.Add(Me.tpOptions)
        Me.TabControl1.ItemSize = New System.Drawing.Size(87, 18)
        Me.TabControl1.Location = New System.Drawing.Point(8, 8)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(520, 270)
        Me.TabControl1.TabIndex = 2
        '
        'tpLog
        '
        Me.tpLog.Controls.Add(Me.lbLog)
        Me.tpLog.Location = New System.Drawing.Point(4, 22)
        Me.tpLog.Name = "tpLog"
        Me.tpLog.Size = New System.Drawing.Size(512, 244)
        Me.tpLog.TabIndex = 2
        Me.tpLog.Text = "Log"
        '
        'lbLog
        '
        Me.lbLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbLog.HorizontalScrollbar = True
        Me.lbLog.Location = New System.Drawing.Point(0, 0)
        Me.lbLog.Name = "lbLog"
        Me.lbLog.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lbLog.Size = New System.Drawing.Size(512, 238)
        Me.lbLog.TabIndex = 0
        '
        'tpProfile
        '
        Me.tpProfile.Controls.Add(Me.TabControl2)
        Me.tpProfile.Controls.Add(Me.Panel5)
        Me.tpProfile.Location = New System.Drawing.Point(4, 22)
        Me.tpProfile.Name = "tpProfile"
        Me.tpProfile.Size = New System.Drawing.Size(512, 244)
        Me.tpProfile.TabIndex = 1
        Me.tpProfile.Text = "Profile"
        '
        'TabControl2
        '
        Me.TabControl2.Alignment = System.Windows.Forms.TabAlignment.Bottom
        Me.TabControl2.Controls.Add(Me.tpGroupHeals)
        Me.TabControl2.Controls.Add(Me.tpTargetHeals)
        Me.TabControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl2.Location = New System.Drawing.Point(0, 36)
        Me.TabControl2.Multiline = True
        Me.TabControl2.Name = "TabControl2"
        Me.TabControl2.SelectedIndex = 0
        Me.TabControl2.Size = New System.Drawing.Size(512, 208)
        Me.TabControl2.TabIndex = 4
        '
        'tpGroupHeals
        '
        Me.tpGroupHeals.Controls.Add(Me.GroupBox1)
        Me.tpGroupHeals.Location = New System.Drawing.Point(4, 4)
        Me.tpGroupHeals.Name = "tpGroupHeals"
        Me.tpGroupHeals.Size = New System.Drawing.Size(504, 182)
        Me.tpGroupHeals.TabIndex = 1
        Me.tpGroupHeals.Text = "Group Heals"
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
        Me.GroupBox1.Size = New System.Drawing.Size(504, 96)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Spread Heals"
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
        'tpTargetHeals
        '
        Me.tpTargetHeals.Controls.Add(Me.lvHeals)
        Me.tpTargetHeals.Controls.Add(Me.Panel7)
        Me.tpTargetHeals.Controls.Add(Me.Panel6)
        Me.tpTargetHeals.Location = New System.Drawing.Point(4, 4)
        Me.tpTargetHeals.Name = "tpTargetHeals"
        Me.tpTargetHeals.Size = New System.Drawing.Size(504, 182)
        Me.tpTargetHeals.TabIndex = 0
        Me.tpTargetHeals.Text = "Target Heals"
        Me.tpTargetHeals.Visible = False
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
        Me.lvHeals.Size = New System.Drawing.Size(472, 102)
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
        Me.Panel7.Controls.Add(Me.btnRemoveHeal)
        Me.Panel7.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel7.Location = New System.Drawing.Point(472, 0)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(32, 102)
        Me.Panel7.TabIndex = 2
        '
        'btnRemoveHeal
        '
        Me.btnRemoveHeal.Font = New System.Drawing.Font("Symbol", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnRemoveHeal.Location = New System.Drawing.Point(4, 4)
        Me.btnRemoveHeal.Name = "btnRemoveHeal"
        Me.btnRemoveHeal.Size = New System.Drawing.Size(24, 24)
        Me.btnRemoveHeal.TabIndex = 5
        Me.btnRemoveHeal.Text = "´"
        '
        'Panel6
        '
        Me.Panel6.Controls.Add(Me.tbHealKey)
        Me.Panel6.Controls.Add(Me.tbHealName)
        Me.Panel6.Controls.Add(Me.cbHealInCombat)
        Me.Panel6.Controls.Add(Me.nudHealDelay)
        Me.Panel6.Controls.Add(Me.nudHealThreshold)
        Me.Panel6.Controls.Add(Me.btnAddHeal)
        Me.Panel6.Controls.Add(Me.Label11)
        Me.Panel6.Controls.Add(Me.Label10)
        Me.Panel6.Controls.Add(Me.Label9)
        Me.Panel6.Controls.Add(Me.Label8)
        Me.Panel6.Controls.Add(Me.Label7)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel6.Location = New System.Drawing.Point(0, 102)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(504, 80)
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
        'btnAddHeal
        '
        Me.btnAddHeal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddHeal.Location = New System.Drawing.Point(454, 56)
        Me.btnAddHeal.Name = "btnAddHeal"
        Me.btnAddHeal.Size = New System.Drawing.Size(48, 21)
        Me.btnAddHeal.TabIndex = 19
        Me.btnAddHeal.Text = "Add"
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
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.btnSaveProfile)
        Me.Panel5.Controls.Add(Me.cbxProfiles)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel5.Location = New System.Drawing.Point(0, 0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(512, 36)
        Me.Panel5.TabIndex = 3
        '
        'btnSaveProfile
        '
        Me.btnSaveProfile.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveProfile.Location = New System.Drawing.Point(468, 7)
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
        Me.cbxProfiles.Size = New System.Drawing.Size(456, 20)
        Me.cbxProfiles.TabIndex = 2
        '
        'tpOptions
        '
        Me.tpOptions.Controls.Add(Me.Panel4)
        Me.tpOptions.Location = New System.Drawing.Point(4, 22)
        Me.tpOptions.Name = "tpOptions"
        Me.tpOptions.Size = New System.Drawing.Size(512, 244)
        Me.tpOptions.TabIndex = 3
        Me.tpOptions.Text = "Options"
        '
        'Panel4
        '
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.Label2)
        Me.Panel4.Controls.Add(Me.tbRegKey)
        Me.Panel4.Controls.Add(Me.tbToA)
        Me.Panel4.Controls.Add(Me.Label1)
        Me.Panel4.Controls.Add(Me.tbCatacombs)
        Me.Panel4.Controls.Add(Me.Label3)
        Me.Panel4.Controls.Add(Me.Label4)
        Me.Panel4.Location = New System.Drawing.Point(8, 6)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(496, 232)
        Me.Panel4.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 80)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 23)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Reg Key:"
        '
        'tbRegKey
        '
        Me.tbRegKey.Location = New System.Drawing.Point(72, 80)
        Me.tbRegKey.Name = "tbRegKey"
        Me.tbRegKey.Size = New System.Drawing.Size(176, 20)
        Me.tbRegKey.TabIndex = 7
        Me.tbRegKey.Text = ""
        '
        'tbToA
        '
        Me.tbToA.Location = New System.Drawing.Point(72, 56)
        Me.tbToA.Name = "tbToA"
        Me.tbToA.Size = New System.Drawing.Size(176, 20)
        Me.tbToA.TabIndex = 4
        Me.tbToA.Text = ""
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 56)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 23)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "ToA:"
        '
        'tbCatacombs
        '
        Me.tbCatacombs.Location = New System.Drawing.Point(72, 32)
        Me.tbCatacombs.Name = "tbCatacombs"
        Me.tbCatacombs.Size = New System.Drawing.Size(176, 20)
        Me.tbCatacombs.TabIndex = 2
        Me.tbCatacombs.Text = ""
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Paths"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(8, 32)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 23)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Catacombs:"
        '
        'tmeKeepAlive
        '
        Me.tmeKeepAlive.Enabled = True
        Me.tmeKeepAlive.Interval = 1080000
        Me.tmeKeepAlive.SynchronizingObject = Me
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.rbToA)
        Me.Panel1.Controls.Add(Me.rbCatacombs)
        Me.Panel1.Controls.Add(Me.btnStartStop)
        Me.Panel1.Location = New System.Drawing.Point(8, 288)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(520, 40)
        Me.Panel1.TabIndex = 9
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
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(320, 8)
        Me.Button1.Name = "Button1"
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Button1"
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(538, 338)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmMain"
        Me.Text = "AKHeal"
        Me.TabControl1.ResumeLayout(False)
        Me.tpLog.ResumeLayout(False)
        Me.tpProfile.ResumeLayout(False)
        Me.TabControl2.ResumeLayout(False)
        Me.tpGroupHeals.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.nudSpreadMembersThreshold, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudSpreadDelay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudSpreadThreshold, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpTargetHeals.ResumeLayout(False)
        Me.Panel7.ResumeLayout(False)
        Me.Panel6.ResumeLayout(False)
        CType(Me.nudHealDelay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHealThreshold, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel5.ResumeLayout(False)
        Me.tpOptions.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        CType(Me.tmeKeepAlive, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnStartStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartStop.Click
        If IsNothing(objAKHeal) Then
            blnTerminateThread = False

            objAKHeal = New Threading.Thread(AddressOf AKHeal)
            objAKHeal.Start()

            btnStartStop.Text = "Stop"
        Else
            Text = "AKHeal"

            btnStartStop.Text = "Start"

            blnTerminateThread = True

            objAKHeal.Join()
            objAKHeal = Nothing
        End If
    End Sub

    Private Sub AKHeal()

        Try
            objDAOC = New AutoKillerScript.clsAutoKillerScript

            _AKHelper = New AKHelper.clsAkHelper

            AddHandler objDAOC.OnLog, AddressOf LogLine
            AddHandler objDAOC.OnRegExTrue, AddressOf ProcessQuery

            LogLineAsync("Initializing DAOCScript v" & objDAOC.getVersion & "....")

            Dim objKeys As DAOCKeyboard

            ' This section sets up various variables for the DLL.
            If rbCatacombs.Checked Then
                objDAOC.RegKey = tbRegKey.Text
                objDAOC.GamePath = tbCatacombs.Text
                objDAOC.EnableToA = False
                objDAOC.EnableCatacombs = True
                objKeys = New DAOCKeyboard(objDAOC, tbCatacombs.Text)
            ElseIf rbToA.Checked Then
                objDAOC.RegKey = tbRegKey.Text
                objDAOC.GamePath = tbToA.Text
                objDAOC.EnableToA = True
                objDAOC.EnableCatacombs = False
                objKeys = New DAOCKeyboard(objDAOC, tbToA.Text)
            End If

            objDAOC.EnableAutoQuery = True
            objDAOC.UseRegEx = True

            'Dim objKeys As New AutoKillerScript.UserKeys(objDAOC)

            'keys
            objDAOC.SetLeftTurnKey = objKeys.TurnLeft_Key
            objDAOC.SetRightTurnKey = objKeys.TurnRight_Key
            objDAOC.SetConsiderKey = objKeys.Consider_Key

            bytShowInventoryKey = objKeys.ShowInventory_Key
            bytMoveForwardKey = objKeys.MoveForward1_Key
            bytMoveBackwardKey = objKeys.MoveBackward2_Key
            bytSitKey = objKeys.Sit_Stand_Key
            bytStickKey = objKeys.Stick_Key
            bytFaceKey = objKeys.Face_Key

            bytSitKey = objKeys.Sit_Stand_Key

            objDAOC.DoInit()

            AddHandler _AKHelper.OnLog, AddressOf LogLineAsync
            _AKHelper.DoInit()

            _MainFormTitle = "AKHeal - " & objDAOC.PlayerName

            LoadProfile()

            objDAOC.AddString(0, "Target is not in view*")
            objDAOC.AddString(1, "Your target is diseased*")
            objDAOC.AddString(2, "Your target is poisoned*")
            'objDAOC.AddString(3, "*You are hit for*")
            'objDAOC.AddString(4, "*not enough power*")

            'get heals
            Dim blnHasSpread As Boolean = cbxEnableSpread.Checked
            Dim bytSpreadKey As Byte = objEncoder.GetBytes(tbSpreadKey.Text)(0)
            Dim intSpreadThreshold As Integer = nudSpreadThreshold.Value
            Dim intSpreadDelay As Integer = nudSpreadDelay.Value
            Dim intSpreadCount As Integer = nudSpreadMembersThreshold.Value
            Dim strSpreadName As String = tbSpreadName.Text

            Dim colHeals As Collection = New Collection
            Dim objHeal As clsHeal
            Dim lvItem As ListViewItem

            For Each lvItem In lvHeals.Items
                objHeal = New clsHeal
                objHeal.Name = lvItem.SubItems(0).Text
                objHeal.Delay = lvItem.SubItems(3).Text
                objHeal.Key = objEncoder.GetBytes(lvItem.SubItems(2).Text)(0)
                objHeal.InCombat = lvItem.SubItems(4).Text = "Yes"
                objHeal.Threshold = lvItem.SubItems(1).Text
                colHeals.Add(objHeal)
            Next

            'Dim objGroupMemberInfo As AKServer.DLL.DAoCServer.Group = objDAOC.GroupMemberInfo
            'Dim htGroupMemberTable As Hashtable = objGroupMemberInfo.GroupMemberTable
            'Dim objGroupMember As AKServer.DLL.DAoCServer.GroupMember

            'Dim htMembers As Hashtable = New Hashtable

            Dim blnEffectsFlag As Boolean = False
            Dim shtLastID As Short = 0

            Dim spellTimer As AKHelper.SpellTimer

            While Not blnTerminateThread
                If IsDAOCActive() And objDAOC.IsPlayerDead = 0 Then

                    If Not blnEffectsFlag Then
                        blnEffectsFlag = True

                        LogLineAsync("Setting /effects to none")
                        _AKHelper.SetEffectsToNone(objDAOC)

                        For Each spellTimer In _SpellTimers.Values
                            spellTimer.Process = True
                            spellTimer.Start()
                        Next
                    End If

                    'taking place of cbxAutoGroup
                    Dim blnAutoGroup As Boolean = True

                    Dim blnInCombat As Boolean = False

                    Dim objLastHealed As clsMember = Nothing
                    Dim objHealMember As clsMember = Nothing
                    Dim objMember As clsMember = Nothing
                    Dim strLastFailed As String = ""    ' try to heal, if fail, set this and next loop skip this one

                    Dim bytHealKey As Byte = 0
                    Dim intHealDelay As Integer = 0
                    Dim strHealName As String = ""

                    Dim blnSingleTarget As Boolean = True
                    Dim intMembersNeedHeal As Integer = 0 ' Used for SpreadHeal

                    'the logic IS NOT in for this it will always be FALSE
                    Dim blnHasMoved As Boolean = False

                    If blnAutoGroup Then
                        Dim objGroupMemberInfo As AKServer.DLL.DAoCServer.Group = objDAOC.GroupMemberInfo
                        Dim htGroupMemberTable As Hashtable = objGroupMemberInfo.GroupMemberTable
                        Dim objGroupMember As AKServer.DLL.DAoCServer.GroupMember

                        Dim htMemberNames As Hashtable = New Hashtable
                        Dim colRemove As Collection = New Collection
                        Dim strName As String = ""

                        If blnAddNewMember = True Then
                            blnAddNewMember = False

                            AddHealTarget(objDAOC, strNewMember)
                        End If

                        'update htMembers hashtable
                        For Each objGroupMember In htGroupMemberTable.Values
                            If objGroupMember.Name <> "" Then
                                strName = objGroupMember.Name
                                htMemberNames.Add(strName.Clone, strName.Clone)
                                objMember = htMembers(CStr(objGroupMember.Name))
                                If IsNothing(objMember) Then
                                    objMember = New clsMember
                                    objMember.Name = objGroupMember.Name
                                    objMember.Heal = True
                                    objMember.InGroup = True
                                    objMember.Index = -1
                                    objMember.Priority = 0
                                    htMembers.Add(CStr(objGroupMember.Name), objMember)
                                    LogLineAsync("objMember.Name: " & objMember.Name)
                                Else
                                    objMember.InGroup = True
                                End If
                            End If
                        Next

                        'see if a name/key in htMembers does not exist in htMemberNames, if it doesn't
                        'add it to the colRemove collection
                        For Each objMember In htMembers.Values
                            If objMember.InGroup Then
                                If Not htMemberNames.ContainsKey(objMember.Name) Then
                                    colRemove.Add(objMember.Name)
                                End If
                            End If
                        Next

                        'remove all memeber from htMembers that exist in the colRemove collection
                        For Each strName In colRemove
                            htMembers.Remove(strName)
                            LogLineAsync("htMembers.Remove: " & strName)
                        Next
                    End If 'If blnAutoGroup Then

                    For Each objMember In htMembers.Values
                        'get the members current index
                        If objMember.Name = objDAOC.PlayerName Then
                            objMember.Index = objDAOC.PlayerID
                        Else
                            Try 'HACK for AK bug
                                objMember.Index = objDAOC.SetTarget(objMember.Name, False)
                            Catch ex As Exception
                                'if it bombs set the index to the healers for now
                                objMember.Index = objDAOC.PlayerID
                            End Try

                            'If objMember.Index <> shtLastID Then
                            '    LogLineAsync(objMember.Name & "'s id changed from " & shtLastID & " to " & objMember.Index)
                            '    shtLastID = objMember.Index
                            'End If
                        End If

                        If objMember.LastHealed Then
                            objLastHealed = objMember
                            objMember.LastHealed = False
                        End If

                        'set members current health
                        If objMember.Index <> -1 Then
                            objMember.PreviousHealth = objMember.Health

                            If objMember.Name = objDAOC.PlayerName Then
                                objMember.Health = objDAOC.PlayerHealth
                            Else
                                Try 'HACK - this is throwing an exception (bug) atm 
                                    objMember.Health = objDAOC.MobHealth(objMember.Index)
                                Catch ex As Exception
                                End Try
                            End If

                            'set objMember.InRange
                            If objMember.Index = objDAOC.PlayerID Then
                                objMember.InRange = True
                            Else
                                'might have to disable range checks in TD's
                                objMember.InRange = True
                                'objMember.InRange = objDAOC.ZDistance(objDAOC.gPlayerXCoord, objDAOC.gPlayerYCoord, objDAOC.gPlayerZCoord, _
                                '    objDAOC.MobXCoord(objMember.Index), objDAOC.MobYCoord(objMember.Index), objDAOC.MobZCoord(objMember.Index)) < 2000
                            End If

                            'might comment out
                            objMember.PriorityHealth = objMember.Health
                            If objMember.Health <> 100 AndAlso objMember.Priority = 1 Then
                                objMember.PriorityHealth = objMember.Health - objMember.Health / 3
                            ElseIf objMember.Health <> 100 AndAlso objMember.Priority = 2 Then
                                objMember.PriorityHealth = objMember.Health + ((100 - objMember.Health) / 4)
                            End If

                            'If Member.Health > 0 And Member.Health < 100 And Member.InRange And Member.Heal Then
                            If objMember.Health < 100 AndAlso objMember.InRange AndAlso objMember.Heal Then ' Add Rez as a 0 health Heal ???
                                If IsNothing(objHealMember) AndAlso (strLastFailed <> objMember.Name) Then
                                    objHealMember = objMember
                                ElseIf objMember.PriorityHealth < objHealMember.PriorityHealth AndAlso (strLastFailed <> objMember.Name) Then
                                    objHealMember = objMember
                                End If
                            End If

                            'spreadheal logic
                            If objMember.InGroup AndAlso objMember.Health < intSpreadThreshold AndAlso objMember.Health > 0 _
                                AndAlso objMember.InRange AndAlso objMember.Heal AndAlso (strLastFailed <> objMember.Name) Then
                                intMembersNeedHeal = intMembersNeedHeal + 1
                            End If

                            'in combat check might have to disable
                            'blnInCombat = blnInCombat OrElse (objDAOC.isMobInCombat(objMember.Index) <> 0)
                            blnInCombat = True
                        End If
                    Next 'For Each objMember In htMembers.Values

                    Dim blnDidHeal As Boolean = False

                    If IsDAOCActive() AndAlso Not IsNothing(objHealMember) AndAlso objDAOC.QueryString(4) = False Then
                        ' Simple SpreadHeal 'hack', might revise
                        If intMembersNeedHeal > intSpreadCount AndAlso blnHasSpread AndAlso objHealMember.InGroup Then
                            blnSingleTarget = False

                            bytHealKey = bytSpreadKey
                            intHealDelay = intSpreadDelay
                            strHealName = strSpreadName
                        Else       ' actual healing decided
                            For Each objHeal In colHeals
                                If objHealMember.Health < objHeal.Threshold Then
                                    If objHeal.InCombat OrElse Not blnInCombat Then
                                        bytHealKey = objHeal.Key
                                        intHealDelay = objHeal.Delay
                                        strHealName = objHeal.Name
                                    End If
                                ElseIf objHealMember.Health = 0 AndAlso objHeal.Threshold = 0 Then       ' Dead Member
                                    If objHeal.InCombat OrElse Not blnInCombat Then
                                        bytHealKey = objHeal.Key
                                        intHealDelay = objHeal.Delay
                                        strHealName = objHeal.Name
                                    End If
                                Else
                                    ' Previous heal wasnt good enough since current health is lower then before the heal
                                    ' (Do not do this if the member in question is the healer, since he was most likely interrupted)
                                    If objHealMember.Health < objHealMember.PreviousHealth AndAlso objHealMember Is objLastHealed AndAlso objHealMember.Name <> objDAOC.PlayerName Then
                                        If objHeal.InCombat OrElse Not blnInCombat Then
                                            bytHealKey = objHeal.Key
                                            intHealDelay = objHeal.Delay
                                            strHealName = objHeal.Name
                                        End If
                                    End If

                                    Exit For
                                End If
                            Next
                        End If 'If intMembersNeedHeal > intSpreadCount AndAlso blnHasSpread AndAlso objHealMember.InGroup Then

                        If bytHealKey <> 0 Then
                            If Not blnHasMoved Then
                                ' Always target the member that needs a heal the most
                                ' this way you can instant-heal the person on the move
                                ' or when spread heal is done.
                                Dim shtOrgIndex As Short = objHealMember.Index

                                objDAOC.SetTarget(objHealMember.Index)
                                objHealMember.Index = shtOrgIndex

                                If blnSingleTarget Then
                                    LogLineAsync("Casting: " & strHealName & " on " & objHealMember.Name)

                                    'objHealMember.lvItem.BackColor = Color.Green 'not used atm
                                    Try
                                        CastTargetHeal(objDAOC, objHealMember, bytHealKey, intHealDelay)

                                        objHealMember.LastHealed = True
                                    Finally
                                        'not used yet
                                        'objHealMember.lvItem.BackColor = Color.White 'not used atm
                                    End Try
                                Else
                                    LogLineAsync("Casting: " & strHealName)
                                    CastGroupHeal(objDAOC, bytHealKey, intHealDelay)
                                End If

                                ' added check for not enough power when casting
                                If objDAOC.QueryString(4) Then
                                    If blnSingleTarget Then
                                        strLastFailed = objHealMember.Name
                                    Else
                                        strLastFailed = ""
                                    End If
                                    blnDidHeal = False
                                Else
                                    strLastFailed = ""
                                    blnDidHeal = True
                                End If
                            Else 'WONT GET HERE EVER ATM
                                If blnSingleTarget Then
                                    LogLineAsync("Has moved, cant cast " & strHealName & " on " & objHealMember.Name & "!")
                                Else
                                    LogLineAsync("Has moved, cant cast " & strHealName & " on group!")
                                End If
                            End If 'If Not blnHasMoved Then
                        End If 'If bytHealKey <> 0 Then
                    End If 'If IsDAOCActive() AndAlso Not IsNothing(objHealMember) AndAlso objDAOC.QueryString(4) = False Then

                    'SIT LOGIC GOES HERE, ADD LATER
                    'If cbSitMode.SelectedIndex > 0 Then
                    'End If

                ElseIf objDAOC.IsPlayerDead = 1 Then
                    ' accept rez / wait for rez
                    'WaitForRez(objDAOC)
                End If 'If IsDAOCActive() And objDAOC.IsPlayerDead = 0 Then


                'keep alive logic
                If blnKeepAlive Then
                    blnKeepAlive = False

                    tmeKeepAlive.Stop()

                    objDAOC.SendKeys(bytShowInventoryKey, False)
                    objAKHeal.CurrentThread.Sleep(500)
                    objDAOC.SendKeys(bytShowInventoryKey, False)

                    tmeKeepAlive.Start()
                End If

                'spelltimers
                For Each spellTimer In _SpellTimers.Values
                    If spellTimer.Process = True Then
                        spellTimer.Process = False

                        _AKHelper.Cast(objDAOC, "", "", spellTimer.SpellList, False)
                        objAKHeal.Sleep(250)
                        objDAOC.SendString("/qbar 1~")
                        objAKHeal.Sleep(250)

                        spellTimer.Start()
                    End If
                Next

                objAKHeal.Sleep(250)

                'If (objDAOC.IsPlayerDead = 1) AndAlso cbxStopAfterDeath.Checked Then
                '    blnTerminateThread = True
                'End If
            End While 'While Not blnTerminateThread

        Catch ex As Exception
            LogLineAsync("Exception occured! Heal-O-Matic stopped!")
            LogLineAsync(ex.Message)
            LogLineAsync(ex.Source)
            LogLineAsync(ex.StackTrace)
            MsgBox(ex.Message)
            MsgBox(ex.Source)
            MsgBox(ex.StackTrace)
        Finally
            objDAOC.StopInit()
            _AKHelper.StopInit()

            'Update
            Dim mi4 As UpdateText
            mi4 = New UpdateText(AddressOf UpdateTextBox)
            Me.BeginInvoke(mi4, New Object() {btnStartStop, "Start"})
            'btnLoad.Text = "Start"

            objAKHeal = Nothing
            objDAOC = Nothing
        End Try
    End Sub

    Private Sub ProcessQuery(ByVal e As AutoKillerScript.clsAutoKillerScript.AutokillerRegExEventParams)

        Dim methodName As String = "ProcessQuery()"

        Dim playerName As String = ""
        Dim message As String = ""

        Dim casterName As String = ""
        Dim targetPlayerNameList As String = ""
        Dim spellList As String = ""

        LogLineAsync(_AKHelper.GetLogNameByIndex(e.QueryID))

        Select Case _AKHelper.GetLogNameByIndex(e.QueryID)
            Case "PassThru"
                playerName = e.RegExMatch.Groups("PlayerName").Value
                message = e.RegExMatch.Groups("Message").Value

                If _AKHelper.ValidateUser(playerName) Then
                    LogLineAsync("Valid player name: " & playerName)
                    LogLineAsync("Message: " & message)
                    _AKHelper.PassThru(objDAOC, message, _AKHelper.GetSetting("PassThruFlag"))
                Else
                    LogLineAsync("INVALID PLAYER NAME: " & playerName)
                End If

            Case "AutoFollow"
                playerName = e.RegExMatch.Groups("PlayerName").Value
                message = e.RegExMatch.Groups("Message").Value

                If _AKHelper.ValidateUser(playerName) Then
                    LogLineAsync("Valid player name: " & playerName)
                    LogLineAsync("Message: " & message)
                    _AKHelper.AutoFollow(objDAOC, playerName, bytStickKey)
                Else
                    LogLineAsync("INVALID PLAYER NAME: " & playerName)
                End If

            Case "BreakAutoFollow"
                playerName = e.RegExMatch.Groups("PlayerName").Value
                message = e.RegExMatch.Groups("Message").Value

                If _AKHelper.ValidateUser(playerName) Then
                    LogLineAsync("Valid player name: " & playerName)
                    LogLineAsync("Message: " & message)
                    _AKHelper.BreakAutoFollow(objDAOC, bytMoveBackwardKey)
                Else
                    LogLineAsync("INVALID PLAYER NAME: " & playerName)
                End If

            Case "Disband"
                playerName = e.RegExMatch.Groups("PlayerName").Value
                message = e.RegExMatch.Groups("Message").Value

                If _AKHelper.ValidateUser(playerName) Then
                    LogLineAsync("Valid player name: " & playerName)
                    LogLineAsync("Message: " & message)
                    _AKHelper.Disband(objDAOC)
                Else
                    LogLineAsync("INVALID PLAYER NAME: " & playerName)
                End If

            Case "AcceptDlg"
                playerName = e.RegExMatch.Groups("PlayerName").Value

                If _AKHelper.ValidateUser(playerName) Then
                    LogLineAsync("Valid player name: " & playerName)
                    _AKHelper.AcceptDialog(objDAOC)
                Else
                    LogLineAsync("INVALID PLAYER NAME: " & playerName)
                End If

            Case "Cast"
                playerName = e.RegExMatch.Groups("PlayerName").Value
                casterName = e.RegExMatch.Groups("CasterName").Value
                spellList = e.RegExMatch.Groups("SpellList").Value
                targetPlayerNameList = e.RegExMatch.Groups("TargetPlayerNameList").Value

                If _AKHelper.ValidateUser(playerName) Then
                    LogLineAsync("Valid player name:" & playerName)
                    LogLineAsync("CasterName:" & casterName)
                    LogLineAsync("TargetPlayerNameList:" & targetPlayerNameList)
                    LogLineAsync("SpellList:" & spellList)

                    If casterName = "" Then
                        _AKHelper.Cast(objDAOC, playerName, targetPlayerNameList, spellList, True)
                    Else
                        If casterName = objDAOC.PlayerName.ToLower Then
                            _AKHelper.Cast(objDAOC, playerName, targetPlayerNameList, spellList, True)
                        End If
                    End If

                    objAKHeal.Sleep(250)
                    objDAOC.SendString("/qbar 1~")
                    objAKHeal.Sleep(250)

                Else
                    LogLineAsync("INVALID PLAYER NAME: " & playerName)
                End If

            Case "AddHealTarget"
                playerName = e.RegExMatch.Groups("PlayerName").Value
                casterName = e.RegExMatch.Groups("CasterName").Value
                targetPlayerNameList = e.RegExMatch.Groups("TargetPlayerNameList").Value

                If _AKHelper.ValidateUser(playerName) Then
                    LogLineAsync("Valid player name:" & playerName)
                    LogLineAsync("CasterName:" & casterName)
                    LogLineAsync("TargetPlayerNameList:" & targetPlayerNameList)

                    If casterName = "" Then
                        blnAddNewMember = True
                        strNewMember = targetPlayerNameList
                    Else
                        If casterName = objDAOC.PlayerName.ToLower Then
                            blnAddNewMember = True
                            strNewMember = targetPlayerNameList
                        End If
                    End If
                Else
                    LogLineAsync("INVALID PLAYER NAME: " & playerName)
                End If

            Case "Test"
                playerName = e.RegExMatch.Groups("PlayerName").Value

                If _AKHelper.ValidateUser(playerName) Then
                    LogLineAsync("Valid player name: " & playerName)

                Else
                    LogLineAsync("INVALID PLAYER NAME: " & playerName)
                End If

        End Select

    End Sub

    Private Sub AddHealTarget(ByVal objdaoc As AutoKillerScript.clsAutoKillerScript, _
                              ByVal Target As String)

        Dim index As Short
        Dim name As String
        Dim member As clsMember

        index = objdaoc.SetTarget(Target, False)

        If index <> -1 Then
            name = objdaoc.MobName(index)

            ' Check if it wasnt already in the list
            member = htMembers(name)
            If IsNothing(member) OrElse (index <> member.Index) Then
                member = New clsMember
                member.Name = name
                member.Heal = True
                member.InGroup = False
                member.Index = index
                member.Priority = 0

                htMembers.Add(name, member)
                LogLineAsync("objMember.Name: " & member.Name)
            End If
        End If

    End Sub

    Private Sub CastGroupHeal(ByVal objDAOC As AutoKillerScript.clsAutoKillerScript, ByVal Heal As Byte, ByVal Delay As Integer)
        If objDAOC.isPlayerSitting <> 0 Then
            LogLineAsync(objDAOC.PlayerName & " is sitting, standing up to cast.")
            objDAOC.SendKeys(bytSitKey, 0)

            ' Delay for standing up
            objAKHeal.Sleep(100)
            'TotalSleep = TotalSleep + 100
        End If

        objDAOC.SendKeys(Heal, 0)

        objAKHeal.Sleep(Delay)
        'TotalSleep = TotalSleep + Delay

        LogLineAsync("Group Healed")
    End Sub

    Private Sub CastTargetHeal(ByVal objDAOC As AutoKillerScript.clsAutoKillerScript, ByVal Member As clsMember, ByVal Heal As Byte, ByVal Delay As Integer)
        If objDAOC.isPlayerSitting <> 0 Then
            LogLineAsync(objDAOC.PlayerName & " is sitting, standing up to cast.")
            objDAOC.SendKeys(bytSitKey, 0)

            ' Delay for standing up
            objAKHeal.Sleep(100)
        Else
            ' Delay for targeting (done outside this function)
            objAKHeal.Sleep(100)
        End If

        If objDAOC.TargetIndex = Member.Index Then
            objDAOC.SendKeys(Heal, 0)

            objAKHeal.Sleep(Delay)

            LogLineAsync("Healed " & Member.Name & " from " & Member.Health & "% HP to " & objDAOC.MobHealth(Member.Index) & "% HP")

            'poison logic
            'If objDAOC.QueryString(2) Then
            '    objDAOC.SendKeys(K_Poison)
            '    objAKHeal.Sleep(2500)
            'End If

            'dz logic
            'If objDAOC.QueryString(1) Then
            '    objDAOC.SendKeys(K_Disease)
            '    objAKHeal.Sleep(2500)
            'End If
        Else
            LogLineAsync("Target index is invalid (" & Member.Index & ")(" & objDAOC.TargetIndex & ")")
        End If
    End Sub

    Private Sub cbxProfiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxProfiles.SelectedIndexChanged
        If IO.File.Exists(cbxProfiles.Text & ".prf") Then
            Try
                Dim xmlDoc As Xml.XmlDocument = New Xml.XmlDocument
                xmlDoc.Load(cbxProfiles.Text & ".prf")

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

    Private Sub btnSaveProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveProfile.Click
        If IO.File.Exists(cbxProfiles.Text & ".prf") Then
            If MsgBox("The profile already exists, do you want to overwrite it?", MsgBoxStyle.OKCancel) = MsgBoxResult.Cancel Then
                Return
            End If
        End If

        Dim xmlDoc As Xml.XmlDocument = New Xml.XmlDocument
        Dim xmlNode As Xml.XmlNode

        xmlDoc.LoadXml("<Profile/>")

        xmlNode = xmlDoc.CreateElement("Settings")

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

        xmlDoc.Save(cbxProfiles.Text & ".prf")

        Dim Profile As String = cbxProfiles.Text

        UpdateProfiles()

        Dim C As Integer
        For C = 0 To cbxProfiles.Items.Count - 1
            If cbxProfiles.Items.Item(C) = Profile Then
                cbxProfiles.SelectedIndex = C
            End If
        Next
    End Sub

    Private Sub btnAddHeal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddHeal.Click
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

    Private Sub btnRemoveHeal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveHeal.Click
        Dim lvItem As ListViewItem

        If lvHeals.SelectedItems.Count > 0 Then
            For Each lvItem In lvHeals.SelectedItems
                lvHeals.Items.Remove(lvItem)
            Next
        End If
    End Sub

    Private Sub UpdateProfiles()
        cbxProfiles.Items.Clear()

        Dim Filename As String
        For Each Filename In IO.Directory.GetFiles(".")
            If IO.Path.GetExtension(Filename).ToUpper = ".PRF" Then
                cbxProfiles.Items.Add(IO.Path.GetFileNameWithoutExtension(Filename))
            End If
        Next
    End Sub

    Private Sub addAttribute(ByVal xmlDoc As Xml.XmlDocument, ByVal xmlNode As Xml.XmlNode, ByVal Attr As String, ByVal value As String)
        Dim xmlAttr As Xml.XmlAttribute
        xmlAttr = xmlDoc.CreateAttribute(Attr)
        xmlAttr.Value = value
        xmlNode.Attributes.Append(xmlAttr)
    End Sub

    Private Function ReadSetting(ByVal key As String, ByVal type As Type)
        Dim configurationAppSettings As System.Configuration.AppSettingsReader

        configurationAppSettings = New System.Configuration.AppSettingsReader

        Return configurationAppSettings.GetValue(key, type)
    End Function

    Private Delegate Sub UpdateText(ByRef aTextBox As Object, ByRef Text As Object)

    Private Delegate Sub LogIt(ByVal Line As String)

    Public Sub UpdateTextBox(ByRef aTextBox As Object, ByRef Text As Object)
        If TypeName(aTextBox) = "TextBox" Then
            CType(aTextBox, TextBox).Text = CStr(Text)
        ElseIf TypeName(aTextBox) = "Button" Then
            CType(aTextBox, Button).Text = CStr(Text)
        End If
    End Sub

    Private Sub LogLineAsync(ByVal Line As String)
        Dim mi As New LogIt(AddressOf LogLine)
        Me.BeginInvoke(mi, New Object() {Line})
    End Sub

    Private Sub LogLine(ByVal Line As String)
        Line = Format(Year(Now), "0000") & "-" & Format(Month(Now), "00") & "-" & Format(Microsoft.VisualBasic.Day(Now), "00") & "|" & Format(Hour(Now), "00") & ":" & Format(Minute(Now), "00") & ":" & Format(Second(Now), "00") & "| " & Line
        lbLog.BeginUpdate()
        lbLog.Items.Insert(0, Line)

        If lbLog.Items.Count > 128 Then
            lbLog.Items.RemoveAt(127)
        End If
        lbLog.EndUpdate()
    End Sub

    Private Delegate Sub UpdateTitle(ByRef Text As Object)

    Private Sub UpdateFormTitle(ByRef Text As Object)
        Me.Text = CStr(Text)
    End Sub

    Private Sub LoadProfile()

        Dim methodName As String = "LoadProfile()"

        Dim encoder As New System.Text.UTF8Encoding

        Dim xmlDoc As Xml.XmlDocument = New Xml.XmlDocument
        Dim xmlNode As Xml.XmlNode
        Dim xmlNode2 As Xml.XmlNode

        Dim count As Integer

        Try

            LogLineAsync("--------------------------------------------------------------------------------")

            _AKHelper.LoadChatLogLines("AKHelper.xml")
            count = _AKHelper.AddChatLogLinesStrings(objDAOC, 3)

            _AKHelper.LoadLogLines("AKHelper.xml")
            count = _AKHelper.AddLogLinesStrings(objDAOC, count)

            _AKHelper.LoadUsers("AKHelper.xml")
            _AKHelper.LoadSettings("AKHelper.xml")
            _AKHelper.LoadSpells("AKHelper.xml")

            _AKHelper.LoadSpellTimers("AKHelper.xml")
            _SpellTimers = _AKHelper.GetSpellTimers

            LogLineAsync("--------------------------------------------------------------------------------")

        Catch Ex As Exception
            LogLineAsync(Ex.Message)
        End Try

    End Sub

    Private Function IsDAOCActive() As Boolean

        If GetForegroundWindow = FindWindow("DAocMWC", Nothing) Then
            Dim mi As New UpdateTitle(AddressOf UpdateFormTitle)
            '_MainForm.BeginInvoke(mi, New Object() {"AKRemoteAni (Active)"})
            Me.BeginInvoke(mi, New Object() {_MainFormTitle & " (Active)"})
            'Text = "AKNecro (Active)"

            Return True
        Else
            Dim mi As New UpdateTitle(AddressOf UpdateFormTitle)
            '_MainForm.BeginInvoke(mi, New Object() {"AKRemoteAni (Paused)"})
            Me.BeginInvoke(mi, New Object() {_MainFormTitle & " (Paused)"})
            'Text = "AKNecro (Active)"

            Return False

        End If
    End Function

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim regKey As RegistryKey
        Dim strLastProfile As String
        Dim strDefault As String = "Default"

        LogLine("AKHeal v0.01 (c) Version125")

        'Try
        '    System.Diagnostics.Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.AboveNormal
        'Catch Ex As Exception
        '    ' On Windows 98 machines you get an error for 'above normal'
        '    System.Diagnostics.Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.High
        'End Try

        'cbxPriority.SelectedIndex = 0

        UpdateProfiles()

        'load saved setting from registry
        'get registry info
        regKey = Registry.LocalMachine.OpenSubKey("Software\AKHeal")

        'key doesn't exist, make it
        If regKey Is Nothing Then
            regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE", True)
            regKey.CreateSubKey("AKHeal")
            'regKey.Close()
        End If

        'lastprofile
        strLastProfile = regKey.GetValue("LastProfile", "")

        'form position
        Me.Top = regKey.GetValue("FormTop", 22)
        Me.Left = regKey.GetValue("FormLeft", 22)

        'options
        rbCatacombs.Checked = regKey.GetValue("Catacombs", "1")
        rbToA.Checked = regKey.GetValue("ToA", "0")

        tbCatacombs.Text = regKey.GetValue("CatacombsPath", "c:\mythic\catacombs")
        tbToA.Text = regKey.GetValue("ToAPath", "c:\mythic\atlantis")
        tbRegKey.Text = regKey.GetValue("RegKey", "")

        If tbCatacombs.Text = "" Then tbCatacombs.Text = "c:\mythic\catacombs"
        If tbToA.Text = "" Then tbToA.Text = "c:\mythic\atlantis"
        If tbRegKey.Text = "" Then tbRegKey.Text = ""

        Dim C As Integer
        If Not strLastProfile = "" Then
            'try and load last used profile
            For C = 0 To cbxProfiles.Items.Count - 1
                If cbxProfiles.Items.Item(C).ToLower = strLastProfile.ToLower Then
                    cbxProfiles.SelectedIndex = C
                End If
            Next
        End If

        If cbxProfiles.SelectedItem Is Nothing Then
            For C = 0 To cbxProfiles.Items.Count - 1
                If cbxProfiles.Items.Item(C).ToLower = strDefault.ToLower Then
                    cbxProfiles.SelectedIndex = C
                End If
            Next
        End If

        Threading.Thread.CurrentThread.Name = "MainThread"
    End Sub

    Private Sub frmMain_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Dim strLastProfile = cbxProfiles.SelectedItem
        Dim regKey As RegistryKey

        Try
            regKey = Registry.LocalMachine.OpenSubKey("Software\AKHeal", True)

            If Not strLastProfile = "" Then
                'lastprofile
                regKey.SetValue("LastProfile", strLastProfile)
            End If

            'form position
            regKey.SetValue("FormTop", Me.Top)
            regKey.SetValue("FormLeft", Me.Left)

            'options
            regKey.SetValue("Catacombs", rbCatacombs.Checked)
            regKey.SetValue("ToA", rbToA.Checked)

            regKey.SetValue("CatacombsPath", tbCatacombs.Text)
            regKey.SetValue("ToAPath", tbToA.Text)
            regKey.SetValue("RegKey", tbRegKey.Text)

            regKey.Close()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub tmeKeepAlive_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles tmeKeepAlive.Elapsed
        blnKeepAlive = True
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim objDaoc As AutoKillerScript.clsAutoKillerScript

        Try

            objDaoc = New AutoKillerScript.clsAutoKillerScript

            LogLineAsync("Initializing DAOCScript v" & objDaoc.getVersion & "....")

            ' This section sets up various variables for the DLL.

            objDaoc.RegKey = "DEREKBEATTIE1249"
            objDaoc.GamePath = "c:\mythic\catacombs"
            objDaoc.EnableToA = False
            objDaoc.EnableCatacombs = True
            
            objDaoc.DoInit()

            Dim temp As Short
            temp = objDaoc.TargetIndex()
            LogLine("name: " & objDaoc.MobName(temp))
            LogLine(objDaoc.ZDistance(objDaoc.gPlayerXCoord, objDaoc.gPlayerYCoord, objDaoc.gPlayerZCoord, _
                    objDaoc.MobXCoord(temp), objDaoc.MobYCoord(temp), objDaoc.MobZCoord(temp)))

            LogLine("level: " & objDaoc.MobLevel(temp))

            objDaoc.StopInit()

        Catch ex As Exception
            LogLine(ex.Message)
        End Try
    End Sub
End Class
