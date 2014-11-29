Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Maintenance_HQInfo
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
            bindData()
        End If


    End Sub

    Private Sub bindData()
        Dim searchModel As New CPM.HQInfoEntity
        Dim HQInfoDao As New CPM.HQInfoDAO
        Dim sqlmap As New SQLMap
        Dim dt As DataTable

        Try


            Dim strSQL As String = sqlmap.getMappedStatement("SetupMstr/Search-HQInfo", searchModel)
            If ViewState("strSQL") Is Nothing Then
                ViewState("strSQL") = strSQL
            End If

            dt = dm.execTable(strSQL)

            If dt.Rows.Count > 0 Then
                txtCompanyName.Text = dt.Rows(0).Item(HQInfoDao.COLUMN_CompanyName)
                txtCompanyNo.Text = dt.Rows(0).Item(HQInfoDao.COLUMN_CompanyNo)
                txtAddress1.Text = dt.Rows(0).Item(HQInfoDao.COLUMN_Address1)
                txtAddress2.Text = dt.Rows(0).Item(HQInfoDao.COLUMN_Address2)
                txtAddress3.Text = dt.Rows(0).Item(HQInfoDao.COLUMN_Address3)
                txtPostCode.Text = dt.Rows(0).Item(HQInfoDao.COLUMN_PostCode)
                ddState.SelectedValue = dt.Rows(0).Item(HQInfoDao.COLUMN_State)
                txtTelephone.Text = dt.Rows(0).Item(HQInfoDao.COLUMN_Telephone)
                txtFax.Text = dt.Rows(0).Item(HQInfoDao.COLUMN_Fax)
                txtEmail.Text = dt.Rows(0).Item(HQInfoDao.COLUMN_Email)
                txtUrl.Text = dt.Rows(0).Item(HQInfoDao.COLUMN_Url)
                txtRemark.Text = dt.Rows(0).Item(HQInfoDao.COLUMN_Remark)
                txtTaxAmount.Text = dt.Rows(0).Item(HQInfoDao.COLUMN_Tax)
                txtTaxDesc.Text = dt.Rows(0).Item(HQInfoDao.COLUMN_TaxDescription)
            End If

        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            HQInfoDao = Nothing
            searchModel = Nothing
            dt = Nothing

        End Try

    End Sub


    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        lblmsg.Text = ""

        If txtEmail.Text <> "" Then
            If Not Utility.Tools.EmailAddressCheck(Trim(txtEmail.Text)) Then
                lblmsg.Text = "Invalid email address."
                txtEmail.Focus()
                Exit Sub
            End If
        End If

        If Trim(txtTaxAmount.Text) = "" Then
            lblmsg.Text = "Please input a Tax Amount"
            txtTaxAmount.Focus()
            Exit Sub
        End If

        If txtTaxAmount.Text <> "" Then
            If Not Utility.Tools.NumericValidation(txtTaxAmount.Text) Then
                lblmsg.Text = "Please input numeric value for the Tax Amount"
                txtTaxAmount.Focus()
                Exit Sub
            End If
            If Trim(txtTaxDesc.Text) = "" Then
                lblmsg.Text = "Please input the Tax description"
                txtTaxDesc.Focus()
                Exit Sub
            End If
        End If



        If Not Page.IsValid Then
            Exit Sub
        End If


        Dim sqlmap As New SQLMap
        Dim strSQL As String = ""
        Dim searchModel As New CPM.CodeMstrEntity

        cn = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("path"))
        If Not cn.State = ConnectionState.Open Then
            cn.Open()
        End If
        trans = cn.BeginTransaction


        Try

            UpdateRecord(cn, trans)
            trans.Commit()
            bindData()
            lblmsg.Text = ConstantGlobal.Record_Updated


        Catch ex As Exception
            lblmsg.Text = ex.Message
            trans.Rollback()
        Finally
            trans.Dispose()
            cn.Close()
        End Try
    End Sub

    Protected Sub UpdateRecord(ByVal cn As SqlConnection, ByVal trans As SqlTransaction)

        Dim searchModel As New CPM.HQInfoEntity
        Dim HQInfoEnt As New CPM.HQInfoEntity()
        Dim HQInfoDao As New CPM.HQInfoDAO()
        Dim sqlmap As New SQLMap
        Dim dt As New DataTable

        Dim strSQL As String = sqlmap.getMappedStatement("SetupMstr/Search-HQInfo", searchModel)
        Try
            If ViewState("strSQL") Is Nothing Then
                ViewState("strSQL") = strSQL
            End If


            dt = dm.execTableInTrans(strSQL, cn, trans)

            If dt.Rows.Count > 0 Then
                HQInfoEnt.setHQInfoId(dt.Rows(0).Item(HQInfoDao.COLUMN_HQInfoID))
                HQInfoEnt.setCompanyName(Trim(txtCompanyName.Text.ToUpper))
                HQInfoEnt.setCompanyNo(Trim(txtCompanyNo.Text.ToUpper))
                HQInfoEnt.setAddress1(Trim(txtAddress1.Text.ToUpper))
                HQInfoEnt.setAddress2(Trim(txtAddress2.Text.ToUpper))
                HQInfoEnt.setAddress3(Trim(txtAddress3.Text.ToUpper))
                HQInfoEnt.setPostCode(Trim(txtPostCode.Text.ToUpper))
                HQInfoEnt.setState(ddState.SelectedValue)
                HQInfoEnt.setTelephone(Trim(txtTelephone.Text.ToUpper))
                HQInfoEnt.setFax(Trim(txtFax.Text.ToUpper))
                HQInfoEnt.setUrl(Trim(txtUrl.Text.ToUpper))
                If Trim(txtEmail.Text) <> "" Then
                    If Not Utility.Tools.EmailAddressCheck(txtEmail.Text) Then
                        lblmsg.Text = "Invalid Email Address."
                        Exit Sub
                    Else
                        HQInfoEnt.setEmail(Trim(txtEmail.Text.ToUpper))
                    End If
                End If
                HQInfoEnt.setRemark(Trim(txtRemark.Text))
                HQInfoEnt.setLastUpdatedBy(lp.getUserMstrId)
                HQInfoEnt.setLastUpdatedDatetime(Now)
                HQInfoEnt.setTax(txtTaxAmount.Text)
                HQInfoEnt.setTaxDescription(txtTaxDesc.Text.ToUpper)
                HQInfoDao.updateDB(HQInfoEnt, cn, trans)
            Else
                HQInfoEnt.setCompanyName(Trim(txtCompanyName.Text.ToUpper))
                HQInfoEnt.setCompanyNo(Trim(txtCompanyNo.Text.ToUpper))
                HQInfoEnt.setAddress1(Trim(txtAddress1.Text.ToUpper))
                HQInfoEnt.setAddress2(Trim(txtAddress2.Text.ToUpper))
                HQInfoEnt.setAddress3(Trim(txtAddress3.Text.ToUpper))
                HQInfoEnt.setPostCode(Trim(txtPostCode.Text.ToUpper))
                HQInfoEnt.setState(ddState.SelectedValue)
                HQInfoEnt.setTelephone(Trim(txtTelephone.Text.ToUpper))
                HQInfoEnt.setFax(Trim(txtFax.Text.ToUpper))
                If Trim(txtEmail.Text) <> "" Then
                    If Not Utility.Tools.EmailAddressCheck(txtEmail.Text) Then
                        lblmsg.Text = "Invalid Email Address."
                        Exit Sub
                    Else
                        HQInfoEnt.setEmail(Trim(txtEmail.Text.ToUpper))
                    End If
                End If
                HQInfoEnt.setUrl(Trim(txtUrl.Text.ToUpper))
                HQInfoEnt.setRemark(Trim(txtRemark.Text))
                HQInfoEnt.setLastUpdatedBy(lp.getUserMstrId)
                HQInfoEnt.setLastUpdatedDatetime(Now)
                HQInfoEnt.setTax(txtTaxAmount.Text)
                HQInfoEnt.setTaxDescription(txtTaxDesc.Text.ToUpper)
                HQInfoDao.insertDB(HQInfoEnt, cn, trans)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            HQInfoDao = Nothing
            HQInfoEnt = Nothing
            dt = Nothing

        End Try

       
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub
End Class
