<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="RegionMgt.aspx.cs" Inherits="RMS.Setup.RegionMgt"%>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main"/>
    <uc1:Messages ID="ucMessage" runat="server" />
    
    
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="3%"></td>
            <td height="10" colspan="2"></td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td width="34%" valign="top">
            
             
             <asp:GridView ID="grdRegs" runat="server" DataKeyNames="CodeID" OnSelectedIndexChanged="grdRegs_SelectedIndexChanged" 
                    AutoGenerateColumns="False" Width="70%"  AllowPaging="True" OnRowDataBound="grdRegs_RowDataBound" OnPageIndexChanging="grdRegs_PageIndexChanging">
                    <HeaderStyle CssClass ="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                    <Columns>
                        
                        <asp:BoundField DataField="CodeDesc" HeaderText="Region Name"/>
                        <asp:BoundField DataField="Enabled" HeaderText="Status"/>
                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                            <ItemStyle />
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
				
             
             
            
            </td>
            <td width="63%" valign="top">
             
                
             <table width="270" cellspacing="0" class="stats1" align="left">
             <asp:Panel runat="server" ID="pnlMain">
              <tr>
                <td colspan="2" valign="top" class="bg_input_area"></td>
              </tr>
              <tr>
                <td>
                    <asp:Label ID="lblName" runat="server" Text="Region Name:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRegion" CssClass="RequiredField" runat="server" MaxLength="80"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtRegion"
                        ErrorMessage="Please enter region name" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                </td>
                
            </tr>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblEnable" runat="server" Text="Status:"></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblStatus" runat="server" 
                        RepeatDirection="Horizontal" Width="80%">
                        <asp:ListItem Value="1" Selected="True">Enable</asp:ListItem>
                        <asp:ListItem Value="0">Disable</asp:ListItem>
                    </asp:RadioButtonList>
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

            </asp:Panel>  
          
          <tr>
            <td valign="top">&nbsp;</td>
            <td valign="top">
        
            <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
            <!--img src="images/btn_new.jpg" width="60" height="20" /> <img src="images/btn_edit.jpg" width="60" height="20" /> <img src="images/btn_delete.jpg" alt="" width="60" height="20" /> <img src="images/btn_save.jpg" width="60" height="20" /> <img src="images/btn_cancel.jpg" width="60" height="20" /-->
            </td>
            
          </tr>
          
        </table>
        
        </td>
      </tr>
    </table>
        
</asp:Content>
