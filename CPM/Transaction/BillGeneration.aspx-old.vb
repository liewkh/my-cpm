Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Transaction_BillGeneration
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

            sql = "Select DEBTORID,NAME AS DEBTOR,0 as Seq From Debtor Where Status = '" & DebtorStatusEnum.ACTIVE & "'" & _
                  "And LocationInfoId = " & lp.getDefaultLocationInfoId & " And Category='C' UNION ALL SELECT CODEMSTRID,CODEDESC,SEQ FROM CODEMSTR WHERE CODECAT = 'ALL'" & _
                  "ORDER BY SEQ,DEBTOR"

            dsDebtor.SelectCommand = sql
            dsDebtor.DataBind()

            ddMonth.SelectedValue = Now.Month
            ddLocation.SelectedValue = lp.getDefaultLocationInfoId

        End If

    End Sub


    Public Sub bindDropDown()
        Dim searchModel As New CPM.DebtorEntity
        Dim sqlmap As New SQLMap


        Try
            clear()

            If rbcompany.checked = True Then
                searchModel.setCategory(CategoryEnum.COMPANY)
            Else
                searchModel.setCategory(CategoryEnum.INDIVIDUAL)
            End If

            searchModel.setStatus(DebtorStatusEnum.ACTIVE)
            searchModel.setLocationInfoId(ddLocation.SelectedValue)

            If ddInvFreq.SelectedIndex > 0 Then
                searchModel.setInvoicingFrequency(ddInvFreq.SelectedValue)
            End If



            Dim strSQL As String = sqlmap.getMappedStatement("BillGeneration/Search-Debtor", searchModel)

            ViewState("strSQL") = strSQL


            dsDebtor.SelectCommand = ViewState("strSQL")
            ddDebtor.DataBind()

        Catch ex As Exception
            logger.Debug(ex.Message)
            lblMsg.Text = ex.Message

        End Try

    End Sub


    Private Sub clear()
        lblMsg.Text = ""
        lbl.Text = ""
        ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        ddInvFreq.SelectedIndex = 0
        ddMonth.SelectedIndex = 0
        ddDebtor.SelectedIndex = 0
    End Sub


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)        
        hdPreview.Value = ""
    End Sub

    Protected Sub ddLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        bindDropDown()
    End Sub

    Protected Sub rbIndividual_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        bindDropDown()
    End Sub

    Protected Sub rbCompany_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        bindDropDown()
    End Sub

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim sql As String = ""
        Dim i, j As Integer
        Dim invMgr As New InvoiceManager
        Dim retValue As String = ""
        Dim searchModel As New BillGenSearchModel
        Dim sqlmap As New SQLMap
        Dim dt As New DataTable
        Dim passCard() As String = {""}
        Dim genFlag As Boolean = False
        Dim RptYear As Integer
        Dim BatchNo As String = ""
        Dim noOfInvoice As Integer = 0
        Dim generatedOnce As Boolean = False

        Try
            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If

            trans = cn.BeginTransaction

            If ddInvFreq.SelectedIndex = 0 Then
                lblMsg.Text = "Please select a Invoicing Frequency."
                Exit Sub
            End If

            If ddMonth.SelectedIndex = 0 Then
                lblMsg.Text = "Please select a Month to generate the invoice."
                Exit Sub
            End If

            BatchNo = dm.getNextBatchNo(trans, cn)

            logger.Debug("Start Generating Invoice")
            '   System.Threading.Thread.Sleep(3000)
            lbl.Text = ""


            RptYear = DatePart(DateInterval.Year, DateAdd(DateInterval.Month, +1, Today))

            Dim strDate As New DateTime(Now.Year, ddMonth.SelectedValue.ToString, 1)
            Dim nextMonth As String
            Dim passStr As String = ""
            nextMonth = strDate.AddMonths(1).ToString("MMM")
            Dim array As New ArrayList

            If ddDebtor.SelectedIndex = 0 Then 'Generate for all the debtor inside the list
                For i = 0 To ddDebtor.Items.Count - 1
                    If i = 0 Then 'Skip for the first empty 
                        Continue For
                    End If
                    searchModel.setDebtorId(ddDebtor.Items(i).Value.ToString)
                    searchModel.setMonthFrom(Now.Year & ddMonth.SelectedValue.ToString & "01")
                    searchModel.setMonthto(Now.Year & nextMonth & "01")
                    searchModel.setInvoicingFrequency(ddInvFreq.SelectedValue)

                    Dim strSQL As String = sqlmap.getMappedStatement("BillGeneration/Generate-Invoice", searchModel)

                    dt = dm.execTableInTrans(strSQL, cn, trans)

                    For j = 0 To dt.Rows.Count - 1
                        array.Add(dt.Rows(j).Item(0).ToString)
                        genFlag = True
                    Next j

                    If genFlag Then
                        noOfInvoice += 1
                        'lbl.Text = "Processing : " & noOfInvoice.ToString & "/" & ddDebtor.Items.Count.ToString
                        'me.UpdatePanel1     
                        retValue = invMgr.createInvoice(ddDebtor.Items(i).Value.ToString, ddMonth.SelectedValue.ToString, array, BatchNo, cn, trans)
                        trans.Commit()
                        trans = cn.BeginTransaction
                        genFlag = False
                        array.Clear()
                        generatedOnce = True
                        'UpdatePanel1.Update()
                    End If

                Next

            Else 'Generate for selected debtor in the list
                searchModel.setDebtorId(ddDebtor.SelectedValue)
                searchModel.setMonthFrom(Now.Year & ddMonth.SelectedValue.ToString & "01")
                searchModel.setMonthto(Now.Year & nextMonth & "01")
                searchModel.setInvoicingFrequency(ddInvFreq.SelectedValue)

                Dim strSQL As String = sqlmap.getMappedStatement("BillGeneration/Generate-Invoice", searchModel)

                dt = dm.execTableInTrans(strSQL, cn, trans)

                For j = 0 To dt.Rows.Count - 1
                    array.Add(dt.Rows(j).Item(0).ToString)
                    'ReDim Preserve passCard(j)
                    'passCard(j) = dt.Rows(j).Item(0).ToString
                    'j = j + 1
                    genFlag = True
                Next j

                If genFlag Then
                    If Not (trans.Connection.State = ConnectionState.Open) Then
                        trans = cn.BeginTransaction
                    End If
                    noOfInvoice += 1                   
                    retValue = invMgr.createInvoice(ddDebtor.SelectedValue.ToString, ddMonth.SelectedValue.ToString, array, BatchNo, cn, trans)
                    trans.Commit()
                    array.Clear()
                    generatedOnce = True
                    genFlag = False

                End If

            End If
            'trans.Commit()

            lblMsg.Text = ""
            If generatedOnce Then
                lbl.Text = "Batch No : " & BatchNo & ". No Of Invoice Generated : " & noOfInvoice.ToString
            Else
                lbl.Text = "No Of Invoice Generated : " & noOfInvoice.ToString
            End If


            logger.Debug("End Generating Invoice")

        Catch ex As Exception
            trans.Rollback()
            lblMsg.Text = ex.Message
            logger.Debug(ex.Message)
        Finally
            trans.Dispose()
            invMgr = Nothing
            cn.Close()
        End Try
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ddLocation.SelectedIndex = 0
        ddDebtor.SelectedIndex = 0
        ddMonth.SelectedIndex = 0
        ddInvFreq.SelectedIndex = 0
        lbl.Text = ""
        lblMsg.Text = ""
    End Sub
End Class
