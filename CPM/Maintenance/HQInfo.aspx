<%@ Page Language="VB" AutoEventWireup="false" CodeFile="HQInfo.aspx.vb" Inherits="Maintenance_HQInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
<title>Company Info</title>
<link rel="stylesheet" type="text/css" href="../include/stylesheet.css" />
<script type="text/javascript" language="JavaScript" src='../include/effect.js'></script>
<script type="text/javascript" language="JavaScript" src='../include/javascript.js'></script>
</head>




<body leftmargin='0' topmargin='0' scroll='auto'>
<form action='' runat = "server" >
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td class="header1">
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td class="hSpace">&nbsp;</td>
					<td class="header1" style="border:none;">Company Information</td>
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
			  <td class="header2" style="border:none;">Setup&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			</tr>
		  </table>
		  
		       <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errorMsg"
                                            DisplayMode="List" />
                    <asp:RequiredFieldValidator ID="CompanyName" runat="server" ControlToValidate="txtCompanyName" Display="None" ErrorMessage="Company Name is a required field."></asp:RequiredFieldValidator>                    
                    
		</td>
	</tr>

	<tr class="vSpace">
		        <td></td>
		        <td></td>
		        <td colspan="13"><asp:Label ID="lblmsg" runat="server" CssClass="errorMsg"></asp:Label></td>
	</tr>

	<tr>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Company Name <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtCompanyName" runat="server" CssClass="textBoxLarge" MaxLength="200" TabIndex="1"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Company No</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtCompanyNo" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="2"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
	</tr>

	<tr>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Address 1</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtAddress1" runat="server" CssClass="textBoxLarge" MaxLength="200" TabIndex="3"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
	</tr>

	<tr>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Address 2</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtAddress2" runat="server" CssClass="textBoxLarge" MaxLength="200" TabIndex="4"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
	</tr>

    <tr>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Address 3</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtAddress3" runat="server" CssClass="textBoxLarge" MaxLength="200" TabIndex="5"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">PostCode</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtPostCode" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="6"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
	</tr>
 
    <tr>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">State</td>
		<td class="hSpace">&nbsp;</td>
        <td nowrap>                   <asp:DropDownList ID="ddState" runat="server" TabIndex="7" DataSourceID="dsState" DataTextField="CodeDesc" DataValueField="codeabbr" CssClass="dropdownLarge"></asp:DropDownList><asp:SqlDataSource ID="dsState" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            SelectCommand="select codeabbr,codedesc,0 as seq from codemstr where codecat='STA' union all select codeabbr,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,codedesc">
                                        </asp:SqlDataSource>&nbsp;
        </td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Telephone</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtTelephone" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="8"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
	</tr>

    <tr>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Fax</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtFax" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="9"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Email</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtEmail" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="10"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
	</tr>

    <tr>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Url</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtUrl" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="11"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Remark</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtRemark" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="12"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
	</tr>

    <tr>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Tax Amount (%)</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtTaxAmount" runat="server" CssClass="textBoxSmall" MaxLength="10" TabIndex="13"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
	</tr>

    <tr>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Tax Description</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtTaxDesc" runat="server" CssClass="textBoxMedium" MaxLength="250" TabIndex="14"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
	</tr>

	<tr class="vSpace">
		<td colspan="15">&nbsp;</td>
	</tr>



	<tr class="vSpace">
		<td colspan="15">&nbsp;</td>
	</tr>



  <tr>
      <td colspan='14' align='right'>
     	<asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="buttonMedium" CausesValidation="true" TabIndex="13"/>
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
