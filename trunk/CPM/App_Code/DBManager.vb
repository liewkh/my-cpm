Imports Microsoft.VisualBasic
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports System.Data

Public Class DBManager
    Public cn As SqlConnection
    Public dr As SqlDataReader
    Public trans As SqlTransaction

    Public cmd_select As SqlCommand
    Public cmd_delete As SqlCommand
    Public cmd_insert_update As SqlCommand
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Public Function getDBConn() As String
        Dim strConn As String = System.Configuration.ConfigurationManager.AppSettings("path")
        getDBConn = strConn
    End Function

    'Public Function getSqlDBConn() As SqlConnection
    '    Dim strConn As String = System.Configuration.ConfigurationManager.AppSettings("path")
    '    getSqlDBConn = strConn
    'End Function

    Public Function execReader(ByVal sqlString As String) As SqlDataReader

        cn = New SqlConnection(getDBConn())
        cmd_select = New SqlCommand()
        If Not cn.State = ConnectionState.Open Then
            cn.Open()
        End If
        'cn.Open()
        cmd_select.Connection = cn
        cmd_select.CommandText = sqlString
        dr = cmd_select.ExecuteReader()
        cn.Close()
        Return dr
    End Function

    Public Function execTable(ByVal sqlString As String) As DataTable
        Dim dt As DataTable
        Try
            cn = New SqlConnection(getDBConn())
            cmd_select = New SqlCommand()
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If
            'cn.Open()
            cmd_select.Connection = cn
            cmd_select.CommandText = sqlString
            dr = cmd_select.ExecuteReader()
            dt = readerToTable(dr)
            cn.Close()
            Return dt
        Catch ex As Exception
            'MsgBox("ERROR. Fail to retrieve data. Statement: " + sqlString + " " + ex.Message)
            logger.Debug("ERROR. Fail to retrieve data. Statement: " + sqlString + " " + ex.Message)
        Finally
            cn.Close()
        End Try
        Return New DataTable
    End Function
    Public Function execTableInTrans(ByVal sqlString As String, ByVal conn As SqlConnection, ByVal trans As SqlTransaction) As DataTable
        Dim dt As DataTable
        Try
            cn = conn
            cmd_select = New SqlCommand()
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If
            'cn.Open()
            cmd_select.Connection = cn
            cmd_select.Transaction = trans
            cmd_select.CommandText = sqlString
            dr = cmd_select.ExecuteReader()
            dt = readerToTable(dr)
            'cn.Close()
            Return dt
        Catch ex As Exception
            'MsgBox("ERROR. Fail to retrieve data. Statement: " + sqlString + " " + ex.Message)
            logger.Debug("ERROR. Fail to retrieve data. Statement: " + sqlString + " " + ex.Message)
        Finally
            dr.Close()
        End Try
        Return New DataTable
    End Function

    Public Function execDataSet(ByVal sqlString As String) As DataSet
        Dim ds As DataSet
        Try
            cn = New SqlConnection(getDBConn())
            cmd_select = New SqlCommand()
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If
            'cn.Open()
            cmd_select.Connection = cn
            cmd_select.CommandText = sqlString
            dr = cmd_select.ExecuteReader()
            ds = ReaderToDataSet(dr)
            cn.Close()
            Return ds
        Catch ex As Exception
            'MsgBox("ERROR. Fail to retrieve data. Statement: " + sqlString + " " + ex.Message)
            logger.Debug("ERROR. Fail to retrieve data. Statement: " + sqlString + " " + ex.Message)
        Finally
            cn.Close()
        End Try
        Return New DataSet
    End Function

    Public Function readerToTable(ByVal reader As SqlClient.SqlDataReader) As DataTable
        Dim newTable As New DataTable()
        Dim col As DataColumn
        Dim row As DataRow
        Dim i As Integer

        For i = 0 To reader.FieldCount - 1
            col = New DataColumn()
            col.ColumnName = reader.GetName(i)
            col.DataType = reader.GetFieldType(i)
            newTable.Columns.Add(col)
        Next

        While reader.Read
            row = newTable.NewRow()
            For i = 0 To reader.FieldCount - 1
                row(i) = reader.Item(i)
            Next
            newTable.Rows.Add(row)
        End While

        Return newTable
    End Function
    Public Function ReaderToDataSet(ByVal reader As SqlDataReader) As DataSet
        Dim dataSet As DataSet = New DataSet()
        Dim schemaTable As DataTable = reader.GetSchemaTable()
        Dim dataTable As DataTable = New DataTable()
        Dim intCounter As Integer

        For intCounter = 0 To schemaTable.Rows.Count - 1
            Dim dataRow As DataRow = schemaTable.Rows(intCounter)
            Dim columnName As String = CType(dataRow("ColumnName"), String)
            Dim column As DataColumn = New DataColumn(columnName, CType(dataRow("DataType"), Type))
            dataTable.Columns.Add(column)
        Next

        dataSet.Tables.Add(dataTable)
        While reader.Read()
            Dim dataRow As DataRow = dataTable.NewRow()
            For intCounter = 0 To reader.FieldCount - 1
                dataRow(intCounter) = reader.GetValue(intCounter)
            Next
            dataTable.Rows.Add(dataRow)
        End While
        Return dataSet
    End Function

    Public Function getFieldById(ByVal tableName As String, ByVal id As String, ByVal fieldName As String)
        Dim dt As DataTable
        Dim retField As String = ""
        Dim sqlString As String = "SELECT " + fieldName + " FROM " + tableName + " WHERE " + tableName + "Id=" & id
        dt = execTable(sqlString)

        If dt.Rows.Count = 1 Then
            Dim i As Integer = 0
            While i < dt.Rows.Count
                retField = dt.Rows(i).Item(fieldName).ToString.Trim

                i = i + 1
            End While
        End If
        Return retField
    End Function

    Public Function execGetCodeMstrDesc(ByVal codeCat As String, ByVal code As String) As String

        Try
            cn = New SqlConnection(getDBConn())
            cmd_select = New SqlCommand()
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If
            'cn.Open()
            cmd_select.Connection = cn
            cmd_select.CommandText = "select dbo.getCodeMstrDesc(@Cat,@Code)"
            cmd_select.Parameters.Add("@Cat", SqlDbType.NVarChar, 10, ParameterDirection.Input).Value = codeCat
            cmd_select.Parameters.Add("@Code", SqlDbType.NVarChar, 10, ParameterDirection.Input).Value = code
            Dim CodeDesc As String = cmd_select.ExecuteScalar()

            cmd_select.Dispose()
            cn.Close()
            Return CodeDesc.ToString
        Catch ex As Exception
            'MsgBox("ERROR. Fail to retrieve data. Stored Procedure dbo.getCodeMstrDesc: " + " codeCat: " + codeCat + " code: " + code + " " + ex.Message)
            logger.Debug("ERROR. Fail to retrieve data. Stored Procedure dbo.getCodeMstrDesc: " + " codeCat: " + codeCat + " code: " + code + " " + ex.Message)
        Finally
            cn.Close()
        End Try
        Return ""
    End Function

    Public Function getGridViewRecordCount(ByRef ds As System.Web.UI.WebControls.SqlDataSource) As Int64
        Dim totalRecCount As Int64
        Dim gv As New System.Web.UI.WebControls.GridView
        gv.DataSource = ds
        gv.AllowPaging = False
        gv.DataBind()
        totalRecCount = gv.Rows.Count
        gv.Dispose()
        Return totalRecCount
    End Function

    Public Function getNextRunningNo(ByVal debtorCategory As String, ByVal LocationInfoId As String, ByRef trans As SqlTransaction, ByRef cn As SqlConnection, Optional ByVal TaxInvoice As String = "") As String
        'Get the RunningNo
        Dim runningNo As Integer
        Dim selectSQL As String
        Dim prefix As String = ""
        Dim dm As New DBManager
        Dim dt As New DataTable



        'Update the RunningNo to the next increment
        Try

            If debtorCategory = CategoryEnum.COMPANY Then
                selectSQL = "Select CompanyInvoiceNo,CompanyInvoicePrefix From LocationInfo Where LocationInfoId = '" + LocationInfoId + "'"
            Else
                selectSQL = "Select IndividualInvoiceNo,IndividualInvoicePrefix From LocationInfo Where LocationInfoId = '" + LocationInfoId + "'"
            End If


            dt = dm.execTable(selectSQL)

            If dt.Rows.Count > 0 Then
                If debtorCategory = CategoryEnum.COMPANY Then
                    If Not dt.Rows(0)("CompanyInvoiceNo").Equals(System.DBNull.Value) Then
                        runningNo = dt.Rows(0)("CompanyInvoiceNo")
                    Else
                        Throw New ApplicationException(ConstantGlobal.No_Record_Found)
                    End If
                    If Not dt.Rows(0)("CompanyInvoicePrefix").Equals(System.DBNull.Value) Then
                        prefix = dt.Rows(0)("CompanyInvoicePrefix")
                    Else
                        Throw New ApplicationException(ConstantGlobal.No_Record_Found)
                    End If
                Else
                    If Not dt.Rows(0)("IndividualInvoiceNo").Equals(System.DBNull.Value) Then
                        runningNo = dt.Rows(0)("IndividualInvoiceNo")
                    Else
                        Throw New ApplicationException(ConstantGlobal.No_Record_Found)
                    End If
                    If Not dt.Rows(0)("IndividualInvoicePrefix").Equals(System.DBNull.Value) Then
                        prefix = dt.Rows(0)("IndividualInvoicePrefix")
                    Else
                        Throw New ApplicationException(ConstantGlobal.No_Record_Found)
                    End If

                End If
            Else
                Return 0
            End If

            If prefix <> "" Then
                If prefix = "YY" Then
                    prefix = Now.Year.ToString.Remove(0, 2)
                End If
            End If


            Dim updateSQL As String
            If debtorCategory = CategoryEnum.COMPANY Then
                updateSQL = "Update LocationInfo set CompanyInvoiceNo = @InvoiceNo Where LocationInfoId = '" + LocationInfoId + "'"
            Else
                updateSQL = "Update LocationInfo set IndividualInvoiceNo = @InvoiceNo Where LocationInfoId = '" + LocationInfoId + "'"
            End If

            Dim cmd As New SqlCommand(updateSQL, cn, trans)

            cmd.Parameters.Add(New SqlParameter("@InvoiceNo", SqlDbType.Int))
            cmd.Parameters("@InvoiceNo").Value = runningNo + 1

            cmd.ExecuteNonQuery()
            cmd.Dispose()
        Catch ex As Exception
            'trans.Rollback()
            Throw ex
        Finally
            dt = Nothing
        End Try

        If Not String.IsNullOrEmpty(TaxInvoice) Then
            prefix = prefix.Substring(0, prefix.Length - 1) + TaxInvoice
        End If

        Return prefix & runningNo.ToString.PadLeft(7, "0"c)
    End Function

    Public Function getReceiptNextRunningNo(ByVal locationInfoId As String, ByRef trans As SqlTransaction, ByRef cn As SqlConnection) As String
        'Get the RunningNo
        Dim runningNo As Integer
        Dim selectSQL As String
        Dim prefix As String = ""
        Dim dm As New DBManager
        Dim dt As New DataTable



        'Update the RunningNo to the next increment
        Try

            selectSQL = "Select ReceiptNo,ReceiptPrefix From LocationInfo WITH (NOLOCK) Where LocationInfoId = " + locationInfoId
            dt = dm.execTable(selectSQL)

            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("ReceiptNo").Equals(System.DBNull.Value) Then
                    Return 0 'If never setup
                End If
                runningNo = dt.Rows(0)("ReceiptNo")
                prefix = dt.Rows(0)("ReceiptPrefix")
            Else
                Return 0
            End If

            If prefix <> "" Then
                If prefix = "YY" Then
                    prefix = Now.Year.ToString.Remove(0, 2)
                End If
            End If


            Dim updateSQL As String
            updateSQL = "Update LocationInfo WITH (ROWLOCK) set ReceiptNo = @ReceiptNo Where LocationInfoId = " + locationInfoId
            Dim cmd As New SqlCommand(updateSQL, cn, trans)

            cmd.Parameters.Add(New SqlParameter("@ReceiptNo", SqlDbType.Int))
            cmd.Parameters("@ReceiptNo").Value = runningNo + 1

            cmd.ExecuteNonQuery()
            cmd.Dispose()
        Catch ex As Exception
            'trans.Rollback()
            Throw ex
        Finally
            dt = Nothing
        End Try
        Return prefix & runningNo.ToString.PadLeft(7, "0"c)
    End Function

    Public Function getNextBatchNo(ByRef trans As SqlTransaction, ByRef cn As SqlConnection) As String
        'Get the RunningNo
        Dim runningNo As Integer
        Dim selectSQL As String
        Dim dm As New DBManager
        Dim dt As New DataTable


        Try

            selectSQL = "Select ParameterValue From Parameter Where ParameterName  = '" + ParameterEnum.BATCHNO + "'"

            dt = dm.execTable(selectSQL)

            If dt.Rows.Count > 0 Then
                If Not dt.Rows(0)("ParameterValue").Equals(System.DBNull.Value) Then
                    runningNo = dt.Rows(0)("ParameterValue")
                Else
                    Throw New ApplicationException(ConstantGlobal.No_Record_Found)
                End If              
            Else
                Return 0
            End If


            Dim updateSQL As String

            updateSQL = "Update Parameter set ParameterValue = @BatchNo Where ParameterName  = '" + ParameterEnum.BATCHNO + "'"
           

            Dim cmd As New SqlCommand(updateSQL, cn, trans)

            cmd.Parameters.Add(New SqlParameter("@BatchNo", SqlDbType.Int))
            cmd.Parameters("@BatchNo").Value = runningNo + 1

            cmd.ExecuteNonQuery()
            cmd.Dispose()
        Catch ex As Exception
            'trans.Rollback()
            Throw ex
        Finally
            dt = Nothing
        End Try
        Return runningNo.ToString.PadLeft(7, "0"c)
    End Function

    Public Function getCRNoteNextRunningNo(ByVal locationInfoId As String, ByRef trans As SqlTransaction, ByRef cn As SqlConnection) As String
        'Get the RunningNo
        Dim runningNo As Integer
        Dim selectSQL As String
        Dim prefix As String = ""
        Dim dm As New DBManager
        Dim dt As New DataTable



        'Update the RunningNo to the next increment
        Try

            selectSQL = "Select CreditNoteNo,CreditNotePrefix From LocationInfo WITH (NOLOCK) Where LocationInfoId = " + locationInfoId
            dt = dm.execTable(selectSQL)

            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("CreditNoteNo").Equals(System.DBNull.Value) Then
                    Return 0 'If never setup
                End If
                runningNo = dt.Rows(0)("CreditNoteNo")
                prefix = dt.Rows(0)("CreditNotePrefix")
            Else
                Return 0
            End If

            If prefix <> "" Then
                If prefix = "YY" Then
                    prefix = Now.Year.ToString.Remove(0, 2)
                End If
            End If


            Dim updateSQL As String
            updateSQL = "Update LocationInfo WITH (ROWLOCK) set CreditNoteNo = @CreditNoteNo Where LocationInfoId = " + locationInfoId
            Dim cmd As New SqlCommand(updateSQL, cn, trans)

            cmd.Parameters.Add(New SqlParameter("@CreditNoteNo", SqlDbType.Int))
            cmd.Parameters("@CreditNoteNo").Value = runningNo + 1

            cmd.ExecuteNonQuery()
            cmd.Dispose()
        Catch ex As Exception
            'trans.Rollback()
            Throw ex
        Finally
            dt = Nothing
        End Try
        Return prefix & runningNo.ToString.PadLeft(7, "0"c)
    End Function

    Public Function getDebitNoteNextRunningNo(ByVal locationInfoId As String, ByRef trans As SqlTransaction, ByRef cn As SqlConnection) As String
        'Get the RunningNo
        Dim runningNo As Integer
        Dim selectSQL As String
        Dim prefix As String = ""
        Dim dm As New DBManager
        Dim dt As New DataTable



        'Update the RunningNo to the next increment
        Try

            selectSQL = "Select DebitNoteNo,DebitNotePrefix From LocationInfo WITH (NOLOCK) Where LocationInfoId = " + locationInfoId
            dt = dm.execTable(selectSQL)

            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("DebitNoteNo").Equals(System.DBNull.Value) Then
                    Return 0 'If never setup
                End If
                runningNo = dt.Rows(0)("DebitNoteNo")
                prefix = dt.Rows(0)("DebitNotePrefix")
            Else
                Return 0
            End If

            If prefix <> "" Then
                If prefix = "YY" Then
                    prefix = Now.Year.ToString.Remove(0, 2)
                End If
            End If


            Dim updateSQL As String
            updateSQL = "Update LocationInfo WITH (ROWLOCK) set DebitNoteNo = @DebitNoteNo Where LocationInfoId = " + locationInfoId
            Dim cmd As New SqlCommand(updateSQL, cn, trans)

            cmd.Parameters.Add(New SqlParameter("@DebitNoteNo", SqlDbType.Int))
            cmd.Parameters("@DebitNoteNo").Value = runningNo + 1

            cmd.ExecuteNonQuery()
            cmd.Dispose()
        Catch ex As Exception
            'trans.Rollback()
            Throw ex
        Finally
            dt = Nothing
        End Try
        Return prefix & runningNo.ToString.PadLeft(7, "0"c)
    End Function

    Public Function getDailyCollectionNextRunningNo(ByVal locationInfoId As String, ByRef trans As SqlTransaction, ByRef cn As SqlConnection) As String
        'Get the RunningNo
        Dim runningNo As Integer
        Dim selectSQL As String
        Dim prefix As String = ""
        Dim dm As New DBManager
        Dim dt As New DataTable



        'Update the RunningNo to the next increment
        Try

            selectSQL = "Select DailyCollectionNo,DailyCollectionPrefix From LocationInfo WITH (NOLOCK) Where LocationInfoId = " + locationInfoId
            dt = dm.execTable(selectSQL)

            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("DailyCollectionNo").Equals(System.DBNull.Value) Then
                    Return 0 'If never setup
                End If
                runningNo = dt.Rows(0)("DailyCollectionNo")
                prefix = dt.Rows(0)("DailyCollectionPrefix")
            Else
                Return 0
            End If

            If prefix <> "" Then
                If prefix = "YY" Then
                    prefix = Now.Year.ToString.Remove(0, 2)
                End If
            End If


            Dim updateSQL As String
            updateSQL = "Update LocationInfo WITH (ROWLOCK) set DailyCollectionNo = @DailyCollectionNo Where LocationInfoId = " + locationInfoId
            Dim cmd As New SqlCommand(updateSQL, cn, trans)

            cmd.Parameters.Add(New SqlParameter("@DailyCollectionNo", SqlDbType.Int))
            cmd.Parameters("@DailyCollectionNo").Value = runningNo + 1

            cmd.ExecuteNonQuery()
            cmd.Dispose()
        Catch ex As Exception
            'trans.Rollback()
            Throw ex
        Finally
            dt = Nothing
        End Try
        Return prefix & runningNo.ToString.PadLeft(7, "0"c)
    End Function

    Public Function getCurrentTax() As Double

        Dim selectSQL As String
        Dim dm As New DBManager
        Dim dt As New DataTable
        Dim taxAmount As Double

        Try

            selectSQL = "SELECT TAX,TAXDESCRIPTION FROM HQINFO"
            dt = dm.execTable(selectSQL)

            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("TAX").Equals(System.DBNull.Value) Then
                    Return 0 'If never setup
                End If
                taxAmount = dt.Rows(0)("TAX")
            Else
                Return 0
            End If

        Catch ex As Exception
            Throw ex
        Finally
            dt = Nothing
        End Try
        Return taxAmount
    End Function

    Public Function getTaxCode(ByVal locationInfoId As String, ByVal type As String, ByRef trans As SqlTransaction, ByRef cn As SqlConnection) As String

        Dim selectSQL As String
        Dim dm As New DBManager
        Dim dt As New DataTable
        Dim taxCode As String

        Try

            selectSQL = "SELECT TAXCODE FROM MISCPAYMENTTYPEMSTR WHERE LOCATIONINFOID = " + locationInfoId + _
                        " AND TAXCODE = '" + type + "'"

            dt = dm.execTable(selectSQL)

            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("TAXCODE").Equals(System.DBNull.Value) Then
                    Return "" 'If never setup
                End If
                taxCode = dt.Rows(0)("TAXCODE")
            Else
                Throw New Exception("Missing Tax Code")
            End If

        Catch ex As Exception
            Throw ex
        Finally
            dt = Nothing
        End Try
        Return taxCode
    End Function

    Public Function getDebtorCategory(ByVal debtorId As String) As String

        Dim selectSQL As String
        Dim dm As New DBManager
        Dim dt As New DataTable
        Dim category As String

        Try

            selectSQL = "SELECT CATEGORY FROM DEBTOR WHERE DEBTORID = " + debtorId

            dt = dm.execTable(selectSQL)

            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("CATEGORY").Equals(System.DBNull.Value) Then
                    Return "" 'If never setup
                End If
                category = dt.Rows(0)("CATEGORY")
            Else
                Throw New Exception("Missing Debtor Category")
            End If

        Catch ex As Exception
            Throw ex
        Finally
            dt = Nothing
        End Try
        Return category
    End Function
End Class
