<%@ Page Language="VB" AutoEventWireup="false" CodeFile="BillPayment.aspx.vb" Inherits="Transaction_BillPayment" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar"%>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral,PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Bill Payment</title>
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
                    }
                    
                        function open_popupModal(page,size) 
	{
		if (size=='S'){
			window.showModalDialog(page,'File','dialogWidth=500px,dialogHeight=300px');}
			//__doPostBack('lnkRefreshPassBay','');}
		else if (size=='M'){
		    if (document.getElementById("ddLocation").value == '0') 
		        {alert('Please Provide Location Info.');
		         return false;}
		    
		    window.showModalDialog(page,'File','dialogWidth=780px,dialogHeight=600px');}
			//__doPostBack('lnkRefreshPassBay','');}
		else if (size=='L'){
			window.showModalDialog(page,'File','dialogWidth=1000px,dialogHeight=800px');}
			//__doPostBack('lnkRefreshPassBay','');}
		return false;
	}
	
	     function popTheWin(URL) {
	        showDialogScrollable(URL, 800, 640);
           //__doPostBack('lnkRefreshPassBay','');           

        }
        
        
function upParent() {
//var url = document.getElementById('txtDebtor').value;
var e = document.getElementById("ddDebtor"); // select element
var f = document.getElementById("ddLocation"); // select element

//var strUser = e.options[e.selectedIndex].value;
var strDebtorName = e.options[e.selectedIndex].text;
var strDebtorId = e.options[e.selectedIndex].value;
var strLocationName = f.options[f.selectedIndex].text;
var strLocationId = f.options[f.selectedIndex].value;

var url = '../Search/searchDebtor.aspx?debtorName=' + strDebtorName + '&debtorId=' + strDebtorId + '&locationName=' + strLocationName + '&locationId=' + strLocationId;
//alert(url);
open_popupModal(url,'M');
}

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
					<td class="hSpace">&nbsp;</td>
					<td class="header1" style="border:none;">Bill</td>
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
			  <td class="header2" style="border:none;">Payment&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			</tr>
		  </table>
		  
		  <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errorMsg"
                                DisplayMode="List" />
                            <asp:RequiredFieldValidator ID="Location" runat="server" ControlToValidate="ddLocation"
                                Display="None" ErrorMessage="Location is a required field."></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="DebtorName" runat="server" ControlToValidate="ddDebtor"
                                Display="None" ErrorMessage="Debtor Name is a required field."></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="PaymentDate" runat="server" ControlToValidate="txtPaymentDate"
                                Display="None" ErrorMessage="Payment Date is a required field."></asp:RequiredFieldValidator>                                
                            <asp:RequiredFieldValidator ID="PaymentType" runat="server" ControlToValidate="ddPaymentType"
                                Display="None" ErrorMessage="Payment Type is a required field."></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="PaymentAmount" runat="server" ControlToValidate="txtPaymentAmount"
                                Display="None" ErrorMessage="Payment Amount is a required field."></asp:RequiredFieldValidator>  
                  <asp:HiddenField ID="hidDebtorId" runat="server"/>              
		</td>
	</tr>

                    <tr class="vSpace">
                        <td colspan="15"><asp:Label ID="lblmsg" runat="server" CssClass="errorMsg"></asp:Label></td>
                    </tr>

	<tr class="vSpace">
		<td class="normalLabel">Location <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
        <td nowrap><asp:DropDownList ID="ddLocation" runat="server" TabIndex="1" DataSourceID="dsLocation" DataTextField="locationName" DataValueField="locationinfoid" CssClass="dropdownLarge" AutoPostBack="true"  OnSelectedIndexChanged="ddLocation_SelectedIndexChanged"></asp:DropDownList><asp:SqlDataSource ID="dsLocation" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>">
                                        </asp:SqlDataSource>
		</td>
        <td class="hSpace">&nbsp;</td>
        <td class="normalLabel">Invoice No</td>
        <td class="hSpace">&nbsp;</td>
      	<td><asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="textBoxSmallDisabled" TabIndex="6" MaxLength="50"></asp:TextBox></td>
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
		<td class="normalLabel">Category <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:RadioButton ID="rbIndividual" runat="server" GroupName="Category" TabIndex="2" Text="Individual" AutoPostBack="true" OnCheckedChanged="rbIndividual_CheckedChanged"/>&nbsp;
                   <asp:RadioButton ID="rbCompany" runat="server" GroupName="Category" TabIndex="3" Text="Company" Checked="True" AutoPostBack="true" OnCheckedChanged="rbCompany_CheckedChanged"/>
        </td>
        <td class="hSpace">&nbsp;</td>
        <td class="normalLabel">Payment Date <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
        <td class="normalLabel">
		<asp:TextBox ID="txtPaymentDate1" runat="server" CssClass="dateBox" MaxLength="12" TabIndex="4"></asp:TextBox>          
        <rjs:PopCalendar id="popCalendar11" runat="server"
           Control="txtPaymentDate"
           Shadow="True" ShowWeekend="True" Move="True"
           Format="ddmmyyyy" Fade="0.5"
           ToolTip="Click For Calendar: ([Format])"                                
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
		<td class="normalLabel">Debtor Name <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:DropDownList ID="ddDebtor" runat="server" TabIndex="5" DataSourceID="dsDebtor" DataTextField="Debtor" DataValueField="DebtorId" CssClass="dropdownLarge"></asp:DropDownList><asp:SqlDataSource ID="dsDebtor" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"></asp:SqlDataSource>   
		    <img id="imgDebtor"  runat = "server" alt="" border="0" src="../img/magnifier.gif" width="10" height="10" onclick="javascript:upParent();" />
		</td>
		
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Receipt No</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtReceiptNo" runat="server" CssClass="textBoxSmallDisabled" TabIndex="6" MaxLength="50"></asp:TextBox></td>
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
		<td class="normalLabel">Description</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtDescription" runat="server" CssClass="textBoxLarge" TabIndex="7" MaxLength="50"></asp:TextBox></td>
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
	    <td class="normalLabel">Payment Type <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:DropDownList ID="ddPaymentType"  AutoPostBack ="true" runat="server" TabIndex="8" DataSourceID="dsPaymentType" DataTextField="codedesc" DataValueField="codeabbr" CssClass="dropdownMedium" OnSelectedIndexChanged="ddPaymentType_SelectedIndexChanged"></asp:DropDownList><asp:SqlDataSource ID="dsPaymentType" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            SelectCommand="select codeabbr,codedesc,0 as seq from codemstr where codecat = 'PTY' and active = 'Y' union all select codeabbr,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,codedesc">
                                        </asp:SqlDataSource>
		</td>
		<td class="hSpace">&nbsp;</td>
        
        <div id="info" runat ="server" visible = "false" >
                   <td><asp:Label ID="lblDisplay" runat="server"></asp:Label></td>
           <td class="hSpace">&nbsp;</td>
           <td><asp:TextBox ID="txtRefNo" runat="server" CssClass="textBoxSmall" TabIndex="8" MaxLength="50"></asp:TextBox></td>
        </div>
        
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
        <td class="normalLabel">Payment Amount (RM) <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtPaymentAmount" runat="server" CssClass="textBoxSmall" TabIndex="9" MaxLength="50"></asp:TextBox></td>
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
		<td class="normalLabel">No Of Receipt To Print?</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtNoPrint" runat="server" CssClass="textBoxExtraSmall" TabIndex="10" MaxLength="50"></asp:TextBox></td>
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
      <td colspan='14' align='right'>
     	    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="buttonMedium" CausesValidation="true"
                TabIndex="30" OnClick="btnAdd_Click" />
            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="buttonMedium" CausesValidation="true" OnClick ="doDelete"
                TabIndex="31" OnClientClick='return confirm("Are you sure you want to delete this entry?");'/>
    </td>
    <td class="hspace"></td>
  </tr>


	<tr class="vSpace">
		<td colspan="15">&nbsp;</td>
	</tr>
	
    <tr>
		                    <td colspan="15">
			                    <!-- Spreadsheet Header -->
			                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                     <tr>
                            <td bgcolor="white">
                                <asp:GridView ID="DefaultGrid" runat="server" AllowPaging="True" AllowSorting = "True" 
                                    AutoGenerateColumns="False" CellPadding="0" Width="100%" DataKeyNames ="amount,paymentType,paymentTypeDesc,paymentDate,paymentDescription,refNo"
                                    BorderWidth="1px" HorizontalAlign="Left" CellSpacing="1">
                                    <Columns>
                                        <asp:BoundField DataField="paymentDate" HeaderText="Payment Date" SortExpression="paymentDate"
                                            />
                                        <asp:BoundField DataField="paymentDescription" HeaderText="Payment Description" SortExpression="paymentDescription"
                                            />
                                        <asp:BoundField DataField="paymentType" HeaderText="Payment Type" SortExpression="paymentType" Visible ="false"
                                             />
                                        <asp:BoundField DataField="amount" HeaderText="Amount (RM)" SortExpression="amount"
                                           />
                                        <asp:BoundField DataField="refNo" HeaderText="RefNo" SortExpression="refNo" Visible ="false"
                                           />                                                                                                                                
                                        <asp:BoundField DataField="paymentTypeDesc" HeaderText="Payment Type" SortExpression="paymentTypeDesc" 
                                           />  
                                         <asp:TemplateField> 
                                            <ItemTemplate> 
                                              <asp:LinkButton ID="LinkButton3" Text="Delete" CommandName="deleterow" OnClientClick='return confirm("Are you sure you want to delete this entry?");' runat="server"></asp:LinkButton> 
                                            </ItemTemplate> 
                                         </asp:TemplateField>                                            
                                    </Columns>
                                    <RowStyle CssClass="grid_row1" />
                                    <SelectedRowStyle CssClass="tb-highlight" />
                                    <HeaderStyle CssClass="grid_header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <AlternatingRowStyle CssClass="grid_row2" />
                                    <PagerStyle Font-Bold="True" Font-Underline="True" />
                                </asp:GridView>
                            </td>
                        </tr>
               	                    </table>
		                    </td>
	                    </tr>         
                        
	<tr>
		<td class="hSpace" height="22">&nbsp;</td>
		<td class="hSpace" height="22" colspan="13">&nbsp;</td>
		<td class="hSpace" height="22">&nbsp;</td>
	</tr>



  <tr>
  <td class="hspace"></td>
      <td colspan='12' align='right'>
     	    <asp:Button ID="btnConfirm" runat="server" Text="Confirm Payment" CssClass="buttonLarge" CausesValidation="false"
                TabIndex="32" />
            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="buttonMedium" CausesValidation="true"
                TabIndex="33" />
    </td>
    <td class="hspace"></td>
  </tr>

	<tr class="vSpace">
		<td colspan="15">&nbsp;</td>
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
