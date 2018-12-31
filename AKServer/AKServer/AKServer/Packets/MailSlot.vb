#Region "Imports"
Imports System.Threading
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Runtime.Remoting.Messaging
#End Region
Namespace DAoCServer
#Region "MailSlot"
    Public NotInheritable Class MailSlot

#Region "Structs/Variables"
        Private Structure SECURITY_ATTRIBUTES
            Private nLength As Int32
            Private lpSecurityDescriptor As Int32
            Private bInheritHandle As Int32
        End Structure
        ' This is the OverLapped structure used by the calls to the Windows API.
        <StructLayout(LayoutKind.Sequential, Pack:=1)> Private Structure OVERLAPPED
            Public Internal As Integer
            Public InternalHigh As Integer
            Public Offset As Integer
            Public OffsetHigh As Integer
            Public hEvent As Integer
        End Structure
        Private Const MAILSLOT_NO_MESSAGE As Int32 = -1
        Private sMailSlot As String = "\\.\mailslot\SERVERPACKET"
        Private cMailSlot As String = "\\.\mailslot\CLIENTPACKET"
        Private m_hInputslotServer As IntPtr
        Private m_hInputslotClient As IntPtr
        Private ServerLoopThread As Threading.Thread
        Private ClientLoopThread As Threading.Thread
        Private mRunning As Boolean
        Private Declare Function ReadFile Lib "kernel32.dll" (ByVal hFile As IntPtr, ByVal Buffer As Byte(), ByVal nNumberOfBytesToRead As Integer, ByRef lpNumberOfBytesRead As Integer, ByRef lpOverlapped As OVERLAPPED) As Integer
        Private Declare Function CreateMailslot Lib "kernel32.dll" Alias "CreateMailslotA" (ByVal lpName As String, ByVal nMaxMessageSize As Integer, ByVal lReadTimeout As Integer, ByRef lpSecurityAttributes As SECURITY_ATTRIBUTES) As IntPtr
        Private Declare Function GetMailslotInfo Lib "kernel32.dll" (ByVal hMailslot As IntPtr, ByRef lpMaxMessageSize As Integer, ByRef lpNextSize As Integer, ByRef lpMessageCount As Integer, ByRef lpReadTimeout As Integer) As Boolean
        Private Declare Function CloseHandle Lib "kernel32.dll" (ByVal hObject As IntPtr) As Integer
        Private Delegate Sub OnSend(ByVal aPacket As DAOCPacket)
        Private Delegate Sub OnReceive(ByVal aPacket As DAOCPacket)
#End Region

#Region "Events"
        Public Event OnServerPacketReceived(ByVal Packet As DAOCPacket)
        Public Event OnClientPacketReceived(ByVal Packet As DAOCPacket)
#End Region

#Region "Constructor"
        Public Sub New()
            mRunning = True

            'no writing will take place in the hook unless these are created
            m_hInputslotServer = CreateMailslot(sMailSlot, 0, -1, Nothing)
            ServerLoopThread = New Threading.Thread(AddressOf ProcessServerMail)
            ServerLoopThread.Name = "ServerLoopThread"
            ServerLoopThread.Start()

            m_hInputslotClient = CreateMailslot(cMailSlot, 0, -1, Nothing)
            ClientLoopThread = New Threading.Thread(AddressOf ProcessClientMail)
            ClientLoopThread.Name = "ClientLoopThread"
            ClientLoopThread.Start()

        End Sub
#End Region

#Region "Mail Methods"
        Private Sub ProcessClientMail()
            Dim Message As Integer
            Dim noofMessages As Integer
            Dim dataRead As Integer
            Dim pBuffer() As Byte

            While mRunning
                Dim result As Boolean = GetMailslotInfo(m_hInputslotClient, Nothing, Message, noofMessages, Nothing)

                If Message = MAILSLOT_NO_MESSAGE Then
                    'Debug.WriteLine("No messages waiting")
                Else
                    While Not noofMessages = 0
                        'Handle to the file to be read
                        'Pointer to the buffer that receives the data read from the file
                        'Number of bytes to be read from the file
                        'Pointer to the variable that receives the number of bytes read
                        'Pointer to an OVERLAPPED structure
                        ReDim pBuffer(Message - 1)
                        ReadFile(m_hInputslotClient, pBuffer, Message, dataRead, Nothing)
                        Dim APacket As DAOCPacket = New DAOCPacket
                        APacket.Size = Message
                        APacket.PacketData = pBuffer
                        APacket.Position = 0

                        Dim Invoking As New OnSend(AddressOf ProcessSend)
                        Invoking.BeginInvoke(APacket, Nothing, Nothing)

                        APacket = Nothing

                        result = GetMailslotInfo(m_hInputslotClient, Nothing, Message, noofMessages, Nothing)
                    End While
                End If
                Thread.Sleep(10)
            End While

        End Sub

        Private Sub ProcessServerMail()
            Dim Message As Integer
            Dim noofMessages As Integer
            Dim dataRead As Integer
            Dim pBuffer() As Byte

            While mRunning
                Dim result As Boolean = GetMailslotInfo(m_hInputslotServer, Nothing, Message, noofMessages, Nothing)

                If Message = MAILSLOT_NO_MESSAGE Then
                    'Debug.WriteLine("No messages waiting")
                Else
                    While Not noofMessages = 0
                        'Handle to the file to be read
                        'Pointer to the buffer that receives the data read from the file
                        'Number of bytes to be read from the file
                        'Pointer to the variable that receives the number of bytes read
                        'Pointer to an OVERLAPPED structure
                        ReDim pBuffer(Message - 1)
                        ReadFile(m_hInputslotServer, pBuffer, Message, dataRead, Nothing)

                        Dim APacket As DAOCPacket = New DAOCPacket
                        APacket.Size = Message
                        APacket.PacketData = pBuffer
                        APacket.Position = 0

                        Dim Invoking As New OnReceive(AddressOf ProcessReceive)
                        Invoking.BeginInvoke(APacket, Nothing, Nothing)
                        APacket = Nothing

                        result = GetMailslotInfo(m_hInputslotServer, Nothing, Message, noofMessages, Nothing)
                    End While
                End If
                Thread.Sleep(10)
            End While
        End Sub

        Private Sub ProcessSend(ByVal aPacket As DAOCPacket)
            RaiseEvent OnClientPacketReceived(aPacket)
        End Sub

        Private Sub ProcessReceive(ByVal aPacket As DAOCPacket)
            RaiseEvent OnServerPacketReceived(aPacket)
        End Sub

        Public Sub CleanUp()
            mRunning = False
            CloseHandle(m_hInputslotServer) 'close handle or we won't be able to reopen it until program is closed
            m_hInputslotServer = Nothing
            CloseHandle(m_hInputslotClient)
            m_hInputslotClient = Nothing
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
#End Region

#Region "Properties"
        Public Property Running() As Boolean
            Get
                Return mRunning
            End Get
            Set(ByVal Value As Boolean)
                mRunning = Value
            End Set
        End Property

#End Region

    End Class
#End Region
End Namespace
