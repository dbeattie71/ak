Public Class SpellTimer
    Inherits System.Timers.Timer

    Private _Name As String
    Private _SpellList As String
    Private _Process As Boolean

    Public Property Name() As String

        Get
            Return _Name
        End Get
        Set(ByVal Value As String)
            _Name = Value
        End Set

    End Property

    Public Property SpellList() As String

        Get
            Return _SpellList
        End Get
        Set(ByVal Value As String)
            _SpellList = Value
        End Set

    End Property

    Public Property Process() As Boolean

        Get
            Return _Process
        End Get
        Set(ByVal Value As Boolean)
            _Process = Value
        End Set

    End Property
End Class
