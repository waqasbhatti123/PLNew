<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="frmGLYear.aspx.cs" Inherits="RMS.GLSetup.frmGLYear" Title="GL Year" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function pageLoad() {

            if ($("#<%= ddlGLYrProcess.ClientID %>").val() == "0") {
                $("span[id*=lblText]").text('Move to next GL Year.');
                $("#<%= btnNext.ClientID %>").show();
                $("#<%= btnPrev.ClientID %>").hide();
            }
            else {
                $("span[id*=lblText]").text('Move to previous GL Year.');
                $("#<%= btnNext.ClientID %>").hide();
                $("#<%= btnPrev.ClientID %>").show();
            }

            $('#<%= ddlGLYrProcess.ClientID%>').change(function (event) {

                if ($("#<%= ddlGLYrProcess.ClientID %>").val() == "0") {
                    $("span[id*=lblText]").text('Move to next GL Year.');
                    $("#<%= btnNext.ClientID %>").show();
                    $("#<%= btnPrev.ClientID %>").hide();
                }
                else {
                    $("span[id*=lblText]").text('Move to previous GL Year.');
                    $("#<%= btnNext.ClientID %>").hide();
                    $("#<%= btnPrev.ClientID %>").show();
                }
            });
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4 ">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List" ValidationGroup="main" />
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:Label ID="lblText" runat="server"></asp:Label>
                            <asp:Label ID="lblGLYear" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:DropDownList ID="ddlGLYrProcess" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="0" Selected="True">Move To Next GL Year</asp:ListItem>
                                    <asp:ListItem Value="1">Move To Previous GL Year</asp:ListItem>
                                </asp:DropDownList>
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
                                <asp:Button ID="btnNext" runat="server" CssClass="btn btn-info" OnClick="btn_Click" Text="Move" />
                                <asp:Button ID="btnPrev" runat="server" CssClass="btn btn-info" OnClick="btn_Click" Text="Move" />
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
