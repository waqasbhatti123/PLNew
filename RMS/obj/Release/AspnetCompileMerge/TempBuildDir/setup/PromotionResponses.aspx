<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="PromotionResponses.aspx.cs" Inherits="RMS.Setup.PromotionResponses"
    Title="Response Type" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="3%">
            </td>
            <td height="10" colspan="2">
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td width="45%" valign="top">
                <asp:GridView ID="grdResponse" runat="server" DataKeyNames="RespId" Width="350px"
                    OnSelectedIndexChanged="grdResponse_SelectedIndexChanged" AutoGenerateColumns="False"
                    AllowPaging="True" PageSize="13" OnPageIndexChanging="grdResponse_PageIndexChanging">
                    <HeaderStyle CssClass="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                    <Columns>
                    <asp:BoundField DataField="ResponseType" HeaderText="Response Type" />
                        <asp:BoundField DataField="Desc" HeaderText="Response" />
                        <asp:BoundField DataField="RespCode" HeaderText="Abv" />
                        <asp:CheckBoxField DataField="Critical" HeaderText="Critical" ItemStyle-HorizontalAlign="Center" />
                        <asp:CheckBoxField DataField="Enabled" HeaderText="Enabled" ItemStyle-HorizontalAlign="Center" />
                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                            <ItemStyle />
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
            </td>
            <td width="50%" valign="top">
                <table width="500" class="stats1" align="left">
                    <asp:Panel runat="server" ID="pnlMain">
                        <tr>
                            <td colspan="2" valign="top" class="bg_input_area">
                            </td>
                        </tr>
                        
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblResponseType" runat="server" Text="Response Type:"></asp:Label>
                            </td>
                            <td style="padding-left: 10px;">
                                <asp:DropDownList ID="ddlResponseType" runat="server" AppendDataBoundItems="true" style="width:200px;">
                                    <asp:ListItem Value="0" Text="Select Type" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlResponseType"
                                    ErrorMessage="Please select response type" SetFocusOnError="true" ValidationGroup="main"
                                    InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblName" runat="server" Text="Response:"></asp:Label>
                            </td>
                            <td style="padding-left: 10px;">
                                <asp:TextBox ID="txtResponse" CssClass="RequiredField" runat="server" MaxLength="100" style="width:200px;"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtResponse"
                                    ErrorMessage="Please enter response" SetFocusOnError="true" ValidationGroup="main"
                                    Display="None"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblRespCode" runat="server" Text="Abv:"></asp:Label>
                            </td>
                            <td style="padding-left: 10px;">
                                <asp:TextBox ID="txtRespCode" CssClass="RequiredField" runat="server" MaxLength="2" style="width:100px;"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtRespCode"
                                    ErrorMessage="Please enter response code" SetFocusOnError="true" ValidationGroup="main"
                                    Display="None"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblCritical" runat="server" Text="Critical:"></asp:Label>
                            </td>
                            <td style="padding-left: 10px;">
                                <asp:CheckBox ID="chkCritical" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblEnabled" runat="server" Text="Enable:"></asp:Label>
                            </td>
                            <td style="padding-left: 10px;">
                                <asp:CheckBox ID="chkEnabled" runat="server" Checked="true" />
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
                        <td valign="top">
                            &nbsp;
                        </td>
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
