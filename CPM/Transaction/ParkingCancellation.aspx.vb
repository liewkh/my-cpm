Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports PopCalendarSpecialDay

Partial Class Transaction_ParkingCancellation
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim manualReceipt As String = ""


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            Session.LCID = 2057

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

                SpecialDays.AddHolidays(popCalendar1)
                SpecialDays.AddSpecialDays(popCalendar1)

                txtCancellationDate.Text = Utility.DataTypeUtils.formatDateString(Now)
                txtProcessedBy.Text = lp.getUserName

            End If
            ddLocation.SelectedValue = lp.getDefaultLocationInfoId

            divSearch.Visible = True
            divInv.Visible = False

        Catch ex As Exception
            lblmsg.Text = ex.Message

        End Try




    End Sub

    Private Sub bindData()
        Dim searchModel As New DebtorSearchModel
        Dim passCardMstrDao As New CPM.DebtorDAO
        Dim sqlmap As New SQLMap

        Try

            lblmsg.Text = ""

            If rbCompany.Checked = True Then
                searchModel.setName(Trim(txtDebtorName.Text.ToUpper))
                searchModel.setCategory(CategoryEnum.COMPANY)
            Else
                searchModel.setName(Trim(txtDebtorName.Text.ToUpper))
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

            searchModel.setStatus(DebtorStatusEnum.ACTIVE)
            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-Debtor", searchModel)

            ViewState("strSQL") = strSQL


            dsDebtorEnq.SelectCommand = ViewState("strSQL")
            gvDebtorEnq.DataBind()

            gvDebtorEnq.PageIndex = 0

            If gvDebtorEnq.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsDebtorEnq).ToString + " " + "Record Found"
            End If


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
        ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        txtDebtorName.Text = ""
        txtUnused.Text = ""
        rbCompany.Checked = True
        rbIndividual.Checked = False        
        gvDebtorEnq.DataSource = Nothing

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        bindData()
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        clear()
        ViewState("strSQL") = Nothing
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvDebtorEnq.Rows.Count
            ClientScript.RegisterForEventValidation(gvDebtorEnq.UniqueID, "Select$" + i.ToString)
        Next

        MyBase.Render(writer)
    End Sub

    Protected Sub gvDebtorEnq_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDebtorEnq.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvDebtorEnq.UniqueID + "','Select$" + gvDebtorEnq.Rows.Count.ToString + "');")
            Dim int As Integer = gvDebtorEnq.Rows.Count / 2
            Dim dob As Double = gvDebtorEnq.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If
        End If

    End Sub


    Protected Sub gvDebtorEnq_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDebtorEnq.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindData()
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub

    Protected Sub gvDebtorEnq_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDebtorEnq.SelectedIndexChanged
        Dim debtorDao As New CPM.DebtorDAO

        Try

            hidDebtorId.Value = gvDebtorEnq.SelectedDataKey(debtorDao.COLUMN_DebtorID).ToString
            txtDebtorName.Text = gvDebtorEnq.SelectedDataKey("DEBTOR").ToString
            hidLocationInfoId.Value = gvDebtorEnq.SelectedDataKey(debtorDao.COLUMN_LocationInfoId).ToString
            DataMode()


        Catch ex As Exception
            logger.Error(ex.Message)
            lblmsg.Text = ex.Message
        Finally
            debtorDao = Nothing
        End Try

    End Sub

    Private Sub DataMode()
        bindPass()
        txtOutstanding.Text = 0.0
        txtUnused.Text = 0.0
        divSearch.Visible = False
        divInv.Visible = True
        txtDebtorName.ReadOnly = True
        txtDebtorName.CssClass = CSSEnum.TXTFIELD_3_DISABLED
        ddLocation.Enabled = False
        ddLocation.CssClass = CSSEnum.DROPDOWN_DISABLED
        rbCompany.Enabled = False
        rbIndividual.Enabled = False

    End Sub

    Private Sub SearchMode()
        divSearch.Visible = True
        divInv.Visible = False
        txtDebtorName.ReadOnly = False
        txtDebtorName.CssClass = CSSEnum.TXTFIELD_3_DISABLED
        ddLocation.Enabled = True
        ddLocation.CssClass = CSSEnum.DROPDOWN
        rbCompany.Enabled = True
        rbIndividual.Enabled = True

    End Sub

    Protected Sub btnDataBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        SearchMode()
        clearData()
    End Sub

    Private Sub clearData()
        ddReason.SelectedIndex = 0
        ddPassCondition.SelectedIndex = 0
        ddDeposit.selectedindex = 0
        txtCancellationDate.Text = Utility.DataTypeUtils.formatDateString(Now)
        lblPassDeposit.Text = ""
        txtOutstanding.Text = 0
        txtUnused.Text = 0
        txtRemark.Text = ""

    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim dpbcEnt As New CPM.DebtorPassBayNoCancelEntity
        Dim dpbcDao As New CPM.DebtorPassBayNoCancelDAO
        Dim pcEnt As New CPM.PassCardMstrEntity
        Dim pcDao As New CPM.PassCardMstrDAO
        Dim dpbEnt As New CPM.DebtorPassBayEntity
        Dim dpbDao As New CPM.DebtorPassBayDAO
        Dim pchEnt As New CPM.PassCardHistoryEntity
        Dim pchDao As New CPM.PassCardHistoryDAO
        Dim sql As String
        Dim dt As New DataTable
        Dim dpbId As String = ""
        Dim refundCutOfDay As Integer = 0

        Try

            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If
            trans = cn.BeginTransaction


           

            If Trim(txtCancellationDate.Text) = "" Then
                lblmsg.Text = "Cancellation Date is a required field."
                Exit Sub
            End If

            If Trim(txtEffectiveFrom.Text) = "" Then
                lblmsg.Text = "Effective Date is a required field."
                Exit Sub
            End If


            If ddReason.SelectedIndex = 0 Then
                lblmsg.Text = "Reason is a required field."
                Exit Sub
            End If

            If Not Page.IsValid Then
                Exit Sub
            End If

            sql = "SELECT RefundCutOffDate From LocationInfo Where LocationInfoId = " & hidLocationInfoId.Value
            dt = dm.execTable(sql)

            If dt.Rows.Count > 0 Then
                refundCutOfDay = IIf(dt.Rows(0).Item("RefundCutOffDate").Equals(System.DBNull.Value), 0, dt.Rows(0).Item("RefundCutOffDate"))
            End If



            sql = "SELECT * FROM DEBTORPASSBAY WHERE DEBTORID = " & hidDebtorId.Value & _
                  " AND STATUS = 'A' AND PASSCARDMSTRID = " & ddPass.SelectedValue
            dt = dm.execTableInTrans(sql, cn, trans)

            dpbId = dt.Rows(0).Item("DEBTORPASSBAYID")

            dpbEnt.setDebtorPassBayId(dpbId)
            dpbEnt.setStatus("C")
            dpbEnt.setLastUpdatedBy(lp.getUserMstrId)
            dpbEnt.setLastUpdatedDatetime(Now)
            dpbDao.updateDB(dpbEnt, cn, trans)


            ''Update Old PassCardMstr
            pcEnt.setPassCardMstrId(ddPass.SelectedValue)
            pcEnt.setLastUpdatedBy(lp.getUserMstrId)
            pcEnt.setLastUpdatedDatetime(Now)
            If ddPassCondition.SelectedValue = PassCardStatusEnum.LOST Then
                pcEnt.setStatus(PassCardStatusEnum.LOST)
            ElseIf ddPassCondition.SelectedValue = PassCardStatusEnum.SPOILT Then
                pcEnt.setStatus(PassCardStatusEnum.SPOILT)
            Else
                pcEnt.setStatus(PassCardStatusEnum.AVAILABLE)
            End If
            pcEnt.setDepositPrint(ConstantGlobal.No)
            pcEnt.setDeposit(0)
            pcEnt.setDebtorId(0)
            pcDao.updateDB(pcEnt, cn, trans)


            'Update Old PassCardHistory Before create new pass history
            Dim selectSQL As String = "select max(PassCardHistoryId) as ID from PassCardHistory Where PassCardMstrId = " & ddPass.SelectedValue

            dt = dm.execTable(selectSQL)

            If dt.Rows.Count > 0 Then
                If dt.Rows(0).Item("ID").ToString <> "" Then
                    Dim updateSQL As String = "Update PassCardHistory set enddate = getDate() where PassCardHistoryId = " & dt.Rows(0).Item("ID").ToString
                    dm.execTableInTrans(updateSQL, cn, trans)
                End If
            End If

            dpbcEnt.setCancellationDate(Utility.DataTypeUtils.formatDateString(txtCancellationDate.Text))
            dpbcEnt.setDebtorId(hidDebtorId.Value)
            dpbcEnt.setDeposit(ddDeposit.SelectedValue)
            dpbcEnt.setDepositAmount(Val(hidDeposit.Value))

            If Trim(txtEffectiveFrom.Text) <> "" Then
                dpbcEnt.setEffectiveFrom(Utility.DataTypeUtils.formatDateString(txtEffectiveFrom.Text))
            End If

            dpbcEnt.setLastUpdatedBy(lp.getUserMstrId)
            dpbcEnt.setLastUpdatedDatetime(Now)
            dpbcEnt.setPassCardCondition(ddPassCondition.SelectedValue)

            If Trim(txtOutstanding.Text) <> "" Then
                dpbcEnt.setOutstanding(txtOutstanding.Text)
            Else
                dpbcEnt.setOutstanding(0)
            End If

            If Trim(txtUnused.Text) <> "" Then
                dpbcEnt.setUnused(txtUnused.Text)
            Else
                dpbcEnt.setUnused(0)
            End If

            dpbcEnt.setPassCardMstrId(ddPass.SelectedValue)
            dpbcEnt.setProcessedBy(lp.getUserMstrId)
            dpbcEnt.setReason(ddReason.SelectedValue)
            dpbcEnt.setRemark(txtRemark.Text)
            dpbcEnt.setStatus(ItemReplacementStatusEnum.SUBMITTED)
            dpbcDao.insertDB(dpbcEnt, cn, trans)



            trans.Commit()

            trans = cn.BeginTransaction
            checkDebtorPassBay(hidDebtorId.Value, cn, trans)
            trans.Commit()

            lblmsg.Text = ""

            clearData()
            bindPass()

        Catch ex As Exception
            trans.Rollback()
            lblmsg.Text = ex.Message
            logger.Error(ex.Message)
        Finally
            trans.Dispose()
            cn.Close()

        End Try
    End Sub

    Private Sub checkDebtorPassBay(ByVal debtorId As String, ByRef cn As SqlConnection, ByRef trans As SqlTransaction)
        'To inactive debtor if no more passbay tied to the debtor

        Dim dt As New DataTable

        Try

            Dim selectSQL As String = "select count(DebtorPassBayId) as ID from DebtorPassBay Where DebtorId = " & debtorId & _
                                      " and status = 'A'"

            dt = dm.execTableInTrans(selectSQL, cn, trans)


            If dt.Rows(0).Item("ID") = 0 Then
                Dim updateSQL As String = "Update Debtor set status = 'I',Remark='Parking Services Cancelled' where DebtorId = " & debtorId
                dm.execTableInTrans(updateSQL, cn, trans)
            End If



        Catch ex As Exception
            lblmsg.Text = ex.Message
            Throw ex
        Finally
            dt = Nothing

        End Try
    End Sub

    Private Sub bindPass()
        Try
            Dim sql As String
            sql = "select passcardmstrid,SERIALNO,0 as seq from passcardmstr " & _
                  " where debtorid = " & hidDebtorId.Value & _
                  " and status = '" & PassCardStatusEnum.INUSE & "'" & _
                  " union all " & _
                  " select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' " & _
                  " order by seq,serialno"

            dsPass.SelectCommand = sql
            dsPass.DataBind()

        Catch ex As Exception
            logger.Error(ex.Message)
        End Try
    End Sub

    Protected Sub ddPass_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt As New DataTable
        Dim sql As String = ""

        Try
            sql = "SELECT DEPOSIT FROM PASSCARDMSTR WHERE PASSCARDMSTRID = " & ddPass.SelectedValue
            dt = dm.execTable(sql)

            If dt.Rows.Count = 0 Then
                lblPassDeposit.Text = ""
                hidDeposit.value = ""
            Else
                lblPassDeposit.Text = "Deposit - RM " & dt.Rows(0).Item("DEPOSIT").ToString
                hidDeposit.value = dt.Rows(0).Item("DEPOSIT").ToString
            End If


        Catch ex As Exception
            logger.Debug(ex.Message)
            lblmsg.Text = ex.Message

        Finally
            dt = Nothing
        End Try
        
    End Sub
End Class
