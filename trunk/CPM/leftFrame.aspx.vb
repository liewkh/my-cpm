Imports System.Data

Partial Class topFrame
    Inherits System.Web.UI.Page

    Dim lp As New LoginProfile
    Dim dm As New DBManager


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'WebMenu1.MenuXMLPath = Server.MapPath("menudata.xml")
        'Load the Menu from database
        Dim sqlmap As New SQLMap
        Dim dsMenu As New DataSet
        Dim searchModelUM As New CPM.UserMstrEntity

        lp = Session("LoginProfile")

        searchModelUM.setAccessLevel(lp.getAccessLevel)
        searchModelUM.setUserMstrId(lp.getUserMstrId)

        Dim sqlStr1 As String = sqlmap.getMappedStatement("UserMstr/Search-UserMenuAccess", searchModelUM)
        dsMenu = dm.execDataSet(sqlStr1)

        If dsMenu.Tables(0).Rows.Count > 0 Then
            WebMenu1.DataSource = dsMenu
        End If



    End Sub
End Class
