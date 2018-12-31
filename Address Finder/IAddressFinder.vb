Imports System.text
Imports System.IO

Public Interface IAddressFinder
    Function TargetIndexAddress() As String
    Function ArrayToString(ByVal a() As Byte) As String
    Function StringtoArray(ByVal s As String) As Byte()
    Function ArrayToValue(ByVal a() As Byte) As Long
    Function AddressOffset(ByVal a() As Byte) As Long
    Function FindAdresses() As String
    Function RunAddress() As String
    Function ZoneAddress() As String
    Function LocalPlayerInfo() As String
    Function NewSeekSpawnID() As String
    Function NewBeginMobData() As String
    Function NewStrPointer() As String
    Function NewSeekXYZ() As String
    Function GroundTarget() As String
    Function NextIndex() As String
    Function SeekLevel() As String
    Function SeekHealth() As String
    Function NewSeekDirection() As String
End Interface

Public MustInherit Class AddressFinder
    Implements IAddressFinder

    Protected fs As FileStream
    Protected re As System.Text.RegularExpressions.Regex
    Protected mt As System.Text.RegularExpressions.Match
    Protected s As String
    Protected sa() As Byte
    Protected t As String
    Protected SpawnID As String
    Protected SeekNPC As String
    Protected GroupSizeStruct As String
    Protected OffsetFromMana As String
    Protected SignArray As ArrayList = New ArrayList
    Protected ClientFolder As String
    Protected LastMatch As System.text.RegularExpressions.Match
    Private _FileName As String

    Public Sub New(ByVal FileName As String)
        Dim finfo As New FileInfo(FileName)

        _FileName = FileName

        If finfo.Exists Then
            fs = File.OpenRead(FileName)

            'Dim DirInfo As DirectoryInfo = New DirectoryInfo(Application.StartupPath & "\" & finfo.Directory.Name())
            'If Not DirInfo.Exists Then
            '    DirInfo.Create()
            'End If
            'DirInfo = New DirectoryInfo(DirInfo.FullName & "\" & Now.ToString("MMddyyyy"))
            'If Not DirInfo.Exists Then
            '    DirInfo.Create()
            'End If
            'ClientFolder = DirInfo.FullName
            'finfo.CopyTo(DirInfo.FullName & "\" & finfo.Name, True)
        Else
            Throw New Exception("File does not exist")
        End If

    End Sub
    Protected Sub ClearArray()
        SignArray.Clear()
    End Sub
    Protected Sub AddSignature(ByVal s As String)
        SignArray.Add(s)
    End Sub
    Protected Function ArrayToString(ByVal a() As Byte) As String Implements IAddressFinder.ArrayToString
        Dim s As StringBuilder = New StringBuilder
        s.EnsureCapacity(a.Length * 2)
        For i As Integer = a.GetLowerBound(0) To a.GetUpperBound(0)
            s.Append(IIf(Hex(a(i)).Length = 1, "0", "") & Hex(a(i)))
        Next
        Return s.ToString

    End Function
    Protected Function StringtoArray(ByVal s As String) As Byte() Implements IAddressFinder.StringtoArray
        Dim a() As Byte
        If s.Length = 1 Or s.Length = 0 Then
            ReDim a(0)
        Else
            ReDim a((s.Length \ 2) - 1)
        End If

        Try
            If s.Length > 1 Then
                Dim i As Integer
                Dim j As Integer = 0
                For i = 0 To s.Length - 1 Step 2
                    a(j) = Val("&H" & s.Chars(i) & s.Chars(i + 1))
                    j += 1
                Next
            ElseIf s.Length = 1 Then
                a(0) = Val("&H" & s.Chars(0))
            Else
                a(0) = Val("&H0")
            End If
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try

        Return a
    End Function
    Protected Function GetPointer(ByVal CallAddress As Integer, ByVal Length As Integer) As Integer
        Dim Result As Long
        Dim fs As FileStream = New FileStream(_FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)

        CallAddress -= &H400000
        CallAddress += 1

        fs.Seek(CallAddress, SeekOrigin.Begin)

        Dim myarray(Length - 1) As Byte

        fs.Read(myarray, 0, myarray.Length)

        For i As Integer = myarray.GetLowerBound(0) To myarray.GetUpperBound(0)
            Result += myarray(i) * (256 ^ i)
        Next

        Result += CallAddress - 1 + 5 + &H400000

        fs.Close()
        fs = Nothing

        Dim s As String = Hex(Result)

        If s.Length > 6 Then
            s = s.Remove(0, s.Length - 6)
        End If

        s = "&H" & s 'Val(s) gives me the address of the call

        Return CType(s, Integer)

    End Function
    Protected Function AddressOffset(ByVal a() As Byte) As Long Implements IAddressFinder.AddressOffset
        Dim i As Integer
        Dim Result As Long = 0

        For i = a.GetLowerBound(0) To a.GetUpperBound(0)
            Result = Result + a(i) * (256 ^ i)
        Next

        Return (Result And &HFFFFFFFF) + 5
    End Function
    Protected Function ArrayToValue(ByVal a() As Byte) As Long Implements IAddressFinder.ArrayToValue
        Dim i As Integer
        Dim Result As Long = 0

        For i = a.GetLowerBound(0) To a.GetUpperBound(0)
            Result = Result + a(i) * (256 ^ i)
        Next

        Return Result
    End Function
    Protected Function LeftFillWithZeros(ByVal s As String) As String
        While s.Length < 5
            s = "0" & s
        End While
        Return s
    End Function
    Protected Function FindAdresses() As String Implements IAddressFinder.FindAdresses
        Dim ba(fs.Length - 1) As Byte
        Dim i As Integer = fs.Read(ba, 0, fs.Length)
        fs.Close()
        t = ArrayToString(ba)
        Dim Result As New StringBuilder

        'DONT CHANGE ORDER

        'Result.Append(SeekNewMob)
        'Result.Append(SeekUpdateMob)
        Result.Append(FindNetFunctions)
        Result.Append(LocalPlayerInfo())
        'Result.Append(NewStrPointer)
        Result.Append(RunAddress)
        'Result.Append(ZoneAddress) 'fixed 4-12
        Result.Append(TargetIndexAddress) 'new
        'Result.Append(NewBeginMobData)
        'Result.Append(GroundTarget)
        'Result.Append(NextIndex)
        'Result.Append(NewSeekXYZ)
        'Result.Append(SeekLevel)
        'Result.Append(SeekHealth)
        'Result.Append(NewSeekDirection)
        'Result.Append(NewSeekSpawnID) 'NEED TO FIX THIS ALSO THIS IS IN ANOTHER FUNCTION

        'Result.Append(SpawnID)
        'Result.Append(SeekNPC) 'this is missing completely THIS IS IN ANOTHER FUNCTION

        'Dim output As TextWriter = New StreamWriter(ClientFolder & "\" & "MemoryLocs.txt", FileMode.Create)
        'output.Write(Result.ToString)
        'output.Close()

        Return Result.ToString
    End Function
    'Protected MustOverride Function BeginMobData() As String Implements IAddressFinder.BeginMobData
    Protected MustOverride Function NewBeginMobData() As String Implements IAddressFinder.NewBeginMobData
    Protected MustOverride Function NewStrPointer() As String Implements IAddressFinder.NewStrPointer
    Protected MustOverride Function GroundTarget() As String Implements IAddressFinder.GroundTarget
    Protected MustOverride Function NextIndex() As String Implements IAddressFinder.NextIndex
    Protected MustOverride Function RunAddress() As String Implements IAddressFinder.RunAddress
    Protected MustOverride Function NewSeekDirection() As String Implements IAddressFinder.NewSeekDirection
    Protected MustOverride Function SeekHealth() As String Implements IAddressFinder.SeekHealth
    Protected MustOverride Function SeekLevel() As String Implements IAddressFinder.SeekLevel
    Protected MustOverride Function NewSeekXYZ() As String Implements IAddressFinder.NewSeekXYZ
    Protected MustOverride Function NewSeekSpawnID() As String Implements IAddressFinder.NewSeekSpawnID
    Protected MustOverride Function FindNetFunctions() As String
    Protected MustOverride Function ZoneAddress() As String Implements IAddressFinder.ZoneAddress
    Protected MustOverride Function SeekNewMob() As String
    Protected MustOverride Function SeekUpdateMob() As String
    Protected MustOverride Function TargetIndexAddress() As String Implements IAddressFinder.TargetIndexAddress
    Protected MustOverride Function LocalPlayerInfo() As String Implements IAddressFinder.LocalPlayerInfo
End Class