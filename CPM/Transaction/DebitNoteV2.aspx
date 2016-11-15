<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DebitNoteV2.aspx.vb" Inherits="Transaction_DebitNoteV2" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar"%>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral,PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Debit Note</title>
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
                        var gridView = $get('<%= gvDebtorEnq.ClientID %>');

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
                    

     function checkToPopUpViewer(){ 
                             if(document.forms(0).hdPreview.value == "1"){           
            showCrystalReportDialog("../Report/CrystalReportView.aspx", 900, 1000);
            hdPreview.value = "";
        }
    } 

function funcsum()
{ 
var table = document.getElementById("gvDebtorInv");
var sum = 0;
for(var i=1;i<table.rows.length;i++) //setting the incrementor=0, but if you have a header set it to 1 
{ 
if(!isNaN(table.rows[i].cells[5].innerText))
{
sum=(parseFloat(sum)+parseFloat(table.rows[i].cells[5].innerText)).toString();
}
}
document.getElementById("txtPaymentAmount").innerText=formatCurrency(sum);
} 
                                    
</script>

<body> 
<form id="Form1" action='' runat="server" >
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
					<td class="header1" style="border:none;">Debit Note</td>
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
			  <td class="header2" style="border:none;">Entry&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			<asp:HiddenField ID="hidDebtorId" runat="server"/>
			<asp:HiddenField ID="hidDebtorAccountHeaderId" runat="server"/>
			<asp:HiddenField ID="hidLocationInfoId" runat="server"/>
		    <asp:HiddenField ID="hdPreview" runat="server"/>		    
		    <asp:HiddenField ID="hdPaymentTypeDesc" runat="server"/>
		    <asp:HiddenField ID="hdTaxCode" runat="server"/>
		    <asp:HiddenField ID="hdInvoiceNo" runat="server"/>
		    <asp:HiddenField ID="hdSubTotal" runat="server"/>
			</tr>
		  </table>
		</td>
	</tr>

	                <tr class="vSpace">		    
		                <td colspan="15"><asp:Label ID="lblmsg" runat="server" CssClass="errorMsg"></asp:Label></td>		                
                        </td>
	                </tr>


  
	<tr class="vSpace">
	    <td class="normalLabel">[Location]</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:DropDownList ID="ddLocation" runat="server" TabIndex="1" DataSourceID="dsLocation" DataTextField="locationName" DataValueField="locationinfoid" CssClass="dropdownLarge">
                            </asp:DropDownList><asp:SqlDataSource ID="dsLocation" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>">
                            </asp:SqlDataSource>
        </td>
		<td class="hSpace">&nbsp;</td>
        <td class="normalLabel">Debit Note Date <font color="#FF0000">*</font></td>
        <td class="hSpace">&nbsp;</td>
      	<td class="normalLabel">
		<asp:TextBox ID="txtDebitNoteDate" runat="server" CssClass="dateBox" Enabled="false" MaxLength="12" TabIndex="2"></asp:TextBox> 
        <rjs:PopCalendar id="popCalendar1" runat="server"
           Control="txtTaxInvoiceDate" To-Today="true" 
           Shadow="True" ShowWeekend="True" Move="True"
           Format="dd mm yyyy" Fade="0.5" Separator= "/"
           ToolTip="Click For Calendar: ([Format])" Enabled="true"                               
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
	    <td class="normalLabel">[Category]</td>
		<td class="hSpace">&nbsp;</td>
    	<td nowrap><asp:RadioButton ID="rbIndividual" runat="server" GroupName="Category" TabIndex="3" Text="Individual" AutoPostBack="true"/>&nbsp;
                   <asp:RadioButton ID="rbCompany" runat="server" GroupName="Category" TabIndex="4" Text="Company" Checked="True" AutoPostBack="true"/>
        </td>
		<td class="hSpace">&nbsp;</td>		
        <td class="normalLabel">Invoice Period <font color="#FF0000">*</font></td>		
        <td class="hSpace">&nbsp;</td>
		<td class="normalLabel">
		<asp:TextBox ID="txtInvoicePeriodDate" runat="server" class="date-picker" CssClass="dateBox" Enabled="true" MaxLength="12" TabIndex="2"></asp:TextBox> 
        <rjs:PopCalendar id="popCalendar2" runat="server"
           Control="txtInvoicePeriodDate" To-Today="false" 
           Shadow="True" ShowWeekend="True" Move="True"
           Format="mm yyyy" Fade="0.5" Separator= "/"
           ToolTip="Click For Calendar: ([Format])" Enabled="true"                               
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
		<td class="normalLabel">[Debtor Name]</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtDebtorName" runat="server" CssClass="textBoxLarge" MaxLength="200" TabIndex="6"></asp:TextBox></td>
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
	
  <div id="divSearch" runat="server" visible="true">
	
    <tr>
		<td colspan="15" height="17"></td>
    </tr>
                    <tr>
                        <td class="hSpace" height="22" style="width: 92px">
                            &nbsp;</td>
                        <td class="hSpace" height="22" style="width: 13px">
                            &nbsp;</td>
                        <td height="22" colspan="7" align="right">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonMedium" CausesValidation="false"
                                TabIndex="15" OnClick="btnSearch_Click" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonMedium" CausesValidation="false"
                                TabIndex="8" OnClick="btnClear_Click" />
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
                                <%  If gvDebtorEnq.Rows.Count > 0 Then%>
                                <tr>
                                    <td bgcolor="white">
                                        <asp:GridView ID="gvDebtorEnq" runat="server" AllowPaging="True" AllowSorting="True"
                                            AutoGenerateColumns="False" CellPadding="0" Width="100%" BorderWidth="1px" HorizontalAlign="Left"
                                            CellSpacing="1" DataKeyNames="DEBTORID,CATEGORY,DEBTOR,LOCATIONINFOID,LOCATIONNAME,ADDRESS"
                                            DataSourceID="dsDebtorEnq">
                                            <Columns>
                                                <asp:BoundField DataField="LOCATIONNAME" HeaderText="Location" SortExpression="LOCATIONNAME"
                                                    NullDisplayText="N/A" />                                                
                                                <asp:BoundField DataField="DEBTOR" HeaderText="Debtor Name" SortExpression="DEBTOR"
                                                    NullDisplayText="N/A" />
                                                <asp:BoundField DataField="ADDRESS" HeaderText="Address" SortExpression="ADDRESS"
                                                    NullDisplayText="N/A" />                                                   
                                            </Columns>
                                            <RowStyle CssClass="grid_row1" />
                                            <SelectedRowStyle CssClass="tb-highlight" />
                                            <HeaderStyle CssClass="grid_header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <AlternatingRowStyle CssClass="grid_row2" />
                                            <PagerStyle Font-Bold="True" Font-Underline="True" />
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="dsDebtorEnq" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
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
	
