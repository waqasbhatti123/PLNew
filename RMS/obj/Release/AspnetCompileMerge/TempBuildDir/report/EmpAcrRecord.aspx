<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmpAcrRecord.aspx.cs" Inherits="RMS.report.EmpAcrRecord" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                                ValidationGroup="main" />
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label Text="Division" ID="Label2" runat="server" />
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>From Date*</label>
                            <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtDateFromCal" runat="server" TargetControlID="txtDateFrom" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>To Date</label>
                            <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtDateToCal" runat="server" TargetControlID="txtDateTo" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Report Type</label>
                            <asp:DropDownList ID="ddlReportType" CssClass="form-control" runat="server" AppendDataBoundItems="False">
                                <asp:ListItem Value="0">All</asp:ListItem>
                                <asp:ListItem Value="1">Serving Employee</asp:ListItem>
                                <asp:ListItem Value="2">Relieved Employee</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button Text="Search" runat="server" OnClick="Search_click" CssClass="btn btn-primary" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:GridView ID="grdAcr" runat="server" CssClass="table table-responsive-sm" DataKeyNames="EmpAcrID,EmpID" OnSelectedIndexChanged="grdAcrEmps_SelectedIndexChanged"
                                AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdAcrEmps_PageIndexChanging" OnRowDataBound="grdAcr_rowbound"
                                EmptyDataText="There is no employee Experience" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="FullName" HeaderText="Employee Name" />
                                    <asp:BoundField DataField="CodeDesc" HeaderText="Designation" />
                                    <asp:BoundField DataField="DateFrom" HeaderText="From Date" />
                                    <asp:BoundField DataField="DateTo" HeaderText="To Date" />
                                    <asp:BoundField DataField="ReportingOfficer" HeaderText="Reporting Officer" />
                                    <asp:BoundField DataField="RepOffDate" HeaderText="Reporting Officer Date" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkPrintAcrExp" runat="server" Text="Print" ToolTip="Print ACR  Record" CommandArgument='<%#Eval("EmpID")%>' OnClick="lnkAcrPrint_Click" CssClass="lnk">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                        <ControlStyle CssClass="lnk"></ControlStyle>
                                    </asp:CommandField>--%>
                                </Columns>
                                <HeaderStyle CssClass="grid_hdr" />
                                <RowStyle CssClass="grid_row" />
                                <AlternatingRowStyle CssClass="gridAlternateRow" />
                                <SelectedRowStyle CssClass="gridSelectedRow" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
