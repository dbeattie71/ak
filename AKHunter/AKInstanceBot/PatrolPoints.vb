Option Explicit On 

Imports System.Threading

Imports AutoKillerScript
Imports AKServer.DLL

Namespace AKInstanceBot

    Public Enum AttackType
        None
        Ranged
        Melee
        WaitForAgro
    End Enum

    Public Class PatrolPoints

        Private _Ak As AutoKillerScript.clsAutoKillerScript
        Private _PatrolPoints As ArrayList
        Private _PatrolPointIterator As Integer = 0
        Private _CurrentPatrolPoint As patrolPoint

        Private temp As String

        Public Sub New(ByVal Ak As AutoKillerScript.clsAutoKillerScript)

            _Ak = Ak
            _PatrolPoints = New ArrayList

        End Sub

        Public Function MoveToNextPatrolPoint(ByRef FindTargets As Boolean, _
                                              ByRef AttackType As AttackType, _
                                              ByRef CheckForAgro As Boolean) As Boolean

            If _PatrolPoints.Count = 0 Then

                Return False

            End If

            If _PatrolPointIterator = _PatrolPoints.Count Then

                Return False

            End If


            Dim tempPatrolPoint As patrolPoint

            'move to patrolpoint
            Thread.Sleep(100)
            tempPatrolPoint = _PatrolPoints(_PatrolPointIterator)

            FindTargets = tempPatrolPoint.FindTargets
            AttackType = tempPatrolPoint.AttackType
            CheckForAgro = tempPatrolPoint.CheckForAgro

            tempPatrolPoint.MoveTo()
            Thread.Sleep(100)

            _PatrolPointIterator = _PatrolPointIterator + 1

            Return True

        End Function

        Public Sub AddPatrolPoint(ByVal Ak As AutoKillerScript.clsAutoKillerScript, _
                                  ByVal FindTargets As Boolean, _
                                  ByVal AttackType As AttackType, _
                                  ByVal CheckForAgro As Boolean, _
                                  ByVal X As Double, _
                                  ByVal Y As Double, _
                                  ByVal Z As Double)

            Dim patrolPoint As New patrolPoint(Ak, FindTargets, AttackType, CheckForAgro, X, Y, Z)

            _PatrolPoints.Add(patrolPoint)

        End Sub

        'Private Function FormatCoords(ByVal Type As String, _
        '                     ByVal X As Double, _
        '                     ByVal Y As Double, _
        '                     ByVal Z As Double)

        '    Return Type & " - x:" & X & "  y:" & Y & "  z:" & Z

        'End Function

    End Class

#Region " patrolPoint class "

    Public Class patrolPoint

        Private _Ak As AutoKillerScript.clsAutoKillerScript
        Private _FindTargets As Boolean
        Private _AttackType As AttackType
        Private _CheckForAgro As Boolean
        Private _X As Double
        Private _Y As Double
        Private _Z As Double

        Public Sub New(ByVal Ak As AutoKillerScript.clsAutoKillerScript, _
                       ByVal FindTargets As Boolean, _
                       ByVal AttackType As AttackType, _
                       ByVal CheckForAgro As Boolean, _
                       ByVal X As Double, _
                       ByVal Y As Double, _
                       ByVal Z As Double)

            _Ak = Ak
            _FindTargets = FindTargets
            _AttackType = AttackType
            _CheckForAgro = CheckForAgro
            _X = X
            _Y = Y
            _Z = Z

        End Sub

        Public Sub MoveTo()

            'needs stuck logic

            If Not (_Ak.ZDistance(_Ak.zPlayerXCoord, _Ak.zPlayerYCoord, _Ak.zPlayerZCoord, _X, _Y, _Z)) < 100 Then
                _Ak.StartRunning()

                While _Ak.ZDistance(_Ak.zPlayerXCoord, _Ak.zPlayerYCoord, _Ak.zPlayerZCoord, _X, _Y, _Z) > 100 _
                    And Not _Ak.IsPlayerDead()

                    _Ak.TurnToHeading(_Ak.FindHeading(_Ak.zPlayerXCoord, _Ak.zPlayerYCoord, _X, _Y))
                    Thread.Sleep(100)
                End While

                _Ak.StopRunning()
            End If

        End Sub

        Public ReadOnly Property FindTargets() As Boolean

            Get
                Return _FindTargets
            End Get

        End Property

        Public ReadOnly Property AttackType() As AttackType

            Get
                Return _AttackType
            End Get

        End Property

        Public ReadOnly Property CheckForAgro() As Boolean

            Get
                Return _CheckForAgro
            End Get

        End Property


    End Class

#End Region

End Namespace


