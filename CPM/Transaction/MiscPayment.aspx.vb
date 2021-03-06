Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Transaction_MiscPayment
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Dim myDate As DateTime
    Dim myYear As Integer
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


            If Not (String.IsNullOrEmpty(Request.QueryString("debtorId"))) Then
                sql = "Select DEBTORID,NAME AS DEBTOR,0 as Seq From Debtor Where Status = '" & DebtorStatusEnum.ACTIVE & "'" & _
                     "And DebtorId = " & Request.QueryString("debtorId")
                ddDebtor.Enabled = False
                ddLocation.Enabled = False
                rbIndividual.Enabled = False
                rbCompany.Enabled = False
            Else
                sql = "Select DEBTORID,NAME AS DEBTOR,0 as Seq From Debtor Where Status = '" & DebtorStatusEnum.ACTIVE & "'" & _
                 "And LocationInfoId = " & lp.getDefaultLocationInfoId & " And Category='C' UNION ALL SELECT CODEMSTRID,CODEDESC,SEQ FROM CODEMSTR WHERE CODECAT = 'ALL'" & _
                 "ORDER BY SEQ,DEBTOR"
                ddDebtor.Enabled = True
                rbIndividual.Enabled = True
                ddLocation.Enabled = True
                rbCompany.Enabled = True
            End If
            

            dsDebtor.SelectCommand = sql
            dsDebtor.DataBind()


            sql = "select 0 as MISCPAYMENTTYPEMSTRID,codedesc as PAYMENTCODE,codedesc as PAYMENTDESC,0 as Amount from codemstr where codecat = 'DEFAULT' union "
            sql = sql + "Select MISCPAYMENTTYPEMSTRID,PAYMENTCODE,PAYMENTDESC,AMOUNT FROM MISCPAYMENTTYPEMSTR Where LocationInfoId = " & lp.getDefaultLocationInfoId & " and Active = 'Y'"

            dsMiscPaymentType.SelectCommand = sql
            dsMiscPaymentType.DataBind()

            If Not (String.IsNullOrEmpty(Request.QueryString("debtorId"))) Then
                Dim dt As New DataTable
                sql = "Select LocationInfoId from Debtor where DebtorId = " & Request.QueryString("debtorId")
                dt = dm.execTable(sql)
                If (dt.Rows.Count > 0) Then
                    ddLocation.SelectedValue = dt.Rows.Item(0).Item("LOCATIONINFOID")                    
                End If
            Else
                ddLocation.SelectedValue = lp.getDefaultLocationInfoId
            End If



            ddMiscPaymentType.SelectedIndex = 0
            bindCharges()

        End If

        If Not (String.IsNullOrEmpty(Request.QueryString("debtorId"))) Then
            chkVisitor.enabled = False
        Else
            chkVisitor.enabled = True
        End If



    End Sub


    Public Sub bindDropDown()        

        Dim Sql As String

        Try
            clear()

            Sql = "select 0 as MISCPAYMENTTYPEMSTRID,codedesc as PAYMENTCODE,codedesc as PAYMENTDESC,0 as Amount from codemstr where codecat = 'DEFAULT' union "
            Sql = Sql + "Select MISCPAYMENTTYPEMSTRID,PAYMENTCODE,PAYMENTDESC,AMOUNT FROM MISCPAYMENTTYPEMSTR Where LocationInfoId = " & ddLocation.SelectedValue & " and Active = 'Y'"

            dsMiscPaymentType.SelectCommand = Sql
            dsMiscPaymentType.DataBind()

            If (chkVisitor.Checked) Then
                ddDebtor.DataSource = Nothing
                ddDebtor.Items.Clear()
                ddDebtor.Items.Insert(0, New ListItem("Visitor", "Visitor"))
            Else
                bindDebtorDropDown()
            End If



        Catch ex As Exception
            logger.Debug(ex.Message)
            lblMsg.Text = ex.Message

        End Try

    End Sub


    Private Sub clear()
        lblMsg.Text = ""                       
        txtAmount.Text = ""
        txtQty.Text = ""        
        txtRemark.Text = ""
    End Sub


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub

    Protected Sub ddLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        bindDropDown()
    End Sub


    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim sql As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow


        Try
            
           
            If Not Page.IsValid Then
                Exit Sub
            End If

            If Not Utility.Tools.NumericValidation(txtQty.Text) Then
                lblMsg.Text = "Please enter numeric value for Qty."
                Exit Sub
            End If

            If String.IsNullOrEmpty(txtQty.Text) Then
                lblMsg.Text = "Qty is a Required field."
                Exit Sub
            End If


            If String.IsNullOrEmpty(Request.QueryString("debtorId")) Then
                If (ddDebtor.SelectedIndex = 0 And not chkVisitor.Checked) Then
                    lblMsg.Text = "Debtor is a Required field."
                    Exit Sub
                End If
            End If

            If ViewState("CurrentTable") Is Nothing Then
                dt.Columns.Add("MISCPAYMENTTYPEMSTRID")
                dt.Columns.Add("QTY")
                dt.Columns.Add("PAYMENTDESC")
                dt.Columns.Add("REMARK")
                dt.Columns.Add("AMOUNT")
                dt.Columns.Add("TOTAL")
                dr = dt.NewRow
                dr("MISCPAYMENTTYPEMSTRID") = ddMiscPaymentType.SelectedValue
                dr("PAYMENTDESC") = hdPaymentTypeDesc.Value
                dr("QTY") = Trim(txtQty.Text)
                dr("REMARK") = Trim(txtRemark.Text)
                dr("AMOUNT") = Trim(txtAmount.Text)
                dr("TOTAL") = CInt(txtQty.Text) * CInt(txtAmount.Text)
                dt.Rows.Add(dr)

                ViewState("CurrentTable") = dt
                gvMisc.DataSource = dt
                gvMisc.DataBind()
            Else
                dt = ViewState("CurrentTable")
                dr = dt.NewRow
                dr("MISCPAYMENTTYPEMSTRID") = ddMiscPaymentType.SelectedValue
                dr("PAYMENTDESC") = hdPaymentTypeDesc.Value
                dr("REMARK") = Trim(txtRemark.Text)
                dr("QTY") = Trim(txtQty.Text)
                dr("AMOUNT") = Trim(txtAmount.Text)
                dr("TOTAL") = CInt(txtQty.Text) * CInt(txtAmount.Text)
                dt.Rows.Add(dr)

                ViewState("CurrentTable") = dt
                gvMisc.DataSource = dt
                gvMisc.DataBind()

            End If

            If ViewState("CurrentTable") Is Nothing Then
                btnConfirm.Visible = False
                txtSubTotal.Visible = False
            Else
                btnConfirm.Visible = True
                txtSubTotal.Visible = True
                calcSubtotal()
                clear()
                ddDebtor.Enabled = False
                ddLocation.Enabled = False
                chkVisitor.Enabled = False
                rbIndividual.Enabled = False
                rbCompany.Enabled = False
            End If

            ddMiscPaymentType.SelectedIndex = 0




            'logger.Debug("End Generating Invoice")

        Catch ex As Exception
            trans.Rollback()
            lblMsg.Text = ex.Message
            logger.Debug(ex.Message)
        Finally
            'trans.Dispose()
            'cn.Close()
        End Try
    End Sub





    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        If Not (String.IsNullOrEmpty(Request.QueryString("debtorId"))) Then
            Dim dt As New DataTable
            Dim Sql As String = "Select LocationInfoId from Debtor where DebtorId = " & Request.QueryString("debtorId")
            dt = dm.execTable(Sql)
            If (dt.Rows.Count > 0) Then
                ddLocation.SelectedValue = dt.Rows.Item(0).Item("LOCATIONINFOID")
            End If
        Else
            ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        End If

        ddMiscPaymentType.SelectedIndex = 0
        lblMsg.Text = ""        
        txtAmount.Text = ""
        txtQty.Text = ""
        gvMisc.DataSource = Nothing
        gvMisc.DataBind()
        btnConfirm.Visible = False
        ViewState("CurrentTable") = Nothing
        txtSubTotal.Text = ""
        txtSubTotal.Visible = False
        txtRemark.Text = ""
        ddDebtor.Enabled = True
        ddLocation.Enabled = True
        chkVisitor.Enabled = True
        rbIndividual.Enabled = True
        rbCompany.Enabled = True
        chkVisitor.Checked = False

    End Sub

    Protected Sub ddMiscPaymentType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        clear()
        bindCharges()

    End Sub

    Private Sub bindCharges()
        Dim sql As String
        Dim dt As New DataTable

        Try

            sql = "Select AMOUNT,PAYMENTDESC FROM MISCPAYMENTTYPEMSTR Where MISCPAYMENTTYPEMSTRID = " & ddMiscPaymentType.SelectedValue

            dt = dm.execTable(sql)

            If (dt.Rows.Count > 0) Then                
                txtAmount.Text = dt.Rows.Item(0).Item("AMOUNT")
                hdPaymentTypeDesc.Value = dt.Rows.Item(0).Item("PAYMENTDESC")
            End If

        Catch ex As Exception

            lblMsg.Text = ex.Message
            logger.Debug(ex.Message)

        Finally


        End Try
    End Sub

    Protected Sub gvMisc_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)
        
        Dim dt As DataTable = ViewState("CurrentTable")

        dt.Rows(e.RowIndex).Delete()

        gvMisc.DataSource = dt
        gvMisc.DataBind()
        calcSubtotal()


    End Sub

    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt As DataTable = ViewState("CurrentTable")
        Dim miscPaymentEnt As New CPM.MiscPaymentEntity
        Dim miscPaymentDao As New CPM.MiscPaymentDAO
        Dim rcpNo As String
        Dim miscPaymentId As Long

        Try

            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If

            trans = cn.BeginTransaction

            rcpNo = dm.getReceiptNextRunningNo(ddLocation.SelectedValue, trans, cn)

            For Each row As DataRow In dt.Rows
                miscPaymentEnt.setReceiptNo(rcpNo)
                miscPaymentEnt.setQuantity(row.Item("QTY"))
                miscPaymentEnt.setMiscPaymentTypeMstrId(row.Item(miscPaymentDao.COLUMN_MiscPaymentTypeMstrId))
                miscPaymentEnt.setAmount(row.Item("TOTAL"))
                miscPaymentEnt.setUnitPrice(row.Item("AMOUNT"))
                miscPaymentEnt.setPaymentDate(Now)
                miscPaymentEnt.setRemark(row.Item("REMARK"))
                miscPaymentEnt.setLastUpdatedBy(lp.getUserMstrId)
                miscPaymentEnt.setLocationInfoId(ddLocation.SelectedValue)
                miscPaymentEnt.setLastUpdatedDatetime(Now)

                If Not (String.IsNullOrEmpty(Request.QueryString("debtorId"))) Then
                    miscPaymentEnt.setDebtorId(Trim(Request.QueryString("debtorId")))
                Else
                    If Not (chkVisitor.Checked) Then
                        miscPaymentEnt.setDebtorId(ddDebtor.SelectedValue)
                    End If
                End If

                miscPaymentId = miscPaymentDao.insertDB(miscPaymentEnt, cn, trans)
            Next row

            trans.Commit()

            hdRcpNo.Value = rcpNo
            hdSubTotal.Value = txtSubTotal.Text
            hdMiscPaymentId.Value = miscPaymentId


            'Clear the screen
            ddLocation.SelectedIndex = 0
            ddLocation.SelectedValue = lp.getDefaultLocationInfoId
            ddMiscPaymentType.SelectedIndex = 0
            lblMsg.Text = ""
            txtAmount.Text = ""
            txtQty.Text = ""
            gvMisc.DataSource = Nothing
            gvMisc.DataBind()
            btnConfirm.Visible = False
            ViewState("CurrentTable") = Nothing
            txtSubTotal.Text = ""
            txtSubTotal.Visible = False
            txtRemark.Text = ""

            PrintReceipt(hdMiscPaymentId.Value, ddDebtor.SelectedValue, hdSubTotal.Value, hdRcpNo.Value)
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopUp", "checkToPopUpViewer();", True)



        Catch ex As Exception
            trans.Rollback()
            lblMsg.Text = ex.Message
            logger.Debug(ex.Message)
        Finally
            trans.Dispose()
            cn.Close()
            miscPaymentEnt = Nothing
            miscPaymentDao = Nothing

        End Try

    End Sub

    Protected Sub calcSubtotal()
        Dim dt As DataTable = ViewState("CurrentTable")
        Dim subTotal As Double
        For Each row As DataRow In dt.Rows
            subTotal += row.Item("Total")
        Next row

        txtSubTotal.Text = String.Format("{0:n2}", subTotal)

    End Sub

    Private Sub PrintReceipt(ByVal miscPaymentId As Long, ByVal debtorId As String, ByVal Amt As String, ByVal RcpNo As String)
        Dim rptMgr As New ReportManager
        Dim sqlmap As New SQLMap
        Dim mySql As String = ""
        Dim dt As New DataTable
        Dim companyName As String = ""
        Dim companyAddress As String = ""
        Dim debtorName As String = ""
        Dim category As String = ""
        Dim passCard As String = ""

        Dim tel As String = ""
        Dim fax As String = ""
        Dim unitNo As String = ""
        Dim block As String = ""
        Dim duration As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim rcpPaymentFor As String = ""

        Dim hqInfoDao As New CPM.HQInfoDAO
        Dim debtorDao As New CPM.DebtorDAO
        'Dim passCardDao As New CPM.PassCardMstrDAO
        'Dim searchModel As New CPM.PassCardMstrEntity


        Try


            mySql = "SELECT COMPANYNAME,ADDRESS1,ADDRESS2,ADDRESS3,POSTCODE,TELEPHONE,FAX,(SELECT CODEDESC FROM CODEMSTR WHERE CODECAT='STA' AND CODEABBR= STATE) AS STATE FROM HQINFO"
            dt = dm.execTable(mySql)

            If dt.Rows.Count > 0 Then
                companyName = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_CompanyName)
                companyAddress = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address1) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address2) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Address3) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_PostCode) & vbCrLf & dt.Rows.Item(0).Item(hqInfoDao.COLUMN_State)
                tel = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Telephone)
                fax = dt.Rows.Item(0).Item(hqInfoDao.COLUMN_Fax)
            End If


            rptMgr.setReportName("MiscReceipt.Rpt")

            'rptMgr.setParameterDiscrete("CompanyName", companyName)
            'rptMgr.setParameterDiscrete("Address", companyAddress)

            rptMgr.setParameterDiscrete("TelephoneNo", tel)
            
            'rptMgr.setParameterDiscrete("Fax", fax)


            If Not (String.IsNullOrEmpty(debtorId)) Then
                Dim myDebtorSql As String = "SELECT * FROM DEBTOR WHERE DEBTORID = " & debtorId
                dt = dm.execTable(myDebtorSql)
                If dt.Rows.Count > 0 Then
                    debtorName = dt.Rows.Item(0).Item(debtorDao.COLUMN_Name)
                    category = dt.Rows.Item(0).Item(debtorDao.COLUMN_Category)
                Else
                    debtorName = "VISITOR"
                End If
            Else

            End If


            'searchModel.setDebtorId(debtorId)
            'Dim strSQL As String = sqlmap.getMappedStatement("Debtor/Search-DebtorPassBayNo", searchModel)

            'dt = dm.execTable(strSQL)
            'If dt.Rows.Count > 0 Then
            '    passCard = dt.Rows(0).Item(0).ToString
            'End If

            rptMgr.setParameterDiscrete("miscPaymentId", miscPaymentId)
            rptMgr.setParameterDiscrete("others", "")
            rptMgr.setParameterDiscrete("rm", Utility.Tools.SpellNumber(Amt))

            rptMgr.setParameterDiscrete("ReceiptNo", RcpNo)

            rptMgr.setParameterDiscrete("PrintedBy", lp.getUserLoginId)

            rptMgr.setParameterDiscrete("DebtorName", debtorName)


            'rptMgr.setParameterDiscrete("CardNo", passCard)
            'rptMgr.setParameterDiscrete("season", "")
            'rptMgr.setParameterDiscrete("reserved", "")
            'rptMgr.setParameterDiscrete("deposit", "")
            'rptMgr.setParameterDiscrete("seasonQty", "")
            'rptMgr.setParameterDiscrete("reservedQty", "")
            'rptMgr.setParameterDiscrete("depositQty", "")
            'rptMgr.setParameterDiscrete("othersQty", "")



            rptMgr.Logon()
            hdPreview.Value = "1"
            'set reportManager to session
            Session("ReportManager") = rptMgr
            lblMsg.Text = ""



        Catch ex As Exception
            lblMsg.Text = ex.Message

        Finally
            hqInfoDao = Nothing
            debtorDao = Nothing
            'passCardDao = Nothing
            dt = Nothing
            rptMgr = Nothing

        End Try

    End Sub


    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        If (hdRcpNo.Value <> "" And hdMiscPaymentId.Value <> "" And hdSubTotal.Value <> "") Then
            PrintReceipt(hdMiscPaymentId.Value, Request.QueryString("debtorId"), hdSubTotal.Value, hdRcpNo.Value)
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopUp", "checkToPopUpViewer();", True)
        End If
    End Sub

    Protected Sub chkVisitor_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If (chkVisitor.Checked) Then
            ddDebtor.DataSource = Nothing
            ddDebtor.Items.Clear()
            ddDebtor.Items.Insert(0, New ListItem("Visitor", "Visitor"))
            ddDebtor.Enabled = False
            rbIndividual.Enabled = False
            rbCompany.Enabled = False
        Else
            ddDebtor.Enabled = True
            rbIndividual.Enabled = True
            rbCompany.Enabled = True
            bindDebtorDropDown()
        End If

    End Sub


    Public Sub bindDebtorDropDown()
        Dim searchModel As New CPM.DebtorEntity
        Dim sqlmap As New SQLMap


        Try
            clear()

            If rbCompany.Checked = True Then
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
        bindDebtorDropDown()
    End Sub

    Protected Sub rbCompany_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        bindDebtorDropDown()
    End Sub

End Class
