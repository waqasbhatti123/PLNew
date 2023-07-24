<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/home/RMSMasterHome.Master"
 CodeBehind="MonthlyCCAcDet.aspx.cs" Culture="auto" UICulture="auto"
 Inherits="RMS.GLSetup.MonthlyCCAcDet" %>

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
                            <asp:ValidationSummary ID="ValidationSummary1" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                                ValidationGroup="main" />
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            Branch*
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="searchbranchchange form-control" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged"
                                                AppendDataBoundItems="True" AutoPostBack="true">
                                                <asp:ListItem Value="0">Select Branch</asp:ListItem>
                                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            GL Year*
                            <asp:DropDownList ID="ddlGlYear" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="2012">2012</asp:ListItem>
                                                <asp:ListItem Value="2013">2013</asp:ListItem>
                                                <asp:ListItem Value="2014">2014</asp:ListItem>
                                                <asp:ListItem Value="2015">2015</asp:ListItem>
                                                <asp:ListItem Value="2016">2016</asp:ListItem>
                                                <asp:ListItem Value="2017">2017</asp:ListItem>
                                                <asp:ListItem Value="2018">2018</asp:ListItem>
                                                <asp:ListItem Value="2019">2019</asp:ListItem>
                                                <asp:ListItem Value="2020">2020</asp:ListItem>
                                                <asp:ListItem Value="2021">2021</asp:ListItem>
                                                <asp:ListItem Value="2022">2022</asp:ListItem>
                                                <asp:ListItem Value="2023">2023</asp:ListItem>
                                                <asp:ListItem Value="2024">2024</asp:ListItem>
                                                <asp:ListItem Value="2025">2025</asp:ListItem>
                                                <asp:ListItem Value="2026">2026</asp:ListItem>
                                                <asp:ListItem Value="2027">2027</asp:ListItem>
                                                <asp:ListItem Value="2028">2028</asp:ListItem>
                                                <asp:ListItem Value="2029">2029</asp:ListItem>
                                                <asp:ListItem Value="2030">2030</asp:ListItem>
                                            </asp:DropDownList>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-4">
                            From Date*
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="RequiredField form-control">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFromDate"
                                                ErrorMessage="Please enter from date" SetFocusOnError="true" ValidationGroup="main" Display="None" EnableClientScript="true">
                                            </asp:RequiredFieldValidator>
                                            <ajaxToolkit:CalendarExtender ID="calFromDate" runat="server" TargetControlID="txtFromDate" PopupPosition="BottomRight">
                                            </ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-md-4">
                            To Date*
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="RequiredField form-control">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtToDate"
                                                ErrorMessage="Please enter to date" SetFocusOnError="true" ValidationGroup="main" Display="None" EnableClientScript="true">
                                            </asp:RequiredFieldValidator>
                                            <ajaxToolkit:CalendarExtender ID="calToDate" runat="server" TargetControlID="txtToDate" PopupPosition="BottomRight">
                                            </ajaxToolkit:CalendarExtender>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-4">
                            Status*
                            <asp:DropDownList ID="ddlstatus" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="A" Selected="True">Approved</asp:ListItem>
                                                <asp:ListItem Value="P">Pending</asp:ListItem>
                                                <asp:ListItem Value=" ">All</asp:ListItem>
                                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            Cost Center Group
                            <asp:DropDownList ID="ddlCCGroup" runat="server" AppendDataBoundItems="true" CssClass="form-control">
                                                <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                            </asp:DropDownList>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-4">
                            Cost Center
                                  <asp:DropDownList ID="costCenterDropDown" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                           Account Heads
                            <asp:DropDownList ID="codeDropDown" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                    </div>
                   
                    <br />
                     <asp:Button ID="btnRepor" runat="server" Text="Report" ValidationGroup="main" OnClick="btnReport_Click" CssClass="btn btn-primary"/>
                                    
                    <br />
                    <asp:Panel ID="pnlMain" runat="server" Width="99%" Height="600">
                        <rsweb:ReportViewer ID="viewer" runat="server" Width="100%" Height="580px">
                        </rsweb:ReportViewer>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>