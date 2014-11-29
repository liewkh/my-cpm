Imports Microsoft.VisualBasic
Imports System

Public Class DebtorSearchModel : Inherits CPM.DebtorEntity
    Dim locationId As String = ""
    Dim serialNo As String = ""


    Public Function getLocationId() As String
        Return locationId
    End Function

    Public Sub setLocationId(ByVal LocationId As String)
        Me.locationId = LocationId
    End Sub

    Public Function getSerialNo() As String
        Return serialNo
    End Function

    Public Sub setSerialNo(ByVal SerialNo As String)
        Me.serialNo = SerialNo
    End Sub
End Class
