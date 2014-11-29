<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DailyCollection.aspx.vb" Inherits="Transaction_DailyCollection" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar"%>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral,      PublicKeyToken=31bf3856ad364e35"      Namespace="System.Web.UI" TagPrefix="asp" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>Daily Collection</title>
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
        
        function checkConfirm()
        {
          //if (document.getElementById('txtTotalCollection').value == '' || parseInt(document.getElementById('txtTotalCollection').value) == 0 )
          //   {alert('Total Collection amount cannot be 0');return false; }
                  
          var a = document.getElementById('txtTotalDaily').value;
          var b = document.getElementById('txtSeasonTotal').value;
          var c = document.getElementById('txtTotalDeposit').value;
          var d = document.getElementById('txtOtherTotal').value;
          var e = document.getElementById('txtInfoTotal').value;
          
          
          if (confirm('Are you sure you want to add this entry? \rTotal Daily: RM ' + a + '\rTotal Season: RM ' + b + '\rTotal Deposit: RM ' + c + '\rTotal Others: RM ' + d + '\rTotal Info: Qty ' + e))
             return true;
          return false;           
        }
        
        function sumCashierTotal()
        { var a = parseFloat(document.getElementById('txtCashierShift1').value);
          var b = parseFloat(document.getElementById('txtCashierShift2').value);
          var c = parseFloat(document.getElementById('txtCashierShift3').value);

          if(isNaN(a))
             a = 0;
          if(isNaN(b))
             b = 0;
          if(isNaN(c))
             c = 0;                
           
          var total = a + b + c;
          document.getElementById('txtCashierTotal').value = formatCurrency(total);
          sumTotalDaily();
        }
        
        function sumValetTotal()
        { var a = parseFloat(document.getElementById('txtValet1').value);
          var b = parseFloat(document.getElementById('txtValet2').value);
          var c = parseFloat(document.getElementById('txtValet3').value);

          if(isNaN(a))
             a = 0;
          if(isNaN(b))
             b = 0;
          if(isNaN(c))
             c = 0;                
           
          var total = a + b + c;
          document.getElementById('txtValetTotal').value = formatCurrency(total);
          sumTotalDaily();
        } 
        
        function sumMotorcycleTotal()
        { var a = parseFloat(document.getElementById('txtMotorcycle1').value);
          var b = parseFloat(document.getElementById('txtMotorcycle2').value);
          var c = parseFloat(document.getElementById('txtMotorcycle3').value);

          if(isNaN(a))
             a = 0;
          if(isNaN(b))
             b = 0;
          if(isNaN(c))
             c = 0;                
           
          var total = a + b + c;
          document.getElementById('txtMotorcyleTotal').value = formatCurrency(total);
          sumTotalDaily();
        }                
        
        function sumAPSTotal()
        { var a = parseFloat(document.getElementById('txtAPS1').value);
          var b = parseFloat(document.getElementById('txtAPS2').value);
          var c = parseFloat(document.getElementById('txtAPS3').value);
          var d = parseFloat(document.getElementById('txtAPS4').value);
          var e = parseFloat(document.getElementById('txtAPS5').value);
          var f = parseFloat(document.getElementById('txtAPS6').value);

          if(isNaN(a))
             a = 0;
          if(isNaN(b))
             b = 0;
          if(isNaN(c))
             c = 0;                
          if(isNaN(d))
             d = 0;   
          if(isNaN(e))
             e = 0;   
          if(isNaN(f))
             f = 0;   
           
          var total = a + b + c + d + e + f;
          document.getElementById('txtAPSTotal').value = formatCurrency(total);
          sumTotalDaily();
        }
        
       function sumTotalDaily()
        { var a = parseFloat(document.getElementById('txtCashierTotal').value.replace(/\$|\,/g,''));
          var b = parseFloat(document.getElementById('txtValetTotal').value.replace(/\$|\,/g,''));
          var c = parseFloat(document.getElementById('txtMotorcyleTotal').value.replace(/\$|\,/g,''));
          var d = parseFloat(document.getElementById('txtAPSTotal').value.replace(/\$|\,/g,''));

          if(isNaN(a))
             a = 0;
          if(isNaN(b))
             b = 0;
          if(isNaN(c))
             c = 0;                
          if(isNaN(d))
             d = 0;            
                                       
          var total = a + b + c + d;
          document.getElementById('txtTotalDaily').value = formatCurrency(total);
          sumTotalCollection();
        }        
        
        function sumSeasonTotal()
        { var a = parseFloat(document.getElementById('txtMotorCash').value);
          var b = parseFloat(document.getElementById('txtMotorChq').value);
          var c = parseFloat(document.getElementById('txtCarCash').value);
          var d = parseFloat(document.getElementById('txtCarChq').value);
          var e = parseFloat(document.getElementById('txtCarCreditCard').value);
          var f = parseFloat(document.getElementById('txtMotorCreditCard').value);

          if(isNaN(a))
             a = 0;
          if(isNaN(b))
             b = 0;
          if(isNaN(c))
             c = 0;                
          if(isNaN(d))
             d = 0;           
          if(isNaN(e))
             e = 0; 
          if(isNaN(f))
             f = 0; 
                                       
          var total = a + b + c + d + e + f;
          document.getElementById('txtSeasonTotal').value = formatCurrency(total);
          sumTotalCollection();
        }
        
        function sumTotalDeposit()
        { var a = parseFloat(document.getElementById('txtDepositCar').value.replace(/\$|\,/g,''));
          var b = parseFloat(document.getElementById('txtDepositMotor').value.replace(/\$|\,/g,''));
          var c = parseFloat(document.getElementById('txtDepositOther').value.replace(/\$|\,/g,''));
           
          if(isNaN(a))
             a = 0;
          if(isNaN(b))
             b = 0;
          if(isNaN(c))
             c = 0;                
          var total = a + b + c;
          document.getElementById('txtTotalDeposit').value = formatCurrency(total);
          sumTotalCollection();
        }           
        
        function sumOtherTotal()
        { var a = parseFloat(document.getElementById('txtClamp').value);
          var b = parseFloat(document.getElementById('txtTransponder').value);
          var c = parseFloat(document.getElementById('txtNoPlate').value);
          var d = parseFloat(document.getElementById('txtMisc').value);
          var e = parseFloat(document.getElementById('txtTemporaryRental').value);

            
          if(isNaN(a))
             a = 0;
          if(isNaN(b))
             b = 0;
          if(isNaN(c))
             c = 0;                
          if(isNaN(d))
             d = 0;           
          if(isNaN(e))
             e = 0;                           
          var total = a + b + c + d + e;
          document.getElementById('txtOtherTotal').value = formatCurrency(total);
          sumTotalCollection();
        }        
            
        function sumTotalCollection()
        { var a = parseFloat(document.getElementById('txtTotalDaily').value.replace(/\$|\,/g,''));
          var b = parseFloat(document.getElementById('txtSeasonTotal').value.replace(/\$|\,/g,''));
          var c = parseFloat(document.getElementById('txtOtherTotal').value.replace(/\$|\,/g,''));
          var d = parseFloat(document.getElementById('txtTotalDeposit').value.replace(/\$|\,/g,''));
                      
          if(isNaN(a))
             a = 0;
          if(isNaN(b))
             b = 0;
          if(isNaN(c))
             c = 0;                
          if(isNaN(d))
             d = 0;                                                              
                          
          var total = a + b + c + d;
          document.getElementById('txtTotalCollection').value = formatCurrency(total);
        }   
        
        function sumInfoTotal()
        { var a = parseInt(document.getElementById('txtComplimentaryTicket').value.replace(/\$|\,/g,''));
          var b = parseInt(document.getElementById('txtManualRaised').value.replace(/\$|\,/g,''));
          var c = parseInt(document.getElementById('txtOutstandingTicket').value.replace(/\$|\,/g,''));
          var d = parseInt(document.getElementById('txtEarlyBird').value.replace(/\$|\,/g,''));
          var e = parseInt(document.getElementById('txtInfoOthers').value.replace(/\$|\,/g,''));
           
          if(isNaN(a))
             a = 0;
          if(isNaN(b))
             b = 0;
          if(isNaN(c))
             c = 0;  
          if(isNaN(d))
             d = 0;  
          if(isNaN(e))
             e = 0;        
                           
          var total = a + b + c + d + e;
          document.getElementById('txtInfoTotal').value = total;
        }  
        
 
                                                 
		</script>
	
