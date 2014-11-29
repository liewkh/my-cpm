<%@ Page Language="VB" AutoEventWireup="false" CodeFile="login.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>Prominent Excel Sdn Bhd</title><link href="css/menu.css" rel="stylesheet" type="text/css" />
<link rel="stylesheet" type="text/css" href="include/stylesheet.css" />
<script type="text/javascript" language="JavaScript" src='include/effect.js'></script>
<script type="text/javascript" language="JavaScript" src='include/javascript.js'></script>
<base target="_top" />
</head>


 
 
<body onload="document.forms(0).txtUserName.focus();top.location='#login.aspx'">
    <form id="form1" runat="server">
<table width="100%" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">


  <tr>
    <td align="center" background="" style="height: 140px">
        &nbsp;

                            <table border="0" cellpadding="0">
                                <tr>
                                    <td align="left" colspan="2" style="height: 26px">
                                        <table border="0" cellpadding="0">
                                            <tr>
                                                <td align="center" colspan="2" style="color: red">
                                                    &nbsp;<asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errorMsg"
                                            ValidationGroup="Login1" DisplayMode="List" />
                                    </td>
                                            </tr>
                                        </table>
                                        <asp:Label ID="lblMsg" runat="server" CssClass="errorMsg"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="UserNameLabel" runat="server" cssClass= "normalLabel" AssociatedControlID="txtUserName">User Name:</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtUserName" runat="server" TabIndex="2" CssClass="textBoxMedium"></asp:TextBox><asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="txtUserName"
                                            ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator></td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="PasswordLabel" runat="server" cssClass= "normalLabel" AssociatedControlID="txtPassword">Password:</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" TabIndex="3" CssClass="textBoxMedium"></asp:TextBox><asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="txtPassword"
                                            ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator></td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 16px">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        &nbsp;<asp:Button ID="btnLogin" runat="server" CommandName="Login" CssClass="buttonMedium" OnClick="btnLogin_Click"
                                            Text="Log In" ValidationGroup="Login1" TabIndex="3" />
                                    </td>
                                </tr>
                            </table>
        &nbsp;
    
    </td>
  </tr>
  <tr>
    <td height="30" align="center" valign="middle" bgcolor="BLACK" class="copyright"><asp:label ID="lblCopyRight" runat="server" text=""></asp:label></td>
  </tr>
</table>
    </form>
</body>
</html>

