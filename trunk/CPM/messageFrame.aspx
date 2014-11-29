<%@ Page Language="VB" AutoEventWireup="false" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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
        <!--<meta http-equiv="refresh" content="60"/>-->
</head>
<body leftmargin="0" topmargin="0" marginwidth="0" marginheight="0"  >
<%
    'Dim lp As New LoginProfile()
    'Dim dt As new Data.DataTable 
    'Dim dm As New DBManager()
    'dim noOfMsg as String 
    'lp = Session("LoginProfile")
    'If IsNothing(lp) Then
    '    Response.Redirect("index.aspx?login=expired")
    'else
        
    '    Dim sqlString As String = "SELECT count(*) as NOOFMSG FROM ReminderList" & _
    '                " WHERE UserMstrId = " & lp.getUserMstrId  & " AND NEWREMINDERIND='Y' " & _
    '                " AND  (DATEDIFF(dd,ExpiryDate,GETDATE())+1) >= 1"
                       

    '    dt = dm.execTable(sqlString)
    '    If dt.Rows.Count >= 1 Then
    '      noOfMsg = dt.Rows(0).Item("NOOFMSG").ToString 
    '      if Integer.Parse (noOfMsg) > 0 then
    '      lblMsg.Text = "You have " + noOfMsg + " new reminder"
    '      end if
    '    end if 
            
    '    dt.Clear  
            
    '   sqlString = "SELECT COUNT(*) AS NOOFMSG " & _
    '                        "FROM ReminderList RL " & _
    '                        "WHERE UserMstrId = " & lp.getUserMstrId  & _
    '                        " AND  (DATEDIFF(dd,ExpiryDate,GETDATE())+1) >= 1"
    '                                   dt = dm.execTable(sqlString)
    '    If dt.Rows.Count >= 1 Then
    '      noOfMsg = dt.Rows(0).Item("NOOFMSG").ToString 
    '      if Integer.Parse (noOfMsg) > 0 then
    '      if String.IsNullOrEmpty (lblMsg.Text) then
    '      lblMsg.Text = "You have " + noOfMsg + " expired reminder"
    '      else
    '      lblMsg.Text = lblMsg.Text + " and " + noOfMsg + " expired reminder"
    '      end if
    '    end if 
    '    end if
    '    dt.Clear  
    '    dt = nothing
    'End If
    'lblMsg.Text = "Messages..."
%>
    <form id="form1" runat="server">
<!--<table width="100%" border="0" cellspacing="0"  cellpadding="0">-->
<!--  <tr>-->
    <!--<td align="left" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<marquee behavior="scroll"  direction="left" loop="infinite"><asp:Label runat="server" id="lblMsg" Text=""  CssClass="alertMsg"></asp:Label></marquee>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>-->
  <!--</tr>-->
<!--</table>-->
    </form>
</body>
</html>
