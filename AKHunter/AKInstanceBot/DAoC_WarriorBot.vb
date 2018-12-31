Option Explicit On 

Imports System.Threading

Imports AutoKillerScript
Imports AKServer.DLL

Namespace AKInstanceBot

    Public Class DAoC_WarriorBot
        Inherits DAoC_BaseBot

        'misc vars
        Private _PlayerSwinging As Boolean = False
        Private _PlayerThrowing As Boolean = False

        Private _PullAttempted As Boolean = False
        Private _PullOk As Boolean = False

        Public blah As String

        ''''''''''''''''''''''''''''''''''''''''''
        ' class profile
        ''''''''''''''''''''''''''''''''''''''''''
        'vars that will be profiled when done

        Public Sub New(ByVal Ak As AutoKillerScript.clsAutoKillerScript)

            MyBase.New(Ak)
            '_Ak = Ak

            Timers.DefineCooldown("ThrowDelay", 6000)
            Timers.DefineCooldown("FightDelay", 1500)

        End Sub

        Public Overrides Sub DoInitialize()

            LogLine("Our thread priority is " + Thread.CurrentThread.Priority.ToString)

            Interaction.Appactivate(Ak.GameProcess)
            LogLine("Starting bot.")
            LogLine("Sleep for 2 seconds.")
            Thread.Sleep(2000)

            '_Action = BotAction.ZoneIntoTD
            Action = BotAction.CheckAgro
            '_ActionQueue.Enqueue(BotAction.CheckAgro)
            ActionQueue.Enqueue(BotAction.CheckBuffs)
            ActionQueue.Enqueue(BotAction.MoveToNextPatrolPoint)
            ActionQueue.Enqueue(BotAction.Rest)
            ActionQueue.Enqueue(BotAction.FindTarget)

        End Sub

#Region " DoCheckBuffs "

        Public Overrides Sub DoCheckBuffs()

            'setup default action
            Action = BotAction.CheckAgro

        End Sub

#End Region

#Region " DoMeleeFight "

        Public Overrides Sub DoMeleeFight()

            Try

                '********
                If Ak.TargetIndex < 1 Or Ak.IsMobDead(Ak.TargetIndex) Then

                    LogLine("Target dead.")
                    Thread.Sleep(1000)

                    Action = BotAction.CheckAgro

                    ActionQueue.Clear()
                    ActionQueue.Enqueue(BotAction.Rest)
                    ActionQueue.Enqueue(BotAction.CheckAgro)

                    If Ak.isPlayerInCombat Then
                        AkKeys.AttackMode(KeyDirection.KeyUpDown)
                        Thread.Sleep(200)
                    End If

                    _PlayerSwinging = False

                    Thread.Sleep(500)

                    Return

                End If

            Catch ex As Exception

                Thread.Sleep(1000)
                Action = BotAction.FindTarget
                Thread.Sleep(1000)

                Return
            End Try

            'just in case
            Ak.StopRunning()

            Action = BotAction.MeleeFight
            ActionQueue.Clear()

            If _PlayerSwinging And Not Timers.IsReady("FightDelay") Then
                Return
            End If

            If Ak.TargetIndex > 0 Then
                LogLine("Close Range Style ")

                UseQbar("1", "3")

                Timers.SetTime("FightDelay")
                _PlayerSwinging = True
            End If

        End Sub

#End Region

#Region " DoRangedFight "

        Public Overrides Sub DoRangedFight()

            'just in case
            Ak.StopRunning()

            Action = BotAction.Protect
            ActionQueue.Clear()
            ActionQueue.Enqueue(BotAction.RangedFight)

            'if target close enough to switch to melee fight, do it
            'done before cooldown check since we can cast melee range spells immediately

            '**** variableize 300
            'If _Ak.TargetIndex > 0 And DistanceToMob(_Ak.TargetIndex) < 300 Then

            '    'make sure wep is ready
            '    '**** variableize wep key
            '    Thread.Sleep(500)
            '    _Ak.SendString("2")

            '    _Action = BotAction.MeleeFight

            '    'face target
            '    _Keys.Face(KeyDirection.KeyUpDown)

            '    Return

            'End If

            'nothing to do if cooldowns arent ready
            If Not Timers.IsReady("ThrowDelay") Then
                Return
            End If

            'if a pull was attempted, melee or ignore and find a new target
            If _PullAttempted Then
                _PullAttempted = False

                If _PullOk Then

                    'ready wep
                    Ak.SendString("2")
                    Thread.Sleep(500)

                    Action = BotAction.MeleeFight

                    Return
                Else
                    'ready wep
                    Ak.SendString("2")
                    Thread.Sleep(500)

                    IgnoreMob(Ak.TargetIndex)
                    Action = BotAction.FindTarget

                    Return
                End If

            Else

                'face target
                AkKeys.Face(KeyDirection.KeyUpDown)

                'pull
                Ak.SendString("1")
                Thread.Sleep(500)

                Ak.SendString("1")
                Thread.Sleep(500)

                Ak.SendString("1")
                Timers.SetTime("ThrowDelay")
                _PullAttempted = True
                _PullOk = False

            End If

        End Sub

#End Region

#Region " DoProtect "

        Public Overrides Sub DoProtect()

            'next Action
            Action = ActionQueue.Dequeue()

        End Sub

#End Region

#Region " ParseLog "

        Public Overrides Sub ProcessDaocLog(ByVal e As AutoKillerScript.clsAutoKillerScript.AutokillerRegExEventParams)

            Dim daocLogLine As String
            daocLogLine = e.Logline
            'LogLine(daocLogLine)

            If _PullAttempted Then

                If daocLogLine.IndexOf("You shoot the") > -1 Then

                    LogLine(daocLogLine)
                    _PullOk = True

                End If

                If daocLogLine.IndexOf("You miss") > -1 Then

                    LogLine(daocLogLine)
                    _PullOk = True

                End If

            End If

            

        End Sub

#End Region


    End Class

End Namespace


