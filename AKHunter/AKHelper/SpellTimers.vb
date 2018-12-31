Friend Class SpellTimers
    Inherits Base

    Private _SpellTimers As Hashtable

    Public Sub New()

        _SpellTimers = New Hashtable

    End Sub

    Public Function GetSpellTimers() As Hashtable

        Return _SpellTimers

    End Function

    Public Sub ProcessTimers(ByVal sender As Object, _
                             ByVal e As System.Timers.ElapsedEventArgs)

        Try

            Dim spellTimer As SpellTimer = DirectCast(sender, SpellTimer)
            Log("Timer " & spellTimer.Name & " fired")
            spellTimer.Enabled = False
            spellTimer.Process = True

        Catch ex As Exception
            Log(ex.Message)
        End Try

    End Sub

    Public Sub Load(ByVal FileName As String)

        Dim xmlDoc As Xml.XmlDocument = New Xml.XmlDocument
        Dim xmlNode As Xml.XmlNode

        Dim spellTimer As SpellTimer

        Try
            xmlDoc.Load(FileName)

            Log("---- Loading SpellTimers data ----")
            For Each xmlNode In xmlDoc.SelectNodes("//Profile/SpellTimers/SpellTimer")
                spellTimer = New SpellTimer
                spellTimer.Name = xmlNode.SelectSingleNode("@Name").Value
                spellTimer.Interval = xmlNode.SelectSingleNode("@Interval").Value
                spellTimer.SpellList = xmlNode.SelectSingleNode("@SpellList").Value
                spellTimer.Stop()

                AddHandler spellTimer.Elapsed, AddressOf ProcessTimers

                Log("Name:" & spellTimer.Name _
                    & "  SpellList:" & spellTimer.SpellList _
                    & "  Process:" & spellTimer.Process _
                    & "  Interval:" & spellTimer.Interval)

                _SpellTimers.Add(spellTimer.Name, spellTimer)
            Next

            xmlDoc = Nothing

        Catch ex As Exception
            Log(ex.Message)
        End Try
    End Sub
End Class
