Imports Microsoft.VisualBasic
Imports CrystalDecisions.Web.CrystalReportViewer
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine


Public Class ReportManager

#Region "Dim"
    Dim crViewer As CrystalDecisions.Web.CrystalReportViewer
    Dim CR As New ReportDocument
    'table Logon
    Dim crTableLogonInfos As New TableLogOnInfos
    Dim crTableLogonInfo As TableLogOnInfo
    Dim crConnectionInfo As ConnectionInfo
    'parameter
    Dim crParameterDiscreteValue As ParameterDiscreteValue
    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
    Dim crParameterFieldDefinition As ParameterFieldDefinition
    Dim crParameterValues As ParameterValues
    Dim crParameterRangeValue As ParameterRangeValue
#End Region

#Region "Report Name"
    Public Function setReportName(ByVal ReportName As String) As Integer
        Dim path As String
        Dim CRpath As String = System.Configuration.ConfigurationManager.AppSettings("CRReportPath")
        path = CRpath + ReportName
        CR.Load(path)
        crParameterFieldDefinitions = CR.DataDefinition.ParameterFields

    End Function
#End Region

#Region "Parameter"
    Public Function setParameterDiscrete(ByVal Parameter As String, ByVal Value As Object) As Integer
        crParameterFieldDefinition = crParameterFieldDefinitions.Item(Parameter)
        crParameterValues = crParameterFieldDefinition.CurrentValues
        crParameterDiscreteValue = New ParameterDiscreteValue
        crParameterDiscreteValue.Value = Value
        crParameterValues.Add(crParameterDiscreteValue)
        crParameterFieldDefinition.ApplyCurrentValues(crParameterValues)
    End Function

    Public Function setParameterRange(ByVal Parameter As String, ByVal LowerValue As Object, ByVal UpperValue As Object) As Integer
        crParameterRangeValue = New ParameterRangeValue()
        With crParameterRangeValue
            .EndValue = UpperValue
            .LowerBoundType = RangeBoundType.BoundInclusive
            .StartValue = LowerValue
            .UpperBoundType = RangeBoundType.BoundInclusive
        End With
        crParameterFieldDefinition = crParameterFieldDefinitions.Item(Parameter)
        crParameterValues = crParameterFieldDefinition.CurrentValues
        crParameterDiscreteValue = New ParameterDiscreteValue
        crParameterValues.Add(crParameterRangeValue)
        crParameterFieldDefinition.ApplyCurrentValues(crParameterValues)
    End Function
#End Region

#Region "TableLogOnInfo"
    'Public Function setTablesLogOnInfo(ByVal TableName As String) As Integer
    '    crTableLogonInfo = New TableLogOnInfo()
    '    crConnectionInfo = New ConnectionInfo()
    '    Dim serverName As String = System.Configuration.ConfigurationManager.AppSettings("CRServerName")
    '    Dim dbName As String = System.Configuration.ConfigurationManager.AppSettings("CRDatabaseName")
    '    Dim userID As String = System.Configuration.ConfigurationManager.AppSettings("CRUserID")
    '    Dim passWord As String = System.Configuration.ConfigurationManager.AppSettings("CRPassword")
    '    With crConnectionInfo
    '        .ServerName = serverName
    '        .DatabaseName = dbName
    '        .UserID = userID
    '        .Password = passWord
    '    End With
    '    crTableLogonInfo.ConnectionInfo = crConnectionInfo
    '    crTableLogonInfo.TableName = TableName
    '    crTableLogonInfos.Add(crTableLogonInfo)
    'End Function

    'Public Function setTablesLogOnInfo() As Integer
    '    crTableLogonInfo = New TableLogOnInfo()
    '    crConnectionInfo = New ConnectionInfo()
    '    Dim serverName As String = System.Configuration.ConfigurationManager.AppSettings("CRServerName")
    '    Dim dbName As String = System.Configuration.ConfigurationManager.AppSettings("CRDatabaseName")
    '    Dim userID As String = System.Configuration.ConfigurationManager.AppSettings("CRUserID")
    '    Dim passWord As String = System.Configuration.ConfigurationManager.AppSettings("CRPassword")
    '    With crConnectionInfo
    '        .ServerName = serverName
    '        .DatabaseName = dbName
    '        .UserID = userID
    '        .Password = passWord
    '    End With
    '    crTableLogonInfo.ConnectionInfo = crConnectionInfo
    '    crTableLogonInfo.TableName = "COMMAND"
    '    crTableLogonInfos.Add(crTableLogonInfo)
    'End Function

    Private Function setLogonConnection() As Boolean
        'function to setup the connection
        crConnectionInfo = New ConnectionInfo()
        Dim serverName As String = System.Configuration.ConfigurationManager.AppSettings("CRServerName")
        Dim dbName As String = System.Configuration.ConfigurationManager.AppSettings("CRDatabaseName")
        Dim userID As String = System.Configuration.ConfigurationManager.AppSettings("CRUserID")
        Dim passWord As String = System.Configuration.ConfigurationManager.AppSettings("CRPassword")
        With crConnectionInfo
            .ServerName = dbName 'Initailly Was serverName
            .DatabaseName = dbName
            .UserID = userID
            .Password = passWord        
        End With
        'CR.SetDatabaseLogon(userID, passWord, serverName, dbName)

    End Function

    Private Function ApplyLogonMain() As Boolean
        Dim li As TableLogOnInfo
        Dim tbl As Table

        Try
            For Each tbl In CR.Database.Tables
                li = tbl.LogOnInfo
                li.ConnectionInfo = crConnectionInfo
                tbl.ApplyLogOnInfo(li)
                'tbl.Location = crConnectionInfo.DatabaseName & ".dbo." & tbl.Location.Substring(tbl.Location.LastIndexOf(".") + 1)
            Next
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function ApplyLogonSubReport() As Boolean
        Dim li As TableLogOnInfo
        Dim tbl As Table

        Dim obj As ReportObject
        Dim subObj As SubreportObject

        Try
            For Each obj In CR.ReportDefinition.ReportObjects()
                If (obj.Kind = ReportObjectKind.SubreportObject) Then
                    subObj = CType(obj, SubreportObject)
                    For Each tbl In CR.OpenSubreport(subObj.SubreportName).Database.Tables
                        li = tbl.LogOnInfo
                        li.ConnectionInfo = crConnectionInfo
                        tbl.ApplyLogOnInfo(li)
                    Next
                End If
            Next
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function Logon() As Boolean
        setLogonConnection()
        If Not (ApplyLogonMain()) Then
            Return False
        End If
        If Not (ApplyLogonSubReport()) Then
            Return False
        End If
        Return True
    End Function
