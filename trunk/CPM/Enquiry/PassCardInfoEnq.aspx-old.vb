Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Enquiry_PassCardInfoEnq
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Dim theID As String

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        lp = Session("LoginProfile")
        If IsNothing(lp) Then
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("appPath") + "/login.aspx?login=expired")
        End If
        If Not Page.IsPostBack Then


            Dim sql As String = "select locationinfoid,locationName,0 as seq from locationinfo where locationinfoid = " & lp.getDefaultLocationInfoId & _
                                "union select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' " & _
                                "union select el.locationinfoid,li.locationname,0 as seq from employeelocation el,locationinfo li " & _
                                "where el.locationinfoid = li.locationinfoid and el.employeemstrid = " & lp.getEmployeeMstrId & " order by seq,locationname"
            dsLocation.SelectCommand = sql
            dsLocation.DataBind()

            ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        End If

        Session.LCID = 2057

    End Sub

    Private Sub bindData()
        Dim searchModel As New PassCardSearchModel
        Dim passCardMstrDao As New CPM.PassCardMstrDAO
        Dim sqlmap As New SQLMap
        Dim strSQL As String

        Try

            searchModel.setSerialNo(Trim(txtPassCardSN.Text.ToUpper))
            searchModel.setItemType(ddItemType.SelectedValue)
            searchModel.setLocationInfoId(ddLocation.SelectedValue)
            searchModel.setStatus(ddStatus.SelectedValue)

            If rbCompany.Checked = True Then
                searchModel.setCategory(CategoryEnum.COMPANY)
            Else
                searchModel.setCategory(CategoryEnum.INDIVIDUAL)
            End If

            If Trim(txtDebtorName.Text) <> "" Then
                searchModel.setDebtor(Trim(txtDebtorName.Text.ToUpper))
                strSQL = sqlmap.getMappedStatement("PassCardMstr/Search-PassCardMstrDebtor", searchModel)
            Else
                strSQL = sqlmap.getMappedStatement("PassCardMstr/Search-PassCardMstr", searchModel)
            End If


            ViewState("strSQL") = strSQL

            dsPassCard.SelectCommand = ViewState("strSQL")
            gvPassCardEnq.DataBind()

            gvPassCardEnq.PageIndex = 0

            If gvPassCardEnq.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsPassCard).ToString + " " + "Record Found"
            End If

        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            passCardMstrDao = Nothing
            searchModel = Nothing
            sqlmap = Nothing

        End Try

    End Sub

    Private Sub clear()
        lblmsg.Text = ""
        lblRecCount.Text = ""
        txtPassCardSN.Text = ""
        txtDebtorName.Text = ""
        hidPassCardMstrId.value = ""
        ddItemType.SelectedIndex = 0
        ddStatus.SelectedIndex = 0
        rbCompany.Checked = True
        rbIndividual.Checked = False
        gvPassCardEnq.DataSource = Nothing
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        bindData()
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        clear()
        ViewState("strSQL") = Nothing
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvPassCardEnq.Rows.Count
            ClientScript.RegisterForEventValidation(gvPassCardEnq.UniqueID, "Select$" + i.ToString)
        Next

        MyBase.Render(writer)
    End Sub

    Protected Sub gvPassCardEnq_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPassCardEnq.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvPassCardEnq.UniqueID + "','Select$" + gvPassCardEnq.Rows.Count.ToString + "');")
            Dim int As Integer = gvPassCardEnq.Rows.Count / 2
            Dim dob As Double = gvPassCardEnq.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

            Dim history As Button = CType(e.Row.FindControl("btnHistory"), Button)
            Dim a As String = ""
            a = Convert.ToString(e.Row.DataItem("PASSCARDMSTRID").ToString)

            history.Attributes.Add("onclick", "javascript:window.open('../Enquiry/PassCardInfoHist.aspx?PassCardMstrId=" + a.ToString + "&ItemType=" + ddItemType.SelectedItem.Text + "&PassCard=" + txtPassCardSN.Text + "',menubar=1,resizable=1,width=800,height=600);")
        End If
      

    End Sub

    Protected Sub gvPassCardEnq_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPassCardEnq.SelectedIndexChanged
        Dim passCardMstrDao As New CPM.PassCardMstrDAO

        Try

            lblmsg.Text = ""
            txtPassCardSN.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassCardEnq.SelectedDataKey(passCardMstrDao.COLUMN_SerialNo))
            ddItemType.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassCardEnq.SelectedDataKey(passCardMstrDao.COLUMN_ItemType))
            ddLocation.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassCardEnq.SelectedDataKey(passCardMstrDao.COLUMN_LocationInfoId))
            ddStatus.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassCardEnq.SelectedDataKey(passCardMstrDao.COLUMN_Status))
            'hidPassCardMstrId.value = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassCardEnq.SelectedDataKey(passCardMstrDao.COLUMN_PassCardMstrID))

            If gvPassCardEnq.SelectedDataKey("CATEGORY").Equals(CategoryEnum.COMPANY) Then
                rbCompany.Checked = True
                rbIndividual.Checked = False
            Else
                rbCompany.Checked = False
                rbIndividual.Checked = True
            End If

        Catch ex As Exception
            Throw ex
        Finally
            passCardMstrDao = Nothing
        End Try

    End Sub

    Protected Sub gvPassCardEnq_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPassCardEnq.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindData()
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub

End Class
