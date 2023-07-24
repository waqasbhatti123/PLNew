<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="SalaryTransferBankReportNew.aspx.cs" Inherits="RMS.report.rdlc.SalaryTransferBankReportNew"
    Title="Salary Transfer Report" Culture="auto" UICulture="auto" EnableEventValidation="true" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" 
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <script type="text/javascript">

    function pageLoad() {

        $(".classOnlyInt").keydown(function(event) {
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }
        });
    }
    </script>
    
    
    
    
    
    <div class="row">
         <div class="col-lg-12 col-md-12 col-sm-12">
             <div class="card card-shadow mb-4">
                 <div class="card-body">
                     <div class="row">
                         <div class="col-lg-12 col-md-12 col-sm-12">
                             <asp:Label ID="lblMsg" runat="server"></asp:Label>
                         </div>
                     </div>
                     &nbsp;
                     <div class="row">
                         <div class="col-lg-4 col-md-4 col-sm-4">
                             <asp:Label ID="lblPerd" runat="server" Text="For Selected Pay Period* "></asp:Label>
                             <asp:DropDownList ID="ddlPayPerd" runat="server" CssClass="form-control"></asp:DropDownList>
                         </div>
                         <div class="col-lg-4 col-md-4 col-sm-4">
                             <asp:Label ID="Label1" runat="server" Text="Department* "></asp:Label>
                             <asp:DropDownList ID="ddlDept" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                             <asp:ListItem Value="0">All</asp:ListItem>
                         </asp:DropDownList>
                         </div>
                         <div class="col-lg-4 col-md-4 col-sm-4">
                             <asp:Label ID="Label2" runat="server" Text="Min. Salary* "></asp:Label>
                             <asp:TextBox ID="txtMinSal" runat="server" CssClass="classOnlyInt form-control" MaxLength="8"></asp:TextBox>
                         </div>
                     </div>
                     &nbsp;
                     <div class="row">
                      <div class="col-lg-4 col-md-4 col-sm-4">
                          <asp:Label ID="Label3" runat="server" Text="Max. Salary* "></asp:Label>
                          <asp:TextBox ID="txtMaxSal" runat="server" CssClass="classOnlyInt form-control" MaxLength="8"></asp:TextBox>
                      </div>
                      <div class="col-lg-4 col-md-4 col-sm-4">
                          <asp:Label ID="Label4" runat="server" Text="Job Type* "></asp:Label>
                          <asp:DropDownList ID="ddlJobType" CssClass="form-control" runat="server">
                          <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                          <asp:ListItem Value="Permanent">Permanent</asp:ListItem>
                          <asp:ListItem Value="Temporary">Temporary</asp:ListItem>
                          <asp:ListItem Value="Worker">Worker</asp:ListItem>
                      </asp:DropDownList> 
                      </div>
                     </div>
                     &nbsp;
                     <div class="row">
                      <div class="col-lg-4 col-md-4 col-sm-4">
                          <asp:Button ID="btnGenerat" runat="server" CssClass="btn btn-primary" Text="Report" OnClick="btnGenerat_Click" />
            
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
