#Region "Imports"
Imports System.Net.Sockets
Imports System.Text
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Soap
#End Region
Namespace SocketServer
    Public NotInheritable Class UserConnection
        Public Event LineReceived(ByVal sender As UserConnection, ByVal Data As String)
        Public Event UserDisconnected(ByVal sender As UserConnection)
        Private Const READ_BUFFER_SIZE As Integer = 2048
        Private TheClient As TcpClient
        Private readBuffer(READ_BUFFER_SIZE) As Byte
        Private strName As String
        ' Overload the New operator to set up a read thread.
        Public Sub New(ByVal client As TcpClient)
            TheClient = client

            ' This starts the asynchronous read thread.  The data will be saved into
            ' readBuffer.
            TheClient.GetStream.BeginRead(readBuffer, 0, READ_BUFFER_SIZE, AddressOf StreamReceiver, Nothing)
        End Sub
        ' The Name property uniquely identifies the user connection.
        Public Property Name() As String
            Get
                Return strName
            End Get
            Set(ByVal Value As String)
                strName = Value
            End Set
        End Property
        Public Sub EndConnection()
            TheClient.Close()
        End Sub
        ' This subroutine uses a StreamWriter to send a message to the user.
        Public Function SendData(ByVal Data As String) As Boolean
            Try
                Dim networkStream As NetworkStream = TheClient.GetStream()

                If networkStream.CanWrite Then
                    Dim sendBytes As [Byte]() = Encoding.ASCII.GetBytes(Data)
                    networkStream.Write(sendBytes, 0, sendBytes.Length)
                    networkStream.Flush()
                Else
                    EndConnection()
                    Return False
                End If
                Return True
            Catch ex As Exception
                Return False
            End Try

        End Function
        ' This is the callback function for TcpClient.GetStream.Begin. It begins an 
        ' asynchronous read from a stream.
        Private Sub StreamReceiver(ByVal ar As IAsyncResult)
            Dim BytesRead As Integer
            Dim strMessage As String

            Try
                ' Ensure that no other threads try to use the stream at the same time.
                SyncLock TheClient.GetStream
                    ' Finish asynchronous read into readBuffer and get number of bytes read.
                    BytesRead = TheClient.GetStream.EndRead(ar)
                End SyncLock

                strMessage = Encoding.ASCII.GetString(readBuffer, 0, BytesRead)
                If BytesRead = 0 Then
                    TheClient.Close()
                    RaiseEvent UserDisconnected(Me)
                    Exit Sub
                Else
                    RaiseEvent LineReceived(Me, strMessage)
                End If

                ' Ensure that no other threads try to use the stream at the same time.
                SyncLock TheClient.GetStream
                    ' Start a new asynchronous read into readBuffer.
                    TheClient.GetStream.BeginRead(readBuffer, 0, READ_BUFFER_SIZE, AddressOf StreamReceiver, Nothing)
                End SyncLock
            Catch ex As Exception
                RaiseEvent UserDisconnected(Me)
                Debug.WriteLine(ex.Message)
            End Try
        End Sub
    End Class
    Public NotInheritable Class ListenerClass
#Region "Variables"
        Private listener As TcpListener
        Private listenerThread As Threading.Thread
        Private intLocalPort As Integer = 8000
        Public isServerStarted As Boolean = False
        Public Event LineReceived(ByVal Data As String)
        Private clients As New Hashtable
        Public Event CommandReceived(ByVal Data As String)
