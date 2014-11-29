<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SOR.aspx.vb" Inherits="Transaction_SOR" %>

<%@ Register TagPrefix="rjs" Namespace="RJS.Web.WebControl" Assembly="RJS.Web.WebControl.PopCalendar"%>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral,PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Parking Refund</title>
    <link rel="stylesheet" type="text/css" href="../include/stylesheet.css" />
    <script type="text/javascript" language="JavaScript" src='../include/effect.js'></script>
    <script type="text/javascript" language="JavaScript" src='../include/javascript.js'></script>
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>  
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>

        
</head>


<script language="javascript" type="text/javascript">
         window.onscroll=move;
                       
         function onUpdating(){
                        // get the update progress div
                        var pnlPopup = $get('<%= pnlPopup.ClientID %>');

                        //  get the gridview element
                        var gridView = $get('<%= gvSor.ClientID %>');

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
                
                
//               $(function() {		
//                  $( "#datepicker1" ).datepicker
//                      ({	
//                   		 showButtonPanel: false, 
//                   		 changeMonth: true,			
//                   		 changeYear: true,  
//                         dateFormat: 'MM-yy'		
//                       });	
//                    });



                             
//               $(function() {		
//                  $( "#datepicker1" ).datepicker
//                      ({	
//                   		 showButtonPanel: false, 
//                   		 changeMonth: true,			
//                   		 changeYear: true,  
//                         dateFormat: 'MM-yy',
//                         onClose: function( selectedDate ) {
//                                    var today = new Date(selectedDate);
//                                    var tomorrow = new Date(today.getTime() + (24 * 60 * 60 * 1000));
//                                    var curr_date = tomorrow.getDate();
//                                    var curr_month = tomorrow.getMonth() + 3; //Months are zero based
//                                    var curr_year = tomorrow.getFullYear();

//                                    var max_string = curr_month+"/"+curr_date+"/"+curr_year;

//                                    $( "#datepicker2" ).datepicker( "option", "minDate", selectedDate );
//                                    $( "#datepicker2" ).datepicker( "option", "maxDate", max_string );
//                                }


//                         		
//                       });	
//                       
//                       $( "#datepicker2" ).datepicker({                            
//                            changeMonth: true,
//                            changeYear: true, 
//                   		    dateFormat: 'MM-yy', 
//                            onClose: function( selectedDate ) {
//                                $( "#datepicker1" ).datepicker( "option", "maxDate", selectedDate );
//                            }
//                        });



//                    });
//        
        


//$(function()
//{   
//    $("#datepicker1").datepicker({
//        dateFormat: 'MM yy',
//        changeMonth: true,
//        changeYear: true,
//        showButtonPanel: true,
//        onClose: function(dateText, inst) {
//            var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
//            var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
//            $(this).val($.datepicker.formatDate('MM yy', new Date(year, month, 1)));
//        }
//    });

//    $("#datepicker1").focus(function () {
//        $(".ui-datepicker-calendar").hide();
//        $("#ui-datepicker-div").position({
//            my: "center top",
//            at: "center bottom",
//            of: $(this)
//        });
//    });
//    
//     
//    
//});

//$(function()
//{ 
//$("#datepicker1").datepicker({
//           
//                 dateFormat: 'MM yy',
//                changeMonth: true,
//                changeYear: true,
//                numberOfMonths: 1,
//                onClose: function (selectedDate) {
//                    var d = new Date(selectedDate);
//                    d.setDate(d.getDate() + 1);
//                    $("#datepicker2").datepicker("option", "minDate", selectedDate);
//                    $("#datepicker2").datepicker("option", "maxDate", d);
//                    $(this).val(d.getDate());
//                }
//            });
//            $("#datepicker2").datepicker({
//      
//                changeMonth: true,
//                changeYear: true,
//                 dateFormat: 'MM yy',
//                numberOfMonths: 1,
//                onClose: function (selectedDate) {
//                    $("#datepicker1").datepicker("option", "maxDate", selectedDate);
//                }
//            });
//            
//});








              
    
</script>







<body> 
<form id="Form1" action='' runat="server" >
<asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        
 
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <contenttemplate> 
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td class="header1">
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td class="hSpace">&nbsp;</td>
					<td class="header1" style="border:none;"><asp:Label runat="server" id="lblHeader1" Text="Item"></asp:Label></td>
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
		<td class="header2" colspan="15" style="height: 35px">
		  <table border="0" cellpadding="0" cellspacing="0" width="100%">
			<tr>
			  <td class="hSpace">&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			  <td class="header2" style="border:none;">Suspend On Request&nbsp;</td>
			  <td class="hSpace">&nbsp;</td>
			<asp:HiddenField ID="hidDebtorId" runat="server"/>
			<asp:HiddenField ID="hidLocationInfoId" runat="server"/>
			</tr>
		  </table>
		  <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errorMsg"
                                            DisplayMode="List" />
          <asp:RequiredFieldValidator ID="StartMonth" runat="server" ControlToValidate="ddStartMonth" Display="None" ErrorMessage="Start Month is a required field."></asp:RequiredFieldValidator>                                                                          
          <asp:RequiredFieldValidator ID="EndMonth" runat="server" ControlToValidate="ddEndMonth" Display="None" ErrorMessage="End Month is a required field."></asp:RequiredFieldValidator>                                                                          
          <asp:RequiredFieldValidator ID="SerialNo" runat="server" ControlToValidate="ddSerialNo" Display="None" ErrorMessage="Serial No is a required field."></asp:RequiredFieldValidator>                                                                          
		</td>
	</tr>

	                <tr class="vSpace">		    
		                <td colspan="15"><asp:Label ID="lblmsg" runat="server" CssClass="errorMsg"></asp:Label></td>		                
                        </td>
	                </tr>


  
	<tr class="vSpace">
	    <td class="normalLabel">[Location]</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:DropDownList ID="ddLocation" runat="server" AutoPostBack ="true"  TabIndex="1" DataSourceID="dsLocation" DataTextField="locationName" DataValueField="locationinfoid" CssClass="dropdownLarge" OnSelectedIndexChanged="ddLocation_SelectedIndexChanged">
                            </asp:DropDownList><asp:SqlDataSource ID="dsLocation" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>">
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
	    <td class="normalLabel">[Category]</td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:RadioButton ID="rbIndividual" runat="server" GroupName="Category" TabIndex="3" Text="Individual" AutoPostBack="true" OnCheckedChanged="rbIndividual_CheckedChanged"/>&nbsp;
                   <asp:RadioButton ID="rbCompany" runat="server" GroupName="Category" TabIndex="4" Text="Company" Checked="True" AutoPostBack="true" OnCheckedChanged="rbCompany_CheckedChanged"/>
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
		<td class="normalLabel">Debtor <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:DropDownList ID="ddDebtor" runat="server" AutoPostBack ="true" TabIndex="5" DataSourceID="dsDebtor" DataTextField="Debtor" DataValueField="DebtorId" CssClass="dropdownLarge" OnSelectedIndexChanged="ddDebtor_SelectedIndexChanged"></asp:DropDownList><asp:SqlDataSource ID="dsDebtor" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"></asp:SqlDataSource>
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
		<td class="normalLabel">Serial No <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
		<td nowrap><asp:DropDownList ID="ddSerialNo" runat="server" TabIndex="5" DataSourceID="dsSerialNo" DataTextField="SERIALNO" DataValueField="PASSCARDMSTRID" CssClass="dropdownLarge"></asp:DropDownList><asp:SqlDataSource ID="dsSerialNo" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"></asp:SqlDataSource>
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
		<td class="normalLabel">Start Month <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
        <td nowrap>  <asp:DropDownList ID="ddStartMonth"  AutoPostBack ="true" runat="server" OnSelectedIndexChanged="ddStartMonth_SelectedIndexChanged">                                                
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
		<td class="normalLabel">End Month <font color="#FF0000">*</font></td>
		<td class="hSpace">&nbsp;</td>
        <td nowrap>  <asp:DropDownList ID="ddEndMonth"  AutoPostBack ="true" runat="server">                                                
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
	
	<tr class="vSpace">
		<td class="normalLabel">Remark</td>
		<td class="hSpace">&nbsp;</td>
		<td><asp:TextBox ID="txtRemark" runat="server" CssClass="textBoxExtraLarge" TabIndex="10" Enabled="true" MaxLength="200"></asp:TextBox></td>        
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
		<td colspan="15" height="17"></td>
    </tr>
                    <tr>
                        <td class="hSpace" height="22" style="width: 92px">
                            &nbsp;</td>
                        <td class="hSpace" height="22" style="width: 13px">
                            &nbsp;</td>
                        <td height="22" colspan="7" align="right">
                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonMedium" CausesValidation="false"
                                TabIndex="8" OnClick="btnClear_Click"/>
                           <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="buttonMedium" CausesValidation="true"
                                TabIndex="8" OnClick="btnAdd_Click"/>                                      
                        </td>
                        <td class="hSpace" height="22">
                            &nbsp;</td>
                        <td class="hSpace" height="22">
                            &nbsp;</td>
                    </tr>
                    
    <tr>
		<td colspan="15" height="17"></td>
    </tr>
    
                    <tr>
                        <td colspan="13" align="left" style="width: 92px">
                            <asp:Label ID="lblRecCount" runat="server" CssClass="errorMsg"></asp:Label></td>
                        <td class="hSpace" height="22">
                            &nbsp;</td>
                        <td class="hSpace" height="22" style="width: 13px">
                            &nbsp;</td>
                    </tr>  
                    
                    <tr>
                        <td colspan="15">
                            <!-- Spreadsheet Header -->
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <%  If gvSor.Rows.Count > 0 Then%>
                                <tr>
                                    <td bgcolor="white">
                                        <asp:GridView ID="gvSor" runat="server" AllowPaging="True" AllowSorting="True"
                                            AutoGenerateColumns="False" CellPadding="0" Width="100%" BorderWidth="1px" HorizontalAlign="Left"
                                            CellSpacing="1" DataKeyNames="SORID,DEBTORID,PASSCARDMSTRID,STARTMONTH,ENDMONTH,REMARKS,SERIALNO" DataSourceID="dsSor">                                            
                                            <Columns>
                                                <asp:TemplateField HeaderText="Delete">
                                                  <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" CausesValidation="false" CommandArgument='<%# Eval("SORID") %>'  CommandName="DeleteSOR" runat="server" OnClientClick="return confirm('Are you sure')">Delete</asp:LinkButton>
                                                  </ItemTemplate>
                                                </asp:TemplateField>                                                                                        
                                                <asp:BoundField DataField="PASSCARDMSTRID" HeaderText="PASSCARDMSTRID" SortExpression="PASSCARDMSTRID"
                                                NullDisplayText="N/A" Visible="false" />                                                                                        
                                                <asp:BoundField DataField="DEBTORID" HeaderText="DEBTORID" SortExpression="DEBTORID"
                                                NullDisplayText="N/A" Visible="false" />         
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
                                        <asp:SqlDataSource ID="dsSor" runat="server" ConnectionString="<%$ ConnectionStrings:CPMConnectionString %>"
                                            ProviderName="System.Data.SqlClient"></asp:SqlDataSource>
                                    </td>
                                </tr>
                                <%End If%>
                            </table>
                        </td>
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
