<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="InvCancelGP.aspx.cs" Inherits="RMS.Inv.InvCancelGP" Culture="auto" UICulture="auto"
    EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
    //DISABLING DOUBLE CLICK
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    function BeginRequestHandler(sender, args) { var oControl = args.get_postBackElement(); oControl.disabled = true; }
    function prompt4Cancel() {
        return confirm("Are your sure, you want to cancel GP?");
    }
</script>

<script type="text/javascript">

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

    
    function pageLoad() {

        calculatequantity();


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        $("[id*=GridView1] input[id*='chkIncluded']:checkbox").click(function () {
            if ($(this).is(':checked')) {
                ValidatorEnable($(this).closest('tr').find('[id*=reqPacks]')[0], true);
                ValidatorEnable($(this).closest('tr').find('[id*=reqUnitSize]')[0], true);
                ValidatorEnable($(this).closest('tr').find('[id*=reqQty]')[0], true);
            }
            else {
                ValidatorEnable($(this).closest('tr').find('[id*=reqPacks]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqUnitSize]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqQty]')[0], false);
            }
        });

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        $("[id*=GridView1]input[type=text][id*=txtItem]").each(function() {
            var itm = $(this).val();
            if (itm == "") {
                $(this).closest('tr').find("input[type=text][id*=txtItem]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtItemDesc]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtPacks]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtUnitSize]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtUomQty]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtQuantity]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtGrossWt]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtRemarks]").val('');
                ValidatorEnable($(this).closest('tr').find('[id*=reqPacks]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqUnitSize]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqQty]')[0], false);

                $(this).closest('tr').find("input[id*='chkIncluded']:checkbox").attr('checked', false);
            }
            else {
                ValidatorEnable($(this).closest('tr').find('[id*=reqPacks]')[0], true);
                ValidatorEnable($(this).closest('tr').find('[id*=reqUnitSize]')[0], true);
                ValidatorEnable($(this).closest('tr').find('[id*=reqQty]')[0], true);
                $(this).closest('tr').find("input[id*='chkIncluded']:checkbox").attr('checked', true);
            }
        });
        $("[id*=GridView1]input[type=text][id*=txtItem]").change(function(e) {
            var itm = $(this).val();
            if (itm == "") {
                $(this).closest('tr').find("input[type=text][id*=txtItem]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtItemDesc]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtPacks]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtUnitSize]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtUomQty]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtQuantity]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtGrossWt]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtRemarks]").val('');
                ValidatorEnable($(this).closest('tr').find('[id*=reqPacks]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqUnitSize]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqQty]')[0], false);

                $(this).closest('tr').find("input[id*='chkIncluded']:checkbox").attr('checked', false);
            }
            else {
                $(this).closest('tr').find("input[id*='chkIncluded']:checkbox").attr('checked', true);
            }
        });
        $("[id*=GridView1]input[type=text][id*=txtItem]").autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: "IGPMgt.aspx/GetItemDetail",
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
                                value: item.itm_cd + ' , ' + item.itm_dsc + ' (' + item.uom_dsc + ')',
                                result: item.itm_cd + ' , ' + item.itm_dsc + ' , ' + item.uom_dsc + ' , ' + item.stk_qty,
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
                codeItm = codeItm.split(" , ");
                if (codeItm[1] != null && codeItm[1] != "") {
                    $(e.target).closest('tr').find("input[type=text][id*=txtItem]").val(codeItm[0]);
                    $(e.target).closest('tr').find("input[type=text][id*=txtItemDesc]").val(codeItm[1]);
                    $(e.target).closest('tr').find("input[type=text][id*=txtUomQty]").val(codeItm[2]);
                    ValidatorEnable($(this).closest('tr').find('[id*=reqPacks]')[0], true);
                    ValidatorEnable($(this).closest('tr').find('[id*=reqUnitSize]')[0], true);
                    ValidatorEnable($(this).closest('tr').find('[id*=reqQty]')[0], true);
                    $(e.target).closest('tr').find("input[id*='chkIncluded']:checkbox").attr('checked', true);
                }
                else {
                    $(e.target).closest('tr').find("input[type=text][id*=txtItem]").val('');
                    $(e.target).closest('tr').find("input[type=text][id*=txtItemDesc]").val('');
                    $(e.target).closest('tr').find("input[type=text][id*=txtUomQty]").val('');
                    ValidatorEnable($(this).closest('tr').find('[id*=reqPacks]')[0], false);
                    ValidatorEnable($(this).closest('tr').find('[id*=reqUnitSize]')[0], false);
                    ValidatorEnable($(this).closest('tr').find('[id*=reqQty]')[0], false);
                    $(e.target).closest('tr').find("input[id*='chkIncluded']:checkbox").attr('checked', false);
                }
                return false;
            },

            minLength: 1
        });
   
        $('#<%= ddlVendor.ClientID %>').change(function(event) {

            var ddlVendorVal = $('#<%= ddlVendor.ClientID %>').val();
            //alert(ddlVendorVal + "    change");
            $find('<%=AutoCompleteExtender1.ClientID %>').set_contextKey(ddlVendorVal);
        });

        $('#<%= txtPoSrch.ClientID %>').keydown(function(event) {

            var ddlVendorVal = $('#<%= ddlVendor.ClientID %>').val();
            if (ddlVendorVal == "0") {

                alert("Select party");
                $('#<%= txtPoSrch.ClientID %>').val('');
                $('#<%= ddlVendor.ClientID %>').focus();
            }
            else {
                //alert(ddlVendorVal+"    keydown");
                $find('<%=AutoCompleteExtender1.ClientID %>').set_contextKey(ddlVendorVal);
            }
        });
        
        
        var perAgeExist, flag, minval, maxval;
        $("[id*=GridView1]input[type=text][id*=txtPacks]").blur(function(event) {

            //QUANTITY VARIATION PERCENTAGE CHECK
            if ($(this).closest("tr").find("input[type=text][id*=txtPoref]").val() != '99999') {


                var qty = $(this).closest("tr").find("input[type=text][id*=txtQuantity]").val()
                var poref = $(this).closest("tr").find("input[type=text][id*=txtPoref]").val();
                var seqno = $(this).closest("tr").find("input[type=text][id*=txtSeqNo]").val();

                $.ajax({
                    url: "IGPMgt.aspx/VariantData",
                    async: false,
                    data: JSON.stringify({ poref: poref, seqno: seqno, qty: qty }),
                    type: "POST",
                    contentType: "application/json",
                    dataType: "json",
                    success: function(values) {
                        var values = values.d;
                        if (values.length > 0) {

                            perAgeExist = values[0].percentageexist;
                            flag = values[0].flag;
                            minval = values[0].min;
                            maxval = values[0].max;
                        }
                    }
                });
            }

            if (perAgeExist) {
                if (!flag) {

                    if ($(this).closest("tr").find("input[type=text][id*=txtQuantity]").val() > 0) {

                        //alert("Quantity must be greater than equal to " + minval + " and less than equal to " + maxval + ".");
                        alert("Quantity must be less than equal to " + maxval + ".");
                        $(this).closest("tr").find("input[type=text][id*=txtQuantity]").val('');
                        $(this).closest("tr").find("input[type=text][id*=txtUnitSize]").val('');
                        $(this).closest("tr").find("input[type=text][id*=txtPacks]").val('');
                        $(this).closest("tr").find("input[type=text][id*=txtPacks]").focus();
                    }
                }
            }

            var totalQuantity = calculatequantity();
            $("span[id*=lblGrossQuantityTotal]").text(totalQuantity);
        });


        $("[id*=GridView1]input[type=text][id*=txtUnitSize]").blur(function(event) {

            //QUANTITY VARIATION PERCENTAGE CHECK
            if ($(this).closest("tr").find("input[type=text][id*=txtPoref]").val() != '99999') {


                var qty = $(this).closest("tr").find("input[type=text][id*=txtQuantity]").val()
                var poref = $(this).closest("tr").find("input[type=text][id*=txtPoref]").val();
                var seqno = $(this).closest("tr").find("input[type=text][id*=txtSeqNo]").val();

                $.ajax({
                    url: "IGPMgt.aspx/VariantData",
                    async: false,
                    data: JSON.stringify({ poref: poref, seqno: seqno, qty: qty }),
                    type: "POST",
                    contentType: "application/json",
                    dataType: "json",
                    success: function(values) {
                        var values = values.d;
                        if (values.length > 0) {

                            perAgeExist = values[0].percentageexist;
                            flag = values[0].flag;
                            minval = values[0].min;
                            maxval = values[0].max;
                            
                            
                        }
                    }
                });
            }

            if (perAgeExist) {
                if (!flag) {
                    if ($(this).closest("tr").find("input[type=text][id*=txtQuantity]").val() > 0) {

                        //alert("Quantity must be greater than equal to " + minval + " and less than equal to " + maxval + ".");
                        alert("Quantity must be less than equal to " + maxval + ".");
                        $(this).closest("tr").find("input[type=text][id*=txtQuantity]").val('');
                        $(this).closest("tr").find("input[type=text][id*=txtUnitSize]").val('');
                        $(this).closest("tr").find("input[type=text][id*=txtPacks]").val('');
                        $(this).closest("tr").find("input[type=text][id*=txtPacks]").focus();
                    }
                }
            }

            var totalQuantity = calculatequantity();
            $("span[id*=lblGrossQuantityTotal]").text(totalQuantity);
        });


        
        $("[id*=GridView1]input[type=text][id*=txtQuantity]").keyup(function(event) {

            var totalQuantity = calculatequantity();
            $("span[id*=lblGrossQuantityTotal]").text(totalQuantity);
        });
     
        
        var totalQuantity = calculatequantity();
        $("span[id*=lblGrossQuantityTotal]").text(totalQuantity);


        $("[id*=GridView1]input[type=text][id*=txtPacks]").keyup(function(event) {
            var value = parseFloat($(this).val());
            if (isNaN(value)) value = 0;
            var unitsize = parseFloat($(this).closest("tr").find("input[type=text][id*=txtUnitSize]").val());
            if (isNaN(unitsize)) unitsize = 0;
            $(this).closest("tr").find("input[type=text][id*=txtQuantity]").val(unitsize * value);
        });
        $("[id*=GridView1]input[type=text][id*=txtUnitSize]").keyup(function(event) {
            var value = parseFloat($(this).val());
            if (isNaN(value)) value = 0;
            var packs = parseFloat($(this).closest("tr").find("input[type=text][id*=txtPacks]").val());
            if (isNaN(packs)) packs = 0;
            $(this).closest("tr").find("input[type=text][id*=txtQuantity]").val(packs * value);
        });



        
        $("[id*=GridView1]input[type=text][id*=txtPacks]").keyup(function(event) {
            var totalQuantity = calculatequantity();
            $("span[id*=lblGrossQuantityTotal]").text(totalQuantity);
        });
        $("[id*=GridView1]input[type=text][id*=txtUnitSize]").keyup(function(event) {
            var totalQuantity = calculatequantity();
            $("span[id*=lblGrossQuantityTotal]").text(totalQuantity);
        });

  
        function calculatequantity() {
            var totalQty = 0, temp1=0;
            $("[id*=GridView1]input[type=text][id*=txtUnitSize]").each(function() {
                temp = parseFloat($(this).val());
                if (isNaN(temp)) temp = 0;
                temp1 = parseFloat($(this).closest("tr").find("input[type=text][id*=txtPacks]").val());
                if (isNaN(temp1)) temp1 = 0;
                $(this).closest("tr").find("input[type=text][id*=txtQuantity]").val(temp * temp1);
                totalQty = totalQty + temp * temp1;
            });
            return totalQty;
        }

        
    }
   
