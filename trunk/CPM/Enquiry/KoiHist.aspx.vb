Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Enquiry_KoiHist
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Dim paramLocationInfoId As String = ""
    Dim paramLocationName As String = ""
    Dim paramDate As String = ""

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try

            Session.LCID = 2057

            lp = Session("LoginProfile")
            If IsNothing(lp) Then
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("appPath") + "/login.aspx?login=expired")
            End If

            txtDate.Text = ""
            txtLocation.Text = ""

            If Not Page.IsPostBack Then
                paramLocationInfoId = Request.Params("LocationInfoId")
                paramLocationName = Request.Params("LocationName")
                paramDate = Request.Params("Date")
                txtLocation.Text = paramLocationName
                txtDate.Text = paramDate
                bindData()
            End If

            'Session.LCID = 2057

        Catch ex As Exception
            logger.Error(ex.Message)
        End Try
        


    End Sub

    Private Sub bindData()
        Dim searchModel As New dailyCollectionSearchModel
        Dim sqlmap As New SQLMap
        Dim strSQL As String

        Try

            searchModel.setLocationInfoId(paramLocationInfoId)
            searchModel.setTransactionDate(Format(CDate(paramDate), "dd/MM/yyyy"))


            strSQL = sqlmap.getMappedStatement("DailyCollection/Search-DailyCollectionKoi", searchModel)

            ViewState("strSQL") = strSQL

            dsKoiHist.SelectCommand = ViewState("strSQL")
            gvKoiHist.DataBind()

            gvKoiHist.PageIndex = 0

        Catch ex As Exception
            logger.Error(ex.Message)
        Finally
            searchModel = Nothing
            sqlmap = Nothing

        End Try

    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        Try
            For i As Integer = 0 To gvKoiHist.Rows.Count
                ClientScript.RegisterForEventValidation(gvKoiHist.UniqueID, "Select$" + i.ToString)
            Next

            btnClose.Attributes.Add("onclick", "window.close();return false;")

            MyBase.Render(writer)
        Catch ex As Exception
            logger.Error(ex.Message)
        End Try
        
    End Sub

    Protected Sub gvKoiHist_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvKoiHist.RowDataBound
        Try
            If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
                e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
                e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvKoiHist.UniqueID + "','Select$" + gvKoiHist.Rows.Count.ToString + "');")
                Dim int As Integer = gvKoiHist.Rows.Count / 2
                Dim dob As Double = gvKoiHist.Rows.Count / 2

                If (dob.Equals(int)) Then
                    e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
                Else
                    e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
                End If

            End If

            If (e.Row.RowType = DataControlRowType.Header) Then
                Dim oGridView As New GridView
                Dim oGridViewRow As GridViewRow = New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)
                Dim oTableCell As TableCell = New TableCell()


                oTableCell.Text = "Daily Total"
                oTableCell.ColumnSpan = 5
                oTableCell.CssClass = "grid_header"
                oTableCell.ForeColor = Drawing.Color.FloralWhite
                oGridViewRow.Cells.Add(oTableCell)

                oTableCell = New TableCell()
                oTableCell.Text = "Month To Date"
                oTableCell.CssClass = "grid_header"
                oTableCell.ColumnSpan = 5
                oTableCell.ForeColor = Drawing.Color.FloralWhite
                oGridViewRow.Cells.Add(oTableCell)

                oTableCell = New TableCell()
                oTableCell.Text = "Last Month"
                oTableCell.ColumnSpan = 5
                oTableCell.ForeColor = Drawing.Color.FloralWhite
                oTableCell.CssClass = "grid_header"
                oGridViewRow.Cells.Add(oTableCell)

                gvKoiHist.Controls(0).Controls.AddAt(0, oGridViewRow)
            End If
        Catch ex As Exception
            logger.Error(ex.Message)
        End Try
       


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
        Catch ex As Exception
            logger.Error(ex.Message)
        End Try

    End Sub

End Class
