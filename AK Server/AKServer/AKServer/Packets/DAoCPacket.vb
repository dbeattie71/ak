Namespace DAoCServer

#Region "DAOC Event Arguments"
    <Serializable()> _
    Public Class DAOCEventArgs
        Inherits EventArgs
        Private _DAOCObject As DAOCObject
        Private _Item As DAOCNameValuePair
        Private _Pet As DAOCPet
        Private _SpellCastObj As SpellCast
        Private _SpellEffectObj As SpellEffectAnimation
        Public Sub New()

        End Sub

        Public Sub New(ByVal aObject As DAOCObject)
            _DAOCObject = aObject
        End Sub

        Public Sub New(ByVal aItem As DAOCNameValuePair)
            _Item = aItem
        End Sub

        Public Sub New(ByVal aPet As DAOCPet)
            _Pet = aPet
        End Sub

        Public Sub New(ByVal aSpellCastObj As SpellCast)
            _SpellCastObj = aSpellCastObj
        End Sub

        Public Sub New(ByVal aSpellEffectObj As SpellEffectAnimation)
            _SpellEffectObj = aSpellEffectObj
        End Sub

        Public Property SpellEffectObj() As SpellEffectAnimation
            Get
                Return _SpellEffectObj
            End Get
            Set(ByVal Value As SpellEffectAnimation)
                _SpellEffectObj = Value
            End Set
        End Property

        Public Property [DAOCObject]() As DAOCObject
            Get
                Return _DAOCObject
            End Get
            Set(ByVal Value As DAOCObject)
                _DAOCObject = Value
            End Set
        End Property

        Public Property Item() As DAOCNameValuePair
            Get
                Return _Item
            End Get
            Set(ByVal Value As DAOCNameValuePair)
                _Item = Value
            End Set
        End Property

        Public Property Pet() As DAOCPet
            Get
                Return _Pet
            End Get
            Set(ByVal Value As DAOCPet)
                _Pet = Value
            End Set
        End Property

        Public Property SpellCastObj() As SpellCast
            Get
                Return _SpellCastObj
            End Get
            Set(ByVal Value As SpellCast)
                _SpellCastObj = Value
            End Set
        End Property
    End Class
#End Region

#Region "DAOCPacket"
    Public Class DAOCPacket
        Private mPacketData() As Byte
        Private mSize As Integer
        Private mPosition As Integer
        Protected Overrides Sub Finalize()
            mPacketData = Nothing
            MyBase.Finalize()
        End Sub

        Public Sub Seek(ByVal dwCount As Integer)
            mPosition += dwCount
        End Sub

        Public Function GetByte() As Byte
            Dim Value As Byte
            Value = mPacketData(mPosition)
            Seek(1)
            Return Value
        End Function

        Public Function PeekByte() As Byte
            Dim Value As Byte
            Value = mPacketData(mPosition)
            Return Value
        End Function

        Public Function GetShort() As Integer
            Dim Value As Integer
            Value = mPacketData(mPosition) * 256 + mPacketData(mPosition + 1)
            Seek(2)
            Return Value
        End Function

        Public Function GetLong() As Integer
            Dim Value As Integer
            Value = ((((mPacketData(mPosition) * 256) + mPacketData(mPosition + 1)) * 256) + mPacketData(mPosition + 2)) * 256 + mPacketData(mPosition + 3)
            Seek(4)
            Return Value
        End Function

        Public Function GetANullTermString(ByVal AMinLen As Integer) As String

            Try
                Dim Result As String = System.Text.Encoding.ASCII.GetString(mPacketData, mPosition, mSize - mPosition)
                Seek(AMinLen)
                Return Result.Substring(0, Result.IndexOf(vbNullChar))
            Catch ex As Exception
                Return String.Empty
            End Try

        End Function

        Public Function PeekANullTermString() As String
            Try
                Dim Result As String = System.Text.Encoding.Default.GetString(mPacketData, mPosition, mSize - mPosition)
                Return Result.Substring(0, Result.IndexOf(vbNullChar))
            Catch ex As Exception
                Return String.Empty
            End Try

        End Function

        Public Function GetPascalString() As String

            Dim iLen As Int16 = GetByte()

            If iLen = 0 Then
                Return String.Empty
            Else
                Dim Result As String = System.Text.Encoding.Default.GetString(mPacketData, mPosition, iLen)
                Seek(iLen)
                Return Result
            End If
        End Function

        Public Function PeekPascalString() As String

            Dim iLen As Int16 = GetByte()

            If iLen = 0 Then
                Return String.Empty
            Else
                Dim Result As String = System.Text.Encoding.Default.GetString(mPacketData, mPosition, iLen)
                Return Result
            End If
        End Function

        Public Overrides Function ToString() As String
            Dim result As String
            Dim shex As String
            Dim sascii As String
            Dim c As Char
            Dim i As Integer
            Dim Data() As Byte = mPacketData

            shex = String.Empty
            sascii = String.Empty
            result = String.Empty

            For i = LBound(Data) To UBound(Data)
                If (i Mod 16 = 0) AndAlso i <> 0 Then
                    result = result & shex & " " & sascii & vbCrLf
                    shex = String.Empty
                    sascii = String.Empty
                ElseIf i Mod 16 = 8 Then
                    shex = shex & "- "
                End If

                c = Chr(Data(i))
                shex = shex & IIf(Len(Hex(Data(i))) = 1, "0", "").ToString & (Hex(Data(i))) & " "
                If (Data(i) < Asc(" ")) OrElse (Data(i) > Asc("~")) Then
                    sascii = sascii & "."
                Else
                    sascii = sascii & c
                End If
            Next

            While Len(sascii) < 16
                If Len(sascii) Mod 16 = 8 Then
                    shex = shex & "- "
                End If
                shex = shex & "   "
                sascii = sascii & " "
            End While

            result = result & shex & " " & sascii & vbCrLf
            Erase Data
            Return result

        End Function

        Public Function EOF() As Boolean
            Return CBool(mPosition >= mSize)
        End Function

        Public Property Size() As Integer
            Get
                Return mSize
            End Get
            Set(ByVal Value As Integer)
                mSize = Value
            End Set
        End Property

        Public Property Position() As Integer
            Get
                Return mPosition
            End Get
            Set(ByVal Value As Integer)
                mPosition = Value
            End Set
        End Property

        Public Property PacketData() As Byte()
            Get
                Return mPacketData
            End Get
            Set(ByVal Value As Byte())
                mPacketData = Value
            End Set
        End Property


        'Public Overridable Function ReadShort() As System.UInt16
        '    Dim v1 As Integer = ReadByte
        '    Dim v2 As Integer = ReadByte
        '    Return CType(((v2 And 255) Or (v1 And 255) << 8), System.UInt16)
        'End Function

        'Public Overridable Function ReadShortLowEndian() As System.UInt16
        '    Dim v1 As Integer = ReadByte
        '    Dim v2 As Integer = ReadByte
        '    Return CType(((v1 And 255) Or (v2 And 255) << 8), System.UInt16)
        'End Function

        'Public Overridable Function ReadInt() As System.UInt32
        '    Dim v1 As Integer = ReadByte
        '    Dim v2 As Integer = ReadByte
        '    Dim v3 As Integer = ReadByte
        '    Dim v4 As Integer = ReadByte
        '    Return CType(((v1 << 24) Or (v2 << 16) Or (v3 << 8) Or v4), System.UInt32)
        'End Function
    End Class
#End Region
End Namespace
