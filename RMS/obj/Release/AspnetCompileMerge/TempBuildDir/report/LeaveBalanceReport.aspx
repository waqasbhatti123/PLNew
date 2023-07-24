<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="LeaveBalanceReport.aspx.cs" Inherits="RMS.report.LeaveBalanceReport"
    Title="Salary Transfer Report" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

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
                             <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>To Date*</label>
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="cal1" runat="server" TargetControlID="txtToDate" PopupPosition="BottomRight"></ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4" style="margin-top:30px">
                            <asp:Button ID="btnGenerat" runat="server" CssClass="btn btn-primary" Text="Report" onclick="btnGenerat_Click" />
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
