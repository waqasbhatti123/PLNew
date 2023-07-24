<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
Culture="auto" UICulture="auto" EnableEventValidation="false" 
CodeBehind="StoreLocation.aspx.cs" Inherits="RMS.Inv.StoreLocation"  %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
<script type="text/javascript" language="javascript">
    //DISABLING DOUBLE CLICK
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    function BeginRequestHandler(sender, args) { var oControl = args.get_postBackElement(); oControl.disabled = true; }

</script>
<script type="text/javascript">
//Disabling default submit behaiour
    function disableEnterKey(e) {
        var key;
        if (window.event)
            key = window.event.keyCode; //IE
        else
            key = e.which; //firefox
        return (key != 13);
    }
    document.onkeypress = disableEnterKey;    
</script>
    
  <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="3%"></td>
            <td height="10"></td>
            <td width="3%"></td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td valign="top">
             <table class="stats1">
             <asp:Panel runat="server" ID="pnlMain">
              <tr>
                <td colspan="2" valign="top" class="bg_input_area"></td>
              </tr>
             
              
              <tr>
                <td>
                    <asp:Label ID="lblLocID" runat="server" Text="Location ID:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtLocID" runat="server" Enabled="false" Width="100px" CssClass="RequiredField"></asp:TextBox>
                </td>
                
            </tr>
              <tr>
                <td>
                    <asp:Label ID="lblLocCode" runat="server" Text="Location Code:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtLocCode" runat="server" Width="100px" MaxLength="15" CssClass="RequiredField"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="req_LocCode" runat="server" ControlToValidate="txtLocCode"
                        ErrorMessage="Please enter Location Code" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                </td>
                
            </tr>
            
            <tr>
                <td>
                   <asp:Label ID="lblLocName" runat="server" Text="Location Name:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtLocName" runat="server" Width="300px" MaxLength="100" CssClass="RequiredField"></asp:TextBox>
                  <asp:RequiredFieldValidator ID="req_LocName" runat="server" ControlToValidate="txtLocName"
                        ErrorMessage="Please enter Location Name" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                </td>
                
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblLocAddress" runat="server" Text="Location Address:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtLocAdd" CssClass="RequiredField" runat="server" Width="300px" MaxLength="200"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="req_LocAdd" runat="server" ControlToValidate="txtLocAdd"
                        ErrorMessage="Please enter Location Address" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                </td>
                
            </tr>
         
            <tr>
                <td>
                    <asp:Label ID="lblStoreCat" runat="server" Text="Store Category:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStoreCat" runat="server" CssClass="RequiredField">
                        <asp:ListItem Value="0">Select Store Category</asp:ListItem>
                        <asp:ListItem Value="M">Main Store</asp:ListItem>
                        <asp:ListItem Value="S">Sub Store</asp:ListItem>
                        <asp:ListItem Value="P">Project Store</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="req_SotreCat" runat="server" ControlToValidate="ddlStoreCat"
                        ErrorMessage="Please select Store Category" InitialValue="0" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                        
                </td>
                
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblStatus" runat="server" Text="Status:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="RequiredField">
                        <asp:ListItem Value="0">Select Status</asp:ListItem>
                        <asp:ListItem Value="A">Active</asp:ListItem>
                        <asp:ListItem Value="I">Inactive</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="req_Status" runat="server" ControlToValidate="ddlStatus"
                        ErrorMessage="Please select Status" InitialValue="0" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                        
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
                    <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server"/>
                </td>
            </tr>

            </asp:Panel>  
          
          <tr>
            <%--<td valign="top">&nbsp;</td>--%>
            <td valign="top">
        
            <%--i did (buttons)--%>
            
            
            <!--img src="images/btn_new.jpg" width="60" height="20" /> <img src="images/btn_edit.jpg" width="60" height="20" /> <img src="images/btn_delete.jpg" alt="" width="60" height="20" /> <img src="images/btn_save.jpg" width="60" height="20" /> <img src="images/btn_cancel.jpg" width="60" height="20" /-->
            </td>
            
          </tr>

        </table>
        
        </td>
        <td></td>
      </tr>
      <tr><td colspan="3">&nbsp;</td></tr>
      <tr>
        <td></td>
        <td>
            <table class="filterTable" cellpadding="1" cellspacing="2" width="100%"><tr><td colspan="9">&nbsp;</td></tr>
              <tr>
                <td>&nbsp;</td>
                <td>Loc. Code</td>
                <td>
                    <asp:TextBox runat="server" ID="txtSrchLocCode" Width="80" class="filter"></asp:TextBox>
                </td>
                <td>Loc. Name</td>
                <td>
                    <asp:TextBox runat="server" ID="txtSrchLocName" class="filter"></asp:TextBox>
                </td>
                <td>Store Cat.</td>
                <td>
                   <asp:DropDownList ID="ddlSrchStoreCat" runat="server" CssClass="RequiredField">
                        <asp:ListItem Value="0">All</asp:ListItem>
                        <asp:ListItem Value="M">Main Store</asp:ListItem>
                        <asp:ListItem Value="S">Sub Store</asp:ListItem>
                        <asp:ListItem Value="P">Project Store</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>Status</td>
                <td>
                     <asp:DropDownList ID="ddlSrchStatus" runat="server" CssClass="RequiredField">
                        <asp:ListItem Value="0">All</asp:ListItem>
                        <asp:ListItem Value="A">Active</asp:ListItem>
                        <asp:ListItem Value="I">Inactive</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/search-icon-blue.gif" class="filterclick"
                          OnClick="btnsearch_Click" ToolTip="Search Store Location" Visible="true"/>
                 </td>
                <td>&nbsp;</td>
            </tr>
            <tr><td colspan="9">&nbsp;</td></tr></table>
        </td>
        <td></td>
      </tr>
      <tr>
          <td></td>
          <td>
          <asp:GridView ID="grdStockLoc" runat="server" DataKeyNames="LocId, br_id" 
                    OnSelectedIndexChanged="grdStockLoc_SelectedIndexChanged" 
                    OnPageIndexChanging="grdStockLoc_PageIndexChanging" 
                    OnRowDataBound="grdStockLoc_RowDataBound" 
                    AutoGenerateColumns="False" AllowPaging="True" Width="100%" PageSize="25" > 
                    <HeaderStyle CssClass ="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                    <Columns>
                        <asp:BoundField DataField="LocId" HeaderText="Loc. ID"  />
                        <asp:BoundField DataField="LocCode" HeaderText="Loc. Code" />
                        <asp:BoundField DataField="LocName" HeaderText="Loc. Name" />
                        <asp:BoundField DataField="LocAddress" HeaderText="Loc. Address" />
                        <asp:BoundField DataField="LocCategory" HeaderText="Store Category"  />
                        <asp:BoundField DataField="Status" HeaderText="Status"  />
                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                            <ItemStyle />
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
          
          </td>
          <td></td>
          </tr>
    </table>

            
             
    

</asp:Content>
