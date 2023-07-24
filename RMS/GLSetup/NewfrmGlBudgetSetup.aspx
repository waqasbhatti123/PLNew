<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
CodeBehind="NewfrmGlBudgetSetup.aspx.cs" Inherits="RMS.GL.Setup.NewfrmGlBudgetSetup"  %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">

    function AcFr_ItemSelected(sender, e) {
        var AcFr = $get('<%= AcFr.ClientID %>');
        AcFr.value=e.get_value(); 
    }
    function AcTo_ItemSelected(sender, e) {
        var AcTo = $get('<%= AcTo.ClientID %>');
        AcTo.value = e.get_value();
    }

    //--------------------
    function pageLoad() {
        $("#<%= txtCode.ClientID %>").keyup(function(event) {

            var selectedTypeId1 = $("#<%= ddlcodetype.ClientID %>").val();
            var selectedTypeId2 = $("#<%= ddlBudgetheadcode.ClientID %>").val();

            if ($("#<%= ddlcodetype.ClientID %>").val() != "") {
               // alert("ab");
                $.ajax({
                url: "NewfrmGlBudgetSetup.aspx/CodeVal",
                    data: JSON.stringify({ selectedTypeId1: selectedTypeId1, selectedTypeId2: selectedTypeId2 }),
                    type: 'POST',
                    contentType: 'application/json;',
                    dataType: 'json',
                    success: function(heads) {
                        var heads = heads.d;
                        if (heads.length > 0) {
                         
                            if ($("#<%= txtCode.ClientID %>").val().substring(0, heads[0].p_ct_len) != heads[0].Bgt_Code) {
                                $("#<%= txtCode.ClientID %>").val(heads[0].Bgt_Code);
                            }
                        }
                    }

                });
            }
        });

        $("#<%= txtCode.ClientID %>").keydown(function(event) {

            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }
        });

    }
    //-----------------
    
    
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
                
                    <asp:Label ID="lblCodeType" runat="server" Text="Code Type:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlcodetype"  OnSelectedIndexChanged="ddlcodetype_SelectedIndexChanged" AutoPostBack="true"  runat="server" >
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlcodetype"
                        ErrorMessage="Please Select Code Type" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                </td>
                
            </tr>
              <tr>
                <td>
                    <asp:Label ID="lblBudgetHeadCode" runat="server" Text="Budget Head Code:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBudgetheadcode"   OnSelectedIndexChanged="ddlBudgetheadcode_SelectedIndexChanged"  runat="server" AutoPostBack="true"></asp:DropDownList>
                    <asp:HiddenField ID="hidglHeadCode" runat="server" />
                </td>
                
            </tr>
            
            <tr>
                <td>
                   <asp:Label ID="lblBudgetcode" runat="server" Text="Budget Code:"></asp:Label>
                </td>
                    <td> 
                    <asp:TextBox ID="txtCode" runat="server" CssClass="RequiredField" Width="120"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtCode" Display="None" ErrorMessage="Enter code." SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>   
                    </td>
                
               <%-- <td>
                <asp:TextBox ID="txtBudgetcode1" CssClass="RequiredField"  runat="server" Width="30px" ReadOnly="true"></asp:TextBox>
                    <asp:TextBox ID="txtBudgetcode" CssClass="RequiredField"  runat="server" Width="50px" MaxLength="150"></asp:TextBox>
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtBudgetcode"
                        ErrorMessage="Please enter Budget Code" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                </td>--%>
                
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbldescription" runat="server" Text="Description:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtdescription" CssClass="RequiredField" runat="server" MaxLength="150" Width="400"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtdescription"
                        ErrorMessage="Please enter Budget Description" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                </td>
                
            </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblAcRange" Text="A/C Range  From :" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAcFrom" runat="server" OnTextChanged="txtAcFrom_TextChanged" AutoPostBack="true"></asp:TextBox>
                        
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAcFrom"
                        ErrorMessage="Please enter A/C range" SetFocusOnError="true" ValidationGroup="main" Display="None" EnableClientScript="true"></asp:RequiredFieldValidator>
                       &nbsp <asp:Label ID="lblAcTo" Text="To :" runat="server"></asp:Label>
                       &nbsp
                        <asp:TextBox ID="txtAcTo" runat="server" OnTextChanged="txtAcTo_TextChanged" AutoPostBack="true"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtAcTo"
                        ErrorMessage="Please enter A/C range" SetFocusOnError="true" ValidationGroup="main" Display="None" EnableClientScript="true"></asp:RequiredFieldValidator>
                       
                       <ajaxToolkit:AutoCompleteExtender runat="server" ID="AutoComplete1" TargetControlID="txtAcFrom" ServiceMethod="getAcRange" ServicePath="~/InvSetupSupport/AutoCompleteSearch.asmx" MinimumPrefixLength="1"
                        CompletionSetCount="5" CompletionInterval="100" FirstRowSelected="true" EnableCaching="true" OnClientItemSelected="AcFr_ItemSelected" ></ajaxToolkit:AutoCompleteExtender>
                        <asp:HiddenField runat="server" ID="AcFr" />
                        
                         <ajaxToolkit:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1" TargetControlID="txtAcTo" ServiceMethod="getAcRange" ServicePath="~/InvSetupSupport/AutoCompleteSearch.asmx" MinimumPrefixLength="1"
                        CompletionSetCount="5" CompletionInterval="100" FirstRowSelected="true" EnableCaching="true"  OnClientItemSelected="AcTo_ItemSelected"></ajaxToolkit:AutoCompleteExtender>
                        <asp:HiddenField runat="server" ID="AcTo" />
                                       </td>
                </tr>
         
            <tr>
                <td>
                    <asp:Label ID="lblBudgettype" runat="server" Text="Budget Type:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBudgetType" runat="server" >
                   
                    <asp:ListItem Text="Yearly" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="Quarterly" Value="Q" ></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlBudgetType"
                        ErrorMessage="Please select Budget Type" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                        
                </td>
                
            </tr>
         
            
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top">
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
                    <asp:TextBox runat="server" ID="txtFltBudgetCode" Width="80"></asp:TextBox>
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
                <td>Code Type:</td>
                <td>
                    <asp:DropDownList ID="ddlFltCodeType" runat="server" AppendDataBoundItems="true">
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
        
          <asp:GridView ID="grdcode" runat="server" DataKeyNames="Bgt_Code" OnSelectedIndexChanged="grdcode_SelectedIndexChanged" OnRowDataBound="grdcode_RowDataBound"
                    AutoGenerateColumns="False" AllowPaging="True" Width="100%" PageSize="25" OnPageIndexChanging="grdcode_PageIndexChanging" >
                    <HeaderStyle CssClass ="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                    <Columns>
                        <asp:BoundField DataField="Bgt_Code" HeaderText="Code"  />
                        <asp:BoundField DataField="Headg_Desc" HeaderText="Description" />
                        <asp:BoundField DataField="Code_Type" HeaderText="Code Type" />
                        <asp:BoundField DataField="cnt_bgt_code" HeaderText="Budget Code Head" ControlStyle-CssClass="DisplayNone" ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone"/>
                        <asp:BoundField DataField="AC_Fr_To" HeaderText="A/C Range" />
                        <asp:BoundField DataField="Bgt_Type" HeaderText="Budget Type"  />
                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                            <ItemStyle />
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
          
         </ContentTemplate>
                </asp:UpdatePanel>

            
             
    

</asp:Content>
