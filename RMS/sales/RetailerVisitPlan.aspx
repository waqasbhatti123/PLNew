<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="RetailerVisitPlan.aspx.cs" Inherits="RMS.sales.RetailerVisitPlan"
    Culture="auto" UICulture="auto" EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        //$(document).ready(function() {
        var shopId = 0;
        //---------------
        function pageLoad() {

            $("[id*=GridView1]input[type=text][id*=txtRetailer]").autocomplete({
                source: function(request, response) {
                    $.ajax({
                        url: "RetailerVisitPlan.aspx/GetRetailer",
                        contentType: "application/json; charset=utf-8",
                        responseType: "json",
                        data: "{ 'searchText': '" + request.term + "' }",
                        type: "POST",
                        async: false,
                        cache: false,
                        dataType: "json",
                        success: function(data) {
                            try {

                                response($.map(data.d, function(item) {
                                    //alert(item.ar_cd);
                                    return {
                                        value: item.ar_cd + ' - ' + item.ar_dsc,
                                        result: item.ar_cd + ' - ' + item.ar_dsc,
                                        id: item.id
                                    }
                                }))
                            }
                            catch (e) {
                                alert(e);
                            }
                        }
                    });
                },

                select: function(e, ui) {
                    var codeItm = ui.item.result;
                    codeItm = codeItm.split(" - ");
                    var ItmId = ui.item.id;
                    var shopId = ui.item.id;

                    if (codeItm[1] != null && codeItm[1] != "") {
                        $(e.target).closest('tr').find("input[type=text][id*=txtRetailer]").val(codeItm[1]);
                        $(e.target).closest('tr').find("input[type=hidden][id*=hdnRetailerId]").val(ItmId);
                        var date = getLastVisitDate(shopId);
                        $(e.target).closest('tr').find("input[type=text][id*=txtLastVisit]").val(date);

                    }
                    else {
                        $(e.target).closest('tr').find("input[type=text][id*=txtRetailer]").val('');
                        $(e.target).closest('tr').find("input[type=hidden][id*=hdnRetailerId]").val('');
                    }
                    //checkForDuplicate(ItmId);
                    return false;

                },

                minLength: 1
            });

            //---------------
            $("[id*=GridView1]input[type=text][id*=txtRetailer]").blur(function() {
                if ($(this).val().trim() != "") {
                    checkForDuplicate($(this), $(this).val().trim());
                }
            });

        }

        function getLastVisitDate(shopId) {
            var date = '';
            $.ajax({
                url: "RetailerVisitPlan.aspx/GetLastVisitDate",
                contentType: "application/json; charset=utf-8",
                responseType: "json",
                data: "{ 'shopId': '" + shopId + "' }",
                type: "POST",
                async: false,
                cache: false,
                dataType: "json",
                success: function(data) {
                    date= data.d;
                }
            });
            return date;
        }

        function checkForDuplicate(id, textVal) {
            var count = 0;
            $("[id*=GridView1]input[type=text][id*=txtRetailer]").each(function() {
                var itm = $(this).val().trim();
                if (itm == textVal) {
                    count = parseInt(count) + 1;
                }
            });

            if (count > 1) {
                alert("Retailer already exists");
                $(id).closest('tr').find("input[type=hidden][id*=hdnRetailerId]").val('');
                $(id).closest('tr').find("input[type=text][id*=txtRetailer]").val('');
                $(id).closest('tr').find("input[type=text][id*=txtLastVisit]").val('');
                $(id).closest('tr').find("input[type=text][id*=txtRemarks]").val('');
                $(id).focus();
            }
        }

        
       
    </script>

    <style type="text/css">
        .grd
        {
            /* margin:0 auto !important; */
        }
        .cln
        {
            /*position:absolute;*/
            background-color: White;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
    <asp:Panel ID="pnlTop" runat="server" Visible="false">
    </asp:Panel>
    <div style="width: 100%;">
        <%--<asp:UpdatePanel ID="updPnl" runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
        <table style="width: 95%;">
            <tr>
                <td>
                    <asp:Label ID="lblDistributor" runat="server" Text="Distributor:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:Label ID="lblDistributorName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblDocNo" runat="server" Text="Doc No:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDocNo" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblDocDate" runat="server" Text="Doc Date:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDocDate" runat="server"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="calDocDate" runat="server" TargetControlID="txtDocDate"
                        CssClass="cln">
                    </ajaxToolkit:CalendarExtender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDocDate"
                        ErrorMessage="Please select document date" SetFocusOnError="true" ValidationGroup="main"
                        Display="None"></asp:RequiredFieldValidator>
                    <span class="DteLtrl">
                        <asp:Literal ID="Literal2" runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblScheduleDate" runat="server" Text="Schedule Date:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtScheduleDate" runat="server"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="calSchDate" runat="server" TargetControlID="txtScheduleDate"
                        CssClass="cln">
                    </ajaxToolkit:CalendarExtender>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtScheduleDate"
                        ErrorMessage="Please select schedule date" SetFocusOnError="true" ValidationGroup="main"
                        Display="None"></asp:RequiredFieldValidator>
                    <span class="DteLtrl">
                        <asp:Literal ID="Literal1" runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                    </span>
                </td>
                <td>
                    <asp:Label ID="lblSalesPerson" runat="server" Text="Sales Person:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSalesPerson" runat="server" AppendDataBoundItems="true"
                        Width="95%">
                        <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlSalesPerson"
                        ErrorMessage="Please select sales person" SetFocusOnError="true" ValidationGroup="main"
                        Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
    </div>
    <div style="width: 100%;">
        <div style="width: 100%; margin-top: 25px;">
            <asp:UpdatePanel ID="uPnl" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="table">
                        <tr>
                            <td>
                                <asp:GridView ID="GridView1" runat="server" CssClass="t_grd" Width="100%" AutoGenerateColumns="false"
                                    OnRowDataBound="GridView1_RowDataBound" ShowFooter="true">
                                    <HeaderStyle CssClass="t_grd_hdr"></HeaderStyle>
                                    <FooterStyle CssClass="t_grd_footer"></FooterStyle>
                                    <RowStyle CssClass="t_grd_Row"></RowStyle>
                                    <EditRowStyle CssClass="t_grd_Edit_Row" />
                                    <SelectedRowStyle CssClass="t_grd_Selected_Row" />
                                    <AlternatingRowStyle CssClass="t_grd_Alter_Row" />
                                    <PagerStyle CssClass="t_grd_Pager"></PagerStyle>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <EmptyDataRowStyle CssClass="Label_Small_Bold" ForeColor="#C00000" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblSr" Text='<%#Eval("Sr") %>' Width="20px">
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ControlStyle />
                                            <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="20px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Retailer*">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRetailer" runat="server" Text='<%#Eval("Retailer") %>' />
                                                <asp:HiddenField ID="hdnRetailerId" runat="server" Value='<%#Eval("RetailerId") %>' />
                                            </ItemTemplate>
                                            <ControlStyle />
                                            <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Last Visit">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLastVisit" runat="server" Text='<%#Eval("LastVisit") %>' ReadOnly="true" />
                                            </ItemTemplate>
                                            <ControlStyle />
                                            <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("Remarks")%>' MaxLength="150"
                                                    Width="150px" />
                                            </ItemTemplate>
                                            <ControlStyle />
                                            <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="150px" />
                                            <FooterStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="150px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" Checked='<%#Convert.ToBoolean(Eval("Selected")) %>' />
                                            </ItemTemplate>
                                            <ControlStyle />
                                            <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="addRow" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div style="text-align: left; padding-bottom: 15px; width: 100%;">
            <asp:UpdatePanel ID="updButPnl" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:LinkButton runat="server" ID="addRow" Text="Add Rows" OnClick="addRow_Click"
                        CssClass="lnk"></asp:LinkButton>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="width: 100%; padding-top: 25px;">
            <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
        </div>
    </div>
    <div style="width: 100%; clear: both; margin-top: 50px;">
        <table class="filterTable" width="100%">
            <tr>
                <td>
                </td>
                <td>
                    <asp:Label ID="lblFltDocno" runat="server" Text="Document No:"></asp:Label>
                    <asp:TextBox runat="server" ID="txtFltDocNo" Width="100"></asp:TextBox>
                </td>
                <td colspan="2">
                    <asp:Label ID="Label2" runat="server" Text="Sales Person:"></asp:Label>
                    <asp:DropDownList ID="ddlFltSalesPerson" runat="server" AppendDataBoundItems="true"
                        Width="200px">
                        <asp:ListItem Text="All" Value="0" />
                    </asp:DropDownList>
                </td>
                <%--<td>
                    <asp:Label ID="Label1" runat="server" Text="Retailer:"></asp:Label>
                    <asp:DropDownList ID="ddlFltOutlet" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="All" Value="0" />
                    </asp:DropDownList>
                </td>--%>
                <td>
                    <%--<asp:Label ID="Label8" runat="server" Text="Response Type:"></asp:Label>
                    <asp:DropDownList ID="ddlFltRespType" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="All" Value="0" />
                    </asp:DropDownList>--%>
                </td>
                <td>
                    <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                        OnClick="btnSearch_Click" ToolTip="Search visit plan" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="grd" runat="server" DataKeyNames="vr_id, sale_person_id" OnSelectedIndexChanged="grd_SelectedIndexChanged"
            OnPageIndexChanging="grd_PageIndexChanging" OnRowDataBound="grd_RowDataBound"
            AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No visit plan data found"
            Width="100%">
            <Columns>
                <asp:BoundField DataField="doc_no" HeaderText="Doc No" />
                <asp:BoundField DataField="doc_dt" HeaderText="Doc Date" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="SalesPerson" HeaderText="Sales Person" />
                <asp:BoundField DataField="sch_dt" HeaderText="Schedule Date" DataFormatString="{0:dd-MMM-yyyy}" />
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
</asp:Content>
