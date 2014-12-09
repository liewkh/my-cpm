Imports Microsoft.VisualBasic
Imports System.Data
Imports CPM


Public Class LoginProfile
    Private dbManager As New DBManager
    Private userName As String
    Private userLoginId As String
    Private userMstrId As String
    Private employeeMstrId As String
    Private branchInfoId As String
    Private defaultLocationInfoId As String
    Private defaultBankCode As String


    Private selectedRoleName As String
    Private roles As New Hashtable
    Private isManagerOfRole As Boolean
    Private accessLevel As String


    Public Function authenticate(ByVal uName As String, ByVal passwd As String)
        Dim isAuthenticate As Boolean = False
        Dim dt As DataTable

        Dim searchModelUM As New CPM.UserMstrEntity
        Dim sqlmap As New SQLMap
        Dim strSQL As String = ""

        searchModelUM.setUserId(uName)
        searchModelUM.setPassword(passwd)


        Dim sqlString As String = sqlmap.getMappedStatement("UserMstr/Search-CheckUserNameExist", searchModelUM)
        dt = dbManager.execTable(sqlString)

        If dt.Rows.Count = 1 Then
            userName = uName

            'searchModelUM.setAccessId(dt.Rows(0).Item("ACCESSID").ToString)

            ''get all roles
            'Dim dt1 As DataTable
            'Dim sqlStr1 As String = sqlmap.getMappedStatement("UserMstr/Search-UserMenuAccess", searchModelUM)
            'dt1 = dbManager.execTable(sqlStr1)
            'Dim i As Integer = 0
            'While i < dt1.Rows.Count
            '    If dt1.Rows(i).Item("ISDEFAULT").Equals("Y") Then
            'userMstrId = dt1.Rows(i).Item("USERMSTRID").ToString.Trim
            accessLevel = dt.Rows(0).Item("ACCESSLEVEL").ToString
            userLoginId = dt.Rows(0).Item("USERID").ToString
            userName = dt.Rows(0).Item("EMPLOYEENAME").ToString
            userMstrId = dt.Rows(0).Item("USERMSTRID").ToString
            branchInfoId = dt.Rows(0).Item("BRANCHINFOID").ToString
            defaultLocationInfoId = dt.Rows(0).Item("DEFAULTLOCATIONINFOID").ToString
            employeeMstrId = dt.Rows(0).Item("EMPLOYEEMSTRID").ToString
            '        defaultRole = dt1.Rows(i).Item("ROLEMSTRID").ToString.Trim
            '        selectedRoleId = dt1.Rows(i).Item("ROLEMSTRID").ToString.Trim
            '        selectedRoleName = dt1.Rows(i).Item("ROLENAME").ToString.Trim
            '        If "Y".Equals(dt1.Rows(i).Item("ISMANAGER").ToString.Trim) = True Then
            '            isManagerOfRole = True
            '        Else
            '            isManagerOfRole = False
            '        End If
            '    End If
            '    roles.Add(dt1.Rows(i).Item("ROLEMSTRID").ToString.Trim, dt1.Rows(i).Item("ROLENAME").ToString.Trim)
            '    i = i + 1
            'End While

            If Not (String.IsNullOrEmpty(defaultLocationInfoId)) Then
                strSQL = "SELECT BANKCODE FROM LOCATIONINFO WHERE LOCATIONINFOID = " + defaultLocationInfoId
                dt = dbManager.execTable(strSQL)

                If dt.Rows.Count = 1 Then
                    defaultBankCode = dt.Rows(0).Item("BANKCODE").ToString
                End If
            End If

            isAuthenticate = True
        Else
            isAuthenticate = False
        End If
        'userName = uName
        Return isAuthenticate
    End Function

    Public Function getBranchInfoId() As String
        Return branchInfoId
    End Function

    Public Function getDefaultLocationInfoId() As String
        Return defaultLocationInfoId
    End Function

    Public Function getDefaultBankCode() As String
        Return defaultBankCode
    End Function
    Public Function getAccessLevel() As String
        Return accessLevel
    End Function

    Public Function getUserName() As String
        Return userName
    End Function
    Public Function getUserLoginId() As String
        Return userLoginId
    End Function
    Public Function getUserMstrId() As String
        Return userMstrId
    End Function
    Public Function getEmployeeMstrId() As String
        Return employeeMstrId
    End Function
    Public Function getRoles() As Hashtable
        Return roles
    End Function

    Public Function getSelectedRoleName() As String
        Return selectedRoleName
    End Function

    Public Function isManager() As Boolean
        Return isManagerOfRole
    End Function

    Public Function checkAccessRight(ByVal roleId As Int64, ByVal screenName As String, ByVal rightType As String)
        Dim isAccessible As Boolean = False
        Dim dt As DataTable

        Dim sqlString As String = "SELECT RM." + rightType + "ABLE " & _
                    "FROM ROLEMENU RM, MENUMSTR MM " & _
                    "WHERE RM.MENUMSTRID=MM.MENUMSTRID " & _
                    "AND RM.ROLEMSTRID='" & roleId & _
                    "' AND MM.Description='" & screenName & "'"

        dt = dbManager.execTable(sqlString)

        If dt.Rows.Count = 1 Then
            If "Y".Equals(dt.Rows(0).Item(rightType + "ABLE")) Then
                isAccessible = True
            End If
        End If

        Return isAccessible
    End Function

End Class
