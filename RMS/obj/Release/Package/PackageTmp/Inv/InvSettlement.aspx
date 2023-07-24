<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
Culture="auto" UICulture="auto" EnableEventValidation="false" 
CodeBehind="InvSettlement.aspx.cs" Inherits="RMS.Inv.InvSettlement"  %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
 <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List" ValidationGroup="main" />
 <uc1:Messages ID="ucMessage" runat="server" />
 <table width="80%">
  <tr>
   <td colspan="2">
    <table width="50%">
     <tr align="left">
      <td>
       Vendor:
      </td>
      <td>
       <asp:DropDownList ID="ddlVendor" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged">
        <asp:ListItem Selected="True" Value="0">Select Party</asp:ListItem>
       </asp:DropDownList>
      </td>
     </tr>
    </table>
   </td>
  </tr>
  <tr>
   <td>
       <asp:GridView ID="grdInvoices" runat="server" DataKeyNames="vrid" GridLines="None"  
                     OnSelectedIndexChanged="grdInvoices_SelectedIndexChanged" OnRowDataBound="grdInvoices_RowDataBound"
                     AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No record found." 
                     width="450px" PageSize="50" OnPageIndexChanging="grdInvoices_PageIndexChanging">
        <HeaderStyle CssClass ="grid_hdr" />
        <RowStyle CssClass="grid_row" />
        <AlternatingRowStyle CssClass="gridAlternateRow" />
        <SelectedRowStyle CssClass="gridSelectedRow" />
        <Columns>
            <asp:BoundField DataField="IV_NO" HeaderText="JV/MPN #" />
            <asp:BoundField DataField="IV_Date" HeaderText="Date" />
            <asp:BoundField DataField="IV_Due_Date" HeaderText="Due Date" />
            <asp:BoundField DataField="invtotal" HeaderText="Bill Amount" />
            <asp:BoundField DataField="settled_amnt" HeaderText="Settled" />
            <asp:BoundField DataField="balance" HeaderText="Balance" />
            <asp:TemplateField HeaderText="Select">
             <ItemTemplate>
              <asp:CheckBox ID="chkInvoice" runat="server" />
             </ItemTemplate>
            </asp:TemplateField>
        </Columns>
       </asp:GridView>
   </td>
   <td>
        <asp:GridView ID="grdPayments" runat="server" DataKeyNames="TransNo" GridLines="None"  
                     OnSelectedIndexChanged="grdPayments_SelectedIndexChanged" OnRowDataBound="grdPayments_RowDataBound"
                     AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No record found." 
                     Width="400px" PageSize="50" OnPageIndexChanging="grdPayments_PageIndexChanging">
        <HeaderStyle CssClass ="grid_hdr" />
        <RowStyle CssClass="grid_row" />
        <AlternatingRowStyle CssClass="gridAlternateRow" />
        <SelectedRowStyle CssClass="gridSelectedRow" />
        <Columns>
            <asp:BoundField DataField="vr_no" HeaderText="Voucher #" />
            <asp:BoundField DataField="TransDate" HeaderText="Date" />
            <asp:BoundField DataField="TransAmt" HeaderText="Paid Amount" />
            <asp:BoundField DataField="settled_amnt" HeaderText="Settled" />
            <asp:BoundField DataField="balance" HeaderText="Balance" />
            <asp:TemplateField HeaderText="Select">
             <ItemTemplate>
              <asp:CheckBox ID="chkPayment" runat="server" />
             </ItemTemplate>
            </asp:TemplateField>
        </Columns>
       </asp:GridView>
   </td>
  </tr>
  <tr>
   <td colspan="2">
     <asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btn_save.png"
                                OnClick="btnSave_Click" />
   </td> 
  </tr>
 </table>
    
   
</asp:Content>
