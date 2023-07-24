<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="CpfDeductionfromsal.aspx.cs" Inherits="RMS.report.CpfDeductionfromsal"
    Title="Salary Transfer Report" Culture="auto" UICulture="auto" EnableEventValidation="true" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
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
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Select Month *</label>
                            <asp:TextBox ID="MonthSelected" runat="server" CssClass="form-control" TextMode="Month"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button Text="Report" runat="server" CssClass="btn btn-primary" OnClick="btnReport_Gen" />
                        </div>
                    </div>
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
