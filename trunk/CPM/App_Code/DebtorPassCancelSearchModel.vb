Imports Microsoft.VisualBasic

Public Class DebtorPassCancelSearchModel : Inherits CPM.DebtorPassBayNoCancelEntity
    Dim MonthFrom As String = ""
    Dim YearFrom As String = ""
    Dim Name As String = ""
    Dim Category As String = ""
    Dim LocationInfoId As String = ""

    Public Function getMonthFrom() As String
        Return MonthFrom
    End Function

    Public Sub setMonthFrom(ByVal MonthFrom As String)
        Me.MonthFrom = MonthFrom
    End Sub

    Public Function getYearFrom() As String
        Return YearFrom
    End Function

    Public Sub setYearFrom(ByVal YearFrom As String)
        Me.YearFrom = YearFrom
    End Sub

    Public Function getName() As String
        Return Name
    End Function

    Public Sub setName(ByVal Name As String)
        Me.Name = Name
    End Sub

    Public Function getCategory() As String
        Return Category
    End Function

    Public Sub setCategory(ByVal Category As String)
        Me.Category = Category
    End Sub

    Public Function getLocationInfoId() As String
        Return LocationInfoId
    End Function

    Public Sub setLocationInfoId(ByVal LocationInfoId As String)
        Me.LocationInfoId = LocationInfoId
    End Sub

End Class
