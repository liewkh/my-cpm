<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ItemReplacement.aspx.vb" Inherits="Transaction_ItemReplacement"  EnableEventValidation="false"  %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar"%>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral,PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Receipt</title>
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
    
    function verifyAction(){
    
    var p=document.getElementById("<%=ddReason.ClientID%>");
    
    if (p.options[p.selectedIndex].value== "<%= ItemReplacementReasonEnum.Lost %>" || p.options[p.selectedIndex].value== "<%= ItemReplacementReasonEnum.Mishandling %>" )
       {    var agree=confirm("Are you sure you wish to Continue? Chargeable Amount RM " + document.getElementById('txtDeposit').value);
            if (agree)
	          return true;
            else
	           return false;
	   }        
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
					<td class="header1" style="border:none;"><asp:Label runat="server" id="lblHeader1" Text="Item"></asp:Label></td>
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
			  <td class="header2" style="border:none;">Replacement&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			<asp:HiddenField ID="hidDebtorId" runat="server"/>
			<asp:HiddenField ID="hidLocationInfoId" runat="server"/>
			<asp:HiddenField ID="hdPreview" runat="server" />
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
        <td class="normalLabel">Transaction Date <font color="#FF0000">*</font></td>
        <td class="hSpace">&nbsp;</td>
      	<td class="normalLabel">
		<asp:TextBox ID="txtTransactionDate" runat="server" CssClass="dateBox" MaxLength="12" TabIndex="2"></asp:TextBox> 
        <rjs:PopCalendar id="popCalendar1" runat="server"
           Control="txtTransactionDate"
           Shadow="True" ShowWeekend="True" Move="True"
           Format="dd mm yyyy" Fade="0.5" Separator= "/"
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

	<tr class="vSpace">
	    <td class="normalLabel">[Category]</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:RadioButton ID="rbIndividual" runat="server" GroupName="Category" TabIndex="3" Text="Individual" AutoPostBack="true"/>&nbsp;
                   <asp:RadioButton ID="rbCompany" runat="server" GroupName="Category" TabIndex="4" Text="Company" Checked="True" AutoPostBack="true"/>
        </td>
		<td class="hSpace">&nbsp;</td>
        <td class="normalLabel">Activation Date</td>
        <td class="hSpace">&nbsp;</td>
      	<td class="normalLabel">
		<asp:TextBox ID="txtActivationDate" runat="server" CssClass="dateBox" MaxLength="12" TabIndex="2"></asp:TextBox> 
        <rjs:PopCalendar id="popCalendar2" runat="server"
           Control="txtActivationDate"
           Shadow="True" ShowWeekend="True" Move="True"
           Format="dd mm yyyy" Fade="0.5" Separator= "/"
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

</div> 

	            
  <div id="divInv" runat="server" visible="true">
  
              
	            
    <tr>
		<td class="normalLabel">Reason <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
        <td nowrap><asp:DropDownList ID="ddReason" runat="server" TabIndex="1" DataSourceID="dsReason"
                                    DataTextField="CodeDesc" DataValueField="codeabbr" CssClass="dropdownLarge" AutoPostBack="true"  OnSelectedIndexChanged="ddReason_SelectedIndexChanged">
                                   </asp:DropDownList><asp:SqlDataSource ID="dsReason" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                    SelectCommand="select codeabbr,codedesc,0 as seq from codemstr where codecat='RRS' union all select codeabbr,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,codedesc">
                                   </asp:SqlDataSource>
                            &nbsp;
        </td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel"><asp:Label id="lblPaymentType" runat="server" Text="Payment Type" Visible="False"></asp:Label></td>
		<td class="hSpace">&nbsp;</td>		
	    <td nowrap><asp:DropDownList ID="ddPaymentType" runat="server" TabIndex="11" DataSourceID="dsPaymentType" visible="false"  AutoPostBack="true" DataTextField="codedesc" DataValueField="codeabbr" CssClass="dropdownMedium" OnSelectedIndexChanged="ddPaymentType_SelectedIndexChanged"></asp:DropDownList><asp:SqlDataSource ID="dsPaymentType" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            SelectCommand="select codeabbr,codedesc,0 as seq from codemstr where codecat='PTY' and Active='Y' union all select codeabbr,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,codedesc">
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
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
        <td class="hSpace">&nbsp;</td>
        <td class="normalLabel"><asp:Label id="lblRef" runat="server" Text="Cheque/Credit Card/Approval No" Visible="False"></asp:Label></td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtNo" runat="server" CssClass="textBoxMedium" Visible="False" MaxLength="50" TabIndex="13"></asp:TextBox></td>
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
		<td class="normalLabel">Item Type <font color="#FF0000">*</font></td>
        <td class="hSpace">&nbsp;</td>
        <td nowrap><asp:DropDownList ID="ddItemType" runat="server"  AutoPostBack="true" TabIndex="7" DataSourceID="dsItemType" DataTextField="codedesc" DataValueField="codeabbr" CssClass="dropdownMedium" OnSelectedIndexChanged="ddItemType_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:SqlDataSource ID="dsItemType" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                           SelectCommand="SELECT CODEABBR,CODEDESC,0 AS SEQ FROM CODEMSTR WHERE CODECAT = 'ITY' AND ACTIVE = 'Y' UNION ALL SELECT CODEABBR,CODEDESC,SEQ FROM CODEMSTR WHERE CODECAT = 'DEFAULT' ORDER BY SEQ,CODEDESC">
                                        </asp:SqlDataSource>
		</td>
		<td class="hSpace">&nbsp;</td>
      	<td class="normalLabel">Deposit (RM)</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtDeposit" runat="server" CssClass="textBoxSmallDisabled" MaxLength="200" TabIndex="9"></asp:TextBox></td>
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
	    <td class="normalLabel">Old Pass<font color="#FF0000">*</font></td>
	    <td class="hSpace">&nbsp;</td>
        <td nowrap><asp:DropDownList ID="ddOldPass" runat="server" TabIndex="11" DataSourceID="dsOldPass" DataTextField="serialNo" DataValueField="passcardmstrid" CssClass="dropdownMedium"  AutoPostBack="true" OnSelectedIndexChanged="ddOldPass_SelectedIndexChanged"></asp:DropDownList><asp:SqlDataSource ID="dsOldPass" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>">
                                        </asp:SqlDataSource>
		</td>
        <td class="hSpace">&nbsp;</td>           
		<td class="normalLabel">New Pass<font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:DropDownList ID="ddNewPass" runat="server" TabIndex="11" DataSourceID="dsNewPass" DataTextField="serialNo" DataValueField="passcardmstrid" CssClass="dropdownMedium"></asp:DropDownList><asp:SqlDataSource ID="dsNewPass" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>">
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
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">DO No</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtDoNo" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="13"></asp:TextBox></td>
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
	    <td class="normalLabel">Processed By</td>
        <td class="hSpace">&nbsp;</td>
      	<td><asp:TextBox ID="txtProcessedBy" runat="server" CssClass="textBoxMediumDisabled" MaxLength="200" TabIndex="13"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Remark</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtRemark" runat="server" CssClass="textBoxLarge" MaxLength="200" TabIndex="13"></asp:TextBox></td>
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
                            <asp:Button ID="btnConfirm" runat="server" Text="Add" CssClass="buttonMedium" CausesValidation="true"
                                TabIndex="15" OnClick="btnConfirm_Click" onclientclick="return verifyAction();"/>
                            <asp:Button ID="btnDataBack" runat="server" Text="Back" CssClass="buttonMedium" CausesValidation="false"
                                TabIndex="16" OnClick="btnDataBack_Click" />    
                        </td>
                        <td class="hSpace" height="22">
                            &nbsp;</td>
                        <td class="hSpace" height="22">
                            &nbsp;</td>
                    </tr>
                    


                                        		
  
  </div> 
  

	</table>
	        
  <ajaxToolkit:FilteredTextBoxExtender ID="ftbe1" runat="server" TargetControlID="txtDeposit" ValidChars="1234567890." />

  
  <asp:RegularExpressionValidator ID="v1" runat="server" ControlToValidate="txtDeposit" Display="None" ErrorMessage="Invalid format. Payment Amount is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>

	
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
