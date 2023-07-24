<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmpMgtSalPayment.aspx.cs" Inherits="RMS.Profile.EmpMgtSalPayment" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script type="text/javascript">
    
    function pageLoad() {

        $('#<%= ddlPayType.ClientID %>').change(function(event) { 
        
        if ($('#<%= ddlPayType.ClientID %>').val() == 'All' || $('#<%= ddlPayType.ClientID %>').val() == 'Cash') {
            $('#<%= ddlBank.ClientID %>').prop('selectedIndex', 0); 
        }
        else {
            $('#<%= ddlBank.ClientID %>').prop('selectedIndex', 1); 
        }
    });

    $('#<%= ddlBank.ClientID %>').change(function(event) {

        if ($('#<%= ddlBank.ClientID %>').val() == 'All') {
            $('#<%= ddlPayType.ClientID %>').prop('selectedIndex', 0);
        }
        else {
            $('#<%= ddlPayType.ClientID %>').prop('selectedIndex', 1);
        }
    });



    $('#<%= txtChqAmnt.ClientID %>').attr('readonly', true);
    $('#<%= txtTaxDed.ClientID %>').attr('readonly', true);
    $('#<%= txtLoanAdvDed.ClientID %>').attr('readonly', true);
    $('#<%= txtEobiDed.ClientID %>').attr('readonly', true);
    $('#<%= txtOtherDed.ClientID %>').attr('readonly', true);
    $('#<%= txtMiscDed.ClientID %>').attr('readonly', true);

        $('#<%= btnSearch.ClientID %>').click(function(event) {
            $('#<%= txtChqAmnt.ClientID %>').val('0');
            $('#<%= txtTaxDed.ClientID %>').val('0');
            $('#<%= txtEobiDed.ClientID %>').val('0');
            $('#<%= txtLoanAdvDed.ClientID %>').val('0');
            $('#<%= txtOtherDed.ClientID %>').val('0');
            $('#<%= txtMiscDed.ClientID %>').val('0');
        });

        $("#<%=grdSalPay.ClientID%> input[id*='chkIncludeAll']:checkbox").click(function() {
            if ($(this).is(':checked')) {
                //alert("checked");
                //$("#<%=grdSalPay.ClientID%> input[id*='chkIncludeInVoucher']:checkbox").attr('Checked', true);
                $("#<%=grdSalPay.ClientID%> input[id*='chkIncludeInVoucher']:checkbox").each(function() { this.checked = true; });
                
                /////////////////////////////////////////////////////////////////
                var sal = 0;
                $("#<%=grdSalPay.ClientID%> tr td:nth-child(13)").each(function() {
                    var val = $(this).text();
                    sal = sal + parseInt(val);
                });
                $('#<%= txtChqAmnt.ClientID %>').val(sal);
                /////////////////////////////////////////////////////////////////
                var otrded = 0;
                $("#<%=grdSalPay.ClientID%> tr td:nth-child(11)").each(function () {
                    var val = $(this).text();
                    otrded = otrded + parseInt(val);
                });
                $('#<%= txtOtherDed.ClientID %>').val(otrded);
                /////////////////////////////////////////////////////////////////
                var miscded = 0;
                $("#<%=grdSalPay.ClientID%> tr td:nth-child(12)").each(function () {
                    var val = $(this).text();
                    miscded = miscded + parseInt(val);
                });
                $('#<%= txtMiscDed.ClientID %>').val(miscded);
                /////////////////////////////////////////////////////////////////
                var eobi = 0;
                $("#<%=grdSalPay.ClientID%> tr td:nth-child(10)").each(function() {
                    var val = $(this).text();
                    eobi = eobi + parseInt(val);
                });
                $('#<%= txtEobiDed.ClientID %>').val(eobi);
                /////////////////////////////////////////////////////////////////
                var ld = 0;
                $("#<%=grdSalPay.ClientID%> tr td:nth-child(9)").each(function() {
                    var val = $(this).text();
                    ld = ld + parseInt(val);
                });
                $('#<%= txtLoanAdvDed.ClientID %>').val(ld);
                /////////////////////////////////////////////////////////////////
                var tax = 0;
                $("#<%=grdSalPay.ClientID%> tr td:nth-child(8)").each(function() {
                    var val = $(this).text();
                    tax = tax + parseInt(val);
                });
                $('#<%= txtTaxDed.ClientID %>').val(tax);
                /////////////////////////////////////////////////////////////////
            }
            else {
                //alert("checked");
                //$("#<%=grdSalPay.ClientID%> input[id*='chkIncludeInVoucher']:checkbox").attr('Checked', false);
                $("#<%=grdSalPay.ClientID%> input[id*='chkIncludeInVoucher']:checkbox").each(function() { this.checked = false; });
                $('#<%= txtChqAmnt.ClientID %>').val(0);
                $('#<%= txtTaxDed.ClientID %>').val('0');
                $('#<%= txtLoanAdvDed.ClientID %>').val('0');
                $('#<%= txtEobiDed.ClientID %>').val('0');
                $('#<%= txtOtherDed.ClientID %>').val('0');
                $('#<%= txtMiscDed.ClientID %>').val('0');
            }
        });


        $("#<%=grdSalPay.ClientID%> input[id*='chkIncludeInVoucher']:checkbox").click(function() {

            if ($(this).is(':checked')) {
                /////////////////////////////////////////////////////////////////
                var sal = parseInt($(this).closest('tr').find('td:nth-child(13)').text());
                var txtVal = $('#<%= txtChqAmnt.ClientID %>').val();
                if (txtVal != '') {
                    sal = parseInt(txtVal) + sal;
                }
                $('#<%= txtChqAmnt.ClientID %>').val(sal);
                /////////////////////////////////////////////////////////////////
                var otrded = parseInt($(this).closest('tr').find('td:nth-child(11)').text());
                var txtVal = $('#<%= txtOtherDed.ClientID %>').val();
                if (txtVal != '') {
                    otrded = parseInt(txtVal) + otrded;
                }
                $('#<%= txtOtherDed.ClientID %>').val(otrded);
                /////////////////////////////////////////////////////////////////
                var miscded = parseInt($(this).closest('tr').find('td:nth-child(12)').text());
                var txtVal = $('#<%= txtMiscDed.ClientID %>').val();
                if (txtVal != '') {
                    miscded = parseInt(txtVal) + miscded;
                }
                $('#<%= txtMiscDed.ClientID %>').val(miscded);
                /////////////////////////////////////////////////////////////////
                var eobi = parseInt($(this).closest('tr').find('td:nth-child(10)').text());
                var txtEobiVal = $('#<%= txtEobiDed.ClientID %>').val();
                if (txtEobiVal != '') {
                    eobi = parseInt(txtEobiVal) + eobi;
                }
                $('#<%= txtEobiDed.ClientID %>').val(eobi);
                /////////////////////////////////////////////////////////////////
                var ld = parseInt($(this).closest('tr').find('td:nth-child(9)').text());
                var txtValLd = $('#<%= txtLoanAdvDed.ClientID %>').val();
                if (txtValLd != '') {
                    ld = parseInt(txtValLd) + ld;
                }
                $('#<%= txtLoanAdvDed.ClientID %>').val(ld);
                /////////////////////////////////////////////////////////////////
                var tax = parseInt($(this).closest('tr').find('td:nth-child(8)').text());
                var txtValTax = $('#<%= txtTaxDed.ClientID %>').val();
                if (txtValTax != '') {
                    tax = parseInt(txtValTax) + tax;
                }
                $('#<%= txtTaxDed.ClientID %>').val(tax);
                /////////////////////////////////////////////////////////////////
            }
            else {
                /////////////////////////////////////////////////////////////////
                var sal = parseInt($(this).closest('tr').find('td:nth-child(13)').text());
                var txtVal = $('#<%= txtChqAmnt.ClientID %>').val();
                if (txtVal != '') {
                    sal = parseInt(txtVal) - sal;
                }
                $('#<%= txtChqAmnt.ClientID %>').val(sal);
                /////////////////////////////////////////////////////////////////
                var otrded = parseInt($(this).closest('tr').find('td:nth-child(11)').text());
                var txtVal = $('#<%= txtOtherDed.ClientID %>').val();
                if (txtVal != '') {
                    otrded = parseInt(txtVal) - otrded;
                }
                $('#<%= txtOtherDed.ClientID %>').val(otrded);
                /////////////////////////////////////////////////////////////////
                var miscded = parseInt($(this).closest('tr').find('td:nth-child(12)').text());
                var txtVal = $('#<%= txtMiscDed.ClientID %>').val();
                if (txtVal != '') {
                    miscded = parseInt(txtVal) - miscded;
                }
                $('#<%= txtMiscDed.ClientID %>').val(miscded);
                /////////////////////////////////////////////////////////////////
                var eobi = parseInt($(this).closest('tr').find('td:nth-child(10)').text());
                var txtEobiVal = $('#<%= txtEobiDed.ClientID %>').val();
                if (txtEobiVal != '') {
                    eobi = parseInt(txtEobiVal) - eobi;
                }
                $('#<%= txtEobiDed.ClientID %>').val(eobi);
                /////////////////////////////////////////////////////////////////
                var ld = parseInt($(this).closest('tr').find('td:nth-child(9)').text());
                var txtValLd = $('#<%= txtLoanAdvDed.ClientID %>').val();
                if (txtValLd != '') {
                    ld = parseInt(txtValLd) - ld;
                }
                $('#<%= txtLoanAdvDed.ClientID %>').val(ld);
                /////////////////////////////////////////////////////////////////
                var tax = parseInt($(this).closest('tr').find('td:nth-child(8)').text());
                var txtValTax = $('#<%= txtTaxDed.ClientID %>').val();
                if (txtValTax != '') {
                    tax = parseInt(txtValTax) - tax;
                }
                $('#<%= txtTaxDed.ClientID %>').val(tax);
                /////////////////////////////////////////////////////////////////
            }



            var allChecked = true;
            $("#<%=grdSalPay.ClientID%> input[id*='chkIncludeInVoucher']:checkbox").each(function() {
                if (!$(this).is(':checked')) {
                    allChecked = false;
                    return false;
                }
            });
            if (allChecked) {
                $("#<%=grdSalPay.ClientID%> input[id*='chkIncludeAll']:checkbox").attr('checked', true);

            }
            else {
                $("#<%=grdSalPay.ClientID%> input[id*='chkIncludeAll']:checkbox").attr('checked', false);
            }

        });


//        if ($('#<%= ddlAppStatus.ClientID %>').val() == 'A') {
//            $('#<%= divChq.ClientID %>').show();
//            ValidatorEnable($('[id*=RequiredFieldValidator8]')[0], true);
//            ValidatorEnable($('[id*=RequiredFieldValidator9]')[0], true);
//            ValidatorEnable($('[id*=RequiredFieldValidator10]')[0], true);
//        }


////        $('#<%= ddlAppStatus.ClientID %>').val('P');
////        $('#<%= divChq.ClientID %>').hide();
//        ValidatorEnable($('[id*=RequiredFieldValidator8]')[0], false);
//        ValidatorEnable($('[id*=RequiredFieldValidator9]')[0], false);
//        ValidatorEnable($('[id*=RequiredFieldValidator10]')[0], false);

//        $('#<%= ddlAppStatus.ClientID %>').change(function(event) {
//            if ($('#<%= ddlAppStatus.ClientID %>').val() == 'P') {
//                $('#<%= divChq.ClientID %>').hide();
//                ValidatorEnable($('[id*=RequiredFieldValidator8]')[0], false);
//                ValidatorEnable($('[id*=RequiredFieldValidator9]')[0], false);
//                ValidatorEnable($('[id*=RequiredFieldValidator10]')[0], false);

//            }
//            else {
//                $('#<%= divChq.ClientID %>').show();
//                ValidatorEnable($('[id*=RequiredFieldValidator8]')[0], true);
//                ValidatorEnable($('[id*=RequiredFieldValidator9]')[0], true);
//                ValidatorEnable($('[id*=RequiredFieldValidator10]')[0], true);

//            }
//        });



        $('#<%= txtMinSal.ClientID %>').keydown(function(event) {
            $(this).css("text-align", "right");
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }
        });
        $('#<%= txtMinSal.ClientID %>').css('text-align', 'right');
        
        $('#<%= txtMaxSal.ClientID %>').keydown(function(event) {
            $(this).css("text-align", "right");
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }
        });
        $('#<%= txtMaxSal.ClientID %>').css('text-align', 'right');
        
        $('#<%= txtChqAmnt.ClientID %>').keydown(function(event) {
            $(this).css("text-align", "right");
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }
        });
        $('#<%= txtChqAmnt.ClientID %>').css('text-align', 'right');
        $('#<%= txtTaxDed.ClientID %>').css('text-align', 'right');
        $('#<%= txtLoanAdvDed.ClientID %>').css('text-align', 'right');
        $('#<%= txtEobiDed.ClientID %>').css('text-align', 'right');
        $('#<%= txtOtherDed.ClientID %>').css('text-align', 'right');
        $('#<%= txtMiscDed.ClientID %>').css('text-align', 'right');

