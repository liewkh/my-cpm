Imports Microsoft.VisualBasic
Imports System

Public Class SeasonTypeSearchModel : Inherits CPM.SeasonTypeMstrEntity
    Dim locationId As String = ""


    Public Function getLocationId() As String
        Return locationId
    End Function

    Public Sub setLocationId(ByVal LocationId As String)
        Me.locationId = LocationId
    End Sub
End Class
