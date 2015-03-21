Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports PopCalendarSpecialDay

Partial Class Maintenance_Debtor
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
            'SpecialDays.AddHolidays(popCalendar1)
            'SpecialDays.AddSpecialDays(popCalendar1)
            'txtCommencementDate.Text = Now.ToShortDateString
            Dim sql As String = "select locationinfoid,locationName,0 as seq from locationinfo where locationinfoid = " & lp.getDefaultLocationInfoId & _
                               "union select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' " & _
                               "union select el.locationinfoid,li.locationname,0 as seq from employeelocation el,locationinfo li " & _
                               "where el.locationinfoid = li.locationinfoid and el.employeemstrid = " & lp.getEmployeeMstrId & " order by seq,locationname"
            dsLocation.SelectCommand = sql
            dsLocation.DataBind()

            ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        End If




    End Sub

    Public Sub clear()

        lblmsg.Text = ""
        lblRecCount.Text = ""
        'rbCompany.Checked = True
        'rbIndividual.Checked = False
        ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        txtName.Text = ""
        txtIC.Text = ""
        txtTelOff.Text = ""
        txtTelMob.Text = ""
        txtTelHome.Text = ""
        txtTelNoOffice2.Text = ""
        txtEmployerName.Text = ""
        txtHomeAddress1.Text = ""
        txtHomeAddress2.Text = ""
        txtHomeAddress3.Text = ""
        txtPostCode.Text = ""
        txtCompanyName.Text = ""
        'txtUserName.Text = ""
        txtAddress1.Text = ""
        txtAddress2.Text = ""
        txtAddress3.Text = ""
        txtPostCode2.Text = ""
        txtTelNo.Text = ""
        txtContactPerson.Text = ""
        txtDesignation.Text = ""
        txtRemark.Text = ""
        ddStatus.SelectedIndex = 0
        gvDebtor.DataSource = Nothing
        lblRecCount.Text = ""
        hidDebtorIds.Value = ""
        hidLocationId.value = ""
        ddInvFreq.SelectedIndex = 0
        ddState1.SelectedIndex = -1
        ddState2.SelectedIndex = -1
        chkInitial.Checked = False
        'chkInitial.Enabled = True
        txtFaxNo.Text = ""
        txtEmail.Text = ""
        ddBank.SelectedIndex = 0
        txtBankAccountNo.Text = ""
        txtCompanyNo.Text = ""

        If rbCompany.Checked = True Then
            ddBank.SelectedIndex = 0
        Else
            ddBankInd.SelectedIndex = 0
        End If

        txtEmailInd.Text = ""
        txtBankAccountNoInd.Text = ""
        addMode()
    End Sub

    Private Sub addMode()

        btnAdd.Enabled = True
        btnUpdate.Enabled = False
        btnSearch.Enabled = True
        rbIndividual.Enabled = True
        rbCompany.Enabled = True
        btnPassCard.Enabled = False
        btnDebtorSOR.Enabled = False
        ddStatus.Enabled = True
        txtName.Enabled = True
        txtCompanyName.Enabled = True
        ddLocation.Enabled = True
    End Sub

    Private Sub updateMode()
        btnAdd.Enabled = False
        btnUpdate.Enabled = True
        btnSearch.Enabled = False
        rbIndividual.Enabled = False
        rbCompany.Enabled = False
        btnPassCard.Enabled = True
        btnDebtorSOR.Enabled = True
        ddStatus.Enabled = False
        txtName.Enabled = False
        txtCompanyName.Enabled = False
        ddLocation.Enabled = False

        If ddStatus.Text = DebtorStatusEnum.ACTIVE Then
            ddStatus.Enabled = True
        Else
            ddStatus.Enabled = False
        End If

        lblmsg.Text = ""
    End Sub

    Private Sub bindData()
        Dim searchModel As New DebtorSearchModel
        Dim debtorDao As New CPM.DebtorDAO
        Dim sqlmap As New SQLMap

        Try

            lblmsg.Text = ""

            If Trim(txtIC.Text) <> "" Then
                searchModel.setICNo(Trim(txtIC.Text.ToUpper))
            End If

            If rbCompany.Checked = True Then
                searchModel.setName(Trim(txtCompanyName.Text.ToUpper))
                searchModel.setCategory(CategoryEnum.COMPANY)
            Else
                searchModel.setName(Trim(txtName.Text.ToUpper))
                searchModel.setCategory(CategoryEnum.INDIVIDUAL)
            End If

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

            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-Debtor", searchModel)

            ViewState("strSQL") = strSQL


            dsDebtor.SelectCommand = ViewState("strSQL")
            gvDebtor.DataBind()

            gvDebtor.PageIndex = 0

            If gvDebtor.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsDebtor).ToString + " " + "Record Found"
                'chkInitial.Enabled = False
            End If

        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            debtorDao = Nothing
            searchModel = Nothing
            sqlmap = Nothing

        End Try

    End Sub

    Protected Sub rbIndividual_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbIndividual.CheckedChanged
        If rbIndividual.Checked = True Then
            divIndividual.Visible = True
            divCompany.Visible = False
        End If
        lblRecCount.Text = ""
        gvDebtor.DataSource = Nothing
    End Sub

    Protected Sub rbCompany_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbCompany.CheckedChanged
        If rbCompany.Checked = True Then
            divCompany.Visible = True
            divIndividual.Visible = False
        End If
        gvDebtor.DataSource = Nothing
        lblRecCount.Text = ""
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblmsg.Text = ""

        If Not Page.IsValid Then
            Exit Sub
        End If

        If rbCompany.Checked = True Then
            If Trim(txtCompanyName.Text) = "" Then
                lblmsg.Text = "Please enter a value for Company Name."
                Exit Sub
            End If


            If txtEmail.Text <> "" Then
                If Not Utility.Tools.EmailAddressCheck(Trim(txtEmail.Text)) Then
                    lblmsg.Text = "Invalid email address."
                    txtEmail.Focus()
                    Exit Sub
                End If
            End If

        Else

            If Trim(txtName.Text) = "" Then
                lblmsg.Text = "Please enter a value for Name."
                Exit Sub
            End If

            If Trim(txtIC.Text) = "" Then
                lblmsg.Text = "Please enter a value for IC/Passport No."
                Exit Sub
            End If

            If Trim(txtHomeAddress1.Text) = "" Then
                lblmsg.Text = "Please enter a value for Address."
                Exit Sub
            End If

            If txtEmailInd.Text <> "" Then
                If Not Utility.Tools.EmailAddressCheck(Trim(txtEmailInd.Text)) Then
                    lblmsg.Text = "Invalid email address."
                    txtEmailInd.Focus()
                    Exit Sub
                End If
            End If

        End If


        If ddInvFreq.SelectedIndex = 0 Then
            lblmsg.Text = "Invoicing Frequency is a mandatory field."
            Exit Sub
        End If

        Dim sqlmap As New SQLMap
        Dim strSQL As String = ""
        Dim searchModel As New CPM.DebtorEntity

        cn = New SqlConnection(dm.getDBConn)
        If Not cn.State = ConnectionState.Open Then
            cn.Open()
        End If
        trans = cn.BeginTransaction

        Try

            UpdateRecord(cn, trans)
            trans.Commit()
            clear()
            Me.bindData()

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

        Dim debtorEnt As New CPM.DebtorEntity
        Dim debtorDao As New CPM.DebtorDAO

        Try
            debtorEnt.setDebtorId(gvDebtor.SelectedDataKey(debtorDao.COLUMN_DebtorID))
            debtorEnt.setLocationInfoId(ddLocation.SelectedValue)
            debtorEnt.setRemark(Trim(txtRemark.Text))


            If ddInvFreq.SelectedIndex > 0 Then
                debtorEnt.setInvoicingFrequency(ddInvFreq.SelectedValue)
            Else
                debtorEnt.setInvoicingFrequency(0)
            End If

            If ddBank.SelectedIndex > 0 Then
                debtorEnt.setBankType(ddBank.SelectedValue)
            End If

            If ddBankInd.SelectedIndex > 0 Then
                debtorEnt.setBankType(ddBankInd.SelectedValue)
            End If

            If rbCompany.Checked = True Then
                debtorEnt.setCategory(CategoryEnum.COMPANY)
                debtorEnt.setName(Trim(txtCompanyName.Text.ToUpper))
                'debtorEnt.setUserName(Trim(txtUserName.Text.ToUpper))
                debtorEnt.setPostCode(Trim(txtPostCode2.Text.ToUpper))
                debtorEnt.setTelNoHome(Trim(txtTelNo.Text.ToUpper))
                debtorEnt.setTelNoOffice(Trim(txtTelNoOffice2.Text))
                debtorEnt.setContactPerson(Trim(txtContactPerson.Text.ToUpper))
                debtorEnt.setDesignation(Trim(txtDesignation.Text.ToUpper))
                debtorEnt.setAddress1(Trim(txtAddress1.Text.ToUpper))
                debtorEnt.setAddress2(Trim(txtAddress2.Text.ToUpper))
                debtorEnt.setAddress3(Trim(txtAddress3.Text.ToUpper))
                debtorEnt.setState(ddState2.SelectedValue)
                debtorEnt.setFaxNo(Trim(txtFaxNo.Text))
                debtorEnt.setEmailAddress(Trim(txtEmail.Text))                 
                debtorEnt.setBankAccNo(Trim(txtBankAccountNo.Text))
                debtorEnt.setCompanyNo(Trim(txtCompanyNo.Text))
            Else
                debtorEnt.setCategory(CategoryEnum.INDIVIDUAL)
                debtorEnt.setName(Trim(txtName.Text.ToUpper))
                debtorEnt.setICNo(Trim(txtIC.Text.ToUpper))
                debtorEnt.setTelNoMobile(Trim(txtTelMob.Text.ToUpper))
                debtorEnt.setTelNoOffice(Trim(txtTelOff.Text.ToUpper))
                debtorEnt.setTelNoHome(Trim(txtTelHome.Text.ToUpper))
                debtorEnt.setEmployerName(Trim(txtEmployerName.Text.ToUpper))
                debtorEnt.setPostCode(Trim(txtPostCode.Text.ToUpper))
                debtorEnt.setAddress1(Trim(txtHomeAddress1.Text.ToUpper))
                debtorEnt.setAddress2(Trim(txtHomeAddress2.Text.ToUpper))
                debtorEnt.setAddress3(Trim(txtHomeAddress3.Text.ToUpper))
                debtorEnt.setState(ddState1.SelectedValue)
                debtorEnt.setEmailAddress(Trim(txtEmailInd.Text))
                debtorEnt.setBankAccNo(Trim(txtBankAccountNoInd.Text))
            End If

            If chkInitial.Checked = True Then
                debtorEnt.setInitialHalfMonth(ConstantGlobal.Yes)
            Else
                debtorEnt.setInitialHalfMonth(ConstantGlobal.No)
            End If

            If ddStatus.SelectedIndex <> 0 Then
                debtorEnt.setStatus(ddStatus.SelectedValue)
            End If


            debtorEnt.setLastUpdatedDatetime(gvDebtor.SelectedDataKey("LUDT"))
            debtorEnt.setLastUpdatedBy(lp.getUserMstrId)
            debtorDao.updateDB(debtorEnt, cn, trans)

        Catch ex As Exception
            Throw ex
        Finally
            debtorEnt = Nothing
            debtorDao = Nothing

        End Try



    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strSQL As String = ""
        Dim msg As String = ""
        Dim isExist As Boolean = False

        If Not Page.IsValid Then
            Exit Sub
        End If

        If rbCompany.Checked = True Then
            If Trim(txtCompanyName.Text) = "" Then
                lblmsg.Text = "Please enter a value for Company Name."
                Exit Sub
            End If


            If txtEmail.Text <> "" Then
                If Not Utility.Tools.EmailAddressCheck(Trim(txtEmail.Text)) Then
                    lblmsg.Text = "Invalid email address."
                    txtEmail.Focus()
                    Exit Sub
                End If
            End If

        Else
            If Trim(txtName.Text) = "" Then
                lblmsg.Text = "Please enter a value for Name."
                Exit Sub
            End If

            If Trim(txtIC.Text) = "" Then
                lblmsg.Text = "Please enter a value for IC/Passport No."
                Exit Sub
            End If

            If Trim(txtHomeAddress1.Text) = "" Then
                lblmsg.Text = "Please enter a value for Address."
                Exit Sub
            End If

            If txtEmailInd.Text <> "" Then
                If Not Utility.Tools.EmailAddressCheck(Trim(txtEmailInd.Text)) Then
                    lblmsg.Text = "Invalid email address."
                    txtEmailInd.Focus()
                    Exit Sub
                End If
            End If

        End If

        If ddInvFreq.SelectedIndex = 0 Then
            lblmsg.Text = "Invoicing Frequency is a mandatory field."
            Exit Sub
        End If


        lblmsg.Text = ""

        cn = New SqlConnection(dm.getDBConn())
        If Not cn.State = ConnectionState.Open Then
            cn.Open()
        End If
        trans = cn.BeginTransaction

        Try

            'validate the name is not existed before insert
            Dim searchModel As New DebtorSearchModel
            Dim sqlmap As New SQLMap
            If rbCompany.Checked = True Then
                searchModel.setName(Trim(txtCompanyName.Text))
            Else
                searchModel.setName(Trim(txtName.Text))
            End If
            searchModel.setLocationInfoId(ddLocation.SelectedValue)

            strSQL = sqlmap.getMappedStatement("Debtor/Search-Debtor", searchModel)

            Dim dt As DataTable
            dt = dm.execTable(strSQL)
            If dt.Rows.Count > 0 Then
                isExist = True
            End If

            strSQL = ""
            dt.Dispose()

            If Not isExist Then
                InsertRecord(cn, trans)
                trans.Commit()
                clear()
                lblmsg.Text = ConstantGlobal.Record_Added
            Else
                Throw New ApplicationException(ConstantGlobal.Record_Already_Exist)
            End If

        Catch ex As Exception
            lblmsg.Text = ex.Message
            trans.Rollback()
        Finally
            trans.Dispose()
            cn.Close()
        End Try
    End Sub

    Protected Sub InsertRecord(ByVal cn As SqlConnection, ByVal trans As SqlTransaction)

        Dim debtorEnt As New CPM.DebtorEntity
        Dim debtorDao As New CPM.DebtorDAO

        Try


            debtorEnt.setLocationInfoId(ddLocation.SelectedValue)
            debtorEnt.setRemark(Trim(txtRemark.Text))
            debtorEnt.setStatus(DebtorStatusEnum.ACTIVE)
            debtorEnt.setBalance(0)

            If ddInvFreq.SelectedIndex > 0 Then
                debtorEnt.setInvoicingFrequency(ddInvFreq.SelectedValue)
            Else
                debtorEnt.setInvoicingFrequency(0)
            End If

            If ddBank.SelectedIndex > 0 Then
                debtorEnt.setBankType(ddBank.SelectedValue)
            End If

            If ddBankInd.SelectedIndex > 0 Then
                debtorEnt.setBankType(ddBankInd.SelectedValue)
            End If

            If chkInitial.Checked = True Then
                debtorEnt.setInitialHalfMonth(ConstantGlobal.Yes)
            Else
                debtorEnt.setInitialHalfMonth(ConstantGlobal.No)
            End If


            If rbCompany.Checked = True Then
                debtorEnt.setCategory(CategoryEnum.COMPANY)
                debtorEnt.setName(Trim(txtCompanyName.Text.ToUpper))
                'debtorEnt.setUserName(Trim(txtUserName.Text.ToUpper))                
                debtorEnt.setPostCode(Trim(txtPostCode2.Text.ToUpper))
                debtorEnt.setTelNoHome(Trim(txtTelNo.Text.ToUpper))
                debtorEnt.setTelNoOffice(Trim(txtTelNoOffice2.Text))
                debtorEnt.setContactPerson(Trim(txtContactPerson.Text.ToUpper))
                debtorEnt.setDesignation(Trim(txtDesignation.Text.ToUpper))
                debtorEnt.setAddress1(Trim(txtAddress1.Text.ToUpper))
                debtorEnt.setAddress2(Trim(txtAddress2.Text.ToUpper))
                debtorEnt.setAddress3(Trim(txtAddress3.Text.ToUpper))
                debtorEnt.setState(ddState2.SelectedValue)
                debtorEnt.setFaxNo(Trim(txtFaxNo.Text))
                debtorEnt.setEmailAddress(Trim(txtEmail.Text))
                debtorEnt.setBankAccNo(Trim(txtBankAccountNo.Text))
                debtorEnt.setCompanyNo(Trim(txtCompanyNo.Text))
            Else
                debtorEnt.setCategory(CategoryEnum.INDIVIDUAL)
                debtorEnt.setName(Trim(txtName.Text.ToUpper))
                debtorEnt.setICNo(Trim(txtIC.Text.ToUpper))
                debtorEnt.setTelNoMobile(Trim(txtTelMob.Text.ToUpper))
                debtorEnt.setTelNoOffice(Trim(txtTelOff.Text.ToUpper))
                debtorEnt.setTelNoHome(Trim(txtTelHome.Text.ToUpper))
                debtorEnt.setEmployerName(Trim(txtEmployerName.Text.ToUpper))
                debtorEnt.setPostCode(Trim(txtPostCode.Text.ToUpper))
                debtorEnt.setAddress1(Trim(txtHomeAddress1.Text.ToUpper))
                debtorEnt.setAddress2(Trim(txtHomeAddress2.Text.ToUpper))
                debtorEnt.setAddress3(Trim(txtHomeAddress3.Text.ToUpper))
                debtorEnt.setState(ddState1.SelectedValue)
                debtorEnt.setEmailAddress(Trim(txtEmailInd.Text))
                debtorEnt.setBankAccNo(Trim(txtBankAccountNoInd.Text))
            End If

            debtorEnt.setLastUpdatedDatetime(Now)
            debtorEnt.setLastUpdatedBy(lp.getUserMstrId)
            debtorDao.insertDB(debtorEnt, cn, trans)

        Catch ex As Exception
            Throw ex

        Finally
            debtorEnt = Nothing
            debtorDao = Nothing
        End Try


    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        bindData()
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        clear()
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvDebtor.Rows.Count
            ClientScript.RegisterForEventValidation(gvDebtor.UniqueID, "Select$" + i.ToString)
        Next

        Dim info As String = ""



        If rbIndividual.Checked = True Then
            info = "&debtorName=" & txtName.Text & "&userName=" & txtName.Text
        Else
            info = "&debtorName=" & txtCompanyName.Text
        End If

        If hidDebtorIds.Value <> "" Then
            btnPassCard.Attributes.Add("OnClick", "javascript:open_popupModal('../Transaction/DebtorPassCard.aspx?debtorId=" + hidDebtorIds.Value + "&locationInfoId=" & hidLocationId.Value + info + "','L');__doPostBack('btnSearch_Click','');")
            btnDebtorSOR.Attributes.Add("OnClick", "javascript:open_popupModal('../Transaction/DebtorSOR.aspx?debtorId=" + hidDebtorIds.Value + "&locationInfoId=" & hidLocationId.Value + info + "','L');__doPostBack('btnSearch_Click','');")
        End If

        'ClientScript.RegisterForEventValidation(lnkRefreshPassBay.UniqueID)

        MyBase.Render(writer)
    End Sub

    Protected Sub gvDebtor_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDebtor.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvDebtor.UniqueID + "','Select$" + gvDebtor.Rows.Count.ToString + "');")
            'e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('gvSeasonType','Select$" + gvSeasonType.Rows.Count.ToString + "');")
            Dim int As Integer = gvDebtor.Rows.Count / 2
            Dim dob As Double = gvDebtor.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

        End If
    End Sub

    Protected Sub gvDebtor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDebtor.SelectedIndexChanged
        Dim debtorDao As New CPM.DebtorDAO
        Dim searchModel As New CPM.PassCardMstrEntity
        Dim sqlmap As New SQLMap
        'Dim dtPassBayNo As New DataTable

        Try
            searchModel.setDebtorId(gvDebtor.SelectedDataKey(debtorDao.COLUMN_DebtorID))
            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorPassBayNo", searchModel)

            '            dtPassBayNo = dm.execTable(strSQL)
            'If dtPassBayNo.Rows.Count > 0 Then
            '    txtPassBay.Text = dtPassBayNo.Rows(0).Item(0).ToString
            'Else
            '    txtPassBay.Text = ""
            'End If

            txtRemark.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_Remark))
            ddLocation.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_LocationInfoId))
            ddStatus.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_Status))

            If Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_InvoicingFrequency)) <> 0 Then
                ddInvFreq.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_InvoicingFrequency))
            Else
                ddInvFreq.SelectedIndex = 0
            End If


            If gvDebtor.SelectedDataKey(debtorDao.COLUMN_Category).ToString.Equals(CategoryEnum.COMPANY) Then
                rbCompany.Checked = True
                rbIndividual.Checked = False
                txtCompanyName.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey("DEBTOR")).ToString
                '                txtUserName.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_UserName))
                txtAddress1.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_Address1))
                txtAddress2.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_Address2))
                txtAddress3.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_Address3))
                txtPostCode2.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_PostCode))
                txtTelNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_TelNoHome))
                txtTelNoOffice2.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_TelNoOffice))
                txtContactPerson.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_ContactPerson))
                txtDesignation.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_Designation))
                ddState2.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_State))
                ddBank.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_BankType))
                txtFaxNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_FaxNo))
                txtEmail.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_EmailAddress))
                txtBankAccountNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_BankAccNo))
                txtCompanyNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_CompanyNo))
            Else
                rbIndividual.Checked = True
                rbCompany.Checked = False
                txtPostCode.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_PostCode))
                txtName.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey("DEBTOR"))
                txtIC.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_ICNo))
                txtTelOff.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_TelNoOffice))
                txtTelMob.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_TelNoMobile))
                txtTelHome.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_TelNoHome))
                txtHomeAddress1.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_Address1))
                txtHomeAddress2.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_Address2))
                txtHomeAddress3.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_Address3))
                txtEmployerName.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_EmployerName))
                ddState1.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_State))
                txtEmailInd.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_EmailAddress))
                txtBankAccountNoInd.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_BankAccNo))
                ddBankInd.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_BankType))
            End If

            If gvDebtor.SelectedDataKey(debtorDao.COLUMN_InitialHalfMonth).Equals(ConstantGlobal.Yes) Then
                chkInitial.Checked = True
            Else
                chkInitial.Checked = False
            End If


            hidDebtorIds.Value = gvDebtor.SelectedDataKey(debtorDao.COLUMN_DebtorID)
            hidLocationId.Value = gvDebtor.SelectedDataKey(debtorDao.COLUMN_LocationInfoId)

            updateMode()            

        Catch ex As Exception
            lblmsg.Text = ex.Message
            logger.Error(ex.Message)
        Finally
            debtorDao = Nothing
            'dtPassBayNo = Nothing
        End Try

    End Sub

    Protected Sub gvDebtor_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDebtor.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindData()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub


    '    Protected Sub lnkRefreshPassBay_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '        Dim debtorDao As New CPM.DebtorDAO
    '        Dim searchModel As New CPM.PassCardMstrEntity
    '        Dim sqlmap As New SQLMap
    '        Dim dtPassBayNo As New DataTable

    '        Try
    '            searchModel.setDebtorId(hidDebtorIds.Value)
    '            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorPassBayNo", searchModel)

    '            dtPassBayNo = dm.execTable(strSQL)
    '            'If dtPassBayNo.Rows.Count > 0 Then
    '            '    txtPassBay.Text = dtPassBayNo.Rows(0).Item(0).ToString
    '            'Else
    '            '    txtPassBay.Text = ""
    '            'End If

    '            'popCalendar1.SetDateValue(hidDate.Value)
    '            txtCommencementDate.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_CommencementDate))

    '        Catch ex As Exception
    '            Throw ex
    '        Finally
    '            debtorDao = Nothing
    '            dtPassBayNo = Nothing
    '        End Try
    '    End Sub

    Protected Sub btnInvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim invMgr As New InvoiceManager


        Try

            'invMgr.createInvoice(gvDebtor.SelectedDataKey("DEBTORID"))

        Catch ex As Exception
            lblmsg.Text = ex.Message
            logger.Error(ex.Message)
        End Try

    End Sub

End Class