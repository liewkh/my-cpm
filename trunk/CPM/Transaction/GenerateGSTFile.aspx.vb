Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports System.Threading

Partial Class Transaction_GenerateGSTFile
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Dim myDate As DateTime
    Dim myYear As Integer
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)



    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        lp = Session("LoginProfile")
        Session.LCID = 2057

        If IsNothing(lp) Then
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("appPath") + "/login.aspx?login=expired")
        End If

        If Not Page.IsPostBack Then


            Dim sql As String = "select locationinfoid,locationName,0 as seq from locationinfo where locationinfoid = " & lp.getDefaultLocationInfoId & _
                                "union select -1 as codemstrid,codedesc,seq from codemstr where codecat = 'ALL' " & _
                                "union select el.locationinfoid,li.locationname,0 as seq from employeelocation el,locationinfo li " & _
                                "where el.locationinfoid = li.locationinfoid and el.employeemstrid = " & lp.getEmployeeMstrId & " order by seq,locationname"
            dsLocation.SelectCommand = sql
            dsLocation.DataBind()

            ddLocation.SelectedValue = lp.getDefaultLocationInfoId

            ddMonth.SelectedValue = Now.Month
            ddLocation.SelectedValue = lp.getDefaultLocationInfoId

            myDate = DateTime.Now()
            Dim x As Integer
            myYear = myDate.Year

            x = myYear - 5
            For x = x To (x + 10)
                ddYear.Items.Add(x)
            Next x

            ddYear.Items.FindByText(myYear).Selected = True

        End If

    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)        
        hdPreview.Value = ""
    End Sub


    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim t As New Thread(New ThreadStart(AddressOf DoAsyncWork))
        t.Priority = Threading.ThreadPriority.Normal
        t.Start()
    End Sub

    Private Sub releaseobject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        End Try
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ddLocation.SelectedIndex = 0
    End Sub

    Private Sub DoAsyncWork()
        Try

            Dim app As Object
            Dim xlbook As Object
            Dim xlsheet As Object
            app = CreateObject("Excel.Application")
            xlbook = app.Workbooks.Add()
            xlsheet = xlbook.ActiveSheet

            Dim iX As Integer
            Dim iY As Integer
            Dim iC As Integer
            Dim iz As Integer
            Dim Sql As String
            Dim dt As DataTable

            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If

            trans = cn.BeginTransaction

            logger.Debug("Start Generating GST Files... " + DateTime.Now())

            If ddLocation.SelectedIndex > 0 Then
                Sql = "select * from GSTExportVw where Years = " + ddYear.SelectedValue + " and Months = " + ddMonth.SelectedValue + _
                      " and locationinfoid = " + ddLocation.SelectedValue
            Else
                Sql = "select * from GSTExportVw where Years = " + ddYear.SelectedValue + " and Months = " + ddMonth.SelectedValue
            End If

            Sql = Sql + " order by date,seq"

            dt = dm.execTable(Sql)

            For iC = 0 To dt.Columns.Count - 6 'No Need to export the LocationInfoId,Seq,Source,Year,Month
                xlsheet.Cells(1, iC + 1).Value = dt.Columns(iC).ToString()
            Next
            iz = 1
            For iX = 0 To dt.Rows.Count - 1
                For iY = 0 To dt.Columns.Count - 6 'No Need to export the LocationInfoId,Seq,Source,Year,Month
                    Dim a As String = dt.Rows(iX)(iY).ToString()
                    If a <> Nothing Then xlsheet.Cells(iz + 1, iY + 1).value = dt.Rows(iX)(iY).ToString()
                Next
                iz = iz + 1
            Next


            app.Visible = True
            app.UserControl = True

            Dim saveAsName As String
            Dim todaysDate As String
            todaysDate = Today()
            todaysDate = Replace(todaysDate, "/", "-")

            saveAsName = todaysDate & ".xls"

            xlsheet.SaveAs("C:\Temp\" + saveAsName)

            releaseobject(app)
            releaseobject(xlbook)
            releaseobject(xlsheet)

            logger.Debug("End Generating GST Files... " + DateTime.Now())

        Catch ex As System.Runtime.InteropServices.COMException
            If ex.ErrorCode = -2147221164 Then
                lblMsg.Text = "Error in export: Please install Microsoft Office (Excel) to use the Export to Excel feature."
            ElseIf ex.ErrorCode = -2146827284 Then
                lblMsg.Text = "Error in export: Excel allows only 65,536 maximum rows in a sheet."
            Else
                lblMsg.Text = (("Error in export: " & ex.Message) + Environment.NewLine & " Error: ") + ex.ErrorCode
            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message
            logger.Warn(ex.Message)
        End Try
    End Sub


End Class
