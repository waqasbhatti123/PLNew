<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="PORevMgt.aspx.cs" Inherits="RMS.Inv.PORevMgt" Culture="auto" UICulture="auto"
    EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" language="javascript">
    //DISABLING DOUBLE CLICK
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    function BeginRequestHandler(sender, args) { var oControl = args.get_postBackElement(); oControl.disabled = true; }

</script>
<script type="text/javascript">

    

    function pageLoad() 
    {

        $("[id*=GridView1]input[type=text][id*=txtItem]").each(function() {
            var itm = $(this).val();
            if (itm == "") {
                $(this).closest('tr').find("input[type=text][id*=txtItem]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtItemDesc]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtUomItem]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtPoQuantity]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtQuantity]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtRate]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtAmnt]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtDueDate]").val('');
                ValidatorEnable($(this).closest('tr').find('[id*=reqPoQty]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqQty]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqRt]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqDD]')[0], false);
            }
            else {
                ValidatorEnable($(this).closest('tr').find('[id*=reqPoQty]')[0], true);
                ValidatorEnable($(this).closest('tr').find('[id*=reqQty]')[0], true);
                ValidatorEnable($(this).closest('tr').find('[id*=reqRt]')[0], true);
                ValidatorEnable($(this).closest('tr').find('[id*=reqDD]')[0], true);
            }
        });
        $("[id*=GridView1]input[type=text][id*=txtItem]").change(function(e) {
            var itm = $(this).val();
            if (itm == "") {
                $(this).closest('tr').find("input[type=text][id*=txtItem]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtItemDesc]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtUomItem]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtPoQuantity]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtQuantity]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtRate]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtAmnt]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtDueDate]").val('');
                ValidatorEnable($(this).closest('tr').find('[id*=reqPoQty]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqQty]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqRt]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqDD]')[0], false);
            }
        });
        $("[id*=GridView1]input[type=text][id*=txtItem]").autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: "PurchReqMgt.aspx/GetItemDetail",
                    data: "{ 'itemid': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    async: false,
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function(data) { return data; },
                    success: function(data) {
                        response($.map(data.d, function(item) {
                            return {
                                value: item.itm_cd + ' <> ' + item.itm_dsc + ' (' + item.uom_dsc + ')',
                                result: item.itm_cd + ' <> ' + item.itm_dsc + ' <> ' + item.uom_dsc + ' <> ' + item.stk_qty,
                                id: item.itm_cd
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
                codeItm = codeItm.split(" <> ");
                if (codeItm[1] != null && codeItm[1] != "") {
                    $(e.target).closest('tr').find("input[type=text][id*=txtItem]").val(codeItm[0]);
                    $(e.target).closest('tr').find("input[type=text][id*=txtItemDesc]").val(codeItm[1]);
                    $(e.target).closest('tr').find("input[type=text][id*=txtUomItem]").val(codeItm[2]);
                    $(e.target).closest('tr').find("input[type=text][id*=txtPoQuantity]").val('0');
                    ValidatorEnable($(this).closest('tr').find('[id*=reqPoQty]')[0], true);
                    ValidatorEnable($(this).closest('tr').find('[id*=reqQty]')[0], true);
                    ValidatorEnable($(this).closest('tr').find('[id*=reqRt]')[0], true);
                    ValidatorEnable($(this).closest('tr').find('[id*=reqDD]')[0], true);
                }
                else {
                    $(e.target).closest('tr').find("input[type=text][id*=txtItem]").val('');
                    $(e.target).closest('tr').find("input[type=text][id*=txtItemDesc]").val('');
                    $(e.target).closest('tr').find("input[type=text][id*=txtUomItem]").val('');
                    $(e.target).closest('tr').find("input[type=text][id*=txtPoQuantity]").val('0');
                    ValidatorEnable($(this).closest('tr').find('[id*=reqPoQty]')[0], false);
                    ValidatorEnable($(this).closest('tr').find('[id*=reqQty]')[0], false);
                    ValidatorEnable($(this).closest('tr').find('[id*=reqRt]')[0], false);
                    ValidatorEnable($(this).closest('tr').find('[id*=reqDD]')[0], false);
                }
                return false;
            },

            minLength: 1
        });


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//        $(".classDdlItem").change(function(e) {
//            FillItemDetailFields($(this));

//        });
//        
//        function FillItemDetailFields(gRow) {
//            var itemid = gRow.find('option:selected').val();
//            $.ajax({
//                url: "PORevMgt.aspx/GetItemDetail",
//                data: JSON.stringify({ itemid: itemid }),
//                type: 'POST',
//                contentType: 'application/json;',
//                dataType: 'json',
//                success: function(heads) {
//                    var heads = heads.d;
//                    if (heads.length > 0) {
//                        gRow.closest('tr').find("input[type=text][id*=txtUomItem]").val(heads[0].uom_dsc);
//                        ValidatorEnable(gRow.closest('tr').find('[id*=reqPoQty]')[0], true);
//                        ValidatorEnable(gRow.closest('tr').find('[id*=reqQty]')[0], true);
//                        ValidatorEnable(gRow.closest('tr').find('[id*=reqRt]')[0], true);
//                        ValidatorEnable(gRow.closest('tr').find('[id*=reqDD]')[0], true);
//                    }
//                    else {
//                        gRow.closest('tr').find("input[type=text][id*=txtUomItem]").val('');
//                        ValidatorEnable(gRow.closest('tr').find('[id*=reqPoQty]')[0], false);
//                        ValidatorEnable(gRow.closest('tr').find('[id*=reqQty]')[0], false);
//                        ValidatorEnable(gRow.closest('tr').find('[id*=reqRt]')[0], false);
//                        ValidatorEnable(gRow.closest('tr').find('[id*=reqDD]')[0], false);
//                    }
//                    gRow.focus();
//                }
//            });
//        }


//        $("[id*=GridView1][id*=ddlItem]").each(function() {
//            if ($(this).val() != '0') {
//                ValidatorEnable($(this).closest('tr').find('[id*=reqPoQty]')[0], true);
//                ValidatorEnable($(this).closest('tr').find('[id*=reqQty]')[0], true);
//                ValidatorEnable($(this).closest('tr').find('[id*=reqRt]')[0], true);
//                ValidatorEnable($(this).closest('tr').find('[id*=reqDD]')[0], true);
//                var gRow = $(this);
//                var itemid = $(this).val();

//                $.ajax({
//                    url: "PurchReqMgt.aspx/GetItemDetail",
//                    data: JSON.stringify({ itemid: itemid }),
//                    type: 'POST',
//                    contentType: 'application/json;',
//                    dataType: 'json',
//                    success: function(heads) {
//                        var heads = heads.d;
//                        if (heads.length > 0) {
//                            gRow.closest('tr').find("input[type=text][id*=txtUomItem]").val(heads[0].uom_dsc);
//                        }
//                        else {
//                            gRow.closest('tr').find("input[type=text][id*=txtUomItem]").val('');
//                        }
//                    }
//                });
//            }
//            else {
//                ValidatorEnable($(this).closest('tr').find('[id*=reqPoQty]')[0], false);
//                ValidatorEnable($(this).closest('tr').find('[id*=reqQty]')[0], false);
//                ValidatorEnable($(this).closest('tr').find('[id*=reqRt]')[0], false);
//                ValidatorEnable($(this).closest('tr').find('[id*=reqDD]')[0], false);
//            }
//        });
//        
  
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        var totalAmount = calculateTotalAmnt();
        $("span[id*=lblTotalAmount]").html(totalAmount);
        
        $("[id*=GridView1]input[type=text][id*=txtRate]").keyup(function(event) {
            var rt = parseFloat($(this).val());
            if (isNaN(rt)) rt = 0;
            var qty = parseFloat($(this).closest("tr").find("input[type=text][id*=txtQuantity]").val());
            if (isNaN(qty)) qty = 0;
            $(this).closest("tr").find("input[type=text][id*=txtAmnt]").val((rt * qty).toFixed(2));
            
            var totalAmount = calculateTotalAmnt();
            $("span[id*=lblTotalAmount]").html(totalAmount);
        });

        $("[id*=GridView1]input[type=text][id*=txtQuantity]").keyup(function(event) {
            var qty = parseFloat($(this).val());
            if (isNaN(qty)) qty = 0;
            var rt = parseFloat($(this).closest("tr").find("input[type=text][id*=txtRate]").val());
            if (isNaN(rt)) rt = 0;
            $(this).closest("tr").find("input[type=text][id*=txtAmnt]").val((rt * qty).toFixed(2));

            var totalAmount = calculateTotalAmnt();
            $("span[id*=lblTotalAmount]").html(totalAmount);
        });
    }

    function checkIfQtyEquals() {
        var v1 = $("[id*=txtTotalQty]").val();
        var f1 = $("span[id*=lblGrossQuantityTotal]").html();
        if (v1 == f1) {
            return true;
        }
        else {
            alert("Total quantity should match quantity in parts.");
            return false;
        }
    }

    function ace_ItemSelected(sender, e) {
    
        var aceValue = $get('<%= aceValue.ClientID %>');
        aceValue.value = e.get_value();
    }

    function calculateTotalAmnt() {
        var totalAmnt = 0;
        $("[id*=GridView1]input[type=text][id*=txtAmnt]").each(function() {
             temp = parseFloat($(this).val());
             if (isNaN(temp)) 
                temp = 0;
            totalAmnt = totalAmnt + temp;
        });
            return totalAmnt.toFixed(2);
    }
   
</script>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="3%">
            </td>
            <td>
                <table cellspacing="2" align="center" border="0" width="100%">
                    <asp:Panel runat="server" ID="pnlMain">
                        <tr>
                        <td class="LblBgSetup">
                                <asp:Label ID="Label3" runat="server" Text="PO No:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPoNo" runat="server" width = "80" MaxLength="10" Enabled="false">
                                </asp:TextBox>
                                &nbsp;
                                <asp:Label ID="Label17" runat="server" Text="Revision:"></asp:Label>
                                &nbsp;
                                <asp:TextBox ID="txtPoRev" runat="server" width = "60" Enabled="false">
                                </asp:TextBox>
                                  <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtPoNo"
                                    ErrorMessage="Please enter PO No" SetFocusOnError="true" ValidationGroup="main" Display="None" ></asp:RequiredFieldValidator> --%>
                            </td>
                           
                            <td class="LblBgSetup">
                                <asp:Label ID="Label9" runat="server" Text="Status:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="RequiredFieldDropDown" Width="110px">
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
                         <td class="LblBgSetup">
                                <asp:Label ID="lblloc" runat="server" Text="PO Type:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPoType" runat="server" AppendDataBoundItems="true" CssClass="RequiredFieldDropDown">
                                <asp:ListItem Text="Select PO Type" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Local" Selected="True" Value="L"></asp:ListItem>
                                <asp:ListItem Text="Foreign" Value="F"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlPoType"
                                    ErrorMessage="Please select PO type" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator> 
                            </td>
                            
                            <td class="LblBgSetup">
                                <asp:Label ID="Label4" runat="server" Text="PO Date:"></asp:Label>
                            </td>
                            <td>
                                
                                <asp:TextBox ID="txtPoDate" runat="server" CssClass="RequiredFieldDate">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPoDate"
                                    ErrorMessage="Please select PO date" SetFocusOnError="true" ValidationGroup="main" Display="None" ></asp:RequiredFieldValidator> 
                                <span class="DteLtrl">
                                    <asp:Literal ID="Literal2"  runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                                </span>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtPoDate" PopupPosition="BottomRight" >
                                  </ajaxToolkit:CalendarExtender>
                                
                            </td>
                        </tr>
                         <tr>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label26" runat="server" Text="Supply Type:"></asp:Label>
                            </td>
                            <td>
                               <asp:DropDownList ID="ddlSupType" runat="server" AppendDataBoundItems="true" CssClass="RequiredFieldDropDown">
                                <asp:ListItem Text="Select supply type" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Goods" Selected="True" Value="G"></asp:ListItem>
                                <asp:ListItem Text="Services" Value="S"></asp:ListItem>
                                <asp:ListItem Text="Asset" Value="A"></asp:ListItem>
                                <asp:ListItem Text="Consumable Expense" Value="C"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlSupType"
                                    ErrorMessage="Please select supply type" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator> 
                            </td>
                             <td class="LblBgSetup">
                                <asp:Label ID="Label11" runat="server" Text="PO Currency:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCurrency" runat="server" AppendDataBoundItems="true" Width="120">
                                <asp:ListItem Text="Select currency" Value="0"  Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label122" runat="server" Text="Vendor:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlVendor" runat="server" CssClass="RequiredFieldDropDownVendor" AppendDataBoundItems="true" Width="220">
                                    <asp:ListItem Text="Select Vendor" Selected="True" Value="0">
                                    </asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlVendor"
                                    ErrorMessage="Please select vendor" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label2" runat="server" Text="Shipment:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlShipment" runat="server" AppendDataBoundItems="true" Width="110px">
                                    <asp:ListItem Text="Select Shipment"  Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Road" Selected="True" Value="t"></asp:ListItem>
                                    <asp:ListItem Text="Air" Value="a"></asp:ListItem>
                                    <asp:ListItem Text="Sea"  Value="s"></asp:ListItem>
                                </asp:DropDownList>
                                
                            </td>
                        </tr>
                        
                        <tr>
                         <td class="LblBgSetup">
                                <asp:Label ID="Label15" runat="server" Text="Vendor Doc Ref:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID ="txtVendorDocRef" runat="server" Width="100px" MaxLength="20"></asp:TextBox>
                            </td>
                            
                            <td class="LblBgSetup">
                                <asp:Label ID="Label16" runat="server" Text="Ref Date:"></asp:Label>
                            </td>
                            <td>
                                
                                <asp:TextBox ID="txtRefDate" runat="server" Width="80px">
                                </asp:TextBox>
                                <span class="DteLtrl">
                                    <asp:Literal ID="Literal1"  runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                                </span>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtRefDate" PopupPosition="BottomRight" >
                                  </ajaxToolkit:CalendarExtender>
                                
                            </td>
                        </tr>
                        
                        <tr>
                         <td class="LblBgSetup">
                                <asp:Label ID="Label6" runat="server" Text="Delivery Location:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDelLoc" runat="server" CssClass="TxtNormal" MaxLength="50">
                                </asp:TextBox>
                            </td>
                             
                            
                            <td class="LblBgSetup">
                                <asp:Label ID="Label18" runat="server" Text="Withholding Tax:"></asp:Label>
                            </td>
                            <td>
                                
                                <asp:DropDownList ID="ddlWHT" runat="server" AppendDataBoundItems="true">
                                    <asp:ListItem Value="0" Selected="True">No WHT</asp:ListItem>
                                </asp:DropDownList>
                                                                
                            </td>
                        </tr>
                        
                        <tr class="DisplayNone">
                           <td class="LblBgSetup">
                                
                            </td>
                            <td>
                                
                            </td>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label7" runat="server" Text="Delivery Period:"></asp:Label>
                            </td>
                         
                            <td>
                                <asp:TextBox ID="txtDelPeriod" runat="server" MaxLength="3" Width="40" />
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtDelPeriod"
                                    ErrorMessage="Please enter delivery period" SetFocusOnError="true" ValidationGroup="main" Display="None" ></asp:RequiredFieldValidator> --%>
                                <asp:RangeValidator runat="server" ID="RangeValidator3" ValidationGroup="main" ErrorMessage="Invalid/Out of range delivery period, cannot be greater than 255" SetFocusOnError="true"
                                    ControlToValidate="txtDelPeriod" Display="None" MinimumValue="1" MaximumValue="255" Type="Integer" ></asp:RangeValidator>
                                
                            </td>
                        </tr>
                        <tr>
                           <td class="LblBgSetup">
                                <asp:Label ID="Label1" runat="server" Text="Qty Variation %:"></asp:Label>
                            </td>
                            <td>
                               <asp:TextBox ID="txtQtyVar" runat="server" MaxLength="2"  Width="40"/>
                               <asp:RangeValidator runat="server" ID="RangeValidator1" ValidationGroup="main" ErrorMessage="Invalid/Out of range Qty Variation %" SetFocusOnError="true"
                                    ControlToValidate="txtQtyVar" Display="None" MinimumValue="1" MaximumValue="99" Type="Integer" ></asp:RangeValidator>
                            </td>
                             <td class="LblBgSetup">
                                <asp:Label ID="Label5" runat="server" Text="Payment Days:" Visible="false"></asp:Label>
                                <asp:Label ID="Label19" runat="server" Text="Overall Discount:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPayDays" runat="server" CssClass="TxtNormal" MaxLength="3"  Width="40" Visible="false"/>
                                <asp:RangeValidator runat="server" ID="RangeValidator2" ValidationGroup="main" ErrorMessage="Invalid/Out of range payment days, cannot be greater than 255" SetFocusOnError="true"
                                    ControlToValidate="txtPayDays" Display="None" MinimumValue="1" MaximumValue="255" Type="Integer" ></asp:RangeValidator>

                                <asp:TextBox ID="txtOverAllDisc" runat="server" CssClass="TxtNormal" MaxLength="12" style="text-align:right;" />
                                <asp:RangeValidator runat="server" ID="RangeValidator4" ValidationGroup="main" ErrorMessage="Invalid/Out of range overall discount, it must be an integer value" SetFocusOnError="true"
                                    ControlToValidate="txtOverAllDisc" Display="None" MinimumValue="0.00" MaximumValue="999999999.00" Type="Double" ></asp:RangeValidator>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="Label10" runat="server" Text="Payment Terms:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtPayTerms" runat="server" TextMode="MultiLine" width="500" height="50" 
                                 onkeyup="LimitText(this,200);" onblur="LimitText(this,200);"></asp:TextBox>
                            </td>
                            
                        </tr>
                        <tr valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="Label12" runat="server" Text="Terms:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtTerms" runat="server" TextMode="MultiLine" width="500" height="50" 
                                 onkeyup="LimitText(this,100);" onblur="LimitText(this,100);"></asp:TextBox>
                            </td>
                            
                        </tr>
                        <tr valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="Label13" runat="server" Text="Instructions:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtInst" runat="server" TextMode="MultiLine" width="500" height="50" 
                                 onkeyup="LimitText(this,200);" onblur="LimitText(this,200);"></asp:TextBox>
                            </td>
                            
                        </tr>
                        <tr><td colspan="4">&nbsp;</td></tr>
                        <tr>
                        
                          <td colspan="4">
                            
                            
                            
                            
                                <div style="width:72%; float:left; vertical-align:bottom;" class="LblBgSetup">
                                        <asp:Label ID="Label14" runat="server" Text="Search PO Request:"></asp:Label>
                                    <br />
                                        <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="CompletionListCssClass"  TargetControlID="txtReqNoSrch" OnClientItemSelected="ace_ItemSelected" EnableCaching="true" MinimumPrefixLength="1"  ServiceMethod="AutoCompletSrchPOReq" ServicePath="~/InvSetupSupport/AutoCompleteSearch.asmx" FirstRowSelected="true" CompletionInterval="100" CompletionSetCount="8">
                                        </ajaxToolkit:AutoCompleteExtender>
                                        <asp:HiddenField runat="server" ID="aceValue" />
                                        <asp:TextBox ID="txtReqNoSrch" runat="server" Width="98%"></asp:TextBox>
                                </div>
                                <div style="float:left; vertical-align:bottom;">
                                        <asp:LinkButton ID="btnReqNoSrch" runat="server" Text="Add PO Request" CssClass="lnk" OnClick="btnReqNoSrch_Click" ToolTip="Add PO Request to grid."></asp:LinkButton>
                                </div>
                                     
                                <br />     
                                     
                                           
                                           
                                <asp:UpdatePanel ID="uPnl" runat="server" UpdateMode="Conditional">
                                      <ContentTemplate>
                                      
                                      <table class="table" style="float:left;">
                                      <tr>
                                      <td>
                                      
                                      
                                        <asp:GridView ID="GridView1" runat="server"  CssClass="t_grd" Width="100%" AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound" ShowFooter="true">
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
                                              
                                              <asp:TemplateField HeaderText="Sr.">
                                                  <ItemTemplate>
                                                      <asp:Label runat="server" ID="lblSr" Text='<%#Eval("Sr") %>' Width="20px">
                                                      </asp:Label>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="20px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Req No*">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtReqNo" runat="server" Text='<%#Eval("ReqNo")%>' ReadOnly="true" TabIndex="-1" Width="50px"/>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="50px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Item*">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtItem" runat="server" Text='<%#Eval("Item") %>' Width="80px" />
                                                </ItemTemplate>
                                                <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtItemDesc" runat="server" Text='<%#Eval("ItemDesc") %>' TabIndex="-1" Width="150px" />
                                                </ItemTemplate>
                                                <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="150px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Specs">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtSpec" runat="server" Text='<%#Eval("Spec")%>' MaxLength="100" Width="60px" />
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" CssClass="DisplayNone"/>
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px" CssClass="DisplayNone"/>
                                                  <FooterStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px" CssClass="DisplayNone"/>
                                              </asp:TemplateField>
                                              
                                              
                                             <asp:TemplateField HeaderText="UOM">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtUomItem" runat="server" Text='<%#Eval("UOM") %>'  Width="40px" ReadOnly="true" TabIndex="-1" />
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"  Width="40px"/>
                                              </asp:TemplateField>
                                              
                                              
                                              <asp:TemplateField HeaderText="Req Qty">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtPoQuantity" runat="server" Text='<%#Eval("PoQuantity")%>' style="text-align:right;" MaxLength="15" ReadOnly="true" TabIndex="-1"  Width="50px"/>
                                                    <asp:RangeValidator runat="server" ID="rngPpQty" ValidationGroup="main" ErrorMessage="Invalid/Out of range Qty" SetFocusOnError="true"
                                                        ControlToValidate="txtPoQuantity" Display="Dynamic" CssClass="validateGridView" MinimumValue="000000000.00" MaximumValue="999999999.999" Type="Double" ></asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="reqPoQty" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtPoQuantity" ErrorMessage="PO Qty Reqd" ></asp:RequiredFieldValidator>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="50px"/>
                                              </asp:TemplateField>
                                              
                                              
                                              
                                              <asp:TemplateField HeaderText="Qty*" >
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtQuantity" runat="server" Text='<%#Eval("Quantity")%>' style="text-align:right;" MaxLength="15"  Width="50px" />
                                                    <asp:RangeValidator runat="server" ID="rngQty" ValidationGroup="main" ErrorMessage="Invalid/Out of range Qty" SetFocusOnError="true"
                                                        ControlToValidate="txtQuantity" Display="Dynamic" CssClass="validateGridView" MinimumValue="000000000.00" MaximumValue="999999999.999" Type="Double" ></asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="reqQty" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtQuantity" ErrorMessage="Qty Reqd" ></asp:RequiredFieldValidator>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="50px"/>
                                              </asp:TemplateField>
                                              
                                              
                                              
                                              
                                              <asp:TemplateField HeaderText="Rate*">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtRate" runat="server" Text='<%#Eval("Rate")%>' style="text-align:right;" MaxLength="15"  Width="50px"/>
                                                    <asp:RangeValidator runat="server" ID="rngRt" ValidationGroup="main" ErrorMessage="Invalid/Out of range rate" SetFocusOnError="true"
                                                        ControlToValidate="txtRate" Display="Dynamic" CssClass="validateGridView" MinimumValue="000000000.00" MaximumValue="999999999.999" Type="Double" ></asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="reqRt" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtRate" ErrorMessage="Rate Reqd" ></asp:RequiredFieldValidator>
                                                  </ItemTemplate>
                                                  <FooterTemplate>
                                                  <asp:Label ID="lbl1" runat="server" Text="Total:"></asp:Label>
                                                  </FooterTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="50px"/>
                                                  <FooterStyle VerticalAlign="Middle" HorizontalAlign="Right"/>
                                              </asp:TemplateField>
                                              
                                              
                                              <asp:TemplateField HeaderText="Amount">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtAmnt" runat="server" Text='<%#Eval("Amnt")%>' style="text-align:right;" ReadOnly="false" TabIndex="-1"  Width="60px" />
                                                  </ItemTemplate>
                                                  <FooterTemplate>
                                                  <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                                                  </FooterTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                                                  <FooterStyle VerticalAlign="Middle" HorizontalAlign="Right"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="GST">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlGST" runat="server" AppendDataBoundItems="true" CssClass="classDdlItem">
                                                        <asp:ListItem Selected="True" Text="No GST" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Due Date*">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtDueDate" runat="server" Text='<%#Eval("DueDate")%>'  Width="60px"/>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender23" runat="server" TargetControlID="txtDueDate" PopupPosition="BottomLeft" Format="dd-MMM-yy"></ajaxToolkit:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="reqDD" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtDueDate" ErrorMessage="Due Date Reqd" ></asp:RequiredFieldValidator>
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
                                <div width="50%" style="text-align:left; padding-bottom:15px;">
                                    <asp:UpdatePanel ID="updButPnl" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:LinkButton runat="server" ID="addRow" Text="Add Rows" OnClick="addRow_Click" CssClass="lnk"></asp:LinkButton>
                                </ContentTemplate>  
                            </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="top" colspan="4">
                                <br />
                                <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
                            </td>
                        </tr>
                    </asp:Panel>
                </table>
            </td>
            <td width="3%">
            </td>
        </tr>
        <tr>
            <td width="3%">
            </td>
            <td valign="top">
                <br />
                <table class="filterTable" width="70%">
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblFltName" runat="server" Text="PO No:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltPoNo" Width="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label22" runat="server" Text="Party:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltParty" Width="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                OnClick="btnSearch_Click" ToolTip="Search purchase requests" />
                        </td>
                    </tr>
                </table>
                
                <asp:GridView ID="grdPo" runat="server" DataKeyNames="vr_id" 
                OnSelectedIndexChanged="grdPo_SelectedIndexChanged" OnPageIndexChanging="grdPo_PageIndexChanging"
                OnRowDataBound="grdPo_RowDataBound"
                    AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No PO found" Width="750px">
                    <Columns>
                        <asp:BoundField DataField="vr_no" HeaderText="PO No" />
                        <asp:BoundField DataField="RevSeq" HeaderText="Revision" />
                        <asp:BoundField DataField="vendorNme" HeaderText="Vendor" />
                        <asp:BoundField DataField="vr_dt" HeaderText="PO Date" />                        
                        <asp:BoundField DataField="vr_apr" HeaderText="Status" />                       
                        <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                        
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPrint" runat="server" Text="Print" CssClass="lnk" OnClick="lnkPrint_Click"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                            
                    </Columns>
                    <HeaderStyle CssClass="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                </asp:GridView>
            </td>
            <td width="3%">
            </td>
        </tr>
    </table>
</asp:Content>