<body>
<form action='' runat="server">
<asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" runat="server"></asp:ScriptManager> 
<asp:UpdatePanel ID="UpdatePanel1" runat="server">                         
<ContentTemplate> 
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td class="header1">
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td class="hSpace">&nbsp;</td>
					<td class="header1" style="border:none;">Daily</td>
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
		<td class="header2" colspan="15">
		  <table border="0" cellpadding="0" cellspacing="0" width="100%">
			<tr>
			  <td class="hSpace">&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			  <td class="header2" style="border:none;">Collection&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			</tr>
		  </table>
		   <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errorMsg" DisplayMode="List" />		                                    
               <asp:RequiredFieldValidator ID="Location" runat="server" ControlToValidate="ddLocation" Display="None" ErrorMessage="Location is a required field."></asp:RequiredFieldValidator>               
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
        <td nowrap><asp:DropDownList ID="ddLocation" runat="server" TabIndex="1" DataSourceID="dsLocation" DataTextField="locationName" DataValueField="locationinfoid" CssClass="dropdownMedium"></asp:DropDownList><asp:SqlDataSource ID="dsLocation" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            SelectCommand="select locationinfoid,locationName,0 as seq from locationinfo union all select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,locationname">
                                        </asp:SqlDataSource>
		</td>
        <td class="hSpace">&nbsp;</td>
        <td class="normalLabel">Date <font color="#FF0000">*</font></td>
        <td class="hSpace">&nbsp;</td>
      	<td class="normalLabel">
		<asp:TextBox ID="txtDate" runat="server" CssClass="dateBox" MaxLength="12" TabIndex="2"></asp:TextBox>          
        <rjs:PopCalendar id="popCalendar1" runat="server"
           Control="txtDate"
           Shadow="True" ShowWeekend="True" Move="True"
           Format="dd mm yyyy" Fade="0.5"
           ToolTip="Click For Calendar: ([Format])"    
           To-Today="True" Separator="/"         
          />
        </td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
				<td height="22" align="right">
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="buttonMedium" CausesValidation="true" OnClick ="doInsert"
                TabIndex="50" OnClientClick='return checkConfirm();' /></td>
		<td class="hSpace">&nbsp;</td>		
	    <td class="hSpace"> <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonMedium" CausesValidation="false"
                             TabIndex="51" OnClick="btnClear_Click" /></td>
	</tr>

	<tr>
		<td class="header2" colspan="15">
		  <table border="0" cellpadding="0" cellspacing="0" width="100%">
			<tr>
			  <td class="hSpace">&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			  <td class="header2" style="border:none;">
                  DAILY (RM)&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			</tr>
		  </table>
		</td>
	</tr>

	<tr>
		<td class="normalLabel">CASHIER</td>
		<td align="right" class="normalLabel">Shift 1</td>
		<td nowrap><asp:TextBox ID="txtCashierShift1" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="3"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
        <td align="right" class="normalLabel">Shift 2</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtCashierShift2" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="4"></asp:TextBox></td>
        <td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel">Shift 3</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtCashierShift3" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="5"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel"><b>Cashier Total</b></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtCashierTotal" enabled="false" runat="server" CssClass="textBoxSmallDisabled" MaxLength="20" TabIndex="6"></asp:TextBox></td>
	</tr>

    <tr class="vSpace">
		<td colspan="15">&nbsp;</td>
	</tr>
	
	<tr>
		<td class="normalLabel">VALET SERVICE</td>
		<td align="right" class="normalLabel">Shift 1</td>
		<td nowrap><asp:TextBox ID="txtValet1" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="7"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
        <td align="right" class="normalLabel">Shift 2</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtValet2" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="8"></asp:TextBox></td>
        <td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel">Shift 3</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtValet3" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="9"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel"><b>Valet Total</b></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtValetTotal" enabled="false" runat="server" CssClass="textBoxSmallDisabled" MaxLength="20" TabIndex="10"></asp:TextBox></td>
	</tr>
	
    <tr class="vSpace">
		<td colspan="15">&nbsp;</td>
	</tr>
	
	<tr>
		<td class="normalLabel">MOTORCYCLE</td>
		<td align="right" class="normalLabel">Shift 1</td>
		<td nowrap><asp:TextBox ID="txtMotorcycle1" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="11"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
        <td align="right" class="normalLabel">Shift 2</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtMotorcycle2" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="12"></asp:TextBox></td>
        <td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel">Shift 3</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtMotorcycle3" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="13"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel"><b>M/Cycle Total</b></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtMotorcyleTotal" enabled="false" runat="server" CssClass="textBoxSmallDisabled" MaxLength="20" TabIndex="14"></asp:TextBox></td>
	</tr>	
	
    <tr class="vSpace">
		<td colspan="15">&nbsp;</td>
	</tr>	
	
	<tr>
		<td class="normalLabel">AUTOPAY</td>
		<td align="right" class="normalLabel">APS 1</td>
		<td nowrap><asp:TextBox ID="txtAPS1" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="15"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
        <td align="right" class="normalLabel">APS 2</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtAPS2" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="16"></asp:TextBox></td>
        <td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel">APS 3</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtAPS3" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="17"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
	</tr>
	
	<tr>		
		<td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel">APS 4</td>
		<td nowrap><asp:TextBox ID="txtAPS4" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="18"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
        <td align="right" class="normalLabel">APS 5</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtAPS5" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="19"></asp:TextBox></td>
        <td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel">APS 6</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtAPS6" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="20"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel"><b>APS Total</b></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtAPSTotal" enabled="false" runat="server" CssClass="textBoxSmallDisabled" MaxLength="20" TabIndex="21"></asp:TextBox></td>
	</tr>	
	
	<tr>
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
		<td align="right" class="normalLabel"><b>Total Daily</b></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtTotalDaily" enabled="false" runat="server" CssClass="textBoxSmallDisabled" MaxLength="20" TabIndex="22"></asp:TextBox></td>
	</tr>	

	<tr>
		<td class="header2" colspan="15">
		  <table border="0" cellpadding="0" cellspacing="0" width="100%">
			<tr>
			  <td class="hSpace">&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			  <td class="header2" style="border:none;">
                  SEASON (RM)&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			</tr>
		  </table>
		</td>
	</tr>


	<tr>
		<td class="normalLabel">CAR</td>
		<td align="right" class="normalLabel">Cash</td>
		<td nowrap><asp:TextBox ID="txtCarCash" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="23"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
        <td align="right" class="normalLabel">Cheque</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtCarChq" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="24"></asp:TextBox></td>
        <td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel">Credit Card</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtCarCreditCard" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="25"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
	</tr>
	
    <tr class="vSpace">
		<td colspan="15">&nbsp;</td>
	</tr>	
	
	<tr>
		<td class="normalLabel">MOTORCYCLE</td>
		<td align="right" class="normalLabel">Cash</td>
		<td nowrap><asp:TextBox ID="txtMotorCash" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="26"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
        <td align="right" class="normalLabel">Cheque</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtMotorChq" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="27"></asp:TextBox></td>
        <td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel">Credit Card</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtMotorCreditCard" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="28"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel"><b>Total Season</b></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtSeasonTotal" enabled="false" runat="server" CssClass="textBoxSmallDisabled" MaxLength="19" TabIndex="29"></asp:TextBox></td>
	</tr>	
	
    <tr>
		<td class="header2" colspan="15">
		  <table border="0" cellpadding="0" cellspacing="0" width="100%">
			<tr>
			  <td class="hSpace">&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			  <td class="header2" style="border:none;">DEPOSIT (RM)&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			</tr>
		  </table>
		</td>
	</tr>
	
	<tr>		
		<td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel">Car</td>
		<td nowrap><asp:TextBox ID="txtDepositCar" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="30"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
        <td align="right" class="normalLabel">Motorcycle</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtDepositMotor" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="31"></asp:TextBox></td>
        <td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel">Others</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtDepositOther" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="32"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel"><b>Total Deposit</b></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtTotalDeposit" enabled="false" runat="server" CssClass="textBoxSmallDisabled" MaxLength="20" TabIndex="33"></asp:TextBox></td>
	</tr>	

	


    <tr>
		<td class="header2" colspan="15">
		  <table border="0" cellpadding="0" cellspacing="0" width="100%">
			<tr>
			  <td class="hSpace">&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			  <td class="header2" style="border:none;">OTHERS (QTY)&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			</tr>
		  </table>
		</td>
	</tr>
	
	<tr>		
		<td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel">Clamp</td>
		<td nowrap><asp:TextBox ID="txtClamp" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="34"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
        <td align="right" class="normalLabel">Card/Disc</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtTransponder" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="35"></asp:TextBox></td>
        <td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel">Num.Plate</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtNoPlate" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="36"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
	</tr>	

	<tr>		
		<td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel">Temporary<br>Rental</td>
		<td nowrap><asp:TextBox ID="txtTemporaryRental" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="37"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
        <td align="right" class="normalLabel">Misc</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtMisc" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="38"></asp:TextBox></td>
        <td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel"><b>Others Total</b></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtOtherTotal" enabled="false" runat="server" CssClass="textBoxSmallDisabled" MaxLength="20" TabIndex="39"></asp:TextBox></td>
	</tr>	
 	
   <tr>
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
		<td align="right" class="normalLabel"><b>Total Collection</b></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtTotalCollection" enabled="false" runat="server" CssClass="textBoxSmallDisabled" MaxLength="20" TabIndex="40"></asp:TextBox></td>
	</tr>	

	<tr>
		<td class="header2" colspan="15">
		  <table border="0" cellpadding="0" cellspacing="0" width="100%">
			<tr>
			  <td class="hSpace">&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			  <td class="header2" style="border:none;">INFO (QTY)&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			</tr>
		  </table>
		</td>
	</tr>

    <tr>		
		<td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel">Early Bird</td>
		<td nowrap><asp:TextBox ID="txtEarlyBird" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="41"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
        <td align="right" class="normalLabel">Complimentary<br>Ticket</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtComplimentaryTicket" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="42"></asp:TextBox></td>
        <td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel">Boomgate - <br>Manual Raised</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtManualRaised" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="43"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
	</tr>
	
	<tr>		
		<td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel">Outstanding<br>Ticket</td>
		<td nowrap><asp:TextBox ID="txtOutstandingTicket" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="44"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
        <td align="right" class="normalLabel">Others</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtInfoOthers" runat="server" CssClass="textBoxSmall" MaxLength="20" TabIndex="45"></asp:TextBox></td>
        <td class="hSpace">&nbsp;</td>
        <td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td align="right" class="normalLabel"><b>Total Info</b></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:TextBox ID="txtInfoTotal" enabled="false" runat="server" CssClass="textBoxSmallDisabled" MaxLength="20" TabIndex="46"></asp:TextBox></td>
	</tr>

  
    <tr class="vSpace">
		<td colspan="15">&nbsp;</td>
	</tr>

            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe1" runat="server" TargetControlID="txtCashierShift1" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe2" runat="server" TargetControlID="txtCashierShift2" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe3" runat="server" TargetControlID="txtCashierShift3" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe4" runat="server" TargetControlID="txtValet1" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe5" runat="server" TargetControlID="txtValet2" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe6" runat="server" TargetControlID="txtValet3" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe7" runat="server" TargetControlID="txtMotorcycle1" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe8" runat="server" TargetControlID="txtMotorcycle2" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe9" runat="server" TargetControlID="txtMotorcycle3" ValidChars="1234567890." />            
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe10" runat="server" TargetControlID="txtAPS1" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe11" runat="server" TargetControlID="txtAPS2" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe12" runat="server" TargetControlID="txtAPS3" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe13" runat="server" TargetControlID="txtAPS4" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe14" runat="server" TargetControlID="txtAPS5" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe15" runat="server" TargetControlID="txtAPS6" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe16" runat="server" TargetControlID="txtCarCash" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe17" runat="server" TargetControlID="txtCarChq" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe18" runat="server" TargetControlID="txtCarCreditCard" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe19" runat="server" TargetControlID="txtMotorCash" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe20" runat="server" TargetControlID="txtMotorChq" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe21" runat="server" TargetControlID="txtMotorCreditCard" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe22" runat="server" TargetControlID="txtDepositCar" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe23" runat="server" TargetControlID="txtDepositMotor" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe24" runat="server" TargetControlID="txtDepositOther" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe25" runat="server" TargetControlID="txtClamp" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe26" runat="server" TargetControlID="txtTransponder" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe27" runat="server" TargetControlID="txtNoPlate" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe28" runat="server" TargetControlID="txtTemporaryRental" ValidChars="1234567890." />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe29" runat="server" TargetControlID="txtMisc" ValidChars="1234567890." />            
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe30" runat="server" TargetControlID="txtEarlyBird" ValidChars="1234567890" />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe31" runat="server" TargetControlID="txtComplimentaryTicket" ValidChars="1234567890" />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe32" runat="server" TargetControlID="txtManualRaised" ValidChars="1234567890" />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe33" runat="server" TargetControlID="txtOutstandingTicket" ValidChars="1234567890" />
            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe34" runat="server" TargetControlID="txtInfoOthers" ValidChars="1234567890" />

            
            
            <asp:RegularExpressionValidator ID="v1" runat="server" ControlToValidate="txtCashierShift1" Display="None" ErrorMessage="Invalid format. Cashier Shift 1 is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v2" runat="server" ControlToValidate="txtCashierShift2" Display="None" ErrorMessage="Invalid format. Cashier Shift 2 is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v3" runat="server" ControlToValidate="txtCashierShift3" Display="None" ErrorMessage="Invalid format. Cashier Shift 3 is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v4" runat="server" ControlToValidate="txtValet1" Display="None" ErrorMessage="Invalid format. Valet Shift 1 is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v5" runat="server" ControlToValidate="txtValet2" Display="None" ErrorMessage="Invalid format. Valet Shift 2 is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v6" runat="server" ControlToValidate="txtValet3" Display="None" ErrorMessage="Invalid format. Valet Shift 3 is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v7" runat="server" ControlToValidate="txtMotorcycle1" Display="None" ErrorMessage="Invalid format. Motorcycle Shift 1 is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v8" runat="server" ControlToValidate="txtMotorcycle2" Display="None" ErrorMessage="Invalid format. Motorcycle Shift 2 is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v9" runat="server" ControlToValidate="txtMotorcycle3" Display="None" ErrorMessage="Invalid format. Motorcycle Shift 3 is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v10" runat="server" ControlToValidate="txtAPS1" Display="None" ErrorMessage="Invalid format. APS 1 is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v11" runat="server" ControlToValidate="txtAPS2" Display="None" ErrorMessage="Invalid format. APS 2 is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v12" runat="server" ControlToValidate="txtAPS3" Display="None" ErrorMessage="Invalid format. APS 3 is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v13" runat="server" ControlToValidate="txtAPS4" Display="None" ErrorMessage="Invalid format. APS 4 is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v14" runat="server" ControlToValidate="txtAPS5" Display="None" ErrorMessage="Invalid format. APS 5 is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v15" runat="server" ControlToValidate="txtAPS6" Display="None" ErrorMessage="Invalid format. APS 6 is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v16" runat="server" ControlToValidate="txtMotorCash" Display="None" ErrorMessage="Invalid format. Motor Cash is a numeric field." ValidationExpression="\^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v17" runat="server" ControlToValidate="txtMotorChq" Display="None" ErrorMessage="Invalid format. Motor Cheque is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v18" runat="server" ControlToValidate="txtMotorCreditCard" Display="None" ErrorMessage="Invalid format. Motor Credit Card is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v19" runat="server" ControlToValidate="txtCarCash" Display="None" ErrorMessage="Invalid format. Car Cash is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v20" runat="server" ControlToValidate="txtCarChq" Display="None" ErrorMessage="Invalid format. Car Cheque is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v21" runat="server" ControlToValidate="txtCarCreditCard" Display="None" ErrorMessage="Invalid format. Car Credit Card is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v22" runat="server" ControlToValidate="txtClamp" Display="None" ErrorMessage="Invalid format. Clamp is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v23" runat="server" ControlToValidate="txtTransponder" Display="None" ErrorMessage="Invalid format. Transponder is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v24" runat="server" ControlToValidate="txtNoPlate" Display="None" ErrorMessage="Invalid format. Number Plate is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v25" runat="server" ControlToValidate="txtMisc" Display="None" ErrorMessage="Invalid format. Misc is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v26" runat="server" ControlToValidate="txtTemporaryRental" Display="None" ErrorMessage="Invalid format. Temporary Rental is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v27" runat="server" ControlToValidate="txtDepositCar" Display="None" ErrorMessage="Invalid format. Deposit Car is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v28" runat="server" ControlToValidate="txtDepositMotor" Display="None" ErrorMessage="Invalid format. Deposit Motorcycle is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v29" runat="server" ControlToValidate="txtDepositOther" Display="None" ErrorMessage="Invalid format. Deposit Other is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([.][0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([.][0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v30" runat="server" ControlToValidate="txtEarlyBird" Display="None" ErrorMessage="Invalid format. Early Bird is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v31" runat="server" ControlToValidate="txtComplimentaryTicket" Display="None" ErrorMessage="Invalid format. Complimentary is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v32" runat="server" ControlToValidate="txtManualRaised" Display="None" ErrorMessage="Invalid format. Manual Raised is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v33" runat="server" ControlToValidate="txtOutstandingTicket" Display="None" ErrorMessage="Invalid format. Outstanding Ticket is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="v34" runat="server" ControlToValidate="txtInfoOthers" Display="None" ErrorMessage="Invalid format. Info Others is a numeric field." ValidationExpression="^[$]?([0-9][0-9]?([,][0-9]{3}){0,4}([0-9]{0,4})?)$|^[$]?([0-9]{1,14})?([0-9]{1,4})$|^[$]?[0-9]{1,14}$"></asp:RegularExpressionValidator>
            
            
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
