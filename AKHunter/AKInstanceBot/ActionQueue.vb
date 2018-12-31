Option Explicit On 

Imports System.Collections

Namespace AKInstanceBot

    Public Class ActionQueue

        Private _Actions As ArrayList

        Public Sub New()

            _Actions = New ArrayList

        End Sub

        Public Sub Enqueue(ByVal Action As BotAction)

            _Actions.Add(Action)

        End Sub

        Public Function Dequeue() As BotAction

            Dim action As BotAction

            If _Actions.Count = 0 Then
                Throw New Exception("Can't dequeue botaction because the queue is empty.")
            End If

            action = _Actions(0)
            _Actions.RemoveAt(0)

            Return action

        End Function

        Public Function Peek(ByVal Index As Integer) As BotAction

            If (Index > _Actions.Count - 1) Then
                Throw New Exception("Can't Peek because the index is greater than the number of actions in the queue.")
            End If

            Return _Actions(Index)

        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal Action As BotAction)

            _Actions.Insert(Index, Action)

        End Sub

        Public Sub Clear()

            _Actions.Clear()

        End Sub

        Public Function Count() As Integer

            Return _Actions.Count

        End Function

    End Class

End Namespace