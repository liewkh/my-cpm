Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports PopCalendarSpecialDay

Partial Class Maintenance_DebtorSOR
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

        'If Not Page.IsPostBack Then
        bindData()

        'End If

        Session.LCID = 2057

    End Sub



    Private Sub bindData()


        Dim searchModel As New CPM.SOREntity
        Dim sqlmap As New SQLMap

        Try

            searchModel.setDebtorId(paramDebtorId)

            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorSOR", searchModel)

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



   

    Protected Sub gvPassBay_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPassBay.SelectedIndexChanged
        Dim dpbDao As New CPM.DebtorPassBayDAO
        Dim gotChecked As Boolean = False

        Try

            lblmsg.Text = ""


            For Each row1 As GridViewRow In gvPassBay.Rows
                Dim chk1 As CheckBox
                chk1 = row1.FindControl("chkSelect")
                If Not chk1 Is Nothing Then
                    If chk1.Checked Then
                        gotChecked = True
                    End If
                End If
            Next


        Catch ex As Exception
            Throw ex
        Finally
            dpbDao = Nothing

        End Try

    End Sub



End Class

