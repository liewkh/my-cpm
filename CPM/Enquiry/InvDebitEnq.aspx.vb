Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Enquiry_InvDebEnq
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


            If gvDebtorInv.DataKeys(e.Row.RowIndex)("STATUSCODE").ToString = InvoiceStatusEnum.CANCEL Then
                Dim prn As Button = CType(e.Row.FindControl("btnPrint"), Button)
                prn.Enabled = False
            End If

        End If


    End Sub

    Protected Sub gvDebtorInv_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDebtorInv.RowCommand
        'For Print
        If e.CommandName.Equals("Print") Then             
            PrintData(e.CommandArgument)
        ElseIf e.CommandName.Equals("Sort") Then
            SearchData()
        ElseIf e.CommandName.Equals("Page") Then
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
        txtDebtorName.CssClass = CSSEnum.TXTFIELD_3_DISABLED
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
        txtDebtorName.CssClass = CSSEnum.TXTFIELD_3
        ddLocation.Enabled = True
        ddLocation.CssClass = CSSEnum.DROPDOWN
        rbCompany.Enabled = True
        rbIndividual.Enabled = True

    End Sub

    Private Sub SearchData()
        Dim searchModel As New CPM.DebtorAccountHeaderEntity
        Dim sqlmap As New SQLMap

        Try

            lblmsg.Text = ""

            searchModel.setDebtorId(hidDebtorId.Value)

            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorInvoice", searchModel)

            ViewState("strSQL") = strSQL

            dsDebtorInv.SelectCommand = ViewState("strSQL")
            gvDebtorInv.DataBind()

            gvDebtorInv.PageIndex = 0

            If gvDebtorInv.Rows.Count = 0 Then
                lblmsg.Text = ConstantGlobal.No_Record_Found
                SearchMode()
                lblmsg.Text = "No Invoice found for the selected debtor."
            Else
                lblRecCount2.Text = dm.getGridViewRecordCount(dsDebtorInv).ToString + " " + "Record Found"
            End If


        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            searchModel = Nothing
            sqlmap = Nothing

        End Try
    End Sub

    Protected Sub btnDataBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        SearchMode()
    End Sub


    Private Sub PrintData(ByVal e As Object)
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
        Dim companyNo As String = ""



        Dim hqInfoDao As New CPM.HQInfoDAO
        Dim debtorDao As New CPM.DebtorDAO

        Try

            mySql = "SELECT COMPANYNAME,COMPANYNO,ADDRESS1,ADDRESS2,ADDRESS3,POSTCODE,TELEPHONE,FAX,(SELECT CODEDESC FROM CODEMSTR WHERE CODECAT='STA' AND CODEABBR= STATE) AS STATE FROM HQINFO"
            dt = dm.execTable(mySql)

            If dt.Rows.Count > 0 Then
                companyName = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_CompanyName)
                companyAddress = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address1) & " " & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address2) & " " & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address3) & " " & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_PostCode) & " " & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_State)
                tel = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Telephone)
                fax = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Fax)
                companyNo = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_CompanyNo)
            End If


            'searchModel.setInvoiceNo(e.ToString)
            'Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorPassByInvoice", searchModel)

            'dtPassBayNo = dm.execTable(strSQL)
            'If dtPassBayNo.Rows.Count > 0 Then
            '    strPassBay = dtPassBayNo.Rows(0).Item(0).ToString
            'Else
            '    strPassBay = ""
            'End If

            dt = dm.execTable(mySql)
            If dt.Rows.Count > 0 Then

                If String.IsNullOrEmpty(e.ToString.Split("|")(3).ToString()) Then 'PasscardNo

                    If e.ToString.Split("|")(1).Equals(TxnTypeEnum.DEBITNOTE) Then
                        rptMgr.setReportName("DebitNoteGST.Rpt")
                    Else
                        If e.ToString.Split("|")(0).Substring(4, 1).Equals("S") Then
                            rptMgr.setReportName("TaxInvoice.Rpt")
                            rptMgr.setParameterDiscrete("MIRemark", e.ToString.Split("|")(2).ToString())
                        Else
                            rptMgr.setReportName("Invoice.Rpt")
                        End If
                        'rptMgr.setParameterDiscrete("PassBay", strPassBay)
                    End If
                Else
                    rptMgr.setReportName("SeasonInvoice.Rpt")
                End If


                rptMgr.setParameterDiscrete("CompanyName", companyName)
                rptMgr.setParameterDiscrete("CompanyAddress", companyAddress)
                rptMgr.setParameterDiscrete("TelephoneNo", tel)
                rptMgr.setParameterDiscrete("Fax", fax)

                rptMgr.setParameterDiscrete("PrintedBy", lp.getUserLoginId)
                rptMgr.setParameterDiscrete("debtorid", hidDebtorId.Value)
                rptMgr.setParameterDiscrete("invoiceno", e.ToString.Split("|")(0))
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

    Protected Sub btnDataSearch_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim searchModel As New CPM.DebtorAccountHeaderEntity
        Dim sqlmap As New SQLMap

        Try

            lblmsg.Text = ""

            searchModel.setDebtorId(hidDebtorId.Value)
            searchModel.setInvoiceNo(Trim(txtNo.Text.ToUpper))

            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorInvoice", searchModel)

            ViewState("strSQL") = strSQL

            dsDebtorInv.SelectCommand = ViewState("strSQL")
            gvDebtorInv.DataBind()

            gvDebtorInv.PageIndex = 0

            If gvDebtorInv.Rows.Count = 0 Then
                lblRecCount2.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount2.Text = dm.getGridViewRecordCount(dsDebtorInv).ToString + " " + "Record Found"
            End If


        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            searchModel = Nothing
            sqlmap = Nothing

        End Try
    End Sub

End Class
