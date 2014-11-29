Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient


Partial Class Maintenance_EmployeeSetup
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
            ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        End If


    End Sub

    Public Sub clear()

        lblmsg.Text = ""
        lblRecCount.Text = ""
        txtEmployeeName.Text = ""
        txtEmployeeCode.Text = ""
        txtLoginId.Text = ""
        txtPassword.Text = ""
        ddAccessLevel.SelectedIndex = 0
        ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        txtTitle.Text = ""
        txtContactNo.Text = ""
        txtDepartment.Text = ""
        txtEmail.Text = ""
        txtPostCode.Text = ""
        txtAddress1.Text = ""
        txtAddress2.Text = ""
        txtAddress3.Text = ""
        ddState.SelectedIndex = 0
        txtRemark.Text = ""
        rbYes.Checked = True
        gvEmployees.DataSource = Nothing
        lblRecCount.Text = ""
        hidEmpMstrId.Value = ""
        txtMultipleLoc.Text = ""
        Session("tempLocation") = Nothing
        ddBranch.SelectedIndex = 0
        addMode()

    End Sub

    Private Sub addMode()

        btnAdd.Enabled = True
        btnUpdate.Enabled = False
        btnReset.Enabled = False
        btnSearch.Enabled = True

    End Sub

    Private Sub updateMode()
        btnAdd.Enabled = False
        btnUpdate.Enabled = True
        btnReset.Enabled = True
        btnSearch.Enabled = False
        lblmsg.Text = ""
    End Sub

    Private Sub bindData()

        Dim empSearchModel As New EmpSearchModel
        Dim sqlmap As New SQLMap

        Try

            lblmsg.Text = ""

            empSearchModel.setEmployeeName(Trim(txtEmployeeName.Text.ToUpper))
            empSearchModel.setUserId(Trim(txtLoginId.Text.ToUpper))
            empSearchModel.setBranchInfoId(ddBranch.SelectedValue)

            If rbYes.Checked = True Then
                empSearchModel.setStatus(ConstantGlobal.Yes)
            Else
                empSearchModel.setStatus(ConstantGlobal.No)
            End If

            Dim strSQL As String = sqlmap.getMappedStatement("UserMstr/Search-Employees", empSearchModel)

            ViewState("strSQL") = strSQL


            dsEmployee.SelectCommand = ViewState("strSQL")
            gvEmployees.DataBind()

            gvEmployees.PageIndex = 0

            If gvEmployees.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsEmployee).ToString + " " + "Record Found"
            End If

        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            empSearchModel = Nothing
            sqlmap = Nothing

        End Try

    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        lblmsg.Text = ""

        If Not Page.IsValid Then
            Exit Sub
        End If

        If ddBranch.SelectedIndex = 0 Then
            lblmsg.Text = "Branch is a required field."
            Exit Sub
        End If

        If ddAccessLevel.SelectedIndex = 0 Then
            lblmsg.Text = "Access Level is a required field."
            Exit Sub
        End If

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

        Dim empEnt As New CPM.EmployeeMstrEntity
        Dim empDao As New CPM.EmployeeMstrDAO
        Dim umEnt As New CPM.UserMstrEntity
        Dim umDao As New CPM.UserMstrDAO

        Try

            empEnt.setEmployeeMstrId(gvEmployees.SelectedDataKey(empDao.COLUMN_EmployeeMstrID))
            empEnt.setBranchInfoId(ddBranch.SelectedValue)
            empEnt.setEmployeeName(Trim(txtEmployeeName.Text.ToUpper))
            empEnt.setEmployeeCode(Trim(txtEmployeeCode.Text.ToUpper))
            umEnt.setUserMstrId((gvEmployees.SelectedDataKey(umDao.COLUMN_UserMstrID)))
            umEnt.setUserId(Trim(txtLoginId.Text.ToUpper))
            umEnt.setPassword(Trim(txtPassword.Text.ToUpper))
            umEnt.setAccessLevel(ddAccessLevel.SelectedValue)
            empEnt.setDefaultLocationInfoId(ddLocation.SelectedValue)
            empEnt.setTitle(Trim(txtTitle.Text.ToUpper))
            empEnt.setContactNo(Trim(txtContactNo.Text))
            empEnt.setDepartment(Trim(txtDepartment.Text.ToUpper))
            empEnt.setEmail(Trim(txtEmail.Text))
            empEnt.setAddress1(Trim(txtAddress1.Text.ToUpper))
            empEnt.setAddress2(Trim(txtAddress2.Text.ToUpper))
            empEnt.setAddress3(Trim(txtAddress3.Text.ToUpper))
            empEnt.setPostCode(Trim(txtPostCode.Text.ToUpper))
            empEnt.setState(ddState.SelectedValue)
            empEnt.setRemark(Trim(txtRemark.Text.ToUpper))

            If rbYes.Checked = True Then
                empEnt.setStatus(ConstantGlobal.Yes)
            Else
                empEnt.setStatus(ConstantGlobal.No)
            End If


            empEnt.setLastUpdatedDatetime(gvEmployees.SelectedDataKey("LUDT"))
            empEnt.setLastUpdatedBy(lp.getUserMstrId)

            umEnt.setLastUpdatedDatetime(Now)
            umEnt.setLastUpdatedBy(lp.getUserMstrId)

            empDao.updateDB(empEnt, cn, trans)
            umDao.updateDB(umEnt, cn, trans)


        Catch ex As Exception
            Throw ex
        Finally

            empEnt = Nothing
            empDao = Nothing
            umEnt = Nothing
            umDao = Nothing

        End Try



    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strSQL As String = ""
        Dim msg As String = ""
        Dim isExist As Boolean = False
        Dim searchModel As New EmpSearchModel
        Dim sqlmap As New SQLMap

        If Not Page.IsValid Then
            Exit Sub
        End If

        If ddBranch.SelectedIndex = 0 Then
            lblmsg.Text = "Branch is a required field."
            Exit Sub
        End If


        If ddAccessLevel.SelectedIndex = 0 Then
            lblmsg.Text = "Access Level is a required field."
            Exit Sub
        End If

        lblmsg.Text = ""

        cn = New SqlConnection(dm.getDBConn())
        If Not cn.State = ConnectionState.Open Then
            cn.Open()
        End If
        trans = cn.BeginTransaction

        Try


            'validate the user id is not existed before update
            searchModel.setUserId(Trim(txtLoginId.Text).ToUpper)
            strSQL = SQLMap.getMappedStatement("UserMstr/Search-CheckUserNameExist", searchModel)
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

        Dim empEnt As New CPM.EmployeeMstrEntity
        Dim empDao As New CPM.EmployeeMstrDAO
        Dim umEnt As New CPM.UserMstrEntity
        Dim umDao As New CPM.UserMstrDAO

        Try

            empEnt.setBranchInfoId(ddBranch.SelectedValue)
            empEnt.setEmployeeName(Trim(txtEmployeeName.Text.ToUpper))
            empEnt.setEmployeeCode(Trim(txtEmployeeCode.Text.ToUpper))
            empEnt.setDefaultLocationInfoId(ddLocation.SelectedValue)
            empEnt.setTitle(Trim(txtTitle.Text.ToUpper))
            empEnt.setContactNo(Trim(txtContactNo.Text))
            empEnt.setDepartment(Trim(txtDepartment.Text.ToUpper))
            empEnt.setEmail(Trim(txtEmail.Text))
            empEnt.setAddress1(Trim(txtAddress1.Text.ToUpper))
            empEnt.setAddress2(Trim(txtAddress2.Text.ToUpper))
            empEnt.setAddress3(Trim(txtAddress3.Text.ToUpper))
            empEnt.setPostCode(Trim(txtPostCode.Text.ToUpper))
            empEnt.setState(ddState.SelectedValue)
            empEnt.setRemark(Trim(txtRemark.Text.ToUpper))

            If rbYes.Checked = True Then
                empEnt.setStatus(ConstantGlobal.Yes)
            Else
                empEnt.setStatus(ConstantGlobal.No)
            End If

            empEnt.setLastUpdatedDatetime(Now)
            empEnt.setLastUpdatedBy(lp.getUserMstrId)

            Dim empKey As Long = empDao.insertDB(empEnt, cn, trans)
            logger.Debug("Insert EmployeeMstr : " & empKey)

            umEnt.setEmployeeMstrId(empKey)
            umEnt.setUserId(Trim(txtLoginId.Text.ToUpper))
            umEnt.setPassword(Trim(txtPassword.Text.ToUpper))
            umEnt.setAccessLevel(ddAccessLevel.SelectedValue)
            umEnt.setLastUpdatedDatetime(Now)
            umEnt.setLastUpdatedBy(lp.getUserMstrId)

            umDao.insertDB(umEnt, cn, trans)

            If Not Session("tempLocation") Is Nothing Then
                Dim empLocEnt As New CPM.EmployeeLocationEntity
                Dim empLocDao As New CPM.EmployeeLocationDAO
                Dim lb As New ListBox
                lb = Session("tempLocation")

                For Each item As ListItem In lb.Items
                    empLocEnt.setLocationInfoId(item.Value)
                    empLocEnt.setEmployeeMstrId(empKey)
                    empLocEnt.setLastUpdatedBy(lp.getUserMstrId)
                    empLocEnt.setLastUpdatedDatetime(Now)
                    empLocDao.insertDB(empLocEnt, cn, trans)
                Next

            End If


        Catch ex As Exception
            logger.Debug(ex.Message)
            Throw ex
        Finally

            empEnt = Nothing
            empDao = Nothing
            umEnt = Nothing
            umDao = Nothing

        End Try

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        bindData()
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        clear()
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvEmployees.Rows.Count
            ClientScript.RegisterForEventValidation(gvEmployees.UniqueID, "Select$" + i.ToString)
        Next

        imgEmpId.Attributes.Add("OnClick", "javascript:open_popupModal('../Maintenance/LocationList.aspx?employeeMstrId=" + hidEmpMstrId.Value + "','M');")

        ClientScript.RegisterForEventValidation(lnkRefreshMulLoc.UniqueID)

        MyBase.Render(writer)
    End Sub

    Protected Sub gvEmployees_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvEmployees.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvEmployees.UniqueID + "','Select$" + gvEmployees.Rows.Count.ToString + "');")
            Dim int As Integer = gvEmployees.Rows.Count / 2
            Dim dob As Double = gvEmployees.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

        End If
    End Sub

    Protected Sub gvEmployees_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvEmployees.SelectedIndexChanged
        Dim empDao As New CPM.EmployeeMstrDAO
        Dim umDao As New CPM.UserMstrDAO
        Dim searchModel As New EmpSearchModel
        Dim sqlmap As New SQLMap
        Dim dm As New DBManager
        Dim dtMulLoc As New DataTable

        Try

            Session("tempLocation") = Nothing

            searchModel.setEmployeeMstrId(gvEmployees.SelectedDataKey(empDao.COLUMN_EmployeeMstrID))
            Dim strSQL As String = sqlmap.getMappedStatement("UserMstr/Search-EmployeeMultipleLocation", searchModel)

            dtMulLoc = dm.execTable(strSQL)
            If dtMulLoc.Rows.Count > 0 Then
                txtMultipleLoc.Text = dtMulLoc.Rows(0).Item(0).ToString
            Else
                txtMultipleLoc.Text = ""
            End If

            txtEmployeeName.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvEmployees.SelectedDataKey(empDao.COLUMN_EmployeeName))
            txtEmployeeCode.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvEmployees.SelectedDataKey(empDao.COLUMN_EmployeeCode))
            txtLoginId.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvEmployees.SelectedDataKey(umDao.COLUMN_UserId))
            txtPassword.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvEmployees.SelectedDataKey(umDao.COLUMN_Password))
            ddAccessLevel.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvEmployees.SelectedDataKey(umDao.COLUMN_AccessLevel))
            ddLocation.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvEmployees.SelectedDataKey(empDao.COLUMN_DefaultLocationInfoId))
            txtTitle.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvEmployees.SelectedDataKey(empDao.COLUMN_Title))
            txtContactNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvEmployees.SelectedDataKey(empDao.COLUMN_ContactNo))
            txtDepartment.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvEmployees.SelectedDataKey(empDao.COLUMN_Department))
            txtEmail.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvEmployees.SelectedDataKey(empDao.COLUMN_Email))
            txtPostCode.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvEmployees.SelectedDataKey(empDao.COLUMN_PostCode))
            txtAddress1.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvEmployees.SelectedDataKey(empDao.COLUMN_Address1))
            txtAddress2.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvEmployees.SelectedDataKey(empDao.COLUMN_Address2))
            txtAddress3.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvEmployees.SelectedDataKey(empDao.COLUMN_Address3))
            ddState.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvEmployees.SelectedDataKey(empDao.COLUMN_State))
            txtRemark.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvEmployees.SelectedDataKey(empDao.COLUMN_Remark))
            ddBranch.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvEmployees.SelectedDataKey(empDao.COLUMN_BranchInfoId))

            hidEmpMstrId.Value = gvEmployees.SelectedDataKey(empDao.COLUMN_EmployeeMstrID)

            If gvEmployees.SelectedDataKey(empDao.COLUMN_Status).Equals(ConstantGlobal.Yes) Then
                rbYes.Checked = True
            Else
                rbYes.Checked = False
            End If

            updateMode()

        Catch ex As Exception
            Throw ex
        Finally
            empDao = Nothing
            umDao = Nothing
            dtMulLoc = Nothing
            dm = Nothing
        End Try

    End Sub

    Protected Sub gvEmployees_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvEmployees.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindData()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub

    Protected Sub lnkRefreshMulLoc_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim searchModel As New EmpSearchModel
        Dim sqlmap As New SQLMap
        Dim dtMulLoc As New DataTable

        Try

            If hidEmpMstrId.Value = "" Then
                'Get from session
                Dim lb As New ListBox
                Dim i As Integer
                lb = Session("tempLocation")
                txtMultipleLoc.Text = ""

                If Not lb Is Nothing Then
                    For i = 0 To lb.Items.Count - 1
                        txtMultipleLoc.Text = txtMultipleLoc.Text & lb.Items(i).Text + "| "
                    Next i
                End If
                
            Else
                searchModel.setEmployeeMstrId(hidEmpMstrId.Value)
                Dim strSQL As String = sqlmap.getMappedStatement("UserMstr/Search-EmployeeMultipleLocation", searchModel)

                dtMulLoc = dm.execTable(strSQL)
                If dtMulLoc.Rows.Count > 0 Then
                    txtMultipleLoc.Text = dtMulLoc.Rows(0).Item(0).ToString
                Else
                    txtMultipleLoc.Text = ""
                End If
            End If
            

        Catch ex As Exception
            Throw ex
        Finally
            searchModel = Nothing
            dtMulLoc = Nothing
        End Try
    End Sub


    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim umEnt As New CPM.UserMstrEntity
        Dim umDao As New CPM.UserMstrDAO
        Dim pDao As New CPM.ParameterDAO
        Dim sql As String
        Dim dt As DataTable

        Try

            If ddBranch.SelectedIndex = 0 Then
                lblmsg.Text = "Branch is a required field."
                Exit Sub
            End If

            If ddAccessLevel.SelectedIndex = 0 Then
                lblmsg.Text = "Access Level is a required field."
                Exit Sub
            End If

            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If
            trans = cn.BeginTransaction

            sql = "SELECT PARAMETERVALUE FROM PARAMETER WHERE PARAMETERNAME = '" & ParameterEnum.DEFAULTPASSWORD & "'"
            dt = dm.execTable(sql)

            If dt.Rows(0).Item(pDao.COLUMN_PARAMETERVALUE).ToString <> "" Then
                umEnt.setUserMstrId((gvEmployees.SelectedDataKey(umDao.COLUMN_UserMstrID)))
                umEnt.setPassword(dt.Rows(0).Item(pDao.COLUMN_PARAMETERVALUE).ToString)
            Else
                Throw New ApplicationException("No Parameter Setup For Default Password")
            End If

            umEnt.setLastUpdatedDatetime(Now)
            umEnt.setLastUpdatedBy(lp.getUserMstrId)
            umDao.updateDB(umEnt, cn, trans)
            trans.Commit()
            clear()
            lblmsg.Text = ConstantGlobal.Record_Updated


        Catch ex As Exception
            logger.Error(ex.Message)
            lblmsg.Text = ex.Message
            trans.Rollback()

        Finally

            trans.Dispose()
            cn.Close()
            umEnt = Nothing
            umDao = Nothing

        End Try

    End Sub
End Class
