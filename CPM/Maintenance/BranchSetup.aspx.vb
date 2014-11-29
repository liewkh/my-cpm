Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Maintenance_BranchSetup
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

    Private Sub bindData()
        Dim searchModel As New CPM.BranchInfoEntity
        Dim BranchInfoDao As New CPM.BranchInfoDAO
        Dim sqlmap As New SQLMap

        Try

            searchModel.setBranchCode(Trim(txtBranchCode.Text.ToUpper))
            searchModel.setBranchName(Trim(txtBranchName.Text.ToUpper))

            If rbActiveYes.Checked Then
                searchModel.setActive(ConstantGlobal.Yes)
            Else
                searchModel.setActive(ConstantGlobal.No)
            End If


            Dim strSQL As String = sqlmap.getMappedStatement("SetupMstr/Search-BranchInfo", searchModel)

            ViewState("strSQL") = strSQL


            dsBranch.SelectCommand = ViewState("strSQL")
            gvBranch.DataBind()

            gvBranch.PageIndex = 0

            If gvBranch.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsBranch).ToString + " " + "Record Found"
            End If

        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            BranchInfoDao = Nothing
            searchModel = Nothing
            sqlmap = Nothing

        End Try

    End Sub

    Private Sub clear()
        lblmsg.Text = ""
        lblRecCount.Text = ""
        txtBranchCode.Text = ""
        txtBranchName.Text = ""
        txtAddress1.Text = ""
        txtAddress2.Text = ""
        txtAddress3.Text = ""
        txtPostCode.Text = ""
        ddState.SelectedIndex = 0
        txtTelephone.Text = ""
        txtFax.Text = ""
        txtEmail.Text = ""
        txtBranchManager.Text = ""
        txtRemark.Text = ""
        txtBranchManagerHp.Text = ""
        rbActiveYes.Checked = True
        gvBranch.DataSource = Nothing
        addMode()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        bindData()
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim strSQL As String = ""
        Dim msg As String = ""
        Dim isExist As Boolean = False

        If Not Page.IsValid Then
            Exit Sub
        End If

        lblmsg.Text = ""

        cn = New SqlConnection(dm.getDBConn())
        If Not cn.State = ConnectionState.Open Then
            cn.Open()
        End If
        trans = cn.BeginTransaction

        Try
            'validate the name is not existed before insert
            Dim searchModel As New CPM.BranchInfoEntity
            Dim sqlmap As New SQLMap
            searchModel.setBranchCode(Trim(txtBranchCode.Text.ToUpper))
            strSQL = sqlmap.getMappedStatement("SetupMstr/Search-BranchInfo", searchModel)

            Dim dt As DataTable
            dt = dm.execTableInTrans(strSQL, cn, trans)
            If dt.Rows.Count > 0 Then
                isExist = True
            End If

            strSQL = ""
            dt.Dispose()


            If Not isExist Then
                InsertRecord(cn, trans)
                trans.Commit()
                clear()
                'bindGridView()
                lblmsg.Text = ConstantGlobal.Record_Added
            Else
                Throw New ApplicationException(ConstantGlobal.Record_Already_Exist)
            End If

        Catch ex As Exception
            lblmsg.Text = ex.Message
            trans.Rollback()
        Finally
            trans.Dispose()
            cn.Close()
        End Try
    End Sub

    Protected Sub InsertRecord(ByVal cn As SqlConnection, ByVal trans As SqlTransaction)
        Dim activeInd As String
        Dim branchInfoEnt As New CPM.BranchInfoEntity
        Dim branchInfoDao As New CPM.BranchInfoDAO

        Try
            branchInfoEnt.setBranchCode(Trim(txtBranchCode.Text.ToUpper))
            branchInfoEnt.setBranchName(Trim(txtBranchName.Text.ToUpper))
            branchInfoEnt.setAddress1(Trim(txtAddress1.Text.ToUpper))
            branchInfoEnt.setAddress2(Trim(txtAddress2.Text.ToUpper))
            branchInfoEnt.setAddress3(Trim(txtAddress3.Text.ToUpper))
            branchInfoEnt.setPostCode(Trim(txtPostCode.Text.ToUpper))
            branchInfoEnt.setState(ddState.SelectedValue)
            branchInfoEnt.setTelephone(Trim(txtTelephone.Text.ToUpper))
            branchInfoEnt.setFax(Trim(txtFax.Text.ToUpper))
            branchInfoEnt.setEmail(Trim(txtEmail.Text))
            branchInfoEnt.setBranchManager(Trim(txtBranchManager.Text.ToUpper))
            branchInfoEnt.setRemark(Trim(txtRemark.Text))

            branchInfoEnt.setBranchManagerHpNo(Trim(txtBranchManagerHp.Text))


            If rbActiveYes.Checked = True Then activeInd = ConstantGlobal.Yes Else activeInd = ConstantGlobal.No

            branchInfoEnt.setActive(activeInd)

            branchInfoEnt.setLastUpdatedDatetime(Now())
            branchInfoEnt.setLastUpdatedBy(lp.getUserMstrId)
            branchInfoDao.insertDB(branchInfoEnt, cn, trans)

        Catch ex As Exception
            Throw ex
        Finally
            branchInfoEnt = Nothing
            branchInfoDao = Nothing
        End Try
        

    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        clear()
        ViewState("strSQL") = Nothing
        rbActiveNo.Checked = False
        rbActiveYes.Checked = True
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvBranch.Rows.Count
            ClientScript.RegisterForEventValidation(gvBranch.UniqueID, "Select$" + i.ToString)
        Next

        MyBase.Render(writer)
    End Sub

    Protected Sub gvBranch_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBranch.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvBranch.UniqueID + "','Select$" + gvBranch.Rows.Count.ToString + "');")
            'e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('gvBranch','Select$" + gvBranch.Rows.Count.ToString + "');")
            Dim int As Integer = gvBranch.Rows.Count / 2
            Dim dob As Double = gvBranch.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

        End If
    End Sub

    Protected Sub gvBranch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBranch.SelectedIndexChanged
        Dim branchInfoDao As New CPM.BranchInfoDAO

        Try
            lblmsg.Text = ""

            txtBranchCode.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBranch.SelectedDataKey(branchInfoDao.COLUMN_BranchCode))
            txtBranchName.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBranch.SelectedDataKey(branchInfoDao.COLUMN_BranchName))
            txtAddress1.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBranch.SelectedDataKey(branchInfoDao.COLUMN_Address1))
            txtAddress2.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBranch.SelectedDataKey(branchInfoDao.COLUMN_Address2))
            txtAddress3.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBranch.SelectedDataKey(branchInfoDao.COLUMN_Address3))
            txtPostCode.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBranch.SelectedDataKey(branchInfoDao.COLUMN_PostCode))
            ddState.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvBranch.SelectedDataKey(branchInfoDao.COLUMN_State))
            txtTelephone.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBranch.SelectedDataKey(branchInfoDao.COLUMN_Telephone))
            txtFax.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBranch.SelectedDataKey(branchInfoDao.COLUMN_Fax))
            txtEmail.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBranch.SelectedDataKey(branchInfoDao.COLUMN_Email))
            txtBranchManager.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBranch.SelectedDataKey(branchInfoDao.COLUMN_BranchManager))
            txtRemark.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBranch.SelectedDataKey(branchInfoDao.COLUMN_Remark))
            txtBranchManagerHp.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBranch.SelectedDataKey(branchInfoDao.COLUMN_BranchManagerHpNo))


            Dim vActiveInd As String = Utility.DataTypeUtils.parseHTMLSafeToString(gvBranch.SelectedDataKey(branchInfoDao.COLUMN_Active))

            If ConstantGlobal.No.Equals(vActiveInd) Then
                rbActiveYes.Checked = False
                rbActiveNo.Checked = True
            Else
                rbActiveYes.Checked = True
                rbActiveNo.Checked = False
            End If

            updateMode()

        Catch ex As Exception
            Throw ex
        Finally
            branchInfoDao = Nothing
        End Try
      
    End Sub

    Private Sub addMode()
        rbActiveNo.Checked = False
        rbActiveYes.Checked = True

        btnAdd.Enabled = True
        btnUpdate.Enabled = False
        btnSearch.Enabled = True

        txtBranchCode.ReadOnly = False
        txtBranchCode.CssClass = CSSEnum.TXTFIELD_2
    End Sub

    Private Sub updateMode()
        btnAdd.Enabled = False
        btnUpdate.Enabled = True
        btnSearch.Enabled = False

        txtBranchCode.ReadOnly = True
        txtBranchCode.CssClass = CSSEnum.TXTFIELD_2_DISABLED
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        lblmsg.Text = ""

        If Not Page.IsValid Then
            Exit Sub
        End If

        If Trim(txtEmail.Text) <> "" Then
            If Not Utility.Tools.EmailAddressCheck(Trim(txtEmail.Text)) Then
                lblmsg.Text = "Invalid email address."
                Exit Sub
            End If
        End If


        Dim sqlmap As New SQLMap
        Dim strSQL As String = ""
        Dim searchModel As New CPM.BranchInfoEntity

        cn = New SqlConnection(dm.getDBConn)
        If Not cn.State = ConnectionState.Open Then
            cn.Open()
        End If
        trans = cn.BeginTransaction

        Try

            UpdateRecord(cn, trans)
            trans.Commit()
            clear()
            Me.bindData()

            lblmsg.Text = ConstantGlobal.Record_Updated


        Catch ex As Exception
            lblmsg.Text = ex.Message
            trans.Rollback()
        Finally
            trans.Dispose()
            cn.Close()
        End Try
    End Sub

    Protected Sub UpdateRecord(ByVal cn As SqlConnection, ByVal trans As SqlTransaction)

        Dim activeInd As String
        Dim branchInfoEnt As New CPM.BranchInfoEntity
        Dim branchInfoDao As New CPM.BranchInfoDAO

        Try
            branchInfoEnt.setBranchInfoId(gvBranch.SelectedDataKey(branchInfoDao.COLUMN_BranchInfoID))
            branchInfoEnt.setBranchCode(Trim(txtBranchCode.Text.ToUpper))
            branchInfoEnt.setBranchName(Trim(txtBranchName.Text.ToUpper))
            branchInfoEnt.setAddress1(Trim(txtAddress1.Text.ToUpper))
            branchInfoEnt.setAddress2(Trim(txtAddress2.Text.ToUpper))
            branchInfoEnt.setAddress3(Trim(txtAddress3.Text.ToUpper))
            branchInfoEnt.setPostCode(Trim(txtPostCode.Text.ToUpper))
            branchInfoEnt.setState(ddState.SelectedValue)
            branchInfoEnt.setTelephone(Trim(txtTelephone.Text.ToUpper))
            branchInfoEnt.setFax(Trim(txtFax.Text.ToUpper))
            branchInfoEnt.setEmail(Trim(txtEmail.Text))
            branchInfoEnt.setBranchManager(Trim(txtBranchManager.Text.ToUpper))
            branchInfoEnt.setRemark(Trim(txtRemark.Text))
            branchInfoEnt.setBranchManagerHpNo(Trim(txtBranchManagerHp.Text))

            If rbActiveYes.Checked = True Then activeInd = ConstantGlobal.Yes Else activeInd = ConstantGlobal.No

            branchInfoEnt.setActive(activeInd)

            branchInfoEnt.setLastUpdatedDatetime(gvBranch.SelectedDataKey("LUDT"))

            branchInfoEnt.setLastUpdatedBy(lp.getUserMstrId)
            branchInfoDao.updateDB(branchInfoEnt, cn, trans)
        Catch ex As Exception
            Throw ex
        Finally
            branchInfoEnt = Nothing
            branchInfoDao = Nothing

        End Try

       

    End Sub

    Protected Sub gvBranch_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvBranch.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindData()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub
End Class