//        $('#<%= txtChqBranch.ClientID %>').click(function(event) {
//            this.select();
//        });

//        $('#<%= txtChqBranch.ClientID %>').autocomplete({

//            source: function(request, response) {
//                $.ajax({
//                    url: "EmpMgtSalPayment.aspx/GetBranch",
//                    data: "{ 'bank': '" + request.term + "' }",
//                    dataType: "json",
//                    type: "POST",
//                    contentType: "application/json; charset=utf-8",

//                    dataFilter: function(data) { return data; },
//                    success: function(data) {
//                        response($.map(data.d, function(item) {
//                            return {
//                                value: item.gl_cd + ' - ' + item.gl_dsc,
//                                result: item.STN,
//                                id: item.gl_cd
//                            }
//                        }))
//                    },
//                    error: function(XMLHttpRequest, textStatus, errorThrown) {
//                        alert(textStatus);
//                    }
//                });
//            },
//            select: function(e, ui) {

//                $('#<%= hdnGlCode.ClientID %>').val(ui.item.id);
//                if (ui.item.result != '') {
//                    $('#<%= txtChqAcctNo.ClientID %>').val(ui.item.result);
//                }
//                else {
//                    $('#<%= txtChqAcctNo.ClientID %>').val('');
//                }
//                $('#<%= txtChqNo.ClientID %>').focus();

