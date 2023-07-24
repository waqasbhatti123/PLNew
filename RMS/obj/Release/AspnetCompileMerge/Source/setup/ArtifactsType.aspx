<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="ArtifactsType.aspx.cs" Inherits="RMS.Setup.ArtifactsType"
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
            <td width="34%" valign="top">
                <asp:GridView ID="grdArtifactsType" runat="server" DataKeyNames="ArtTypeId" Width="325px" OnSelectedIndexChanged="grdArtifactsType_SelectedIndexChanged"
                    AutoGenerateColumns="False" AllowPaging="True" PageSize="13" OnPageIndexChanging="grdArtifactsType_PageIndexChanging">
                    <HeaderStyle CssClass="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                    <Columns>
                        <asp:BoundField DataField="Desc" HeaderText="Artifact Type" />
                        <asp:CheckBoxField DataField="Enabled" HeaderText="Enabled" ItemStyle-HorizontalAlign="Center" />
                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                            <ItemStyle />
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
            </td>
            <td width="63%" valign="top">
                <table width="613" class="stats1" align="left">
                    <asp:Panel runat="server" ID="pnlMain">
                        <tr>
                            <td colspan="2" valign="top" class="bg_input_area">
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblName" runat="server" Text="Artifact Type:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtArtifactType" CssClass="RequiredField" runat="server" MaxLength="100" style="width:250px;"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtArtifactType"
                                    ErrorMessage="Please enter artifact type" SetFocusOnError="true" ValidationGroup="main"
                                    Display="None"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblEnabled" runat="server" Text="Enable:"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkEnabled" runat ="server" Checked="true" />
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
