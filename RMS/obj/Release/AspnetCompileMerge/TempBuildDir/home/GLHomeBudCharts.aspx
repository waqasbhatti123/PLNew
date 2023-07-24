<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="GLHomeBudCharts.aspx.cs" Inherits="RMS.home.GLHomeBudCharts" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="../assets/js/jquery.min.js"></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

    <script>
        var chartDataEmpSalaries; // globar variable for hold chart data
        var chartBudgetPieChart; // globar variable for hold chart data
        var PerAppBudgetBarChart; // globar variable for hold chart data
        var AppBudgetEstablStackChart; // globar variable for hold chart data
        var AppBudgetIncomeGrantStackChart; // globar variable for hold chart data
        var AppBudgeCouncilStackChart; // globar variable for hold chart data
        var PerAppBudgetbifrucBarChart; // globar variable for hold chart data
        google.charts.load("visualization", "1", { packages: ["corechart", 'bar'] });

        // Here We will fill chartData

        $(document).ready(function () {
            BudgetAppChartFunc();
            BudgetPereAppChartFunc();
            BudgetAppEstabChartFunc();
            BudgetAppnIncomeGrantChartFunc();
            BudgetAppCouncilChartFunc();
            BudgetBudgetvsVeriChartFunc();
            var brID = '<%=Session["BranchID"].ToString() %>';
            $('.searchbranchchange').val(brID);

            // DptEmpSalariesFunc();


            $('.searchbranchchange').change(function () {

                brID = $('.searchbranchchange').val();
                changeBranch(brID);

                // DptEmpSalariesFunc();

            });

        });



        function changeBranch(BrVal) {
            $.ajax({
                url: "GLHome.aspx/BranchSelectList",
                data: JSON.stringify({ BrId: BrVal }),
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {


                },
                error: function () {
                    alert("Error loading data! Please try again.");
                }
            });
        }



        function DptEmpSalariesFunc() {
            $.ajax({
                url: "GLHomeBudCharts.aspx/GetChartEmpSalariesinPrevMonthData",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {
                    chartDataEmpSalaries = data.d;


                },
                error: function () {
                    alert("Error loading data! Please try again.");
                }
            }).done(function () {
                // after complete loading data
                google.setOnLoadCallback(drawDptEmpSalarisChart);
                drawDptEmpSalarisChart();
            });
        }
        function drawDptEmpSalarisChart() {

            var data = google.visualization.arrayToDataTable(chartDataEmpSalaries);

            var options = {

                pointSize: 5
            };
            var barChart = new google.visualization.PieChart(document.getElementById('deptEmpSalarischart_div'));

            barChart.draw(data, options);

        }

        function BudgetAppChartFunc() {
            $.ajax({
                url: "GLHomeBudCharts.aspx/GetAppBudPieChart",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {
                    chartBudgetPieChart = data.d;


                },
                error: function () {
                    alert("Error loading data! Please try again.");
                }
            }).done(function () {
                // after complete loading data
                google.setOnLoadCallback(drawBudgetPieChart);
                drawBudgetPieChart();
            });
        }
        function drawBudgetPieChart() {

            var data = google.visualization.arrayToDataTable(chartBudgetPieChart);

            var options = {
                pointSize: 5
            };
            var barChart = new google.visualization.PieChart(document.getElementById('divPieChart_Dis'));

            barChart.draw(data, options);

        }
        //My work
        function BudgetPereAppChartFunc() {
            $.ajax({
                url: "GLHomeBudCharts.aspx/GetPerAppBudBarChart",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {

                    PerAppBudgetBarChart = data.d;
                },
                error: function () {
                    alert("Error loading data! Please try again.");
                }
            }).done(function () {
                // after complete loading data
                google.setOnLoadCallback(drawBudgetPerAppBarChart);
                //drawBudgetPerAppBarChart();
            });
        }
        function drawBudgetPerAppBarChart() {


            debugger
            var data = google.visualization.arrayToDataTable(PerAppBudgetBarChart);

            var materialOptions = {
                chart: {
                    title: 'Demand / SNE Allocation of Grant'
                },
                bars: 'horizontal',
                series: {
                    0: { color: 'Blue' },
                    1: { color: 'Green' }
                }
            };
            var barChart = new google.charts.Bar(document.getElementById('divBarChart_Dis'));

            barChart.draw(data, google.charts.Bar.convertOptions(materialOptions));

        }

        function BudgetAppEstabChartFunc() {
            $.ajax({
                url: "GLHomeBudCharts.aspx/GetAppEstConStackChart",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {

                    AppBudgetEstablStackChart = data.d;
                },
                error: function () {
                    alert("Error loading data! Please try again.");
                }
            }).done(function () {
                // after complete loading data
                google.setOnLoadCallback(drawBudgetAppEstabStackChart);
                //drawBudgetPerAppBarChart();
            });
        }
        function drawBudgetAppEstabStackChart() {


            debugger
            var data = google.visualization.arrayToDataTable(AppBudgetEstablStackChart);

            var options = {
                bars: 'Vertical',
                isStacked: true,
                series: {
                    0: { color: 'Blue' },
                    1: { color: 'Green' },
                    2: {color: 'Yellow'}
                }
            };
            var barChart = new google.visualization.BarChart(document.getElementById('divStackChart_Dis'));

            barChart.draw(data, options);

        }

        function BudgetAppnIncomeGrantChartFunc() {
            $.ajax({
                url: "GLHomeBudCharts.aspx/GetAppIncoGranttackChart",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {

                    AppBudgetIncomeGrantStackChart = data.d;
                },
                error: function () {
                    alert("Error loading data! Please try again.");
                }
            }).done(function () {
                // after complete loading data
                google.setOnLoadCallback(drawBudgetAppIncomeGrantStackChart);
                //drawBudgetPerAppBarChart();
            });
        }
        function drawBudgetAppIncomeGrantStackChart() {


            debugger
            var data = google.visualization.arrayToDataTable(AppBudgetIncomeGrantStackChart);

            var options = {
                isStacked: true,
                series: {
                    0: { color: 'Blue' },
                    1: { color: 'Green' }
                    
                }

            };
            var barChart = new google.visualization.BarChart(document.getElementById('divIncomeStackChart_Dis'));

            barChart.draw(data, options);

        }

        function BudgetAppCouncilChartFunc() {
            $.ajax({
                url: "GLHomeBudCharts.aspx/GetAppbudgetCouncilsStackChart",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {

                    AppBudgeCouncilStackChart = data.d;
                },
                error: function () {
                    alert("Error loading data! Please try again.");
                }
            }).done(function () {
                // after complete loading data
                google.setOnLoadCallback(drawBudgetAppCouncilStackChart);
                //drawBudgetPerAppBarChart();
            });
        }
        function drawBudgetAppCouncilStackChart() {


            debugger
            var data = google.visualization.arrayToDataTable(AppBudgeCouncilStackChart);

            var options = {
                bars: 'Vertical',
                isStacked: true,
                series: {
                    0: { color: 'Blue' },
                    1: { color: 'Green' },
                    2: {color: 'Yellow'}
                }
            };
            var barChart = new google.visualization.BarChart(document.getElementById('divSappCountackChart_Dis'));

            barChart.draw(data, options);

        }

        function BudgetBudgetvsVeriChartFunc() {
            $.ajax({
                url: "GLHomeBudCharts.aspx/GetAppbudgetbifructionStackChart",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {

                    PerAppBudgetbifrucBarChart = data.d;
                },
                error: function () {
                    alert("Error loading data! Please try again.");
                }
            }).done(function () {
                // after complete loading data
                google.setOnLoadCallback(drawBudgetvsVarisBarChart);
                //drawBudgetPerAppBarChart();
            });
        }
        function drawBudgetvsVarisBarChart() {
            var data = google.visualization.arrayToDataTable(PerAppBudgetbifrucBarChart);

            var materialOptions = {
                chart: {
                    title: 'Budget vs Utilization'
                },
                bars: 'horizontal',
                series: {
                    0: { color: 'Blue' },
                    1: { color: 'Green' }
                }
            };
            var barChart = new google.charts.Bar(document.getElementById('divSappbudgveriChart_Dis'));

            barChart.draw(data, materialOptions);

        }

    </script>










    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="fltr" />
    <uc1:Messages ID="ucMessage" runat="server" />

    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:ValidationSummary ID="ValidationSummary1" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                                ValidationGroup="fltr" />
                            <uc1:Messages ID="Messages1" runat="server" />
                        </div>
                    </div>


                    &nbsp;
                    <div id="chart_div">
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <div id="divBarChart_Dis" style="width: 100%; height: 500px;">
                                <%-- Here Chart Will Load --%>
                            </div>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <label>Distribution of Grant among all Councils</label>
                            <div id="divPieChart_Dis" style="width: 100%; height: 500px;">
                                <%-- Here Chart Will Load --%>
                            </div>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <label>Bifurcation of Grant to each Council</label>
                            <div id="divStackChart_Dis" style="width: 100%; height: 500px;">
                                <%-- Here Chart Will Load --%>
                            </div>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <label>Approved budget of all Councils</label>
                            <div id="divIncomeStackChart_Dis" style="width: 100%; height: 500px;">
                                <%-- Here Chart Will Load --%>
                            </div>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <label>Bifurcation of Approved budget of all Councils</label>
                            <div id="divSappCountackChart_Dis" style="width: 100%; height: 500px;">
                                <%-- Here Chart Will Load --%>
                            </div>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <div id="divSappbudgveriChart_Dis" style="width: 100%; height: 500px;">
                                <%-- Here Chart Will Load --%>
                            </div>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-md-4">
                            <asp:Label ID="Label1" runat="server" Text="Divisions:"></asp:Label><br />
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" AppendDataBoundItems="True" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Division</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4"></div>
                        <div class="col-md-4"></div>
                    </div>
                    &nbsp;

                    <asp:UpdatePanel ID="Disupnl" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="grdVouchers" runat="server" CssClass="table table-responsive-sm table-bordered" DataKeyNames="vrid" OnSelectedIndexChanged="grdVch_PageIndexChanged"
                                AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdVch_PageIndexChangin" OnRowDataBound="grdVoucher_RowDataBound"
                                EmptyDataText="There is no Voucher defined" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="vr_no" HeaderText="Sr no" />
                                    <asp:BoundField DataField="ref_no" HeaderText="Voucher no" />
                                    <asp:BoundField DataField="vr_nrtn" HeaderText="Naration" />
                                    <asp:BoundField DataField="vr_dt" HeaderText="Date" />
                                    <asp:BoundField DataField="status" HeaderText="Status" />
                                </Columns>
                                <HeaderStyle CssClass="grid_hdr" />
                                <RowStyle CssClass="grid_row" />
                                <AlternatingRowStyle CssClass="gridAlternateRow" />
                                <SelectedRowStyle CssClass="gridSelectedRow" />
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="searchBranchDropDown" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
