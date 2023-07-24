<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="InvMPNMgt.aspx.cs" Inherits="RMS.Inv.InvMPNMgt" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript" language="javascript">
    //DISABLING DOUBLE CLICK
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    function BeginRequestHandler(sender, args) { var oControl = args.get_postBackElement(); oControl.disabled = true; }

</script>
<script type="text/javascript"> 
    function pageLoad() {
//        $(".datepicker").datepicker();
        /*INITIALIZE****************************************************/
        var totalAmount = calculateAmount();
        $("span[id*=lblAmountTotal]").text(totalAmount);
        var totalCDAmount = calculateCustomDuty();
        $("span[id*=lblCustomDutyTotal]").text(totalCDAmount);
        var totalGstAmount = calculateGSTAmount();
        $("span[id*=lblGSTTotal]").text(totalGstAmount);
        parseFloat($('#<%= txtGrAmount.ClientID %>').val(parseFloat(totalAmount) + parseFloat(totalGstAmount) + parseFloat(totalCDAmount))).toFixed(2);
        var totalCostAmount = calculatePopupAmount();
        if (totalCostAmount > 0) {
            $("span[id*=popuplblAmountTotal]").text(totalCostAmount);
            $('#<%= txtOtrCost.ClientID %>').val(totalCostAmount);
        }
        else {
            $("span[id*=popuplblAmountTotal]").text('0');
            $('#<%= txtOtrCost.ClientID %>').val('0');
        }
        var totalDue = calculateDueAmount();
        $('#<%= txtDue.ClientID %>').val(totalDue);
        /**************************************************************/
        $('#<%= txtFrt.ClientID %>').keydown(function(event) {
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }
        });
        $('#<%= txtImpFrt.ClientID %>').keydown(function (event) {
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }
        });
        $("[id*=GridView1]input[type=text][id*=txtAmount]").blur(function() {
            var num = parseFloat($(this).val());
            var cleanNum = num.toFixed(2);
            $(this).val(cleanNum);
            if (num / cleanNum < 1) {
                $('#error').text('Please enter only 2 decimal places, we have truncated extra points');
            }
        });

        $("[id*=GridView1]input[type=text][id*=txtAmount]").keydown(function(event) {

            if (event.keyCode != 110 && event.keyCode != 190 && (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }

            var txt = $("[id*=GridView1]input[type=text][id*=txtAmount]").val();
            if (event.keyCode == 110 || event.keyCode == 190) {
                if ((txt.split(".").length) > 1) {
                    event.preventDefault();
                }
            }
        });

        $("[id*=GridView1]input[type=text][id*=txtAmount]").keyup(function (event) {
            var totalAmount = calculateAmount();

            $("span[id*=lblAmountTotal]").text(totalAmount);
        
           //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            var poref = $(this).closest("tr").find("input[type=text][id*=txtPoRef]").val();
            var gstId = $(this).closest("tr").find("[id*=ddlGST]").val();

            var gRow = $(this);
            if (poref != '9999/9' && gstId != '0' && gstId != 'VAR') {
                var gst = 0;
                
                $.ajax({
                    url: "InvMPNMgt.aspx/GetGSTPercent",
                    data: JSON.stringify({ gstId: gstId }),
                    type: 'POST',
                    contentType: 'application/json;',
                    dataType: 'json',
                    success: function (heads) {
                        var heads = heads.d;
                        if (heads.length > 0) {
                            var amnt = gRow.closest("tr").find("input[type=text][id*=txtAmount]").val();
                            if (isNaN(amnt)) amnt = 0;

                            gRow.closest("tr").find("input[type=text][id*=txtGSTAmount]").val((heads[0].TaxRate * amnt / 100).toFixed(0));
                        }
                        else {
                            gRow.closest("tr").find("input[type=text][id*=txtGSTAmount]").val('0');
                        }
                    }
                });
            }
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

            var totalCDAmount = calculateCustomDuty();
            $("span[id*=lblCustomDutyTotal]").text(totalCDAmount);

            var totalGstAmount = calculateGSTAmount();
            $("span[id*=lblGSTTotal]").text(totalGstAmount);

            parseFloat($('#<%= txtGrAmount.ClientID %>').val(parseFloat(totalAmount) + parseFloat(totalGstAmount) + parseFloat(totalCDAmount))).toFixed(2);
            var totalDue = calculateDueAmount();
            $('#<%= txtDue.ClientID %>').val(totalDue);

            var totalCostAmount = calculatePopupAmount();
            $("span[id*=popuplblAmountTotal]").text(totalCostAmount);
        });

        $("[id*=GridView1]input[type=text][id*=txtAmount]").change(function(event) {
            var totalAmount = calculateAmount();
            $("span[id*=lblAmountTotal]").text(totalAmount);

//            var gst = parseFloat($(this).closest("tr").find("input[type=text][id*=txtGST]").val());
//            if (isNaN(gst)) gst = 0;
//            $(this).closest("tr").find("input[type=text][id*=txtGSTAmount]").val((gst * $(this).val() / 100).toFixed(0));

            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            var poref = $(this).closest("tr").find("input[type=text][id*=txtPoRef]").val();
            var gstId = $(this).closest("tr").find("[id*=ddlGST]").val();
            var gRow = $(this);
            if (poref == '9999/9' && gstId != '0' && gstId != 'VAR') {
                var gst = 0;
                $.ajax({
                    url: "InvMPNMgt.aspx/GetGSTPercent",
                    data: JSON.stringify({ gstId: gstId }),
                    type: 'POST',
                    contentType: 'application/json;',
                    dataType: 'json',
                    success: function(heads) {
                        var heads = heads.d;
                        if (heads.length > 0) {
                            var amnt = gRow.closest("tr").find("input[type=text][id*=txtAmount]").val();
                            if (isNaN(amnt)) amnt = 0;
                            gRow.closest("tr").find("input[type=text][id*=txtGSTAmount]").val((heads[0].TaxRate * amnt / 100).toFixed(0));
                        }
                        else {
                            gRow.closest("tr").find("input[type=text][id*=txtGSTAmount]").val('0');
                        }
                    }
                });
            }
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

            var totalCDAmount = calculateCustomDuty();
            $("span[id*=lblCustomDutyTotal]").text(totalCDAmount);

            var totalGstAmount = calculateGSTAmount();
            $("span[id*=lblGSTTotal]").text(totalGstAmount);

            parseFloat($('#<%= txtGrAmount.ClientID %>').val(parseFloat(totalAmount) + parseFloat(totalGstAmount) +parseFloat(totalCDAmount))).toFixed(2);
            var totalDue = calculateDueAmount();
            $('#<%= txtDue.ClientID %>').val(totalDue);

            var totalCostAmount = calculatePopupAmount();
            $("span[id*=popuplblAmountTotal]").text(totalCostAmount);
        });
        /****************************************************/
        $("[id*=GridView1]input[type=text][id*=txtGSTAmount]").keydown(function (event) {

            if (event.keyCode != 110 && event.keyCode != 190 && (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }

            var txt = $("[id*=GridView1]input[type=text][id*=txtGSTAmount]").val();
            if (event.keyCode == 110 || event.keyCode == 190) {
                if ((txt.split(".").length) > 1) {
                    event.preventDefault();
                }
            }
        });

        $("[id*=GridView1]input[type=text][id*=txtGSTAmount]").keyup(function (event) {
            var totalAmount = calculateAmount();

            $("span[id*=lblAmountTotal]").text(totalAmount);

            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            var poref = $(this).closest("tr").find("input[type=text][id*=txtPoRef]").val();
            var gstId = $(this).closest("tr").find("[id*=ddlGST]").val();

            var gRow = $(this);
            if (poref != '9999/9' && gstId != '0' && gstId != 'VAR') {
                var gst = 0;

                $.ajax({
                    url: "InvMPNMgt.aspx/GetGSTPercent",
                    data: JSON.stringify({ gstId: gstId }),
                    type: 'POST',
                    contentType: 'application/json;',
                    dataType: 'json',
                    success: function (heads) {
                        var heads = heads.d;
                        if (heads.length > 0) {
                            var amnt = gRow.closest("tr").find("input[type=text][id*=txtAmount]").val();
                            if (isNaN(amnt)) amnt = 0;

                            gRow.closest("tr").find("input[type=text][id*=txtGSTAmount]").val((heads[0].TaxRate * amnt / 100).toFixed(0));
                        }
                        else {
                            gRow.closest("tr").find("input[type=text][id*=txtGSTAmount]").val('0');
                        }
                    }
                });
            }
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

            var totalCDAmount = calculateCustomDuty();
            $("span[id*=lblCustomDutyTotal]").text(totalCDAmount);

            var totalGstAmount = calculateGSTAmount();
            $("span[id*=lblGSTTotal]").text(totalGstAmount);

            parseFloat($('#<%= txtGrAmount.ClientID %>').val(parseFloat(totalAmount) + parseFloat(totalGstAmount) + parseFloat(totalCDAmount))).toFixed(2);
            var totalDue = calculateDueAmount();
            $('#<%= txtDue.ClientID %>').val(totalDue);

            var totalCostAmount = calculatePopupAmount();
            $("span[id*=popuplblAmountTotal]").text(totalCostAmount);
        });
        $("[id*=GridView1]input[type=text][id*=txtGSTAmount]").change(function (event) {
            var totalAmount = calculateAmount();

            $("span[id*=lblAmountTotal]").text(totalAmount);

            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            var poref = $(this).closest("tr").find("input[type=text][id*=txtPoRef]").val();
            var gstId = $(this).closest("tr").find("[id*=ddlGST]").val();

            var gRow = $(this);
            if (poref != '9999/9' && gstId != '0' && gstId != 'VAR') {
                var gst = 0;

                $.ajax({
                    url: "InvMPNMgt.aspx/GetGSTPercent",
                    data: JSON.stringify({ gstId: gstId }),
                    type: 'POST',
                    contentType: 'application/json;',
                    dataType: 'json',
                    success: function (heads) {
                        var heads = heads.d;
                        if (heads.length > 0) {
                            var amnt = gRow.closest("tr").find("input[type=text][id*=txtAmount]").val();
                            if (isNaN(amnt)) amnt = 0;

                            gRow.closest("tr").find("input[type=text][id*=txtGSTAmount]").val((heads[0].TaxRate * amnt / 100).toFixed(0));
                        }
                        else {
                            gRow.closest("tr").find("input[type=text][id*=txtGSTAmount]").val('0');
                        }
                    }
                });
            }
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

            var totalCDAmount = calculateCustomDuty();
            $("span[id*=lblCustomDutyTotal]").text(totalCDAmount);

            var totalGstAmount = calculateGSTAmount();
            $("span[id*=lblGSTTotal]").text(totalGstAmount);

            parseFloat($('#<%= txtGrAmount.ClientID %>').val(parseFloat(totalAmount) + parseFloat(totalGstAmount) + parseFloat(totalCDAmount))).toFixed(2);
            var totalDue = calculateDueAmount();
            $('#<%= txtDue.ClientID %>').val(totalDue);

            var totalCostAmount = calculatePopupAmount();
            $("span[id*=popuplblAmountTotal]").text(totalCostAmount);
        });
        /****************************************************/
        $('#<%= txtDisc.ClientID %>').keyup(function(event) {
            var totalDue = calculateDueAmount();
            $('#<%= txtDue.ClientID %>').val(totalDue);
        });
        $('#<%= txtDisc.ClientID %>').change(function(event) {
            var totalDue = calculateDueAmount();
            $('#<%= txtDue.ClientID %>').val(totalDue);
        });
        /****************************************************/
        /****************************************************/
        $('#<%= txtAdv.ClientID %>').keyup(function(event) {
            var totalDue = calculateDueAmount();
            $('#<%= txtDue.ClientID %>').val(totalDue);
        });
        $('#<%= txtAdv.ClientID %>').change(function(event) {
            var totalDue = calculateDueAmount();
            $('#<%= txtDue.ClientID %>').val(totalDue);
        });
        /****************************************************/
        /****************************************************/
        $('#<%= txtFrt.ClientID %>').keyup(function(event) {
            var totalDue = calculateDueAmount();
            $('#<%= txtDue.ClientID %>').val(totalDue);
        });
        $('#<%= txtFrt.ClientID %>').change(function(event) {
            var totalDue = calculateDueAmount();
            $('#<%= txtDue.ClientID %>').val(totalDue);
        });
        /****************************************************/
        /****************************************************/
        $('#<%= txtImpFrt.ClientID %>').keyup(function (event) {
            var totalDue = calculateDueAmount();
            $('#<%= txtDue.ClientID %>').val(totalDue);
        });
        $('#<%= txtImpFrt.ClientID %>').change(function (event) {
            var totalDue = calculateDueAmount();
            $('#<%= txtDue.ClientID %>').val(totalDue);
        });
        /****************************************************/
        /****************************************************/
        $('#<%= txtWHT.ClientID %>').keyup(function(event) {
            var totalDue = calculateDueAmount();
            $('#<%= txtDue.ClientID %>').val(totalDue);
        });
        $('#<%= txtWHT.ClientID %>').change(function(event) {
            var totalDue = calculateDueAmount();
            $('#<%= txtDue.ClientID %>').val(totalDue);
        });
        /****************************************************/
        //Custom Duty
        /****************************************************/
        $("[id*=GridView1]input[type=text][id*=txtCustomDuty]").blur(function() {
            var num = parseFloat($(this).val());
            var cleanNum = num.toFixed(0);
            $(this).val(cleanNum);
            if (num / cleanNum < 1) {
                $('#error').text('Please enter only 2 decimal places, we have truncated extra points');
            }
        });

        $("[id*=GridView1]input[type=text][id*=txtCustomDuty]").keydown(function(event) {

            if (event.keyCode != 110 && event.keyCode != 190 && (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }

            var txt = $("[id*=GridView1]input[type=text][id*=txtCustomDuty]").val();
            if (event.keyCode == 110 || event.keyCode == 190) {
                if ((txt.split(".").length) > 1) {
                    event.preventDefault();
                }
            }
        });

        $("[id*=GridView1]input[type=text][id*=txtCustomDuty]").keyup(function(event) {
            var totalAmount = calculateAmount();
            $("span[id*=lblAmountTotal]").text(totalAmount);

            var totalGstAmount = calculateGSTAmount();
            $("span[id*=lblGSTTotal]").text(totalGstAmount);

            var totalCDAmount = calculateCustomDuty();
            $("span[id*=lblCustomDutyTotal]").text(totalCDAmount);

            parseFloat($('#<%= txtGrAmount.ClientID %>').val(parseFloat(totalAmount) + parseFloat(totalGstAmount) + parseFloat(totalCDAmount))).toFixed(2);
            var totalDue = calculateDueAmount();
            $('#<%= txtDue.ClientID %>').val(totalDue);

            var totalCostAmount = calculatePopupAmount();
            $("span[id*=popuplblAmountTotal]").text(totalCostAmount);
        });

        $("[id*=GridView1]input[type=text][id*=txtCustomDuty]").change(function (event) {
  
            var totalAmount = calculateAmount();
            $("span[id*=lblAmountTotal]").text(totalAmount);

            var totalGstAmount = calculateGSTAmount();
            $("span[id*=lblGSTTotal]").text(totalGstAmount);

            var totalCDAmount = calculateCustomDuty();
            $("span[id*=lblCustomDutyTotal]").text(totalCDAmount);

            parseFloat($('#<%= txtGrAmount.ClientID %>').val(parseFloat(totalAmount) + parseFloat(totalGstAmount) + parseFloat(totalCDAmount))).toFixed(2);
            var totalDue = calculateDueAmount();
            $('#<%= txtDue.ClientID %>').val(totalDue);

            var totalCostAmount = calculatePopupAmount();
            $("span[id*=popuplblAmountTotal]").text(totalCostAmount);
        });
        /****************************************************/
        //End Custom Duty
        /****************************************************/
        function calculateCustomDuty() {
            var totalvat = 0;
            $("[id*=GridView1]input[type=text][id*=txtCustomDuty]").each(function() {
                temp = parseFloat($(this).val());
                if (isNaN(temp)) temp = 0;
                totalvat = totalvat + temp;
            });
            return parseFloat(totalvat).toFixed(0);
        }
        function calculateAmount() {
            var totalvat = 0;
            $("[id*=GridView1]input[type=text][id*=txtAmount]").each(function() {
                temp = parseFloat($(this).val());
                if (isNaN(temp)) temp = 0;
                totalvat = totalvat + temp;
            });
            return parseFloat(totalvat).toFixed(2);
        }
        function calculateGSTAmount() {
            var totalvat = 0;
            $("[id*=GridView1]input[type=text][id*=txtGSTAmount]").each(function() {
                temp = parseFloat($(this).val());
                if (isNaN(temp)) temp = 0;
                totalvat = totalvat + temp;
            });
            return parseFloat(totalvat).toFixed(0);
        }
        function calculateDueAmount() {
            var grAmount = 0, tax = 0, disc = 0, frt = 0, impfrt = 0, otrcost = 0, adv = 0;
            grAmount = parseFloat($('#<%= txtGrAmount.ClientID %>').val()); if (isNaN(grAmount)) grAmount = 0;
            tax = parseFloat($('#<%= txtWHT.ClientID %>').val()); if (isNaN(tax)) tax = 0;
            disc = parseFloat($('#<%= txtDisc.ClientID %>').val()); if (isNaN(disc)) disc = 0;

            //LESS ON GST/////////////////////////////////////////////////////
           var amount = calculateAmount();
            if (isNaN(amount)) amount = 1;
           var gst = calculateGSTAmount();
            if (isNaN(gst)) gst = 0;
            var txtLessOnGST = parseFloat(disc * gst / amount).toFixed(2);
            if (isNaN(txtLessOnGST)) txtLessOnGST = 0;
            $('#<%= txtLessOnGST.ClientID %>').val(txtLessOnGST);
            var lessOnGST = parseFloat($('#<%= txtLessOnGST.ClientID %>').val()); if (isNaN(lessOnGST)) lessOnGST = 0;
            //END OF LESS ON GST/////////////////////////////////////////////////////

            frt = parseFloat($('#<%= txtFrt.ClientID %>').val()); if (isNaN(frt)) frt = 0;
            impfrt = parseFloat($('#<%= txtImpFrt.ClientID %>').val()); if (isNaN(impfrt)) impfrt = 0;
            otrcost = parseFloat($('#<%= txtOtrCost.ClientID %>').val()); if (isNaN(otrcost)) otrcost = 0;
            adv = parseFloat($('#<%= txtAdv.ClientID %>').val()); if (isNaN(adv)) adv = 0;
            
            return parseFloat(grAmount + tax - disc + frt + impfrt + otrcost - adv - lessOnGST).toFixed(2);
        }
        function calculatePopupAmount() {
            var totalvat = 0;
            $("[id*=popupGrd]input[type=text][id*=txtAmount]").each(function() {
                temp = parseFloat($(this).val());
                if (isNaN(temp)) temp = 0;
                totalvat = totalvat + temp;
            });
            return totalvat;
        }
        //<%--&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&--%>
        $("[id*=popupGrd]input[type=text][id*=txtAmount]").keydown(function(event) {
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }
        });
        $("[id*=popupGrd]input[type=text][id*=txtAmount]").keyup(function (event) {
            
            var totalAmount = calculatePopupAmount();
            $("span[id*=popuplblAmountTotal]").text(totalAmount);
            $('#<%= txtOtrCost.ClientID %>').val(totalAmount);
            var totalDue = calculateDueAmount();
            $('#<%= txtDue.ClientID %>').val(totalDue);
        });

        $("[id*=popupGrd]input[type=text][id*=txtAmount]").change(function(event) {
            var totalAmount = calculatePopupAmount();
            $("span[id*=popuplblAmountTotal]").text(totalAmount);
            $('#<%= txtOtrCost.ClientID %>').val(totalAmount);
            var totalDue = calculateDueAmount();
            $('#<%= txtDue.ClientID %>').val(totalDue);
        });
        //<%--&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&--%>
        $("[id*=GridView1][id*=ddlGST]").change(function (event) {

            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            var gstId = $(this).val();
            var gRow = $(this);
            var gst = 0;
            if (gstId != '0' && gstId != 'VAR') {
                $.ajax({
                    url: "InvMPNMgt.aspx/GetGSTPercent",
                    data: JSON.stringify({ gstId: gstId }),
                    type: 'POST',
                    contentType: 'application/json;',
                    dataType: 'json',
                    success: function (heads) {
                        var heads = heads.d;
                        if (heads.length > 0) {
                            var amnt = gRow.closest("tr").find("input[type=text][id*=txtAmount]").val();
                            gRow.closest("tr").find("input[type=text][id*=txtGSTAmount]").val((heads[0].TaxRate * amnt / 100).toFixed(0));

                            var totalAmount = calculateAmount();
                            $("span[id*=lblAmountTotal]").text(totalAmount);
                            var totalCDAmount = calculateCustomDuty();
                            $("span[id*=lblCustomDutyTotal]").text(totalCDAmount);
                            var totalGstAmount = calculateGSTAmount();
                            $("span[id*=lblGSTTotal]").text(totalGstAmount);
                            $('#<%= txtGrAmount.ClientID %>').val(parseFloat(parseFloat(totalAmount) + parseFloat(totalGstAmount) + parseFloat(totalCDAmount)).toFixed(2));

                            var totalDue = calculateDueAmount();
                            $('#<%= txtDue.ClientID %>').val(totalDue);

                            var totalCostAmount = calculatePopupAmount();
                            $("span[id*=popuplblAmountTotal]").text(totalCostAmount);
                        }
                        else {
                            gRow.closest("tr").find("input[type=text][id*=txtGSTAmount]").val('0');
                        }
                    }
                });
            }
            else {
                if (gstId != 'VAR')
                    gRow.closest("tr").find("input[type=text][id*=txtGSTAmount]").val('0');

                var totalAmount = calculateAmount();
                $("span[id*=lblAmountTotal]").text(totalAmount);
                var totalCDAmount = calculateCustomDuty();
                $("span[id*=lblCustomDutyTotal]").text(totalCDAmount);
                var totalGstAmount = calculateGSTAmount();
                $("span[id*=lblGSTTotal]").text(totalGstAmount);
                $('#<%= txtGrAmount.ClientID %>').val(parseFloat(parseFloat(totalAmount) + parseFloat(totalGstAmount) + parseFloat(totalCDAmount)).toFixed(2));
                
                var totalDue = calculateDueAmount();
                $('#<%= txtDue.ClientID %>').val(totalDue);

                var totalCostAmount = calculatePopupAmount();
                $("span[id*=popuplblAmountTotal]").text(totalCostAmount);
            }
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

        });
        //<%--&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&--%>
    }
</script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
         
        <%--<asp:UpdatePanel ID="upnlIGPs" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
        <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List" ValidationGroup="main"/>
        <uc1:Messages ID="uMsg" runat="server" />

        <asp:Panel ID="pnlSrchIGPs" runat="server">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="1%">
              </td>
              <td>
                <asp:Label ID="lblSelectVendor" runat="server" Text="Select Vendor:" Width="100px">
                </asp:Label>
                &nbsp;
                <asp:DropDownList ID="ddlSelectVendor" runat="server" DataMember="LocId">
                </asp:DropDownList>
                <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlSelectVendor"
                PromptText="Search Vendor" PromptPosition="Top" QueryPattern="Contains" PromptCssClass="ListSearchExtenderPrompt">
                </ajaxToolkit:ListSearchExtender>
                &nbsp;
                <asp:LinkButton ID="lnkSelectVendor" runat="server" Text="Select" CssClass="lnk" ToolTip="Select vendor" OnClick="LnkSelectVendor_Click">
                </asp:LinkButton>
              </td>
              <td width="1%">
              </td>
             </tr>
            </table>
          </asp:Panel>
      
          <asp:Panel ID="pnlGetIGPs" runat="server">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="1%">
                </td>
                <td>           
                   <asp:GridView ID="grdIGPs" runat="server" DataKeyNames="LocId"
                                 AutoGenerateColumns="False"
                                 Width="100%" ShowFooter="true" FooterStyle-HorizontalAlign="Center" OnRowDataBound="GrdIGPs_OnRowDataBound">
                    <HeaderStyle CssClass ="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow"  />
                    <SelectedRowStyle CssClass="gridSelectedRow" />        
                    <Columns>
                        <asp:BoundField DataField="Sr" HeaderText="Sr." ControlStyle-Width="15px" HeaderStyle-Width="15px"  />
                        <asp:BoundField DataField="IGPNo" HeaderText="IGP No"  ControlStyle-Width="50px" HeaderStyle-Width="50px"  />
                        <asp:BoundField DataField="PO_Ref_Formatted" HeaderText="PO No"  ControlStyle-Width="50px" HeaderStyle-Width="50px"  />
                        <asp:BoundField DataField="IGPDate" HeaderText="IGP Date"  ControlStyle-Width="80px" HeaderStyle-Width="80px"  />
                        <asp:BoundField DataField="Party" HeaderText="Vendor"  ControlStyle-Width="230px" HeaderStyle-Width="230px"  />
                        
                        <asp:TemplateField HeaderText="Select" ControlStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" >
                        <ItemTemplate>
                        <asp:CheckBox ID="cbxSelectIGP" runat="server" ToolTip="Select to prepare MPN." />
                        </ItemTemplate>
                        <FooterTemplate>
                        <asp:LinkButton ID="lnkMergIgps" runat="server" Text="Prepare" OnClick="LnkMergIgps_Click" ToolTip="Prepare MPN." Font-Bold="true" Font-Underline="false" CssClass="lnk">
                        </asp:LinkButton>
                        </FooterTemplate>
                        </asp:TemplateField>                             
                        
                        <asp:BoundField DataField="matrec_id" HeaderText="matrec_id" ControlStyle-CssClass="DisplayNone" ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" FooterStyle-CssClass="DisplayNone"/>
                    </Columns>        
                   </asp:GridView>  
              </td>
              <td width="1%">
              </td>
            </tr>
          </table>
          </asp:Panel>
        <%--</ContentTemplate>
        <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnSave" />
        <asp:AsyncPostBackTrigger ControlID="btnClear" />
        <asp:AsyncPostBackTrigger ControlID="grdIGPs" />
        <asp:AsyncPostBackTrigger  ControlID="grdMPN"/>
        </Triggers>
        </asp:UpdatePanel>--%>
      
        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
         <ContentTemplate>--%>
          <asp:Panel ID="pnlFields" runat="server">
           <table width="100%" border="0" cellspacing="0" cellpadding="0" visible="false">
             <tr>
             <td width="1%">
             </td>
             <td> 
              <table cellspacing="2" class="stats2" align="center" border="0" width="100%">
               <tr>
                <td  class="LblBgSetup">
                   <asp:Label ID="lblShowLoc" runat="server" Text="Location:">
                   </asp:Label>
                </td>
                <td>
                   <asp:DropDownList ID="ddlShowLoc" runat="server" AppendDataBoundItems="true" Enabled="false">
                   </asp:DropDownList>
                </td>
               </tr>
               <tr>
                <td  class="LblBgSetup">
                 <asp:Label ID="lblShowVendor" runat="server" Text="Vendor:">
                 </asp:Label>
                </td>
                <td>
                 <asp:DropDownList ID="ddlShowVendor" runat="server" AppendDataBoundItems="true" Enabled="false">
                 </asp:DropDownList>
                </td>
               </tr>
               <tr>
                <td  class="LblBgSetup">
                 <asp:Label ID="lblCity" runat="server" Text="City:">
                 </asp:Label>  
                </td>
                <td>
                 <asp:DropDownList ID="ddlShowCity" runat="server" AppendDataBoundItems="true" Enabled="false">
                 </asp:DropDownList>
                </td>
               </tr>
               <tr>
                <td  class="LblBgSetup">
                 <asp:Label ID="Label13" runat="server" Text="PO Type:">
                 </asp:Label>  
                </td>
                <td>
                 <asp:TextBox ID="txtPOType" runat="server"></asp:TextBox>
                </td>
               </tr>
              </table>
             </td>
             <td>
              <table  cellspacing="0" class="stats2" align="center" border="0" width="100%">
               <tr>
                <td  class="LblBgSetup">
                  <asp:Label ID="lblMPNNo" runat="server" Text="MPN No:"></asp:Label>
                 </td>
                 <td>
                  <asp:TextBox ID="txtMPNNo" runat="server" Width="80" Enabled="false">
                  </asp:TextBox>
                 </td>
                 <td   class="LblBgSetup">
                  <asp:Label ID="Label9" runat="server" Text="Status:"></asp:Label>
                 </td>
                 <td>
                  <asp:DropDownList ID="ddlStatus" runat="server" CssClass="RequiredFieldDropDown" Width="110px">
                   <asp:ListItem Text="Select Status" Value="0"></asp:ListItem>
                   <asp:ListItem Text="Approved" Value="A"></asp:ListItem>
                   <asp:ListItem Text="Pending" Value="P" Selected="True"></asp:ListItem>
                   <asp:ListItem Text="Cancelled" Value="C"></asp:ListItem>
                  </asp:DropDownList>
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlStatus"
                  ErrorMessage="Please select status" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                  InitialValue="0"></asp:RequiredFieldValidator> 
                 </td>
               </tr>
               <tr>
                 <td   class="LblBgSetup">
                  <asp:Label ID="Label4" runat="server" Text="MPN Date:"></asp:Label>
                 </td>
                 <td>
                  <asp:TextBox ID="txtMPNDate" runat="server" CssClass="RequiredFieldDate" Width="80px">
                  </asp:TextBox>
                  <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtMPNDate" PopupPosition="BottomRight" >
                  </ajaxToolkit:CalendarExtender>
                 </td>
                 <td  class="LblBgSetup">
                  <asp:Label ID="lblIGPDate" runat="server" Text="IGP Date:">
                  </asp:Label>
                 </td>
                 <td>
                  <asp:TextBox ID="txtIGPDate" runat="server" CssClass="RequiredFieldDate" Enabled="false">
                  </asp:TextBox>
                  <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtIGPDate" PopupPosition="BottomRight" >
                  </ajaxToolkit:CalendarExtender>
                 </td>
                </tr>
                <tr>
                 <td   class="LblBgSetup">
                  <asp:Label ID="Label11" runat="server" Text="Invoice #:"></asp:Label>
                 </td>
                 <td>
                  <asp:TextBox ID="txtInvNo" runat="server" MaxLength="20" Width="80px">
                  </asp:TextBox>
                 </td>
                 <td  class="LblBgSetup">
                  <asp:Label ID="Label12" runat="server" Text="Invoice Date:">
                  </asp:Label>
                 </td>
                 <td>
                  <asp:TextBox ID="txtInvDate" runat="server" Width="80px">
                  </asp:TextBox>
                  <ajaxToolkit:CalendarExtender ID="CalendarExtender7" runat="server" TargetControlID="txtInvDate" PopupPosition="BottomRight" >
                  </ajaxToolkit:CalendarExtender>
                 </td>
                </tr>
                <tr>
                 <td   class="LblBgSetup">
                  <asp:Label ID="lblDueDate" runat="server" Text="Due Date:"></asp:Label>
                 </td>
                 <td>
                  <asp:TextBox ID="txtDueDate" runat="server" style="background-color:IndianRed; width:80px;">
                  </asp:TextBox>
                  <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtDueDate" PopupPosition="BottomRight" >
                  </ajaxToolkit:CalendarExtender>
                 </td>
                 <td   class="LblBgSetup">
                  <asp:Label ID="Label10" runat="server" Text="Remarks:"></asp:Label>
                 </td>
                 <td>
                  <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Height="67" Width="182"
                     onkeyup="LimitText(this,100);" onblur="LimitText(this,100);" CssClass="txtRemz" ></asp:TextBox>
                 </td>
                </tr>
                <tr valign="top">
                 <td >
                  <asp:Label ID="Label25" runat="server" Visible="false" Text="Commission:"></asp:Label>
                 </td>
                 <td>
                  <asp:TextBox ID="txtCommission" runat="server" Visible="false" Width="74px" MaxLength="7">
                  </asp:TextBox>
                 </td>
                </tr>
            </table>
           </td>
           <td width="1%">
           </td>
          </tr>
         </table>
      </asp:Panel>
    <%--</ContentTemplate>
     <Triggers>
      <asp:AsyncPostBackTrigger ControlID="grdIGPs" />
      <asp:AsyncPostBackTrigger ControlID="btnSave" />
      <asp:AsyncPostBackTrigger  ControlID="grdMPN"/>
      <asp:AsyncPostBackTrigger ControlID="lnkSelectVendor" />
     </Triggers>
    </asp:UpdatePanel>--%>
      
      
    <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
     <ContentTemplate>--%>
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
       <tr>
        <td width="1%"></td>
        <td valign="top">
         <div style="float:left">
          <b>
           <asp:Label ID="lblIGPsDet" runat="server" Text="Details:" Font-Size="12px" Visible="false">
           </asp:Label>
          </b>
         </div>

         <div id="div1" runat="server" style="float:right; display:none;">
          <asp:LinkButton ID="lnkPurchCostSheet" runat="server" OnClick="lnkPurchCostSheet_Click" Text="" CssClass="lnk">
          </asp:LinkButton>
          <ajaxToolkit:ModalPopupExtender ID="MPE" runat="server"
            TargetControlID="lnkPurchCostSheet"
            PopupControlID="popupPnl"
            BackgroundCssClass="modalBackground"
            CancelControlID="btnBack" 
            DropShadow="false" 
            PopupDragHandleControlID="pnlDiv">      
          </ajaxToolkit:ModalPopupExtender>
          </div>
          <%-- *************************************************************************--%>
          <asp:Panel ID="popupPnl" runat="server" CssClass="pnlBackGround" style="width:820px; display:none;">
           <div>
             <table>
               <tr>
                <td width=".5%"></td>
                <td>
               <table class="table">
                <tr>
                 <td>
                  <asp:ValidationSummary ID="ValidationSummary1" CssClass="ErrorSummary" runat="server" DisplayMode="List" ValidationGroup="CS" />
                  <uc1:Messages ID="ucMsgCS" runat="server" />        
                  <div  id="pnlDiv" class="model_popup_panel_hdr" style="font-size:20px;background-color:#ffffff; color:#6c74ad">
                   <asp:Label ID="lblHdr" runat="server" Text="Purchase Cost Sheet"></asp:Label>
                  </div>
                  <div style="height:5px;">&nbsp;</div>
                 </td>
                </tr>
                <tr>
                 <td>
                  <asp:Panel ID="pnlCost" runat="server">
                   <asp:GridView ID="popupGrd" runat="server"  Width="100%" OnRowDataBound="popupGrd_RowDataBound" AutoGenerateColumns="false" ShowFooter="true" >
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
                      <asp:TemplateField HeaderText="ID">
                       
                       <ItemTemplate>
                        <asp:TextBox ID="txtCostId" runat="server" Text='<%#Eval("CostId")%>' ReadOnly="true" TabIndex="-1" Width="60px">
                        </asp:TextBox>                        
                       </ItemTemplate>
                       <ControlStyle />
                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                      </asp:TemplateField>
                                                    
                      <asp:TemplateField HeaderText="Temp ID">
                       <ItemTemplate>
                        <asp:TextBox ID="txtTempId" runat="server" Text='<%#Eval("TempId")%>' ReadOnly="true" TabIndex="-1" Width="60px">
                        </asp:TextBox>                        
                       </ItemTemplate>
                       <ControlStyle />
                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                      </asp:TemplateField>
                                                    
                      <asp:TemplateField HeaderText="Description">
                       <ItemTemplate>
                        <asp:TextBox ID="txtDesc" runat="server" Text='<%#Eval("Desc")%>' ReadOnly="true" TabIndex="-1" Width="150px">
                        </asp:TextBox>                        
                       </ItemTemplate>
                       <ControlStyle />
                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="150px"/>
                      </asp:TemplateField>
                                                         
                      <asp:TemplateField HeaderText="Date">
                       <ItemTemplate>
                        <asp:TextBox ID="txtDate" runat="server" Text='<%#Eval("Date")%>' Width="80px">
                        </asp:TextBox> 
                                      
                       </ItemTemplate>
                       <ControlStyle />
                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="80px"/>
                      </asp:TemplateField>
                                                  
                      <asp:TemplateField HeaderText="Doc Ref">
                       <ItemTemplate>
                        <asp:TextBox ID="txtDocRef" runat="server" Text='<%#Eval("DocRef")%>' MaxLength="9" Width="80px"/>
                        <ajaxToolkit:CalendarExtender ID="C2" runat="server" TargetControlID="txtDate" PopupPosition="BottomRight">
                        </ajaxToolkit:CalendarExtender>  
                       </ItemTemplate>
                       <FooterTemplate>
                        <asp:Label ID="lblTotal" runat="server" Text="Total"></asp:Label>
                       </FooterTemplate>
                       <ControlStyle />
                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                        <FooterStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="80px"/>
                      </asp:TemplateField>

                      <asp:TemplateField HeaderText="Amount*">
                       <ItemTemplate>
                        <asp:TextBox ID="txtAmount" runat="server" Text='<%#Eval("Amount")%>' style="text-align:right;" MaxLength="9" Width="80px"/>
                       </ItemTemplate>
                       <FooterTemplate>
                        <asp:Label ID="popuplblAmountTotal" runat="server" Text="0"></asp:Label>
                       </FooterTemplate>
                       <ControlStyle />
                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                        <FooterStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Right" Width="80px"/>
                      </asp:TemplateField>
                                                  
                      <asp:TemplateField HeaderText="Remarks">
                        <ItemTemplate>
                         <asp:TextBox ID="txtRem" runat="server" Text='<%#Eval("Remarks")%>' MaxLength="100" Width="160px"/>
                        </ItemTemplate>
                        <ControlStyle />
                         <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                         <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="160px"/>
                       </asp:TemplateField>
                       
                     
                       
                      </Columns>
                     </asp:GridView>
                    </asp:Panel>
                   </td>
                  </tr>
                 </table>
                </td>
                <td width=".5%"></td>
               </tr>
              </table>
             </div>
             <div style="height:5px;">&nbsp;</div>  
             <div style="margin-left:1%">
              <asp:ImageButton ID="btnSavePurchCostSheet" runat ="server"  OnClick="btnSavePurchCostSheet_Click" ImageUrl="~/images/btn_attach.png" onMouseOver="this.src='../images/btn_attach_m.png'" onMouseOut="this.src='../images/btn_attach.png'" ToolTip="Attach Cost Sheet Info"  ValidationGroup="CS" />
              <asp:ImageButton ID="btnBack" runat="server"  ImageUrl="~/images/btn_Back.png" onMouseOver="this.src='../images/btn_Back_m.png'" onMouseOut="this.src='../images/btn_Back.png'" />
             </div>                                                     
             <div style="height:5px;">&nbsp;</div>  
            </asp:Panel>
            <%-- *************************************************************************--%>
            <div id="divCostSht" runat="server" style="float:right; display:none;" >
             <asp:LinkButton ID="lnkCostSht" runat="server" Text="" CssClass="lnk" >
             </asp:LinkButton>
            </div>
           </td>
           <td>
           </td>
         </tr>
        <tr>
         <td width="1%">
         </td>
         <td>
          <asp:Panel ID="pnlIGPs" runat="server">
           <table class="table">
            <tr>
             <td>
              <asp:GridView ID="GridView1" runat="server" CssClass="t_grd" Width="100%" AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="GridView1_RowDataBound" >
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
                
                <asp:TemplateField HeaderText="Sr">
                 <ItemTemplate>
                  <asp:Label runat="server" ID="lblSr" Text='<%#Eval("Sr") %>' Width="20px">
                  </asp:Label>
                 </ItemTemplate>
                 <ControlStyle />
                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="20px"/>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="IGP No">
                 <ItemTemplate>
                  <asp:TextBox ID="txtIGP" runat="server" Text='<%#Eval("vr_no")%>' ReadOnly="true" TabIndex="-1" Width="60px">
                  </asp:TextBox>                        
                 </ItemTemplate>
                 <ControlStyle />
                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                </asp:TemplateField>
                           
                <asp:TemplateField HeaderText="PO Ref">
                 <ItemTemplate>
                  <asp:TextBox ID="txtPoRef" runat="server" Text='<%#Eval("PO_Ref")%>' ReadOnly="true" TabIndex="-1" Width="60px">
                  </asp:TextBox>                        
                 </ItemTemplate>
                 <ControlStyle />
                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                 </asp:TemplateField>
                           
                 <asp:TemplateField HeaderText="Date">
                  <ItemTemplate>
                   <asp:TextBox ID="txtDate" runat="server" Text='<%#Eval("Date")%>'  ReadOnly="true" TabIndex="-1" Width="80px">
                   </asp:TextBox>                        
                  </ItemTemplate>
                  <ControlStyle />
                   <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" CssClass="DisplayNone"/>
                   <FooterStyle VerticalAlign="Middle" HorizontalAlign="Center" CssClass="DisplayNone"/>
                   <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="80px" CssClass="DisplayNone"/>
                  </asp:TemplateField>
                           
                  <asp:TemplateField HeaderText="Item Code">
                   <ItemTemplate>
                    <asp:TextBox ID="txtItemCode" runat="server" Text='<%#Eval("ItmCd")%>' ReadOnly="true" TabIndex="-1" Width="100px">
                    </asp:TextBox>                        
                   </ItemTemplate>
                   <ControlStyle />
                    <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" CssClass="DisplayNone"/>
                    <FooterStyle VerticalAlign="Middle" HorizontalAlign="Center" CssClass="DisplayNone"/>
                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="80px" CssClass="DisplayNone"/>
                   </asp:TemplateField>
                            
                   <asp:TemplateField HeaderText="Item">
                    <ItemTemplate>
                     <asp:TextBox ID="txtItemDsc" runat="server" Text='<%#Eval("ItmDsc")%>' ReadOnly="true" TabIndex="-1" Width="170px">
                     </asp:TextBox>                        
                    </ItemTemplate>
                    <ControlStyle />
                     <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                     <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="170px"/>
                   </asp:TemplateField>
                            
                   <asp:TemplateField HeaderText="UOM">
                    <ItemTemplate>
                     <asp:DropDownList ID="ddlUOM" runat="server" AppendDataBoundItems="true" TabIndex="-1" Width="60px">
                     </asp:DropDownList>
                    </ItemTemplate>
                    <ControlStyle />
                     <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                     <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                   </asp:TemplateField>
                            
                   <asp:TemplateField HeaderText="IGP Qty">
                    <ItemTemplate>
                     <asp:TextBox ID="txtIgpQty" runat="server" Text='<%#Eval("IGPQty")%>' ReadOnly="true" TabIndex="-1" Width="60px" style="text-align:right;">
                     </asp:TextBox>                        
                    </ItemTemplate>
                    <ControlStyle />
                     <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                     <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                  </asp:TemplateField>
                            
                  <asp:TemplateField HeaderText="Received Qty">
                   <ItemTemplate>
                    <asp:TextBox ID="txtRecQty" runat="server" Text='<%#Eval("RecQty")%>' ReadOnly="true" TabIndex="-1" Width="60px" style="text-align:right;">
                    </asp:TextBox>                        
                   </ItemTemplate>
                   <FooterTemplate>
                    <asp:Label ID="lblTotal" runat="server" Text="Total"></asp:Label>
                   </FooterTemplate>
                   <ControlStyle />
                    <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                    <FooterStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                  </asp:TemplateField>
                            
                  <asp:TemplateField HeaderText="Amount">
                   <ItemTemplate>
                    <asp:TextBox ID="txtAmount" runat="server" Text='<%#Eval("Amount")%>' Width="80px" style="text-align:right;" MaxLength="9" />
                    <asp:RangeValidator runat="server" ID="rngQty" ValidationGroup="main" ErrorMessage="Invalid/Out of range Qty" SetFocusOnError="true" CssClass="validateGridView"
                        ControlToValidate="txtAmount" Display="None" MinimumValue="000000000.00" MaximumValue="999999999.99" Type="Double" ></asp:RangeValidator>
                  </ItemTemplate>
                  <FooterTemplate>
                   <asp:Label ID="lblAmountTotal" runat="server" Text="0"></asp:Label>
                  </FooterTemplate>
                  <ControlStyle />
                   <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                   <FooterStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                   <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="80px"/>
                  </asp:TemplateField>
                            
                  <asp:TemplateField HeaderText="GST">
                   <ItemTemplate>
                    <asp:DropDownList ID="ddlGST" runat="server" AppendDataBoundItems="true" >
                        <asp:ListItem Selected="True" Value="0">No GST</asp:ListItem>
                     </asp:DropDownList>
                   </ItemTemplate>
                   <ControlStyle />
                    <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Right" Width="60px"/>
                  </asp:TemplateField>
                            
                  <asp:TemplateField HeaderText="GST Amount">
                   <ItemTemplate>
                     <asp:TextBox ID="txtGSTAmount" runat="server" Text='<%#Eval("GSTAmount")%>' style="text-align:right;" Width="70px" TabIndex="-1"/>
                   </ItemTemplate>
                   <FooterTemplate>
                    <asp:Label ID="lblGSTTotal" runat="server" Text="0"></asp:Label>
                   </FooterTemplate>
                   <ControlStyle />
                    <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                    <FooterStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Right" Width="70px"/>
                  </asp:TemplateField>
                  
                  <asp:TemplateField HeaderText="Custom Duty">
                   <ItemTemplate>
                    <asp:TextBox ID="txtCustomDuty" runat="server" Text='<%#Eval("CustomDuty")%>' Width="80px" style="text-align:right;" MaxLength="9" />
                    <asp:RangeValidator runat="server" ID="rngCustomDuty" ValidationGroup="main" ErrorMessage="Invalid/Out of range Custom Duty" SetFocusOnError="true" CssClass="validateGridView"
                        ControlToValidate="txtCustomDuty" Display="None" MinimumValue="000000000" MaximumValue="999999999" Type="Double" ></asp:RangeValidator>
                  </ItemTemplate>
                  <FooterTemplate>
                   <asp:Label ID="lblCustomDutyTotal" runat="server" Text="0"></asp:Label>
                  </FooterTemplate>
                  <ControlStyle />
                   <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                   <FooterStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                   <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="80px"/>
                  </asp:TemplateField>
                          
                    <asp:TemplateField HeaderText="matrec_id">
                        <ItemTemplate>
                         <asp:TextBox ID="txtMatRec_Id" runat="server" Text='<%#Eval("matrec_id")%>' />
                        </ItemTemplate>
                        <ControlStyle />
                         <HeaderStyle CssClass="DisplayNone" />
                         <ItemStyle CssClass="DisplayNone"/>
                         <FooterStyle CssClass="DisplayNone"/>
                    </asp:TemplateField>
                               
                  </Columns>
                 </asp:GridView>
                </td>
               </tr>
              </table>
             </asp:Panel>
            </td>
           <td width="1%">
           </td>
          </tr>
         </table>
        <%--</ContentTemplate>
         <Triggers>
          <asp:AsyncPostBackTrigger ControlID="grdIGPs" />
          <asp:AsyncPostBackTrigger ControlID="btnSave" />
          <asp:AsyncPostBackTrigger  ControlID="grdMPN"/>
          <asp:AsyncPostBackTrigger ControlID="lnkSelectVendor" />
         </Triggers>
      </asp:UpdatePanel>--%>
      
     <%--<asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
      <ContentTemplate>--%>
       <asp:Panel ID="pnlFieldsTotal" runat="server" Visible ="false">
       <table width="30%"><tr><td width="1%"> 
        <table width="100%">
         <tr>
          <td>
           <asp:Label ID="Label1" runat="server" Text="Gross Amount:" Width="100px"></asp:Label>
          </td>
          <td>                         
           <asp:TextBox ID="txtGrAmount" runat="server" style="text-align:right;" ReadOnly="true" TabIndex="-1" Width="120px">
           </asp:TextBox>
          </td>
        </tr>
        <tr>
           <td>
            <asp:Label ID="Label2" runat="server" Text="Discount:" Width="100px"></asp:Label>
           </td>
           <td>                         
            <asp:TextBox ID="txtDisc" runat="server" ReadOnly="true" TabIndex="-1" style="text-align:right;" Width="120px">
            </asp:TextBox>
            <asp:RangeValidator runat="server" ID="rngDisc" ValidationGroup="main" ErrorMessage="Invalid/Out of range Discount" SetFocusOnError="true" CssClass="validateGridView"
                                                        ControlToValidate="txtDisc" Display="None" MinimumValue="000000000" MaximumValue="999999999" Type="Integer" ></asp:RangeValidator>
           </td>
        </tr>
        <tr>
           <td>
            <asp:Label ID="Label16" runat="server" Text="Less On GST:" Width="100px"></asp:Label>
           </td>
           <td>                         
            <asp:TextBox ID="txtLessOnGST" runat="server" style="text-align:right;" ReadOnly="true" TabIndex="-1"  Width="120px">
            </asp:TextBox>
            </td>
        </tr>
        <tr>
           <td>
            <asp:Label ID="Label5" runat="server" Text="Tax Amount:" Width="100px"></asp:Label>
           </td>
           <td>                         
            <asp:TextBox ID="txtWHT" runat="server" style="text-align:right;" ReadOnly="true" TabIndex="-1" Width="120px">
            </asp:TextBox>
           </td>
        </tr>
        <tr>
           <td>
            <asp:Label ID="Label3" runat="server" Text="Local Freight:" Width="100px"></asp:Label>
           </td>
           <td>                         
            <asp:TextBox ID="txtFrt" runat="server" style="text-align:right;" Width="120px">
            </asp:TextBox>
           </td>
        </tr>
        <tr>
           <td>
            <asp:Label ID="Label14" runat="server" Text="Freight On Imp.:" Width="100px"></asp:Label>
           </td>
           <td>                         
            <div>
                <asp:TextBox ID="txtImpFrt" runat="server" Text="0" style="text-align:right;" Width="120px" Enabled="false">
                </asp:TextBox>
            </div>
            <div style="float:left">
              <asp:Label ID="Label15" runat="server" Text="Forwarder:"></asp:Label>
              <asp:DropDownList ID="ddlForwarder" runat="server" AppendDataBoundItems="true" Enabled="false">
                <asp:ListItem Selected="True" Value="0">Select Forwarder</asp:ListItem>
              </asp:DropDownList>
            </div>
           </td>
        </tr>
        <asp:Panel ID="pnlHide" runat="server" Visible="false">
        <tr>
           <td>
            <asp:Label ID="Label6" runat="server" Text="Other Cost:" Width="100px"></asp:Label>
           </td>
           <td>                         
            <asp:TextBox ID="txtOtrCost" runat="server" style="text-align:right;" TabIndex="-1" Width="120px">
            </asp:TextBox>
           </td>
        </tr>
        </asp:Panel>
        <tr class="DisplayNone">
           <td>
            <asp:Label ID="Label7" runat="server" Text="Advance:" Width="100px"></asp:Label>
           </td>
           <td>                         
            <asp:TextBox ID="txtAdv" runat="server" style="text-align:right;" Width="120px">
            </asp:TextBox>
            <asp:RangeValidator runat="server" ID="rngAdv" ValidationGroup="main" ErrorMessage="Invalid/Out of range Advance" SetFocusOnError="true" CssClass="validateGridView"
                                                        ControlToValidate="txtAdv" Display="None" MinimumValue="000000000" MaximumValue="999999999" Type="Integer" ></asp:RangeValidator>
           </td>
        </tr>
        <tr>
           <td>
            <b><asp:Label ID="Label8" runat="server" Text="Total Due:" Width="100px"></asp:Label></b>
           </td>
           <td>                         
            <asp:TextBox ID="txtDue" runat="server" style="text-align:right;" TabIndex="-1" Width="120px">
            </asp:TextBox>
           </td>
        </tr>
      </table>
      </td></tr></table>
      </asp:Panel>
     <%-- </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="grdIGPs" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" />
             <asp:AsyncPostBackTrigger  ControlID="grdMPN"/>
             <asp:AsyncPostBackTrigger ControlID="lnkSelectVendor" />
            </Triggers>
        </asp:UpdatePanel>--%>
      
      
      
       <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
      <ContentTemplate>--%>
  
              <table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
              <td width="1%">
              </td>
              <td> 
                    <asp:ImageButton ID="btnSave" runat="server"  ImageUrl="~/images/btn_save.png" OnClick="btnSave_Click"
                        onMouseOver="this.src='../images/btn_save_m.png'" onMouseOut="this.src='../images/btn_save.png'" ValidationGroup="main" />
                    <asp:ImageButton ID="btnClear" runat="server"  ImageUrl="~/images/btn_clear.png" OnClick="btnClear_Click"
                        onMouseOver="this.src='../images/btn_clear_m.png'" onMouseOut="this.src='../images/btn_clear.png'" />
             </td>
              <td width="1%">
              </td>
              </tr>
              </table>

      <%--</ContentTemplate>
        <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lnkSelectVendor" />
        <asp:AsyncPostBackTrigger ControlID="grdIGPs" />
        <asp:AsyncPostBackTrigger ControlID="grdMPN" />
        </Triggers>
      </asp:UpdatePanel>--%>
    
       
       <%--<asp:UpdatePanel ID="mpnGrid" runat="server" UpdateMode="Conditional">
       <ContentTemplate>--%>
  
       
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
        
        
        
        <tr>
      <td width="1%">
      </td>
          <td>
                
                  <table class="filterTable" cellpadding="1" cellspacing="2" width="100%"><tr><td colspan="9">&nbsp;</td></tr>
                  <tr>
                    <td>&nbsp;</td>
                    <td>Party:</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtFltParty" Width="250px" MaxLength="50"></asp:TextBox>
                        
                    </td>
                    <td>From:</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtFltFrmDt" Width="80px" ></asp:TextBox>
                         <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtFltFrmDt" PopupPosition="BottomRight"  EnableViewState="false">
                                          </ajaxToolkit:CalendarExtender>
                    </td>
                    <td>To:</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtFltToDt" Width="80px"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender5"  runat="server" TargetControlID="txtFltToDt" PopupPosition="BottomRight"  EnableViewState="false">
                                          </ajaxToolkit:CalendarExtender>
                    </td>
                    <td>Status:</td>
                    <td>
                        <asp:DropDownList ID="ddlFltType" runat="server">
                            <asp:ListItem Value="M">All
                            </asp:ListItem>
                            <asp:ListItem Text="Approved" Value="A">
                            </asp:ListItem>
                            <asp:ListItem Text="Pending" Value="P">
                            </asp:ListItem>
                            <asp:ListItem Text="Cancelled" Value="C">
                            </asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                              OnClick="btnsearch_Click" ToolTip="Search Code" Visible="true"/>
                     </td>
                    <td>&nbsp;</td>
                </tr>
                <tr><td colspan="9">&nbsp;</td></tr></table>
                
                
          </td>
      <td width="1%">
      </td>
      </tr>
        
        
        
        
        
        <tr>
          <td width="1%">
          </td>
              <td>
                    <asp:GridView ID="grdMPN" runat="server" DataKeyNames="LocId" OnSelectedIndexChanged="grdMPN_SelectedIndexChanged" OnRowDataBound="grdMPN_RowDataBound"
                        AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No record found." Width="100%" PageSize="25" OnPageIndexChanging="grdMPN_PageIndexChanging" >
                        <HeaderStyle CssClass ="grid_hdr" />
                        <RowStyle CssClass="grid_row" />
                        <AlternatingRowStyle CssClass="gridAlternateRow" />
                        <SelectedRowStyle CssClass="gridSelectedRow" />
                        <Columns>
                            <asp:BoundField DataField="mpnNo" HeaderText="MPN No"  />
                            <asp:BoundField DataField="mpnDate" HeaderText="MPN Date" />
                            <asp:BoundField DataField="igpNo" HeaderText="IGP No" />
                            <asp:BoundField DataField="igpDate" HeaderText="IGP Date" />
                            <asp:BoundField DataField="LocName" HeaderText="Location" />
                            <asp:BoundField DataField="party" HeaderText="Party" />
                            <asp:BoundField DataField="status" HeaderText="Status" />
                            <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle />
                                <ControlStyle CssClass="lnk"></ControlStyle>
                            </asp:CommandField>
                        </Columns>
                    </asp:GridView>
              </td>
          <td width="1%">
          </td>
        </tr>
        </table>
         
         
              
       <%--</ContentTemplate>
       <Triggers>
       <asp:AsyncPostBackTrigger ControlID="btnSave" />
       </Triggers>
       </asp:UpdatePanel>--%>

</asp:Content>
