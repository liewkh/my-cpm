Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Enquiry_PassCardInfoHist
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Dim paramPassCardMstrId As String = ""


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        lp = Session("LoginProfile")
        If IsNothing(lp) Then
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("appPath") + "/login.aspx?login=expired")
        End If

        txtTotal.Text = ""

        If Not Page.IsPostBack Then
            paramPassCardMstrId = Request.Params("PassCardMstrId")
            txtItemType.Text = Request.Params("ItemType")
            txtPassCard.Text = Request.Params("PassCard")

            bindData()
        End If
        
        Session.LCID = 2057

    End Sub

    Private Sub bindData()
        Dim searchModel As New PassCardSearchModel
        Dim passCardMstrDao As New CPM.PassCardMstrDAO
        Dim sqlmap As New SQLMap
        Dim strSQL As String

        Try

            searchModel.setPassCardMstrId(paramPassCardMstrId)
            strSQL = sqlmap.getMappedStatement("PassCardMstr/Search-PassCardHistory", searchModel)

            ViewState("strSQL") = strSQL

            dsPassHist.SelectCommand = ViewState("strSQL")
            gvPassHist.DataBind()

            gvPassHist.PageIndex = 0

            If gvPassHist.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsPassHist).ToString + " " + "Record Found"
            End If

        Catch ex As Exception
            Throw ex

        Finally
            passCardMstrDao = Nothing
            searchModel = Nothing
            sqlmap = Nothing

        End Try

    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvPassHist.Rows.Count
            ClientScript.RegisterForEventValidation(gvPassHist.UniqueID, "Select$" + i.ToString)
        Next

        MyBase.Render(writer)
    End Sub

    Protected Sub gvPassHist_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPassHist.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvPassHist.UniqueID + "','Select$" + gvPassHist.Rows.Count.ToString + "');")
            Dim int As Integer = gvPassHist.Rows.Count / 2
            Dim dob As Double = gvPassHist.Rows.Count / 2
            Dim total As Integer

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

            If Not DataBinder.Eval(e.Row.DataItem, "DURATION").Equals(System.DBNull.Value) Then
                total += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "DURATION"))
            End If
            txtTotal.Text = total
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub

End Class
