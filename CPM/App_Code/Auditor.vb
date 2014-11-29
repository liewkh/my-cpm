Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Namespace Utility
    Public Class Auditor


        Private Shared logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Shared Sub log(ByVal lp As LoginProfile, _
                              ByVal moduleName As String, _
                              ByVal operationType As String, _
                              ByVal trans As SqlTransaction, _
                              ByVal conn As SqlConnection, _
                              ByVal details As String)

            'Dim auditLogEnt As New AUDIT_LOGEntity()
            'Dim auditLogDao As New AUDIT_LOGDAO()


            'Try
            '    logger.Debug("Invoking Auditor")

            '    Dim auditLogId As Byte() = HexEncoding.StringToHex(DataTypeUtils.getGUID().ToString, 1)
            '    auditLogEnt.AUDIT_LOG_ID = auditLogId
            '    auditLogEnt.DEFUNCT_IND = ConstantEnum.NO
            '    auditLogEnt.IP_ADDRESS = lp.getIpClientAddress
            '    auditLogEnt.HOSTNAME = lp.getClientHostName
            '    auditLogEnt.TRANSACTION_DATE = Now()

            '    auditLogEnt.OBJECT_TYPE = moduleName
            '    auditLogEnt.OPERATION_TYPE = operationType

            '    If (details.Length > 0) Then
            '        auditLogEnt.DETAILS = details
            '    End If

            '    auditLogEnt.LAST_UPDATED_DATETIME = Now()
            '    auditLogEnt.LAST_UPDATED_BY = HexEncoding.StringToHex(lp.getUserMstrId, 1)

            '    auditLogDao.Save(auditLogEnt, conn, trans)
            'Catch ex As Exception
            '    trans.Rollback()
            '    logger.Debug(ex.Message)
            'Finally
            '    logger.Debug("Finished Auditor")
            'End Try




        End Sub

        Public Shared Function log(ByVal conn As SqlConnection, ByVal trans As SqlTransaction, ByVal lp As LoginProfile, ByVal tableName As String, ByVal key As Long, ByVal description As String) As Long

            Dim auditTrailEnt As New CPM.AuditTrailEntity()
            Dim auditTrailDao As New CPM.AuditTrailDAO()


            Try
                logger.Debug("Invoking Auditor")
                auditTrailEnt.setChangeDescription(description)
                auditTrailEnt.setReferenceId(key)
                auditTrailEnt.setActive(ConstantGlobal.Yes)
                auditTrailEnt.setTableName(tableName)
                auditTrailEnt.setLastUpdatedDatetime(Now)
                auditTrailEnt.setLastUpdatedBy(lp.getUserMstrId)
                Return auditTrailDao.insertDB(auditTrailEnt, conn, trans)


            Catch ex As Exception
                trans.Rollback()
                logger.Debug(ex.Message)
            Finally
                logger.Debug("Finished Auditor")
            End Try




        End Function

    End Class

End Namespace


