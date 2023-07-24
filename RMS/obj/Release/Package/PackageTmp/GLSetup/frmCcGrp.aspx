<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
Culture="auto" UICulture="auto" EnableEventValidation="false" 
CodeBehind="frmCcGrp.aspx.cs" Inherits="RMS.GLSetup.frmCcGrp"  %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" language="javascript">
    function pageLoad() {
        $('#<%= txtCCFrom.ClientID %>').change(function () {
            var lst = $(this).val().split('-');
            $(this).val(lst[0]);
        });
        $('#<%= txtCCTo.ClientID %>').change(function () {
            var lst = $(this).val().split('-');
            $(this).val(lst[0]);
        });
    }
</script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List" ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
  
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                        <table>
                            <tr valign="top">
                                <td class="LblBgSetup">
                                    <asp:Label ID="Label3" runat="server" Text="CC Group:" Width="110"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGrp" runat="server" Width="80" MaxLength="3"></asp:TextBox>
                                    <asp:RangeValidator ID="rvGrp" runat="server" ControlToValidate="txtGrp" Display="None" ErrorMessage="Group must be within 1 to 999" MinimumValue="1" MaximumValue="999" SetFocusOnError="true" ValidationGroup="main"></asp:RangeValidator> 
                                    <asp:RequiredFieldValidator ID="rfvGrp" runat="server" ControlToValidate="txtGrp" Display="None" ErrorMessage="Please enter CC Group" SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>
                                </td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr valign="top">
                                <td class="LblBgSetup">
                                    <asp:Label ID="Label2" runat="server" Text="Group Description:" Width="110"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGrpDesc" runat="server" Width="150" MaxLength="50"></asp:TextBox>
                                </td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr valign="top">
                                <td class="LblBgSetup">
                                    <asp:Label ID="Label1" runat="server" Text="Code From:" Width="110"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCCFrom" runat="server" Width="150" Text="" MaxLength="12"></asp:TextBox>
                                    <ajaxToolkit:AutoCompleteExtender runat="server" ID="AutoComplete1" TargetControlID="txtCCFrom" ServiceMethod="GetCostCenter" ServicePath="~/InvSetupSupport/AutoCompleteSearch.asmx" MinimumPrefixLength="1"
                                    CompletionSetCount="5" CompletionInterval="100" FirstRowSelected="true" EnableCaching="true">
                                    </ajaxToolkit:AutoCompleteExtender>
                                </td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr valign="top">
                                <td class="LblBgSetup">
                                    <asp:Label ID="Label6" runat="server" Text="Code To:" Width="110"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCCTo" runat="server" Width="150" Text="" MaxLength="12"></asp:TextBox>
                                    <ajaxToolkit:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1" TargetControlID="txtCCTo" ServiceMethod="GetCostCenter" ServicePath="~/InvSetupSupport/AutoCompleteSearch.asmx" MinimumPrefixLength="1"
                                    CompletionSetCount="5" CompletionInterval="100" FirstRowSelected="true" EnableCaching="true">
                                    </ajaxToolkit:AutoCompleteExtender>
                                </td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                                       
                            </table>
                </td>
                </tr>
                <tr>
                    <td>
                            <asp:ImageButton ID="btnSave" runat ="server"  OnClick="btnSave_Click" ImageUrl="~/images/btn_save.png" onMouseOver="this.src='../images/btn_save_m.png'" onMouseOut="this.src='../images/btn_save.png'"  ValidationGroup="main" />
                            <asp:ImageButton ID="btnClear" runat ="server"  OnClick="btnClear_Click" ImageUrl="~/images/btn_clear.png" onMouseOver="this.src='../images/btn_clear_m.png'" onMouseOut="this.src='../images/btn_clear.png'" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grdcode" runat="server" DataKeyNames="CompId, CCGrp, CCSeq" OnSelectedIndexChanged="grdcode_SelectedIndexChanged" AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No record found." Width="100%" PageSize="25" OnPageIndexChanging="grdcode_PageIndexChanging" OnRowDataBound="grdcode_RowDataBound">                       
                            <Columns>
                                <asp:BoundField DataField="CCGrp" HeaderText="Group"  />
                                <asp:BoundField DataField="CCSeq" HeaderText="Seq."  />
                                <asp:BoundField DataField="GrpDesc" HeaderText="Description"  />
                                <asp:BoundField DataField="CC_From" HeaderText="From CC" />
                                <asp:BoundField DataField="CC_To" HeaderText="To CC" />
                                <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                                    <ItemStyle />
                                    <ControlStyle CssClass="lnk"></ControlStyle>
                                </asp:CommandField>
                            </Columns>                     
                            <HeaderStyle CssClass ="grid_hdr" />
                            <RowStyle CssClass="grid_row" />
                            <AlternatingRowStyle CssClass="gridAlternateRow" />                                                    
                        </asp:GridView>
                    </td>
                </tr>
        </table>
 
</asp:Content>
