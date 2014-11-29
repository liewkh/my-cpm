Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports PopCalendarSpecialDay

Partial Class Transaction_TicketAllocation
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        lp = Session("LoginProfile")

        Session.LCID = 2057

        If IsNothing(lp) Then
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("appPath") + "/login.aspx?login=expired")
        End If

        SpecialDays.AddHolidays(popCalendar1)
        SpecialDays.AddSpecialDays(popCalendar1)

        If Not Page.IsPostBack Then


            Dim sql As String = "select locationinfoid,locationName,0 as seq from locationinfo where locationinfoid = " & lp.getDefaultLocationInfoId & _
                                "union select -1 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' " & _
                                "union select el.locationinfoid,li.locationname,0 as seq from employeelocation el,locationinfo li " & _
                                "where el.locationinfoid = li.locationinfoid and el.employeemstrid = " & lp.getEmployeeMstrId & " order by seq,locationname"
            dsLocation.SelectCommand = sql
            dsLocation.DataBind()

            ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        End If

    End Sub

    Private Sub bindData()
        Dim searchModel As New CPM.TicketAllocationEntity       
        Dim sqlmap As New SQLMap

        Try

            If ddItem.SelectedIndex > 0 Then
                searchModel.setItem(ddItem.SelectedValue)
            End If

            searchModel.setSupplier(Trim(txtSupplier.Text))

            If ddLocation.SelectedIndex = 0 Then
                Dim a As ListItem
                Dim b As String = ""
                For Each a In ddLocation.Items
                    b = b + a.Value.ToString() + ","
                Next
                searchModel.setLocationInfoId(b)
            Else
                searchModel.setLocationInfoId(ddLocation.SelectedValue)
            End If


            Dim strSQL As String = sqlmap.getMappedStatement("TicketAllocation/Search-TicketAllocation", searchModel)

            ViewState("strSQL") = strSQL

            dsTA.SelectCommand = ViewState("strSQL")
            gvTA.DataBind()

            gvTA.PageIndex = 0

            If gvTA.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsTA).ToString + " " + "Record Found"
            End If

        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            searchModel = Nothing
            sqlmap = Nothing

        End Try

    End Sub

    Private Sub clear()
        lblmsg.Text = ""
        lblRecCount.Text = ""
        ddItem.SelectedIndex = 0
        ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        ddUnit.SelectedIndex = 0
        txtQtyAllocation.Text = ""
        txtStartNo.Text = ""
        txtEndNo.Text = ""
        txtUnitQty.Text = ""
        txtSupplier.Text = ""
        txtDoDate.Text = ""
        txtAllocationDate.Text = ""
        txtRemark.Text = ""
        gvTA.DataSource = Nothing
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

        If ddItem.SelectedIndex = 0 Then
            lblmsg.Text = "Item is a required field."
            Exit Sub
        End If

        cn = New SqlConnection(dm.getDBConn())
        If Not cn.State = ConnectionState.Open Then
            cn.Open()
        End If
        trans = cn.BeginTransaction

        Try
            
            InsertRecord(cn, trans)
            trans.Commit()
            clear()
            bindData()
            lblmsg.Text = ConstantGlobal.Record_Added
           

        Catch ex As Exception
            lblmsg.Text = ex.Message
            trans.Rollback()
        Finally
            trans.Dispose()
            cn.Close()
        End Try
    End Sub

    Protected Sub InsertRecord(ByVal cn As SqlConnection, ByVal trans As SqlTransaction)

        Dim taEnt As New CPM.TicketAllocationEntity
        Dim taDao As New CPM.TicketAllocationDAO

        Try
            taEnt.setItem(ddItem.SelectedValue)
            taEnt.setSupplier(Trim(txtSupplier.Text.ToUpper))
            If txtDoDate.Text <> "" Then
                taEnt.setDODate(txtDoDate.Text)
            End If

            taEnt.setLocationInfoId(ddLocation.SelectedValue)

            If txtAllocationDate.Text <> "" Then
                taEnt.setAllocationDate(txtAllocationDate.Text)
            End If

            If ddUnit.SelectedIndex > 0 Then
                taEnt.setUnit(ddUnit.SelectedValue)
            End If

            taEnt.setUnitQty(txtUnitQty.Text)
            taEnt.setQtyAllocation(txtQtyAllocation.Text)
            taEnt.setStartNo(txtStartNo.Text)
            taEnt.setEndNo(txtEndNo.Text)
            taEnt.setRemark(Trim(txtRemark.Text))

            taEnt.setLastUpdatedDatetime(Now())
            taEnt.setLastUpdatedBy(lp.getUserMstrId)
            taDao.insertDB(taEnt, cn, trans)


        Catch ex As Exception
            Throw ex
        Finally
            taEnt = Nothing
            taDao = Nothing

        End Try


    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        clear()
        ViewState("strSQL") = Nothing
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvTA.Rows.Count
            ClientScript.RegisterForEventValidation(gvTA.UniqueID, "Select$" + i.ToString)
        Next

        MyBase.Render(writer)
    End Sub

    Protected Sub gvTA_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTA.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvTA.UniqueID + "','Select$" + gvTA.Rows.Count.ToString + "');")
            'e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('gvPassCard','Select$" + gvPassCard.Rows.Count.ToString + "');")
            Dim int As Integer = gvTA.Rows.Count / 2
            Dim dob As Double = gvTA.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

        End If
    End Sub

    Protected Sub gvTA_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTA.SelectedIndexChanged
        Dim taDao As New CPM.TicketAllocationDAO
        Try

            lblmsg.Text = ""            
            ddItem.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvTA.SelectedDataKey(taDao.COLUMN_Item))
            txtSupplier.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvTA.SelectedDataKey(taDao.COLUMN_Supplier))
            If gvTA.SelectedDataKey(taDao.COLUMN_DODate).Equals(System.DBNull.Value) Then
                txtDoDate.Text = ""
            Else
                txtDoDate.Text = gvTA.SelectedDataKey(taDao.COLUMN_DODate)
            End If

            ddLocation.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvTA.SelectedDataKey(taDao.COLUMN_LocationInfoId))

            If gvTA.SelectedDataKey(taDao.COLUMN_AllocationDate).Equals(System.DBNull.Value) Then
                txtAllocationDate.Text = ""
            Else
                txtAllocationDate.Text = gvTA.SelectedDataKey(taDao.COLUMN_AllocationDate)
            End If

            ddUnit.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvTA.SelectedDataKey(taDao.COLUMN_Unit))
            txtUnitQty.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvTA.SelectedDataKey(taDao.COLUMN_UnitQty))
            txtQtyAllocation.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvTA.SelectedDataKey(taDao.COLUMN_QtyAllocation))
            txtStartNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvTA.SelectedDataKey(taDao.COLUMN_StartNo))
            txtEndNo.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvTA.SelectedDataKey(taDao.COLUMN_EndNo))
            txtRemark.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvTA.SelectedDataKey(taDao.COLUMN_Remark))


            updateMode()

        Catch ex As Exception
            lblmsg.Text = ex.Message
        Finally
            taDao = Nothing
        End Try

    End Sub

    Private Sub addMode()

        btnAdd.Enabled = True
        btnUpdate.Enabled = False
        btnSearch.Enabled = True

    End Sub

    Private Sub updateMode()
        btnAdd.Enabled = False
        btnUpdate.Enabled = True
        btnSearch.Enabled = False

    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        lblmsg.Text = ""

        If Not Page.IsValid Then
            Exit Sub
        End If

        If ddItem.SelectedIndex = 0 Then
            lblmsg.Text = "Item is a required field."
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

        Dim taEnt As New CPM.TicketAllocationEntity
        Dim taDao As New CPM.TicketAllocationDAO

        Try
            taEnt.setTicketAllocationId(gvTA.SelectedDataKey(taDao.COLUMN_TicketAllocationID))

            taEnt.setItem(ddItem.SelectedValue)
            taEnt.setSupplier(Trim(txtSupplier.Text.ToUpper))
            If txtDoDate.Text <> "" Then
                taEnt.setDODate(txtDoDate.Text)
            End If

            taEnt.setLocationInfoId(ddLocation.SelectedValue)

            If txtAllocationDate.Text <> "" Then
                taEnt.setAllocationDate(txtAllocationDate.Text)
            End If

            If ddUnit.SelectedIndex > 0 Then
                taEnt.setUnit(ddUnit.SelectedValue)
            End If

            taEnt.setUnitQty(txtUnitQty.Text)
            taEnt.setQtyAllocation(txtQtyAllocation.Text)
            taEnt.setStartNo(txtStartNo.Text)
            taEnt.setEndNo(txtEndNo.Text)
            taEnt.setRemark(Trim(txtRemark.Text))

            taEnt.setLastUpdatedDatetime(Now())
            taEnt.setLastUpdatedBy(lp.getUserMstrId)
            taDao.updateDB(taEnt, cn, trans)

        Catch ex As Exception
            Throw ex
        Finally
            taDao = Nothing
            taEnt = Nothing

        End Try



    End Sub


    Protected Sub gvTA_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTA.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindData()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub

End Class
