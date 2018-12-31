Imports System.Runtime.InteropServices
Imports System.Runtime.Remoting.Messaging

Public NotInheritable Class MailSlot
#Region "Structs/Variables"
    ' This is the OverLapped structure used by the calls to the Windows API.
    <StructLayout(LayoutKind.Sequential, Pack:=1)> Private Structure OVERLAPPED
        Public Internal As Integer
        Public InternalHigh As Integer
        Public Offset As Integer
        Public OffsetHigh As Integer
        Public hEvent As Integer
    End Structure
    Private Const FILE_SHARE_READ As Long = &H1
    Private Const GENERIC_WRITE As Long = &H40000000
    Private Const OPEN_EXISTING As Long = 3
    Private Const FILE_ATTRIBUTE_NORMAL As Long = &H80
    Private Declare Function CloseHandle Lib "kernel32.dll" (ByVal hObject As IntPtr) As Integer
    Private Declare Auto Function CreateFile Lib "kernel32.dll" _
   (ByVal lpFileName As String, ByVal dwDesiredAccess As Integer, _
      ByVal dwShareMode As Integer, ByVal lpSecurityAttributes As IntPtr, _
         ByVal dwCreationDisposition As Integer, ByVal dwFlagsAndAttributes As Integer, _
            ByVal hTemplateFile As IntPtr) As IntPtr
    Private Declare Function WriteFile Lib "kernel32.dll" (ByVal hFile As IntPtr, ByVal lpBuffer As Byte(), ByVal nNumberOfBytesToWrite As Integer, ByRef lpNumberOfBytesWritten As Integer, ByRef lpOverlapped As OVERLAPPED) As Integer

#End Region
    Public Sub New()
    End Sub
    Public Shared Sub WriteMail(ByVal pBuffer As Byte())
        Dim dataRead As Integer
        Dim m_slotHandle As IntPtr = CreateFile("\\.\mailslot\AKPACKET", GENERIC_WRITE, FILE_SHARE_READ, IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero)
        Dim ret As Integer = WriteFile(m_slotHandle, pBuffer, pBuffer.Length, dataRead, Nothing)
        CloseHandle(m_slotHandle)
    End Sub
End Class


