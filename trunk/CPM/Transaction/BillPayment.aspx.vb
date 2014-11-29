Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports PopCalendarSpecialDay

Partial Class Transaction_BillPayment
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim myDataColumn As DataColumn
    Dim myDataRow As DataRow
    Dim myDataTable As New DataTable("AdditionalCodes")


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        lp = Session("LoginProfile")
        If IsNothing(lp) Then
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("appPath") + "/login.aspx?login=expired")
        End If

        If Not Page.IsPostBack Then
            SpecialDays.AddHolidays(popCalendar11)
            SpecialDays.AddSpecialDays(popCalendar11)
            txtPaymentDate1.Text = Now.ToShortDateString

            Dim sql As String = "select locationinfoid,locationName,0 as seq from locationinfo where locationinfoid = " & lp.getDefaultLocationInfoId & _
                                "union select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' " & _
                                "union select el.locationinfoid,li.locationname,0 as seq from employeelocation el,locationinfo li " & _
                                "where el.locationinfoid = li.locationinfoid and el.employeemstrid = " & lp.getEmployeeMstrId & " order by seq,locationname"
            dsLocation.SelectCommand = sql
            dsLocation.DataBind()

            ddLocation.SelectedValue = lp.getDefaultLocationInfoId
            bindDebtor()

        End If



    End Sub

    Public Sub clear()

        lblmsg.Text = ""
        txtPaymentDate1.Text = Now.ToShortDateString
        rbCompany.Checked = True
        ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        ddDebtor.SelectedIndex = 0
        txtInvoiceNo.text = ""
        txtDescription.Text = ""
        txtReceiptNo.Text = ""
        ddPaymentType.SelectedIndex = 0
        txtRefNo.text = ""
        txtPaymentAmount.Text = ""
        txtNoPrint.Text = ""

        'gvDebtor.DataSource = Nothing
        hidDebtorId.Value = ""

    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        clear()
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To DefaultGrid.Rows.Count
            ClientScript.RegisterForEventValidation(DefaultGrid.UniqueID, "Select$" + i.ToString)
        Next
        Dim cat As String = ""

        If rbCompany.Checked = True Then
            cat = CategoryEnum.COMPANY
        Else
            cat = CategoryEnum.INDIVIDUAL
        End If

        'imgDebtor.Attributes.Add("OnClick", "javascript:open_popupModal('../Maintenance/DebtorPassCard.aspx?category=" & cat & "','M');")        

        'Dim myColorDDL As DropDownList = Me.FindControl("ddLocation")

        'myColorDDL.Items.FindByText("ASIA CAFE").Attributes.Add("style", "COLOR:green")


        MyBase.Render(writer)
    End Sub

    Protected Sub defaultGrid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DefaultGrid.SelectedIndexChanged
        Dim debtorDao As New CPM.DebtorDAO
        Dim searchModel As New CPM.PassCardMstrEntity
        Dim sqlmap As New SQLMap
        Dim dtPassBayNo As New DataTable

        Try
            txtPaymentAmount.Text = DefaultGrid.SelectedDataKey("amount")
            ddPaymentType.SelectedValue = DefaultGrid.SelectedDataKey("paymentType")
            txtDescription.Text = DefaultGrid.SelectedDataKey("paymentDescription")
            txtPaymentDate1.Text = DefaultGrid.SelectedDataKey("paymentDate")
            txtRefNo.Text = DefaultGrid.SelectedDataKey("refNo")


            'hidDebtorIds.Value = gvDebtor.SelectedDataKey(debtorDao.COLUMN_DebtorID)

            'updateMode()

        Catch ex As Exception
            Throw ex
        Finally
            debtorDao = Nothing
            dtPassBayNo = Nothing
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub




    Protected Sub ddPaymentType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If ddPaymentType.Text = PaymentTypeEnum.CHEQUE Then
            info.Visible = True
            lblDisplay.Text = "Cheque No"
        ElseIf ddPaymentType.Text = PaymentTypeEnum.CREDITCARD Then
            info.Visible = True
            lblDisplay.Text = "Approval No"
        Else
            info.Visible = False
        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        lblmsg.Text = ""

        If Not Page.IsValid Then
            Exit Sub
        End If

        If Trim(txtPaymentAmount.Text) <> "" Then
            If Not Utility.Tools.NumericValidation(Trim(txtPaymentAmount.Text)) Then
                lblmsg.Text = "Please enter a numeric value for Payment Amount."
                Exit Sub
            End If

        End If

        Try



            If Session("AdditionalCodes") Is Nothing Then
                myDataTable = New DataTable("AdditionalCodes")

                myDataColumn = New DataColumn
                myDataColumn.DataType = System.Type.GetType("System.String")
                myDataColumn.ColumnName = "paymentDate"
                myDataColumn.Caption = "Payment Date"
                myDataColumn.Unique = False
                myDataTable.Columns.Add(myDataColumn)

                myDataColumn = New DataColumn
                myDataColumn.DataType = System.Type.GetType("System.String")
                myDataColumn.ColumnName = "paymentDescription"
                myDataColumn.Caption = "Payment Description"
                myDataColumn.Unique = False
                myDataTable.Columns.Add(myDataColumn)

                myDataColumn = New DataColumn
                myDataColumn.DataType = System.Type.GetType("System.String")
                myDataColumn.ColumnName = "paymentType"
                myDataColumn.Caption = "Payment Type"
                myDataColumn.Unique = False
                myDataTable.Columns.Add(myDataColumn)

                myDataColumn = New DataColumn
                myDataColumn.DataType = System.Type.GetType("System.String")
                myDataColumn.ColumnName = "paymentTypeDesc"
                myDataColumn.Caption = "Payment Type Desc"
                myDataColumn.Unique = False
                myDataTable.Columns.Add(myDataColumn)

                myDataColumn = New DataColumn
                myDataColumn.DataType = System.Type.GetType("System.String")
                myDataColumn.ColumnName = "amount"
                myDataColumn.Caption = "Amount (RM)"
                myDataColumn.Unique = False
                myDataTable.Columns.Add(myDataColumn)

                myDataColumn = New DataColumn
                myDataColumn.DataType = System.Type.GetType("System.String")
                myDataColumn.ColumnName = "refNo"
                myDataColumn.Caption = "Ref No"
                myDataColumn.Unique = False
                myDataTable.Columns.Add(myDataColumn)


                myDataRow = myDataTable.NewRow()
                myDataRow("paymentDate") = txtPaymentDate1.Text
                myDataRow("paymentDescription") = Trim(txtDescription.Text.ToUpper)
                myDataRow("paymentType") = ddPaymentType.SelectedValue
                myDataRow("paymentTypeDesc") = ddPaymentType.SelectedItem.Text
                myDataRow("amount") = Trim(txtPaymentAmount.Text)
                myDataRow("refNo") = Trim(txtRefNo.Text)
                myDataTable.Rows.Add(myDataRow)

                Session("AdditionalCodes") = myDataTable

            Else

                myDataTable = Session("AdditionalCodes")
                myDataRow = myDataTable.NewRow()
                myDataRow("paymentDate") = txtPaymentDate1.Text
                myDataRow("paymentDescription") = Trim(txtDescription.Text.ToUpper)
                myDataRow("paymentType") = ddPaymentType.SelectedValue
                myDataRow("paymentTypeDesc") = ddPaymentType.SelectedItem.Text
                myDataRow("amount") = Trim(txtPaymentAmount.Text)
                myDataRow("refNo") = Trim(txtRefNo.Text)
                myDataTable.Rows.Add(myDataRow)

                Session("AdditionalCodes") = myDataTable

            End If


            lblmsg.Text = ""
            txtDescription.Text = ""
            ddPaymentType.SelectedIndex = 0
            txtRefNo.Text = ""
            txtPaymentAmount.Text = ""

            Me.DefaultGrid.DataSource = myDataTable
            Me.DefaultGrid.DataBind()

        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally

        End Try

    End Sub

    Protected Sub DefaultGrid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles DefaultGrid.RowDataBound
        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + DefaultGrid.UniqueID + "','Select$" + DefaultGrid.Rows.Count.ToString + "');")

            Dim int As Integer = DefaultGrid.Rows.Count / 2
            Dim dob As Double = DefaultGrid.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

        End If
    End Sub

    Protected Sub DefaultGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DefaultGrid.RowDeleting
        'do nothing



    End Sub

    Protected Sub doDelete(ByVal sender As Object, ByVal e As System.EventArgs)
        myDataTable = Session("AdditionalCodes")
        myDataTable.Rows(DefaultGrid.SelectedIndex).Delete()
        Session("AdditionalCodes") = myDataTable

        Me.DefaultGrid.DataSource = myDataTable
        Me.DefaultGrid.DataBind()

        lblmsg.Text = ""
        txtDescription.Text = ""
        ddPaymentType.SelectedIndex = 0
        txtRefNo.Text = ""
        txtPaymentAmount.Text = ""

    End Sub

    Protected Sub ddLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        bindDebtor()
    End Sub

    Protected Sub bindDebtor()
        Dim searchModel As New CPM.DebtorEntity
        Dim sqlmap As New SQLMap


        Try


            If ddLocation.SelectedIndex > 0 Then
                searchModel.setLocationInfoId(ddLocation.SelectedValue)
            End If

            If rbCompany.Checked = True Then
                searchModel.setCategory(CategoryEnum.COMPANY)
            Else
                searchModel.setCategory(CategoryEnum.INDIVIDUAL)
            End If

            Dim strSQL As String = sqlmap.getMappedStatement("BillGeneration/Search-Debtor", searchModel)

            ViewState("strSQL") = strSQL


            dsDebtor.SelectCommand = ViewState("strSQL")
            ddDebtor.DataBind()

        Catch ex As Exception
            Throw ex

        End Try

    End Sub

    Protected Sub rbIndividual_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        bindDebtor()
    End Sub

    Protected Sub rbCompany_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        bindDebtor()
    End Sub
End Class
