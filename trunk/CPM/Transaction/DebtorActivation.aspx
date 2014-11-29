<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DebtorActivation.aspx.vb" Inherits="Maintenance_DebtorActivation" %>
<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar"%>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral,      PublicKeyToken=31bf3856ad364e35"      Namespace="System.Web.UI" TagPrefix="asp" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
<title>Application Form</title>
<link rel="stylesheet" type="text/css" href="../include/stylesheet.css" />
<script type="text/javascript" language="JavaScript" src='../include/effect.js'></script>
<script type="text/javascript" language="JavaScript" src='../include/javascript.js'></script>


     <!-- Start Of ModalBox-->
     <script type="text/javascript" src="../include/ModalBox/prototype.js"></script>
     <script type="text/javascript" src="../include/ModalBox/scriptaculous.js?load=effects"></script>
     <script type="text/javascript" src="../include/ModalBox/modalbox.js"></script>
     <link rel="stylesheet" href="../include/ModalBox/modalbox.css" type="text/css" media="screen" />
     <!-- End Of ModalBox-->
     
</head>

<script language="javascript" type="text/javascript">


window.onscroll=move;

function onUpdating(){
                // get the update progress div
                var pnlPopup = $get('<%= pnlPopup.ClientID %>');

                //  get the gridview element
                var gridView = $get('<%= gvDebtor.ClientID %>');

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
			window.showModalDialog(page,'File','dialogWidth=1000px,dialogHeight=1000px');}
			//__doPostBack('lnkRefreshPassBay','');}
		return false;
	}
	
	     function popTheWin(URL) {
	        showDialogScrollable(URL, 800, 640);
           //__doPostBack('lnkRefreshPassBay','');           

        }
        
                                 
		</script>


<body>
<form id="Form1" action='' runat ="server">
	
<asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" runat="server"></asp:ScriptManager> 
<asp:UpdatePanel ID="UpdatePanel1" runat="server">                         
<ContentTemplate> 
               
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td class="header1">
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td class="hSpace">&nbsp;</td>
					<td class="header1" style="border:none;">Debtor</td>
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
			  <td class="header2" style="border:none;">&nbsp; Debtor Activation</td>
			  <td class="hSpace">&nbsp;</td>
			</tr>
		  </table>
		  <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errorMsg" DisplayMode="List" />		                                    
               <asp:RequiredFieldValidator ID="Location" runat="server" ControlToValidate="ddLocation" Display="None" ErrorMessage="Location is a required field."></asp:RequiredFieldValidator>                              
               <asp:HiddenField ID="hidDebtorIds" runat="server"/>
               <asp:HiddenField ID="hidLocationId" runat="server"/>               
		</td>
	</tr>

	            <tr class="vSpace">
		                    <td colspan="13"><asp:Label ID="lblmsg" runat="server" CssClass="errorMsg"></asp:Label></td>		        		        
		        	                        <td class="hSpace" height="22">&nbsp;</td>
		                    <td class="hSpace" height="22" style="width: 13px">&nbsp;</td>
	            </tr>	            

	<tr>
		<td class="normalLabel" style="width: 134px">[Category] <font color="#FF0000">*</font></td>
        <td class="hSpace">&nbsp;</td>
        <td nowrap><asp:RadioButton ID="rbIndividual" runat="server" GroupName="Category" TabIndex="1" Text="Individual" AutoPostBack="true"/>&nbsp;
                   <asp:RadioButton ID="rbCompany" runat="server" GroupName="Category" TabIndex="2" Text="Company" Checked="True" AutoPostBack="true"/></td>
		<td class="hSpace">&nbsp;</td>
      	<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">
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
        <td class="normalLabel">[Location] <font color="#FF0000">*</font></td>        
        <td class="hSpace">&nbsp;</td>
        <td nowrap><asp:DropDownList ID="ddLocation" runat="server" TabIndex="3" DataSourceID="dsLocation" DataTextField="locationName" DataValueField="locationinfoid" CssClass="dropdownLarge"></asp:DropDownList><asp:SqlDataSource ID="dsLocation" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>">
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

<div id="divIndividual" runat="server" visible="false">
    <tr>
		<td class="normalLabel" style="width: 134px"><u>Individual</u></td>
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
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
	</tr>
	

    <tr>		
		<td class="normalLabel" style="width: 134px">[Name(as in I.C)] <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtName" runat="server" CssClass="textBoxMedium" MaxLength="200" TabIndex="4"></asp:TextBox></td>
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
</div>
	


<div id="divCompany" runat="server" visible="true">
    <tr>
		<td class="normalLabel" style="width: 134px"><u>Company</u></td>
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
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>		
	</tr>

    <tr>
		<td class="normalLabel" style="width: 134px">[Company Name] <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtCompanyName" runat="server" CssClass="textBoxMedium" MaxLength="200" TabIndex="15"></asp:TextBox></td>
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
	   

</div>	

   
	
    <tr>
		<td class="normalLabel">Status</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:DropDownList ID="ddStatus" runat="server" TabIndex="29" DataSourceID="dsStatus" DataTextField="codedesc" DataValueField="codeabbr" CssClass="dropdownMedium"></asp:DropDownList><asp:SqlDataSource ID="dsStatus" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            SelectCommand="select codeabbr,codedesc,0 as seq from codemstr where codecat='DS' and active = 'Y' union all select codeabbr,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,codedesc">
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
		<td class="normalLabel" style="width: 134px">Remark <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtRemark" runat="server" CssClass="textBoxLarge" MaxLength="1000" TabIndex="30"></asp:TextBox></td>
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
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonMedium" CausesValidation="false"
                TabIndex="40" OnClick="btnSearch_Click" />
            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="buttonMedium" CausesValidation="true"
                TabIndex="42" OnClick="btnUpdate_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonMedium" CausesValidation="false"
                TabIndex="43" OnClick="btnClear_Click" />                                
        </td>
        <td class="hSpace" height="22">
            &nbsp;</td>
        <td class="hSpace" height="22">
            &nbsp;</td>           		
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
                			    

                        <%  If gvDebtor.Rows.Count > 0 Then%>
                        
                        <tr>
                            <td bgcolor="white">
                                <asp:GridView ID="gvDebtor" runat="server" AllowPaging="True" AllowSorting = "True" 
                                    AutoGenerateColumns="False" CellPadding="0" Width="100%"
                                    BorderWidth="1px" HorizontalAlign="Left" CellSpacing="1" DataKeyNames="DEBTORID,CATEGORY,LOCATIONINFOID,DEBTOR,
                                    ICNO,TELNOOFFICE,TELNOMOBILE,TELNOHOME,EMPLOYERNAME,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS,POSTCODE,LOCATIONNAME,
                                    CONTACTPERSON,DESIGNATION,REMARK,LASTINVOICENO,LASTINVOICEDATE,LASTPAYMENT,STATE,
                                    BALANCE,STATUS,LUDT,LUB,INITIALHALFMONTH,INVOICINGFREQUENCY" DataSourceID="dsDebtor">
                                    <Columns>
                                        <asp:BoundField DataField="LOCATIONNAME" HeaderText="Location" SortExpression="LOCATIONNAME"
                                            NullDisplayText="N/A" />
                                        <asp:BoundField DataField="DEBTOR" HeaderText="Debtor" SortExpression="DEBTOR"
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
