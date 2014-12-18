Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports PopCalendarSpecialDay

Public Class InvoiceManager
    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction

    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Dim invEnt As New CPM.InvoiceHistoryEntity
    Dim invDao As New CPM.InvoiceHistoryDAO
    Dim dahEnt As New CPM.DebtorAccountHeaderEntity
    Dim dahDao As New CPM.DebtorAccountHeaderDAO
    Dim dadEnt As New CPM.DebtorAccountDetailEntity
    Dim dadDao As New CPM.DebtorAccountDetailDAO

    Public Function createInvoice(ByVal userId As String, ByVal debtorId As String, ByVal month As String, ByVal passCardMstrId As ArrayList, ByVal batchNo As String, ByVal year As Integer, ByRef cn As SqlConnection, ByRef trans As SqlTransaction) As String
        Dim sql As String = ""
        Dim dt As New DataTable
        Dim debtorDao As New CPM.DebtorDAO
        Dim retValue As String = ""


        Try


            sql = "SELECT CATEGORY,LOCATIONINFOID,INVOICINGFREQUENCY,STATUS,INITIALHALFMONTH FROM DEBTOR WHERE DEBTORID = " & debtorId & _
                  " AND STATUS = '" & DebtorStatusEnum.ACTIVE & "'"

            dt = dm.execTable(sql)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0).Item(debtorDao.COLUMN_Status).Equals(DebtorStatusEnum.ACTIVE) Then
                    Dim duration As Integer = 0
                    duration = dt.Rows(0).Item(debtorDao.COLUMN_InvoicingFrequency)

                    'Find the nearest to quarter of calendar for the duration if = 3
                    If duration = 3 Then
                        Dim strDate As New DateTime(Now.Year, month, 1)
                        duration = 0

                        If Val(month) < 4 Then  '1'st Quarter
                            duration = 4 - Val(month)
                        ElseIf Val(month) < 7 Then '2'nd Quarter
                            duration = 7 - Val(month)
                        ElseIf Val(month) < 10 Then '3'rd Quarter
                            duration = 10 - Val(month)
                        ElseIf Val(month) <= 12 Then '4'th Quarter
                            duration = 13 - Val(month)
                        End If
                    End If

                    If dt.Rows(0).Item(debtorDao.COLUMN_InitialHalfMonth).Equals(ConstantGlobal.Yes) Then
                        retValue = processHalfMonth(userId, debtorId, dt.Rows(0).Item(debtorDao.COLUMN_Category), dt.Rows(0).Item(debtorDao.COLUMN_LocationInfoId), passCardMstrId, batchNo, month, duration, year, cn, trans)
                        sql = "UPDATE DEBTOR SET INITIALHALFMONTH = 'N',LASTUPDATEDDATETIME=getDate() WHERE DEBTORID = " & debtorId
                        dt = dm.execTableInTrans(sql, cn, trans)
                    Else
                        'todo if not half month
                        retValue = processMonth(userId, debtorId, dt.Rows(0).Item(debtorDao.COLUMN_Category), dt.Rows(0).Item(debtorDao.COLUMN_LocationInfoId), passCardMstrId, batchNo, month, duration, year, cn, trans)
                        sql = "UPDATE DEBTOR SET INITIALHALFMONTH = 'N',LASTUPDATEDDATETIME=getDate() WHERE DEBTORID = " & debtorId
                        dt = dm.execTableInTrans(sql, cn, trans)
                    End If
                End If
            Else
                Throw New Exception("Debtor Not Found.")
            End If


            Return retValue

        Catch ex As Exception
            logger.Error(ex.Message)
            Throw ex

        Finally
            debtorDao = Nothing
            dt = Nothing
        End Try
    End Function

    Private Function processHalfMonth(ByVal userId As String, ByVal debtorId As String, ByVal debtorCategory As String, ByVal locationInfoId As String, ByVal passCardMstrId As ArrayList, ByVal batchNo As String, ByVal whichMonth As String, ByVal duration As Integer, ByVal year As Integer, ByVal cn As SqlConnection, ByVal trans As SqlTransaction) As String

        Try
            Dim sql As String = ""
            Dim updateSql As String = ""
            Dim dt As New DataTable
            Dim i, a, b As Integer
            Dim debtorDao As New CPM.DebtorDAO
            Dim invDuration As String = ""
            Dim lastInvoice As Date
            Dim RptYear As Integer
            Dim halfIndicator As Double = 0.5
            Dim retInvoiceNo As String = ""
            Dim locationMsg1, locationMsg2, locationMsg3 As String

            locationMsg1 = dm.getFieldById("LOCATIONINFO", locationInfoId, "LOCATIONMESSAGE1").ToString
            locationMsg2 = dm.getFieldById("LOCATIONINFO", locationInfoId, "LOCATIONMESSAGE2").ToString
            locationMsg3 = dm.getFieldById("LOCATIONINFO", locationInfoId, "LOCATIONMESSAGE3").ToString

            'If Now.Day >= 15 Then


            'RptYear = DatePart(DateInterval.Year, DateAdd(DateInterval.Month, +1, Today))
            RptYear = year

            'Dim strDate As New DateTime(Now.Year, whichMonth, 15)
            Dim strDate As New DateTime(RptYear, whichMonth, 15)
            Dim nextMonth As String
            Dim passStr As String = ""

            If whichMonth <> 12 Then
                nextMonth = strDate.AddMonths(1).ToString("MMM")
            Else
                nextMonth = strDate.AddMonths(0).ToString("MMM")
            End If

            For z As Integer = 0 To passCardMstrId.Count - 1
                If passCardMstrId(z) <> "" Then
                    passStr = passStr & passCardMstrId(z) & ","
                End If

            Next

            passStr = passStr.Substring(0, passStr.Length - 1)


            'Check Month Record In Invoice History
            sql = "select count(IH.InvoiceHistoryId) as CNT from InvoiceHistory IH,InvoiceHistoryDetail IHD,DebtorAccountHeader dah " & _
                  "where IH.DebtorAccountHeaderId =  dah.DebtorAccountHeaderId " & _
                  "and ihd.DebtorAccountHeaderId = dah.DebtorAccountHeaderId And IH.month >= '" & RptYear & _
                   whichMonth & "01' AND IH.month <= '" & RptYear & nextMonth & "01'" & _
                   " and IH.debtorid =" & debtorId & _
                   " and IHD.PassCardMstrId in (" & passStr & ")"

            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            dt1 = dm.execTable(sql)


            If dt1.Rows(0).Item("CNT") > 0 Then
                'do nothing
            Else
                'Create new record for invoicehistory
                Dim tempLastInvoiceMonth As Date = strDate

                For b = 1 To duration 'eg if 5 then generate 5 months invoice history
                    invDuration = invDuration + MonthName(DatePart(DateInterval.Month, DateAdd(DateInterval.Month, +0, tempLastInvoiceMonth)), True) + "-"
                    tempLastInvoiceMonth = tempLastInvoiceMonth.AddMonths(1)
                Next
                'RptYear = DatePart(DateInterval.Year, tempLastInvoiceMonth.AddMonths(-1))
                invDuration = invDuration & RptYear


                'Create Master
                'Enter Header Transaction
                dahEnt.setDebtorId(debtorId)
                dahEnt.setLocationMessage1(locationMsg1)
                dahEnt.setLocationMessage2(locationMsg2)
                dahEnt.setLocationMessage3(locationMsg3)
                dahEnt.setInvoiceNo(dm.getNextRunningNo(debtorCategory, locationInfoId, trans, cn))
                retInvoiceNo = dahEnt.getInvoiceNo
                dahEnt.setInvoiceDate(Now.ToShortDateString)
                dahEnt.setInvoicePeriod(invDuration)
                dahEnt.setLastUpdatedBy(userId)
                dahEnt.setLastUpdatedDatetime(Now)
                dahEnt.setStatus(InvoiceStatusEnum.OUTSTANDING)
                dahEnt.setBatchNo(batchNo)
                dahEnt.setTxnType(TxnTypeEnum.INVOICE)
                Dim dahId As Long = dahDao.insertDB(dahEnt, cn, trans)
                Dim invHistId As Long


                'Create Detail Transaction 2 tables :- DebtorAccountDetail,InvoiceHistoryDeatil        
                sql = " select d.category,d.locationinfoid,stm.seasontypedesc,stm.amount as unitprice,count(stm.seasontypemstrid) as qty, " & _
                      " stm.seasonduration, stm.other from debtorpassbay dbp,debtor d,seasontypemstr stm " & _
                      " where dbp.debtorid = d.debtorid " & _
                      " and stm.seasontypemstrid = dbp.seasontypemstrid " & _
                      " and dbp.debtorid = " & debtorId & _
                      " and dbp.passcardmstrid in (" & passStr & ")" & _
                      " and dbp.Status = 'A' " & _
                      " group by d.locationinfoid,stm.seasontypedesc,stm.seasonduration,stm.amount,stm.other,d.category " & _
                      " order by stm.seasontypedesc"

                dt2 = dm.execTable(sql)

                If dt2.Rows.Count = 0 Then
                    Throw New Exception("Invalid Selection Combination")
                End If

                Dim invAmount As Double = 0

                For i = 0 To dt2.Rows.Count - 1
                    'Create Detail Transaction'
                    dadEnt.setDebtorAccountHeaderId(dahId)
                    dadEnt.setMonths(duration - halfIndicator)
                    dadEnt.setDetails(dt2.Rows(i).Item("SEASONTYPEDESC"))
                    dadEnt.setUnitPrice(dt2.Rows(i).Item("UNITPRICE"))
                    dadEnt.setQuantity(dt2.Rows(i).Item("QTY"))
                    dadEnt.setAmount(dadEnt.getMonths * (dt2.Rows(i).Item("QTY") * dadEnt.getUnitPrice))
                    dadEnt.setTaxCode(dm.getTaxCode(locationInfoId, ConstantGlobal.StandardRated, trans, cn))
                    dadEnt.setLastUpdatedBy(userId)
                    dadEnt.setLastUpdatedDatetime(Now)
                    dadDao.insertDB(dadEnt, cn, trans)
                    invAmount += (dadEnt.getMonths * (dt2.Rows(i).Item("QTY") * dadEnt.getUnitPrice))
                Next

                'Throw New Exception("Test")

                'Create Tax If Applicable 09/03/2010
                Dim taxSql As String = "SELECT TAX,TAXDESCRIPTION FROM HQINFO"
                Dim dtTax = dm.execTable(taxSql)
                Dim taxAmt As Double = 0
                Dim taxDesc As String = ""

                If dtTax.Rows.Count > 0 Then
                    If dtTax.Rows.Item(0).Item("TAX") > 0 Then
                        taxAmt = dtTax.Rows.Item(0).Item("TAX")
                        taxDesc = dtTax.Rows.Item(0).Item("TAXDESCRIPTION")
                        dadEnt.setDebtorAccountHeaderId(dahId)
                        dadEnt.setMonths(0)
                        dadEnt.setDetails(taxDesc)
                        dadEnt.setUnitPrice(0)
                        dadEnt.setQuantity(0)
                        'dadEnt.setAmount(taxAmt * invAmount / 100)
                        dadEnt.setAmount(roundingAdjustment(taxAmt * invAmount / 100))
                        dadEnt.setTaxCode(dm.getTaxCode(locationInfoId, ConstantGlobal.ZeroRated, trans, cn))
                        dadEnt.setLastUpdatedBy(userId)
                        dadEnt.setLastUpdatedDatetime(Now)
                        dadDao.insertDB(dadEnt, cn, trans)
                        invAmount += dadEnt.getAmount
                    End If
                End If



                'Create Deposit Different
                Dim depositSql As String = "SELECT COUNT(S.SEASONTYPEMSTRID) AS QTY,SUM(ISNULL(PCM.DEPOSIT,0)) AS DEPOSIT,S.SEASONTYPEDESC,PCM.DEPOSIT AS UNITPRICE " & _
                " FROM PASSCARDMSTR PCM,DEBTORPASSBAY DPB,SEASONTYPEMSTR S " & _
                " WHERE PCM.PASSCARDMSTRID IN (" & passStr & ")" & _
                " AND PCM.PASSCARDMSTRID = DPB.PASSCARDMSTRID " & _
                " AND S.SEASONTYPEMSTRID = DPB.SEASONTYPEMSTRID " & _
                " AND DPB.DEBTORID = " & debtorId & _
                " and DPB.Status = 'A' " & _
                " AND PCM.DEPOSITPRINT = '" & ConstantGlobal.Yes & "'" & _
                " GROUP BY S.SEASONTYPEMSTRID,S.SEASONTYPEDESC,PCM.DEPOSIT"


                Dim dtDeposit As New DataTable
                Dim depositAmount As Long = 0

                dtDeposit = dm.execTableInTrans(depositSql, cn, trans)
                If dtDeposit.Rows.Count > 0 Then
                    For i = 0 To dtDeposit.Rows.Count - 1
                        dadEnt.setDebtorAccountHeaderId(dahId)
                        dadEnt.setMonths("")
                        dadEnt.setDetails("Deposit - " & dtDeposit.Rows(i).Item("SEASONTYPEDESC"))
                        dadEnt.setUnitPrice(dtDeposit.Rows(i).Item("UNITPRICE"))
                        dadEnt.setQuantity(dtDeposit.Rows(i).Item("QTY"))
                        dadEnt.setAmount(dtDeposit.Rows(i).Item("DEPOSIT"))
                        dadEnt.setTaxCode(dm.getTaxCode(locationInfoId, ConstantGlobal.ZeroRated, trans, cn))
                        dadEnt.setLastUpdatedBy(userId)
                        dadEnt.setLastUpdatedDatetime(Now)
                        dadDao.insertDB(dadEnt, cn, trans)
                        depositAmount += dtDeposit.Rows(i).Item("DEPOSIT")
                        invAmount += dadEnt.getAmount
                    Next
                End If

                updateSql = "UPDATE PASSCARDMSTR SET DEPOSITPRINT = '" & ConstantGlobal.No & "',LASTUPDATEDDATETIME=getDate() WHERE PASSCARDMSTRID IN (" & passStr & ")"
                dm.execTableInTrans(updateSql, cn, trans)


                Dim actualSeasonInvAmt As Double = invAmount - depositAmount
                Dim actualSeasonMonthlyAmt As Double = actualSeasonInvAmt / duration
                Dim chkAmt As Double = actualSeasonMonthlyAmt * duration
                Dim differentAmt As Double = actualSeasonInvAmt - chkAmt

                'Create InvoiceHistory
                For a = 1 To duration
                    lastInvoice = strDate
                    invEnt.setDebtorId(debtorId)
                    invEnt.setDebtorAccountHeaderId(dahId)
                    invEnt.setStatus(InvoiceStatusEnum.OUTSTANDING)
                    invEnt.setMonth(lastInvoice)

                    If depositAmount > 0 And a = 1 Then
                        invEnt.setAmount(depositAmount + actualSeasonMonthlyAmt + differentAmt)
                    Else
                        invEnt.setAmount(actualSeasonMonthlyAmt)
                    End If

                    'invEnt.setAmount(invAmount / duration)
                    invEnt.setLastUpdatedBy(userId)
                    invEnt.setLastUpdatedDatetime(Now)
                    invHistId = invDao.insertDB(invEnt, cn, trans)
                    strDate = DateAdd(DateInterval.Month, +1, strDate)
                Next a

                'Update Invoice Header total amount
                dahEnt.setDebtorAccountHeaderId(dahId)
                'dahEnt.setAmount(invAmount)
                dahEnt.setAmount(roundingAdjustment(invAmount))
                dahEnt.setLastUpdatedBy(userId)
                dahEnt.setLastUpdatedDatetime(Now.AddMilliseconds(1))
                dahDao.updateDB(dahEnt, cn, trans)

                'Create InvoiceHistoryDetail
                Dim ihdEnt As New CPM.InvoiceHistoryDetailEntity
                Dim ihdDao As New CPM.InvoiceHistoryDetailDAO

                For x As Integer = 0 To passCardMstrId.Count - 1
                    passStr = passCardMstrId(x)
                    If passStr <> "" Then
                        ihdEnt.setDebtorAccountHeaderId(dahId)
                        ihdEnt.setLastUpdatedBy(userId)
                        ihdEnt.setLastUpdatedDatetime(Now)
                        ihdEnt.setPassCardMstrId(passCardMstrId(x))
                        ihdDao.insertDB(ihdEnt, cn, trans)
                    End If

                Next

                checkMismatchAmount(dahId, cn, trans)

            End If

                'End If
                'System.Threading.Thread.Sleep(20) 'Too fast update casuing dirty data buffer
                Return retInvoiceNo

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function processMonth(ByVal userId As String, ByVal debtorId As String, ByVal debtorCategory As String, ByVal locationInfoId As String, ByVal passCardMstrId As ArrayList, ByVal batchNo As String, ByVal whichMonth As String, ByVal duration As Integer, ByVal year As Integer, ByVal cn As SqlConnection, ByVal trans As SqlTransaction) As String
        Try
            Dim sql As String = ""
            Dim updateSql As String = ""
            Dim dt As New DataTable
            Dim i, a, b As Integer
            Dim debtorDao As New CPM.DebtorDAO
            Dim invDuration As String = ""
            Dim lastInvoice As Date
            Dim RptYear As Integer
            Dim retInvoiceNo As String = ""
            Dim locationMsg1, locationMsg2, locationMsg3 As String

            locationMsg1 = dm.getFieldById("LOCATIONINFO", locationInfoId, "LOCATIONMESSAGE1").ToString
            locationMsg2 = dm.getFieldById("LOCATIONINFO", locationInfoId, "LOCATIONMESSAGE2").ToString
            locationMsg3 = dm.getFieldById("LOCATIONINFO", locationInfoId, "LOCATIONMESSAGE3").ToString

            'RptYear = DatePart(DateInterval.Year, DateAdd(DateInterval.Month, +1, Today))
            RptYear = year

            'Dim strDate As New DateTime(Now.Year, whichMonth, 1)
            Dim strDate As New DateTime(RptYear, whichMonth, 1)
            Dim nextMonth As String
            Dim passStr As String = ""

            If whichMonth <> 12 Then
                nextMonth = strDate.AddMonths(1).ToString("MMM")
            Else
                nextMonth = strDate.AddMonths(0).ToString("MMM")
            End If

            For z As Integer = 0 To passCardMstrId.Count - 1
                If passCardMstrId(z) <> "" Then
                    passStr = passStr & passCardMstrId(z) & ","
                End If

            Next

            passStr = passStr.Substring(0, passStr.Length - 1)


            'Check Month Record In Invoice History
            sql = "select count(IH.InvoiceHistoryId) as CNT from InvoiceHistory IH,InvoiceHistoryDetail IHD,DebtorAccountHeader dah " & _
                  "where IH.DebtorAccountHeaderId =  dah.DebtorAccountHeaderId " & _
                  "and ihd.DebtorAccountHeaderId = dah.DebtorAccountHeaderId And IH.month >= '" & RptYear & _
                   whichMonth & "01' AND IH.month <= '" & RptYear & nextMonth & "01'" & _
                   " and IH.debtorid =" & debtorId & _
                   " and IHD.PassCardMstrId in (" & passStr & ")"

            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            dt1 = dm.execTable(sql)


            If dt1.Rows(0).Item("CNT") > 0 Then
                'do nothing
            Else
                'Create new record for invoicehistory
                Dim tempLastInvoiceMonth As Date = strDate

                For b = 1 To duration 'eg if 5 then generate 5 months invoice history
                    invDuration = invDuration + MonthName(DatePart(DateInterval.Month, DateAdd(DateInterval.Month, +0, tempLastInvoiceMonth)), True) + "-"
                    tempLastInvoiceMonth = tempLastInvoiceMonth.AddMonths(1)
                Next
                'RptYear = DatePart(DateInterval.Year, tempLastInvoiceMonth.AddMonths(-1))
                invDuration = invDuration & RptYear


                'Create Master
                'Enter Header Transaction
                dahEnt.setDebtorId(debtorId)
                dahEnt.setLocationMessage1(locationMsg1)
                dahEnt.setLocationMessage2(locationMsg2)
                dahEnt.setLocationMessage3(locationMsg3)
                dahEnt.setInvoiceNo(dm.getNextRunningNo(debtorCategory, locationInfoId, trans, cn))
                retInvoiceNo = dahEnt.getInvoiceNo
                dahEnt.setInvoiceDate(Now.ToShortDateString)
                dahEnt.setInvoicePeriod(invDuration)
                dahEnt.setLastUpdatedBy(userId)
                dahEnt.setLastUpdatedDatetime(Now)
                dahEnt.setStatus(InvoiceStatusEnum.OUTSTANDING)
                dahEnt.setBatchNo(batchNo)
                dahEnt.setTxnType(TxnTypeEnum.INVOICE)
                Dim dahId As Long = dahDao.insertDB(dahEnt, cn, trans)
                Dim invHistId As Long


                'Create Detail Transaction 2 tables :- DebtorAccountDetail,InvoiceHistoryDeatil        
                sql = " select d.category,d.locationinfoid,stm.seasontypedesc,stm.amount as unitprice,count(stm.seasontypemstrid) as qty, " & _
                      " sum(pcm.deposit) as deposit,stm.seasonduration, stm.other from debtorpassbay dbp,debtor d,seasontypemstr stm,passcardmstr pcm " & _
                      " where dbp.debtorid = d.debtorid " & _
                      " and stm.seasontypemstrid = dbp.seasontypemstrid " & _
                      " and pcm.passCardMstrid = dbp.passCardMstrid " & _
                      " and dbp.debtorid = " & debtorId & _
                      " and dbp.passcardmstrid in (" & passStr & ")" & _
                      " and dbp.Status = 'A' " & _
                      " group by d.locationinfoid,stm.seasontypedesc,stm.seasonduration,stm.amount,stm.other,d.category " & _
                      " order by stm.seasontypedesc"

                dt2 = dm.execTable(sql)

                If dt2.Rows.Count = 0 Then
                    Throw New Exception("Invalid Selection Combination")
                End If

                Dim invAmount As Double = 0

                For i = 0 To dt2.Rows.Count - 1
                    'Create Detail Transaction'
                    dadEnt.setDebtorAccountHeaderId(dahId)
                    dadEnt.setMonths(duration)
                    dadEnt.setDetails(dt2.Rows(i).Item("SEASONTYPEDESC"))
                    dadEnt.setUnitPrice(dt2.Rows(i).Item("SEASONDURATION") * dt2.Rows(i).Item("UNITPRICE"))
                    dadEnt.setQuantity(dt2.Rows(i).Item("QTY"))
                    dadEnt.setAmount(duration * (dt2.Rows(i).Item("QTY") * dadEnt.getUnitPrice))
                    dadEnt.setTaxCode(dm.getTaxCode(locationInfoId, ConstantGlobal.StandardRated, trans, cn))
                    dadEnt.setLastUpdatedBy(userId)
                    dadEnt.setLastUpdatedDatetime(Now)
                    dadDao.insertDB(dadEnt, cn, trans)
                    invAmount += duration * (dt2.Rows(i).Item("QTY") * dt2.Rows(i).Item("UNITPRICE"))
                Next

                'Create Tax If Applicable 09/03/2010
                Dim taxSql As String = "SELECT TAX,TAXDESCRIPTION FROM HQINFO"
                Dim dtTax = dm.execTable(taxSql)
                Dim taxAmt As Double = 0
                Dim taxDesc As String = ""

                If dtTax.Rows.Count > 0 Then
                    If dtTax.Rows.Item(0).Item("TAX") > 0 Then
                        taxAmt = dtTax.Rows.Item(0).Item("TAX")
                        taxDesc = dtTax.Rows.Item(0).Item("TAXDESCRIPTION")
                        dadEnt.setDebtorAccountHeaderId(dahId)
                        dadEnt.setMonths(0)
                        dadEnt.setDetails(taxDesc)
                        dadEnt.setUnitPrice(0)
                        dadEnt.setQuantity(0)
                        'dadEnt.setAmount(taxAmt * invAmount / 100)
                        dadEnt.setAmount(roundingAdjustment(taxAmt * invAmount / 100))
                        dadEnt.setTaxCode(dm.getTaxCode(locationInfoId, ConstantGlobal.ZeroRated, trans, cn))
                        dadEnt.setLastUpdatedBy(userId)
                        dadEnt.setLastUpdatedDatetime(Now)
                        dadDao.insertDB(dadEnt, cn, trans)
                        invAmount += dadEnt.getAmount
                    End If
                End If


                'Create Deposit Different
                Dim depositSql As String = "SELECT COUNT(S.SEASONTYPEMSTRID) AS QTY,SUM(ISNULL(PCM.DEPOSIT,0)) AS DEPOSIT,S.SEASONTYPEDESC,PCM.DEPOSIT AS UNITPRICE " & _
                " FROM PASSCARDMSTR PCM,DEBTORPASSBAY DPB,SEASONTYPEMSTR S " & _
                " WHERE PCM.PASSCARDMSTRID IN (" & passStr & ")" & _
                " AND PCM.PASSCARDMSTRID = DPB.PASSCARDMSTRID " & _
                " AND S.SEASONTYPEMSTRID = DPB.SEASONTYPEMSTRID " & _
                " AND DPB.DEBTORID = " & debtorId & _
                " and DPB.Status = 'A' " & _
                " AND PCM.DEPOSITPRINT = '" & ConstantGlobal.Yes & "'" & _
                " GROUP BY S.SEASONTYPEMSTRID,S.SEASONTYPEDESC,PCM.DEPOSIT"


                Dim dtDeposit As New DataTable
                Dim depositAmount As Long = 0

                dtDeposit = dm.execTableInTrans(depositSql, cn, trans)
                If dtDeposit.Rows.Count > 0 Then
                    For i = 0 To dtDeposit.Rows.Count - 1
                        dadEnt.setDebtorAccountHeaderId(dahId)
                        dadEnt.setMonths("")
                        dadEnt.setDetails("Deposit - " & dtDeposit.Rows(i).Item("SEASONTYPEDESC"))
                        dadEnt.setUnitPrice(dtDeposit.Rows(i).Item("UNITPRICE"))
                        dadEnt.setQuantity(dtDeposit.Rows(i).Item("QTY"))
                        dadEnt.setAmount(dtDeposit.Rows(i).Item("DEPOSIT"))
                        dadEnt.setTaxCode(dm.getTaxCode(locationInfoId, ConstantGlobal.ZeroRated, trans, cn))
                        dadEnt.setLastUpdatedBy(userId)
                        dadEnt.setLastUpdatedDatetime(Now)
                        dadDao.insertDB(dadEnt, cn, trans)
                        depositAmount += dtDeposit.Rows(i).Item("DEPOSIT")
                        invAmount += dadEnt.getAmount
                    Next
                End If

                updateSql = "UPDATE PASSCARDMSTR SET DEPOSITPRINT = '" & ConstantGlobal.No & "',LASTUPDATEDDATETIME=getDate() WHERE PASSCARDMSTRID IN (" & passStr & ")"
                dm.execTableInTrans(updateSql, cn, trans)

                ' Throw New Exception("Test")

                Dim actualSeasonInvAmt As Double = invAmount - depositAmount
                Dim actualSeasonMonthlyAmt As Double = actualSeasonInvAmt / duration
                Dim chkAmt As Double = actualSeasonMonthlyAmt * duration
                Dim differentAmt As Double = actualSeasonInvAmt - chkAmt

                'Create InvoiceHistory
                For a = 1 To duration
                    lastInvoice = strDate
                    invEnt.setDebtorId(debtorId)
                    invEnt.setDebtorAccountHeaderId(dahId)
                    invEnt.setStatus(InvoiceStatusEnum.OUTSTANDING)
                    invEnt.setMonth(lastInvoice)

                    If depositAmount > 0 And a = 1 Then
                        invEnt.setAmount(depositAmount + actualSeasonMonthlyAmt + differentAmt)
                    Else
                        invEnt.setAmount(actualSeasonMonthlyAmt)
                    End If

                    'invEnt.setAmount(invAmount / duration)
                    invEnt.setLastUpdatedBy(userId)
                    invEnt.setLastUpdatedDatetime(Now)
                    invHistId = invDao.insertDB(invEnt, cn, trans)
                    strDate = DateAdd(DateInterval.Month, +1, strDate)
                Next a


                'Update Invoice Header total amount
                dahEnt.setDebtorAccountHeaderId(dahId)
                '                dahEnt.setAmount(invAmount)
                dahEnt.setAmount(roundingAdjustment(invAmount))
                dahEnt.setLastUpdatedBy(userId)
                dahEnt.setLastUpdatedDatetime(Now.AddMilliseconds(1))
                dahDao.updateDB(dahEnt, cn, trans)

                'Create InvoiceHistoryDetail
                Dim ihdEnt As New CPM.InvoiceHistoryDetailEntity
                Dim ihdDao As New CPM.InvoiceHistoryDetailDAO

                For x As Integer = 0 To passCardMstrId.Count - 1
                    passStr = passCardMstrId(x)
                    If passStr <> "" Then
                        ihdEnt.setDebtorAccountHeaderId(dahId)
                        ihdEnt.setLastUpdatedBy(userId)
                        ihdEnt.setLastUpdatedDatetime(Now)
                        ihdEnt.setPassCardMstrId(passCardMstrId(x))
                        ihdDao.insertDB(ihdEnt, cn, trans)
                    End If

                Next

                End If

                'System.Threading.Thread.Sleep(20) 'Too fast update casuing dirty data buffer
                Return retInvoiceNo

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function roundingAdjustment(ByVal TaxAmount As Double) As Double
        Dim ModAmount As Double = 0
        Dim AddUpAmount As Double = 0
        Dim NewTaxAmount As Double = 0

        Try
            ModAmount = TaxAmount Mod 0.05
            If ModAmount > 0.02 Then
                AddUpAmount = 0.05 - ModAmount
                NewTaxAmount = TaxAmount + AddUpAmount
            Else
                'NewTaxAmount = TaxAmount.ToString.Substring(0, TaxAmount.ToString.LastIndexOf(".") + 2)
		NewTaxAmount = TaxAmount - ModAmount
            End If

            Return NewTaxAmount

        Catch ex As Exception
            Throw ex

        End Try

    End Function

    Public Function createInvoiceAuto(ByVal userId As String, ByVal debtorId As String, ByVal month As String, ByVal passCardMstrId As ArrayList, ByVal batchNo As String, ByVal year As Integer, ByRef cn As SqlConnection, ByRef trans As SqlTransaction) As String
        Dim sql As String = ""
        Dim dt As New DataTable
        Dim debtorDao As New CPM.DebtorDAO
        Dim retValue As String = ""
        Dim debtorSearchModel As New DebtorSearchModel
        Dim sqlmap As New SQLMap

        Try


            sql = "SELECT CATEGORY,LOCATIONINFOID,INVOICINGFREQUENCY,STATUS,INITIALHALFMONTH FROM DEBTOR WHERE DEBTORID = " & debtorId & _
                  " AND STATUS = '" & DebtorStatusEnum.ACTIVE & "'"

            dt = dm.execTable(sql)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0).Item(debtorDao.COLUMN_Status).Equals(DebtorStatusEnum.ACTIVE) Then
                    Dim duration As Integer = 0
                    duration = dt.Rows(0).Item(debtorDao.COLUMN_InvoicingFrequency)

                    'Find the nearest to quarter of calendar for the duration if = 3
                    If duration = 3 Then

                        Dim strDate As New DateTime(Now.Year, month, 1)
                        duration = 0

                        If DateTime.Parse("1." + month + " 2008").Month <> 12 Then
                            month = strDate.AddMonths(1).ToString("MM")
                        Else
                            month = strDate.AddMonths(0).ToString("MM")
                        End If

                        If Val(month) < 4 Then  '1'st Quarter
                            duration = 4 - Val(month)
                        ElseIf Val(month) < 7 Then '2'nd Quarter
                            duration = 7 - Val(month)
                        ElseIf Val(month) < 10 Then '3'rd Quarter
                            duration = 10 - Val(month)
                        ElseIf Val(month) <= 12 Then '4'th Quarter
                            duration = 13 - Val(month)
                        End If

                        If Now.Month = 12 Then
                            month = strDate.AddMonths(0).ToString("MM")
                        End If

                    End If


                    Try

                        If duration = 3 Then 'For Querterly cases
                            month = Convert.ToString(Val(month) - 1)
                        End If

                        If dt.Rows(0).Item(debtorDao.COLUMN_InitialHalfMonth).Equals(ConstantGlobal.Yes) Then
                            retValue = processHalfMonthAuto(userId, debtorId, dt.Rows(0).Item(debtorDao.COLUMN_Category), dt.Rows(0).Item(debtorDao.COLUMN_LocationInfoId), passCardMstrId, batchNo, MonthName(month), duration, year, cn, trans)
                            sql = "UPDATE DEBTOR SET INITIALHALFMONTH = 'N',LASTUPDATEDDATETIME=getDate() WHERE DEBTORID = " & debtorId
                            dt = dm.execTableInTrans(sql, cn, trans)
                        Else
                            'todo if not half month
                            retValue = processMonthByAuto(userId, debtorId, dt.Rows(0).Item(debtorDao.COLUMN_Category), dt.Rows(0).Item(debtorDao.COLUMN_LocationInfoId), passCardMstrId, batchNo, MonthName(month), duration, year, cn, trans)
                            sql = "UPDATE DEBTOR SET INITIALHALFMONTH = 'N',LASTUPDATEDDATETIME=getDate() WHERE DEBTORID = " & debtorId
                            dt = dm.execTableInTrans(sql, cn, trans)
                        End If
                    Catch ex As Exception
                        logger.Error("Error executing Auto Bill Gen for Debtor : " + debtorId)
                    End Try

                End If
            Else
                Throw New Exception("Debtor Not Found.")
            End If


            Return retValue

        Catch ex As Exception
            logger.Error(ex.Message)

        Finally
            debtorDao = Nothing
            dt = Nothing
        End Try
    End Function

    Private Function processHalfMonthAuto(ByVal userId As String, ByVal debtorId As String, ByVal debtorCategory As String, ByVal locationInfoId As String, ByVal passCardMstrId As ArrayList, ByVal batchNo As String, ByVal whichMonth As String, ByVal duration As Integer, ByVal year As Integer, ByVal cn As SqlConnection, ByVal trans As SqlTransaction) As String

        Try
            Dim sql As String = ""
            Dim updateSql As String = ""
            Dim dt As New DataTable
            Dim i, a, b As Integer
            Dim debtorDao As New CPM.DebtorDAO
            Dim invDuration As String = ""
            Dim lastInvoice As Date
            Dim RptYear As Integer
            Dim halfIndicator As Double = 0.5
            Dim retInvoiceNo As String = ""
            Dim locationMsg1, locationMsg2, locationMsg3 As String

            locationMsg1 = dm.getFieldById("LOCATIONINFO", locationInfoId, "LOCATIONMESSAGE1").ToString
            locationMsg2 = dm.getFieldById("LOCATIONINFO", locationInfoId, "LOCATIONMESSAGE2").ToString
            locationMsg3 = dm.getFieldById("LOCATIONINFO", locationInfoId, "LOCATIONMESSAGE3").ToString

            'If Now.Day >= 15 Then


            'RptYear = DatePart(DateInterval.Year, DateAdd(DateInterval.Month, +1, Today))
            RptYear = year

            'Dim strDate As New DateTime(Now.Year, whichMonth, 15)
            Dim strDate As New DateTime(RptYear, DateTime.Parse("1." + whichMonth + " 2008").Month, 15)
            Dim nextMonth As String
            Dim passStr As String = ""

            'If DateTime.Parse("1." + whichMonth + " 2008").Month <> 12 Then
            nextMonth = strDate.AddMonths(1).ToString("MMM")
            'Else
            '    nextMonth = strDate.AddMonths(0).ToString("MMM")
            'End If

            For z As Integer = 0 To passCardMstrId.Count - 1
                If passCardMstrId(z) <> "" Then
                    passStr = passStr & passCardMstrId(z) & ","
                End If

            Next

            passStr = passStr.Substring(0, passStr.Length - 1)


            'Check Month Record In Invoice History
            sql = "select count(IH.InvoiceHistoryId) as CNT from InvoiceHistory IH,InvoiceHistoryDetail IHD,DebtorAccountHeader dah " & _
                  "where IH.DebtorAccountHeaderId =  dah.DebtorAccountHeaderId " & _
                  "and ihd.DebtorAccountHeaderId = dah.DebtorAccountHeaderId And IH.month >= '" & DatePart(DateInterval.Year, DateAdd(DateInterval.Month, +1, Today)) & _
                   whichMonth & "01' AND IH.month <= '" & DatePart(DateInterval.Year, DateAdd(DateInterval.Month, +1, Today)) & nextMonth & "01'" & _
                   " and IH.debtorid =" & debtorId & _
                   " and IHD.PassCardMstrId in (" & passStr & ")"

            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            dt1 = dm.execTable(sql)


            If dt1.Rows(0).Item("CNT") > 0 Then
                'do nothing
            Else
                'Create new record for invoicehistory
                Dim tempLastInvoiceMonth As Date = strDate

                For b = 1 To duration 'eg if 5 then generate 5 months invoice history
                    invDuration = invDuration + MonthName(DatePart(DateInterval.Month, DateAdd(DateInterval.Month, +0, tempLastInvoiceMonth)), True) + "-"
                    tempLastInvoiceMonth = tempLastInvoiceMonth.AddMonths(1)
                Next
                'RptYear = DatePart(DateInterval.Year, tempLastInvoiceMonth.AddMonths(-1))
                invDuration = invDuration & DatePart(DateInterval.Year, DateAdd(DateInterval.Month, +1, Today))


                'Create Master
                'Enter Header Transaction
                dahEnt.setDebtorId(debtorId)
                dahEnt.setLocationMessage1(locationMsg1)
                dahEnt.setLocationMessage2(locationMsg2)
                dahEnt.setLocationMessage3(locationMsg3)
                dahEnt.setInvoiceNo(dm.getNextRunningNo(debtorCategory, locationInfoId, trans, cn))
                retInvoiceNo = dahEnt.getInvoiceNo
                dahEnt.setInvoiceDate(Now.ToShortDateString)
                dahEnt.setInvoicePeriod(invDuration)
                dahEnt.setLastUpdatedBy(userId)
                dahEnt.setLastUpdatedDatetime(Now)
                dahEnt.setStatus(InvoiceStatusEnum.OUTSTANDING)
                dahEnt.setBatchNo(batchNo)
                dahEnt.setTxnType(TxnTypeEnum.INVOICE)
                Dim dahId As Long = dahDao.insertDB(dahEnt, cn, trans)
                Dim invHistId As Long


                'Create Detail Transaction 2 tables :- DebtorAccountDetail,InvoiceHistoryDeatil        
                sql = " select d.category,d.locationinfoid,stm.seasontypedesc,stm.amount as unitprice,count(stm.seasontypemstrid) as qty, " & _
                      " stm.seasonduration, stm.other from debtorpassbay dbp,debtor d,seasontypemstr stm " & _
                      " where dbp.debtorid = d.debtorid " & _
                      " and stm.seasontypemstrid = dbp.seasontypemstrid " & _
                      " and dbp.debtorid = " & debtorId & _
                      " and dbp.passcardmstrid in (" & passStr & ")" & _
                      " and dbp.Status = 'A' " & _
                      " group by d.locationinfoid,stm.seasontypedesc,stm.seasonduration,stm.amount,stm.other,d.category " & _
                      " order by stm.seasontypedesc"

                dt2 = dm.execTable(sql)

                If dt2.Rows.Count = 0 Then
                    Throw New Exception("Invalid Selection Combination")
                End If

                Dim invAmount As Double = 0

                For i = 0 To dt2.Rows.Count - 1
                    'Create Detail Transaction'
                    dadEnt.setDebtorAccountHeaderId(dahId)
                    dadEnt.setMonths(duration - halfIndicator)
                    dadEnt.setDetails(dt2.Rows(i).Item("SEASONTYPEDESC"))
                    dadEnt.setUnitPrice(dt2.Rows(i).Item("UNITPRICE"))
                    dadEnt.setQuantity(dt2.Rows(i).Item("QTY"))
                    dadEnt.setAmount(dadEnt.getMonths * (dt2.Rows(i).Item("QTY") * dadEnt.getUnitPrice))
                    dadEnt.setTaxCode(dm.getTaxCode(locationInfoId, ConstantGlobal.StandardRated, trans, cn))
                    dadEnt.setLastUpdatedBy(userId)
                    dadEnt.setLastUpdatedDatetime(Now)
                    dadDao.insertDB(dadEnt, cn, trans)
                    invAmount += (dadEnt.getMonths * (dt2.Rows(i).Item("QTY") * dadEnt.getUnitPrice))
                Next

                'Throw New Exception("Test")

                'Create Tax If Applicable 09/03/2010
                Dim taxSql As String = "SELECT TAX,TAXDESCRIPTION FROM HQINFO"
                Dim dtTax = dm.execTable(taxSql)
                Dim taxAmt As Double = 0
                Dim taxDesc As String = ""

                If dtTax.Rows.Count > 0 Then
                    If dtTax.Rows.Item(0).Item("TAX") > 0 Then
                        taxAmt = dtTax.Rows.Item(0).Item("TAX")
                        taxDesc = dtTax.Rows.Item(0).Item("TAXDESCRIPTION")
                        dadEnt.setDebtorAccountHeaderId(dahId)
                        dadEnt.setMonths(0)
                        dadEnt.setDetails(taxDesc)
                        dadEnt.setUnitPrice(0)
                        dadEnt.setQuantity(0)
                        'dadEnt.setAmount(taxAmt * invAmount / 100)
                        dadEnt.setAmount(roundingAdjustment(taxAmt * invAmount / 100))
                        dadEnt.setTaxCode(dm.getTaxCode(locationInfoId, ConstantGlobal.ZeroRated, trans, cn))
                        dadEnt.setLastUpdatedBy(userId)
                        dadEnt.setLastUpdatedDatetime(Now)
                        dadDao.insertDB(dadEnt, cn, trans)
                        invAmount += dadEnt.getAmount
                    End If
                End If



                'Create Deposit Different
                Dim depositSql As String = "SELECT COUNT(S.SEASONTYPEMSTRID) AS QTY,SUM(ISNULL(PCM.DEPOSIT,0)) AS DEPOSIT,S.SEASONTYPEDESC,PCM.DEPOSIT AS UNITPRICE " & _
                " FROM PASSCARDMSTR PCM,DEBTORPASSBAY DPB,SEASONTYPEMSTR S " & _
                " WHERE PCM.PASSCARDMSTRID IN (" & passStr & ")" & _
                " AND PCM.PASSCARDMSTRID = DPB.PASSCARDMSTRID " & _
                " AND S.SEASONTYPEMSTRID = DPB.SEASONTYPEMSTRID " & _
                " AND DPB.DEBTORID = " & debtorId & _
                " and DPB.Status = 'A' " & _
                " AND PCM.DEPOSITPRINT = '" & ConstantGlobal.Yes & "'" & _
                " GROUP BY S.SEASONTYPEMSTRID,S.SEASONTYPEDESC,PCM.DEPOSIT"


                Dim dtDeposit As New DataTable
                Dim depositAmount As Long = 0

                dtDeposit = dm.execTableInTrans(depositSql, cn, trans)
                If dtDeposit.Rows.Count > 0 Then
                    For i = 0 To dtDeposit.Rows.Count - 1
                        dadEnt.setDebtorAccountHeaderId(dahId)
                        dadEnt.setMonths("")
                        dadEnt.setDetails("Deposit - " & dtDeposit.Rows(i).Item("SEASONTYPEDESC"))
                        dadEnt.setUnitPrice(dtDeposit.Rows(i).Item("UNITPRICE"))
                        dadEnt.setQuantity(dtDeposit.Rows(i).Item("QTY"))
                        dadEnt.setAmount(dtDeposit.Rows(i).Item("DEPOSIT"))
                        dadEnt.setTaxCode(dm.getTaxCode(locationInfoId, ConstantGlobal.ZeroRated, trans, cn))
                        dadEnt.setLastUpdatedBy(userId)
                        dadEnt.setLastUpdatedDatetime(Now)
                        dadDao.insertDB(dadEnt, cn, trans)
                        depositAmount += dtDeposit.Rows(i).Item("DEPOSIT")
                        invAmount += dadEnt.getAmount
                    Next
                End If

                updateSql = "UPDATE PASSCARDMSTR SET DEPOSITPRINT = '" & ConstantGlobal.No & "',LASTUPDATEDDATETIME=getDate() WHERE PASSCARDMSTRID IN (" & passStr & ")"
                dm.execTableInTrans(updateSql, cn, trans)


                Dim actualSeasonInvAmt As Double = invAmount - depositAmount
                Dim actualSeasonMonthlyAmt As Double = actualSeasonInvAmt / duration
                Dim chkAmt As Double = actualSeasonMonthlyAmt * duration
                Dim differentAmt As Double = actualSeasonInvAmt - chkAmt

                'Create InvoiceHistory
                For a = 1 To duration
                    lastInvoice = strDate.AddMonths(1)
                    invEnt.setDebtorId(debtorId)
                    invEnt.setDebtorAccountHeaderId(dahId)
                    invEnt.setStatus(InvoiceStatusEnum.OUTSTANDING)
                    invEnt.setMonth(lastInvoice)

                    If depositAmount > 0 And a = 1 Then
                        invEnt.setAmount(depositAmount + actualSeasonMonthlyAmt + differentAmt)
                    Else
                        invEnt.setAmount(actualSeasonMonthlyAmt)
                    End If

                    'invEnt.setAmount(invAmount / duration)
                    invEnt.setLastUpdatedBy(userId)
                    invEnt.setLastUpdatedDatetime(Now)
                    invHistId = invDao.insertDB(invEnt, cn, trans)
                    strDate = DateAdd(DateInterval.Month, +1, strDate)
                Next a

                'Update Invoice Header total amount
                dahEnt.setDebtorAccountHeaderId(dahId)
                'dahEnt.setAmount(invAmount)
                dahEnt.setAmount(roundingAdjustment(invAmount))
                dahEnt.setLastUpdatedBy(userId)
                dahEnt.setLastUpdatedDatetime(Now.AddMilliseconds(1))
                dahDao.updateDB(dahEnt, cn, trans)

                'Audit AutoGenInvoice
                'Dim autoBillGenerationEnt As New CPM.AutoBillGenerationEntity
                'Dim autoBillGenerationDao As New CPM.AutoBillGenerationDAO

                'autoBillGenerationEnt.setBatchNo(batchNo)
                'autoBillGenerationEnt.setDebtorId(debtorId)
                'autoBillGenerationEnt.setLastUpdatedBy(userId)
                'autoBillGenerationEnt.setLastUpdatedDatetime(Now)
                'autoBillGenerationEnt.setTxnDate(Now)
                'autoBillGenerationEnt.setStatus("S")
                'autoBillGenerationDao.insertDB(autoBillGenerationEnt, cn, trans)

                'Reset Debtor BillGenerationCounter back to 0 if able to turn out the invoice meaning no outstandng amount
                Dim updateDebSql = "Update Debtor set BillGenerationCounter = 0 where DebtorId = " + debtorId
                dm.execTableInTrans(updateDebSql, cn, trans)
                'End Audit


                'Create InvoiceHistoryDetail
                Dim ihdEnt As New CPM.InvoiceHistoryDetailEntity
                Dim ihdDao As New CPM.InvoiceHistoryDetailDAO

                For x As Integer = 0 To passCardMstrId.Count - 1
                    passStr = passCardMstrId(x)
                    If passStr <> "" Then
                        ihdEnt.setDebtorAccountHeaderId(dahId)
                        ihdEnt.setLastUpdatedBy(userId)
                        ihdEnt.setLastUpdatedDatetime(Now)
                        ihdEnt.setPassCardMstrId(passCardMstrId(x))
                        ihdDao.insertDB(ihdEnt, cn, trans)
                    End If

                Next

                checkMismatchAmount(dahId, cn, trans)

            End If

            'End If
            'System.Threading.Thread.Sleep(20) 'Too fast update casuing dirty data buffer
            Return retInvoiceNo

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function processMonthByAuto(ByVal userId As String, ByVal debtorId As String, ByVal debtorCategory As String, ByVal locationInfoId As String, ByVal passCardMstrId As ArrayList, ByVal batchNo As String, ByVal whichMonth As String, ByVal duration As Integer, ByVal year As Integer, ByVal cn As SqlConnection, ByVal trans As SqlTransaction) As String
        Try
            Dim sql As String = ""
            Dim updateSql As String = ""
            Dim dt As New DataTable
            Dim i, a, b As Integer
            Dim debtorDao As New CPM.DebtorDAO
            Dim invDuration As String = ""
            Dim lastInvoice As Date
            Dim RptYear As Integer            
            Dim retInvoiceNo As String = ""
            Dim locationMsg1, locationMsg2, locationMsg3 As String

            locationMsg1 = dm.getFieldById("LOCATIONINFO", locationInfoId, "LOCATIONMESSAGE1").ToString
            locationMsg2 = dm.getFieldById("LOCATIONINFO", locationInfoId, "LOCATIONMESSAGE2").ToString
            locationMsg3 = dm.getFieldById("LOCATIONINFO", locationInfoId, "LOCATIONMESSAGE3").ToString

            'RptYear = DatePart(DateInterval.Year, DateAdd(DateInterval.Month, +1, Today))
            RptYear = year

            'Dim strDate As New DateTime(Now.Year, whichMonth, 1)
            Dim strDate As New DateTime(RptYear, DateTime.Parse("1." + whichMonth + " 2008").Month, 1)
            Dim nextMonth As String
            Dim passStr As String = ""


            'If DateTime.Parse("1." + whichMonth + " 2008").Month <> 12 Then
            nextMonth = strDate.AddMonths(1).ToString("MMM")
            'Else
            '    nextMonth = strDate.AddMonths(0).ToString("MMM")
            'End If


            For z As Integer = 0 To passCardMstrId.Count - 1
                If passCardMstrId(z) <> "" Then
                    passStr = passStr & passCardMstrId(z) & ","
                End If

            Next

            passStr = passStr.Substring(0, passStr.Length - 1)


            'Check Month Record In Invoice History
            sql = "select count(IH.InvoiceHistoryId) as CNT from InvoiceHistory IH,InvoiceHistoryDetail IHD,DebtorAccountHeader dah " & _
                  "where IH.DebtorAccountHeaderId =  dah.DebtorAccountHeaderId " & _
                  "and ihd.DebtorAccountHeaderId = dah.DebtorAccountHeaderId And IH.month >= '" & DatePart(DateInterval.Year, DateAdd(DateInterval.Month, +1, Today)) & _
                   Now.AddMonths(1).ToString("MMM") & "01' AND IH.month <= '" & DatePart(DateInterval.Year, DateAdd(DateInterval.Month, +1, Today)) & nextMonth & "01'" & _
                   " and IH.debtorid =" & debtorId & _
                   " and IHD.PassCardMstrId in (" & passStr & ")"

            Dim dt1 As New DataTable
            Dim dt2 As New DataTable

            dt1 = dm.execTable(sql)


            If dt1.Rows(0).Item("CNT") > 0 Then
                'do nothing
            Else
                'Create new record for invoicehistory
                Dim tempLastInvoiceMonth As Date = strDate

                For b = 1 To duration 'eg if 5 then generate 5 months invoice history
                    invDuration = invDuration + MonthName(DatePart(DateInterval.Month, DateAdd(DateInterval.Month, +1, tempLastInvoiceMonth)), True) + "-"
                    tempLastInvoiceMonth = tempLastInvoiceMonth.AddMonths(1)
                Next
                'RptYear = DatePart(DateInterval.Year, tempLastInvoiceMonth.AddMonths(-1))
                invDuration = invDuration & DatePart(DateInterval.Year, DateAdd(DateInterval.Month, +1, Today))


                'Create Master
                'Enter Header Transaction
                dahEnt.setDebtorId(debtorId)
                dahEnt.setLocationMessage1(locationMsg1)
                dahEnt.setLocationMessage2(locationMsg2)
                dahEnt.setLocationMessage3(locationMsg3)
                dahEnt.setInvoiceNo(dm.getNextRunningNo(debtorCategory, locationInfoId, trans, cn))
                retInvoiceNo = dahEnt.getInvoiceNo
                dahEnt.setInvoiceDate(Now.ToShortDateString)
                dahEnt.setInvoicePeriod(invDuration)
                dahEnt.setLastUpdatedBy(userId)
                dahEnt.setLastUpdatedDatetime(Now)
                dahEnt.setStatus(InvoiceStatusEnum.OUTSTANDING)
                dahEnt.setBatchNo(batchNo)
                dahEnt.setTxnType(TxnTypeEnum.INVOICE)
                Dim dahId As Long = dahDao.insertDB(dahEnt, cn, trans)
                Dim invHistId As Long


                'Create Detail Transaction 2 tables :- DebtorAccountDetail,InvoiceHistoryDeatil        
                sql = " select d.category,d.locationinfoid,stm.seasontypedesc,stm.amount as unitprice,count(stm.seasontypemstrid) as qty, " & _
                      " sum(pcm.deposit) as deposit,stm.seasonduration, stm.other from debtorpassbay dbp,debtor d,seasontypemstr stm,passcardmstr pcm " & _
                      " where dbp.debtorid = d.debtorid " & _
                      " and stm.seasontypemstrid = dbp.seasontypemstrid " & _
                      " and pcm.passCardMstrid = dbp.passCardMstrid " & _
                      " and dbp.debtorid = " & debtorId & _
                      " and dbp.passcardmstrid in (" & passStr & ")" & _
                      " and dbp.Status = 'A' " & _
                      " group by d.locationinfoid,stm.seasontypedesc,stm.seasonduration,stm.amount,stm.other,d.category " & _
                      " order by stm.seasontypedesc"

                dt2 = dm.execTable(sql)

                If dt2.Rows.Count = 0 Then
                    Throw New Exception("Invalid Selection Combination")
                End If

                Dim invAmount As Double = 0

                For i = 0 To dt2.Rows.Count - 1
                    'Create Detail Transaction'
                    dadEnt.setDebtorAccountHeaderId(dahId)
                    dadEnt.setMonths(duration)
                    dadEnt.setDetails(dt2.Rows(i).Item("SEASONTYPEDESC"))
                    dadEnt.setUnitPrice(dt2.Rows(i).Item("SEASONDURATION") * dt2.Rows(i).Item("UNITPRICE"))
                    dadEnt.setQuantity(dt2.Rows(i).Item("QTY"))
                    dadEnt.setAmount(duration * (dt2.Rows(i).Item("QTY") * dadEnt.getUnitPrice))
                    dadEnt.setTaxCode(dm.getTaxCode(locationInfoId, ConstantGlobal.StandardRated, trans, cn))
                    dadEnt.setLastUpdatedBy(userId)
                    dadEnt.setLastUpdatedDatetime(Now)
                    dadDao.insertDB(dadEnt, cn, trans)
                    invAmount += duration * (dt2.Rows(i).Item("QTY") * dt2.Rows(i).Item("UNITPRICE"))
                Next

                'Create Tax If Applicable 09/03/2010
                Dim taxSql As String = "SELECT TAX,TAXDESCRIPTION FROM HQINFO"
                Dim dtTax = dm.execTable(taxSql)
                Dim taxAmt As Double = 0
                Dim taxDesc As String = ""

                If dtTax.Rows.Count > 0 Then
                    If dtTax.Rows.Item(0).Item("TAX") > 0 Then
                        taxAmt = dtTax.Rows.Item(0).Item("TAX")
                        taxDesc = dtTax.Rows.Item(0).Item("TAXDESCRIPTION")
                        dadEnt.setDebtorAccountHeaderId(dahId)
                        dadEnt.setMonths(0)
                        dadEnt.setDetails(taxDesc)
                        dadEnt.setUnitPrice(0)
                        dadEnt.setQuantity(0)
                        'dadEnt.setAmount(taxAmt * invAmount / 100)
                        dadEnt.setAmount(roundingAdjustment(taxAmt * invAmount / 100))
                        dadEnt.setTaxCode(dm.getTaxCode(locationInfoId, ConstantGlobal.ZeroRated, trans, cn))
                        dadEnt.setLastUpdatedBy(userId)
                        dadEnt.setLastUpdatedDatetime(Now)
                        dadDao.insertDB(dadEnt, cn, trans)
                        invAmount += dadEnt.getAmount
                    End If
                End If


                'Create Deposit Different
                Dim depositSql As String = "SELECT COUNT(S.SEASONTYPEMSTRID) AS QTY,SUM(ISNULL(PCM.DEPOSIT,0)) AS DEPOSIT,S.SEASONTYPEDESC,PCM.DEPOSIT AS UNITPRICE " & _
                " FROM PASSCARDMSTR PCM,DEBTORPASSBAY DPB,SEASONTYPEMSTR S " & _
                " WHERE PCM.PASSCARDMSTRID IN (" & passStr & ")" & _
                " AND PCM.PASSCARDMSTRID = DPB.PASSCARDMSTRID " & _
                " AND S.SEASONTYPEMSTRID = DPB.SEASONTYPEMSTRID " & _
                " AND DPB.DEBTORID = " & debtorId & _
                " and DPB.Status = 'A' " & _
                " AND PCM.DEPOSITPRINT = '" & ConstantGlobal.Yes & "'" & _
                " GROUP BY S.SEASONTYPEMSTRID,S.SEASONTYPEDESC,PCM.DEPOSIT"


                Dim dtDeposit As New DataTable
                Dim depositAmount As Long = 0

                dtDeposit = dm.execTableInTrans(depositSql, cn, trans)
                If dtDeposit.Rows.Count > 0 Then
                    For i = 0 To dtDeposit.Rows.Count - 1
                        dadEnt.setDebtorAccountHeaderId(dahId)
                        dadEnt.setMonths("")
                        dadEnt.setDetails("Deposit - " & dtDeposit.Rows(i).Item("SEASONTYPEDESC"))
                        dadEnt.setUnitPrice(dtDeposit.Rows(i).Item("UNITPRICE"))
                        dadEnt.setQuantity(dtDeposit.Rows(i).Item("QTY"))
                        dadEnt.setAmount(dtDeposit.Rows(i).Item("DEPOSIT"))
                        dadEnt.setTaxCode(dm.getTaxCode(locationInfoId, ConstantGlobal.ZeroRated, trans, cn))
                        dadEnt.setLastUpdatedBy(userId)
                        dadEnt.setLastUpdatedDatetime(Now)
                        dadDao.insertDB(dadEnt, cn, trans)
                        depositAmount += dtDeposit.Rows(i).Item("DEPOSIT")
                        invAmount += dadEnt.getAmount
                    Next
                End If

                updateSql = "UPDATE PASSCARDMSTR SET DEPOSITPRINT = '" & ConstantGlobal.No & "',LASTUPDATEDDATETIME=getDate() WHERE PASSCARDMSTRID IN (" & passStr & ")"
                dm.execTableInTrans(updateSql, cn, trans)

                ' Throw New Exception("Test")

                Dim actualSeasonInvAmt As Double = invAmount - depositAmount
                Dim actualSeasonMonthlyAmt As Double = actualSeasonInvAmt / duration
                Dim chkAmt As Double = actualSeasonMonthlyAmt * duration
                Dim differentAmt As Double = actualSeasonInvAmt - chkAmt

                'Create InvoiceHistory
                For a = 1 To duration
                    lastInvoice = strDate.AddMonths(1)
                    invEnt.setDebtorId(debtorId)
                    invEnt.setDebtorAccountHeaderId(dahId)
                    invEnt.setStatus(InvoiceStatusEnum.OUTSTANDING)
                    invEnt.setMonth(lastInvoice)

                    If depositAmount > 0 And a = 1 Then
                        invEnt.setAmount(depositAmount + actualSeasonMonthlyAmt + differentAmt)
                    Else
                        invEnt.setAmount(actualSeasonMonthlyAmt)
                    End If

                    'invEnt.setAmount(invAmount / duration)
                    invEnt.setLastUpdatedBy(userId)
                    invEnt.setLastUpdatedDatetime(Now)
                    invHistId = invDao.insertDB(invEnt, cn, trans)
                    strDate = DateAdd(DateInterval.Month, +1, strDate)
                Next a


                'Update Invoice Header total amount
                dahEnt.setDebtorAccountHeaderId(dahId)
                '                dahEnt.setAmount(invAmount)
                dahEnt.setAmount(roundingAdjustment(invAmount))
                dahEnt.setLastUpdatedBy(userId)
                dahEnt.setLastUpdatedDatetime(Now.AddMilliseconds(1))
                dahDao.updateDB(dahEnt, cn, trans)

                'Audit AutoGenInvoice
                'Dim autoBillGenerationEnt As New CPM.AutoBillGenerationEntity
                'Dim autoBillGenerationDao As New CPM.AutoBillGenerationDAO

                'autoBillGenerationEnt.setBatchNo(batchNo)
                'autoBillGenerationEnt.setDebtorId(debtorId)
                'autoBillGenerationEnt.setLastUpdatedBy(userId)
                'autoBillGenerationEnt.setLastUpdatedDatetime(Now)
                'autoBillGenerationEnt.setTxnDate(Now)
                'autoBillGenerationEnt.setStatus("S")
                'autoBillGenerationDao.insertDB(autoBillGenerationEnt, cn, trans)

                'Reset Debtor BillGenerationCounter back to 0 if able to turn out the invoice meaning no outstandng amount
                Dim updateDebSql = "Update Debtor set BillGenerationCounter = 0 where DebtorId = " + debtorId
                dm.execTableInTrans(updateDebSql, cn, trans)
                'End Audit


                'Create InvoiceHistoryDetail
                Dim ihdEnt As New CPM.InvoiceHistoryDetailEntity
                Dim ihdDao As New CPM.InvoiceHistoryDetailDAO

                For x As Integer = 0 To passCardMstrId.Count - 1
                    passStr = passCardMstrId(x)
                    If passStr <> "" Then
                        ihdEnt.setDebtorAccountHeaderId(dahId)
                        ihdEnt.setLastUpdatedBy(userId)
                        ihdEnt.setLastUpdatedDatetime(Now)
                        ihdEnt.setPassCardMstrId(passCardMstrId(x))
                        ihdDao.insertDB(ihdEnt, cn, trans)
                    End If

                Next

            End If

            'System.Threading.Thread.Sleep(20) 'Too fast update casuing dirty data buffer
            Return retInvoiceNo

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub checkMismatchAmount(ByVal DAHId As Long, ByVal cn As SqlConnection, ByVal trans As SqlTransaction)
        'Check MisMatch Amount for DebtorAccountDetail vs InvoiceHistory
        Dim sqlDad As String
        Dim sqlIH As String
        Dim sqlUpdate As String
        Dim dt1 As DataTable
        Dim dt2 As DataTable
        Dim dt3 As DataTable


        Try
            sqlDad = "select sum(dad.amount) as amountDad from debtoraccountdetail dad where dad.debtoraccountheaderid = " + DAHId.ToString()
            sqlIH = "select sum(ih.amount) as amountIH from invoicehistory ih where ih.debtoraccountheaderid = " + DAHId.ToString()

            dt1 = dm.execTableInTrans(sqlDad, cn, trans)
            dt2 = dm.execTableInTrans(sqlIH, cn, trans)

            If (dt1.Rows.Count = dt2.Rows.Count) Then
                If (dt1.Rows(0).Item(0).ToString() <> dt2.Rows(0).Item(0).ToString()) Then
                    sqlUpdate = "select min(invoicehistoryid) from invoicehistory where debtoraccountheaderid = " + DAHId.ToString()
                    dt3 = dm.execTableInTrans(sqlUpdate, cn, trans)
                    sqlUpdate = "Update invoicehistory set Amount = Amount + 0.01 where invoicehistoryid = " + dt3.Rows(0).Item(0).ToString()
                    dm.execTableInTrans(sqlUpdate, cn, trans)
                End If
            End If


        Catch ex As Exception
            logger.Debug(ex.Message)

        Finally
            dt1 = Nothing
            dt2 = Nothing
            dt3 = Nothing
        End Try




        'End of Check Mismatch

    End Sub

End Class
