<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PassCardInfo.aspx.vb" Inherits="Maintenance_PassCardInfo" %>

<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral,PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>Pass Card Information</title>
    <link rel="stylesheet" type="text/css" href="../include/stylesheet.css" />

    <script type="text/javascript" language="JavaScript" src='../include/effect.js'></script>

    <script type="text/javascript" language="JavaScript" src='../include/javascript.js'></script>

</head>

<script language="javascript" type="text/javascript">
<!--

window.onscroll=move;

function DateChanged(_TextBox,_PopCal)
{
	var _TextBoxWeek=document.getElementById("txtPurchasedDate")
	_TextBoxWeek.value=''
	if((!_TextBox)||(!_PopCal)) return
	var _format=_TextBox.getAttribute("Format")
	var _Date=_PopCal.getDate(_TextBox.value, _format)
	if(_Date)
	{
		_TextBoxWeek.value=_PopCal.getWeekNumber(_Date)
	}
}

function SpecialDateStyle(_Date, _PopCal, _Tooltip)
{
	if(_Tooltip.toLowerCase().indexOf("cumpleaños")!=-1)
	{
		var _Style="Background-Position:center center;"
		_Style+="Background-Image:url("+_PopCal.imgDir+"special.gif);"
		_Style+="Background-Repeat:no-repeat;"
		_Style+="Background-Color:#00aa00;"

		return(_Style)
	}
	return("")
}

