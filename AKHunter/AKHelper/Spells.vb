Option Explicit On 

Friend Class Spells
    Inherits Base

    Private _Spells As Hashtable

    Public Sub New()
        _Spells = New Hashtable
    End Sub

    Public Sub Cast(ByVal Daoc As AutoKillerScript.clsAutoKillerScript, _
                    ByVal PlayerName As String, _
                    ByVal TargetPlayerNameList As String, _
                    ByVal SpellList As String, _
                    Optional ByVal SetGroundTarget As Boolean = False)

        Dim spell As New Spell

        Dim parsedSpellList As Collection
        Dim parsedSpell As String

        Dim parsedTargetPlayerNameList As Collection
        Dim parsedTargetPlayerName As String

        Try
            'parse spell list
            parsedSpellList = ParseWords(SpellList)

            For Each parsedSpell In parsedSpellList
                If _Spells.ContainsKey(parsedSpell) Then
                    spell = _Spells(parsedSpell)

                    If TargetPlayerNameList = "" Then
                        spell.Cast(Daoc, PlayerName, "", SetGroundTarget)
                    Else
                        parsedTargetPlayerNameList = ParseWords(TargetPlayerNameList)

                        For Each parsedTargetPlayerName In parsedTargetPlayerNameList
                            spell.Cast(Daoc, PlayerName, Trim(parsedTargetPlayerName), SetGroundTarget)
                        Next
                    End If
                End If
            Next

        Catch ex As Exception
            Log(ex.Message)
        End Try

    End Sub

    Private Function ParseWords(ByVal ParseString As String) As Collection

        Dim strWord As String

        Dim objWords As New Collection

        Dim intCount As Integer
        Dim strTempName As String
        Dim strChar As String
        Dim intLen As Integer
        Dim intNameCount As Integer
        Dim blnIsSpace As Boolean

        strWord = Trim(ParseString)

        intLen = Len(ParseString)

        intNameCount = 0

        For intCount = 1 To intLen
            strChar = Mid(strWord, intCount, 1)

            If strChar = " " And Not blnIsSpace Then
                blnIsSpace = True
                objWords.Add(strTempName)
                intNameCount = intNameCount + 1
                strTempName = ""
            Else
                If Not strChar = " " Then
                    blnIsSpace = False
                    strTempName = strTempName & strChar
                End If
            End If

            If intCount = intLen Then
                objWords.Add(strTempName)
                intNameCount = intNameCount + 1
                strTempName = ""
            End If
        Next

        ParseWords = objWords

    End Function

    Public Sub Load(ByVal FileName As String)

        Dim encoder As New System.Text.UTF8Encoding

        Dim xmlDoc As Xml.XmlDocument = New Xml.XmlDocument
        Dim xmlNode As Xml.XmlNode

        Dim spell As Spell

        Try
            xmlDoc.Load(FileName)

            Log("---- Loading Spells  ----")

            For Each xmlNode In xmlDoc.SelectNodes("//Profile/Spells/Spell")
                spell = New Spell

                spell.Name = xmlNode.SelectSingleNode("@Name").Value
                spell.Qbar = xmlNode.SelectSingleNode("@Qbar").Value
                spell.Key = encoder.GetBytes(xmlNode.SelectSingleNode("@Key").Value)(0)
                spell.CastTime = xmlNode.SelectSingleNode("@CastTime").Value
                spell.Target = xmlNode.SelectSingleNode("@Target").Value

                Me.AddSpell(spell)
                Log("Spell   Name:" & spell.Name & _
                    "  Qbar:" & spell.Qbar & _
                    "  Key:" & xmlNode.SelectSingleNode("@Key").Value & _
                    "  CastTime:" & spell.CastTime & _
                    "  Target:" & spell.Target)
            Next

            xmlDoc = Nothing

        Catch ex As Exception
            Log(ex.Message)
        End Try
    End Sub

    Private Sub AddSpell(ByVal Spell As Spell)

        _Spells.Add(Spell.Name, Spell)

    End Sub

End Class
