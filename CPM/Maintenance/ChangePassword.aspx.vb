Imports System.Data
Imports System.Data.SqlTypes
Imports System.Data.SqlClient


Partial Class Maintenance_ChangePassword
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


    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim umEnt As New CPM.UserMstrEntity
        Dim umDao As New CPM.UserMstrDAO
        Dim flag As Boolean = False
	

        lblmsg.Text = ""

        If Not Page.IsValid Then
            Exit Sub
        End If

        If Trim(txtNewPassword.Text) <> "" And Trim(txtRetypePassword.Text) <> "" Then
            If Trim(txtNewPassword.Text) <> Trim(txtRetypePassword.Text) Then
                lblmsg.Text = "New Password Not same with Retype Password."
                Exit Sub
	    else
		If Len(txtNewPassword.Text) < 10 Then
			lblmsg.Text = "New Password Must Be At Least 10 characters !"
                	Exit Sub
		End If
            End If
        End If

	 	 

	        Dim dt As New DataTable
        Dim sql As String = ""



        cn = New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("path"))
        If Not cn.State = ConnectionState.Open Then
            cn.Open()
        End If
        trans = cn.BeginTransaction


        Try
            sql = "SELECT PASSWORD FROM USERMSTR WHERE USERMSTRID = " & lp.getUserMstrId
            dt = dm.execTable(sql)

            If dt.Rows.Count > 0 Then
                If dt.Rows(0).Item(umDao.COLUMN_Password).ToString.ToUpper.Equals(Trim(txtOldPassword.Text.ToUpper)) Then
                    umEnt.setUserMstrId(lp.getUserMstrId)
                    umEnt.setPassword(Trim(txtNewPassword.Text))
                    umEnt.setLastUpdatedBy(lp.getUserMstrId)
                    umEnt.setLastUpdatedDatetime(Now)
                    umDao.updateDB(umEnt, cn, trans)
                    flag = True
                Else
                    lblmsg.Text = "Invalid Old Password."
                    Exit Sub
                End If
            End If

            trans.Commit()
            lblmsg.Text = ConstantGlobal.Record_Updated


        Catch ex As Exception
            lblmsg.Text = ex.Message
            trans.Rollback()
        Finally
            trans.Dispose()
            cn.Close()
            If flag Then
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("appPath") + "/login.aspx?login=expired")
            End If
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
    End Sub

End Class


