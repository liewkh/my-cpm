<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Cancellation.aspx.vb" Inherits="Transaction_Cancellation" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar"%>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral,      PublicKeyToken=31bf3856ad364e35"      Namespace="System.Web.UI" TagPrefix="asp" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>Parking Services</title>
<link rel="stylesheet" type="text/css" href="../include/stylesheet.css" />
<script type="text/javascript" language="JavaScript" src='../include/effect.js'></script>
<script type="text/javascript" language="JavaScript" src='../include/javascript.js'></script>
</head>

<script language="javascript" type="text/javascript">


window.onscroll=move;

function DateChanged(_TextBox,_PopCal)
{
	var _TextBoxWeek=document.getElementById("txtCommencementDate")
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
                var gridView = $get('<%= gvBill.ClientID %>');

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
                                 
		</script>
		
<body>
<form action='' runat ="server">
<asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" runat="server"></asp:ScriptManager> 
<asp:UpdatePanel ID="UpdatePanel1" runat="server">                         
<ContentTemplate> 

<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td class="header1">
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td class="hSpace">&nbsp;</td>
					<td class="header1" style="border:none;">Parking Services</td>
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
			  <td class="header2" style="border:none;">Cancellation&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			</tr>
		  </table>
		  <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errorMsg" DisplayMode="List" />		                                    
               <asp:RequiredFieldValidator ID="Location" runat="server" ControlToValidate="ddLocation" Display="None" ErrorMessage="Location is a required field."></asp:RequiredFieldValidator>
               <asp:RequiredFieldValidator ID="Debtor" runat="server" ControlToValidate="txtDebtor" Display="None" ErrorMessage="Debtor Name is a required field."></asp:RequiredFieldValidator>               
               <asp:RequiredFieldValidator ID="PaymentDate" runat="server" ControlToValidate="txtPaymentDate" Display="None" ErrorMessage="Payment Date is a required field."></asp:RequiredFieldValidator>
               <asp:RequiredFieldValidator ID="CancellationDate" runat="server" ControlToValidate="txtCancellationDate" Display="None" ErrorMessage="Cancellation Date is a required field."></asp:RequiredFieldValidator>
               <asp:RequiredFieldValidator ID="Remark" runat="server" ControlToValidate="txtRemark" Display="None" ErrorMessage="Remark is a required field."></asp:RequiredFieldValidator>
		</td>
	</tr>

	            <tr class="vSpace">
		                    <td colspan="13"><asp:Label ID="lblmsg" runat="server" CssClass="errorMsg"></asp:Label></td>		        		        
		        	                        <td class="hSpace" height="22">&nbsp;</td>
		                    <td class="hSpace" height="22" style="width: 13px">&nbsp;</td>
	            </tr>

	<tr class="vSpace">
		<td class="normalLabel">Location <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
        <td nowrap><asp:DropDownList ID="ddLocation" runat="server" TabIndex="1" DataSourceID="dsLocation" DataTextField="locationName" DataValueField="locationinfoid" CssClass="dropdownLarge"></asp:DropDownList><asp:SqlDataSource ID="dsLocation" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            SelectCommand="select locationinfoid,locationName,0 as seq from locationinfo union all select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,locationname">
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
		<td class="normalLabel">Category <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:RadioButton ID="rbIndividual" runat="server" GroupName="Category" TabIndex="2" Text="Individual" AutoPostBack="true"/>&nbsp;
                   <asp:RadioButton ID="rbCompany" runat="server" GroupName="Category" TabIndex="3" Text="Company" Checked="True" AutoPostBack="true"/>
        </td>
        <td class="hSpace">&nbsp;</td>
        <td class="normalLabel">Cancellation Date <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
        <td class="normalLabel">
		<asp:TextBox ID="txtCancellationDate" runat="server" CssClass="dateBox" MaxLength="12" TabIndex="4"></asp:TextBox>          
        <rjs:PopCalendar id="popCalendar1" runat="server"
           Control="txtCancellationDate"
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
		<td><asp:TextBox ID="txtDebtor" runat="server" CssClass="textBoxLarge" TabIndex="5" MaxLength="200"></asp:TextBox>   
		    <img id="imgDebtor"  runat = "server" alt="" border="0" src="../img/magnifier.gif" width="10" height="10">
		</td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Effective From <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
        <td class="normalLabel">
		<asp:TextBox ID="txtEffectiveFrom" runat="server" CssClass="dateBox" MaxLength="12" TabIndex="6"></asp:TextBox>          
        <rjs:PopCalendar id="popCalendar2" runat="server"
           Control="txtEffectiveFrom"
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
	    <td class="normalLabel">Reason <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:DropDownList ID="ddCancelReason" runat="server" TabIndex="7" DataSourceID="dsCancelReason" DataTextField="codedesc" DataValueField="codeabbr" CssClass="dropdownLarge"></asp:DropDownList><asp:SqlDataSource ID="dsCancelReason" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            SelectCommand="select codeabbr,codedesc,0 as seq from codemstr where codecat = 'CRS' and active = 'Y' union all select codeabbr,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,codedesc">
                                        </asp:SqlDataSource>
		</td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Pass Card Condition <font color="#FF0000">*</font></td>
		<td nowrap><asp:DropDownList ID="ddPassCardCondition" runat="server" TabIndex="8" DataSourceID="dsPassCardCondition" DataTextField="codedesc" DataValueField="codeabbr" CssClass="dropdownMedium"></asp:DropDownList><asp:SqlDataSource ID="dsPassCardCondition" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            SelectCommand="select codeabbr,codedesc,0 as seq from codemstr where codecat = 'PCC' and active = 'Y' union all select codeabbr,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,codedesc">
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
	</tr>

    <tr>
        <td class="normalLabel">Deposit <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:DropDownList ID="ddDeposit" runat="server" TabIndex="9" DataSourceID="dsDeposit" DataTextField="codedesc" DataValueField="codeabbr" CssClass="dropdownMedium"></asp:DropDownList><asp:SqlDataSource ID="dsDeposit" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            SelectCommand="select codeabbr,codedesc,0 as seq from codemstr where codecat = 'DPS' and active = 'Y' union all select codeabbr,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,codedesc">
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



    <tr class="vSpace">
        <td class="normalLabel">Remark</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtRemark" runat="server" CssClass="textBoxLarge" MaxLength="2000" TabIndex="10"></asp:TextBox></td>
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
        <td class="normalLabel">Processed By</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtProcessedBy" runat="server" CssClass="textBoxMediumDisabled" MaxLength="200" TabIndex="11"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
        <td class="normalLabel">Checked By</td>
        <td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtCheckedBy" runat="server" CssClass="textBoxMediumDisabled" MaxLength="200" TabIndex="12"></asp:TextBox></td>
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
        <td class="normalLabel">Verified By</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtVerifiedBy" runat="server" CssClass="textBoxMediumDisabled" MaxLength="200" TabIndex="13"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
        <td class="normalLabel">Status</td>
        <td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtStatus" runat="server" CssClass="textBoxLargeDisabled" MaxLength="200" TabIndex="14"></asp:TextBox></td>
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
		                    <td colspan = "13" align ="left" style="width: 92px"><asp:Label ID="lblRecCount" runat="server" CssClass="errorMsg"></asp:Label></td>		       
		        		                    <td class="hSpace" height="22">&nbsp;</td>
		                    <td class="hSpace" height="22" style="width: 13px">&nbsp;</td>
                        </tr>	


    <tr>
		                    <td colspan="15">
			                    <!-- Spreadsheet Header -->
			                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                			    

                        <%  If gvBill.Rows.Count > 0 Then%>
                        
                        <tr>
                            <td bgcolor="white">
                                <asp:GridView ID="gvBill" runat="server" AllowPaging="True" AllowSorting = "True" 
                                    AutoGenerateColumns="False" CellPadding="0" Width="100%"
                                    BorderWidth="1px" HorizontalAlign="Left" CellSpacing="1" DataKeyNames="DEBTORID,CATEGORY,COMMENCEMENTDATE,BRANCHINFOID,CARREGISTRATIONNO,MAKE,MODEL,DEBTOR,
                                    ICNO,TELNOOFFICE,TELNOMOBILE,TELNOHOME,EMPLOYERNAME,ADDRESS1,ADDRESS2,ADDRESS3,POSTCODE,
                                    USERNAME,CONTACTPERSON,DESIGNATION,UNITNO,BLOCK,REMARK,LASTINVOICENO,LASTINVOICEDATE,LASTPAYMENT,
                                    BALANCE,STATUS,LUDT,LUB,INVOICEREQUIRED" DataSourceID="dsDebtor">
                                    <Columns>
                                        <asp:BoundField DataField="DEBTOR" HeaderText="Cancel" SortExpression="DEBTOR"
                                            NullDisplayText="N/A" />
                                        <asp:BoundField DataField="CARREGISTRATIONNO" HeaderText="Pass/Bay No" SortExpression="CARREGISTRATIONNO"
                                            NullDisplayText="N/A" />                                                            
                                    </Columns>
                                    <RowStyle CssClass="grid_row1" />
                                    <SelectedRowStyle CssClass="tb-highlight" />
                                    <HeaderStyle CssClass="grid_header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <AlternatingRowStyle CssClass="grid_row2" />
                                    <PagerStyle Font-Bold="True" Font-Underline="True" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="dsDebtor" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                    ProviderName="System.Data.SqlClient"></asp:SqlDataSource>
                            </td>
                        </tr>
                        <%End If%>   		
                			    
                			    
			                    </table>
		                    </td>
	                    </tr>
	                    
	                    

	<tr>
		<td class="hSpace" height="22">&nbsp;</td><td class="hSpace" height="22" colspan="13">&nbsp;</td>
		<td class="hSpace" height="22">&nbsp;</td>
	</tr>


    <tr>
        <td class="hSpace" height="22" style="width: 92px">&nbsp;</td>
        <td class="hSpace" height="22" style="width: 13px">&nbsp;</td>
        <td height="22" colspan="12" align="right">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="buttonMedium" CausesValidation="false"
                TabIndex="32"  />
            <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="buttonMedium" CausesValidation="false"
                TabIndex="33"  />
            <asp:Button ID="btnApproved" runat="server" Text="Approve" CssClass="buttonMedium" CausesValidation="false"
                TabIndex="34"  />
            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="buttonMedium" CausesValidation="false"
                TabIndex="35"  />            
        </td>
        <td class="hSpace" height="22">
            &nbsp;</td>
        <td class="hSpace" height="22">
            &nbsp;</td>
    </tr>
    


	<tr class="vSpace">
		<td colspan="15">&nbsp;</td>
	</tr>


	</table>
	
	</ContentTemplate>                    
</asp:UpdatePanel> 

<ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" runat="server" TargetControlID="UpdatePanel1">
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
<asp:Panel ID="pnlPopup" runat="server" CssClass="progress" style="display:none;">
    <div class="container">
        <div class="header">Loading, please wait...</div>
        <div class="body">
            <img src="../img/activity.gif" />
        </div>
    </div>
</asp:Panel> 	

</form>
</body>
</html>
