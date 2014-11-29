<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PassCardInfoHist.aspx.vb" Inherits="Enquiry_PassCardInfoHist" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral,      PublicKeyToken=31bf3856ad364e35"      Namespace="System.Web.UI" TagPrefix="asp" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Pass Card History</title>
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
                    var gridView = $get('<%= gvPassHist.ClientID %>');

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
                                       
		    </script>		    

<body>
    <form id="Form1" runat = "server" onsubmit="return true;">

    <asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" runat="server"></asp:ScriptManager> 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">                         
    <ContentTemplate> 

        <table border="0" cellpadding="0" cellspacing="0" width="100%">

         

   	    <tr class="vSpace">   	        
		    <td colspan="15">&nbsp;</td>		    
	    </tr>
	    
	<tr class="vSpace">
        <td class="normalLabel">Pass Card</td>
        <td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtPassCard" runat="server" CssClass="textBoxMediumDisabled" MaxLength="50" TabIndex="1"></asp:TextBox></td>
        <td class="hSpace">&nbsp;</td>
      	<td class="normalLabel">Item Name</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtItemType" runat="server" CssClass="textBoxMediumDisabled" MaxLength="50" TabIndex="2"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Total Months In Used</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtTotal" runat="server" CssClass="textBoxSmallDisabled" MaxLength="50" TabIndex="3"></asp:TextBox></td>
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
            <asp:Button ID="Button1" runat="server" Text="Close" CssClass="buttonMedium" CausesValidation="false"
                TabIndex="33" OnClientClick="javascript:window.close();"/>               
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
                    			    

                            <%  If gvPassHist.Rows.Count > 0 Then%>
                            
                            <tr>
                                <td bgcolor="white">
                                    <asp:GridView ID="gvPassHist" runat="server" AllowPaging="True" AllowSorting = "True" 
                                        AutoGenerateColumns="False" CellPadding="0" Width="100%"
                                        BorderWidth="1px" HorizontalAlign="Left" CellSpacing="1" DataKeyNames="PASSCARDMSTRID,DEBTORID,STARTDATE,ENDDATE,DURATION" 
                                        DataSourceID="dsPassHist">
                                        <Columns>                                                                                        
                                            <asp:BoundField DataField="DEBTOR" HeaderText="Debtor" SortExpression="DEBTOR"
                                                NullDisplayText="N/A" />
                                            <asp:BoundField DataField="STARTDATE" HeaderText="Start Date" SortExpression="STARTDATE"
                                                NullDisplayText="N/A" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="ENDDATE" HeaderText="End Date" SortExpression="ENDDATE"
                                                NullDisplayText="N/A" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="DURATION" HeaderText="Duration(Months)" SortExpression="DURATION"
                                                NullDisplayText="N/A" />                                            
                                        </Columns>
                                        <RowStyle CssClass="grid_row1" />
                                        <SelectedRowStyle CssClass="tb-highlight" />
                                        <HeaderStyle CssClass="grid_header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <AlternatingRowStyle CssClass="grid_row2" />
                                        <PagerStyle Font-Bold="True" Font-Underline="True" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="dsPassHist" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
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
