<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"
    AutoEventWireup="true" CodeBehind="CouncilCourtCases.aspx.cs" Inherits="RMS.GLSetup.CouncilCourtCases"
    Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
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
                                ValidationGroup="main" />
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Divisions</label>
                            <asp:DropDownList ID="ddldivision" CssClass="form-control" runat="server" AppendDataBoundItems="False">
                                <asp:ListItem Value="0" Selected="True">Select Division</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Case title</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtcasetitle" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Case Date</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtCaseDate" />
                            <ajaxToolkit:CalendarExtender ID="txtCaseDateCal" runat="server" TargetControlID="txtCaseDate" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Lawyer Name</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtLawyerName" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Lawyer Contact Number</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtLawyerContact" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Select Court</label>
                            <asp:DropDownList ID="ddlCourtName" CssClass="form-control" runat="server" AppendDataBoundItems="False">
                                <asp:ListItem Value="0" Selected="True">Select Court</asp:ListItem>
                                <asp:ListItem Value="Punjab Mohtasib Court">Punjab Mohtasib Court</asp:ListItem>
                                <asp:ListItem Value="Labour Court">Labour Court</asp:ListItem>
                                <asp:ListItem Value="Civil Court">Civil Court</asp:ListItem>
                                <asp:ListItem Value="Session Court">Session Court</asp:ListItem>
                                <asp:ListItem Value="High Court">High Court</asp:ListItem>
                                <asp:ListItem Value="Supreme Court">Supreme Court</asp:ListItem>
                                <asp:ListItem Value="Anti Corruption">Anti Corruption Court</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Judge Name</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtJudgeName" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Party One</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtpartyone" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Party Two</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtpartytwo" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>City</label>
                            <asp:DropDownList ID="ddlCity" CssClass="form-control" runat="server" AppendDataBoundItems="False">
                                <asp:ListItem Value="0" Selected="True">Select City</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Status</label>
                            <asp:DropDownList ID="ddlStatus" CssClass="form-control" runat="server" AppendDataBoundItems="False">
                                <asp:ListItem Value="0" Selected="True">Select Status</asp:ListItem>
                                <asp:ListItem Value="Initiated">Initiated</asp:ListItem>
                                <asp:ListItem Value="Inprogress">In progress</asp:ListItem>
                                <asp:ListItem Value="DisposedOff">Disposed Off</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <label>Remarks*</label>
                            <asp:TextBox ID="txtarearemarks" runat="server" CssClass="form-control" TextMode="MultiLine" onkeyup="LimitText(this,50);" onblur="LimitText(this,50);" Height="80px"> </asp:TextBox>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <asp:Button Text="Save" CssClass="btn btn-primary" ID="btnSave" OnClick="Savebtn_Click" runat="server" />
                        <asp:Button Text="Clear" CssClass="btn btn-success" ID="btnClear" OnClick="Clearbtn_Click" runat="server" />
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:GridView ID="grdCourt" runat="server" CssClass="table table-responsive-sm" DataKeyNames="CourtID" OnSelectedIndexChanged="grdCourt_SelectedIndexChanged"
                                AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdCourt_PageIndexChanging" OnRowDataBound="grdCourt_rowbound"
                                EmptyDataText="No Record" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="br_nme" HeaderText="Division" />
                                    <asp:BoundField DataField="CaseTitle" HeaderText="Case Title" />
                                    <asp:BoundField DataField="caseDate" HeaderText="Case Date" />
                                    <asp:BoundField DataField="JudgeName" HeaderText="Judge Name" />
                                    <asp:BoundField DataField="partyOne" HeaderText="Party One" />
                                    <asp:BoundField DataField="partyTwo" HeaderText="Party Two" />
                                    <asp:BoundField DataField="status" HeaderText="Status" />
                                    <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                        <ControlStyle CssClass="lnk"></ControlStyle>
                                    </asp:CommandField>
                                </Columns>
                                <HeaderStyle CssClass="grid_hdr" />
                                <RowStyle CssClass="grid_row" />
                                <AlternatingRowStyle CssClass="gridAlternateRow" />
                                <SelectedRowStyle CssClass="gridSelectedRow" />
                            </asp:GridView>
                        </div>
                    </div>
                    <hr />
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <p><u style="color: white; font-size: 20px; background-color: #17a2b8; padding: 10px">Proceedings</u></p>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Case title</label>
                            <asp:DropDownList ID="ddlcasetitle" CssClass="form-control" runat="server" AppendDataBoundItems="False">
                                <asp:ListItem Value="0" Selected="True">Select Status</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Date</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtupdate" />
                            <ajaxToolkit:CalendarExtender ID="txtupdateCal" runat="server" TargetControlID="txtupdate" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Status</label>
                            <asp:DropDownList ID="ddlupdStatus" CssClass="form-control" runat="server" AppendDataBoundItems="False">
                                <asp:ListItem Value="0" Selected="True">Select Status</asp:ListItem>
                                <asp:ListItem Value="Pending">Pending</asp:ListItem>
                                <asp:ListItem Value="Inprogress">In progress</asp:ListItem>
                                <asp:ListItem Value="DisposedOff">Disposed Off</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Next Hearing Date</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtNextHearing" />
                            <ajaxToolkit:CalendarExtender ID="txtNextHearingCal" runat="server" TargetControlID="txtNextHearing" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <label>Remarks*</label>
                            <asp:TextBox ID="txtRemarksUpd" runat="server" CssClass="form-control" TextMode="MultiLine" onkeyup="LimitText(this,50);" onblur="LimitText(this,50);" Height="80px"> </asp:TextBox>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button Text="Updated" CssClass="btn btn-primary" OnClick="Update_Click" runat="server" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:GridView ID="grdUpd" runat="server" CssClass="table table-responsive-sm" DataKeyNames="CourtDetaiID" OnSelectedIndexChanged="grdUpd_SelectedIndexChanged"
                                AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdUpd_PageIndexChanging" OnRowDataBound="grdUpd_rowbound"
                                EmptyDataText="No Record" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="casetitle" HeaderText="Case Title" />
                                    <asp:BoundField DataField="DateUpdate" HeaderText="Update Date" />
                                    <asp:BoundField DataField="status" HeaderText="Status" />
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                    <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                        <ControlStyle CssClass="lnk"></ControlStyle>
                                    </asp:CommandField>
                                </Columns>
                                <HeaderStyle CssClass="grid_hdr" />
                                <RowStyle CssClass="grid_row" />
                                <AlternatingRowStyle CssClass="gridAlternateRow" />
                                <SelectedRowStyle CssClass="gridSelectedRow" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
