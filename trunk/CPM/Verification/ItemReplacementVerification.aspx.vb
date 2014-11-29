Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient

Partial Class Verification_ItemReplacementVerification
    Inherits System.Web.UI.Page

    Dim dm As New DBManager
    Dim cn As SqlConnection
    Dim trans As SqlTransaction
    Dim lp As New LoginProfile
    Private logger As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim manualReceipt As String = ""


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try
            Session.LCID = 2057

            lp = Session("LoginProfile")
            If IsNothing(lp) Then
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("appPath") + "/login.aspx?login=expired")
            End If
            If Not Page.IsPostBack Then


                Dim sql As String = "select locationinfoid,locationName,0 as seq from locationinfo where locationinfoid = " & lp.getDefaultLocationInfoId & _
                                    "union select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' " & _
                                    "union select el.locationinfoid,li.locationname,0 as seq from employeelocation el,locationinfo li " & _
                                    "where el.locationinfoid = li.locationinfoid and el.employeemstrid = " & lp.getEmployeeMstrId & " order by seq,locationname"
                dsLocation.SelectCommand = sql
                dsLocation.DataBind()

                txtVerifiedBy.Text = lp.getUserName
                bindData()

            End If
            ddLocation.SelectedValue = lp.getDefaultLocationInfoId

        Catch ex As Exception
            lblmsg.Text = ex.Message

        End Try




    End Sub

    Private Sub bindData()
        Dim searchModel As New ItemReplacementSearchModel
        Dim sqlmap As New SQLMap

        Try

            lblmsg.Text = ""

            If ddLocation.SelectedIndex = 0 Then
                Dim a As ListItem
                Dim b As String = ""
                For Each a In ddLocation.Items
                    b = b + a.Value.ToString() + ","
                Next
                searchModel.setLocationInfoId(b)
            Else
                searchModel.setLocationInfoId(ddLocation.SelectedValue)
            End If

            searchModel.setStatus(ItemReplacementStatusEnum.SUBMITTED)

            Dim strSQL As String = sqlmap.getMappedStatement("ItemReplacement/Search-ItemReplacement", searchModel)

            ViewState("strSQL") = strSQL


            dsIRV.SelectCommand = ViewState("strSQL")
            gvIRV.DataBind()

            gvIRV.PageIndex = 0

            If gvIRV.Rows.Count = 0 Then
                lblRecCount.Text = ConstantGlobal.No_Record_Found
            Else
                lblRecCount.Text = dm.getGridViewRecordCount(dsIRV).ToString + " " + "Record Found"
            End If


        Catch ex As Exception
            lblmsg.Text = ex.Message

        Finally
            searchModel = Nothing
            sqlmap = Nothing

        End Try

    End Sub

    Private Sub clear()
        lblmsg.Text = ""
        lblRecCount.Text = ""
        ddLocation.SelectedValue = lp.getDefaultLocationInfoId
        gvIRV.DataSource = Nothing

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        bindData()
    End Sub


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        For i As Integer = 0 To gvIRV.Rows.Count
            ClientScript.RegisterForEventValidation(gvIRV.UniqueID, "Select$" + i.ToString)
        Next

        MyBase.Render(writer)
    End Sub

    Protected Sub gvIRV_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvIRV.RowDataBound

        If (e.Row.RowType.ToString.Equals(System.Web.UI.WebControls.DataControlRowType.DataRow.ToString)) Then
            e.Row.Attributes.Add("OnMouseOver", "javascript:this.className='tb-highlight';")
            e.Row.Attributes.Add("OnClick", "javascript:__doPostBack('" + gvIRV.UniqueID + "','Select$" + gvIRV.Rows.Count.ToString + "');")
            Dim int As Integer = gvIRV.Rows.Count / 2
            Dim dob As Double = gvIRV.Rows.Count / 2

            If (dob.Equals(int)) Then
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row1';")
            Else
                e.Row.Attributes.Add("OnMouseOut", "javascript:this.className='grid_row2';")
            End If
        End If

    End Sub


    Protected Sub gvIRV_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvIRV.RowCommand
        'For Sorting
        If Not e.CommandName.Equals("Select") Then
            bindData()
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub



    Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim irEnt As New CPM.ItemReplacementEntity
        Dim irDao As New CPM.ItemReplacementDAO
        Dim gotChecked As Boolean = False
        Dim irId As String = ""
        Dim count = gvIRV.Rows.Count

        Try

            cn = New SqlConnection(dm.getDBConn)
            If Not cn.State = ConnectionState.Open Then
                cn.Open()
            End If
            trans = cn.BeginTransaction


            If Not Page.IsValid Then
                Exit Sub
            End If


            For Each row1 As GridViewRow In gvIRV.Rows
                Dim chk1 As CheckBox
                chk1 = row1.FindControl("chkSelect")
                If Not chk1 Is Nothing Then
                    If chk1.Checked Then
                        gotChecked = True
                        irId = gvIRV.DataKeys(row1.RowIndex)(irDao.COLUMN_ItemReplacementID).ToString
                        irEnt.setItemReplacementId(irId)
                        irEnt.setStatus(ItemReplacementStatusEnum.VERIFIED)
                        irEnt.setLastUpdatedDatetime(Now)
                        irEnt.setVerifiedBy(lp.getUserMstrId)
                        irEnt.setLastUpdatedBy(lp.getUserMstrId)
                        irDao.updateDB(irEnt, cn, trans)
                    End If
                End If
            Next

            If Not gotChecked Then
                If count > 0 Then
                    lblRecCount.Text = "Please made a selection."
                    Exit Sub
                End If
            End If



        

            trans.Commit()
            bindData()
            lblmsg.Text = ""


        Catch ex As Exception
            trans.Rollback()
            lblmsg.Text = ex.Message
            logger.Error(ex.Message)
        Finally
            trans.Dispose()
            cn.Close()

        End Try
    End Sub

    Protected Sub ddLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            bindData()

        Catch ex As Exception
            lblmsg.Text = ex.Message
            logger.Error(ex.Message)
        End Try
    End Sub
End Class
