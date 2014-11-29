Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Partial Class Maintenance_AccessLevel
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        lp = Session("LoginProfile")
        If IsNothing(lp) Then
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("appPath") + "/login.aspx?login=expired")
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = 0
        Response.Cache.SetNoStore()
        Response.AppendHeader("Pragma", "no-cache")

        If Not Page.IsPostBack Then
            'ddLvl.SelectedValue = lp.getAccessLevel
            'bindSelectionList()
        End If

    End Sub

    Protected Sub bindSelectionList()
        Dim sql As String

        Try
            If Not Session("tempMenuItem") Is Nothing Then
                Dim lb As New ListBox
                lb = Session("tempMenuItem")

                For Each item As ListItem In lb.Items
                    lstSelection.Items.Add(item)
                Next



                sql = "select m.menuid as value,isnull((select name from menumstr where menuid = m.root) + ' - ' + m.name,m.name) as display from menumstr m order by m.root,m.seq"
                'Load Available Location
                dsLocation.SelectCommand = sql
                lstLocation.DataTextField = "Display"
                lstLocation.DataValueField = "Value"
                lstLocation.DataBind()

                removeSelectedItem()

            Else

                'Load Selected Location
                sql = "select ua.menuid as value,isnull((select name from menumstr where menuid = m.root) + ' - ' + m.name,m.name) as display from useraccess ua,menumstr m " & _
                      " where(m.menuid = ua.menuid and accessid = " & ddLvl.SelectedValue & ") order by m.root,m.seq"
                dsSelection.SelectCommand = sql
                lstSelection.DataTextField = "Display"
                lstSelection.DataValueField = "Value"
                lstSelection.DataBind()


                sql = "select m.menuid as value,isnull((select name from menumstr where menuid = m.root) + ' - ' + m.name,m.name) as display from menumstr m where m.menuid not in " & _
                      "(select menuid from useraccess where accessid = " & ddLvl.SelectedValue & ") order by m.root,m.seq"
                'Load Available Location
                dsLocation.SelectCommand = sql
                lstLocation.DataTextField = "Display"
                lstLocation.DataValueField = "Value"
                lstLocation.DataBind()

                removeSelectedItem()

            End If

        Catch ex As Exception
            logger.Error(ex.Message)
        End Try



    End Sub

    Protected Sub btnAddItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddItem.Click
        Dim li As New ListItem
        Dim valueArr As String()

        'If Not li Is Nothing Then
        '    For intLoopIndex As Integer = 0 To lstLocation.Items.Count
        '        If lstLocation.Items(intLoopIndex).Selected Then
        '            li = lstLocation.Items(intLoopIndex)
        '            valueArr = li.Value.Split("/")
        '            If String.IsNullOrEmpty(valueArr(1)) Then
        '                lstSelection.Items.Add(li)
        '                lstLocation.Items.Remove(li)
        '                InsertRecords(li)
        '            End If
        '        End If
        '        On Error GoTo 0
        '    Next

        'End If
        Dim i As Integer

        If lstLocation.SelectedIndex = -1 Then Exit Sub

        For i = lstLocation.Items.Count - 1 To 0 Step -1
            If lstLocation.Items(i).Selected = True Then
                li = lstLocation.Items(i)
                valueArr = li.Value.Split("/")
                lstSelection.Items.Add(li)
                lstLocation.Items.Remove(li)
                If ddLvl.SelectedValue <> "" Then
                    InsertRecords(li)
                Else
                    'Set In to Session
                    Session("tempMenuItem") = lstSelection
                End If
            End If
        Next i


        lstSelection.SelectedIndex = -1
    End Sub

    Protected Sub btnRemoveItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveItem.Click
        Dim li As ListItem
        Dim valueArr As String()
        'li = lstSelection.SelectedItem


        'If Not li Is Nothing Then
        '    Dim valueArr As String()
        '    valueArr = lstSelection.SelectedItem.Value.Split("/")

        '    If Not String.IsNullOrEmpty(valueArr(1)) Then
        '        If lstLocation.Items.Count > 0 Then
        '            lstLocation.Items.Add(li)
        '        End If
        '        lstSelection.Items.Remove(li)
        '        removeRecords(li)
        '    Else
        '        lstSelection.Items.Remove(li)
        '        removeRecords(li)
        '    End If
        'End If

        Dim i As Integer

        If lstSelection.SelectedIndex = -1 Then Exit Sub

        For i = lstSelection.Items.Count - 1 To 0 Step -1
            If lstSelection.Items(i).Selected = True Then
                li = lstSelection.Items(i)
                valueArr = li.Value.Split("/")
                lstLocation.Items.Add(li)
                lstSelection.Items.Remove(li)
                If ddLvl.SelectedValue <> "" Then
                    removeRecords(li)
                Else
                    'Set In to Session
                    Session("tempMenuItem") = lstSelection
                End If
            End If
        Next i


        lstSelection.SelectedIndex = -1

    End Sub


    Protected Sub removeRecords(ByVal li As ListItem)
        Dim docDao As New CPM.EmployeeLocationDAO()
        Dim id As String = li.Value.Split("/")(0)

        Dim sql As String = "DELETE USERACCESS WHERE MENUID = " & id & " AND ACCESSID = " & ddLvl.SelectedValue

        cn = New SqlConnection(dm.getDBConn)
        If Not cn.State = ConnectionState.Open Then
            cn.Open()
        End If
        trans = cn.BeginTransaction

        Try
            dm.execTableInTrans(sql, cn, trans)
            trans.Commit()

        Catch ex As Exception
            lblmsg.Text = ex.Message
            trans.Rollback()
        Finally
            trans.Dispose()
            cn.Close()
        End Try

    End Sub

    Protected Sub InsertRecords(ByVal li As ListItem)
        Dim uaEnt As New CPM.UserAccessEntity
        Dim uaDao As New CPM.UserAccessDAO

        cn = New SqlConnection(dm.getDBConn)
        If Not cn.State = ConnectionState.Open Then
            cn.Open()
        End If
        trans = cn.BeginTransaction

        Try
            uaEnt.setMenuId(li.Value.Split("/")(0))
            uaEnt.setAccessId(ddLvl.SelectedValue)
            uaEnt.setLastUpdatedDatetime(Now)
            uaEnt.setLastUpdatedBy(lp.getUserMstrId)
            uaDao.insertDB(uaEnt, cn, trans)
            trans.Commit()

        Catch ex As Exception
            lblmsg.Text = ex.Message
            trans.Rollback()
        Finally
            trans.Dispose()
            cn.Close()
        End Try


    End Sub


    Protected Sub removeSelectedItem()
        Dim valueArr As String()

        Dim liMat As ListItem
        For Each li As ListItem In lstSelection.Items
            valueArr = li.Value.Split("/")
            liMat = lstLocation.Items.FindByValue(valueArr(0))
            If Not liMat Is Nothing Then
                If li.Enabled Then
                    liMat.Value = li.Value
                    liMat.Enabled = False
                End If
            End If
        Next
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        If Not (String.IsNullOrEmpty(lblmsg.Text)) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "script1", "location.href='#top';", True)
        End If
    End Sub

    Protected Sub ddLvl_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        bindSelectionList()
    End Sub
End Class