#End Region
        ' This subroutine sends a message to all attached clients
        Public Sub Broadcast(ByVal strMessage As String)
            Dim client As UserConnection

            For Each client In clients.Values
                If Not client.SendData(strMessage) Then
                    DisconnectUser(client)
                End If
            Next
        End Sub
        ' This subroutine checks to see if username already exists in the clients 
        ' Hashtable.  If it does, send a REFUSE message, otherwise confirm with a JOIN.
        Private Sub ConnectUser(ByVal userName As String, ByVal sender As UserConnection)
            sender.Name = userName
            Dim msg As New SocketServer.PullMessage
            msg.ID = SocketServer.OpCode.TextMsg
            msg.TextMessage.ID = TextMessageCode.Connected
            msg.TextMessage.Text = "CONNECTED"
            clients.Add(userName, sender)
            Dim s As String = msg.Serialize 'send via tcp
            ReplyToSender(s, sender)
        End Sub
        Private Sub DisconnectUser(ByVal sender As UserConnection)
            If Not sender.Name = Nothing Then
                clients.Remove(sender.Name)
            End If
        End Sub
        ' This subroutine is used as a background listener thread to allow reading incoming
        ' messages without lagging the user interface.
        Private Sub DoListen()
            Try
                ' Listen for new connections.
                listener = New TcpListener(System.Net.IPAddress.Any, intLocalPort)
                listener.Start()
                isServerStarted = True
                Do
                    ' Create a new user connection using TcpClient returned by
                    ' TcpListener.AcceptTcpClient()
                    Dim client As New UserConnection(listener.AcceptTcpClient)

                    ' Create an event handler to allow the UserConnection to communicate
                    ' with the window.
                    AddHandler client.LineReceived, AddressOf OnLineReceived
                    AddHandler client.UserDisconnected, AddressOf DisconnectUser
                Loop Until False
            Catch
            End Try
        End Sub
        Public Sub Listen(ByVal LocalPort As Integer)
            intLocalPort = LocalPort
            listenerThread = New Threading.Thread(AddressOf DoListen)
            listenerThread.Start()
        End Sub
        Public Sub Listen()
            intLocalPort = 8000
            listenerThread = New Threading.Thread(AddressOf DoListen)
            listenerThread.Start()
        End Sub
        Public Sub Close()
            listener.Stop()
            isServerStarted = False
            listenerThread.Abort()
        End Sub
        Private Sub OnLineReceived(ByVal sender As UserConnection, ByVal data As String)
            Dim msg As New SocketServer.PullMessage

            Try
                msg = msg.Deserialize(data)
            Catch ex As Exception
                Debug.WriteLine(ex.Message)
            End Try

            Select Case msg.ID
                Case SocketServer.OpCode.Connect
                    ConnectUser(msg.Name, sender) 'connect user
                    RaiseEvent CommandReceived(data)
                Case Else
                    RaiseEvent CommandReceived(data)
            End Select
        End Sub
        ' This subroutine sends a response to the sender.
        Private Sub ReplyToSender(ByVal strMessage As String, ByVal sender As UserConnection)
            sender.SendData(strMessage)
        End Sub
        Public Function IsConnected() As Boolean
            If clients.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
    End Class
    Public NotInheritable Class SocketClient
#Region "Variables"
        Private IsConnected As Boolean
        Public Event DataReceived(ByVal Data As String)
        Public Event onError(ByVal errorNumber As Integer, ByVal errorString As String)
        Private tcpClient As New System.Net.Sockets.TcpClient
        Private Const READ_BUFFER_SIZE As Integer = 2048
        Private readBuffer(READ_BUFFER_SIZE) As Byte
#End Region
        Public ReadOnly Property Connected() As Boolean
            Get
                Return IsConnected
            End Get
        End Property
        Public Function Connect(ByVal IP As String, ByVal Port As Integer) As Boolean
            Try
                tcpClient.Connect(IP, Port)
                ' This starts the asynchronous read thread.  The data will be saved into readBuffer.
                tcpClient.GetStream.BeginRead(readBuffer, 0, READ_BUFFER_SIZE, AddressOf StreamReceiver, Nothing)
            Catch e As Exception
                IsConnected = False
                RaiseEvent onError(Err.Number, e.Message)
                Return False
            End Try
            IsConnected = True
            Return True
        End Function
        Public Sub send(ByVal Data As String)
            Dim networkStream As NetworkStream = tcpClient.GetStream()
            Dim sendBytes As [Byte]() = Encoding.ASCII.GetBytes(Data)
            networkStream.Write(sendBytes, 0, sendBytes.Length)
        End Sub
        ' This is the callback function for TcpClient.GetStream.Begin. It begins an 
        ' asynchronous read from a stream.
        Private Sub StreamReceiver(ByVal ar As IAsyncResult)
            Dim BytesRead As Integer
            Dim strMessage As String

            Try
                ' Ensure that no other threads try to use the stream at the same time.
                SyncLock tcpClient.GetStream
                    ' Finish asynchronous read into readBuffer and get number of bytes read.
                    BytesRead = tcpClient.GetStream.EndRead(ar)
                End SyncLock

                strMessage = Encoding.ASCII.GetString(readBuffer, 0, BytesRead)
                If BytesRead = 0 Then
                    Disconnect()
                    Exit Sub
                Else
                    RaiseEvent DataReceived(strMessage)
                End If

                ' Ensure that no other threads try to use the stream at the same time.
                SyncLock tcpClient.GetStream
                    ' Start a new asynchronous read into readBuffer.
                    tcpClient.GetStream.BeginRead(readBuffer, 0, READ_BUFFER_SIZE, AddressOf StreamReceiver, Nothing)
                End SyncLock
            Catch ex As Exception
                Disconnect()
                RaiseEvent onError(Err.Number, ex.Message)
            End Try
        End Sub
        Public Sub Disconnect()
            tcpClient.Close()
            IsConnected = False
        End Sub
    End Class
