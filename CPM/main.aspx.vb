
Partial Class main
    Inherits System.Web.UI.Page

    'Dim lp As New LoginProfile()
    'Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    '    lp = Session("LoginProfile")
    '    If IsNothing(lp) Then
    '        Response.Redirect(Request.ApplicationPath + "/index.aspx?login=expired")
    '    End If
    'End Sub

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    If Not Page.IsPostBack Then
    '        'Dim strSQL As String = "SELECT ReminderListId, MenuMstrId, ReferenceId, ReferenceStatus, ActivateDate, Action,(DAY(GETDATE() - ActivateDate) - Duration) AS NoOfDaysExp FROM ReminderList TDL, USERMSTR UM  WHERE TDL.USERMSTRID=UM.USERMSTRID AND UserName='" + Trim(lp.getUserName) + "' "
    '        dvBind()

    '        Dim cn As Data.SqlClient.SqlConnection
    '        Dim trans As Data.SqlClient.SqlTransaction
    '        Dim cmd_insert_update As Data.SqlClient.SqlCommand
    '        Dim dm As New DBManager()
    '        Dim conString As String = dm.getDBConn()

    '        Dim strSQLUpdate As String = "UPDATE ReminderList SET " & _
    '                   "NewReminderInd='N' " & _
    '                   "WHERE UserMstrId = " & lp.getUserMstrId

    '        cn = New Data.SqlClient.SqlConnection(conString)
    '        If Not cn.State = Data.ConnectionState.Open Then
    '            cn.Open()
    '        End If
    '        trans = cn.BeginTransaction
    '        Try
    '            cmd_insert_update = New Data.SqlClient.SqlCommand(strSQLUpdate, cn, trans)

    '            cmd_insert_update.ExecuteNonQuery()
    '            trans.Commit()

    '            cmd_insert_update.Dispose()
    '            strSQLUpdate = ""
    '        Catch ex As Exception
    '            lblmsg.Text = ex.Message
    '            trans.Rollback()
    '            Exit Sub
    '        Finally
    '            trans.Dispose()
    '            cn.Close()
    '        End Try
    '    End If
    'End Sub

    'Protected Sub gvToDoList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvToDoList.RowCommand
    '    'If e.CommandName.Equals("Sort") Then
    '    dvBind()
    '    'End If
    'End Sub
    'Protected Sub gvToDoList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvToDoList.RowDataBound
    '    If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then

    '        If Not String.IsNullOrEmpty(gvToDoList.DataKeys(gvToDoList.Rows.Count).Item("ReminderUrl").ToString) Then
    '            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
    '            'e.Row.Cells(0).Controls(0).Visible = False
    '            e.Row.Cells(0).FindControl("btnDelete").Visible = False
    '            e.Row.Attributes.Add("Onclick", "javascript:location.href('" + gvToDoList.DataKeys(gvToDoList.Rows.Count).Item("ReminderUrl").ToString + "&key=" + gvToDoList.DataKeys(gvToDoList.Rows.Count).Item("ReferenceId").ToString + "');")
    '            Dim int As Integer = gvToDoList.Rows.Count / 2
    '            Dim dob As Double = gvToDoList.Rows.Count / 2

    '            If (dob.Equals(int)) Then
    '                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='tb-row1';")
    '            Else
    '                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='tb-row2';")
    '            End If
    '            'Else
    '            'e.Row.Cells(0)..Add("Onclick", "javascript:return confirm('Are you sure want to remove this reminder?');")
    '        End If

    '    End If
    'End Sub


    'Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
    '    txtDaysOfExp.Text = ""
    '    ddScreenName.SelectedValue = ""
    '    lblmsg.Text = ""
    'End Sub

    'Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
    '    dvBind()
    '    If gvToDoList.Rows.Count = 0 Then
    '        lblmsg.Text = "Record not found."
    '    Else
    '        lblmsg.Text = DBManager.getGridViewRecordCount(dsToDoList).ToString + " record(s) found."
    '    End If
    'End Sub
    'Public Sub dvBind()

    '    Dim lp As New LoginProfile
    '    lp = Session("LoginProfile")

    '    'Dim strSQL As String = "SELECT ReminderListId, MenuMstrId, ReferenceId, ReferenceStatus, ActivateDate, Action, (DAY(GETDATE() - ActivateDate) - Duration) AS NoOfDaysExp FROM ReminderList TDL, USERMSTR UM  WHERE TDL.USERMSTRID=UM.USERMSTRID AND  UserName='" + lp.getUserName + "' "
    '    Dim strSQL As String = "SELECT ReminderListId, MaidCode, EmployerName, RL.MenuMstrId, ReferenceId, " & _
    '                            "ReferenceStatus, ReminderUrl, MM.Description AS ScreenName, " & _
    '                            "ExpiryDate, ActivateDate, Action, " & _
    '                            "(DATEDIFF(dd,ExpiryDate,GETDATE())+1) AS NoOfDaysExp " & _
    '                            "FROM ReminderList RL, USERMSTR UM, MenuMstr MM " & _
    '                            "WHERE(RL.USERMSTRID = UM.USERMSTRID) " & _
    '                            "AND RL.MENUMSTRID=MM.MenuMstrId " & _
    '                            "AND (DATEDIFF(dd,ActivateDate,GETDATE())) >= 0 " & _
    '                            "AND UserName='" + lp.getUserName + "' " & _
    '                            "AND RoleMstrId =" + lp.getSelectedRoleId

    '    If (Not String.IsNullOrEmpty(txtDaysOfExp.Text)) Then
    '        strSQL = strSQL + "AND (DATEDIFF(dd,ExpiryDate,GETDATE())) >=" + txtDaysOfExp.Text
    '    End If
    '    If Not ((String.IsNullOrEmpty(ddScreenName.SelectedValue)) Or (ddScreenName.SelectedIndex = 0)) Then
    '        strSQL = strSQL + "AND RL.MenuMstrId =" + Trim(ddScreenName.SelectedValue)
    '    End If
    '    dsToDoList.SelectCommand = strSQL

    '    'dsToDoList.DataBind()
    '    gvToDoList.DataBind()

    'End Sub

    'Sub gvToDoList_RowDeleted(ByVal sender As Object, ByVal e As GridViewDeletedEventArgs)
    '    If e.Exception Is Nothing Then
    '        If e.AffectedRows = 1 Then
    '            lblmsg.Text = "Record deleted successfully."
    '            dvBind()
    '        Else
    '            lblmsg.Text = "An error occurred during the delete operation."
    '        End If
    '    Else
    '        lblmsg.Text = "An error occurred during the delete operation."
    '        e.ExceptionHandled = True
    '    End If
    'End Sub

End Class
