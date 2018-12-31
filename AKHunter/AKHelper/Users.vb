Option Explicit On 

Friend Class Users
    Inherits Base

    Private _Users As Hashtable

    Public Sub New()
        _Users = New Hashtable
    End Sub

    Public Sub Load(ByVal FileName As String)

        Dim xmlDoc As Xml.XmlDocument = New Xml.XmlDocument
        Dim xmlNode As Xml.XmlNode

        Dim logLine As LogLine

        Try
            xmlDoc.Load(FileName)

            Log("---- Loading User data  ----")

            For Each xmlNode In xmlDoc.SelectNodes("//Profile/Users/User")
                _Users.Add(xmlNode.SelectSingleNode("@Name").Value, xmlNode.SelectSingleNode("@Name").Value)
                Log("User name: " & xmlNode.SelectSingleNode("@Name").Value)
            Next

            xmlDoc = Nothing

        Catch ex As Exception
            Log(ex.Message)
        End Try
    End Sub

    Public Sub AddUser(ByVal UserName As String)

        _Users.Add(UserName, UserName)

    End Sub

    Public Function ValidateUser(ByVal UserName As String) As Boolean

        Return _Users.ContainsKey(UserName)

    End Function
End Class
