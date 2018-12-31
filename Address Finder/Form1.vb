Imports System.IO
Public Class Form1
    Inherits System.Windows.Forms.Form

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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbUS As System.Windows.Forms.RadioButton
    Friend WithEvents rbEuro As System.Windows.Forms.RadioButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rbEuro = New System.Windows.Forms.RadioButton
        Me.rbUS = New System.Windows.Forms.RadioButton
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(296, 16)
        Me.Label1.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button1.Location = New System.Drawing.Point(352, 8)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(88, 24)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Search"
        '
        'Button2
        '
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button2.Location = New System.Drawing.Point(312, 8)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(32, 24)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "..."
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(8, 72)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox1.Size = New System.Drawing.Size(432, 640)
        Me.TextBox1.TabIndex = 3
        Me.TextBox1.Text = ""
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.DefaultExt = "*.dll"
        Me.OpenFileDialog1.Filter = "DAOC|game.dll"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbEuro)
        Me.GroupBox1.Controls.Add(Me.rbUS)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 32)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(184, 40)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Region"
        '
        'rbEuro
        '
        Me.rbEuro.Location = New System.Drawing.Point(104, 16)
        Me.rbEuro.Name = "rbEuro"
        Me.rbEuro.Size = New System.Drawing.Size(56, 16)
        Me.rbEuro.TabIndex = 1
        Me.rbEuro.Text = "Euro"
        '
        'rbUS
        '
        Me.rbUS.Checked = True
        Me.rbUS.Location = New System.Drawing.Point(16, 16)
        Me.rbUS.Name = "rbUS"
        Me.rbUS.Size = New System.Drawing.Size(72, 16)
        Me.rbUS.TabIndex = 0
        Me.rbUS.TabStop = True
        Me.rbUS.Text = "US"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(448, 717)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Address Finder"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            Label1.Text = OpenFileDialog1.FileName
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ' find all values
        Dim address As IAddressFinder
        If rbUS.Checked Then
            address = New USAddressFinder(Label1.Text)
        Else
            address = New EuroAddressFinder(Label1.Text)
        End If
        TextBox1.Text = address.FindAdresses
    End Sub
End Class
