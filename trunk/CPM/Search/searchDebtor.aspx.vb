Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports PopCalendarSpecialDay

Partial Class Search_searchDebtor
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Dim paramDebtorId As String
    Dim paramDebtorName As String
    Dim paramLocationInfoId As String
    Dim paramLocationName As String

    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        lp = Session("LoginProfile")
        If IsNothing(lp) Then
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("appPath") + "/login.aspx?login=expired")
        End If

        paramDebtorName = Request.Params("debtorName")
        txtDebtor.Text = paramDebtorName
        paramDebtorId = Request.Params("debtorId")

        paramLocationInfoId = Request.Params("locationId")
        paramLocationName = Request.Params("locationName")
        txtLocation.Text = paramLocationName

        If Not Page.IsPostBack Then
            bindData()
        End If


    End Sub

    Public Sub clear()

        gvInv.DataSource = Nothing

    End Sub


    Private Sub bindData()
        Dim searchModel As New CPM.DebtorEntity
        Dim debtorDao As New CPM.DebtorDAO
        Dim sqlmap As New SQLMap

        Try

            searchModel.setDebtorId(paramDebtorId)


            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorOS", searchModel)

            ViewState("strSQL") = strSQL


            dsInv.SelectCommand = ViewState("strSQL")
            gvInv.DataBind()

            'txtCategory.Text = gvInv.CaptioColumns(debtorDao.COLUMN_Category)

            gvInv.PageIndex = 0

            If gvInv.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsInv).ToString + " " + "Record Found"

            End If

        Catch ex As Exception
            lblRecCount.Text = ex.Message

        Finally
            searchModel = Nothing
            debtorDao = Nothing
            sqlmap = Nothing

        End Try

    End Sub


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        'Dim param As String = ""

        For i As Integer = 0 To gvInv.Rows.Count
            ClientScript.RegisterForEventValidation(gvInv.UniqueID, "Select$" + i.ToString)
        Next

        ''btnSearchPassCard.Attributes.Add("OnClick", "javascript:open_popupModal('../Maintenance/SearchPassCard.aspx?debtorId=" + paramDebtorId.ToString + "','M');")
        'param = "debtorId=" + paramDebtorId.ToString + "&branchInfoId=" + paramBranchInfoId

        'btnSearchPassCard.Attributes.Add("OnClick", "javascript:open_popupModal('../Maintenance/SearchPassCard.aspx?" + param + "','M');")

        'ClientScript.RegisterForEventValidation(lnkProcess.UniqueID)

        MyBase.Render(writer)
    End Sub

    Protected Sub gvInv_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvInv.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvInv.UniqueID + "','Select$" + gvInv.Rows.Count.ToString + "');")

            Dim int As Integer = gvInv.Rows.Count / 2
            Dim dob As Double = gvInv.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

        End If
    End Sub

    Protected Sub gvPassBay_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvInv.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindData()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub

    Protected Sub lnkProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        bindData()
    End Sub
End Class
