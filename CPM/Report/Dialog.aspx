<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Dialog.aspx.vb" Inherits="Report_Dialog" %>
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
Param: <%= Request.Params("key") %>
        <!--<CR:CrystalReportViewer ID="CRViewer" runat="server" AutoDataBind="true" />-->
    </div>
    </form>
</body>
</html>
