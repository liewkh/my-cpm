Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Maintenance_LocationSetup
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
        Dim searchModel As New CPM.LocationInfoEntity
        Dim BranchInfoDao As New CPM.LocationInfoDAO
        Dim sqlmap As New SQLMap

        Try

            searchModel.setLocationCode(Trim(txtLocationCode.Text.ToUpper))
            searchModel.setLocationName(Trim(txtLocationName.Text.ToUpper))
            searchModel.setBranchInfoId(ddBranch.SelectedValue)

            If rbActiveYes.Checked Then
                searchModel.setActive(ConstantGlobal.Yes)
            Else
                searchModel.setActive(ConstantGlobal.No)
            End If


            Dim strSQL As String = sqlmap.getMappedStatement("SetupMstr/Search-LocationInfo", searchModel)

            ViewState("strSQL") = strSQL


            dsLocation.SelectCommand = ViewState("strSQL")
            gvLocation.DataBind()

            gvLocation.PageIndex = 0

            If gvLocation.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsLocation).ToString + " " + "Record Found"
            End If

        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            BranchInfoDao = Nothing
            searchModel = Nothing
            sqlmap = Nothing
            'ViewState("strSQL") = Nothing

        End Try

    End Sub

    Private Sub clear()
        lblmsg.Text = ""
        lblRecCount.Text = ""
        ddBranch.SelectedIndex = 0
        txtLocationCode.Text = ""
        txtLocationName.Text = ""
        txtManagerName.Text = ""
        txtManagerHpNo.Text = ""
        txtSupervisorName.Text = ""
        txtSupervisorHpNo.Text = ""
        txtLocationCapacity.Text = ""
        txtLocationType.Text = ""
        txtAddress1.Text = ""
        txtAddress2.Text = ""
        txtAddress3.Text = ""
        txtPostCode.Text = ""
        ddState.SelectedIndex = 0
        txtTelephone.Text = ""
        txtFax.Text = ""
        txtEmail.Text = ""
        txtUrl.Text = ""
        txtRemark.Text = ""
        txtComInvoiceNo.Text = ""
        txtIndInvoiceNo.Text = ""
        txtComInvoicePrefix.Text = ""
        txtIndInvoicePrefix.Text = ""
        txtReceiptNo.Text = ""
        txtReceiptPrefix.Text = ""
        txtDebitNoteNo.Text = ""
        txtDebitNotePrefix.Text = ""
        txtCreditNoteNo.Text = ""
        txtCreditNotePrefix.Text = ""
        txtCutOff.Text = ""
        txtSeasonAmount.Text = ""
        txtVisitorAmount.Text = ""
        txtDailyCollectionNo.Text = ""
        txtDailyCollectionPrefix.Text = ""
        txtAccountCode.Text = ""
        ddBankCode.SelectedIndex = 0
        rbActiveYes.Checked = True
        gvLocation.DataSource = Nothing
        addMode()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        bindData()
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim strSQL As String = ""
        Dim msg As String = ""
        Dim isExist As Boolean = False

        lblmsg.Text = ""

        If Not Page.IsValid Then
            Exit Sub
        End If

        If ddBranch.SelectedIndex = 0 Then
            lblmsg.Text = "Please enter value for Branch."
            Exit Sub
        End If

        If txtLocationCapacity.Text <> "" Then
            If Not Utility.Tools.NumericValidation(txtLocationCapacity.Text) Then
                lblmsg.Text = "Please enter numeric value for location capacity."
                Exit Sub
            End If
        End If

        If txtComInvoiceNo.Text <> "" Then
            If Not Utility.Tools.NumericValidation(txtComInvoiceNo.Text) Then
                lblmsg.Text = "Please enter numeric value for Company Invoice No."
                Exit Sub
            End If
        End If

        If txtIndInvoiceNo.Text <> "" Then
            If Not Utility.Tools.NumericValidation(txtIndInvoiceNo.Text) Then
                lblmsg.Text = "Please enter numeric value for Individual Invoice No."
                Exit Sub
            End If
        End If

        If txtReceiptNo.Text <> "" Then
            If Not Utility.Tools.NumericValidation(txtReceiptNo.Text) Then
                lblmsg.Text = "Please enter numeric value for Receipt No."
                Exit Sub
            End If
        End If

        If txtDebitNoteNo.Text <> "" Then
            If Not Utility.Tools.NumericValidation(txtDebitNoteNo.Text) Then
                lblmsg.Text = "Please enter numeric value for Debit Note No."
                Exit Sub
            End If
        End If

        If txtDailyCollectionNo.Text <> "" Then
            If Not Utility.Tools.NumericValidation(txtDailyCollectionNo.Text) Then
                lblmsg.Text = "Please enter numeric value for Daily Collection No."
                Exit Sub
            End If
        End If

        If Trim(txtEmail.Text) <> "" Then
            If Not Utility.Tools.EmailAddressCheck(Trim(txtEmail.Text)) Then
                lblmsg.Text = "Invalid email address."
                Exit Sub
            End If
        End If

        If txtCutOff.Text <> "" Then
            If Not Utility.Tools.NumericValidation(txtCutOff.Text) Then
                lblmsg.Text = "Please enter numeric value for Cancellation Cut Off."
                Exit Sub
            End If
        End If

        If txtSeasonAmount.Text <> "" Then
            If Not Utility.Tools.NumericValidation(txtSeasonAmount.Text) Then
                lblmsg.Text = "Please enter numeric value for Season Budgetted Amount."
                Exit Sub
            End If
        End If

        If txtVisitorAmount.Text <> "" Then
            If Not Utility.Tools.NumericValidation(txtVisitorAmount.Text) Then
                lblmsg.Text = "Please enter numeric value for Visitor Budgetted Amount."
                Exit Sub
            End If
        End If

        cn = New SqlConnection(dm.getDBConn())
        If Not cn.State = ConnectionState.Open Then
            cn.Open()
        End If
        trans = cn.BeginTransaction

        Try
            'validate the name is not existed before insert
            Dim searchModel As New CPM.LocationInfoEntity
            Dim sqlmap As New SQLMap
            searchModel.setLocationCode(Trim(txtLocationCode.Text.ToUpper))
            strSQL = sqlmap.getMappedStatement("SetupMstr/Search-LocationInfo", searchModel)

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
        Dim locInfoEnt As New CPM.LocationInfoEntity
        Dim locInfoDao As New CPM.LocationInfoDAO

        Try
            locInfoEnt.setBranchInfoId(ddBranch.SelectedValue)
            locInfoEnt.setBankCode(ddBankCode.SelectedValue)
            locInfoEnt.setLocationCode(Trim(txtLocationCode.Text.ToUpper))
            locInfoEnt.setLocationName(Trim(txtLocationName.Text.ToUpper))
            locInfoEnt.setAddress1(Trim(txtAddress1.Text.ToUpper))
            locInfoEnt.setAddress2(Trim(txtAddress2.Text.ToUpper))
            locInfoEnt.setAddress3(Trim(txtAddress3.Text.ToUpper))
            locInfoEnt.setPostCode(Trim(txtPostCode.Text.ToUpper))
            locInfoEnt.setState(ddState.SelectedValue)
            locInfoEnt.setTelephone(Trim(txtTelephone.Text.ToUpper))
            locInfoEnt.setFax(Trim(txtFax.Text.ToUpper))
            locInfoEnt.setEmail(Trim(txtEmail.Text))
            locInfoEnt.setUrl(Trim(txtUrl.Text))
            locInfoEnt.setRemark(Trim(txtRemark.Text))

            locInfoEnt.setManagerName(Trim(txtManagerName.Text.ToUpper))
            locInfoEnt.setManagerHpNo(Trim(txtManagerHpNo.Text.ToUpper))
            locInfoEnt.setSupervisorName(Trim(txtSupervisorName.Text.ToUpper))
            locInfoEnt.setSupervisorHpNo(Trim(txtSupervisorHpNo.Text.ToUpper))

            If txtLocationCapacity.Text <> "" Then
                locInfoEnt.setLocationCapacity(Trim(txtLocationCapacity.Text))
            End If

            If txtSeasonAmount.Text <> "" Then
                locInfoEnt.setSeasonBudgetAmount(txtSeasonAmount.Text)
            End If

            If txtVisitorAmount.Text <> "" Then
                locInfoEnt.setVisitorBudgetAmount(txtVisitorAmount.Text)
            End If

            locInfoEnt.setLocationType(Trim(txtLocationType.Text.ToUpper))

            If txtComInvoiceNo.Text <> "" Then
                locInfoEnt.setCompanyInvoiceNo(Trim(txtComInvoiceNo.Text))
            End If

            If txtIndInvoiceNo.Text <> "" Then
                locInfoEnt.setIndividualInvoiceNo(Trim(txtIndInvoiceNo.Text))
            End If

            If txtReceiptNo.Text <> "" Then
                locInfoEnt.setReceiptNo(Trim(txtReceiptNo.Text))
            End If

            If txtDebitNoteNo.Text <> "" Then
                locInfoEnt.setDebitNoteNo(Trim(txtDebitNoteNo.Text))
            End If

            If txtCreditNoteNo.Text <> "" Then
                locInfoEnt.setCreditNoteNo(Trim(txtCreditNoteNo.Text))
            End If

            If txtDailyCollectionNo.Text <> "" Then
                locInfoEnt.setDailyCollectionNo(Trim(txtDailyCollectionNo.Text))
            End If

            If txtAccountCode.Text <> "" Then
                locInfoEnt.setAccountCode(Trim(txtAccountCode.Text))
            End If

            If txtCutOff.Text <> "" Then
                locInfoEnt.setRefundCutOffDate(txtCutOff.Text)
            End If

            locInfoEnt.setIndividualInvoicePrefix(Trim(txtIndInvoicePrefix.Text.ToUpper))
            locInfoEnt.setCompanyInvoicePrefix(Trim(txtComInvoicePrefix.Text.ToUpper))

            locInfoEnt.setReceiptPrefix(Trim(txtReceiptPrefix.Text.ToUpper))
            locInfoEnt.setDebitNotePrefix(Trim(txtDebitNotePrefix.Text.ToUpper))
            locInfoEnt.setCreditNotePrefix(Trim(txtCreditNotePrefix.Text.ToUpper))
            locInfoEnt.setDailyCollectionPrefix(Trim(txtDailyCollectionPrefix.Text.ToUpper))

            If rbActiveYes.Checked = True Then activeInd = ConstantGlobal.Yes Else activeInd = ConstantGlobal.No

            locInfoEnt.setActive(activeInd)

            locInfoEnt.setLastUpdatedDatetime(Now())
            locInfoEnt.setLastUpdatedBy(lp.getUserMstrId)
            locInfoDao.insertDB(locInfoEnt, cn, trans)

        Catch ex As Exception
            Throw ex
        Finally
            locInfoEnt = Nothing
            locInfoDao = Nothing
        End Try


    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        clear()
        ViewState("strSQL") = Nothing
        rbActiveNo.Checked = False
        rbActiveYes.Checked = True
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvLocation.Rows.Count
            ClientScript.RegisterForEventValidation(gvLocation.UniqueID, "Select$" + i.ToString)
        Next

        MyBase.Render(writer)
    End Sub

    Protected Sub gvLocation_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvLocation.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvLocation.UniqueID + "','Select$" + gvLocation.Rows.Count.ToString + "');")
            'e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('gvLocation','Select$" + gvLocation.Rows.Count.ToString + "');")
            Dim int As Integer = gvLocation.Rows.Count / 2
            Dim dob As Double = gvLocation.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

        End If
    End Sub

    Protected Sub gvLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvLocation.SelectedIndexChanged
        Dim locInfoDao As New CPM.LocationInfoDAO

        Try

            lblmsg.Text = ""

            ddBranch.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_BranchInfoId))
            txtLocationCode.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_LocationCode))
            txtLocationName.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_LocationName))
            txtAddress1.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_Address1))
            txtAddress2.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_Address2))
            txtAddress3.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_Address3))
            txtPostCode.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_PostCode))
            ddState.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_State))
            ddBankCode.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_BankCode))
            txtTelephone.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_Telephone))
            txtFax.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_Fax))
            txtEmail.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_Email))
            txtUrl.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_Url))
            txtRemark.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_Remark))


            txtManagerName.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_ManagerName))
            txtManagerHpNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_ManagerHpNo))
            txtSupervisorName.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_SupervisorName))
            txtSupervisorHpNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_SupervisorHpNo))
            txtLocationCapacity.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_LocationCapacity))
            txtLocationType.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_LocationType))


            txtComInvoiceNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_CompanyInvoiceNo))
            txtIndInvoiceNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_IndividualInvoiceNo))

            txtComInvoicePrefix.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_CompanyInvoicePrefix))
            txtIndInvoicePrefix.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_IndividualInvoicePrefix))

            txtReceiptNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_ReceiptNo))
            txtReceiptPrefix.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_ReceiptPrefix))
            txtDebitNoteNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_DebitNoteNo))
            txtDebitNotePrefix.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_DebitNotePrefix))
            txtCreditNoteNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_CreditNoteNo))
            txtCreditNotePrefix.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_CreditNotePrefix))
            txtDailyCollectionPrefix.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_DailyCollectionPrefix))
            txtDailyCollectionNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_DailyCollectionNo))
            txtAccountCode.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_AccountCode))

            txtCutOff.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_RefundCutOffDate))

            txtSeasonAmount.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_SeasonBudgetAmount))
            txtVisitorAmount.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_VisitorBudgetAmount))

            Dim vActiveInd As String = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_Active))

            If ConstantGlobal.No.Equals(vActiveInd) Then
                rbActiveYes.Checked = False
                rbActiveNo.Checked = True
            Else
                rbActiveYes.Checked = True
                rbActiveNo.Checked = False
            End If

            updateMode()

        Catch ex As Exception
            lblmsg.Text = ex.Message
            'Throw ex

        Finally
            locInfoDao = Nothing
        End Try

    End Sub

    Private Sub addMode()
        rbActiveNo.Checked = False
        rbActiveYes.Checked = True

        btnAdd.Enabled = True
        btnUpdate.Enabled = False
        btnSearch.Enabled = True

        txtLocationCode.ReadOnly = False
        txtLocationCode.CssClass = CSSEnum.TXTFIELD_2
    End Sub

    Private Sub updateMode()
        btnAdd.Enabled = False
        btnUpdate.Enabled = True
        btnSearch.Enabled = False

        txtLocationCode.ReadOnly = True
        txtLocationCode.CssClass = CSSEnum.TXTFIELD_2_DISABLED
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        lblmsg.Text = ""

        If Not Page.IsValid Then
            Exit Sub
        End If

        If ddBranch.SelectedIndex = 0 Then
            lblmsg.Text = "Please enter value for Branch."
            Exit Sub
        End If


        If txtLocationCapacity.Text <> "" Then
            If Not Utility.Tools.NumericValidation(txtLocationCapacity.Text) Then
                lblmsg.Text = "Please enter numeric value for location capacity."
                Exit Sub
            End If
        End If

        If Trim(txtEmail.Text) <> "" Then
            If Not Utility.Tools.EmailAddressCheck(Trim(txtEmail.Text)) Then
                lblmsg.Text = "Invalid email address."
                Exit Sub
            End If
        End If

        If txtComInvoiceNo.Text <> "" Then
            If Not Utility.Tools.NumericValidation(txtComInvoiceNo.Text) Then
                lblmsg.Text = "Please enter numeric value for Company Invoice No."
                Exit Sub
            End If
        End If

        If txtIndInvoiceNo.Text <> "" Then
            If Not Utility.Tools.NumericValidation(txtIndInvoiceNo.Text) Then
                lblmsg.Text = "Please enter numeric value for Individual Invoice No."
                Exit Sub
            End If
        End If

        If txtReceiptNo.Text <> "" Then
            If Not Utility.Tools.NumericValidation(txtReceiptNo.Text) Then
                lblmsg.Text = "Please enter numeric value for Receipt No."
                Exit Sub
            End If
        End If

        If txtDebitNoteNo.Text <> "" Then
            If Not Utility.Tools.NumericValidation(txtDebitNoteNo.Text) Then
                lblmsg.Text = "Please enter numeric value for Debit Note No."
                Exit Sub
            End If
        End If

        If txtCreditNoteNo.Text <> "" Then
            If Not Utility.Tools.NumericValidation(txtCreditNoteNo.Text) Then
                lblmsg.Text = "Please enter numeric value for Credit Note No."
                Exit Sub
            End If
        End If

        If txtDailyCollectionNo.Text <> "" Then
            If Not Utility.Tools.NumericValidation(txtDailyCollectionNo.Text) Then
                lblmsg.Text = "Please enter numeric value for Daily Collection No."
                Exit Sub
            End If
        End If

        If txtCutOff.Text <> "" Then
            If Not Utility.Tools.NumericValidation(txtCutOff.Text) Then
                lblmsg.Text = "Please enter numeric value for Cancellation Cut Off."
                Exit Sub
            End If
        End If

        If txtSeasonAmount.Text <> "" Then
            If Not Utility.Tools.NumericValidation(txtSeasonAmount.Text) Then
                lblmsg.Text = "Please enter numeric value for Season Budgetted Amount."
                Exit Sub
            End If
        End If

        If txtVisitorAmount.Text <> "" Then
            If Not Utility.Tools.NumericValidation(txtVisitorAmount.Text) Then
                lblmsg.Text = "Please enter numeric value for Visitor Budgetted Amount."
                Exit Sub
            End If
        End If

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
        Dim locInfoEnt As New CPM.LocationInfoEntity
        Dim locInfoDao As New CPM.LocationInfoDAO

        Try
            locInfoEnt.setLocationInfoId(gvLocation.SelectedDataKey(locInfoDao.COLUMN_LocationInfoID))
            locInfoEnt.setBranchInfoId(ddBranch.SelectedValue)
            locInfoEnt.setBankCode(ddBankCode.SelectedValue)
            locInfoEnt.setLocationCode(Trim(txtLocationCode.Text.ToUpper))
            locInfoEnt.setLocationName(Trim(txtLocationName.Text.ToUpper))
            locInfoEnt.setAddress1(Trim(txtAddress1.Text.ToUpper))
            locInfoEnt.setAddress2(Trim(txtAddress2.Text.ToUpper))
            locInfoEnt.setAddress3(Trim(txtAddress3.Text.ToUpper))
            locInfoEnt.setPostCode(Trim(txtPostCode.Text.ToUpper))
            locInfoEnt.setState(ddState.SelectedValue)
            locInfoEnt.setTelephone(Trim(txtTelephone.Text.ToUpper))
            locInfoEnt.setFax(Trim(txtFax.Text.ToUpper))
            locInfoEnt.setEmail(Trim(txtEmail.Text))
            locInfoEnt.setUrl(Trim(txtUrl.Text))
            locInfoEnt.setRemark(Trim(txtRemark.Text))

            locInfoEnt.setManagerName(Trim(txtManagerName.Text.ToUpper))
            locInfoEnt.setManagerHpNo(Trim(txtManagerHpNo.Text.ToUpper))
            locInfoEnt.setSupervisorName(Trim(txtSupervisorName.Text.ToUpper))
            locInfoEnt.setSupervisorHpNo(Trim(txtSupervisorHpNo.Text.ToUpper))

            If txtLocationCapacity.Text <> "" Then
                locInfoEnt.setLocationCapacity(Trim(txtLocationCapacity.Text))
            End If

            locInfoEnt.setLocationType(Trim(txtLocationType.Text.ToUpper))

            If txtComInvoiceNo.Text <> "" Then
                locInfoEnt.setCompanyInvoiceNo((txtComInvoiceNo.Text))
            End If

            If txtIndInvoiceNo.Text <> "" Then
                locInfoEnt.setIndividualInvoiceNo(Trim(txtIndInvoiceNo.Text))
            End If

            If txtReceiptNo.Text <> "" Then
                locInfoEnt.setReceiptNo(Trim(txtReceiptNo.Text))
            End If


            If txtCutOff.Text <> "" Then
                locInfoEnt.setRefundCutOffDate(txtCutOff.Text)
            End If

            If txtDebitNoteNo.Text <> "" Then
                locInfoEnt.setDebitNoteNo(Trim(txtDebitNoteNo.Text))
            End If

            If txtCreditNoteNo.Text <> "" Then
                locInfoEnt.setCreditNoteNo(Trim(txtCreditNoteNo.Text))
            End If

            If txtDailyCollectionNo.Text <> "" Then
                locInfoEnt.setDailyCollectionNo(Trim(txtDailyCollectionNo.Text))
            End If

            If txtAccountCode.Text <> "" Then
                locInfoEnt.setAccountCode(Trim(txtAccountCode.Text))
            End If


            locInfoEnt.setCompanyInvoicePrefix(Trim(txtComInvoicePrefix.Text.ToUpper))
            locInfoEnt.setIndividualInvoicePrefix(Trim(txtIndInvoicePrefix.Text.ToUpper))

            locInfoEnt.setReceiptPrefix(Trim(txtReceiptPrefix.Text.ToUpper))
            locInfoEnt.setDebitNotePrefix(Trim(txtDebitNotePrefix.Text.ToUpper))
            locInfoEnt.setCreditNotePrefix(Trim(txtCreditNotePrefix.Text.ToUpper))
            locInfoEnt.setDailyCollectionPrefix(Trim(txtDailyCollectionPrefix.Text.ToUpper))


            If txtSeasonAmount.Text <> "" Then
                locInfoEnt.setSeasonBudgetAmount(txtSeasonAmount.Text)
            End If

            If txtVisitorAmount.Text <> "" Then
                locInfoEnt.setVisitorBudgetAmount(txtVisitorAmount.Text)
            End If


            If rbActiveYes.Checked = True Then activeInd = ConstantGlobal.Yes Else activeInd = ConstantGlobal.No

            locInfoEnt.setActive(activeInd)

            locInfoEnt.setLastUpdatedDatetime(gvLocation.SelectedDataKey("LUDT"))

            locInfoEnt.setLastUpdatedBy(lp.getUserMstrId)
            locInfoDao.updateDB(locInfoEnt, cn, trans)
        Catch ex As Exception
            Throw ex
        Finally
            locInfoEnt = Nothing
            locInfoDao = Nothing

        End Try



    End Sub

    Protected Sub gvLocation_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvLocation.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindData()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
        'ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('error message');", true);
        'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "OpenNewWindow", "window.open('New page.aspx')", False)
    End Sub

End Class