Option Explicit On 

Imports System.Threading

Imports AutoKillerScript
Imports AKServer.DLL

Namespace AKInstanceBot

    Public Enum BotAction
        Initialize
        ZoneIntoTD
        MoveToNextPatrolPoint
        Protect
        FindTarget
        CheckAgro
        RangedFight
        MeleeFight
        Rest
        CheckBuffs
        Pause
        Quit
        NOP
    End Enum

    Public Class DAoC_BaseBot

        Private _Ak As AutoKillerScript.clsAutoKillerScript

        'public _Profile as 
        Private _PatrolPoints As PatrolPoints
        Private _Timers As Timers
        Private _Interaction As Interaction

        Private _Action As BotAction
        Private _CurrentAction As BotAction
        Private _PreviousAction As BotAction
        Private _ActionQueue As ActionQueue

        Private _Paused As Boolean = False

        Private _AkKeys As AutoKillerScript.UserKeys
        'Public _Keys2 As DAOCKeyboard

        Public Delegate Sub Log(ByVal Line As String)
        Private _LogPtr As Log

        'misc vars
        Private _FindDoorIndexCount As Integer = 0
        Private _ZoneAttemptCount As Integer = 0

        Private _Resting As Boolean = False

        Private _IgnoreTargetObjects As ArrayList

        Private _AttackType As AttackType
        Private _FindTargets As Boolean

        ''''''''''''''''''''''''''''''''''''''''''
        ' class profile - base
        ''''''''''''''''''''''''''''''''''''''''''
        'vars that will be profiled when done
        Dim RestBelowHealth As Integer = 90
        Dim RestBelowMana As Integer = 0
        Dim RestBelowEndurance As Integer = 90
        Dim RestUntilFullyRecovered As Boolean = True
        Dim AutomaticLevelSelect As Boolean = True
        Dim FightGreens As Boolean = True
        Dim FightBlues As Boolean = True
        Dim FightYellows As Boolean = True
        Dim FightOranges As Boolean = True
        Dim SearchDistance As Long = 900

        ''''''''''''''''''''''''''''''''''''''''''
        ' instance profile
        ''''''''''''''''''''''''''''''''''''''''''
        'vars that will be profiled when done
        Dim ZoneName As String = "Varulvhamn"
        Dim TDZoneName As String = "The_Cursed_Lair"
        Dim PreZoneXCoord As Double = 163989
        Dim PreZoneYCoord As Double = 30674
        Dim PreZoneZCoord As Double = 14730
        Dim PreZoneFaceLocX As Double = 24700
        Dim PreZoneFaceLocY As Double = 22620

#Region " Properties "

        Public Property Ak() As AutoKillerScript.clsAutoKillerScript
            Get
                Return _Ak
            End Get
            Set(ByVal value As AutoKillerScript.clsAutoKillerScript)
                _Ak = value
            End Set
        End Property

        Public Property AkKeys() As AutoKillerScript.UserKeys
            Get
                Return _AkKeys
            End Get
            Set(ByVal value As AutoKillerScript.UserKeys)
                _AkKeys = value
            End Set
        End Property

        Public Property Timers() As Timers
            Get
                Return _Timers
            End Get
            Set(ByVal value As Timers)
                _Timers = value
            End Set
        End Property

        Public Property Interaction() As Interaction
            Get
                Return _Interaction
            End Get
            Set(ByVal value As Interaction)
                _Interaction = value
            End Set
        End Property

        Public Property Action() As BotAction
            Get
                Return _Action
            End Get
            Set(ByVal value As BotAction)
                _Action = value
            End Set
        End Property

        Public Property ActionQueue() As ActionQueue
            Get
                Return _ActionQueue
            End Get
            Set(ByVal value As ActionQueue)
                _ActionQueue = value
            End Set
        End Property

        Public Property Paused() As Boolean
            Get
                Return _Paused
            End Get
            Set(ByVal value As Boolean)
                _Paused = value
            End Set
        End Property