function DisabledDateStyle(_Date, _PopCal, _Tooltip, _RazonDisabled)
{
	var _Today=_PopCal.getDate("Hoy")
	var _Style="Background-Position:center center;"
	_Style+="Background-Image:url("+_PopCal.imgDir+"baddate.gif);"
	_Style+="Background-Repeat:no-repeat;"
	if(_RazonDisabled.indexOf("Holiday")!=-1)
	{
		if((_PopCal.isGoodFriday(_Date))||(_PopCal.isCarnival(_Date)))
		{
			if((_Date>_Today)&&(_RazonDisabled.indexOf("RangeFrom")!=-1))
			{
				return(_Style+"Color:#ffffff;Background-Color:#ff0000!important;")
			}
			return("DisabledCustom")
		}
	}
	if((_Date>_Today)&&(_RazonDisabled.indexOf("RangeFrom")!=-1))
	{
		if(_Tooltip.toLowerCase().indexOf("cumpleaños")!=-1)
		{
			_Style+="Background-Color:#00aa00!important;"
		}
		return(_Style+"Color:#0a0a0a!important;")
	}
	return("")
}


         function onUpdating(){
                        // get the update progress div
                        var pnlPopup = $get('<%= pnlPopup.ClientID %>');

                        //  get the gridview element
                        var gridView = $get('<%= gvPassCard.ClientID %>');

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
                    
// -->
                             
            
</script>

<body>
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
                                        Pass Card</td>
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
                                        Information&nbsp;</td>
                                    <td class="hSpace">
                                        &nbsp;</td>
                                </tr>
                            </table>
                            <asp:RequiredFieldValidator ID="PassCardSN" runat="server" ControlToValidate="txtPassCardSN" SetFocusOnError="True"
                                Display="None" ErrorMessage="Serial No/Bay No is a required field."></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="ItemType" runat="server" ControlToValidate="ddItemType" SetFocusOnError="True"
                                Display="None" ErrorMessage="Item Type is a required field."></asp:RequiredFieldValidator>                            
                                
        <rjs:PopCalendarMessageContainer ID="PopCalMessagesForCalendar1" runat="server"   
            Calendar="popCalendar1" /> 
        <rjs:PopCalendarMessageContainer ID="PopCalMessagesForCalendar2" runat="server" 
            Calendar="popCalendar2" />
            
         <asp:CustomValidator id="MyValidator" Runat="Server" ControlToValidate="txtWarranty" Display="Dynamic"  SetFocusOnError="True" OnServerValidate="Validate_TextBox"/>
  
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
                        <td class="normalLabel">[Pass Card Serial No] <font color="#FF0000">*</font></td>
                        <td class="hSpace">&nbsp;</td>
                        <td><asp:TextBox ID="txtPassCardSN" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="1"></asp:TextBox></td>
                        <td class="hSpace">&nbsp;</td>
                        <td class="normalLabel">[Item Type] <font color="#FF0000">*</font></td>
                        <td class="hSpace">&nbsp;</td>
                        <td nowrap><asp:DropDownList ID="ddItemType" runat="server" TabIndex="2" DataSourceID="dsItemType"
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
                        <td class="normalLabel">Supplier</td>
                        <td class="hSpace">&nbsp;</td>
                        <td><asp:TextBox ID="txtSupplier" runat="server" CssClass="textBoxLarge" MaxLength="200" TabIndex="3"></asp:TextBox></td>
                        <td class="hSpace">&nbsp;</td>
                        <td class="normalLabel">D.O Date</font></td>
                        <td class="hSpace">&nbsp;</td>
                        <td class="normalLabel"><asp:TextBox ID="txtDoDate" runat="server" CssClass="textBoxSmall" MaxLength="12" TabIndex="4"></asp:TextBox>
                            <rjs:PopCalendar ID="popCalendar1" From-Date="" To-Today = "true" runat="server" Control="txtDoDate" InvalidDateMessage="Invalid Date Range Selection"
                                Shadow="True" ShowWeekend="True" Move="True" Format="dd mm yyyy" Separator="/" Fade="0.5" ToolTip="Click For Calendar: ([Format])" />
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
                        <td class="normalLabel">[Location] <font color="#FF0000">*</font></td>
                        <td class="hSpace">&nbsp;</td>
                        <td nowrap><asp:DropDownList ID="ddLocation" AutoPostBack="true" runat="server" TabIndex="5" DataSourceID="dsLocation" DataTextField="locationName" DataValueField="locationinfoid" CssClass="dropdownLarge" OnSelectedIndexChanged="ddLocation_SelectedIndexChanged">
                            </asp:DropDownList><asp:SqlDataSource ID="dsLocation" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>">
                            </asp:SqlDataSource>
                        </td>
                        <td class="hSpace">&nbsp;</td>
                        <td class="normalLabel">Allocation Date</td>
                        <td class="hSpace">&nbsp;</td>
                        <td class="normalLabel"><asp:TextBox ID="txtAllocationDate" runat="server" CssClass="textBoxSmall" MaxLength="12" TabIndex="7"></asp:TextBox>
                            <rjs:PopCalendar ID="popCalendar2" runat="server" Control="txtAllocationDate" InvalidDateMessage="Invalid Date Range Selection"
                                Shadow="True" ShowWeekend="True" Move="True" Format="dd mm yyyy" Fade="0.5" ToolTip="Click For Calendar: ([Format])" 
                                From-Control="txtDoDate" Separator="/"
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
                    </tr>
                    
                    <tr>
                        <td class="normalLabel">Remark</td>
                        <td class="hSpace">&nbsp;</td>
                        <td><asp:TextBox ID="txtRemark" runat="server" CssClass="textBoxLarge" MaxLength="1000" TabIndex="6"></asp:TextBox></td>
                        <td class="hSpace">&nbsp;</td>
                        <td class="normalLabel">Warranty Period</td>
                        <td class="hSpace">&nbsp;</td>
                        <td><asp:TextBox ID="txtWarranty" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="7"></asp:TextBox>month(s)</td>
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
                        <td class="normalLabel">[Status] <font color="#FF0000">*</font></td>
                        <td class="hSpace">&nbsp;</td>
                        <td nowrap><asp:DropDownList ID="ddStatus" runat="server" TabIndex="8" DataSourceID="dsStatus" DataTextField="codeDesc" DataValueField="codeAbbr" CssClass="dropdownMedium">
                            </asp:DropDownList><asp:SqlDataSource ID="dsStatus" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                SelectCommand="select codeAbbr,codeDesc,0 as seq from codemstr where codecat = 'PCS' and active = 'Y' union all select  codeabbr,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,codedesc">
                            </asp:SqlDataSource>
                        </td>
                        <td class="hSpace">&nbsp;</td>
                        <td class="normalLabel">Print Deposit</td>
                        <td class="hSpace">&nbsp;</td>
                        <td><asp:CheckBox ID="chkDepositPrinted" Checked="false" runat="server" CssClass="checkBox" TabIndex="9"></asp:CheckBox></td>        
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
                        <td class="normalLabel">Active<font color="#FF0000">*</font></td>
                        <td class="hSpace">&nbsp;</td>
                        <td nowrap><asp:RadioButton ID="rbActiveYes" runat="server" GroupName="ActiveInd" TabIndex="13" Text="Yes" Checked="True" />&nbsp;
                                  <asp:RadioButton ID="rbActiveNo" runat="server" GroupName="ActiveInd" TabIndex="14" Text="No" /></td>
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
                        <td colspan="15" height="17">
                        </td>
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
                                TabIndex="15" />
                            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="buttonMedium" CausesValidation="true"
                                TabIndex="16" />
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="buttonMedium" CausesValidation="true"
                                TabIndex="17" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonMedium" CausesValidation="false"
                                TabIndex="18" /></td>
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
                                <%  If gvPassCard.Rows.Count > 0 Then%>
                                <tr>
                                    <td bgcolor="white">
                                        <asp:GridView ID="gvPassCard" runat="server" AllowPaging="True" AllowSorting="True"
                                            AutoGenerateColumns="False" CellPadding="0" Width="100%" BorderWidth="1px" HorizontalAlign="Left"
                                            CellSpacing="1" DataKeyNames="PASSCARDMSTRID,SERIALNO,ALLOCATIONDATE,WARRANTYPERIOD,DELIVERYDATE,SUPPLIER,STATUSDESC,ITEMTYPE,ITEMTYPEDESC,STATUS,REMARK,ACTIVE,LOCATIONINFOID,LUDT,LUB,DEPOSITPRINT"
                                            DataSourceID="dsPassCard">
                                            <Columns>
                                                <asp:BoundField DataField="LOCATION" HeaderText="Location" SortExpression="LOCATION"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="SERIALNO" HeaderText="Serial No" SortExpression="SERIALNO"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="ITEMTYPEDESC" HeaderText="Item Type" SortExpression="ITEMTYPEDESC"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="STATUSDESC" HeaderText="Status" SortExpression="STATUSDESC"
                                                    NullDisplayText="N/A" />    
                                                <asp:BoundField DataField="SUPPLIER" HeaderText="Supplier" SortExpression="SUPPLIER"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="REMARK" HeaderText="Remark" SortExpression="REMARK" NullDisplayText="N/A" />
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
