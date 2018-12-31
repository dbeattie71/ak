Option Explicit On 

Friend Class ChatLogLines
    Inherits Base

    Private _ChatLogLines As Collection

    Public Sub New()

        _ChatLogLines = New Collection

    End Sub

    Public Function AddStrings(ByVal Daoc As AutoKillerScript.clsAutoKillerScript, _
                               ByVal StartIndex As Integer, _
                               ByRef LogIndex As Hashtable) As Integer

        Dim chatLogLine As ChatLogLine
        Dim temp As String
        Dim count As Integer = StartIndex
        Dim x As Collection

        Try
            temp = ""

            For Each chatLogLine In _ChatLogLines
                'if say is true, add a string for it
                If chatLogLine.Say = True Then
                    temp = "@@(?<PlayerName>[A-Za-z]*)\ssays,\s(?<Message>""" & chatLogLine.RegEx & """)"

                    Log("-----")
                    Log("Say=True")
                    Daoc.AddString(count, temp)
                    Log("Daoc.AddString(" & chatLogLine.Name & ", " & temp & " )")

                    LogIndex.Add(count, chatLogLine.Name)
                    Log("_LogIndex(" & count & "): " & chatLogLine.Name)

                    count = count + 1
                End If

                'if group is true, add a string for it
                If chatLogLine.Group = True Then
                    temp = "@@\[Party\]\s(?<PlayerName>[A-Za-z]*):\s(?<Message>""" & chatLogLine.RegEx & """)"

                    Log("-----")
                    Log("Group=True")
                    Daoc.AddString(count, temp)
                    Log("Daoc.AddString(" & chatLogLine.Name & ", " & temp & " )")

                    LogIndex.Add(count, chatLogLine.Name)
                    Log("_LogIndex(" & count & "): " & chatLogLine.Name)

                    count = count + 1
                End If

                'if tell is true, add a string for it
                If chatLogLine.Tell = True Then
                    temp = "@@(?<PlayerName>[A-Za-z]*)\ssends,\s(?<Message>""" & chatLogLine.RegEx & """)"

                    Log("-----")
                    Log("Tell=True")
                    Daoc.AddString(count, temp)
                    Log("Daoc.AddString(" & chatLogLine.Name & ", " & temp & " )")

                    LogIndex.Add(count, chatLogLine.Name)
                    Log("_LogIndex(" & count & "): " & chatLogLine.Name)

                    count = count + 1
                End If

                'if chat is true, add a string for it
                If chatLogLine.Chat = True Then
                    temp = "@@\[Chat\]\s(?<PlayerName>[A-Za-z]*):\s(?<Message>""" & chatLogLine.RegEx & """)"

                    Log("-----")
                    Log("Chat=True")
                    Daoc.AddString(count, temp)
                    Log("DaocAddString(" & chatLogLine.Name & ", " & temp & " )")

                    LogIndex.Add(count, chatLogLine.Name)
                    Log("_LogIndex(" & count & "): " & chatLogLine.Name)

                    count = count + 1

                End If
            Next

            Return count

        Catch ex As Exception
            Log(ex.Message)
        End Try

    End Function

    Public Sub Load(ByVal FileName As String)

        Dim xmlDoc As Xml.XmlDocument = New Xml.XmlDocument
        Dim xmlNode As Xml.XmlNode

        Dim chatLogLine As ChatLogLine

        Try
            xmlDoc.Load(FileName)

            Log("---- Loading ChatLogLine data ----")
            For Each xmlNode In xmlDoc.SelectNodes("//Profile/ChatLogLines/ChatLogLine")
                chatLogLine = New ChatLogLine
                chatLogLine.Name = xmlNode.SelectSingleNode("@Name").Value
                chatLogLine.RegEx = xmlNode.SelectSingleNode("@RegEx").Value
                chatLogLine.Say = xmlNode.SelectSingleNode("@Say").Value
                chatLogLine.Group = xmlNode.SelectSingleNode("@Group").Value
                chatLogLine.Tell = xmlNode.SelectSingleNode("@Tell").Value
                chatLogLine.Chat = xmlNode.SelectSingleNode("@Chat").Value

                Log("Name:" & chatLogLine.Name _
                    & "  RegEx:" & chatLogLine.RegEx _
                    & "  Say:" & chatLogLine.Say _
                    & "  Group:" & chatLogLine.Group _
                    & "  Tell:" & chatLogLine.Tell _
                    & "  Chat:" & chatLogLine.Chat)

                _ChatLogLines.Add(chatLogLine)
            Next

            xmlDoc = Nothing

        Catch ex As Exception
            Log(ex.Message)
        End Try
    End Sub

End Class
