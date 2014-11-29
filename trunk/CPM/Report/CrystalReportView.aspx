<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CrystalReportView.aspx.vb" Inherits="Report_CrystalReportView" %>
<!--
<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>-->

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <base target="_self">  
    
</head>

<body >

    <form id="form1" runat="server">
    <div> 
    <%if Request.Params("key") is nothing then %>
    <iframe src="Dialog.aspx" width="100%" height="100%"></iframe>
    <%else %>
        <iframe src="Dialog.aspx?key=1" width="100%" height="100%"></iframe>
    <%End If %>
        <!--<CR:CrystalReportViewer ID="CRViewer" runat="server" AutoDataBind="true" />-->
    </div>
    </form>
</body>
</html>
