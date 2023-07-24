<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"
    AutoEventWireup="true" CodeBehind="ADPScheme.aspx.cs" Inherits="RMS.GLSetup.ADPScheme"
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
                            <label>Scheme ID</label>
                            <asp:TextBox runat="server" ID="txtSchemeID" CssClass="form-control" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Scheme Title</label>
                            <asp:TextBox runat="server" ID="txtSchemeTitle" CssClass="form-control" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Approval Date</label>
                            <asp:TextBox runat="server" ID="txtApprovalDate" CssClass="form-control" />
                            <ajaxToolkit:CalendarExtender ID="txtApprovalDateCal" runat="server" TargetControlID="txtApprovalDate" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Location</label>
                            <asp:DropDownList ID="ddlDivisional" CssClass="form-control" runat="server" AppendDataBoundItems="False">
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Scheme Category</label>
                            <asp:TextBox runat="server" ID="txtSchemeCat" CssClass="form-control" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Estimated Cost(In Million)</label>
                            <asp:TextBox runat="server" ID="txtEstiCost" CssClass="form-control" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Status*</label>
                            <asp:DropDownList ID="ddlStatus" CssClass="form-control" runat="server" AppendDataBoundItems="False">
                                <asp:ListItem Value="0">Select Status</asp:ListItem>
                                <asp:ListItem Value="pending">Pending</asp:ListItem>
                                <asp:ListItem Value="OnGoing">On Going</asp:ListItem>
                                <asp:ListItem Value="DisposedOff">Disposed Off</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3" style="margin-top:35px">
                            <asp:CheckBox ID="checkIsActive" Checked="true" class="checkbox" runat="server" />&nbsp;
                                <label id="per">Is Active</label>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBasicPay"
                                    ErrorMessage="Please Enter Basic Pay" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button runat="server" CssClass="btn btn-primary" ID="btnSave" OnClick="Save_click" Text="Save" />
                            <asp:Button runat="server" CssClass="btn btn-success" ID="btnClear" OnClick="Clear_Click" Text="Clear" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:GridView ID="grdAdpScheme" runat="server" CssClass="table table-responsive-sm" DataKeyNames="ADID" OnSelectedIndexChanged="grdadp_SelectedIndexChanged"
                                AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdadp_PageIndexChanging" OnRowDataBound="grdadp_RowDataBound"
                                EmptyDataText="No Scheme define" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="title" HeaderText="Scheme Title" />
                                    <asp:BoundField DataField="SchemeID" HeaderText="Scheme ID" />
                                    <asp:BoundField DataField="EstimatedCost" HeaderText="Estimated Cost" />
                                    <asp:BoundField DataField="AppDate" HeaderText="Approved Date" />
                                    <asp:BoundField DataField="br_nme" HeaderText="Location" />
                                    <asp:BoundField DataField="status" HeaderText="Status" />
                                    <asp:BoundField DataField="isActive" HeaderText="Active" />
                                    <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                        <ControlStyle CssClass="lnk"></ControlStyle>
                                    </asp:CommandField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <hr />
                    &nbsp;
                    <div class="col-lg-12 col-md-12 col-sm-12">
                        <h1 style="text-align: center; font-size: 30px">Scheme Progress</h1>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Scheme Title</label>
                            <asp:DropDownList ID="ddlSchemeTitle" CssClass="form-control" runat="server" AppendDataBoundItems="False">
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Finaincial Year</label>
                            <asp:DropDownList ID="SelectedYear" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Year</asp:ListItem>
                                <asp:ListItem Value="2020-2021">2020 - 2021</asp:ListItem>
                                <asp:ListItem Value="2021-2022">2021 - 2022</asp:ListItem>
                                <asp:ListItem Value="2022-2023">2022 - 2023</asp:ListItem>
                                <asp:ListItem Value="2023-2024">2023 - 2024</asp:ListItem>
                                <asp:ListItem Value="2024-2025">2024 - 2025</asp:ListItem>
                                <asp:ListItem Value="2025-2026">2025 - 2026</asp:ListItem>
                                <asp:ListItem Value="2026-2027">2026 - 2027</asp:ListItem>
                                <asp:ListItem Value="2027-2028">2027 - 2028</asp:ListItem>
                                <asp:ListItem Value="2028-2029">2028 - 2029</asp:ListItem>
                                <asp:ListItem Value="2029-2030">2029 - 2030</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Allocation(in million)</label>
                            <asp:TextBox runat="server" ID="txtAllocationPrice" CssClass="form-control" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Release(in million)</label>
                            <asp:TextBox runat="server" ID="txtrealseRrice" CssClass="form-control" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Total Exp. Upto(in million)</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtTotalExp" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <label>Progress*</label>
                            <%--<asp:TextBox ID="EnqRemarks" runat="server" CssClass="form-control"></asp:TextBox>--%>
                            <asp:TextBox ID="txtarearemaks" runat="server" CssClass="form-control" TextMode="MultiLine" onkeyup="LimitText(this,1000);" onblur="LimitText(this,1000);" Height="80px"> </asp:TextBox>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button Text="Save" ID="btnSave1" CssClass="btn btn-primary" OnClick="btnProgress_Save" runat="server" />
                            <asp:Button Text="Clear" CssClass="btn btn-success" ID="btnClear2" OnClick="btnProgress_Clear" runat="server" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:GridView ID="grdPro" runat="server" CssClass="table table-responsive-sm" DataKeyNames="ADPID" OnSelectedIndexChanged="grdpro_SelectedIndexChanged"
                                AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdpro_PageIndexChanging" OnRowDataBound="grdpro_RowDataBound"
                                EmptyDataText="No Scheme define" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="title" HeaderText="Scheme Title" />
                                    <asp:BoundField DataField="year" HeaderText="Fin Year " />
                                    <asp:BoundField DataField="Allocation" HeaderText="Allocation" />
                                    <asp:BoundField DataField="Release" HeaderText="Release" />
                                    <asp:BoundField DataField="TotalExp" HeaderText="Total Exp" />
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
