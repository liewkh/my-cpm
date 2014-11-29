<%@ Page Language="VB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        lblCopyRight.Text = ConstantGlobal.Copyright
    End Sub
    
</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
        <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" Content="Visual Basic .NET 7.1" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
		<!--Need to include this file for menu style-->
		<link href="css/menu.css" type="text/css" rel="stylesheet" />
		<!-- files with geometry and styles structures -->
		<script type="text/javascript" language="JavaScript" src="menu.js"></script>
		<!-- files with geometry and styles structures -->
		<script type="text/javascript" language="JavaScript" src="menu_tpl.js"></script>
</head>

<body>
<%
    'Dim lp As New LoginProfile()
    'lp = Session("LoginProfile")
    'If IsNothing(lp) Then
    '    Response.Redirect("index.aspx?login=expired")
    'End If
%>    
    <form id="form1" runat="server">
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td height="20" align="center" valign="top" bgcolor="RED"><span class="copyright">&copy; <asp:Label ID="lblCopyRight" runat="server" Text=""></asp:Label></span></td>
  </tr>
</table>
    </form>
</body>
</html>
