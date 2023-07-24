<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="SalesPromotionResponse.aspx.cs" Inherits="RMS.sales.SalesPromotionResponse"
    Culture="auto" UICulture="auto" EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        $(document).ready(function() {
        });
       
    </script>

    <style type="text/css">
        .grd
        {
            /* margin:0 auto !important; */
        }
        .cln
        {
            /*position:absolute;*/
            background-color:White;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
    <asp:Panel ID="pnlTop" runat="server" Visible="false">
    </asp:Panel>
    <div style="width: 100%;">
        <%--<asp:UpdatePanel ID="updPnl" runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
        <table style="width: 85%;">
            <tr>
                <td>
                    <asp:Label ID="lblDocNo" runat="server" Text="Doc No:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDocNo" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblDocDate" runat="server" Text="Doc Date:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDocDate" runat="server"></asp:TextBox>
                    
                    <ajaxToolkit:CalendarExtender ID="calDocDate" runat="server" TargetControlID="txtDocDate" CssClass="cln">
                    </ajaxToolkit:CalendarExtender>
                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDocDate"
                        ErrorMessage="Please select document date" SetFocusOnError="true" ValidationGroup="main"
                        Display="None"></asp:RequiredFieldValidator>
                    <span class="DteLtrl">
                        <asp:Literal ID="Literal2" runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblOutlet" runat="server" Text="Outlet:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlOutlet" runat="server" AppendDataBoundItems="true" Width="50%">
                        <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlOutlet"
                        ErrorMessage="Please select outlet" SetFocusOnError="true" ValidationGroup="main"
                        Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblArtifactType" runat="server" Text="Artifact Type:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlArtifactType" runat="server" AppendDataBoundItems="true"
                        OnSelectedIndexChanged="ddlArtifactType_SelectedIndexChanged" Style="width: 50%;"
                        AutoPostBack="true">
                        <asp:ListItem Value="0" Text="Select" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlArtifactType"
                        ErrorMessage="Please select artifact type" SetFocusOnError="true" ValidationGroup="main"
                        InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblResponseType" runat="server" Text="Response Type:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlResponseType" runat="server" AppendDataBoundItems="true"
                        OnSelectedIndexChanged="ddlResponseType_SelectedIndexChanged" Width="50%" AutoPostBack="true">
                        <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlResponseType"
                        ErrorMessage="Please select response type" SetFocusOnError="true" ValidationGroup="main"
                        Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
    </div>
    <div style="width: 100%;">
        <div style="width: 100%; margin-top: 25px;">
            <asp:GridView ID="gvResp" runat="server" AutoGenerateColumns="false" CssClass="grd">
                <Columns>
                </Columns>
                <HeaderStyle CssClass="grid_hdr" />
                <RowStyle CssClass="grid_row" />
                <AlternatingRowStyle CssClass="gridAlternateRow" />
            </asp:GridView>
        </div>
        <div style="width: 100%; padding-top: 25px;">
            <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
        </div>
    </div>
    <div style="width: 100%; clear: both; margin-top: 50px;">
        <table class="filterTable" width="100%">
            <tr>
                <td>
                </td>
                <td>
                    <asp:Label ID="lblFltDocno" runat="server" Text="Document No:"></asp:Label>
                    <asp:TextBox runat="server" ID="txtFltDocNo" Width="100"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Outlet:"></asp:Label>
                    <asp:DropDownList ID="ddlFltOutlet" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="All" Value="0" />
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Artifact Type:"></asp:Label>
                    <asp:DropDownList ID="ddlFltArtType" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="All" Value="0" />
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="Response Type:"></asp:Label>
                    <asp:DropDownList ID="ddlFltRespType" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="All" Value="0" />
                    </asp:DropDownList>
                </td>
                <%--<td>
                            <asp:Label ID="Label2" runat="server" Text="Response Type:"></asp:Label>
                            <asp:DropDownList  ID="DropDownList1" runat="server">
                                <asp:ListItem Text="All" Value="0" />
                                <asp:ListItem Text="Approved" Value="A" />
                                <asp:ListItem Text="Pending" Value="P" />
                                <asp:ListItem Text="Cancelled" Value="C" />    
                            </asp:DropDownList>
                        </td>--%>
                <td>
                    <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                        OnClick="btnSearch_Click" ToolTip="Search sales promotion responses" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="grd" runat="server" DataKeyNames="vr_id, DlrId, ArtTypeId, RespTypeId"
            OnSelectedIndexChanged="grd_SelectedIndexChanged" OnPageIndexChanging="grd_PageIndexChanging"
            OnRowDataBound="grd_RowDataBound" AutoGenerateColumns="False" AllowPaging="True"
            EmptyDataText="No sales promotion data found" Width="100%">
            <Columns>
                <asp:BoundField DataField="doc_no" HeaderText="Doc No" />
                <asp:BoundField DataField="doc_dt" HeaderText="Doc Date" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="outlet" HeaderText="Outlet" />
                <asp:BoundField DataField="artType" HeaderText="Artifact Type" />
                <asp:BoundField DataField="respType" HeaderText="Response Type" />
                <%--<asp:BoundField DataField="vr_apr" HeaderText="Status" /> --%>
                <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                    <ControlStyle CssClass="lnk"></ControlStyle>
                </asp:CommandField>
            </Columns>
            <HeaderStyle CssClass="grid_hdr" />
            <RowStyle CssClass="grid_row" />
            <AlternatingRowStyle CssClass="gridAlternateRow" />
            <SelectedRowStyle CssClass="gridSelectedRow" />
        </asp:GridView>
    </div>
</asp:Content>
