<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="ChangeLot.aspx.cs" Inherits="RMS.InvSetup.ChangeLot" Culture="auto" UICulture="auto"
    EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
</script>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <uc1:Messages ID="ucMessage" runat="server" />
        <uc1:Messages ID="ucMessage1" runat="server" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnSave"/>
        <asp:AsyncPostBackTrigger ControlID="lnkExstLot"/>
        <asp:AsyncPostBackTrigger ControlID="lnkChkStts"/>
        <asp:AsyncPostBackTrigger ControlID="GridView1"/>
        <asp:AsyncPostBackTrigger ControlID="GridView2"/>
    </Triggers>
    </asp:UpdatePanel>
  
    <br />
  
  
  <asp:UpdatePanel ID="mainUpdPnl" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
  
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
      <td width="3%"></td>
      <td>
      
            <table>
                <tr>
                <td  class="LblBgSetup">
                    <asp:Label ID="lblExistingLot" runat="server" Text="Existing Lot No."></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtExistingLot" runat="server" CssClass="RequiredField" Width="80px" MaxLength="9"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="r1" runat="server" ControlToValidate="txtExistingLot" Display="None" ErrorMessage="Please enter exisisting lot no. to change." SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>
                </td>
                <td> 
                    <asp:LinkButton ID="lnkExstLot" runat="server" Text="Check Status" OnClick="lnkExstLot_Click" CssClass="lnk" ToolTip="Check status of existing lot no."></asp:LinkButton>
                </td>
                </tr>
                
                <tr>
                <td   class="LblBgSetup">
                    <asp:Label ID="lblNewLot" runat="server" Text="New Lot No."></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNewLot" runat="server" CssClass="RequiredField" Width="80px" MaxLength="9"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="r2" runat="server" ControlToValidate="txtNewLot" Display="None" ErrorMessage="Please enter new lot no." SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:LinkButton ID="lnkChkStts" runat="server" Text="Check Status" OnClick="lnkChkStts_Click" CssClass="lnk" ToolTip="Check status of new lot no."></asp:LinkButton>
                </td>
                </tr>
                
                <tr>
                <td   class="LblBgSetup">
                    <asp:Label ID="lblConfrmLot" runat="server" Text="Re-Type Lot No."></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtConfirmLot" runat="server" CssClass="RequiredField" Width="80px" MaxLength="9"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="r3" runat="server" ControlToValidate="txtConfirmLot" Display="None" ErrorMessage="Please enter confirmation lot no." SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>
                </td>
                <td> &nbsp; </td>
                </tr>
                
                <tr>
                <td> &nbsp; </td>
                <td> 
                    <asp:Label ID="lblInfo" runat="server" Text="Format:yymm-nn" CssClass="lnk"></asp:Label>
                </td>
                <td> &nbsp; </td>
                </tr>
                
                <tr>
                <td>
                    <asp:ImageButton ID="btnSave" runat="server"  ImageUrl="~/images/btn_save.png" OnClick="btnSave_Click" onMouseOver="this.src='../images/btn_save_m.png'" onMouseOut="this.src='../images/btn_save.png'" ValidationGroup="main" />   
                    <asp:ImageButton ID="btnClear" runat="server"  ImageUrl="~/images/btn_clear.png" OnClick="btnClear_Click" onMouseOver="this.src='../images/btn_clear_m.png'" onMouseOut="this.src='../images/btn_clear.png'" />
                </td>
                <td> &nbsp; </td>
                <td> &nbsp; </td>
                </tr>
                
                <tr>
                <td> &nbsp; </td>
                <td> &nbsp; </td>
                <td> &nbsp; </td>
                </tr>
                
            </table>
            
      
      </td>
      <td width="3%"></td>
      </tr>
      
      
      
      
      <tr>
      <td width="3%"></td>
      <td>
            <div>
                <b><asp:Label ID="grd2Hd" runat="server" Text="Existing Lot Status" Visible="false"></asp:Label></b>
            </div>
            <div>
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" Width="98%" Visible="false" >
                    
                    <HeaderStyle CssClass ="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                
                    <Columns>
                        <asp:BoundField DataField="strLotNo" HeaderText="Lot No."/>
                        <asp:BoundField DataField="IgpNo" HeaderText="Strt IGP."/>
                        <asp:BoundField DataField="Broker" HeaderText="Broker"/>
                        <asp:BoundField DataField="Product" HeaderText="Product"/>
                        <asp:BoundField DataField="LotQty" HeaderText="Pieces" ItemStyle-HorizontalAlign="Right"/>
                    </Columns>
                    
                </asp:GridView>
            </div>
      </td>
      <td width="3%"></td>
      </tr>
      
      
      <tr>
      <td width="3%"></td>
      <td> &nbsp; </td>
      <td width="3%"></td>
      </tr>
      
      
      <tr>
      <td width="3%"></td>
      <td>
            <div>
                <b><asp:Label ID="grd1Hd" runat="server" Text="New Lot Status" Visible="false"></asp:Label></b>
            </div>
            <div>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" Width="98%" Visible="false" >
                    
                    <HeaderStyle CssClass ="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                
                    <Columns>
                        <asp:BoundField DataField="strLotNo" HeaderText="Lot No."/>
                        <asp:BoundField DataField="IgpNo" HeaderText="Strt IGP."/>
                        <asp:BoundField DataField="Broker" HeaderText="Broker"/>
                        <asp:BoundField DataField="Product" HeaderText="Product"/>
                        <asp:BoundField DataField="LotQty" HeaderText="Pieces" ItemStyle-HorizontalAlign="Right"/>
                    </Columns>
                    
                </asp:GridView>
            </div>
      </td>
      <td width="3%"></td>
      </tr>
      
      </table>
  
  </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
