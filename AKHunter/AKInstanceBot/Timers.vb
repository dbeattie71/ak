Option Explicit On 

Imports System
Imports System.Collections

Namespace AKInstanceBot

    Public Class Timers

        Private _Timers As Hashtable

        Public Sub New()

            _Timers = New Hashtable

        End Sub

        Private Class Timer

            Public Name As String
            Public Time As DateTime
            Public Timeout As Long
            Public Disabled As Boolean

        End Class

        Public Sub DefineCooldown(ByVal Name As String, ByVal Timeout As Long)

            Dim timer As New Timer
            timer = New Timer

            timer.Name = Name
            timer.Timeout = Timeout
            timer.Time = DateTime.Today
            timer.Disabled = False

            _Timers.Add(Name, timer)

        End Sub

        Public Function IsReady(ByVal Name As String) As Boolean

            Dim timer As Timer

            If Not _Timers.ContainsKey(Name) Then
                Throw New Exception(String.Format("Invalid cooldown: " & Name))
            End If

            timer = _Timers(Name)

            If timer.Disabled Then
                Return False
            End If

            If (DateTime.Now.Subtract(timer.Time)).TotalMilliseconds > timer.Timeout Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Sub SetTime(ByVal Name As String)

            Dim timer As Timer

            timer = _Timers(Name)
            timer.Time = DateTime.Now

        End Sub

        Public Sub ClearTime(ByVal Name As String)

            Dim timer As Timer

            timer = _Timers(Name)
            timer.Time = DateTime.Today
            timer.Disabled = False

        End Sub

        Public Sub Disable(ByVal Name As String)

            Dim timer As Timer

            timer = _Timers(Name)
            timer.Disabled = True

        End Sub

    End Class

End Namespace


