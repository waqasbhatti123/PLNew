<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="AttendanceReport.aspx.cs" Inherits="RMS.report.AttendanceReport"
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
    <uc1:Messages ID="ucMessage" runat="server"/>
    <div style="float:left">
        <p>

        </p>

        <p>
            Year:
            &nbsp;
            <asp:DropDownList ID="ddlYear" runat="server" Width="70px">
                <asp:ListItem Text="2012" Value="2012" />
                <asp:ListItem Text="2013" Value="2013" />
                <asp:ListItem Text="2014" Value="2014" />
                <asp:ListItem Text="2015" Value="2015" />
                <asp:ListItem Text="2016" Value="2016" />
                <asp:ListItem Text="2017" Value="2017" />
                <asp:ListItem Text="2018" Value="2018" />
                <asp:ListItem Text="2019" Value="2019" />
                <asp:ListItem Text="2020" Value="2020" />
            </asp:DropDownList>
            &nbsp;
            Month:
            &nbsp;
            <asp:DropDownList ID="ddlMonth" runat="server" Width="70px">
                <asp:ListItem Text="January" Value="1" />
                <asp:ListItem Text="February" Value="2" />
                <asp:ListItem Text="March" Value="3" />
                <asp:ListItem Text="April" Value="4" />
                <asp:ListItem Text="May" Value="5" />
                <asp:ListItem Text="June" Value="6" />
                <asp:ListItem Text="July" Value="7" />
                <asp:ListItem Text="August" Value="8" />
                <asp:ListItem Text="September" Value="9" />
                <asp:ListItem Text="October" Value="10" />
                <asp:ListItem Text="November" Value="11" />
                <asp:ListItem Text="December" Value="12" />
            </asp:DropDownList>
            &nbsp;
            <asp:Button ID="btnGenerat" runat="server" Text="Report" OnClick="btnGenerat_Click" />
        </p>
    </div>
    <div style="float:right">
        <asp:LinkButton ID="lnkExport" runat="server" Text="Export" OnClick="lnkExport_Click" Visible="false" ToolTip="Export To Excel" CssClass="lnk"></asp:LinkButton>
    </div>
    <br />
    <br />
    <br />
   <asp:Panel ID="Panel1" runat="server" Width="780px" Height="600">
        <rsweb:ReportViewer ID="viewer" runat="server" Width="100%" Height="580px">
        </rsweb:ReportViewer>
   </asp:Panel> 
    <%--
     <br />
    <br />
    <br />
    <div>
        <asp:Panel ID="pnlMain" runat="server" ScrollBars="Both" Width="780px" Height="600px">
            <asp:GridView ID="grdAttendance" runat="server" GridLines="Both" Visible="false">
            </asp:GridView>
        </asp:Panel>
    </div>
    --%>
    

</asp:Content>
