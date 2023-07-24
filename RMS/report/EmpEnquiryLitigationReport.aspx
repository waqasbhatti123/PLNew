<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmpEnquiryLitigationReport.aspx.cs" Inherits="RMS.report.EmpEnquiryLitigationReport" Culture="auto" UICulture="auto"
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
                                <asp:ListItem Value="0">Select</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Enquiry/Litigation*</label>
                            <asp:DropDownList ID="ddltypes" CssClass="form-control" runat="server" AppendDataBoundItems="False" OnSelectedIndexChanged="Types_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                                <asp:ListItem Value="1">Enquiry</asp:ListItem>
                                <asp:ListItem Value="2">Litigation</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Type</label>
                            <asp:UpdatePanel ID="Disupnl" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlenquiryAud" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                        <asp:ListItem Value="0">Select Type</asp:ListItem>
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddltypes" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Status*</label>
                            <asp:DropDownList ID="ddlStatus" CssClass="form-control" runat="server" AppendDataBoundItems="False">
                                <asp:ListItem Value="0">Select Status</asp:ListItem>
                                <asp:ListItem Value="Initiated">Initiated</asp:ListItem>
                                <asp:ListItem Value="In Progress">In Progress</asp:ListItem>
                                <asp:ListItem Value="Disposed Off">Disposed Off</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        &nbsp;
                        <div class="col-lg-4 col-md-4 col-sm-4" style="margin-top:10px">
                            <asp:Button runat="server" ID="tbnSearch" CssClass="btn btn-primary" OnClick="EnqLitiSearch_Click" Text="Search"/>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:GridView ID="grdLitiEnq" runat="server" CssClass="table table-responsive-sm" DataKeyNames="EmpAcrID,EmpID" OnSelectedIndexChanged="grdLitiEnq_SelectedIndexChanged"
                                AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdLitiEnq_PageIndexChanging" OnRowDataBound="grdLitiEnq_rowbound"
                                EmptyDataText="No  Record" Width="100%">
                                <Columns>
                                   <asp:BoundField DataField="FullName" HeaderText="Employee Name" />
                                   <asp:BoundField DataField="EnquiryAud" HeaderText="Enquiry Type" />
                                   <asp:BoundField DataField="EnqTitle" HeaderText="Enquiry Title" />
                                   <asp:BoundField DataField="EnquiryDate" HeaderText="Enquiry Date" />
                                   <asp:BoundField DataField="Statuss" HeaderText="Status" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                         <asp:LinkButton ID="lnkPrintTenExp" runat="server" Text="Print" ToolTip="Print Employee Tenore Exprience Record" CommandArgument='<%#Eval("EmpID")%>' OnClick="lnkEnqPrint_Click" CssClass="lnk">
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
                            <asp:GridView ID="grdLiti" runat="server" CssClass="table table-responsive-sm" DataKeyNames="EmpAcrID,EmpID" OnSelectedIndexChanged="grdLiti_SelectedIndexChanged"
                                AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdLiti_PageIndexChanging" OnRowDataBound="grdLiti_rowbound"
                                EmptyDataText="No Record" Width="100%">
                                <Columns>
                                   <asp:BoundField DataField="FullName" HeaderText="Employee Name" />
                                   <asp:BoundField DataField="LitiName" HeaderText="Litigation Type" />
                                   <asp:BoundField DataField="LitiTitle" HeaderText="Litigation Title" />
                                   <asp:BoundField DataField="LitiDate" HeaderText="Litigation Date" />
                                   <asp:BoundField DataField="Status" HeaderText="Status" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                         <asp:LinkButton ID="lnkPrintTenExp" runat="server" Text="Print" ToolTip="Print Employee Tenore Exprience Record" CommandArgument='<%#Eval("EmpID")%>' OnClick="lnkLitiPrint_Click" CssClass="lnk">
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
                            
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
