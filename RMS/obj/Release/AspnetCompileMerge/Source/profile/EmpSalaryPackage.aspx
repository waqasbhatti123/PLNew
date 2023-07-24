<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true" CodeBehind="EmpSalaryPackage.aspx.cs"
    Inherits="RMS.profile.EmpSalaryPackage" Culture="auto" UICulture="auto" EnableEventValidation="true" %>



<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../js/empsaltotal.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>
    <script>


        $(document).ready(function () {
            $('#<%= txtEmpSearch.ClientID %>').autocomplete({
                source: function (request, response) {
                    debugger
                    var param = { employee: $(".txtsearch").val() };
                    $.ajax({
                        url: "EmpSalaryPackage.aspx/GetEmployee",
                        data: JSON.stringify(param),
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            debugger
                            response($.map(data.d, function (item) {
                                return {
                                    value: item.FullName,
                                    result: item.EmpID,
                                    id: item.EmpID
                                }
                            }))
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus);
                        }
                    });
                },
                select: function (e, ui) {
                    //glCd = ui.item.id;
                    // getFirstRowCode();
                    if (ui.item.result != '') {
                        $('#<%= ddlEmployee.ClientID %>').val(ui.item.result);
                }
                else {
                    $('#<%= ddlEmployee.ClientID %>').val('0');
                    }

                },
                minLength: 1
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


            $(".txtBasicPay").keyup(function () {
                var basic = $(".txtBasicPay").val();
                if (basic == null || basic == "") {
                    basic = 0;
                }
                else {
                    $(".netpay").val(basic);
                }
                Calc10PercentofBasic(basic);
                Calc30PercentofBasic(basic);
                Calc19PercentofBasic(basic);
                Calc21PercentofBasic(basic);
                Calc5PercentofBasic(basic);
            });

            function Calc10PercentofBasic(basic) {

                if (isNaN(basic)) {
                    basic = 0;
                }
                var calc = parseInt((basic * 10) / 100);
                
                $('.cal10').each(function (i, itm) {
                    $(itm).val(calc);
                });

                GetAllowanceTotal();
                GetNetAllowanceTotal();
                GetGrossAllowanceTotal();
                GetDeductionTotal();
                GetNetDeductionTotal();
            }

            function Calc19PercentofBasic(basic) {

                if (isNaN(basic)) {
                    basic = 0;
                }
                var calc = parseInt((basic * 10) / 100);
                
                $('.cal19').each(function (i, itm) {
                    $(itm).val(calc);
                });

                GetAllowanceTotal();
                GetNetAllowanceTotal();
                GetGrossAllowanceTotal();
            }

            function Calc21PercentofBasic(basic) {

                if (isNaN(basic)) {
                    basic = 0;
                }
                var calc = parseInt((basic * 10) / 100);
                
                $('.cal21').each(function (i, itm) {
                    $(itm).val(calc);
                });

                GetAllowanceTotal();
                GetNetAllowanceTotal();
                GetGrossAllowanceTotal();
            }

            function Calc30PercentofBasic(basic) {
                debugger

                if (isNaN(basic)) {
                    basic = 0;
                }
                var calc = parseInt((basic * 30) / 100);
                
                $('.cal30').each(function (i, itm) {
                    $(itm).val(calc);
                });

                GetAllowanceTotal();
                GetNetAllowanceTotal();
                GetGrossAllowanceTotal();
            }

            function Calc5PercentofBasic(basic) {

                if (isNaN(basic)) {
                    basic = 0;
                }
                var calc = parseInt((basic * 5) / 100);
                
                $('.cal5').each(function (i, itm) {
                    $(itm).val(calc);
                });

                GetAllowanceTotal();
                GetNetAllowanceTotal();
                GetGrossAllowanceTotal();
            }
            //Get Deduction
            var total = 0;
            $('.getDeduction').find('input').each(function (i, itm) {

                var singleVal = $(itm).val();
                if (singleVal == null || singleVal == "" || typeof singleVal === "undefined") {
                    singleVal = 0;
                }
                total += parseFloat(singleVal);
            })
            $(".totalDeductions").val(total);
            // end Deduction

            //Get Allowance
            var totalAllowance = 0;
            $('.getallowance').find('input').each(function (i, itm) {

                var singleValall = $(itm).val();
                if (singleValall == null || singleValall == "" || typeof singleValall === "undefined") {
                    singleValall = 0;
                }
                totalAllowance += parseFloat(singleValall);
            })
            $(".totalAllowances").val(totalAllowance);
            // End Allowance

            // Get Net Allowance
            var totalNetAllowance = 0;
            var singleValallowance = $(".totalAllowances").val();
            if (singleValallowance == null || singleValallowance == "" || typeof singleValallowance === "undefined") {
                singleValallowance = 0;
            }
            var basic = $(".txtBasicPay").val();
            if (basic == null || basic == "") {
                basic = 0;
            }
            var dedValded = $(".totalDeductions").val();
            if (dedValded == null || dedValded == "" || typeof dedValded === "undefined") {
                dedValded = 0;
            }
            totalNetAllowance = (parseInt(singleValallowance) + parseInt(basic)) - parseInt(dedValded);

            $(".netpay").val(totalNetAllowance);
            // End Net Allowance
            // Get net Deduction
            var totalDeducti = 0;
            var singleValnet = $(".totalAllowances").val();
            if (singleValnet == null || singleValnet == "" || typeof singleValnet === "undefined") {
                singleValnet = 0;
            }
            var basicnet = $(".txtBasicPay").val();
            if (basicnet == null || basicnet == "") {
                basicnet = 0;
            }
            var dedValNet = $(".totalDeductions").val();
            if (dedValNet == null || dedValNet == "" || typeof dedValNet === "undefined") {
                dedValNet = 0;
            }
            totalDeducti = (parseInt(singleValnet) + parseInt(basicnet)) - parseInt(dedValNet);

            $(".netpay").val(totalDeducti);
            // End Net Deduction
            //Gross Pay
            var totalFross = 0;


            var singleVal = $(".totalAllowances").val();
            if (singleVal == null || singleVal == "" || typeof singleVal === "undefined") {
                singleVal = 0;
            }
            var basic = $(".txtBasicPay").val();
            if (basic == null || basic == "") {
                basic = 0;
            }
            var dedVal = $(".totalDeductions").val();
            if (dedVal == null || dedVal == "" || typeof singleVal === "undefined") {
                dedVal = 0;
            }
            totalFross = (parseInt(singleVal) + parseInt(basic));

            $(".GrossPay").val(totalFross);
            // End Gross Pay
            $(".employeeType").change(function () {


                let empTypeVal = $(".employeeType").val();
                CascadingEmployee(empTypeVal);
                if (empTypeVal == "5") {
                    $(".gridallred").hide();
                    //$(".btnDiv").hide();
                    $("#basicpayLable").html("Lump Sum");
                }
                else {
                    $(".gridallred").show();
                    // $(".btnDiv").show();
                    $("#basicpayLable").html("Basic Pay");
                }
            });

            $('.getallowance').find('input').keyup(function () {

                GetAllowanceTotal();
                GetNetAllowanceTotal();
                GetGrossAllowanceTotal();
            });
            $('.getallowance').find('input').keydown(function () {

                GetAllowanceTotal();
            });

            function GetAllowanceTotal() {
                var total = 0;
                $('.getallowance').find('input').each(function (i, itm) {

                    var singleVal = $(itm).val();
                    if (singleVal == null || singleVal == "" || typeof singleVal === "undefined") {
                        singleVal = 0;
                    }
                    total += parseFloat(singleVal);
                })
                $(".totalAllowances").val(total);

            }

            function GetNetAllowanceTotal() {
                var total = 0;


                var singleVal = $(".totalAllowances").val();
                if (singleVal == null || singleVal == "" || typeof singleVal === "undefined") {
                    singleVal = 0;
                }
                var basic = $(".txtBasicPay").val();
                if (basic == null || basic == "") {
                    basic = 0;
                }
                var dedVal = $(".totalDeductions").val();
                if (dedVal == null || dedVal == "" || typeof singleVal === "undefined") {
                    dedVal = 0;
                }
                total = (parseInt(singleVal) + parseInt(basic)) - parseInt(dedVal);

                $(".netpay").val(total);

            }

            function GetGrossAllowanceTotal() {
                var total = 0;


                var singleVal = $(".totalAllowances").val();
                if (singleVal == null || singleVal == "" || typeof singleVal === "undefined") {
                    singleVal = 0;
                }
                var basic = $(".txtBasicPay").val();
                if (basic == null || basic == "") {
                    basic = 0;
                }
                var dedVal = $(".totalDeductions").val();
                if (dedVal == null || dedVal == "" || typeof singleVal === "undefined") {
                    dedVal = 0;
                }
                total = (parseInt(singleVal) + parseInt(basic));

                $(".GrossPay").val(total);

            }

            $('.getDeduction').find('input').keyup(function () {

                GetDeductionTotal();
                GetNetDeductionTotal();
            });
            $('.getDeduction').find('input').keydown(function () {

                GetDeductionTotal();

            });

            

            function GetDeductionTotal() {
                var total = 0;
                $('.getDeduction').find('input').each(function (i, itm) {

                    var singleVal = $(itm).val();
                    if (singleVal == null || singleVal == "" || typeof singleVal === "undefined") {
                        singleVal = 0;
                    }
                    total += parseFloat(singleVal);
                })
                $(".totalDeductions").val(total);

            }

            function GetNetDeductionTotal() {
                var total = 0;
                var singleVal = $(".totalAllowances").val();
                if (singleVal == null || singleVal == "" || typeof singleVal === "undefined") {
                    singleVal = 0;
                }
                var basic = $(".txtBasicPay").val();
                if (basic == null || basic == "") {
                    basic = 0;
                }
                var dedVal = $(".totalDeductions").val();
                if (dedVal == null || dedVal == "" || typeof singleVal === "undefined") {
                    dedVal = 0;
                }
                total = (parseInt(singleVal) + parseInt(basic)) - parseInt(dedVal);

                $(".netpay").val(total);

            }





            function CascadingEmployee(empTypes) {


                $.ajax({
                    url: "EmpSalaryPackage.aspx/GetCascadingEmployeeList",
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


                    },
                    error: function () {
                        alert("Error loading data! Please try again.");
                    }
                })

            }




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
                            <label>Employee Type*</label>
                            <asp:DropDownList ID="ddlJobType" runat="server" CssClass="form-control" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlJobtype_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3 txtsearchresult">
                            <label>Search Employee</label>
                            <asp:TextBox runat="server" ID="txtEmpSearch" CssClass="form-control txtsearch"></asp:TextBox>
                            <div id="EmployeeList">
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Employee*</label>

                            <asp:UpdatePanel ID="upnl" runat="server">
                                <ContentTemplate>

                                    <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control" AppendDataBoundItems="False" OnSelectedIndexChanged="select_change" AutoPostBack="true">
                                        <%--<asp:ListItem Value="0">Select Employee</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <triggers>
                                  <asp:AsyncPostBackTrigger ControlID="ddlJobType" />
                             </triggers>
                            </asp:UpdatePanel>
                            
                            

                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldEmployee" runat="server" ControlToValidate="ddlEmployee"
                                    ErrorMessage="Please select Employee" SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator>--%>
                        </div>

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Effective Date *</label>
                            <span class="DteLtrl">
                                <asp:Literal ID="Literal1" runat="server" Text="<%$ AppSettings: DateFormatPageText %>" /></span>
                            <asp:TextBox ID="txtEffDate" runat="server" MaxLength="11" CssClass="RequiredField form-control"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtEffDateCal" runat="server" Enabled="True" TargetControlID="txtEffDate">
                            </ajaxToolkit:CalendarExtender>
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEffDate"
                                    ErrorMessage="Please select effective date" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                        </div>

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label id="basicpayLable">Basic Pay *</label>
                            <asp:TextBox ID="txtBasicPay" Style="text-align: left" runat="server" CssClass="txtBasicPay form-control" MaxLength="6"></asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBasicPay"
                                    ErrorMessage="Please Enter Basic Pay" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>From Period</label>
                            <span class="DteLtrl">
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ AppSettings: DateFormatPageText %>" /></span>
                            <asp:TextBox ID="txtfromPerid" runat="server" MaxLength="11" CssClass="form-control" ></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtfromPeridCal" runat="server" Enabled="True" TargetControlID="txtfromPerid">
                            </ajaxToolkit:CalendarExtender>
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEffDate"
                                    ErrorMessage="Please select effective date" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>To Period </label>
                            <span class="DteLtrl">
                                <asp:Literal ID="Literal3" runat="server" Text="<%$ AppSettings: DateFormatPageText %>" /></span>
                            <asp:TextBox ID="txtToPeriod" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtToPeriodCal" runat="server" Enabled="True" TargetControlID="txtToPeriod">
                            </ajaxToolkit:CalendarExtender>
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEffDate"
                                    ErrorMessage="Please select effective date" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Per Day Rate</label>
                            <asp:TextBox ID="txtPerDay" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
                            
                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEffDate"
                                    ErrorMessage="Please select effective date" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                        </div>
                    </div>
                    &nbsp;
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <asp:CheckBox ID="checkIsActive" Checked="true" class="checkbox" runat="server" />&nbsp;
                                <asp:Label Text="Is Active" ID="lblActive" runat="server" />
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBasicPay"
                                    ErrorMessage="Please Enter Basic Pay" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                            </div>

                        </div>
                    <br />
                    <%--<div class="row gridallred">
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <h3>Allowances</h3>
                                <hr />
                                <div class="row" id="allowanceClass">
                                    <div class="col-md-8">
                                        <asp:Panel ID="EmployAllowanceLableGridDetail" Width="100%" runat="server">
                                        </asp:Panel>
                                        <%--  <input type="text" value="Total Allowances" class="form-control" />
                                    </div>
                                    <div class="col-md-4">
                                        <div id="allowanceinputdiv">
                                            <asp:Panel ID="EmployAllowanceGridDetail" Width="100%" runat="server">
                                            </asp:Panel>
                                        </div>
                                        <%--    <asp:TextBox ID="totalAllowances" runat="server" CssClass="form-control totalAllowances"></asp:TextBox>
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
                                        <%--    <input type="text" value="Total Deductions" class="form-control" />
                                    </div>
                                    <div class="col-md-4">
                                        <div id="deductioninputdiv">
                                            <asp:Panel ID="EmployDeductionGridDetail" Width="100%" runat="server">
                                            </asp:Panel>
                                        </div>
                                        <%--   <asp:TextBox ID="totalDeductions" runat="server" CssClass="form-control totalDeductions"></asp:TextBox>
                                    </div>

                                </div>
                            </div>
                        </div>--%>




                    <div class="row gridallred">
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <h3>Allowances</h3>
                            <hr />
                            <div class="row" id="allowanceClass">

                                <div class="col-md-8">
                                    <asp:Panel ID="EmployAllowanceLableGridDetail" CssClass="getallowancelable" Width="100%" runat="server">
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
                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control GrossPay"></asp:TextBox>
                                </div>
                            </div>




                            <%--  <input type="text" value="Total Allowances" class="form-control"/>--%>
                        </div>

                        <div class="col-lg-6 col-md-6 col-sm-6">

                            <h3>Deductions</h3>
                            <hr />
                            <div class="row" id="deductionClass">

                                <div class="col-md-8">
                                    <asp:Panel ID="EmployDeductionLableGridDetail" Width="100%" runat="server">
                                    </asp:Panel>
                                    <label style="margin-left: 150px; margin-top: 13px"><b>Total Deduction:-</b></label>
                                    <%--<input type="text" value="Total Deductions" class="form-control" />--%>
                                </div>
                                <div class="col-md-4">
                                    <div id="deductioninputdiv">
                                        <asp:Panel ID="EmployDeductionGridDetail" CssClass="getDeduction" Width="100%" runat="server">
                                        </asp:Panel>
                                    </div>
                                    <asp:TextBox ID="totalDeductions" runat="server" CssClass="form-control totalDeductions"></asp:TextBox>
                                    <%--   <input type="text " id="totalDeductions" class="form-control"/>--%>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-lg-8">
                                    <label style="margin-left: 150px; margin-top: 13px"><b>Net Pay:-</b></label>
                                </div>
                                <div class="col-md-4">
                                    <asp:TextBox runat="server" ID="txtNetPay" CssClass="form-control netpay" />
                                </div>
                            </div>

                        </div>
                    </div>







                    <%--        <div class="row" id="NetPayClass">
            <div class="col-md-4">
                <input type="text" value="Net Pay" class="form-control" />
            </div>
            <div class="col-md-4">
                <div id="netPayinputdiv">
                </div>
                <asp:TextBox ID="NetPay" runat="server" CssClass="form-control NetPay"></asp:TextBox>
            </div>

        </div>--%>

                    <br />
                    <div class="row btnDiv">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button ID="Save" runat="server" OnClick="Button_Command" Text="Save" class="btn btn-primary" />
                            <asp:Button ID="Clear" runat="server" OnClick="Clear_All" Text="Clear" class="btn btn-danger" />

                        </div>
                    </div>

                    <br />
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label ID="Label1" runat="server" Text="Branch:"></asp:Label><br />
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged" AppendDataBoundItems="True" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Branch</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label ID="Label2" runat="server" Text="Name:"></asp:Label><br />
                            <asp:TextBox ID="searchEmployeeTxt" runat="server" CssClass="txtBasicPay form-control"></asp:TextBox>
                        </div>

                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <br />
                            <asp:Button ID="btnSearch" runat="server" OnClick="Button_CommandSearch" Text="Search" class="btn btn-primary" />
                        </div>
                    </div>

                    <br />
                    <asp:GridView ID="grdSalaryPackage" runat="server" CssClass="table table-responsive-sm table-bordered" DataKeyNames="EmpID,CompID,EffDate,BranchID" OnSelectedIndexChanged="grdSalaryPackage_PageIndexChanged"
                        AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdSalaryPackage_PageIndexChanging" OnRowDataBound="grdSalaryPackage_RowBound"
                        EmptyDataText="There is no package defined" Width="100%">
                        <Columns>
                            <%--<asp:TemplateField>
                                      <ItemTemplate>
                                          <asp:Label runat="server" ></asp:Label>
                                      </ItemTemplate>
                                      
                                  </asp:TemplateField>--%>
                            <asp:BoundField DataField="FullName" HeaderText="Name" />
                            <asp:BoundField DataField="EffDate" HeaderText="Eff. Date" />
                            <asp:BoundField DataField="Basic" HeaderText="Basic Pay" />
                            <asp:BoundField DataField="IsActive" HeaderText="Is Active" />
                            <%--                                  <asp:BoundField DataField="OtrAlow" HeaderText="Total Allowances" />
                                  <asp:BoundField DataField="OtherDed" HeaderText="Total Deductions" />
                                  <asp:BoundField DataField="NSHA" HeaderText="Net Pay" />--%>

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
            </div>
        </div>
    </div>


</asp:Content>






