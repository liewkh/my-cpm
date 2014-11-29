Imports Microsoft.VisualBasic
Imports System.Xml
Imports System.Xml.XPath
Imports System.Reflection
Imports System.web

Public Class SQLMap
    Private pSqlMapName As String
    Private pSqlMapSource As String
    Private pSearchModel As Object
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Public Function getMappedStatement(ByVal mapName As String, ByRef searchModel As Object)
        Try
            Dim nameArr = Split(mapName, "/")
            Me.pSqlMapSource = nameArr(0)
            Me.pSqlMapName = nameArr(1)
            Me.pSearchModel = searchModel
            Return mapSql()
        Catch e As Exception
            Throw New Exception(e.Message, e)
            logger.Error(e.Message)
        End Try
    End Function

    Private Function mapSql()
        Try
            Dim pXldDocument As New XmlDocument
            Dim pXpnNavigator As XPathNavigator
            Dim pXniIterator As XPathNodeIterator
            Dim xIterator As XPathNodeIterator
            Dim pName As String
            Dim filePath As String
            Dim sql As String = ""
            Dim type As String
            Dim prepend As String

            Dim sqlMapFiles As New SQLMapLookup
            sqlMapFiles = System.Web.HttpContext.Current.Application("SQLMapLookup")
            filePath = sqlMapFiles.lookup(pSqlMapSource)

            logger.Info("XML file loaded: " + filePath)
            pXldDocument.Load(filePath)
            pXpnNavigator = pXldDocument.CreateNavigator

            pXniIterator = pXpnNavigator.Select("sql-map/dynamic-mapped-statement")
            While pXniIterator.MoveNext()
                pName = pXniIterator.Current.GetAttribute("name", "")
                Dim dynamicNode As String
                If pName.Equals(pSqlMapName) Then
                    xIterator = pXniIterator.Current.Select("part")
                    While xIterator.MoveNext()
                        type = xIterator.Current.GetAttribute("type", "")
                        If type.Equals("static") Then
                            sql = sql + xIterator.Current.Value '.ToUpper
                        Else

                            dynamicNode = xIterator.Current.Value
                            Dim NewDateVar = Split(dynamicNode, "#")

                            Dim result As String = pSearchModel.GetType.InvokeMember("get" + NewDateVar(1), _
                             BindingFlags.InvokeMethod, Nothing, _
                            pSearchModel, Nothing)


                            Dim abc As String = xIterator.Current.GetAttribute("skip", "")
                            If abc <> "" Then
                                If result = abc Then
                                    Continue While
                                End If
                            End If
                            

                            If Not String.IsNullOrEmpty(result) Then 'if value is null, don't build
                                If Not sql.ToUpper.Contains("WHERE") Then 'If no where clause, add in
                                    sql = sql + " WHERE"
                                Else 'add prepend e.g AND, OR & etc
                                    prepend = xIterator.Current.GetAttribute("prepend", "")
                                    sql = sql + " " + prepend.ToUpper
                                End If
                                sql = sql + NewDateVar(0) 'Add static part
                                If NewDateVar(0).ToString.ToUpper.Contains("IN") Then
                                    For i As Integer = 0 To Split(result, ",").Length - 1
                                        sql = sql + parseDataType(Split(result, ",")(i)) 'Add dynamic part
                                        If i < Split(result, ",").Length - 1 Then
                                            sql = sql + "," 'Add separator
                                        End If
                                    Next
                                Else
                                    sql = sql + parseDataType(result) 'Add dynamic part
                                End If
                                sql = sql + NewDateVar(2) 'Add static part
                            End If
                        End If
                    End While
                    Exit While
                End If
            End While
            '    End If

            'End While
            logger.Info("Mapped Statement: " + sql)
            Return sql
        Catch e As Exception
            Throw New Exception("SqlMapErr: Mapping Error. " + e.Message, e)
        End Try
    End Function
    Private Function parseDataType(ByVal value As Object) As String
        Try
            If value Is Nothing Then Return "NULL"

            Select Case value.GetType.ToString.ToUpper
                Case "SYSTEM.STRING"
                    value = CType(value, String).Replace("'", "''")
                    Return "'" & value & "'"
                Case "SYSTEM.DATETIME"
                    If Year(value) = 1 Then
                        Return "NULL"
                    Else
                        Return "'" & CDate(value).ToString("yyyy-MM-dd HH:mm:ss") & "'"
                    End If
                Case "SYSTEM.CHAR" : Return "'" & value & "'"
                Case Else : Return value
            End Select
        Catch e As Exception
            Throw New Exception("SqlMapErr: Parse Data Type Error.", e)
            logger.Error("SqlMapErr: Parse Data Type Error. " + e.Message)
        End Try
    End Function

End Class




