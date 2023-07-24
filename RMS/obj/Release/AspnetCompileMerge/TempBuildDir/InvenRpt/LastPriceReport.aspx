<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="LastPriceReport.aspx.cs" Inherits="RMS.InvenRpt.LastPriceReport" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" 
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

    
    </script>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="fltr" />
    <uc1:Messages ID="ucMessage" runat="server" />
    <fieldset class="fieldSet">
        <legend></legend>
        <br />
        <table style="text-align: center;">
        
            <tr>
                
                <td>
                Item Group:
                    <asp:DropDownList ID="ddlItemGroup" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="All" Selected="True" Value="0">All</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    Order By:
                    <asp:DropDownList ID="ddlOrderBy" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Item" Selected="True" Value="item"></asp:ListItem>
                        <asp:ListItem Text="PO No" Value="pono"></asp:ListItem>
                        <asp:ListItem Text="Vendor" Value="vendor"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                 <td>
                  Export To:&nbsp;
                    <asp:DropDownList ID ="ddlExtension" runat="server">
                        <asp:ListItem Value="PDF" Selected="True">PDF</asp:ListItem>
                        <asp:ListItem Value="Excel">Excel</asp:ListItem>
                    </asp:DropDownList>
                 </td>
                <td>
                    &nbsp; &nbsp;
                    <asp:LinkButton ID="linkBtnSearch" runat="server" Text="Generate Report" OnClick="btnSearch_Click" ForeColor="Black"></asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
    </fieldset>
    <div>
        <rsweb:ReportViewer ID="reportViewer" runat="server" Width="98%" ShowRefreshButton="False"
            AsyncRendering="false">
        </rsweb:ReportViewer>
    </div>
</asp:Content>
