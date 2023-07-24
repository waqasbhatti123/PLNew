<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/home/RMSMasterHome.Master"
    CodeBehind="frmLedgerCard.aspx.cs" Inherits="RMS.GLSetup.frmLedgerCard" %>


<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.jquery.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.min.css" rel="stylesheet" />
    <script type="text/javascript">
        $(function () {
            $('.code').autocomplete({
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

            $('.code').blur(function () {
                selectval = $(this).val().split(" ");
                $(this).val(selectval[0]);

            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4 ">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>

                       <div class="form-group">
                        <div class="row">

                            <div class="col-lg-4 col-md-4 col-sm-4">
                               <label>Divisions*</label>
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged"
                                AppendDataBoundItems="True" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Division</asp:ListItem>
                            </asp:DropDownList>
                            </div>

                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Status*</label>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="P">Pending</asp:ListItem>
                                    <asp:ListItem Value="A" Selected="True">Approved</asp:ListItem>
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>From Date*</label>
                                <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" />
                                <ajaxToolkit:CalendarExtender ID="txtFromDate" runat="server" TargetControlID="txtFrom" Enabled="True" />
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>To Date*</label>
                                <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" />
                                <ajaxToolkit:CalendarExtender ID="txtToDate" runat="server" TargetControlID="txtTo" Enabled="True" CssClass="form-control" />
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Account Heads</label>
                                <%--<asp:TextBox ID="txtcodefrom" class="code" runat="server" CssClass="form-control" />--%>
                                <asp:DropDownList ID="codeDropDown" runat="server" CssClass="form-control acount" AppendDataBoundItems="True">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                              <%--  <label>To Code</label>
                                <asp:TextBox ID="txtcodeto" class="code" runat="server" CssClass="form-control" />--%>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                        </div>
                    </div>
                 
                    <asp:Button ID="Button1" runat="server" Text="Report" OnClick="btnGenerat_Click" CssClass="btn btn-primary" />

                      <br />
                    &nbsp;

    <asp:Panel ID="pnlMain" runat="server" Width="99%" Height="600">
        <rsweb:ReportViewer ID="viewer" runat="server" Width="100%" Height="580px">
        </rsweb:ReportViewer>
    </asp:Panel>
                </div>
            </div>
        </div>
    </div>



  <script>
        $(".acount").chosen();
    </script>

</asp:Content>
