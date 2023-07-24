<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"
    AutoEventWireup="true" CodeBehind="TheatreInfoReport.aspx.cs" Inherits="RMS.GLSetup.TheatreInfoReport"
    Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" 
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


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
                                ValidationGroup="main" />
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Division</label>
                            <asp:DropDownList ID="ddlDivisional" CssClass="form-control" runat="server" AppendDataBoundItems="False">
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4" style="margin-top:30px">
                            <asp:Button Text="Report" runat="server" CssClass="btn btn-primary" ID="btnReport" OnClick="Click_Generate" />
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