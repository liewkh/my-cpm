Imports Microsoft.VisualBasic

Public Class ItemReplacementSearchModel : Inherits CPM.ItemReplacementEntity
    Dim LocationInfoId As String = ""


    Public Function getLocationInfoId() As String
        Return LocationInfoId
    End Function

    Public Sub setLocationInfoId(ByVal LocationInfoId As String)
        Me.LocationInfoId = LocationInfoId
    End Sub
End Class
