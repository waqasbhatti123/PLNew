<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
 CodeBehind="ApproperiationReport.aspx.cs" Inherits="RMS.report.ApproperiationReport"
    Title="Budget Report" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>
<%--<%@ Page Language="C#"  AutoEventWireup="true"
    CodeBehind="BudgetReport.aspx.cs" Inherits="RMS.report.BudgetReport" %>--%>


<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>


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
                        

                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Budget Year*</label>
                                <asp:DropDownList ID="SelectedYear" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                    <asp:ListItem Value="0">Select Year</asp:ListItem>
                                    <asp:ListItem Value="2020-2021">2020 - 21</asp:ListItem>
                                    <asp:ListItem Value="2021-2022">2021 - 22</asp:ListItem>
                                    <asp:ListItem Value="2022-2023">2022 - 23</asp:ListItem>
                                    <asp:ListItem Value="2023-2024">2023 - 24</asp:ListItem>
                                    <asp:ListItem Value="2024-2025">2024 - 25</asp:ListItem>
                                    <asp:ListItem Value="2025-2026">2025 - 26</asp:ListItem>
                                    <asp:ListItem Value="2026-2027">2026 - 27</asp:ListItem>
                                    <asp:ListItem Value="2027-2028">2027 - 28</asp:ListItem>
                                    <asp:ListItem Value="2028-2029">2028 - 29</asp:ListItem>
                                    <asp:ListItem Value="2029-2030">2029 - 30</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="SelectedYear"
                                    ErrorMessage="Please Select year" SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator>
                            </div>

                          <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Durations*</label>
                                <asp:DropDownList ID="dropDownDuration" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                    
                                    <asp:ListItem Value="7">First Appropriation</asp:ListItem>
                                    <asp:ListItem Value="8">Second Appropriation</asp:ListItem>
                                    <asp:ListItem Value="9">Third Appropriation</asp:ListItem>
                                    <asp:ListItem Value="10">Fourth Appropriation</asp:ListItem>

                                </asp:DropDownList>
                                
                            </div>
                         </div>

                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button ID="btnGenerat" CssClass="btn btn-primary" runat="server" Text="Report" 
                              onclick="btnGenerat_Click" />
                        </div>
                    </div>
                    <br />
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


<%--
 <!DOCTYPE html>   
    <html>
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="X-UA-Compatible">
</head>
<body>
    <form runat="server">
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12">
                <div class="card card-shadow mb-4">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Budget Year</label>
                                <asp:DropDownList ID="SelectedYear" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                    <asp:ListItem Value="0">Select Year</asp:ListItem>
                                    <asp:ListItem Value="2020-21">2020 - 21</asp:ListItem>
                                    <asp:ListItem Value="2021-22">2021 - 22</asp:ListItem>
                                    <asp:ListItem Value="2022-23">2022 - 23</asp:ListItem>
                                    <asp:ListItem Value="2023-24">2023 - 24</asp:ListItem>
                                    <asp:ListItem Value="2024-25">2024 - 25</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldYear" runat="server" ControlToValidate="SelectedYear"
                                    ErrorMessage="Please Select year" SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Button ID="btnGenerat" CssClass="btn btn-primary" runat="server" Text="Report"
                                    OnClick="btnGenerat_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12">
                                <asp:Panel ID="pnlMain" runat="server" Width="99%" Height="600">
                                    <asp:ScriptManager runat="server"></asp:ScriptManager>
                                    <rsweb:ReportViewer ID="viewer" runat="server" Width="100%" Height="580px">
                                    </rsweb:ReportViewer>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

</body>
</html>--%>


 