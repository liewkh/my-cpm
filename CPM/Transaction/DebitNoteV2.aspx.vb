Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports PopCalendarSpecialDay

Partial Class Transaction_DebitNoteV2
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


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

                txtDebitNoteDate.Text = Utility.DataTypeUtils.formatDateString(Now)


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
        txtAmount.Text = 0
        txtQty.Text = 0
        txtMonth.Text = 0
        ddDescription.SelectedIndex = 0
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        bindData()
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        clear()
        txtDebtorName.Text = ""        
        ViewState("strSQL") = Nothing
        bindData()
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
            hidDebtorId.value = gvDebtorEnq.SelectedDataKey(debtorDao.COLUMN_DebtorID).ToString
            txtDebtorName.Text = gvDebtorEnq.SelectedDataKey("DEBTOR").ToString
            hidLocationInfoId.value = gvDebtorEnq.SelectedDataKey(debtorDao.COLUMN_LocationInfoId).ToString
            DataMode()


        Catch ex As Exception
            logger.Error(ex.Message)
            lblmsg.Text = ex.Message
        Finally
            debtorDao = Nothing
        End Try

    End Sub

    Private Sub DataMode()

        Dim Sql As String = ""
        Dim debtorDao As New CPM.DebtorDAO        

        Try

            divSearch.Visible = False
            divInv.Visible = True
            txtDebtorName.ReadOnly = True
            txtDebtorName.CssClass = CSSEnum.TXTFIELD_3_DISABLED
            ddLocation.Enabled = False
            ddLocation.CssClass = CSSEnum.DROPDOWN_DISABLED
            rbCompany.Enabled = False
            rbIndividual.Enabled = False



            Sql = "select dah.invoiceno as invoice1,CONVERT(VARCHAR(19),dah.invoicedate,103) + ' | ' + dah.invoiceno + ' | ' + " & _
               " dah.invoiceperiod + ' | ' + CONVERT(varchar(100),dah.amount) + ' | ' + CONVERT(varchar(100),dah.amount-isnull(dah.PaidAmount,0)) as invoiceno,'' as seq " & _
               " from debtoraccountheader dah where dah.invoicedate >dateadd(mm,-6,getdate()) and dah.status <> 'C'and dah.txntype = 'I'and dah.debtorid = " + gvDebtorEnq.SelectedDataKey(debtorDao.COLUMN_DebtorID).ToString & _
               " union all select codeabbr,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,invoiceno"

            dsInvoice.SelectCommand = Sql
            dsInvoice.DataBind()

            '''''''''''''''''''''''

            If Request.Params("TaxCode").Contains(ConstantGlobal.OutOfScope) Or Request.Params("TaxCode").Contains(ConstantGlobal.ZeroRated) Then
                Sql = "select '0' as SeasonTypeMstrId,CodeDesc as Description from codemstr where codecat = 'DEFAULT' " & _
                      " union select 9999,'DEPOSIT'order by SeasonTypeMstrId"
            ElseIf Request.Params("TaxCode").Contains(ConstantGlobal.StandardRated) Then
                Sql = "select '0' as SeasonTypeMstrId,CodeDesc as Description from codemstr where codecat = 'DEFAULT' " & _
                                      " union select SeasonTypeMstrId,SeasonTypeDesc from seasontypemstr where locationinfoid = " + ddLocation.SelectedValue & _
                                      " and Active = 'Y' order by SeasonTypeMstrId"
            End If

            dsDescription.SelectCommand = Sql
            dsDescription.DataBind()

            hidDebtorId.Value = gvDebtorEnq.SelectedDataKey(debtorDao.COLUMN_DebtorID).ToString
            txtDebtorName.Text = gvDebtorEnq.SelectedDataKey("DEBTOR").ToString
            hidLocationInfoId.Value = gvDebtorEnq.SelectedDataKey(debtorDao.COLUMN_LocationInfoId).ToString
            'DataMode()


        Catch ex As Exception
            logger.Error(ex.Message)
            lblmsg.Text = ex.Message
        Finally
            debtorDao = Nothing
        End Try


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

    Private Sub clearInvoice()

        lblmsg.Text = ""
        txtAmount.Text = 0
        txtQty.Text = 0
        txtMonth.Text = 0
        ddDescription.SelectedIndex = 0
        gvMisc.DataSource = Nothing
        gvMisc.DataBind()
        btnConfirm.Visible = False
        ViewState("CurrentTable") = Nothing
        txtSubTotal.Text = ""
        txtSubTotal.Visible = False


    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt As DataTable = ViewState("CurrentTable")
        Dim invEnt As New CPM.InvoiceHistoryEntity
        Dim invDao As New CPM.InvoiceHistoryDAO
        Dim dahEnt As New CPM.DebtorAccountHeaderEntity
        Dim dahDao As New CPM.DebtorAccountHeaderDAO
        Dim dadEnt As New CPM.DebtorAccountDetailEntity
        Dim dadDao As New CPM.DebtorAccountDetailDAO
        Dim debtorCategory As String = ""
        Dim txtTotalAmount As Double = 0
        Dim txtTotalGSTAmount As Double = 0

        Try
            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If

            trans = cn.BeginTransaction

            If ddInvoice.SelectedIndex = 0 Then
                lblmsg.Text = "Invoice is a Required field."
                Exit Sub
            End If

            If String.IsNullOrEmpty(txtRemark.Text) Then
                lblmsg.Text = "Remark is a Required field."
                Exit Sub
            End If


            


            'rcpNo = dm.getReceiptNextRunningNo(ddLocation.SelectedValue, trans, cn)
            If rbCompany.Checked = True Then
                debtorCategory = CategoryEnum.COMPANY
            Else
                debtorCategory = CategoryEnum.INDIVIDUAL
            End If

            dahEnt.setDebtorId(hidDebtorId.Value)
            dahEnt.setReferenceOldInvoice(ddInvoice.SelectedValue)
            dahEnt.setInvoiceNo(dm.getDebitNoteNextRunningNo(hidLocationInfoId.Value, trans, cn, Request.Params("TaxCode")))
            dahEnt.setInvoiceDate(txtDebitNoteDate.Text)
            dahEnt.setInvoicePeriod("")
            dahEnt.setLastUpdatedBy(lp.getUserMstrId)
            dahEnt.setLastUpdatedDatetime(Now)
            dahEnt.setStatus(InvoiceStatusEnum.OUTSTANDING)
            dahEnt.setBatchNo("")

            dahEnt.setTxnType(TxnTypeEnum.DEBITNOTE)
            dahEnt.setMIRemark(Trim(txtRemark.Text))

            Dim dahId As Long = dahDao.insertDB(dahEnt, cn, trans)

            For Each row As DataRow In dt.Rows

                If row.Item("TAXCODE").Equals(ConstantGlobal.StandardRated) Then
                    'Get the GST value and apply to chargeble item gn Standard Rated(SR)
                    txtTotalGSTAmount += Val(row.Item("TOTAL")) * (dm.getCurrentTax() / 100)
                End If

                txtTotalAmount += Val(row.Item("TOTAL"))
                dadEnt.setDebtorAccountHeaderId(dahId)
                dadEnt.setMonths(row.Item("MONTH"))
                dadEnt.setDetails(row.Item("DESCRIPTION"))
                dadEnt.setUnitPrice(row.Item("AMOUNT"))
                dadEnt.setQuantity(row.Item("QTY"))
                dadEnt.setAmount(row.Item("TOTAL"))
                dadEnt.setTaxCode(row.Item("TAXCODE"))
                dadEnt.setLastUpdatedBy(lp.getUserMstrId)
                dadEnt.setLastUpdatedDatetime(Now)
                If Request.Params("TaxCode").Contains(ConstantGlobal.StandardRated) Then
                    dadEnt.setxRef(TxnTypeEnum.DEBITNOTESR)
                ElseIf Request.Params("TaxCode").Contains(ConstantGlobal.OutOfScope) Then
                    dadEnt.setxRef(TxnTypeEnum.DEBITNOTEOS)
                ElseIf Request.Params("TaxCode").Contains(ConstantGlobal.ZeroRated) Then
                    dadEnt.setxRef(TxnTypeEnum.DEBITNOTEZRL)
                End If
                dadDao.insertDB(dadEnt, cn, trans)
            Next row

            txtTotalAmount += txtTotalGSTAmount


            invEnt.setDebtorId(hidDebtorId.Value)
            invEnt.setDebtorAccountHeaderId(dahId)
            invEnt.setStatus(InvoiceStatusEnum.OUTSTANDING)
            invEnt.setMonth(txtDebitNoteDate.Text)
            invEnt.setAmount(txtTotalAmount)
            invEnt.setLastUpdatedBy(lp.getUserMstrId)
            invEnt.setLastUpdatedDatetime(Now)
            invDao.insertDB(invEnt, cn, trans)


            dahEnt.setDebtorAccountHeaderId(dahId)
            dahEnt.setAmount(txtTotalAmount)
            dahEnt.setLastUpdatedDatetime(Now)
            dahDao.updateDB(dahEnt, cn, trans)

            'To create a new record for GST Amount
            If txtTotalGSTAmount > 0 Then
                dadEnt.setDebtorAccountHeaderId(dahId)
                dadEnt.setMonths("")
                dadEnt.setDetails("GST Amount")
                dadEnt.setUnitPrice(0)
                dadEnt.setQuantity(0)
                dadEnt.setAmount(txtTotalGSTAmount)
                dadEnt.setTaxCode(ConstantGlobal.NotAvailable)
                dadEnt.setLastUpdatedBy(lp.getUserMstrId)
                dadEnt.setLastUpdatedDatetime(Now)
                dadEnt.setxRef(TxnTypeEnum.DEBITNOTESRGST)
                dadDao.insertDB(dadEnt, cn, trans)
            End If

            trans.Commit()

            hdInvoiceNo.Value = dahEnt.getInvoiceNo
            hdSubTotal.Value = txtSubTotal.Text
            hidDebtorAccountHeaderId.Value = dahId


            'Clear the screen
            ddLocation.SelectedIndex = 0
            ddLocation.SelectedValue = lp.getDefaultLocationInfoId
            lblmsg.Text = ""
            txtAmount.Text = 0
            txtMonth.Text = 0
            txtQty.Text = 0
            ddDescription.SelectedIndex = 0
            gvMisc.DataSource = Nothing
            gvMisc.DataBind()
            btnConfirm.Visible = False
            ViewState("CurrentTable") = Nothing
            txtSubTotal.Text = ""
            txtSubTotal.Visible = False

            PrintDebitNoteGST(dahEnt.getInvoiceNo)
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopUp", "checkToPopUpViewer();", True)


        Catch ex As Exception
            trans.Rollback()
            lblmsg.Text = ex.Message
            logger.Debug(ex.Message)
        Finally
            trans.Dispose()
            cn.Close()
            invEnt = Nothing
            dahEnt = Nothing
            dadDao = Nothing

        End Try

    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        If (hdInvoiceNo.Value <> "" And hidDebtorAccountHeaderId.Value <> "" And hdSubTotal.Value <> "") Then
            'PrintTaxInvoice(hdInvoiceNo.Value, Trim(txtDescription.Text))
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopUp", "checkToPopUpViewer();", True)
        End If
    End Sub

    Protected Sub gvMisc_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

        Dim dt As DataTable = ViewState("CurrentTable")

        dt.Rows(e.RowIndex).Delete()

        gvMisc.DataSource = dt
        gvMisc.DataBind()
        calcSubtotal()


    End Sub

    Protected Sub calcSubtotal()
        Dim dt As DataTable = ViewState("CurrentTable")
        Dim subTotal As Double

        For Each row As DataRow In dt.Rows
            subTotal += Val(row.Item("TOTAL"))
        Next row

        txtSubTotal.Text = String.Format("{0:n2}", subTotal)

    End Sub

    Private Sub PrintDebitNoteGST(ByVal invNo As String)
        Dim rptMgr As New ReportManager
        Dim mySql As String = ""
        Dim dt As New DataTable
        Dim companyName As String = ""
        Dim companyAddress As String = ""
        Dim tel As String = ""
        Dim fax As String = ""
        Dim companyNo As String = ""
        Dim searchModel As New CPM.DebtorAccountHeaderEntity
        Dim sqlmap As New SQLMap
        Dim dtPassBayNo As New DataTable


        Dim hqInfoDao As New CPM.HQInfoDAO
        Dim debtorDao As New CPM.DebtorDAO

        Try

            mySql = "SELECT COMPANYNAME,COMPANYNO,ADDRESS1,ADDRESS2,ADDRESS3,POSTCODE,TELEPHONE,FAX,(SELECT CODEDESC FROM CODEMSTR WHERE CODECAT='STA' AND CODEABBR= STATE) AS STATE FROM HQINFO"
            dt = dm.execTable(mySql)

            If dt.Rows.Count > 0 Then
                companyName = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_CompanyName) & " " & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_CompanyNo)
                companyAddress = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address1) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address2) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address3) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_PostCode) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_State)
                tel = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Telephone)
                fax = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Fax)
                companyNo = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_CompanyNo)
            End If


            searchModel.setInvoiceNo(invNo)

            If dt.Rows.Count > 0 Then

                rptMgr.setReportName("DebitNoteGST.Rpt")
                rptMgr.setParameterDiscrete("CompanyName", companyName)
                rptMgr.setParameterDiscrete("CompanyAddress", companyAddress)
                rptMgr.setParameterDiscrete("TelephoneNo", tel)
                rptMgr.setParameterDiscrete("Fax", fax)
                rptMgr.setParameterDiscrete("PrintedBy", lp.getUserLoginId)
                rptMgr.setParameterDiscrete("debtorid", hidDebtorId.Value)
                rptMgr.setParameterDiscrete("invoiceno", invNo)
                rptMgr.setParameterDiscrete("CompanyNo", companyNo)
                'rptMgr.setParameterDiscrete("MIRemark", MIRemark)

                rptMgr.Logon()

                hdPreview.Value = "1"
                'set reportManager to session
                Session("ReportManager") = rptMgr
                lblmsg.Text = ""

            End If



        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            hqInfoDao = Nothing
            debtorDao = Nothing
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


    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim sql As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow


        Try


            If Not Page.IsValid Then
                Exit Sub
            End If

            If Not Utility.Tools.NumericValidation(txtQty.Text) Then
                lblMsg.Text = "Please enter numeric value for Qty."
                Exit Sub
            End If

            If Not Utility.Tools.NumericValidation(txtAmount.Text) Then
                lblmsg.Text = "Please enter numeric value for Amount."
                Exit Sub
            End If

            If String.IsNullOrEmpty(txtQty.Text) Or Val(Trim(txtQty.Text)) = 0 Then
                lblmsg.Text = "Qty is a Required field."
                Exit Sub
            End If

            If String.IsNullOrEmpty(txtAmount.Text) Or Val(Trim(txtAmount.Text)) = 0 Then
                lblmsg.Text = "Amount is a Required field."
                Exit Sub
            End If

            If String.IsNullOrEmpty(txtMonth.Text) Or Val(Trim(txtMonth.Text)) = 0 Then
                lblmsg.Text = "Month(s) is a Required field."
                Exit Sub
            End If

            If Not Utility.Tools.NumericValidation(txtMonth.Text) Then
                lblmsg.Text = "Please enter numeric value for Month(s)."
                Exit Sub
            End If

         

            If ddDescription.SelectedIndex = 0 Then
                lblmsg.Text = "Description is a Required field."
                Exit Sub
            End If

            If String.IsNullOrEmpty(Request.Params("TaxCode")) Then
                lblmsg.Text = "Empty TaxCode is not Allowed. Please contact Administrator!"
                Exit Sub
            End If

            If ViewState("CurrentTable") Is Nothing Then
                dt.Columns.Add("QTY")
                dt.Columns.Add("AMOUNT")
                dt.Columns.Add("TOTAL")
                dt.Columns.Add("DESCRIPTION")
                dt.Columns.Add("TAXCODE")
                dt.Columns.Add("MONTH")

                dr = dt.NewRow
                dr("QTY") = Trim(txtQty.Text)
                dr("AMOUNT") = Trim(txtAmount.Text)                
                dr("TOTAL") = String.Format("{0:n2}", txtQty.Text * txtAmount.Text * txtMonth.Text)
                dr("DESCRIPTION") = Trim(ddDescription.SelectedItem.Text)
                dr("MONTH") = Trim(txtMonth.Text)

                Select Case Request.Params("TaxCode")
                    Case ConstantGlobal.OutOfScope
                        dr("TAXCODE") = ConstantGlobal.OutOfScope
                    Case ConstantGlobal.ZeroRated
                        dr("TAXCODE") = ConstantGlobal.ZeroRated
                    Case ConstantGlobal.StandardRated
                        dr("TAXCODE") = ConstantGlobal.StandardRated
                    Case Else
                        lblmsg.Text = "Empty TaxCode is not Allowed. Please contact Administrator!"
                        Exit Sub
                End Select

                dt.Rows.Add(dr)

                ViewState("CurrentTable") = dt
                gvMisc.DataSource = dt
                gvMisc.DataBind()
            Else
                dt = ViewState("CurrentTable")
                dr = dt.NewRow
                dr("QTY") = Trim(txtQty.Text)
                dr("AMOUNT") = Trim(txtAmount.Text)
                dr("TOTAL") = String.Format("{0:n2}", txtQty.Text * txtAmount.Text * txtMonth.Text)
                dr("DESCRIPTION") = Trim(ddDescription.SelectedItem.Text)
                dr("MONTH") = Trim(txtMonth.Text)

                Select Case Request.Params("TaxCode")
                    Case ConstantGlobal.OutOfScope
                        dr("TAXCODE") = ConstantGlobal.OutOfScope
                    Case ConstantGlobal.ZeroRated
                        dr("TAXCODE") = ConstantGlobal.ZeroRated
                    Case ConstantGlobal.StandardRated
                        dr("TAXCODE") = ConstantGlobal.StandardRated
                    Case Else
                        lblmsg.Text = "Empty TaxCode is not Allowed. Please contact Administrator!"
                        Exit Sub
                End Select

                dt.Rows.Add(dr)

                ViewState("CurrentTable") = dt
                gvMisc.DataSource = dt
                gvMisc.DataBind()

            End If

            If ViewState("CurrentTable") Is Nothing Then
                btnConfirm.Visible = False
                'txtSubTotal.Visible = False
            Else
                btnConfirm.Visible = True
                txtSubTotal.Visible = True
                calcSubtotal()
                clear()
            End If

        Catch ex As Exception
            trans.Rollback()
            lblmsg.Text = ex.Message
            logger.Debug(ex.Message)
        Finally
            'trans.Dispose()
            'cn.Close()
        End Try
    End Sub



    Protected Sub btnDataBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        SearchMode()
        clearInvoice()
    End Sub

End Class
