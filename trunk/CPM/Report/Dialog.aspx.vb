Imports CrystalDecisions.Web.CrystalReportViewer
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Partial Class Report_Dialog
    Inherits System.Web.UI.Page
    Dim rptMgr As New ReportManager
    Dim lp As New LoginProfile()
    Dim src As Integer = 0
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        lp = Session("LoginProfile")
        If IsNothing(lp) Then
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("appPath") + "/login.aspx?login=expired")
        End If
        Session.LCID = 2057
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'get the reportmanager from session
        If Request.Params("key") Is Nothing Then
            rptMgr = Session("ReportManager")
            src = 0
        Else
            rptMgr = Session("ReportManager1")
            src = 1
        End If

        If Not rptMgr Is Nothing Then
            previewInPDF(rptMgr.getRptDoc)
        End If

        'rptMgr.setViewer(CRViewer)

        'rptMgr.setStandardButton()
        'rptMgr.getReport()
    End Sub
    Public Sub previewInPDF(ByVal crReportDocument As ReportDocument)

        Dim crExportOptions As ExportOptions
        Dim crDiskFileDestinationOptions As DiskFileDestinationOptions
        Dim Fname As String
        Dim exportPath As String = System.Configuration.ConfigurationManager.AppSettings("CRReportPath") + "exports\"
        If Not My.Computer.FileSystem.DirectoryExists(exportPath) Then
            My.Computer.FileSystem.CreateDirectory(exportPath)
        End If
        Fname = exportPath & Session.SessionID.ToString & ".pdf"

        crDiskFileDestinationOptions = New DiskFileDestinationOptions()
        crDiskFileDestinationOptions.DiskFileName = Fname

        crExportOptions = crReportDocument.ExportOptions
        With crExportOptions
            .DestinationOptions = crDiskFileDestinationOptions
            .ExportDestinationType = ExportDestinationType.DiskFile
            .ExportFormatType = ExportFormatType.PortableDocFormat            
        End With

        crReportDocument.Export()
        ' The following code writes the pdf file to the Client’s browser.
        Response.ClearContent()
        Response.ClearHeaders()
        Response.ContentType = "application/pdf"
        Response.WriteFile(Fname)
        Response.Flush()
        Response.Close()
        'delete the exported file from disk
        System.IO.File.Delete(Fname)
        If src = 0 Then
            Session("ReportManager") = Nothing
        Else
            Session("ReportManager1") = Nothing
        End If
    End Sub
End Class
