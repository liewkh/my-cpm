Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Maintenance_LocationMsgSetup
    Inherits System.Web.UI.Page
    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        lp = Session("LoginProfile")
        If IsNothing(lp) Then
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("appPath") + "/login.aspx?login=expired")
        End If

    End Sub

    Private Sub bindData()
        Dim searchModel As New CPM.LocationInfoEntity
        Dim BranchInfoDao As New CPM.LocationInfoDAO
        Dim sqlmap As New SQLMap

        Try

            searchModel.setLocationCode(Trim(txtLocationCode.Text.ToUpper))
            searchModel.setLocationName(Trim(txtLocationName.Text.ToUpper))

            Dim strSQL As String = sqlmap.getMappedStatement("SetupMstr/Search-LocationInfo", searchModel)

            ViewState("strSQL") = strSQL


            dsLocation.SelectCommand = ViewState("strSQL")
            gvLocation.DataBind()

            gvLocation.PageIndex = 0

            If gvLocation.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsLocation).ToString + " " + "Record Found"
            End If

        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            BranchInfoDao = Nothing
            searchModel = Nothing
            sqlmap = Nothing

        End Try

    End Sub

    Private Sub clear()
        lblmsg.Text = ""
        lblRecCount.Text = ""
        txtLocationCode.Text = ""
        txtLocationName.Text = ""
        txtMessage1.Text = ""
        txtMessage2.Text = ""
        txtMessage3.Text = ""
        txtLocationCode.Enabled = True
        txtLocationName.Enabled = True

        chkAll.Checked = False
        gvLocation.DataSource = Nothing
        addMode()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        bindData()
    End Sub


    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        clear()
        ViewState("strSQL") = Nothing
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvLocation.Rows.Count
            ClientScript.RegisterForEventValidation(gvLocation.UniqueID, "Select$" + i.ToString)
        Next

        MyBase.Render(writer)
    End Sub

    Protected Sub gvLocation_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvLocation.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvLocation.UniqueID + "','Select$" + gvLocation.Rows.Count.ToString + "');")            
            Dim int As Integer = gvLocation.Rows.Count / 2
            Dim dob As Double = gvLocation.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If

        End If
    End Sub

    Protected Sub gvLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvLocation.SelectedIndexChanged
        Dim locInfoDao As New CPM.LocationInfoDAO

        Try

            lblmsg.Text = ""

            txtLocationCode.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_LocationCode))
            txtLocationName.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_LocationName))
            txtMessage1.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_LocationMessage1))
            txtMessage2.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_LocationMessage2))
            txtMessage3.Text = Utility.DataTypeUtils.parseHTMLSafeToString(gvLocation.SelectedDataKey(locInfoDao.COLUMN_LocationMessage3))

            updateMode()

        Catch ex As Exception
            lblmsg.Text = ex.Message
            'Throw ex

        Finally
            locInfoDao = Nothing
        End Try

    End Sub

    Private Sub addMode()

        btnUpdate.Enabled = False
        btnSearch.Enabled = True

        txtLocationCode.ReadOnly = False
        txtLocationCode.CssClass = CSSEnum.TXTFIELD_2
    End Sub

    Private Sub updateMode()
        btnUpdate.Enabled = True
        btnSearch.Enabled = False

        txtLocationCode.ReadOnly = True
        txtLocationCode.CssClass = CSSEnum.TXTFIELD_2_DISABLED
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        lblmsg.Text = ""

        If Not Page.IsValid Then
            Exit Sub
        End If

        cn = New SqlConnection(dm.getDBConn)
        If Not cn.State = ConnectionState.Open Then
            cn.Open()
        End If
        trans = cn.BeginTransaction

        Try

            UpdateRecord(cn, trans)
            trans.Commit()
            clear()
            Me.bindData()

            lblmsg.Text = ConstantGlobal.Record_Updated


        Catch ex As Exception
            lblmsg.Text = ex.Message
            trans.Rollback()
        Finally
            trans.Dispose()
            cn.Close()
        End Try
    End Sub

    Protected Sub UpdateRecord(ByVal cn As SqlConnection, ByVal trans As SqlTransaction)

        Dim locInfoEnt As New CPM.LocationInfoEntity
        Dim searchModel As New CPM.LocationInfoEntity
        Dim locInfoDao As New CPM.LocationInfoDAO
        Dim sqlmap As New SQLMap
        Dim dt As New DataTable
        Dim i As Integer

        Try

            'If Trim(txtMessage1.Text) <> "" Or Trim(txtMessage2.Text) <> "" Or Trim(txtMessage3.Text) <> "" Then
                If chkAll.Checked = True Then
                    searchModel.setLocationCode(Trim(txtLocationCode.Text.ToUpper))
                    searchModel.setLocationName(Trim(txtLocationName.Text.ToUpper))

                    Dim strSQL As String = sqlmap.getMappedStatement("SetupMstr/Search-LocationInfo", searchModel)

                    dt = dm.execTable(strSQL)

                    For i = 0 To dt.Rows.Count - 1
                        locInfoEnt.setLocationInfoId(dt.Rows(i)(locInfoDao.COLUMN_LocationInfoID).ToString())
                        locInfoEnt.setLocationMessage1(txtMessage1.Text.Trim())
                        locInfoEnt.setLocationMessage2(txtMessage2.Text.Trim())
                        locInfoEnt.setLocationMessage3(txtMessage3.Text.Trim())
                        locInfoEnt.setLastUpdatedDatetime(Now)
                        locInfoEnt.setLastUpdatedBy(lp.getUserMstrId)
                        locInfoDao.updateDB(locInfoEnt, cn, trans)
                    Next i

                Else
                    locInfoEnt.setLocationInfoId(gvLocation.SelectedDataKey(locInfoDao.COLUMN_LocationInfoID))
                    locInfoEnt.setLocationMessage1(txtMessage1.Text.Trim())
                    locInfoEnt.setLocationMessage2(txtMessage2.Text.Trim())
                    locInfoEnt.setLocationMessage3(txtMessage3.Text.Trim())
                    locInfoEnt.setLastUpdatedDatetime(gvLocation.SelectedDataKey("LUDT"))
                    locInfoEnt.setLastUpdatedBy(lp.getUserMstrId)
                    locInfoDao.updateDB(locInfoEnt, cn, trans)
                End If

            'End If




        Catch ex As Exception
            Throw ex
        Finally
            locInfoEnt = Nothing
            locInfoDao = Nothing

        End Try



    End Sub

    Protected Sub gvLocation_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvLocation.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindData()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)        
    End Sub

    Protected Sub chkAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If chkAll.Checked = True Then
            txtLocationCode.Enabled = False
            txtLocationName.Enabled = False
            txtLocationCode.Text = ""
            txtLocationName.Text = ""
        Else
            txtLocationCode.Enabled = True
            txtLocationName.Enabled = True
        End If
    End Sub

End Class