<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmpTenureExpReport.aspx.cs" Inherits="RMS.report.EmpTenureExpReport" Culture="auto" UICulture="auto"
    EnableEventValidation="true" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>
    <script src="../Scripts/jquery-ui-1.8.16.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.jquery.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.min.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label Text="Division" ID="Label2" runat="server" />
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label ID="lblFltName" runat="server" Text="Emp Name:"></asp:Label><br />
                            <asp:DropDownList ID="ddlEmpDrpdwn" runat="server" CssClass="form-control ddlEmpDrpdwnty" OnSelectedIndexChanged="ddlEmpDrpdown_change" AutoPostBack="true">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label ID="Label1" runat="server" Text="Personal File No:"></asp:Label><br />

                            <asp:DropDownList ID="ddlperson" runat="server" CssClass="form-control ddlEmpDrpdwnrt" OnSelectedIndexChanged="ddlPersonal_change" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Personal File Number</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Post Held*</label>
                            <asp:DropDownList ID="ddlTenurePost" runat="server" CssClass="form-control held" AppendDataBoundItems="true">
                                <asp:ListItem Value="0">Select Post</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <%--<div class="col-lg-3 col-md-3 col-sm-3">
                            <label>From Scale</label>
                            <asp:DropDownList ID="ddlTenureScale" runat="server" CssClass="form-control from" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Scale</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>To Scale</label>
                            <asp:DropDownList ID="ddlTenureScaleTo" runat="server" CssClass="form-control to" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Scale</asp:ListItem>
                            </asp:DropDownList>
                        </div>--%>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Report Type</label>
                            <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control to" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">All</asp:ListItem>
                                <asp:ListItem Value="1">Serving Employee</asp:ListItem>
                                <asp:ListItem Value="2">Relieved Employee</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button runat="server" ID="searchBtn" Text="Search" OnClick="TenureGridShow_Click" CssClass="btn btn-primary" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:GridView ID="gedTenure" runat="server" CssClass="table table-responsive-sm" DataKeyNames="EmpID" OnSelectedIndexChanged="grdTenEmps_SelectedIndexChanged"
                                AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdTenEmps_PageIndexChanging" OnRowDataBound="grdten_rowbound"
                                EmptyDataText="No Experience Record" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="EmpCode" HeaderText="Personal File No" />
                                    <asp:BoundField DataField="FullName" HeaderText="Employee Name" />
                                    <asp:BoundField DataField="CodeDesc" HeaderText="Last Post" />
                                    <asp:BoundField DataField="ScaleName" HeaderText="Scale" />
                                    <asp:BoundField DataField="br_nme" HeaderText="Place of Posting" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkPrintTenExp" runat="server" Text="Print" ToolTip="Print Employee Tenore Exprience Record" CommandArgument='<%#Eval("EmpID")%>' OnClick="lnkTenPrint_Click" CssClass="lnk">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="grid_hdr" />
                                <RowStyle CssClass="grid_row" />
                                <AlternatingRowStyle CssClass="gridAlternateRow" />
                                <SelectedRowStyle CssClass="gridSelectedRow" />
                            </asp:GridView>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:Panel ID="Panel1" runat="server" Width="780px" Height="600">
                                <rsweb:ReportViewer ID="viewer" runat="server" Width="100%" Height="580px">
                                </rsweb:ReportViewer>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(".searchbranchchange").chosen();
        $(".held").chosen();
        $(".ddlEmpDrpdwnty").chosen();
        $(".ddlEmpDrpdwnrt").chosen();
    </script>
</asp:Content>

