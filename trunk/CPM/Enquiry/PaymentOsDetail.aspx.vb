Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Enquiry_PaymentOsDetail
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    'Dim paramLocationName As String = ""
    'Dim paramLocationInfoId As String = ""


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        lp = Session("LoginProfile")
        If IsNothing(lp) Then
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("appPath") + "/login.aspx?login=expired")
        End If

        If Not Page.IsPostBack Then
            hdLocationName.Value = Request.Params("LocationName")
            hdLocationId.Value = Request.Params("LocationInfoId")
            txtLocationName.Text = hdLocationName.Value
            bindData()
        End If

        Session.LCID = 2057


    End Sub

    Private Sub bindData()
        Dim searchModel As New CPM.DebtorEntity
        Dim sqlmap As New SQLMap
        Dim strSQL As String

        Try

            searchModel.setLocationInfoId(hdLocationId.Value)
            strSQL = sqlmap.getMappedStatement("Debtor/Debtor-OSDetail", searchModel)

            ViewState("strSQL") = strSQL

            dsPaymentHistDetail.SelectCommand = ViewState("strSQL")
            gvPaymentHistDetail.DataBind()

            gvPaymentHistDetail.PageIndex = 0

            If gvPaymentHistDetail.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsPaymentHistDetail).ToString + " " + "Record Found"
            End If

        Catch ex As Exception
            logger.Error(ex.Message)
            Throw ex

        Finally
            searchModel = Nothing
            sqlmap = Nothing

        End Try

    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvPaymentHistDetail.Rows.Count
            ClientScript.RegisterForEventValidation(gvPaymentHistDetail.UniqueID, "Select$" + i.ToString)
        Next

        Button1.Attributes.Add("onclick", "window.close();return false;")

        MyBase.Render(writer)
    End Sub

    Protected Sub gvPaymentHistDetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPaymentHistDetail.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvPaymentHistDetail.UniqueID + "','Select$" + gvPaymentHistDetail.Rows.Count.ToString + "');")
            Dim int As Integer = gvPaymentHistDetail.Rows.Count / 2
            Dim dob As Double = gvPaymentHistDetail.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

        End If

    End Sub

    Protected Sub gvPaymentHistDetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPaymentHistDetail.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindData()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub

End Class
