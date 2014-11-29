<%@ Page Language="VB" AutoEventWireup="false" CodeFile="dailyCollectionEnq.aspx.vb" Inherits="Enquiry_dailyCollectionEnq" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar"%>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral,      PublicKeyToken=31bf3856ad364e35"      Namespace="System.Web.UI" TagPrefix="asp" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

    <html>
    <head>
    <title>Daily Collection Enquiry</title>
    <link rel="stylesheet" type="text/css" href="../include/stylesheet.css" />
    <script type="text/javascript" language="JavaScript" src='../include/effect.js'></script>
    <script type="text/javascript" language="JavaScript" src='../include/javascript.js'></script>
    <script type="text/javascript" language="JavaScript" src='../include/BoxOver.js'></script>

    </head>

<script language="javascript" type="text/javascript">

    window.onscroll=move;

     function onUpdating(){
                    // get the update progress div
                    var pnlPopup = $get('<%= pnlPopup.ClientID %>');

                    //  get the gridview element
                    var gridView = $get('<%= gvDailyCollection.ClientID %>');

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
                    
                }
                       
    function open_popupModal(page,size) 
	{
		if (size=='S'){
			window.showModalDialog(page,'File','dialogWidth=500px,dialogHeight=300px');}
			//__doPostBack('lnkRefreshPassBay','');}
		else if (size=='M'){
		    //alert(page); 
			window.showModalDialog(page,'File','dialogWidth=780px,dialogHeight=600px');__doPostBack('lnkProcess','');}
			
			//__doPostBack('lnkRefreshPassBay','');}
		else if (size=='L'){
			window.showModalDialog(page,'File','dialogWidth=1000px,dialogHeight=800px');}
			//__doPostBack('lnkRefreshPassBay','');}
		return false;
	}
          
		    </script>
		        
