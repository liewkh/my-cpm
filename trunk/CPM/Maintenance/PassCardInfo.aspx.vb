Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports PopCalendarSpecialDay

Partial Class Maintenance_PassCardInfo
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

            ddStatus.SelectedValue = PassCardStatusEnum._NEW

        End If

    End Sub

    Private Sub bindData()
        Dim searchModel As New PassCardSearchModel
        Dim passCardMstrDao As New CPM.PassCardMstrDAO
        Dim sqlmap As New SQLMap

        Try

            searchModel.setSerialNo(Trim(txtPassCardSN.Text.ToUpper))
            searchModel.setItemType(ddItemType.SelectedValue)
            searchModel.setStatus(ddStatus.SelectedValue)

            If ddLocation.SelectedIndex = 0 Then
                Dim a As ListItem
                Dim b As String = ""
                For Each a In ddLocation.Items
                    b = b + a.Value.ToString() + ","
                Next
                b = b.Substring(0, b.Length - 1)
                searchModel.setLocationId(b)
            Else
                searchModel.setLocationInfoId(ddLocation.SelectedValue)
            End If

            If rbActiveYes.Checked Then
                searchModel.setActive(ConstantGlobal.Yes)
            Else
                searchModel.setActive(ConstantGlobal.No)
            End If


            Dim strSQL As String = sqlmap.getMappedStatement("SetupMstr/Search-PassCardMstr", searchModel)

            ViewState("strSQL") = strSQL

            dsPassCard.SelectCommand = ViewState("strSQL")
            gvPassCard.DataBind()

            gvPassCard.PageIndex = 0

            If gvPassCard.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsPassCard).ToString + " " + "Record Found"
            End If


            'ddLocation.Items.FindByValue("6").Enabled = False

        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            passCardMstrDao = Nothing
            searchModel = Nothing
            sqlmap = Nothing

        End Try

    End Sub

    Private Sub clear()
        lblmsg.Text = ""
        lblRecCount.Text = ""
        txtPassCardSN.Text = ""
        ddItemType.SelectedIndex = 0
        txtSupplier.Text = ""
        txtDoDate.Text = ""
        txtAllocationDate.Text = ""
        txtWarranty.Text = ""
        txtRemark.Text = ""
        ddStatus.SelectedValue = PassCardStatusEnum._NEW
        rbActiveYes.Checked = True
        gvPassCard.DataSource = Nothing
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

        If ddStatus.SelectedIndex = 0 Then
            lblmsg.Text = "Status is a required field."
            Exit Sub
        End If

        If ddLocation.SelectedIndex = 0 Then
            lblmsg.Text = "Location is a required field."
            Exit Sub
        End If

        cn = New SqlConnection(dm.getDBConn())
        If Not cn.State = ConnectionState.Open Then
            cn.Open()
        End If
        trans = cn.BeginTransaction

        Try
            'validate the name is not existed before insert
            Dim searchModel As New PassCardSearchModel
            Dim sqlmap As New SQLMap
            searchModel.setSerialNo(Trim(txtPassCardSN.Text.ToUpper))

            If Trim(ddLocation.SelectedIndex) > 0 Then
                searchModel.setLocationInfoId(ddLocation.SelectedValue)
            End If



            strSQL = sqlmap.getMappedStatement("SetupMstr/Search-PassCardMstr", searchModel)

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
                bindData()
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
        Dim passCardMstrId As Long
        Dim passCardMstrEnt As New CPM.PassCardMstrEntity
        Dim passCardMstrDao As New CPM.PassCardMstrDAO
        Dim passCardHistEnt As New CPM.PassCardHistoryEntity
        Dim passCardHistDao As New CPM.PassCardHistoryDAO

        Try
            passCardMstrEnt.setSerialNo(Trim(txtPassCardSN.Text.ToUpper))
            passCardMstrEnt.setItemType(ddItemType.SelectedValue)
            passCardMstrEnt.setSupplier(Trim(txtSupplier.Text.ToUpper))
            passCardMstrEnt.setLocationInfoId(ddLocation.SelectedValue)

            If txtDoDate.Text <> "" Then
                passCardMstrEnt.setDeliveryDate(txtDoDate.Text)
            End If

            If txtAllocationDate.Text <> "" Then
                passCardMstrEnt.setAllocationDate(txtAllocationDate.Text)
            End If

            If Trim(txtWarranty.Text) <> "" Then
                passCardMstrEnt.setWarrantyPeriod(txtWarranty.Text)
            End If


            passCardMstrEnt.setRemark(Trim(txtRemark.Text))


            If rbActiveYes.Checked = True Then activeInd = ConstantGlobal.Yes Else activeInd = ConstantGlobal.No

            passCardMstrEnt.setActive(activeInd)
            passCardMstrEnt.setStatus(ddStatus.SelectedValue)

            If chkDepositPrinted.Checked = True Then
                passCardMstrEnt.setDepositPrint(ConstantGlobal.Yes)
            Else
                passCardMstrEnt.setDepositPrint(ConstantGlobal.No)
            End If

            passCardMstrEnt.setLastUpdatedDatetime(Now())
            passCardMstrEnt.setLastUpdatedBy(lp.getUserMstrId)
            passCardMstrId = passCardMstrDao.insertDB(passCardMstrEnt, cn, trans)

            'Create PassCardHistory
            passCardHistEnt.setStatus(ddStatus.SelectedValue)
            passCardHistEnt.setPassCardMstrId(passCardMstrId)
            passCardHistEnt.setLocationInfoId(ddLocation.SelectedValue)
            passCardHistEnt.setLastUpdatedBy(lp.getUserMstrId)
            passCardHistEnt.setLastUpdatedDatetime(Now)
            passCardHistDao.insertDB(passCardHistEnt, cn, trans)



        Catch ex As Exception
            Throw ex
        Finally
            passCardMstrEnt = Nothing
            passCardMstrDao = Nothing
            passCardHistEnt = Nothing
            passCardHistDao = Nothing

        End Try
        

    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        clear()
        ViewState("strSQL") = Nothing
        rbActiveNo.Checked = False
        rbActiveYes.Checked = True
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvPassCard.Rows.Count
            ClientScript.RegisterForEventValidation(gvPassCard.UniqueID, "Select$" + i.ToString)
        Next

        MyBase.Render(writer)
    End Sub

    Protected Sub gvPassCard_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPassCard.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvPassCard.UniqueID + "','Select$" + gvPassCard.Rows.Count.ToString + "');")
            'e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('gvPassCard','Select$" + gvPassCard.Rows.Count.ToString + "');")
            Dim int As Integer = gvPassCard.Rows.Count / 2
            Dim dob As Double = gvPassCard.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

        End If
    End Sub

    Protected Sub gvPassCard_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPassCard.SelectedIndexChanged
        Dim passCardMstrDao As New CPM.PassCardMstrDAO

        Try

            lblmsg.Text = ""
            txtPassCardSN.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassCard.SelectedDataKey(passCardMstrDao.COLUMN_SerialNo))
            ddItemType.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassCard.SelectedDataKey(passCardMstrDao.COLUMN_ItemType))
            ddLocation.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassCard.SelectedDataKey(passCardMstrDao.COLUMN_LocationInfoId))
            txtSupplier.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassCard.SelectedDataKey(passCardMstrDao.COLUMN_Supplier))
            'txtDoDate.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassCard.SelectedDataKey(passCardMstrDao.COLUMN_DeliveryDate))
            'txtAllocationDate.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassCard.SelectedDataKey(passCardMstrDao.COLUMN_AllocationDate))
            txtRemark.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassCard.SelectedDataKey(passCardMstrDao.COLUMN_Remark))
            ddStatus.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassCard.SelectedDataKey(passCardMstrDao.COLUMN_Status))
            txtWarranty.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassCard.SelectedDataKey(passCardMstrDao.COLUMN_WarrantyPeriod))

            Dim SQLDateNull As SqlDateTime = SqlDateTime.Null

            If gvPassCard.SelectedDataKey(passCardMstrDao.COLUMN_DeliveryDate).Equals(System.DBNull.Value) Then
                txtDoDate.Text = ""
            Else
                txtDoDate.Text = gvPassCard.SelectedDataKey(passCardMstrDao.COLUMN_DeliveryDate)
            End If


            If gvPassCard.SelectedDataKey(passCardMstrDao.COLUMN_AllocationDate).Equals(System.DBNull.Value) Then
                txtAllocationDate.Text = ""
            Else
                txtAllocationDate.Text = gvPassCard.SelectedDataKey(passCardMstrDao.COLUMN_AllocationDate)
            End If

            Dim vActiveInd As String = Utility.DataTypeUtils.parseHTMLSafeToString(gvPassCard.SelectedDataKey(passCardMstrDao.COLUMN_Active))

            If ConstantGlobal.No.Equals(vActiveInd) Then
                rbActiveYes.Checked = False
                rbActiveNo.Checked = True
            Else
                rbActiveYes.Checked = True
                rbActiveNo.Checked = False
            End If

            If gvPassCard.SelectedDataKey(passCardMstrDao.COLUMN_DepositPrint).Equals(ConstantGlobal.Yes) Then
                chkDepositPrinted.Checked = True
            Else
                chkDepositPrinted.Checked = False
            End If

            updateMode()

        Catch ex As Exception
            lblmsg.Text = ex.Message
        Finally
            passCardMstrDao = Nothing
        End Try

    End Sub

    Private Sub addMode()
        rbActiveNo.Checked = False
        rbActiveYes.Checked = True

        btnAdd.Enabled = True
        btnUpdate.Enabled = False
        btnSearch.Enabled = True

        txtPassCardSN.ReadOnly = False
        txtPassCardSN.CssClass = CSSEnum.TXTFIELD_2
    End Sub

    Private Sub updateMode()
        btnAdd.Enabled = False
        btnUpdate.Enabled = True
        btnSearch.Enabled = False

        txtPassCardSN.ReadOnly = True
        txtPassCardSN.CssClass = CSSEnum.TXTFIELD_2_DISABLED
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        lblmsg.Text = ""

        If Not Page.IsValid Then
            Exit Sub
        End If

        If ddStatus.SelectedIndex = 0 Then
            lblmsg.Text = "Status is a required field."
            Exit Sub
        End If

        If ddLocation.SelectedIndex = 0 Then
            lblmsg.Text = "Location is a required field."
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

        'To tackle for Defunct
        If rbActiveNo.Checked = True Then
            If ddStatus.SelectedValue = PassCardStatusEnum.INUSE Then
                lblmsg.Text = "Update Failed.The Item Status Under IN-USE."
                Exit Sub
            End If
        End If

        Try

            UpdateRecord(cn, trans)
            trans.Commit()
            clear()
            'Me.bindData()

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
        Dim passCardMstrEnt As New CPM.PassCardMstrEntity
        Dim passCardMstrDao As New CPM.PassCardMstrDAO
        Dim passCardHistEnt As New CPM.PassCardHistoryEntity
        Dim passCardHistDao As New CPM.PassCardHistoryDAO
        Dim sql As String
        Dim dt As New DataTable

        Try
            passCardMstrEnt.setPassCardMstrId(gvPassCard.SelectedDataKey(passCardMstrDao.COLUMN_PassCardMstrID))

            passCardMstrEnt.setSerialNo(Trim(txtPassCardSN.Text.ToUpper))
            passCardMstrEnt.setItemType(ddItemType.SelectedValue)
            passCardMstrEnt.setSupplier(Trim(txtSupplier.Text.ToUpper))
            passCardMstrEnt.setLocationInfoId(ddLocation.SelectedValue)

            Dim SQLDateNull As SqlDateTime = SqlDateTime.Null

            If (txtDoDate.Text <> String.Empty) Then
                passCardMstrEnt.setDeliveryDate(DateTime.Parse(txtDoDate.Text))
            Else

                passCardMstrEnt.setDeliveryDate(SQLDateNull)
            End If

            If (txtAllocationDate.Text <> String.Empty) Then
                passCardMstrEnt.setAllocationDate(DateTime.Parse(txtAllocationDate.Text))
            Else
                passCardMstrEnt.setAllocationDate(SQLDateNull)
            End If

            If Trim(txtWarranty.Text) <> "" Then
                passCardMstrEnt.setWarrantyPeriod(txtWarranty.Text)
            End If

            If chkDepositPrinted.Checked = True Then
                passCardMstrEnt.setDepositPrint(ConstantGlobal.Yes)
            Else
                passCardMstrEnt.setDepositPrint(ConstantGlobal.No)
            End If

            passCardMstrEnt.setRemark(Trim(txtRemark.Text))

            If rbActiveYes.Checked = True Then activeInd = ConstantGlobal.Yes Else activeInd = ConstantGlobal.No

            passCardMstrEnt.setActive(activeInd)
            passCardMstrEnt.setStatus(ddStatus.SelectedValue)

            passCardMstrEnt.setLastUpdatedDatetime(gvPassCard.SelectedDataKey("LUDT"))

            passCardMstrEnt.setLastUpdatedBy(lp.getUserMstrId)
            passCardMstrDao.updateDB(passCardMstrEnt, cn, trans)

            'To tackle for terminate
            If ddStatus.SelectedValue = PassCardStatusEnum.TERMINATE Then
                sql = "SELECT DEBTORPASSBAYID FROM DEBTORPASSBAY WHERE PASSCARDMSTRID = " & gvPassCard.SelectedDataKey(passCardMstrDao.COLUMN_PassCardMstrID) & _
                      " And Status = 'A'"
                dt = dm.execTableInTrans(sql, cn, trans)
                If dt.Rows.Count > 0 Then
                    sql = "UPDATE DEBTORPASSBAY SET LASTUPDATEDDATETIME = getDate(),STATUS = '" & PassCardStatusEnum.TERMINATE & "' WHERE DEBTORPASSBAYID = " & dt.Rows(0).Item(0)
                    dm.execTableInTrans(sql, cn, trans)
                End If

            ElseIf ddStatus.SelectedValue = PassCardStatusEnum.INUSE Then
                sql = "SELECT DEBTORPASSBAYID FROM DEBTORPASSBAY WHERE PASSCARDMSTRID = " & gvPassCard.SelectedDataKey(passCardMstrDao.COLUMN_PassCardMstrID) & _
                                      " And Status = '" & PassCardStatusEnum.TERMINATE & "'"
                dt = dm.execTableInTrans(sql, cn, trans)
                If dt.Rows.Count > 0 Then
                    sql = "UPDATE DEBTORPASSBAY SET LASTUPDATEDDATETIME = getDate(),STATUS = '" & PassCardStatusEnum.AVAILABLE & "' WHERE DEBTORPASSBAYID = " & dt.Rows(0).Item(0)
                    dm.execTableInTrans(sql, cn, trans)
                End If


            End If


            'Create PassCardHistory
            passCardHistEnt.setStatus(ddStatus.SelectedValue)
            passCardHistEnt.setPassCardMstrId(gvPassCard.SelectedDataKey(passCardMstrDao.COLUMN_PassCardMstrID))
            passCardHistEnt.setLocationInfoId(ddLocation.SelectedValue)
            passCardHistEnt.setLastUpdatedBy(lp.getUserMstrId)
            passCardHistEnt.setLastUpdatedDatetime(Now)
            passCardHistDao.insertDB(passCardHistEnt, cn, trans)

        Catch ex As Exception
            Throw ex
        Finally
            passCardMstrEnt = Nothing
            passCardMstrDao = Nothing
            passCardHistEnt = Nothing
            passCardHistDao = Nothing
            dt = Nothing

        End Try



    End Sub


    Public Function ConvertDATE(ByVal xValue As String) As Nullable(Of DateTime)
        Try
            ConvertDATE = Convert.ToDateTime(xValue)
        Catch ex As Exception
            ConvertDATE = SqlDateTime.Null.Value
        End Try
    End Function

    Protected Sub gvPassCard_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPassCard.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindData()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub

    Protected Sub ddLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If ddLocation.SelectedIndex = 0 Then
            ddStatus.SelectedIndex = 0
        End If
    End Sub

    Sub Validate_TextBox(ByVal Src As Object, ByVal Args As ServerValidateEventArgs)

        If Not IsNumeric(Args.Value) Then
            MyValidator.ErrorMessage = "Please enter a number for Warranty Period."
            Args.IsValid = False
        Else
            If Not Args.Value Mod 1 = 0 Then
                MyValidator.ErrorMessage = "Please enter an integer for Warranty Period."
                Args.IsValid = False
            End If
            If Args.Value < 0 Then
                MyValidator.ErrorMessage = "Please enter a positive integer for Warranty Period."
                Args.IsValid = False
            End If
            If Args.Value > 99 Then
                MyValidator.ErrorMessage = "Please enter a positive integer " & _
                                           "between 0 and 99"
                Args.IsValid = False
            End If
        End If

    End Sub
End Class
