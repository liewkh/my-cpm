Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports System.Math

Partial Class Maintenance_MiscPaymentType
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

            ddTaxCode.Items.Insert(0, New ListItem("---Please Choose One---", "0"))
            ddTaxCode.Items.Insert(1, New ListItem("Standard Rated", ConstantGlobal.StandardRated))
            ddTaxCode.Items.Insert(2, New ListItem("Zero Rated", ConstantGlobal.ZeroRated))

        End If

    End Sub

    Private Sub bindData()
        Dim searchModel As New MiscPaymentTypeSearchModel
        Dim miscPaymentTypeMstrDao As New CPM.MiscPaymentTypeMstrDAO
        Dim sqlmap As New SQLMap

        Try

            searchModel.setPaymentCode(Trim(txtMiscPayment.Text.ToUpper))
            searchModel.setPaymentDesc(Trim(txtMiscPaymentDesc.Text.ToUpper))


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


            Dim strSQL As String = sqlmap.getMappedStatement("SetupMstr/Search-MiscPaymentTypeMstr", searchModel)

            ViewState("strSQL") = strSQL


            dsMiscPaymentType.SelectCommand = ViewState("strSQL")
            gvMiscPaymentType.DataBind()

            gvMiscPaymentType.PageIndex = 0

            If gvMiscPaymentType.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsMiscPaymentType).ToString + " " + "Record Found"
            End If

        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            miscPaymentTypeMstrDao = Nothing
            searchModel = Nothing
            sqlmap = Nothing

        End Try

    End Sub

    Private Sub clear()
        lblmsg.Text = ""
        lblRecCount.Text = ""
        txtMiscPayment.Text = ""
        txtMiscPaymentDesc.Text = ""
        txtAmount.Text = ""
        ddTaxCode.SelectedValue = 0
        ddLocation.SelectedValue = lp.getDefaultLocationInfoId        
        rbActiveYes.Checked = True       
        gvMiscPaymentType.DataSource = Nothing
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

        If Not Utility.Tools.NumericValidation(txtAmount.Text) Then
            lblmsg.Text = "Please enter numeric value for Amount."
            Exit Sub
        End If


        If ddTaxCode.SelectedIndex = 0 Then
            lblmsg.Text = "Tax Code is a Required field."
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
            Dim searchModel As New MiscPaymentTypeSearchModel
            Dim sqlmap As New SQLMap
            searchModel.setPaymentCode(Trim(txtMiscPayment.Text.ToUpper))
            searchModel.setLocationInfoId(ddLocation.SelectedValue)
            strSQL = sqlmap.getMappedStatement("SetupMstr/Search-MiscPaymentTypeMstr", searchModel)

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
        Dim miscPaymentTypeMstrEnt As New CPM.MiscPaymentTypeMstrEntity
        Dim miscPaymentTypeMstrDAO As New CPM.MiscPaymentTypeMstrDAO

        Try

           

            miscPaymentTypeMstrEnt.setPaymentCode(Trim(txtMiscPayment.Text.ToUpper))
            miscPaymentTypeMstrEnt.setPaymentDesc(Trim(txtMiscPaymentDesc.Text.ToUpper))
            miscPaymentTypeMstrEnt.setLocationInfoId(ddLocation.SelectedValue)
            miscPaymentTypeMstrEnt.setAmount(txtAmount.Text)
            miscPaymentTypeMstrEnt.setTaxCode(ddTaxCode.SelectedValue)


            If rbActiveYes.Checked = True Then activeInd = ConstantGlobal.Yes Else activeInd = ConstantGlobal.No

            miscPaymentTypeMstrEnt.setActive(activeInd)

            miscPaymentTypeMstrEnt.setLastUpdatedDatetime(Now())
            miscPaymentTypeMstrEnt.setLastUpdatedBy(lp.getUserMstrId)
            miscPaymentTypeMstrDAO.insertDB(miscPaymentTypeMstrEnt, cn, trans)

        Catch ex As Exception
            Throw ex

        Finally
            miscPaymentTypeMstrEnt = Nothing
            miscPaymentTypeMstrDAO = Nothing
        End Try


    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        clear()
        ViewState("strSQL") = Nothing
        rbActiveNo.Checked = False
        rbActiveYes.Checked = True
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvMiscPaymentType.Rows.Count
            ClientScript.RegisterForEventValidation(gvMiscPaymentType.UniqueID, "Select$" + i.ToString)
        Next

        MyBase.Render(writer)
    End Sub

    Protected Sub gvSeasonType_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMiscPaymentType.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvMiscPaymentType.UniqueID + "','Select$" + gvMiscPaymentType.Rows.Count.ToString + "');")
            'e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('gvSeasonType','Select$" + gvSeasonType.Rows.Count.ToString + "');")
            Dim int As Integer = gvMiscPaymentType.Rows.Count / 2
            Dim dob As Double = gvMiscPaymentType.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

        End If
    End Sub

    Protected Sub gvMiscPaymentType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvMiscPaymentType.SelectedIndexChanged
        Dim miscPaymentTypeMstrDAO As New CPM.MiscPaymentTypeMstrDAO

        Try
            lblmsg.Text = ""
            txtMiscPayment.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvMiscPaymentType.SelectedDataKey(miscPaymentTypeMstrDAO.COLUMN_PaymentCode))
            txtMiscPaymentDesc.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvMiscPaymentType.SelectedDataKey(miscPaymentTypeMstrDAO.COLUMN_PaymentDesc))
            txtAmount.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvMiscPaymentType.SelectedDataKey(miscPaymentTypeMstrDAO.COLUMN_Amount))
            ddTaxCode.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvMiscPaymentType.SelectedDataKey(miscPaymentTypeMstrDAO.COLUMN_TaxCode))
            ddLocation.SelectedValue = Utility.DataTypeUtils.parseHTMLSafeToString(gvMiscPaymentType.SelectedDataKey(miscPaymentTypeMstrDAO.COLUMN_LocationInfoId))



            Dim vActiveInd As String = Utility.DataTypeUtils.parseHTMLSafeToString(gvMiscPaymentType.SelectedDataKey(miscPaymentTypeMstrDAO.COLUMN_Active))

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
            miscPaymentTypeMstrDAO = Nothing
        End Try

    End Sub

    Private Sub addMode()
        rbActiveNo.Checked = False
        rbActiveYes.Checked = True

        btnAdd.Enabled = True
        btnUpdate.Enabled = False
        btnSearch.Enabled = True

        txtMiscPayment.ReadOnly = False
        txtMiscPayment.CssClass = CSSEnum.TXTFIELD_2
    End Sub

    Private Sub updateMode()
        btnAdd.Enabled = False
        btnUpdate.Enabled = True
        btnSearch.Enabled = False

        txtMiscPayment.ReadOnly = True
        txtMiscPayment.CssClass = CSSEnum.TXTFIELD_2_DISABLED
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        lblmsg.Text = ""

        If Not Page.IsValid Then
            Exit Sub
        End If

        If Not Utility.Tools.NumericValidation(txtAmount.Text) Then
            lblmsg.Text = "Please enter numeric value for Amount."
            Exit Sub
        End If

        If ddTaxCode.SelectedIndex = 0 Then
            lblmsg.Text = "Tax Code is a Required field."
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

        Dim activeInd As String
        Dim miscPaymentTypeMstrEntity As New CPM.MiscPaymentTypeMstrEntity
        Dim miscPaymentTypeMstrDAO As New CPM.MiscPaymentTypeMstrDAO

        Try

            miscPaymentTypeMstrEntity.setMiscPaymentTypeMstrId(gvMiscPaymentType.SelectedDataKey(miscPaymentTypeMstrDAO.COLUMN_MiscPaymentTypeMstrID))
            miscPaymentTypeMstrEntity.setPaymentCode(Trim(txtMiscPayment.Text.ToUpper))
            miscPaymentTypeMstrEntity.setPaymentDesc(Trim(txtMiscPaymentDesc.Text.ToUpper))           
            miscPaymentTypeMstrEntity.setLocationInfoId(ddLocation.SelectedValue)
            miscPaymentTypeMstrEntity.setAmount(txtAmount.Text)
            miscPaymentTypeMstrEntity.setTaxCode(Trim(ddTaxCode.SelectedValue))

            If rbActiveYes.Checked = True Then activeInd = ConstantGlobal.Yes Else activeInd = ConstantGlobal.No

            miscPaymentTypeMstrEntity.setActive(activeInd)

            miscPaymentTypeMstrEntity.setLastUpdatedDatetime(gvMiscPaymentType.SelectedDataKey("LUDT"))

            miscPaymentTypeMstrEntity.setLastUpdatedBy(lp.getUserMstrId)
            miscPaymentTypeMstrDAO.updateDB(miscPaymentTypeMstrEntity, cn, trans)
        Catch ex As Exception
            Throw ex
        Finally
            miscPaymentTypeMstrEntity = Nothing
            miscPaymentTypeMstrDAO = Nothing

        End Try



    End Sub

    Protected Sub gvMiscPaymentType_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMiscPaymentType.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindData()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub

End Class
