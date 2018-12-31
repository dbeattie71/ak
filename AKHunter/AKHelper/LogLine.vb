Option Explicit On 

Friend Class LogLine
    Inherits Base

    Private _Name As String
    Private _RegEx As String

    Public Property Name() As String

        Get
            Return _Name
        End Get
        Set(ByVal Value As String)
            _Name = Value
        End Set

    End Property

    Public Property RegEx() As String

        Get
            Return _RegEx
        End Get
        Set(ByVal Value As String)
            _RegEx = Value
        End Set

    End Property

End Class
