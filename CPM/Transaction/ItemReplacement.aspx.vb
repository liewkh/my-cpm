Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports PopCalendarSpecialDay


Partial Class Transaction_ItemReplacement
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim receiptNo As String = ""


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            Session.LCID = 2057

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

                SpecialDays.AddHolidays(popCalendar1)
                SpecialDays.AddSpecialDays(popCalendar1)

                txtTransactionDate.Text = Utility.DataTypeUtils.formatDateString(Now)
                txtProcessedBy.Text = lp.getUserName

            End If
            ddLocation.SelectedValue = lp.getDefaultLocationInfoId

            divSearch.Visible = True
            divInv.Visible = False

        Catch ex As Exception
            lblmsg.Text = ex.Message

        End Try




    End Sub

    Private Sub bindData()
        Dim searchModel As New DebtorSearchModel
        Dim passCardMstrDao As New CPM.DebtorDAO
        Dim sqlmap As New SQLMap

        Try

            lblmsg.Text = ""

            If rbCompany.Checked = True Then
                searchModel.setName(Trim(txtDebtorName.Text.ToUpper))
                searchModel.setCategory(CategoryEnum.COMPANY)
            Else
                searchModel.setName(Trim(txtDebtorName.Text.ToUpper))
                searchModel.setCategory(CategoryEnum.INDIVIDUAL)
            End If

            If ddLocation.SelectedIndex = 0 Then
                Dim a As ListItem
                Dim b As String = ""
                For Each a In ddLocation.Items
                    b = b + a.Value.ToString() + ","
                Next
                searchModel.setLocationId(b)
            Else
                searchModel.setLocationInfoId(ddLocation.SelectedValue)
            End If

            searchModel.setStatus(DebtorStatusEnum.ACTIVE)

            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-Debtor", searchModel)

            ViewState("strSQL") = strSQL


            dsDebtorEnq.SelectCommand = ViewState("strSQL")
            gvDebtorEnq.DataBind()

            gvDebtorEnq.PageIndex = 0

            If gvDebtorEnq.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsDebtorEnq).ToString + " " + "Record Found"
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
        ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        txtDebtorName.Text = ""
        rbCompany.Checked = True
        rbIndividual.Checked = False
        gvDebtorEnq.DataSource = Nothing

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        bindData()
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        clear()
        ViewState("strSQL") = Nothing
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvDebtorEnq.Rows.Count
            ClientScript.RegisterForEventValidation(gvDebtorEnq.UniqueID, "Select$" + i.ToString)
        Next

        MyBase.Render(writer)
    End Sub

    Protected Sub gvDebtorEnq_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDebtorEnq.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvDebtorEnq.UniqueID + "','Select$" + gvDebtorEnq.Rows.Count.ToString + "');")
            Dim int As Integer = gvDebtorEnq.Rows.Count / 2
            Dim dob As Double = gvDebtorEnq.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If
        End If

    End Sub


    Protected Sub gvDebtorEnq_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDebtorEnq.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindData()
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub

    Protected Sub gvDebtorEnq_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDebtorEnq.SelectedIndexChanged
        Dim debtorDao As New CPM.DebtorDAO

        Try

            hidDebtorId.Value = gvDebtorEnq.SelectedDataKey(debtorDao.COLUMN_DebtorID).ToString
            txtDebtorName.Text = gvDebtorEnq.SelectedDataKey("DEBTOR").ToString
            hidLocationInfoId.Value = gvDebtorEnq.SelectedDataKey(debtorDao.COLUMN_LocationInfoId).ToString
            DataMode()


        Catch ex As Exception
            logger.Error(ex.Message)
            lblmsg.Text = ex.Message
        Finally
            debtorDao = Nothing
        End Try

    End Sub

    Private Sub DataMode()

        divSearch.Visible = False
        divInv.Visible = True
        txtDebtorName.ReadOnly = True
        txtDebtorName.CssClass = CSSEnum.TXTFIELD_3_DISABLED
        ddLocation.Enabled = False
        ddLocation.CssClass = CSSEnum.DROPDOWN_DISABLED
        rbCompany.Enabled = False
        rbIndividual.Enabled = False


    End Sub

    Private Sub SearchMode()
        divSearch.Visible = True
        divInv.Visible = False
        txtDebtorName.ReadOnly = False
        txtDebtorName.CssClass = CSSEnum.TXTFIELD_3_DISABLED
        ddLocation.Enabled = True
        ddLocation.CssClass = CSSEnum.DROPDOWN
        rbCompany.Enabled = True
        rbIndividual.Enabled = True

    End Sub

    Protected Sub btnDataBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        SearchMode()
        clearData()
    End Sub

    Private Sub clearData()
        ddReason.SelectedIndex = 0
        ddItemType.SelectedIndex = 0
        ddNewPass.SelectedIndex = -1
        ddOldPass.SelectedIndex = -1
        txtDeposit.Text = ""
        txtActivationDate.Text = ""
        txtDoNo.Text = ""
        ddPaymentType.SelectedIndex = -1
        txtNo.Text = ""
	lblmsg.Text = ""
        txtRemark.Text = ""

    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim irEnt As New CPM.ItemReplacementEntity
        Dim irDao As New CPM.ItemReplacementDAO
        Dim pcEnt As New CPM.PassCardMstrEntity
        Dim pcDao As New CPM.PassCardMstrDAO
        Dim dpbEnt As New CPM.DebtorPassBayEntity
        Dim dpbDao As New CPM.DebtorPassBayDAO
        Dim pchEnt As New CPM.PassCardHistoryEntity
        Dim pchDao As New CPM.PassCardHistoryDAO
        Dim sql As String
        Dim dt As New DataTable
        Dim dpbId As String = ""
        Dim dpId As Long

        Try

            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If
            trans = cn.BeginTransaction


            If Not Page.IsValid Then
                Exit Sub
            End If

            If Trim(txtActivationDate.Text) = "" Then
                lblmsg.Text = "Activation Date is a required field."
                Exit Sub
            End If

            If Trim(txtTransactionDate.Text) = "" Then
                lblmsg.Text = "Transaction Date is a required field."
                Exit Sub
            End If


            If ddReason.SelectedIndex = 0 Then
                lblmsg.Text = "Reason is a required field."
                Exit Sub
            End If

            If ddNewPass.SelectedIndex = 0 Or ddNewPass.SelectedIndex = -1 Then
                lblmsg.Text = "New Pass is a required field."
                Exit Sub
            End If

            If ddOldPass.SelectedIndex = 0 Or ddOldPass.SelectedIndex = 0 Then
                lblmsg.Text = "Old Pass is a required field."
                Exit Sub
            End If


	    If ddReason.SelectedValue = ItemReplacementReasonEnum.LOST Or ddReason.SelectedValue = ItemReplacementReasonEnum.MISHANDLING Then
	

	      If (ddPaymentType.Text = "") and (ddPaymentType.Text <> PaymentTypeEnum.CASH) Then
		lblmsg.Text = "Please select a payment type."
	        Exit Sub
	      else
            	If (ddPaymentType.Text = PaymentTypeEnum.CHEQUE) and (trim(txtNo.Text) = "") Then
			lblmsg.Text = "Cheque No is a required field."
			Exit Sub
		else
			if (ddPaymentType.Text = PaymentTypeEnum.CREDITCARD) and (trim(txtNo.Text) = "") Then
			lblmsg.Text = "Credit Card no is a required field."
			Exit Sub
			End If
	    	End If
	      End If

	    End If



            If ddReason.SelectedValue = ItemReplacementReasonEnum.LOST Or ddReason.SelectedValue = ItemReplacementReasonEnum.MISHANDLING Then
                'Auto Create debtornote for charging and receipt payment
                dpId = process(cn, trans)
            End If


            sql = "SELECT * FROM DEBTORPASSBAY WHERE DEBTORID = " & hidDebtorId.Value & _
                  " AND STATUS = 'A' AND PASSCARDMSTRID = " & ddOldPass.SelectedValue
            dt = dm.execTableInTrans(sql, cn, trans)

            dpbId = dt.Rows(0).Item("DEBTORPASSBAYID")

            dpbEnt.setDebtorPassBayId(dpbId)
            dpbEnt.setPassCardMstrId(ddNewPass.SelectedValue)
            dpbEnt.setSerialNo(ddNewPass.SelectedItem.Text)
            dpbEnt.setLastUpdatedBy(lp.getUserMstrId)
            dpbEnt.setLastUpdatedDatetime(Now)
            dpbDao.updateDB(dpbEnt, cn, trans)


            Dim chkFirtUseSql As String = ""

            pcEnt.setPassCardMstrId(ddNewPass.SelectedValue)
            pcEnt.setDeposit(txtDeposit.Text)
            pcEnt.setStatus(PassCardStatusEnum.INUSE)
            pcEnt.setDebtorId(hidDebtorId.Value)
            pcEnt.setLastUpdatedBy(lp.getUserMstrId)
            pcEnt.setLastUpdatedDatetime(Now)
            pcEnt.setDepositPrint(ConstantGlobal.No)

            chkFirtUseSql = "Select FirstUsedDate From PassCardMstr where PassCardMstrId = " & ddNewPass.SelectedValue
            dt = dm.execTable(chkFirtUseSql)



            If dt.Rows.Count > 0 Then
                If dt.Rows(0).Item(pcDao.COLUMN_FirstUsedDate).Equals(System.DBNull.Value) Then
                    pcEnt.setFirstUsedDate(Now)
                End If
            End If

            pcDao.updateDB(pcEnt, cn, trans)

            'Update Old PassCardMstr
            pcEnt.setPassCardMstrId(ddOldPass.SelectedValue)
            pcEnt.setLastUpdatedBy(lp.getUserMstrId)
            pcEnt.setLastUpdatedDatetime(Now)
            'For reason "Customer Request - Reliability" and "Others", pls set the status of the passcard to availble. 
            'For "Lost & Replacement", it's lost and for the rest, it's considered spoilt.


            If ddReason.SelectedValue = ItemReplacementReasonEnum.LOST Then
                pcEnt.setStatus(PassCardStatusEnum.LOST)
            ElseIf ddReason.SelectedValue = ItemReplacementReasonEnum.RELIABILITY Then
                pcEnt.setStatus(PassCardStatusEnum.AVAILABLE)
            ElseIf ddReason.SelectedValue = ItemReplacementReasonEnum.OTHERS Then
                pcEnt.setStatus(PassCardStatusEnum.AVAILABLE)
            Else

                Dim sqlWarranty As String = "SELECT * FROM PASSCARDMSTR WHERE PASSCARDMSTRID = " & ddOldPass.SelectedValue
                Dim dtWarranty As New DataTable
                dtWarranty = dm.execTableInTrans(sqlWarranty, cn, trans)
                If dtWarranty.Rows.Count > 0 Then
                    If Not dtWarranty.Rows(0).Item("DELIVERYDATE").Equals(System.DBNull.Value) And Not dtWarranty.Rows(0).Item("WARRANTYPERIOD").Equals(System.DBNull.Value) Then
                        If CDate(dtWarranty.Rows(0).Item("DELIVERYDATE")).AddMonths(dtWarranty.Rows(0).Item("WARRANTYPERIOD")) >= CDate(Now.ToShortDateString) Then
                            pcEnt.setStatus(PassCardStatusEnum.WARRANTYCLAIM)
