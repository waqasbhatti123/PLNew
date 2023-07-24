<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmpPermotionReport.aspx.cs" Inherits="RMS.report.EmpPermotionReport" Culture="auto" UICulture="auto"
    EnableEventValidation="true" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Type*</label>
                            <asp:DropDownList ID="ddlpertypes" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Type</asp:ListItem>
                                <asp:ListItem Value="Appointment">Appointment</asp:ListItem>
                                <asp:ListItem Value="Promotion">Promotion</asp:ListItem>
                                <asp:ListItem Value="Time Scale">Time Scale</asp:ListItem>
                                <asp:ListItem Value="Upgradation">Upgradation</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>From Date*</label>
                            <asp:TextBox ID="txtperfrom" runat="server" CssClass="form-control"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtperfromCal" runat="server" TargetControlID="txtperfrom" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>To Date</label>
                            <asp:TextBox ID="txtPerTo" runat="server" CssClass="form-control"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtPerToCal" runat="server" TargetControlID="txtPerTo" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Report Type*</label>
                            <asp:DropDownList ID="ddlReportType" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">All</asp:ListItem>
                                <asp:ListItem Value="1">Serving Employee</asp:ListItem>
                                <asp:ListItem Value="2">Relieved Employee</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button runat="server" CssClass="btn btn-primary" OnClick="btnPermo_Click" Text="Search" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:GridView ID="grdPermotion" runat="server" CssClass="table table-responsive-sm" DataKeyNames="PerID" OnSelectedIndexChanged="grdPerEmps_SelectedIndexChanged"
                                AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdPerEmps_PageIndexChanging" OnRowDataBound="grdPer_rowbound"
                                EmptyDataText="No Record" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="FullName" HeaderText="Name" />
                                    <asp:BoundField DataField="pertype" HeaderText="Permotion Type" />
                                    <asp:BoundField DataField="ScaleName" HeaderText="Scale" />
                                    <asp:BoundField DataField="FromDate" HeaderText="From Date" />
                                    <asp:BoundField DataField="todate" HeaderText="To Date" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkPrintExp" runat="server" Text="Print" ToolTip="Print Employee Education Record" CommandArgument='<%#Eval("empID")%>' OnClick="lnkPriorPrint_Click" CssClass="lnk">
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
</asp:Content>
