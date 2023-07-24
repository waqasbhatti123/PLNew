<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="Home.aspx.cs" Inherits="RMS.home.Home" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%-- Here We need to write some js code for load google chart with database data --%>

    <script src="../assets/js/jquery.min.js"></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

    <script>
        var chartDataEmp; // globar variable for hold chart data
        var ChartEmpScale; // globar variable for hold chart data
        var ChartEmpQuali; // globar variable for hold chart data
        var chartDataEmpSalaries; // globar variable for hold chart data
        google.load("visualization", "1", { packages: ["corechart"] });

        // Here We will fill chartData

        $(document).ready(function () {

            var brID = '<%=Session["BranchID"].ToString() %>';
            $('.searchbranchchange').val(brID);



            DptEmpFunc();
            DptEmpSalariesFunc();
            DptEmpFuncScaleWise();
            DptEmpFuncQualiWise();


            $('.searchbranchchange').change(function () {

                brID = $('.searchbranchchange').val();
                changeBranch(brID);

                DptEmpFunc();
                DptEmpFuncScaleWise();
                DptEmpFuncQualiWise();

            });

        });



        function changeBranch(BrVal) {
            $.ajax({
                url: "Home.aspx/BranchSelectList",
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


        function DptEmpFunc() {
            $.ajax({
                url: "Home.aspx/GetChartEmpData",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {
                    chartDataEmp = data.d;
                },
                error: function () {
                    alert("Error loading data! Please try again.");
                }
            }).done(function () {
                // after complete loading data
                google.setOnLoadCallback(drawDptEmpChart);
                drawDptEmpChart();
            });
        }
        function DptEmpFuncScaleWise() {
            $.ajax({
                url: "Home.aspx/GetChartEmpDataScaleWise",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {
                    ChartEmpScale = data.d;
                },
                error: function () {
                    alert("Error loading data! Please try again.");
                }
            }).done(function () {
                // after complete loading data
                google.setOnLoadCallback(drawScaleEmpChart);
                drawScaleEmpChart();
            });
        }
        function DptEmpFuncQualiWise() {
            $.ajax({
                url: "Home.aspx/GetChartEmpDataQualiWise",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {
                    ChartEmpQuali = data.d;
                },
                error: function () {
                    alert("Error loading data! Please try again.");
                }
            }).done(function () {
                // after complete loading data
                google.setOnLoadCallback(drawQualiEmpChart);
                drawQualiEmpChart();
            });
        }
        //function DptEmpFunc() {
        //    debugger
        //    $.ajax({
        //        url: "Home.aspx/GetChartEmpData1",
        //        data: "",
        //        dataType: "json",
        //        type: "POST",
        //        contentType: "application/json; chartset=utf-8",
        //        success: function (data) {
        //            chartDataEmp = data.d;

        //        },
        //        error: function () {
        //            alert("Error loading data! Please try again.");
        //        }
        //    }).done(function () {
        //        // after complete loading data
        //        google.setOnLoadCallback(drawDptEmpChart);
        //        drawDptEmpChart();
        //    });
        //}
        function DptEmpSalariesFunc() {
            debugger
            $.ajax({
                url: "Home.aspx/GetChartEmpSalariesData",
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



        function drawDptEmpChart() {

            var data = google.visualization.arrayToDataTable(chartDataEmp);

            var options = {

                pointSize: 5,
                seriesType: "bars",
                series: { 3: { type: "line" } }
            };
            var barChart = new google.visualization.ColumnChart(document.getElementById('deptEmpchart_div'));

            barChart.draw(data, options);

        }

        function drawScaleEmpChart() {

            var data = google.visualization.arrayToDataTable(ChartEmpScale);
            
            var options = {

                pointSize: 5,
                seriesType: "bars",
                series: { 3: { title: "line" } }
                
            };
            var barChart = new google.visualization.PieChart(document.getElementById('EmpScaleWise'));

            barChart.draw(data, options);

        }
        function drawQualiEmpChart() {

            var data = google.visualization.arrayToDataTable(ChartEmpQuali);
            
            var options = {

                pointSize: 5,
                seriesType: "bars",
                series: { 3: { title: "line" } }
                
            };
            var barChart = new google.visualization.ColumnChart(document.getElementById('EmpQualiWise'));

            barChart.draw(data, options);

        }

        function drawDptEmpSalarisChart() {

            var data = google.visualization.arrayToDataTable(chartDataEmpSalaries);

            var options = {

                pointSize: 5
            };
            var barChart = new google.visualization.ColumnChart(document.getElementById('deptEmpSalarischart_div'));

            barChart.draw(data, options);

        }

    </script>


    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                                ValidationGroup="fltr" />
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <asp:Label ID="Label1" runat="server" Text="Divisions:"></asp:Label><br />
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4"></div>
                        <div class="col-md-4"></div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <label><b>Department Wise</b></label>
                            <div id="deptEmpchart_div" style="width: 100%; height: 500px;">
                                <%-- Here Chart Will Load --%>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <label><b>Scale Wise</b></label>
                            <div id="EmpScaleWise" style="width: 100%; height: 500px;">
                                <%-- Here Chart Will Load --%>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <label><b>Qualification Wise</b></label>
                            <div id="EmpQualiWise" style="width: 100%; height: 500px;">
                                <%-- Here Chart Will Load --%>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-12">
                            <div id="deptEmpSalarischart_div" style="width: 100%; height: 500px;">
                                <%-- Here Chart Will Load --%>
                            </div>
                        </div>
                    </div>

                    <%--<div id="deptEmpSalarischart_div" style="width: 100%; height: 500px;">
                        <%-- Here Chart Will Load 
                    </div>--%>




                </div>
            </div>
        </div>
    </div>


    <%-- <table align="center" cellpadding="4" cellspacing="4" width="98%" height="350px">
    <tr>
  
    <td valign="bottom">
        
       
  <%--<img src="../images/banner_hrms.jpg" id="imgBanner" alt="" width="100%" />
        
    </td>
    
    

    </tr>
  </table>--%>
</asp:Content>
