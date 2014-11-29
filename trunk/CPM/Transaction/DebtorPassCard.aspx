<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DebtorPassCard.aspx.vb" Inherits="Maintenance_DebtorPassCard" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar"%>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral,      PublicKeyToken=31bf3856ad364e35"      Namespace="System.Web.UI" TagPrefix="asp" %> 

    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

    <html>
    <head>
    <title>Debtor Pass Card</title>
    <link rel="stylesheet" type="text/css" href="../include/stylesheet.css" />
    <script type="text/javascript" language="JavaScript" src='../include/effect.js'></script>
    <script type="text/javascript" language="JavaScript" src='../include/javascript.js'></script>

     <!-- Start Of ModalBox-->
     <script type="text/javascript" src="../include/ModalBox/prototype.js"></script>
     <script type="text/javascript" src="../include/ModalBox/scriptaculous.js?load=effects"></script>
     <script type="text/javascript" src="../include/ModalBox/modalbox.js"></script>
     <link rel="stylesheet" href="../include/ModalBox/modalbox.css" type="text/css" media="screen" />
     <!-- End Of ModalBox-->
    <base target="_self" />
 
    </head>

    <script language="javascript" type="text/javascript">

    window.onscroll=move;
    
     function onUpdating(){
                    // get the update progress div
                    var pnlPopup = $get('<%= pnlPopup.ClientID %>');

                    //  get the gridview element
                    var gridView = $get('<%= gvPassBay.ClientID %>');

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
		    //alert(page); 
			window.showModalDialog(page,'File','dialogWidth=780px,dialogHeight=600px');__doPostBack('lnkProcess','');}
			
			//__doPostBack('lnkRefreshPassBay','');}
		else if (size=='L'){
			window.showModalDialog(page,'File','dialogWidth=1000px,dialogHeight=800px');}
			//__doPostBack('lnkRefreshPassBay','');}
		return false;
	}

 function doProcess() {
 var p=document.getElementById("<%=ddMonth.ClientID%>");
 var q=document.getElementById("<%=hidMonth.ClientID%>");
 var r=document.getElementById("<%=hidYear.ClientID%>");
 var s=document.getElementById("<%=ddYear.ClientID%>");
 
 if (p.options[p.selectedIndex].value=="")
    { alert('Please select the month to generate invoice!'); 
      return false;       
    }
 if (s.options[s.selectedIndex].value=="")
    { alert('Please select a year to generate invoice!'); 
      return false;       
    }
       
q.value = p.options[p.selectedIndex].value;
r.value = s.options[s.selectedIndex].value;

__doPostBack('lnkProcess','');
document.forms(0).submit; 

}


 function checkToPopUpViewer(){ 
                             if(document.forms(0).hdPreview.value == "1"){           
            showCrystalReportDialog("../Report/CrystalReportView.aspx", 900, 1000);
            document.forms(0).hdPreview.value = "";
        }
    }
    
                
		    </script>		    
    		
    <body>
    <form id="Form1" runat = "server" onsubmit="return true;">

    <asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" runat="server"></asp:ScriptManager> 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">                         
    <ContentTemplate> 

        <table border="0" cellpadding="0" cellspacing="0" width="100%">

         
  
	    	    <tr class="vSpace">
		                    <td colspan="13">
		                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errorMsg" DisplayMode="List" />		                                    
            <asp:RequiredFieldValidator ID="CommencementDate" runat="server" ControlToValidate="txtCommencementDate" Display="None" ErrorMessage="Commencement Date is a required field."></asp:RequiredFieldValidator>                               	        
            <asp:RequiredFieldValidator ID="SeasonType" runat="server" ControlToValidate="ddSeasonType" Display="None" ErrorMessage="Season Type is a required field."></asp:RequiredFieldValidator>            
            <asp:RequiredFieldValidator ID="CarRegNo" runat="server" ControlToValidate="txtCarRegistrationNo" Display="None" ErrorMessage="Car Registration No is a required field."></asp:RequiredFieldValidator>
            <asp:HiddenField ID="hidConfirm" runat="server"/>
            <asp:HiddenField ID="hidMonth" runat="server"/>
            <asp:HiddenField ID="hidYear" runat="server"/>
            <asp:HiddenField ID="hdPreview" runat="server"/>
   	        <asp:LinkButton ID="lnkProcess" Visible="false"  runat="server" OnClick="lnkProcess_Click"></asp:LinkButton></td>		        		        
		        	        <td class="hSpace" height="22">&nbsp;</td>
		                    <td class="hSpace" height="22" style="width: 13px">&nbsp;</td>
	            </tr>
	            
	    
	            <tr class="vSpace">
		                    <td colspan="13"><asp:Label ID="lblmsg" runat="server" CssClass="errorMsg"></asp:Label></td>		        		        
		        	                        <td class="hSpace" height="22">&nbsp;</td>
		                    <td class="hSpace" height="22" style="width: 13px">&nbsp;</td>
	            </tr>	 
	            
	            <tr>
	              		<div id="longTextContent" style="display: none">
			<p>Please select the month and year for the invoice.</p>			
                                           <asp:DropDownList ID="ddMonth" runat="server">
                                                <asp:ListItem Value="">Please Choose One</asp:ListItem>
                                                <asp:ListItem Value="01">January</asp:ListItem>
                                                <asp:ListItem Value="02">February</asp:ListItem>
                                                <asp:ListItem Value="03">March</asp:ListItem>
                                                <asp:ListItem Value="04">April</asp:ListItem>
                                                <asp:ListItem Value="05">May</asp:ListItem>
                                                <asp:ListItem Value="06">June</asp:ListItem>
                                                <asp:ListItem Value="07">July</asp:ListItem>
                                                <asp:ListItem Value="08">August</asp:ListItem>
                                                <asp:ListItem Value="09">September</asp:ListItem>
                                                <asp:ListItem Value="10">October</asp:ListItem>
                                                <asp:ListItem Value="11">November</asp:ListItem>
                                                <asp:ListItem Value="12">December</asp:ListItem>
                                            </asp:DropDownList>&nbsp;
            <p>Year &nbsp; <asp:DropDownList ID="ddYear" runat="server">
                                            </asp:DropDownList>&nbsp;	                                            </p>
            
                                            <asp:Button ID="btnInvoiceTest" runat="server" Text="OK" CssClass="buttonMedium" CausesValidation="false"
                                                   TabIndex="50" OnClientClick ="doProcess();return true;"/>
		</div>
	            </tr>	               
	    
	<tr class="vSpace">
        <td class="normalLabel">Location</td>
        <td class="hSpace">&nbsp;</td>
        <td nowrap><asp:DropDownList ID="ddLocation" enabled="false" runat="server" TabIndex="1" DataSourceID="dsLocation" DataTextField="locationName" DataValueField="locationinfoid" CssClass="dropdownLarge"></asp:DropDownList><asp:SqlDataSource ID="dsLocation" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
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
		<td class="normalLabel">Debtor Name</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtDebtorName" runat="server" CssClass="textBoxMediumDisabled" MaxLength="200" TabIndex="2"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">User's Name</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtUserName" runat="server" CssClass="textBoxMedium" MaxLength="200" TabIndex="3"></asp:TextBox></td>
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
		<td class="normalLabel">Commencement Date <font color="#FF0000">*</font></td>
        <td class="hSpace">&nbsp;</td>
        <td class="normalLabel">
		<asp:TextBox ID="txtCommencementDate" runat="server" CssClass="dateBox" MaxLength="12" TabIndex="1"></asp:TextBox>          
        <rjs:PopCalendar id="popCalendar1" runat="server"
           Control="txtCommencementDate"
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
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
	</tr>	

	<tr class="vSpace">
        <td class="normalLabel">Season Type <font color="#FF0000">*</font></td>
        <td class="hSpace">&nbsp;</td>
        <td nowrap><asp:DropDownList ID="ddSeasonType" runat="server" AutoPostBack="true"  TabIndex="2" DataSourceID="dsSeasonType" DataTextField="seasonTypeDesc" DataValueField="seasontypemstrid" CssClass="dropdownLarge" OnSelectedIndexChanged="ddSeasonType_SelectedIndexChanged"></asp:DropDownList><asp:SqlDataSource ID="dsSeasonType" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>">
                                        </asp:SqlDataSource>
		</td>
        <td class="hSpace">&nbsp;</td>
      	<td class="normalLabel">Vehicle Type</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtVehicleType" Enabled ="false" runat="server" CssClass="textBoxSmallDisabled" MaxLength="50" TabIndex="3"></asp:TextBox></td>
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
        <td class="normalLabel">Car Registration No <font color="#FF0000">*</font></td>
        <td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtCarRegistrationNo" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="4"></asp:TextBox></td>
        <td class="hSpace">&nbsp;</td>
      	<td class="normalLabel">Make</td>
		<td class="hSpace">&nbsp;</td>
        <td nowrap><asp:DropDownList ID="ddMake" runat="server" TabIndex="5" DataSourceID="dsMake" DataTextField="codedesc" DataValueField="codeabbr" CssClass="dropdownMedium"></asp:DropDownList>
                                        <asp:SqlDataSource ID="dsMake" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                           SelectCommand="SELECT CODEABBR,CODEDESC,0 AS SEQ FROM CODEMSTR WHERE CODECAT = 'MAKE' AND ACTIVE = 'Y' UNION ALL SELECT CODEABBR,CODEDESC,SEQ FROM CODEMSTR WHERE CODECAT = 'DEFAULT' ORDER BY SEQ,CODEDESC">
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

	<tr class="vSpace">
        <td class="normalLabel">Model</td>
        <td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtModel" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="6"></asp:TextBox></td>
        <td class="hSpace">&nbsp;</td>
      	<td class="normalLabel">Deposit (RM)</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtDeposit" Enabled ="false" runat="server" CssClass="textBoxSmallDisabled" MaxLength="50" TabIndex="7"></asp:TextBox></td>
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
      	<td class="normalLabel">Season Amount (RM)</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtSeasonAmount" Enabled ="false" runat="server" CssClass="textBoxSmallDisabled" MaxLength="50" TabIndex="7"></asp:TextBox></td>
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
	    <td class="hSpace">&nbsp;</td>
	    <td class="hSpace">&nbsp;</td>
        <td nowrap><asp:DropDownList ID="ddPass" runat="server" TabIndex="11" DataSourceID="dsPass" DataTextField="serialNo" DataValueField="passcardmstrid" CssClass="dropdownMedium"></asp:DropDownList><asp:SqlDataSource ID="dsPass" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>">
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
		<td colspan="15">&nbsp;</td>
	</tr>
	
    <tr>
        <td class="hSpace" height="22" style="width: 92px">&nbsp;</td>
        <td class="hSpace" height="22" style="width: 13px">&nbsp;</td>
        <td height="22" colspan="12" align="right">
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="buttonMedium" CausesValidation="true"
                TabIndex="30" OnClick="btnAdd_Click" />
            <asp:Button ID="btnUpdate" runat="server" Text="Update" Enabled ="false"  CssClass="buttonMedium" CausesValidation="false"
                TabIndex="31" OnClick="btnUpdate_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonMedium" CausesValidation="false"
                TabIndex="32" OnClick="btnClear_Click" />
            <asp:Button ID="btnInvoice" runat="server" Text="Generate Invoice" Enabled="false" CssClass="buttonLarge" CausesValidation="false"
                TabIndex="33" title="Confirm" onclientclick="Modalbox.show($('longTextContent'), {title: this.title, height: 200, autoFocusing: false, closeValue: 'Close me' }); return false;"/>                                               
            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="buttonMedium" CausesValidation="false"
                TabIndex="34" OnClientClick="javascript:window.close();"/>               
        </td>
        <td class="hSpace" height="22">
            &nbsp;</td>
        <td class="hSpace" height="22">
            &nbsp;</td>
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
                    			    

                            <%  If gvPassBay.Rows.Count > 0 Then%>
                            
                            <tr>
                                <td bgcolor="white">
                                    <asp:GridView ID="gvPassBay" runat="server" AllowSorting = "True" 
                                        AutoGenerateColumns="False" CellPadding="0" Width="100%"
                                        BorderWidth="1px" HorizontalAlign="Left" CellSpacing="1" DataKeyNames="DEBTORPASSBAYID,DEBTORID,PASSCARDMSTRID,SERIALNO,SEASONTYPEMSTRID,COMMENCEMENTDATE,
                                        USERNAME,CARREGISTRATIONNO,VEHICLETYPE,MAKE,MAKEDESC,MODEL,DEPOSIT,AMOUNT,TYPES" DataSourceID="dsPassBay">
                                        <Columns>  
                                            <asp:TemplateField HeaderText="Select"> 
                                                 <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                 </ItemTemplate>
                                                 <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PASSCARDMSTRID" HeaderText="PASSCARDMSTRID" SortExpression="PASSCARDMSTRID"
                                                NullDisplayText="N/A" Visible="false" />                                                                                        
                                            <asp:BoundField DataField="DEBTORID" HeaderText="DEBTORID" SortExpression="DEBTORID"
                                                NullDisplayText="N/A" Visible="false" />         
                                            <asp:BoundField DataField="SERIALNO" HeaderText="Serial No" SortExpression="SERIALNO"
                                                NullDisplayText="N/A" />
                                            <asp:BoundField DataField="CARREGISTRATIONNO" HeaderText="Car Reg No" SortExpression="CARREGISTRATIONNO"
                                                NullDisplayText="N/A" />
                                            <asp:BoundField DataField="MAKEDESC" HeaderText="Make" SortExpression="MAKEDESC"
                                                NullDisplayText="N/A" />
                                            <asp:BoundField DataField="MODEL" HeaderText="Model" SortExpression="MODEL"
                                                NullDisplayText="N/A" />
                                            <asp:BoundField DataField="DEPOSIT" HeaderText="Deposit" SortExpression="DEPOSIT"
                                                NullDisplayText="N/A" />                                                
                                        </Columns>
                                        <RowStyle CssClass="grid_row1" />
                                        <SelectedRowStyle CssClass="tb-highlight" />
                                        <HeaderStyle CssClass="grid_header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <AlternatingRowStyle CssClass="grid_row2" />
                                        <PagerStyle Font-Bold="True" Font-Underline="True" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="dsPassBay" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
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
