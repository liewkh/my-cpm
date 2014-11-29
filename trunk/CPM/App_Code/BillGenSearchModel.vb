Imports Microsoft.VisualBasic

Public Class BillGenSearchModel : Inherits CPM.DebtorEntity
    Dim MonthFrom As String = ""
    Dim MonthTo As String = ""

    Public Function getMonthFrom() As String
        Return MonthFrom
    End Function

    Public Sub setMonthFrom(ByVal MonthFrom As String)
        Me.MonthFrom = MonthFrom
    End Sub

    Public Function getMonthTo() As String
        Return MonthTo
    End Function

    Public Sub setMonthto(ByVal MonthTo As String)
        Me.MonthTo = MonthTo
    End Sub

End Class
