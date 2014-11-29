Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Transaction_ParkingRefund
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

                bindData()
                clear()

            End If
            ddLocation.SelectedValue = lp.getDefaultLocationInfoId

        Catch ex As Exception
            lblmsg.Text = ex.Message

        End Try




    End Sub

    Private Sub bindData()
        Dim searchModel As New ParkingCancellationSearchModel
        Dim sqlmap As New SQLMap

        Try

            lblmsg.Text = ""


            If Not Page.IsPostBack Then
                searchModel.setLocationInfoId(lp.getDefaultLocationInfoId)
            Else
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
            End If
            

            searchModel.setStatus(ItemReplacementStatusEnum.VERIFIED)
            searchModel.setName(Trim(txtDebtorName.Text.ToUpper))

            If rbCompany.Checked = True Then
                searchModel.setCategory(CategoryEnum.COMPANY)
            Else
                searchModel.setCategory(CategoryEnum.INDIVIDUAL)
            End If

            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Parking-CancellationVerified", searchModel)

            ViewState("strSQL") = strSQL


            dsRefundEnq.SelectCommand = ViewState("strSQL")
            gvRefundEnq.DataBind()

            gvRefundEnq.PageIndex = 0

            If gvRefundEnq.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsRefundEnq).ToString + " " + "Record Found"
            End If


        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            searchModel = Nothing
            sqlmap = Nothing

        End Try

    End Sub

    Private Sub clear()
        lblmsg.Text = ""
        lblRecCount.Text = ""
        ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        gvRefundEnq.DataSource = Nothing

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        bindData()
    End Sub


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvRefundEnq.Rows.Count
            ClientScript.RegisterForEventValidation(gvRefundEnq.UniqueID, "Select$" + i.ToString)
        Next

        MyBase.Render(writer)
    End Sub

    Protected Sub gvRefundEnq_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRefundEnq.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            'e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvRefundEnq.UniqueID + "','Select$" + gvRefundEnq.Rows.Count.ToString + "');")
            Dim int As Integer = gvRefundEnq.Rows.Count / 2
            Dim dob As Double = gvRefundEnq.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If
        End If

    End Sub


    Protected Sub gvRefundEnq_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvRefundEnq.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindData()
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        clear()
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dcEnt As New CPM.DebtorPassBayNoCancelEntity
        Dim dcDao As New CPM.DebtorPassBayNoCancelDAO
        Dim gotChecked As Boolean = False
        Dim dcId As String = ""
        Dim count = gvRefundEnq.Rows.Count

        Try

            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If
            trans = cn.BeginTransaction

            For Each row1 As GridViewRow In gvRefundEnq.Rows
                Dim txt As TextBox = DirectCast(row1.FindControl("txtChequeNo"), TextBox)
                Dim dt As TextBox = DirectCast(row1.FindControl("txtChequeDate"), TextBox)
                If Trim(txt.Text) <> "" Then
                    dcId = gvRefundEnq.DataKeys(row1.RowIndex)(dcDao.COLUMN_DebtorPassBayNoCancelID).ToString
                    dcEnt.setDebtorPassBayNoCancelId(dcId)
                    dcEnt.setChequeNo(Trim(txt.Text))
                    If dt.Text <> "" Then
                        dcEnt.setChequeDate(CDate(dt.Text))
                    End If
                    dcEnt.setLastUpdatedDatetime(Now)
                    dcEnt.setLastUpdatedBy(lp.getUserMstrId)
                    dcDao.updateDB(dcEnt, cn, trans)
                End If
            Next

            trans.Commit()
            bindData()
            lblmsg.Text = ""


        Catch ex As Exception
            trans.Rollback()
            lblmsg.Text = ex.Message
            logger.Error(ex.Message)
        Finally
            trans.Dispose()
            cn.Close()

        End Try
    End Sub
End Class
