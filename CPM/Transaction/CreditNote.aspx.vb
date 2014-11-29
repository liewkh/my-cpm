Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports PopCalendarSpecialDay

Partial Class Transaction_CreditNote
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

                txtPaymentDate.Text = Utility.DataTypeUtils.formatDateString(Now)

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
        txtTotalOS.Text = ""
        txtSelected.text = ""
        txtDescription.Text = ""
        txtPaymentAmount.Text = ""
        gvDebtorEnq.DataSource = Nothing
        gvDebtorInv.DataSource = Nothing
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
        ElseIf Not e.CommandName.Equals("Select") Then
            SearchData()        
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

    Protected Sub gvDebtorInv_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDebtorInv.SelectedIndexChanged
        Dim total As Double = 0


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
            txtselected.text = String.Format("{0:n2}", total)

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

        Try

            lblmsg.Text = ""

            searchModel.setDebtorId(hidDebtorId.Value)
            searchModel.setStatus(InvoiceStatusEnum.OUTSTANDING)

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


        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            searchModel = Nothing
            sqlmap = Nothing

        End Try
    End Sub


    Protected Sub btnDataBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        SearchMode()
        clearInvoice()
    End Sub

    Private Sub clearInvoice()
        txtDescription.Text = ""
        txtPaymentAmount.Text = ""
        txtSelected.Text = ""
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
        Dim totalActualPay As Double = 0
        Dim chkMoreThan1 As Integer = 0

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
                        chkMoreThan1 += 1
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
                                invHistEnt.setPaidAmount(dtInvHist.Rows(0).Item(dahDao.COLUMN_PaidAmount) + payAmt)
                            Else
                                invHistEnt.setPaidAmount(dtInvHist.Rows(0).Item(dahDao.COLUMN_PaidAmount) + Val(gvDebtorInv.DataKeys(row.RowIndex)("OSAMOUNT").ToString))
                            End If



                            invHistEnt.setLastUpdatedBy(lp.getUserMstrId)
                            invHistEnt.setLastUpdatedDatetime(Now)
                            invHistDao.updateDB(invHistEnt, cn, trans)



                            payAmt -= Val(gvDebtorInv.DataKeys(row.RowIndex)("OSAMOUNT").ToString)

                            If payAmt > 0 Then
                                totalActualPay += Val(gvDebtorInv.DataKeys(row.RowIndex)("OSAMOUNT").ToString)
                            Else
                                totalActualPay += Val(gvDebtorInv.DataKeys(row.RowIndex)("OSAMOUNT").ToString) + payAmt
                            End If

                            'Get the latest PaidAmount from header
                            selectSql = "SELECT ISNULL(PAIDAMOUNT,0) AS PAIDAMOUNT FROM DEBTORACCOUNTHEADER WHERE DEBTORACCOUNTHEADERID = " & gvDebtorInv.DataKeys(row.RowIndex)(invHistDao.COLUMN_DebtorAccountHeaderId).ToString
                            dtHeader = dm.execTableInTrans(selectSql, cn, trans)

                            dahEnt.setDebtorAccountHeaderId(gvDebtorInv.DataKeys(row.RowIndex)(invHistDao.COLUMN_DebtorAccountHeaderId).ToString)
                            If payAmt < 0 Then
                                'Issue reported double charged
                                'dahEnt.setPaidAmount(dtHeader.Rows(0).Item(dahDao.COLUMN_PaidAmount) + invHistEnt.getPaidAmount)
                                If chkMoreThan1 > 1 Then
                                    dahEnt.setPaidAmount(dtHeader.Rows(0).Item(dahDao.COLUMN_PaidAmount) + invHistEnt.getPaidAmount)
                                    strInvHistoryId = strInvHistoryId & invHistEnt.getInvoiceHistoryId & "-" & CStr(invHistEnt.getPaidAmount) & "|"
                                Else
                                    dahEnt.setPaidAmount(dtHeader.Rows(0).Item(dahDao.COLUMN_PaidAmount) + totalActualPay)
                                    strInvHistoryId = strInvHistoryId & invHistEnt.getInvoiceHistoryId & "-" & totalActualPay & "|"
                                End If
                                'strInvHistoryId = strInvHistoryId & invHistEnt.getInvoiceHistoryId & "-" & totalActualPay & "|"
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
            dpEnt.setInvoiceHistoryIdAndAmount(strInvHistoryId)
            dpEnt.setPaymentFor(paymentFor)
            dpEnt.setDebtorAccountHeaderId(hidDebtorAccountHeaderId.Value)
            dpEnt.setPaymentDate(txtPaymentDate.Text)
            dpEnt.setDescription(Trim(txtDescription.Text))
            'dpEnt.setPaymentType(PaymentTypeEnum.CREDITNOTE)
            dpEnt.setTxnType(TxnTypeEnum.CREDITNOTE)
            dpEnt.setStatus(ReceiptStatusEnum._NEW)

            dpEnt.setReceiptNo(dm.getCRNoteNextRunningNo(hidLocationInfoId.Value, trans, cn))


            dpEnt.setLastUpdatedDatetime(Now)
            Dim dpId As Long = dpDao.insertDB(dpEnt, cn, trans)

            'Throw New Exception("Test")

            trans.Commit()
            'trans.Rollback()

            SearchData()

            PrintReceipt(dpId, hidDebtorId.Value, dpEnt.getAmount)
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopUp", "checkToPopUpViewer();", True)


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

            rptMgr.setReportName("CreditNote.Rpt")
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
End Class
