Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Enquiry_RcpDebitEnq
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Dim theID As String


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
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
        End If
        Session.LCID = 2057

        ddLocation.SelectedValue = lp.getDefaultLocationInfoId
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
        rbCompany.Checked = True
        rbIndividual.Checked = False
        gvDebtorEnq.DataSource = Nothing
        gvDebtorRcp.DataSource = Nothing
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


    Protected Sub gvDebtorRcp_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDebtorRcp.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvDebtorRcp.UniqueID + "','Select$" + gvDebtorRcp.Rows.Count.ToString + "');")
            Dim int As Integer = gvDebtorRcp.Rows.Count / 2
            Dim dob As Double = gvDebtorRcp.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

            If gvDebtorRcp.DataKeys(e.Row.RowIndex)("STATUS").ToString = ReceiptStatusEnum.CANCEL Then
                Dim prn As Button = CType(e.Row.FindControl("btnPrint"), Button)
                prn.Enabled = False
            End If
            

        End If


    End Sub

    Protected Sub gvDebtorInv_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDebtorRcp.RowCommand
        'For Print
        If e.CommandName.Equals("Print") Then
            PrintData(e.CommandArgument)
        ElseIf e.CommandName.Equals("Sort") Then
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
            hidDebtorId.Value = gvDebtorEnq.SelectedDataKey(debtorDao.COLUMN_DebtorID).ToString
            txtDebtorName.Text = gvDebtorEnq.SelectedDataKey("DEBTOR").ToString
            dataMode()


        Catch ex As Exception
            logger.Error(ex.Message)
            lblmsg.Text = ex.Message
        Finally
            debtorDao = Nothing
        End Try

    End Sub


    Private Sub DataMode()
        divSearch.Visible = False
        divData.Visible = True
        txtDebtorName.ReadOnly = True
        txtDebtorName.CssClass = CSSEnum.TXTFIELD_2_DISABLED
        ddLocation.Enabled = False
        ddLocation.CssClass = CSSEnum.DROPDOWN_DISABLED
        rbCompany.Enabled = False
        rbIndividual.Enabled = False
        SearchData()

    End Sub

    Private Sub SearchMode()
        divSearch.Visible = True
        divData.Visible = False
        txtDebtorName.ReadOnly = False
        txtDebtorName.CssClass = CSSEnum.TXTFIELD_2
        ddLocation.Enabled = True
        ddLocation.CssClass = CSSEnum.DROPDOWN
        rbCompany.Enabled = True
        rbIndividual.Enabled = True

    End Sub

    Private Sub SearchData()
        Dim searchModel As New CPM.DebtorPaymentEntity
        Dim sqlmap As New SQLMap

        Try

            lblmsg.Text = ""

            searchModel.setDebtorId(hidDebtorId.Value)

            'If rbReceipt.Checked = True Then
            '    searchModel.setTxnType(TxnTypeEnum.RECEIPT)
            'Else
            '    searchModel.setTxnType(TxnTypeEnum.CREDITNOTE)
            'End If


            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorReceipt", searchModel)

            ViewState("strSQL") = strSQL

            dsDebtorRcp.SelectCommand = ViewState("strSQL")
            gvDebtorRcp.DataBind()

            gvDebtorRcp.PageIndex = 0

            If gvDebtorRcp.Rows.Count = 0 Then
                lblmsg.Text = ConstantGlobal.No_Record_Found
                SearchMode()
                lblmsg.Text = "No Receipt found for the selected debtor."
            Else
                lblRecCount2.Text = dm.getGridViewRecordCount(dsDebtorRcp).ToString + " " + "Record Found"
            End If


        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            searchModel = Nothing
            sqlmap = Nothing

        End Try
    End Sub

    Protected Sub btnDataBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        hdPreview.Value = ""
        SearchMode()
    End Sub

    Private Sub PrintData(ByVal e As Object)
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
        Dim amount As String = ""

        Dim hqInfoDao As New CPM.HQInfoDAO
        Dim debtorDao As New CPM.DebtorDAO
        Dim passCardDao As New CPM.PassCardMstrDAO
        Dim searchModel As New CPM.PassCardMstrEntity


        Try


            mySql = "SELECT COMPANYNAME,COMPANYNO,ADDRESS1,ADDRESS2,ADDRESS3,POSTCODE,TELEPHONE,FAX,(SELECT CODEDESC FROM CODEMSTR WHERE CODECAT='STA' AND CODEABBR= STATE) AS STATE FROM HQINFO"
            dt = dm.execTable(mySql)

            If dt.Rows.Count > 0 Then
                companyName = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_CompanyName) & " " & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_CompanyNo)
                companyAddress = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address1) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address2) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address3) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_PostCode) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_State)
                tel = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Telephone)
                fax = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Fax)
            End If

            If e.ToString.Split("|")(1).Equals(PaymentTypeEnum.CREDITNOTE) Then
                rptMgr.setReportName("CreditNote.Rpt")
            Else
                rptMgr.setReportName("Receipt.Rpt")
            End If


            rptMgr.setParameterDiscrete("CompanyName", companyName)
            rptMgr.setParameterDiscrete("Address", companyAddress)
            rptMgr.setParameterDiscrete("TelephoneNo", tel)
            rptMgr.setParameterDiscrete("Fax", fax)

            Dim myDebtorSql As String = "SELECT * FROM DEBTOR WHERE DEBTORID = " & hidDebtorId.Value
            dt = dm.execTable(myDebtorSql)
            If dt.Rows.Count > 0 Then
                debtorName = dt.Rows.Item(0).Item(debtorDao.COLUMN_Name)
                category = dt.Rows.Item(0).Item(debtorDao.COLUMN_Category)
            End If

            searchModel.setDebtorId(hidDebtorId.Value)
            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorPassBayNo", searchModel)

            dt = dm.execTable(strSQL)
            If dt.Rows.Count > 0 Then
                passCard = dt.Rows(0).Item(0).ToString
            End If



            strSQL = "SELECT AMOUNT FROM DEBTORPAYMENT WHERE DEBTORPAYMENTID = " & CInt(e.ToString.Split("|")(0))

            dt = dm.execTable(strSQL)
            If dt.Rows.Count > 0 Then
                amount = dt.Rows(0).Item(0).ToString
            End If

            rptMgr.setParameterDiscrete("debtorPaymentId", e.ToString.Split("|")(0))
            rptMgr.setParameterDiscrete("CardNo", passCard)

            rptMgr.setParameterDiscrete("season", "")
            rptMgr.setParameterDiscrete("reserved", "")
            rptMgr.setParameterDiscrete("deposit", "")
            rptMgr.setParameterDiscrete("others", "")

            rptMgr.setParameterDiscrete("seasonQty", "")
            rptMgr.setParameterDiscrete("reservedQty", "")
            rptMgr.setParameterDiscrete("depositQty", "")
            rptMgr.setParameterDiscrete("othersQty", "")

            rptMgr.setParameterDiscrete("rm", Utility.Tools.SpellNumber(amount))


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

    Protected Sub btnDataSearch_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim searchModel As New CPM.DebtorPaymentEntity
        Dim sqlmap As New SQLMap

        Try

            lblmsg.Text = ""

            searchModel.setDebtorId(hidDebtorId.Value)
            searchModel.setReceiptNo(Trim(txtNo.Text))


            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorReceipt", searchModel)

            ViewState("strSQL") = strSQL

            dsDebtorRcp.SelectCommand = ViewState("strSQL")
            gvDebtorRcp.DataBind()

            gvDebtorRcp.PageIndex = 0

            If gvDebtorRcp.Rows.Count = 0 Then
                lblRecCount2.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount2.Text = dm.getGridViewRecordCount(dsDebtorRcp).ToString + " " + "Record Found"
            End If


        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            searchModel = Nothing
            sqlmap = Nothing

        End Try
    End Sub
End Class
