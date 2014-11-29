<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MiscPayment.aspx.vb" Inherits="Transaction_MiscPayment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
<title>Misc Payment</title>
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
                        document.getElementById('<%=btnAdd.ClientID%>').disabled = true;
                    }

                    function onUpdated() {
                        // get the update progress div
                        var pnlPopup = $get('<%= pnlPopup.ClientID %>');
                        // make it invisible
                        pnlPopup.style.display = 'none';
                        document.getElementById('<%=btnAdd.ClientID%>').disabled = false;
                    }
                    
                     function checkToPopUpViewer(){ 
                             if (document.forms(0).hdPreview.value == "1")           
                                {                                     
                                   showCrystalReportDialog("../Report/CrystalReportView.aspx", 900, 1000);
                                   hdPreview.value = "";
                                }
    
        }
                            
</script>

   
<body>
<form action='' runat="server">
<!--10 minutes -->
 <asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" runat="server" AsyncPostBackTimeOut="600">
        </asp:ScriptManager>
        
<script language="javascript" type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequest);
    
    
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(startRequest);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

    function startRequest( sender, e ) { 
      //disable search button during the AJAX call
      document.getElementById('<%=btnAdd.ClientID%>').disabled = true;
      document.getElementById('<%=btnClear.ClientID%>').disabled = true;
    }

 
     function endRequest( sender, e ) { 
     //re-enable the search button once the AJAX call has completed
     document.getElementById('<%=btnAdd.ClientID%>').disabled = false;
     document.getElementById('<%=btnClear.ClientID%>').disabled = false;
     }


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
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"  >
            <contenttemplate> 
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td class="header1">
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td class="hSpace">&nbsp;</td>
					<td class="header1" style="border:none;">Payment</td>
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
			  <td class="header2" style="border:none;">&nbsp; Misc Payment</td>
			  <td class="hSpace">&nbsp;</td>
			</tr>
		  </table>
		  <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errorMsg"
                                            DisplayMode="List" />
		   <asp:RequiredFieldValidator ID="Location" runat="server" ControlToValidate="ddLocation" Display="None" ErrorMessage="Location is a required field."></asp:RequiredFieldValidator>
           <asp:RequiredFieldValidator ID="Debtor" runat="server" ControlToValidate="ddMiscPaymentType" Display="None" ErrorMessage="Payment Type is a required field."></asp:RequiredFieldValidator>           
           <asp:HiddenField ID="hdPaymentTypeDesc" runat="server"/>
           <asp:HiddenField ID="hdPreview" runat="server"/>
           <asp:HiddenField ID="hdMiscPaymentId" runat="server"/>
           <asp:HiddenField ID="hdRcpNo" runat="server"/>
           <asp:HiddenField ID="hdSubTotal" runat="server"/>
           
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
	
	<tr>
		<td class="normalLabel">Visitor</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:CheckBox ID="chkVisitor" Checked="false" runat="server" AutoPostBack="true"  CssClass="checkBox" TabIndex="15" OnCheckedChanged="chkVisitor_CheckedChanged"></asp:CheckBox></td>
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
		<td class="normalLabel" style="width: 134px; height: 20px;">[Category] <font color="#FF0000">*</font></td>
        <td class="hSpace" style="height: 20px">&nbsp;</td>
        <td nowrap style="height: 20px"><asp:RadioButton ID="rbIndividual" runat="server" GroupName="Category" TabIndex="1" Text="Individual" AutoPostBack="true" OnCheckedChanged="rbIndividual_CheckedChanged"/>&nbsp;
                   <asp:RadioButton ID="rbCompany" runat="server" GroupName="Category" TabIndex="2" Text="Company" Checked="True" AutoPostBack="true" OnCheckedChanged="rbCompany_CheckedChanged"/></td>
		<td class="hSpace" style="height: 20px">&nbsp;</td>
      	<td class="hSpace" style="height: 20px">&nbsp;</td>
		<td class="hSpace" style="height: 20px">&nbsp;</td>
		<td class="normalLabel" style="height: 20px">
		<td class="hSpace" style="height: 20px">&nbsp;</td>
		<td class="hSpace" style="height: 20px">&nbsp;</td>
		<td class="hSpace" style="height: 20px">&nbsp;</td>
		<td class="hSpace" style="height: 20px">&nbsp;</td>
		<td class="hSpace" style="height: 20px">&nbsp;</td>
		<td class="hSpace" style="height: 20px">&nbsp;</td>
		<td class="hSpace" style="height: 20px">&nbsp;</td>
		<td class="hSpace" style="height: 20px">&nbsp;</td>
		<td class="hSpace" style="height: 20px">&nbsp;</td>
	</tr>
	
		
  <tr class="vSpace">
		<td class="normalLabel">Location <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:DropDownList ID="ddLocation" runat="server" TabIndex="4" DataSourceID="dsLocation" DataTextField="locationName" DataValueField="locationinfoid" AutoPostBack ="True" CssClass="dropdownLarge" OnSelectedIndexChanged="ddLocation_SelectedIndexChanged"></asp:DropDownList><asp:SqlDataSource ID="dsLocation" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"></asp:SqlDataSource>
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
		<td class="normalLabel">Debtor <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:DropDownList ID="ddDebtor" runat="server" TabIndex="5" DataSourceID="dsDebtor" DataTextField="Debtor" DataValueField="DebtorId" CssClass="dropdownLarge"></asp:DropDownList><asp:SqlDataSource ID="dsDebtor" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"></asp:SqlDataSource>
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
		<td class="normalLabel">Payment Type <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:DropDownList ID="ddMiscPaymentType"  AutoPostBack="true"  runat="server" TabIndex="5" DataSourceID="dsMiscPaymentType" DataTextField="PaymentDesc" DataValueField="MiscPaymentTypeMstrId" CssClass="dropdownLarge" OnSelectedIndexChanged="ddMiscPaymentType_SelectedIndexChanged"></asp:DropDownList><asp:SqlDataSource ID="dsMiscPaymentType" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"></asp:SqlDataSource>		
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
		<td class="normalLabel">Amount</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtAmount" runat="server" CssClass="textBoxExtraSmall" TabIndex="10" Enabled="false"   MaxLength="10" BackColor="#FFFF80" ForeColor="Red"></asp:TextBox></td>        
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
		<td class="normalLabel">Remark</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtRemark" runat="server" CssClass="textBoxLarge" TabIndex="11" MaxLength="2000"></asp:TextBox></td>        
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
      <td colspan='15' align='right'>
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="buttonMedium" CausesValidation="true"
                TabIndex="32" OnClick="btnAdd_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonMedium" CausesValidation="false"
                TabIndex="34" OnClick="btnClear_Click"/>
            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="buttonMedium" CausesValidation="false" TabIndex="33" OnClientClick="javascript:window.close();"/>
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
                                    <asp:BoundField DataField="MISCPAYMENTTYPEMSTRID" HeaderText="MISCPAYMENTTYPEMSTRID" SortExpression="MISCPAYMENTTYPEMSTRID" Visible="false" 
                                        NullDisplayText="N/A" /> 
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" CausesValidation="false" CommandName="Delete" runat="server" OnClientClick="return confirm('Are you sure')">Delete</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                       
                                    <asp:BoundField DataField="PAYMENTDESC" HeaderText="Payment Description" SortExpression="PAYMENTDESC"
                                        NullDisplayText="N/A" />
                                    <asp:BoundField DataField="REMARK" HeaderText="Remark" SortExpression="Remark"
                                        NullDisplayText="N/A" />
                                    <asp:BoundField DataField="QTY" HeaderText="Quantity" SortExpression="Quantity"
                                        NullDisplayText="N/A" />
                                    <asp:BoundField DataField="AMOUNT" HeaderText="Amount" SortExpression="Amount"
                                        NullDisplayText="N/A" DataFormatString="{0:F2}" />
                                    <asp:BoundField DataField="TOTAL" HeaderText="TOTAL" SortExpression="TOTAL"
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
  
  

	
	<tr class="vSpace">
		<td colspan="15">&nbsp;</td>
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