#End Region

        Public Sub New(ByVal Ak As AutoKillerScript.clsAutoKillerScript)

            _Ak = Ak
            AddHandler _Ak.OnRegExTrue, AddressOf ProcessDaocLog

            _AkKeys = New AutoKillerScript.UserKeys(_Ak)
            '_Keys2 = New DAOCKeyboard(_Ak)

            'set movement keys
            _Ak.SetLeftTurnKey = _AkKeys.TurnLeftKey
            _Ak.SetRightTurnKey = _AkKeys.TurnRightKey
            _Ak.SetConsiderKey = _AkKeys.ConsiderKey

            'this.profile = profile;
            _PatrolPoints = New PatrolPoints(_Ak)

            'Ak, FindTargets, AttackType, CheckForAgro, X, Y, Z
            _PatrolPoints.AddPatrolPoint(_Ak, False, AttackType.WaitForAgro, False, 25875, 24516, 16001)
            _PatrolPoints.AddPatrolPoint(_Ak, False, AttackType.WaitForAgro, False, 26216, 24818, 16013)
            _PatrolPoints.AddPatrolPoint(_Ak, False, AttackType.None, False, 25687, 24383, 16002)
            _PatrolPoints.AddPatrolPoint(_Ak, False, AttackType.WaitForAgro, False, 25251, 24550, 16000)
            _PatrolPoints.AddPatrolPoint(_Ak, False, AttackType.None, False, 24812, 24540, 16013)
            _PatrolPoints.AddPatrolPoint(_Ak, False, AttackType.WaitForAgro, False, 24923, 25118, 16008)
            _PatrolPoints.AddPatrolPoint(_Ak, True, AttackType.Ranged, True, 24923, 25118, 16008)
            _PatrolPoints.AddPatrolPoint(_Ak, False, AttackType.WaitForAgro, False, 24884, 23820, 16014)
            _PatrolPoints.AddPatrolPoint(_Ak, False, AttackType.WaitForAgro, False, 24302, 23777, 16005)
            _PatrolPoints.AddPatrolPoint(_Ak, False, AttackType.WaitForAgro, False, 23972, 23888, 16006)
            _PatrolPoints.AddPatrolPoint(_Ak, False, AttackType.WaitForAgro, False, 23814, 23723, 16007)
            _PatrolPoints.AddPatrolPoint(_Ak, True, AttackType.Ranged, True, 23749, 22943, 16268)
            _PatrolPoints.AddPatrolPoint(_Ak, True, AttackType.Ranged, True, 23728, 23835, 16009)


            '_PatrolPoints._PatrolPointIterator = 33

            _Action = BotAction.Initialize
            _ActionQueue = New ActionQueue

            _Timers = New Timers
            _Timers.DefineCooldown("Global", 500)

            _Interaction = New Interaction
            _IgnoreTargetObjects = New ArrayList

        End Sub

        Public Sub DoAction()

            _PreviousAction = _CurrentAction
            _CurrentAction = _Action

            Try

                LogLine("Current Action: " & _Action.ToString)

                Select Case (_Action)

                    Case BotAction.Initialize
                        DoInitialize()

                    Case BotAction.ZoneIntoTD
                        DoZoneIntoTD()

                    Case BotAction.MoveToNextPatrolPoint
                        DoMoveToNextPatrolPoint()

                    Case BotAction.Protect
                        DoProtect()

                    Case BotAction.FindTarget
                        DoFindTarget()

                    Case BotAction.CheckAgro
                        DoCheckAgro()

                    Case BotAction.RangedFight
                        DoRangedFight()

                    Case BotAction.MeleeFight
                        DoMeleeFight()

                    Case BotAction.Rest
                        DoRest()

                    Case BotAction.CheckBuffs
                        DoCheckBuffs()

                    Case BotAction.Pause
                        DoPause()

                    Case BotAction.Quit
                        DoQuit()

                    Case BotAction.NOP
                        'do nothing

                End Select

            Catch ex As Exception
                LogLine("Exception in DoAction. _CurrentAction: " & _CurrentAction _
                        & "  _PreviousAction: " & _PreviousAction)
                LogLine(ex.Message)
                LogLine(ex.Source)
                LogLine(ex.StackTrace)
                _Ak.StopInit()
                _Action = BotAction.Quit

            End Try

        End Sub

        Public Overridable Sub DoInitialize()

        End Sub

        Private Sub DoZoneIntoTD()

            Dim doorIndex As Integer

            'find instance door
            LogLine("Find instance door.")

            Thread.Sleep(1500)
            doorIndex = _Ak.SetTarget("door", True)
            LogLine("doorIndex: " & doorIndex)

            If doorIndex = -1 Then
                _FindDoorIndexCount = _FindDoorIndexCount + 1
                LogLine("Door not found, set _Action to try again.")

                If _FindDoorIndexCount < 3 Then
                    _Action = BotAction.ZoneIntoTD
                Else
                    _Action = BotAction.Quit
                End If

            Else
                LogLine("Found door.")

                Thread.Sleep(500)
                LogLine("Move to zone spot.")
                MoveToGXY(PreZoneXCoord, PreZoneYCoord, PreZoneZCoord, 50)
                Thread.Sleep(500)

                Thread.Sleep(500)
                LogLine("Face Loc.")
                '_Keys.Face(KeyDirection.KeyUpDown)
                _Ak.SendString("/faceloc " & PreZoneFaceLocX & " " & PreZoneFaceLocY & "~")
                Thread.Sleep(500)

                Thread.Sleep(500)
                LogLine("Open door. **uses sendstring atm.")
                _Ak.SendString("o~")
                Thread.Sleep(3000)

                'move forward to zone in 
                LogLine("Move to zone in.")
                _AkKeys.MoveForward(KeyDirection.KeyDown)
                Thread.Sleep(1500)
                _AkKeys.MoveForward(KeyDirection.KeyUp)

                LogLine("Sleep for 10 seconds.")
                Thread.Sleep(10000)

                LogLine("Execute kickout logic **COMMENTED OUT FOR TEST.")
                'Kickout(Daoc)

                LogLine("Sleeping for 18 seconds.")
                Thread.Sleep(18000)

                If _Ak.ZoneName = ZoneName Then
                    _ZoneAttemptCount = _ZoneAttemptCount + 1
                    LogLine("Zone in failed, zoneAttemptCount: " & _ZoneAttemptCount)

                    If _ZoneAttemptCount < 2 Then
                        LogLine("Set action for re-try.")
                        _Action = BotAction.ZoneIntoTD
                    Else
                        _Action = BotAction.Quit
                    End If
                    '
                Else
                    LogLine("Zone in ok.")
                    _Action = _ActionQueue.Dequeue
                End If

            End If

        End Sub

        Private Sub DoMoveToNextPatrolPoint()

            'Dim tempFindTargets As Boolean
            'Dim tempAttackType As AttackType
            Dim tempCheckForAgro As Boolean

            If _PatrolPoints.MoveToNextPatrolPoint(_FindTargets, _AttackType, tempCheckForAgro) Then

                If _FindTargets Then

                    'clear the ignored targets list
                    LogLine("ClearIgnoreMobList()")
                    ClearIgnoreMobList()

                    If tempCheckForAgro Then
                        _Action = BotAction.CheckAgro

                        _ActionQueue.Clear()
                        _ActionQueue.Enqueue(BotAction.FindTarget)

                    Else

                        _Action = BotAction.FindTarget

                    End If

                Else 'dont findtargets

                    Select Case (_AttackType)

                        Case AttackType.WaitForAgro
                            _Action = BotAction.CheckAgro

                        Case AttackType.None
                            If _FindTargets Then

                                _Action = BotAction.CheckAgro

                                _ActionQueue.Clear()
                                _ActionQueue.Enqueue(BotAction.MoveToNextPatrolPoint)
                            Else
                                _Action = BotAction.MoveToNextPatrolPoint

                            End If
                            
                    End Select

                End If

            Else 'no more patrol points

                _Action = BotAction.CheckAgro

                _ActionQueue.Clear()
                _ActionQueue.Enqueue(BotAction.Quit)

                Return

            End If

        End Sub

        Public Overridable Sub DoProtect()

        End Sub

        Private Sub DoFindTarget()

            Dim mobMinimumLevel As Integer
            Dim mobMaximumLevel As Integer

            Dim mobConTest As AKServer.DLL.DAoCServer.ConColors.DAOCConRangeDefinition

            Dim spawnId As Integer

            'this is the best time to check if we need a rest
            If (_Ak.PlayerHealth < RestBelowHealth Or _
                _Ak.PlayerMana < RestBelowMana Or _
                _Ak.PlayerStamina < RestBelowEndurance) Then

                'stop running
                _Ak.StopRunning()

                'switch to rest
                _Action = BotAction.Rest

                'clear the queue
                _ActionQueue.Clear()

                Return
            End If

            'calculate min and max levels if automatic is selected.
            'we do this each pull in case we just leveled
            If AutomaticLevelSelect Then

                mobMinimumLevel = 0
                mobMaximumLevel = 0

                mobConTest = AKServer.DLL.DAoCServer.ConColors.CON_RANGES(_Ak.PlayerLevel)

                'find lowest level to fight
                If FightGreens Then
                    mobMinimumLevel = mobConTest.GrayMax + 1
                ElseIf FightBlues Then
                    mobMinimumLevel = mobConTest.GreenMax + 1
                ElseIf FightYellows Then
                    mobMinimumLevel = mobConTest.BlueMax + 1
                ElseIf FightOranges Then
                    mobMinimumLevel = mobConTest.YellowMax + 1
                End If

                'find highest level to fight
                If FightOranges Then
                    mobMaximumLevel = mobConTest.OrangeMax
                ElseIf FightYellows Then
                    mobMaximumLevel = mobConTest.YellowMax
                ElseIf FightBlues Then
                    mobMaximumLevel = mobConTest.BlueMax
                ElseIf FightGreens Then
                    mobMaximumLevel = mobConTest.GreenMax
                End If

            End If

            spawnId = _Ak.FindClosestMob(mobMinimumLevel, mobMaximumLevel, SearchDistance)
            LogLine("spawnId:" & spawnId & "  mobMinimumLevel:" & mobMinimumLevel & "  mobMaximumLevel:" & mobMaximumLevel)

            While spawnId <> -1

                'does this object match our fight selections?
                If TestTarget(spawnId) Then

                    'must have found a target we like
                    'marget it
                    _Ak.StopRunning()

                    Thread.Sleep(200)
                    _Ak.SetTarget(spawnId)
                    Thread.Sleep(200)

                    LogLine("Found target " & _Ak.MobName(spawnId) & " at distance " & DistanceToMob(spawnId))

                    'setup the appropriate action
                    _Action = BotAction.RangedFight

                    Return

                Else

                    spawnId = _Ak.FindNextClosestMob(mobMinimumLevel, mobMaximumLevel, SearchDistance)

                End If

            End While

            'no mob found
            LogLine("Nothing left to fight at this patrolpoint!")

            'we must be done with all targets at this waypoint, lets move on
            _Action = BotAction.Protect

            _ActionQueue.Clear()
            _ActionQueue.Enqueue(BotAction.CheckAgro)
            _ActionQueue.Enqueue(BotAction.MoveToNextPatrolPoint)
            _ActionQueue.Enqueue(BotAction.FindTarget)

        End Sub

        Private Function TestTarget(ByVal SpawnId As Integer) As Boolean

            'is it dead?
            If _Ak.IsMobDead(SpawnId) Then
                Return False
            End If

            'is it in our ignore list?
            If _IgnoreTargetObjects.Contains(SpawnId) Then
                Return False
            End If

            Return True

        End Function

        Private Sub DoCheckAgro()

            Dim spawnId As Integer

            LogLine("Checking agro.")

            spawnId = _Ak.FindClosestMobWithPlayerAsTarget(800)
            'LogLine("DoCheckAgro()-spawnId:" & spawnId)

            If spawnId > 0 Then
                'got agro
                LogLine("Got agro.")

                If _Ak.isPlayerSitting Then
                    LogLine("Player is sitting, stand.")

                    _AkKeys.SitStand(KeyDirection.KeyUpDown)
                    Thread.Sleep(1000)

                End If

                'stop running if running
                _Ak.StopRunning()

                'forget whatever we where suppose to do
                _ActionQueue.Clear()

                'target out attacker
                _Ak.SetTarget(spawnId, True)

                'face target
                _AkKeys.Face(KeyDirection.KeyUpDown)

                'face/stick to whatever it is
                If _Ak.PlayerMana = 0 Then

                    _AkKeys.Face(KeyDirection.KeyUpDown)

                End If

                _AttackType = AttackType.None
                _Action = BotAction.MeleeFight

            Else

                'if attack type is CheckAgro, keep checking agro until mobs are dead
                If _AttackType = AttackType.WaitForAgro Then

                    _Action = BotAction.CheckAgro

                    Return
                End If

                'set current action to next action in queue
                _Action = _ActionQueue.Dequeue

            End If

        End Sub

        Public Overridable Sub DoRangedFight()

        End Sub

        Public Overridable Sub DoMeleeFight()

        End Sub

        'Private _Resting As Boolean = False
        Private Sub DoRest()

            'stop running if needed
            If Not _Ak.isPlayerSitting Then
                _Ak.StopRunning()
            End If

            'setup default action
            _Action = BotAction.Protect

            _ActionQueue.Clear()
            _ActionQueue.Enqueue(BotAction.CheckAgro)
            _ActionQueue.Enqueue(BotAction.Rest)

            'if we're casting something, like a heal, wait till we're done
            If (_Ak.isPlayerCasting) Then
                Return
            End If

            'do we really need to rest?
            If (_Ak.PlayerHealth < RestBelowHealth Or _
                _Ak.PlayerMana < RestBelowMana Or _
                _Ak.PlayerStamina < RestBelowEndurance) Then

                'is this our first time finding we need to rest? If so add message
                If Not _Resting Then
                    LogLine("Resting.")
                End If

                'yes, set the rest flag
                'this will stay set until users full health conditions are met
                _Resting = True
                If Not _Ak.isPlayerSitting Then

                    _AkKeys.SitStand(KeyDirection.KeyUpDown)
                    Thread.Sleep(2000)

                End If

            End If

            'is the RestUntilFullyRecovered bool set to true?
            If RestUntilFullyRecovered Then
                'yes, was our health low enough that we are really needing to rest?

                If _Resting Then
                    'are we back to full health?
                    'zero for non-mana users

                    If (_Ak.PlayerHealth > 98 And _
                        (_Ak.PlayerMana = 0 Or _Ak.PlayerMana > 98) And _
                        _Ak.PlayerStamina > 98) Then

                        'we are fully recovered, lets move on
                        _Action = BotAction.Protect
                        _ActionQueue.Clear()
                        _ActionQueue.Enqueue(BotAction.CheckAgro)
                        _ActionQueue.Enqueue(BotAction.CheckBuffs)

                        If _FindTargets Then
                            _ActionQueue.Enqueue(BotAction.FindTarget)
                        Else
                            _ActionQueue.Enqueue(BotAction.MoveToNextPatrolPoint)
                        End If

                        'Done resting
                        _Resting = False
                        If _Ak.isPlayerSitting Then

                            _AkKeys.SitStand(KeyDirection.KeyUpDown)
                            Return

                        End If 'If _Ak.isPlayerSitting Then

                    End If 'If (_Ak.PlayerHealth > 98 And (_Ak.PlayerMana = 0 Or _Ak.PlayerMana > 98) And _Ak.PlayerStamina > 98) Then

                Else 'our health didnt get low enough that we need to rest? Then we can move on

                    'we are fully recovered, lets move on
                    _Action = BotAction.Protect
                    _ActionQueue.Clear()
                    _ActionQueue.Enqueue(BotAction.CheckAgro)
                    _ActionQueue.Enqueue(BotAction.CheckBuffs)

                    If _FindTargets Then
                        _ActionQueue.Enqueue(BotAction.FindTarget)
                    Else
                        _ActionQueue.Enqueue(BotAction.MoveToNextPatrolPoint)
                    End If

                    If _Ak.isPlayerSitting Then

                        _AkKeys.SitStand(KeyDirection.KeyUpDown)
                        Return

                    End If 'If _Ak.isPlayerSitting Then

                End If 'If _Resting Then

            ElseIf (_Ak.PlayerHealth < RestBelowHealth Or _
                    _Ak.PlayerMana < RestBelowMana Or _
                    _Ak.PlayerStamina < RestBelowEndurance) Then

                'we are fully recovered, lets move on
                _Action = BotAction.Protect
                _ActionQueue.Clear()
                _ActionQueue.Enqueue(BotAction.CheckAgro)
                _ActionQueue.Enqueue(BotAction.CheckBuffs)

                If _FindTargets Then
                    _ActionQueue.Enqueue(BotAction.FindTarget)
                Else
                    _ActionQueue.Enqueue(BotAction.MoveToNextPatrolPoint)
                End If

                'done resting
                _Resting = False

                If (_Ak.isPlayerSitting) Then
                    _AkKeys.SitStand(KeyDirection.KeyUpDown)
                End If

                Return

            End If 'If RestUntilFullyRecovered Then

        End Sub

        Public Overridable Sub DoCheckBuffs()

        End Sub

        Private Sub DoPause()

        End Sub

        Private Sub DoQuit()

            'blah
            LogLine("Shutting _Ak down.")
            _Ak.StopInit()
            _Action = BotAction.NOP

        End Sub

        Private Sub NOP()

            'blah

        End Sub

