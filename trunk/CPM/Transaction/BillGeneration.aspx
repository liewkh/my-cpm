<%@ Page Language="VB" AutoEventWireup="false" CodeFile="BillGeneration.aspx.vb" Inherits="Transaction_BillGeneration" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
<title>Bill Generation</title>
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
                        document.getElementById('<%=btnGenerate.ClientID%>').disabled = true;
                    }

                    function onUpdated() {
                        // get the update progress div
                        var pnlPopup = $get('<%= pnlPopup.ClientID %>');
                        // make it invisible
                        pnlPopup.style.display = 'none';
                        document.getElementById('<%=btnGenerate.ClientID%>').disabled = false;
                    }
                            
</script>

   
<body>
<form action='' runat="server">
<!--10 minutes -->
 <asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" runat="server" AsyncPostBackTimeOut="600">
        </asp:ScriptManager>
        
<script language="javascript" type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequest);
    
    
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(startRequest);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

    function startRequest( sender, e ) { 
      //disable search button during the AJAX call
      document.getElementById('<%=btnGenerate.ClientID%>').disabled = true;
      document.getElementById('<%=btnClear.ClientID%>').disabled = true;
    }

 
     function endRequest( sender, e ) { 
     //re-enable the search button once the AJAX call has completed
     document.getElementById('<%=btnGenerate.ClientID%>').disabled = false;
     document.getElementById('<%=btnClear.ClientID%>').disabled = false;
     }


    function InitializeRequest(sender, args) {
      if (prm.get_isInAsyncPostBack())
       {
          args.set_cancel(true);
       }
    }
    function AbortPostBack() {
      if (prm.get_isInAsyncPostBack()) {
           prm.abortPostBack();
      }        
    }
    
</script>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"  >
            <contenttemplate> 
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td class="header1">
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td class="hSpace">&nbsp;</td>
					<td class="header1" style="border:none;">Invoice</td>
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
			  <td class="header2" style="border:none;">&nbsp; Bill Generation</td>
			  <td class="hSpace">&nbsp;</td>
			</tr>
		  </table>
		   <asp:RequiredFieldValidator ID="Location" runat="server" ControlToValidate="ddLocation" Display="None" ErrorMessage="Location is a required field."></asp:RequiredFieldValidator>
           <asp:RequiredFieldValidator ID="Debtor" runat="server" ControlToValidate="ddDebtor" Display="None" ErrorMessage="Debtor is a required field."></asp:RequiredFieldValidator>
           <input id="hdPreview" type="hidden" runat="server" />
		</td>
	</tr>



    <tr>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:Label ID="lblMsg" runat="server" CssClass="errorMsg"></asp:Label></td>
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
		<td class="normalLabel" style="width: 134px; height: 20px;">[Category] <font color="#FF0000">*</font></td>
        <td class="hSpace" style="height: 20px">&nbsp;</td>
        <td nowrap style="height: 20px"><asp:RadioButton ID="rbIndividual" runat="server" GroupName="Category" TabIndex="1" Text="Individual" AutoPostBack="true" OnCheckedChanged="rbIndividual_CheckedChanged"/>&nbsp;
                   <asp:RadioButton ID="rbCompany" runat="server" GroupName="Category" TabIndex="2" Text="Company" Checked="True" AutoPostBack="true" OnCheckedChanged="rbCompany_CheckedChanged"/></td>
		<td class="hSpace" style="height: 20px">&nbsp;</td>
      	<td class="hSpace" style="height: 20px">&nbsp;</td>
		<td class="hSpace" style="height: 20px">&nbsp;</td>
		<td class="normalLabel" style="height: 20px">
		<td class="hSpace" style="height: 20px">&nbsp;</td>
		<td class="hSpace" style="height: 20px">&nbsp;</td>
		<td class="hSpace" style="height: 20px">&nbsp;</td>
		<td class="hSpace" style="height: 20px">&nbsp;</td>
		<td class="hSpace" style="height: 20px">&nbsp;</td>
		<td class="hSpace" style="height: 20px">&nbsp;</td>
		<td class="hSpace" style="height: 20px">&nbsp;</td>
		<td class="hSpace" style="height: 20px">&nbsp;</td>
		<td class="hSpace" style="height: 20px">&nbsp;</td>
	</tr>
		
  <tr class="vSpace">
		<td class="normalLabel">Location <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:DropDownList ID="ddLocation" runat="server" TabIndex="4" DataSourceID="dsLocation" DataTextField="locationName" DataValueField="locationinfoid" AutoPostBack ="True" CssClass="dropdownLarge" OnSelectedIndexChanged="ddLocation_SelectedIndexChanged"></asp:DropDownList><asp:SqlDataSource ID="dsLocation" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"></asp:SqlDataSource>
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
		<td class="normalLabel">Debtor <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:DropDownList ID="ddDebtor" runat="server" TabIndex="5" DataSourceID="dsDebtor" DataTextField="Debtor" DataValueField="DebtorId" CssClass="dropdownLarge"></asp:DropDownList><asp:SqlDataSource ID="dsDebtor" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"></asp:SqlDataSource>
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
	</tr>

	<tr>
		<td class="normalLabel">Invoice Period <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
        <td nowrap>  <asp:DropDownList ID="ddMonth" runat="server">
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
		<td class="normalLabel">Year <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
        <td nowrap><asp:DropDownList ID="ddYear" runat="server" AutoPostBack="true" >
                                            </asp:DropDownList>&nbsp;
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
		<td class="normalLabel">Invoicing Frequency <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
        <td nowrap><asp:DropDownList ID="ddInvFreq" runat="server" TabIndex="28" DataSourceID="dsInvFreq" DataTextField="codedesc" DataValueField="codeabbr" CssClass="dropdownMedium"></asp:DropDownList><asp:SqlDataSource ID="dsInvFreq" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            SelectCommand="select codeabbr,codedesc,0 as seq from codemstr where codecat='IVF' and active = 'Y' union all select codeabbr,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,codedesc">
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
		<td class="normalLabel">Status</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:label runat="server" CssClass="errorMsg" id="lbl"></asp:label></td>        
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
      <td colspan='15' align='right'>
            <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="buttonMedium" CausesValidation="true"
                TabIndex="32" OnClick="btnGenerate_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonMedium" CausesValidation="false"
                TabIndex="34" OnClick="btnClear_Click"/>
    </td>
    <td class="hspace"></td>
  </tr>

	<tr class="vSpace">
		<td colspan="15">&nbsp;</td>
	</tr>

	</table>

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
