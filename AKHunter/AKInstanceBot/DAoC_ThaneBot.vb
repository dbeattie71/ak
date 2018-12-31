Option Explicit On 

Imports System.Threading

Imports AutoKillerScript
Imports AKServer.DLL

Namespace AKInstanceBot

    Public Class DAoC_ThaneBot
        Inherits DAoC_BaseBot

        'misc vars
        Private _PlayerSwinging As Boolean = False
        Private _PullOk As Boolean = False

        ''''''''''''''''''''''''''''''''''''''''''
        ' class profile
        ''''''''''''''''''''''''''''''''''''''''''
        'vars that will be profiled when done
        Dim BuffStrConQ As String = "1"
        Dim BuffStrConKey As String = "9"
        Dim BuffDmgAddQ As String = "1"
        Dim BuffDmgAddKey As String = "0"

        Public Sub New(ByVal Ak As AutoKillerScript.clsAutoKillerScript)

            MyBase.New(Ak)
            '_Ak = Ak

            Timers.DefineCooldown("Buff", 3 * 1000)
            Timers.DefineCooldown("BuffStrCon", 18 * 60 * 1000)
            Timers.DefineCooldown("BuffDmgAdd", 18 * 60 * 1000)
            '_Timers.DefineCooldown("FightDelay", (1.5 * 1000))
            Timers.DefineCooldown("FightDelay", 1500)
            'LogLine("New() - " & _Timers.IsReady("FightDelay"))

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

            If Not Timers.IsReady("Buff") Or Ak.isPlayerCasting Then

                'recheck buffs
                If ActionQueue.Peek(0) <> BotAction.CheckBuffs Then
                    ActionQueue.Insert(0, BotAction.CheckBuffs)

                    Return
                End If

            End If

            'just in case
            Ak.StopRunning()

            'are we sitting?
            If Ak.isPlayerSitting Then

                AkKeys.SitStand(KeyDirection.KeyUpDown)
                Thread.Sleep(750)
                Timers.SetTime("Global")

                'recheck buffs
                If ActionQueue.Peek(0) <> BotAction.CheckBuffs Then
                    ActionQueue.Insert(0, BotAction.CheckBuffs)

                    Return

                End If

            End If

            If Timers.IsReady("BuffStrCon") Then

                LogLine("Casting StrCon buff.")
                Ak.SetTarget(Ak.PlayerID)
                Thread.Sleep(500)
                '
                'switch(quickbar And cast)
                UseQbar(BuffStrConQ, BuffStrConKey)

                Timers.SetTime("BuffStrCon")
                Timers.SetTime("Buff")

                'recheck buffs
                If (ActionQueue.Peek(0) <> BotAction.CheckBuffs) Then
                    ActionQueue.Insert(0, BotAction.CheckBuffs)

                    Return

                End If

            End If

            If Timers.IsReady("BuffDmgAdd") Then

                LogLine("Casting DmgAdd buff.")
                Ak.SetTarget(Ak.PlayerID)
                Thread.Sleep(500)
                '
                'switch(quickbar And cast)
                UseQbar(BuffDmgAddQ, BuffDmgAddKey)

                Timers.SetTime("BuffDmgAdd")
                Timers.SetTime("Buff")

                'recheck buffs
                If (ActionQueue.Peek(0) <> BotAction.CheckBuffs) Then
                    ActionQueue.Insert(0, BotAction.CheckBuffs)

                    Return

                End If

            End If

        End Sub

#End Region

#Region " DoMeleeFight "

        Public Overrides Sub DoMeleeFight()

            'check if mob was pulled
            If _PullOk = False Then

                If Ak.TargetIndex > 0 Then

                    IgnoreMob(Ak.TargetIndex)
                    Action = BotAction.FindTarget

                    Return

                End If

            End If


            Try

                '********
                If Ak.TargetIndex < 1 Or Ak.IsMobDead(Ak.TargetIndex) Then

                    LogLine("Target dead.")
                    Thread.Sleep(2000)

                    Action = BotAction.CheckAgro

                    ActionQueue.Clear()
                    ActionQueue.Enqueue(BotAction.Rest)
                    ActionQueue.Enqueue(BotAction.CheckAgro)

                    If Ak.isPlayerInCombat Then
                        AkKeys.AttackMode(KeyDirection.KeyUpDown)
                        Thread.Sleep(200)
                    End If

                    _PlayerSwinging = False
                    _PullOk = False

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

            'face target
            AkKeys.Face(KeyDirection.KeyUpDown)

            'pull
            Thread.Sleep(500)
            Ak.SendString("5")
            Thread.Sleep(3000)

            Action = BotAction.CheckAgro
            ActionQueue.Enqueue(BotAction.MeleeFight)

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
            LogLine(daocLogLine)

            If daocLogLine.IndexOf("You hit the") > -1 Then

                LogLine(daocLogLine)
                _PullOk = True

            End If

        End Sub

#End Region

    End Class

End Namespace



