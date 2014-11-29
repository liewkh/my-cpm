    <%@ Page Language="VB" AutoEventWireup="false" CodeFile="SearchPassCard.aspx.vb" Inherits="Maintenance_SearchPassCard" %>
    <%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral,      PublicKeyToken=31bf3856ad364e35"      Namespace="System.Web.UI" TagPrefix="asp" %> 

    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

    <html>
    <head>
    <title>Search Pass Card</title>
    <link rel="stylesheet" type="text/css" href="../include/stylesheet.css">
    <script language="JavaScript" src='../include/effect.js'></script>
    <script language="JavaScript" src='../include/javascript.js'></script>

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
    var Value1 =0;
    var Value2 =0;

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
                
                     function checkToPopUpViewer(){
                             if(document.forms(0).hdPreview.value == "1"){           
            showCrystalReportDialog("../Report/CrystalReportView.aspx", 900, 1000);
            return true;
        }
        doCalculate();
    }                 
                       

function doProcess() { 
__doPostBack('lnkProcess','');
document.forms(0).submit; 

}

function doCalculate(){
  var frm = document.frmSearchPassCard;
  var rowId = "";
  var totalDeposit = 0;
  var total = 0;
  
  for(i=0; i<frm.length; i++){
  
     if(frm.elements[i].id.indexOf(rowId + "_chkSelect") != -1 )
        {         if(frm.elements[i].checked == true){
                     total = total + 1;     
                    //  aryTest = frm.elements[i].value.split('|');
                  //MonthlyRate | Deposit
                  //Value1 = aryTest[0];
                  //Value2 = aryTest[1];                  
                     }
               
          }
          
  
          
     if(frm.elements[i].id.indexOf(rowId + "_ddSeasonType") != -1 )
        {         totalDeposit = totalDeposit + parseFloat(frm.elements[i].value);
                                
               
        }          
    }
    
    if (total > 0)
       {  document.forms(0).txtDeposit.value = parseFloat(totalDeposit); }
}


