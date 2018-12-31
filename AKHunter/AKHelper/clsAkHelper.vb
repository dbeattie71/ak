Option Explicit On 

Public Class clsAkHelper
    Inherits Base

    Private _LogIndex As Hashtable

    Private _ChatLogLines As ChatLogLines
    Private _LogLines As LogLines

    Private _Users As Users
    Private _Settings As Settings
    Private _Spells As Spells

    Private _Movement As Movement
    Private _Misc As Misc

    Private _SpellTimers As SpellTimers

    Private _SpamSpellList As String

    Public Sub New()

    End Sub

    Public Sub DoInit()

        Try
            _LogIndex = New Hashtable

            _ChatLogLines = New ChatLogLines
            _LogLines = New LogLines

            _Users = New Users
            _Settings = New Settings
            _Spells = New Spells

            _Movement = New Movement
            _Misc = New Misc

            _SpellTimers = New SpellTimers

        Catch ex As Exception
            Log(ex.Message)
        End Try

    End Sub

    Public Sub StopInit()

        Try

            _LogIndex = Nothing

            _ChatLogLines = Nothing
            _LogLines = Nothing

            _Users = Nothing
            _Settings = Nothing
            _Spells = Nothing

            _Movement = Nothing
            _Misc = Nothing

            _SpellTimers = Nothing

        Catch ex As Exception
            Log(ex.Message)
        End Try

    End Sub

    Public Property SpamSpellList() As String

        Get
            Return _SpamSpellList
        End Get
        Set(ByVal Value As String)
            _SpamSpellList = Value
        End Set

    End Property

    'chatloglines
    Public Sub LoadChatLogLines(ByVal FileName As String)

        _ChatLogLines.Load(FileName)

    End Sub

    Public Function AddChatLogLinesStrings(ByVal Daoc As AutoKillerScript.clsAutoKillerScript, _
                                           ByVal StartIndex As Integer) As Integer

        Return _ChatLogLines.AddStrings(Daoc, StartIndex, _LogIndex)

    End Function

    'loglines
    Public Sub LoadLogLines(ByVal FileName As String)

        _LogLines.Load(FileName)

    End Sub

    Public Function AddLogLinesStrings(ByVal Daoc As AutoKillerScript.clsAutoKillerScript, _
                                       ByVal StartIndex As Integer) As Integer

        Return _LogLines.AddStrings(Daoc, StartIndex, _LogIndex)

    End Function

    Public Function GetLogNameByIndex(ByVal LogIndex As Integer) As String

        Return _LogIndex(LogIndex)

    End Function

    'users
    Public Sub LoadUsers(ByVal FileName As String)

        _Users.Load(FileName)

    End Sub

    Public Function ValidateUser(ByVal User As String) As Boolean

        Return _Users.ValidateUser(User)

    End Function

    'settings
    Public Sub LoadSettings(ByVal FileName As String)

        _Settings.Load(FileName)

    End Sub

    Public Function GetSetting(ByVal SettingName As String) As String

        Return _Settings.GetSetting(SettingName)

    End Function

    'spells
    Public Sub LoadSpells(ByVal FileName As String)

        _Spells.Load(FileName)

    End Sub

    Public Sub Cast(ByVal Daoc As AutoKillerScript.clsAutoKillerScript, _
                    ByVal PlayerName As String, _
                    ByVal TargetPlayerNameList As String, _
                    ByVal SpellList As String, _
                    Optional ByVal SetGroundTarget As Boolean = False)

        _Spells.Cast(Daoc, PlayerName, TargetPlayerNameList, SpellList, SetGroundTarget)

    End Sub

    'movement
    Public Sub AutoFollow(ByRef Daoc As AutoKillerScript.clsAutoKillerScript, _
                          ByVal Name As String, _
                          ByVal StickKey As Byte)

        _Movement.AutoFollow(Daoc, Name, StickKey)

    End Sub

    Public Sub AutoFollow(ByRef Daoc As AutoKillerScript.clsAutoKillerScript, _
                          ByVal Name As String)

        _Movement.AutoFollow(Daoc, Name)

    End Sub

    Public Sub BreakAutoFollow(ByVal Daoc As AutoKillerScript.clsAutoKillerScript, _
                               ByVal MoveBackwardKey As Byte)

        _Movement.BreakAutoFollow(Daoc, MoveBackwardKey)

    End Sub

    Public Sub Stand(ByVal Daoc As AutoKillerScript.clsAutoKillerScript)

        _Movement.Stand(Daoc)

    End Sub

    Public Sub Sit(ByVal Daoc As AutoKillerScript.clsAutoKillerScript)

        _Movement.Sit(Daoc)

    End Sub

    'misc
    Public Sub PassThru(ByVal Daoc As AutoKillerScript.clsAutoKillerScript, _
                        ByVal Message As String, _
                        ByVal PassThruFlag As String)

        _Misc.PassThru(Daoc, Message, PassThruFlag)

    End Sub

    Public Sub AcceptDialog(ByVal Daoc As AutoKillerScript.clsAutoKillerScript)

        _Misc.AcceptDialog(Daoc)

    End Sub

    Public Sub SetGroundTarget(ByVal Daoc As AutoKillerScript.clsAutoKillerScript, _
                               ByVal PlayerName As String)

        _Misc.SetGroundTarget(Daoc, PlayerName)

    End Sub

    Public Sub SetEffectsToNone(ByVal Daoc As AutoKillerScript.clsAutoKillerScript)

        _Misc.SetEffectsToNone(Daoc)

    End Sub

    Public Sub Disband(ByVal Daoc As AutoKillerScript.clsAutoKillerScript)

        _Misc.Disband(Daoc)

    End Sub

    'spelltimers
    Public Sub LoadSpellTimers(ByVal FileName As String)

        _SpellTimers.Load(FileName)

    End Sub

    Public Function GetSpellTimers() As Hashtable

        Return _SpellTimers.GetSpellTimers

    End Function

End Class
