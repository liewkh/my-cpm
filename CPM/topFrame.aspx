<%@ Page Language="VB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body leftmargin="0" topmargin="0" marginwidth="0" marginheight="0" >
<%
    Dim lp As New LoginProfile()
    lp = Session("LoginProfile")
    If IsNothing(lp) Then
        Response.Redirect("login.aspx?login=expired")
    Else
        lblUserId.Text = lp.getUserLoginId
        lblUserId.ToolTip = lp.getUserName
    End If
%>
    <form id="form1" runat="server">
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td align="left" valign="top" bgcolor="Black">
    <img alt="" src="img/BizLog.jpg" width="180" height="80"/></td>   
    <td valign="top" bgcolor="Black" style="width: 100px">
    <A href="logout.aspx"><b>Sign Out</b></A>
    <br/><br/>
    <asp:Label ID="lblUserId" runat="server"   Font-Bold="true"  ForeColor="red"  Text=""></asp:Label>/>             
    </td>
   </tr>
</table>
    </form>
</body>
</html>
