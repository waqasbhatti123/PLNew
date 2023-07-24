<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" Culture="auto"
    UICulture="auto" EnableEventValidation="true" AutoEventWireup="true"
    CodeBehind="frmPreferences.aspx.cs" Inherits="RMS.GL.Setup.frmPreferences" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>
    <script type="text/javascript">
        $(function () {

    //$('#<%= txtcashcode.ClientID %>').autocomplete({
            $('.form-control').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "frmPreferences.aspx/GetControlAccount",
                        data: "{ 'sname': '" + request.term + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",

                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    value: item,
                                    result: item
                                }
                            }))
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus);
                        }
                    });
                },
                minLength: 1
            });
        });

        $(function () {
            $('.form-control').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "frmPreferences.aspx/GetDetailAccount",
                        data: "{ 'sname': '" + request.term + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",

                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    value: item,
                                    result: item
                                }
                            }))
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus);
                        }
                    });
                },
                minLength: 1
            });
        });

    </script>
    <script type="text/javascript">

        $(document).ready(function () {
            $('.form-control').blur(function () {
                selectval = $(this).val().split(" ");
                $(this).val(selectval[0]);

            });

            $('.form-control').blur(function () {
                selectval = $(this).val().split(" ");
                $(this).val(selectval[0]);

            });



        });


    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4 ">
                <div class="card-header">
                    <div class="card-title">
                        <small>GL Codes</small>
                    </div>
                </div>
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
                                <label>Cash Account Head*</label>
                                <asp:TextBox ID="txtcashcode" class="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Bank Account Head*</label>
                                <asp:TextBox ID="txtbankcode" class="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Vendor Account Head*</label>
                                <asp:TextBox ID="txtvendor" class="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Department Account*</label>
                                <asp:TextBox ID="txtdepartment" class="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Customer Account Head*</label>
                                <asp:TextBox ID="txtcustomer" class="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Bank Account*</label>
                                <asp:TextBox ID="txtbankaccount" class="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Cash Account*</label>
                                <asp:TextBox ID="txtcashaccount" class="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Income Tax*</label>
                                <asp:TextBox ID="txtIncomeTax" class="form-control" runat="server"></asp:TextBox>
                            </div>
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Gst Tax*</label>
                                <asp:TextBox ID="txtGSTTax" class="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>PRA Tax*</label>
                                <asp:TextBox ID="txtPraTax" class="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Temp Account*</label>
                                <asp:TextBox ID="txtTempAcc" class="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Label ID="lblapprove" runat="server" Text="Approve" Visible="false"></asp:Label>
                                <asp:CheckBox ID="chkapprove" runat="server" Visible="false" />
                                <asp:Label ID="lblprint" runat="server" Text="Print" Visible="false"></asp:Label>
                                <asp:CheckBox ID="chkprint" runat="server" Visible="false" />
                                <asp:Button ID="btnSaveCode" runat="server" CssClass="btn btn-primary" Text="Save" OnCommand="btnSaveCodes_Click" ValidationGroup="main" />
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



    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4 ">
                <div class="card-header">
                    <div class="card-title">
                        <small>Inventory Codes</small>
                    </div>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:ValidationSummary ID="ValidationSummary1" CssClass="text-warning" runat="server" DisplayMode="List" ValidationGroup="main1" />
                            <uc1:Messages ID="ucMessage1" runat="server" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Other Amount Account*</label>
                                <asp:TextBox ID="txtAmntCode" class="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>GST Account*</label>
                                <asp:TextBox ID="txtgstCode" class="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>WHT Account*</label>
                                <asp:TextBox ID="txtwhtCode" class="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Custom Duty Account*</label>
                                <asp:TextBox ID="txtCustCode" class="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Freight Account*</label>
                                <asp:TextBox ID="txtFreight" class="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Other Cost Account*</label>
                                <asp:TextBox ID="txtOtherCost" class="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Imported Freight Account*</label>
                                <asp:TextBox ID="txtImpFreight" class="form-control" runat="server"></asp:TextBox>
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
                                <asp:Button ID="btnSaveInv" runat="server" CssClass="btn btn-primary" Text="Save" OnCommand="btnSaveInvent_Click" ValidationGroup="main1" />
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
