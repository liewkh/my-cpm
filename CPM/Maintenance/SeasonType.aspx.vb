Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports System.Math

Partial Class Maintenance_SeasonType
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
            dsLocation.SelectCommand = sql
            dsLocation.DataBind()

            ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        End If

    End Sub

    Private Sub bindData()
        Dim searchModel As New SeasonTypeSearchModel
        Dim seasonTypeMstrDao As New CPM.SeasonTypeMstrDAO
        Dim sqlmap As New SQLMap

        Try

            searchModel.setSeasonType(Trim(txtSeasonType.Text.ToUpper))
            searchModel.setSeasonTypeDesc(Trim(txtSeasonTypeDesc.Text.ToUpper))


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

            If rbActiveYes.Checked Then
                searchModel.setActive(ConstantGlobal.Yes)
            Else
                searchModel.setActive(ConstantGlobal.No)
            End If


            Dim strSQL As String = sqlmap.getMappedStatement("SetupMstr/Search-SeasonTypeMstr", searchModel)

            ViewState("strSQL") = strSQL


            dsSeasonType.SelectCommand = ViewState("strSQL")
            gvSeasonType.DataBind()

            gvSeasonType.PageIndex = 0

            If gvSeasonType.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsSeasonType).ToString + " " + "Record Found"
            End If

        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            seasonTypeMstrDao = Nothing
            searchModel = Nothing
            sqlmap = Nothing

        End Try

    End Sub

    Private Sub clear()
        lblmsg.Text = ""
        lblRecCount.Text = ""
        txtSeasonType.Text = ""
        txtSeasonTypeDesc.Text = ""
        txtMonthlyAmount.Text = ""
        txtDeposit.Text = ""
        txtOther.Text = ""
        chkOther.Checked = False
        ddDuration.SelectedIndex = 0
        ddDuration.Enabled = True
        txtOther.Enabled = False
        ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        ddVehicle.SelectedIndex = 0
        rbActiveYes.Checked = True
        txtOther.CssClass = "textBoxSmallDisabled"
        gvSeasonType.DataSource = Nothing
        addMode()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        bindData()
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim strSQL As String = ""
        Dim msg As String = ""
        Dim isExist As Boolean = False

        If Not Page.IsValid Then
            Exit Sub
        End If

        If Not Utility.Tools.NumericValidation(txtMonthlyAmount.Text) Then
            lblmsg.Text = "Please enter numeric value for Monthly Amount."
            Exit Sub
        End If

        If Not Utility.Tools.NumericValidation(txtDeposit.Text) Then
            lblmsg.Text = "Please enter numeric value for Deposit Amount."
            Exit Sub
        End If

        If ddDuration.SelectedIndex = 0 And txtOther.Text = "" Then
            lblmsg.Text = "Duration is a required field."
            Exit Sub
        End If


        lblmsg.Text = ""

        cn = New SqlConnection(dm.getDBConn())
        If Not cn.State = ConnectionState.Open Then
            cn.Open()
        End If
        trans = cn.BeginTransaction

        Try
            'validate the name is not existed before insert within same locationid
            Dim searchModel As New SeasonTypeSearchModel
            Dim sqlmap As New SQLMap
            searchModel.setSeasonType(Trim(txtSeasonType.Text.ToUpper))
            searchModel.setLocationInfoId(ddLocation.SelectedValue)
            strSQL = sqlmap.getMappedStatement("SetupMstr/Search-SeasonTypeMstr", searchModel)

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
                'bindGridView()
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

        Dim activeInd As String
        Dim seasonTypeMstrEnt As New CPM.SeasonTypeMstrEntity
        Dim seasonTypeMstrDao As New CPM.SeasonTypeMstrDAO

        Try
            seasonTypeMstrEnt.setSeasonType(Trim(txtSeasonType.Text.ToUpper))
            seasonTypeMstrEnt.setSeasonTypeDesc(Trim(txtSeasonTypeDesc.Text.ToUpper))
            seasonTypeMstrEnt.setAmount(txtMonthlyAmount.Text)
            seasonTypeMstrEnt.setDeposit(txtDeposit.Text)
            seasonTypeMstrEnt.setLocationInfoId(ddLocation.SelectedValue)
            seasonTypeMstrEnt.setVehicleType(ddVehicle.SelectedValue)

            If chkOther.Checked = True Then
                seasonTypeMstrEnt.setSeasonDuration(txtOther.Text)
                seasonTypeMstrEnt.setOther(ConstantGlobal.Yes)
            Else
                seasonTypeMstrEnt.setSeasonDuration(ddDuration.SelectedValue)
                seasonTypeMstrEnt.setOther(ConstantGlobal.No)
            End If


            If rbActiveYes.Checked = True Then activeInd = ConstantGlobal.Yes Else activeInd = ConstantGlobal.No

            seasonTypeMstrEnt.setActive(activeInd)

            seasonTypeMstrEnt.setLastUpdatedDatetime(Now())
            seasonTypeMstrEnt.setLastUpdatedBy(lp.getUserMstrId)
            seasonTypeMstrDao.insertDB(seasonTypeMstrEnt, cn, trans)

        Catch ex As Exception
            Throw ex

        Finally
            seasonTypeMstrEnt = Nothing
            seasonTypeMstrDao = Nothing
        End Try
        
       
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        clear()
        ViewState("strSQL") = Nothing
        rbActiveNo.Checked = False
        rbActiveYes.Checked = True
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvSeasonType.Rows.Count
            ClientScript.RegisterForEventValidation(gvSeasonType.UniqueID, "Select$" + i.ToString)
        Next

        MyBase.Render(writer)
    End Sub

    Protected Sub gvSeasonType_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSeasonType.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvSeasonType.UniqueID + "','Select$" + gvSeasonType.Rows.Count.ToString + "');")
            'e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('gvSeasonType','Select$" + gvSeasonType.Rows.Count.ToString + "');")
            Dim int As Integer = gvSeasonType.Rows.Count / 2
            Dim dob As Double = gvSeasonType.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

        End If
    End Sub

    Protected Sub gvSeasonType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSeasonType.SelectedIndexChanged
        Dim seasonTypeMstrDao As New CPM.SeasonTypeMstrDAO

        Try
            lblmsg.Text = ""
            txtSeasonType.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvSeasonType.SelectedDataKey(seasonTypeMstrDao.COLUMN_SeasonType))
            txtSeasonTypeDesc.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvSeasonType.SelectedDataKey(seasonTypeMstrDao.COLUMN_SeasonTypeDesc))
            txtMonthlyAmount.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvSeasonType.SelectedDataKey(seasonTypeMstrDao.COLUMN_Amount))
            txtDeposit.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvSeasonType.SelectedDataKey(seasonTypeMstrDao.COLUMN_Deposit))
            ddLocation.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvSeasonType.SelectedDataKey(seasonTypeMstrDao.COLUMN_LocationInfoId))
            ddVehicle.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvSeasonType.SelectedDataKey(seasonTypeMstrDao.COLUMN_VehicleType))

            If (gvSeasonType.SelectedDataKey(seasonTypeMstrDao.COLUMN_Other)).Equals(ConstantGlobal.No) Then
                chkOther.Checked = False
                txtOther.Text = ""
                txtOther.Enabled = False
                txtOther.CssClass = "textBoxSmallDisabled"
                ddDuration.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(CInt(gvSeasonType.SelectedDataKey(seasonTypeMstrDao.COLUMN_SeasonDuration)))
                ddDuration.Enabled = True
            Else
                txtOther.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvSeasonType.SelectedDataKey(seasonTypeMstrDao.COLUMN_SeasonDuration))
                chkOther.Checked = True
                txtOther.Enabled = True
                txtOther.CssClass = "textBoxSmall"
                ddDuration.SelectedIndex = 0
                ddDuration.Enabled = False
            End If



            Dim vActiveInd As String = Utility.DataTypeUtils.parseHTMLSafeToString(gvSeasonType.SelectedDataKey(seasonTypeMstrDao.COLUMN_Active))

            If ConstantGlobal.No.Equals(vActiveInd) Then
                rbActiveYes.Checked = False
                rbActiveNo.Checked = True
            Else
                rbActiveYes.Checked = True
                rbActiveNo.Checked = False
            End If

            updateMode()

        Catch ex As Exception
            Throw ex
        Finally
            seasonTypeMstrDao = Nothing
        End Try

    End Sub

    Private Sub addMode()
        rbActiveNo.Checked = False
        rbActiveYes.Checked = True

        btnAdd.Enabled = True
        btnUpdate.Enabled = False
        btnSearch.Enabled = True

        txtSeasonType.ReadOnly = False
        txtSeasonType.CssClass = CSSEnum.TXTFIELD_2
    End Sub

    Private Sub updateMode()
        btnAdd.Enabled = False
        btnUpdate.Enabled = True
        btnSearch.Enabled = False

        txtSeasonType.ReadOnly = True
        txtSeasonType.CssClass = CSSEnum.TXTFIELD_2_DISABLED
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        lblmsg.Text = ""

        If Not Page.IsValid Then
            Exit Sub
        End If

        If Not Utility.Tools.NumericValidation(txtMonthlyAmount.Text) Then
            lblmsg.Text = "Please enter numeric value for Monthly Amount."
            Exit Sub
        End If

        If Not Utility.Tools.NumericValidation(txtDeposit.Text) Then
            lblmsg.Text = "Please enter numeric value for Deposit Amount."
            Exit Sub
        End If

        If ddDuration.SelectedIndex = 0 And txtOther.Text = "" Then
            lblmsg.Text = "Duration is a required field."
            Exit Sub
        End If



        Dim sqlmap As New SQLMap
        Dim strSQL As String = ""
        Dim searchModel As New CPM.PassCardMstrEntity

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

        Dim activeInd As String
        Dim seasonTypeMstrEnt As New CPM.SeasonTypeMstrEntity
        Dim seasonTypeMstrDao As New CPM.SeasonTypeMstrDAO

        Try
            seasonTypeMstrEnt.setSeasonTypeMstrId(gvSeasonType.SelectedDataKey(seasonTypeMstrDao.COLUMN_SeasonTypeMstrID))

            seasonTypeMstrEnt.setSeasonType(Trim(txtSeasonType.Text.ToUpper))
            seasonTypeMstrEnt.setSeasonTypeDesc(Trim(txtSeasonTypeDesc.Text.ToUpper))
            seasonTypeMstrEnt.setAmount(txtMonthlyAmount.Text)
            seasonTypeMstrEnt.setDeposit(txtDeposit.Text)
            seasonTypeMstrEnt.setLocationInfoId(ddLocation.SelectedValue)
            seasonTypeMstrEnt.setVehicleType(ddVehicle.SelectedValue)

            If chkOther.Checked = True Then
                seasonTypeMstrEnt.setSeasonDuration(txtOther.Text)
                seasonTypeMstrEnt.setOther(ConstantGlobal.Yes)
            Else
                seasonTypeMstrEnt.setSeasonDuration(ddDuration.SelectedValue)
                seasonTypeMstrEnt.setOther(ConstantGlobal.No)
            End If

            If rbActiveYes.Checked = True Then activeInd = ConstantGlobal.Yes Else activeInd = ConstantGlobal.No

            seasonTypeMstrEnt.setActive(activeInd)

            seasonTypeMstrEnt.setLastUpdatedDatetime(gvSeasonType.SelectedDataKey("LUDT"))

            seasonTypeMstrEnt.setLastUpdatedBy(lp.getUserMstrId)
            seasonTypeMstrDao.updateDB(seasonTypeMstrEnt, cn, trans)
        Catch ex As Exception
            Throw ex
        Finally
            seasonTypeMstrEnt = Nothing
            seasonTypeMstrDao = Nothing

        End Try



    End Sub

    Protected Sub gvSeasonType_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSeasonType.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindData()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub

    Protected Sub chkOther_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If chkOther.Checked = True Then
            ddDuration.SelectedIndex = 0
            ddDuration.Enabled = False
            txtOther.CssClass = "textBoxSmall"
            txtOther.Enabled = True
        Else
            ddDuration.Enabled = True
            txtOther.CssClass = "textBoxSmallDisabled"
            txtOther.Enabled = False
            txtOther.Text = ""
        End If

    End Sub
End Class
