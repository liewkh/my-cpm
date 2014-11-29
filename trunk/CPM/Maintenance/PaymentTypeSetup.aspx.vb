Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Maintenance_PaymentTypeSetup
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

        searchModel.setCodeCat(CodeCatEnum.PAYMENTTYPE)
        searchModel.setCodeAbbr(Trim(txtPaymentTypeCode.Text.ToUpper))
        searchModel.setCodeDesc(Trim(txtPaymentTypeDesc.Text.ToUpper))

        If rbActiveYes.Checked Then
            searchModel.setActive(ConstantGlobal.Yes)
        Else
            searchModel.setActive(ConstantGlobal.No)
        End If

        Dim strSQL As String = sqlmap.getMappedStatement("SetupMstr/Search-CodeMstr", searchModel)

        ViewState("strSQL") = strSQL


        dsPaymentType.SelectCommand = ViewState("strSQL")
        gvPaymentType.DataBind()

        gvPaymentType.PageIndex = 0

        If gvPaymentType.Rows.Count = 0 Then
            lblRecCount.Text = ConstantGlobal.No_Record_Found
        Else
            lblRecCount.Text = dm.getGridViewRecordCount(dsPaymentType).ToString + " " + "Record Found"
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
        txtPaymentTypeCode.Text = ""
        txtPaymentTypeDesc.Text = ""
        gvPaymentType.DataSource = Nothing
        addMode()
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvPaymentType.Rows.Count
            ClientScript.RegisterForEventValidation(gvPaymentType.UniqueID, "Select$" + i.ToString)
        Next

        MyBase.Render(writer)
    End Sub

    Protected Sub gvPaymentType_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPaymentType.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            'e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvPaymentType.UniqueID + "','Select$" + gvPaymentType.Rows.Count.ToString + "');")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('gvPaymentType','Select$" + gvPaymentType.Rows.Count.ToString + "');")
            Dim int As Integer = gvPaymentType.Rows.Count / 2
            Dim dob As Double = gvPaymentType.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

        End If
    End Sub

    Protected Sub gvPaymentType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPaymentType.SelectedIndexChanged
        lblmsg.Text = ""
        txtPaymentTypeCode.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvPaymentType.SelectedRow.Cells(0).Text)
        txtPaymentTypeDesc.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvPaymentType.SelectedRow.Cells(1).Text)

        Dim vDefunctInd As String = gvPaymentType.SelectedDataKey.Item("ACTIVE").ToString

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

        txtPaymentTypeCode.ReadOnly = False
        txtPaymentTypeCode.CssClass = CSSEnum.TXTFIELD_2
    End Sub

    Private Sub updateMode()
        btnAdd.Enabled = False
        btnUpdate.Enabled = True
        btnSearch.Enabled = False

        txtPaymentTypeCode.ReadOnly = True
        txtPaymentTypeCode.CssClass = CSSEnum.TXTFIELD_2_DISABLED
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
            searchModel.setCodeCat(CodeCatEnum.PAYMENTTYPE)
            searchModel.setCodeAbbr(Trim(txtPaymentTypeCode.Text.ToUpper))
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
            codeMasterEnt.setCodeAbbr(Trim(txtPaymentTypeCode.Text.ToUpper))
            codeMasterEnt.setCodeDesc(Trim(txtPaymentTypeDesc.Text.ToUpper))
            codeMasterEnt.setCodeCat(CodeCatEnum.PAYMENTTYPE)

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
            codeMasterEnt.setCodeMstrId(gvPaymentType.SelectedDataKey(codeMasterDao.COLUMN_CodeMstrID).ToString)
            codeMasterEnt.setCodeDesc(txtPaymentTypeDesc.Text.ToUpper)

            If rbActiveYes.Checked = True Then VDefunctInd = ConstantGlobal.Yes Else VDefunctInd = ConstantGlobal.No
            codeMasterEnt.setActive(VDefunctInd)

            codeMasterEnt.setLastUpdatedBy(gvPaymentType.SelectedDataKey("LUB").ToString)
            codeMasterEnt.setLastUpdatedDatetime(gvPaymentType.SelectedDataKey("LUDT"))
            codeMasterDao.updateDB(codeMasterEnt, cn, trans)


        Catch ex As Exception
            Throw ex
        Finally
            codeMasterEnt = Nothing
            codeMasterDao = Nothing

        End Try
        
    End Sub

    Protected Sub gvPaymentType_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPaymentType.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindGridView()
        End If
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub
End Class