#Region "Protocol"
    Public Enum OpCode
        Connect = &H1
        TextMsg = &H2
        Aggro = &H3
        Moving = &H4
        Target = &H5
    End Enum
    Public Enum TextMessageCode
        Say = &H1
        Send = &H2
        Quit = &H3
        Start = &H4
        Connected = &H5
    End Enum
    Interface IMessage
        Property ID() As OpCode
    End Interface
    <Serializable()> Public NotInheritable Class StringMessage
        Private mID As TextMessageCode
        Private mText As String
        Public Property ID() As TextMessageCode
            Get
                Return mID
            End Get
            Set(ByVal Value As TextMessageCode)
                mID = Value
            End Set
        End Property
        Public Property Text() As String
            Get
                Return mText
            End Get
            Set(ByVal Value As String)
                mText = Value
            End Set
        End Property
    End Class
    <Serializable()> Public NotInheritable Class PullMessage
        Implements IMessage
#Region "Variables"
        Private mID As OpCode
        Private mSpawnID As Short
        Private mXPos As Integer
        Private mYPos As Integer
        Private mZPos As Integer
        Private mDestXPos As Integer
        Private mDestYPos As Integer
        Private mDestZPos As Integer
        Private mName As String
        Private mTextMessage As New StringMessage
#End Region
        Public Property ID() As OpCode Implements IMessage.ID
            Get
                Return mID
            End Get
            Set(ByVal Value As OpCode)
                mID = Value
            End Set
        End Property
        Public Property TextMessage() As StringMessage
            Get
                Return mTextMessage
            End Get
            Set(ByVal Value As StringMessage)
                mTextMessage = Value
            End Set
        End Property
        Public Property Name() As String
            Get
                Return mName
            End Get
            Set(ByVal Value As String)
                mName = Value
            End Set
        End Property
        Public Property SpawnID() As Short
            Get
                Return mSpawnID
            End Get
            Set(ByVal Value As Short)
                mSpawnID = Value
            End Set
        End Property
        Public Property XPos() As Integer
            Get
                Return mXPos
            End Get
            Set(ByVal Value As Integer)
                mXPos = Value
            End Set
        End Property
        Public Property YPos() As Integer
            Get
                Return mYPos
            End Get
            Set(ByVal Value As Integer)
                mYPos = Value
            End Set
        End Property
        Public Property ZPos() As Integer
            Get
                Return mZPos
            End Get
            Set(ByVal Value As Integer)
                mZPos = Value
            End Set
        End Property
        Public Property DestXPos() As Integer
            Get
                Return mDestXPos
            End Get
            Set(ByVal Value As Integer)
                mDestXPos = Value
            End Set
        End Property
        Public Property DestYPos() As Integer
            Get
                Return mDestYPos
            End Get
            Set(ByVal Value As Integer)
                mDestYPos = Value
            End Set
        End Property
        Public Property DestZPos() As Integer
            Get
                Return mDestZPos
            End Get
            Set(ByVal Value As Integer)
                mDestZPos = Value
            End Set
        End Property
        Public Function Serialize() As String
            ' return serialized xml string of this class to send through tcp
            Dim memory As New MemoryStream
            Dim fm As New SoapFormatter
            fm.AssemblyFormat = Formatters.FormatterAssemblyStyle.Simple
            fm.Serialize(memory, Me)
            memory.Position = 0
            Return Encoding.ASCII.GetString(memory.ToArray())
        End Function
        Public Function Deserialize(ByVal Message As String) As PullMessage
            'return PullMessage object from deserialization (Message is string sent through tcp)
            Dim memory As New MemoryStream
            Dim fm As New SoapFormatter
            fm.AssemblyFormat = Formatters.FormatterAssemblyStyle.Simple
            memory.Write(Encoding.ASCII.GetBytes(Message), 0, Message.Length)
            memory.Position = 0
            Return DirectCast(fm.Deserialize(memory), PullMessage)
        End Function
    End Class
#End Region
End Namespace
