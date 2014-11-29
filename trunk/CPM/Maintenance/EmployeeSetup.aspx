<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EmployeeSetup.aspx.vb" Inherits="Maintenance_EmployeeSetup" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral,      PublicKeyToken=31bf3856ad364e35"      Namespace="System.Web.UI" TagPrefix="asp" %> 

<%@ Register Assembly="TextBoxEx" Namespace="TextBoxEx" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Employee Information</title>
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
                var gridView = $get('<%= gvEmployees.ClientID %>');

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
		else if (size=='M'){
		    if (document.getElementById("ddLocation").value == '0') 
		        {alert('Please Provide Location Info.');
		         return false;}
		    
		    window.showModalDialog(page,'File','dialogWidth=780px,dialogHeight=600px');
			__doPostBack('lnkRefreshMulLoc','');}
		else if (size=='L'){
			window.showModalDialog(page,'File','dialogWidth=1000px,dialogHeight=800px');}
		return false;
	}            
    
</script>
		
<body leftmargin='0' topmargin='0' scroll='auto'>
<form action='' runat="Server">
<asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" runat="server"></asp:ScriptManager> 
<asp:UpdatePanel ID="UpdatePanel1" runat="server">                         
<ContentTemplate> 

<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td class="header1">
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td class="hSpace">&nbsp;</td>
					<td class="header1" style="border:none;">User</td>
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
			  <td class="header2" style="border:none;">Information&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			</tr>
		  </table>
              <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errorMsg" DisplayMode="List" />		                                    
              <asp:RequiredFieldValidator ID="Location" runat="server" ControlToValidate="ddLocation" Display="None" ErrorMessage="Location is a required field."></asp:RequiredFieldValidator>               		  
              <asp:RequiredFieldValidator ID="EmployeeName" runat="server" ControlToValidate="txtEmployeeName" Display="None" ErrorMessage="Employee Name is a required field."></asp:RequiredFieldValidator>
              <asp:RequiredFieldValidator ID="LoginId" runat="server" ControlToValidate="txtLoginId" Display="None" ErrorMessage="User Login Id is a required field."></asp:RequiredFieldValidator>
              <asp:RequiredFieldValidator ID="Password" runat="server" ControlToValidate="txtPassword" Display="None" ErrorMessage="Password is a required field."></asp:RequiredFieldValidator>
              <asp:RequiredFieldValidator ID="AccessLevel" runat="server" ControlToValidate="ddAccessLevel" Display="None" ErrorMessage="Access Level is a required field."></asp:RequiredFieldValidator>              
              <asp:RequiredFieldValidator ID="Branch" runat="server" ControlToValidate="ddBranch" Display="None" ErrorMessage="Branch is a required field."></asp:RequiredFieldValidator>              
              <asp:HiddenField ID="hidEmpMstrId" runat="server"/>
              <asp:LinkButton ID="lnkRefreshMulLoc" Visible="false"  runat="server" OnClick="lnkRefreshMulLoc_Click"></asp:LinkButton>
		</td>
	</tr>

	            <tr class="vSpace">
		                    <td colspan="13"><asp:Label ID="lblmsg" runat="server" CssClass="errorMsg"></asp:Label></td>		        		        
		        	                        <td class="hSpace" height="22">&nbsp;</td>
		                    <td class="hSpace" height="22" style="width: 13px">&nbsp;</td>
	            </tr>

	<tr>
		<td class="normalLabel">[Branch] <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:DropDownList ID="ddBranch" runat="server" TabIndex="1" DataSourceID="dsBranch" DataTextField="BranchName" DataValueField="BranchInfoId" CssClass="dropdownLarge"></asp:DropDownList>
		           <asp:SqlDataSource ID="dsBranch" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            SelectCommand="select branchInfoId,branchName,0 as seq from BranchInfo where Active = 'Y' union all select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,branchName">
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
		<td class="normalLabel">[Employee Name] <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtEmployeeName" runat="server" CssClass="textBoxLarge" MaxLength="200" TabIndex="2"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Employee Code</td>
		<td class="hSpace">&nbsp;</td>
		<td style="width: 319px"><asp:TextBox ID="txtEmployeeCode" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="3"></asp:TextBox></td>
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
		<td class="normalLabel">[User Login Id] <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtLoginId" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="4"></asp:TextBox></td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Password <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>		 
        <td style="width: 319px"><cc1:textboxex id="txtPassword" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="5" textmode="Password"></cc1:textboxex></td>
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
		<td class="normalLabel">Access Level <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:DropDownList ID="ddAccessLevel" runat="server" TabIndex="6" DataSourceID="dsAccessLevel" DataTextField="codedesc" DataValueField="codeabbr" CssClass="dropdownMedium"></asp:DropDownList>
		           <asp:SqlDataSource ID="dsAccessLevel" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            SelectCommand="select codeabbr,codedesc,0 as seq from codemstr where codecat = 'LVL' union all select 0 as codemstrid,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq">
                                        </asp:SqlDataSource>
		</td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Default Location  <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
        <td nowrap style="width: 319px"><asp:DropDownList ID="ddLocation" runat="server" TabIndex="7" DataSourceID="dsLocation" DataTextField="locationName" DataValueField="locationinfoid" CssClass="dropdownLarge"></asp:DropDownList><asp:SqlDataSource ID="dsLocation" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
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
	</tr>
	
    <tr>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
        <td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Multiple Location</td>
		<td class="hSpace">&nbsp;</td>
        <td style="width: 319px"><asp:TextBox ID="txtMultipleLoc"  runat="server" CssClass="textBoxLarge" MaxLength="200" TabIndex="8" TextMode="MultiLine"></asp:TextBox></td>
        <td class="hSpace"><img id="imgEmpId" alt=""  runat= "server" border="0" src="../img/magnifier.gif" width="20" height="20" onclick="return imgEmp_onclick()"></td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>		
	</tr>	


	<tr>
		<td class="normalLabel">Title</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtTitle" runat="server" CssClass="textBoxSmall" MaxLength="50" TabIndex="9"></asp:TextBox></td>		
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Contact No</td>
		<td class="hSpace">&nbsp;</td>
        <td style="width: 319px"><asp:TextBox ID="txtContactNo" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="10"></asp:TextBox></td>		
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
		<td class="normalLabel">Department</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtDepartment" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="11"></asp:TextBox></td>		
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Email</td>
		<td class="hSpace">&nbsp;</td>
        <td style="width: 319px"><asp:TextBox ID="txtEmail" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="12"></asp:TextBox></td>		
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
		<td class="normalLabel">Address 1</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtAddress1" runat="server" CssClass="textBoxLarge" MaxLength="200" TabIndex="13"></asp:TextBox></td>		
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
        <td class="hSpace" style="width: 319px">&nbsp;</td>
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
		<td class="normalLabel">Address 2</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtAddress2" runat="server" CssClass="textBoxLarge" MaxLength="200" TabIndex="14"></asp:TextBox></td>		
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
        <td class="hSpace" style="width: 319px">&nbsp;</td>
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
		<td class="normalLabel">Address 3</td>
		<td class="hSpace">&nbsp;</td>
        <td><asp:TextBox ID="txtAddress3" runat="server" CssClass="textBoxLarge" MaxLength="200" TabIndex="15"></asp:TextBox></td>				
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">PostCode</td>
		<td class="hSpace">&nbsp;</td>
		<td style="width: 319px"><asp:TextBox ID="txtPostCode" runat="server" CssClass="textBoxMedium" MaxLength="50" TabIndex="16"></asp:TextBox></td>				
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
		<td class="normalLabel">State</td>
		<td class="hSpace">&nbsp;</td>
        <td nowrap><asp:DropDownList ID="ddState" runat="server" TabIndex="17" DataSourceID="dsState" DataTextField="codeDesc" DataValueField="codeabbr" CssClass="dropdownMedium"></asp:DropDownList><asp:SqlDataSource ID="dsState" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            SelectCommand="select codeabbr,codedesc,0 as seq from codemstr where codecat = 'STA' union all select codeabbr,codedesc,seq from codemstr where codecat = 'DEFAULT' order by seq,codedesc">
                                        </asp:SqlDataSource>
		</td>		
		<td class="hSpace">&nbsp;</td>
		<td class="normalLabel">Remark</td>
		<td class="hSpace">&nbsp;</td>
        <td style="width: 319px"><asp:TextBox ID="txtRemark" runat="server" CssClass="textBoxLarge" MaxLength="1000" TabIndex="18"></asp:TextBox></td>				
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
		<td class="normalLabel">
            [Active] <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:RadioButton ID="rbYes" runat="server" GroupName="Active" TabIndex="20" Text="Yes" checked="True" AutoPostBack="true"/>&nbsp;
                   <asp:RadioButton ID="rbNo" runat="server" GroupName="Active" TabIndex="21" Text="No" Checked="False" AutoPostBack="true"/></td>
		<td class="hSpace">&nbsp;</td>
		<td class="hSpace">&nbsp;</td>
        <td class="hSpace">&nbsp;</td>
      	<td class="hSpace" style="width: 319px">&nbsp;</td>
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
        <td class="hSpace" style="width: 92px; height: 22px;">&nbsp;</td>
        <td class="hSpace" style="width: 13px; height: 22px;">&nbsp;</td>
        <td colspan="12" align="right" style="height: 22px">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonMedium" CausesValidation="false"
                TabIndex="30" OnClick="btnSearch_Click" />
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="buttonMedium" CausesValidation="true"
                TabIndex="31" OnClick="btnAdd_Click" />
            <asp:Button ID="btnUpdate" runat="server" Text="Update" Enabled ="false"  CssClass="buttonMedium" CausesValidation="true"
                TabIndex="32" OnClick="btnUpdate_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonMedium" CausesValidation="false"
                TabIndex="33" OnClick="btnClear_Click" />
            <asp:Button ID="btnReset" runat="server" Text="Reset Password" Enabled ="false" CssClass="buttonLarge" CausesValidation="false"
                TabIndex="33" OnClick="btnReset_Click"/>    
        </td>
        <td class="hSpace" style="height: 22px">
            &nbsp;</td>
        <td class="hSpace" style="height: 22px">
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
                			    

                        <%  If gvEmployees.Rows.Count > 0 Then%>
                        
                        <tr>
                            <td bgcolor="white">
                                <asp:GridView ID="gvEmployees" runat="server" AllowPaging="True" AllowSorting = "True" 
                                    AutoGenerateColumns="False" CellPadding="0" Width="100%"
                                    BorderWidth="1px" HorizontalAlign="Left" CellSpacing="1" DataKeyNames="EMPLOYEEMSTRID,EMPLOYEENAME,EMPLOYEECODE,TITLE,BRANCHINFOID,REMARK,
                                    EMAIL,CONTACTNO,DEPARTMENT,ADDRESS1,ADDRESS2,ADDRESS3,POSTCODE,STATE,STATUS,DEFAULTLOCATIONINFOID,LUB,LUDT,
                                    USERMSTRID,USERID,PASSWORD,ACCESSLEVEL,ENQUIREREPORT" DataSourceID="dsEmployee">
                                    <Columns>
                                        <asp:BoundField DataField="EMPLOYEENAME" HeaderText="Employee Name" SortExpression="EMPLOYEENAME"
                                            NullDisplayText="N/A" />
                                        <asp:BoundField DataField="USERID" HeaderText="User Login Id" SortExpression="USERID"
                                            NullDisplayText="N/A" />
                                        <asp:BoundField DataField="ACCESSLEVEL" HeaderText="Access Level" SortExpression="ACCESSLEVEL"
                                            NullDisplayText="N/A" />
                                        <asp:BoundField DataField="CONTACTNO" HeaderText="Contact No" SortExpression="CONTACTNO"
                                            NullDisplayText="N/A" />                                                                                                                         
                                    </Columns>
                                    <RowStyle CssClass="grid_row1" />
                                    <SelectedRowStyle CssClass="tb-highlight" />
                                    <HeaderStyle CssClass="grid_header" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <AlternatingRowStyle CssClass="grid_row2" />
                                    <PagerStyle Font-Bold="True" Font-Underline="True" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="dsEmployee" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
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
