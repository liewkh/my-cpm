Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports System.Web.HttpApplication

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class WebService
    Inherits System.Web.Services.WebService


    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function


    <WebMethod()> _
    Public Sub GenerateInvoiceMonthly()
        Dim sql As String = ""
        Dim j As Integer
        Dim invMgr As New InvoiceManager
        Dim retValue As String = ""
        Dim searchModel As New BillGenSearchModel
        Dim debtorSearchModel As New DebtorSearchModel
        Dim sqlmap As New SQLMap
        Dim dt, dtInv As New DataTable
        Dim passCard() As String = {""}
        Dim genFlag As Boolean = False
        Dim OSFlag As Boolean = False
        Dim RptYear As Integer        
        Dim BatchNo As String = ""
        Dim noOfInvoice As Integer = 0
        Dim generatedOnce As Boolean = False
        Dim dm As New DBManager
        Dim cn As SqlConnection
        Dim trans As SqlTransaction
        Dim lp As New LoginProfile
        Dim debtorEnt As New CPM.DebtorEntity
        Dim debtorDao As New CPM.DebtorDAO

        Dim logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Try
            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If

            trans = cn.BeginTransaction


            BatchNo = dm.getNextBatchNo(trans, cn)

            logger.Debug("Start Auto Generating Invoice By Monthly")
            logger.Debug("Batch No : " + BatchNo)
            debtorSearchModel.setStatus(DebtorStatusEnum.ACTIVE)

            'RptYear = DatePart(DateInterval.Year, DateAdd(DateInterval.Month, +1, Today))
            RptYear = Now.Year

            Dim strDate As New DateTime(Now.Year, Now.Month, 1)
            Dim nextMonth As String
            Dim passStr As String = ""
            Dim array As New ArrayList


            'If Now.Month <> 12 Then
            nextMonth = strDate.AddMonths(1).ToString("MMM")
            'Else
            '    nextMonth = strDate.AddMonths(0).ToString("MMM")
            'End If


            debtorSearchModel.setInvoicingFrequency(1)

            Dim category As String = System.Web.HttpContext.Current.Request.QueryString("Category")
            Dim prmDebtorId As String = System.Web.HttpContext.Current.Request.QueryString("DebtorId")
            Dim prmLocationInfoId As String = System.Web.HttpContext.Current.Request.QueryString("locationInfoId")

            If Not String.IsNullOrEmpty(category) Then
                debtorSearchModel.setCategory(category)
            End If

            If Not String.IsNullOrEmpty(prmDebtorId) Then
                debtorSearchModel.setDebtorId(prmDebtorId)
            End If

            If Not String.IsNullOrEmpty(prmLocationInfoId) Then
                debtorSearchModel.setLocationInfoId(prmLocationInfoId)
            End If


            Dim strDebtorSQL As String = sqlmap.getMappedStatement("Debtor/Search-Debtor", debtorSearchModel)
            dt = dm.execTable(strDebtorSQL)
            If dt.Rows.Count > 0 Then
                For Each row As DataRow In dt.Rows
                    'debtorEnt.setDebtorId(debtorDao.COLUMN_DebtorID)
                    searchModel.setDebtorId(row(debtorDao.COLUMN_DebtorID))                    
                    searchModel.setMonthFrom(DatePart(DateInterval.Year, DateAdd(DateInterval.Month, +1, Today)) & nextMonth & "01")
                    searchModel.setMonthto(DatePart(DateInterval.Year, DateAdd(DateInterval.Month, +1, Today)) & Now.AddMonths(2).ToString("MMM") & "01")
                    searchModel.setInvoicingFrequency(1)                     

                    If Not String.IsNullOrEmpty(category) Then
                        searchModel.setCategory(category)
                    End If

                    If Not String.IsNullOrEmpty(prmLocationInfoId) Then
                        searchModel.setLocationInfoId(prmLocationInfoId)
                    End If


                    Dim strSQL As String = sqlmap.getMappedStatement("BillGeneration/Generate-Invoice", searchModel)
                    dtInv = dm.execTableInTrans(strSQL, cn, trans)

                    'Reset Debtor BillGenerationCounter back to 0 if able to turn out the invoice meaning no outstandng amount
                    debtorSearchModel.setDebtorId(row(debtorDao.COLUMN_DebtorID))

                    If Not String.IsNullOrEmpty(category) Then
                        debtorSearchModel.setCategory(category)
                    End If

                    Dim strSQLCheckOSCounter As String = sqlmap.getMappedStatement("Debtor/Search-DebtorAutoBillGenOS", debtorSearchModel)
                    Dim dtOSCounter As DataTable = dm.execTableInTrans(strSQLCheckOSCounter, cn, trans)

                    Dim updateDebCounterSql As String = ""
                    If dtOSCounter.Rows.Count > 0 Then
                        updateDebCounterSql = "Update Debtor set BillGenerationCounter = BillGenerationCounter + 1 where DebtorId = " + row(debtorDao.COLUMN_DebtorID).ToString()
                        OSFlag = True
                        dm.execTable(updateDebCounterSql)
                        AuditOSDebtor(lp.getUserMstrId, row(debtorDao.COLUMN_DebtorID).ToString(), BatchNo, dtOSCounter.Rows(0).Item("amount").ToString(), cn, trans)
                        trans.Commit()
                        trans = cn.BeginTransaction
                    Else
                        updateDebCounterSql = "Update Debtor set BillGenerationCounter = 0 where DebtorId = " + row(debtorDao.COLUMN_DebtorID).ToString()
                        dm.execTable(updateDebCounterSql)
                        OSFlag = False
                    End If
                    'End of Reset Counter Check

                    If Not OSFlag Then
                        For j = 0 To dtInv.Rows.Count - 1
                            array.Add(dtInv.Rows(j).Item(0).ToString)
                            genFlag = True
                        Next j
                    End If

                    If genFlag Then
                        noOfInvoice += 1
                        'logger.Debug("Processing : " & noOfInvoice.ToString & "/" & ddDebtor.Items.Count.ToString)
                        retValue = invMgr.createInvoiceAuto(lp.getUserMstrId, row(debtorDao.COLUMN_DebtorID), Now.Month, array, BatchNo, RptYear, cn, trans)
                        trans.Commit()
                        trans = cn.BeginTransaction
                        genFlag = False
                        array.Clear()
                        generatedOnce = True
                    End If
                Next row
            End If


            logger.Debug("End Auto Generating Invoice By Monthly")

        Catch ex As Exception
            trans.Rollback()
            'lblMsg.Text = ex.Message
            logger.Debug(ex.Message)
        Finally
            trans.Dispose()
            invMgr = Nothing
            cn.Close()
        End Try

    End Sub

    <WebMethod()> _
    Public Sub GenerateInvoiceQuarterly()
        Dim sql As String = ""
        Dim j As Integer
        Dim invMgr As New InvoiceManager
        Dim retValue As String = ""
        Dim searchModel As New BillGenSearchModel
        Dim debtorSearchModel As New DebtorSearchModel
        Dim sqlmap As New SQLMap
        Dim dt, dtInv As New DataTable
        Dim passCard() As String = {""}
        Dim genFlag As Boolean = False
        Dim OSFlag As Boolean = False
        Dim RptYear As Integer
        Dim BatchNo As String = ""
        Dim noOfInvoice As Integer = 0
        Dim generatedOnce As Boolean = False
        Dim dm As New DBManager
        Dim cn As SqlConnection
        Dim trans As SqlTransaction
        Dim lp As New LoginProfile
        Dim debtorEnt As New CPM.DebtorEntity
        Dim debtorDao As New CPM.DebtorDAO

        Dim logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Try
            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If

            trans = cn.BeginTransaction


            BatchNo = dm.getNextBatchNo(trans, cn)

            logger.Debug("Start Auto Generating Invoice By Quarterly")
            logger.Debug("Batch No : " + BatchNo)
            debtorSearchModel.setStatus(DebtorStatusEnum.ACTIVE)

            'RptYear = DatePart(DateInterval.Year, DateAdd(DateInterval.Month, +1, Today))
            RptYear = Now.Year

            Dim strDate As New DateTime(Now.Year, Now.Month, 1)
            Dim nextMonth As String
            Dim passStr As String = ""
            Dim array As New ArrayList


            'If Now.Month <> 12 Then
            nextMonth = strDate.AddMonths(1).ToString("MMM")
            'Else
            '    nextMonth = strDate.AddMonths(0).ToString("MMM")
            'End If


            debtorSearchModel.setInvoicingFrequency(3)

            Dim category As String = System.Web.HttpContext.Current.Request.QueryString("Category")
            Dim prmDebtorId As String = System.Web.HttpContext.Current.Request.QueryString("DebtorId")
            Dim prmLocationInfoId As String = System.Web.HttpContext.Current.Request.QueryString("locationInfoId")

            If Not String.IsNullOrEmpty(category) Then
                debtorSearchModel.setCategory(category)
            End If

            If Not String.IsNullOrEmpty(prmDebtorId) Then
                debtorSearchModel.setDebtorId(prmDebtorId)
            End If

            If Not String.IsNullOrEmpty(prmLocationInfoId) Then
                debtorSearchModel.setLocationInfoId(prmLocationInfoId)
            End If


            Dim strDebtorSQL As String = sqlmap.getMappedStatement("Debtor/Search-Debtor", debtorSearchModel)
            dt = dm.execTable(strDebtorSQL)
            If dt.Rows.Count > 0 Then
                For Each row As DataRow In dt.Rows
                    'debtorEnt.setDebtorId(debtorDao.COLUMN_DebtorID)
                    searchModel.setDebtorId(row(debtorDao.COLUMN_DebtorID))
                    'searchModel.setMonthFrom(Now.Year & Now.AddMonths(0).ToString("MMM") & "01")
                    'searchModel.setMonthto(DatePart(DateInterval.Year, DateAdd(DateInterval.Month, +1, Today)) & nextMonth & "01")
                    searchModel.setMonthFrom(DatePart(DateInterval.Year, DateAdd(DateInterval.Month, +1, Today)) & nextMonth & "01")
                    searchModel.setMonthto(DatePart(DateInterval.Year, DateAdd(DateInterval.Month, +1, Today)) & Now.AddMonths(3).ToString("MMM") & "01")
                    searchModel.setInvoicingFrequency(3)

                    If Not String.IsNullOrEmpty(category) Then
                        searchModel.setCategory(category)
                    End If

                    If Not String.IsNullOrEmpty(prmLocationInfoId) Then
                        searchModel.setLocationInfoId(prmLocationInfoId)
                    End If


                    Dim strSQL As String = sqlmap.getMappedStatement("BillGeneration/Generate-Invoice", searchModel)
                    dtInv = dm.execTableInTrans(strSQL, cn, trans)

                    'Reset Debtor BillGenerationCounter back to 0 if able to turn out the invoice meaning no outstandng amount
                    debtorSearchModel.setDebtorId(row(debtorDao.COLUMN_DebtorID))

                    If Not String.IsNullOrEmpty(category) Then
                        debtorSearchModel.setCategory(category)
                    End If

                    Dim strSQLCheckOSCounter As String = sqlmap.getMappedStatement("Debtor/Search-DebtorAutoBillGenOS", debtorSearchModel)
                    Dim dtOSCounter As DataTable = dm.execTableInTrans(strSQLCheckOSCounter, cn, trans)

                    Dim updateDebCounterSql As String = ""
                    If dtOSCounter.Rows.Count > 0 Then
                        updateDebCounterSql = "Update Debtor set BillGenerationCounter = BillGenerationCounter + 1 where DebtorId = " + row(debtorDao.COLUMN_DebtorID).ToString()
                        OSFlag = True
                        dm.execTable(updateDebCounterSql)
                        AuditOSDebtor(lp.getUserMstrId, row(debtorDao.COLUMN_DebtorID).ToString(), BatchNo, dtOSCounter.Rows(0).Item("amount").ToString(), cn, trans)
                        trans.Commit()
                        trans = cn.BeginTransaction
                    Else
                        updateDebCounterSql = "Update Debtor set BillGenerationCounter = 0 where DebtorId = " + row(debtorDao.COLUMN_DebtorID).ToString()
                        dm.execTable(updateDebCounterSql)
                        OSFlag = False
                    End If
                    'End of Reset Counter Check

                    If Not OSFlag Then
                        For j = 0 To dtInv.Rows.Count - 1
                            array.Add(dtInv.Rows(j).Item(0).ToString)
                            genFlag = True
                        Next j
                    End If

                    If genFlag Then
                        noOfInvoice += 1
                        'logger.Debug("Processing : " & noOfInvoice.ToString & "/" & ddDebtor.Items.Count.ToString)
                        retValue = invMgr.createInvoiceAuto(lp.getUserMstrId, row(debtorDao.COLUMN_DebtorID), Now.Month, array, BatchNo, RptYear, cn, trans)
                        trans.Commit()
                        trans = cn.BeginTransaction
                        genFlag = False
                        array.Clear()
                        generatedOnce = True
                    End If
                Next row
            End If


            logger.Debug("End Auto Generating Invoice By Quarterly")

        Catch ex As Exception
            trans.Rollback()
            'lblMsg.Text = ex.Message
            logger.Debug(ex.Message)
        Finally
            trans.Dispose()
            invMgr = Nothing
            cn.Close()
        End Try

    End Sub

    Private Sub AuditOSDebtor(ByVal userId As String, ByVal debtorId As String, ByVal batchNo As String, ByVal OSAmount As String, ByRef cn As SqlConnection, ByRef trans As SqlTransaction)

        Dim logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Try

            'Audit AutoGenInvoice
            Dim autoBillGenerationEnt As New CPM.AutoBillGenerationEntity
            Dim autoBillGenerationDao As New CPM.AutoBillGenerationDAO

            autoBillGenerationEnt.setBatchNo(batchNo)
            autoBillGenerationEnt.setDebtorId(debtorId)
            autoBillGenerationEnt.setLastUpdatedBy(userId)
            autoBillGenerationEnt.setLastUpdatedDatetime(Now)
            autoBillGenerationEnt.setTxnDate(Now)
            autoBillGenerationEnt.setStatus("F")
            autoBillGenerationEnt.setErrorMessage("OS Amount : " & OSAmount)
            autoBillGenerationDao.insertDB(autoBillGenerationEnt, cn, trans)

        Catch ex As Exception
            trans.Rollback()
            logger.Debug(ex.Message)
        End Try
    End Sub

End Class