#Region " Misc methods "

        Public Sub UseQbar(ByVal Qbar As String, ByVal Key As String)

            Dim shift As Byte

            shift = Keys.ShiftKey

            _Ak.SendKeys(shift, True, False)
            _Ak.SendString(Qbar)
            _Ak.SendKeys(shift, False, True)
            Thread.Sleep(100)

            _Ak.SendString(Key)
            Thread.Sleep(100)

        End Sub

        Private Sub MoveToGXY(ByVal X As Double, ByVal Y As Double, ByVal Z As Double, ByVal Range As Double)

            'needs stuck logic

            If Not (_Ak.ZDistance(_Ak.gPlayerXCoord, _Ak.gPlayerYCoord, _Ak.gPlayerZCoord, X, Y, Z)) < Range Then
                _Ak.StartRunning()

                While _Ak.ZDistance(_Ak.gPlayerXCoord, _Ak.gPlayerYCoord, _Ak.gPlayerZCoord, X, Y, Z) > Range _
                    And Not _Ak.IsPlayerDead()

                    _Ak.TurnToHeading(_Ak.FindHeading(_Ak.gPlayerXCoord, _Ak.gPlayerYCoord, X, Y))
                    Thread.Sleep(100)
                End While

                _Ak.StopRunning()
            End If

        End Sub

        Private Sub MoveToZXY(ByVal X As Double, ByVal Y As Double, ByVal Z As Double, ByVal Range As Double)

            'needs stuck logic

            If Not (_Ak.ZDistance(_Ak.zPlayerXCoord, _Ak.zPlayerYCoord, _Ak.zPlayerZCoord, X, Y, Z)) < Range Then
                _Ak.StartRunning()

                While _Ak.ZDistance(_Ak.zPlayerXCoord, _Ak.zPlayerYCoord, _Ak.zPlayerZCoord, X, Y, Z) > Range _
                    And Not _Ak.IsPlayerDead()

                    _Ak.TurnToHeading(_Ak.FindHeading(_Ak.zPlayerXCoord, _Ak.zPlayerYCoord, X, Y))
                    Thread.Sleep(100)
                End While

                _Ak.StopRunning()
            End If

        End Sub

        Public Sub IgnoreMob(ByVal BadTarget As Integer)

            _IgnoreTargetObjects.Add(BadTarget)
            LogLine("IgnoreMob: " & BadTarget)

            'lets only keep 5 most recent kills, new spawns might have the same number
            'While _IgnoreTargetObjects.Count > 6
            '    _IgnoreTargetObjects.RemoveAt(0)
            'End While

        End Sub

        Public Sub ClearIgnoreMobList()

            _IgnoreTargetObjects.Clear()

        End Sub

        Public Function DistanceToMob(ByVal SpawnID As Integer) As Long

            Return _Ak.ZDistance(_Ak.gPlayerXCoord, _Ak.gPlayerYCoord, _Ak.gPlayerZCoord, _Ak.MobXCoord(SpawnID), _Ak.MobYCoord(SpawnID), _Ak.MobZCoord(SpawnID))

        End Function

        Public Overridable Sub ProcessDaocLog(ByVal e As AutoKillerScript.clsAutoKillerScript.AutokillerRegExEventParams)

            'LogLine(e.Logline)

        End Sub

        Public Sub SetLog(ByVal LogPtr As Log)

            _LogPtr = LogPtr

        End Sub

        Public Sub LogLine(ByVal Line As String)

            _LogPtr.Invoke(Line)

        End Sub

#End Region

    End Class

End Namespace