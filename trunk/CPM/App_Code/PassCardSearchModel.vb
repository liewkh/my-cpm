Imports Microsoft.VisualBasic

Public Class PassCardSearchModel : Inherits CPM.PassCardMstrEntity
    Dim Category As String = ""
    Dim Debtor As String = ""
    Dim LocationId As String = ""

    Public Function getLocationId() As String
        Return LocationId
    End Function

    Public Sub setLocationId(ByVal LocationId As String)
        Me.LocationId = LocationId
    End Sub

    Public Function getCategory() As String
        Return Category
    End Function

    Public Sub setCategory(ByVal Category As String)
        Me.Category = Category
    End Sub

    Public Function getDebtor() As String
        Return Debtor
    End Function

    Public Sub setDebtor(ByVal Debtor As String)
        Me.Debtor = Debtor
    End Sub

End Class
