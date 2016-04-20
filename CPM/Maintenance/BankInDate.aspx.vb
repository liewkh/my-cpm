Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Maintenance_BankInDate
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim conString As String = ""
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
        Dim searchModel As New CPM.CodeMstrEntity
        Dim sqlmap As New SQLMap

        searchModel.setCodeCat(CodeCatEnum.BANKINDATE)
        searchModel.setCodeAbbr(Trim(txtEntryDate.Text.ToUpper))
        searchModel.setCodeDesc(Trim(txtBankInDate.Text.ToUpper))

        If rbActiveYes.Checked Then
            searchModel.setActive(ConstantGlobal.Yes)
        Else
            searchModel.setActive(ConstantGlobal.No)
        End If

        Dim strSQL As String = sqlmap.getMappedStatement("SetupMstr/Search-CodeMstr", searchModel)

        ViewState("strSQL") = strSQL


        dsBankInDate.SelectCommand = ViewState("strSQL")
        gvBankInDate.DataBind()

        gvBankInDate.PageIndex = 0

        If gvBankInDate.Rows.Count = 0 Then
            lblRecCount.Text = ConstantGlobal.No_Record_Found
        Else
            lblRecCount.Text = dm.getGridViewRecordCount(dsBankInDate).ToString + " " + "Record Found"
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
        txtEntryDate.Text = ""
        txtBankInDate.Text = ""
        gvBankInDate.DataSource = Nothing
        addMode()
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvBankInDate.Rows.Count
            ClientScript.RegisterForEventValidation(gvBankInDate.UniqueID, "Select$" + i.ToString)
        Next

        MyBase.Render(writer)
    End Sub

    Protected Sub gvBankInDate_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBankInDate.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            'e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvPaymentType.UniqueID + "','Select$" + gvPaymentType.Rows.Count.ToString + "');")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('gvBankInDate','Select$" + gvBankInDate.Rows.Count.ToString + "');")
            Dim int As Integer = gvBankInDate.Rows.Count / 2
            Dim dob As Double = gvBankInDate.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

        End If
    End Sub

    Protected Sub gvBankInDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBankInDate.SelectedIndexChanged
        lblmsg.Text = ""
        txtEntryDate.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBankInDate.SelectedRow.Cells(0).Text)
        txtBankInDate.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvBankInDate.SelectedRow.Cells(1).Text)

        Dim vDefunctInd As String = gvBankInDate.SelectedDataKey.Item("ACTIVE").ToString

        If ConstantGlobal.No.Equals(vDefunctInd) Then
            rbActiveYes.Checked = False
            rbActiveNo.Checked = True
        Else
            rbActiveYes.Checked = True
            rbActiveNo.Checked = False
        End If
        updateMode()
    End Sub

    Private Sub addMode()
        rbActiveNo.Checked = False
        rbActiveYes.Checked = True

        btnAdd.Enabled = True
        btnUpdate.Enabled = False
        btnSearch.Enabled = True

        txtEntryDate.ReadOnly = False
        txtBankInDate.CssClass = CSSEnum.TXTFIELD_2
    End Sub

    Private Sub updateMode()
        btnAdd.Enabled = False
        btnUpdate.Enabled = True
        btnSearch.Enabled = False

        txtEntryDate.ReadOnly = True
        txtEntryDate.CssClass = CSSEnum.TXTFIELD_2_DISABLED
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

        cn = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("path"))
        If Not cn.State = ConnectionState.Open Then
            cn.Open()
        End If
        trans = cn.BeginTransaction

        Try
            'validate the name is not existed before insert
            Dim searchModel As New CPM.CodeMstrEntity
            Dim sqlmap As New SQLMap
            searchModel.setCodeCat(CodeCatEnum.BANKINDATE)
            searchModel.setCodeAbbr(Trim(txtEntryDate.Text.ToUpper))
            strSQL = sqlmap.getMappedStatement("SetupMstr/Search-CodeName", searchModel)

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
        Dim codeMasterEnt As New CPM.CodeMstrEntity()
        Dim codeMasterDao As New CPM.CodeMstrDAO()
        Try
            codeMasterEnt.setCodeAbbr(Trim(txtEntryDate.Text.ToUpper))
            codeMasterEnt.setCodeDesc(Trim(txtBankInDate.Text.ToUpper))
            codeMasterEnt.setCodeCat(CodeCatEnum.BANKINDATE)

            If rbActiveYes.Checked = True Then VDefunctInd = ConstantGlobal.Yes Else VDefunctInd = ConstantGlobal.No
            codeMasterEnt.setActive(VDefunctInd)

            codeMasterEnt.setLastUpdatedDatetime(Now())
            codeMasterEnt.setLastUpdatedBy(lp.getUserMstrId)
            codeMasterDao.insertDB(codeMasterEnt, cn, trans)
        Catch ex As Exception
            Throw ex
        Finally
            codeMasterEnt = Nothing
            codeMasterDao = Nothing
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

        cn = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("path"))
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

        Dim codeMasterEnt As New CPM.CodeMstrEntity()
        Dim codeMasterDao As New CPM.CodeMstrDAO()

        Try
            codeMasterEnt.setCodeMstrId(gvBankInDate.SelectedDataKey(codeMasterDao.COLUMN_CodeMstrID).ToString)
            codeMasterEnt.setCodeDesc(txtBankInDate.Text.ToUpper)

            If rbActiveYes.Checked = True Then VDefunctInd = ConstantGlobal.Yes Else VDefunctInd = ConstantGlobal.No
            codeMasterEnt.setActive(VDefunctInd)

            codeMasterEnt.setLastUpdatedBy(gvBankInDate.SelectedDataKey("LUB").ToString)
            codeMasterEnt.setLastUpdatedDatetime(gvBankInDate.SelectedDataKey("LUDT"))
            codeMasterDao.updateDB(codeMasterEnt, cn, trans)


        Catch ex As Exception
            Throw ex
        Finally
            codeMasterEnt = Nothing
            codeMasterDao = Nothing

        End Try

    End Sub

    Protected Sub gvBankInDate_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvBankInDate.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindGridView()
        End If
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub
End Class
