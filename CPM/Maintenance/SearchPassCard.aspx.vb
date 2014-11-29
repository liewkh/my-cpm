Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports PopCalendarSpecialDay

Partial Class Maintenance_SearchPassCard
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Dim paramDebtorId As String
    Dim paramSeasonTypeMstrId As String
    Dim paramBranchInfoId As String
    Dim accumulatedDeposit As Double = 0
    Dim monthlyAmount As Double = 0
    Dim deposit As Double = 0

    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        lp = Session("LoginProfile")
        If IsNothing(lp) Then
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("appPath") + "/login.aspx?login=expired")
        End If

        paramDebtorId = Request.Params("debtorId")
        paramSeasonTypeMstrId = Request.Params("seasonTypeMstrId")
        paramBranchInfoId = Request.Params("branchInfoId")

        ddLocation.SelectedValue = paramBranchInfoId



        If Not Page.IsPostBack Then
            bindData()
            txtDeposit.Text = ""
            txtMonthlyRate.Text = ""
            txtInitialPayment.Text = ""
            txtTotal.Text = ""
            Dim sql As String = "select deposit,seasontypedesc,0 as seq from seasontypemstr where branchinfoid = " & paramBranchInfoId & " and active = 'Y' union all select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,seasontypedesc"
            'Dim sql As String = "select convert(varchar(50),amount) + '|' + convert(varchar(50),deposit) as deposit,seasontypedesc,0 as seq from seasontypemstr where branchinfoid = " & paramBranchInfoId & " and active = 'Y' union all select convert(varchar(50),0) as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,seasontypedesc"
            dsSeasonType.SelectCommand = sql
            'ddSeasonType.DataBind() bind later

        End If





    End Sub

    Public Sub clear()

        gvPassBay.DataSource = Nothing
        txtDeposit.Text = ""

    End Sub


    Private Sub bindData()
        Dim searchModel As New CPM.PassCardMstrEntity
        Dim sqlmap As New SQLMap

        Try


            searchModel.setActive(ConstantGlobal.Yes)
            searchModel.setLocationInfoId(paramBranchInfoId)


            Dim strSQL As String = sqlmap.getMappedStatement("SetupMstr/Search-PassCardMstrAvailable", searchModel)

            ViewState("strSQL") = strSQL


            dsPassBay.SelectCommand = ViewState("strSQL")
            gvPassBay.DataBind()

            gvPassBay.PageIndex = 0

            If gvPassBay.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
                btnSelect.Enabled = False
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsPassBay).ToString + " " + "Record Found"
                btnSelect.Enabled = True
            End If

        Catch ex As Exception
            lblRecCount.Text = ex.Message

        Finally
            searchModel = Nothing
            sqlmap = Nothing

        End Try

    End Sub



    Protected Sub UpdateRecord(ByVal cn As SqlConnection, ByVal trans As SqlTransaction, ByVal debtorIdkey As Long, ByVal passCardMstrKey As Long, ByVal deposit As Long)

        Dim passEnt As New CPM.PassCardMstrEntity
        Dim passDao As New CPM.PassCardMstrDAO

        'Dim debEnt As New CPM.DebtorEntity
        'Dim debDao As New CPM.DebtorDAO




        Try

            passEnt.setPassCardMstrId(passCardMstrKey)
            passEnt.setDebtorId(debtorIdkey)
            passEnt.setDeposit(deposit)
            passEnt.setLastUpdatedDatetime(Now)
            passEnt.setLastUpdatedBy(lp.getUserMstrId)

            passDao.updateDB(passEnt, cn, trans)

            ''Update Debtor Mstr for column lastpayment,balance
            'debEnt.setDebtorId(debtorIdkey)
            'debEnt.setLastPayment()
            'debEnt.setBalance()
            'debDao.updateDB(debEnt, cn, trans)

            Utility.Tools.updatePassCardHist(cn, trans, passEnt)

        Catch ex As Exception
            Throw ex

        Finally
            passEnt = Nothing
            passDao = Nothing
        End Try


    End Sub


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvPassBay.Rows.Count
            ClientScript.RegisterForEventValidation(gvPassBay.UniqueID, "Select$" + i.ToString)
        Next
        ClientScript.RegisterForEventValidation(lnkProcess.UniqueID)
        MyBase.Render(writer)
    End Sub

    Protected Sub gvPassBay_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPassBay.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvPassBay.UniqueID + "','Select$" + gvPassBay.Rows.Count.ToString + "');")

            Dim int As Integer = gvPassBay.Rows.Count / 2
            Dim dob As Double = gvPassBay.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

            Dim dd As DropDownList
            dd = e.Row.FindControl("ddSeasonType")
            dd.DataBind()

        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub

    Protected Sub lnkProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim gotChecked As Boolean = False
        Dim count = gvPassBay.Rows.Count
        Dim passCardMstrDao As New CPM.PassCardMstrDAO
        Dim bay() As String = {""}


        Try

            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If
            trans = cn.BeginTransaction

            'For Each row As GridViewRow In gvPassBay.Rows
            '    Dim chk As CheckBox
            '    chk = row.FindControl("chkSelect")
            '    If Not chk Is Nothing Then
            '        If chk.Checked Then
            '            gotChecked = True
            '            If Not String.IsNullOrEmpty(gvPassBay.DataKeys(row.RowIndex)(passCardMstrDao.COLUMN_SerialNo).ToString) Then

            '                Dim cbo As DropDownList = DirectCast(row.FindControl("ddSeasonType"), DropDownList)
            '                lblmsg.Text = cbo.SelectedValue

            '                If gvPassBay.DataKeys(row.RowIndex)("ddSeasonType") = "0" Then
            '                    lblmsg.Text = "Please verify the checked value for Season Type."
            '                    Exit Sub
            '                End If
            '            End If
            '        End If
            '    End If
            'Next



            'If ddSeasonType.SelectedIndex = 0 Then
            '    lblmsg.Text = "Season type is a mandatory field."
            '    Exit Sub
            'End If

            For Each row As GridViewRow In gvPassBay.Rows
                Dim chk As CheckBox
                Dim i As Integer = 1
                chk = row.FindControl("chkSelect")
                If Not chk Is Nothing Then
                    If chk.Checked Then
                        gotChecked = True
                        ReDim bay(i)
                        Dim cbo As DropDownList = DirectCast(row.FindControl("ddSeasonType"), DropDownList)
                        If Not String.IsNullOrEmpty(gvPassBay.DataKeys(row.RowIndex)(passCardMstrDao.COLUMN_SerialNo).ToString) Then
                            UpdateRecord(cn, trans, paramDebtorId, gvPassBay.DataKeys(row.RowIndex)(passCardMstrDao.COLUMN_PassCardMstrID), cbo.SelectedValue)
                            bay(i) = gvPassBay.DataKeys(row.RowIndex)(passCardMstrDao.COLUMN_SerialNo)
                            i = i + 1
                        End If
                    End If
                End If
            Next
            If Not gotChecked Then
                If count > 0 Then
                    lblRecCount.Text = "Please select the Pass/Bay No."
                    Exit Sub
                End If
            End If

            trans.Commit()
            lblmsg.Text = ""
            'ddSeasonType.SelectedIndex = 0

            bindData()

            If chkPrint.Checked = True Then
                printReceipt(paramDebtorId, bay)
            End If

        Catch ex As Exception
            trans.Rollback()
            Throw ex
        Finally
            trans.Dispose()
            cn.Close()
            passCardMstrDao = Nothing
        End Try
    End Sub

    Protected Sub printReceipt(ByVal debtorId As String, ByVal bayArray() As String)
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
        Dim rcpDuration As String = ""

        Dim hqInfoDao As New CPM.HQInfoDAO
        Dim debtorDao As New CPM.DebtorDAO
        Dim passCardDao As New CPM.PassCardMstrDAO
        Dim searchModel As New CPM.PassCardMstrEntity


        Try


            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If
            trans = cn.BeginTransaction


            Dim RptYear As Integer

            RptYear = DatePart(DateInterval.Year, DateAdd(DateInterval.Month, +CInt(txtInitialPayment.Text), Today))

            For i = 0 To txtInitialPayment.Text - 1
                rcpDuration = rcpDuration & MonthName(DatePart(DateInterval.Month, DateAdd(DateInterval.Month, +i, Today)), True) & "-"
            Next i

            'rcpDuration = rcpDuration.Substring(0, rcpDuration.Length - 1)

            rcpDuration = rcpDuration & RptYear


            mySql = "SELECT * FROM HQINFO"
            dt = dm.execTable(mySql)

            If dt.Rows.Count > 0 Then
                companyName = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_CompanyName)
                companyAddress = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address1) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address2) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address3)
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




            rptMgr.setParameterDiscrete("debtorName", debtorName)
            rptMgr.setParameterDiscrete("RcpDate", Now.ToShortDateString)
            rptMgr.setParameterDiscrete("RcpPeriod", rcpDuration)

            rptMgr.setParameterDiscrete("RcpNo", dm.getReceiptNextRunningNo(ddLocation.SelectedValue, trans, cn))
            rptMgr.setParameterDiscrete("RcpCategory", category)
            rptMgr.setParameterDiscrete("CardNo", passCard)

            rptMgr.setParameterDiscrete("season", monthlyAmount)
            rptMgr.setParameterDiscrete("reserved", monthlyAmount)
            rptMgr.setParameterDiscrete("deposit", deposit)
            rptMgr.setParameterDiscrete("others", "")

            rptMgr.setParameterDiscrete("seasonQty", "")
            rptMgr.setParameterDiscrete("reservedQty", "")
            rptMgr.setParameterDiscrete("depositQty", "")
            rptMgr.setParameterDiscrete("othersQty", "")

            rptMgr.setParameterDiscrete("rm", "")
            rptMgr.setParameterDiscrete("chequeNo", "")


            rptMgr.Logon()
            hdPreview.Value = "1"
            'set reportManager to session
            Session("ReportManager") = rptMgr
            lblmsg.Text = ""



        Catch ex As Exception
            lblmsg.Text = ex.Message
            trans.Rollback()

        Finally
            hqInfoDao = Nothing
            debtorDao = Nothing
            passCardDao = Nothing
            dt = Nothing
            rptMgr = Nothing
            trans.Commit()

        End Try


    End Sub

    Protected Sub gvPassBay_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPassBay.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindData()
        End If
    End Sub


    Protected Sub gvPassBay_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim passCardMstrDao As New CPM.PassCardMstrDAO
        Dim seasonTypeDao As New CPM.SeasonTypeMstrDAO
        Dim searchModel As New CPM.SeasonTypeMstrEntity
        Dim sqlmap As New SQLMap

        Try
            For Each row As GridViewRow In gvPassBay.Rows
                Dim chk As CheckBox
                Dim dd As ListBox

                chk = row.FindControl("chkSelect")
                If Not chk Is Nothing Then
                    If chk.Checked Then
                        If Not String.IsNullOrEmpty(gvPassBay.DataKeys(row.RowIndex)(passCardMstrDao.COLUMN_SerialNo).ToString) Then
                            '                            searchModel.setBranchInfoId(ddLocation.SelectedValue)
                            dd = row.FindControl("ddSeasonType")
                            searchModel.setSeasonTypeMstrId(dd.SelectedValue)
                            searchModel.setActive(ConstantGlobal.Yes)

                            Dim strSQL As String = sqlmap.getMappedStatement("SetupMstr/Search-SeasonTypeMstr", searchModel)
                            Dim dt As DataTable
                            dt = dm.execTable(strSQL)
                            If dt.Rows.Count > 0 Then
                                accumulatedDeposit = accumulatedDeposit + dt.Rows(0).Item(passCardMstrDao.COLUMN_Deposit)
                                monthlyAmount = dt.Rows(0).Item(seasonTypeDao.COLUMN_Amount)
                                deposit = dt.Rows(0).Item(seasonTypeDao.COLUMN_Deposit)
                            End If

                            strSQL = ""
                            dt.Dispose()
                        End If
                    End If
                End If
            Next

            txtDeposit.Text = String.Format("{0:n2}", accumulatedDeposit)
            txtMonthlyRate.Text = String.Format("{0:n2}", monthlyAmount)

            If Not String.IsNullOrEmpty(txtInitialPayment.Text) Then
                txtTotal.Text = String.Format("{0:n2}", (txtInitialPayment.Text * txtMonthlyRate.Text) + txtDeposit.Text)
            End If

        Catch ex As Exception
            Throw ex
        Finally
            passCardMstrDao = Nothing
        End Try
        
    End Sub

    Protected Sub txtInitialPayment_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not Utility.Tools.NumericValidation(txtInitialPayment.Text) Then
            lblmsg.Text = "Please enter numeric value for Initial Payment."
            Exit Sub
        End If

        If Not String.IsNullOrEmpty(txtInitialPayment.Text) And Not String.IsNullOrEmpty(txtMonthlyRate.Text) And Not String.IsNullOrEmpty(txtDeposit.Text) Then
            txtTotal.Text = String.Format("{0:n2}", (txtInitialPayment.Text * txtMonthlyRate.Text) + txtDeposit.Text)
        End If
    End Sub


End Class
