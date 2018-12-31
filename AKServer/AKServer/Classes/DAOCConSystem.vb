Namespace DAoCServer
    Public NotInheritable Class ConColors
        Public Enum DAOCConColor
            Gray
            Green
            Blue
            Yellow
            Orange
            Red
            Purple
        End Enum

        Public Class DAOCConRangeDefinition
            Public GrayMax As Short
            Public GreenMax As Short
            Public BlueMax As Short
            Public YellowMax As Short
            Public OrangeMax As Short
            Public RedMax As Short
            Public Sub New(ByVal _GrayMax As Short, ByVal _GreenMax As Short, ByVal _BlueMax As Short, ByVal _YellowMax As Short, ByVal _OrangeMax As Short, ByVal _RedMax As Short)
                GrayMax = _GrayMax
                GreenMax = _GreenMax
                BlueMax = _BlueMax
                YellowMax = _YellowMax
                OrangeMax = _OrangeMax
                RedMax = _RedMax
            End Sub
        End Class

        Public Shared CON_RANGES() As DAOCConRangeDefinition = { _
            New DAOCConRangeDefinition(0, 0, 0, 0, 0, 0), _
            New DAOCConRangeDefinition(-1, -1, 0, 1, 2, 3), _
            New DAOCConRangeDefinition(-1, 0, 1, 2, 3, 4), _
            New DAOCConRangeDefinition(0, 1, 2, 3, 4, 5), _
            New DAOCConRangeDefinition(1, 2, 3, 4, 5, 6), _
            New DAOCConRangeDefinition(2, 3, 4, 5, 6, 7), _
            New DAOCConRangeDefinition(3, 4, 5, 6, 7, 8), _
            New DAOCConRangeDefinition(4, 5, 6, 7, 8, 9), _
            New DAOCConRangeDefinition(5, 6, 7, 8, 9, 10), _
            New DAOCConRangeDefinition(6, 7, 8, 9, 10, 11), _
            New DAOCConRangeDefinition(6, 7, 9, 10, 11, 13), _
            New DAOCConRangeDefinition(6, 7, 9, 11, 13, 15), _
            New DAOCConRangeDefinition(6, 8, 10, 12, 14, 16), _
            New DAOCConRangeDefinition(7, 9, 11, 13, 15, 17), _
            New DAOCConRangeDefinition(8, 10, 12, 14, 16, 18), _
            New DAOCConRangeDefinition(9, 11, 13, 15, 17, 19), _
            New DAOCConRangeDefinition(10, 12, 14, 16, 18, 20), _
            New DAOCConRangeDefinition(11, 13, 15, 17, 19, 21), _
            New DAOCConRangeDefinition(12, 14, 16, 18, 20, 22), _
            New DAOCConRangeDefinition(13, 15, 17, 19, 21, 23), _
            New DAOCConRangeDefinition(13, 15, 18, 20, 22, 25), _
            New DAOCConRangeDefinition(13, 15, 18, 21, 24, 27), _
            New DAOCConRangeDefinition(13, 16, 19, 22, 25, 28), _
            New DAOCConRangeDefinition(14, 17, 20, 23, 26, 29), _
            New DAOCConRangeDefinition(15, 18, 21, 24, 27, 30), _
            New DAOCConRangeDefinition(16, 19, 22, 25, 28, 31), _
            New DAOCConRangeDefinition(17, 20, 23, 26, 29, 32), _
            New DAOCConRangeDefinition(18, 21, 24, 27, 30, 33), _
            New DAOCConRangeDefinition(19, 22, 25, 28, 31, 34), _
            New DAOCConRangeDefinition(20, 23, 26, 29, 32, 35), _
            New DAOCConRangeDefinition(21, 24, 27, 30, 33, 36), _
            New DAOCConRangeDefinition(22, 25, 28, 31, 34, 37), _
            New DAOCConRangeDefinition(23, 26, 29, 32, 35, 38), _
            New DAOCConRangeDefinition(24, 27, 30, 33, 36, 39), _
            New DAOCConRangeDefinition(25, 28, 31, 34, 37, 40), _
            New DAOCConRangeDefinition(25, 28, 31, 35, 39, 42), _
            New DAOCConRangeDefinition(25, 28, 31, 36, 41, 45), _
            New DAOCConRangeDefinition(25, 29, 32, 37, 42, 47), _
            New DAOCConRangeDefinition(25, 29, 33, 38, 43, 48), _
            New DAOCConRangeDefinition(25, 29, 34, 39, 44, 49), _
            New DAOCConRangeDefinition(25, 30, 35, 40, 45, 50), _
            New DAOCConRangeDefinition(26, 31, 36, 41, 46, 51), _
            New DAOCConRangeDefinition(27, 32, 37, 42, 47, 52), _
            New DAOCConRangeDefinition(28, 33, 38, 43, 48, 53), _
            New DAOCConRangeDefinition(29, 34, 39, 44, 49, 54), _
            New DAOCConRangeDefinition(30, 35, 40, 45, 50, 55), _
            New DAOCConRangeDefinition(31, 36, 41, 46, 51, 56), _
            New DAOCConRangeDefinition(32, 37, 42, 47, 52, 57), _
            New DAOCConRangeDefinition(33, 38, 43, 48, 53, 58), _
            New DAOCConRangeDefinition(34, 39, 44, 49, 54, 59), _
            New DAOCConRangeDefinition(35, 40, 45, 50, 55, 60) _
        }

        Public Shared Function GetConColor(ByVal aViewerLvl As Integer, ByVal aTargetLvl As Integer) As DAOCConColor
            If (aViewerLvl < CON_RANGES.GetLowerBound(0)) Or (aViewerLvl > CON_RANGES.GetUpperBound(0)) Then
                Return DAOCConColor.Gray
            Else
                With DirectCast(CON_RANGES.GetValue(aViewerLvl), DAOCConRangeDefinition)
                    If aTargetLvl <= .GrayMax Then
                        Return DAOCConColor.Gray
                    ElseIf aTargetLvl <= .GreenMax Then
                        Return DAOCConColor.Green
                    ElseIf aTargetLvl <= .BlueMax Then
                        Return DAOCConColor.Blue
                    ElseIf aTargetLvl <= .YellowMax Then
                        Return DAOCConColor.Yellow
                    ElseIf aTargetLvl <= .OrangeMax Then
                        Return DAOCConColor.Orange
                    ElseIf aTargetLvl <= .RedMax Then
                        Return DAOCConColor.Red
                    Else
                        Return DAOCConColor.Purple
                    End If
                End With
            End If
        End Function
    End Class
End Namespace
