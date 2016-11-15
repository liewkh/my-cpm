Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Report_BatchInvoice
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
                     "union select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' " & _
                     "union select el.locationinfoid,li.locationname,0 as seq from employeelocation el,locationinfo li " & _
                     "where el.locationinfoid = li.locationinfoid and el.employeemstrid = " & lp.getEmployeeMstrId & " order by seq,locationname"
            'dsLocation.SelectCommand = sql
            'dsLocation.DataBind()

            sql = "Select DEBTORID,NAME AS DEBTOR,0 as Seq From Debtor Where Status = '" & DebtorStatusEnum.ACTIVE & "'" & _
                  "And LocationInfoId = " & lp.getDefaultLocationInfoId & " And Category='C' UNION ALL SELECT CODEMSTRID,CODEDESC,SEQ FROM CODEMSTR WHERE CODECAT = 'ALL'" & _
                  "ORDER BY SEQ,DEBTOR"

            'dsDebtor.SelectCommand = sql
            'dsDebtor.DataBind()

            'ddLocation.SelectedValue = lp.getDefaultLocationInfoId


        End If

    End Sub


    Public Sub bindDropDown()
        Dim searchModel As New CPM.DebtorEntity
        Dim sqlmap As New SQLMap


        Try

            'If rbcompany.checked = True Then
            '    searchModel.setCategory(CategoryEnum.COMPANY)
            'Else
            '    searchModel.setCategory(CategoryEnum.INDIVIDUAL)
            'End If

            'searchModel.setStatus(DebtorStatusEnum.ACTIVE)
            'searchModel.setLocationInfoId(ddLocation.SelectedValue)

            'Dim strSQL As String = sqlmap.getMappedStatement("BillGeneration/Search-Debtor", searchModel)

            'ViewState("strSQL") = strSQL


            'dsDebtor.SelectCommand = ViewState("strSQL")
            'ddDebtor.DataBind()

        Catch ex As Exception
            logger.Debug(ex.Message)
            lblMsg.Text = ex.Message

        End Try

    End Sub


    Private Sub clear()
        lblmsg.Text = ""
        'ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        ddBatchNo.SelectedIndex = 0
        'ddDebtor.SelectedIndex = 0
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

    Protected Sub rbIndividual_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        bindDropDown()
    End Sub

    Protected Sub rbCompany_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        bindDropDown()
    End Sub

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim rptMgr As New ReportManager
        Dim mySql As String = ""
        Dim dt As New DataTable
        Dim companyName As String = ""
        Dim companyAddress As String = ""
        Dim tel As String = ""
        Dim fax As String = ""
        Dim searchModel As New CPM.DebtorAccountHeaderEntity
        Dim sqlmap As New SQLMap
        Dim hqInfoDao As New CPM.HQInfoDAO
        Dim companyNo As String = ""

        Try

            If ddMonth.SelectedIndex = 0 Then
                lblMsg.Text = "Please select the month."
                Exit Sub
            End If

            If ddBatchNo.SelectedIndex = -1 Then
                lblMsg.Text = "Please select the batch no."
                Exit Sub
            End If


            mySql = "SELECT COMPANYNAME,COMPANYNO,ADDRESS1,ADDRESS2,ADDRESS3,POSTCODE,TELEPHONE,FAX,(SELECT CODEDESC FROM CODEMSTR WHERE CODECAT='STA' AND CODEABBR= STATE) AS STATE FROM HQINFO"
            dt = dm.execTable(mySql)

            If dt.Rows.Count > 0 Then
                companyName = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_CompanyName)
                companyAddress = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address1) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address2) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address3) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_PostCode) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_State)
                tel = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Telephone)
                fax = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Fax)
                companyNo = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_CompanyNo)
            End If


            rptMgr.setReportName("BatchInvoice.Rpt")
            rptMgr.setParameterDiscrete("CompanyName", companyName)
            rptMgr.setParameterDiscrete("CompanyAddress", companyAddress)
            rptMgr.setParameterDiscrete("TelephoneNo", tel)
            rptMgr.setParameterDiscrete("Fax", ddMonth.SelectedIndex)
            rptMgr.setParameterDiscrete("PrintedBy", lp.getUserLoginId)
            rptMgr.setParameterDiscrete("batchNo", ddBatchNo.SelectedValue)
            rptMgr.setParameterDiscrete("companyNo", companyNo)


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

    Protected Sub ddMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        bindBatchNo()
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ddMonth.SelectedIndex = 0
        bindBatchNo()
    End Sub

    Private Sub bindBatchNo()
        Dim sql As String
        Dim dt As New DataTable

        Try
            sql = ""
            dsBatchNo.SelectCommand = sql
            dsBatchNo.DataBind()


            sql = "SELECT  batchNo FROM debtoraccountheader WHERE MONTH(InvoiceDate) = '" & ddMonth.SelectedValue & "'" & _
                  "GROUP BY YEAR(InvoiceDate),  datename(M,InvoiceDate),batchNo " & _
                  "ORDER BY YEAR(InvoiceDate),  datename(M,InvoiceDate),batchNo"
            dsBatchNo.SelectCommand = sql
            dsBatchNo.DataBind()

        Catch ex As Exception
            logger.Error(ex.Message)
            lblMsg.Text = ex.Message
        End Try
    End Sub
End Class
