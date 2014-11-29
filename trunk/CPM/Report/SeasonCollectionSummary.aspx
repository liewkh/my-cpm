<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SeasonCollectionSummary.aspx.vb" Inherits="Report_SeasonCollectionSummary" %>

<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
<title>Daily Collection Summary</title>
<link rel="stylesheet" type="text/css" href="../include/stylesheet.css" />

    <script type="text/javascript" language="JavaScript" src='../include/effect.js'></script>

    <script type="text/javascript" language="JavaScript" src='../include/javascript.js'></script>
</head>

<script language="javascript" type="text/javascript">
         window.onscroll=move;

         function onUpdating(){
                        // get the update progress div
                        var pnlPopup = $get('<%= pnlPopup.ClientID %>');

                        //  get the gridview element
                       

                        // make it visible
                        pnlPopup.style.display = '';

                        // get the bounds of both the gridview and the progress div
                        var gridViewBounds = Sys.UI.DomElement.getBounds(gridView);
                        var pnlPopupBounds = Sys.UI.DomElement.getBounds(pnlPopup);

                        //  center of gridview
                        var x = gridViewBounds.x + Math.round(gridViewBounds.width / 2) - Math.round(pnlPopupBounds.width / 2);
                        var y = gridViewBounds.y + Math.round(gridViewBounds.height / 2) - Math.round(pnlPopupBounds.height / 2);

                        //	set the progress element to this position
                        Sys.UI.DomElement.setLocation(pnlPopup, x, y);
                    }

                    function onUpdated() {
                        // get the update progress div
                        var pnlPopup = $get('<%= pnlPopup.ClientID %>');
                        // make it invisible
                        pnlPopup.style.display = 'none';
                        checkToPopUpViewer();
                    }
                    
                     function checkToPopUpViewer(){
                             if(document.forms(0).hdPreview.value == "1"){           
            showCrystalReportDialog("../Report/CrystalReportView.aspx", 900, 1000);
            hdPreview.value = "";
            
        }
    } 


        
</script>

   
<body>
<form id="Form1" action='' runat="server">
<!--10 minutes -->
 <asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" runat="server" AsyncPostBackTimeOut="600">
        </asp:ScriptManager>
        
<script language="javascript" type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequest);
    function InitializeRequest(sender, args) {
      if (prm.get_isInAsyncPostBack())
       {
          args.set_cancel(true);
       }
    }
    function AbortPostBack() {
      if (prm.get_isInAsyncPostBack()) {
           prm.abortPostBack();
      }        
    }
