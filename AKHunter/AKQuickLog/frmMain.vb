Option Explicit On 
Option Strict Off

Imports Microsoft.Win32

Imports System
Imports System.Xml
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary


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

    Dim _QuickLogs As New SortedList
    Dim _ServerNumbers As New Hashtable
    'Dim _ServerNumbers As New SortedList

#End Region
#Region " Classes "
    <Serializable()> Class clsQuickLog
        Public Account As String
        Public Password As String
        Public CharacterName As String
        Public Description As String
        Public Realm As String
        Public ServerName As String
        Public ServerIP As String
        Public ServerPort As String
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
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tbQuickLogin As System.Windows.Forms.TabPage
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnChange As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnLogIn As System.Windows.Forms.Button
    Friend WithEvents cbxCharacterList As System.Windows.Forms.ComboBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cbxRealm As System.Windows.Forms.ComboBox
    Friend WithEvents tbServerPort As System.Windows.Forms.TextBox
    Friend WithEvents tbServerIP As System.Windows.Forms.TextBox
    Friend WithEvents tbDescription As System.Windows.Forms.TextBox
    Friend WithEvents tbPassword As System.Windows.Forms.TextBox
    Friend WithEvents tbAccount As System.Windows.Forms.TextBox
    Friend WithEvents tbCharacterName As System.Windows.Forms.TextBox
    Friend WithEvents cbxServerName As System.Windows.Forms.ComboBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents rbCatacombs As System.Windows.Forms.RadioButton
    Friend WithEvents rbToA As System.Windows.Forms.RadioButton
    Friend WithEvents rbSI As System.Windows.Forms.RadioButton
    Friend WithEvents tbOptions As System.Windows.Forms.TabPage
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents tbCatacombs As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents tbToA As System.Windows.Forms.TextBox
    Friend WithEvents tbSI As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents cbxCloseAfterLogin As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.tbQuickLogin = New System.Windows.Forms.TabPage
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.btnAdd = New System.Windows.Forms.Button
        Me.cbxCharacterList = New System.Windows.Forms.ComboBox
        Me.btnChange = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.cbxServerName = New System.Windows.Forms.ComboBox
        Me.cbxRealm = New System.Windows.Forms.ComboBox
        Me.tbServerPort = New System.Windows.Forms.TextBox
        Me.tbServerIP = New System.Windows.Forms.TextBox
        Me.tbDescription = New System.Windows.Forms.TextBox
        Me.tbPassword = New System.Windows.Forms.TextBox
        Me.tbAccount = New System.Windows.Forms.TextBox
        Me.tbCharacterName = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.btnLogIn = New System.Windows.Forms.Button
        Me.rbSI = New System.Windows.Forms.RadioButton
        Me.rbToA = New System.Windows.Forms.RadioButton
        Me.rbCatacombs = New System.Windows.Forms.RadioButton
        Me.tbOptions = New System.Windows.Forms.TabPage
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.Label12 = New System.Windows.Forms.Label
        Me.tbSI = New System.Windows.Forms.TextBox
        Me.tbToA = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.tbCatacombs = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.cbxCloseAfterLogin = New System.Windows.Forms.CheckBox
        Me.TabControl1.SuspendLayout()
        Me.tbQuickLogin.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.tbOptions.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tbQuickLogin)
        Me.TabControl1.Controls.Add(Me.tbOptions)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(290, 402)
        Me.TabControl1.TabIndex = 0
        '
        'tbQuickLogin
        '
        Me.tbQuickLogin.Controls.Add(Me.Panel1)
        Me.tbQuickLogin.Controls.Add(Me.Panel2)
        Me.tbQuickLogin.Controls.Add(Me.Panel3)
        Me.tbQuickLogin.Location = New System.Drawing.Point(4, 22)
        Me.tbQuickLogin.Name = "tbQuickLogin"
        Me.tbQuickLogin.Size = New System.Drawing.Size(282, 376)
        Me.tbQuickLogin.TabIndex = 0
        Me.tbQuickLogin.Text = "Quick Login"
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.btnAdd)
        Me.Panel1.Controls.Add(Me.cbxCharacterList)
        Me.Panel1.Controls.Add(Me.btnChange)
        Me.Panel1.Controls.Add(Me.btnDelete)
        Me.Panel1.Location = New System.Drawing.Point(8, 8)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(264, 80)
        Me.Panel1.TabIndex = 4
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(8, 40)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(64, 23)
        Me.btnAdd.TabIndex = 1
        Me.btnAdd.Text = "Add"
        '
        'cbxCharacterList
        '
        Me.cbxCharacterList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxCharacterList.Location = New System.Drawing.Point(8, 8)
        Me.cbxCharacterList.Name = "cbxCharacterList"
        Me.cbxCharacterList.Size = New System.Drawing.Size(240, 21)
        Me.cbxCharacterList.TabIndex = 0
        '
        'btnChange
        '
        Me.btnChange.Location = New System.Drawing.Point(80, 40)
        Me.btnChange.Name = "btnChange"
        Me.btnChange.Size = New System.Drawing.Size(64, 23)
        Me.btnChange.TabIndex = 2
        Me.btnChange.Text = "Change"
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(152, 40)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(64, 23)
        Me.btnDelete.TabIndex = 3
        Me.btnDelete.Text = "Delete"
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.cbxServerName)
        Me.Panel2.Controls.Add(Me.cbxRealm)
        Me.Panel2.Controls.Add(Me.tbServerPort)
        Me.Panel2.Controls.Add(Me.tbServerIP)
        Me.Panel2.Controls.Add(Me.tbDescription)
        Me.Panel2.Controls.Add(Me.tbPassword)
        Me.Panel2.Controls.Add(Me.tbAccount)
        Me.Panel2.Controls.Add(Me.tbCharacterName)
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Location = New System.Drawing.Point(8, 96)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(264, 224)
        Me.Panel2.TabIndex = 5
        '
        'cbxServerName
        '
        Me.cbxServerName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxServerName.Location = New System.Drawing.Point(96, 144)
        Me.cbxServerName.Name = "cbxServerName"
        Me.cbxServerName.Size = New System.Drawing.Size(152, 21)
        Me.cbxServerName.TabIndex = 11
        '
        'cbxRealm
        '
        Me.cbxRealm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxRealm.Items.AddRange(New Object() {"Albion", "Midgard", "Hibernia"})
        Me.cbxRealm.Location = New System.Drawing.Point(96, 112)
        Me.cbxRealm.Name = "cbxRealm"
        Me.cbxRealm.Size = New System.Drawing.Size(121, 21)
        Me.cbxRealm.TabIndex = 10
        '
        'tbServerPort
        '
        Me.tbServerPort.Location = New System.Drawing.Point(96, 192)
        Me.tbServerPort.Name = "tbServerPort"
        Me.tbServerPort.Size = New System.Drawing.Size(152, 20)
        Me.tbServerPort.TabIndex = 13
        Me.tbServerPort.Text = ""
        '
        'tbServerIP
        '
        Me.tbServerIP.Location = New System.Drawing.Point(96, 168)
        Me.tbServerIP.Name = "tbServerIP"
        Me.tbServerIP.Size = New System.Drawing.Size(152, 20)
        Me.tbServerIP.TabIndex = 12
        Me.tbServerIP.Text = ""
        '
        'tbDescription
        '
        Me.tbDescription.Location = New System.Drawing.Point(96, 88)
        Me.tbDescription.Name = "tbDescription"
        Me.tbDescription.Size = New System.Drawing.Size(152, 20)
        Me.tbDescription.TabIndex = 9
        Me.tbDescription.Text = ""
        '
        'tbPassword
        '
        Me.tbPassword.Location = New System.Drawing.Point(96, 32)
        Me.tbPassword.Name = "tbPassword"
        Me.tbPassword.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.tbPassword.Size = New System.Drawing.Size(152, 20)
        Me.tbPassword.TabIndex = 7
        Me.tbPassword.Text = ""
        '
        'tbAccount
        '
        Me.tbAccount.Location = New System.Drawing.Point(96, 8)
        Me.tbAccount.Name = "tbAccount"
        Me.tbAccount.Size = New System.Drawing.Size(152, 20)
        Me.tbAccount.TabIndex = 6
        Me.tbAccount.Text = ""
        '
        'tbCharacterName
        '
        Me.tbCharacterName.Location = New System.Drawing.Point(96, 64)
        Me.tbCharacterName.Name = "tbCharacterName"
        Me.tbCharacterName.Size = New System.Drawing.Size(152, 20)
        Me.tbCharacterName.TabIndex = 8
        Me.tbCharacterName.Text = ""
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(8, 192)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(64, 23)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Server Port:"
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(8, 168)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(64, 23)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "Server IP:"
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(8, 144)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(80, 23)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Server Name:"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(8, 32)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(64, 23)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Password:"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(8, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(48, 23)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Account:"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 23)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Character Name:"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 88)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 23)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Description:"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 112)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 23)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Realm:"
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.btnLogIn)
        Me.Panel3.Controls.Add(Me.rbSI)
        Me.Panel3.Controls.Add(Me.rbToA)
        Me.Panel3.Controls.Add(Me.rbCatacombs)
        Me.Panel3.Location = New System.Drawing.Point(8, 328)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(264, 40)
        Me.Panel3.TabIndex = 6
        '
        'btnLogIn
        '
        Me.btnLogIn.Location = New System.Drawing.Point(176, 8)
        Me.btnLogIn.Name = "btnLogIn"
        Me.btnLogIn.Size = New System.Drawing.Size(72, 23)
        Me.btnLogIn.TabIndex = 5
        Me.btnLogIn.Text = "Log In"
        '
        'rbSI
        '
        Me.rbSI.Location = New System.Drawing.Point(136, 8)
        Me.rbSI.Name = "rbSI"
        Me.rbSI.TabIndex = 2
        Me.rbSI.Text = "SI"
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
        'tbOptions
        '
        Me.tbOptions.Controls.Add(Me.Panel4)
        Me.tbOptions.Location = New System.Drawing.Point(4, 22)
        Me.tbOptions.Name = "tbOptions"
        Me.tbOptions.Size = New System.Drawing.Size(282, 376)
        Me.tbOptions.TabIndex = 1
        Me.tbOptions.Text = "Options"
        '
        'Panel4
        '
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.cbxCloseAfterLogin)
        Me.Panel4.Controls.Add(Me.Label12)
        Me.Panel4.Controls.Add(Me.tbSI)
        Me.Panel4.Controls.Add(Me.tbToA)
        Me.Panel4.Controls.Add(Me.Label11)
        Me.Panel4.Controls.Add(Me.tbCatacombs)
        Me.Panel4.Controls.Add(Me.Label9)
        Me.Panel4.Controls.Add(Me.Label10)
        Me.Panel4.Location = New System.Drawing.Point(8, 8)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(264, 152)
        Me.Panel4.TabIndex = 0
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(8, 80)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(24, 23)
        Me.Label12.TabIndex = 6
        Me.Label12.Text = "SI:"
        '
        'tbSI
        '
        Me.tbSI.Location = New System.Drawing.Point(72, 80)
        Me.tbSI.Name = "tbSI"
        Me.tbSI.Size = New System.Drawing.Size(176, 20)
        Me.tbSI.TabIndex = 5
        Me.tbSI.Text = ""
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
        Me.Label9.Text = "Game Paths"
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(8, 32)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(72, 23)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Catacombs:"
        '
        'cbxCloseAfterLogin
        '
        Me.cbxCloseAfterLogin.Location = New System.Drawing.Point(8, 120)
        Me.cbxCloseAfterLogin.Name = "cbxCloseAfterLogin"
        Me.cbxCloseAfterLogin.Size = New System.Drawing.Size(112, 24)
        Me.cbxCloseAfterLogin.TabIndex = 8
        Me.cbxCloseAfterLogin.Text = "Close after login"
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(290, 402)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmMain"
        Me.Text = "AKQuickLog"
        Me.TabControl1.ResumeLayout(False)
        Me.tbQuickLogin.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.tbOptions.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        QuickLogsAdd()
    End Sub

    Private Sub btnChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChange.Click
        QuickLogsChange()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        QuickLogsDelete()
    End Sub

    Private Sub btnLogIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogIn.Click
        Dim strQuickLogKey = cbxCharacterList.SelectedItem

        Dim strLaunch As String
        Dim strLaunchDirectory As String
        Dim strEuroFlag As String = "0"
        Dim strServerID As String
        Dim strRealm As String

        Dim objProcess As Process = New Process

        Try
            If Not strQuickLogKey = "" Then
                If rbCatacombs.Checked Then
                    strLaunchDirectory = tbCatacombs.Text
                ElseIf rbToA.Checked Then
                    strLaunchDirectory = tbToA.Text
                ElseIf rbSI.Checked Then
                    strLaunchDirectory = tbSI.Text
                End If

                If cbxRealm.SelectedItem = "Albion" Then
                    strRealm = "2"
                ElseIf cbxRealm.SelectedItem = "Midgard" Then
                    strRealm = "1"
                ElseIf cbxRealm.SelectedItem = "Hibernia" Then
                    strRealm = "3"
                End If

                strServerID = _ServerNumbers(cbxServerName.SelectedItem)

                strLaunch = " """ & strLaunchDirectory & """" & _
                            " " & strEuroFlag & _
                            " " & tbServerIP.Text & _
                            " " & tbServerPort.Text & _
                            " " & strServerID & _
                            " " & tbAccount.Text & _
                            " " & tbPassword.Text & _
                            " " & tbCharacterName.Text & _
                            " " & strRealm

                objProcess.StartInfo.FileName = "login.dll"
                objProcess.EnableRaisingEvents = False
                objProcess.StartInfo.UseShellExecute = False
                objProcess.StartInfo.Arguments = strLaunch
                objProcess.Start()

                If cbxCloseAfterLogin.Checked = True Then
                    Me.Close()
                End If

                'MsgBox(strLaunch)
            Else
                MsgBox("Select a Character")
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub cbxCharacterList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxCharacterList.SelectedIndexChanged
        Dim strQuickLogKey = cbxCharacterList.SelectedItem
        Dim objQuickLog As New clsQuickLog

        Try
            objQuickLog = _QuickLogs(strQuickLogKey)

            tbAccount.Text = objQuickLog.Account
            tbPassword.Text = objQuickLog.Password
            tbCharacterName.Text = objQuickLog.CharacterName
            tbDescription.Text = objQuickLog.Description
            cbxRealm.SelectedItem = objQuickLog.Realm
            cbxServerName.SelectedItem = objQuickLog.ServerName
            tbServerIP.Text = objQuickLog.ServerIP
            tbServerPort.Text = objQuickLog.ServerPort

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strQuickLogsKey As String
        Dim regKey As RegistryKey
        Dim strLastCharKey As String
        Dim strCloseAfterLogin As String

        Try
            'set combo box for realm
            cbxRealm.SelectedIndex = 0
            'cbxRealm.SelectedItem = "Albion"

            tbServerIP.Text = "208.254.16.XXX"

            'load server names
            If IO.File.Exists("Serverlist.xml") Then
                Try
                    Dim xmlDoc As Xml.XmlDocument = New Xml.XmlDocument
                    xmlDoc.Load("Serverlist.xml")

                    Dim xmlNode As Xml.XmlNode
                    For Each xmlNode In xmlDoc.SelectNodes("//sections/section/Server")
                        cbxServerName.Items.Add(xmlNode.SelectSingleNode("@name").Value)
                        _ServerNumbers.Add(xmlNode.SelectSingleNode("@name").Value, xmlNode.SelectSingleNode("@ServerID").Value)
                    Next

                    'set combo box for server
                    cbxServerName.SelectedIndex = 0

                Catch Ex As Exception
                    MsgBox(Ex.Message)
                End Try
            End If

            'load the hashtable
            QuickLogsLoad()

            'populate characterlist
            For Each strQuickLogsKey In _QuickLogs.Keys
                cbxCharacterList.Items.Add(strQuickLogsKey)
            Next

            'get registry info
            regKey = Registry.LocalMachine.OpenSubKey("Software\AKQuickLog")

            'key doesn't exist, make it
            If regKey Is Nothing Then
                regKey = Registry.LocalMachine.OpenSubKey("SOFTWARE", True)
                regKey.CreateSubKey("AKQuickLog")
                'regKey.Close()
            End If

            'lastcharkey
            strLastCharKey = regKey.GetValue("LastCharKey", "")

            'form position
            Me.Top = regKey.GetValue("FormTop", 22)
            Me.Left = regKey.GetValue("FormLeft", 22)

            'game paths
            tbCatacombs.Text = regKey.GetValue("CatacombsPath", "c:\mythic\catacombs")
            tbToA.Text = regKey.GetValue("ToAPath", "c:\mythic\atlantis")
            tbSI.Text = regKey.GetValue("SIPath", "c:\mythic\isles")

            If tbCatacombs.Text = "" Then tbCatacombs.Text = "c:\mythic\catacombs"
            If tbToA.Text = "" Then tbToA.Text = "c:\mythic\atlantis"
            If tbSI.Text = "" Then tbSI.Text = "c:\mythic\isles"

            strCloseAfterLogin = regKey.GetValue("CloseAfterLogin", "0")
            If strCloseAfterLogin = 1 Then
                cbxCloseAfterLogin.Checked = True
            End If

            If Not strLastCharKey = "" Then
                'see if last key exists in hashtable
                Dim objQuickLog As New clsQuickLog
                objQuickLog = _QuickLogs(strLastCharKey)

                If Not objQuickLog Is Nothing Then
                    cbxCharacterList.SelectedItem = strLastCharKey
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        'for testing
        'tbAccount.Text = "eddy652"
        'tbPassword.Text = "asdf"
        'tbCharacterName.Text = "Oexon"
        'tbDescription.Text = "bard"
        'tbServerIP.Text = "123.123.123.1"
        'tbServerPort.Text = "1234"
    End Sub

    Private Sub frmMain_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Dim strQuickLogKey = cbxCharacterList.SelectedItem
        Dim regKey As RegistryKey

        Try
            regKey = Registry.LocalMachine.OpenSubKey("Software\AKQuickLog", True)

            If Not strQuickLogKey = "" Then
                'lastcharkey
                regKey.SetValue("LastCharKey", strQuickLogKey)
            End If

            'form position
            regKey.SetValue("FormTop", Me.Top)
            regKey.SetValue("FormLeft", Me.Left)

            'game paths
            regKey.SetValue("CatacombsPath", tbCatacombs.Text)
            regKey.SetValue("ToAPath", tbToA.Text)
            regKey.SetValue("SIPath", tbSI.Text)

            If cbxCloseAfterLogin.Checked Then
                regKey.SetValue("CloseAfterLogin", "1")
            Else
                regKey.SetValue("CloseAfterLogin", "0")
            End If
            
            regKey.Close()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        QuickLogsSave()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        QuickLogsSave()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        QuickLogsLoad()
        
    End Sub

    Private Sub QuickLogsAdd()
        Try
            If tbAccount.Text = "" Then
                MsgBox("Account required")
            ElseIf tbPassword.Text = "" Then
                MsgBox("Password required")
            ElseIf tbCharacterName.Text = "" Then
                MsgBox("Character Name required")
            ElseIf tbServerIP.Text = "" Then
                MsgBox("Server IP required")
            ElseIf tbServerPort.Text = "" Then
                MsgBox("Server Port required")
            Else
                Dim objQuickLog As New clsQuickLog

                objQuickLog.Account = Trim(tbAccount.Text)
                objQuickLog.Password = Trim(tbPassword.Text)
                objQuickLog.CharacterName = Trim(tbCharacterName.Text)
                objQuickLog.Description = Trim(tbDescription.Text)
                objQuickLog.Realm = cbxRealm.SelectedItem
                objQuickLog.ServerName = cbxServerName.SelectedItem
                objQuickLog.ServerIP = Trim(tbServerIP.Text)
                objQuickLog.ServerPort = Trim(tbServerPort.Text)
                _QuickLogs.Add(objQuickLog.CharacterName & "-" & objQuickLog.ServerName, objQuickLog)

                QuickLogsRefresh()

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub QuickLogsChange()
        Try
            If tbAccount.Text = "" Then
                MsgBox("Account required")
            ElseIf tbPassword.Text = "" Then
                MsgBox("Password required")
            ElseIf tbCharacterName.Text = "" Then
                MsgBox("Character Name required")
            ElseIf tbServerIP.Text = "" Then
                MsgBox("Server IP required")
            ElseIf tbServerPort.Text = "" Then
                MsgBox("Server Port required")
            Else
                Dim objQuickLog As New clsQuickLog
                Dim strQuickLogsKey As String
                Dim strQuickLogKey = cbxCharacterList.SelectedItem

                objQuickLog.Account = Trim(tbAccount.Text)
                objQuickLog.Password = Trim(tbPassword.Text)
                objQuickLog.CharacterName = Trim(tbCharacterName.Text)
                objQuickLog.Description = Trim(tbDescription.Text)
                objQuickLog.Realm = cbxRealm.SelectedItem
                objQuickLog.ServerName = cbxServerName.SelectedItem
                objQuickLog.ServerIP = Trim(tbServerIP.Text)
                objQuickLog.ServerPort = Trim(tbServerPort.Text)

                _QuickLogs.Remove(strQuickLogKey)
                _QuickLogs.Add(objQuickLog.CharacterName & "-" & objQuickLog.ServerName, objQuickLog)

                QuickLogsRefresh()
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub QuickLogsDelete()
        Try
            Dim strQuickLogsKey As String
            Dim strQuickLogKey = cbxCharacterList.SelectedItem

            _QuickLogs.Remove(strQuickLogKey)

            QuickLogsRefresh()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub QuickLogsRefresh()
        Try
            Dim strQuickLogsKey As String

            cbxCharacterList.Items.Clear()

            tbAccount.Text = ""
            tbPassword.Text = ""
            tbCharacterName.Text = ""
            tbDescription.Text = ""
            cbxRealm.SelectedIndex = 0
            cbxServerName.SelectedIndex = 0
            tbServerIP.Text = "208.254.16.XXX"
            tbServerPort.Text = ""

            For Each strQuickLogsKey In _QuickLogs.Keys
                cbxCharacterList.Items.Add(strQuickLogsKey)
            Next

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub QuickLogsLoad()
        'Dim plainText As String
        Dim account As String
        Dim password As String
        Dim cipherText As String

        Dim passPhrase As String
        Dim saltValue As String
        Dim hashAlgorithm As String
        Dim passwordIterations As Integer
        Dim initVector As String
        Dim keySize As Integer

        Dim objFS As FileStream
        Dim objBinFormatter As New BinaryFormatter

        Try
            If IO.File.Exists("QuickLog.cfg") Then
                'plainText = "Hello, World!"             ' original plaintext

                passPhrase = "Pas5pr@se"                ' can be any string
                saltValue = "s@1tValue"                 ' can be any string
                hashAlgorithm = "SHA1"                  ' can be "MD5"
                passwordIterations = 2                  ' can be any number
                initVector = "@1B2c3D4e5F6g7H8"         ' must be 16 bytes
                keySize = 256                           ' can be 192 or 128

                objFS = New FileStream("QuickLog.cfg", FileMode.Open)
                _QuickLogs = objBinFormatter.Deserialize(objFS)
                objFS.Close()

                'iterate hashtable and encrypt account and password
                Dim objQuickLog As New clsQuickLog

                For Each objQuickLog In _QuickLogs.Values
                    account = objQuickLog.Account
                    password = objQuickLog.Password

                    account = RijndaelSimple.Decrypt(account, _
                                                passPhrase, _
                                                saltValue, _
                                                hashAlgorithm, _
                                                passwordIterations, _
                                                initVector, _
                                                keySize)

                    objQuickLog.Account = account

                    password = RijndaelSimple.Decrypt(password, _
                                                passPhrase, _
                                                saltValue, _
                                                hashAlgorithm, _
                                                passwordIterations, _
                                                initVector, _
                                                keySize)

                    objQuickLog.Password = password
                Next
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
            objFS.Close()
        End Try
    End Sub

    Private Sub QuickLogsSave()
        'Dim plainText As String

        Dim account As String
        Dim password As String
        Dim cipherText As String

        Dim passPhrase As String
        Dim saltValue As String
        Dim hashAlgorithm As String
        Dim passwordIterations As Integer
        Dim initVector As String
        Dim keySize As Integer

        Dim objBinFormatter As New BinaryFormatter
        Dim objFS As FileStream

        Try

            'plainText = "Hello, World!"             ' original plaintext

            passPhrase = "Pas5pr@se"                ' can be any string
            saltValue = "s@1tValue"                 ' can be any string
            hashAlgorithm = "SHA1"                  ' can be "MD5"
            passwordIterations = 2                  ' can be any number
            initVector = "@1B2c3D4e5F6g7H8"         ' must be 16 bytes
            keySize = 256                           ' can be 192 or 128

            'iterate hashtable and encrypt account and password
            Dim objQuickLog As New clsQuickLog

            For Each objQuickLog In _QuickLogs.Values
                account = objQuickLog.Account
                password = objQuickLog.Password

                cipherText = RijndaelSimple.Encrypt(account, _
                                            passPhrase, _
                                            saltValue, _
                                            hashAlgorithm, _
                                            passwordIterations, _
                                            initVector, _
                                            keySize)

                objQuickLog.Account = cipherText

                cipherText = RijndaelSimple.Encrypt(password, _
                                            passPhrase, _
                                            saltValue, _
                                            hashAlgorithm, _
                                            passwordIterations, _
                                            initVector, _
                                            keySize)

                objQuickLog.Password = cipherText
            Next

            objFS = New FileStream("QuickLog.cfg", FileMode.Create)
            objBinFormatter.Serialize(objFS, _QuickLogs)
            objFS.Close()

        Catch ex As Exception
            MsgBox(ex.Message)
            objFS.Close()
        End Try
    End Sub
End Class
