<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EarningRecordReport.aspx.cs" Inherits="RMS.report.EarningRecordReport"
    Title="Salary Transfer Report" Culture="auto" UICulture="auto" EnableEventValidation="true" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" 
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <p>
        <asp:Label ID="lblPerd" runat="server" Text="From Pay Period: "></asp:Label>
        <asp:DropDownList ID="ddlFromPayPerd" runat="server" Width="100px">
        </asp:DropDownList>
        &nbsp;
        <asp:Label ID="Label1" runat="server" Text="To Pay Period: "></asp:Label>
        <asp:DropDownList ID="ddlToPayPerd" runat="server" Width="100px">
        </asp:DropDownList>
        &nbsp;
        <asp:Button ID="btnGenerat" runat="server" Text="Report" OnClick="btnGenerat_Click" />
    </p>
    <br />
   <asp:Panel ID="pnlMain" runat="server" Width="780px" Height="600">
       <rsweb:ReportViewer ID="viewer" runat="server"  Width="100%" Height="580px">
       </rsweb:ReportViewer>
   </asp:Panel> 
</asp:Content>
