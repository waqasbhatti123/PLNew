<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="SalaryTransferBankReport.aspx.cs" Inherits="RMS.report.rdlc.SalaryTransferBankReport"
    Title="Salary Transfer Report" Culture="auto" UICulture="auto" EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <p>
        <asp:Label ID="lblPerd" runat="server" Text="For Selected Pay Period: "></asp:Label>
        <asp:DropDownList ID="ddlPayPerd" runat="server" Width="100px">
        </asp:DropDownList>
    </p>
    <br />
    <p>
        &nbsp;
        <asp:DropDownList ID="ddlExport" runat="server" Width="70px">
            <asp:ListItem Text="PDF" Value="PDF" />
            <asp:ListItem Text="XLS" Value="XLS" />
        </asp:DropDownList>
        &nbsp;
        <asp:Button ID="btnGenerat" runat="server" Text="Report" OnClick="btnGenerat_Click" />
    </p>
</asp:Content>
