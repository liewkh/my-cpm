Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports PopCalendarSpecialDay

Partial Class Transaction_Receipt
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim manualReceipt As String = ""


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            Session.LCID = 2057

            manualReceipt = Request.Params("manual")

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

                Dim dtPaymentDate As New DataTable
                Dim dtBankInDate As New DataTable

                sql = "select getDate()"
                dtPaymentDate = dm.execTable(sql)

                txtPaymentDate.Text = Utility.DataTypeUtils.formatDateString(dtPaymentDate.Rows.Item(0)(0).ToString())

                Dim searchModel As New CPM.CodeMstrEntity
                Dim sqlmap As New SQLMap

                searchModel.setCodeCat(CodeCatEnum.BANKINDATE)
                searchModel.setCodeAbbr(Trim(txtPaymentDate.Text.ToUpper))
                searchModel.setActive(ConstantGlobal.Yes)

                Dim strSQL As String = sqlmap.getMappedStatement("SetupMstr/Search-CodeMstr", searchModel)
                dtBankInDate = dm.execTable(strSQL)

                If dtBankInDate.Rows.Count > 0 Then
                    txtBankInDate.Text = Utility.DataTypeUtils.formatDateString(dtBankInDate.Rows.Item(0)(2))
                Else
                    txtBankInDate.Text = Utility.DataTypeUtils.formatDateString(DateAdd(DateInterval.Day, 1, dtPaymentDate.Rows.Item(0)(0)))
                End If

                txtNoPrint.Text = 3

            End If
            ddLocation.SelectedValue = lp.getDefaultLocationInfoId
            ddBankCode.SelectedValue = lp.getDefaultBankCode

            divSearch.Visible = True
            divInv.Visible = False

            If manualReceipt = "Y" Then
                divPrint.Visible = False
                lblHeader1.Text = "Manual Receipt"
                txtPaymentDate.Enabled = True
                popCalendar1.Enabled = True
            Else
                divPrint.Visible = True
                lblHeader1.Text = "Online Receipt"
                txtPaymentDate.Enabled = False
                popCalendar1.Enabled = False
            End If

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

            If Trim(txtRef1.Text) <> "" Then
                searchModel.setRef1(Trim(txtRef1.Text.ToUpper))
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

            Dim strSQL As String = ""

            If txtPassCardNo.Text <> "" Then
                searchModel.setSerialNo(Trim(txtPassCardNo.Text))
                strSQL = sqlmap.getMappedStatement("Debtor/Search-DebtorByPassBayNo", searchModel)
            Else
                strSQL = sqlmap.getMappedStatement("Debtor/Search-Debtor", searchModel)
            End If

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
        ddBankCode.SelectedValue = lp.getDefaultBankCode
        txtDebtorName.Text = ""
        rbCompany.Checked = True
        rbIndividual.Checked = False
        'txtPaymentDate.Text = Now.ToShortDateString
        txtTotalOS.Text = ""
        txtSelected.text = ""
        txtDescription.Text = ""
        txtPaymentAmount.Text = ""
        'ddPaymentType.SelectedIndex = 0
        txtReceiptNo.Text = ""
        txtNo.Text = ""
        txtPassCardNo.Text = ""
        txtNoPrint.Text = 3
        gvDebtorEnq.DataSource = Nothing
        gvDebtorInv.DataSource = Nothing
        ddInvoice.Items.Clear()
        txtBankInDate.Enabled = True
        txtBankInDate.Text = ""
        txtRef1.Text = ""
        getBankInDate()
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


        btnMiscPayment.Attributes.Add("OnClick", "javascript:open_popupModal('../Transaction/MiscPayment.aspx?debtorId=" + hidDebtorId.Value + "','L');__doPostBack('btnSearch_Click','');")

        Dim lnkbtn As LinkButton
        lnkbtn = Me.FindControl("LinkButton1")
        ClientScript.RegisterForEventValidation(lnkbtn.UniqueID)

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


    Protected Sub gvDebtorInv_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDebtorInv.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvDebtorInv.UniqueID + "','Select$" + gvDebtorInv.Rows.Count.ToString + "');")
            Dim int As Integer = gvDebtorInv.Rows.Count / 2
            Dim dob As Double = gvDebtorInv.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

        End If


    End Sub

    Protected Sub gvDebtorInv_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDebtorInv.RowCommand
        'For Print
        If e.CommandName.Equals("Print") Then
            'PrintReceipt(e.CommandArgument)
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
        Dim Sql As String = ""

        Try

            Sql = "select dah.debtoraccountheaderid,CONVERT(VARCHAR(19),dah.invoicedate,103) + ' | ' + dah.invoiceno + ' | ' + " & _
               " dah.invoiceperiod + ' | ' + CONVERT(varchar(100),dah.amount) + ' | ' + CONVERT(varchar(100),dah.amount-isnull(dah.PaidAmount,0)) as invoiceno,'' as seq " & _
               " from debtoraccountheader dah where (dah.amount-isnull(dah.PaidAmount,0)) <> 0 and dah.status <> 'C'and dah.debtorid = " + gvDebtorEnq.SelectedDataKey(debtorDao.COLUMN_DebtorID).ToString & _
               " union all select codeabbr,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,invoiceno"

            dsInvoice.SelectCommand = Sql
            dsInvoice.DataBind()

            '''''''''''''''''''''''

            hidDebtorId.Value = gvDebtorEnq.SelectedDataKey(debtorDao.COLUMN_DebtorID).ToString
            txtDebtorName.Text = gvDebtorEnq.SelectedDataKey("DEBTOR").ToString
            hidLocationInfoId.Value = gvDebtorEnq.SelectedDataKey(debtorDao.COLUMN_LocationInfoId).ToString
            txtRef1.Text = gvDebtorEnq.SelectedDataKey(debtorDao.COLUMN_Ref1).ToString
            'DataMode()


        Catch ex As Exception
            logger.Error(ex.Message)
            lblmsg.Text = ex.Message
        Finally
            debtorDao = Nothing
        End Try

    End Sub

    Protected Sub ddInvoice_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        DataMode()
    End Sub

    Protected Sub gvDebtorInv_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDebtorInv.SelectedIndexChanged
        Dim total As Double = 0
        Dim strSQL As String = "" 
       
        hidDebtorAccountHeaderId.Value = ""

        Try
            For Each row As GridViewRow In gvDebtorInv.Rows
                Dim chk As CheckBox

                chk = row.FindControl("chkSelect")
                If Not chk Is Nothing Then

                    If chk.Checked Then
                        If Not String.IsNullOrEmpty(gvDebtorInv.DataKeys(row.RowIndex)("MONTH").ToString) Then
                            total += Val(gvDebtorInv.DataKeys(row.RowIndex)("OSAMOUNT").ToString)

                            hidDebtorAccountHeaderId.Value = hidDebtorAccountHeaderId.Value & "," & gvDebtorInv.DataKeys(row.RowIndex)("DEBTORACCOUNTHEADERID").ToString
                            Dim s As String = hidDebtorAccountHeaderId.Value.ToLower

                            Dim sp() As String = s.Split(",")
                            Dim al As New ArrayList()

                            For Each sx As String In sp
                                If (Not al.Contains(sx)) Then
                                    al.Add(sx)
                                End If
                            Next
                            Dim dupRemoved() As String = al.ToArray(GetType(String))
                            hidDebtorAccountHeaderId.Value = String.Join(",", dupRemoved)

                        End If
                    End If
                End If
            Next

            txtPaymentAmount.Text = String.Format("{0:n2}", total)
            txtSelected.Text = String.Format("{0:n2}", total)


        Catch ex As Exception
            lblmsg.Text = ex.Message
            logger.Error(ex.Message)
        Finally

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
        SearchData()

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

    Private Sub SearchData()
        Dim searchModel As New CPM.DebtorAccountHeaderEntity
        Dim sqlmap As New SQLMap
        Dim total As Double
        Dim dt As DataTable

        Try

            lblmsg.Text = ""

            searchModel.setDebtorId(hidDebtorId.Value)
            searchModel.setStatus(InvoiceStatusEnum.OUTSTANDING)
            searchModel.setDebtorAccountHeaderId(ddInvoice.SelectedValue)

            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorInvoiceReceipt", searchModel)

            ViewState("strSQL") = strSQL

            dsDebtorInv.SelectCommand = ViewState("strSQL")
            gvDebtorInv.DataBind()

            gvDebtorInv.PageIndex = 0

            If gvDebtorInv.Rows.Count = 0 Then
                lblRecCount2.Text = ConstantGlobal.No_Record_Found
                SearchMode()
                lblmsg.Text = "No Invoice for the selected debtor."
            Else
                lblRecCount2.Text = dm.getGridViewRecordCount(dsDebtorInv).ToString + " " + "Record Found"

                For Each row As GridViewRow In gvDebtorInv.Rows
                    total += Val(gvDebtorInv.DataKeys(row.RowIndex)("OSAMOUNT").ToString)
                Next


            End If

            txtTotalOS.Text = String.Format("{0:n2}", total)

            If Not (String.IsNullOrEmpty(ddLocation.SelectedValue)) Then
                strSQL = "SELECT BANKCODE FROM LOCATIONINFO WHERE LOCATIONINFOID = " + ddLocation.SelectedValue
                dt = dm.execTable(strSQL)

                If dt.Rows.Count = 1 Then
                    ddBankCode.SelectedValue = dt.Rows(0).Item("BANKCODE").ToString
                End If
            End If

        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            searchModel = Nothing
            sqlmap = Nothing

        End Try
    End Sub


    
    Protected Sub ddPaymentType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If ddPaymentType.Text = PaymentTypeEnum.CHEQUE Then
            divNoRef.Visible = True          
        ElseIf ddPaymentType.Text = PaymentTypeEnum.CREDITCARD Then
            divNoRef.Visible = True
        ElseIf ddPaymentType.Text = PaymentTypeEnum.INTERBANKGIRO Then
            divNoRef.Visible = True
        Else
            divNoRef.Visible = False
        End If

        manualReceipt = IIf(String.IsNullOrEmpty(Request.Params("manual")), "", Request.Params("manual"))

 
 

        txtBankInDate.Enabled = True
        popCalendar2.Enabled = True

        If String.IsNullOrEmpty(manualReceipt) Then   'if true means online receipt program
            If ddPaymentType.Text = PaymentTypeEnum.CASH Then
'vk - changed to false for online receipt on 25/7/2016
                txtBankInDate.Enabled = False
                popCalendar2.Enabled = False
                getBankInDate()
            ElseIf ddPaymentType.Text = PaymentTypeEnum.CREDITCARD Then
                txtBankInDate.Enabled = False
                popCalendar2.Enabled = False
                txtBankInDate.Text = txtPaymentDate.Text
            End If 
	          
        End If



    End Sub

    Protected Sub btnDataBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        SearchMode()
        clearInvoice()
    End Sub

    Private Sub clearInvoice()
        txtDescription.Text = ""
        txtPaymentAmount.Text = ""
        txtSelected.Text = ""
        ddPaymentType.SelectedIndex = 0
        txtNo.Text = ""
        divNoRef.Visible = False
    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim dpEnt As New CPM.DebtorPaymentEntity
        Dim dpDao As New CPM.DebtorPaymentDAO
        Dim payAmt As Double = 0
        Dim invAmt As Double = 0
        Dim invHistEnt As New CPM.InvoiceHistoryEntity
        Dim invHistDao As New CPM.InvoiceHistoryDAO
        Dim dahEnt As New CPM.DebtorAccountHeaderEntity
        Dim dahDao As New CPM.DebtorAccountHeaderDAO
        Dim paymentFor As String = ""
        Dim updateSql As String = ""
        Dim selectSql As String = ""
        Dim dtHeader As New DataTable
        Dim dtInvHist As New DataTable
        Dim flgNotEnufFund As Boolean = False
        Dim invoiceNo As String = ""
        Dim invoiceDate As String = ""
        
        Try

            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If
            trans = cn.BeginTransaction

            If Trim(txtPaymentAmount.Text) = "" Or Trim(Val(txtPaymentAmount.Text)) = 0 Then
                lblmsg.Text = "Please enter payment amount."
                Exit Sub
            End If

            If ddPaymentType.SelectedIndex = 0 Then
                lblmsg.Text = "Please select payment type."
                Exit Sub
            End If

            If ddPaymentType.Text = PaymentTypeEnum.CHEQUE Then
                If Trim(txtNo.Text) = "" Then
                    lblmsg.Text = "Please enter cheque no."
                    Exit Sub
                End If
            End If

            If ddPaymentType.Text = PaymentTypeEnum.CREDITCARD Then
                If Trim(txtNo.Text) = "" Then
                    lblmsg.Text = "Please enter credit card approval no."
                    Exit Sub
                End If
            End If

            If Trim(txtPaymentDate.Text) = "" Then
                lblmsg.Text = "Please enter Payment Date"
                Exit Sub
            End If

            If Trim(txtBankInDate.Text) = "" Then
                lblmsg.Text = "Please enter Bank In Date"
                Exit Sub
            End If


            payAmt = txtPaymentAmount.Text
            Dim str As String = ""
            Dim sql As String = ""
            Dim dt As New DataTable

            For Each row As GridViewRow In gvDebtorInv.Rows
                Dim chk As CheckBox

                'Referring to invoice amount
                chk = row.FindControl("chkSelect")
                If Not chk Is Nothing Then
                    If chk.Checked Then
                        str = str + gvDebtorInv.DataKeys(row.RowIndex)(dahDao.COLUMN_DebtorAccountHeaderID).ToString + ","
                        invoiceNo = gvDebtorInv.DataKeys(row.RowIndex)("INVOICENO").ToString
                        invoiceDate = gvDebtorInv.DataKeys(row.RowIndex)("INVOICEDATE").ToString
                    End If
                End If
            Next

            str = Mid(str, 1, str.Length - 1)
            sql = "select sum(ISNULL(dah.amount,0) - ISNULL(dah.paidamount,0)) as invAmt from debtoraccountheader dah where dah.debtoraccountheaderid in (" & str & ")"
            dt = dm.execTable(sql)

            If dt.Rows(0).Item("INVAMT") > 0 Then
                invAmt = dt.Rows(0).Item("INVAMT")
            Else
                Throw New ApplicationException("Error")
            End If



            'Pay less ok but not extra per invoice base
            If payAmt > invAmt Then
                lblmsg.Text = "Payment Amount cannot be more than Total Outstanding Amount!"
                Exit Sub
            End If



            'Split payment for which invoice if happen
            Dim splitHeaderId As String() = hidDebtorAccountHeaderId.Value.Split(",")
            hidDebtorAccountHeaderId.Value = Mid(hidDebtorAccountHeaderId.Value.ToString, 2)

            'To store PaymentForWhichMonth Id + Amount in future for cancellation reference
            Dim strInvHistoryId As String = ""
            Dim runningPayAmt As Double = 0

            For Each row As GridViewRow In gvDebtorInv.Rows
                Dim chk As CheckBox


                chk = row.FindControl("chkSelect")
                If Not chk Is Nothing Then
                    If chk.Checked Then
                        If Not String.IsNullOrEmpty(gvDebtorInv.DataKeys(row.RowIndex)("OSAMOUNT").ToString) Then
                            'If payAmt > 0 Then                                
                            'If payAmt > Val(gvDebtorInv.DataKeys(row.RowIndex)("OSAMOUNT").ToString) Then
                            'If payAmt < Val(gvDebtorInv.DataKeys(row.RowIndex)("OSAMOUNT").ToString) Then

                            '    invHistEnt.setInvoiceHistoryId(gvDebtorInv.DataKeys(row.RowIndex)(invHistDao.COLUMN_InvoiceHistoryID).ToString)
                            '    invHistEnt.setPaidAmount(payAmt)
                            '    invHistEnt.setLastUpdatedBy(lp.getUserMstrId)
                            '    invHistEnt.setLastUpdatedDatetime(Now)
                            '    invHistDao.updateDB(invHistEnt, cn, trans)

                            '    strInvHistoryId = strInvHistoryId & invHistEnt.getInvoiceHistoryId & "-" & invHistEnt.getPaidAmount & "|"

                            '    'Get the latest PaidAmount from header
                            '    selectSql = "SELECT ISNULL(PAIDAMOUNT,0) AS PAIDAMOUNT FROM DEBTORACCOUNTHEADER WHERE DEBTORACCOUNTHEADERID = " & gvDebtorInv.DataKeys(row.RowIndex)(invHistDao.COLUMN_DebtorAccountHeaderId).ToString
                            '    dtHeader = dm.execTableInTrans(selectSql, cn, trans)

                            '    dahEnt.setDebtorAccountHeaderId(gvDebtorInv.DataKeys(row.RowIndex)(invHistDao.COLUMN_DebtorAccountHeaderId).ToString)
                            '    dahEnt.setPaidAmount(dtHeader.Rows(0).Item(dahDao.COLUMN_PaidAmount) + payAmt)
                            '    dahEnt.setLastUpdatedDatetime(Now)
                            '    dahDao.updateDB(dahEnt, cn, trans)

                            '    selectSql = "SELECT AMOUNT,ISNULL(PAIDAMOUNT,0) AS PAIDAMOUNT FROM DEBTORACCOUNTHEADER WHERE DEBTORACCOUNTHEADERID = " & gvDebtorInv.DataKeys(row.RowIndex)(invHistDao.COLUMN_DebtorAccountHeaderId).ToString
                            '    dtHeader = dm.execTableInTrans(selectSql, cn, trans)
                            '    If dtHeader.Rows(0).Item(dahDao.COLUMN_Amount).Equals(dtHeader.Rows(0).Item(dahDao.COLUMN_PaidAmount)) Then
                            '        updateSql = "UPDATE DEBTORACCOUNTHEADER SET STATUS = '" & InvoiceStatusEnum.PAID & "' WHERE DEBTORACCOUNTHEADERID = " & gvDebtorInv.DataKeys(row.RowIndex)(invHistDao.COLUMN_DebtorAccountHeaderId).ToString
                            '        dtHeader = dm.execTableInTrans(updateSql, cn, trans)
                            '        updateSql = "UPDATE INVOICEHISTORY SET STATUS = '" & InvoiceStatusEnum.PAID & "' WHERE DEBTORACCOUNTHEADERID = " & gvDebtorInv.DataKeys(row.RowIndex)(invHistDao.COLUMN_DebtorAccountHeaderId).ToString
                            '        dtHeader = dm.execTableInTrans(updateSql, cn, trans)
                            '    End If
                            '    payAmt -= Val(gvDebtorInv.DataKeys(row.RowIndex)("OSAMOUNT").ToString)


                            'Else 'Still got enough to cover the monthly amount

                            selectSql = "SELECT ISNULL(PAIDAMOUNT,0) AS PAIDAMOUNT FROM INVOICEHISTORY WHERE INVOICEHISTORYID = " & gvDebtorInv.DataKeys(row.RowIndex)(invHistDao.COLUMN_InvoiceHistoryID).ToString
                            dtInvHist = dm.execTableInTrans(selectSql, cn, trans)


                            invHistEnt.setInvoiceHistoryId(gvDebtorInv.DataKeys(row.RowIndex)(invHistDao.COLUMN_InvoiceHistoryID).ToString)
                            If payAmt < Val(gvDebtorInv.DataKeys(row.RowIndex)("OSAMOUNT").ToString) Then
                                If payAmt < 0 Then
                                    flgNotEnufFund = True
                                    invHistEnt.setPaidAmount(0)
                                Else
                                    invHistEnt.setPaidAmount(dtInvHist.Rows(0).Item(dahDao.COLUMN_PaidAmount) + payAmt)
                                End If
                            Else
                                invHistEnt.setPaidAmount(dtInvHist.Rows(0).Item(dahDao.COLUMN_PaidAmount) + Val(gvDebtorInv.DataKeys(row.RowIndex)("OSAMOUNT").ToString))
                            End If



                            invHistEnt.setLastUpdatedBy(lp.getUserMstrId)
                            invHistEnt.setLastUpdatedDatetime(Now)
                            invHistDao.updateDB(invHistEnt, cn, trans)


                            runningPayAmt += payAmt
                            payAmt -= Val(gvDebtorInv.DataKeys(row.RowIndex)("OSAMOUNT").ToString)


                            'Get the latest PaidAmount from header
                            selectSql = "SELECT ISNULL(PAIDAMOUNT,0) AS PAIDAMOUNT FROM DEBTORACCOUNTHEADER WHERE DEBTORACCOUNTHEADERID = " & gvDebtorInv.DataKeys(row.RowIndex)(invHistDao.COLUMN_DebtorAccountHeaderId).ToString
                            dtHeader = dm.execTableInTrans(selectSql, cn, trans)

                            dahEnt.setDebtorAccountHeaderId(gvDebtorInv.DataKeys(row.RowIndex)(invHistDao.COLUMN_DebtorAccountHeaderId).ToString)
                            If payAmt < 0 Then
                                If flgNotEnufFund Then
                                    Continue For
                                End If
                                'dahEnt.setPaidAmount(dtHeader.Rows(0).Item(dahDao.COLUMN_PaidAmount) + runningPayAmt)
                                dahEnt.setPaidAmount(dtHeader.Rows(0).Item(dahDao.COLUMN_PaidAmount) + (Val(gvDebtorInv.DataKeys(row.RowIndex)("OSAMOUNT").ToString) + payAmt))
                                'strInvHistoryId = strInvHistoryId & invHistEnt.getInvoiceHistoryId & "-" & dtHeader.Rows(0).Item(dahDao.COLUMN_PaidAmount) + (Val(gvDebtorInv.DataKeys(row.RowIndex)("OSAMOUNT").ToString) + payAmt) & "|"
                                strInvHistoryId = strInvHistoryId & invHistEnt.getInvoiceHistoryId & "-" & (Val(gvDebtorInv.DataKeys(row.RowIndex)("OSAMOUNT").ToString) + payAmt) & "|"
                            Else
                                dahEnt.setPaidAmount(dtHeader.Rows(0).Item(dahDao.COLUMN_PaidAmount) + Val(gvDebtorInv.DataKeys(row.RowIndex)("OSAMOUNT").ToString))
                                strInvHistoryId = strInvHistoryId & invHistEnt.getInvoiceHistoryId & "-" & gvDebtorInv.DataKeys(row.RowIndex)("OSAMOUNT").ToString & "|"
                            End If

                            dahEnt.setLastUpdatedDatetime(Now)
                            dahDao.updateDB(dahEnt, cn, trans)

                            selectSql = "SELECT AMOUNT,ISNULL(PAIDAMOUNT,0) AS PAIDAMOUNT FROM DEBTORACCOUNTHEADER WHERE DEBTORACCOUNTHEADERID = " & gvDebtorInv.DataKeys(row.RowIndex)(invHistDao.COLUMN_DebtorAccountHeaderId).ToString

                            dtHeader = dm.execTableInTrans(selectSql, cn, trans)
                            If dtHeader.Rows(0).Item(dahDao.COLUMN_Amount).Equals(dtHeader.Rows(0).Item(dahDao.COLUMN_PaidAmount)) Then
                                updateSql = "UPDATE DEBTORACCOUNTHEADER SET STATUS = '" & InvoiceStatusEnum.PAID & "' WHERE DEBTORACCOUNTHEADERID = " & gvDebtorInv.DataKeys(row.RowIndex)(invHistDao.COLUMN_DebtorAccountHeaderId).ToString
                                dtHeader = dm.execTableInTrans(updateSql, cn, trans)
                                updateSql = "UPDATE INVOICEHISTORY SET STATUS = '" & InvoiceStatusEnum.PAID & "' WHERE DEBTORACCOUNTHEADERID = " & gvDebtorInv.DataKeys(row.RowIndex)(invHistDao.COLUMN_DebtorAccountHeaderId).ToString
                                dtHeader = dm.execTableInTrans(updateSql, cn, trans)
                            End If

                            'End If


                        End If
                    End If
                End If
                'End If
            Next

            'The additional extra will put into last invoice
            If payAmt > 0 Then
                Dim iSql
                Dim dtiSql As DataTable


                iSql = "select * from invoicehistory ih where (ih.amount-isnull(ih.PaidAmount,0)) > 0 and ih.status = '" & InvoiceStatusEnum.OUTSTANDING & "'"
                dtiSql = dm.execTableInTrans(iSql, cn, trans)

                invHistEnt.setInvoiceHistoryId(dtiSql.Rows(0).Item(invHistDao.COLUMN_InvoiceHistoryID))
                invHistEnt.setPaidAmount(payAmt)
                invHistEnt.setLastUpdatedBy(lp.getUserMstrId)
                invHistEnt.setLastUpdatedDatetime(Now)
                invHistDao.updateDB(invHistEnt, cn, trans)

                strInvHistoryId = strInvHistoryId & dtiSql.Rows(0).Item(invHistDao.COLUMN_InvoiceHistoryID) & "-" & invHistEnt.getPaidAmount & "|"

                'Get the latest PaidAmount from header
                iSql = "SELECT ISNULL(PAIDAMOUNT,0) AS PAIDAMOUNT FROM DEBTORACCOUNTHEADER WHERE DEBTORACCOUNTHEADERID = " & dtiSql.Rows(0).Item(invHistDao.COLUMN_DebtorAccountHeaderId)
                dtHeader = dm.execTableInTrans(iSql, cn, trans)

                dahEnt.setDebtorAccountHeaderId(dtiSql.Rows(0).Item(invHistDao.COLUMN_DebtorAccountHeaderId))
                dahEnt.setPaidAmount(dtHeader.Rows(0).Item(dahDao.COLUMN_PaidAmount) + payAmt)
                dahEnt.setLastUpdatedDatetime(Now)
                dahDao.updateDB(dahEnt, cn, trans)



                selectSql = "SELECT AMOUNT,ISNULL(PAIDAMOUNT,0) AS PAIDAMOUNT FROM DEBTORACCOUNTHEADER WHERE DEBTORACCOUNTHEADERID = " & dtiSql.Rows(0).Item(invHistDao.COLUMN_DebtorAccountHeaderId)
                dtHeader = dm.execTableInTrans(selectSql, cn, trans)
                If dtHeader.Rows(0).Item(dahDao.COLUMN_Amount).Equals(dtHeader.Rows(0).Item(dahDao.COLUMN_PaidAmount)) Then
                    updateSql = "UPDATE DEBTORACCOUNTHEADER SET STATUS = '" & InvoiceStatusEnum.PAID & "' WHERE DEBTORACCOUNTHEADERID = " & dtiSql.Rows(0).Item(invHistDao.COLUMN_DebtorAccountHeaderId)
                    dtHeader = dm.execTableInTrans(updateSql, cn, trans)
                    updateSql = "UPDATE INVOICEHISTORY SET STATUS = '" & InvoiceStatusEnum.PAID & "' WHERE DEBTORACCOUNTHEADERID = " & dtiSql.Rows(0).Item(invHistDao.COLUMN_DebtorAccountHeaderId)
                    dtHeader = dm.execTableInTrans(updateSql, cn, trans)
                End If


            End If

            'Get the payment for which month
            For Each row As GridViewRow In gvDebtorInv.Rows
                Dim chk As CheckBox

                chk = row.FindControl("chkSelect")
                If Not chk Is Nothing Then

                    If chk.Checked Then
                        If Not String.IsNullOrEmpty(gvDebtorInv.DataKeys(row.RowIndex)("MONTH").ToString) Then
                            paymentFor += gvDebtorInv.DataKeys(row.RowIndex)("MONTH").ToString & ","
                        End If
                    End If
                End If
            Next

            paymentFor = Mid(paymentFor, 1, paymentFor.Length)
            strInvHistoryId = Mid(strInvHistoryId, 1, strInvHistoryId.Length)

            dpEnt.setAmount(txtPaymentAmount.Text)
            dpEnt.setLastUpdatedBy(lp.getUserMstrId)
            dpEnt.setDebtorId(hidDebtorId.Value)
            dpEnt.setRefNo(txtNo.Text)
            dpEnt.setInvoiceHistoryIdAndAmount(strInvHistoryId)
            dpEnt.setPaymentFor(paymentFor)
            dpEnt.setDebtorAccountHeaderId(ddInvoice.SelectedValue)
            dpEnt.setPaymentDate(txtPaymentDate.Text)
            dpEnt.setBankInDate(txtBankInDate.Text)
            dpEnt.setDescription(Trim(txtDescription.Text))
            dpEnt.setPaymentType(ddPaymentType.SelectedValue)
            dpEnt.setStatus(ReceiptStatusEnum._NEW)            
            dpEnt.setTxnType(TxnTypeEnum.RECEIPT)
            dpEnt.setBankCode(ddBankCode.SelectedValue)
            dpEnt.setInvoiceNo(invoiceNo)
            dpEnt.setInvoiceDate(CDate(invoiceDate))

            dpEnt.setReceiptNo(dm.getReceiptNextRunningNo(hidLocationInfoId.Value, trans, cn))


            dpEnt.setLastUpdatedDatetime(Now)
            Dim dpId As Long = dpDao.insertDB(dpEnt, cn, trans)

            'Throw New Exception("Test")

            trans.Commit()
            'trans.Rollback()

            SearchData()

            If manualReceipt = "Y" Then
                divPrint.Visible = False
            Else
                PrintReceipt(dpId, hidDebtorId.Value, dpEnt.getAmount)
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopUp", "checkToPopUpViewer();", True)
            End If


            lblmsg.Text = ""

            clearInvoice()

        Catch ex As Exception
            trans.Rollback()
            lblmsg.Text = ex.Message
        Finally
            trans.Dispose()
            cn.Close()

            dpEnt = Nothing
            dpDao = Nothing
            invHistEnt = Nothing
            invHistDao = Nothing
            dahEnt = Nothing
            dahDao = Nothing
            dtHeader = Nothing
            dtInvHist = Nothing
        End Try
    End Sub

    Private Sub PrintReceipt(ByVal debtorPaymentId As Long, ByVal debtorId As Long, ByVal Amt As String)
        Dim rptMgr As New ReportManager
        Dim sqlmap As New SQLMap
        Dim mySql As String = ""
        Dim dt As New DataTable
        Dim companyName As String = ""
        Dim companyAddress As String = ""
        Dim debtorName As String = ""
        Dim category As String = ""
        Dim passCard As String = ""

        Dim tel As String = ""
        Dim fax As String = ""
        Dim unitNo As String = ""
        Dim block As String = ""
        Dim duration As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim rcpPaymentFor As String = ""

        Dim hqInfoDao As New CPM.HQInfoDAO
        Dim debtorDao As New CPM.DebtorDAO
        Dim passCardDao As New CPM.PassCardMstrDAO
        Dim searchModel As New CPM.PassCardMstrEntity


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

            searchModel.setDebtorId(debtorId)
            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorPassBayNo", searchModel)

            dt = dm.execTable(strSQL)
            If dt.Rows.Count > 0 Then
                passCard = dt.Rows(0).Item(0).ToString
            End If

            rptMgr.setParameterDiscrete("debtorPaymentId", debtorPaymentId)
            rptMgr.setParameterDiscrete("CardNo", passCard)

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
            passCardDao = Nothing
            dt = Nothing
            rptMgr = Nothing

        End Try

    End Sub

    Public Function RemoveDuplicates(ByVal items As String()) As String()
        Dim noDupsArrList As New ArrayList()
        For i As Integer = 0 To items.Length - 1
            If Not noDupsArrList.Contains(items(i).Trim()) Then
                noDupsArrList.Add(items(i).Trim())
            End If
        Next

        Dim uniqueItems As String() = New String(noDupsArrList.Count - 1) {}
        noDupsArrList.CopyTo(uniqueItems)
        Return uniqueItems
    End Function

    Public Sub getBankInDate()
        Dim dtPaymentDate As New DataTable
        Dim dtBankInDate As New DataTable
        Dim Sql As String

        Sql = "select getDate()"
        dtPaymentDate = dm.execTable(Sql)

        txtPaymentDate.Text = Utility.DataTypeUtils.formatDateString(dtPaymentDate.Rows.Item(0)(0).ToString())

        Dim searchModel As New CPM.CodeMstrEntity
        Dim sqlmap As New SQLMap

        searchModel.setCodeCat(CodeCatEnum.BANKINDATE)
        searchModel.setCodeAbbr(Trim(txtPaymentDate.Text.ToUpper))
        searchModel.setActive(ConstantGlobal.Yes)

        Dim strSQL As String = sqlmap.getMappedStatement("SetupMstr/Search-CodeMstr", searchModel)
        dtBankInDate = dm.execTable(strSQL)


        'If ddPaymentType.Text = PaymentTypeEnum.CASH Then
        If dtBankInDate.Rows.Count > 0 Then
            txtBankInDate.Text = Utility.DataTypeUtils.formatDateString(dtBankInDate.Rows.Item(0)(2))
        Else
            txtBankInDate.Text = Utility.DataTypeUtils.formatDateString(DateAdd(DateInterval.Day, 1, dtPaymentDate.Rows.Item(0)(0)))
        End If
        'txtBankInDate.Enabled = False
        'popCalendar2.Enabled = False
        'ElseIf ddPaymentType.Text = PaymentTypeEnum.CREDITCARD Then
        'txtBankInDate.Text = txtPaymentDate.Text
        'txtBankInDate.Enabled = False
        'ElseIf ddPaymentType.Text = PaymentTypeEnum.INTERBANKGIRO Or ddPaymentType.Text = PaymentTypeEnum.CHEQUE Then
        'txtBankInDate.Enabled = True
        'popCalendar2.Enabled = True
        'End If


    End Sub


    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        manualReceipt = IIf(String.IsNullOrEmpty(Request.Params("manual")), "", Request.Params("manual"))

        If Not String.IsNullOrEmpty(manualReceipt) Then
            Dim dtPaymentDate As New DataTable
            Dim dtBankInDate As New DataTable
            Dim Sql As String

            Sql = "select getDate()"
            dtPaymentDate = dm.execTable(Sql)

            'txtPaymentDate.Text = Utility.DataTypeUtils.formatDateString(dtPaymentDate.Rows.Item(0)(0).ToString())

            Dim searchModel As New CPM.CodeMstrEntity
            Dim sqlmap As New SQLMap

            searchModel.setCodeCat(CodeCatEnum.BANKINDATE)
            searchModel.setCodeAbbr(Trim(txtPaymentDate.Text))
            searchModel.setActive(ConstantGlobal.Yes)

            Dim strSQL As String = sqlmap.getMappedStatement("SetupMstr/Search-CodeMstr", searchModel)
            dtBankInDate = dm.execTable(strSQL)

            If dtBankInDate.Rows.Count > 0 Then
                txtBankInDate.Text = Utility.DataTypeUtils.formatDateString(dtBankInDate.Rows.Item(0)(2))
            Else
                txtBankInDate.Text = Utility.DataTypeUtils.formatDateString(DateAdd(DateInterval.Day, 1, Date.Parse(txtPaymentDate.Text)))
            End If

        End If
    End Sub
End Class
