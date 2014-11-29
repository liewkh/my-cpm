<%@ Page Language="VB" AutoEventWireup="false" CodeFile="BranchSetup.aspx.vb" Inherits="Maintenance_BranchSetup" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral,PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>Branch Setup</title>
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
                        var gridView = $get('<%= gvBranch.ClientID %>');

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
                        <td class="header2" colspan="15" align="left">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td class="hSpace" style="height: 17px">
                                        &nbsp;</td>
                                    <td class="hSpace" style="height: 17px">
                                        &nbsp;</td>
                                    <td class="header2" style="border: none; height: 17px;">
                                        Branch&nbsp;</td>
                                    <td style="height: 17px">
                                        &nbsp;</td>
                                    <td class="hSpace">
                                        &nbsp;</td>
                                </tr>
                            </table>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errorMsg"
                                DisplayMode="List" />
                            <asp:RequiredFieldValidator ID="BranchCode" runat="server" ControlToValidate="txtBranchCode"
                                Display="None" ErrorMessage="Branch Code is a required field."></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="BranchName" runat="server" ControlToValidate="txtBranchName"
                                Display="None" ErrorMessage="Branch Name is a required field."></asp:RequiredFieldValidator>
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
                            [Branch Code] <font color="#FF0000">*</font></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtBranchCode" runat="server" CssClass="textBoxSmall" MaxLength="50"
                                TabIndex="1"></asp:TextBox></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="normalLabel">
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
                        <td class="normalLabel">
                            [Branch Name] <font color="#FF0000">*</font>
                        </td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td nowrap>
                            <asp:TextBox ID="txtBranchName" runat="server" CssClass="textBoxLarge" MaxLength="200"
                                TabIndex="2"></asp:TextBox>
                        </td>
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
                        <td class="hSpace" style="width: 92px">
                            &nbsp;</td>
                        <td class="hSpace" style="width: 13px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="normalLabel">
                            Address 1</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtAddress1" runat="server" CssClass="textBoxLarge" MaxLength="200"
                                TabIndex="3"></asp:TextBox></td>
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
                        <td class="hSpace" style="width: 92px">
                            &nbsp;</td>
                        <td class="hSpace" style="width: 13px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="normalLabel">
                            Address 2</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtAddress2" runat="server" CssClass="textBoxLarge" MaxLength="200"
                                TabIndex="4"></asp:TextBox></td>
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
                        <td class="hSpace" style="width: 92px">
                            &nbsp;</td>
                        <td class="hSpace" style="width: 13px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="normalLabel">
                            Address 3</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtAddress3" runat="server" CssClass="textBoxLarge" MaxLength="200"
                                TabIndex="5"></asp:TextBox></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="normalLabel">
                            PostCode</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtPostCode" runat="server" CssClass="textBoxMedium" MaxLength="50"
                                TabIndex="6"></asp:TextBox></td>
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
                        <td class="hSpace" style="width: 92px">
                            &nbsp;</td>
                        <td class="hSpace" style="width: 13px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="normalLabel">
                            State</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td nowrap>
                            <asp:DropDownList ID="ddState" runat="server" TabIndex="7" DataSourceID="dsState"
                                DataTextField="CodeDesc" DataValueField="codemstrid" CssClass="dropdownLarge">
                            </asp:DropDownList><asp:SqlDataSource ID="dsState" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                SelectCommand="select codemstrid,codedesc,0 as seq from codemstr where codecat='STA' union all select codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,codedesc">
                            </asp:SqlDataSource>
                            &nbsp;
                        </td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="normalLabel">
                            Telephone</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtTelephone" runat="server" CssClass="textBoxMedium" MaxLength="50"
                                TabIndex="8"></asp:TextBox></td>
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
                        <td class="hSpace" style="width: 92px">
                            &nbsp;</td>
                        <td class="hSpace" style="width: 13px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="normalLabel">
                            Fax</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtFax" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="9"></asp:TextBox></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="normalLabel">
                            Email</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="textBoxMedium" MaxLength="50"
                                TabIndex="10"></asp:TextBox></td>
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
                        <td class="hSpace" style="width: 92px">
                            &nbsp;</td>
                        <td class="hSpace" style="width: 13px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="normalLabel">
                            Branch Manager</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtBranchManager" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="11"></asp:TextBox></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="normalLabel">
                            Remark</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtRemark" runat="server" CssClass="textBoxMedium" MaxLength="1000"
                                TabIndex="12"></asp:TextBox></td>
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
                        <td class="hSpace" style="width: 92px">
                            &nbsp;</td>
                        <td class="hSpace" style="width: 13px">
                            &nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td class="normalLabel">
                            Branch Manager Hp No</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtBranchManagerHp" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="13"></asp:TextBox></td>
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
                        <td class="hSpace" style="width: 92px">
                            &nbsp;</td>
                        <td class="hSpace" style="width: 13px">
                            &nbsp;</td>
                    </tr>
                              
                    <tr>
                        <td class="normalLabel">
                            Active<font color="#FF0000">*</font></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td nowrap>
                            <asp:RadioButton ID="rbActiveYes" runat="server" GroupName="ActiveInd" TabIndex="17"
                                Text="Yes" Checked="True" />&nbsp;
                            <asp:RadioButton ID="rbActiveNo" runat="server" GroupName="ActiveInd" TabIndex="18"
                                Text="No" />
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
                        <td class="hSpace" style="width: 92px">
                            &nbsp;</td>
                        <td class="hSpace" style="width: 13px">
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
                                <%  If gvBranch.Rows.Count > 0 Then%>
                                <tr>
                                    <td bgcolor="white">
                                        <asp:GridView ID="gvBranch" runat="server" AllowPaging="True" AllowSorting="True"
                                            AutoGenerateColumns="False" CellPadding="0" Width="100%" BorderWidth="1px" HorizontalAlign="Left"
                                            CellSpacing="1" DataKeyNames="BRANCHINFOID,BRANCHCODE,BRANCHNAME,ADDRESS1,ADDRESS2,ADDRESS3,
                               POSTCODE,STATE,TELEPHONE,FAX,EMAIL,BRANCHMANAGER,BRANCHMANAGERHPNO,REMARK,ACTIVE,LUDT,LUB" DataSourceID="dsBranch">
                                            <Columns>
                                                <asp:BoundField DataField="BRANCHCODE" HeaderText="Branch Code" SortExpression="BRANCHCODE"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="BRANCHNAME" HeaderText="Branch Name" SortExpression="BRANCHNAME"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="ADDRESS1" HeaderText="Address 1" SortExpression="Address 1"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="ADDRESS2" HeaderText="Address 2" SortExpression="Address 2"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="ADDRESS3" HeaderText="Address 3" SortExpression="Address 3"
                                                    NullDisplayText="N/A" />
                                            </Columns>
                                            <RowStyle CssClass="grid_row1" />
                                            <SelectedRowStyle CssClass="tb-highlight" />
                                            <HeaderStyle CssClass="grid_header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <AlternatingRowStyle CssClass="grid_row2" />
                                            <PagerStyle Font-Bold="True" Font-Underline="True" />
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="dsBranch" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            ProviderName="System.Data.SqlClient"></asp:SqlDataSource>
                                    </td>
                                </tr>
                                <%End If%>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="hSpace" height="22" style="width: 92px">
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
