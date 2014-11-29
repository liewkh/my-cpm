Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Report_PasscardWCList

    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

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

             



        End If

    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
        hdPreview.Value = ""
    End Sub


    Protected Sub ddLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        bindDropDown()
    End Sub

    Public Sub bindDropDown()
        Dim sql As String
        Try

             


        Catch ex As Exception
            logger.Debug(ex.Message)
            lblMsg.Text = ex.Message

        End Try

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
        Dim debtorId As String = ""
        Dim searchModel As New CPM.DebtorAccountHeaderEntity
        Dim sqlmap As New SQLMap
        Dim hqInfoDao As New CPM.HQInfoDAO

        Try

            If ddLocation.SelectedIndex = 0 Then
                locationInfoId = "%"
            Else
                locationInfoId = ddLocation.SelectedValue
            End If

            

            mySql = "SELECT COMPANYNAME,COMPANYNO,ADDRESS1,ADDRESS2,ADDRESS3,POSTCODE,TELEPHONE,FAX,(SELECT CODEDESC FROM CODEMSTR WHERE CODECAT='STA' AND CODEABBR= STATE) AS STATE FROM HQINFO"
            dt = dm.execTable(mySql)

            If dt.Rows.Count > 0 Then
                companyName = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_CompanyName) & " " & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_CompanyNo)
                companyAddress = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address1) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address2) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address3) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_PostCode) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_State)
                tel = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Telephone)
                fax = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Fax)
            End If


            rptMgr.setReportName("PasscardWCList.Rpt")
            rptMgr.setParameterDiscrete("CompanyName", companyName)
            rptMgr.setParameterDiscrete("PrintedBy", lp.getUserLoginId)
            rptMgr.setParameterDiscrete("locationInfoId", locationInfoId)
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
        
    End Sub
End Class
