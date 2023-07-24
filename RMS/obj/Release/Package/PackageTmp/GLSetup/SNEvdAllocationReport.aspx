<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"
    AutoEventWireup="true" CodeBehind="SNEvdAllocationReport.aspx.cs" Inherits="RMS.GLSetup.SNEvdAllocationReport"
    Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <uc1:Messages ID="ucMessage" runat="server" />
                    </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Finaincial Year</label>
                            <asp:DropDownList ID="SelectedYear" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Year</asp:ListItem>
                                <asp:ListItem Value="2020-2021">2020 - 2021</asp:ListItem>
                                <asp:ListItem Value="2021-2022">2021 - 2022</asp:ListItem>
                                <asp:ListItem Value="2022-2023">2022 - 2023</asp:ListItem>
                                <asp:ListItem Value="2023-2024">2023 - 2024</asp:ListItem>
                                <asp:ListItem Value="2024-2025">2024 - 2025</asp:ListItem>
                                <asp:ListItem Value="2025-2026">2025 - 2026</asp:ListItem>
                                <asp:ListItem Value="2026-2027">2026 - 2027</asp:ListItem>
                                <asp:ListItem Value="2027-2028">2027 - 2028</asp:ListItem>
                                <asp:ListItem Value="2028-2029">2028 - 2029</asp:ListItem>
                                <asp:ListItem Value="2029-2030">2029 - 2030</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="SelectedYear"
                                ErrorMessage="Please Select year" SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4" style="margin-top:30px">
                            <asp:Button Text="Report" CssClass="btn btn-primary" OnClick="GeneReport_click" runat="server" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:Panel ID="pnlMain" runat="server" Width="99%" Height="600">
                              <rsweb:ReportViewer ID="viewer" runat="server"  Width="100%" Height="580px">
                              </rsweb:ReportViewer>
                          </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

