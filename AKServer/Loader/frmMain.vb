#Region "Imports"
Imports System.Xml
Imports Microsoft.Win32
Imports System.Net
Imports System.IO
Imports AKServer.DLL.DAoCServer
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Ipc
Imports System.Runtime.InteropServices
#End Region
Public Class frmMain
    <Runtime.InteropServices.DllImport("AKDataHook.dll", CharSet:=Runtime.InteropServices.CharSet.Auto)> _
    Shared Sub InstallHook()
    End Sub

    <Runtime.InteropServices.DllImport("AKDataHook.dll", CharSet:=Runtime.InteropServices.CharSet.Auto)> _
    Shared Sub RemoveHook()
    End Sub

    'Private Declare Sub InstallHook Lib "AKDataHook.dll" ()
    'Private Declare Sub RemoveHook Lib "AKDataHook.dll" ()
    Private packets As DAOCMain
    Private ref1 As ObjRef
    Private channel As IChannel

#Region "   Form Functions"
    Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            NotifyIcon1.Visible = False

            'DeleteRegKey()
            packets.CleanUp()

            RemotingServices.Disconnect(packets)
            ChannelServices.UnregisterChannel(channel)
            GC.Collect()
            GC.WaitForPendingFinalizers()
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
        RemoveHook()
    End Sub

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ExitToolStripMenuItem.Image = ImageList1.Images(0)

        'CreateRegKey()
        'CreateValues("Log Path", General.Value("AutoKiller", "chatpath", "C:\\chat.log", Application.StartupPath & "\akserver.ini"))

        RemoveHook()
        InstallHook()

        CreateRemote()

        Me.Hide()
        NotifyIcon1.Visible = True
        NotifyIcon1.Text = Me.Text

    End Sub

    Private Sub CreateRemote()

        'RemotingConfiguration.Configure("AKServer.Loader.exe.config", False) 'uncomment for remote errors

        Dim clientProvider As BinaryClientFormatterSinkProvider = New BinaryClientFormatterSinkProvider
        Dim serverProvider As BinaryServerFormatterSinkProvider = New BinaryServerFormatterSinkProvider
        serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full

        Dim props As IDictionary = New Hashtable
        props.Add("portName", "akserverport")
        props.Add("typeFilterLevel", System.Runtime.Serialization.Formatters.TypeFilterLevel.Full)
        Dim channel As IChannel = New IpcChannel(props, clientProvider, serverProvider)
        ChannelServices.RegisterChannel(channel, False)
        RemotingConfiguration.RegisterWellKnownServiceType(GetType(AKServer.DLL.DAoCServer.DAOCMain), "AKServerRemote", WellKnownObjectMode.Singleton)

        packets = New DAOCMain

        ' Creates the single instance of ServiceClass. All clients
        ' will use this instance.
        ref1 = RemotingServices.Marshal(packets, "AKServerRemote")

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
#End Region

#Region "   Registry Functions"
    'Private Sub CreateValues(ByVal Key As String, ByVal Value As String)
    '    Dim regKey As RegistryKey
    '    regKey = Registry.LocalMachine.OpenSubKey("Software\AutoKillerServer", True)
    '    regKey.SetValue(Key, Value)
    '    regKey.Close()
    'End Sub

    'Private Sub CreateRegKey()
    '    Dim regKey As RegistryKey
    '    regKey = Registry.LocalMachine.OpenSubKey("Software", True)
    '    regKey.CreateSubKey("AutoKillerServer")
    '    regKey.Close()
    'End Sub

    'Private Sub DeleteRegKey()
    '    Dim regKey As RegistryKey
    '    regKey = Registry.LocalMachine.OpenSubKey("Software", True)
    '    regKey.DeleteSubKey("AutoKillerServer", True)
    '    regKey.Close()
    'End Sub
#End Region

    Public Class General
        Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
        Public Shared ReadOnly Property Value(ByVal aSection As String, ByVal aKey As String, ByVal aDefault As Object, ByVal m_sPath As String) As String
            Get
                Dim sBuf As String = Space(255)
                Dim iSize As Integer = sBuf.Length
                Dim iRetCode As Integer = GetPrivateProfileString(aSection, aKey, CStr(aDefault), sBuf, iSize, m_sPath)

                If (iRetCode > 0) Then
                    Return sBuf.Substring(0, iRetCode)
                Else
                    Return String.Empty
                End If
            End Get
        End Property
    End Class
End Class
