Option Explicit On 

Public Class WayPoints

    Private _WayPoints As Hashtable

    Public Sub New()

        _WayPoints = New Hashtable

    End Sub

    Public Sub AddWayPoint(ByVal Number As Integer, _
                           ByVal X As Double, _
                           ByVal Y As Double, _
                           ByVal Z As Double)

        Dim wayPoint As New WayPoint

        wayPoint.Number = Number
        wayPoint.X = X
        wayPoint.Y = Y
        wayPoint.Z = Z

        _WayPoints.Add(Number, wayPoint)


    End Sub

    Public Function GetWayPoint(ByVal Number As Integer) As WayPoint

        Return _WayPoints(Number)

    End Function

    Public Function GetNumberOfWayPoints() As Integer

        Return _WayPoints.Count

    End Function

End Class
