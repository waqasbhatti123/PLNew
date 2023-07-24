<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="ItemGroup.aspx.cs" Inherits="RMS.Setup.ItemGroup" Title="ItemGroup" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main"/>
    <uc1:Messages ID="ucMessage" runat="server" />
    <asp:Panel ID="pnlMain" runat="server">
        <table width="80%">
            <tr>
                <td>
                    <asp:Label ID="lblItemDesc" runat="server" Text="Item Group"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtItemDesc" runat="server" MaxLength="100"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="DescValidator" ControlToValidate="txtItemDesc" 
                    ValidationGroup="main" ErrorMessage="Fill Item Group Field" Display="None" SetFocusOnError="true"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:Label ID="lblStatus" runat="server" Text="Status" ></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem Value="0" Selected="True">Active</asp:ListItem>
                        <asp:ListItem Value="1">InActive</asp:ListItem>
                    </asp:DropDownList>
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
            <td valign="top">&nbsp;</td>
            <td valign="top" colspan="2">
        
            <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
            <!--img src="images/btn_new.jpg" width="60" height="20" /> <img src="images/btn_edit.jpg" width="60" height="20" /> <img src="images/btn_delete.jpg" alt="" width="60" height="20" /> <img src="images/btn_save.jpg" width="60" height="20" /> <img src="images/btn_cancel.jpg" width="60" height="20" /-->
            </td>
            
          </tr>
          
          <tr>
            <td colspan="4">
            
             
             <asp:GridView ID="grdItmGroup" runat="server"  Width="100%" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                    AllowPaging="True" PageSize="5" DataKeyNames="itm_grp_id"  OnSelectedIndexChanged="grdItmGroup_SelectedIndexChanged" 
                    OnPageIndexChanging="grdItmGroup_PageIndexChanging" OnRowDataBound="grdItmGroup_RowDataBound">
                    <HeaderStyle CssClass ="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                    <Columns>
                        <asp:BoundField DataField="itm_grp_desc" HeaderText="Description" />
                        <asp:BoundField  DataField="status" HeaderText="Status"/>
                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                            <ItemStyle />
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
            
            </td>
          </tr>
            
        </table>
        </asp:Panel>
</asp:Content>
