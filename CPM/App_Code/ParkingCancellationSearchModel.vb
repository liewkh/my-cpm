Imports Microsoft.VisualBasic

Public Class ParkingCancellationSearchModel : Inherits CPM.DebtorPassBayNoCancelEntity
    Dim LocationInfoId As String = ""
    Dim Name As String = ""
    Dim Category As String = ""

    Public Function getLocationInfoId() As String
        Return LocationInfoId
    End Function

    Public Sub setLocationInfoId(ByVal LocationInfoId As String)
        Me.LocationInfoId = LocationInfoId
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

End Class
