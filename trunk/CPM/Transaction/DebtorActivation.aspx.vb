Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports PopCalendarSpecialDay

Partial Class Maintenance_DebtorActivation
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

    Public Sub clear()

        lblmsg.Text = ""
        lblRecCount.Text = ""
        ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        txtName.Text = ""   
        txtCompanyName.Text = ""       
        ddStatus.SelectedIndex = 0
        gvDebtor.DataSource = Nothing
        lblRecCount.Text = ""
        hidDebtorIds.Value = ""
        hidLocationId.Value = ""
        txtRemark.Text = ""
        addMode()

    End Sub

    Private Sub addMode()

        btnUpdate.Enabled = False
        btnSearch.Enabled = True
        rbIndividual.Enabled = True
        rbCompany.Enabled = True

    End Sub

    Private Sub updateMode()
        btnUpdate.Enabled = True
        btnSearch.Enabled = False
        rbIndividual.Enabled = False
        rbCompany.Enabled = False

        lblmsg.Text = ""
    End Sub

    Private Sub bindData()
        Dim searchModel As New DebtorSearchModel
        Dim debtorDao As New CPM.DebtorDAO
        Dim sqlmap As New SQLMap

        Try

            lblmsg.Text = ""

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

        Else

            If Trim(txtName.Text) = "" Then
                lblmsg.Text = "Please enter a value for Name."
                Exit Sub
            End If

        End If

        If Trim(txtRemark.Text) = "" Then
            lblmsg.Text = "Please enter a value for Remark."
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
        Dim debtorOldEnt As New CPM.DebtorEntity
        Dim debtorDao As New CPM.DebtorDAO
        Dim debtorActivationEnt As New CPM.DebtorActivationEntity
        Dim debtorActivationDao As New CPM.DebtorActivationDAO


        Dim searchModel As New DebtorSearchModel
        Dim sqlmap As New SQLMap
        Dim dt As New DataTable
        Dim strChange As String = ""
        Dim flgChanged As Boolean = False
        Dim retLogId As Long

        Try

            searchModel.setDebtorId(gvDebtor.SelectedDataKey(debtorDao.COLUMN_DebtorID))
            debtorEnt.setDebtorId(gvDebtor.SelectedDataKey(debtorDao.COLUMN_DebtorID))

            If rbCompany.Checked = True Then
                debtorEnt.setName(Trim(txtCompanyName.Text.ToUpper))
            Else
                debtorEnt.setName(Trim(txtName.Text.ToUpper))
            End If


            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-Debtor", searchModel)

            dt = dm.execTable(strSQL)
            If dt.Rows.Count > 0 Then
                debtorOldEnt.setName(dt.Rows(0)("Debtor"))
                debtorOldEnt.setStatus(dt.Rows(0)(debtorDao.COLUMN_Status))
            End If

            If ddStatus.SelectedIndex <> 0 Then
                debtorEnt.setStatus(ddStatus.SelectedValue)
            End If

            If (String.Compare(debtorOldEnt.getName, debtorEnt.getName)) Then
                strChange = "Changed Debtor Name From : " + debtorOldEnt.getName + " To " + debtorEnt.getName
                retLogId = Utility.Auditor.log(cn, trans, lp, "Debtor", debtorEnt.getDebtorId, strChange)
                flgChanged = True
            End If

            If (String.Compare(debtorOldEnt.getStatus, debtorEnt.getStatus)) Then
                strChange = "Changed Debtor Status From : " + debtorOldEnt.getStatus + " To " + debtorEnt.getStatus
                retLogId = Utility.Auditor.log(cn, trans, lp, "Debtor", debtorEnt.getDebtorId, strChange)
                flgChanged = True
            End If

            If (flgChanged = True) Then
                debtorEnt.setLastUpdatedDatetime(Now)
                debtorEnt.setLastUpdatedBy(lp.getUserMstrId)
                debtorDao.updateDB(debtorEnt, cn, trans)

                debtorActivationEnt.setActive(ConstantGlobal.Yes)
                debtorActivationEnt.setDebtorId(gvDebtor.SelectedDataKey(debtorDao.COLUMN_DebtorID))
                debtorActivationEnt.setLastUpdatedBy(lp.getUserMstrId)
                debtorActivationEnt.setLastUpdatedDatetime(Now)
                debtorActivationEnt.setRemarks(Trim(txtRemark.Text))
                debtorActivationEnt.setAuditTrailId(retLogId)
                debtorActivationDao.insertDB(debtorActivationEnt, cn, trans)


            End If


        Catch ex As Exception
            Throw ex
        Finally
            debtorEnt = Nothing
            debtorDao = Nothing
            debtorOldEnt = Nothing
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

            ddLocation.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_LocationInfoId))
            ddStatus.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey(debtorDao.COLUMN_Status))

           
            If gvDebtor.SelectedDataKey(debtorDao.COLUMN_Category).ToString.Equals(CategoryEnum.COMPANY) Then
                rbCompany.Checked = True
                rbIndividual.Checked = False
                txtCompanyName.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey("DEBTOR")).ToString                
            Else
                rbIndividual.Checked = True
                rbCompany.Checked = False               
                txtName.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvDebtor.SelectedDataKey("DEBTOR"))             
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