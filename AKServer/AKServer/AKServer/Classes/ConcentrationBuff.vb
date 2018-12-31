Imports System.ComponentModel

Namespace DAoCServer
    <Serializable()> _
    Public Class ConcentrationBuff
        Private mName As String
        Private mIndex As Integer
        Private mCon As Integer
        Private mID As Integer
        Private mTarget As String

        Public Property Index() As Integer
            Get
                Return mIndex
            End Get
            Set(ByVal Value As Integer)
                mIndex = Value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return mName
            End Get
            Set(ByVal Value As String)
                mName = Value
            End Set
        End Property

        Public Property ID() As Integer
            Get
                Return mID
            End Get
            Set(ByVal Value As Integer)
                mID = Value
            End Set
        End Property

        Public Property Target() As String
            Get
                Return mTarget
            End Get
            Set(ByVal Value As String)
                mTarget = Value
            End Set
        End Property

        Public Property Con() As Integer
            Get
                Return mCon
            End Get
            Set(ByVal Value As Integer)
                mCon = Value
            End Set
        End Property

    End Class
    <Serializable()> _
        Public Class ConcentrationBuffList
        Inherits List(Of ConcentrationBuff)

        Default Public Shadows Property Item(ByVal Index As Integer) As ConcentrationBuff
            Get
                Return CType(MyBase.Item(Index), ConcentrationBuff)
            End Get
            Set(ByVal Value As ConcentrationBuff)
                MyBase.Item(Index) = Value
            End Set
        End Property

        Public Function FindOrAddConcentrationBuff(ByVal AName As String) As ConcentrationBuff
            Dim i As Integer
            Dim aBuff As ConcentrationBuff

            For i = 0 To Count - 1
                aBuff = DirectCast(MyBase.Item(i), ConcentrationBuff)
                If aBuff.Name = AName Then
                    Return aBuff
                End If
            Next

            aBuff = New ConcentrationBuff
            aBuff.Name = AName
            Add(aBuff)

            Return aBuff
        End Function

        Public Shadows Sub Clear()
            MyBase.Clear()
        End Sub

    End Class

End Namespace
