Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports CrystalDecisions.Shared

Partial Class Report_BillingSummary
    Inherits System.Web.UI.Page


    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim myDate As DateTime
    Dim myYear As Integer



    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        lp = Session("LoginProfile")
        If IsNothing(lp) Then
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("appPath") + "/login.aspx?login=expired")
        End If

        If Not Page.IsPostBack Then


            Dim sql As String = "select locationinfoid,locationName,0 as seq from locationinfo where locationinfoid = " & lp.getDefaultLocationInfoId & _
                                "union select -1 as codemstrid,codedesc,seq from codemstr where codecat = 'ALL' " & _
                                "union select el.locationinfoid,li.locationname,0 as seq from employeelocation el,locationinfo li " & _
                                "where el.locationinfoid = li.locationinfoid and el.employeemstrid = " & lp.getEmployeeMstrId & " order by seq,locationname"
            dsLocation.SelectCommand = sql
            dsLocation.DataBind()

            ddLocation.SelectedValue = lp.getDefaultLocationInfoId

            myDate = DateTime.Now()
            Dim x As Integer
            myYear = myDate.Year
            x = myYear - 5
            For x = x To (x + 10)
                ddYear.Items.Add(x)
            Next x

            ddYear.Items.FindByText(myYear).Selected = True



        End If

    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
        hdPreview.Value = ""
    End Sub


    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim rptMgr As New ReportManager
        Dim mySql As String = ""
        Dim dt As New DataTable
        Dim companyName As String = ""
        Dim companyAddress As String = ""
        Dim tel As String = ""
        Dim fax As String = ""
        Dim locationInfoId As String = ""
        Dim debtorName As String = ""
        Dim status As String = ""
        Dim searchModel As New CPM.DebtorAccountHeaderEntity
        Dim sqlmap As New SQLMap
        Dim hqInfoDao As New CPM.HQInfoDAO
        Dim month1 As String = ""
        Dim month2 As String = ""
        Dim month3 As String = ""
        Dim monthName1 As String = ""
        Dim monthName2 As String = ""
        Dim monthName3 As String = ""


        Try

            If ddLocation.SelectedIndex = 0 Then
                locationInfoId = "%"
            Else
                locationInfoId = ddLocation.SelectedValue
            End If

            If ddQuarter.SelectedIndex = 0 Then
                lblMsg.Text = "Please select a Quarter."
                Exit Sub
            End If



            If ddQuarter.SelectedValue = "01" Then
                month1 = "1"
                monthName1 = "Jan"
                month2 = "2"
                monthName2 = "Feb"
                month3 = "3"
                monthName3 = "Mac"
            ElseIf ddQuarter.SelectedValue = "02" Then
                month1 = "4"
                monthName1 = "Apr"
                month2 = "5"
                monthName2 = "May"
                month3 = "6"
                monthName3 = "Jun"
            ElseIf ddQuarter.SelectedValue = "03" Then
                month1 = "7"
                monthName1 = "Jul"
                month2 = "8"
                monthName2 = "Aug"
                month3 = "9"
                monthName3 = "Sep"
            ElseIf ddQuarter.SelectedValue = "04" Then
                month1 = "10"
                monthName1 = "Oct"
                month2 = "11"
                monthName2 = "Nov"
                month3 = "12"
                monthName3 = "Dec"
            End If



            mySql = "SELECT COMPANYNAME,COMPANYNO,ADDRESS1,ADDRESS2,ADDRESS3,POSTCODE,TELEPHONE,FAX,(SELECT CODEDESC FROM CODEMSTR WHERE CODECAT='STA' AND CODEABBR= STATE) AS STATE FROM HQINFO"
            dt = dm.execTable(mySql)

            If dt.Rows.Count > 0 Then
                companyName = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_CompanyName) & " " & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_CompanyNo)
                companyAddress = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address1) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address2) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address3) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_PostCode) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_State)
                tel = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Telephone)
                fax = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Fax)
            End If


            rptMgr.setReportName("BillingSummary.Rpt")
            rptMgr.setParameterDiscrete("CompanyName", companyName)
            rptMgr.setParameterDiscrete("PrintedBy", lp.getUserLoginId)
            rptMgr.setParameterDiscrete("month1", month1)
            rptMgr.setParameterDiscrete("month2", month2)
            rptMgr.setParameterDiscrete("month3", month3)
            'rptMgr.setParameterDiscrete("monthName1", monthName1)
            'rptMgr.setParameterDiscrete("monthName2", monthName2)
            'rptMgr.setParameterDiscrete("monthName3", monthName3)
            rptMgr.setParameterDiscrete("locationInfoId", locationInfoId)
            rptMgr.setParameterDiscrete("year", ddYear.SelectedValue)
            rptMgr.setParameterDiscrete("Quarter", ddQuarter.SelectedItem.Text)
            rptMgr.setParameterDiscrete("employeeMstrId", lp.getEmployeeMstrId)

            rptMgr.Logon()

            hdPreview.Value = "1"

            'set reportManager to session
            Session("ReportManager") = rptMgr
            lblMsg.Text = ""




        Catch ex As Exception
            Throw ex

        Finally
            hqInfoDao = Nothing
            dt = Nothing
            rptMgr = Nothing


        End Try
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ddLocation.SelectedIndex = 0
        ddQuarter.SelectedIndex = 0
        ddYear.SelectedIndex = 0

    End Sub


End Class
