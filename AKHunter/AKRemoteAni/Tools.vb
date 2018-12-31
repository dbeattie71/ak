Module Tools
    Public Function ParseWord(ByVal sLine As String, ByRef iStartpos As Integer) As String
        Dim Result As String = ""
        Try
            While (iStartpos < sLine.Length) And Not (Char.IsLetterOrDigit(sLine.Chars(iStartpos)) = True)
                iStartpos += 1
            End While

            While (iStartpos < sLine.Length) And (Char.IsLetterOrDigit(sLine.Chars(iStartpos)) = True)
                Result = Result & sLine.Chars(iStartpos)
                iStartpos += 1
            End While
        Catch
        End Try
        Return Result
    End Function
End Module
