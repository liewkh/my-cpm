Imports Microsoft.VisualBasic
Imports System


Public Class EmpSearchModel : Inherits CPM.EmployeeMstrEntity

    Dim UserId As String = ""
    Dim Password As String = ""


    Public Function getUserId() As String
        Return UserId
    End Function

    Public Sub setUserId(ByVal UserId As String)
        Me.UserId = UserId
    End Sub

    Public Function getPassword() As String
        Return Password
    End Function

    Public Sub setPassword(ByVal Password As String)
        Me.Password = Password
    End Sub

End Class
