Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Net.WebRequest
Imports System.IO
Imports System.Object

Partial Class Transaction_GenerateGSTFile
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Dim myDate As DateTime
    Dim myYear As Integer
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Delegate Function MyDelegate() As String
    Dim myContext As HttpContext
    Dim canDownload As Boolean = True
    Dim downloadPath As String = ""


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

        myContext = HttpContext.Current

    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)        
        hdPreview.Value = ""
    End Sub


    Private Sub DownloadFile(ByVal By)
        Response.ContentType = ContentType
        Response.AppendHeader("Content-Disposition", "attachment; filename=myFile.txt")
        Response.WriteFile(Server.MapPath("~/uploads/myFile.txt"))
        Response.Flush()
        System.IO.File.Delete(Server.MapPath("~/uploads/myFile.txt"))
        Response.End()
    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim t As New Thread(New ThreadStart(AddressOf DoAsyncWork))
        't.Priority = Threading.ThreadPriority.Normal
        't.Start()
        Dim d As MyDelegate = AddressOf DoAsyncWork
        d.BeginInvoke(New AsyncCallback(AddressOf MyCallback), Nothing)

        'Dim result As IAsyncResult
        While canDownload
            If Not String.IsNullOrEmpty(downloadPath) Then
                Response.ClearHeaders()
                Response.ClearContent()
                Response.ContentType = "application/ms-excel"
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + downloadPath)
                'Response.WriteFile(Server.MapPath("~/uploads/myFile.txt"))
                Response.Flush()
                Response.TransmitFile(downloadPath)
                'System.IO.File.Delete(downloadPath)
                Response.End()
                canDownload = False

            End If
        End While

        'Response.ClearContent()
        'Response.Buffer = True
        'Response.AddHeader("content-disposition", String.Format("attachment; filename={0}", d.EndInvoke()))
        'Response.ContentType = "application/ms-excel"
        'Response.End()

    End Sub

    Sub MyCallback(ByVal result As IAsyncResult)
        Console.WriteLine("Now I know PrintStuff finished")
        Dim resultClass = CType(result, IAsyncResult)
        Dim d As MyDelegate = CType(resultClass.AsyncDelegate, MyDelegate)
        'Console.WriteLine("And I also know that the result is: " & d.EndInvoke(result))
        Dim a As String = d.EndInvoke(result)

        canDownload = True
        downloadPath = a


        'myContext.Response.Clear() 'Clear the content of the response
        'myContext.Response.ClearContent()
        'myContext.Response.ClearHeaders() 'Buffer response so that page is sent
        '' after processing is complete.
        'myContext.Response.BufferOutput = True


        'myContext.Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", a))
        'myContext.Response.ContentType = "application/ms-excel"
        'myContext.Response.WriteFile(a)
        'myContext.Response.End()
        'HttpContext.Current.ApplicationInstance.CompleteRequest()

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

    Private Function DoAsyncWork() As String
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
                Sql = "select * from GSTExportVw with (nowait) where Years = " + ddYear.SelectedValue + " and Months = " + ddMonth.SelectedValue + _
                      " and locationinfoid = " + ddLocation.SelectedValue
            Else
                Sql = "select * from GSTExportVw with (nowait) where Years = " + ddYear.SelectedValue + " and Months = " + ddMonth.SelectedValue
            End If

            Sql = Sql + " order by seq"

            dt = dm.execTable(Sql)

            For iC = 0 To dt.Columns.Count - 6 'No Need to export the LocationInfoId,Seq,Source,Year,Month
                xlsheet.Cells(1, iC + 1).Value = dt.Columns(iC).ToString()
            Next
            iz = 1
            For iX = 0 To dt.Rows.Count - 1
                For iY = 0 To dt.Columns.Count - 6 'No Need to export the LocationInfoId,Seq,Source,Year,Month
                    Dim a As String = dt.Rows(iX)(iY).ToString()
                    xlsheet.Cells.NumberFormat = "@" 'Format Cell to Text
                    If a <> Nothing Then xlsheet.Cells(iz + 1, iY + 1).value = dt.Rows(iX)(iY).ToString()
                Next
                iz = iz + 1
            Next


            'app.Visible = True
            'app.UserControl = True
            'app.Quit()

            Dim saveAsName As String
            Dim todaysDate As String
            todaysDate = DateTime.Now.ToString("MM/dd/yyyy_hh_mm_ss")
            todaysDate = Replace(todaysDate, "/", "-")

            saveAsName = todaysDate & ".xls"

            app.DisplayAlerts = False
            xlsheet.SaveAs("C:\Temp\" + saveAsName, True)




            app.Quit()

            releaseobject(app)
            releaseobject(xlbook)
            releaseobject(xlsheet)


            Dim xlp() As System.Diagnostics.Process = System.Diagnostics.Process.GetProcessesByName("EXCEL")

            For Each Process As System.Diagnostics.Process In xlp
                Process.Kill()
                If System.Diagnostics.Process.GetProcessesByName("EXCEL").Length = 0 Then
                    Exit For
                End If
            Next

            logger.Debug("End Generating GST Files... " + DateTime.Now())

            Return "C:\Temp\" + saveAsName

        Catch ex As System.Runtime.InteropServices.COMException
            If ex.ErrorCode = -2147221164 Then
                lblMsg.Text = "Error in export: Please install Microsoft Office (Excel) to use the Export to Excel feature."
                logger.Warn(lblMsg.Text)
            ElseIf ex.ErrorCode = -2146827284 Then
                lblMsg.Text = "Error in export: Excel allows only 65,536 maximum rows in a sheet."
                logger.Warn(lblMsg.Text)
            Else
                lblMsg.Text = "Error in export: " & ex.Message.ToString()
                logger.Warn(lblMsg.Text)
            End If

        Catch ex As Exception
            lblMsg.Text = ex.Message
            logger.Warn(ex.Message)
        End Try
    End Function


End Class