'added by vk - 16/07/2009 to set oldpasscard to spoit if warranty period over
                        else
			    pcEnt.setStatus(PassCardStatusEnum.SPOILT)
                        End If
                    Else
                        pcEnt.setStatus(PassCardStatusEnum.SPOILT)
                    End If
                End If

            End If

            pcEnt.setDepositPrint(ConstantGlobal.No)
            pcDao.updateDB(pcEnt, cn, trans)


            'Update Old PassCardHistory Before create new pass history
            Dim selectSQL As String = "select max(PassCardHistoryId) as ID from PassCardHistory Where PassCardMstrId = " & ddOldPass.SelectedValue

            dt = dm.execTable(selectSQL)

            If dt.Rows.Count > 0 Then
                If dt.Rows(0).Item("ID").ToString <> "" Then
                    Dim updateSQL As String = "Update PassCardHistory set enddate = getDate() where PassCardHistoryId = " & dt.Rows(0).Item("ID").ToString
                    dm.execTableInTrans(updateSQL, cn, trans)
                End If
            End If

            'New pass card
            pchEnt.setPassCardMstrId(ddNewPass.SelectedValue)
            pchEnt.setDebtorId(hidDebtorId.Value)
            pchEnt.setDeposit(txtDeposit.Text)
            pchEnt.setStartDate(txtActivationDate.Text)
            pchEnt.setLastUpdatedBy(lp.getUserMstrId)
            pchEnt.setLastUpdatedDatetime(Now)
            pchEnt.setLocationInfoId(hidLocationInfoId.Value)
            pchDao.insertDB(pchEnt, cn, trans)



            'Insert into ItemReplacement Table
            irEnt.setActivationDate(txtActivationDate.Text)
            irEnt.setDebtorId(hidDebtorId.Value)
            irEnt.setDONo(Trim(txtDoNo.Text))
            irEnt.setLastUpdatedBy(lp.getUserMstrId)
            irEnt.setLastUpdatedDatetime(Now)
            irEnt.setOldPassCardMstrId(ddOldPass.SelectedValue)
            irEnt.setNewPassCardMstrId(ddNewPass.SelectedValue)
            irEnt.setProcessedBy(lp.getUserMstrId)
            irEnt.setReason(ddReason.SelectedValue)
            irEnt.setReceiptNo(receiptNo)
            irEnt.setDeposit(txtDeposit.Text)
            irEnt.setStatus(ItemReplacementStatusEnum.SUBMITTED)
            irEnt.setTransactionDate(txtTransactionDate.Text)
            irDao.insertDB(irEnt, cn, trans)

            trans.Commit()

            If dpId > 0 Then
                PrintReceipt(dpId, hidDebtorId.Value, txtDeposit.Text, ddNewPass.SelectedItem.Text)
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopUp", "checkToPopUpViewer();", True)
            End If


            lblmsg.Text = ""

            clearData()

        Catch ex As Exception
            trans.Rollback()
            lblmsg.Text = ex.Message
            logger.Error(ex.Message)
        Finally
            trans.Dispose()
            cn.Close()

        End Try
    End Sub

    Protected Sub ddItemType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        bindPass()
    End Sub

    Private Sub bindPass()
        Try
            Dim sql As String
            sql = "select passcardmstrid,SERIALNO,0 as seq from passcardmstr " & _
                  " where debtorid = " & hidDebtorId.Value & _
                  " and itemtype = '" & ddItemType.SelectedValue & "'" & _
                  " and status = '" & PassCardStatusEnum.INUSE & "'" & _
                  " union all " & _
                  " select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' " & _
                  " order by seq,serialno"

            dsOldPass.SelectCommand = sql
            dsOldPass.DataBind()

            sql = "select passcardmstrid,SERIALNO,0 as seq from passcardmstr " & _
                 " where status = '" & PassCardStatusEnum.AVAILABLE & "'" & _
                 " and itemtype = '" & ddItemType.SelectedValue & "'" & _
                 " and locationinfoid = " & hidLocationInfoId.Value & _
                 " union all " & _
                 " select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' " & _
                 " order by seq,serialno"

            dsNewPass.SelectCommand = sql
            dsNewPass.DataBind()

            txtDeposit.Text = ""

        Catch ex As Exception
            logger.Error(ex.Message)
        End Try
    End Sub

    Protected Sub ddOldPass_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt As New DataTable
        Dim sql As String = ""

        Try
            sql = "SELECT DEPOSIT FROM PASSCARDMSTR WHERE PASSCARDMSTRID = " & ddOldPass.SelectedValue
            dt = dm.execTable(sql)

            If dt.Rows.Count = 0 Then
                txtDeposit.Text = 0
            Else
                txtDeposit.Text = dt.Rows(0).Item("DEPOSIT").ToString
            End If


        Catch ex As Exception
            logger.Debug(ex.Message)
            lblmsg.Text = ex.Message

        Finally
            dt = Nothing
        End Try
    End Sub

    Protected Sub ddPaymentType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If ddPaymentType.Text = PaymentTypeEnum.CHEQUE Then
            lblRef.Visible = True
            lblRef.Text = "Cheque No"
            txtNo.Visible = True
        ElseIf ddPaymentType.Text = PaymentTypeEnum.CREDITCARD Then
            lblRef.Visible = True
            lblRef.Text = "Approval No"
            txtNo.Visible = True
        Else
            lblRef.Visible = False
            txtNo.Visible = False
        End If
    End Sub


    Private Function process(ByRef cn As SqlConnection, ByRef trans As SqlTransaction) As Long

        Dim invEnt As New CPM.InvoiceHistoryEntity
        Dim invDao As New CPM.InvoiceHistoryDAO
        Dim dahEnt As New CPM.DebtorAccountHeaderEntity
        Dim dahDao As New CPM.DebtorAccountHeaderDAO
        Dim dadEnt As New CPM.DebtorAccountDetailEntity
        Dim dadDao As New CPM.DebtorAccountDetailDAO
        Dim dpEnt As New CPM.DebtorPaymentEntity
        Dim dpDao As New CPM.DebtorPaymentDAO

        Try
            dahEnt.setDebtorId(hidDebtorId.Value)
            dahEnt.setInvoiceNo(dm.getDebitNoteNextRunningNo(hidLocationInfoId.Value, trans, cn))
            dahEnt.setInvoiceDate(Now)
            dahEnt.setInvoicePeriod("")
            dahEnt.setLastUpdatedBy(lp.getUserMstrId)
            dahEnt.setLastUpdatedDatetime(Now)
            dahEnt.setStatus(InvoiceStatusEnum.PAID)
            dahEnt.setAmount(Val(txtDeposit.Text))
            dahEnt.setBatchNo("")
            dahEnt.setTxnType(TxnTypeEnum.DEBITNOTE)
            Dim dahId As Long = dahDao.insertDB(dahEnt, cn, trans)

            dadEnt.setDebtorAccountHeaderId(dahId)
            dadEnt.setMonths("")
            dadEnt.setDetails(ddReason.SelectedItem.Text)
            dadEnt.setUnitPrice(0)
            dadEnt.setQuantity(0)
            dadEnt.setAmount(Val(txtDeposit.Text))
            dadEnt.setLastUpdatedBy(lp.getUserMstrId)
            dadEnt.setLastUpdatedDatetime(Now)
            dadDao.insertDB(dadEnt, cn, trans)

            invEnt.setDebtorId(hidDebtorId.Value)
            invEnt.setDebtorAccountHeaderId(dahId)
            invEnt.setStatus(InvoiceStatusEnum.PAID)
            invEnt.setMonth(txtTransactionDate.Text)
            invEnt.setAmount(Val(txtDeposit.Text))
            invEnt.setLastUpdatedBy(lp.getUserMstrId)
            invEnt.setLastUpdatedDatetime(Now)
            Dim invHistId As Long = invDao.insertDB(invEnt, cn, trans)

            dpEnt.setAmount(Val(txtDeposit.Text))
            dpEnt.setDebtorAccountHeaderId(dahId)
            dpEnt.setDebtorId(hidDebtorId.Value)
            dpEnt.setDescription("Payment For Pass Card Deposit")
            dpEnt.setPaymentFor(Trim(txtRemark.Text))
            dpEnt.setLastUpdatedBy(lp.getUserMstrId)
            dpEnt.setLastUpdatedDatetime(Now)
            dpEnt.setPaymentType(ddPaymentType.SelectedValue)
            dpEnt.setRefNo(txtNo.Text)
            dpEnt.setPaymentDate(txtTransactionDate.Text)
            dpEnt.setTxnType(TxnTypeEnum.RECEIPT)
            dpEnt.setStatus(ReceiptStatusEnum._NEW)
            dpEnt.setReceiptNo(dm.getReceiptNextRunningNo(hidLocationInfoId.Value, trans, cn))
            dpEnt.setInvoiceHistoryIdAndAmount(invHistId & "|" & Val(txtDeposit.Text))
            Dim dpId As Long = dpDao.insertDB(dpEnt, cn, trans)

            receiptNo = dpEnt.getReceiptNo

            Return dpId

        Catch ex As Exception
            Throw ex
        Finally
            invEnt = Nothing
            invDao = Nothing
            dahEnt = Nothing
            dahDao = Nothing
        End Try

    End Function

    Private Sub PrintReceipt(ByVal debtorPaymentId As Long, ByVal debtorId As Long, ByVal Amt As String, ByVal CardNo As String)
        Dim rptMgr As New ReportManager
        Dim sqlmap As New SQLMap
        Dim mySql As String = ""
        Dim dt As New DataTable
        Dim companyName As String = ""
        Dim companyAddress As String = ""
        Dim debtorName As String = ""
        Dim category As String = ""
        'Dim passCard As String = ""

        Dim tel As String = ""
        Dim fax As String = ""

        Dim hqInfoDao As New CPM.HQInfoDAO
        Dim debtorDao As New CPM.DebtorDAO
        'Dim passCardDao As New CPM.PassCardMstrDAO
        'Dim searchModel As New CPM.PassCardMstrEntity


        Try


            mySql = "SELECT COMPANYNAME,ADDRESS1,ADDRESS2,ADDRESS3,POSTCODE,TELEPHONE,FAX,(SELECT CODEDESC FROM CODEMSTR WHERE CODECAT='STA' AND CODEABBR= STATE) AS STATE FROM HQINFO"
            dt = dm.execTable(mySql)

            If dt.Rows.Count > 0 Then
                companyName = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_CompanyName)
                companyAddress = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address1) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address2) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address3) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_PostCode) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_State)
                tel = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Telephone)
                fax = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Fax)
            End If

            rptMgr.setReportName("Receipt.Rpt")
            rptMgr.setParameterDiscrete("CompanyName", companyName)
            rptMgr.setParameterDiscrete("Address", companyAddress)
            rptMgr.setParameterDiscrete("TelephoneNo", tel)
            rptMgr.setParameterDiscrete("Fax", fax)

            Dim myDebtorSql As String = "SELECT * FROM DEBTOR WHERE DEBTORID = " & debtorId
            dt = dm.execTable(myDebtorSql)
            If dt.Rows.Count > 0 Then
                debtorName = dt.Rows.Item(0).Item(debtorDao.COLUMN_Name)
                category = dt.Rows.Item(0).Item(debtorDao.COLUMN_Category)
            End If

            'searchModel.setDebtorId(debtorId)
            'Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorPassBayNo", searchModel)

            'dt = dm.execTable(strSQL)
            'If dt.Rows.Count > 0 Then
            '    passCard = dt.Rows(0).Item(0).ToString
            'End If

            rptMgr.setParameterDiscrete("debtorPaymentId", debtorPaymentId)
            rptMgr.setParameterDiscrete("CardNo", CardNo)

            rptMgr.setParameterDiscrete("season", "")
            rptMgr.setParameterDiscrete("reserved", "")
            rptMgr.setParameterDiscrete("deposit", "")
            rptMgr.setParameterDiscrete("others", "")

            rptMgr.setParameterDiscrete("seasonQty", "")
            rptMgr.setParameterDiscrete("reservedQty", "")
            rptMgr.setParameterDiscrete("depositQty", "")
            rptMgr.setParameterDiscrete("othersQty", "")

            rptMgr.setParameterDiscrete("rm", Utility.Tools.SpellNumber(Amt))

	    rptMgr.setParameterDiscrete("PrintedBy", lp.getUserLoginId)

            rptMgr.Logon()
            hdPreview.Value = "1"
            'set reportManager to session
            Session("ReportManager") = rptMgr
            lblmsg.Text = ""



        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            hqInfoDao = Nothing
            debtorDao = Nothing
            'passCardDao = Nothing
            dt = Nothing
            rptMgr = Nothing

        End Try

    End Sub

    Protected Sub ddReason_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If ddReason.SelectedValue = ItemReplacementReasonEnum.LOST Or ddReason.SelectedValue = ItemReplacementReasonEnum.MISHANDLING Then
            ddPaymentType.Visible = True
            lblPaymentType.Visible = True
        Else
            ddPaymentType.Visible = False
            lblPaymentType.Visible = False
        End If
    End Sub
End Class

