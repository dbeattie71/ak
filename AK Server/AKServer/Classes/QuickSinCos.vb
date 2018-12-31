Public Class QuickSinCos
    '{ NOTE: angle is in degrees! }
    Private Const TRIG_TABLE_SIZE As Integer = 128
    Private Const TRIG_TABLE_LERP As Single = (TRIG_TABLE_SIZE / 180.0)
    Private Shared COS_TABLE(TRIG_TABLE_SIZE) As Double
    Private Shared SIN_TABLE(TRIG_TABLE_SIZE) As Double
    Private Shared IsInitialized As Boolean = False

    Public Shared Function cos_quick(ByVal a As Integer) As Double
        Dim iQuantized As Integer
        If Not IsInitialized Then BuildSinCosTables()
        iQuantized = CInt(Math.Round(a * TRIG_TABLE_LERP))

        If iQuantized < 0 Then
            iQuantized = -iQuantized
        End If
        If iQuantized > TRIG_TABLE_SIZE Then
            iQuantized = iQuantized Mod (2 * TRIG_TABLE_SIZE)
            If iQuantized > TRIG_TABLE_SIZE Then
                iQuantized = (2 * TRIG_TABLE_SIZE) - iQuantized
            End If
        End If
        Return COS_TABLE(iQuantized)
    End Function

    Public Shared Function sin_quick(ByVal a As Integer) As Double
        Dim iQuantized As Integer
        Dim bNegative As Boolean
        If Not IsInitialized Then BuildSinCosTables()
        iQuantized = CInt(Math.Round(a * TRIG_TABLE_LERP))

        If iQuantized < 0 Then
            iQuantized = -iQuantized
            bNegative = True
        Else
            bNegative = False
        End If
        If iQuantized > TRIG_TABLE_SIZE Then
            iQuantized = iQuantized Mod (2 * TRIG_TABLE_SIZE)
            If iQuantized > TRIG_TABLE_SIZE Then
                iQuantized = (2 * TRIG_TABLE_SIZE) - iQuantized
                bNegative = Not bNegative
            End If
        End If
        If bNegative Then
            Return -SIN_TABLE(iQuantized)
        Else
            Return SIN_TABLE(iQuantized)
        End If
    End Function

    Public Shared Sub sincos_quick(ByVal a As Integer, ByRef s As Double, ByRef c As Double)
        Dim iQuantized As Integer
        Dim bNegativeS As Boolean
        If Not IsInitialized Then BuildSinCosTables()

        iQuantized = CInt(Math.Round(a * TRIG_TABLE_LERP))

        If iQuantized < 0 Then
            iQuantized = -iQuantized
            bNegativeS = True
        Else
            bNegativeS = False
        End If

        If iQuantized > TRIG_TABLE_SIZE Then
            iQuantized = iQuantized Mod (2 * TRIG_TABLE_SIZE)
            If iQuantized > TRIG_TABLE_SIZE Then
                iQuantized = (2 * TRIG_TABLE_SIZE) - iQuantized
                bNegativeS = Not bNegativeS
            End If
        End If

        c = COS_TABLE(iQuantized)
        If bNegativeS Then
            s = -SIN_TABLE(iQuantized)
        Else
            s = SIN_TABLE(iQuantized)
        End If
    End Sub

    Private Shared Sub BuildSinCosTables()
        Dim I As Integer
        For I = 0 To TRIG_TABLE_SIZE
            COS_TABLE(I) = Math.Cos(Math.PI * I / TRIG_TABLE_SIZE)
            SIN_TABLE(I) = Math.Sin(Math.PI * I / TRIG_TABLE_SIZE)
        Next
        IsInitialized = True
    End Sub

End Class
