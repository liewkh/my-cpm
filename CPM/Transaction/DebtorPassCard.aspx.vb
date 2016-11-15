Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports PopCalendarSpecialDay

Partial Class Maintenance_DebtorPassCard
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Dim paramDebtorId As String
    Dim paramSeasonTypeMstrId As String
    Dim paramLocationInfoId As String
    Dim paramDebtorName As String
    Dim paramUserName As String
    Dim myDate As DateTime
    Dim myYear As Integer

    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        lp = Session("LoginProfile")
        If IsNothing(lp) Then
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("appPath") + "/login.aspx?login=expired")
        End If


        paramDebtorId = Request.Params("debtorId")
        paramLocationInfoId = Request.Params("locationInfoId")
        paramDebtorName = Request.Params("debtorName")
        paramUserName = Request.Params("userName")

        If Not Page.IsPostBack Then
            bindData()

            myDate = DateTime.Now()
            Dim x As Integer
            myYear = myDate.Year

            ddYear.Items.Add("")

            x = myYear - 2
            For x = x To (x + 3)
                ddYear.Items.Add(x)
            Next x

            ddYear.Items.FindByText(myYear).Selected = True

        End If

        Session.LCID = 2057

    End Sub

    Public Sub clear()
        Dim sql As String

        Try
            txtUserName.Text = ""
            ddSeasonType.Enabled = True
            txtCommencementDate.Enabled = True
            txtDeposit.Text = ""
            txtSeasonAmount.Text = ""
            ddSeasonType.SelectedIndex = 0
            ddMake.SelectedIndex = 0
            txtModel.Text = ""
            txtCarRegistrationNo.Text = ""
            txtCommencementDate.Text = ""
            lblmsg.Text = ""
            btnUpdate.Enabled = False

            sql = "select passcardmstrid,SERIALNO,0 as seq from passcardmstr " & _
                    "where status = 'A' and active = 'Y' And locationinfoid =" & paramLocationInfoId & _
                    " union all " & _
                    " select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' " & _
                    " order by seq,serialno"
            dsPass.SelectCommand = sql
            dsPass.DataBind()

        Catch ex As Exception
            lblmsg.Text = ex.Message
        End Try
        
    End Sub


    Private Sub bindData()
        Dim searchModel As New CPM.DebtorPassBayEntity
        Dim sqlmap As New SQLMap

        Try


            Dim sql As String = "select stm.seasontypemstrid,seasontypedesc,0 as seq " & _
                               "from seasontypemstr stm,locationinfo li " & _
            "where(stm.locationinfoid = li.locationinfoid) " & _
            "and li.locationinfoid = " & paramLocationInfoId & _
            "union all " & _
            "select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq"

            dsSeasonType.SelectCommand = sql
            dsSeasonType.DataBind()

            'sql = "select passcardmstrid,SERIALNO,0 as seq from passcardmstr " & _
            '      "where status = 'A' and active = 'Y' And locationinfoid =" & paramLocationInfoId & _
            '      " union all " & _
            '      " select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' " & _
            '      " order by seq,serialno"
            'dsPass.SelectCommand = sql
            'dsPass.DataBind()


            ddLocation.SelectedValue = paramLocationInfoId
            txtDebtorName.Text = paramDebtorName
            txtUserName.Text = paramUserName

            searchModel.setDebtorId(paramDebtorId)

            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorPassBay", searchModel)

            ViewState("strSQL") = strSQL


            dsPassBay.SelectCommand = ViewState("strSQL")
            gvPassBay.DataBind()

            gvPassBay.PageIndex = 0

            If gvPassBay.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsPassBay).ToString + " " + "Record Found"
            End If

        Catch ex As Exception
            Throw ex

        Finally
            searchModel = Nothing
            sqlmap = Nothing

        End Try

    End Sub


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        Dim param As String = ""

        For i As Integer = 0 To gvPassBay.Rows.Count
            ClientScript.RegisterForEventValidation(gvPassBay.UniqueID, "Select$" + i.ToString)
        Next

        'btnSearchPassCard.Attributes.Add("OnClick", "javascript:open_popupModal('../Maintenance/SearchPassCard.aspx?debtorId=" + paramDebtorId.ToString + "','M');")
        'param = "debtorId=" + paramDebtorId.ToString + "&locationInfoId=" + paramLocationInfoId

        'btnSearchPassCard.Attributes.Add("OnClick", "javascript:open_popupModal('../Maintenance/SearchPassCard.aspx?" + param + "','M');")

        ClientScript.RegisterForEventValidation(lnkProcess.UniqueID)

        MyBase.Render(writer)
    End Sub

    Protected Sub gvPassBay_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPassBay.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvPassBay.UniqueID + "','Select$" + gvPassBay.Rows.Count.ToString + "');")
            'e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('gvSeasonType','Select$" + gvSeasonType.Rows.Count.ToString + "');")
            Dim int As Integer = gvPassBay.Rows.Count / 2
            Dim dob As Double = gvPassBay.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

        End If
    End Sub

    Protected Sub gvPassBay_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPassBay.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindData()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)

    End Sub

    Protected Sub lnkProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim gotChecked As Boolean = False
        Dim count = gvPassBay.Rows.Count
        Dim dbpDao As New CPM.DebtorPassBayDAO
        Dim passCard As New ArrayList
        Dim i As Integer = 1

        Try

            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If
            trans = cn.BeginTransaction


            For Each row1 As GridViewRow In gvPassBay.Rows
                Dim chk1 As CheckBox
                chk1 = row1.FindControl("chkSelect")
                If Not chk1 Is Nothing Then
                    If chk1.Checked Then
                        gotChecked = True
                        'ReDim Preserve passCard(i)
                        'passCard(i) = gvPassBay.DataKeys(row1.RowIndex)(dbpDao.COLUMN_PassCardMstrId).ToString
                        passCard.Add(gvPassBay.DataKeys(row1.RowIndex)(dbpDao.COLUMN_PassCardMstrId).ToString)
                        i = i + 1
                    End If
                End If
            Next

            If Not gotChecked Then
                If count > 0 Then
                    lblRecCount.Text = "Please check a selection before generate the Invoice."
                    Exit Sub
                End If
            End If


            Dim invMgr As New InvoiceManager
            Dim retValue As String = ""
            Dim BatchNo As String = ""
            Dim RptYear As Integer

            'RptYear = DatePart(DateInterval.Year, DateAdd(DateInterval.Month, +1, Today))
            RptYear = hidYear.Value
            BatchNo = dm.getNextBatchNo(trans, cn)

            'Consist of those selected passcard to generate the invoice
            retValue = invMgr.createInvoice(lp.getUserMstrId, gvPassBay.SelectedDataKey.Item("DEBTORID").ToString, hidMonth.Value, passCard, BatchNo, RptYear, cn, trans)
            trans.Commit()
            passCard.Clear()
            lblmsg.Text = ""

            If retValue <> "" Then
                PrintInvoice(gvPassBay.SelectedDataKey.Item("DEBTORID").ToString, retValue)
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popUp", "checkToPopUpViewer();", True)
            Else
                lblmsg.Text = "No Invoice Generated."
            End If

            bindData()



        Catch ex As Exception
            trans.Rollback()
            lblmsg.Text = ex.Message
        Finally
            trans.Dispose()
            cn.Close()
            dbpDao = Nothing
        End Try
    End Sub

    Private Sub PrintInvoice(ByVal debtorId As String, ByVal InvoiceNo As String)
        Dim rptMgr As New ReportManager
        Dim mySql As String = ""
        Dim dt As New DataTable
        Dim companyName As String = ""
        Dim companyAddress As String = ""
        Dim tel As String = ""
        Dim fax As String = ""
        Dim searchModel As New CPM.DebtorAccountHeaderEntity
        Dim sqlmap As New SQLMap
        Dim dtPassBayNo As New DataTable
        Dim strPassBay As String
        Dim CompanyNo As String = ""


        Dim hqInfoDao As New CPM.HQInfoDAO
        Dim debtorDao As New CPM.DebtorDAO

        Try

            mySql = "SELECT COMPANYNO,COMPANYNAME,ADDRESS1,ADDRESS2,ADDRESS3,POSTCODE,TELEPHONE,FAX,(SELECT CODEDESC FROM CODEMSTR WHERE CODECAT='STA' AND CODEABBR= STATE) AS STATE FROM HQINFO"
            dt = dm.execTable(mySql)

            If dt.Rows.Count > 0 Then
                companyName = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_CompanyName)
                companyAddress = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address1) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address2) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address3) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_PostCode) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_State)
                tel = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Telephone)
                fax = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Fax)
                CompanyNo = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_CompanyNo)
            End If


            searchModel.setInvoiceNo(InvoiceNo)
            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorPassByInvoice", searchModel)

            dtPassBayNo = dm.execTable(strSQL)
            If dtPassBayNo.Rows.Count > 0 Then
                strPassBay = dtPassBayNo.Rows(0).Item(0).ToString
            Else
                strPassBay = ""
            End If

            dt = dm.execTable(mySql)
            If dt.Rows.Count > 0 Then

                rptMgr.setReportName("Invoice.Rpt")
                rptMgr.setParameterDiscrete("CompanyName", companyName)
                rptMgr.setParameterDiscrete("CompanyAddress", companyAddress)
                rptMgr.setParameterDiscrete("TelephoneNo", tel)
                rptMgr.setParameterDiscrete("Fax", fax)
                rptMgr.setParameterDiscrete("PassBay", strPassBay)
                rptMgr.setParameterDiscrete("PrintedBy", lp.getUserloginId)
                rptMgr.setParameterDiscrete("debtorid", debtorId)
                rptMgr.setParameterDiscrete("invoiceno", InvoiceNo)
                rptMgr.setParameterDiscrete("companyno", CompanyNo)

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

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strSQL As String = ""
        Dim msg As String = ""
        Dim isExist As Boolean = False
        Dim searchModel As New SeasonTypeSearchModel
        Dim dt As DataTable

        Try

            If Not Page.IsValid Then
                Exit Sub
            End If

            cn = New SqlConnection(dm.getDBConn())
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If
            trans = cn.BeginTransaction

            If ddSeasonType.SelectedIndex = 0 Then
                lblmsg.Text = "Season Type is a required field."
                Exit Sub
            End If

            If ddPass.SelectedIndex = 0 Then
                lblmsg.Text = "Please select a Pass."
                Exit Sub
            End If


            'validate the name is not existed before insert
            Dim sqlmap As New SQLMap

            searchModel.setSeasonTypeMstrId(ddSeasonType.SelectedValue)
            strSQL = sqlmap.getMappedStatement("SetupMstr/Search-SeasonTypeMstr", searchModel)


            dt = dm.execTable(strSQL)
            If dt.Rows.Count > 0 Then
                isExist = True
            End If

            strSQL = ""
            dt.Dispose()



            If isExist Then
                InsertRecord(cn, trans)
                trans.Commit()
                clear()
                bindData()
                lblmsg.Text = ConstantGlobal.Record_Added
            Else
                lblmsg.Text = "Please check the Season Type Setup."
            End If



        Catch ex As Exception
            lblmsg.Text = ex.Message
            trans.Rollback()
        Finally
            searchModel = Nothing
            dt = Nothing
            trans.Dispose()
            cn.Close()

        End Try
    End Sub

    Protected Sub InsertRecord(ByVal cn As SqlConnection, ByVal trans As SqlTransaction)

        Dim debtorPBEnt As New CPM.DebtorPassBayEntity
        Dim debtorPBDao As New CPM.DebtorPassBayDAO
        Dim pcDao As New CPM.PassCardMstrDAO
        Dim pcEnt As New CPM.PassCardMstrEntity
        Dim pchDao As New CPM.PassCardHistoryDAO
        Dim pchEnt As New CPM.PassCardHistoryEntity
        Dim dt As New DataTable


        Try

            debtorPBEnt.setDebtorId(paramDebtorId)
            debtorPBEnt.setCarRegistrationNo(Trim(txtCarRegistrationNo.Text.ToUpper))
            debtorPBEnt.setCommencementDate(txtCommencementDate.Text)
            debtorPBEnt.setMake(ddMake.SelectedValue)
            debtorPBEnt.setModel(Trim(txtModel.Text.ToUpper))
            debtorPBEnt.setSeasonTypeMstrId(ddSeasonType.SelectedValue)
            debtorPBEnt.setVehicleType(txtVehicleType.Text)
            debtorPBEnt.setUserName(Trim(txtUserName.Text).ToUpper)
            debtorPBEnt.setStatus("A") 'By default new entry is active
            debtorPBEnt.setPassCardMstrId(ddPass.SelectedValue)

            'If rbPass.Checked = True Then
            'PassCardMstr
            Dim chkFirtUseSql As String = ""

            debtorPBEnt.setSerialNo(ddPass.SelectedItem.Text)
            pcEnt.setPassCardMstrId(ddPass.SelectedValue)
            pcEnt.setDeposit(txtDeposit.Text)
            pcEnt.setStatus(PassCardStatusEnum.INUSE)
            pcEnt.setDebtorId(paramDebtorId)
            pcEnt.setLastUpdatedBy(lp.getUserMstrId)
            pcEnt.setLastUpdatedDatetime(Now)
            pcEnt.setDepositPrint(ConstantGlobal.Yes)

	    'VK - 20140301 - Set off deposit printing in invoices. Deposit manually via debit note.
 	    'VK - 20150105 - Set on deposit printing in invoices per GST Tax invoice requirement.

            chkFirtUseSql = "Select FirstUsedDate From PassCardMstr where PassCardMstrId = " & ddPass.SelectedValue
            dt = dm.execTable(chkFirtUseSql)



            If dt.Rows.Count > 0 Then
                If dt.Rows(0).Item(pcDao.COLUMN_FirstUsedDate).Equals(System.DBNull.Value) Then
                    pcEnt.setFirstUsedDate(Now)
                End If
            End If

            pcDao.updateDB(pcEnt, cn, trans)


            'PassCardHistory
            Dim selectSQL As String = "select max(PassCardHistoryId) as ID from PassCardHistory Where PassCardMstrId = " & ddPass.SelectedValue


            dt = dm.execTable(selectSQL)

            If dt.Rows.Count > 0 Then
                If dt.Rows(0).Item("ID").ToString <> "" Then
                    Dim updateSQL As String = "Update PassCardHistory set enddate = getDate() where PassCardHistoryId = " & dt.Rows(0).Item("ID").ToString
                    dm.execTableInTrans(updateSQL, cn, trans)
                End If
            End If

            pchEnt.setPassCardMstrId(ddPass.SelectedValue)
            pchEnt.setDebtorId(paramDebtorId)
            pchEnt.setDeposit(txtDeposit.Text)
            pchEnt.setStartDate(txtCommencementDate.Text)
            pchEnt.setLastUpdatedBy(lp.getUserMstrId)
            pchEnt.setLastUpdatedDatetime(Now)
            pchEnt.setLocationInfoId(paramLocationInfoId)
            debtorPBEnt.setTypes("P")
            pchDao.insertDB(pchEnt, cn, trans)
            debtorPBEnt.setLastUpdatedDatetime(Now)
            debtorPBEnt.setLastUpdatedBy(lp.getUserMstrId)


            debtorPBDao.insertDB(debtorPBEnt, cn, trans)

            bindPass()

        Catch ex As Exception
            Throw ex

        Finally
            debtorPBEnt = Nothing
            debtorPBDao = Nothing
        End Try


    End Sub

    Protected Sub ddSeasonType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        getDeposit()
    End Sub

    Public Sub getDeposit()
        Dim searchModel As New SeasonTypeSearchModel
        Dim strSQL As String = ""
        Dim dt As DataTable

        Try
            Dim sqlmap As New SQLMap

            txtDeposit.Text = ""
            txtSeasonAmount.Text = ""

            searchModel.setSeasonTypeMstrId(ddSeasonType.SelectedValue)

            strSQL = sqlmap.getMappedStatement("SetupMstr/Search-SeasonTypeMstr", searchModel)


            dt = dm.execTable(strSQL)
            If dt.Rows.Count = 1 Then
                txtDeposit.Text = dt.Rows(0).Item("DEPOSIT").ToString
                txtSeasonAmount.Text = dt.Rows(0).Item("AMOUNT").ToString
                txtVehicleType.Text = dt.Rows(0).Item("VEHICLETYPEDESC").ToString
            End If

            strSQL = ""
            dt.Dispose()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvPassBay_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPassBay.SelectedIndexChanged
        Dim dpbDao As New CPM.DebtorPassBayDAO
        Dim gotChecked As Boolean = False

        Try

            lblmsg.Text = ""

            ddSeasonType.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassBay.SelectedDataKey(dpbDao.COLUMN_SeasonTypeMstrId))
            txtVehicleType.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassBay.SelectedDataKey(dpbDao.COLUMN_VehicleType))
            ddMake.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassBay.SelectedDataKey(dpbDao.COLUMN_Make))
            txtModel.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassBay.SelectedDataKey(dpbDao.COLUMN_Model))
            txtDeposit.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassBay.SelectedDataKey("DEPOSIT"))
            txtSeasonAmount.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassBay.SelectedDataKey("AMOUNT"))
            txtCommencementDate.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassBay.SelectedDataKey(dpbDao.COLUMN_CommencementDate))
            txtCarRegistrationNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassBay.SelectedDataKey(dpbDao.COLUMN_CarRegistrationNo))
            txtUserName.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassBay.SelectedDataKey(dpbDao.COLUMN_UserName))

            For Each row1 As GridViewRow In gvPassBay.Rows
                Dim chk1 As CheckBox
                chk1 = row1.FindControl("chkSelect")
                If Not chk1 Is Nothing Then
                    If chk1.Checked Then
                        gotChecked = True
                    End If
                End If
            Next

            updateMode()

            If Not gotChecked Then
                btnInvoice.Enabled = False
                Exit Sub
            Else
                btnInvoice.Enabled = True
            End If

        Catch ex As Exception
            Throw ex
        Finally
            dpbDao = Nothing

        End Try

    End Sub

    Private Sub updateMode()
        ddSeasonType.Enabled = False
        txtCommencementDate.Enabled = False
        ddPass.Enabled = False
        lblmsg.Text = ""
        btnUpdate.Enabled = True
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        clear()
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim debtorPBEnt As New CPM.DebtorPassBayEntity
        Dim debtorPBDao As New CPM.DebtorPassBayDAO

        Try

            cn = New SqlConnection(dm.getDBConn())
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If
            trans = cn.BeginTransaction

            debtorPBEnt.setDebtorPassBayId(gvPassBay.SelectedDataKey(debtorPBDao.COLUMN_DebtorPassBayID))
            debtorPBEnt.setCarRegistrationNo(Trim(txtCarRegistrationNo.Text.ToUpper))
            debtorPBEnt.setMake(ddMake.SelectedValue)
            debtorPBEnt.setModel(Trim(txtModel.Text.ToUpper))
            debtorPBEnt.setUserName(Trim(txtUserName.Text).ToUpper)
            debtorPBEnt.setLastUpdatedDatetime(Now)
            debtorPBEnt.setLastUpdatedBy(lp.getUserMstrId)

            debtorPBDao.updateDB(debtorPBEnt, cn, trans)
            trans.Commit()
            clear()

        Catch ex As Exception
            lblmsg.Text = ex.Message
            trans.Rollback()
            Throw ex

        Finally
            trans.Dispose()
            cn.Close()

            debtorPBEnt = Nothing
            debtorPBDao = Nothing
        End Try
    End Sub

    Protected Sub btnInvoiceTest_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim invMgr As New InvoiceManager
        Try

            'Pass in debtor id and which month to generate
            'invMgr.createInvoice(gvDebtor.SelectedDataKey("DEBTORID"), ddMonth.SelectedValue)

        Catch ex As Exception
            lblmsg.Text = ex.Message
            logger.Error(ex.Message)
        End Try

    End Sub

    Protected Sub ddItemType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        bindPass()
    End Sub

    Private Sub bindPass()
        Try
            Dim sql As String
            sql = "select passcardmstrid,SERIALNO,0 as seq from passcardmstr " & _
                  " where status = 'A' and active = 'Y' And locationinfoid =" & paramLocationInfoId & _
                  " and itemtype = '" & ddItemType.SelectedValue & "'" & _
                  " union all " & _
                  " select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' " & _
                  " order by seq,serialno"

            dsPass.SelectCommand = sql
            dsPass.DataBind()

        Catch ex As Exception
            logger.Error(ex.Message)
        End Try
    End Sub

End Class

