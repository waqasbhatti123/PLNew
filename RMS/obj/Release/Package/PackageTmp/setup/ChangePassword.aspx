<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="ChangePassword.aspx.cs" Inherits="RMS.Setup.ChangePassword" Title="Untitled Page"
    Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4 ">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:ValidationSummary ID="main" CssClass="text-warning" runat="server" DisplayMode="List" ValidationGroup="main" />
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Old Password*</label>
                                <asp:TextBox ID="txtPswd_Old" runat="server" MaxLength="100" TextMode="Password"
                                    ValidationGroup="main" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="ReqValid_PswdOld" runat="server" ControlToValidate="txtPswd_Old"
                                    ErrorMessage="Please enter old password" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>New Password*</label>
                                <asp:TextBox ID="txtPswd_New" runat="server" MaxLength="100" TextMode="Password"
                                    ValidationGroup="main" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="ReqValid_PswdNew" runat="server" ControlToValidate="txtPswd_New"
                                    ErrorMessage="Please enter new password" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Confirm Password*</label>
                                <asp:TextBox ID="txtPswd_retype" runat="server" MaxLength="100" TextMode="Password"
                                    ValidationGroup="main" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="ReqValid_PswdRetype" runat="server" ControlToValidate="txtPswd_retype"
                                    ErrorMessage="Please enter confirm password" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPswd_retype"
                                    ControlToValidate="txtPswd_New" SetFocusOnError="true" Display="None" ErrorMessage="Retype password should be same as New Password"
                                    ValidationGroup="main"></asp:CompareValidator>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" ValidationGroup="main" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" Text="Clear" OnClick="btnCancel_Click" />
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
