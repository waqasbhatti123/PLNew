<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmployeeProfileDetailReport.aspx.cs" Inherits="RMS.report.EmployeeProfileDetailReport" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>
    <script type="text/javascript">
       
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
            <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12">
                <div class="card card-shadow mb-4">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Select*</label>
                                <asp:DropDownList runat="server" ID="ddlProfile" AppendDataBoundItems="true" CssClass="form-control form-control-sm">
                                    <asp:ListItem Text="ALL" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Qualificaton" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Prior Experience" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Tenure Experience" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Acr Record" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Enquiry/Disciplinary Proceeding" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Litigations/Court Cases" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="Permotion/Time Scale/Upgration" Value="7"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="margin-top:30px">
                                <asp:Button Text="Report" runat="server" ID="Btnsearch" OnClick="GenReport_click" CssClass="btn btn-primary"/>
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
        </div>

</asp:Content>
