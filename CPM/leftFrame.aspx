<%@ Page Language="VB" AutoEventWireup="false" CodeFile="leftFrame.aspx.vb" Inherits="topFrame" %>

<%@ Register Assembly="SuperControls.WebMenu" Namespace="SuperControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
		<title>Car Park Management System</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<!--Need to include this file for menu style-->
		<LINK href="css/menu.css" type="text/css" rel="stylesheet">
		<!-- files with geometry and styles structures -->
		<script language="JavaScript" src="menu.js"></script>
		<!-- files with geometry and styles structures -->
		<script language="JavaScript" src="menu_tpl.js"></script>
		<base target="main"></base>  

</head>



<body bgcolor="black">
    <form id="form1" runat="server" target="main" >
    <div align ="center">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="black">
      <tr>     
          <cc1:WebMenu id="WebMenu1" style="MARGIN-TOP: 0px; POSITION: absolute" PadWidth="24" runat="server" ImageArrow="&lt;img src=img\\OneStep.gif border=0"></cc1:WebMenu>      
      </tr>
    </table>
      </div>
    </form>
</body>
</html>
