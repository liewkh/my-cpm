Imports Microsoft.VisualBasic

Public Class dailyCollectionSearchModel : Inherits CPM.DailyCollectionEntity
    Dim TransactionDate As String = ""
    Dim locationId As String = ""


    Public Overloads Function getTransactionDate() As String
        Return TransactionDate
    End Function

    Public Overloads Sub setTransactionDate(ByVal TransactionDate As String)
        Me.TransactionDate = TransactionDate
    End Sub

    Public Overloads Function getLocationId() As String
        Return locationId
    End Function

    Public Overloads Sub setLocationId(ByVal LocationId As String)
        Me.locationId = LocationId
    End Sub


End Class
