
Partial Class logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("LoginProfile") = Nothing
        Response.Redirect("login.aspx?login=logout")
    End Sub

End Class
