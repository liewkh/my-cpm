Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Maintenance_BankSetup
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
    End Sub


    Public Sub bindGridView()
        Dim searchModel As New CPM.BankMstrEntity
        Dim sqlmap As New SQLMap



        searchModel.setBankCode(Trim(txtBankCode.Text.ToUpper))
        searchModel.setBankDesc(Trim(txtBankDesc.Text.ToUpper))


        If rbActiveYes.Checked Then
            searchModel.setActive(ConstantGlobal.Yes)
        Else
            searchModel.setActive(ConstantGlobal.No)
        End If

        Dim strSQL As String = sqlmap.getMappedStatement("SetupMstr/Search-BankMstr", searchModel)

        ViewState("strSQL") = strSQL


        dsBank.SelectCommand = ViewState("strSQL")
        gvBank.DataBind()

        gvBank.PageIndex = 0

        If gvBank.Rows.Count = 0 Then
            lblRecCount.Text = ConstantGlobal.No_Record_Found
        Else
            lblRecCount.Text = dm.getGridViewRecordCount(dsBank).ToString + " " + "Record Found"
        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        ViewState("strSQL") = Nothing
        bindGridView()
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        clear()
        ViewState("strSQL") = Nothing
        rbActiveNo.Checked = False
        rbActiveYes.Checked = True
    End Sub

    Private Sub clear()
        lblmsg.Text = ""
        lblRecCount.Text = ""
        txtBankCode.Text = ""
        txtBankDesc.Text = ""
        txtAccountNo.Text = ""
        txtAddress1.Text = ""
        txtAddress2.Text = ""
        txtAddress3.Text = ""
        txtTelNo.Text = ""
        txtFaxNo.Text = ""
        txtManagerName.Text = ""
        txtManagerHpNo.Text = ""
        txtOfficer.Text = ""
        txtOfficerHpNo.Text = ""
        rbActiveYes.Checked = True

        gvBank.DataSource = Nothing
        addMode()
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvBank.Rows.Count
            ClientScript.RegisterForEventValidation(gvBank.UniqueID, "Select$" + i.ToString)
        Next

        MyBase.Render(writer)
    End Sub

    Protected Sub gvBank_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBank.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            'e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvBank.UniqueID + "','Select$" + gvBank.Rows.Count.ToString + "');")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('gvBank','Select$" + gvBank.Rows.Count.ToString + "');")
            Dim int As Integer = gvBank.Rows.Count / 2
            Dim dob As Double = gvBank.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

        End If
    End Sub

    Protected Sub gvBank_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBank.SelectedIndexChanged

        Dim bankMstrDao As New CPM.BankMstrDAO

        Try
            lblmsg.Text = ""
            txtBankCode.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBank.SelectedDataKey(bankMstrDao.COLUMN_BankCode))
            txtBankDesc.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBank.SelectedDataKey(bankMstrDao.COLUMN_BankDesc))
            txtAccountNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBank.SelectedDataKey(bankMstrDao.COLUMN_AccountNo))
            txtAddress1.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBank.SelectedDataKey(bankMstrDao.COLUMN_Address1))
            txtAddress2.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBank.SelectedDataKey(bankMstrDao.COLUMN_Address2))
            txtAddress3.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBank.SelectedDataKey(bankMstrDao.COLUMN_Address3))
            txtTelNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBank.SelectedDataKey(bankMstrDao.COLUMN_TelNo))
            txtFaxNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBank.SelectedDataKey(bankMstrDao.COLUMN_FaxNo))
            txtManagerName.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBank.SelectedDataKey(bankMstrDao.COLUMN_Manager))
            txtManagerHpNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBank.SelectedDataKey(bankMstrDao.COLUMN_ManagerHpNo))
            txtOfficer.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBank.SelectedDataKey(bankMstrDao.COLUMN_Officer))
            txtOfficerHpNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBank.SelectedDataKey(bankMstrDao.COLUMN_OfficerHpNo))

            If Utility.DataTypeUtils.parseHTMLSafeToString(gvBank.SelectedDataKey(bankMstrDao.COLUMN_Active)).Equals(ConstantGlobal.Yes) Then
                rbActiveYes.Checked = True
                rbActiveNo.Checked = False
            Else
                rbActiveYes.Checked = False
                rbActiveNo.Checked = True
            End If

            updateMode()

        Catch ex As Exception
            Throw (ex)

        Finally

            bankMstrDao = Nothing

        End Try

    End Sub

    Private Sub addMode()
        rbActiveNo.Checked = False
        rbActiveYes.Checked = True

        btnAdd.Enabled = True
        btnUpdate.Enabled = False
        btnSearch.Enabled = True

        txtBankCode.ReadOnly = False
        txtBankCode.CssClass = CSSEnum.TXTFIELD_2
    End Sub

    Private Sub updateMode()
        btnAdd.Enabled = False
        btnUpdate.Enabled = True
        btnSearch.Enabled = False

        txtBankCode.ReadOnly = True
        txtBankCode.CssClass = CSSEnum.TXTFIELD_2_DISABLED
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim strSQL As String = ""
        Dim msg As String = ""
        Dim isExist As Boolean = False
        Dim isDescExist As Boolean = False

        If Not Page.IsValid Then
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
            Dim searchModel As New CPM.BankMstrEntity
            Dim sqlmap As New SQLMap
            searchModel.setBankCode(Trim(txtBankCode.Text.ToUpper))
            strSQL = sqlmap.getMappedStatement("SetupMstr/Search-BankMstr", searchModel)

            Dim dt As DataTable
            dt = dm.execTableInTrans(strSQL, cn, trans)
            If dt.Rows.Count > 0 Then
                isExist = True
            End If

            strSQL = ""
            dt.Dispose()


            If Not isExist Then
                InsertRecord(cn, trans)
                trans.Commit()
                clear()
                bindGridView()
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

        Dim VDefunctInd As String
        Dim bankMstrEnt As New CPM.BankMstrEntity
        Dim bankMstrDao As New CPM.BankMstrDAO

        Try

            bankMstrEnt.setBankCode(Trim(txtBankCode.Text.ToUpper))
            bankMstrEnt.setBankDesc(Trim(txtBankDesc.Text.ToUpper))
            bankMstrEnt.setAccountNo(Trim(txtAccountNo.Text.ToUpper))
            bankMstrEnt.setAddress1(Trim(txtAddress1.Text.ToUpper))
            bankMstrEnt.setAddress2(Trim(txtAddress2.Text.ToUpper))
            bankMstrEnt.setAddress3(Trim(txtAddress3.Text.ToUpper))
            bankMstrEnt.setTelNo(Trim(txtTelNo.Text.ToUpper))
            bankMstrEnt.setFaxNo(Trim(txtFaxNo.Text.ToUpper))
            bankMstrEnt.setManager(Trim(txtManagerName.Text.ToUpper))
            bankMstrEnt.setManagerHpNo(Trim(txtManagerHpNo.Text.ToUpper))
            bankMstrEnt.setOfficer(Trim(txtOfficer.Text.ToUpper))
            bankMstrEnt.setOfficerHpNo(Trim(txtOfficerHpNo.Text))

            If rbActiveYes.Checked = True Then VDefunctInd = ConstantGlobal.Yes Else VDefunctInd = ConstantGlobal.No
            bankMstrEnt.setActive(VDefunctInd)

            bankMstrEnt.setLastUpdatedDatetime(Now())
            bankMstrEnt.setLastUpdatedBy(lp.getUserMstrId)

        Catch ex As Exception
            Throw ex
        Finally
            bankMstrEnt = Nothing
            bankMstrDao = Nothing
        End Try



    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        lblmsg.Text = ""

        If Not Page.IsValid Then
            Exit Sub
        End If


        Dim sqlmap As New SQLMap
        Dim strSQL As String = ""
        Dim searchModel As New CPM.CodeMstrEntity

        cn = New SqlConnection(dm.getDBConn)
        If Not cn.State = ConnectionState.Open Then
            cn.Open()
        End If
        trans = cn.BeginTransaction


        Try

            UpdateRecord(cn, trans)
            trans.Commit()
            clear()
            Me.bindGridView()

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
        Dim VDefunctInd As String
        Dim bankMstrEnt As New CPM.BankMstrEntity
        Dim bankMstrDao As New CPM.BankMstrDAO

        Try
            bankMstrEnt.setBankMstrId(gvBank.SelectedDataKey(bankMstrDao.COLUMN_BankMstrID))
            bankMstrEnt.setBankCode(Trim(txtBankCode.Text.ToUpper))
            bankMstrEnt.setBankDesc(Trim(txtBankDesc.Text.ToUpper))
            bankMstrEnt.setAccountNo(Trim(txtAccountNo.Text.ToUpper))
            bankMstrEnt.setAddress1(Trim(txtAddress1.Text.ToUpper))
            bankMstrEnt.setAddress2(Trim(txtAddress2.Text.ToUpper))
            bankMstrEnt.setAddress3(Trim(txtAddress3.Text.ToUpper))
            bankMstrEnt.setTelNo(Trim(txtTelNo.Text.ToUpper))
            bankMstrEnt.setFaxNo(Trim(txtFaxNo.Text.ToUpper))
            bankMstrEnt.setManager(Trim(txtManagerName.Text.ToUpper))
            bankMstrEnt.setManagerHpNo(Trim(txtManagerHpNo.Text.ToUpper))
            bankMstrEnt.setOfficer(Trim(txtOfficer.Text.ToUpper))
            bankMstrEnt.setOfficerHpNo(Trim(txtOfficerHpNo.Text))

            If rbActiveYes.Checked = True Then VDefunctInd = ConstantGlobal.Yes Else VDefunctInd = ConstantGlobal.No
            bankMstrEnt.setActive(VDefunctInd)

            bankMstrEnt.setLastUpdatedBy(lp.getUserMstrId)
            bankMstrEnt.setLastUpdatedDatetime(gvBank.SelectedDataKey("LUDT"))
            bankMstrDao.updateDB(bankMstrEnt, cn, trans)

        Catch ex As Exception
            Throw ex
        Finally
            bankMstrEnt = Nothing
            bankMstrDao = Nothing

        End Try
        

    End Sub

    Protected Sub gvBank_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvBank.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindGridView()
        End If
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub
End Class
