<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SeasonType.aspx.vb" Inherits="Maintenance_SeasonType" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral,      PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>Season Type Setup</title>
    <link rel="stylesheet" type="text/css" href="../include/stylesheet.css">

    <script language="JavaScript" src='../include/effect.js'></script>

    <script language="JavaScript" src='../include/javascript.js'></script>

</head>

<script type="text/javascript">

window.onscroll=move;

            function onUpdating(){
                // get the update progress div
                var pnlPopup = $get('<%= pnlPopup.ClientID %>');

                //  get the gridview element
                var gridView = $get('<%= gvSeasonType.ClientID %>');

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

</script>

<body leftmargin='0' topmargin='0' scroll='auto'>
    <form action='' runat="server">
        <asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td class="header1">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td class="hSpace">
                                        &nbsp;</td>
                                    <td class="header1" style="border: none;">
                                        Setup</td>
                                    <td>
                                        &nbsp;</td>
                                    <td colspan="8">
                                        &nbsp;</td>
                                    <td class="hSpace">
                                        &nbsp;</td>
                                    <td align="right" class="normalLabel" colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td class="header2" colspan="15">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td class="hSpace">
                                        &nbsp;</td>
                                    <td class="hSpace">
                                        &nbsp;</td>
                                    <td class="header2" style="border: none;">
                                        Season Type&nbsp;</td>
                                    <td class="hSpace">
                                        &nbsp;</td>
                                </tr>
                            </table>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errorMsg"
                                DisplayMode="List" />
                            <asp:RequiredFieldValidator ID="SeasonType" runat="server" ControlToValidate="txtSeasonType"
                                Display="None" ErrorMessage="Season Type is a required field."></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="SeasonTypeDesc" runat="server" ControlToValidate="txtSeasonTypeDesc"
                                Display="None" ErrorMessage="Season Type Description is a required field."></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="MonthlyAmount" runat="server" ControlToValidate="txtMonthlyAmount"
                                Display="None" ErrorMessage="Season Amount is a required field."></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="Location" runat="server" ControlToValidate="ddLocation"
                                Display="None" ErrorMessage="Location is a required field."></asp:RequiredFieldValidator>                                
                            <asp:RequiredFieldValidator ID="Deposit" runat="server" ControlToValidate="txtDeposit"
                                Display="None" ErrorMessage="Deposit Amount is a required field."></asp:RequiredFieldValidator> 
                            <asp:RequiredFieldValidator ID="Vehicle" runat="server" ControlToValidate="ddVehicle"
                                Display="None" ErrorMessage="Vehicle Type is a required field."></asp:RequiredFieldValidator>                                                                                            
                        </td>
                    </tr>
                    <tr class="vSpace">
                        <td colspan="13">
                            <asp:Label ID="lblmsg" runat="server" CssClass="errorMsg"></asp:Label></td>
                        <td class="hSpace" height="22">
                            &nbsp;</td>
                        <td class="hSpace" height="22" style="width: 13px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="normalLabel">
                            [Season Type] <font color="#FF0000">*</font></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtSeasonType" runat="server" CssClass="textBoxSmall" MaxLength="50"
                                TabIndex="1"></asp:TextBox></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="normalLabel">[Location] <font color="#FF0000">*</font></td>
                        <td class="hSpace">
                            &nbsp;</td>
        <td nowrap><asp:DropDownList ID="ddLocation" runat="server" TabIndex="2" DataSourceID="dsLocation" DataTextField="locationName" DataValueField="locationinfoid" CssClass="dropdownLarge"></asp:DropDownList><asp:SqlDataSource ID="dsLocation" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>">
                                        </asp:SqlDataSource>
		 </td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="normalLabel">
                            [Season Type Description] <font color="#FF0000">*</font></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td nowrap>
                            <asp:TextBox ID="txtSeasonTypeDesc" runat="server" CssClass="textBoxLarge" MaxLength="200"
                                TabIndex="3"></asp:TextBox>
                        </td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="normalLabel">Vehicle Type <font color="#FF0000">*</font></td>
                        <td class="hSpace">
                        <td nowrap><asp:DropDownList ID="ddVehicle" runat="server" TabIndex="4" DataSourceID="dsVehicle" DataTextField="CODEDESC" DataValueField="CODEABBR" CssClass="dropdownMedium"></asp:DropDownList><asp:SqlDataSource ID="dsVehicle" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            SelectCommand="SELECT CODEABBR,CODEDESC,0 AS SEQ FROM CODEMSTR WHERE CODECAT = 'VTY' AND ACTIVE = 'Y' UNION ALL SELECT CODEABBR,CODEDESC,SEQ FROM CODEMSTR WHERE CODECAT = 'DEFAULT' ORDER BY SEQ,CODEDESC">
                                        </asp:SqlDataSource>
		                </td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="normalLabel">
                            Season Amount (RM) <font color="#FF0000">*</font>
                        </td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td nowrap>
                            <asp:TextBox ID="txtMonthlyAmount" runat="server" CssClass="textBoxSmall" MaxLength="50"
                                TabIndex="5"></asp:TextBox>
                        </td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="normalLabel">
                            Deposit (RM) <font color="#FF0000">*</font>
                        </td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td nowrap>
                            <asp:TextBox ID="txtDeposit" runat="server" CssClass="textBoxSmall" MaxLength="50"
                                TabIndex="6"></asp:TextBox>
                        </td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td class="normalLabel">
                            Duration <font color="#FF0000">*</font>
                        </td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td nowrap><asp:DropDownList ID="ddDuration" runat="server" TabIndex="47" DataSourceID="dsDuration" DataTextField="CODEDESC" DataValueField="CODEABBR" CssClass="dropdownExtraSmall"></asp:DropDownList> month(s)<asp:SqlDataSource ID="dsDuration" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            SelectCommand="SELECT CODEABBR,CODEDESC,0 AS SEQ FROM CODEMSTR WHERE CODECAT = 'SDRT' AND ACTIVE = 'Y' UNION ALL SELECT CODEABBR,CODEDESC,SEQ FROM CODEMSTR WHERE CODECAT = 'DEFAULT' ORDER BY SEQ,CODEDESC">
                                        </asp:SqlDataSource>
		                </td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                    </tr>
                    
    <tr>
        <td class="normalLabel">Other <asp:CheckBox ID="chkOther" Checked="false" runat="server" AutoPostBack="true"  CssClass="checkBox" TabIndex="8" OnCheckedChanged="chkOther_CheckedChanged"></asp:CheckBox></td>
		<td class="hSpace">&nbsp;</td>
        <td nowrap><asp:TextBox ID="txtOther" runat="server" enabled="false" CssClass="textBoxSmallDisabled" MaxLength="5" TabIndex="9"></asp:TextBox> month(s)</td>
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
                        <td class="normalLabel">
                            Active<font color="#FF0000">*</font></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td nowrap>
                            <asp:RadioButton ID="rbActiveYes" runat="server" GroupName="ActiveInd" TabIndex="10"
                                Text="Yes" Checked="True" />&nbsp;
                            <asp:RadioButton ID="rbActiveNo" runat="server" GroupName="ActiveInd" TabIndex="11"
                                Text="No" /></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="normalLabel">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                    </tr>
                    <tr class="vSpace">
                        <td colspan="15">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="hSpace" height="22" style="width: 92px">
                            &nbsp;</td>
                        <td class="hSpace" height="22" style="width: 13px">
                            &nbsp;</td>
                        <td height="22" colspan="11" align="right">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonMedium" CausesValidation="false"
                                TabIndex="30" />
                            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="buttonMedium" CausesValidation="true"
                                TabIndex="31" />
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="buttonMedium" CausesValidation="true"
                                TabIndex="32" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonMedium" CausesValidation="false"
                                TabIndex="33" /></td>
                        <td class="hSpace" height="22">
                            &nbsp;</td>
                        <td class="hSpace" height="22">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="15" height="17">
                        </td>
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
                                <%  If gvSeasonType.Rows.Count > 0 Then%>
                                <tr>
                                    <td bgcolor="white">
                                        <asp:GridView ID="gvSeasonType" runat="server" AllowPaging="True" AllowSorting="True"
                                            AutoGenerateColumns="False" CellPadding="0" Width="100%" BorderWidth="1px" HorizontalAlign="Left"
                                            CellSpacing="1" DataKeyNames="SEASONTYPEMSTRID,SEASONTYPE,SEASONTYPEDESC,AMOUNT,VEHICLETYPE,DEPOSIT,SEASONDURATION,LOCATIONINFOID,ACTIVE,OTHER,LUDT,LUB"
                                            DataSourceID="dsSeasonType">
                                            <Columns>
                                                <asp:BoundField DataField="SEASONTYPE" HeaderText="Season Type" SortExpression="SEASONTYPE"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="SEASONTYPEDESC" HeaderText="Season Type Description" SortExpression="SEASONTYPEDESC"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="VEHICLETYPEDESC" HeaderText="Vehicle Type" SortExpression="VEHICLETYPEDESC"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="AMOUNT" HeaderText="Season Amount" SortExpression="AMOUNT"
                                                    NullDisplayText="N/A" />
                                                 <asp:BoundField DataField="DEPOSIT" HeaderText="Deposit" SortExpression="DEPOSIT"
                                                    NullDisplayText="N/A" />    
                                            </Columns>
                                            <RowStyle CssClass="grid_row1" />
                                            <SelectedRowStyle CssClass="tb-highlight" />
                                            <HeaderStyle CssClass="grid_header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <AlternatingRowStyle CssClass="grid_row2" />
                                            <PagerStyle Font-Bold="True" Font-Underline="True" />
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="dsSeasonType" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            ProviderName="System.Data.SqlClient"></asp:SqlDataSource>
                                    </td>
                                </tr>
                                <%End If%>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="hSpace" height="22">
                            &nbsp;</td>
                        <td class="hSpace" height="22" colspan="13">
                            &nbsp;</td>
                        <td class="hSpace" height="22">
                            &nbsp;</td>
                    </tr>
                </table>
            </ContentTemplate>
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
