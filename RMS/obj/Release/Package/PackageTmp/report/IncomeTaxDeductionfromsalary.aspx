<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="IncomeTaxDeductionfromsalary.aspx.cs" Inherits="RMS.report.IncomeTaxDeductionfromsalary"
    Title="Salary Transfer Report" Culture="auto" UICulture="auto" EnableEventValidation="true" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>
    <script>
        $(document).ready(function () {
            var brID = '<%=Session["BranchID"].ToString() %>';
        if (brID == 1) {
            $(".dede").show();
            $(".ded").hide();
        }
        else {
            $(".dede").hide();
            $(".ded").show();
            }
        });
    </script>
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
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                                 <label>Divisions*</label>
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged"
                                AppendDataBoundItems="True" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Division</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredsearchBranchDropDown" runat="server" ControlToValidate="searchBranchDropDown"
                                    ErrorMessage="Please Select Branch" SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator>
                            </div>
                        <div class="col-lg-4 col-md-4 col-sm-4 dede">
                            <label>Select Deduction*</label>
                            <asp:DropDownList ID="ddlDeductions" runat="server" CssClass="form-control"  AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                                <asp:ListItem Value="Income Tax / Other">Income Tax / Other</asp:ListItem>
                                <asp:ListItem Value="Group Insurance E.D">Group Insurance E.D</asp:ListItem>
                                <asp:ListItem Value="Benevolent Fund(E.D)">Benevolent Fund(E.D)</asp:ListItem>
                                <asp:ListItem Value="G.P Fund Subscription">G.P Fund Subscription</asp:ListItem>
                                <asp:ListItem Value="5% House Rent">5% House Rent</asp:ListItem>
                                <asp:ListItem Value="House Rent Ded">House Rent Ded</asp:ListItem>
                                <asp:ListItem Value="G.Ins">G.Ins</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4 ded">
                            <label>Select Deduction*</label>
                            <asp:DropDownList ID="ddlded" runat="server" CssClass="form-control"  AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                                <asp:ListItem Value="Income Tax / Other">Income Tax / Other</asp:ListItem>
                                <asp:ListItem Value="G.Ins">G.Ins</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Type*</label>
                            <asp:DropDownList ID="ddlJobType" runat="server" CssClass="form-control employeeType" OnSelectedIndexChanged="ddlJobType_SelectedIndexChanged" AppendDataBoundItems="True" AutoPostBack="true">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Select Month *</label>
                            <asp:TextBox ID="MonthSelected" runat="server" CssClass="form-control" TextMode="Month"></asp:TextBox>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button Text="Report" runat="server" CssClass="btn btn-primary" OnClick="btnReport_Gen" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12">
                                <asp:Panel ID="pnlMain" runat="server" Width="99%" Height="600">
                                   <%-- <asp:ScriptManager runat="server"></asp:ScriptManager>--%>
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
