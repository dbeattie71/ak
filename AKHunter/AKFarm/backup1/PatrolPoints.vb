Option Explicit On 

Public Class PatrolPoints

    Private _PatrolPoints As Hashtable

    Public Sub New()

        _PatrolPoints = New Hashtable

    End Sub

    Public Sub AddPatrolPoint(ByVal Number As Integer, _
                              ByVal WayPointsNeeded As Boolean, _
                              ByVal X As Double, _
                              ByVal Y As Double, _
                              ByVal Z As Double, _
                              Optional ByVal WayPoints As WayPoints = Nothing)

        Dim patrolPoint As New patrolPoint

        patrolPoint.Number = Number
        patrolPoint.WayPointsNeeded = WayPointsNeeded
        patrolPoint.WayPoints = WayPoints
        patrolPoint.X = X
        patrolPoint.Y = Y
        patrolPoint.Z = Z

        _PatrolPoints.Add(Number, patrolPoint)

    End Sub

    Public Function GetPatrolPoint(ByVal Number As Integer)

        Return _PatrolPoints(Number)

    End Function

    Public Function GetNumberOfPatrolPoints() As Integer

        Return _PatrolPoints.Count

    End Function

End Class
