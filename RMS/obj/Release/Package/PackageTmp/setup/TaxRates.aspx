<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="TaxRates.aspx.cs" Inherits="RMS.Setup.TaxRates" Title="TaxRates" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main"/>
    <asp:ValidationSummary ID="taxRate" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="taxRate"/>
    <uc1:Messages ID="ucMessage" runat="server" />
    <asp:Panel ID="pnlMain" runat="server">
    <table width="100%" >
     <tr>
        <td width="70%">
        <fieldset>
            <legend style="margin:5px;"> <b>Tax Description </b></legend>
        <table width="100%">
            <tr>
                <td>
                    <asp:Label ID="lblTaxType" runat="server" Text="Tax Type" ></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlTaxType" runat="server">
                        <asp:ListItem Value="GST" Selected="True">GST</asp:ListItem>
                        <asp:ListItem Value="WHT">WHT</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblTaxDesc" runat="server" Text="Tax Description"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDesc" runat="server" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="DescValidator" ControlToValidate="txtDesc" 
                    ValidationGroup="main" ErrorMessage="Fill Description Field" Display="None" SetFocusOnError="true"></asp:RequiredFieldValidator>
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
            
             
             <asp:GridView ID="grdTax" runat="server"  Width="100%" AutoGenerateColumns="False" EmptyDataText="No Record Found"
                    AllowPaging="True" PageSize="5" DataKeyNames="TaxID"  OnSelectedIndexChanged="grdTax_SelectedIndexChanged" 
                    OnPageIndexChanging="grdTax_PageIndexChanging">
                    <HeaderStyle CssClass ="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                    <Columns>
                        <asp:BoundField  DataField="Type" HeaderText="Taxt Type"/>
                        <asp:BoundField DataField="TaxDesc" HeaderText="Tax Description" />
                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                            <ItemStyle />
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
				
             
             
            
            </td>
          </tr>
         </table>
      </fieldset>
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
            
           <%--   Tax Rate --%>
      <tr>
        <td>
            <fieldset>
                <legend style="margin:5px;"><b>Tax Rates</b></legend>
          <table width="100%">
           <tr>
            <td>
                <asp:Label ID="lblDesc" runat="server" Text="Tax Description"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlDesc" runat="server" ValidationGroup="taxRate" AppendDataBoundItems="true">
                    <asp:ListItem Value="0" Selected= "True">Select Tax Description</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlDesc" 
                    ValidationGroup="taxRate" ErrorMessage="Please select Tax Description" Display="None" InitialValue="0" SetFocusOnError="true"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlStatus" runat="server">
                    <asp:ListItem Selected="True" Value="OP">Active</asp:ListItem>
                    <asp:ListItem  Value="CL">InActive</asp:ListItem>
                </asp:DropDownList>
            </td>
           </tr> 
           <tr>
            <td>
                <asp:Label ID="lblDate" runat="server" Text="Effective Date"></asp:Label>
            </td>
            <td>
                <ajaxToolkit:CalendarExtender ID="txtdtEffective" runat="server" TargetControlID="txtEffectiveDate" Enabled="true"></ajaxToolkit:CalendarExtender>
                <asp:TextBox ID="txtEffectiveDate" runat="server" Width="80px"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="lblRate" runat="server" Text="TaxRate"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtRate" runat="server" ValidationGroup="taxRate" width="70px" style="text-align:right"></asp:TextBox>
                <asp:Literal ID="ltrlRate" runat="server" Text="%"></asp:Literal>
                <asp:RangeValidator ID="rngTxtRate" runat="server" ControlToValidate="txtRate" MinimumValue="0.01" MaximumValue="99.99"
                 ValidationGroup="taxRate" Display="None" Type="Double" ErrorMessage="Rate must be within  0 and 100"></asp:RangeValidator>
                 <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtRate" 
                    ValidationGroup="taxRate" ErrorMessage="Please Fill Tax Rate Field" Display="None" SetFocusOnError="true"></asp:RequiredFieldValidator>
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
        
            <uc2:Buttons ID="Buttons1" OnButtonClick="ButtonCommand_click" runat="server" ValidationGroupName="taxRate" />
            <!--img src="images/btn_new.jpg" width="60" height="20" /> <img src="images/btn_edit.jpg" width="60" height="20" /> <img src="images/btn_delete.jpg" alt="" width="60" height="20" /> <img src="images/btn_save.jpg" width="60" height="20" /> <img src="images/btn_cancel.jpg" width="60" height="20" /-->
            </td>
            
          </tr>
          
           <tr>
            <td colspan="4">
            
             
             <asp:GridView ID="grdTaxRate" runat="server"  Width="100%" AutoGenerateColumns="False" EmptyDataText="No Record Found" 
                    AllowPaging="True" PageSize="10" OnPageIndexChanging="grdTaxRate_PageIndexChanging" DataKeyNames="TaxID, EffDate, TaxIdTaxType"
                      OnRowDataBound="grdTaxRate_RowDataBound">
                    <HeaderStyle CssClass ="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                    <Columns>
                        <asp:BoundField DataField="Type" HeaderText="Tax Type" />
                        <asp:BoundField DataField="TaxDesc" HeaderText="Tax Description" />
                        <asp:BoundField DataField="EffDate"  HeaderText="Effective Date"/>
                        <asp:BoundField DataField="TaxRate"  HeaderText="Taxt Rate"/>
                        <asp:TemplateField >
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server"  Text="Select" CssClass="lnk" OnClick="Select_Click"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            
                </td>
              </tr>
            </table>
          </fieldset>
          </td>
         </tr>
     </table>
  </asp:Panel>
</asp:Content>
