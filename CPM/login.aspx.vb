
Partial Class _Default
        Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim param As String = Request.Params("login")

        lblCopyRight.Text = ConstantGlobal.Copyright

        If Not String.IsNullOrEmpty(param) Then
            If param.Equals("failed") Then
                lblMsg.Text = "Login failed. Invalid User Name or Password."
            ElseIf param.Equals("logout") Then
                lblMsg.Text = "You had successfully logged out."
            ElseIf param.Equals("locked") Then
                lblMsg.Text = "Your account have been locked. Please contact your administrator to reset your account."
            ElseIf param.Equals("pass_expired") Then
                lblMsg.Text = "Password is expired. Please contact administrator."
            ElseIf param.Equals("expired") Then
                lblMsg.Text = "Invalid Session. Please re-login."
            End If
        End If

#If debug Then
        Dim lp As New LoginProfile()
        lp.authenticate("IT", "IT")
        Session("LoginProfile") = lp
        Response.Redirect("default.aspx")
#End If
    End Sub

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Dim lp As New LoginProfile()
        If lp.authenticate(txtUserName.Text, txtPassword.Text) Then
            Session("LoginProfile") = lp
            Response.Redirect("default.aspx")
        Else
            Response.Redirect("login.aspx?login=failed")
        End If
    End Sub


End Class
