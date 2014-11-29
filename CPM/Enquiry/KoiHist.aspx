<%@ Page Language="VB" AutoEventWireup="false" CodeFile="KoiHist.aspx.vb" Inherits="Enquiry_KoiHist" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral,      PublicKeyToken=31bf3856ad364e35"      Namespace="System.Web.UI" TagPrefix="asp" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Key Operation Indices</title>
    <link rel="stylesheet" type="text/css" href="../include/stylesheet.css" />
    <script type="text/javascript" language="JavaScript" src='../include/effect.js'></script>
    <script type="text/javascript" language="JavaScript" src='../include/javascript.js'></script>  
</head>

	    
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
        <td class="normalLabel">Location</td>
        <td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtLocation" runat="server" CssClass="textBoxMediumDisabled" MaxLength="50" TabIndex="1"></asp:TextBox></td>
        <td class="hSpace">&nbsp;</td>
      	<td class="normalLabel">Collection Date</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtDate" runat="server" CssClass="textBoxMediumDisabled" MaxLength="50" TabIndex="2"></asp:TextBox></td>
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
        <td class="hSpace" height="22" style="width: 92px">&nbsp;</td>
        <td class="hSpace" height="22" style="width: 13px">&nbsp;</td>
        <td height="22" colspan="12" align="right">
            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="buttonMedium" CausesValidation="false"
                TabIndex="33"/>               
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
		                        <td colspan="15">
			                        <!-- Spreadsheet Header -->
			                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    			    

                            <%  If gvKoiHist.Rows.Count > 0 Then%>   
                   
                            <tr>
                                <td bgcolor="white">
                                    <asp:GridView ID="gvKoiHist" runat="server" AllowPaging="True" AllowSorting = "True" 
                                        AutoGenerateColumns="False" CellPadding="0" Width="100%"
                                        BorderWidth="1px" HorizontalAlign="Left" CellSpacing="1" DataKeyNames="LOCATIONNAME,EARLYBIRD,COMPLIMENTARY,MANUALRAISED,OUTSTANDING,OTHERS,MonthToDateEB,
                                        MonthToDateComplimentary,MonthToDateManualRaised,MonthToDateOs,MonthToDateOts,LastMonthEB,LastMonthComplimentary,LastMonthManualRaised,LastMonthOs,LastMonthOts" 
                                        DataSourceID="dsKoiHist">
                                        <Columns>
                                            <asp:BoundField DataField="EARLYBIRD" HeaderText="EB" SortExpression="EARLYBIRD"
                                                NullDisplayText="N/A" DataFormatString="{0:###,###}" htmlencode="false"/>                                                                                        
                                            <asp:BoundField DataField="COMPLIMENTARY" HeaderText="CT" SortExpression="COMPLIMENTARY"
                                                NullDisplayText="N/A" DataFormatString="{0:###,###}" htmlencode="false"/>
                                            <asp:BoundField DataField="MANUALRAISED" HeaderText="BMR" SortExpression="MANUALRAISED"
                                                NullDisplayText="N/A" DataFormatString="{0:###,###}" htmlencode="false"/>
                                            <asp:BoundField DataField="OUTSTANDING" HeaderText="O/S" SortExpression="OUTSTANDING"
                                                NullDisplayText="N/A" DataFormatString="{0:###,###}" htmlencode="false"/>                                                
                                            <asp:BoundField DataField="OTHERS" HeaderText="OTS" SortExpression="OTHERS"
                                                NullDisplayText="N/A" DataFormatString="{0:###,###}" htmlencode="false"/>   
                                            <asp:BoundField DataField="MonthToDateEB" HeaderText="EB" SortExpression="MonthToDateEB"
                                                NullDisplayText="N/A" DataFormatString="{0:###,###}" htmlencode="false"/>
                                            <asp:BoundField DataField="MonthToDateComplimentary" HeaderText="CT" SortExpression="MonthToDateComplimentary"
                                                NullDisplayText="N/A" DataFormatString="{0:###,###}" htmlencode="false"/>
                                            <asp:BoundField DataField="MonthToDateManualRaised" HeaderText="BMR" SortExpression="MonthToDateManualRaised"
                                                NullDisplayText="N/A" DataFormatString="{0:###,###}" htmlencode="false"/>
                                            <asp:BoundField DataField="MonthToDateOs" HeaderText="O/S" SortExpression="MonthToDateOs"
                                                NullDisplayText="N/A" DataFormatString="{0:###,###}" htmlencode="false"/>
                                            <asp:BoundField DataField="MonthToDateOts" HeaderText="OTS" SortExpression="MonthToDateOts"
                                                NullDisplayText="N/A" DataFormatString="{0:###,###}" htmlencode="false"/>
                                            <asp:BoundField DataField="LastMonthEB" HeaderText="EB" SortExpression="LastMonthEB"
                                                NullDisplayText="N/A" DataFormatString="{0:###,###}" htmlencode="false"/>           
                                            <asp:BoundField DataField="LastMonthComplimentary" HeaderText="CT" SortExpression="LastMonthComplimentary"
                                                NullDisplayText="N/A" DataFormatString="{0:###,###}" htmlencode="false"/>
                                            <asp:BoundField DataField="LastMonthManualRaised" HeaderText="BMR" SortExpression="LastMonthManualRaised"
                                                NullDisplayText="N/A" DataFormatString="{0:###,###}" htmlencode="false"/>
                                            <asp:BoundField DataField="LastMonthOs" HeaderText="O/S" SortExpression="LastMonthOs"
                                                NullDisplayText="N/A" DataFormatString="{0:###,###}" htmlencode="false"/>
                                            <asp:BoundField DataField="LastMonthOts" HeaderText="OTS" SortExpression="LastMonthOts"
                                                NullDisplayText="N/A" DataFormatString="{0:###,###}" htmlencode="false"/>                                                                                                                                                                                        
                                        </Columns>
                                        <RowStyle CssClass="grid_row1" />
                                        <SelectedRowStyle CssClass="tb-highlight" />
                                        <HeaderStyle CssClass="grid_header" HorizontalAlign="Left" VerticalAlign="Middle" />                                        
                                        <AlternatingRowStyle CssClass="grid_row2" />
                                        <PagerStyle Font-Bold="True" Font-Underline="True" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="dsKoiHist" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
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
