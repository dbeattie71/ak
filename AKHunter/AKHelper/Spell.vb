Option Explicit On 

Public Class Spell
    Inherits Base

    Private _Name As String
    Private _Target As String
    Private _Qbar As Integer
    Private _Key As Byte
    Private _CastTime As Integer

    Public Sub Cast(ByVal Daoc As AutoKillerScript.clsAutoKillerScript)

        If Daoc.isPlayerSitting() Then
            System.Threading.Thread.CurrentThread.Sleep(100)
            Daoc.SendString("/stand~")
            System.Threading.Thread.CurrentThread.Sleep(500)
        End If

        System.Threading.Thread.CurrentThread.Sleep(100)
        Daoc.SendString("/qbar " & _Qbar & "~")
        System.Threading.Thread.CurrentThread.Sleep(100)

        Daoc.SendKeys(_Key, False)
        System.Threading.Thread.CurrentThread.Sleep(_CastTime)

    End Sub

    Public Function Cast(ByVal Daoc As AutoKillerScript.clsAutoKillerScript, _
                         ByVal PlayerName As String, _
                         ByVal TargetPlayerName As String, _
                         Optional ByVal SetGroundTarget As Boolean = False) As String

        'cast your SpellList ffs 
        'CasterName cast your SpellList ffs 

        'cast SpellList here
        'CasterName cast SpellList here

        'cast SpellList on TargetPlayerName plz
        'CasterName cast SpellList on TargetPlayerName plz

        Select Case _Target.ToLower
            Case "self"

                Log("Casting:" & _Name _
                    & "  PlayerName:" & PlayerName _
                    & "  TargetPlayerName:" & TargetPlayerName)

                Me.Cast(Daoc)

            Case "groundtarget"

                Log("Casting:" & _Name _
                    & "  PlayerName:" & PlayerName _
                    & "  TargetPlayerName:" & TargetPlayerName)

                If SetGroundTarget = True Then
                    System.Threading.Thread.CurrentThread.Sleep(250)
                    Daoc.SendString("/groundassist " & PlayerName & "~")
                    System.Threading.Thread.CurrentThread.Sleep(250)
                End If

                Me.Cast(Daoc)
                
            Case "friend"

                Log("Casting:" & _Name _
                    & "  PlayerName:" & PlayerName _
                    & "  TargetPlayerName:" & TargetPlayerName)

                If TargetPlayerName.ToLower = "me" Then
                    System.Threading.Thread.CurrentThread.Sleep(250)
                    Daoc.SetTarget(PlayerName, True)
                    System.Threading.Thread.CurrentThread.Sleep(250)
                Else
                    If TargetPlayerName.ToLower = "yourself" Then
                        System.Threading.Thread.CurrentThread.Sleep(250)
                        Daoc.SendString("/target " & Daoc.PlayerName & "~")
                        System.Threading.Thread.CurrentThread.Sleep(250)
                    Else
                        System.Threading.Thread.CurrentThread.Sleep(250)
                        Daoc.SetTarget(TargetPlayerName, True)
                        System.Threading.Thread.CurrentThread.Sleep(250)
                    End If
                End If

                Me.Cast(Daoc)

        End Select

    End Function

    Public Property Name() As String

        Get
            Return _Name
        End Get
        Set(ByVal Value As String)
            _Name = Value
        End Set

    End Property

    Public Property Target() As String

        Get
            Return _Target
        End Get
        Set(ByVal Value As String)
            _Target = Value
        End Set

    End Property

    Public Property Qbar() As Integer

        Get
            Return _Qbar
        End Get
        Set(ByVal Value As Integer)
            _Qbar = Value
        End Set

    End Property

    Public Property Key() As Byte

        Get
            Return _Key
        End Get
        Set(ByVal Value As Byte)
            _Key = Value
        End Set

    End Property

    Public Property CastTime() As Integer

        Get
            Return _CastTime
        End Get
        Set(ByVal Value As Integer)
            _CastTime = Value
        End Set

    End Property

End Class
