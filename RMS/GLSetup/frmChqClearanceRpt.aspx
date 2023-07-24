<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/home/RMSMasterHome.Master"
 CodeBehind="frmChqClearanceRpt.aspx.cs" Inherits="RMS.GLSetup.frmChqClearanceRpt" %>


<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %><asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <br />
   
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
                             <asp:Label ID="Label1" runat="server" Text="Divisions*"></asp:Label><br />
                                <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="searchbranchchange form-control" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged"
                                    AppendDataBoundItems="True" AutoPostBack="true">
                                    <asp:ListItem Value="0">Select Division</asp:ListItem>
                                </asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="Label2" runat="server" Text="Type*"></asp:Label><br />
                                <asp:DropDownList ID="ddlFltType" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="0">Pending</asp:ListItem>
                                    <asp:ListItem Value="1">Cleared</asp:ListItem>
                                </asp:DropDownList>
                        </div>
                    </div>


                     <br />
                     <div class="row">
                        <div class="col-md-4">
                             <asp:Label ID="Label4" runat="server" Text="From Date*"></asp:Label><br />
                                <asp:TextBox runat="server" ID="txtFltFromDt" CssClass="form-control" ></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                    TargetControlID="txtFltFromDt">
                                </ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="Label5" runat="server" Text="To Date*"></asp:Label><br />
                                <asp:TextBox runat="server" ID="txtFltToDt" CssClass="form-control"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True"
                                    TargetControlID="txtFltToDt">
                                </ajaxToolkit:CalendarExtender>

                        </div>
                    </div>
                    <br />
                     <div class="row">
                        
                        <div class="col-md-4">
                            <asp:Label ID="lblFltName" runat="server" Text="Bank"></asp:Label><br />
                                <asp:TextBox runat="server" ID="txtFltBank" MaxLength="20" CssClass="form-control"></asp:TextBox>
                        </div>
                         <div class="col-md-4">
                                <asp:Label ID="Label3" runat="server" Text="Cheque No"></asp:Label><br />
                                <asp:TextBox runat="server" ID="txtFltChq" MaxLength="20" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <br />
                     <div class="row">
                        
                        <div class="col-md-4">
                             <asp:Label ID="Label22" runat="server" Text="Voucher No"></asp:Label><br />
                                <asp:TextBox runat="server" ID="txtFltVoucher" MaxLength="20" CssClass="form-control" ></asp:TextBox>
                        </div>
                         <div class="col-md-4">
                                <label>Account Heads</label>
                              
                                <asp:DropDownList ID="codeDropDown" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                    </div>
                   
               <br />
                    <asp:Button ID="Button1" runat="server" Text="Report" OnClick="btnGenerat_Click" CssClass="btn btn-primary"/>
                    <br />
                    <asp:Panel ID="pnlMain" runat="server" Width="99%" Height="600">
                        <rsweb:ReportViewer ID="viewer" runat="server" Width="100%" Height="580px">
                        </rsweb:ReportViewer>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
                    
        
<%--        <p>
         &nbsp;
         <br />&nbsp;
        <asp:ImageButton ID="btnGenerat" runat="server"  ImageUrl="~/images/btn_generate.png"  OnClick="btnGenerat_Click"
       onMouseOver="this.src='../images/btn_generate_m.png'" onMouseOut="this.src='../images/btn_generate.png'" />
             &nbsp;

    </p>--%>
</asp:Content>
