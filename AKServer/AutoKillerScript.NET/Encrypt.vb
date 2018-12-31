#Region "Imports"
Imports System.Net
Imports System.Security.Cryptography
Imports System.IO
#End Region
Public Class Encrypter
#Region "Variables"
    Private AKWebService As New autokillerwebservice.Encrypt
    Private rsa1 As RSACryptoServiceProvider
#End Region
    Public Sub New()
        'autokillerwebservice
    End Sub
    Public Function GetXML(ByVal RegCode As String) As MemoryStream

        SetSecurity(RegCode)

        'create local keys, grab encrypted xml from webservice then decrypt
        Try
            Dim content() As Byte = AKWebService.EncryptFile(CreateKeys())
            Dim data As Byte() = Decrypt(content)
            If Not data Is Nothing Then
                Dim fs As New MemoryStream
                fs.Write(data, 0, data.Length)
                fs.Position = 0
                Return fs
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function
    Public Function LogUserOut(ByVal RegCode As String) As Integer

        SetSecurity(RegCode)
        Return AKWebService.LogUserOut

    End Function
    Private Sub SetSecurity(ByVal RegCode As String)
        'ntlm
        '*********************************************************************************
        'Dim mycredentials As New CredentialCache
        'Dim m_cred As NetworkCredential = New NetworkCredential(RegCode, RegCode)
        ''Negotiate for NTLM or Kerberos authentication.
        'mycredentials.Add(New Uri(AKWebService.Url), "Negotiate", m_cred)
        'AKWebService.Credentials = mycredentials
        '*********************************************************************************


        'soapheader
        '*********************************************************************************
        Dim sheader As New autokillerwebservice.AuthInfo
        sheader.UserName = RegCode
        sheader.Password = RegCode
        AKWebService.AuthInfoValue = sheader
        '*********************************************************************************

    End Sub
    Private Function CreateKeys() As String
        'This creates both the public and the private keys
        rsa1 = New RSACryptoServiceProvider

        Return rsa1.ToXmlString(False) 'public key
    End Function
    Private Function Decrypt(ByVal input() As Byte) As Byte()
        ' by default this will create a 128 bits AES (Rijndael) object
        Dim sa As SymmetricAlgorithm = SymmetricAlgorithm.Create()

        Dim keyex(127) As Byte
        Buffer.BlockCopy(input, 0, keyex, 0, keyex.Length)

        Dim def As New RSAPKCS1KeyExchangeDeformatter(rsa1)
        Dim key As Byte() = def.DecryptKeyExchange(keyex)

        Dim iv(sa.IV.Length - 1) As Byte
        Buffer.BlockCopy(input, keyex.Length, iv, 0, iv.Length)

        Dim ct As ICryptoTransform = sa.CreateDecryptor(key, iv)
        Dim decrypt1 As Byte() = ct.TransformFinalBlock(input, keyex.Length + iv.Length, input.Length - (keyex.Length + iv.Length))
        Return decrypt1
    End Function
End Class