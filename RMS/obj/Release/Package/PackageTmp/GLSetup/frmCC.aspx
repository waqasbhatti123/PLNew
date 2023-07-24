<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    Culture="auto" UICulture="auto" EnableEventValidation="false"
    CodeBehind="frmCC.aspx.cs" Inherits="RMS.GLSetup.frmCC" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">

        function pageLoad() {

            $("#<%= txtCode.ClientID %>").keyup(function (event) {

                var selectedTypeId1 = $("#<%= ddlCodeType.ClientID %>").val();
                var selectedTypeId2 = $("#<%= ddlCodeHead.ClientID %>").val();
                //            alert(selectedTypeId1);
                //            alert(selectedTypeId2);
                if ($("#<%= ddlCodeType.ClientID %>").val() != "") {

                    $.ajax({
                        url: "frmCC.aspx/CodeVal",
                        data: JSON.stringify({ selectedTypeId1: selectedTypeId1, selectedTypeId2: selectedTypeId2 }),
                        type: 'POST',
                        contentType: 'application/json;',
                        dataType: 'json',
                        success: function (heads) {
                            var heads = heads.d;
                            if (heads != null) {
                                if (heads.length > 0) {
                                //                            alert(heads[0].ct_len);
                                //                            alert(heads[0].gl_cd);
                                //                            alert($("#<%= txtCode.ClientID %>").val().substring(0, heads[0].p_ct_len));
                                    //                            alert(heads[0].p_ct_len);
                                    if ($("#<%= txtCode.ClientID %>").val().substring(0, heads[0].p_ct_len) != heads[0].cc_cd) {
                                        $("#<%= txtCode.ClientID %>").val(heads[0].cc_cd);
                                    }
                                }
                            }
                        }

                    });
                }
            });

            $("#<%= txtCode.ClientID %>").keydown(function (event) {

                if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                    event.preventDefault();
                }
            });


        }

    </script>

    <div class="row">
        <div class="col-md-12">
            <div class="card card-shadow mb-4 ">
                <div class="card-body">
                    <asp:UpdatePanel ID="upnlMain" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>

                            <div class="form-group">
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12">
                                            <asp:ValidationSummary ID="main" CssClass="text-warning" runat="server" DisplayMode="List" ValidationGroup="main" />
                                            <uc1:Messages ID="ucMessage" runat="server" />
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-4 col-md-4 col-sm-4">
                                        <label>Code Type*</label>
                                        <asp:DropDownList ID="ddlCodeType" runat="server" CssClass="form-control" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCodeType_SelectedIndexChanged"></asp:DropDownList>
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
                                        <label>Code Head*</label>
                                        <asp:DropDownList ID="ddlCodeHead" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCodeHead_SelectedIndexChanged"></asp:DropDownList>
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
                                        <label>Code*</label>
                                        <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" Width="120" Enabled="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCode" Display="None" ErrorMessage="Enter code." SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-lg-4 col-md-4 col-sm-4">
                                    </div>
                                    <div class="col-lg-4 col-md-4 col-sm-4">
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <label>Description*</label>
                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" onkeyup="LimitText(this,100);" onblur="LimitText(this,100);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDescription" Display="None" ErrorMessage="Enter description." SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group" style="display:none">
                                <div class="row">
                                    <div class="col-lg-4 col-md-4 col-sm-4">
                                        <label>Visible At*</label>
                                        <asp:DropDownList ID="ddlCCType" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                            <asp:ListItem Selected="True" Value="Al">Everywhere</asp:ListItem>
                                            <asp:ListItem Value="GL">GL</asp:ListItem>
                                            <asp:ListItem Value="Iv">Inventory</asp:ListItem>
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
                                        <labl>Status*</labl>
                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                            <asp:ListItem Selected="True" Value="1">Enabled</asp:ListItem>
                                            <asp:ListItem Value="0">Disabled</asp:ListItem>
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
                                        <asp:Button runat="server" ID="btnSave" class="btn btn-primary" Text="Save"
                                            OnClick="btnSave_Click" ValidationGroup="main"></asp:Button>
                                        <asp:Button runat="server" ID="btnClear" class="btn btn-danger" Text="Clear" OnClick="btnClear_Click" />
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="card card-shadow mb-4 ">
                <div class="card-body">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Code*</label>
                                <asp:TextBox runat="server" ID="txtFltCode" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Description*</label>
                                <asp:TextBox runat="server" ID="txtFltDesc" CssClass="form-control" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Code Type*</label>
                                <asp:DropDownList ID="ddlFltCodeType" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                    <asp:ListItem Value="All">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Button runat="server" ID="btnSearch" class="btn btn-info" Text="Search" OnClick="btnSearch_Click" />
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:GridView ID="grdcode" runat="server" CssClass="table table-responsive-sm" DataKeyNames="cc_cd, cct_id,codetype, headgl_cd" OnSelectedIndexChanged="grdcode_SelectedIndexChanged" AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No record found." Width="100%" PageSize="25" OnPageIndexChanging="grdcode_PageIndexChanging" OnRowDataBound="grdcode_RowDataBound" OnDataBound="grdcode_DataBound">
                                <Columns>
                                    <asp:BoundField DataField="cc_cd" HeaderText="Code" />
                                    <asp:BoundField DataField="cc_nme" HeaderText="Description" />
                                    <asp:BoundField DataField="codetype" HeaderText="Code Type" />
                                    <asp:BoundField DataField="typ" HeaderText="Visible At" />
                                    <asp:BoundField DataField="Status" HeaderText="Type" />
                                    <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                                        <ItemStyle />
                                        <ControlStyle></ControlStyle>
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
