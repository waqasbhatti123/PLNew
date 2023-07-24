<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"
    AutoEventWireup="true" CodeBehind="TaxPayableReport.aspx.cs" Inherits="RMS.GLSetup.TaxPayableReport"
    Culture="auto" UICulture="auto" %>
<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>



<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>



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
                             ValidationGroup="main"/>
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                     <div class="col-lg-4 col-md-4 col-sm-4">
                     <asp:DropDownList ID="AccountID" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                     <asp:ListItem Value="0">Select Cheque No:</asp:ListItem>
                    </asp:DropDownList>
                     </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button ID="btnGenerat" CssClass="btn btn-primary" runat="server" Text="Report" 
                              onclick="btnGenerat_Click" />
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
