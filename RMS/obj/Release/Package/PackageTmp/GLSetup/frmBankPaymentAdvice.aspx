<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/home/RMSMasterHome.Master"
    CodeBehind="frmBankPaymentAdvice.aspx.cs" Culture="auto" UICulture="auto"
    Inherits="RMS.GLSetup.frmBankPaymentAdvice" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function pageLoad() {
            calculateChequeAmount();
            $('#<%= txtnarration.ClientID %>').focus();

            var glCd;
            $('#<%= txtChqBranch.ClientID %>').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "frmVoucherDetail.aspx/GetBranch",
                    data: "{ 'bank': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function (data) { return data; },
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                value: item.gl_cd + ' ~ ' + item.gl_dsc,
                                result: item.STN,
                                id: item.gl_cd
                            }
                        }))
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            },
            select: function (e, ui) {
                glCd = ui.item.id;
                if (ui.item.value != '') {
                    $('#<%= txtChqBranch.ClientID %>').val(ui.item.value);
                    $('#<%= hdnAccNo.ClientID %>').val(ui.item.id);
                    $('#<%= txtPayeeAc.ClientID %>').focus();
                }
                else {
                    alert('Invalid bank/branch account');
                }
            },
            minLength: 1
        });


        $("input[type=text][id*=txtPayeeAc]").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "frmPreferences.aspx/GetDetailAccount1",
                    data: "{ 'sname': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    async: false,
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function (data) { return data; },
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                value: item.gl_cd + ' ~ ' + item.gl_dsc,
                                result: item.gl_cd + ' ~ ' + item.gl_dsc,
                                id: item.gl_cd
                            }
                        }))
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            },
            select: function (e, ui) {
                var codeItm = ui.item.result;
                codeItm = codeItm.split(" ~ ");
                $('#<%= txtPayeeAc.ClientID %>').val(codeItm[0] + ' ~ ' + codeItm[1]);
                $('#<%= txtPayee.ClientID %>').val(codeItm[1]);
                $('#<%= hdnAccPayee.ClientID %>').val(ui.item.id);
                $('#<%= txtRefSource.ClientID %>').focus();
                return false;
            },

            minLength: 1
        });

        $("input[type=text][id*=txtAcWHT]").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "frmPreferences.aspx/GetDetailAccount1",
                    data: "{ 'sname': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    async: false,
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function (data) { return data; },
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                value: item.gl_cd + ' ~ ' + item.gl_dsc,
                                result: item.gl_cd + ' ~ ' + item.gl_dsc,
                                id: item.gl_cd
                            }
                        }))
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            },
            select: function (e, ui) {
                var codeItm = ui.item.result;
                codeItm = codeItm.split(" ~ ");
                $('#<%= txtAcWHT.ClientID %>').val(codeItm[0] + ' ~ ' + codeItm[1]);
                    $('#<%= hdnAcWHT.ClientID %>').val(ui.item.id);
                    return false;
                },

                minLength: 1
            });

            $('#<%= txtAmountPayable.ClientID %>').change(function (e) {
                calculateChequeAmount();
            });
            $('#<%= txtWHT.ClientID %>').change(function (e) {
                calculateChequeAmount();
            });
            $('#<%= txtAmountPayable.ClientID %>').keyup(function (e) {
                calculateChequeAmount();
            });
            $('#<%= txtWHT.ClientID %>').keyup(function (e) {
                calculateChequeAmount();
            });
            function calculateChequeAmount() {

                var amountPayable = $('#<%= txtAmountPayable.ClientID %>').val();
                if (isNaN(amountPayable))
                    amountPayable = 0;
                var wht = $('#<%= txtWHT.ClientID %>').val();
            if (isNaN(wht))
                wht = 0;
            $('#<%= txtChequeAmount.ClientID %>').val(amountPayable - wht);
            }
            //only decimals
            $("input[type=text][id*=txtAmountPayable]").keydown(function (event) {
                if (event.keyCode == 13) {
                    $(event.target).closest('tr').find("input[type=text][id*=txtAmountPayable]").focus();

                }
                var txtDbt = $(this).closest('tr').find("input[type=text][id*=txtAmountPayable]").val();
                if (event.keyCode != 110 && event.keyCode != 190 && (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                    event.preventDefault();
                }
                if (event.keyCode == 110 || event.keyCode == 190) {
                    if ((txtDbt.split(".").length) > 1) {
                        event.preventDefault();
                    }
                }
            });
            $("input[type=text][id*=txtAmountPayable]").keyup(function (e) {
            });
            $("input[type=text][id*=txtAmountPayable]").change(function (e) {
            });
            $("input[type=text][id*=txtWHT]").keydown(function (event) {
                if (event.keyCode == 13) {
                    $(event.target).closest('tr').find("input[type=text][id*=txtWHT]").focus();

                }
                var txtDbt = $(this).closest('tr').find("input[type=text][id*=txtWHT]").val();
                if (event.keyCode != 110 && event.keyCode != 190 && (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                    event.preventDefault();
                }
                if (event.keyCode == 110 || event.keyCode == 190) {
                    if ((txtDbt.split(".").length) > 1) {
                        event.preventDefault();
                    }
                }
            });
            $("input[type=text][id*=txtWHT]").keyup(function (e) {
            });
            $("input[type=text][id*=txtWHT]").change(function (e) {
            });
            $("input[type=text][id*=txtChequeAmount]").keydown(function (event) {
                if (event.keyCode == 13) {
                    $(event.target).closest('tr').find("input[type=text][id*=txtChequeAmount]").focus();

                }
                var txtDbt = $(this).closest('tr').find("input[type=text][id*=txtChequeAmount]").val();
                if (event.keyCode != 110 && event.keyCode != 190 && (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                    event.preventDefault();
                }
                if (event.keyCode == 110 || event.keyCode == 190) {
                    if ((txtDbt.split(".").length) > 1) {
                        event.preventDefault();
                    }
                }
            });
            $("input[type=text][id*=txtChequeAmount]").keyup(function (e) {
            });
            $("input[type=text][id*=txtChequeAmount]").change(function (e) {
            });
            //end only decimals
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <asp:ValidationSummary ID="onesumm" CssClass="ErrorSummary" runat="server" DisplayMode="List" ValidationGroup="one" />
    <uc1:Messages ID="ucMessage" runat="server" />


    <asp:Image ID="imgChq" Visible="false" runat="server" Height="100" Width="100" />

    <table width="100%" border="0" cellspacing="0" cellpadding="0">

        <tr>
            <td></td>
            <td>
                <table width="100%" cellspacing="0">
                    <tr>
                        <td>
                            
                              <label>Branch</label>
                        </td>
                        <td>
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged"
                                AppendDataBoundItems="True" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Branch</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblstatus" Text="Status" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlstatus" runat="server" Width="100" AppendDataBoundItems="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbldate" Text="Date" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtdt" runat="server" Width="100"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtDate" runat="server" TargetControlID="txtdt" Enabled="True" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblnarration" Text="Narration" runat="server"></asp:Label>
                            *
                        </td>
                        <td>
                            <asp:TextBox ID="txtnarration" runat="server" TextMode="MultiLine" onkeyup="LimitText(this,100);" onblur="LimitText(this,100);" Width="400" Height="50" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="one" runat="server" ControlToValidate="txtnarration"
                                ErrorMessage="Please enter narration" SetFocusOnError="true" Display="None"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>A/C #
                        </td>
                        <td>
                            <asp:TextBox ID="txtChqAcctNo" runat="server" MaxLength="20" Width="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Cheque # *
                        </td>
                        <td>
                            <asp:TextBox ID="txtChqNo" runat="server" MaxLength="20" Width="100"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtChqNo"
                                ErrorMessage="Please enter cheque #" SetFocusOnError="true" ValidationGroup="one" Display="None"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Cheque Date
                        </td>
                        <td>
                            <asp:TextBox ID="txtChqDate" runat="server" Width="100"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtChqDateCal" runat="server" TargetControlID="txtChqDate" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtChqDate"
                                ErrorMessage="Please select date of cheque" SetFocusOnError="true" ValidationGroup="one" Display="None"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Bank/Branch *
                        </td>
                        <td>
                            <asp:TextBox ID="txtChqBranch" runat="server" MaxLength="50" Width="400"></asp:TextBox>
                            <asp:HiddenField ID="hdnAccNo" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtChqBranch"
                                ErrorMessage="Please enter bank/branch" SetFocusOnError="true" ValidationGroup="one" Display="None"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Payee A/C *
                        </td>
                        <td>
                            <asp:TextBox ID="txtPayeeAc" runat="server" MaxLength="200" Width="400"></asp:TextBox>
                            <asp:HiddenField ID="hdnAccPayee" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtPayeeAc"
                                ErrorMessage="Please enter payee A/C" SetFocusOnError="true" ValidationGroup="one" Display="None"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Payee *
                        </td>
                        <td>
                            <asp:TextBox ID="txtPayee" runat="server" MaxLength="200" Width="400"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtPayee"
                                ErrorMessage="Please enter payee" SetFocusOnError="true" ValidationGroup="one" Display="None"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Payment Reference
                        </td>
                        <td>
                            <asp:TextBox ID="txtRefSource" runat="server" Width="100" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Amount Payable (Rs.) *
                        </td>
                        <td>
                            <asp:TextBox ID="txtAmountPayable" runat="server" Width="100" MaxLength="13"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtAmountPayable"
                                ErrorMessage="Please enter payable amount" SetFocusOnError="true" ValidationGroup="one" Display="None"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>WHT (Rs.) / Account
                        </td>
                        <td>
                            <asp:TextBox ID="txtWHT" runat="server" Width="100" MaxLength="13"></asp:TextBox>
                            <asp:TextBox ID="txtAcWHT" runat="server" Width="295"></asp:TextBox>
                            <asp:HiddenField ID="hdnAcWHT" runat="server" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtWHT"
                                ErrorMessage="Please enter WHT" SetFocusOnError="true" ValidationGroup="one" Display="None"></asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>Cheque Amount (Rs.) *   
                        </td>
                        <td>
                            <asp:TextBox ID="txtChequeAmount" runat="server" Width="100" MaxLength="50" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>A/C Payee Only   
                        </td>
                        <td>
                            <asp:CheckBox ID="chkAcPayeeOnly" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/images/btn_save.png" OnClick="btnSave_Click"
                                onMouseOver="this.src='../images/btn_save_m.png'" onMouseOut="this.src='../images/btn_save.png'" ValidationGroup="one" />
                            <asp:ImageButton ID="btnPrintCheque" runat="server" ImageUrl="~/images/btn_print.PNG" OnClick="btnPrintCheque_Click"
                                onMouseOver="this.src='../images/btn_print_m.png'" onMouseOut="this.src='../images/btn_print.png'" />
                            <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="~/images/btn_clear.png" OnClick="btnCancel_Click"
                                onMouseOver="this.src='../images/btn_clear_m.png'" onMouseOut="this.src='../images/btn_clear.png'" />
                        </td>
                    </tr>
                </table>
            </td>
            <td></td>
        </tr>




        <tr>
            <td></td>
            <td>
                <asp:Panel ID="pnlFilter" runat="server" DefaultButton="ImageButton2">
                    <table class="filterTable" cellpadding="1" cellspacing="2" width="100%">
                        <tr>
                            <td colspan="9">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td><%--Voucher#--%></td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFltCode" Width="80" class="filter" Visible="false"></asp:TextBox>
                            </td>
                            <td>Date From:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFrom" Width="80px" MaxLength="50" class="filter"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="txtFromDate" runat="server" TargetControlID="txtFrom" Enabled="True" />
                            </td>
                            <td>Date To:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtTo" Width="80px" MaxLength="50" class="filter"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="txtToDate" runat="server" TargetControlID="txtTo" Enabled="True" />
                            </td>
                            <td>Status:</td>
                            <td>
                                <asp:DropDownList ID="ddlFltrStatus" runat="server" AppendDataBoundItems="true" class="filter">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/search-icon-blue.gif" OnClick="btnsearch_Click" ToolTip="Search Code" Visible="true" class="filterclick" />
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td width="1%"></td>
            <td>
                <asp:GridView ID="grdVoucher" DataKeyNames="vrid" runat="server" OnSelectedIndexChanged="grdVoucher_SelectedIndexChanging"
                    AutoGenerateColumns="False" OnRowDataBound="grdVoucher_RowDataBound" AllowPaging="true"
                    Width="100%" PageSize="20" OnPageIndexChanging="grdVoucher_PageIndexChanging" RowStyle-VerticalAlign="Top"
                    EmptyDataText="No voucher found.">
                    <HeaderStyle CssClass="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                    <Columns>
                        <asp:BoundField DataField="vrid" ItemStyle-CssClass="DisplayNone" ControlStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" />
                        <asp:BoundField DataField="vr_no" HeaderText="Voucher#" ItemStyle-Width="70" />
                        <asp:BoundField DataField="vr_dt" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false" HeaderText="Date">
                            <ItemStyle Wrap="false" Width="80" />
                        </asp:BoundField>
                        <asp:BoundField DataField="vr_nrtn" HeaderText="Narration" />
                        <asp:BoundField DataField="status" HeaderText="Status" ItemStyle-Width="80" />
                        <asp:TemplateField HeaderText="Print" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblstatus" runat="server" Visible="false" Text='<%# Bind("status") %>'>'></asp:Label>
                                <asp:LinkButton ID="btnprint" runat="server" OnClick="lnkPrint_Click" Text="Print"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowSelectButton="true" ButtonType="Link" ItemStyle-Width="70" />
                    </Columns>
                </asp:GridView>
            </td>
            <td width="1%"></td>
        </tr>
    </table>



    <%--<asp:Panel ID="pnlMain" runat="server" Width="99%" Height="600">
        <rsweb:ReportViewer ID="viewer" runat="server" ShowPrintButton="true" Width="100%" Height="580px">
        </rsweb:ReportViewer>
    </asp:Panel>
    <input type="button" id="PrintButton" value="Print" />--%>

    <script>
        $(document).ready(function () {
            //alert('hello');



            //function doPrint() {
            //    alert(2);
            //    var a = document.getElementById("ReportFrame" + "viewer").contentWindow;
            //    a[1].print();
            //}
            //$('#PrintButton').click(function () {
            //    alert(3);
            //    var a = document.getElementById("ReportFrame" + "viewer").contentWindow;
            //    a[1].print();
            //});
        })

    </script>
</asp:Content>


