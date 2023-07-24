<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
CodeBehind="frmBudgetEntry.aspx.cs" Inherits="RMS.GL.Setup.frmBudgetEntry"  %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">
    function hdnBudgetCode_Selected(sender, e) {
        var hdnBudgetCode = $get('<%= hdnBudgetCode.ClientID %>');
        hdnBudgetCode.value = e.get_value();
    }

    // $(document).ready(function() {
    function pageLoad() {

        $(".classOnlyInt").keydown(function(event) {
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }
        });
    }

 //   });
    
       </script>
    
 
    
      <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                
      <asp:ValidationSummary ID="ValidationSummary1" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
              
             <table class="stats1">
             
           
              <tr>
                <td colspan="2" valign="top"></td>
              </tr>
             
              
              <tr>
                <td>
                
                    <asp:Label ID="lblBudgetYear" runat="server" Text="Budget Year:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtBudgetYear" runat="server" Width="85px" Enabled="false"></asp:TextBox>
                </td>
                
            </tr>
              <tr>
                <td>
                    <asp:Label ID="lblBudgetCode" runat="server" Text="Budget Code:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtBudgetCode" runat="server" Width="400px" AutoPostBack="true" OnTextChanged="txtBudgetCode_TextChanged"></asp:TextBox>
                    <asp:HiddenField ID="hdnBudgetCode" runat="server" />
                    <ajaxToolkit:AutoCompleteExtender ID="ACE1" runat="server"  TargetControlID="txtBudgetCode" EnableCaching="true" FirstRowSelected="true" MinimumPrefixLength="1" ServiceMethod="AutoCompletBgtCode" ServicePath="~/InvSetupSupport/AutoCompleteSearch.asmx"
                     OnClientItemSelected="hdnBudgetCode_Selected" CompletionInterval="100" CompletionSetCount="5" ></ajaxToolkit:AutoCompleteExtender>
                     
                     <asp:RequiredFieldValidator ID="rqFv" runat="server" ControlToValidate="txtBudgetCode" Display="None" ErrorMessage="Please enter Budget Code" SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>
                </td>
                
            </tr>
            
            
                    <tr>
                    <td>
                        <asp:Label ID="lblAcRange" Text="A/C Range  From :" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAcFrom" runat="server" Width="85px" Enabled="false"></asp:TextBox>
                        
                        &nbsp <asp:Label ID="lblAcTo" Text="To :" runat="server"></asp:Label>
                       &nbsp
                        <asp:TextBox ID="txtAcTo" runat="server" Width="85px" Enabled="false"></asp:TextBox>
                                  
                      </td>
                </tr>
                <tr>
                    <td>
                    <asp:Label ID="lblBudgetType" runat="server" Text="Budget Type:"></asp:Label>
                    </td>
                    <td>
                    <asp:TextBox ID="txtBudgetType" runat="server" Width="85px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
         </table>
    
         <table>
       
            <tr>
                   <td>&nbsp</td>
                  <td style="text-align:center;"><asp:Label ID="lblYearl" runat="server" Text="Yearly"></asp:Label></td>
                   <td style="text-align:center;"><asp:Label ID="lblQ1" runat="server" Text="Qtr-1" Visible="false"></asp:Label></td>
                   <td style="text-align:center;"><asp:Label ID="lblQ2" runat="server" Text="Qtr-2" Visible="false"></asp:Label></td>
                   <td style="text-align:center;"><asp:Label ID="lblQ3" runat="server" Text="Qtr-3" Visible="false"></asp:Label></td>
                   <td style="text-align:center;"><asp:Label ID="lblQ4" runat="server" Text="Qtr-4" Visible="false"></asp:Label></td>
           
                    
                </tr>
                
                <tr>
                    <td style="width:102px;"> <asp:Label ID="lblBudgetAmount" runat="server" Text="Budget Amount:"></asp:Label></td>
                    <td><asp:TextBox ID="txtYearlyAmount" runat="server" AutoPostBack="true" OnTextChanged="txtYearlyAmount_textChanged" Width="85px" Class="classOnlyInt" style="text-align:right"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtQ1" runat="server" Width="85px" Visible="false" style="text-align:right" Class="classOnlyInt"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtQ2" runat="server" Width="85px" Visible="false" Class="classOnlyInt" style="text-align:right"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtQ3" runat="server" Width="85px" Visible="false" Class="classOnlyInt" style="text-align:right"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtQ4" runat="server" Width="85px" Visible="false" Class="classOnlyInt" style="text-align:right"></asp:TextBox></td>
                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="None" ControlToValidate="txtYearlyAmount" ErrorMessage="Please enter yearly budget amount" SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="None" ControlToValidate="txtQ1" ErrorMessage="Please enter Qtr-1 budget amount" SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="None" ControlToValidate="txtQ2" ErrorMessage="Please enter Qtr-2 budget amount" SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>
                       <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="None" ControlToValidate="txtQ3" ErrorMessage="Please enter Qtr-3 budget amount" SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="None" ControlToValidate="txtQ4" ErrorMessage="Please enter Qtr-4 budget amount" SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>
                   
                 </tr>
           
            </table> 
    
    <table>
        <tr>
            <td style="width:102px;"><asp:Label ID="lblLastYearBudget" runat="server" Text="Last Yr Budget:" ></asp:Label></td>
            <td><asp:TextBox ID="txtLastYearBudget" runat="server" Enabled="false" Width="85px" style="text-align:right"></asp:TextBox></td>            
        </tr>
         
         <tr>
            <td><asp:Label ID="lblLastYearUtilizedB" runat="server" Text="Last Yr Utilized:"></asp:Label></td>
            <td><asp:TextBox ID="txtLastYearUtilized" runat="server" Width="85px" Enabled="false" style="text-align:right"></asp:TextBox></td>
         </tr>
    </table>                   
                            
         </ContentTemplate>
         <Triggers>
         <asp:AsyncPostBackTrigger  ControlID="grdcode"/>
         <asp:AsyncPostBackTrigger  ControlID="btnSave"/>
         <asp:AsyncPostBackTrigger ControlID="btnClear" />
         <asp:AsyncPostBackTrigger ControlID="ImageButton1" />
         </Triggers>
         
                </asp:UpdatePanel>
                                    
                 <table>
                 <tr><td>&nbsp</td></tr>
                 <tr>
               <td valign="top">
                <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" ImageUrl="../images/btn_save.png" onmouseover="../images/btn_save_m.png"
                onmouseout="../images/btn_save.png"  ValidationGroup="main" />
                
               
                <asp:ImageButton ID="btnClear" runat="server" OnClick="btnClear_Click" ImageUrl="../images/btn_clear.png" onmouseover="../images/btn_clear_m.png"
                onmouseout="../images/btn_save.png" />
                </td>
               </tr>
             </table>
               

        
            <table class="filterTable" cellpadding="1" cellspacing="2" width="100%"><tr><td colspan="9">&nbsp;</td></tr>
              <tr>
                <td>&nbsp;</td>
                <td>Budget Code:</td>
                <td>
                    <asp:TextBox runat="server" ID="txtFltBudgetCode" Width="80" Class="classOnlyInt"></asp:TextBox>
                    
                </td>
                <td>Description:</td>
                <td>
                    <asp:TextBox runat="server" ID="txtFltDesc"></asp:TextBox>
                </td>
                <td>Budget Type:</td>
                <td>
                    <asp:DropDownList ID="ddlFltBudgetType" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Value="0">All</asp:ListItem>
                        <asp:ListItem Text="Yearly" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="Quarterly" Value="Q"></asp:ListItem>    
                   </asp:DropDownList>
                </td>
                <td>Budget Yr</td>
                <td>
                    <asp:DropDownList ID="ddlFltBgtYear" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Value="0">All</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                          OnClick="btnsearch_Click" ToolTip="Search Code" Visible="true"/>
                 </td>
                <td>&nbsp;</td>
            </tr>
            <tr><td colspan="9">&nbsp;</td></tr>
            </table>


<asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional">

<ContentTemplate>
        
          <asp:GridView ID="grdcode" runat="server" DataKeyNames="Bgt_Code,Bgt_Year" OnSelectedIndexChanged="grdcode_SelectedIndexChanged" OnRowDataBound="grdcode_RowDataBound"
                    AutoGenerateColumns="False" AllowPaging="True" Width="100%" PageSize="25" OnPageIndexChanging="grdcode_PageIndexChanging" >
                    <HeaderStyle CssClass ="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                    <Columns>
                        <asp:BoundField DataField="Bgt_Year" HeaderText="Budget Year"  />
                         <asp:BoundField DataField="Bgt_Code" HeaderText="Code"  />
                        <asp:BoundField DataField="Headg_Desc" HeaderText="Description" />
                        <%--<asp:BoundField DataField="Code_Type" HeaderText="Code Type" />
                        <asp:BoundField DataField="cnt_bgt_code" HeaderText="Budget Code Head" />--%>
                        <asp:BoundField DataField="AcRange" HeaderText="A/C Range"  />
                        <asp:BoundField DataField="Bgt_Type" HeaderText="Budget Type"  />
                        <asp:BoundField DataField="BgtAmount" HeaderText="Budget Amount"  />
                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                            <ItemStyle />
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
       
</ContentTemplate>

<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnSave" />
<asp:AsyncPostBackTrigger ControlID="ImageButton1" />
</Triggers>

</asp:UpdatePanel>

            
             
    

</asp:Content>
