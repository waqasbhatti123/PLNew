<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="frmGl_Type.aspx.cs" Inherits="RMS.GL.Setup.frmGl_Type" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>


<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:ValidationSummary ID="main" CssClass="text-warning" runat="server" DisplayMode="List" ValidationGroup="main" />
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:GridView ID="grdtype" runat="server" CssClass="table table-responsive-sm" DataKeyNames="gt_cd" OnSelectedIndexChanged="grdtype_SelectedIndexChanged"
                                    AutoGenerateColumns="False" AllowPaging="True" Width="100%" PageSize="20" OnPageIndexChanging="grdtype_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="gt_cd" HeaderText="Code" />
                                        <asp:BoundField DataField="gt_dsc" HeaderText="Description" />
                                        <asp:BoundField DataField="gt_cf" HeaderText="Balance C/F" />
                                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                                            <ItemStyle />
                                            <ControlStyle></ControlStyle>
                                        </asp:CommandField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6">
                                        <label>Code*</label>
                                        <asp:TextBox ID="txtCode" CssClass="form-control" runat="server" MaxLength="150"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCode"
                                            ErrorMessage="Please enter code" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-6">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6">
                                        <label>Description*</label>
                                        <asp:TextBox ID="txtName" CssClass="form-control" runat="server" MaxLength="150"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                            ErrorMessage="Please enter description" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-6">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6">
                                        <label>Balance C/F*</label>
                                        <asp:CheckBox ID="chkbalance" runat="SErver" CssClass="form-control" />
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-6">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
