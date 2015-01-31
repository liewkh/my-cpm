Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports PopCalendarSpecialDay

Partial Class Transaction_SeasonUpDowngrade
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim receiptNo As String = ""
    Dim genDeposit As Boolean = True


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
        'chkDeposit.Checked = False
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

        ClientScript.RegisterForEventValidation(lnkProcess.UniqueID)

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
        Dim sql As String

        Try

            divSearch.Visible = False
            divInv.Visible = True
            txtDebtorName.ReadOnly = True
            txtDebtorName.CssClass = CSSEnum.TXTFIELD_3_DISABLED
            ddLocation.Enabled = False
            ddLocation.CssClass = CSSEnum.DROPDOWN_DISABLED
            rbCompany.Enabled = False
            rbIndividual.Enabled = False

            sql = "select stm.seasontypemstrid,seasontypedesc,0 as seq " & _
                                 "from seasontypemstr stm,locationinfo li " & _
              "where(stm.locationinfoid = li.locationinfoid) " & _
              "and li.locationinfoid = " & hidLocationInfoId.Value & _
              "union all " & _
              "select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq"

            dsToSeasonType.SelectCommand = sql
            dsToSeasonType.DataBind()

            dsFromSeasonType.SelectCommand = sql
            dsFromSeasonType.DataBind()

            hdOldDeposit.Value = ""
            hdNewDeposit.Value = ""
            hdNewSeasonAmount.Value = ""
            hdOldSeasonAmount.Value = ""

        Catch ex As Exception
            logger.Error(ex.Message)

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

    Protected Sub btnDataBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        SearchMode()
        clearData()
    End Sub

    Private Sub clearData()
        ddItemType.SelectedIndex = 0
        ddNewItemType.SelectedIndex = 0
        txtRemark.Text = ""
        lblNewPassDeposit.Text = ""
        lblOldPassDeposit.Text = ""
        ddOldPass.SelectedIndex = -1
        ddNewPass.SelectedIndex = -1
        lblmsg.Text = ""
        ddToSeasonType.SelectedIndex = -1
        lblOldPassSeason.Text = ""
        ddFromSeasonType.SelectedIndex = -1
        'ddOldPass.DataSource = Nothing
        'ddNewPass.DataSource = Nothing
        'ddOldPass.DataBind()
        'dsNewPass.SelectCommand = ""
        'dsNewPass.DataBind()
        'ddNewPass.SelectedIndex = 0
        'ddOldPass.SelectedIndex = 0
        'bindPass()
        txtEffectiveFrom.Text = ""
        'chkDeposit.Checked = FALSE
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
        Dim invNo As String = ""


        Try

            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If
            trans = cn.BeginTransaction


            If Not Page.IsValid Then
                Exit Sub
            End If

            If ddFromSeasonType.SelectedValue = ddToSeasonType.SelectedValue Then
                lblmsg.Text = "No changes done. Same season selected."
                Exit Sub
            End If

            If Trim(txtEffectiveFrom.Text) = "" Then
                lblmsg.Text = "Effective From Date is a required field."
                Exit Sub
            End If

            If Trim(txtTransactionDate.Text) = "" Then
                lblmsg.Text = "Transaction Date is a required field."
                Exit Sub
            End If

            If ddOldPass.SelectedIndex = 0 Or ddOldPass.SelectedIndex = -1 Then
                lblmsg.Text = "Old Pass is a required field."
                Exit Sub
            End If

            If ddToSeasonType.SelectedIndex = 0 Then
                lblmsg.Text = "New Season Type is a required field."
                Exit Sub
            End If


            sql = "SELECT * FROM DEBTORPASSBAY WHERE DEBTORID = " & hidDebtorId.Value & _
                " AND STATUS = 'A' AND PASSCARDMSTRID = " & ddOldPass.SelectedValue
            dt = dm.execTableInTrans(sql, cn, trans)

            dpbId = dt.Rows(0).Item("DEBTORPASSBAYID")

            If Trim(txtEffectiveFrom.Text) <> "" Then
                If Mid(Trim(txtEffectiveFrom.Text), 1, 2) <> "01" Then
                    lblmsg.Text = "The Effective Date must start from 1'st day of the month"
                    Exit Sub
                End If
            End If


            If ddNewPass.SelectedIndex > 0 Then 'Indicate Change The Pass Card as well
               

                Dim chkFirtUseSql As String = ""

                pcEnt.setPassCardMstrId(ddNewPass.SelectedValue)
                pcEnt.setDeposit(hdNewDeposit.Value)
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
                pcEnt.setStatus(PassCardStatusEnum.AVAILABLE)
                pcEnt.setDeposit(0)
                pcEnt.setDebtorId(0)
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
                pchEnt.setDeposit(hdNewDeposit.Value)
                pchEnt.setStartDate(txtEffectiveFrom.Text)
                pchEnt.setLastUpdatedBy(lp.getUserMstrId)
                pchEnt.setLastUpdatedDatetime(Now)
                pchEnt.setLocationInfoId(hidLocationInfoId.Value)
                pchDao.insertDB(pchEnt, cn, trans)


                'changeSeason(ddNewPass.SelectedValue, dpbId, cn, trans)
                invNo = changeSeason(ddOldPass.SelectedValue, dpbId, cn, trans)

            Else 'Indicate just used back the old pass but change to another season
                invNo = changeSeason(ddOldPass.SelectedValue, dpbId, cn, trans)


            End If



            logSeasonHistory(cn, trans)


            trans.Commit()
            'trans.Rollback()

            If invNo.Length > 0 Then
                PrintTaxInvoice(invNo, Trim(txtRemark.Text))
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopUp", "checkToPopUpViewer();", True)
            End If


            'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopUp", "alertUpdated();", True)

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


    Private Function changeSeason(ByVal passid As String, ByVal dbpid As String, ByRef cn As SqlConnection, ByRef trans As SqlTransaction) As String

        Dim sql As String = ""
        Dim dt As New DataTable
        Dim dpbEnt As New CPM.DebtorPassBayEntity
        Dim dpbDao As New CPM.DebtorPassBayDAO
        Dim dpbId As String = ""
        Dim pcEnt As New CPM.PassCardMstrEntity
        Dim pcDao As New CPM.PassCardMstrDAO
        Dim depositFee As Double
        Dim invNo As String = ""

        Try


            sql = "select count(IH.InvoiceHistoryId) as CNT from InvoiceHistory IH,InvoiceHistoryDetail IHD,DebtorAccountHeader dah " & _
                   "where IH.DebtorAccountHeaderId =  dah.DebtorAccountHeaderId " & _
                   "and ihd.DebtorAccountHeaderId = dah.DebtorAccountHeaderId And IH.month >= '" & Now.Year & _
                    CDate(txtEffectiveFrom.Text).ToString("MM") & "01' " & _
                    " and IH.debtorid =" & hidDebtorId.Value & _
                    " and IHD.PassCardMstrId in (" & passid & ")"

            dt = dm.execTableInTrans(sql, cn, trans)

            If dt.Rows(0).Item("CNT") > 0 Then 'Invoice Generated
                Dim monthCharged As Integer = dt.Rows(0).Item("CNT")
                Dim parkingFee As Double

                Dim total As Double
                parkingFee = monthCharged * (Val(hdNewSeasonAmount.Value) - Val(hdOldSeasonAmount.Value))
                depositFee = Val(hdNewDeposit.Value) - Val(hdOldDeposit.Value)
                total = parkingFee '+ depositFee New Requirement remove deposit

                'Check box "Deposit Update Only". If user checked it, it will not process the "Debit Note" or "Credit Note" 
                'generation but all other logic and updating remained the same

                'Check with Vincent
                'If Not chkDeposit.Checked Then
                '    If total < 0 Then
                '        receiptNo = createCreditNote(Math.Abs(parkingFee), Math.Abs(depositFee), "Refund Parking Fee : RM " & Math.Abs(parkingFee).ToString & ", Deposit : RM " & Math.Abs(depositFee).ToString, cn, trans)
                '        'Else
                '        '   receiptNo = createDebitNote(Math.Abs(parkingFee), Math.Abs(depositFee), cn, trans)
                '    End If
                'End If
                'Check with Vincent

                If total > 0 Then 'upgrade
                    invNo = createTaxInvoice(Math.Abs(parkingFee), depositFee, cn, trans)
                ElseIf total < 0 Then
                    'downgrade
                    'pending

                End If


            Else ' No generated invoice

                depositFee = Val(hdNewDeposit.Value) - Val(hdOldDeposit.Value)

                'If Not chkDeposit.Checked Then

                '    If depositFee <> 0 Then '

                If depositFee > 0 Then
                    'receiptNo = createDebitNote(0, Math.Abs(depositFee), cn, trans)
                    MsgBox("Season Upgrade : Please generate invoice manually!")
                ElseIf depositFee < 0 Then
                    'receiptNo = createCreditNote(0, Math.Abs(depositFee), "Refund Deposit : RM " & Math.Abs(depositFee).ToString, cn, trans)
                    MsgBox("Season Downgrade : Please generate invoice manually!")
                End If

            End If

            'End If





            'If ddToSeasonType.SelectedIndex > 0 Then
            '    'Check Is Up/Down Grade
            '    Dim differentSeason As Long = 0
            '    Dim differentDeposit As Long = 0

            '    If Val(hdNewSeasonAmount.Value) > Val(hdOldSeasonAmount.Value) Then 'Upgrade
            '        dt = dm.execTableInTrans(sql, cn, trans)

            '        If dt.Rows(0).Item("CNT") > 0 Then 'if Generated Invoice, so have to create a debit note 
            '            Dim monthCharged As Integer = dt.Rows(0).Item("CNT")
            '            sql = "select sum(IH.Amount) as AMT from InvoiceHistory IH,InvoiceHistoryDetail IHD,DebtorAccountHeader dah " & _
            '                  "where IH.DebtorAccountHeaderId =  dah.DebtorAccountHeaderId " & _
            '                  "and ihd.DebtorAccountHeaderId = dah.DebtorAccountHeaderId And IH.month >= '" & Now.Year & _
            '                  CDate(txtEffectiveFrom.Text).ToString("MM") & "01' " & _
            '                  " and IH.debtorid =" & hidDebtorId.Value & _
            '                  " and IHD.PassCardMstrId in (" & passid & ")"
            '            dt = dm.execTableInTrans(sql, cn, trans)
            '            'Extra Season Amount to add on
            '            differentSeason = (Val(hdNewSeasonAmount.Value) * monthCharged) - dt.Rows(0).Item("AMT")
            '            'Extra Deposit For Card
            '            differentDeposit = (Val(hdNewDeposit.Value) - Val(hdOldDeposit.Value))
            '            genDeposit = False
            '            createDebitNote(differentSeason, differentDeposit, cn, trans)
            '        End If
            '    ElseIf Val(hdNewSeasonAmount.Value) < Val(hdOldSeasonAmount.Value) Then 'DownGrade
            '        dt = dm.execTableInTrans(sql, cn, trans)
            '        If dt.Rows(0).Item("CNT") > 0 Then 'if Generated Invoice, so have to create a credit note 
            '            Dim monthCharged As Integer = dt.Rows(0).Item("CNT")
            '            sql = "select sum(IH.Amount) as AMT from InvoiceHistory IH,InvoiceHistoryDetail IHD,DebtorAccountHeader dah " & _
            '                                         "where IH.DebtorAccountHeaderId =  dah.DebtorAccountHeaderId " & _
            '                                         "and ihd.DebtorAccountHeaderId = dah.DebtorAccountHeaderId And IH.month >= '" & Now.Year & _
            '                                         CDate(txtEffectiveFrom.Text).ToString("MM") & "01' " & _
            '                                         " and IH.debtorid =" & hidDebtorId.Value & _
            '                                         " and IHD.PassCardMstrId in (" & passid & ")"
            '            dt = dm.execTableInTrans(sql, cn, trans)
            '            'Season Amount to deduct
            '            differentSeason = dt.Rows(0).Item("AMT") - (Val(hdNewSeasonAmount.Value) * monthCharged)
            '            'Minus Deposit For Card
            '            differentDeposit = (Val(hdOldDeposit.Value) - Val(hdNewDeposit.Value))
            '            genDeposit = False
            '            createCreditNote(differentSeason, differentDeposit, "Refund Parking Fee : RM " & differentSeason + differentDeposit & ", Deposit : RM " & differentDeposit, cn, trans)
            '        End If
            '    End If

            'End If

            If ddNewPass.SelectedIndex > 0 Then
                dpbEnt.setPassCardMstrId(ddNewPass.SelectedValue)
                dpbEnt.setSerialNo(ddNewPass.SelectedItem.Text)
            End If

            If ddItemType.SelectedIndex > 0 Then
                dpbEnt.setTypes(ddItemType.SelectedValue)
            End If


            dpbEnt.setDebtorPassBayId(dbpid)
            dpbEnt.setSeasonTypeMstrId(ddToSeasonType.SelectedValue)
            dpbEnt.setLastUpdatedBy(lp.getUserMstrId)
            dpbEnt.setLastUpdatedDatetime(Now)
            dpbDao.updateDB(dpbEnt, cn, trans)





            If (hdOldDeposit.Value <> hdNewDeposit.Value) Then
                pcEnt.setPassCardMstrId(ddOldPass.SelectedValue)
                pcEnt.setDeposit(hdNewDeposit.Value)
                pcEnt.setLastUpdatedBy(lp.getUserMstrId)
                pcEnt.setLastUpdatedDatetime(Now)
                pcDao.updateDB(pcEnt, cn, trans)
            End If


            ''New code for the changes if season downgrade extra use to knock off invoices
            'If ddToSeasonType.SelectedIndex > 0 Then
            '    'Check Is Up/Down Grade
            '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "myFunction();", True)

            '    Dim differentSeason As Long = 0
            '    Dim differentDeposit As Long = 0

            '    sql = "select dah.debtoraccountheaderid,CONVERT(VARCHAR(19),dah.invoicedate,103) + ' | ' + dah.invoiceno + ' | ' + " & _
            '     " dah.invoiceperiod + ' | ' + CONVERT(varchar(100),dah.amount-isnull(dah.PaidAmount,0)) as invoiceno,'' as seq " & _
            '     " from debtoraccountheader dah where (dah.amount-isnull(dah.PaidAmount,0)) <> 0 and dah.status <> 'C' and dah.txntype = 'I'and dah.debtorid = " + hidDebtorId.Value & _
            '     " union all select codeabbr,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq"

            '    dsInvoice.SelectCommand = sql
            '    dsInvoice.DataBind()


            '    If Val(hdNewSeasonAmount.Value) > Val(hdOldSeasonAmount.Value) Then 'Upgrade



            '    ElseIf Val(hdNewSeasonAmount.Value) < Val(hdOldSeasonAmount.Value) Then 'DownGrade
            '        'longTextContent.Visible = True
            '        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "myFunction();", True)

            '    End If


            'End If

            Return invNo

        Catch ex As Exception
            logger.Error(ex.Message)
            lblmsg.Text = ex.Message
            Throw ex
        End Try

    End Function

    Private Function createCreditNote(ByVal amtChargeSeason As Double, ByVal amtChargeDeposit As Double, ByVal Desc As String, ByRef cn As SqlConnection, ByRef trans As SqlTransaction) As String
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
        Dim hidDebtorAccountHeaderId As String = ""
        Dim total As Long = amtChargeSeason + amtChargeDeposit

        Try



            payAmt = total
            Dim str As String = ""
            Dim sql As String = ""
            Dim dt As New DataTable

            Dim searchModel As New CPM.DebtorAccountHeaderEntity
            Dim sqlmap As New SQLMap

            searchModel.setDebtorId(hidDebtorId.Value)
            searchModel.setStatus(InvoiceStatusEnum.OUTSTANDING)

            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorInvoiceReceipt", searchModel)



            dt = dm.execTable(strSQL)

            If dt.Rows(0).Item("OSamount") > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    invAmt += dt.Rows(0).Item("OSamount")
                Next

            Else
                Throw New ApplicationException("Unable To Create Credit Note.")
            End If



            'Pay less ok but not extra per invoice base
            If payAmt > invAmt Then
                Throw New Exception("Credit Note Amount Amount cannot be more than Total Outstanding Amount!")
            End If

            'Split payment for which invoice if happen
            Dim splitHeaderId As String() = hidDebtorAccountHeaderId.Split(",")
            hidDebtorAccountHeaderId = Mid(hidDebtorAccountHeaderId.ToString, 2)

            'To store PaymentForWhichMonth Id + Amount in future for cancellation reference
            Dim strInvHistoryId As String = ""


            For z As Integer = 0 To dt.Rows.Count - 1
                If Not String.IsNullOrEmpty(dt.Rows(z).Item("OSAMOUNT").ToString) And payAmt > 0 Then
                    selectSql = "SELECT ISNULL(PAIDAMOUNT,0) AS PAIDAMOUNT FROM INVOICEHISTORY WHERE INVOICEHISTORYID = " & dt.Rows(z).Item(invHistDao.COLUMN_InvoiceHistoryID).ToString
                    dtInvHist = dm.execTableInTrans(selectSql, cn, trans)


                    invHistEnt.setInvoiceHistoryId(dt.Rows(z).Item(invHistDao.COLUMN_InvoiceHistoryID).ToString)
                    If payAmt < Val(dt.Rows(z).Item("OSAMOUNT").ToString) Then
                        invHistEnt.setPaidAmount(dtInvHist.Rows(0).Item(dahDao.COLUMN_PaidAmount) + payAmt)
                    Else
                        invHistEnt.setPaidAmount(dtInvHist.Rows(0).Item(dahDao.COLUMN_PaidAmount) + Val(dt.Rows(z).Item("OSAMOUNT").ToString))
                    End If



                    invHistEnt.setLastUpdatedBy(lp.getUserMstrId)
                    invHistEnt.setLastUpdatedDatetime(Now)
                    invHistDao.updateDB(invHistEnt, cn, trans)



                    payAmt -= Val(dt.Rows(z).Item("OSAMOUNT").ToString)

                    'Get the latest PaidAmount from header
                    selectSql = "SELECT ISNULL(PAIDAMOUNT,0) AS PAIDAMOUNT FROM DEBTORACCOUNTHEADER WHERE DEBTORACCOUNTHEADERID = " & dt.Rows(z).Item(invHistDao.COLUMN_DebtorAccountHeaderId).ToString
                    dtHeader = dm.execTableInTrans(selectSql, cn, trans)

                    dahEnt.setDebtorAccountHeaderId(dt.Rows(z).Item(invHistDao.COLUMN_DebtorAccountHeaderId).ToString)
                    If payAmt < 0 Then
                        dahEnt.setPaidAmount(dtHeader.Rows(0).Item(dahDao.COLUMN_PaidAmount) + invHistEnt.getPaidAmount)
                        strInvHistoryId = strInvHistoryId & invHistEnt.getInvoiceHistoryId & "-" & invHistEnt.getPaidAmount & "|"
                    Else
                        dahEnt.setPaidAmount(dtHeader.Rows(0).Item(dahDao.COLUMN_PaidAmount) + Val(dt.Rows(z).Item("OSAMOUNT").ToString))
                        strInvHistoryId = strInvHistoryId & invHistEnt.getInvoiceHistoryId & "-" & dt.Rows(z).Item("OSAMOUNT").ToString & "|"
                    End If

                    dahEnt.setLastUpdatedDatetime(Now)
                    dahDao.updateDB(dahEnt, cn, trans)

                    selectSql = "SELECT AMOUNT,ISNULL(PAIDAMOUNT,0) AS PAIDAMOUNT FROM DEBTORACCOUNTHEADER WHERE DEBTORACCOUNTHEADERID = " & dt.Rows(z).Item(invHistDao.COLUMN_DebtorAccountHeaderId).ToString
                    dtHeader = dm.execTableInTrans(selectSql, cn, trans)
                    If dtHeader.Rows(0).Item(dahDao.COLUMN_Amount).Equals(dtHeader.Rows(0).Item(dahDao.COLUMN_PaidAmount)) Then
                        updateSql = "UPDATE DEBTORACCOUNTHEADER SET STATUS = '" & InvoiceStatusEnum.PAID & "' WHERE DEBTORACCOUNTHEADERID = " & dt.Rows(z).Item(invHistDao.COLUMN_DebtorAccountHeaderId).ToString
                        dtHeader = dm.execTableInTrans(updateSql, cn, trans)
                        updateSql = "UPDATE INVOICEHISTORY SET STATUS = '" & InvoiceStatusEnum.PAID & "' WHERE DEBTORACCOUNTHEADERID = " & dt.Rows(z).Item(invHistDao.COLUMN_DebtorAccountHeaderId).ToString
                        dtHeader = dm.execTableInTrans(updateSql, cn, trans)
                    End If

                    paymentFor += dt.Rows(z).Item("MONTH").ToString & ","
                    hidDebtorAccountHeaderId = hidDebtorAccountHeaderId & "," & dt.Rows(z).Item("DEBTORACCOUNTHEADERID").ToString

                End If
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


            paymentFor = Mid(paymentFor, 1, paymentFor.Length)
            strInvHistoryId = Mid(strInvHistoryId, 1, strInvHistoryId.Length)

            dpEnt.setAmount(total)
            dpEnt.setLastUpdatedBy(lp.getUserMstrId)
            dpEnt.setDebtorId(hidDebtorId.Value)
            dpEnt.setInvoiceHistoryIdAndAmount(strInvHistoryId)
            dpEnt.setPaymentFor("")
            dpEnt.setDebtorAccountHeaderId(hidDebtorAccountHeaderId)
            dpEnt.setPaymentDate(txtTransactionDate.Text)
            dpEnt.setDescription(Desc)
            dpEnt.setTxnType(TxnTypeEnum.CREDITNOTE)
            dpEnt.setStatus(ReceiptStatusEnum._NEW)

            dpEnt.setReceiptNo(dm.getCRNoteNextRunningNo(hidLocationInfoId.Value, trans, cn))


            dpEnt.setLastUpdatedDatetime(Now)
            Dim dpId As Long = dpDao.insertDB(dpEnt, cn, trans)

            'Throw New Exception("Test")

            'trans.Commit()

            PrintReceipt(dpId, hidDebtorId.Value, dpEnt.getAmount)
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopUp", "checkToPopUpViewer();", True)


            lblmsg.Text = ""

            Return dpEnt.getReceiptNo

        Catch ex As Exception
            'Record not found
            Throw New Exception("Resulted in Credit Amount. Please generate Invoice and re-do this transaction again.")
        Finally
            dpEnt = Nothing
            dpDao = Nothing
            invHistEnt = Nothing
            invHistDao = Nothing
            dahEnt = Nothing
            dahDao = Nothing
            dtHeader = Nothing
            dtInvHist = Nothing

        End Try
    End Function

    Private Function createDebitNote(ByVal amtChargeSeason As Double, ByVal amtChargeDeposit As Double, ByRef cn As SqlConnection, ByRef trans As SqlTransaction) As String

        Dim invEnt As New CPM.InvoiceHistoryEntity
        Dim invDao As New CPM.InvoiceHistoryDAO
        Dim dahEnt As New CPM.DebtorAccountHeaderEntity
        Dim dahDao As New CPM.DebtorAccountHeaderDAO
        Dim dadEnt As New CPM.DebtorAccountDetailEntity
        Dim dadDao As New CPM.DebtorAccountDetailDAO

        Try

            Dim totalCharge As Double = amtChargeSeason + amtChargeDeposit

            dahEnt.setDebtorId(hidDebtorId.Value)
            dahEnt.setInvoiceNo(dm.getDebitNoteNextRunningNo(hidLocationInfoId.Value, trans, cn))
            dahEnt.setInvoiceDate(Now.ToShortDateString)
            dahEnt.setInvoicePeriod(Trim(txtRemark.Text))
            dahEnt.setLastUpdatedBy(lp.getUserMstrId)
            dahEnt.setLastUpdatedDatetime(Now)
            dahEnt.setStatus(InvoiceStatusEnum.OUTSTANDING)
            dahEnt.setAmount(totalCharge)
            dahEnt.setBatchNo("")
            dahEnt.setTxnType(TxnTypeEnum.DEBITNOTE)
            Dim dahId As Long = dahDao.insertDB(dahEnt, cn, trans)

            dadEnt.setDebtorAccountHeaderId(dahId)
            dadEnt.setMonths("")
            dadEnt.setDetails("Additional Season Parking Fee : RM " & amtChargeSeason & " ,Deposit : RM " & amtChargeDeposit)
            dadEnt.setUnitPrice(0)
            dadEnt.setQuantity(0)
            dadEnt.setAmount(totalCharge)
            dadEnt.setLastUpdatedBy(lp.getUserMstrId)
            dadEnt.setLastUpdatedDatetime(Now)
            dadDao.insertDB(dadEnt, cn, trans)

            invEnt.setDebtorId(hidDebtorId.Value)
            invEnt.setDebtorAccountHeaderId(dahId)
            invEnt.setStatus(InvoiceStatusEnum.OUTSTANDING)
            invEnt.setMonth(txtTransactionDate.Text)
            invEnt.setAmount(totalCharge)
            invEnt.setLastUpdatedBy(lp.getUserMstrId)
            invEnt.setLastUpdatedDatetime(Now)
            invDao.insertDB(invEnt, cn, trans)

            'Throw New Exception("Test")

            PrintDebitNote(dahEnt.getInvoiceNo)
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopUp", "checkToPopUpViewer();", True)


            lblmsg.Text = ""

            clear()

            Return dahEnt.getInvoiceNo

        Catch ex As Exception
            lblmsg.Text = ex.Message
            logger.Error(ex.Message)
        Finally
            invEnt = Nothing
            invDao = Nothing
            dahEnt = Nothing
            dahDao = Nothing


        End Try


    End Function

    Private Sub PrintDebitNote(ByVal invNo As String)
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

                rptMgr.setReportName("DebitNote.Rpt")
                rptMgr.setParameterDiscrete("CompanyName", companyName)
                rptMgr.setParameterDiscrete("CompanyAddress", companyAddress)
                rptMgr.setParameterDiscrete("TelephoneNo", tel)
                rptMgr.setParameterDiscrete("Fax", fax)
                rptMgr.setParameterDiscrete("PrintedBy", lp.getUserLoginId)
                rptMgr.setParameterDiscrete("debtorid", hidDebtorId.Value)
                rptMgr.setParameterDiscrete("invoiceno", invNo)
                rptMgr.setParameterDiscrete("CompanyNo", companyNo)

                rptMgr.Logon()

                hdPreview.Value = "1"
                'set reportManager to session
                Session("ReportManager") = rptMgr
                lblmsg.Text = ""

            End If



        Catch ex As Exception
            Throw ex

        Finally
            hqInfoDao = Nothing
            debtorDao = Nothing
            dt = Nothing
            rptMgr = Nothing


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

    Protected Sub ddItemType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        bindPass()
        lblOldPassDeposit.Text = ""
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

            'txtDeposit.Text = ""

        Catch ex As Exception
            logger.Error(ex.Message)
        End Try
    End Sub

    Protected Sub ddOldPass_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt As New DataTable
        Dim sql As String = ""

        Try

            If ddOldPass.SelectedIndex > 0 Then

                sql = "SELECT P.DEPOSIT,D.SEASONTYPEMSTRID FROM PASSCARDMSTR P,DEBTORPASSBAY D WHERE P.PASSCARDMSTRID = " & ddOldPass.SelectedValue & _
                      " AND D.PASSCARDMSTRID = P.PASSCARDMSTRID AND D.STATUS = 'A' AND D.DEBTORID = " & hidDebtorId.Value
                dt = dm.execTable(sql)

                If dt.Rows.Count = 0 Then
                    lblOldPassDeposit.Text = ""
                    hdOldDeposit.Value = ""
                Else
                    lblOldPassDeposit.Text = "Deposit - RM " & dt.Rows(0).Item("DEPOSIT").ToString
                    hdOldDeposit.Value = dt.Rows(0).Item("DEPOSIT")
                End If

                getFromSeasonAmount(dt.Rows(0).Item("SEASONTYPEMSTRID"))
                ddFromSeasonType.SelectedValue = dt.Rows(0).Item("SEASONTYPEMSTRID")
            Else
                lblOldPassDeposit.Text = ""

            End If

        Catch ex As Exception
            logger.Debug(ex.Message)
            lblmsg.Text = ex.Message

        Finally
            dt = Nothing
        End Try
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
            'dahEnt.setDebtorId(hidDebtorId.Value)
            'dahEnt.setInvoiceNo(dm.getDebitNoteNextRunningNo(hidLocationInfoId.Value, trans, cn))
            'dahEnt.setInvoiceDate(Now)
            'dahEnt.setInvoicePeriod("")
            'dahEnt.setLastUpdatedBy(lp.getUserMstrId)
            'dahEnt.setLastUpdatedDatetime(Now)
            'dahEnt.setStatus(InvoiceStatusEnum.PAID)
            'dahEnt.setAmount(Val(txtDeposit.Text))
            'dahEnt.setBatchNo("")
            'dahEnt.setTxnType(TxnTypeEnum.DEBITNOTE)
            'Dim dahId As Long = dahDao.insertDB(dahEnt, cn, trans)

            'dadEnt.setDebtorAccountHeaderId(dahId)
            'dadEnt.setMonths("")
            'dadEnt.setDetails(ddReason.SelectedItem.Text)
            'dadEnt.setUnitPrice(0)
            'dadEnt.setQuantity(0)
            'dadEnt.setAmount(Val(txtDeposit.Text))
            'dadEnt.setLastUpdatedBy(lp.getUserMstrId)
            'dadEnt.setLastUpdatedDatetime(Now)
            'dadDao.insertDB(dadEnt, cn, trans)

            'invEnt.setDebtorId(hidDebtorId.Value)
            'invEnt.setDebtorAccountHeaderId(dahId)
            'invEnt.setStatus(InvoiceStatusEnum.PAID)
            'invEnt.setMonth(txtTransactionDate.Text)
            'invEnt.setAmount(Val(txtDeposit.Text))
            'invEnt.setLastUpdatedBy(lp.getUserMstrId)
            'invEnt.setLastUpdatedDatetime(Now)
            'Dim invHistId As Long = invDao.insertDB(invEnt, cn, trans)

            'dpEnt.setAmount(Val(txtDeposit.Text))
            'dpEnt.setDebtorAccountHeaderId(dahId)
            'dpEnt.setDebtorId(hidDebtorId.Value)
            'dpEnt.setDescription("Payment For Pass Card Deposit")
            'dpEnt.setLastUpdatedBy(lp.getUserMstrId)
            'dpEnt.setLastUpdatedDatetime(Now)
            'dpEnt.setPaymentType(ddPaymentType.SelectedValue)
            'dpEnt.setRefNo(txtNo.Text)
            'dpEnt.setPaymentDate(txtTransactionDate.Text)
            'dpEnt.setTxnType(TxnTypeEnum.RECEIPT)
            'dpEnt.setStatus(ReceiptStatusEnum._NEW)
            'dpEnt.setReceiptNo(dm.getReceiptNextRunningNo(hidLocationInfoId.Value, trans, cn))
            'dpEnt.setInvoiceHistoryIdAndAmount(invHistId & "|" & Val(txtDeposit.Text))
            'Dim dpId As Long = dpDao.insertDB(dpEnt, cn, trans)

            'receiptNo = dpEnt.getReceiptNo

            'Return dpId

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

    Public Sub getFromSeasonAmount(ByVal seasonTypeMstrId As String)
        Dim searchModel As New SeasonTypeSearchModel
        Dim strSQL As String = ""
        Dim dt As DataTable

        Try
            Dim sqlmap As New SQLMap

            'lblOldPassDeposit.Text = ""

            searchModel.setSeasonTypeMstrId(seasonTypeMstrId)

            strSQL = sqlmap.getMappedStatement("SetupMstr/Search-SeasonTypeMstr", searchModel)


            dt = dm.execTable(strSQL)
            If dt.Rows.Count = 1 Then                
                'lblOldPassSeason.Text = " " & dt.Rows(0).Item("SEASONTYPEDESC").ToString
                hdOldSeasonAmount.Value = dt.Rows(0).Item("AMOUNT").ToString
            Else            
                hdOldSeasonAmount.Value = ""
            End If

            strSQL = ""
            dt.Dispose()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ddNewItemType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim sql As String

            sql = "select passcardmstrid,SERIALNO,0 as seq from passcardmstr " & _
                 " where status = '" & PassCardStatusEnum.AVAILABLE & "'" & _
                 " and itemtype = '" & ddNewItemType.SelectedValue & "'" & _
                 " and locationinfoid = " & hidLocationInfoId.Value & _
                 " union all " & _
                 " select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' " & _
                 " order by seq,serialno"

            dsNewPass.SelectCommand = sql
            dsNewPass.DataBind()

        Catch ex As Exception
            logger.Error(ex.Message)
        End Try
    End Sub

    Protected Sub ddToSeasonType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt As New DataTable
        Dim sql As String = ""

        Try
            sql = "SELECT DEPOSIT,AMOUNT FROM SEASONTYPEMSTR WHERE SEASONTYPEMSTRID = " & ddToSeasonType.SelectedValue
            dt = dm.execTable(sql)

            If dt.Rows.Count = 0 Then
                lblNewPassDeposit.Text = ""
                hdNewDeposit.Value = ""
                hdNewSeasonAmount.value = ""
            Else
                lblNewPassDeposit.Text = "Deposit - RM " & dt.Rows(0).Item("DEPOSIT").ToString
                hdNewDeposit.Value = dt.Rows(0).Item("DEPOSIT").ToString
                hdNewSeasonAmount.value = dt.Rows(0).Item("AMOUNT").ToString
            End If


        Catch ex As Exception
            logger.Debug(ex.Message)
            lblmsg.Text = ex.Message

        Finally
            dt = Nothing

        End Try
    End Sub

    Private Sub logSeasonHistory(ByRef cn As SqlConnection, ByRef trans As SqlTransaction)

        Dim shEnt As New CPM.SeasonHistoryEntity
        Dim shDao As New CPM.SeasonHistoryDAO
        
        Try

            shEnt.setDebtorId(hidDebtorId.Value)
            shEnt.setFromSeasonTypeMstrId(ddFromSeasonType.SelectedValue)
            shEnt.setFromPassCardMstrId(ddOldPass.SelectedValue)
            If ddNewPass.SelectedIndex > 0 Then
                shEnt.setToPassCardMstrId(ddNewPass.SelectedValue)
            End If
            shEnt.setTransactionDate(txtTransactionDate.Text)
            shEnt.setToSeasonTypeMstrId(ddToSeasonType.SelectedValue)
            shEnt.setRemark(Trim(txtRemark.Text))
            shEnt.setLastUpdatedBy(lp.getUserMstrId)
            shEnt.setLastUpdatedDatetime(Now)
            shEnt.setProcessedBy(lp.getUserMstrId)
            shEnt.setDocumentNo(receiptNo)
            shDao.insertDB(shEnt, cn, trans)
            receiptNo = ""
        Catch ex As Exception
            logger.Error(ex.Message)
            lblmsg.Text = ex.Message
            Throw ex
        End Try

    End Sub

    Private Function createTaxInvoice(ByVal amtChargeSeason As Double, ByVal amtChargeDeposit As Double, ByRef cn As SqlConnection, ByRef trans As SqlTransaction) As String

        Dim invEnt As New CPM.InvoiceHistoryEntity
        Dim invDao As New CPM.InvoiceHistoryDAO
        Dim dahEnt As New CPM.DebtorAccountHeaderEntity
        Dim dahDao As New CPM.DebtorAccountHeaderDAO
        Dim dadEnt As New CPM.DebtorAccountDetailEntity
        Dim dadDao As New CPM.DebtorAccountDetailDAO
        Dim inv As String = ""

        Try

            Dim totalCharge As Double = amtChargeSeason + amtChargeDeposit
            Dim totalChargeWithGst As Double = totalCharge + (totalCharge * (dm.getCurrentTax() / 100))

            inv = dm.getNextRunningNo(dm.getDebtorCategory(hidDebtorId.Value), hidLocationInfoId.Value, trans, cn)

            'Header
            dahEnt.setDebtorId(hidDebtorId.Value)
            dahEnt.setInvoiceNo(inv)
            dahEnt.setInvoiceDate(Now.ToShortDateString)
            dahEnt.setInvoicePeriod(Trim(txtRemark.Text))
            dahEnt.setLastUpdatedBy(lp.getUserMstrId)
            dahEnt.setLastUpdatedDatetime(Now)
            dahEnt.setStatus(InvoiceStatusEnum.OUTSTANDING)
            dahEnt.setAmount(totalCharge)
            dahEnt.setBatchNo("")
            dahEnt.setTxnType(TxnTypeEnum.INVOICE)
            Dim dahId As Long = dahDao.insertDB(dahEnt, cn, trans)

            'Season Charges
            dadEnt.setDebtorAccountHeaderId(dahId)
            dadEnt.setMonths("")
            dadEnt.setDetails("Additional Season Parking Fee : RM " & amtChargeSeason)
            dadEnt.setUnitPrice(0)
            dadEnt.setQuantity(0)
            dadEnt.setAmount(amtChargeSeason)
            dadEnt.setTaxCode("SR")
            dadEnt.setxRef(TxnTypeEnum.INVOICEENTRYSEASON)
            dadEnt.setLastUpdatedBy(lp.getUserMstrId)
            dadEnt.setLastUpdatedDatetime(Now)
            dadDao.insertDB(dadEnt, cn, trans)

            'Deposit Charges
            dadEnt.setDebtorAccountHeaderId(dahId)
            dadEnt.setMonths("")
            dadEnt.setDetails("Deposit : RM " & amtChargeDeposit)
            dadEnt.setUnitPrice(0)
            dadEnt.setQuantity(0)
            dadEnt.setAmount(amtChargeDeposit)
            dadEnt.setTaxCode("ZR")
            dadEnt.setxRef(TxnTypeEnum.INVOICEENTRYSEASONDEPOSIT)
            dadEnt.setLastUpdatedBy(lp.getUserMstrId)
            dadEnt.setLastUpdatedDatetime(Now)
            dadDao.insertDB(dadEnt, cn, trans)



            'Tax Charges
            dadEnt.setDebtorAccountHeaderId(dahId)
            dadEnt.setMonths("")
            dadEnt.setDetails("GST Amount")
            dadEnt.setUnitPrice(0)
            dadEnt.setQuantity(0)
            dadEnt.setAmount(amtChargeSeason * (dm.getCurrentTax() / 100))
            dadEnt.setTaxCode("NA")
            dadEnt.setxRef(TxnTypeEnum.INVOICEENTRYGST)
            dadEnt.setLastUpdatedBy(lp.getUserMstrId)
            dadEnt.setLastUpdatedDatetime(Now)
            dadDao.insertDB(dadEnt, cn, trans)

            invEnt.setDebtorId(hidDebtorId.Value)
            invEnt.setDebtorAccountHeaderId(dahId)
            invEnt.setStatus(InvoiceStatusEnum.OUTSTANDING)
            invEnt.setMonth(txtTransactionDate.Text)
            invEnt.setAmount(totalChargeWithGst)
            invEnt.setLastUpdatedBy(lp.getUserMstrId)
            invEnt.setLastUpdatedDatetime(Now)            
            invDao.insertDB(invEnt, cn, trans)


            lblmsg.Text = ""

            clear()

            Return inv

        Catch ex As Exception
            lblmsg.Text = ex.Message
            logger.Error(ex.Message)
        Finally
            invEnt = Nothing
            invDao = Nothing
            dahEnt = Nothing
            dahDao = Nothing


        End Try


    End Function

    Private Sub PrintTaxInvoice(ByVal invNo As String, ByVal MIRemark As String)
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

                rptMgr.setReportName("Invoice.Rpt")
                rptMgr.setParameterDiscrete("CompanyName", companyName)
                rptMgr.setParameterDiscrete("CompanyAddress", companyAddress)
                rptMgr.setParameterDiscrete("TelephoneNo", tel)
                rptMgr.setParameterDiscrete("Fax", fax)
                rptMgr.setParameterDiscrete("PrintedBy", lp.getUserLoginId)
                rptMgr.setParameterDiscrete("debtorid", hidDebtorId.Value)
                rptMgr.setParameterDiscrete("invoiceno", invNo)
                rptMgr.setParameterDiscrete("CompanyNo", companyNo)                

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

    Protected Sub lnkProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim selectSql As String
        Dim dtInvHist As New DataTable
        Dim dtHeader As New DataTable
        Dim invHistEnt As New CPM.InvoiceHistoryEntity
        Dim invHistDao As New CPM.InvoiceHistoryDAO
        Dim dahEnt As New CPM.DebtorAccountHeaderEntity
        Dim dahDao As New CPM.DebtorAccountHeaderDAO
        Dim dpEnt As New CPM.DebtorPaymentEntity
        Dim dpDao As New CPM.DebtorPaymentDAO
        Dim payAmt, totalActualPay As Double 'Extra money from downgrade to knock off invoice
        Dim chkMoreThan1 As Integer = 0
        Dim updateSql As String = ""
        Dim strInvHistoryId As String = ""

        Try

            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If
            trans = cn.BeginTransaction

            'MsgBox(hidInvoice.Value)

            'Single Invoice Code *******            
            selectSql = "SELECT ISNULL(PAIDAMOUNT,0) AS PAIDAMOUNT,INVOICEHISTORYID,AMOUNT FROM INVOICEHISTORY WHERE DEBTORACCOUNTHEADERID = " + hidInvoice.Value & _
            " and (amount-isnull(PaidAmount,0)) <> 0"
            dtInvHist = dm.execTableInTrans(selectSql, cn, trans)

            'For loop to credit off the txn
            If dtInvHist.Rows.Count > 0 Then
                For Each rec As DataRow In dtInvHist.Rows
                    Dim recPaidAmount As Double = rec.Item("PAIDAMOUNT")
                    Dim recOSAmount As Double = Double.Parse(rec.Item("AMOUNT")) - recPaidAmount
                    invHistEnt.setInvoiceHistoryId(rec.Item("INVOICEHISTORYID"))

                    If payAmt <= recOSAmount Then
                        invHistEnt.setPaidAmount(recPaidAmount + payAmt)
                    Else
                        invHistEnt.setPaidAmount(recPaidAmount + Double.Parse(rec.Item("AMOUNT")))
                    End If

                    invHistEnt.setLastUpdatedBy(lp.getUserMstrId)
                    invHistEnt.setLastUpdatedDatetime(Now)
                    invHistDao.updateDB(invHistEnt, cn, trans)

                    payAmt -= recOSAmount

                    If payAmt > 0 Then
                        totalActualPay += recOSAmount
                    Else
                        totalActualPay += recOSAmount + payAmt
                    End If


                    'Get the latest PaidAmount from header
                    selectSql = "SELECT ISNULL(PAIDAMOUNT,0) AS PAIDAMOUNT FROM DEBTORACCOUNTHEADER WHERE DEBTORACCOUNTHEADERID = " & hidInvoice.Value
                    dtHeader = dm.execTableInTrans(selectSql, cn, trans)

                    dahEnt.setDebtorAccountHeaderId(hidInvoice.Value)
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
                        dahEnt.setPaidAmount(dtHeader.Rows(0).Item(dahDao.COLUMN_PaidAmount) + Val(totalActualPay))
                        strInvHistoryId = strInvHistoryId & invHistEnt.getInvoiceHistoryId & "-" & totalActualPay.ToString & "|"
                    End If

                    dahEnt.setLastUpdatedDatetime(Now)
                    dahDao.updateDB(dahEnt, cn, trans)

                    selectSql = "SELECT AMOUNT,ISNULL(PAIDAMOUNT,0) AS PAIDAMOUNT FROM DEBTORACCOUNTHEADER WHERE DEBTORACCOUNTHEADERID = " & hidInvoice.Value
                    dtHeader = dm.execTableInTrans(selectSql, cn, trans)
                    If dtHeader.Rows(0).Item(dahDao.COLUMN_Amount).Equals(dtHeader.Rows(0).Item(dahDao.COLUMN_PaidAmount)) Then
                        updateSql = "UPDATE DEBTORACCOUNTHEADER SET STATUS = '" & InvoiceStatusEnum.PAID & "' WHERE DEBTORACCOUNTHEADERID = " & hidInvoice.Value
                        dtHeader = dm.execTableInTrans(updateSql, cn, trans)
                        updateSql = "UPDATE INVOICEHISTORY SET STATUS = '" & InvoiceStatusEnum.PAID & "' WHERE DEBTORACCOUNTHEADERID = " & hidInvoice.Value
                        dtHeader = dm.execTableInTrans(updateSql, cn, trans)
                    End If

                    If payAmt < 0 Then
                        Exit For
                    End If


                Next rec
            End If

            dpEnt.setAmount(payAmt)
            dpEnt.setLastUpdatedBy(lp.getUserMstrId)
            dpEnt.setDebtorId(hidDebtorId.Value)
            dpEnt.setInvoiceHistoryIdAndAmount(strInvHistoryId)
            dpEnt.setPaymentFor("Knock Off from")
            dpEnt.setDebtorAccountHeaderId(hidInvoice.Value)
            dpEnt.setPaymentDate(DateTime.Now)
            dpEnt.setDescription("")            
            dpEnt.setTxnType("")
            dpEnt.setStatus(ReceiptStatusEnum._NEW)
            dpEnt.setGSTAmount("")
            'dpEnt.setInvoiceNo(invoiceNo)
            'dpEnt.setInvoiceDate(CDate(invoiceDate))

            dpEnt.setReceiptNo(dm.getCRNoteNextRunningNo(hidLocationInfoId.Value, trans, cn))


            dpEnt.setLastUpdatedDatetime(Now)
            Dim dpId As Long = dpDao.insertDB(dpEnt, cn, trans)

            trans.Commit()

        Catch ex As Exception
            trans.Rollback()
            lblmsg.Text = ex.Message
        Finally
            trans.Dispose()
            cn.Close()
            dtInvHist = Nothing
            invHistEnt = Nothing
            invHistDao = Nothing
            dahEnt = Nothing
            dahDao = Nothing
            dpEnt = Nothing
            dpDao = Nothing


        End Try
    End Sub
End Class