#End Region

#Region "Viewer"
    Public Function getReport() As Integer
        crViewer.LogOnInfo = crTableLogonInfos
        'CR.Database.Tables(0).ApplyLogOnInfo(crTableLogonInfo)
        crViewer.ReportSource = CR
    End Function

    Public Function printReportDirect(ByVal copies As Integer, ByVal collated As Boolean, ByVal startPage As Integer, ByVal endPage As Integer) As Integer
        'collated default to false for the moment
        'startPage, endPage set to 0 if print all page
        Try
            Dim defaultPrinterName As String = ""

            For Each printerName As String In System.Drawing.Printing.PrinterSettings.InstalledPrinters
                Dim printer As New System.Drawing.Printing.PrinterSettings
                printer.PrinterName = printerName
                If printer.IsDefaultPrinter Then
                    defaultPrinterName = printerName
                End If
            Next

            If Not String.IsNullOrEmpty(defaultPrinterName) Then
                CR.PrintOptions.PrinterName = defaultPrinterName
                CR.PrintToPrinter(copies, False, startPage, endPage)
            End If

            Return 1
        Catch ex As Exception
            Dim test As String = ex.ToString
            Return 0
        End Try
    End Function

    Public Function setViewer(ByRef viewer) As Integer
        crViewer = viewer
    End Function

    Public Function setStandardButton() As Integer
        crViewer.HasCrystalLogo = False
        crViewer.HasDrillUpButton = False
        crViewer.HasExportButton = True
        crViewer.HasGotoPageButton = False
        crViewer.HasRefreshButton = False
        crViewer.HasSearchButton = True
        crViewer.HasToggleGroupTreeButton = False
        crViewer.HasPrintButton = True
        crViewer.HasPageNavigationButtons = True
        crViewer.DisplayGroupTree = False
        crViewer.HasZoomFactorList = False
        crViewer.HasViewList = False
        crViewer.DisplayToolbar = True
        crViewer.Width = 20
    End Function

    'Public Function test() As Integer

    '    Dim crExportOptions As ExportOptions
    '    Dim crDiskFileDestinationOptions As DiskFileDestinationOptions
    '    Dim Fname As String
    '    Fname = "c:\exports\LCS.pdf"
    '    crDiskFileDestinationOptions = New DiskFileDestinationOptions()
    '    crDiskFileDestinationOptions.DiskFileName = Fname
    '    crExportOptions = CR.ExportOptions
    '    With crExportOptions
    '        .DestinationOptions = crDiskFileDestinationOptions
    '        .ExportDestinationType = ExportDestinationType.DiskFile
    '        .ExportFormatType = ExportFormatType.PortableDocFormat
    '    End With
    '    CR.Export()

    '    Return 1
    'End Function
    Public Function getRptDoc()
        'CR.SetDatabaseLogon("sa", "123", "LOCALHOST", "CPM")

        'CR.VerifyDatabase()
        Return CR
    End Function
#End Region

End Class
