Imports System.Threading
Imports System.IO
Imports AKServer.DLL.DAoCServer

Public Class Form1
    Public WithEvents AK As AutoKillerScript.clsAutoKillerScript
    Private XP As Long
    Private Server As AutoKillerScript.SocketServer.ListenerClass
    Private PlayerAggro As New Hashtable
    Private ThreadBool As Boolean = False

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        AK = New AutoKillerScript.clsAutoKillerScript
        If chkEuro.Checked Then AK.EnableEuro = True
        If chkToA.Checked Then AK.EnableToA = True
        If chkCat.Checked Then AK.EnableCatacombs = True
        AK.RegKey = "BUCKROGERS"
        'AK.ChatLog = "c:\Mythic\Atlantis\chat.log"
        AK.GamePath = "c:\Mythic\Atlantis"
        'AK.EnableAutoQuery = True
        AK.SetLeftTurnKey = CByte(Keys.Q)
        AK.SetRightTurnKey = CByte(Keys.E)
        'AK.ChatLog = "d:\chat.log"
        'AK.AddString(0, "*You examine*")
        'AK.AddString(1, "*casts a spell*")
        'AK.AddString(2, "*You get * experience points.*")
        AK.UseRegEx = True
        AK.AddString(0, "You examine (the |)(?<mob>[A-Za-z\s]*).")
        AK.AddString(1, "is too far away to attack")
        AK.AddString(2, "You get (?<exp>[0-9,]*) experience points.( \((?<camp>[0-9,]*) camp bonus\)( \((?<group>[0-9,]*) group bonus\)|)|)")
        AK.AddString(3, "Target is not in view")
        'You get 97,484 experience points. (15,564 camp bonus)
        'AK.MobList(0) = "Melos scout"
        'AK.MobList(1) = "Melos blade warrior"
        'AK.MobList(2) = "Melos mind mangler"
        'AK.MobList(3) = "Melos earth razer"
        'AK.MobList(4) = "Melos high priest"

        'AK.MobAgroList(0) = False
        'AK.MobAgroList(1) = False
        'AK.MobAgroList(2) = False
        'AK.MobAgroList(3) = False
        'AK.MobAgroList(4) = False

        AK.DoInit()
        Debug.WriteLine("done")

        'AK.RemoveString(2)
        'AK.QueryString(2)
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        'Dim MyGroup As New ArrayList
        'Dim MyGroupNames As New ArrayList
        'Dim i As Short

        ''MyGroup = AK.GroupHealth(1)

        'For i = 0 To MyGroup.Count - 1
        '    Debug.WriteLine(MyGroup(i))
        'Next

        'MyGroupNames = AK.GroupNames(1)

        'For i = 0 To MyGroup.Count - 1
        '    Debug.WriteLine(MyGroupNames(i))
        'Next

        Dim g As AKServer.DLL.DAoCServer.Group
        g = AK.GroupMemberInfo

        For Each gm As GroupMember In g.GroupMemberTable.Values
            Debug.WriteLine(gm.Health.ToString)
            Debug.WriteLine(gm.Name.ToString)
            Debug.WriteLine(gm.ID)
        Next


    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        Dim ObjectID As ArrayList = New ArrayList
        Dim Inv As ArrayList
        Dim i As Integer
        ObjectID = AK.GetClosestObject(5000)
        For i = 0 To ObjectID.Count - 1
            Debug.WriteLine(ObjectID(i))
        Next
        Inv = AK.GetInventoryToKeep
        For i = 0 To Inv.Count - 1
            Debug.WriteLine(Inv(i))
        Next

    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        AK.MobList.Add("sjoalf worshipper", True)

        Dim m As AutoKillerScript.clsAutoKillerScript.MobListMob = AK.MobList("sjoalf worshipper")

        Debug.WriteLine(AK.FindClosestMob(1, 50, 5000, True))
        Debug.WriteLine(AK.FindClosestObject(0, 50, 5000))
        Debug.WriteLine(AK.FindNextClosestObject(1, 50, 5000))
    End Sub
    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
        Debug.WriteLine(AK.GameProcess)
    End Sub

    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        Debug.WriteLine(AK.ZDistance(5000, 5000, 10000, 10000, 100, 100))
        Debug.WriteLine(AK.FindHeading(8217, 22710, 8718, 23676)) '326
    End Sub

    Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button17.Click
        'AK.StartRunning()
        Thread.Sleep(3000)
        'AK.StopRunning()
    End Sub

    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button18.Click
        Dim buffs As LocalBuffList = AK.LocalBufs

        For Each b As LocalBuff In buffs.BuffTable.Values
            Debug.WriteLine(b.Name)
        Next


        Dim concbuffs As ConcentrationBuffList = AK.ConcBufs

        For Each c As ConcentrationBuff In concbuffs
            Debug.WriteLine(c.Name)
            Debug.WriteLine(c.Target)
        Next



    End Sub

    Private Sub Button20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button20.Click
        If Not AK Is Nothing Then
            AK.StopInit()
            AK = Nothing
        End If
        If Not Server Is Nothing Then
            Server.Close()
        End If
        Me.Close()
    End Sub

    Private Sub Button22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button22.Click
        Debug.Write("version " & AK.getVersion)
        'Debug.Write("index " & AK.TargetIndex)
    End Sub

    Private Sub Button23_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button23.Click

        'Do
        Dim t As Integer = AK.SetTarget("Ardanos", False)
        'Dim t As Boolean = AK.SetTarget(5220)

        '    If t = -1 Then
        '        Debug.WriteLine("here")
        '    End If
        '    Thread.Sleep(200)
        'Loop


    End Sub

    Private Sub Button24_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button24.Click
        Thread.Sleep(2000)
        AK.SendKeys(CByte(Keys.ShiftKey), True) 'hold shift key
        'AK.SendString("4")
        AK.SendKeys(CByte(Keys.D0))
        Thread.Sleep(100)
        AK.SendKeys(CByte(Keys.ShiftKey), , True)  'release shift key
    End Sub
    Private Sub MyLogger(ByVal LogString As String) Handles AK.OnLog
        Debug.Write(LogString)
    End Sub

    Private Sub Button25_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button25.Click
        Debug.WriteLine("Loot")
        Dim StuffToLoot As ArrayList
        Dim i As Integer

        Dim FilterItems As New Collections.Generic.List(Of String)
        FilterItems.Add("blue diamond")
        AK.FilterItems = FilterItems

        'Threading.Thread.Sleep(4000)
        StuffToLoot = AK.GetAllObjects(1300, True, False, True)
        For i = 0 To StuffToLoot.Count - 1
            'Debug.WriteLine(AK.MobName(AK.SetTarget(StuffToLoot(i), False)))
            Debug.Write(StuffToLoot(i).ToString & " ")
            'If StuffToLoot(i) = 0 Then Exit For
            'AK.SetTarget(, StuffToLoot(i))
            'Threading.Thread.Sleep(250)
            'AK.SendKeys(Keys.G)
            'Threading.Thread.Sleep(250)
        Next

    End Sub
    Public Sub Sleep(ByVal miliseconds As Integer)
        Dim WaitCycles As Integer = CInt(miliseconds / 50)
        Dim I As Integer

        For I = 1 To WaitCycles
            Thread.Sleep(50)
            Application.DoEvents()
        Next
    End Sub
    Private Sub LogF(ByVal strString As String)
        Dim myStreamWriter As StreamWriter = File.AppendText("temp.log")

        Try
            Thread.Sleep(1)
            Application.DoEvents()
            myStreamWriter.WriteLine(TimeString & ": " & strString)
            myStreamWriter.Flush()
        Catch ex As Exception
            Debug.Write(ex.Message)
        Finally
            If Not myStreamWriter Is Nothing Then
                myStreamWriter.Close()
            End If
        End Try
    End Sub

    Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button21.Click
        Dim StuffToLoot As New ArrayList
        Dim i As Integer
        StuffToLoot = AK.GetAllObjects(1000, True)

        For i = 0 To StuffToLoot.Count - 1
            Debug.WriteLine(StuffToLoot(i))
        Next
    End Sub

    Private Sub Button26_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button26.Click
        Debug.WriteLine(AK.getVersion())
    End Sub

    Private Sub Button30_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button30.Click
        Dim x As Integer = AK.SearchArea(3000, DAOCObjectClass.ocMob)
        If Not x = -1 Then
            Debug.WriteLine(AK.MobName(x))
        End If

    End Sub

    Private Sub Button31_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button31.Click
        Dim sw As StreamWriter
        Dim temp As String
        sw = File.AppendText("d:\1.txt")
        Do
            temp = AK.GetString
            If temp <> "" Then sw.WriteLine(temp)
            sw.Flush()
            Sleep(250)
        Loop
    End Sub

    Private Sub Button32_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button32.Click
        'Debug.WriteLine(AK.spawnId(AK.TargetIndex))
        'Debug.WriteLine(AK.TargetIndex)
        'Debug.WriteLine(AK.QueryNPC(AK.TargetIndex))
        Debug.WriteLine(AK.MobTarget(AK.TargetIndex))

        'Dim pet As New ArrayList
        'pet.Add(326)
        'Dim NewMob As Integer = AK.FindClosestMobWithPlayerAsTarget(1000, 2211, pet)

    End Sub
    Private Sub Button33_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button33.Click
        Dim TempThread As Threading.Thread
        TempThread = New Threading.Thread(AddressOf test)
        TempThread.Name = "Temp Thread"
        TempThread.Start()

        Do
            Debug.WriteLine(AK.QueryString(0))
            Debug.WriteLine(AK.GetString)
            Application.DoEvents()
            Sleep(250)
        Loop
    End Sub
    Private Sub test()

    End Sub

    Private Sub Button34_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button34.Click
        test()
    End Sub

    Private Sub Button35_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button35.Click
        Do
            'Debug.WriteLine(AK.FindClosestMob(1, 50, 5000))
            Debug.WriteLine(AK.GetAllObjects(5000))
            Debug.WriteLine(AK.QueryString(0))
            Debug.WriteLine(AK.GetString)
            Application.DoEvents()
            Sleep(250)
            If AK.isPlayerSitting Then
                Beep()
            End If
        Loop
    End Sub






    Private Sub ThreadExceptionHandler(ByVal sender As System.Object, ByVal e As System.Threading.ThreadExceptionEventArgs)
        Debug.WriteLine(e.Exception)
    End Sub


    Private Sub Button40_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button40.Click
        Thread.Sleep(4000)
        Debug.WriteLine("start")
        With AK
            .SendKeys(CByte(Keys.W), True) 'need to break face
            Thread.Sleep(250)
            .SendKeys(CByte(Keys.W), , True)
            Thread.Sleep(250)
            '.StartRunning()
        End With
    End Sub

    Private Sub Button41_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button41.Click
        'Debug.WriteLine(AK.PlayerInAGroupClass(1))
        'Debug.WriteLine(AK.PlayerInAGroupHealth(1))
        'Debug.WriteLine(AK.PlayerInAGroupName(1))

        Dim al As AKServer.DLL.DAoCServer.Group = AK.GroupMemberInfo

        For Each g As AKServer.DLL.DAoCServer.GroupMember In al.GroupMemberTable.Values
            Debug.WriteLine(g.Name)
            Debug.WriteLine(AK.MobName(g.ID))
        Next

        'Dim a As New AutoKillerScript.clsAutoKillerScript.GroupMember

        'For Each a In al
        '    Debug.WriteLine(a.Name & "" & a.Class)
        'Next



    End Sub

    Private Sub Button42_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button42.Click
        Dim RouteFile As String = "d:/test_route.txt"
        AK.AddRoute(250, "sell")
        AK.SaveRoute(RouteFile)
    End Sub

    Private Sub Button43_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button43.Click
        Debug.WriteLine(AK.QueryString(0))
    End Sub
    Private Sub Button44_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button44.Click
        Debug.WriteLine(AK.gtXCoord)
        Debug.WriteLine(AK.gtYCoord)
        Debug.WriteLine(AK.gtZCoord)
    End Sub

    Private Sub Button45_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button45.Click
        Thread.Sleep(4000)
        AK.SendKeys(83, True)
        Thread.Sleep(2000)
        AK.SendKeys(83, , True)
        'Thread.Sleep(4000)
        'AK.SendKeys(Keys.S)
    End Sub

    Private Sub Button46_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button46.Click
        'Thread.Sleep(2000)
        'AK.SendKeys(Keys.ShiftKey, True) 'hold shift key
        'AK.SendKeys(Keys.D1)
        'Thread.Sleep(250)
        'AK.SendKeys(Keys.ShiftKey, , True)  'release shift key
        Thread.Sleep(2000)
        AK.SendKeys(CByte(Keys.D2))
        'AK.SendString("2")
        'AK.SendKeys(Keys.NumLock)
        'Thread.Sleep(4000)
        'AK.SendString("/sit~")
    End Sub


    Private Sub Query(ByVal e As AutoKillerScript.clsAutoKillerScript.AutokillerQueryEventParams) Handles AK.OnQueryStringTrue

        Select Case e.QueryID
            Case 0
                Debug.WriteLine(e.Logline)
            Case 1
                Debug.WriteLine(e.Logline)
            Case 2
                Dim myarray() As String = e.Logline.Split()
                Dim XPGained As Integer = XPGained + CInt(myarray(2))
                Debug.WriteLine("You have gained " & Format(XPGained, "###,###,###") & " experience.")
        End Select
    End Sub
    Private Sub RegExQuery(ByVal e As AutoKillerScript.clsAutoKillerScript.AutokillerRegExEventParams) Handles AK.OnRegExTrue

        Debug.WriteLine(e.Logline)
        Debug.WriteLine(e.QueryID)
        'RegMatch.Groups("PlayerName").Value
        Debug.WriteLine(e.RegExMatch.Groups("mob").Value)
    End Sub
    Private Sub Button49_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button49.Click

        Thread.Sleep(2000)
        'AK.TurnToHeading(AK.FindHeading(AK.gPlayerXCoord, AK.gPlayerYCoord, AK.MobXCoord(AK.TargetIndex), AK.MobYCoord(AK.TargetIndex)))


    End Sub
    Private Sub Button27_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button27.Click
        Debug.WriteLine(AK.GetPlayerINI())
    End Sub

    Private Sub Button48_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button48.Click
        Do
            AK.SendString("8")
            Thread.Sleep(250)
            AK.SendString("8")
            Application.DoEvents()
        Loop
    End Sub


    Private Sub Button51_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button51.Click
        Do
            Debug.WriteLine(AK.PlayerCasting)
            'Debug.WriteLine(AK.GetOKtoCast)
            Thread.Sleep(100)
        Loop

    End Sub

    Private Sub Button52_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button52.Click
        Dim a As DAOCInventoryItem = AK.InventoryItem(0)

        Debug.WriteLine(a.Description)

        'Dim ba As  = AK.MerchantItems

        'Threading.Thread.Sleep(2000)
        'Dim InventoryToKeep As ArrayList
        'Dim a As New AutoKillerScript.QBar(AK)
        'a.ClickSlot(1)
        'Dim statspage As New AutoKillerScript.StatsWindow(AK)
        'statspage.MoveInventoryItemToWeaponSlot(1, 3, AutoKillerScript.WeaponSlots.Ranged)
        'statspage.MoveInventoryItemToQbarSlot(1, 3, 10, a)
        'statspage.MoveWeaponItemToQbarSlot(AutoKillerScript.WeaponSlots.Ranged, 10, a)
        'InventoryToKeep = AK.GetInventoryToKeep
        'Dim b As New AutoKillerScript.Sell(AK)
        'b.Sell(3, InventoryToKeep)

    End Sub



    Private Sub Button54_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button54.Click
        Thread.Sleep(4000)
        AK.MouseMove(25, 319)
        '832 0
    End Sub

    Private Sub Button55_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button55.Click
        Dim a As New ArrayList
        'a.Add(4546)
        'a.Add(323)
        'a.Add(6371)
        'Debug.WriteLine("MobTarget " & AK.FindClosestMobWithPlayerAsTarget(2000, AK.TargetIndex, a))
        'Debug.WriteLine(AK.FindNextClosestMobWithPlayerAsTarget(2000, AK.TargetIndex, a))
        ''Do
        '    Debug.WriteLine("MobTarget " & AK.FindClosestMobWithPlayerAsTarget(3000, a))
        '    Threading.Thread.Sleep(1000)
        'Loop

        'AK.SetTarget("Cerridwene")
    End Sub








    '-----------------------------------------------------------------------------
    ' Name: FindXY()
    ' Desc: Finds x,y coordinate to pull or cast
    ' Dist = distance between patrol point and AK.TargetIndex
    '-----------------------------------------------------------------------------
    Private Function FindXY(ByVal wpX As Integer, ByVal wpY As Integer, ByVal mobX As Integer, ByVal mobY As Integer, _
    ByVal Dist As Integer, ByVal PullDistance As Integer) As Point
        Dim a As Point

        Try
            a.X = CInt(wpX + (Dist - PullDistance) / Dist * (mobX - wpX))
            a.Y = CInt(wpY + (Dist - PullDistance) / Dist * (mobY - wpY))
        Catch ex As Exception
            a.X = wpX
            a.Y = wpY
            Return a
        End Try

        Return a
    End Function




    Private Sub Button62_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button62.Click
        Thread.Sleep(2000)
        Dim Sell As New AutoKillerScript.Sell(AK)
        Dim i As Integer
        Dim InventoryToKeep As ArrayList
        InventoryToKeep = AK.GetInventoryToKeep
        For i = 0 To InventoryToKeep.Count - 1
            Debug.WriteLine(InventoryToKeep(i))
        Next
        Sell.Sell(3, InventoryToKeep) 'number is user defined
    End Sub

    Private Sub Button63_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button63.Click
        Debug.WriteLine(AK.QueryString(0))
    End Sub

    Private Sub Button64_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button64.Click
        Server = New AutoKillerScript.SocketServer.ListenerClass

        Server.Listen()
    End Sub

    Private Sub Button65_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button65.Click
        Dim key As New AutoKillerScript.UserKeys(AK)
        Thread.Sleep(3000)
        key.Sell()
    End Sub



    Private Sub Button67_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button67.Click
        Debug.WriteLine(AK.PlayerName)
        Debug.WriteLine(AK.PlayerClass)
        Debug.WriteLine(AK.PlayerRace)
        Debug.WriteLine(AK.PlayerBaseClass)
        Debug.WriteLine(AK.PlayerGuild)
        'Debug.WriteLine(AK.PlayerRealmRank) 'not working
        'Debug.WriteLine(AK.PlayerProfLevel)
        Debug.WriteLine(AK.PlayerLevel)
        'Debug.WriteLine(AK.PlayerRealm)
        'Debug.WriteLine(AK.PlayerOrderLevel)
        'Debug.WriteLine(AK.PlayerOrder)
        'Debug.WriteLine(AK.PlayerProf)
        'Debug.WriteLine(AK.PlayerHouseLot)
        'Debug.WriteLine(AK.PlayerStrength)
        'Debug.WriteLine(AK.PlayerConstitution)
        'Debug.WriteLine(AK.PlayerQuickness)
        'Debug.WriteLine(AK.PlayerDexterity)
        'Debug.WriteLine(AK.PlayerIntelligence)
        'Debug.WriteLine(AK.PlayerEmpathy)
        'Debug.WriteLine(AK.PlayerPiety)
        'Debug.WriteLine(AK.PlayerCharisma)
        'Debug.WriteLine(AK.PlayerArmorFactor)
        'Debug.WriteLine(AK.PlayerWeaponSkill)
        'Debug.WriteLine(AK.PlayerWeaponDamage)
        'Debug.WriteLine(AK.PlayerHitpoints)
        'Debug.WriteLine(AK.PlayerLastName)


        'Dim c As AutoKillerScript.clsAutoKillerScript.CraftSkill = AK.oCraftSkill(1)
        'Debug.WriteLine(c.Name)
        'Debug.WriteLine(c.Level)


        'Dim p As AutoKillerScript.clsAutoKillerScript.Skill = AK.oPlayerSkill(1)
        'Debug.WriteLine(p.Name)
        'Debug.WriteLine(p.Level)


        Debug.WriteLine(AK.MerchantItem(1, 1))
        'Debug.WriteLine(AK.InventoryItem(1, 1))
        Debug.WriteLine(AK.Equipment(AutoKillerScript.clsAutoKillerScript.oEquipment.oeCloak))
        Debug.WriteLine(AK.VaultItem(1))
    End Sub



    Private Function Distance(ByVal X1 As Double, ByVal Y1 As Double, ByVal X2 As Double, ByVal Y2 As Double) As Double
        Try
            Return System.Math.Sqrt((X1 - X2) ^ 2 + (Y2 - Y1) ^ 2)
        Catch ex As Exception
            LogF(ex.Message)
        End Try
    End Function
    'Private Sub HandleOnPetWindowUpdate(ByVal Pet As AKServer.DLL.DAoCServer.DAOCPet) Handles AK.OnPetWindowUpdate
    '    Debug.WriteLine(Pet.ID)
    '    Debug.WriteLine(Pet.Type)
    '    Debug.WriteLine(Pet.PetState)
    '    Debug.WriteLine(Pet.PetPosition)
    'End Sub
    'Private Sub HandleOnPetUpdate(ByVal ID As Integer, ByVal ID2 As Integer) Handles AK.OnPetUpdate
    '    Debug.WriteLine(ID)
    '    Debug.WriteLine(ID2)
    'End Sub

    Private Sub Button70_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button70.Click
        Static found As Boolean

        If Not found Then
            AK.InjectFindWindow()
            found = True
        End If

        AK.InjectSetCaption("AutoKiller rocks")
    End Sub

    Private Sub Button71_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button71.Click

        If Not ThreadBool Then
            ThreadBool = True
            Dim t As New Thread(AddressOf ThreadLoop)
            t.Start()
        Else
            ThreadBool = False
        End If

    End Sub
    Private Sub ThreadLoop()
        While ThreadBool
            Debug.WriteLine(AK.PlayerDir.ToString)
        End While
    End Sub

    Private Sub Button73_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button73.Click
        Debug.WriteLine(AK.PlayerLeftHand)
        Debug.WriteLine(AK.PlayerRightHand)
    End Sub

   
    Private Sub Button143_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button143.Click
        Debug.WriteLine(AK.gPlayerXCoord)
        Debug.WriteLine(AK.gPlayerYCoord)
        Debug.WriteLine(AK.gPlayerZCoord)
        Debug.WriteLine(AK.ZoneID)

        Debug.WriteLine(AK.gtXCoord)
        Debug.WriteLine(AK.gtYCoord)
        Debug.WriteLine(AK.gtZCoord)
    End Sub

    Private Sub Button142_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button142.Click
        Debug.WriteLine(AK.zPlayerXCoord)
        Debug.WriteLine(AK.zPlayerYCoord)
        Debug.WriteLine(AK.zPlayerZCoord)
    End Sub

    Private Sub Button141_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button141.Click
        Debug.WriteLine(AK.PlayerHealth)
        Debug.WriteLine(AK.PlayerStamina)
        'Do
        Debug.WriteLine(AK.PlayerMana)
        '    Threading.Thread.CurrentThread.Sleep(250)
        'Loop

    End Sub
End Class
Public Class MobAggro
    Inherits ArrayList
End Class
Public Class Partner
    Public Name As String
    Public [Class] As String
End Class


