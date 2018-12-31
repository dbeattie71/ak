Imports System.IO
Imports System.Text.RegularExpressions

Public Class USAddressFinder
    Inherits AddressFinder

    Dim lastaddress As Long = 0

    Public Sub New(ByVal FileName As String)
        MyBase.New(FileName)
        GroupSizeStruct = "&HFD8"
        OffsetFromMana = "+ 4"
    End Sub
    Protected Overrides Function RunAddress() As String
        Dim strRet As String

        'SET RUN AND COMBAT MODE - TOA and SI
        's = "893D(?<SetCombat>.{8})E8(........)5757E8(........)83C410893D(?<SetRun>.{8})"
        's = "8B45.{2}833D(?<SetRun>.{8})008B08"
        's = "74..A1(?<SetRun>.{8})F7D81BC0.{2,8}4040"
        're = New System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.Singleline)

        mt = Regex.Match(t, "74..A1(?<SetRun>.{8})F7D81BC0.{2,8}4040", System.Text.RegularExpressions.RegexOptions.Singleline)
        If mt.Success Then
            With mt
                strRet = "'Address: " & Hex(mt.Index \ 2) & vbCrLf
                With .Groups("SetRun")
                    If .Success Then
                        ReDim sa(.Length)
                        sa = StringtoArray(.Value)
                        strRet &= "RunningAddress =  " & "&H" & Hex(ArrayToValue(sa)) & "'*" & vbCrLf
                    End If
                End With
                'With .Groups("SetCombat")
                '    If .Success Then
                '        ReDim sa(.Length)
                '        sa = StringtoArray(.Value)
                '        strRet &= "CombatAddress =  " & "&H" & Hex(ArrayToValue(sa)) & "'*" & vbCrLf
                '    End If
                'End With
            End With
        Else
            strRet &= "RunningAddress =  NULL" & "'*" & vbCrLf
            'strRet &= "CombatAddress =  NULL" & "'*" & vbCrLf
        End If

        Return strRet
    End Function
    Protected Overrides Function ZoneAddress() As String
        Dim strRet As String

        ' ZONEID - TOA and SI
        's = "E8........8B(?:D8|F0)3B(?:1D|35)(?<ZoneID>.{8})74"
        're = New System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.Singleline)

        mt = Regex.Match(t, "D9..........D9..........D9..........E8........8B(?:D8|F0)3B(?:1D|35)(?<ZoneID>.{8})74", System.Text.RegularExpressions.RegexOptions.Singleline)
        If mt.Success Then
            With mt
                strRet = "'Address: " & Hex(mt.Index \ 2) & vbCrLf
                With .Groups("ZoneID")
                    If .Success Then
                        ReDim sa(.Length)
                        sa = StringtoArray(.Value)
                        strRet &= "ZoneAddress = " & "&H" & Hex(ArrayToValue(sa)) & "'zone number address get value as byte *" & vbCrLf
                    End If
                End With
            End With
        Else
            strRet &= "ZoneAddress = NULL" & "'zone number address get value as byte *" & vbCrLf
        End If

        Return strRet

    End Function
    Protected Overrides Function FindNetFunctions() As String
        Dim strRet As String

        ' ZONEID - TOA and SI
        's = "E8........8B(?:D8|F0)3B(?:1D|35)(?<ZoneID>.{8})74"
        're = New System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.Singleline)

        mt = Regex.Match(t, "5.8B..E8..FDFFFF85C075025.C3", System.Text.RegularExpressions.RegexOptions.Singleline)
        If mt.Success Then
            With mt
                strRet = "'GetTCP Address: " & Hex(&H400000 + mt.Index \ 2) & " -> " & mt.Value.Substring(0, Math.Min(mt.Value.Length, 40)) & vbCrLf
            End With
        Else
            strRet = "'GetTCP Address = NULL" & vbCrLf
        End If
        mt = Regex.Match(t, "(?:558BECB800010000E8........8B4508|558BEC81EC000100008B4D08568D34080F)", System.Text.RegularExpressions.RegexOptions.Singleline)
        If mt.Success Then
            With mt
                strRet &= "'GetUDP Address: " & Hex(&H400000 + mt.Index \ 2) & " -> " & mt.Value.Substring(0, Math.Min(mt.Value.Length, 40)) & vbCrLf
            End With
        Else
            strRet &= "'GetUDP Address = NULL" & vbCrLf
        End If
        mt = Regex.Match(t, "558BECB8..100000E8......00833D|558BECB8..110000E8......00833D", System.Text.RegularExpressions.RegexOptions.Singleline)
        If mt.Success Then
            With mt
                strRet &= "'SendTCP Address: " & Hex(&H400000 + mt.Index \ 2) & " -> " & mt.Value.Substring(0, Math.Min(mt.Value.Length, 40)) & vbCrLf
            End With
        Else
            strRet &= "'SendTCP Address = NULL" & vbCrLf
        End If
        mt = Regex.Match(t, "(?:558BEC81EC0C080000833D.{8}02740433C0C9C3|558BECB80C080000E8.{8}833D.{8}02740433C0C9C3)", System.Text.RegularExpressions.RegexOptions.Singleline)
        If mt.Success Then
            With mt
                strRet &= "'SendUDP Address: " & Hex(&H400000 + mt.Index \ 2) & " -> " & mt.Value.Substring(0, Math.Min(mt.Value.Length, 40)) & vbCrLf
            End With
        Else
            strRet &= "'SendUDP Address = NULL" & vbCrLf
        End If
        mt = Regex.Match(t, "39..F000000074.{14}E8.{8}8BF0", System.Text.RegularExpressions.RegexOptions.Singleline)
        If mt.Success Then
            strRet &= "'ID2Index Address: " & Hex(&H400000 + mt.Index \ 2) & " -> " & mt.Value.Substring(0, Math.Min(mt.Value.Length, 40)) & vbCrLf
        Else
            strRet &= "'ID2Index Address = NULL" & vbCrLf
        End If
        Return strRet
    End Function
    Protected Overrides Function NewBeginMobData() As String
        Dim strRet As String

        ' MobSTART - TOA and SI
        's = "83CFFF6A08897DFCA3(?<MobStart>.{8})E8"
        's = "6A02.*?E8.{8}.*?E8(?<MobStart>.{8})"
        're = New System.Text.RegularExpressions.Regex(S, System.Text.RegularExpressions.RegexOptions.Singleline)
        s = "6A02.*?E8.{8}(?:|.{2}|.{4}|.{6}|.{8}|.{10})(?:E8(?<MobStart>.{8})|8E(?<MobStart>.{2}))"

        Dim mt As Match = Regex.Match(Lastmatch.Value, s, System.Text.RegularExpressions.RegexOptions.Singleline)
        If mt.Success Then
            With mt
                With .Groups("MobStart")
                    If .Success Then
                        ' get subroutine
                        s = "85C(?:0|9).*?8B(?:0D|35|3D)(?<MobStart>.{8}).*?E8(?<XORStart>.{8}).*?C3"

                        'to get the address of the call you need to add the offset value to the address of the call + 5 (size of a call)
                        Dim address As Long = (lastaddress + .Index \ 2 + addressoffset(stringtoarray(.Value)) - 1) And &HFFFFFF
                        address -= &H400000
                        address *= 2

                        Dim Newmt As Match = re.Match(t.Substring(address, 1000), s, System.Text.RegularExpressions.RegexOptions.Singleline)
                        If Newmt.Success Then
                            With Newmt
                                With .Groups("MobStart")
                                    If .Success Then
                                        strRet = "'Address: " & Hex(address \ 2 + .Index \ 2 + &H400000) & vbCrLf
                                        ReDim sa(.Length)
                                        sa = StringtoArray(.Value)
                                        strRet &= "BeginMobData = " & "&H" & Hex(ArrayToValue(sa)) & "'*" & vbCrLf
                                    Else
                                        strRet &= "BeginMobData = NULL" & "'*" & vbCrLf
                                    End If
                                End With
                                With .Groups("XORStart")
                                    If .Success Then
                                        ' go into sub and get the data
                                        address = ((address \ 2 + &H400000 + .Index \ 2) + AddressOffset(StringtoArray(.Value)) - 1) And &HFFFFFF
                                        address -= &H400000
                                        address *= 2

                                        Dim s As String = re.Match(t.Substring(address), "5.*?C20400").Value
                                        mt = re.Match(s, "5..*?81(?:36|37)(?<XOR1>.{8})8B.*?E8(?<StringAddress>.{8}).*?(?:C20400|5BC9C3)", System.Text.RegularExpressions.RegexOptions.Singleline)
                                        If mt.Success Then
                                            With mt
                                                With .Groups("XOR1")
                                                    If .Success Then
                                                        ReDim sa(.Length)
                                                        sa = StringtoArray(.Value)
                                                        strRet &= "XOR1 = " & "&H" & Hex(ArrayToValue(sa)) & "'*" & vbCrLf
                                                    Else
                                                        strRet &= "XOR1 = NULL" & "'*" & vbCrLf
                                                    End If
                                                End With
                                                With .Groups("StringAddress")
                                                    If .Success Then
                                                        address = ((address \ 2 + &H400000 + .Index \ 2) + AddressOffset(StringtoArray(.Value)) - 1) And &HFFFFFF
                                                        strRet &= "GetPtrAddress = " & "&H" & Hex(address) & "'*" & vbCrLf
                                                    Else
                                                        strRet &= "GetPtrAddress = NULL" & "'*" & vbCrLf
                                                    End If
                                                End With
                                            End With
                                        Else
                                            strRet &= "XOR1 = NULL" & "'*" & vbCrLf
                                        End If
                                    Else
                                        strRet &= "XOR1 = NULL" & "'*" & vbCrLf
                                    End If
                                End With
                            End With
                        Else
                            strRet &= "BeginMobData = NULL" & "'*" & vbCrLf
                            strRet &= "XOR1 = NULL" & "'*" & vbCrLf
                        End If
                    End If
                End With
            End With
        Else
            strRet &= "BeginMobData = NULL" & "'*" & vbCrLf
            strRet &= "XOR1 = NULL" & "'*" & vbCrLf
        End If

        Return strRet

    End Function
    Protected Overrides Function NewStrPointer() As String
        Dim strRet As String

        ' MobSTART - TOA and SI
        's = "83CFFF6A08897DFCA3(?<MobStart>.{8})E8"
        's = "8D471D.*?E8.{8}.*?E8(?<MobStart>.{8})"
        're = New System.Text.RegularExpressions.Regex(S, System.Text.RegularExpressions.RegexOptions.Singleline)

        Dim mt As Match = Regex.Match(Lastmatch.Value, "8D(?:47|46)1D.*?E8.{8}.*?E8(?<MobStart>.{8})", System.Text.RegularExpressions.RegexOptions.Singleline)
        If mt.Success Then
            With mt
                With .Groups("MobStart")
                    If .Success Then
                        ' get subroutine
                        s = ".*?8D34B5(?<MobStart>.{8})"

                        'to get the address of the call you need to add the offset value to the address of the call + 5 (size of a call)
                        Dim address As Long = (lastaddress + .Index \ 2 + addressoffset(stringtoarray(.Value)) - 1) And &HFFFFFF
                        address -= &H400000
                        address *= 2

                        mt = re.Match(t.Substring(address), s, System.Text.RegularExpressions.RegexOptions.Singleline)
                        If mt.Success Then
                            With mt
                                With .Groups("MobStart")
                                    If .Success Then
                                        strRet = "'Address: " & Hex((lastmatch.Index + 2 * arraytovalue(StringtoArray(.Value)) + mt.Index) \ 2) & vbCrLf
                                        ReDim sa(.Length)
                                        sa = StringtoArray(.Value)
                                        strRet &= "StrPointer= " & "&H" & Hex(ArrayToValue(sa)) & "'*" & vbCrLf
                                    End If
                                End With
                            End With
                        End If
                    End If
                End With
            End With
        Else
            strRet &= "StrPointer = NULL" & "'*" & vbCrLf
        End If

        Return strRet

    End Function
    Protected Overrides Function NewSeekXYZ() As String
        Dim strRet As String

        ' MobSTART - TOA and SI
        's = "83CFFF6A08897DFCA3(?<MobStart>.{8})E8"
        's = "8B4708.*?D99E(?<X>.{8})"

        Dim mt As Match = Regex.Match(Lastmatch.Value, "8B(?:47|46)08.*?D9(?:9E(?<X>.{8})|5E(?<X>.{2})|9B(?<X>.{8})|5B(?<X>.{2}))", System.Text.RegularExpressions.RegexOptions.Singleline)
        If mt.Success Then
            With mt
                With .Groups("X")
                    If .Success Then
                        ' get subroutine
                        strRet &= "'Address: " & Hex((lastmatch.Index + mt.Index) \ 2) & vbCrLf
                        ReDim sa(.Length)
                        sa = StringtoArray(.Value)
                        strRet &= "SeekXpos= " & "&H" & Hex(ArrayToValue(sa)) & "'*" & vbCrLf
                    End If
                End With
            End With
        Else
            strRet &= "SeekXpos = NULL" & "'*" & vbCrLf
        End If
        's = "8B470C.*?D99E(?<Y>.{8})"

        mt = Regex.Match(Lastmatch.Value, "8B(?:47|46)0C.*?D9(?:9E(?<Y>.{8})|5E(?<Y>.{2})|5B(?<Y>.{2})|9B(?<Y>.{8}))", System.Text.RegularExpressions.RegexOptions.Singleline)
        If mt.Success Then
            With mt
                With .Groups("Y")
                    If .Success Then
                        ' get subroutine
                        'TODO 24h = 24C??? toa
                        strRet &= "'Address: " & Hex((lastmatch.Index + mt.Index) \ 2) & vbCrLf
                        ReDim sa(.Length)
                        sa = StringtoArray(.Value)
                        strRet &= "SeekYpos= " & "&H" & Hex(ArrayToValue(sa)) & "'*" & vbCrLf
                    End If
                End With
            End With
        Else
            strRet &= "SeekYpos = NULL" & "'*" & vbCrLf
        End If
        's = "0FBF4706.*?D99E(?<Z>.{8})"

        mt = Regex.Match(Lastmatch.Value, "0FBF(?:47|46)06.*?D9(?:9E(?<Z>.{8})|5E(?<Z>.{2})|9B(?<Z>.{8}))", System.Text.RegularExpressions.RegexOptions.Singleline)
        If mt.Success Then
            With mt
                With .Groups("Z")
                    If .Success Then
                        ' get subroutine
                        strRet &= "'Address: " & Hex((lastmatch.Index + mt.Index) \ 2) & vbCrLf
                        ReDim sa(.Length)
                        sa = StringtoArray(.Value)
                        strRet &= "SeekZpos= " & "&H" & Hex(ArrayToValue(sa)) & "'*" & vbCrLf
                    End If
                End With
            End With
        Else
            strRet &= "SeekZpos = NULL" & "'*" & vbCrLf
        End If

        Return strRet

    End Function
    Protected Overrides Function NewSeekDirection() As String
        Dim strRet As String

        ' MobSTART - TOA and SI
        's = "83CFFF6A08897DFCA3(?<MobStart>.{8})E8"
        's = "668B4704.*?6689(?:(86(?<DIR>.{8}))|(46(?<DIR>.{2})))"
        Dim mt As Match = Regex.Match(Lastmatch.Value, "668B(?:47|46)04.*?6689(?:(86(?<DIR>.{8}))|(46(?<DIR>.{2}))|(83(?<DIR>.{8}))|(43(?<DIR>.{2})))", System.Text.RegularExpressions.RegexOptions.Singleline)
        If mt.Success Then
            With mt
                With .Groups("DIR")
                    If .Success Then
                        ' get subroutine
                        strRet = "'Address: " & Hex((lastmatch.Index + mt.Index) \ 2) & vbCrLf
                        ReDim sa(.Length)
                        sa = StringtoArray(.Value)
                        strRet &= "SeekDirection= " & "&H" & Hex(ArrayToValue(sa)) & "'*" & vbCrLf
                    End If
                End With
            End With
        Else
            strRet &= "SeekDirection = NULL" & "'*" & vbCrLf
        End If
        Return strRet

    End Function
    Protected Overrides Function NewSeekSpawnID() As String
        Dim strRet As String

        ' MobSTART - TOA and SI
        's = "83CFFF6A08897DFCA3(?<MobStart>.{8})E8"
        's = "0FB707.*?8986(?<ID>.{8})"

        Dim mt As Match = Regex.Match(Lastmatch.Value, "5..*?E8(?<AddressToSpawnID>.{8}).*?6A02.*?E8(?<AddressToNPC>.{8})", System.Text.RegularExpressions.RegexOptions.Singleline)
        If mt.Success Then
            With mt
                With .Groups("AddressToSpawnID")
                    If .Success Then
                        ' get subroutine
                        s = "84C0.*?E8.{8}8B(?:48(?<SpawnID>.{2})|88(?<SpawnID>.{8}))3B"

                        'to get the address of the call you need to add the offset value to the address of the call + 5 (size of a call)
                        Dim address As Long = (lastaddress + .Index \ 2 + addressoffset(stringtoarray(.Value)) - 1) And &HFFFFFF
                        address -= &H400000
                        address *= 2

                        mt = re.Match(t.Substring(address, 1000), s, System.Text.RegularExpressions.RegexOptions.Singleline)
                        If mt.Success Then
                            With mt
                                With .Groups("SpawnID")
                                    If .Success Then
                                        strRet = "'Address: " & Hex((lastmatch.Index + 2 * arraytovalue(StringtoArray(.Value)) + mt.Index) \ 2) & vbCrLf
                                        ReDim sa(.Length)
                                        sa = StringtoArray(.Value)
                                        strRet &= "SeekSpawnID = " & "&H" & Hex(ArrayToValue(sa)) & "'*" & vbCrLf
                                    End If
                                End With
                            End With
                        Else
                            strRet &= "SeekSpawnID = NULL" & "'*" & vbCrLf
                        End If
                    End If
                End With
                With .Groups("AddressToNPC")
                    If .Success Then
                        ' get subroutine
                        s = "83FB046689(?:(9A(?<NPC>.{8}))|(5A(?<NPC>.{2})))75"
                        's = "83FB046689(?:9A|5A)(?<NPC>.{8})75"

                        'to get the address of the call you need to add the offset value to the address of the call + 5 (size of a call)
                        Dim address As Long = (lastaddress + .Index \ 2 + addressoffset(stringtoarray(.Value)) - 1) And &HFFFFFF
                        address -= &H400000
                        address *= 2

                        mt = re.Match(t.Substring(address, 1000), s, System.Text.RegularExpressions.RegexOptions.Singleline)
                        If mt.Success Then
                            With mt
                                With .Groups("NPC")
                                    If .Success Then
                                        strRet &= "'Address: " & Hex((lastmatch.Index + 2 * arraytovalue(StringtoArray(.Value)) + mt.Index) \ 2) & vbCrLf
                                        ReDim sa(.Length)
                                        sa = StringtoArray(.Value)
                                        strRet &= "SeekNPC = " & "&H" & Hex(ArrayToValue(sa)) & "'*" & vbCrLf
                                    End If
                                End With
                            End With
                        Else
                            strRet &= "SeekNPC = NULL" & "'*" & vbCrLf
                        End If
                    End If
                End With
            End With
        Else
            strRet &= "SeekSpawnID = NULL" & "'*" & vbCrLf
            strRet &= "SeekNPC = NULL" & "'*" & vbCrLf
        End If
        Return strRet

    End Function
    Protected Overrides Function GroundTarget() As String
        Dim strRet As String

        ' GROUNDTARGET - TOA and SI
        's = "8B....56A3(?<GroundTarget>.{8})8B...."
        's = "A3(?<GroundTarget>.{8})8B45..(?:D9|46)"
        's = "D905A46A8900D905(?<GroundTarget>.{8})DAE9DFE0"
        're = New System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.Singleline)

        Dim address2 As String = OpcodeSearch(&HDF)
        Dim address As Long = (Val(address2) - &H400000) * 2
        Dim mt2 As Match = Regex.Match(t.Substring(address, 1000), "E8.{8}.*?E8(?<Address>.{8})", RegexOptions.Compiled And RegexOptions.Singleline)
        If mt2.Success Then
            address = (Val(address2) + mt2.Index \ 2 + addressoffset(stringtoarray(mt2.Groups("Address").Value)) - 1) And &HFFFFFF
            address -= &H400000
            address *= 2

            Dim mt As Match = Regex.Match(t.Substring(address, 4000), "3D00100000.*?D91D(?<GroundTarget>.{8})", System.Text.RegularExpressions.RegexOptions.Singleline)
            If mt.Success Then
                With mt
                    strRet = "'Address: " & Hex(mt.Index \ 2) & vbCrLf
                    With .Groups("GroundTarget")
                        If .Success Then
                            ReDim sa(.Length)
                            sa = StringtoArray(.Value)
                            strRet &= "GroundTargetAddress = " & "&H" & Hex(ArrayToValue(sa)) & "'*" & vbCrLf
                        End If
                    End With
                End With
            Else
                strRet &= "GroundTargetAddress = NULL" & "'*" & vbCrLf
            End If
        Else
            strRet &= "GroundTargetAddress = NULL" & "'*" & vbCrLf
        End If

        Return strRet

    End Function
    Protected Overrides Function NextIndex() As String
        Dim strRet As String

        ' NextIndex - TOA and SI  
        s = "5633F63935(?<Index>.{8})7E"
        re = New System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.Singleline)

        mt = re.Match(t, s, System.Text.RegularExpressions.RegexOptions.Singleline)

        If mt.Success Then
            With mt
                strRet &= "'Address: " & Hex(mt.Index \ 2) & vbCrLf
                With .Groups("Index")
                    If .Success Then
                        ReDim sa(.Length)
                        sa = StringtoArray(.Value)
                        strRet &= "NextIndex =  " & "&H" & Hex(ArrayToValue(sa)) & vbCrLf
                    End If
                End With
            End With
        Else
            strRet &= "NextIndex =  NULL" & vbCrLf
        End If

        Return strRet

    End Function
    Protected Overrides Function TargetIndexAddress() As String
        Dim strRet As String

        'short ID2Index[] = "39.{2}F000000074.{14}E8.{8}8BF0"

        's = "538BD8A1(?<TargetIndex>.{8})56E8.{8}33F684C0"

        s = "558BECA1(?<TargetIndex>.{8})56FF750CE8.{8}33F684C0"



        's = "5959E8........A1(?<TargetIndex>.{8})E8.{8}668B.8(?<SeekNPC>.+)6683F90274..6683F90474..663BCF0F.{10}83.{6}0F.{10}39.{4}0F.{10}8B.0(?<SpawnID>(?:.{2}|.{8}))33" ' "668986(?<SeekNPC>.+)74"
        's = "E8.{8}6A00FF35(?<TargetIndex>.{8})"
        re = New System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.Singleline)

        mt = re.Match(t, s, System.Text.RegularExpressions.RegexOptions.Singleline)
        If mt.Success Then
            With mt
                strRet = "'Address: " & Hex(mt.Index \ 2) & vbCrLf
                With .Groups("TargetIndex")
                    If .Success Then
                        ReDim sa(.Length)
                        sa = StringtoArray(.Value)
                        strRet &= "TargetIndexAddress = " & "&H" & Hex(ArrayToValue(sa)) & "'address of character index subtract 1 to it. Represents how many arrays to get to player struct*" & vbCrLf
                    End If
                End With
            End With
        Else
            strRet &= "TargetIndexAddress = NULL" & "'address of character index subtract 1 to it. Represents how many arrays to get to player struct*" & vbCrLf
        End If

        Return strRet

    End Function
    Protected Overrides Function SeeknewMob() As String
        Dim strRet As String

        'SeekCasting - TOA and SI
        's = "8BF0596683BE(?<OKToCast>.{8})FF0F"
        're = New System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.Singleline)
        'Dim x As System.IO.StreamWriter = New System.IO.StreamWriter("c:\game.txt")
        'x.Write(t)
        'x.Close()

        'go through each match till the second match is found, first match will be newmob

        '                                                   any char, 1 or more, few as possible
        Dim mt As Match = re.Match(t, "558BEC(?:83EC68|6A6858|83EC6C).+?5F5E5BC9C3", System.Text.RegularExpressions.RegexOptions.Singleline And RegexOptions.Compiled)
        '558BEC83EC6853568BF0
        While Not mt Is Nothing
            If mt.Success Then
                With mt
                    Debug.WriteLine(&H400000 + .Index \ 2)
                    'either EDI or ESI since 1.77
                    If mt.Length < 10000 AndAlso (System.Text.RegularExpressions.Regex.IsMatch(.Value, ".*F6471620.*", RegexOptions.Compiled And RegexOptions.Singleline) OrElse System.Text.RegularExpressions.Regex.IsMatch(.Value, ".*F6461620.*", RegexOptions.Compiled And RegexOptions.Singleline)) Then
                        strRet &= "'NewMob Address = " & Hex(&H400000 + mt.Index \ 2) & "'*" & vbCrLf
                        Dim a() As Byte = stringtoarray(mt.Value)
                        Dim f As System.IO.FileStream = New System.IO.FileStream(ClientFolder & "\newobject.obj", IO.FileMode.Create)
                        f.Write(a, 0, a.Length)
                        f.Close()
                        lastaddress = &H400000 + mt.Index \ 2
                        lastmatch = mt
                        Exit While
                    End If
                End With
                mt = mt.NextMatch
            Else
                strRet &= "NewMob Address Not Found!" & "'*" & vbCrLf
                Exit While
            End If
        End While

        Return strRet

    End Function
    Protected Overrides Function SeekUpdateMob() As String
        Dim strRet As String

        'SeekCasting - TOA and SI
        's = "8BF0596683BE(?<OKToCast>.{8})FF0F"

        Dim mt As Match = re.Match(t, "558BEC(?:83EC14|6A1058).*?5E5BC9C3", System.Text.RegularExpressions.RegexOptions.Singleline And RegexOptions.Compiled)
        While Not mt Is Nothing
            If mt.Success Then
                With mt
                    If mt.Length < 4000 AndAlso System.Text.RegularExpressions.Regex.IsMatch(.Value, ".*F6471520.*", RegexOptions.Compiled And RegexOptions.Singleline) Then
                        strRet &= "'UpdateMob Address = 004" & LeftFillWithZeros(Hex(mt.Index \ 2)) & "'*" & vbCrLf
                        Dim a() As Byte = stringtoarray(mt.Value)
                        Dim f As System.IO.FileStream = New System.IO.FileStream(ClientFolder & "\updateobject.obj", IO.FileMode.Create)
                        f.Write(a, 0, a.Length)
                        f.Close()
                        Exit While
                    End If
                    mt = mt.NextMatch
                End With
            Else
                strRet &= "UpdateMob Address Not Found!" & "'*" & vbCrLf
                Exit While
            End If
        End While

        Return strRet

    End Function
    Protected Overrides Function SeekLevel() As String
        Dim strRet As String

        ' LEVEL - TOA and SI
        's = "83E07F41..C0(?<LevelParam1>.{2})..C0(?<LevelParam2>.{2})35(?<LevelParam3>.{8})89(..)(?<Level>(?:.{2}|.{8}))F647"
        's = "FF7714E886F7FFFF(?<Type>(?:6BC0|C1E0))(?<LevelParam1>.{2})83C0(?<LevelParam2>.{2})83C42435(?<LevelParam3>.{8})8983(?<Level>(?:.{2}|.{8}))5F5E5BC9C20400"
        's = "83C0(?<LevelParam1>.{2})6BC0(?<LevelParam2>.{2})35(?<LevelParam3>.{8})89.6(?<Level>(?:.{2}|.{8}))8A"
        s = "33DB4383E07F83C0(?<LevelParam2>.{2})6BC0(?<LevelParam3>.{2})35(?<LevelParam1>.{8})89.6(?<Level>(?:.{2}|.{8}))8A"
        re = New System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.Singleline)

        mt = re.Match(t, s, System.Text.RegularExpressions.RegexOptions.Singleline)
        If mt.Success Then
            With mt
                strRet = "'Address: " & Hex(mt.Index \ 2) & vbCrLf
                With .Groups("LevelParam1")
                    If .Success Then
                        ReDim sa(.Length)
                        sa = StringtoArray(.Value)
                        strRet &= "LevelEnc1 = " & "&H" & Hex(ArrayToValue(sa)) & vbCrLf
                    End If
                End With
                With .Groups("LevelParam2")
                    If .Success Then
                        ReDim sa(.Length)
                        sa = StringtoArray(.Value)
                        strRet &= "LevelEnc2 =  " & "&H" & Hex(ArrayToValue(sa)) & " ' add/sub" & vbCrLf
                    End If
                End With
                With .Groups("LevelParam3")
                    If .Success Then
                        ReDim sa(.Length)
                        sa = StringtoArray(.Value)
                        strRet &= "LevelEnc3 =  " & "&H" & Hex(ArrayToValue(sa)) & " ' mul/div" & vbCrLf
                    End If
                End With
                With .Groups("Level")
                    If .Success Then
                        ReDim sa(.Length)
                        sa = StringtoArray(.Value)
                        strRet &= "SeekLevel =  " & ArrayToValue(sa) & vbCrLf
                    End If
                End With
            End With
        Else
            ' LEVEL ToA and SI
            's = "8B83(?<Level>(?:.{8}|.{2}))6A(?<LevelParam1>.{2})35(?<LevelParam3>.{8})59F7F183E8(?<LevelParam2>.{2})508D"
            s = "(?:(?<Multi5>8D0480)|)8D04(?<Multi>.5)(?<Add>.{8})35(?<LevelParam1>.{8})89.6(?<Level>(?:.{2}|.{8}))(?:8A|66)"

            re = New System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.Singleline)

            mt = re.Match(t, s, System.Text.RegularExpressions.RegexOptions.Singleline)
            If mt.Success Then
                With mt
                    strRet &= "'Address: " & Hex(mt.Index \ 2) & vbCrLf
                    With .Groups("LevelParam1")
                        If .Success Then
                            ReDim sa(.Length)
                            sa = StringtoArray(.Value)
                            strRet &= "LevelEnc1 = " & "&H" & Hex(ArrayToValue(sa)) & vbCrLf
                        End If
                    End With
                    With .Groups("Add")
                        If .Success Then
                            ReDim sa(.Length)
                            sa = StringtoArray(.Value)
                            strRet &= "LevelEnc2 =  " & "&H" & Hex(ArrayToValue(sa)) & " ' sub/add" & vbCrLf
                        End If
                    End With
                    Dim Multi As Integer = 1
                    With .Groups("Multi5")
                        If .Success Then
                            Multi = 5
                        End If
                    End With
                    With .Groups("Multi")
                        If .Success Then
                            If .Value = "85" Then
                                Multi *= 4
                            ElseIf .Value = "C5" Then
                                Multi *= 8
                            End If
                        End If
                    End With
                    If Multi <> 1 Then
                        strRet &= "LevelEnc3 =  " & "&H" & Hex(Multi) & " ' mul/div" & vbCrLf
                    End If
                    With .Groups("Level")
                        If .Success Then
                            ReDim sa(.Length)
                            sa = StringtoArray(.Value)
                            strRet &= "SeekLevel =  " & ArrayToValue(sa) & vbCrLf
                        End If
                    End With
                End With

            Else
                ' LEVEL ToA and SI
                s = "508B.7(?<Level>(?:.{8}|.{2}))35(?<LevelParam3>.{8})(?:D(?<LevelParam4>.{1})E8|C1E8(?<LevelParam2>.{2}))83E8(?<LevelParam1>.{2})50"
                re = New System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.Singleline)

                mt = re.Match(t, s, System.Text.RegularExpressions.RegexOptions.Singleline)
                If mt.Success Then
                    With mt
                        strRet &= "'Address: " & Hex(mt.Index \ 2) & vbCrLf
                        With .Groups("LevelParam2")
                            If .Success Then
                                ReDim sa(.Length)
                                sa = StringtoArray(.Value)
                                strRet &= "LevelEnc2 = &H" & Hex(2 ^ CInt("&H" & Hex(ArrayToValue(sa)))) & vbCrLf
                            Else
                                With mt.Groups("LevelParam4")
                                    If .Success Then
                                        ReDim sa(.Length)
                                        sa = StringtoArray(.Value)
                                        strRet &= "LevelEnc2 = &H" & Hex(2 ^ CInt("&H" & Hex(ArrayToValue(sa)))) & vbCrLf
                                    End If
                                End With
                            End If
                        End With
                        With .Groups("LevelParam1")
                            If .Success Then
                                ReDim sa(.Length)
                                sa = StringtoArray(.Value)
                                strRet &= "LevelEnc3 =  " & "&H" & Hex(ArrayToValue(sa)) & vbCrLf
                            End If
                        End With
                        With .Groups("LevelParam3")
                            If .Success Then
                                ReDim sa(.Length)
                                sa = StringtoArray(.Value)
                                strRet &= "LevelEnc1 =  " & "&H" & Hex(ArrayToValue(sa)) & vbCrLf
                            End If
                        End With
                        With .Groups("Level")
                            If .Success Then
                                ReDim sa(.Length)
                                sa = StringtoArray(.Value)
                                strRet &= "SeekLevel =  " & ArrayToValue(sa) & vbCrLf
                            End If
                        End With
                    End With
                Else
                    strRet &= "LevelEnc3 =  NULL" & vbCrLf
                    strRet &= "LevelEnc2 = NULL" & vbCrLf
                    strRet &= "LevelEnc1 =  NULL" & vbCrLf
                    strRet &= "SeekLevel =  NULL" & vbCrLf
                End If
            End If
        End If

        Return strRet

    End Function
    Protected Overrides Function SeekHealth() As String
        Dim strRet As String

        ' HEALTH ToA and SI
        's = "0FB6..83C1(?<HealthParam1>.{2})6BC9(?<HealthParam2>.{2})81F1(?<HealthParam3>.{8})89.8(?<Health>(?:.{2}|.{8}))" '668B"
        's = "8D86(?<Health>(?:.{2}|.{8}))8B1081F2(?<HealthParam3>.{8})(?<Type>(?:D1EA|C1EA))(?:(?<HealthParam1>.{2})|)83EA(?<HealthParam2>.{2})"
        's = "0FB60E6BC9(?<HealthParam3>.{2})83C1(?<HealthParam2>.{2})81F1(?<HealthParam1>.{8})89.8(?<Health>(?:.{2}|.{8}))66"
        's = "8B..(?<Health>(?:.{2}|.{8}))81F1(?<HealthParam1>.{8})C1E9(?<HealthParam3>.{2})83E9(?<HealthParam2>.{2})3BC8"
        ' cata
        s = "0FB60E6BC9(?<HealthParam3>.{2})83C1(?<HealthParam2>.{2})81F1(?<HealthParam1>.{8})89..(?<Health>(?:.{2}|.{8}))66"
        re = New System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.Singleline)

        mt = re.Match(t, s, System.Text.RegularExpressions.RegexOptions.Singleline)
        If mt.Success Then
            With mt
                strRet = "'Address: " & Hex(mt.Index \ 2) & vbCrLf
                With .Groups("HealthParam1")
                    If .Success Then
                        ReDim sa(.Length)
                        sa = StringtoArray(.Value)
                        strRet &= "HealthEnc1 =  &H" & Hex(ArrayToValue(sa)) & vbCrLf
                    End If
                End With
                With .Groups("HealthParam2")
                    If .Success Then
                        ReDim sa(.Length)
                        sa = StringtoArray(.Value)
                        strRet &= "HealthEnc2 = " & "&H" & Hex(ArrayToValue(sa)) & " ' add/sub" & vbCrLf
                    End If
                End With
                With .Groups("HealthParam3")
                    If .Success Then
                        ReDim sa(.Length)
                        sa = StringtoArray(.Value)
                        strRet &= "HealthEnc3 =  " & "&H" & Hex(ArrayToValue(sa)) & " ' div/mul" & vbCrLf
                    End If
                End With
                With .Groups("Health")
                    If .Success Then
                        ReDim sa(.Length)
                        sa = StringtoArray(.Value)
                        strRet &= "SeekHealth =  " & ArrayToValue(sa) & vbCrLf
                    End If
                End With
            End With
        Else
            ' HEALTH ToA and SI
            's = "8B.7(?<Health>(?:.{8}|.{2}))6A(?<HealthParam1>.{2})35(?<HealthParam3>.{8})33D2..F7F12C(?<HealthParam2>.{2})247F"
            's = "0FB60E83C1(?<HealthParam3>.{2})6BC9(?<HealthParam2>.{2})81F1(?<HealthParam1>.{8})89.8(?<Health>(?:.{2}|.{8}))66"
            s = "8BD88B.3(?<Health>(?:.{2}|.{8}))35(?<HealthParam1>.{8})C1E.(?<HealthParam3>.{2})83E8(?<HealthParam2>.{2})"
            re = New System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.Singleline)

            'Xor HealthEnc1) / HealthEnc2) - HealthEnc3) And &H7F
            'HealthEnc1 = xor
            'HealthEnc2 = sub
            'HealthEnc3 = and

            mt = re.Match(t, s, System.Text.RegularExpressions.RegexOptions.Singleline)
            If mt.Success Then
                With mt
                    strRet = "'Address: " & Hex(mt.Index \ 2) & vbCrLf
                    With .Groups("HealthParam1")
                        If .Success Then
                            ReDim sa(.Length)
                            sa = StringtoArray(.Value)
                            strRet &= "HealthEnc1 =  &H" & Hex(ArrayToValue(sa)) & vbCrLf
                        End If
                    End With
                    With .Groups("HealthParam2")
                        If .Success Then
                            ReDim sa(.Length)
                            sa = StringtoArray(.Value)
                            strRet &= "HealthEnc2 = " & "&H" & Hex(ArrayToValue(sa)) & " ' add/sub" & vbCrLf
                        End If
                    End With
                    With .Groups("HealthParam3")
                        If .Success Then
                            ReDim sa(.Length)
                            sa = StringtoArray(.Value)
                            strRet &= "HealthEnc3 =  " & "&H" & Hex(2 ^ CInt("&H" & Hex(ArrayToValue(sa)))) & " ' div/mul" & vbCrLf
                        End If
                    End With
                    With .Groups("Health")
                        If .Success Then
                            ReDim sa(.Length)
                            sa = StringtoArray(.Value)
                            strRet &= "SeekHealth =  " & ArrayToValue(sa) & vbCrLf
                        End If
                    End With
                End With
            Else
                ' HEALTH ToA and SI
                s = "8BDF6A(?<HealthParam1>.{2})F7FB33D28941(?<Health>(?:.{8}|.{2}))8B45005935(?<HealthParam3>.{8})F7F16A(?<HealthParam2>.{2})8BD0"
                re = New System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.Singleline)

                mt = re.Match(t, s, System.Text.RegularExpressions.RegexOptions.Singleline)
                If mt.Success Then
                    With mt
                        strRet &= "'Address: " & Hex(mt.Index \ 2) & vbCrLf
                        With .Groups("HealthParam1")
                            If .Success Then
                                ReDim sa(.Length)
                                sa = StringtoArray(.Value)
                                strRet &= "HealthEnc3 =  " & "&H" & Hex(ArrayToValue(sa)) & vbCrLf
                            End If
                        End With
                        With .Groups("HealthParam2")
                            If .Success Then
                                ReDim sa(.Length)
                                sa = StringtoArray(.Value)
                                strRet &= "HealthEnc2 = " & "&H" & Hex(ArrayToValue(sa)) & vbCrLf
                            End If
                        End With
                        With .Groups("HealthParam3")
                            If .Success Then
                                ReDim sa(.Length)
                                sa = StringtoArray(.Value)
                                strRet &= "HealthEnc1 =  " & "&H" & Hex(ArrayToValue(sa)) & vbCrLf
                            End If
                        End With
                        With .Groups("Health")
                            If .Success Then
                                ReDim sa(.Length)
                                sa = StringtoArray(.Value)
                                strRet &= "SeekHealth =  " & ArrayToValue(sa) & vbCrLf
                            End If
                        End With
                    End With
                Else
                    ' HEALTH ToA and SI
                    s = "6BC0556A645F998BDF6A(?<HealthParam1>.{2})F7FB33D28941..8B(?:.{6}|.{4})59(?:8B.{10}|)35(?<HealthParam3>.{8})F7F18BD083EA(?<HealthParam2>.{2})"
                    re = New System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.Singleline)

                    mt = re.Match(t, s, System.Text.RegularExpressions.RegexOptions.Singleline)
                    If mt.Success Then
                        With mt
                            strRet &= "'Address: " & Hex(mt.Index \ 2) & vbCrLf
                            With .Groups("HealthParam1")
                                If .Success Then
                                    ReDim sa(.Length)
                                    sa = StringtoArray(.Value)
                                    strRet &= "HealthEnc3 =  " & "&H" & Hex(ArrayToValue(sa)) & vbCrLf
                                End If
                            End With
                            With .Groups("HealthParam2")
                                If .Success Then
                                    ReDim sa(.Length)
                                    sa = StringtoArray(.Value)
                                    strRet &= "HealthEnc2 = " & "&H" & Hex(ArrayToValue(sa)) & vbCrLf
                                End If
                            End With
                            With .Groups("HealthParam3")
                                If .Success Then
                                    ReDim sa(.Length)
                                    sa = StringtoArray(.Value)
                                    strRet &= "HealthEnc1 =  " & "&H" & Hex(ArrayToValue(sa)) & vbCrLf
                                End If
                            End With
                            With .Groups("Health")
                                If .Success Then
                                    ReDim sa(.Length)
                                    sa = StringtoArray(.Value)
                                    strRet &= "SeekHealth =  " & ArrayToValue(sa) & vbCrLf
                                End If
                            End With
                        End With
                    Else

                        ' HEALTH ToA and SI
                        s = "8B.7(?<Health>(?:.{8}|.{2}))6A(?<HealthParam1>.{2})35(?<HealthParam3>.{8})33D283E8(?<HealthParam2>.{2})5BF7"
                        re = New System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.Singleline)

                        'Xor HealthEnc1) / HealthEnc2) - HealthEnc3) And &H7F
                        'HealthEnc1 = xor
                        'HealthEnc2 = sub
                        'HealthEnc3 = and

                        mt = re.Match(t, s, System.Text.RegularExpressions.RegexOptions.Singleline)
                        If mt.Success Then
                            With mt
                                strRet &= "'Address: " & Hex(mt.Index \ 2) & vbCrLf
                                With .Groups("HealthParam1") 'sub
                                    If .Success Then
                                        ReDim sa(.Length)
                                        sa = StringtoArray(.Value)
                                        strRet &= "HealthEnc2 =  " & "&H" & Hex(ArrayToValue(sa)) & vbCrLf
                                    End If
                                End With
                                With .Groups("HealthParam3") 'xor
                                    If .Success Then
                                        ReDim sa(.Length)
                                        sa = StringtoArray(.Value)
                                        strRet &= "HealthEnc1 = " & "&H" & Hex(ArrayToValue(sa)) & vbCrLf
                                    End If
                                End With
                                With .Groups("HealthParam2") 'and
                                    If .Success Then
                                        ReDim sa(.Length)
                                        sa = StringtoArray(.Value)
                                        strRet &= "HealthEnc3 =  " & "&H" & Hex(ArrayToValue(sa)) & vbCrLf
                                    End If
                                End With
                                With .Groups("Health")
                                    If .Success Then
                                        ReDim sa(.Length)
                                        sa = StringtoArray(.Value)
                                        strRet &= "SeekHealth =  " & ArrayToValue(sa) & vbCrLf
                                    End If
                                End With
                            End With
                        Else
                            ' HEALTH ToA and SI
                            's = "6A..8B83(?<Health>(?:.{8}|.{2}))35(?<HealthParam3>.{8})(?:D(?<HealthParam4>.{1})E8|C1E8(?<HealthParam2>.{2}))83E8(?<HealthParam1>.{2})89"
                            s = "590FB6..83C1(?<HealthParam1>.{2})(?:D(?<HealthParam4>.{1})E1|C1E1(?<HealthParam2>.{2}))81F1(?<HealthParam3>.{8})8988(?<Health>(?:.{8}|.{2}))66"
                            re = New System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.Singleline)

                            'Xor HealthEnc1) / HealthEnc2) - HealthEnc3) And &H7F
                            'HealthEnc1 = xor
                            'HealthEnc2 = sub
                            'HealthEnc3 = and

                            mt = re.Match(t, s, System.Text.RegularExpressions.RegexOptions.Singleline)
                            If mt.Success Then
                                With mt
                                    strRet &= "'Address: " & Hex(mt.Index \ 2) & vbCrLf
                                    With .Groups("HealthParam1") 'sub
                                        If .Success Then
                                            ReDim sa(.Length)
                                            sa = StringtoArray(.Value)
                                            strRet &= "HealthEnc3 =  " & "&H" & Hex(ArrayToValue(sa)) & vbCrLf
                                        End If
                                    End With
                                    With .Groups("HealthParam3") 'xor
                                        If .Success Then
                                            ReDim sa(.Length)
                                            sa = StringtoArray(.Value)
                                            strRet &= "HealthEnc1 = " & "&H" & Hex(ArrayToValue(sa)) & vbCrLf
                                        End If
                                    End With
                                    With .Groups("HealthParam2")
                                        If .Success Then
                                            ReDim sa(.Length)
                                            sa = StringtoArray(.Value)
                                            strRet &= "HealthEnc2 = &H" & Hex(2 ^ CInt("&H" & Hex(ArrayToValue(sa)))) & vbCrLf
                                        Else
                                            With mt.Groups("HealthParam4")
                                                If .Success Then
                                                    ReDim sa(.Length)
                                                    sa = StringtoArray(.Value)
                                                    strRet &= "HealthEnc2 = &H" & Hex(2 ^ CInt("&H" & Hex(ArrayToValue(sa)))) & vbCrLf
                                                End If
                                            End With
                                        End If
                                    End With
                                    With .Groups("Health")
                                        If .Success Then
                                            ReDim sa(.Length)
                                            sa = StringtoArray(.Value)
                                            strRet &= "SeekHealth =  " & ArrayToValue(sa) & vbCrLf
                                        End If
                                    End With
                                End With
                            Else
                                ' HEALTH ToA and SI
                                s = "6A..8B83(?<Health>(?:.{8}|.{2}))35(?<HealthParam3>.{8})(?:D(?<HealthParam4>.{1})E8|C1E8(?<HealthParam2>.{2}))83E8(?<HealthParam1>.{2})89"
                                's = "590FB6..83C1(?<HealthParam1>.{2})(?:D(?<HealthParam4>.{1})E1|C1E1(?<HealthParam2>.{2}))81F1(?<HealthParam3>.{8})8988(?<Health>(?:.{8}|.{2}))66"
                                re = New System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.Singleline)

                                'Xor HealthEnc1) / HealthEnc2) - HealthEnc3) And &H7F
                                'HealthEnc1 = xor
                                'HealthEnc2 = sub
                                'HealthEnc3 = and

                                mt = re.Match(t, s, System.Text.RegularExpressions.RegexOptions.Singleline)
                                If mt.Success Then
                                    With mt
                                        strRet &= "'Address: " & Hex(mt.Index \ 2) & vbCrLf
                                        With .Groups("HealthParam1") 'sub
                                            If .Success Then
                                                ReDim sa(.Length)
                                                sa = StringtoArray(.Value)
                                                strRet &= "HealthEnc3 =  " & "&H" & Hex(ArrayToValue(sa)) & vbCrLf
                                            End If
                                        End With
                                        With .Groups("HealthParam3") 'xor
                                            If .Success Then
                                                ReDim sa(.Length)
                                                sa = StringtoArray(.Value)
                                                strRet &= "HealthEnc1 = " & "&H" & Hex(ArrayToValue(sa)) & vbCrLf
                                            End If
                                        End With
                                        With .Groups("HealthParam2")
                                            If .Success Then
                                                ReDim sa(.Length)
                                                sa = StringtoArray(.Value)
                                                strRet &= "HealthEnc2 = &H" & Hex(2 ^ CInt("&H" & Hex(ArrayToValue(sa)))) & vbCrLf
                                            Else
                                                With mt.Groups("HealthParam4")
                                                    If .Success Then
                                                        ReDim sa(.Length)
                                                        sa = StringtoArray(.Value)
                                                        strRet &= "HealthEnc2 = &H" & Hex(2 ^ CInt("&H" & Hex(ArrayToValue(sa)))) & vbCrLf
                                                    End If
                                                End With
                                            End If
                                        End With
                                        With .Groups("Health")
                                            If .Success Then
                                                ReDim sa(.Length)
                                                sa = StringtoArray(.Value)
                                                strRet &= "SeekHealth =  " & ArrayToValue(sa) & vbCrLf
                                            End If
                                        End With
                                    End With
                                Else
                                    strRet &= "HealthEnc3 =  NULL" & vbCrLf
                                    strRet &= "HealthEnc2 =  NULL" & vbCrLf
                                    strRet &= "HealthEnc1 =  NULL" & vbCrLf
                                    strRet &= "SeekHealth =  NULL" & vbCrLf
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If

        Return strRet

    End Function
    Protected Function OpcodeSearch(ByVal mOpCode As Byte) As String
        Dim s() As Integer = FindBaseAddress()

        Dim result As Integer = mOpCode + &HFFFFFFFE 'overflow

        Dim FindByteAddress As Integer = s(0) + result 'find the byte here

        FindByteAddress -= &H400000
        FindByteAddress *= 2

        Dim findbyte As String = t.Substring(FindByteAddress, 2)

        Dim Temp() As Byte = StringtoArray(findbyte)

        result = ArrayToValue(Temp) * 4

        result = result + s(1)

        result -= &H400000
        result *= 2

        Dim a As String = t.Substring(result, 8)
        Temp = StringtoArray(a)

        'maybe just return this
        Dim address1 As Integer = ArrayToValue(Temp) 'address to the switch statement

        Return "&H" & Hex(address1)
        'address1 -= &H400000
        'address1 *= 2

        'a = t.Substring(address1 + 22, 8)
        'Temp = StringtoArray(a)

        'Dim finaladdress As Integer = address1 + 21 'move to the call address
        'finaladdress /= 2
        'finaladdress += &H400000

        ''THIS IS offset need to add the address + 5
        'Return finaladdress + ArrayToValue(Temp) + 5


    End Function
    Private Function FindBaseAddress() As Integer()
        Dim ret(1) As Integer

        s = "0FB6450883C0FE3DF9000000..................(?<Address>.{8})......(?<Address2>.{8})"
        re = New System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.Singleline)

        mt = re.Match(t, s, System.Text.RegularExpressions.RegexOptions.Singleline)
        If mt.Success Then
            With mt
                With .Groups("Address")
                    If .Success Then
                        ReDim sa(.Length)
                        sa = StringtoArray(.Value)
                        ret(0) = CInt(ArrayToValue(sa))
                    End If
                End With
                With .Groups("Address2")
                    If .Success Then
                        ReDim sa(.Length)
                        sa = StringtoArray(.Value)
                        ret(1) = CInt(ArrayToValue(sa))
                    End If
                End With
            End With
        Else
            Return Nothing
        End If

        Return ret

    End Function
    Protected Overrides Function LocalPlayerInfo() As String
        Dim strRet As String

        '       short GetLocalPlayerInfo[] = {0x00A3, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x00D9, 0x01FF, 0x01FF, 0x01FF, 
        '0x01FF, 0x01FF, 0x00D9, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x00D9, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x01FF, 0x00E8};

        s = "A3(?<LocalPlayerInfo>.{8})D9.{10}D9.{10}D9.{10}E8"
        re = New System.Text.RegularExpressions.Regex(s, System.Text.RegularExpressions.RegexOptions.Singleline)

        mt = re.Match(t, s, System.Text.RegularExpressions.RegexOptions.Singleline)
        If mt.Success Then
            With mt
                strRet = "'Address: " & Hex(mt.Index \ 2) & vbCrLf

                With .Groups("LocalPlayerInfo")
                    If .Success Then
                        ReDim sa(.Length)
                        sa = StringtoArray(.Value)
                        strRet &= "LocalPlayerInfo = &H" & Hex(ArrayToValue(sa)) & vbCrLf
                    End If
                End With
            End With
        Else
            strRet &= "LocalPlayerInfo = NULL" & vbCrLf
        End If

        Return strRet

    End Function
End Class
