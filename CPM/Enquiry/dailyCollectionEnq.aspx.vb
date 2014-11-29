Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Enquiry_dailyCollectionEnq
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            lp = Session("LoginProfile")
            If IsNothing(lp) Then
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("appPath") + "/login.aspx?login=expired")
            End If
            If Not Page.IsPostBack Then


                Dim sql As String = "select locationinfoid,locationName,0 as seq from locationinfo where locationinfoid = " & lp.getDefaultLocationInfoId & _
                                    "union select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'ALL' " & _
                                    "union select el.locationinfoid,li.locationname,0 as seq from employeelocation el,locationinfo li " & _
                                    "where el.locationinfoid = li.locationinfoid and el.employeemstrid = " & lp.getEmployeeMstrId & " order by seq,locationname"
                dsLocation.SelectCommand = sql
                dsLocation.DataBind()

                ddLocation.SelectedValue = lp.getDefaultLocationInfoId
            End If

            Session.LCID = 2057

        Catch ex As Exception
            lblmsg.Text = ex.Message
        End Try

        
    End Sub

    Private Sub bindData()
        Dim searchModel As New dailyCollectionSearchModel
        Dim sqlmap As New SQLMap
        Dim strSQL As String


        Try

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

            searchModel.setTransactionDate(Format(CDate(txtDate.Text), "dd/MM/yyyy"))


            strSQL = sqlmap.getMappedStatement("DailyCollection/Search-DailyCollectionSales", searchModel)

            ViewState("strSQL") = strSQL

            dsDailyCollection.SelectCommand = ViewState("strSQL")
            gvDailyCollection.DataBind()

            gvDailyCollection.PageIndex = 0

            If gvDailyCollection.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsDailyCollection).ToString + " " + "Record Found"
            End If

        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            searchModel = Nothing
            sqlmap = Nothing

        End Try

    End Sub

    Private Sub clear()
        Try
            lblmsg.Text = ""
            lblRecCount.Text = ""
            txtDate.Text = ""
            ddLocation.SelectedValue = lp.getDefaultLocationInfoId
            gvDailyCollection.DataSource = Nothing
        Catch ex As Exception
            lblmsg.Text = ex.Message
        End Try
        
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        Try
            For i As Integer = 0 To gvDailyCollection.Rows.Count
                ClientScript.RegisterForEventValidation(gvDailyCollection.UniqueID, "Select$" + i.ToString)
            Next

            MyBase.Render(writer)
        Catch ex As Exception
            lblmsg.Text = ex.Message
        End Try
        
    End Sub

    Protected Sub gvPassCardEnq_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDailyCollection.RowDataBound

        Try
            If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
                e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
                e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvDailyCollection.UniqueID + "','Select$" + gvDailyCollection.Rows.Count.ToString + "');")
                Dim int As Integer = gvDailyCollection.Rows.Count / 2
                Dim dob As Double = gvDailyCollection.Rows.Count / 2

                If (dob.Equals(int)) Then
                    e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
                Else
                    e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
                End If

                'e.Row.Cells(0).Attributes.Add("title", "cssbody=[dvbdy1] cssheader=[dvhdr1] STYLE='BORDER: #558844 1px solid;WIDTH:200px;HEIGHT: 75px' body=[<B>Safety Condition Summary</B><br><B>Description</B><br> " + e.Row.DataItem("LOCATIONNAME").ToString + " <br><B>Line Condition</B><br> " + e.Row.DataItem("CASHIER").ToString + " <br> " + " <br><B>Unit Condition</B><br>" + e.Row.DataItem("APS").ToString + "<br>" + e.Row.DataItem("SEASON").ToString + "<br>" + "]")



                'e.Row.Cells(0).Attributes.Add("title", "header=[<b>" + e.Row.DataItem("LOCATIONNAME").ToString + "</b>] body=[<B><U>Cashier</U> RM </B>" + e.Row.DataItem("CASHIER").ToString & _
                '"<br><B><U>S.Motorcycle</U> RM</B> " + e.Row.DataItem("SEASONBIKE").ToString & _
                '"<br><B><U>S.Car</U> RM</B> " + e.Row.DataItem("SEASONCAR").ToString + "<br><B><U>APS</U> RM </B>" & _
                'e.Row.DataItem("APS").ToString + "<br><B><U>Misc</U> RM </B>" + e.Row.DataItem("MISC").ToString & _
                '"<br><B><U>Deposit</U> RM </B>" + e.Row.DataItem("DEPOSIT").ToString & _
                '"<br><br><B><U><I>Daily Total</I></U> RM </B>" + e.Row.DataItem("DAILYTOTAL").ToString & _
                '"<br><B><U><I>Month To Date</I></U> RM </B>" + e.Row.DataItem("MONTHTODATE").ToString & _
                '"<br><B><U><I>Last Month</I></U> RM </B>" + e.Row.DataItem("LASTMONTH").ToString & _
                '"] STYLE=BORDER: #558844 2px solid;WIDTH:200px;HEIGHT: 75px")


                e.Row.Cells(1).Attributes.Add("title", "header=[<b>" + e.Row.DataItem("LOCATIONNAME").ToString + "</b>] body=[" & _
                "<B>Cashier RM </B>" + e.Row.DataItem("CASHIERTOTAL").ToString & _
                "<br><B>Valet RM </B> " + e.Row.DataItem("VALETTOTAL").ToString & _
                "<br><B>Motorcycle RM </B> " + e.Row.DataItem("MOTORCYCLETOTAL").ToString & _
                "<br><B>APS RM </B>" + e.Row.DataItem("APSTOTAL").ToString & _
                "] STYLE=BORDER: #558844 2px solid;WIDTH:200px;HEIGHT: 75px")

                e.Row.Cells(2).Attributes.Add("title", "header=[<b>" + e.Row.DataItem("LOCATIONNAME").ToString + "</b>] body=[" & _
                "<B>Car RM </B>" + e.Row.DataItem("SEASONCARTOTAL").ToString & _
                "<br><B>Motorcycle RM </B> " + e.Row.DataItem("SEASONMOTORCYCLETOTAL").ToString & _
                "] STYLE=BORDER: #558844 2px solid;WIDTH:200px;HEIGHT: 75px")

                Dim history As Button = CType(e.Row.FindControl("btnKoi"), Button)
                Dim a As String = ""
                Dim ID As String = Convert.ToString(e.Row.DataItem("LOCATIONINFOID").ToString)

                a = "javascript:window.open('../Enquiry/KoiHist.aspx?LocationInfoId=" + ID + "&Date=" + txtDate.Text + "&LocationName=" + e.Row.DataItem("LOCATIONNAME").ToString + "',menubar=1,resizable=1,width=800,height=600);"
                history.Attributes.Add("OnClick", a)

            End If
        Catch ex As Exception
            lblmsg.Text = ex.Message
        End Try
     


    End Sub

    Protected Sub gvPassCardEnq_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDailyCollection.SelectedIndexChanged
        Dim dcDao As New CPM.DailyCollectionDAO

        Try

            lblmsg.Text = ""
            ddLocation.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvDailyCollection.SelectedDataKey(dcDao.COLUMN_LocationInfoId))

        Catch ex As Exception
            lblmsg.Text = ex.Message
        Finally
            dcDao = Nothing
        End Try

    End Sub

    Protected Sub gvPassCardEnq_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDailyCollection.RowCommand
        Try
            'For Sorting
            If Not e.CommandName.Equals("Select") Then
                bindData()
            End If
        Catch ex As Exception
            lblmsg.Text = ex.Message
        End Try
        

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Response.Expires = 0
            'Response.Cache.SetNoStore()
            'Response.AppendHeader("Pragma", "no-cache")

            'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
        Catch ex As Exception
            lblmsg.Text = ex.Message

        End Try

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Not Page.IsValid Then
                Exit Sub
            End If
            bindData()
        Catch ex As Exception
            lblmsg.Text = ex.Message
        End Try
       
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        clear()
        ViewState("strSQL") = Nothing
    End Sub

End Class