//            },

//            minLength: 1
//        });
    }
    
</script>--%>s
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../assets/js/jquery.min.js"></script>
    <script>


        $(document).ready(function () {


            $(".salarydays").change(function () {
                debugger
               var Sday =  $(".salarydays").val();
                var monthday = $(".monthdays").val();
                var basicpay = $(".txtBasicPay").val();
                var daydiff = monthday - Sday;
                var bsic = parseInt((basicpay * daydiff) / monthday);
                var basicdif = basicpay - bsic;
                $(".txtBasicPay").val(basicdif);

                $('.getallowance').find('input').each(function (i, itm) {

                    var singleVal = $(itm).val();
                    var allowance = parseInt((singleVal * daydiff) / monthday)
                    var all = singleVal - allowance;
                    $(itm).val(all);
                });

                $('.getDeduction').find('input').each(function (i, deditm) {

                    var dedVal = $(deditm).val();
                    var deduction = parseInt((dedVal * daydiff) / monthday)
                    var ded = dedVal - deduction;
                    $(deditm).val(ded);
                });
            });

            debugger
            var brID = '<%=Session["BranchID"].ToString() %>';
            if (brID == "1") {
                $(".divi").show();
                $(".txtsearchresult").show();
            }
            else {
                $(".divi").hide();
                $(".txtsearchresult").hide();
            }

            var total = 0;
            $('.getallowance').find('input').each(function (i, itm) {

                var singleVal = $(itm).val();
                if (singleVal == null || singleVal == "" || typeof singleVal === "undefined") {
                    singleVal = 0;
                }
                total += parseFloat(singleVal);
            })
            $(".totalAllowances").val(total);

            $(".employeeType").change(function () {
                let empTypeVal = $(".employeeType").val();
                CascadingEmployee(empTypeVal);

            });


            function CascadingEmployee(empTypes) {


                $.ajax({
                    url: "empmgtsalpayment.aspx/GetCascadingEmployeeList",
                    data: JSON.stringify({ empType: empTypes }),
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; chartset=utf-8",
                    success: function (data) {
                        debugger;
                        var chk = data.d;

                        $('.employeeList').empty();

                        $.each(data.d, function (index, row) {
                            $('.employeeList').append("<option value='" + row[0] + "'>" + row[1] + "</option>")
                        });

                        //GetAllowanceTotal();
                        //GetDeductionTotal();
                    },
                    error: function () {
                        alert("Error loading data! Please try again.");
                    }
                })

            }


            //function GetAllowanceTotal() {
            //    var total = 0;
            //    $('.getallowance').find('input').each(function (i, itm) {

            //        var singleVal = $(itm).val();
            //        if (singleVal == null || singleVal == "" || typeof singleVal === "undefined") {
            //            singleVal = 0;
            //        }
            //        total += parseFloat(singleVal);
            //    })
            //        $(".totalAllowances").val(total);

            //}

            //function GetDeductionTotal() {
            //    var total = 0;
            //    $('.getDeduction').find('input').each(function (i, itm) {

            //        var singleVal = $(itm).val();
            //        if (singleVal == null || singleVal == "" || typeof singleVal === "undefined") {
            //            singleVal = 0;
            //        }
            //        total += parseFloat(singleVal);
            //    })
            //        $(".totalDeductions").val(total);

            //}

            var total1 = 0;
            $('.getDeduction').find('input').each(function (i, itm) {

                var singleVal = $(itm).val();
                if (singleVal == null || singleVal == "" || typeof singleVal === "undefined") {
                    singleVal = 0;
                }
                total1 += parseFloat(singleVal);
            })
            $(".totalDeductions").val(total1);
            //Get  netpay with Allowance
            var totalAllowance = 0;


            var singleValallowance = $(".totalAllowances").val();
            if (singleValallowance == null || singleValallowance == "" || typeof singleValallowance === "undefined") {
                singleValallowance = 0;
            }
            var basicnetpay = $(".txtBasicPay").val();
            if (basicnetpay == null || basicnetpay == "") {
                basicnetpay = 0;
            }
            var dedValdeduction = $(".totalDeductions").val();
            if (dedValdeduction == null || dedValdeduction == "" || typeof dedValdeduction === "undefined") {
                dedValdeduction = 0;
            }
            totalAllowance = (parseInt(singleValallowance) + parseInt(basicnetpay)) - parseInt(dedValdeduction);

            $(".netpay").val(totalAllowance);

            // Get net with Deduction

            var totalDeduction = 0;
            var singleValall = $(".totalAllowances").val();
            if (singleValall == null || singleValall == "" || typeof singleValall === "undefined") {
                singleValall = 0;
            }
            var basicnet = $(".txtBasicPay").val();
            if (basicnet == null || basicnet == "") {
                basicnet = 0;
            }
            var dedValded = $(".totalDeductions").val();
            if (dedValded == null || dedValded == "" || typeof dedValded === "undefined") {
                dedValded = 0;
            }
            totalDeduction = (parseInt(singleValall) + parseInt(basicnet)) - parseInt(dedValded);

            $(".netpay").val(totalDeduction);

            // Get Gross Pay
            var Grosstotal = 0;


            var singVal = $(".totalAllowances").val();
            if (singVal == null || singVal == "" || typeof singVal === "undefined") {
                singVal = 0;
            }
            var bas = $(".txtBasicPay").val();
            if (bas == null || bas == "") {
                bas = 0;
            }
            //var dedVal = $(".totalDeductions").val();
            //    if (dedVal == null || dedVal == "" || typeof singleVal === "undefined") {
            //        dedVal = 0;
            //}
            Grosstotal = (parseInt(singVal) + parseInt(bas));

            $(".GrossPay").val(Grosstotal);

        });

    </script>
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                                ValidationGroup="main" />
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>

                    <asp:Label ID="lblMonthSal" runat="server" Font-Bold="True" Text="Salary Calculation for month "></asp:Label>
                    <br />
                    <br />

                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-3 divi">
                            <label>Select Division*</label>
                            <asp:DropDownList ID="ddlBranchDropdown" runat="server" CssClass="form-control" AppendDataBoundItems="False" OnSelectedIndexChanged="ddlBank_SelectedIndexChanged" AutoPostBack="true">
                                <%--<asp:ListItem Value="0">Select Employee</asp:ListItem>--%>
                            </asp:DropDownList>
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldEmployee" runat="server" ControlToValidate="ddlEmployee"
                                    ErrorMessage="Please select Employee" SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Employee Type</label>
                            <asp:DropDownList ID="ddlJobType" runat="server" CssClass="form-control " AppendDataBoundItems="True" OnSelectedIndexChanged="ddlJobtype_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <%--<div class="col-lg-3 col-md-3 col-sm-3 txtsearchresult">
                            <label>Search Employee</label>
                            <asp:TextBox runat="server" ID="txtEmpSearch" CssClass="form-control txtsearch"></asp:TextBox>
                            <div id="EmployeeList">
                            </div>
                        </div>--%>

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Employee</label>
                            <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control" AppendDataBoundItems="False" OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>

                        </div>


                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label id="basicpayLable">Basic Pay *</label>
                            <asp:TextBox ID="txtBasicPay" Style="text-align: left" runat="server" CssClass="txtBasicPay form-control" MaxLength="7"></asp:TextBox>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>From Period</label>
                            <span class="DteLtrl">
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ AppSettings: DateFormatPageText %>" /></span>
                            <asp:TextBox ID="txtfromPerid" runat="server" MaxLength="11" CssClass="form-control" Disabled="true"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtfromPeridCal" runat="server" Enabled="True" TargetControlID="txtfromPerid">
                            </ajaxToolkit:CalendarExtender>
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEffDate"
                                    ErrorMessage="Please select effective date" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>To Period </label>
                            <span class="DteLtrl">
                                <asp:Literal ID="Literal3" runat="server" Text="<%$ AppSettings: DateFormatPageText %>" /></span>
                            <asp:TextBox ID="txtToPeriod" runat="server" MaxLength="11" CssClass="form-control" Disabled="true"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtToPeriodCal" runat="server" Enabled="True" TargetControlID="txtToPeriod">
                            </ajaxToolkit:CalendarExtender>
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEffDate"
                                    ErrorMessage="Please select effective date" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Per Day Rate</label>
                            <asp:TextBox ID="txtPerDay" runat="server" CssClass="form-control" Disabled="true"></asp:TextBox>

                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEffDate"
                                    ErrorMessage="Please select effective date" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <br />
                            <br />
                            <asp:CheckBox ID="CheckIsActive" Checked="true" runat="server" />&nbsp;<label>Is Active</label>
                        </div>
                        <div class="col-lg-6">
                            <label>Remarks*</label>
                            <asp:TextBox ID="txtRemaks" runat="server" CssClass="form-control" TextMode="MultiLine" onkeyup="LimitText(this,1000);" onblur="LimitText(this,1000);" Height="40px"> </asp:TextBox>
                        </div>
                        <div class="col-lg-3">
                            <label>Month Days</label>
                            <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control monthdays"></asp:TextBox>
                        </div>
                        <div class="col-lg-3">
                            <label>Salary Days</label>
                            <asp:TextBox ID="TextBox4" runat="server" CssClass="form-control salarydays"></asp:TextBox>
                        </div>

                    </div>
                    <%--<div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-3 offset-lg-6">
                                <label>Net Pay</label>
                                <asp:TextBox runat="server" ID="txtNetPay" CssClass="form-control netpay" />
                            </div>
                    </div>--%>
                    <br />
                    <div class="row gridallred">
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <h3>Allowances</h3>
                            <hr />
                            <div class="row" id="allowanceClass">

                                <div class="col-md-8">
                                    <asp:Panel ID="EmployAllowanceLableGridDetail" Width="100%" runat="server">
                                    </asp:Panel>
                                    <label style="margin-left: 150px; margin-top: 13px"><b>Total Allowance:-</b></label>
                                </div>
                                <div class="col-md-4">
                                    <div id="allowanceinputdiv">
                                        <asp:Panel ID="EmployAllowanceGridDetail" CssClass="getallowance" Width="100%" runat="server">
                                        </asp:Panel>
                                        <asp:TextBox ID="totalAllowances" runat="server" CssClass="form-control totalAllowances"></asp:TextBox>
                                    </div>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-md-8">
                                    <label style="margin-left: 150px; margin-top: 13px"><b>Gross Pay:-</b></label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control GrossPay"></asp:TextBox>
                                </div>
                            </div>

                        </div>

                        <div class="col-lg-6 col-md-6 col-sm-6">

                            <h3>Deductions</h3>
                            <hr />
                            <div class="row" id="deductionClass">
                                <div class="col-md-8">
                                    <asp:Panel ID="EmployDeductionLableGridDetail" Width="100%" runat="server">
                                    </asp:Panel>
                                    <label style="margin-left: 150px; margin-top: 13px"><b>Total Deduction:-</b></label>
                                </div>
                                <div class="col-md-4">
                                    <div id="deductioninputdiv">
                                        <asp:Panel ID="EmployDeductionGridDetail" CssClass="getDeduction" Width="100%" runat="server">
                                        </asp:Panel>
                                        <asp:TextBox ID="totalDeductions" runat="server" CssClass="form-control totalDeductions"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-8">
                                    <label style="margin-left: 150px; margin-top: 13px"><b>Net Pay:-</b></label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox runat="server" ID="TextBox1" CssClass="form-control netpay" />
                                </div>
                            </div>
                        </div>


                    </div>


                    <br />
                    <div class="row btnDiv">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button ID="Save" runat="server" OnClick="Button_Command" Text="Transfer" class="btn btn-primary" />
                            <asp:Button ID="Clear" runat="server" OnClick="Clear_All" Text="Clear" class="btn btn-danger" />

                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <asp:Label ID="Label1" runat="server" Text="Division:"></asp:Label><br />
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged" AppendDataBoundItems="True" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Division</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <asp:Label ID="Label3" runat="server" Text="Month:"></asp:Label><br />
                            <asp:DropDownList ID="searchMonthDropDown" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select Month</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <asp:Label ID="Label2" runat="server" Text="Name:"></asp:Label><br />
                            <asp:TextBox ID="searchEmployeeTxt" runat="server" CssClass="txtBasicPay form-control"></asp:TextBox>
                        </div>

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <br />
                            <asp:Button ID="btnSearch" runat="server" OnClick="Button_CommandSearch" Text="Search" class="btn btn-primary" />
                        </div>

                    </div>

                    <br />

                    <asp:GridView ID="grdSalaryTranfer" runat="server" CssClass="table table-responsive-sm table-bordered" DataKeyNames="SalTrfID,SalaryMonth" OnSelectedIndexChanged="grdSalaryTranfer_PageIndexChanged"
                        AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdSalaryTranfer_PageIndexChanging" OnRowDataBound="grdSalaryTranfer_RowBound" OnRowDeleting="grdSalaryTranfer_RowDeleting"
                        EmptyDataText="There is no package defined" Width="100%">
                        <Columns>

                            <asp:BoundField DataField="FullName" HeaderText="Name" />
                            <asp:BoundField DataField="Basic" HeaderText="Basic Pay" />
                            <asp:BoundField DataField="MonthVal" HeaderText="Month" />

                            <%--<asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                <ControlStyle CssClass="lnk"></ControlStyle>
                            </asp:CommandField>--%>

                            <asp:CommandField ControlStyle-CssClass="lnk" ShowDeleteButton="True">
                                <ControlStyle CssClass="lnk"></ControlStyle>
                            </asp:CommandField>
                            <%--<asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPrint" runat="server" Text="Delete" ToolTip="Print Employee Education Record" CommandArgument='<%#Eval("SalTrfID")%>' OnClick="lnkEduPrint_Click" CssClass="lnk">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>


                        </Columns>
                        <HeaderStyle CssClass="grid_hdr" />
                        <RowStyle CssClass="grid_row" />
                        <AlternatingRowStyle CssClass="gridAlternateRow" />
                        <SelectedRowStyle CssClass="gridSelectedRow" />
                    </asp:GridView>


                    <%--                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                             ValidationGroup="main" />
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>
                    <div id="divSearch" runat="server">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Label ID="lblPerd" runat="server" Text="Pay Period: "></asp:Label>
                                <asp:DropDownList ID="ddlPayPerd" CssClass="form-control" runat="server"></asp:DropDownList>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Label ID="Label5"  runat="server" Text="Pay Type:"></asp:Label>
                                <asp:DropDownList ID="ddlPayType" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="All">All</asp:ListItem>
                                    <asp:ListItem Value="Cheque">Cheque</asp:ListItem>
                                    <asp:ListItem Value="Cash">Cash</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Label ID="Label1"  runat="server" Text="Department: "></asp:Label>
                                  <asp:DropDownList ID="ddlDept" CssClass="form-control" runat="server" AppendDataBoundItems="True">
                                    <%--<asp:ListItem Value="0">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        &nbsp;
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Label ID="Label6" runat="server" Text="Bank: "></asp:Label>
                                <asp:DropDownList ID="ddlBank" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                                    <asp:ListItem Value="0">All</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Label ID="Label2" runat="server" Text="Min. Salary: "></asp:Label>
                                <asp:TextBox ID="txtMinSal" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Label ID="Label3" runat="server" Text="Max. Salary: "></asp:Label>
                                <asp:TextBox ID="txtMaxSal" runat="server" MaxLength="8" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        &nbsp;
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Label ID="Label4" runat="server" Text="Job Type: " Visible="false"></asp:Label>
                                <asp:DropDownList ID="ddlJobType" runat="server" Visible="false">
                                    <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                    <asp:ListItem Value="Permanent">Permanent</asp:ListItem>
                                    <asp:ListItem Value="Temporary">Temporary</asp:ListItem>
                                    <asp:ListItem Value="Worker">Worker</asp:ListItem>
                                </asp:DropDownList> 
                            </div>
                        </div>
                        &nbsp;
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="btnSearch_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:GridView ID="grdSalPay" CssClass="table table-responsive-sm" runat="server" 
                                    DataKeyNames="CompID,EmpID"
                                    AutoGenerateColumns="False" AllowPaging="false" 
                                    OnPageIndexChanging="grdSalPay_PageIndexChanging" 
                                    OnRowDataBound="grdSalPay_RowDataBound"
                                    EmptyDataText="No employee found" Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="region" HeaderText="Region" ItemStyle-Width="60px"/>
                                        <asp:BoundField DataField="department" HeaderText="Deptartment" ItemStyle-Width="80px"/>
                                        <asp:BoundField DataField="designation" HeaderText="Designation" ItemStyle-Width="100px"/>
                                        <asp:BoundField DataField="empcode" HeaderText="Emp Ref" ItemStyle-Width="60px"/>
                                        <asp:BoundField DataField="fullname" HeaderText="Name" />
                                        <asp:BoundField DataField="bankname" HeaderText="Bank Name" ItemStyle-Width="70px"/>
                                        <asp:BoundField DataField="branch" HeaderText="Branch" ItemStyle-Width="100px"/>
                                        <asp:BoundField DataField="taxded" HeaderText="Tax Ded" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right"/>
                                        <asp:BoundField DataField="loanadvded" HeaderText="L/A Ded" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right"/>
                                        <asp:BoundField DataField="eobided" HeaderText="EOBI Ded" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right"/>
                                        <asp:BoundField DataField="otrded" HeaderText="Other Ded" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right"/>
                                        <asp:BoundField DataField="messded" HeaderText="Misc Ded" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right"/>
                                        <asp:BoundField DataField="netpay" HeaderText="Net Pay" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right"/>
                                        <asp:TemplateField ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkIncludeAll" runat="server" ToolTip="Select All"/>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIncludeInVoucher" runat="server"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="grid_hdr" />
                                    <RowStyle CssClass="grid_row" />
                                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                                    <SelectedRowStyle CssClass="gridSelectedRow" />
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="row" id="divView" runat="server">
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:GridView ID="grdSalPayView" CssClass="table table-responsive-sm" runat="server" 
                        DataKeyNames="CompID,EmpID"
                        AutoGenerateColumns="False" AllowPaging="false" 
                        OnPageIndexChanging="grdSalPayView_PageIndexChanging" 
                        OnRowDataBound="grdSalPayView_RowDataBound"
                        EmptyDataText="No employee found" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="region" HeaderText="Region" ItemStyle-Width="60px"/>
                            <asp:BoundField DataField="department" HeaderText="Deptartment" ItemStyle-Width="80px"/>
                            <asp:BoundField DataField="designation" HeaderText="Designation" ItemStyle-Width="120px"/>
                            <asp:BoundField DataField="empcode" HeaderText="Emp Ref" ItemStyle-Width="70px"/>
                            <asp:BoundField DataField="fullname" HeaderText="Name" />
                            <asp:BoundField DataField="bankname" HeaderText="Bank Name" ItemStyle-Width="80px"/>
                            <asp:BoundField DataField="branch" HeaderText="Branch" ItemStyle-Width="120px"/>
                            <asp:BoundField DataField="taxded" HeaderText="Tax Ded" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right"/>
                            <asp:BoundField DataField="loanadvded" HeaderText="L/A Ded" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right"/>
                            <asp:BoundField DataField="eobided" HeaderText="EOBI Ded" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right"/>
                            <asp:BoundField DataField="otrded" HeaderText="Other Ded" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right"/>
                            <asp:BoundField DataField="messded" HeaderText="Misc Ded" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right"/>
                            <asp:BoundField DataField="netpay" HeaderText="Net Pay" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right"/>
                            
                        </Columns>
                        <HeaderStyle CssClass="grid_hdr" />
                        <RowStyle CssClass="grid_row" />
                        <AlternatingRowStyle CssClass="gridAlternateRow" />
                        <SelectedRowStyle CssClass="gridSelectedRow" />
                    </asp:GridView>
                            </div>
                        </div>
                        &nbsp;
                        <div class="row">
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <label>Amount:</label>
                                <asp:TextBox ID="txtChqAmnt" runat="server" CssClass="RequiredField form-control" MaxLength="9" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtChqAmnt"
                                    ErrorMessage="Please enter cheque amount" SetFocusOnError="true" ValidationGroup="main"
                                    Display="None"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <label>Text Deduction:</label>
                                <asp:TextBox ID="txtTaxDed" runat="server" CssClass="RequiredField form-control" MaxLength="9" TabIndex="-1"></asp:TextBox>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <label>Loan/Adv Deduct:</label>
                                <asp:TextBox ID="txtLoanAdvDed" runat="server" CssClass="RequiredField form-control" MaxLength="9"  TabIndex="-1"></asp:TextBox>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <label>EOBI Deduction:</label>
                                 <asp:TextBox ID="txtEobiDed" runat="server" CssClass="RequiredField form-control" MaxLength="9" TabIndex="-1"></asp:TextBox>

                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <label>Other Deduction:</label>
                                <asp:TextBox ID="txtOtherDed" runat="server" CssClass="RequiredField form-control" MaxLength="9" TabIndex="-1"></asp:TextBox>

                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <label>Misc. Deduction:</label>
                                <asp:TextBox ID="txtMiscDed" runat="server" CssClass="RequiredField form-control" MaxLength="9" TabIndex="-1"></asp:TextBox>   

                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <label>Status:</label>
                                 <asp:DropDownList ID="ddlAppStatus"  runat="server" CssClass="RequiredField form-control">
                                <asp:ListItem Value="P">Pending </asp:ListItem>
                                <asp:ListItem Value="A">Approved</asp:ListItem>
                            </asp:DropDownList>
                            </div>
                        </div>
                        &nbsp;
                        <div runat="server" id="divChq" class="DisplayNone">
                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label><b>Cheque Detail:</b></label>
                                </div>
                            </div>
                            <div class="row">
                                
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label>Branch:</label>
                                    <asp:TextBox ID="txtChqBranch" runat="server" CssClass="RequiredField form-control" MaxLength="50" ></asp:TextBox>
                                <asp:RequiredFieldValidator Enabled="false" ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtChqBranch"
                                    ErrorMessage="Please enter branch name" SetFocusOnError="true" ValidationGroup="main"
                                    Display="None"></asp:RequiredFieldValidator>
                                <asp:HiddenField ID="hdnGlCode" runat="server" />
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label>Account #</label>
                                    <asp:TextBox ID="txtChqAcctNo" runat="server" MaxLength="20" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                     <label>Cheque #</label>
                                    <asp:TextBox ID="txtChqNo" runat="server" MaxLength="20" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator Enabled="false" ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtChqNo"
                                    ErrorMessage="Please enter cheque #" SetFocusOnError="true" ValidationGroup="main"
                                    Display="None"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label>Date:</label>
                                    <ajaxToolkit:CalendarExtender ID="txtChqDateCal" runat="server" TargetControlID="txtChqDate"
                                    Enabled="True">
                                </ajaxToolkit:CalendarExtender>
                                <asp:TextBox ID="txtChqDate" CssClass="form-control"  runat="server" ></asp:TextBox><br />
                                <asp:RequiredFieldValidator Enabled="false" ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtChqDate"
                                    ErrorMessage="Please select date of check" SetFocusOnError="true" ValidationGroup="main"
                                    Display="None"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        &nbsp;
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
                <asp:ImageButton ID="btnDelete" runat ="server"  OnClick="btnDelete_Click" ImageUrl="~/images/btn_delete.png" onMouseOver="this.src='../images/btn_delete_m.png'" onMouseOut="this.src='../images/btn_delete.png'" Visible="false" />
                            </div>
                        </div>
                        &nbsp;
                        <div class="row">
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:GridView ID="grdPayments" CssClass="table table-responsive-sm" runat="server" DataKeyNames="Vrid"
                        OnSelectedIndexChanged="grdPayments_SelectedIndexChanged" 
                        AutoGenerateColumns="False"
                        AllowPaging="True" 
                        PageSize ="20"
                        OnPageIndexChanging="grdPayments_PageIndexChanging" 
                        OnRowDataBound="grdPayments_RowDataBound"
                        EmptyDataText="No record exists" Width="98%">
                        <Columns>
                            <asp:BoundField DataField="RefNo" HeaderText="Ref. No." />
                            <asp:BoundField DataField="RefDate" HeaderText="Ref. Date" />
                            <asp:BoundField DataField="PayPerd" HeaderText="Pay Period" />
                            <asp:BoundField DataField="Amount" HeaderText="Amount" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
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
                            </div>
                        </div>
                    </div>--%>
                </div>

            </div>
        </div>

    </div>
    <asp:Panel ID="pnlMain" runat="server" Width="780px" Height="600">
        <rsweb:ReportViewer ID="viewer" runat="server" Width="100%" Height="580px">
        </rsweb:ReportViewer>
    </asp:Panel>


</asp:Content>
