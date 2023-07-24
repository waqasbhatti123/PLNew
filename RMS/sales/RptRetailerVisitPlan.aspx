<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="RptRetailerVisitPlan.aspx.cs" Inherits="RMS.sales.RptRetailerVisitPlan"
    Title="Retailer Visit Plan Report" Culture="auto" UICulture="auto" EnableEventValidation="true" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" 
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        select
        {
            width: 120px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:Messages ID="ucMessage" runat="server" />
    <br />
    <div>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblArea" runat="server" Text="Area :"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlArea" runat="server" AutoPostBack="true" AppendDataBoundItems="true"
                        OnSelectedIndexChanged="ddlArea_SelectedIndexChanged">
                        <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblSubArea" runat="server" Text="Sub Area :"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSubArea" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblSalesman" runat="server" Text="Salesman :"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSalesman" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="From :"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" Width="117px">
                    </asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="calFromDate" runat="server" PopupPosition="BottomRight"
                        TargetControlID="txtFromDate">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td>
                    <asp:Label ID="lblToDate" runat="server" Text="To :"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" Width="117px">
                    </asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="calToDate" runat="server" PopupPosition="BottomRight"
                        TargetControlID="txtToDate">
                    </ajaxToolkit:CalendarExtender>
                </td>
                <td>
                    <asp:Label ID="lblSortBy" runat="server" Text="Sort By :"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSort" runat="server">
                        <asp:ListItem Text="Date" Value="date"></asp:ListItem>
                        <asp:ListItem Text="Salesman" Value="salesman"></asp:ListItem>
                        <asp:ListItem Text="Area" Value="area"></asp:ListItem>
                        <asp:ListItem Text="SubArea" Value="subarea"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="padding-left: 10px;">
                    <asp:Button ID="btnGenerat" runat="server" Text="Generate Report" OnClick="btnGenerat_Click" />
                </td>
            </tr>
        </table>
    </div>
    <br />
    <asp:Panel ID="pnlMain" runat="server" Width="780px" Height="600">
        <rsweb:ReportViewer ID="viewer" runat="server" Visible="false" Width="100%" Height="580px">
        </rsweb:ReportViewer>
    </asp:Panel>
</asp:Content>