</div> 

  <div id="divInv" runat="server" visible="true">
  
  <tr class="vSpace">
		<td class="normalLabel">Invoice <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:DropDownList ID="ddInvoice" AutoPostBack ="True" runat="server" TabIndex="5" DataSourceID="dsInvoice" DataTextField="InvoiceNo" DataValueField="invoice1" CssClass="dropdownLarge"></asp:DropDownList><asp:SqlDataSource ID="dsInvoice" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"></asp:SqlDataSource>
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
	</tr>
	
  
   <tr class="vSpace">
		<td class="normalLabel">Qty <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtQty" runat="server" CssClass="textBoxExtraSmall" TabIndex="10" MaxLength="10"></asp:TextBox></td>        
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
		<td class="normalLabel">Amount (Excluding GST)<font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtAmount" runat="server" CssClass="textBoxExtraSmall" TabIndex="11" Enabled="true"   MaxLength="10"></asp:TextBox></td>        
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
		<td class="normalLabel">Month(s)<font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtMonth" runat="server" CssClass="textBoxExtraSmall" TabIndex="12" Enabled="true" Value="0"  MaxLength="10"></asp:TextBox></td>        
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

<ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtMonth" ValidChars="1234567890" />

	<tr class="vSpace">
		<td class="normalLabel">Description <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:DropDownList ID="ddDescription" AutoPostBack ="True" runat="server" TabIndex="13" DataSourceID="dsDescription" DataTextField="Description" DataValueField="SeasonTypeMstrId" CssClass="dropdownLarge"></asp:DropDownList><asp:SqlDataSource ID="dsDescription" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"></asp:SqlDataSource>
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
	</tr>
	
	<tr class="vSpace">
		<td class="normalLabel">Remark <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtRemark" runat="server" CssClass="textBoxMedium" MaxLength="200" TabIndex="13"></asp:TextBox></td>
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
		<td colspan="15" height="17"></td>
    </tr>	
	
  <tr>
      <td colspan='15' align='right'>
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="buttonMedium" CausesValidation="true"
                TabIndex="32" OnClick="btnAdd_Click" />
            <asp:Button ID="Button1" runat="server" Text="Clear" CssClass="buttonMedium" CausesValidation="false"
                TabIndex="34" OnClick="btnClear_Click"/>
            <asp:Button ID="btnDataBack" runat="server" Text="Back" CssClass="buttonMedium" CausesValidation="false"
                                TabIndex="16" OnClick="btnDataBack_Click" />    
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
            			    

                    <%  If gvMisc.Rows.Count > 0 Then%>
                    
                    <tr>
                        <td bgcolor="white">
                            <asp:GridView ID="gvMisc" runat="server" AllowPaging="True" AllowSorting = "True" PageSize = "50" 
                                AutoGenerateColumns="False" CellPadding="0" Width="100%"
                                BorderWidth="1px" HorizontalAlign="Left" CellSpacing="1" DataKeyNames="" onrowdeleting="gvMisc_RowDeleting">
                                <Columns>                                    
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" CausesValidation="false" CommandName="Delete" runat="server" OnClientClick="return confirm('Are you sure')">Delete</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="TAXCODE" HeaderText="Tax Code" SortExpression="TaxCode"
                                        NullDisplayText="N/A" />                                       
                                    <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" SortExpression="Description"
                                        NullDisplayText="N/A" />                                       
                                    <asp:BoundField DataField="QTY" HeaderText="Quantity" SortExpression="Quantity"
                                        NullDisplayText="N/A" />
                                    <asp:BoundField DataField="AMOUNT" HeaderText="Amount" SortExpression="Amount"
                                        NullDisplayText="N/A" DataFormatString="{0:F2}" />
                                    <asp:BoundField DataField="MONTH" HeaderText="Month(s)" SortExpression="Month"
                                        NullDisplayText="N/A" />                                        
                                    <asp:BoundField DataField="TOTAL" HeaderText="TOTAL" SortExpression="Total"
                                        NullDisplayText="N/A" DataFormatString="{0:F2}" />           
                                </Columns>
                                <RowStyle CssClass="grid_row1" />
                                <SelectedRowStyle CssClass="tb-highlight" />
                                <HeaderStyle CssClass="grid_header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                <AlternatingRowStyle CssClass="grid_row2" />
                                <PagerStyle Font-Bold="True" Font-Underline="True" />
                            </asp:GridView>
                            <%--<asp:SqlDataSource ID="dsMisc" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                ProviderName="System.Data.SqlClient"></asp:SqlDataSource>--%>
                        </td>
                    </tr>
                    <%End If%>   		
            			    
            			    
			                </table>
		                </td>
	                </tr>  
            	    
	                <tr>
		                <td class="hSpace" height="22">&nbsp;</td>
		                <td class="hSpace" height="22" colspan="13">&nbsp;</td>
		                <td class="hSpace" height="22">&nbsp;</td>
	                </tr>
	                
	                  <tr>
      <td colspan='15' align='right'>
            <font color="#FF0000">Sub Total(RM)</font>
            <asp:TextBox ID="txtSubTotal" runat="server" CssClass="textBoxSmallDisabled" TabIndex="10" Visible="false" MaxLength="10" Enabled ="false" ></asp:TextBox>
    </td>
    <td class="hspace"></td>
  </tr>
	                
	                
	           
	           <tr>
                 <td colspan='15' align='right' style="height: 20px">
                   <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="buttonMedium" CausesValidation="true" Visible = "false" 
                    TabIndex="32" OnClick="btnConfirm_Click" OnClientClick="return confirm('Confirm To Proceed?')" />
                   <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="buttonMedium" CausesValidation="true" Visible = "false" 
                    TabIndex="32" OnClick="btnPrint_Click" />       
                 </td>
                 <td class="hspace" style="height: 20px"></td>
               </tr>
                    


                                        		
  
  </div> 
	
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
