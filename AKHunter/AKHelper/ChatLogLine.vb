Option Explicit On 

Friend Class ChatLogLine
    Inherits LogLine

    Private _Say As Boolean
    Private _Group As Boolean
    Private _Tell As Boolean
    Private _Chat As Boolean

    Public Property Say() As Boolean

        Get
            Return _Say
        End Get
        Set(ByVal Value As Boolean)
            _Say = Value
        End Set

    End Property

    Public Property Group() As Boolean

        Get
            Return _Group
        End Get
        Set(ByVal Value As Boolean)
            _Group = Value
        End Set

    End Property

    Public Property Tell() As Boolean

        Get
            Return _Tell
        End Get
        Set(ByVal Value As Boolean)
            _Tell = Value
        End Set

    End Property

    Public Property Chat() As Boolean

        Get
            Return _Chat
        End Get
        Set(ByVal Value As Boolean)
            _Chat = Value
        End Set

    End Property

End Class