<body>
<form action='' runat = "server">
    <asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" runat="server"></asp:ScriptManager> 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">                         
    <ContentTemplate> 
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td class="header1">
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td class="hSpace">&nbsp;</td>
					<td class="header1" style="border:none;">Daily Collection</td>
					<td>&nbsp;</td>
					<td colspan = "8">&nbsp;</td>
        <td class="hSpace">&nbsp;</td>
        <td align = "right" class="normalLabel" colspan = "3">
			&nbsp;
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
			  <td class="header2" style="border:none;">Enquiry&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			</tr>
		  </table>		
		</td>
	</tr>

               <tr class="vSpace">
		                    <td colspan="13"><asp:Label ID="lblmsg" runat="server" CssClass="errorMsg"></asp:Label></td>		        		        
		        	                        <td class="hSpace" height="22">&nbsp;</td>
		                    <td class="hSpace" height="22" style="width: 13px">&nbsp;</td>
	            </tr>	   

	<tr class="vSpace">
        <td class="normalLabel">[Location]</td>
        <td class="hSpace">&nbsp;</td>
        <td nowrap><asp:DropDownList ID="ddLocation" runat="server" TabIndex="1" DataSourceID="dsLocation" DataTextField="locationName" DataValueField="locationinfoid" CssClass="dropdownLarge"></asp:DropDownList><asp:SqlDataSource ID="dsLocation" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            SelectCommand="select locationinfoid,locationName,0 as seq from locationinfo union all select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,locationname">
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
		<td class="normalLabel">[Collection Date] <font color="#FF0000">*</font></td>
        <td class="hSpace">&nbsp;</td>
        <td class="normalLabel">
		<asp:TextBox ID="txtDate" runat="server" CssClass="dateBox" MaxLength="12" TabIndex="2"></asp:TextBox>          
        <rjs:PopCalendar id="popCalendar1" runat="server"
           Control="txtDate"
           Shadow="True" ShowWeekend="True" Move="True"
           Format="dd mm yyyy" Fade="0.5"
           ToolTip="Click For Calendar: ([Format])"
           To-Today="True"   
           RequiredDate = "true"
           Separator="/"                              
          />
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


   	    <tr class="vSpace">
		    <td colspan="15">&nbsp;</td>
	    </tr>
	    


                    <tr>
                        <td class="hSpace" height="22" style="width: 92px">
                            &nbsp;</td>
                        <td class="hSpace" height="22" style="width: 13px">
                            &nbsp;</td>
                        <td height="22" colspan="11" align="right">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonMedium" CausesValidation="true"
                                TabIndex="15" OnClick="btnSearch_Click" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonMedium" CausesValidation="false"
                                TabIndex="16" OnClick="btnClear_Click" />
                        </td>
                        <td class="hSpace" height="22">
                            &nbsp;</td>
                        <td class="hSpace" height="22">
                            &nbsp;</td>
                    </tr>
    
    
    <tr>
		<td colspan="15" height="17"></td>
    </tr>


                    <tr>
                        <td colspan="13" align="left" style="width: 92px">
                            <asp:Label ID="lblRecCount" runat="server" CssClass="errorMsg"></asp:Label></td>
                        <td class="hSpace" height="22">
                            &nbsp;</td>
                        <td class="hSpace" height="22" style="width: 13px">
                            &nbsp;</td>
                    </tr>  
                    
                                        <tr>
                        <td colspan="15">
                            <!-- Spreadsheet Header -->
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <%  If gvDailyCollection.Rows.Count > 0 Then%>
                                <tr>
                                    <td bgcolor="white">
                                        <asp:GridView ID="gvDailyCollection" runat="server" AllowPaging="True" AllowSorting="True"
                                            AutoGenerateColumns="False" CellPadding="0" Width="100%" BorderWidth="1px" HorizontalAlign="Left"
                                            CellSpacing="1" DataKeyNames="LOCATIONINFOID,LOCATIONNAME,DAILYTOTAL,DAILYSEASONTOTAL,DAILYTOTALOTHERS,TOTALCOLLECTION,MONTHTODATE,LASTMONTH"
                                            DataSourceID="dsDailyCollection">
                                            <Columns>
                                                <asp:BoundField DataField="LOCATIONNAME" HeaderText="Location" SortExpression="LOCATIONNAME"
                                                    NullDisplayText="N/A" /> 
                                                <asp:BoundField DataField="DAILYTOTAL" HeaderText="Daily Total" SortExpression="DAILYTOTAL"
                                                    NullDisplayText="N/A" htmlencode="false" />
                                                <asp:BoundField DataField="DAILYSEASONTOTAL" HeaderText="Season Total" SortExpression="DAILYSEASONTOTAL"
                                                    NullDisplayText="N/A" htmlencode="false" />
                                                <asp:BoundField DataField="DAILYTOTALDEPOSIT" HeaderText="Deposit Total" SortExpression="DAILYTOTALDEPOSIT"
                                                    NullDisplayText="N/A" htmlencode="false" />                                                    
                                                <asp:BoundField DataField="DAILYTOTALOTHERS" HeaderText="Others Total" SortExpression="DAILYTOTALOTHERS"
                                                    NullDisplayText="N/A" htmlencode="false" />
                                                <asp:BoundField DataField="TOTALCOLLECTION" HeaderText="Total Collection" SortExpression="TOTALCOLLECTION"
                                                    NullDisplayText="N/A" htmlencode="false" />     
                                                <asp:BoundField DataField="MONTHTODATE" HeaderText="Month To Date" SortExpression="MONTHTODATE"
                                                    NullDisplayText="N/A" htmlencode="false" />
                                                <asp:BoundField DataField="LASTMONTH" HeaderText="Last Month" SortExpression="LASTMONTH"
                                                    NullDisplayText="N/A" htmlencode="false" />                                                    
                                                <asp:TemplateField HeaderText="Info" SortExpression="INFO"> 
                                                    <ItemTemplate> 
                                                     <asp:Button ID="btnKoi" Text="Details" CommandName="Koi" CssClass="buttonMedium" runat="server"></asp:Button> 
                                                    </ItemTemplate> 
                                                </asp:TemplateField>                                                      
                                            </Columns>
                                            <RowStyle CssClass="grid_row1" />
                                            <SelectedRowStyle CssClass="tb-highlight" />
                                            <HeaderStyle CssClass="grid_header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <AlternatingRowStyle CssClass="grid_row2" />
                                            <PagerStyle Font-Bold="True" Font-Underline="True" />
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="dsDailyCollection" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            ProviderName="System.Data.SqlClient"></asp:SqlDataSource>
                                    </td>
                                </tr>
                                <%End If%>
                            </table>
                        </td>
                    </tr>   

    <tr class="vSpace">
		<td colspan="15">&nbsp;</td>
	</tr>


	</table>

    </ContentTemplate>                    
    </asp:UpdatePanel> 

    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" runat="server" TargetControlID="UpdatePanel1">
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
    <asp:Panel ID="pnlPopup" runat="server" CssClass="progress" style="display:none;">
        <div class="container">
            <div class="header">Loading, please wait...</div>
            <div class="body">
                <img src="../img/activity.gif" />
            </div>
        </div>
    </asp:Panel> 
</form>
</body>
</html>
