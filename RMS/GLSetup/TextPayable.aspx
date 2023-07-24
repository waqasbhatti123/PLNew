<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"
    AutoEventWireup="true" CodeBehind="TextPayable.aspx.cs" Inherits="RMS.GLSetup.TextPayable"
    Culture="auto" UICulture="auto" EnableEventValidation="true"%>


<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>
    <script>
        <%-- $(function () {
        var brID = '<%=Session["BranchID"].ToString() %>';
        $(".searchbranchchange").val(brID);
        });--%>
        
    </script>
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
                    <div class="row">
                        <asp:TextBox ID="vrid" runat="server" Visible="false" CssClass="form-control"/>
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <label>Narration:</label>
                           <asp:TextBox ID="txtnarration" runat="server" CssClass="form-control" disabled="true" TextMode="MultiLine"/>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Voucher No:</label>
                            <asp:TextBox ID="VoucherNO" disabled="true" runat="server" CssClass="form-control"/>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Cheque No:</label>
                            <asp:TextBox ID="ChqNo" disabled="true" runat="server" CssClass="form-control"/>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Cheque Date:</label>
                            <asp:TextBox ID="ChqDate" disabled="true" runat="server" CssClass="form-control"/>
                            <ajaxToolkit:CalendarExtender ID="ChqDateCal" runat="server" TargetControlID="ChqDate" Enabled="True">
                             </ajaxToolkit:CalendarExtender>
                            <ajaxToolkit:CalendarExtender ID="txtDate" runat="server" TargetControlID="ChqDate" Enabled="True"/>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Income Tax:</label>
                            <asp:TextBox ID="txtIncome"  runat="server" CssClass="form-control"/>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>GST Tax:</label>
                            <asp:TextBox ID="txtGST"  runat="server" CssClass="form-control"/>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>PRA Tax:</label>
                            <asp:TextBox ID="txtPRA" runat="server" CssClass="form-control"/>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                        <asp:Button runat="server" ID="Button19" OnClick="Onclick_Save" class="btn btn-primary" Text="Save" ValidationGroup=""></asp:Button>&nbsp;
                        <asp:Button runat="server" ID="Button244" OnClick="Onclick_Clear" CssClass="btn btn-danger" Text="Clear" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row"> 
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Divisions:</label>
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Division</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Account No:</label>
                            <asp:TextBox ID="txtaccount" runat="server" CssClass="form-control"/>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4" style="margin-top:30px">
                            <asp:Button runat="server" ID="Button1" OnClick="Onclick_Search" class="btn btn-primary" Text="Search" ValidationGroup=""></asp:Button>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                   <asp:GridView ID="grdTextPayable" runat="server" CssClass="table table-responsive-sm" DataKeyNames="vrid" OnSelectedIndexChanged="grdText_SelectedIndexChanged"
                  AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdText_PageIndexChanging" OnRowDataBound="grdText_RowDataBound"
                  EmptyDataText="No Record Found" Width="100%">
                  <Columns>
                      <asp:BoundField DataField="type" HeaderText="Sr No" />
                      <asp:BoundField DataField="ref_no" HeaderText="voucher No" />
                      <asp:BoundField DataField="headsInvolved" HeaderText="Heads" />
                      <asp:BoundField DataField="vr_nrtn" HeaderText="Narration" />
                      <asp:BoundField DataField="cheNo" HeaderText="Cheque No" />
                      <asp:BoundField DataField="cheDat" HeaderText="Cheque Date" />
                      <asp:BoundField DataField="ITAmount" HeaderText="Income Tax" />
                      <asp:BoundField DataField="GSTAmount" HeaderText="GST Tax" />
                      <asp:BoundField DataField="PRA" HeaderText="PRA Tax" />
                      <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                          <ControlStyle CssClass="lnk"></ControlStyle>
                      </asp:CommandField>
                  </Columns>
                 </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
