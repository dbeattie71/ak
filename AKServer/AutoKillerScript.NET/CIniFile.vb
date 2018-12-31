''' <exclude/> 
Public NotInheritable Class CIniFile
    ' =========================================================
    ' Class:    cIniFile
    ' Author:   Steve McMahon
    ' Date  :   21 Feb 1997
    '
    ' A nice class wrapper around the INIFile functions
    ' Allows searching,deletion,modification and addition
    ' of Keys or Values.
    '
    ' Updated 10 May 1998 for VB5.
    '   * Added EnumerateAllSections method
    '   * Added Load and Save form position methods
    ' =========================================================

    Private m_sPath As String
    Private m_sKey As String
    Private m_sSection As String
    Private m_sDefault As String
    Private m_lLastReturnCode As Long

    Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer
    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer

    Public ReadOnly Property LastReturnCode() As Long
        Get
            Return m_lLastReturnCode
        End Get
    End Property

    Public ReadOnly Property SUCCESS() As Boolean
        Get
            Return (m_lLastReturnCode <> 0)
        End Get
    End Property

    Public Property [Default]() As String
        Get
            Return m_sDefault
        End Get
        Set(ByVal Value As String)
            m_sDefault = Value
        End Set
    End Property

    Public Property Path() As String
        Get
            Return m_sPath
        End Get
        Set(ByVal Value As String)
            m_sPath = Value
        End Set
    End Property

    Public Property Key() As String
        Get
            Return m_sKey
        End Get
        Set(ByVal Value As String)
            m_sKey = Value
        End Set
    End Property

    Public Property Section() As String
        Get
            Return m_sSection
        End Get
        Set(ByVal Value As String)
            m_sSection = Value
        End Set
    End Property

    Public Property Value() As String
        Get
            Dim sBuf As String
            Dim iSize As Integer
            Dim iRetCode As Integer

            sBuf = Space$(255)
            iSize = Len(sBuf)
            iRetCode = GetPrivateProfileString(m_sSection, m_sKey, m_sDefault, sBuf, iSize, m_sPath)
            If (iSize > 0) Then
                Return Left$(sBuf, iRetCode)
            Else
                Return ""
            End If
        End Get
        Set(ByVal Value As String)
            Dim iPos As Integer
            ' Strip chr$(0):
            iPos = InStr(Value, Chr(0))
            Do While iPos <> 0
                Value = Left$(Value, (iPos - 1)) & Mid$(Value, (iPos + 1))
                iPos = InStr(Value, Chr(0))
            Loop
            m_lLastReturnCode = WritePrivateProfileString(m_sSection, m_sKey, Value, m_sPath)
        End Set
    End Property

    Public Property Value(ByVal aSection As String, ByVal aKey As String, ByVal aDefault As Object) As String
        Get
            Dim sBuf As String
            Dim iSize As Integer
            Dim iRetCode As Integer

            sBuf = Space$(255)
            iSize = Len(sBuf)
            iRetCode = GetPrivateProfileString(aSection, aKey, CStr(aDefault), sBuf, iSize, m_sPath)
            If (iSize > 0) Then
                Return Left$(sBuf, iRetCode)
            Else
                Return ""
            End If
        End Get
        Set(ByVal Value As String)
            Dim iPos As Integer
            ' Strip chr$(0):
            iPos = InStr(Value, Chr(0))
            Do While iPos <> 0
                Value = Left$(Value, (iPos - 1)) & Mid$(Value, (iPos + 1))
                iPos = InStr(Value, Chr(0))
            Loop
            m_lLastReturnCode = WritePrivateProfileString(aSection, aKey, Value, m_sPath)
        End Set
    End Property

    Public Sub DeleteKey()
        m_lLastReturnCode = WritePrivateProfileString(m_sSection, m_sKey, vbNullString, m_sPath)
    End Sub

    Public Sub DeleteSection()
        m_lLastReturnCode = WritePrivateProfileString(m_sSection, vbNullString, vbNullString, m_sPath)
    End Sub

    Public Property INISection() As String
        Get
            Dim sBuf As String
            Dim iSize As Integer
            Dim iRetCode As Integer

            sBuf = Space$(8192)
            iSize = Len(sBuf)
            iRetCode = GetPrivateProfileString(m_sSection, vbNullString, m_sDefault, sBuf, iSize, m_sPath)
            If (iSize > 0) Then
                Return Left$(sBuf, iRetCode)
            Else
                Return ""
            End If
        End Get
        Set(ByVal Value As String)
            m_lLastReturnCode = WritePrivateProfileString(m_sSection, vbNullString, Value, m_sPath)
        End Set
    End Property

    Public ReadOnly Property Sections() As String
        Get
            Dim sBuf As String
            Dim iSize As Integer
            Dim iRetCode As Integer

            sBuf = Space$(8192)
            iSize = Len(sBuf)
            iRetCode = GetPrivateProfileString(vbNullString, vbNullString, m_sDefault, sBuf, iSize, m_sPath)
            If (iSize > 0) Then
                Return Left$(sBuf, iRetCode)
            Else
                Return ""
            End If
        End Get
    End Property

    Public Sub EnumerateCurrentSection(ByRef sKey() As String, ByRef iCount As Integer)
        Dim sSection As String
        Dim iPos As Integer
        Dim iNextPos As Integer
        Dim sCur As String

        iCount = 0
        Erase sKey
        sSection = INISection
        If (Len(sSection) > 0) Then
            iPos = 1
            iNextPos = InStr(iPos, sSection, Chr(0))
            Do While iNextPos <> 0
                sCur = Mid$(sSection, iPos, (iNextPos - iPos))
                If (sCur <> Chr(0)) Then
                    iCount += 1
                    ReDim Preserve sKey(iCount)
                    sKey(iCount) = Mid$(sSection, iPos, (iNextPos - iPos))
                    iPos = iNextPos + 1
                    iNextPos = InStr(iPos, sSection, Chr(0))
                End If
            Loop
        End If
    End Sub

    Public Sub EnumerateAllSections(ByRef sSections() As String, ByRef iCount As Integer)
        Dim sIniFile As String
        Dim iPos As Integer
        Dim iNextPos As Integer
        Dim sCur As String

        iCount = 0
        Erase sSections
        sIniFile = Sections
        If (Len(sIniFile) > 0) Then
            iPos = 1
            iNextPos = InStr(iPos, sIniFile, Chr(0))
            Do While iNextPos <> 0
                If (iNextPos <> iPos) Then
                    sCur = Mid$(sIniFile, iPos, (iNextPos - iPos))
                    iCount += 1
                    ReDim Preserve sSections(iCount)
                    sSections(iCount) = sCur
                End If
                iPos = iNextPos + 1
                iNextPos = InStr(iPos, sIniFile, Chr(0))
            Loop
        End If
    End Sub

    Public Function CLngDefault(ByVal sString As String, Optional ByVal lDefault As Long = 0) As Long
        Dim lR As Long
        On Error Resume Next
        lR = CLng(sString)
        If (Err.Number <> 0) Then
            CLngDefault = lDefault
        Else
            CLngDefault = lR
        End If
    End Function

End Class