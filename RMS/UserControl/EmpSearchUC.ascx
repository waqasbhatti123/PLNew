<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmpSearchUC.ascx.cs" Inherits="RMS.UserControl.EmpSearchUC" %>
<table width="700">
    
 <tr>
  <td>
      <asp:Label ID="lblEmpSrch" Text="Select Employee:" runat="server"></asp:Label>
  </td>
  <td>
        <asp:TextBox runat="server" ID="txtEmpSrch" MaxLength="150" CssClass="RequiredField form-control"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmpSrch"
            ErrorMessage="Please select employee" SetFocusOnError="true" ValidationGroup="main"
            Display="None"></asp:RequiredFieldValidator>
  </td>
  <td>
        <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/search-icon-blue.gif"
              OnClick="btnSearch_Click" ToolTip="Search Employee"/>
  </td>
 </tr>
 <tr>
  <td align="left" colspan="2">
    <div id="divEmpInfo" runat="server" visible="false">
    <table cellspacing="3" cellpadding="2" class="DivTble" width="100%">
    <tr><td>
    <asp:Label ID="lblEmpName" runat="server"></asp:Label>
    </td></tr>
    <tr><td>
        <asp:Label ID="lblEmpId" runat="server" Text=""></asp:Label>
    </td></tr>
    <tr><td>
        <asp:Label ID="lblEmpCode" runat="server" Text=""></asp:Label>
    </td></tr>
    <tr><td>
        <asp:Label ID="lblEmpDesig" runat="server" Text=""></asp:Label>
    </td></tr>
    <tr><td>
        <asp:Label ID="lblEmpDept" runat="server" Text=""></asp:Label>
    </td></tr>
    </table>
    </div>
  </td>
 </tr>
</table>
<asp:GridView runat="server" ID="grdEmpSrchUC" DataKeyNames="EmpID" CssClass="table table-responsive-sm" OnSelectedIndexChanged="grdEmpSrchUC_SelectedIndexChanged" 
    AutoGenerateColumns="False" Visible="false">
    <HeaderStyle CssClass ="grid_hdr" />
    <RowStyle CssClass="grid_row" />
    <AlternatingRowStyle CssClass="gridAlternateRow" />
    <SelectedRowStyle CssClass="gridSelectedRow" />
    <Columns>
        <asp:BoundField DataField="emp_Id" HeaderText="Emp ID"/>
        <asp:BoundField DataField="EmpCode" HeaderText="Emp Ref"/>
        <asp:BoundField DataField="FullName" HeaderText="Name"/>
        <asp:BoundField DataField="Desig" HeaderText="Desig."/>
        <asp:BoundField DataField="Dept" HeaderText="Dept."/>
        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
            <ItemStyle />
            <ControlStyle CssClass="lnk"></ControlStyle>
        </asp:CommandField>
    </Columns>
</asp:GridView>
