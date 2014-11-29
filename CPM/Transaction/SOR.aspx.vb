Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Transaction_SOR
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim paramDebtorId As String
    Dim paramSeasonTypeMstrId As String
    Dim paramLocationInfoId As String
    Dim paramDebtorName As String
    Dim paramUserName As String

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

                sql = "Select DEBTORID,NAME AS DEBTOR,0 as Seq From Debtor Where Status = '" & DebtorStatusEnum.ACTIVE & "'" & _
                      "And LocationInfoId = " & lp.getDefaultLocationInfoId & " And Category='C' UNION ALL SELECT CODEMSTRID,CODEDESC,SEQ FROM CODEMSTR WHERE CODECAT = 'ALL'" & _
                      "ORDER BY SEQ,DEBTOR"

                dsDebtor.SelectCommand = sql
                dsDebtor.DataBind()
                ddLocation.SelectedValue = lp.getDefaultLocationInfoId
                BindDropDownList(ddStartMonth)
                BindDropDownList(ddEndMonth)
            End If

            paramDebtorId = Request.Params("debtorId")
            paramLocationInfoId = Request.Params("locationInfoId")
            paramDebtorName = Request.Params("debtorName")
            paramUserName = Request.Params("userName")

            'If Not Page.IsPostBack Then
            '    bindData()


            'End If




            
        Catch ex As Exception
            lblmsg.Text = ex.Message

        End Try




    End Sub

    Private Sub bindData()
        Dim searchModel As New CPM.SOREntity
        Dim sqlmap As New SQLMap

        Try


         

            ddLocation.SelectedValue = paramLocationInfoId
           
            If String.IsNullOrEmpty(paramDebtorId) Then
                searchModel.setDebtorId(ddDebtor.SelectedValue)
            Else
                searchModel.setDebtorId(paramDebtorId)
            End If

            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorSOR", searchModel)

            ViewState("strSQL") = strSQL


            dsSor.SelectCommand = ViewState("strSQL")
            gvSor.DataBind()

            gvSor.PageIndex = 0

            If gvSor.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsSor).ToString + " " + "Record Found"
            End If

            bindSerialNoData()

        Catch ex As Exception
            Throw ex

        Finally
            searchModel = Nothing
            sqlmap = Nothing

        End Try


    End Sub

    Private Sub bindSerialNoData()
        Dim searchModel As New CPM.DebtorPassBayEntity
        Dim sqlmap As New SQLMap

        Try

            ''ddLocation.SelectedValue = paramLocationInfoId


            If String.IsNullOrEmpty(paramDebtorId) Then
                searchModel.setDebtorId(ddDebtor.SelectedValue)
            Else
                searchModel.setDebtorId(paramDebtorId)
            End If

            Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorPassBayMinusSor", searchModel)

            ViewState("strSQL") = strSQL

            dsSerialNo.SelectCommand = ViewState("strSQL")
            dsSerialNo.DataBind()

        Catch ex As Exception
            Throw ex

        Finally
            searchModel = Nothing
            sqlmap = Nothing

        End Try


    End Sub

    Private Sub clear()
        lblmsg.Text = ""
        lblRecCount.Text = ""
        ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        ddStartMonth.SelectedIndex = 0
        ddEndMonth.SelectedIndex = 0
        ddDebtor.SelectedIndex = 0
        gvSor.DataSource = Nothing
        bindSerialNoData()
        txtRemark.Text = ""


    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        bindData()
    End Sub


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvSor.Rows.Count
            ClientScript.RegisterForEventValidation(gvSor.UniqueID, "Select$" + i.ToString)
        Next

        MyBase.Render(writer)
    End Sub

    Protected Sub gvSor_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSor.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            'e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvRefundEnq.UniqueID + "','Select$" + gvRefundEnq.Rows.Count.ToString + "');")
            Dim int As Integer = gvSor.Rows.Count / 2
            Dim dob As Double = gvSor.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If
        End If

    End Sub


    Protected Sub gvSor_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSor.RowCommand
        'For Sorting
        If e.CommandName.Equals("DeleteSOR") Then
            Dim Id As String = e.CommandArgument
            DeleteSORRecordBySorId(Id)
        ElseIf e.CommandName.Equals("Select") Then
            bindData()
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'bindDropDown()
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        clear()
    End Sub

    Protected Sub ddLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        lblmsg.Text = ""
        lblRecCount.Text = ""      
        ddStartMonth.SelectedIndex = 0
        ddEndMonth.SelectedIndex = 0
        gvSor.DataSource = Nothing
        txtRemark.Text = ""
        bindDropDown()
        bindSerialNoData()

    End Sub

    Protected Sub ddDebtorName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        bindSerialNoData()
    End Sub

    Public Sub bindDropDown()
        Dim searchModel As New CPM.DebtorEntity
        Dim sqlmap As New SQLMap


        Try


            If rbcompany.checked = True Then
                searchModel.setCategory(CategoryEnum.COMPANY)
            Else
                searchModel.setCategory(CategoryEnum.INDIVIDUAL)
            End If

            searchModel.setStatus(DebtorStatusEnum.ACTIVE)
            searchModel.setLocationInfoId(ddLocation.SelectedValue)


            Dim strSQL As String = sqlmap.getMappedStatement("BillGeneration/Search-Debtor", searchModel)

            ViewState("strSQL") = strSQL


            dsDebtor.SelectCommand = ViewState("strSQL")
            ddDebtor.DataBind()

        Catch ex As Exception
            logger.Debug(ex.Message)
            lblMsg.Text = ex.Message

        End Try

    End Sub

    Protected Sub rbIndividual_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        bindDropDown()        
    End Sub

    Protected Sub rbCompany_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        bindDropDown()
    End Sub

    Private Sub BindDropDownList(ByVal obj As DropDownList)
        Dim dt As New DataTable()

        Try

            Dim sqlStatement As String = "select datename(mm,DATEADD(month, DATEDIFF(month, 0, GETDATE()+31) + N.number, 0)) + '-' " & _
            " + datename(yy,DATEADD(month, DATEDIFF(month, 0, GETDATE()) + N.number, 0)) as Date1, " & _
            " convert(Datetime,DATEADD(month, DATEDIFF(month, 0, GETDATE()+31) + N.number, 0),120) as Date2 " & _
            " FROM master.dbo.spt_values N WHERE (N.type = 'P' AND N.number <=  4)"

            dt = dm.execTable(sqlStatement)

            If dt.Rows.Count > 0 Then
                obj.Items.Add(New ListItem("Please Choose One"))
                For i As Integer = 0 To dt.Rows.Count - 1
                    obj.Items.Add(New ListItem(dt.Rows.Item(i)("Date1").ToString(), dt.Rows.Item(i)("Date2").ToString()))
                Next
            End If
        Catch ex As Exception
            Dim msg As String = "Fetch Date Error:"
            msg += ex.Message
            lblmsg.Text = msg
        Finally

        End Try
    End Sub

    Protected Sub ddDebtor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        bindSerialNoData()
        bindData()
    End Sub

    Protected Sub ddStartMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ddEndMonth.SelectedValue = ddStartMonth.SelectedValue
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim sql As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim sorEnt As New CPM.SOREntity
        Dim sorDao As New CPM.SORDAO
        Dim sorVal As String = ""


        Try


            If Not Page.IsValid Then
                Exit Sub
            End If


            'Begin insert record into database
            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If

            trans = cn.BeginTransaction

            sql = "Select ParameterValue From Parameter Where ParameterName  = '" + ParameterEnum.SORMONTH + "'"

            dt = dm.execTable(sql)

            If dt.Rows.Count > 0 Then
                If Not dt.Rows(0)("ParameterValue").Equals(System.DBNull.Value) Then
                    sorVal = dt.Rows(0)("ParameterValue")
                Else
                    Throw New ApplicationException(ConstantGlobal.No_Record_Found)
                End If
            End If

            If DateDiff("m", ddStartMonth.SelectedValue, ddEndMonth.SelectedValue) = sorVal Then
                lblmsg.Text = "Transaction Is Not Allowed. Exceeded " + sorVal + " Months."
                Exit Sub
            End If


            If (ddDebtor.SelectedIndex = 0) Then
                lblmsg.Text = "Debtor is a Required field."
                Exit Sub
            End If

            If (ddStartMonth.SelectedIndex = 0) Then
                lblmsg.Text = "Start Month is a Required field."
                Exit Sub
            End If

            If (ddEndMonth.SelectedIndex = 0) Then
                lblmsg.Text = "End Month is a Required field."
                Exit Sub
            End If


            'If Not (chkVisitor.Checked) Then
            '    If (ddDebtor.SelectedIndex = 0) Then
            '        lblmsg.Text = "Debtor is a Required field."
            '        Exit Sub
            '    End If
            'End If



            'If ViewState("CurrentTable") Is Nothing Then                
            '    dt.Columns.Add("DebtorId")
            '    dt.Columns.Add("StartMonth")
            '    dt.Columns.Add("EndMonth")
            '    dt.Columns.Add("Remarks")
            '    dt.Columns.Add("PassCardMstrId")
            '    dt.Columns.Add("SerialNo")

            '    dr = dt.NewRow                
            '    dr("DebtorId") = ddDebtor.SelectedValue
            '    dr("StartMonth") = DateTime.Parse(ddStartMonth.SelectedValue).ToString("MMMM-yyyy")
            '    dr("EndMonth") = DateTime.Parse(ddEndMonth.SelectedValue).ToString("MMMM-yyyy")
            '    dr("Remarks") = Trim(txtRemark.Text)
            '    dr("PassCardMstrId") = ddSerialNo.SelectedValue
            '    dr("SerialNo") = ddSerialNo.SelectedItem
            '    dt.Rows.Add(dr)

            '    ViewState("CurrentTable") = dt
            '    gvSor.DataSource = dt
            '    gvSor.DataBind()

            '    ddSerialNo.Items.Remove(ddSerialNo.SelectedItem)
            '    ddSerialNo.DataBind()
            'Else
            '    dt = ViewState("CurrentTable")

            '    dr = dt.NewRow
            '    dr("DebtorId") = ddDebtor.SelectedValue
            '    dr("StartMonth") = DateTime.Parse(ddStartMonth.SelectedValue).ToString("MMMM-yyyy")
            '    dr("EndMonth") = DateTime.Parse(ddEndMonth.SelectedValue).ToString("MMMM-yyyy")
            '    dr("Remarks") = Trim(txtRemark.Text)
            '    dr("PassCardMstrId") = ddSerialNo.SelectedValue
            '    dr("SerialNo") = ddSerialNo.SelectedItem
            '    dt.Rows.Add(dr)

            '    ViewState("CurrentTable") = dt
            '    gvSor.DataSource = dt
            '    gvSor.DataBind()

            '    ddSerialNo.Items.Remove(ddSerialNo.SelectedItem)
            '    ddSerialNo.DataBind()

            'End If



            sorEnt.setDebtorId(ddDebtor.SelectedValue)
            sorEnt.setStartMonth(DateTime.Parse(ddStartMonth.SelectedValue))
            sorEnt.setEndMonth(DateTime.Parse(ddEndMonth.SelectedValue))
            sorEnt.setRemarks(Trim(txtRemark.Text))
            sorEnt.setPassCardMstrId(ddSerialNo.SelectedValue)
            sorEnt.setLastUpdatedBy(lp.getUserMstrId)
            sorEnt.setLastUpdatedDatetime(Now)
            sorDao.insertDB(sorEnt, cn, trans)


            trans.Commit()
            bindData()

            lblmsg.Text = ""





        Catch ex As Exception
            trans.Rollback()
            lblmsg.Text = ex.Message
            logger.Debug(ex.Message)
        Finally
            trans.Dispose()
            cn.Close()
            sorEnt = Nothing
            sorDao = Nothing


        End Try
    End Sub



    Private Sub DeleteSORRecordBySorId(ByVal SorId As String)
        Dim sql As String = "DELETE SOR WHERE SORID = " & SorId & ""

        cn = New SqlConnection(dm.getDBConn)
        If Not cn.State = ConnectionState.Open Then
            cn.Open()
        End If
        trans = cn.BeginTransaction

        Try
            dm.execTableInTrans(sql, cn, trans)
            trans.Commit()
            bindData()

        Catch ex As Exception
            lblmsg.Text = ex.Message
            trans.Rollback()
        Finally
            trans.Dispose()
            cn.Close()
        End Try
    End Sub
End Class
