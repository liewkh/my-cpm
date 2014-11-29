Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Enquiry_ParkingCancellationEnq
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

    Private Sub clear()
        lblmsg.Text = ""
        lblRecCount2.Text = ""
        ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        txtDebtorName.Text = ""
        rbCompany.Checked = True
        rbIndividual.Checked = False
        gvPC.DataSource = Nothing
    End Sub


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvPC.Rows.Count
            ClientScript.RegisterForEventValidation(gvPC.UniqueID, "Select$" + i.ToString)
        Next

        MyBase.Render(writer)
    End Sub


    Protected Sub gvPC_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPC.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvPC.UniqueID + "','Select$" + gvPC.Rows.Count.ToString + "');")
            Dim int As Integer = gvPC.Rows.Count / 2
            Dim dob As Double = gvPC.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

            'If gvDebtorRcp.DataKeys(e.Row.RowIndex)("STATUS").ToString = ReceiptStatusEnum.CANCEL Then
            '    Dim prn As Button = CType(e.Row.FindControl("btnPrint"), Button)
            '    prn.Enabled = False
            'End If


        End If


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub


    Private Sub DataMode()
        txtDebtorName.ReadOnly = True
        txtDebtorName.CssClass = CSSEnum.TXTFIELD_2_DISABLED
        ddLocation.Enabled = False
        ddLocation.CssClass = CSSEnum.DROPDOWN_DISABLED
        rbCompany.Enabled = False
        rbIndividual.Enabled = False
        SearchData()

    End Sub

    Private Sub SearchMode()
        txtDebtorName.ReadOnly = False
        txtDebtorName.CssClass = CSSEnum.TXTFIELD_2
        ddLocation.Enabled = True
        ddLocation.CssClass = CSSEnum.DROPDOWN
        rbCompany.Enabled = True
        rbIndividual.Enabled = True

    End Sub

    Private Sub SearchData()
        Dim searchModel As New ParkingCancellationSearchModel
        Dim sqlmap As New SQLMap

        Try

            lblmsg.Text = ""

            searchModel.setDebtorId(hidDebtorId.Value)

            If ddLocation.SelectedIndex = 0 Then
                Dim a As ListItem
                Dim b As String = ""
                For Each a In ddLocation.Items
                    b = b + a.Value.ToString() + ","
                Next
                searchModel.setLocationInfoId(b)
            Else
                searchModel.setLocationInfoId(ddLocation.SelectedValue)
            End If


            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorReceipt", searchModel)

            ViewState("strSQL") = strSQL

            dsPC.SelectCommand = ViewState("strSQL")
            gvPC.DataBind()

            gvPC.PageIndex = 0

            If gvPC.Rows.Count = 0 Then
                lblmsg.Text = ConstantGlobal.No_Record_Found
                SearchMode()
                lblmsg.Text = "No Parking Cancellation found for the selected debtor."
            Else
                lblRecCount2.Text = dm.getGridViewRecordCount(dsPC).ToString + " " + "Record Found"
            End If


        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            searchModel = Nothing
            sqlmap = Nothing

        End Try
    End Sub


    Protected Sub btnDataSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        bindData()
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        clear()
        ViewState("strSQL") = Nothing
    End Sub

    Private Sub bindData()
        Dim searchModel As New DebtorPassCancelSearchModel
        Dim sqlmap As New SQLMap


        Try

            lblmsg.Text = ""

            searchModel.setName(Trim(txtDebtorName.Text))

            If ddLocation.SelectedIndex = 0 Then
                Dim a As ListItem
                Dim b As String = ""
                For Each a In ddLocation.Items
                    b = b + a.Value.ToString() + ","
                Next
                searchModel.setLocationInfoId(b)
            Else
                searchModel.setLocationInfoId(ddLocation.SelectedValue)
            End If

            If rbCompany.Checked = True Then
                searchModel.setCategory(CategoryEnum.COMPANY)
            Else
                searchModel.setCategory(CategoryEnum.INDIVIDUAL)
            End If

            searchModel.setMonthFrom(ddMonth.SelectedValue)
            searchModel.setYearFrom(ddYear.SelectedValue)



            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-ParkingCancellation", searchModel)

            ViewState("strSQL") = strSQL

            dsPC.SelectCommand = ViewState("strSQL")
            gvPC.DataBind()

            gvPC.PageIndex = 0

            If gvPC.Rows.Count = 0 Then
                lblRecCount2.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount2.Text = dm.getGridViewRecordCount(dsPC).ToString + " " + "Record Found"
            End If


        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            searchModel = Nothing
            sqlmap = Nothing

        End Try
    End Sub

    Protected Sub gvPC_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPC.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindData()
        End If
    End Sub
End Class
