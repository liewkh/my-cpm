Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient
Imports System.IO

Partial Class Transaction_GenerateJournal
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim myDate As DateTime
    Dim myYear As Integer



    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        lp = Session("LoginProfile")
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

    Private Sub clear()
        lblMsg.Text = ""
        ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        ddMonth.SelectedIndex = 0
        ddYear.SelectedValue = 0

    End Sub


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
        hdPreview.Value = ""
    End Sub

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim sql As String = ""
        Dim locationInfoId As String = ""
        Dim i As Integer
        Dim dt As New DataTable
        Dim sContents As String = ""
        Dim locIdInd As String = ""
        
        Try
            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If

            trans = cn.BeginTransaction

            If ddYear.SelectedIndex = 0 Then
                lblMsg.Text = "Please select a Year."
                Exit Sub
            End If

            If ddMonth.SelectedIndex = 0 Then
                lblMsg.Text = "Please select a Month."
                Exit Sub
            End If

            logger.Debug("Start Generating Journal")

            If ddLocation.SelectedIndex = 0 Then
                locationInfoId = "%"
            Else
                locationInfoId = ddLocation.SelectedValue
            End If

            sql = "select * from billingSummaryVw where months = '" & ddMonth.SelectedValue & "' and years = ' " & ddYear.SelectedValue & "'" & _
                  " and locationinfoid like '" & locationInfoId & "'"

            dt = dm.execTable(sql)

            'If dt.Rows.Count > 0 Then
            '    For i = 0 To dt.Rows.Count - 1
            '        If locIdInd <> dt.Rows(i).Item("LOCATIONINFOID") Then
            '            sContents = sContents + ConstantGlobal.DSP + vbTab + dt.Rows(i).Item("AMOUNT")
            '        End If
            '        locIdInd = dt.Rows(i).Item("LOCATIONINFOID")
            '    Next

            '    Dim bAns As Boolean = SaveTextToFile(sContents, "D:\Test.txt", sErr)
            '    If bAns Then
            '        Debug.WriteLine("File Saved!")
            '    Else
            '        Debug.WriteLine("Error Saving File: " & sErr)
            '    End If

            'End If









            'Dim strDate As New DateTime(Now.Year, ddMonth.SelectedValue.ToString, 1)
            'Dim nextMonth As String
            'Dim passStr As String = ""
            'nextMonth = strDate.AddMonths(1).ToString("MMM")
            'Dim array As New ArrayList

            'If ddDebtor.SelectedIndex = 0 Then 'Generate for all the debtor inside the list
            '    For i = 0 To ddDebtor.Items.Count - 1
            '        If i = 0 Then 'Skip for the first empty 
            '            Continue For
            '        End If
            '        searchModel.setDebtorId(ddDebtor.Items(i).Value.ToString)
            '        searchModel.setMonthFrom(Now.Year & ddMonth.SelectedValue.ToString & "01")
            '        searchModel.setMonthto(Now.Year & nextMonth & "01")
            '        searchModel.setInvoicingFrequency(ddInvFreq.SelectedValue)

            '        Dim strSQL As String = sqlmap.getMappedStatement("BillGeneration/Generate-Invoice", searchModel)

            '        dt = dm.execTableInTrans(strSQL, cn, trans)

            '        For j = 0 To dt.Rows.Count - 1
            '            array.Add(dt.Rows(j).Item(0).ToString)
            '            genFlag = True
            '        Next j

            '        If genFlag Then
            '            noOfInvoice += 1
            '            'lbl.Text = "Processing : " & noOfInvoice.ToString & "/" & ddDebtor.Items.Count.ToString
            '            'me.UpdatePanel1     
            '            retValue = invMgr.createInvoice(ddDebtor.Items(i).Value.ToString, ddMonth.SelectedValue.ToString, array, BatchNo, cn, trans)
            '            trans.Commit()
            '            trans = cn.BeginTransaction
            '            genFlag = False
            '            array.Clear()
            '            generatedOnce = True
            '            'UpdatePanel1.Update()
            '        End If

            '    Next

            'Else 'Generate for selected debtor in the list
            '    searchModel.setDebtorId(ddDebtor.SelectedValue)
            '    searchModel.setMonthFrom(Now.Year & ddMonth.SelectedValue.ToString & "01")
            '    searchModel.setMonthto(Now.Year & nextMonth & "01")
            '    searchModel.setInvoicingFrequency(ddInvFreq.SelectedValue)

            '    Dim strSQL As String = sqlmap.getMappedStatement("BillGeneration/Generate-Invoice", searchModel)

            '    dt = dm.execTableInTrans(strSQL, cn, trans)

            '    For j = 0 To dt.Rows.Count - 1
            '        array.Add(dt.Rows(j).Item(0).ToString)
            '        'ReDim Preserve passCard(j)
            '        'passCard(j) = dt.Rows(j).Item(0).ToString
            '        'j = j + 1
            '        genFlag = True
            '    Next j

            '    If genFlag Then
            '        If Not (trans.Connection.State = ConnectionState.Open) Then
            '            trans = cn.BeginTransaction
            '        End If
            '        noOfInvoice += 1
            '        retValue = invMgr.createInvoice(ddDebtor.SelectedValue.ToString, ddMonth.SelectedValue.ToString, array, BatchNo, cn, trans)
            '        trans.Commit()
            '        array.Clear()
            '        generatedOnce = True
            '        genFlag = False

            '    End If

            'End If
            ''trans.Commit()

            'lblMsg.Text = ""
            'If generatedOnce Then
            '    lbl.Text = "Batch No : " & BatchNo & ". No Of Invoice Generated : " & noOfInvoice.ToString
            'Else
            '    lbl.Text = "No Of Invoice Generated : " & noOfInvoice.ToString
            'End If


            logger.Debug("End Generating Journal")

        Catch ex As Exception
            trans.Rollback()
            lblMsg.Text = ex.Message
            logger.Debug(ex.Message)
        Finally
            trans.Dispose()
            cn.Close()
        End Try
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ddLocation.SelectedIndex = 0
        ddMonth.SelectedIndex = 0
        ddYear.SelectedIndex = 0
        lblMsg.Text = ""
    End Sub

    Public Function GetFileContents(ByVal FullPath As String, Optional ByRef ErrInfo As String = "") As String

        Dim strContents As String
        Dim objReader As StreamReader
        Try

            objReader = New StreamReader(FullPath)
            strContents = objReader.ReadToEnd()
            objReader.Close()
            Return strContents
        Catch Ex As Exception
            ErrInfo = Ex.Message
        End Try
    End Function

    Public Function SaveTextToFile(ByVal strData As String, ByVal FullPath As String, Optional ByVal ErrInfo As String = "") As Boolean

        Dim bAns As Boolean = False
        Dim objReader As StreamWriter
        Try
            objReader = New StreamWriter(FullPath)
            objReader.Write(strData)
            objReader.Close()
            bAns = True
        Catch Ex As Exception
            ErrInfo = Ex.Message

        End Try
        Return bAns
    End Function
End Class
