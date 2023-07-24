<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="AttenReport.aspx.cs" Inherits="RMS.report.AttenReport" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

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
                            <asp:ValidationSummary ID="ValidationSummary1" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                              ValidationGroup="main"/>
                            <uc1:Messages ID="ucMessage" runat="server" />
                            </div>
                        </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Division*</label>  
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Division</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Job Type*</label>
                            <asp:DropDownList ID="ddlJobType" runat="server" CssClass="form-control" AppendDataBoundItems="True" >
                             <asp:ListItem Value="0" Selected="True">Select Job Type</asp:ListItem>
                              </asp:DropDownList>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlJobType"
                                 ErrorMessage="Please select job type" SetFocusOnError="true" ValidationGroup="main"
                                 Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Select Month*</label>
                             <asp:TextBox TextMode="month" runat="server" CssClass="form-control" ID="txtMonth"></asp:TextBox>
                         </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button runat="server" ID="Button19" OnClick="Onclick_ReportGen" class="btn btn-primary" Text="Report" ValidationGroup=""></asp:Button>
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
