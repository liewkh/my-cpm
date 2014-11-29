Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Transaction_InvoiceCancellation
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
            txtCancellationDate.Text = Utility.DataTypeUtils.formatDateString(Now)
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
        txtCancellationDate.Text = Utility.DataTypeUtils.formatDateString(Now)
        ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        txtDebtorName.Text = ""
        rbCompany.Checked = True
        rbIndividual.Checked = False
        txtRemark.Text = ""
        gvDebtorEnq.DataSource = Nothing
        gvInvoice.DataSource = Nothing
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


    Protected Sub gvInvoice_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvInvoice.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            'e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvInvoice.UniqueID + "','Select$" + gvInvoice.Rows.Count.ToString + "');")
            'e.Row.Attributes.Add("OnClick", "javascript:this.className='tb-highlightSelected';")
            Dim int As Integer = gvInvoice.Rows.Count / 2
            Dim dob As Double = gvInvoice.Rows.Count / 2

            'If (dob.Equals(int)) Then
            '    e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            'Else
            '    e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            'End If
            'e.Row.Attributes.Add("onclick", "onGridViewRowSelected('" + e.Row.RowIndex.ToString + "')")
        End If


    End Sub

    Protected Sub gvInvoice_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvInvoice.RowCommand
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
        Dim dpDao As New CPM.DebtorDAO

        Try
            hidDebtorId.Value = gvDebtorEnq.SelectedDataKey(dpDao.COLUMN_DebtorID).ToString
            txtDebtorName.Text = gvDebtorEnq.SelectedDataKey("DEBTOR").ToString
            DataMode()


        Catch ex As Exception
            logger.Error(ex.Message)
            lblmsg.Text = ex.Message
        Finally
            dpDao = Nothing
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

        Try

            lblmsg.Text = ""

            searchModel.setDebtorId(hidDebtorId.Value)
            searchModel.setStatus(InvoiceStatusEnum.OUTSTANDING)
            searchModel.setAmount(0)

            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorInvoice", searchModel)

            ViewState("strSQL") = strSQL

            dsInvoice.SelectCommand = ViewState("strSQL")
            gvInvoice.DataBind()

            gvInvoice.PageIndex = 0

            If gvInvoice.Rows.Count = 0 Then
                lblRecCount2.Text = ConstantGlobal.No_Record_Found
                SearchMode()
                lblmsg.Text = "No Invoice found for the selected debtor."
            Else
                lblRecCount2.Text = dm.getGridViewRecordCount(dsInvoice).ToString + " " + "Record Found"

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
        clearInvoice()
    End Sub

    Private Sub clearInvoice()
        txtRemark.Text = ""
    End Sub


    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dahEnt As New CPM.DebtorAccountHeaderEntity
        Dim dahDao As New CPM.DebtorAccountHeaderDAO
        Dim sql As String = ""
        Dim dt As New DataTable
        Dim dahid As String = ""
        Dim gotChecked As Boolean = False
        Dim count = gvInvoice.Rows.Count

        Try

            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If
            trans = cn.BeginTransaction

            If Trim(txtRemark.Text) = "" Then
                lblmsg.Text = "Remark is a mandatory field."
                Exit Sub
            End If

            If Trim(txtCancellationDate.Text) = "" Then
                lblmsg.Text = "Cancellation Date is a mandatory field."
                Exit Sub
            End If


            For Each row1 As GridViewRow In gvInvoice.Rows
                Dim chk1 As CheckBox
                chk1 = row1.FindControl("chkSelect")
                If Not chk1 Is Nothing Then
                    If chk1.Checked Then
                        gotChecked = True
                        dahid = gvInvoice.DataKeys(row1.RowIndex)(dahDao.COLUMN_DebtorAccountHeaderID).ToString
                        dahEnt.setDebtorAccountHeaderId(dahid)
                        dahEnt.setStatus(InvoiceStatusEnum.CANCEL)
                        dahEnt.setCancellationRemark(Trim(txtRemark.Text))
                        dahEnt.setCancellationDate(txtCancellationDate.Text)
                        dahEnt.setLastUpdatedBy(lp.getUserMstrId)
                        dahEnt.setLastUpdatedDatetime(Now)
                        dahDao.updateDB(dahEnt, cn, trans)

                        sql = "DELETE INVOICEHISTORY WHERE DEBTORACCOUNTHEADERID = " & dahid
                        dt = dm.execTableInTrans(sql, cn, trans)
                    End If
                End If
            Next

            If Not gotChecked Then
                If count > 0 Then
                    lblRecCount.Text = "Please make a selection."
                    Exit Sub
                End If
            End If


           


            trans.Commit()

            SearchData()

            lblmsg.Text = ""

            clearInvoice()

        Catch ex As Exception
            trans.Rollback()
            lblmsg.Text = ex.Message
        Finally
            trans.Dispose()
            cn.Close()
            dahEnt = Nothing
            dahDao = Nothing
            dt = Nothing
        End Try
    End Sub
End Class