</script>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="1%">
            </td>
            <td>
                <table cellspacing="2" align="center" border="0" width="98%">
                    <asp:Panel runat="server" ID="pnlMain">

                        <asp:Panel runat = "server" ID = "pnlLoc" Enabled ="false">
                        <tr>
                            <td class="LblBgSetup">
                                <asp:Label ID="lblloc" runat="server" Text="To Loc:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLoc" runat="server" AppendDataBoundItems="true" CssClass="RequiredFieldDropDown" AutoPostBack="true" OnSelectedIndexChanged="ddlLoc_OnSelectedIndexChanged">
                                <asp:ListItem Text="Select Store Location" Selected="True" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlLoc"
                                    ErrorMessage="Please select store location" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator> 
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
                                <asp:Label ID="Label26" runat="server" Text="IGP #:"></asp:Label>
                            </td>
                            <td>
                                <table width="100%"><tr>
                                    <td align="left">
                                        <asp:TextBox ID="txtGPNo" runat="server" Width="80" Enabled="false" />
                                    </td>
                                    <td class="LblBgSetup">
                                        <asp:Label ID="Label12" runat="server" Text="GP Ref:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIgpRef" runat="server" CssClass="TxtSmall" MaxLength="20" />
                                    </td>
                                </tr></table>
                            </td>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label25" runat="server" Text="Doc Ref #:" Visible="false"></asp:Label>
                                <asp:Label ID="Label4" runat="server" Text="IGP Date:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocRef" runat="server" CssClass="RequiredFieldTxtSmall" Visible="false" ReadOnly="true">
                                </asp:TextBox>
                                <asp:TextBox ID="txtGpDate" runat="server" CssClass="RequiredFieldDate"></asp:TextBox>
                                
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtGpDate"
                                    ErrorMessage="Please select IGP Date" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator> 
                                <span class="DteLtrl">
                                    <asp:Literal ID="Literal2"  runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                                </span>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtGpDate" PopupPosition="BottomRight" >
                                  </ajaxToolkit:CalendarExtender>
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label122" runat="server" Text="Party:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlVendor" runat="server" CssClass="RequiredFieldDropDownVendor" AppendDataBoundItems="true" Width="300">
                                    <asp:ListItem Text="Select Party" Selected="True" Value="0">
                                    </asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlVendor"
                                    ErrorMessage="Please select party" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label2" runat="server" Text="From Loc:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCity" runat="server" CssClass="RequiredFieldDropDown" AppendDataBoundItems="true" Width="110px">
                                    <asp:ListItem Text="Select City" Selected="True" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlCity"
                                    ErrorMessage="Please select city" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator> 
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label6" runat="server" Text="Vehicle No:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVehicleNo" runat="server" CssClass="TxtNormal" MaxLength="20" Width="100">
                                </asp:TextBox>
                            </td>
                             <td class="LblBgSetup">
                                <asp:Label ID="Label7" runat="server" Text="Freight:" Visible="false"></asp:Label>
                                <asp:Label ID="Label1" runat="server" Text="Total Qty:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFrieght" runat="server" CssClass="TxtNormal" MaxLength="9" Width="100" Visible="false"> 
                                </asp:TextBox>
                                <asp:RangeValidator id="RangeValidator1" ControlToValidate="txtFrieght"
                                       MinimumValue="0" MaximumValue="999999999" Type="Double" SetFocusOnError="true"
                                       ErrorMessage="The frieght value must be numeric" ValidationGroup="main" Display="None" runat="server"/>


                                 <asp:TextBox ID="txtTotalQty" runat="server"  CssClass="RequiredField" Width="100">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="regValidator" runat="server" SetFocusOnError="true" ControlToValidate="txtTotalQty"
                                 ErrorMessage="Please select total quantity" ValidationGroup="main" Display="None">
                                 </asp:RequiredFieldValidator>
                            
                              <asp:RangeValidator id="Range1txtTotalQty" ControlToValidate="txtTotalQty"
                                   MinimumValue="000000000000.001" MaximumValue="99999999999999999.00" Type="Double" SetFocusOnError="true"
                                   ErrorMessage="The total quantity value must be numeric and greater than zero" ValidationGroup="main" Display="None" runat="server"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label5" runat="server" Text="Bilty No:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBiltyNo" runat="server" CssClass="TxtNormal" MaxLength="20" Width="100">
                                </asp:TextBox>
                            </td>
                           <td>
                                
                            </td>
                            <td>
                              
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="Label10" runat="server" Text="Remarks:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" width="182" height="67" 
                                 onkeyup="LimitText(this,100);" onblur="LimitText(this,100);" CssClass="txtRemz"></asp:TextBox>
                            </td>
                            <td>
                                
                            </td>
                            <td>
                            
                            </td>
                        </tr>

                        </asp:Panel>
                        <tr><td colspan="4">&nbsp;</td></tr>
                        <tr>
                        
                          <td colspan="4">
                        
                        
                                <div style="width:72%; float:left; vertical-align:bottom; display:none;" class="LblBgSetup" >
                                        <asp:Label ID="Label14" runat="server" Text="Search PO:"></asp:Label>
                                    <br />
                                        <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="CompletionListCssClass" 
                                         TargetControlID="txtPoSrch" OnClientItemSelected="ace_ItemSelected" EnableCaching="true"
                                          MinimumPrefixLength="1"  ServiceMethod="AutoCompletSrchPO" 
                                          ServicePath="~/InvSetupSupport/AutoCompleteSearch.asmx" FirstRowSelected="true" 
                                          CompletionInterval="100" CompletionSetCount="8" UseContextKey="true">
                                        </ajaxToolkit:AutoCompleteExtender>
                                        <asp:HiddenField runat="server" ID="aceValue" />
                                        <asp:TextBox ID="txtPoSrch" runat="server" Width="98%"></asp:TextBox>
                                </div>
                                <div style="float:left; vertical-align:bottom;  display:none;">
                                        <asp:LinkButton ID="btnPoSrch" runat="server" Text="Add PO" CssClass="lnk" OnClick="btnPoSrch_Click" ToolTip="Add PO to grid."></asp:LinkButton>
                                </div>
                        
                        
                        
                                <asp:UpdatePanel ID="uPnl" runat="server" UpdateMode="Conditional">
                                      <ContentTemplate>
                                      
                                      <table class="table" width="100%" style="float:left;">
                                      <tr>
                                      <td>
                                      
                                        <asp:GridView ID="GridView1" runat="server" CssClass="t_grd"  Width="100%" AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="GridView1_RowDataBound">
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
                                              
                                              
                                              <asp:TemplateField HeaderText="PO Ref#">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtPoref" runat="server" Text='<%#Eval("PoRef")%>' Width="50px" TabIndex="-1"/>
                                                    <asp:RangeValidator runat="server" ID="rv1" ValidationGroup="main" ErrorMessage="Invalid/Out of range PO Ref#" SetFocusOnError="true"
                                                        ControlToValidate="txtPoref" Display="None" MinimumValue="000000000" MaximumValue="999999999" Type="Integer" ></asp:RangeValidator>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="50px"/>
                                              </asp:TemplateField>
                                              
                                              
                                              <asp:TemplateField HeaderText="Item*">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtItem" runat="server" Text='<%#Eval("Item") %>' Width="100px" />
                                                </ItemTemplate>
                                                <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="100px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtItemDesc" runat="server" Text='<%#Eval("ItemDesc") %>' TabIndex="-1" Width="150px" />
                                                </ItemTemplate>
                                                <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="150px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Packs*">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtPacks" runat="server" Text='<%#Eval("Packs")%>' Width="50px" style="text-align:right;"/>
                                                    <asp:RangeValidator runat="server" ID="rv2" ValidationGroup="main" ErrorMessage="Invalid/Out of range packs" SetFocusOnError="true" CssClass="validateGridView"
                                                        ControlToValidate="txtPacks" Display="Dynamic" MinimumValue="1.00" MaximumValue="999999999.99" Type="Double" ></asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="reqPacks" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtPacks" ErrorMessage="Pack Reqd" Enabled="false"></asp:RequiredFieldValidator>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="50px"/>
                                              </asp:TemplateField>
                                              
                                              
                                              <%--<asp:TemplateField HeaderText="UOM*">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlUom" runat="server" AppendDataBoundItems="true" >
                                                        <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="reqUOM" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="ddlUom" ErrorMessage="UOM Reqd" InitialValue="0" Enabled="false"></asp:RequiredFieldValidator>
                                                </ItemTemplate>
                                                <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"/>
                                              </asp:TemplateField>--%>
                                              
                                              
                                              <asp:TemplateField HeaderText="Unit Size*">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtUnitSize" runat="server" Text='<%#Eval("UnitSize")%>' Width="50px" style="text-align:right;"/>
                                                    <asp:RangeValidator runat="server" ID="rv3" ValidationGroup="main" ErrorMessage="Invalid/Out of range unit size" SetFocusOnError="true"
                                                        ControlToValidate="txtUnitSize" Display="Dynamic" CssClass="validateGridView" MinimumValue="000000000.00" MaximumValue="9999999.99" Type="Double" ></asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="reqUnitSize" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtUnitSize" ErrorMessage="Unit Size Reqd" Enabled="false"></asp:RequiredFieldValidator>   
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="50px"/>
                                              </asp:TemplateField>
                                              
                                              
                                              <asp:TemplateField HeaderText="UOM">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtUomQty" runat="server" Text='<%#Eval("UomQty") %>'  Width="50px" ReadOnly="false" TabIndex="-1"/>
                                                  </ItemTemplate>
                                                  <FooterTemplate>
                                                    <asp:Label ID="lbllLtrlTotal" runat="server" Text="Total:"></asp:Label>
                                                  </FooterTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="50px"/>
                                                  <FooterStyle VerticalAlign="Middle" HorizontalAlign="Right"/>
                                              </asp:TemplateField>
                                              
                                              
                                              <asp:TemplateField HeaderText="Qty*">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtQuantity" runat="server" Text='<%#Eval("Quantity")%>' Width="60px" ReadOnly="false" TabIndex="-1" style="text-align:right;" />
                                                    <asp:RangeValidator runat="server" ID="rv4" ValidationGroup="main" ErrorMessage="Invalid/Out of range Qty" SetFocusOnError="true" CssClass="validateGridView"
                                                        ControlToValidate="txtQuantity" Display="Dynamic" MinimumValue="000000000.00" MaximumValue="999999999.99" Type="Double" ></asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="reqQty" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtQuantity" ErrorMessage="Qty Reqd" Enabled="false"></asp:RequiredFieldValidator>   
                                                  </ItemTemplate>
                                                  <FooterTemplate>
                                                    <asp:Label ID="lblGrossQuantityTotal" runat="server" Text="0"></asp:Label>
                                                  </FooterTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                                                  <FooterStyle VerticalAlign="Middle" HorizontalAlign="Right"/>
                                              </asp:TemplateField>
                                              
                                              
                                              <asp:TemplateField HeaderText="Gross Wt">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtGrossWt" runat="server" Text='<%#Eval("GrossWt")%>' Width="60px" style="text-align:right;"/>
                                                    <asp:RangeValidator runat="server" ID="rv5" ValidationGroup="main" ErrorMessage="Invalid/Out of range Gross Wt" SetFocusOnError="true"
                                                        ControlToValidate="txtGrossWt" Display="None" MinimumValue="000000000.00" MaximumValue="999999999.99" Type="Double" ></asp:RangeValidator>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                                              </asp:TemplateField>
                                              
                                              
                                              <asp:TemplateField HeaderText="Remarks">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("Rem")%>' Width="60px" MaxLength="99"/>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                                              </asp:TemplateField>
                                               <%----%>
                                              <asp:TemplateField HeaderText="Seq No">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtSeqNo" runat="server" Text='<%#Eval("SeqNo")%>' MaxLength="99"/>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle CssClass="DisplayNone" />
                                                  <ItemStyle CssClass="DisplayNone"/>
                                                  <FooterStyle CssClass="DisplayNone"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Included">
                                                  <ItemTemplate>
                                                    <asp:CheckBox ID="chkIncluded" runat="server"/>
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
                                <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" Visible = "false" />
                                <asp:ImageButton ID="btnCancelVendor" runat ="server" OnClick="btnCancel_Click" OnClientClick="return prompt4Cancel()" ImageUrl="~/images/btn_cancel.png" onMouseOver="this.src='../images/btn_cancel_m.png'" onMouseOut="this.src='../images/btn_cancel.png'" />
                                <asp:ImageButton ID="btnClear" runat ="server"  OnClick="btnClear_Click" ImageUrl="~/images/btn_clear.png" onMouseOver="this.src='../images/btn_clear_m.png'" onMouseOut="this.src='../images/btn_clear.png'" />
                            </td>
                        </tr>
                    </asp:Panel>
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
                <table class="filterTable" width="100%">
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblFltName" runat="server" Text="IGP#:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltIgpNo" Width="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label22" runat="server" Text="Party:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltParty" Width="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="City:"></asp:Label><br />
                            <asp:DropDownList  ID="ddlFltCity" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Text="All" Value="0" />
                            </asp:DropDownList>
                        </td>
                       <td>
                            <asp:Label ID="Label8" runat="server" Text="Status:"></asp:Label><br />
                            <asp:DropDownList  ID="ddlFltStatus" runat="server" Enabled = "false">
                                <asp:ListItem Text="All" Value="0" />
                                <asp:ListItem Text="Approved" Value="A" Selected = "True" />
                                <asp:ListItem Text="Pending" Value="P" />
                                <asp:ListItem Text="Cancelled" Value="C" />    
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                OnClick="btnSearch_Click" ToolTip="Search purchase requests" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="grdIgp" runat="server" DataKeyNames="locid,br_id,vt_cd,vr_no" 
                OnSelectedIndexChanged="grdIgp_SelectedIndexChanged"
                 OnPageIndexChanging="grdIgp_PageIndexChanging" OnRowDataBound="grdIgp_RowDataBound"
                    AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No IGP found" Width="100%">
                    <Columns>
                        <asp:BoundField DataField="vr_no" HeaderText="IGP #" />
                        <asp:BoundField DataField="LocName" HeaderText="Location" />
                        <asp:BoundField DataField="gl_dsc" HeaderText="Party" />
                        <asp:BoundField DataField="vr_dt" HeaderText="Date" />                        
                        <asp:BoundField DataField="vr_apr" HeaderText="Status" /> 
                          <asp:BoundField DataField="biltyno" HeaderText="Bilty No" />                       
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
            <td width="1%">
            </td>
        </tr>
    </table>
    
    <br />
    <br />
</asp:Content>
