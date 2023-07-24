<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="DCNMgt.aspx.cs" Inherits="RMS.sales.DCNMgt" Culture="auto" UICulture="auto"
    EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">




    function pageLoad() {
        if ($('#<%= ddlDCNType.ClientID %>').val() == 'SaleOrder') {
            $("[id*=GridView1]input[type=text][id*=txtQty]").each(function () {
                ValidatorEnable($(this).closest('tr').find('[id*=cvRQ]')[0], true);
            });

            $('#<%= ddlFltParty.ClientID %>').prop('disabled', false);
            $('#<%= txtSrchDoc.ClientID %>').prop('disabled', false);
            $('#<%= btnSrchDoc.ClientID %>').prop('disabled', false);
        }
        else {
            $("[id*=GridView1]input[type=text][id*=txtQty]").each(function () {
                ValidatorEnable($(this).closest('tr').find('[id*=cvRQ]')[0], false);
            });

            $('#<%= ddlFltParty.ClientID %>').prop('disabled', true);
            $('#<%= txtSrchDoc.ClientID %>').prop('disabled', true);
            $('#<%= btnSrchDoc.ClientID %>').prop('disabled', true);
        }

        $('#<%= ddlDCNType.ClientID %>').change(function () {
            var val = $(this).val();

            if (val != 'SaleOrder') {
                $('#<%= ddlFltParty.ClientID %>').prop('disabled', true);
                $('#<%= txtSrchDoc.ClientID %>').prop('disabled', true);
                $('#<%= btnSrchDoc.ClientID %>').prop('disabled', true);

                $("[id*=GridView1]input[type=text][id*=txtQty]").each(function () {
                    ValidatorEnable($(this).closest('tr').find('[id*=cvRQ]')[0], false);
                });
            }
            else {
                $('#<%= ddlFltParty.ClientID %>').prop('disabled', false);
                $('#<%= txtSrchDoc.ClientID %>').prop('disabled', false);
                $('#<%= btnSrchDoc.ClientID %>').prop('disabled', false);

                $("[id*=GridView1]input[type=text][id*=txtQty]").each(function () {
                    ValidatorEnable($(this).closest('tr').find('[id*=cvRQ]')[0], true);
                });
            }

        });



        var itemid;
        $("[id*=GridView1]input[type=text][id*=txtBatch]").autocomplete({
            search: function(event, ui) {
                itemid = $(event.target).closest('tr').find("[id*=ddlItem]").val();
                //alert(itemid);
            },
            source: function(request, response) {
                $.ajax({
                    url: "DCNMgt.aspx/GetBatchDetail",
                    data: "{ 'batchid': '" + request.term + '-' + itemid + "' }",
                    dataType: "json",
                    type: "POST",
                    async: false,
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function(data) { return data; },
                    success: function(data) {
                        response($.map(data.d, function(item) {
                            return {
                                value: item.batch_id + ' - ' + item.batch_qty,
                                result: item.batch_id + ' - ' + item.batch_qty
                            }
                        }))
                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            },

            select: function(e, ui) {
                var codeItm = ui.item.result;
                codeItm = codeItm.split(" - ");
                //                alert(codeItm[0]);
                //                alert(codeItm[1]);
                $(e.target).closest('tr').find("input[type=text][id*=txtBatch]").val(codeItm[0]);
                $(e.target).closest('tr').find("input[type=text][id*=txt1QtyBatch]").val(codeItm[1]);
                $(e.target).closest('tr').find("input[type=text][id*=txtPacks]").val('');
                $(e.target).closest('tr').find("input[type=text][id*=txtUnitSize]").val('');
                $(e.target).closest('tr').find("input[type=text][id*=txtQty]").val('');
                ValidatorEnable($(this).closest('tr').find('[id*=reqBatch]')[0], false);

                var totalQuantity = calculatequantity();
                $("span[id*=lblTotalQty]").text(totalQuantity);
                $('#<%= hdnTotalQty.ClientID %>').val(totalQuantity);

                // $(e.target).closest('tr').find("input[type=text][id*=txtPacks]").focus();
                return false;
            },

            minLength: 1
        });
    





        $("[id*=GridView1][id*=ddlItem]").each(function() {
            if ($(this).val() != '0') {
                ValidatorEnable($(this).closest('tr').find('[id*=reqPc]')[0], true);
                ValidatorEnable($(this).closest('tr').find('[id*=reqUs]')[0], true);
                var gRow = $(this);
                var itemid = $(this).val();

                $.ajax({
                    url: "DCNMgt.aspx/GetItemDetail",
                    data: JSON.stringify({ itemid: itemid }),
                    type: 'POST',
                    contentType: 'application/json;',
                    dataType: 'json',
                    success: function(heads) {
                        var heads = heads.d;
                        if (heads.length > 0) {
                            if (heads[0].Batch == false) {
                                ValidatorEnable(gRow.closest('tr').find('[id*=reqBatch]')[0], false);
                            }
                            else {
                                ValidatorEnable(gRow.closest('tr').find('[id*=reqBatch]')[0], true);
                            }
                        }
                        else {
                            ValidatorEnable(gRow.closest('tr').find('[id*=reqBatch]')[0], false);
                        }
                    }
                });
            }
            else {
                ValidatorEnable($(this).closest('tr').find('[id*=reqPc]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqUs]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqBatch]')[0], false);
            }
        });

        

        $("[id*=GridView1]input[type=text][id*=txtQty]").each(function() {
            $(this).attr("readonly", true);
        });

        $("[id*=GridView1]input[type=text][id*=txt1QtyBatch]").each(function() {
            $(this).attr("readonly", true);
        });

        $("[id*=GridView1]input[type=text][id*=txtUomItem]").each(function() {
            $(this).attr("readonly", true);
        });

        $(".classDdlItem").keyup(function(e) {
            FillItemDetailFields($(this));

        });
        $(".classDdlItem").change(function(e) {
            FillItemDetailFields($(this));

        });


        $("[id*=GridView1]input[type=text][id*=txtPacks]").keyup(function(event) {
            var packs = $(this).val();
            packs = parseFloat($(this).val());
            if (isNaN(packs)) packs = 0;
            var unitsize = $(this).closest('tr').find("input[type=text][id*=UnitSize]").val();
            unitsize = parseFloat(unitsize);
            if (isNaN(unitsize)) unitsize = 0;

            if (packs * unitsize != 0)
                $(this).closest('tr').find("input[type=text][id*=txtQty]").val(packs * unitsize);
            else
                $(this).closest('tr').find("input[type=text][id*=txtQty]").val('');

            var totalQuantity = calculatequantity();
            $("span[id*=lblTotalQty]").text(totalQuantity);
            $('#<%= hdnTotalQty.ClientID %>').val(totalQuantity);
        });

        $("[id*=GridView1]input[type=text][id*=txtPacks]").blur(function(event) {
            var packs = $(this).val();
            packs = parseFloat($(this).val());
            if (isNaN(packs)) packs = 0;
            var unitsize = $(this).closest('tr').find("input[type=text][id*=UnitSize]").val();
            unitsize = parseFloat(unitsize);
            if (isNaN(unitsize)) unitsize = 0;
            
            if (packs * unitsize != 0)
                $(this).closest('tr').find("input[type=text][id*=txtQty]").val(packs * unitsize);
            else
                $(this).closest('tr').find("input[type=text][id*=txtQty]").val('');

            var totalQuantity = calculatequantity();
            $("span[id*=lblTotalQty]").text(totalQuantity);
            $('#<%= hdnTotalQty.ClientID %>').val(totalQuantity);
        });

        $("[id*=GridView1]input[type=text][id*=txtUnitSize]").keyup(function(event) {
            var unitsize = $(this).val();
            unitsize = parseFloat(unitsize);
            if (isNaN(unitsize)) unitsize = 0;
            var packs = $(this).closest('tr').find("input[type=text][id*=txtPacks]").val();
            packs = parseFloat(packs);
            if (isNaN(packs)) packs = 0;

            if (packs * unitsize != 0)
                $(this).closest('tr').find("input[type=text][id*=txtQty]").val(packs * unitsize);
            else
                $(this).closest('tr').find("input[type=text][id*=txtQty]").val('');

            var totalQuantity = calculatequantity();
            $("span[id*=lblTotalQty]").text(totalQuantity);
            $('#<%= hdnTotalQty.ClientID %>').val(totalQuantity);
        });

        $("[id*=GridView1]input[type=text][id*=txtUnitSize]").blur(function(event) {
            var unitsize = $(this).val();
            unitsize = parseFloat(unitsize);
            if (isNaN(unitsize)) unitsize = 0;
            var packs = $(this).closest('tr').find("input[type=text][id*=txtPacks]").val();
            packs = parseFloat(packs);
            if (isNaN(packs)) packs = 0;

            if (packs * unitsize != 0)
                $(this).closest('tr').find("input[type=text][id*=txtQty]").val(packs * unitsize);
            else
                $(this).closest('tr').find("input[type=text][id*=txtQty]").val('');

            var totalQuantity = calculatequantity();
            $("span[id*=lblTotalQty]").text(totalQuantity);
            $('#<%= hdnTotalQty.ClientID %>').val(totalQuantity);
        });
        
        
        var totalQuantity = calculatequantity();
        $("span[id*=lblTotalQty]").text(totalQuantity);
        $('#<%= hdnTotalQty.ClientID %>').val(totalQuantity);
    }

    function calculatequantity() {
        var totalQty = 0;
        $("[id*=GridView1]input[type=text][id*=txtQty]").each(function() {
            temp = parseFloat($(this).val());
            if (isNaN(temp)) temp = 0;
            totalQty = totalQty + temp;
        });
        return totalQty;
    }

    function FillItemDetailFields(gRow) {
        var itemid = gRow.find('option:selected').val();
        $.ajax({
            url: "DCNMgt.aspx/GetItemDetail",
            data: JSON.stringify({ itemid: itemid }),
            type: 'POST',
            contentType: 'application/json;',
            dataType: 'json',
            success: function(heads) {
                var heads = heads.d;
                if (heads.length > 0) {
                    gRow.closest('tr').find("input[type=text][id*=txtUomItem]").val(heads[0].uom_dsc);
                    ValidatorEnable(gRow.closest('tr').find('[id*=reqPc]')[0], true);
                    ValidatorEnable(gRow.closest('tr').find('[id*=reqUs]')[0], true);
                    if (heads[0].Batch == false) {
                        ValidatorEnable(gRow.closest('tr').find('[id*=reqBatch]')[0], false);
                        gRow.closest('tr').find("input[type=text][id*=txtBatch]").attr('readOnly', true);
                        gRow.closest('tr').find("input[type=text][id*=txtBatch]").attr('tabIndex', -1);
                    }
                    else {
                        ValidatorEnable(gRow.closest('tr').find('[id*=reqBatch]')[0], true);
                        gRow.closest('tr').find("input[type=text][id*=txtBatch]").attr('readOnly', false);
                        gRow.closest('tr').find("input[type=text][id*=txtBatch]").attr('tabIndex', 0);
                    }
                }
                else {
                    gRow.closest('tr').find("input[type=text][id*=txtUomItem]").val('');
                    ValidatorEnable(gRow.closest('tr').find('[id*=reqPc]')[0], false);
                    ValidatorEnable(gRow.closest('tr').find('[id*=reqUs]')[0], false);
                    ValidatorEnable(gRow.closest('tr').find('[id*=reqBatch]')[0], false);
                }
                gRow.closest('tr').find("input[type=text][id*=txtPacks]").val('');
                gRow.closest('tr').find("input[type=text][id*=txtUnitSize]").val('');
                gRow.closest('tr').find("input[type=text][id*=txtQty]").val('');
                gRow.closest('tr').find("input[type=text][id*=txtBatch]").val('');
                if ( heads[0] != null && heads[0].stkqty && !heads[0].Batch)
                    gRow.closest('tr').find("input[type=text][id*=txt1QtyBatch]").val(heads[0].stkqty);
                else
                    gRow.closest('tr').find("input[type=text][id*=txt1QtyBatch]").val('0');

                var totalQuantity = calculatequantity();
                $("span[id*=lblTotalQty]").text(totalQuantity);
                $('#<%= hdnTotalQty.ClientID %>').val(totalQuantity);
                gRow.focus();
            }
        });
    }
</script>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="1%"></td>
            <td>
                <table  cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="100px">
                            DCN Type: 
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDCNType" runat="server">
                                <asp:ListItem Value="SaleOrder" Selected="True">From Sale Order</asp:ListItem>
                                <asp:ListItem Value="Direct">Direct</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td width="100px">
                            Search Sale Order: 
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlFltParty" runat="server" AppendDataBoundItems="true">
                                </asp:DropDownList>
                            <asp:TextBox ID="txtSrchDoc" runat="server" Width="100px" ></asp:TextBox>
                            <asp:ImageButton ID="btnSrchDoc" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                OnClick="btnSrchDoc_Click" ToolTip="Search sale order"/>
                            <br />
                            <asp:GridView ID="grdSrchDoc" runat="server" DataKeyNames="OrderID" Width="100%" AutoGenerateColumns="false" CssClass="t_grd" 
                                          OnRowDataBound="grdSrchDoc_RowDataBound"
                                          OnSelectedIndexChanged="grdSrchDoc_SelectedIndexChanged">
                                <HeaderStyle CssClass="t_grd_hdr"></HeaderStyle>
                                <FooterStyle CssClass="t_grd_footer"></FooterStyle>
                                <RowStyle CssClass="t_grd_Row"></RowStyle>
                                <EditRowStyle CssClass="t_grd_Edit_Row" />
                                <SelectedRowStyle CssClass="t_grd_Selected_Row"  />
                                <AlternatingRowStyle CssClass="t_grd_Alter_Row"/>
                                <PagerStyle CssClass="t_grd_Pager"></PagerStyle>
                                <PagerSettings Mode="NumericFirstLast" />
                                <EmptyDataRowStyle CssClass="Label_Small_Bold" ForeColor="#C00000" HorizontalAlign="Center" />
                              
                                <Columns>
                                  <asp:BoundField HeaderText="Sale Order" DataField="OrderNo"/>
                                  <asp:BoundField HeaderText="Order Date" DataField="OrderDate"/>
                                  <asp:BoundField HeaderText="Party" DataField="party"/>
                                  <asp:BoundField HeaderText="Sales Person" DataField="salesperson"/>
                                  <asp:CommandField ShowSelectButton="true" SelectText="Select" ControlStyle-CssClass="lnk"/>
                                </Columns>
                            </asp:GridView>
                            <br />
                            <asp:GridView ID="grdSaleOrderItems" runat="server" DataKeyNames="OrderID, Oseq" Width="100%" AutoGenerateColumns="false" CssClass="t_grd" >
                                <HeaderStyle CssClass="t_grd_hdr"></HeaderStyle>
                                <FooterStyle CssClass="t_grd_footer"></FooterStyle>
                                <RowStyle CssClass="t_grd_Row"></RowStyle>
                                <EditRowStyle CssClass="t_grd_Edit_Row" />
                                <SelectedRowStyle CssClass="t_grd_Selected_Row"  />
                                <AlternatingRowStyle CssClass="t_grd_Alter_Row"/>
                                <PagerStyle CssClass="t_grd_Pager"></PagerStyle>
                                <PagerSettings Mode="NumericFirstLast" />
                                <EmptyDataRowStyle CssClass="Label_Small_Bold" ForeColor="#C00000" HorizontalAlign="Center" />
                              
                                <Columns>
                                  <asp:BoundField HeaderText="Sale Order" DataField="OrderNo" />
                                  <asp:BoundField HeaderText="Item" DataField="itm_dsc"/>
                                  <asp:BoundField HeaderText="Qty" DataField="OrderQty"/>
                                  <asp:BoundField HeaderText="Shipped Qty" DataField="ShippedQty"/>
                                  <asp:BoundField HeaderText="Pending Qty" DataField="PedingQty"/>
                                  <asp:BoundField HeaderText="Balance" DataField="Balance"/>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
            <td width="1%"></td>
        </tr>
            <td width="1%">
            </td>
            <td>
                <table class="tblMain"  cellspacing="0" cellpadding="0">
                    <asp:Panel runat="server" ID="pnlMain">
                        <tr>
                            <td class="tdlabel">
                                <asp:Label ID="lblloc" runat="server" Text="From Location:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLoc" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Text="Select Store Location" Selected="True" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlLoc"
                                    ErrorMessage="Please select store location" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator> 
                            </td>
                            <td class="tdlabel">
                                <asp:Label ID="Label9" runat="server" Text="Status:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="120px">
                                    <asp:ListItem Text="Select Status" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Approved" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="Pending" Value="P" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Cancelled" Value="C"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlStatus"
                                    ErrorMessage="Please select status" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator> 
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="tdlabel">
                                <asp:Label ID="Label1" runat="server" Text="Party:"></asp:Label>
                            </td>
                           <td>
                                <asp:DropDownList ID="ddlParty" runat="server" AppendDataBoundItems="true">
                                    <asp:ListItem Value="0">Select Party</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlParty"
                                    ErrorMessage="Please select Party" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator> 
                           </td>
                            <td class="tdlabel">
                                <asp:Label ID="Label122" runat="server" Text="GP Reference:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtGpRef" runat="server" Width="80px" MaxLength="20"></asp:TextBox>
                            </td>
                            
                        </tr>
                        <tr>
                            <td class="tdlabel">
                                <asp:Label ID="Label26" runat="server" Text="Document No:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocNo" runat="server" Width="80px" ReadOnly="true" TabIndex="-1">
                                </asp:TextBox>
                            </td>
                            <td class="tdlabel">
                                <asp:Label ID="Label25" runat="server" Text="Doc Ref #:" Visible="false"></asp:Label>
                                <asp:Label ID="Label4" runat="server" Text="Document Date:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocDate" runat="server" Width="80px">
                                </asp:TextBox>
                                
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDocDate"
                                    ErrorMessage="Please select date" SetFocusOnError="true" ValidationGroup="main" Display="None" ></asp:RequiredFieldValidator>
                                <span class="tdlitrel">
                                    <asp:Literal ID="Literal2"  runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                                </span>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDocDate" PopupPosition="BottomRight" >
                                  </ajaxToolkit:CalendarExtender>
                                
                            </td>
                        </tr>
                        <tr>
                            
                            <td class="tdlabel">
                                <asp:Label ID="Label2" runat="server" Text="Total Quantity:" ></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="hdnTotalQty" runat="server" CssClass="DisplayNone"></asp:TextBox>
                                <asp:TextBox ID="txtTotalQty" runat="server" Width="80px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqTQ" runat="server" Display="None" ValidationGroup="main" 
                                                        ControlToValidate="txtTotalQty" ErrorMessage="Total quantity required"></asp:RequiredFieldValidator>
                                <asp:RangeValidator runat="server" ID="rvTQ" CssClass="validateGridView" ValidationGroup="main" ErrorMessage="Invalid/Out of range Total Quantity"
                                            ControlToValidate="txtTotalQty" MinimumValue="1.00" MaximumValue="99999999999.99" Type="Double" Display="None" SetFocusOnError="true"></asp:RangeValidator>
                                <asp:CompareValidator ID="cvTQ" runat="server" ControlToValidate="txtTotalQty" ControlToCompare="hdnTotalQty" Display="None" ValidationGroup="main" ErrorMessage="Total quantity should be matched with quantity in grid" 
                                            Operator="Equal" SetFocusOnError="true"></asp:CompareValidator>
                                                        
                            </td>
                            <td class="tdlabel">
                                <asp:Label ID="Label10" runat="server" Text="Remarks:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"
                                 onkeyup="LimitText(this,100);" onblur="LimitText(this,100);" ></asp:TextBox>
                            </td>
                        </tr>
                        <%--<tr><td colspan="4">&nbsp;</td></tr>--%>
                        <tr>
                        
                          <td colspan="4">
                        
                                <asp:UpdatePanel ID="uPnl" runat="server" UpdateMode="Conditional">
                                      <ContentTemplate>
                                      
                                      <table class="table">
                                        <tr>    
                                      <td>
                                        <asp:GridView ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="false" CssClass="t_grd" OnRowDataBound="GridView1_RowDataBound" ShowFooter="true">
                                            <HeaderStyle CssClass="t_grd_hdr"></HeaderStyle>
                                            <FooterStyle CssClass="t_grd_footer"></FooterStyle>
                                            <RowStyle CssClass="t_grd_Row"></RowStyle>
                                            <EditRowStyle CssClass="t_grd_Edit_Row" />
                                            <SelectedRowStyle CssClass="t_grd_Selected_Row"  />
                                            <AlternatingRowStyle CssClass="t_grd_Alter_Row"/>
                                            <PagerStyle CssClass="t_grd_Pager"></PagerStyle>
                                            <PagerSettings Mode="NumericFirstLast" />
                                            <EmptyDataRowStyle CssClass="Label_Small_Bold" ForeColor="#C00000" HorizontalAlign="Center" />
                                          
                                            <Columns>
                                              <asp:TemplateField HeaderText="Sr. ">
                                                  <ItemTemplate>
                                                      <asp:Label runat="server" ID="lblSr" Text='<%#Eval("Sr") %>' Width="20px">
                                                      </asp:Label>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="20px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Item">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlItem" runat="server" AppendDataBoundItems="true" CssClass="classDdlItem" Width="200px">
                                                        <asp:ListItem Selected="True" Text="Select Item" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ControlStyle />
                                                <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="160px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Batch">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtBatch" runat="server" Text='<%#Eval("Batch") %>' MaxLength="20" Width="60px"/>
                                                    <asp:RequiredFieldValidator ID="reqBatch" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtBatch" ErrorMessage="Batch required" Enabled="false"></asp:RequiredFieldValidator>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="QOH">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txt1QtyBatch" runat="server" Text='<%#Eval("BatchQty") %>' Width="60px"  TabIndex="-1"  style="text-align:right"/>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Packs">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtPacks" runat="server" Text='<%#Eval("Packs") %>' Width="60px" style="text-align:right"/>
                                                    <asp:RequiredFieldValidator ID="reqPc" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtPacks" ErrorMessage="Packs required" Enabled="false"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator runat="server" ID="rvPc" CssClass="validateGridView" ValidationGroup="main" ErrorMessage="Invalid/Out of range Packs"
                                                        ControlToValidate="txtPacks" MinimumValue="1" MaximumValue="999999999" Type="Integer" Display="Dynamic" SetFocusOnError="true"></asp:RangeValidator>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Unit Size">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtUnitSize" runat="server" Text='<%#Eval("UnitSize") %>' Width="60px" style="text-align:right"/>
                                                    <asp:RequiredFieldValidator ID="reqUs" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtUnitSize" ErrorMessage="Unit size required" Enabled="false"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator runat="server" ID="rvUS" CssClass="validateGridView" ValidationGroup="main" ErrorMessage="Invalid/Out of range Packs"
                                                        ControlToValidate="txtUnitSize" MinimumValue="1.00" MaximumValue="999999999.00" Type="Double" Display="Dynamic" SetFocusOnError="true"></asp:RangeValidator>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="UOM">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtUomItem" runat="server" Text='<%#Eval("UOMItem") %>' TabIndex="-1" Width="50px"/>
                                                  </ItemTemplate>
                                                  <FooterTemplate>
                                                    <asp:Label ID="lblTotal" runat="server" Text="Total:"></asp:Label>
                                                  </FooterTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                                                  <FooterStyle VerticalAlign="Middle" HorizontalAlign="Left" />
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Quantity">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtQty" runat="server" Text='<%#Eval("Qty") %>' Width="60px" TabIndex="-1"  style="text-align:right"/>
                                                    <asp:RangeValidator runat="server" ID="rvQt" CssClass="validateGridView" ValidationGroup="main" ErrorMessage="Invalid/Out of range Quantity"
                                                        ControlToValidate="txtQty" MinimumValue="1.00" MaximumValue="999999999.00" Type="Double" Display="Dynamic" SetFocusOnError="true"></asp:RangeValidator>
                                                    <asp:CompareValidator runat="server" ID="cvRQ" CssClass="validateGridView" ValidationGroup="main" ErrorMessage="Quantity must be less than equal to Quantity On Hand"
                                                        ControlToValidate="txtQty" ControlToCompare="txt1QtyBatch" Type="Double" Operator="LessThanEqual" Display="Dynamic" SetFocusOnError="true"></asp:CompareValidator>
                                                  </ItemTemplate>
                                                  <FooterTemplate>
                                                    <asp:Label ID="lblTotalQty" runat="server"></asp:Label>
                                                  </FooterTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                                                  <FooterStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Remarks">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("Rem")%>' Width="60px" MaxLength="99"/>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                                              </asp:TemplateField>
                                          </Columns>
                                        </asp:GridView>
                                        </td>
                                        </tr>
                                        </table>
                                </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger  ControlID="addRow"/> 
                            </Triggers>
                        </asp:UpdatePanel>
                        </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <div style="float:right">
                                        <asp:UpdatePanel ID="updButPnl" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:LinkButton runat="server" ID="addRow" Text="Add Rows" OnClick="addRow_Click" CssClass="lnk"></asp:LinkButton>               
                                            </ContentTemplate>  
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                                    
                        </asp:Panel>
                        
                        <tr>
                            <td align="center" valign="top" colspan="4">
                                <br />
                                <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
                            </td>
                        </tr>
                    
                </table>
            </td>
            <td width="1%">
            </td>
        </tr>
        <tr>
            <td width="1%">
            </td>
            <td valign="top">
                <br />
                <table class="filterTable" width="70%">
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblFltDocno" runat="server" Text="Doc No:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltDocNo" Width="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Status:"></asp:Label><br />
                            <asp:DropDownList  ID="ddlFltStatus" runat="server">
                                <asp:ListItem Text="All" Value="0" />
                                <asp:ListItem Text="Approved" Value="A" />
                                <asp:ListItem Text="Pending" Value="P" />
                                <asp:ListItem Text="Cancelled" Value="C" />    
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                OnClick="btnSearch_Click" Enabled="false" ToolTip="Search purchase requests" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="grd" runat="server" DataKeyNames="vr_id, LocId"
                OnSelectedIndexChanged="grd_SelectedIndexChanged"
                 OnPageIndexChanging="grd_PageIndexChanging" OnRowDataBound="grd_RowDataBound"
                    AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No delivery challan note found" Width="750px">
                    <Columns>
                        <asp:BoundField DataField="vr_no" HeaderText="Doc No" />
                        <asp:BoundField DataField="LocName" HeaderText="Location" />
                        <asp:BoundField DataField="vr_dt" HeaderText="Date" />      
                        <asp:BoundField DataField="vr_nrtn" HeaderText="Remarks" />                  
                        <asp:BoundField DataField="vr_apr" HeaderText="Status" />                       
                        <asp:TemplateField HeaderText="Delivery Challan">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPrint" runat="server" Text="Print" OnClick="lnkPrint_Click" CssClass="lnk"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Invoice">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPrintInvoice" runat="server" Text="Print" OnClick="lnkPrintInvoice_Click" CssClass="lnk"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                    </Columns>
                    <HeaderStyle CssClass="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                </asp:GridView>
            </td>
            <td width="1%">
            </td>
        </tr>
    </table>
    
    <br />
    <br />
</asp:Content>
