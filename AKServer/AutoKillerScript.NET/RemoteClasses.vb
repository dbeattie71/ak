Imports AKServer.DLL.DAoCServer

Public NotInheritable Class OurCallBack
    Inherits RemotelyDelegatableObject
    Public Event OnLogUpdate(ByVal sLine As String)
    Public Event OnPlayerQuit()
    Public Event OnZoneChange(ByVal ZoneID As Integer)
    Public Event OnNewObject(ByVal sender As Object, ByVal e As DAOCEventArgs)
    Public Event OnPetWindowUpdate(ByVal sender As Object, ByVal e As DAOCEventArgs)
    Public Event OnSpellCast(ByVal sender As Object, ByVal e As DAOCEventArgs)
    Public Event OnDialogMessage(ByVal sender As Object, ByVal e As LogUpdateEventArgs)
    Public Event OnProgressMeter(ByVal sender As Object, ByVal e As LogUpdateEventArgs)
    Public Event OnSpellEffectAnimation(ByVal sender As Object, ByVal e As DAOCEventArgs)
    Public Overrides Function InitializeLifetimeService() As Object
        Return Nothing
    End Function
    Protected Overrides Sub LogUpdateCallback(ByVal sender As Object, ByVal submitArgs As LogUpdateEventArgs)
        RaiseEvent OnLogUpdate(submitArgs.sLine)
    End Sub
    Protected Overrides Sub ZoneChangeCallback(ByVal sender As Object, ByVal submitArgs As ZoneEventArgs)
        RaiseEvent OnZoneChange(submitArgs.Zone)
    End Sub
    Protected Overrides Sub NewObjectCallback(ByVal sender As Object, ByVal submitArgs As DAOCEventArgs)
        RaiseEvent OnNewObject(sender, submitArgs)
    End Sub
    Protected Overrides Sub PetWindowUpdateCallback(ByVal sender As Object, ByVal submitArgs As DAOCEventArgs)
        RaiseEvent OnPetWindowUpdate(sender, submitArgs)
    End Sub
    Protected Overrides Sub SpellCastCallback(ByVal sender As Object, ByVal submitArgs As DAOCEventArgs)
        RaiseEvent OnSpellCast(sender, submitArgs)
    End Sub
    Protected Overrides Sub DialogMessageCallback(ByVal sender As Object, ByVal submitArgs As LogUpdateEventArgs)
        RaiseEvent OnDialogMessage(sender, submitArgs)
    End Sub
    Protected Overrides Sub ProgressMeterCallback(ByVal sender As Object, ByVal submitArgs As LogUpdateEventArgs)
        RaiseEvent OnProgressMeter(sender, submitArgs)
    End Sub
    Protected Overrides Sub SpellEffectCallback(ByVal sender As Object, ByVal submitArgs As DAOCEventArgs)
        RaiseEvent OnSpellEffectAnimation(sender, submitArgs)
    End Sub
    Protected Overrides Sub PlayerQuitCallback(ByVal sender As Object, ByVal submitArgs As DAOCEventArgs)
        RaiseEvent OnPlayerQuit()
    End Sub
End Class