function Modalpopupthingy(){
  //var el = document.getElementById('ddLocation');
  var frm = document.frmSearchPassCard;
  var rowId = "";
  var totalDeposit = 0;
  var total = 0;

    
     if(document.forms(0).txtInitialPayment.value == "")
                    { alert('Please fill in the initial payment.');
                      document.forms(0).txtInitialPayment.focus();
                      return false;                   
                     }
                     
      if (document.forms(0).txtInitialPayment.value != parseInt(document.forms(0).txtInitialPayment.value))
          {        alert(document.forms(0).txtInitialPayment.value + ' is not a whole number');
                   document.forms(0).txtInitialPayment.focus();
                   return false;
           }                                
          
          
  for(i=0; i<frm.length; i++){
  
     if(frm.elements[i].id.indexOf(rowId + "_chkSelect") != -1 )
        {         if(frm.elements[i].checked == true){
                     total = total + 1;                    
                     }
               
          }
          
     if(frm.elements[i].id.indexOf(rowId + "_ddSeasonType") != -1 )
        {         totalDeposit = totalDeposit + parseFloat(frm.elements[i].value);
               
        }          
    }
    
//el.blur();
//alert(totalDeposit);
Modalbox.show('<div class=\'warning\'><p>Are you sure to confirm this item?</p>' + total + '<input type=\'button\' value=\'Yes  \' onclick=\'doProcess()\' /> or <input type=\'button\' value=\'Cancel\' onclick=\'Modalbox.hide()\' /></div>',{title: this.title, width: 300});
return false;
}

 
                
		    </script>		    
    		
    <body leftmargin='0' topmargin='0' scroll='auto' onload ="checkToPopUpViewer();">
    <form action="" runat = "server" id="frmSearchPassCard" onsubmit="checkToPopUpViewer();">

    <asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" runat="server"></asp:ScriptManager> 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">                         
    <ContentTemplate> 

        <table border="0" cellpadding="0" cellspacing="0" width="100%">

         

   	    <tr class="vSpace">
   	        <td><asp:LinkButton ID="lnkProcess" Visible="false"  runat="server" OnClick="lnkProcess_Click"></asp:LinkButton></td>
   	        <td><asp:HiddenField ID="hidConfirm" runat="server"/></td>
   	        <td><input id="hdPreview" type="hidden" runat="server" /></td>
		    <td colspan="12">&nbsp;</td>
		    
	    </tr>
	    
	            <tr class="vSpace">
		                    <td colspan="13"><asp:Label ID="lblmsg" runat="server" CssClass="errorMsg"></asp:Label></td>		        		        
		        	                        <td class="hSpace" height="22">&nbsp;</td>
		                    <td class="hSpace" height="22" style="width: 13px">&nbsp;</td>
	            </tr>	    
	            
	<tr class="vSpace">
        <td class="normalLabel">Location</td>
        <td class="hSpace">&nbsp;</td>
    	 <td nowrap><asp:DropDownList ID="ddLocation" runat="server" TabIndex="1" DataSourceID="dsLocation" DataTextField="branchName" DataValueField="branchinfoid" CssClass="dropdownLarge" Enabled="False"></asp:DropDownList><asp:SqlDataSource ID="dsLocation" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            SelectCommand="select branchinfoid,branchName,0 as seq from branchinfo union all select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,branchname">
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
		<td class="normalLabel">Initial Payment <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtInitialPayment" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="3" OnTextChanged="txtInitialPayment_TextChanged"></asp:TextBox> Month(s)</td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Deposit (RM) <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtDeposit" Enabled="false" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="4"></asp:TextBox></td>
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
		<td class="normalLabel">Monthly Rate (RM) <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtMonthlyRate" enabled="false" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="3"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Auto Print Receipt? <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:CheckBox ID="chkPrint"  AutoPostBack ="true" Checked ="true"   TabIndex="3" runat="server"/></td>
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
		<td class="normalLabel">Total (RM)</font></td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtTotal" enabled="false" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="3"></asp:TextBox></td>
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
                                    <asp:GridView ID="gvPassBay" runat="server" AllowPaging="False" AllowSorting = "True"
                                        AutoGenerateColumns="False" CellPadding="0" Width="100%"
                                        BorderWidth="1px" HorizontalAlign="Left" CellSpacing="1" DataKeyNames="PASSCARDMSTRID,DEBTORID,SERIALNO,ITEMTYPEDESC,ITEMTYPE,
                                        SUPPLIER,PURCHASEDDATE,ACTIVE,BRANCHINFOID,REMARK,LUB,LUDT" DataSourceID="dsPassBay" Enabled="True" OnSelectedIndexChanged="gvPassBay_SelectedIndexChanged">
                                        <Columns>
                                               <asp:TemplateField HeaderText="Select"> 
                                                    <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </ItemTemplate>
                                                   <ItemStyle Width="5%" />
                                              </asp:TemplateField>                                             
                                            <asp:BoundField DataField="SERIALNO" HeaderText="Pass/Bay No" SortExpression="SERIALNO"
                                                NullDisplayText="N/A" />
                                              <asp:TemplateField HeaderText="Season Type"> 
                                                    <ItemTemplate>
                                                          <asp:DropDownList ID="ddSeasonType"   AutoPostBack ="true" runat="server" Visible="true" DataSourceID="dsSeasonType" DataTextField="seasontypedesc" DataValueField="deposit"></asp:DropDownList>
                                                    </ItemTemplate>
                                                   <ItemStyle Width="25%" />
                                              </asp:TemplateField>        
                                            <asp:BoundField DataField="PURCHASEDDATE" HeaderText="Purchased Date" SortExpression="PURCHASEDDATE"
                                                NullDisplayText="N/A" DataFormatString="{0:dd/MM/yyyy}"/>                                   
                                        </Columns>
                                        <RowStyle CssClass="grid_row1" />
                                        <SelectedRowStyle CssClass="tb-highlight" />
                                        <HeaderStyle CssClass="grid_header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <AlternatingRowStyle CssClass="grid_row2" />
                                        <PagerStyle Font-Bold="True" Font-Underline="True" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="dsPassBay" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                        ProviderName="System.Data.SqlClient"></asp:SqlDataSource>
                                    <asp:SqlDataSource ID="dsSeasonType" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
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
            <td height="22" colspan="14" align="right">
                <asp:Button ID="btnSelect" runat="server" Text="Select" CssClass="buttonMedium" CausesValidation="false" TabIndex="32" onclientclick="Modalpopupthingy();"/>
                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="buttonMedium" CausesValidation="false" TabIndex="33" OnClientClick="javascript:window.close();"/>                
            <li><a href="#" title="Modalbox auto-focusing disabled test" onclick="Modalbox.show($('longTextContent')+total, {title: this.title, height: 200, autoFocusing: false, closeValue: 'Close me' }); return false;">Modalbox auto-focusing disabled test</a></li>
            	  </td>
            <td class="hSpace" height="22">
                &nbsp;</td>
            <td class="hSpace" height="22">
                &nbsp;</td>
        </tr>

		<div id="longTextContent" style="display: none">
			<p>Lorem ipsum dolor sit amet, borum.</p>
			<% Response.Write(" sees them (which is flat).<BR>\r")%> 
			<script language="Javascript">
                      
             
</script>
		
			<input type="button" name="button" value="Close me" id="button" onclick="Modalbox.hide()" />
		</div>
		
		
		
        <tr>
		    <td colspan="15" height="17"></td>
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
