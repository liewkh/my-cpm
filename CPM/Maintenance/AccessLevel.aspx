<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AccessLevel.aspx.vb" Inherits="Maintenance_AccessLevel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


   <%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral,      PublicKeyToken=31bf3856ad364e35"      Namespace="System.Web.UI" TagPrefix="asp" %> 

    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml" >
    <head>
    <title>Location</title>
    <link rel="stylesheet" type="text/css" href="../include/stylesheet.css" />
    <script type="text/javascript" language="JavaScript" src='../include/effect.js'></script>
    <script type="text/javascript" language="JavaScript" src='../include/javascript.js'></script>


    </head>

    <script language="javascript" type="text/javascript">

    window.onscroll=move;

     function onUpdating(){
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

 
                
		    </script>		    
    		
    <body>
    <form id="Form1" runat = "server" onsubmit="return true;">

    <asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" runat="server"></asp:ScriptManager> 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">                         
    <ContentTemplate> 

        <table border="0" cellpadding="0" cellspacing="0" width="100%">

         

   	    <tr class="vSpace">   	        
   	        <td><asp:HiddenField ID="hidMatSearchLevel" runat="server" /></td>   	        
		    <td colspan="14">&nbsp;</td>
		    
	    </tr>
	    
	    	    <tr class="vSpace">
		                    <td colspan="10"><asp:Label ID="lblmsg" runat="server" CssClass="errorMsg"></asp:Label></td>		        		        
		        	                        
		                    
	            </tr>
	            

   	    <tr class="vSpace">   	        
   	        <td class="normalLabel" style="width: 134px; height: 10px;">Level&nbsp;<asp:DropDownList ID="ddLvl" runat="server" TabIndex="14" DataSourceID="dsLvl" DataTextField="codedesc" DataValueField="codeabbr" CssClass="dropdownMedium"  AutoPostBack="true" OnSelectedIndexChanged="ddLvl_SelectedIndexChanged"></asp:DropDownList><asp:SqlDataSource ID="dsLvl" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            SelectCommand="select codeabbr,codedesc,0 as seq from codemstr where codecat='LVL' union all select codeabbr,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,codedesc">
                                        </asp:SqlDataSource></td>
		    <td colspan="14">&nbsp;</td>		    
	    </tr>
	    
           
            
	            
         <tr>
                                <td valign="top">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#FDF6E9">
                                        <tr class="tbHeader">
                                            <td align="center">
                                                Selected Item
                                            </td>
                                            <td align="center">
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                Available Item
                                            </td>
                                        </tr>
                                        <tr align="center">
                                            <td width="47%">
                                                <asp:ListBox ID="lstSelection" runat="server" Rows="18" Width="300" DataSourceID="dsSelection"
                                                    BorderStyle="Outset" SelectionMode="Multiple"></asp:ListBox>
                                            </td>
                                            <td width="6%">
                                                <asp:Button ID="btnAddItem" runat="server" CssClass="button" Text="<" Width="25px" TabIndex="5"
                                                    CausesValidation="False"></asp:Button><br>
                                                <asp:Button ID="btnRemoveItem" runat="server" CssClass="button" Text=">" Width="25px" TabIndex="6"
                                                    CausesValidation="False"></asp:Button><br>
                                            </td>
                                            <td width="47%">
                                                <asp:ListBox ID="lstLocation" runat="server" Rows="18" Width="300" DataSourceID="dsLocation"
                                                    BorderStyle="Outset" SelectionMode="Multiple"></asp:ListBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" class="tb-button-row">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tb-button-row" align="right" colspan="4">
                                                <asp:Button ID="btnClose" runat="server" CssClass="button" TabIndex="8" Text="Close" OnClientClick="javascript:window.close();" />
                                            </td>
                                        </tr>
                                        <asp:SqlDataSource ID="dsLocation" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"></asp:SqlDataSource>                                            
                                        <asp:SqlDataSource ID="dsSelection" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"></asp:SqlDataSource>
                                    </table>
                                </td>
                            </tr>
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