</script>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <contenttemplate> 
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td class="header1">
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td class="hSpace">&nbsp;</td>
					<td class="header1" style="border:none;">Report</td>
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
			  <td class="header2" style="border:none;">Season Collection Summary</td>
			  <td class="hSpace">&nbsp;</td>
			</tr>
		  </table>		   
           <input id="hdPreview" type="hidden" runat="server" />
           <rjs:PopCalendarMessageContainer ID="PopCalMessagesForCalendar1" runat="server" Calendar="popCalendar1" /> 
           <rjs:PopCalendarMessageContainer ID="PopCalMessagesForCalendar2" runat="server" Calendar="popCalendar2" />
		</td>
	</tr>



    <tr>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:Label ID="lblMsg" runat="server" CssClass="errorMsg"></asp:Label></td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
      	<td class="hSpace">&nbsp;</td>
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
                        <td class="normalLabel">[Location]</td>
                        <td class="hSpace">&nbsp;</td>
                        <td nowrap><asp:DropDownList ID="ddLocation" AutoPostBack="true" runat="server" TabIndex="1" DataSourceID="dsLocation" DataTextField="locationName" DataValueField="locationinfoid" CssClass="dropdownLarge">
                            </asp:DropDownList><asp:SqlDataSource ID="dsLocation" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>">
                            </asp:SqlDataSource>
                        </td>
                        <td class="hSpace">&nbsp;</td>
                        <td class="hSpace">&nbsp;</td>
                        <td class="hSpace">&nbsp;</td>
                        <td class="hSpace">&nbsp;</td>
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
                        <td class="normalLabel">Start Month <font color="#FF0000">*</font></td>
                        <td class="hSpace">&nbsp;</td>
                        <td nowrap><asp:DropDownList ID="ddStartMonth" runat="server" AutoPostBack="true">
                                                <asp:ListItem Value="">Please Choose One</asp:ListItem>
                                                <asp:ListItem Value="01">January</asp:ListItem>
                                                <asp:ListItem Value="02">February</asp:ListItem>
                                                <asp:ListItem Value="03">March</asp:ListItem>
                                                <asp:ListItem Value="04">April</asp:ListItem>
                                                <asp:ListItem Value="05">May</asp:ListItem>
                                                <asp:ListItem Value="06">June</asp:ListItem>
                                                <asp:ListItem Value="07">July</asp:ListItem>
                                                <asp:ListItem Value="08">August</asp:ListItem>
                                                <asp:ListItem Value="09">September</asp:ListItem>
                                                <asp:ListItem Value="10">October</asp:ListItem>
                                                <asp:ListItem Value="11">November</asp:ListItem>
                                                <asp:ListItem Value="12">December</asp:ListItem>
                                            </asp:DropDownList>&nbsp;
		                </td>
                        <td class="hSpace">&nbsp;</td>
                        <td class="normalLabel">Start Year <font color="#FF0000">*</font></td>
                        <td class="hSpace">&nbsp;</td>
                        <td nowrap><asp:DropDownList ID="ddStartYear" runat="server" AutoPostBack="true" >
                                            </asp:DropDownList>&nbsp;
		                </td>
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
                        <td class="normalLabel">End Month <font color="#FF0000">*</font></td>
                        <td class="hSpace">&nbsp;</td>
                        <td nowrap><asp:DropDownList ID="ddEndMonth" runat="server" AutoPostBack="true">
                                                <asp:ListItem Value="">Please Choose One</asp:ListItem>
                                                <asp:ListItem Value="01">January</asp:ListItem>
                                                <asp:ListItem Value="02">February</asp:ListItem>
                                                <asp:ListItem Value="03">March</asp:ListItem>
                                                <asp:ListItem Value="04">April</asp:ListItem>
                                                <asp:ListItem Value="05">May</asp:ListItem>
                                                <asp:ListItem Value="06">June</asp:ListItem>
                                                <asp:ListItem Value="07">July</asp:ListItem>
                                                <asp:ListItem Value="08">August</asp:ListItem>
                                                <asp:ListItem Value="09">September</asp:ListItem>
                                                <asp:ListItem Value="10">October</asp:ListItem>
                                                <asp:ListItem Value="11">November</asp:ListItem>
                                                <asp:ListItem Value="12">December</asp:ListItem>
                                            </asp:DropDownList>&nbsp;
		                </td>
                        <td class="hSpace">&nbsp;</td>
                        <td class="normalLabel">End Year <font color="#FF0000">*</font></td>
                        <td class="hSpace">&nbsp;</td>
                        <td nowrap><asp:DropDownList ID="ddEndYear" runat="server" AutoPostBack="true" >
                                            </asp:DropDownList>&nbsp;
		                </td>
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
		<td colspan="15" style="height: 3px">&nbsp;</td>
	</tr>
	



  <tr>
      <td colspan='15' align='right'>
            <asp:Button ID="btnPrint" runat="server" Text="Generate" CssClass="buttonMedium" CausesValidation="true"
                TabIndex="32" OnClick="btnPrint_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonMedium" CausesValidation="false"
                TabIndex="34" OnClick="btnClear_Click"/>
    </td>
    <td class="hspace"></td>
  </tr>

	<tr class="vSpace">
		<td colspan="15">&nbsp;</td>
	</tr>

	</table>

        </contenttemplate>
        </asp:UpdatePanel>
        
               
        <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" runat="server"
            TargetControlID="UpdatePanel1">
            <Animations>
                            <OnUpdating>
                                <Parallel duration="0">
                                    <%-- place the update progress div over the gridview control --%>
                                    <ScriptAction Script="onUpdating();" />
                                 </Parallel>
                            </OnUpdating>
                            <OnUpdated>
                                <Parallel duration="0">
                                    <%--find the update progress div and place it over the gridview control--%>
                                    <ScriptAction Script="onUpdated();" />
                                </Parallel>
                            </OnUpdated>
            </Animations>
        </ajaxToolkit:UpdatePanelAnimationExtender>
        <asp:Panel ID="pnlPopup" runat="server" CssClass="progress" Style="display: none;">
            <div class="container">
                <div class="header">
                    Loading, please wait...</div>
                <div class="body">
                    <img src="../img/activity.gif" />
                </div>
            </div>
        </asp:Panel>
        
</form>
</body>
</html>
