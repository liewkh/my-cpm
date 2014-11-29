Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient



Module Module1

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Sub Main()

        Dim sql As String = ""

        Try

            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If

            trans = cn.BeginTransaction

            'Flag to SOR
            sql = "Select * from SOR where Active is null"

            Dim dr As DataRow
            Dim dt As DataTable = dm.execTableInTrans(sql, cn, trans)

            For Each dr In dt.Rows


                If (CDate(dr("StartMonth")).ToString("MMMMyyyy") = DateTime.Now.AddMonths(1).ToString("MMMMyyyy")) Then
                    If (CDate(CDate(dr("EndMonth")).ToString("MMMMyyyy")) >= CDate(DateTime.Now.ToString("MMMMyyyy"))) Then
                        Console.WriteLine("Within The Month")
                        sql = "Update PassCardMstr Set Status = 'SOR' where PassCardMstrId = " + dr("PassCardMstrId").ToString()
                        dm.execTableInTrans(sql, cn, trans)
                        sql = "Update DebtorPassBay Set Status = 'SOR' where PassCardMstrId = " + dr("PassCardMstrId").ToString()
                        dm.execTableInTrans(sql, cn, trans)
                        sql = "Update Sor Set Active = 'Y' where SorId = " + dr("SorId").ToString()
                        dm.execTableInTrans(sql, cn, trans)

                    End If
                End If
            Next

            'Unflag SOR
            sql = "Select * from SOR where Active ='Y'"

            dt = dm.execTableInTrans(sql, cn, trans)

            For Each dr In dt.Rows
                Console.WriteLine(dr("PassCardMstrId"))
                Console.WriteLine(dr("StartMonth"))
                Console.WriteLine(dr("EndMonth"))
                Console.WriteLine(dr("DebtorId"))
                Console.WriteLine(dr("SorId"))
                Console.WriteLine(dr("Remarks"))

                If (CDate(dr("EndMonth")).ToString("MMMMyyyy") = CDate(DateTime.Now.ToString("MMMMyyyy"))) Then
                    Console.WriteLine("Expired!")
                    sql = "Update PassCardMstr Set Status = 'IU' where PassCardMstrId = " + dr("PassCardMstrId").ToString()
                    dm.execTableInTrans(sql, cn, trans)
                    sql = "Update DebtorPassBay Set Status = 'A' where PassCardMstrId = " + dr("PassCardMstrId").ToString()
                    dm.execTableInTrans(sql, cn, trans)
                    sql = "Update Sor Set Active = 'N' where SorId = " + dr("SorId").ToString()
                    dm.execTableInTrans(sql, cn, trans)
                End If


            Next






            trans.Commit()

        Catch ex As Exception
            trans.Rollback()
            logger.Debug("ERROR. : " + ex.Message)

        Finally
            trans.Dispose()
            cn.Close()
            End
        End Try





    End Sub

End Module


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

   

End Class
