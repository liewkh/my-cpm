Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Enquiry_PaymentOutstandingEnq
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

            bindData()
            Dim sql As String = "select locationinfoid,locationName,0 as seq from locationinfo where locationinfoid = " & lp.getDefaultLocationInfoId & _
                                "union select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' " & _
                                "union select el.locationinfoid,li.locationname,0 as seq from employeelocation el,locationinfo li " & _
                                "where el.locationinfoid = li.locationinfoid and el.employeemstrid = " & lp.getEmployeeMstrId & " order by seq,locationname"
        End If

        Session.LCID = 2057

    End Sub

    Private Sub bindData()
        Dim searchModel As New DebtorSearchModel
        Dim passCardMstrDao As New CPM.PassCardMstrDAO
        Dim sqlmap As New SQLMap
        Dim strSQL As String
        Dim dt As New DataTable

        Try

            Dim sql As String = "select locationinfoid,locationName,0 as seq from locationinfo where locationinfoid = " & lp.getDefaultLocationInfoId & _
                              "union select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' " & _
                              "union select el.locationinfoid,li.locationname,0 as seq from employeelocation el,locationinfo li " & _
                              "where el.locationinfoid = li.locationinfoid and el.employeemstrid = " & lp.getEmployeeMstrId & " order by seq,locationname"

            dt = dm.execTable(sql)
            If dt.Rows.Count > 0 Then
                Dim b As String = ""
                Dim i As Integer

                For i = 0 To dt.Rows.Count - 1
                    b = b + dt.Rows(i).Item("locationinfoid").ToString() + ","
                Next
                b = b.Substring(1, b.Length - 1)
                searchModel.setLocationId(b)
            End If
            

            strSQL = sqlmap.getMappedStatement("Debtor/Debtor-PaymentOS", searchModel)

            ViewState("strSQL") = strSQL

            dsOS.SelectCommand = ViewState("strSQL")
            gvOSEnq.DataBind()

            gvOSEnq.PageIndex = 0

            If gvOSEnq.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsOS).ToString + " " + "Record Found"
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
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        bindData()
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        clear()
        ViewState("strSQL") = Nothing
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvOSEnq.Rows.Count
            ClientScript.RegisterForEventValidation(gvOSEnq.UniqueID, "Select$" + i.ToString)
        Next

        MyBase.Render(writer)
    End Sub

    Protected Sub gvPassCardEnq_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvOSEnq.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvOSEnq.UniqueID + "','Select$" + gvOSEnq.Rows.Count.ToString + "');")
            Dim int As Integer = gvOSEnq.Rows.Count / 2
            Dim dob As Double = gvOSEnq.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

            Dim detail As Button = CType(e.Row.FindControl("btnDetail"), Button)
            Dim a As String = ""
            Dim b As String = ""
            a = Convert.ToString(e.Row.DataItem("LOCATIONINFOID").ToString)
            b = Convert.ToString(e.Row.DataItem("LOCATIONNAME").ToString)

            detail.Attributes.Add("onclick", "javascript:window.open('../Enquiry/PaymentOSDetail.aspx?LocationInfoId=" + a.ToString + "&LocationName=" + b.ToString + "',menubar=1,resizable=1,width=800,height=600);")
        End If


    End Sub


    Protected Sub gvOSEnq_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvOSEnq.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindData()
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub

End Class
