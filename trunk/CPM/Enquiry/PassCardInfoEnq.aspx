<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PassCardInfoEnq.aspx.vb" Inherits="Enquiry_PassCardInfoEnq" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral,PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %> 

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Pass Card Enquiry</title>
    <link rel="stylesheet" type="text/css" href="../include/stylesheet.css" />
    <script type="text/javascript" src='../include/effect.js' />
    <script type="text/javascript" src='../include/javascript.js' />
</head>
<body>

<script language="javascript" type="text/javascript">
         window.onscroll=move;

         function onUpdating(){
                        // get the update progress div
                        var pnlPopup = $get('<%= pnlPopup.ClientID %>');

                        //  get the gridview element
                        var gridView = $get('<%= gvPassCardEnq.ClientID %>');

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
	{    alert('In');
		if (size=='S'){
			window.showModalDialog(page,'File','dialogWidth=500px,dialogHeight=300px');}
		else if (size=='M'){
			window.showModalDialog(page,'File','dialogWidth=780px,dialogHeight=600px');}		
		else if (size=='L'){
			window.showModalDialog(page,'File','dialogWidth=1000px,dialogHeight=800px');}
		return false;
	}                    
</script>

<form action='' runat="server" onsubmit="return true;">
<asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <contenttemplate> 
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td class="header1">
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td class="hSpace">&nbsp;</td>
					<td class="header1" style="border:none;">Pass Card</td>
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
		<td class="header2" colspan="15" style="height: 35px">
		  <table border="0" cellpadding="0" cellspacing="0" width="100%">
			<tr>
			  <td class="hSpace">&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			  <td class="header2" style="border:none;">Enquiry&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			</tr>
			<asp:HiddenField ID="hidPassCardMstrId" runat="server"/>
		  </table>
		</td>
	</tr>

	                <tr class="vSpace">		    
		                <td><asp:Label ID="lblmsg" runat="server" CssClass="errorMsg"></asp:Label></td>
		                <td colspan="14">&nbsp;
                        </td>
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
	    <td class="normalLabel">[Category]</td>
		<td class="hSpace">&nbsp;</td>
    	<td nowrap><asp:RadioButton ID="rbIndividual" runat="server" GroupName="Category" TabIndex="2" Text="Individual" AutoPostBack="true"/>&nbsp;
                   <asp:RadioButton ID="rbCompany" runat="server" GroupName="Category" TabIndex="3" Text="Company" Checked="True" AutoPostBack="true"/>
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
		<td class="normalLabel">[Debtor Name]</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtDebtorName" runat="server" CssClass="textBoxLarge" MaxLength="200" TabIndex="4"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">[Item Type]</td>
		<td class="hSpace">&nbsp;</td>
        <td nowrap><asp:DropDownList ID="ddItemType" runat="server" TabIndex="5" DataSourceID="dsItemType"
                                    DataTextField="CodeDesc" DataValueField="codeabbr" CssClass="dropdownLarge">
                                   </asp:DropDownList><asp:SqlDataSource ID="dsItemType" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                    SelectCommand="select codeabbr,codedesc,0 as seq from codemstr where codecat='ITY' union all select codeabbr,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,codedesc">
                                   </asp:SqlDataSource>
                            &nbsp;
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
		<td class="normalLabel">[Pass Card Serial No]</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtPassCardSN" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="6"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">[Status]</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:DropDownList ID="ddStatus" runat="server" TabIndex="8" DataSourceID="dsStatus" DataTextField="codeDesc" DataValueField="codeAbbr" CssClass="dropdownMedium">
                            </asp:DropDownList><asp:SqlDataSource ID="dsStatus" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                SelectCommand="select codeAbbr,codeDesc,0 as seq from codemstr where codecat = 'PCS' and active = 'Y' union all select  codeabbr,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,codedesc">
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
	</tr>




    <tr>
		<td colspan="15" height="17"></td>
    </tr>


                    <tr>
                        <td class="hSpace" height="22" style="width: 92px">
                            &nbsp;</td>
                        <td class="hSpace" height="22" style="width: 13px">
                            &nbsp;</td>
                        <td height="22" colspan="11" align="right">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonMedium" CausesValidation="false"
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
                                <%  If gvPassCardEnq.Rows.Count > 0 Then%>
                                <tr>
                                    <td bgcolor="white">
                                        <asp:GridView ID="gvPassCardEnq" runat="server" AllowPaging="True" AllowSorting="True"
                                            AutoGenerateColumns="False" CellPadding="0" Width="100%" BorderWidth="1px" HorizontalAlign="Left"
                                            CellSpacing="1" DataKeyNames="PASSCARDMSTRID,SERIALNO,WARRANTYPERIOD,SUPPLIER,ITEMTYPE,STATUS,REMARK,ACTIVE,CATEGORY,
                                                                          LOCATIONINFOID,LUDT,LUB,PASSCARDSTATUS,NAME"
                                            DataSourceID="dsPassCard">
                                            <Columns>
                                                <asp:BoundField DataField="NAME" HeaderText="Name" SortExpression="Name" Visible="false"  
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="LOCATION" HeaderText="Location" SortExpression="LOCATION"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="SERIALNO" HeaderText="Serial No" SortExpression="SERIALNO"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="FIRSTUSED" HeaderText="First Use Date" SortExpression="FIRSTUSED"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="WARRANTYEXPIRY" HeaderText="Warranty Expiry" SortExpression="WARRANTYEXPIRY"
                                                    NullDisplayText="N/A" DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:TemplateField HeaderText="History" SortExpression="LOCATION"> 
                                                    <ItemTemplate> 
                                                     <asp:Button ID="btnHistory" Text="History" CommandName="History" CssClass="buttonMedium" runat="server"></asp:Button> 
                                                    </ItemTemplate> 
                                                </asp:TemplateField>      
                                                <asp:BoundField DataField="PASSCARDSTATUS" HeaderText="Status" SortExpression="PASSCARDSTATUS" NullDisplayText="N/A" />
                                            </Columns>
                                            <RowStyle CssClass="grid_row1" />
                                            <SelectedRowStyle CssClass="tb-highlight" />
                                            <HeaderStyle CssClass="grid_header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <AlternatingRowStyle CssClass="grid_row2" />
                                            <PagerStyle Font-Bold="True" Font-Underline="True" />
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="dsPassCard" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
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
