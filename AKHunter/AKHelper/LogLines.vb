Option Explicit On 

Friend Class LogLines
    Inherits Base

    Private _LogLines As Collection

    Public Sub New()

        _LogLines = New Collection

    End Sub

    Public Function AddStrings(ByVal Daoc As AutoKillerScript.clsAutoKillerScript, _
                               ByVal StartIndex As Integer, _
                               ByRef LogIndex As Hashtable) As Integer

        Dim logLine As LogLine
        Dim temp As String
        Dim count As Integer = StartIndex

        Try

            For Each logLine In _LogLines
                temp = logLine.RegEx

                Log("-----")
                Daoc.AddString(count, temp)
                Log("Daoc.AddString(" & count & ", " & temp & " )")

                LogIndex.Add(count, logLine.Name)
                Log("_LogIndex(" & count & "): " & logLine.Name)

                count = count + 1
            Next

        Catch ex As Exception
            Log(ex.Message)
        End Try

    End Function

    Public Sub Load(ByVal FileName As String)

        Dim xmlDoc As Xml.XmlDocument = New Xml.XmlDocument
        Dim xmlNode As Xml.XmlNode

        Dim logLine As LogLine

        Try
            xmlDoc.Load(FileName)

            Log("---- Loading LogLine data ----")
            For Each xmlNode In xmlDoc.SelectNodes("//Profile/LogLines/LogLine")
                logLine = New LogLine
                logLine.Name = xmlNode.SelectSingleNode("@Name").Value
                logLine.RegEx = xmlNode.SelectSingleNode("@RegEx").Value

                Log("Name:" & logLine.Name & "  RegEx:" & logLine.RegEx)

                _LogLines.Add(logLine)
            Next

            xmlDoc = Nothing

        Catch ex As Exception
            Log(ex.Message)
        End Try
    End Sub

End Class
