<%@ Page Language="VB" AutoEventWireup="false" CodeFile="LocationSetup.aspx.vb" Inherits="Maintenance_LocationSetup" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral,PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html>
<head>
    <title>Branch Setup</title>
    <link rel="stylesheet" type="text/css" href="../include/stylesheet.css" />

    <script type="text/javascript" language="JavaScript" src='../include/effect.js'></script>

    <script type="text/javascript"  language="JavaScript" src='../include/javascript.js'></script>

</head>

<script language="javascript" type="text/javascript">
         window.onscroll=move;

         function onUpdating(){
                        // get the update progress div
                        var pnlPopup = $get('<%= pnlPopup.ClientID %>');

                        //  get the gridview element
                        var gridView = $get('<%= gvLocation.ClientID %>');

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
                                        Location&nbsp;</td>
                                    <td style="height: 17px">
                                        &nbsp;</td>
                                    <td class="hSpace">
                                        &nbsp;</td>
                                </tr>
                            </table>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errorMsg"
                                DisplayMode="List" />
                            <asp:RequiredFieldValidator ID="LocationCode" runat="server" ControlToValidate="txtLocationCode"
                                Display="None" ErrorMessage="Location Code is a required field."></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="LocationName" runat="server" ControlToValidate="txtLocationName"
                                Display="None" ErrorMessage="Location Name is a required field."></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="Branch" runat="server" ControlToValidate="ddBranch"
                                Display="None" ErrorMessage="Branch is a required field."></asp:RequiredFieldValidator>                                
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
                            [Branch] <font color="#FF0000">*</font></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td nowrap><asp:DropDownList ID="ddBranch" runat="server" TabIndex="1" DataSourceID="dsBranch" DataTextField="branchName" DataValueField="branchinfoid" CssClass="dropdownLarge"></asp:DropDownList><asp:SqlDataSource ID="dsBranch" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            SelectCommand="select branchinfoid,branchName,0 as seq from branchinfo union all select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,branchname">
                                        </asp:SqlDataSource>
		                </td>
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
                            [Location Code] <font color="#FF0000">*</font></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtLocationCode" runat="server" CssClass="textBoxSmall" MaxLength="50"
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
                            [Location Name] <font color="#FF0000">*</font>
                        </td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td nowrap>
                            <asp:TextBox ID="txtLocationName" runat="server" CssClass="textBoxLarge" MaxLength="200"
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
                                DataTextField="CodeDesc" DataValueField="codeabbr" CssClass="dropdownLarge">
                            </asp:DropDownList><asp:SqlDataSource ID="dsState" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                SelectCommand="select codeabbr,codedesc,0 as seq from codemstr where codecat='STA' union all select codeabbr,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,codedesc">
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
                            Manager Name</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtManagerName" runat="server" CssClass="textBoxMedium" MaxLength="200" TabIndex="9"></asp:TextBox></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="normalLabel">
                            Manager H/P No</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtManagerHpNo" runat="server" CssClass="textBoxMedium" MaxLength="50"
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
                            Supervisor Name</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtSupervisorName" runat="server" CssClass="textBoxMedium" MaxLength="200" TabIndex="9"></asp:TextBox></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="normalLabel">
                            Supervisor H/P No</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td><asp:TextBox ID="txtSupervisorHpNo" runat="server" CssClass="textBoxMedium" MaxLength="50"
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
                            Location Capacity</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtLocationCapacity" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="9"></asp:TextBox></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="normalLabel">
                            Location Type</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td><asp:TextBox ID="txtLocationType" runat="server" CssClass="textBoxLarge" MaxLength="200"
                                TabIndex="10" TextMode="MultiLine"></asp:TextBox></td>
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
                            Season Budgetted Amount</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtSeasonAmount" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="11"></asp:TextBox></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="normalLabel">
                            Visitor Budgetted Amount</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td><asp:TextBox ID="txtVisitorAmount" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="12"></asp:TextBox></td>
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
                            Url</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtUrl" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="13"></asp:TextBox></td>
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
                            Company Invoice No</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtComInvoiceNo" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="14"></asp:TextBox></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="normalLabel">
                            Company Invoice Prefix</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtComInvoicePrefix" runat="server" CssClass="textBoxSmall" MaxLength="50"
                                TabIndex="15"></asp:TextBox></td>
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
                            Individual Invoice No</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtIndInvoiceNo" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="16"></asp:TextBox></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="normalLabel">
                            Individual Invoice Prefix</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtIndInvoicePrefix" runat="server" CssClass="textBoxSmall" MaxLength="50"
                                TabIndex="17"></asp:TextBox></td>
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
                            Receipt No</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtReceiptNo" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="18"></asp:TextBox></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="normalLabel">
                            Receipt Prefix</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtReceiptPrefix" runat="server" CssClass="textBoxSmall" MaxLength="50"
                                TabIndex="19"></asp:TextBox></td>
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
                            Debit Note No</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtDebitNoteNo" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="20"></asp:TextBox></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="normalLabel">
                            Debit Note Prefix</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtDebitNotePrefix" runat="server" CssClass="textBoxSmall" MaxLength="50"
                                TabIndex="21"></asp:TextBox></td>
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
                            Credit Note No</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtCreditNoteNo" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="22"></asp:TextBox></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="normalLabel">
                            Credit Note Prefix</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtCreditNotePrefix" runat="server" CssClass="textBoxSmall" MaxLength="50"
                                TabIndex="23"></asp:TextBox></td>
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
                            Daily Collection No</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtDailyCollectionNo" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="24"></asp:TextBox></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="normalLabel">
                            Daily Collection Prefix</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtDailyCollectionPrefix" runat="server" CssClass="textBoxSmall" MaxLength="50"
                                TabIndex="25"></asp:TextBox></td>
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
                            Account Code</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtAccountCode" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="26"></asp:TextBox></td>
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


                    <tr>
                        <td class="normalLabel">
                            Refund Cut-Off Day</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtCutOff" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="27"></asp:TextBox></td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td class="normalLabel">Remark
                            &nbsp;</td>
                        <td class="hSpace">
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtRemark" runat="server" CssClass="textBoxLarge" MaxLength="1000"
                                TabIndex="27" TextMode="MultiLine"></asp:TextBox></td>
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
                            <asp:RadioButton ID="rbActiveYes" runat="server" GroupName="ActiveInd" TabIndex="28"
                                Text="Yes" Checked="True" />&nbsp;
                            <asp:RadioButton ID="rbActiveNo" runat="server" GroupName="ActiveInd" TabIndex="29"
                                Text="No" />
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
                                <%  If gvLocation.Rows.Count > 0 Then%>
                                <tr>
                                    <td bgcolor="white">
                                        <asp:GridView ID="gvLocation" runat="server" AllowPaging="True" AllowSorting="True"
                                            AutoGenerateColumns="False" CellPadding="0" Width="100%" BorderWidth="1px" HorizontalAlign="Left"
                                            CellSpacing="1" DataKeyNames="LOCATIONINFOID,BRANCHINFOID,LOCATIONCODE,LOCATIONNAME,ADDRESS1,ADDRESS2,ADDRESS3,MANAGERNAME,MANAGERHPNO,SUPERVISORNAME,SUPERVISORHPNO,BRANCHNAME,SEASONBUDGETAMOUNT,VISITORBUDGETAMOUNT,
                                                                          LOCATIONCAPACITY,LOCATIONTYPE,COMPANYINVOICENO,COMPANYINVOICEPREFIX,INDIVIDUALINVOICENO,INDIVIDUALINVOICEPREFIX,RECEIPTNO,RECEIPTPREFIX,POSTCODE,STATE,TELEPHONE,FAX,EMAIL,URL,REMARK,
                                                                          ACTIVE,LUDT,LUB,DEBITNOTENO,DEBITNOTEPREFIX,CREDITNOTENO,CREDITNOTEPREFIX,REFUNDCUTOFFDATE,ACCOUNTCODE,DAILYCOLLECTIONNO,DAILYCOLLECTIONPREFIX" DataSourceID="dsLocation">
                                            <Columns>
                                                <asp:BoundField DataField="LOCATIONCODE" HeaderText="Location Code" SortExpression="LOCATIONCODE"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="LOCATIONNAME" HeaderText="Location Name" SortExpression="LOCATIONNAME"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="BRANCHNAME" HeaderText="Branch" SortExpression="BRANCHNAME"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="LOCATIONTYPE" HeaderText="Location Type" SortExpression="LOCATIONTYPE"
                                                    NullDisplayText="N/A" />
                                            </Columns>
                                            <RowStyle CssClass="grid_row1" />
                                            <SelectedRowStyle CssClass="tb-highlight" />
                                            <HeaderStyle CssClass="grid_header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <AlternatingRowStyle CssClass="grid_row2" />
                                            <PagerStyle Font-Bold="True" Font-Underline="True" />
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="dsLocation" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
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