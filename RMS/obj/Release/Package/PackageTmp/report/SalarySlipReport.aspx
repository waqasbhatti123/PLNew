<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="SalarySlipReport.aspx.cs" Inherits="RMS.report.rdlc.SalarySlipReport"
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
            $(".Cars").show();
            $(".salDays").hide();
        }
        else {
            $(".Cars").hide();
            $(".salDays").hide();
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
                    <%--   <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <uc3:EmpSearchUC ID="EmpSrchUC" runat="server" />
                        </div>
                    </div>--%>

                    <%--  <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label ID="Label1" runat="server" Text="Employee Code From:"></asp:Label>
                            <asp:TextBox ID="txtCode_From" runat ="server" CssClass="form-control" Text="" ></asp:TextBox> 
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label ID="Label2" runat="server" Text="Employee Code To:"></asp:Label>
                            <asp:TextBox ID="txtCode_To" runat ="server" CssClass="form-control" Text="" ></asp:TextBox> 
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label ID="lblPerd" runat="server" Text="For Selected Pay Period:"></asp:Label>
                            <asp:DropDownList ID="ddlPayPerd" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>--%>

                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Divisions*</label>
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged" AppendDataBoundItems="True" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Division</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredsearchBranchDropDown" runat="server" ControlToValidate="searchBranchDropDown"
                                ErrorMessage="Please Select Branch" SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator>

                        </div>

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Employee Type</label>
                            <asp:DropDownList ID="ddlJobType" runat="server" CssClass="form-control employeeType" OnSelectedIndexChanged="ddlJobType_SelectedIndexChanged" AppendDataBoundItems="True" AutoPostBack="true">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Employee *</label>
                            <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control" AppendDataBoundItems="False">
                                <asp:ListItem Value="0">Select Employee</asp:ListItem>
                            </asp:DropDownList>
                           
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Select Month *</label>
                            <asp:TextBox ID="MonthSelected" runat="server" CssClass="form-control" TextMode="Month"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldMonth" runat="server" ControlToValidate="MonthSelected"
                                ErrorMessage="Please Select Month" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3 Cars">
                            <label> For Car Alloted</label>
                            <asp:DropDownList ID="ddlCarAlloted" runat="server" CssClass="form-control " AppendDataBoundItems="False">
                                <asp:ListItem Value="">Select</asp:ListItem>
                                <asp:ListItem Value="Certified that the registration of Car No: LEG 230 is alloted to me officially">Certified that the registration of Car No: LEG 1070 is alloted to me officially</asp:ListItem>
                                <asp:ListItem Value="Certified that the registration of Car No: LEG 1070 is alloted to me officially">Certified that the registration of Car No: LEG 1070 is alloted to me officially</asp:ListItem>
                                <asp:ListItem Value="Certified that the registration of Car No: LEG 28 is alloted to me officially">Certified that the registration of Car No: LEG 28 is alloted to me officially</asp:ListItem>
                              </asp:DropDownList>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlCarAlloted"
                                ErrorMessage="Please Select Employee" SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator>--%>
                        </div>
                        <%--<div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Select Head</label>
                            <asp:DropDownList ID="ddlSelectHead" runat="server" CssClass="form-control" AppendDataBoundItems="False">
                                <asp:ListItem Value="0">Select Employee</asp:ListItem>
                            </asp:DropDownList>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlEmployee"
                                ErrorMessage="Please Select Employee" SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator>
                        </div>--%>
                        <div class="col-lg-3 col-md-3 col-sm-3 salDays">
                            <label>Salary Days</label>
                            <asp:TextBox ID="txtSalaryday" runat="server" CssClass="form-control "></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="MonthSelected"
                                ErrorMessage="Please Select Month" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button ID="btnGenerat" runat="server" CssClass="btn btn-primary" Text="Report" OnClick="btnGenerat_Click" />

                            <%-- <asp:Button ID="btnClear" runat="server" CssClass="btn btn-success" Text="Clear" OnClick="btnClear_Click" />--%>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:Panel ID="pnlMain" runat="server" Width="99%" Height="600">
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
