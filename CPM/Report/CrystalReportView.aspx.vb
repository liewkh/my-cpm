Imports CrystalDecisions.Web.CrystalReportViewer
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine

Partial Class Report_CrystalReportView
    Inherits System.Web.UI.Page
    Dim rptMgr As New ReportManager
    Dim lp As New LoginProfile()

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        lp = Session("LoginProfile")
        If IsNothing(lp) Then
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("appPath") + "/login.aspx?login=expired")
        End If
        Session.LCID = 2057
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'dun store in cache
        If Not Page.IsPostBack Then
            Response.Expires = 0
            Response.Cache.SetNoStore()
            Response.AppendHeader("Pragma", "no-cache")
        End If

    End Sub
End Class
