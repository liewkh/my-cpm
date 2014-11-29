<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TicketAllocation.aspx.vb" Inherits="Transaction_TicketAllocation" %>

<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral,PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>Ticket Allocation</title>
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
                        var gridView = $get('<%= gvTA.ClientID %>');

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

<body>
    <form id="Form1" action='' runat="server">
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
                                        Ticket</td>
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
                                        Allocation&nbsp;</td>
                                    <td class="hSpace">
                                        &nbsp;</td>
                                </tr>
                            </table>                          
                                
        <rjs:PopCalendarMessageContainer ID="PopCalMessagesForCalendar1" runat="server"   
            Calendar="popCalendar1" /> 
        <rjs:PopCalendarMessageContainer ID="PopCalMessagesForCalendar2" runat="server" 
            Calendar="popCalendar2" />
            

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
                        <td class="normalLabel">[Item] <font color="#FF0000">*</font></td>
                        <td class="hSpace">&nbsp;</td>
                        <td nowrap><asp:DropDownList ID="ddItem" runat="server" TabIndex="1" DataSourceID="dsItem"
                                    DataTextField="CodeDesc" DataValueField="codeabbr" CssClass="dropdownSmall">
                                   </asp:DropDownList><asp:SqlDataSource ID="dsItem" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                    SelectCommand="select codeabbr,codedesc,0 as seq from codemstr where codecat='ITEM' union all select codeabbr,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,codedesc">
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
                        <td class="hSpace">&nbsp;</td>
                        <td class="hSpace">&nbsp;</td>
                        <td class="hSpace">&nbsp;</td>
                        <td class="hSpace">&nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td class="normalLabel">[Supplier]</td>
                        <td class="hSpace">&nbsp;</td>
                        <td><asp:TextBox ID="txtSupplier" runat="server" CssClass="textBoxLarge" MaxLength="200" TabIndex="2"></asp:TextBox></td>
                        <td class="hSpace">&nbsp;</td>
                        <td class="normalLabel">D.O Date</font></td>
                        <td class="hSpace">&nbsp;</td>
                        <td class="normalLabel"><asp:TextBox ID="txtDoDate" runat="server" CssClass="textBoxSmall" MaxLength="12" TabIndex="3"></asp:TextBox>
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
                        <td class="normalLabel">[Location]</td>
                        <td class="hSpace">&nbsp;</td>
                        <td nowrap><asp:DropDownList ID="ddLocation" AutoPostBack="true" runat="server" TabIndex="4" DataSourceID="dsLocation" DataTextField="locationName" DataValueField="locationinfoid" CssClass="dropdownLarge">
                            </asp:DropDownList><asp:SqlDataSource ID="dsLocation" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>">
                            </asp:SqlDataSource>
                        </td>
                        <td class="hSpace">&nbsp;</td>
                        <td class="normalLabel">[Allocation Date]</td>
                        <td class="hSpace">&nbsp;</td>
                        <td class="normalLabel"><asp:TextBox ID="txtAllocationDate" runat="server" CssClass="textBoxSmall" MaxLength="12" TabIndex="5"></asp:TextBox>
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
                        <td class="normalLabel">Unit</td>
                        <td class="hSpace">&nbsp;</td>
                        <td nowrap><asp:DropDownList ID="ddUnit" runat="server" TabIndex="6" DataSourceID="dsUnit"
                                    DataTextField="CodeDesc" DataValueField="codeabbr" CssClass="dropdownSmall">
                                   </asp:DropDownList><asp:SqlDataSource ID="dsUnit" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                    SelectCommand="select codeabbr,codedesc,0 as seq from codemstr where codecat='UNIT' union all select codeabbr,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,codedesc">
                                   </asp:SqlDataSource>
                            &nbsp;
                        </td>
                        <td class="hSpace">&nbsp;</td>
                        <td class="normalLabel">Unit Qty</td>
                        <td class="hSpace">&nbsp;</td>
                        <td><asp:TextBox ID="txtUnitQty" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="7"></asp:TextBox></td>
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
                        <td class="normalLabel">Qty Allocation</td>
                        <td class="hSpace">&nbsp;</td>
                        <td><asp:TextBox ID="txtQtyAllocation" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="8"></asp:TextBox></td>
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
                        <td class="normalLabel">Start No</td>
                        <td class="hSpace">&nbsp;</td>
                        <td><asp:TextBox ID="txtStartNo" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="9"></asp:TextBox></td>
                        <td class="hSpace">&nbsp;</td>
                        <td class="normalLabel">End No</td>
                        <td class="hSpace">&nbsp;</td>
                        <td><asp:TextBox ID="txtEndNo" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="10"></asp:TextBox></td>
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
                        <td><asp:TextBox ID="txtRemark" runat="server" CssClass="textBoxLarge" MaxLength="1000" TabIndex="11"></asp:TextBox></td>
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
                                <%  If gvTA.Rows.Count > 0 Then%>
                                <tr>
                                    <td bgcolor="white">
                                        <asp:GridView ID="gvTA" runat="server" AllowPaging="True" AllowSorting="True"
                                            AutoGenerateColumns="False" CellPadding="0" Width="100%" BorderWidth="1px" HorizontalAlign="Left"
                                            CellSpacing="1" DataKeyNames="TICKETALLOCATIONID,UNIT,UNITQTY,QTYALLOCATION,STARTNO,ENDNO,ALLOCATIONDATE,DODATE,SUPPLIER,ITEM,ITEMDESC,REMARK,LOCATIONINFOID,LOCATION,LUDT,LUB"
                                            DataSourceID="dsTA">
                                            <Columns>
                                                <asp:BoundField DataField="ITEMDESC" HeaderText="Item" SortExpression="ITEMDESC"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="LOCATION" HeaderText="Location" SortExpression="LOCATION"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="ALLOCATIONDATE" HeaderText="Allocation Date" SortExpression="ALLOCATIONDATE"
                                                    NullDisplayText="N/A" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false"  />
                                                <asp:BoundField DataField="QTYALLOCATION" HeaderText="Qty Allocation" SortExpression="QTYALLOCATION"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="UNIT" HeaderText="Unit" SortExpression="UNIT"
                                                    NullDisplayText="N/A" />    
                                            </Columns>
                                            <RowStyle CssClass="grid_row1" />
                                            <SelectedRowStyle CssClass="tb-highlight" />
                                            <HeaderStyle CssClass="grid_header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <AlternatingRowStyle CssClass="grid_row2" />
                                            <PagerStyle Font-Bold="True" Font-Underline="True" />
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="dsTA" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            ProviderName="System.Data.SqlClient"></asp:SqlDataSource>
                                    </td>
                                </tr>
                                <%End If%>
                            </table>
                        </td>
                    </tr>
                    
                    <ajaxToolkit:FilteredTextBoxExtender ID="ftbe1" runat="server" TargetControlID="txtUnitQty" ValidChars="1234567890" />
                    <ajaxToolkit:FilteredTextBoxExtender ID="ftbe2" runat="server" TargetControlID="txtQtyAllocation" ValidChars="1234567890" />
                    <ajaxToolkit:FilteredTextBoxExtender ID="ftbe3" runat="server" TargetControlID="txtStartNo" ValidChars="1234567890" />
                    <ajaxToolkit:FilteredTextBoxExtender ID="ftbe4" runat="server" TargetControlID="txtEndNo" ValidChars="1234567890" />
                    
                    <asp:RegularExpressionValidator ID="v1" runat="server" ControlToValidate="txtUnitQty" Display="None" ErrorMessage="Invalid format. Unit Qty is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="v2" runat="server" ControlToValidate="txtQtyAllocation" Display="None" ErrorMessage="Invalid format. Qty Allocation is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="v3" runat="server" ControlToValidate="txtStartNo" Display="None" ErrorMessage="Invalid format. Start No is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="v4" runat="server" ControlToValidate="txtEndNo" Display="None" ErrorMessage="Invalid format. End No is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
                    
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
