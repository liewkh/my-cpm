<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ChangePassword.aspx.vb" Inherits="Maintenance_ChangePassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="TextBoxEx" Namespace="TextBoxEx" TagPrefix="cc1" %>

<html>
<head>
<title>Change Password</title>
<link rel="stylesheet" type="text/css" href="../include/stylesheet.css" />
<script type="text/javascript" language="JavaScript" src='../include/effect.js'></script>
<script type="text/javascript" language="JavaScript" src='../include/javascript.js'></script>
</head>




<body leftmargin='0' topmargin='0' scroll='auto'>
<form id="Form1" action='' runat = "server" >
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td class="header1">
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td class="hSpace">&nbsp;</td>
					<td class="header1" style="border:none;">Change Password</td>
					<td>&nbsp;</td>
					<td colspan = "8">&nbsp;</td>
        <td class="hSpace">&nbsp;</td>
        <td colspan = "3">
		 </td>
				</tr>
			</table>
		</td>
	</tr>
</table>

<table border="0" cellpadding="0" cellspacing="0" width="100%" >
	<tr>
		<td class="header2" colspan="15">
		  <table border="0" cellpadding="0" cellspacing="0" width="100%">
			<tr>
			  <td class="hSpace">&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			  <td class="header2" style="border:none;">Maintenance&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			</tr>
			<asp:RequiredFieldValidator ID="OldPassword" runat="server" ControlToValidate="txtOldPassword" Display="None" ErrorMessage="Old Password is a required field."></asp:RequiredFieldValidator>
			<asp:RequiredFieldValidator ID="NewPassword" runat="server" ControlToValidate="txtNewPassword" Display="None" ErrorMessage="New Password is a required field."></asp:RequiredFieldValidator>
		  </table>  		                         
		</td>
	</tr>

	<tr class="vSpace">
	            <td colspan="13"><asp:Label ID="lblmsg" runat="server" CssClass="errorMsg"></asp:Label></td>
		        <td></td>
		        <td></td>
		        
	</tr>

	<tr class="vSpace">
	            <td colspan="13">Old Password <font color="#FF0000">*</font>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<cc1:textboxex id="txtOldPassword" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="1" textmode="Password"></cc1:textboxex></td>
		        <td></td>
		        <td></td>
		        
	</tr>
	
	<tr class="vSpace">
	            <td colspan="13">New Password <font color="#FF0000">*</font>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<cc1:textboxex id="txtNewPassword" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="2" textmode="Password"></cc1:textboxex></td>
		        <td></td>
		        <td></td>
		        
	</tr>

	<tr class="vSpace">
	            <td colspan="13">Retype New Password <font color="#FF0000">*</font><cc1:textboxex id="txtRetypePassword" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="2" textmode="Password"></cc1:textboxex></td>
		        <td></td>
		        <td></td>
		        
	</tr>
		
    <tr class="vSpace">
		<td colspan="15">&nbsp;</td>
	</tr>



  <tr>
      <td colspan='14' align='right'>
     	<asp:Button ID="btnUpdate" runat="server" Text="Update Password" CssClass="buttonLarge" CausesValidation="true" TabIndex="13"/>
       </td>
    <td class="hspace"></td>
  </tr>

	<tr class="vSpace">
		<td colspan="15">&nbsp;</td>
	</tr>

	</table>

</form>
</body>
</html>
