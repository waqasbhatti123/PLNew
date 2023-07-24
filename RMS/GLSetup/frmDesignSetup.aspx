<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
Culture="auto" UICulture="auto" EnableEventValidation="false" 
CodeBehind="frmDesignSetup.aspx.cs" Inherits="RMS.GL.Setup.frmDesignSetup"  %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List" ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
    <br />
    
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
      <td width="3%"></td>
      <td>
      <asp:Panel ID="pnlFields" runat="server">
      
            <table>
                <tr>
                <td  class="LblBgSetup">
                    <asp:Label ID="lblDesignCode" runat="server" Text="Design Code:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDesignCode" runat="server" CssClass="RequiredFieldTxtSmall" MaxLength="5"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="rfv1" ValidationGroup="main" ErrorMessage="Please enter design code." SetFocusOnError="true" ControlToValidate="txtDesignCode" Display="None" ></asp:RequiredFieldValidator>
                </td>
                <td  class="LblBgSetup">
                    <asp:Label ID="lblDescription" runat="server" Text="Design Desc:"></asp:Label>
                </td>
                <td>
                     <asp:TextBox ID="txtDescription" runat="server" CssClass="RequiredField" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="main" ErrorMessage="Please enter design description." SetFocusOnError="true" ControlToValidate="txtDescription" Display="None" ></asp:RequiredFieldValidator>
                </td>
                <td  class="LblBgSetup">
                    <asp:Label ID="lblStatus" runat="server" Text="Status:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server" >
                        <asp:ListItem Text="Enable" Value="A" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Disable" Value="C"></asp:ListItem>
                    </asp:DropDownList> 
                </td>
                </tr>
                
                
                
                <tr>
                <td  class="LblBgSetup">
                    <asp:Label ID="lblThicknessCode" runat="server" Text="Thickness Code:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlThick" runat="server" AppendDataBoundItems="true" CssClass="RequiredFieldDropDown">
                    <asp:ListItem Text="Select Code" Value="0" Selected="True" >
                    </asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="main" ErrorMessage="Please select thickness code." SetFocusOnError="true" ControlToValidate="ddlThick" InitialValue="0" Display="None" ></asp:RequiredFieldValidator>
                </td>
                 <td  class="LblBgSetup">
                     <asp:Label ID="lblColorCode" runat="server" Text="Color Code:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlColor" runat="server" AppendDataBoundItems="true" CssClass="RequiredFieldDropDown">
                    <asp:ListItem Text="Select Code" Value="0" Selected="True" >
                    </asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ValidationGroup="main" ErrorMessage="Please selct color code." SetFocusOnError="true" ControlToValidate="ddlColor" InitialValue="0" Display="None" ></asp:RequiredFieldValidator>
                </td>
                <td> &nbsp; </td>
                <td> &nbsp; </td>
                </tr>
                
                
                <tr>
                <td> &nbsp; </td>
                <td> &nbsp; </td>
                <td> &nbsp; </td>
                <td> &nbsp; </td>
                <td> &nbsp; </td>
                <td> &nbsp; </td>
                </tr>
                
                
                
                <tr>
                <td>
                    <asp:ImageButton ID="btnSave" runat="server" OnCommand="btnSave_click" ImageUrl="~/images/btn_save.png" onMouseOver="this.src='../images/btn_save_m.png'" onMouseOut="this.src='../images/btn_save.png'" ValidationGroup="main" CausesValidation="true" />
                    <asp:ImageButton ID="btnClear" runat="server" OnCommand="btnClear_click" ImageUrl="~/images/btn_clear.png" onMouseOver="this.src='../images/btn_clear_m.png'" onMouseOut="this.src='../images/btn_clear.png'"/>
                </td>
                <td> &nbsp; </td>
                <td> &nbsp; </td>
                <td> &nbsp; </td>
                <td> &nbsp; </td>
                <td> &nbsp; </td>
                </tr>
                
                
                <tr>
                <td> &nbsp; </td>
                <td> &nbsp; </td>
                <td> &nbsp; </td>
                <td> &nbsp; </td>
                <td> &nbsp; </td>
                <td> &nbsp; </td>
                </tr>
             </table>
      
      </asp:Panel>
      </td>
      <td width="3%"></td>
      </tr>
      
      
      <tr>
      <td width="3%"></td>
      <td>
               <asp:GridView ID="grdViewItemD" runat="server" AutoGenerateColumns="false" OnPageIndexChanging="grdViewItemD_PageIndexChanging" OnRowDataBound="grdViewItemD_RowDataBound" OnSelectedIndexChanged="grdViewItemD_SelectedIndexChanged" EmptyDataText="No record found." PageSize="20" AllowPaging="true" Width="100%">
               <HeaderStyle CssClass="grid_hdr" />
               <RowStyle CssClass="grid_row" />
               <AlternatingRowStyle CssClass="gridAlternateRow" />
               <SelectedRowStyle CssClass="gridSelectedRow" />
               <Columns>
               
               <asp:BoundField DataField="DesignId" HeaderText="Design Id" />
               <asp:BoundField DataField="DesignDesc" HeaderText="Design Desc" />
               <asp:BoundField DataField="ThickId" HeaderText="Thick Id" />
               <asp:BoundField DataField="ThickDesc" HeaderText="Thick Desc" />
               <asp:BoundField DataField="Thick" HeaderText="Thickness" />
               <asp:BoundField DataField="ColorId" HeaderText="Color Id" />
               <asp:BoundField DataField="ColorDesc" HeaderText="Color Desc" />
               <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Center"/>
               
               <asp:CommandField ShowSelectButton="true" HeaderText="Option"  ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="lnk"/>

               </Columns>
               </asp:GridView>
      </td>
      <td width="3%"></td>
      </tr>
      </table>

</asp:Content>
