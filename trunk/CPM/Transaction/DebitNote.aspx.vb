Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports PopCalendarSpecialDay

Partial Class Transaction_DebitNote
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
        ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        txtDebtorName.Text = ""
        rbCompany.Checked = True
        rbIndividual.Checked = False
        txtDescription.Text = ""
        txtDebitAmount.Text = ""
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
        clearInvoice()
    End Sub

    Private Sub clearInvoice()
        txtDescription.Text = ""
        txtDebitAmount.Text = ""

    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim invEnt As New CPM.InvoiceHistoryEntity
        Dim invDao As New CPM.InvoiceHistoryDAO
        Dim dahEnt As New CPM.DebtorAccountHeaderEntity
        Dim dahDao As New CPM.DebtorAccountHeaderDAO
        Dim dadEnt As New CPM.DebtorAccountDetailEntity
        Dim dadDao As New CPM.DebtorAccountDetailDAO

        Try

            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If
            trans = cn.BeginTransaction

            If Trim(txtDebitAmount.Text) = "" Or Trim(Val(txtDebitAmount.Text)) = 0 Then
                lblmsg.Text = "Please enter debit amount."
                Exit Sub
            End If


            dahEnt.setDebtorId(hidDebtorId.Value)
            dahEnt.setInvoiceNo(dm.getDebitNoteNextRunningNo(hidLocationInfoId.Value, trans, cn))
            dahEnt.setInvoiceDate(Now.ToShortDateString)
            dahEnt.setInvoicePeriod(Trim(txtDescription.Text))
            dahEnt.setLastUpdatedBy(lp.getUserMstrId)
            dahEnt.setLastUpdatedDatetime(Now)
            dahEnt.setStatus(InvoiceStatusEnum.OUTSTANDING)
            dahEnt.setAmount(Val(txtDebitAmount.Text))
            dahEnt.setBatchNo("")
            dahEnt.setTxnType(TxnTypeEnum.DEBITNOTE)
            Dim dahId As Long = dahDao.insertDB(dahEnt, cn, trans)

            dadEnt.setDebtorAccountHeaderId(dahId)
            dadEnt.setMonths("")
            dadEnt.setDetails(Trim(txtDescription.Text))
            dadEnt.setUnitPrice(0)
            dadEnt.setQuantity(0)
            dadEnt.setAmount(Val(txtDebitAmount.Text))
            dadEnt.setLastUpdatedBy(lp.getUserMstrId)
            dadEnt.setLastUpdatedDatetime(Now)
            dadDao.insertDB(dadEnt, cn, trans)

            invEnt.setDebtorId(hidDebtorId.Value)
            invEnt.setDebtorAccountHeaderId(dahId)
            invEnt.setStatus(InvoiceStatusEnum.OUTSTANDING)
            invEnt.setMonth(txtDebitNoteDate.Text)
            invEnt.setAmount(Val(txtDebitAmount.Text))
            invEnt.setLastUpdatedBy(lp.getUserMstrId)
            invEnt.setLastUpdatedDatetime(Now)
            invDao.insertDB(invEnt, cn, trans)

            'Throw New Exception("Test")

            trans.Commit()


            PrintDebitNote(dahEnt.getInvoiceNo)
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopUp", "checkToPopUpViewer();", True)


            lblmsg.Text = ""

            clearInvoice()

        Catch ex As Exception
            trans.Rollback()
            lblmsg.Text = ex.Message
        Finally
            trans.Dispose()
            cn.Close()
            invEnt = Nothing
            invDao = Nothing
            dahEnt = Nothing
            dahDao = Nothing


        End Try
    End Sub

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
