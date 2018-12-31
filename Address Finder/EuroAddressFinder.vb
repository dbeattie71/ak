Public Class EuroAddressFinder
    Inherits USAddressFinder
    Public Sub New(ByVal FileName As String)
        MyBase.New(FileName)
        Groupsizestruct = "&HFCC"
        OffsetFromMana = "- 4"
    End Sub
    Protected Overrides Function TargetIndexAddress() As String
        Dim strRet As String

        'short ID2Index[] = "39.{2}F000000074.{14}E8.{8}8BF0"

        s = "558BEC8B450C8B0D(?<TargetIndex>.{8})56E8.{8}33F684C0"

        's = "558BECA1(?<TargetIndex>.{8})56FF750CE8.{8}33F684C0"

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
End Class
