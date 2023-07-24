<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    Culture="auto" UICulture="auto" EnableEventValidation="false"
    CodeBehind="frmGlLvlCodeAutoGenCode.aspx.cs" Inherits="RMS.GLSetup.frmGlLvlCodeAutoGenCode" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        //Disabling default submit behaiour
        function disableEnterKey(e) {
            var key;
            if (window.event)
                key = window.event.keyCode; //IE
            else
                key = e.which; //firefox
            return (key != 13);
        }
        document.onkeypress = disableEnterKey;

        function pageLoad() {
            $(".filter").keyup(function (event) {
                if (event.keyCode == 13) {
                    $(".filterclick").click();
                }
            });
        }

        function pageLoad() {

            $(".filter").keyup(function (event) {
                if (event.keyCode == 13) {
                    $(".filterclick").click();
                }
            });

            $("#<%= txtCode.ClientID %>").keyup(function (event) {

                var selectedTypeId1 = $("#<%= ddlCodeType.ClientID %>").val();
                var selectedTypeId2 = $("#<%= ddlCodeHead.ClientID %>").val();
                //            alert(selectedTypeId1);
                //            alert(selectedTypeId2);
                if ($("#<%= ddlCodeType.ClientID %>").val() != "") {

                    $.ajax({
                        url: "frmGlLvlCodeAutoGenCode.aspx/CodeVal",
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
                                    if ($("#<%= txtCode.ClientID %>").val().substring(0, heads[0].p_ct_len) != heads[0].gl_cd) {
                                        $("#<%= txtCode.ClientID %>").val(heads[0].gl_cd);
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

            <%--if ($("option:selected", $("#<%= ddlCodeType.ClientID %>")).text() != 'Detail') {
                $("#<%= lnkAddMore.ClientID %>").hide();
            }
            else {
                $("#<%= lnkAddMore.ClientID %>").show();
            }--%>

            if ($("option:selected", $("#<%= ddlVendorType.ClientID %>")).text() == 'Bank') {
                $("#<%= lblStn.ClientID %>").text('A/C No.:');
            }
            else {
                $("#<%= lblStn.ClientID %>").text('STN:');
            }
        }


        function LnkVendorStatus() {

            <%--if ($("option:selected", $("#<%= ddlCodeType.ClientID %>")).text() != 'Detail') {
                $("#<%= lnkAddMore.ClientID %>").hide();
            }
            else {
                $("#<%= lnkAddMore.ClientID %>").show();
            }--%>
        }

        function ChangeLabel() {

            if ($("option:selected", $("#<%= ddlVendorType.ClientID %>")).text() == 'Bank') {
                $("#<%= lblStn.ClientID %>").text('A/C No.:');
            }
            else {
                $("#<%= lblStn.ClientID %>").text('STN:');
            }

        }

    </script>




    <asp:UpdatePanel ID="upnlMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12">
                    <div class="card card-shadow mb-4 ">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <asp:ValidationSummary ID="main" CssClass="text-danger" runat="server" DisplayMode="List" ValidationGroup="main" />
                                    <uc1:Messages ID="ucMessage" runat="server" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-lg-4 col-md-4 col-sm-4">
                                        <label>Code Type*</label>
                                        <asp:DropDownList ID="ddlCodeType" runat="server" CssClass="form-control" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCodeType_SelectedIndexChanged" onchange="LnkVendorStatus()"></asp:DropDownList>
                                    </div>
                                    <div class="col-lg-4 col-md-4 col-sm-4">
                                            <label>GL Type*</label>
                                            <asp:DropDownList ID="ddlgltype" runat="server" AppendDataBoundItems="true" CssClass="form-control">
                                                <asp:ListItem Selected="True" Value="0">Select GL Type</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlgltype" InitialValue="0"
                                                ErrorMessage="Select GL Type" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
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
                                        <label>System Code*</label>
                                        <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCode" Display="None" ErrorMessage="Enter code." SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-lg-4 col-md-4 col-sm-4">
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="row">
                                    <div class="col-lg-4 col-md-4 col-sm-4">
                                        <label>Description*</label>
                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDescription" Display="None" ErrorMessage="Enter description." SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-lg-4 col-md-4 col-sm-4">
                                        <label>Code*</label>
                                        <asp:TextBox ID="txtManualCode" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

         
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <asp:LinkButton ID="lnkAddMore" runat="server" OnClick="lnkAddMore_ClearVendor" Text="Attach A/C Info" CssClass="btn btn-info "></asp:LinkButton>
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" ValidationGroup="main" />
                                        <asp:Button ID="btnClear" runat="server" CssClass="btn btn-danger" Text="Clear" OnClick="btnClear_Click" />
                                    </div>
                                </div>
                        </div>
                    </div>
                </div>
            </div>





       <asp:Panel ID="popupPnl" runat="server" CssClass="bg-info" Style="display: none;">
        <div id="pnlDiv" class="model_popup_panel_hdr pl-2">
            <asp:Label ID="lblHdr" runat="server" Text="A/C Information"></asp:Label>
        </div>
           <br />
        <asp:ValidationSummary ID="ValidationSummary1" CssClass="text-dark pl-2" runat="server" DisplayMode="List" ValidationGroup="Vendor" />
        <uc1:Messages ID="ucMsgVendor" runat="server" />

           <br />

        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="3%"></td>
                <td>
                    <table width="100%">
                        <tr valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="Label1" runat="server" Text="Divisions:" Width="110"></asp:Label>
                            </td>
                            <td>
                            <asp:DropDownList ID="PnlDivision" runat="server" CssClass=" searchbranchchange" 
                                AppendDataBoundItems="True" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Division</asp:ListItem>
                            </asp:DropDownList>
                                </td>
                        </tr>
                        <tr valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="lblVendorType" runat="server" Text="A/C Type:" Width="110"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlVendorType" runat="server" CssClass="RequiredField" AppendDataBoundItems="true" onchange="ChangeLabel()">
                                    <asp:ListItem Value="0" Selected="True">Select A/C Type</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlVendorType" InitialValue="0"
                                    ErrorMessage="Select A/C type." SetFocusOnError="true" ValidationGroup="Vendor" Display="None"></asp:RequiredFieldValidator>
                            </td>
                        </tr>

                        <tr valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="Label5" runat="server" Text="NTN:" Width="110"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNTN" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="lblStn" runat="server" Text="STN:" Width="110"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSTN" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="Label8" runat="server" Text="Telephone No:" Width="110"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTelNo" runat="server" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>

                        <tr valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="Label14" runat="server" Text="Fax No:" Width="110"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFaxNo" runat="server" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="Label9" runat="server" Text="City:" Width="110"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCity" runat="server" AppendDataBoundItems="true" CssClass="RequiredField">
                                    <asp:ListItem Value="0" Selected="True">Select City</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlCity" InitialValue="0"
                                    ErrorMessage="Select city." SetFocusOnError="true" ValidationGroup="Vendor" Display="None"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="Label10" runat="server" Text="Address:" Width="110"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" CssClass="RequiredField" onkeyup="LimitText(this,200);" onblur="LimitText(this,200);" Width="150" Height="80"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtAddress" Display="None" ErrorMessage="Enter address." SetFocusOnError="true" ValidationGroup="Vendor"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="Label7" runat="server" Text="Contact Person:" Width="110"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactPerson" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="Label13" runat="server" Text="Cell No:" Width="110"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCellNo" runat="server" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="Label11" runat="server" Text="Email:" Width="110"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td width="3%"></td>
            </tr>
        </table>

        <div style="height: 10;">&nbsp;</div>

        <div style="margin-left: 3%">
            <asp:Button ID="btnSaveVendor" runat="server" CssClass="btn btn-primary" Text="Attach" OnClick="btnSaveVendor_Click" ValidationGroup="Vendor" />
            <asp:Button ID="btnCancelVendor" runat="server" CssClass="btn btn-danger" Text="Cancel" OnClick="btnCancel_Click"/>
        </div>
        <div style="height: 10;">&nbsp;</div>
    </asp:Panel>

    <ajaxToolkit:ModalPopupExtender ID="MPE" runat="server"
        TargetControlID="lnkAddMore"
        PopupControlID="popupPnl"
        BackgroundCssClass="modalBackground"
        CancelControlID="btnCancelVendor"
        DropShadow="false"
        PopupDragHandleControlID="pnlDiv" X="600">
    </ajaxToolkit:ModalPopupExtender>



        </ContentTemplate>
    </asp:UpdatePanel>




    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4 ">
                <div class="card-body">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Code*</label>
                                <asp:TextBox runat="server" ID="txtFltCode" class="form-control"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Description*</label>
                                <asp:TextBox runat="server" ID="txtFltDesc" MaxLength="50" class="form-control"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Code Type*</label>
                                <asp:DropDownList ID="ddlFltCodeType" runat="server" AppendDataBoundItems="true" class="form-control">
                                    <asp:ListItem Value="All">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>GL Type</label>
                            <asp:DropDownList ID="ddlFltGlType" runat="server" AppendDataBoundItems="true" class="form-control">
                                <asp:ListItem Value="-">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4" style="margin-top:30px;">
                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search Code" Visible="true" class="btn btn-info" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                        </div>
                    </div>
                </div>
                    
                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12">
                        <asp:GridView ID="grdcode" runat="server" CssClass="table table-responsive-sm" DataKeyNames="gl_cd, ct_id,codetype, headgl_cd, gt_cd" OnSelectedIndexChanged="grdcode_SelectedIndexChanged" AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No record found." Width="100%" PageSize="25" OnPageIndexChanging="grdcode_PageIndexChanging" OnRowDataBound="grdcode_RowDataBound" OnDataBound="grdcode_DataBound">
                            <Columns>
                                <asp:BoundField DataField="gl_cd" HeaderText="GL Code" />
                                <asp:BoundField DataField="code" HeaderText="Code" />
                                <asp:BoundField DataField="gl_dsc" HeaderText="Description" />
                                <asp:BoundField DataField="codetype" HeaderText="Code Type" />
                                <asp:BoundField DataField="gl_type" HeaderText="GL Type" />
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


</asp:Content>
