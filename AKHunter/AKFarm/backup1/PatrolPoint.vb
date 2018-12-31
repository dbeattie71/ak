Public Class patrolPoint

    Private _Number As Integer
    Private _WayPointsNeeded As Boolean
    Private _WayPoints As WayPoints
    Private _X As Double
    Private _Y As Double
    Private _Z As Double

    Public Property Number() As Integer

        Get
            Return _Number
        End Get
        Set(ByVal Value As Integer)
            _number = Value
        End Set

    End Property

    Public Property WayPointsNeeded() As Boolean

        Get
            Return _WayPointsNeeded
        End Get
        Set(ByVal Value As Boolean)
            _WayPointsNeeded = Value
        End Set

    End Property

    Public Property WayPoints() As WayPoints

        Get
            Return _WayPoints
        End Get
        Set(ByVal Value As WayPoints)
            _WayPoints = Value
        End Set

    End Property

    Public Property X() As Double

        Get
            Return _X
        End Get
        Set(ByVal Value As Double)
            _X = Value
        End Set

    End Property

    Public Property Y() As Double

        Get
            Return _Y
        End Get
        Set(ByVal Value As Double)
            _Y = Value
        End Set

    End Property

    Public Property Z() As Double

        Get
            Return _Z
        End Get
        Set(ByVal Value As Double)
            _Z = Value
        End Set

    End Property

End Class
