<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="ItemRate.aspx.cs" Inherits="RMS.sales.ItemRate" 
    Culture="auto" UICulture="auto"%>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List" ValidationGroup="main"/>
    
    <uc1:Messages ID="ucMessage" runat="server" />
    <table cellpadding="0" border="0" width="80%">
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:Panel runat="server" ID="pnlMain">
                    <table  class="table">
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblName" runat="server" Text="Party:" ></asp:Label>
                            <br />
                                <asp:DropDownList ID="ddlParty" runat="server" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlParty_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblEffDate" runat="server" Text="Effective Date:"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtEffDate" runat="server" Text='<%#Eval("EffDate") %>' Width="100px"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="calEffDate" runat="server" TargetControlID="txtEffDate" Format="dd-MMM-yyyy" PopupPosition="BottomRight" >
                                                </ajaxToolkit:CalendarExtender>
                                                <asp:RequiredFieldValidator ID="reqED" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtEffDate" ErrorMessage="Effective date required"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        
                        <tr>
                            <td colspan="4">
                                <asp:GridView ID="grdParty" runat="server" AutoGenerateColumns="False" DataKeyNames="itm_cd"
                                     CssClass="t_grd" OnRowDataBound="grdParty_RowDataBound">
                                    <HeaderStyle CssClass="t_grd_hdr"></HeaderStyle>
                                    <FooterStyle CssClass="t_grd_footer"></FooterStyle>
                                    <RowStyle CssClass="t_grd_Row"></RowStyle>
                                    <EditRowStyle CssClass="t_grd_Edit_Row" />
                                    <SelectedRowStyle CssClass="t_grd_Selected_Row"  />
                                    <AlternatingRowStyle CssClass="t_grd_Alter_Row"/>
                                    <PagerStyle CssClass="t_grd_Pager"></PagerStyle>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <EmptyDataRowStyle CssClass="Label_Small_Bold" ForeColor="#C00000" HorizontalAlign="Center" />
                                      
                                    <Columns>
                                        <asp:TemplateField HeaderText="Effective Date" HeaderStyle-Width="250px"  ControlStyle-Width="250px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtItem" runat="server" Text='<%#Eval("itm_dsc") %>' ReadOnly="true" TabIndex="-1"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="250px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Previous Sale Rate" ControlStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPrevSaleRate" runat="server" Text='<%#Eval("prevSaleRate") %>' style="text-align:right;" MaxLength="7" ReadOnly="true" TabIndex="-1"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sale Rate" ControlStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSaleRate" runat="server" Text='<%#Eval("SaleRate") %>' style="text-align:right;" MaxLength="7"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqSR" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtSaleRate" ErrorMessage="Sale rate required"></asp:RequiredFieldValidator>
                                                <asp:RangeValidator runat="server" ID="rvSR" CssClass="validateGridView" ValidationGroup="main" ErrorMessage="Invalid/Out of range sale rate"
                                                        ControlToValidate="txtSaleRate" MinimumValue="0.00" MaximumValue="9999999.99" Type="Double" Display="Dynamic" SetFocusOnError="true"></asp:RangeValidator>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        </asp:TemplateField>                                      
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
        <td>&nbsp;</td>
        <td>
            <asp:ImageButton ImageUrl="../images/btn_save.png" ID="btnSave" 
            runat="server" CssClass="buttonSave" OnClick="btnSave_Click" 
            onMouseOver="this.src='../images/btn_save_m.png'" onMouseOut="this.src='../images/btn_save.png'" 
            ValidationGroup="main"/>
        </td>
        <td>&nbsp;</td>
        </tr>
    </table>

</asp:Content>
