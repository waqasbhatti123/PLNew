﻿<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmpLoanReport.aspx.cs" Inherits="RMS.report.rdlc.EmpLoanReport"
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

   <%-- <p>
        <asp:Label ID="lblPerd" runat="server" Text="LoanReport: "></asp:Label>
        <asp:DropDownList ID="ddlPayPerd" runat="server" Width="100px">
        </asp:DropDownList>
    </p>--%>

    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button ID="btnGenerat" CssClass="btn btn-primary" runat="server" Text="Report" 
                              onclick="btnGenerat_Click" />
                        </div>
                    </div>
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