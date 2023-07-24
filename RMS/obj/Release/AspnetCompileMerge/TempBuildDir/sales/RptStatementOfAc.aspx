<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="RptStatementOfAc.aspx.cs" Inherits="RMS.sales.RptStatementOfAc"
    Title="Statement Of A/C" Culture="auto" UICulture="auto" EnableEventValidation="true" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" 
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:Messages ID="ucMessage" runat="server" />
    <p>
        <asp:Label ID="Label2" runat="server" Text="Vendor: "></asp:Label>
        <asp:DropDownList ID="ddlVendor" runat="server" AppendDataBoundItems="true">
            <asp:ListItem Selected="True" Value="">All</asp:ListItem>
        </asp:DropDownList>
        &nbsp;
        <asp:Label ID="Label1" runat="server" Text="From Date: "></asp:Label>
        <asp:TextBox ID="txtFromDate" runat="server" Width="80px">
        </asp:TextBox>
        <ajaxToolkit:CalendarExtender ID="calFromDate" runat="server" PopupPosition="BottomRight" 
                                          TargetControlID="txtFromDate">
        </ajaxToolkit:CalendarExtender>
        &nbsp;
        <asp:Label ID="lblToDate" runat="server" Text="To Date: "></asp:Label>
        <asp:TextBox ID="txtToDate" runat="server" Width="80px">
        </asp:TextBox>
        <ajaxToolkit:CalendarExtender ID="calToDate" runat="server" PopupPosition="BottomRight" 
                                          TargetControlID="txtToDate">
        </ajaxToolkit:CalendarExtender>
        &nbsp;
        <asp:Label ID="Label3" runat="server" Text="Status: "></asp:Label>
        <asp:DropDownList ID="ddlStatus" runat="server">
            <asp:ListItem Selected="True" Value="">All</asp:ListItem>
            <asp:ListItem Value="OP">Open</asp:ListItem>
            <asp:ListItem Value="CL">Closed</asp:ListItem>
        </asp:DropDownList>
        &nbsp;
        <asp:Button ID="btnGenerat" runat="server" Text="Report" OnClick="btnGenerat_Click" />
    </p>
    <br />
   <asp:Panel ID="pnlMain" runat="server" Width="99%" Height="600">
       <rsweb:ReportViewer ID="viewer" runat="server" Visible ="false" Width="100%" Height="580px">
       </rsweb:ReportViewer>
   </asp:Panel> 
   
</asp:Content>
