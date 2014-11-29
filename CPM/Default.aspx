<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="SuperControls.WebMenu" Namespace="SuperControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<HTML>
	<HEAD runat = "server" >
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
		
	</HEAD>
	
 <%
    Dim lp As New LoginProfile()
    lp = Session("LoginProfile")
    If IsNothing(lp) Then
 %>
    <script language="javascript" >
    location.href =  "login.aspx?login=expired";       
    </script>
 <%
 end if
 %>
 	
<frameset rows="100,0,700,0" frameborder="NO" border="0" framespacing="0" BGCOLOR="#FFFFFF">
  <frame src="topFrame.aspx" name="topFrame" scrolling="NO" noresize />
  <frame src="messageFrame.aspx" name="msgFrame" scrolling="NO" noresize/>
  <frameset rows="*" cols="180,*">   
    <frame src="leftFrame.aspx" name="leftFrame" scrolling="auto" frameborder ="0">
    <frame src="main.aspx" name="main" scrolling="yes">    
  </frameset>
  <frame src="bottomFrame.aspx" name="bottomFrame" scrolling="NO" noresize/>
</frameset>



	

</HTML>
