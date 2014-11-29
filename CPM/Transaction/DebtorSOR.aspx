<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DebtorSOR.aspx.vb" Inherits="Maintenance_DebtorSOR" %>
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
			window.showModalDialog(page,'File','dialogWidth=780px,dialogHeight=600px');}
			
			//__doPostBack('lnkRefreshPassBay','');}
		else if (size=='L'){
			window.showModalDialog(page,'File','dialogWidth=1000px,dialogHeight=800px');}
			//__doPostBack('lnkRefreshPassBay','');}
		return false;
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
               </tr>
	            
	    
	            <tr class="vSpace">
		                    <td colspan="13"><asp:Label ID="lblmsg" runat="server" CssClass="errorMsg"></asp:Label></td>		        		        
		        	                        <td class="hSpace" height="22">&nbsp;</td>
		                    <td class="hSpace" height="22" style="width: 13px">&nbsp;</td>
	            </tr>	 
	            
		            
 
	
    <tr class="vSpace">
		<td colspan="15">&nbsp;</td>
	</tr>
	
    <tr>
        <td class="hSpace" height="22" style="width: 92px">&nbsp;</td>
        <td class="hSpace" height="22" style="width: 13px">&nbsp;</td>
        <td height="22" colspan="12" align="right">            
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
                                        BorderWidth="1px" HorizontalAlign="Left" CellSpacing="1" DataKeyNames="SerialNo" DataSourceID="dsPassBay">
                                        <Columns>                                                                                                                                                                                                                  
                                            <asp:BoundField DataField="SERIALNO" HeaderText="Serial No" SortExpression="SERIALNO"
                                                NullDisplayText="N/A" />
                                            <asp:BoundField DataField="STARTMONTH" HeaderText="Start Month" SortExpression="STARTMONTH"
                                                NullDisplayText="N/A" DataFormatString="{0:MMMM-yyyy}"/>                                                                                                                                                    
                                            <asp:BoundField DataField="ENDMONTH" HeaderText="End Month" SortExpression="ENDMONTH"
                                                NullDisplayText="N/A" DataFormatString="{0:MMMM-yyyy}"/>
                                            <asp:BoundField DataField="REMARKS" HeaderText="Remarks" SortExpression="REMARKS"
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
