<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="GLHomecopy.aspx.cs" Inherits="RMS.home.GLHomecopy" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="../assets/js/jquery.min.js"></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

    <script>
        var chartDataEmpSalaries;
        var RawalDataEmpSalaries;
        var GujDataEmpSalaries;
        var FaiDataEmpSalaries;
        var SarDataEmpSalaries;
        var MulDataEmpSalaries;
        var BulDataEmpSalaries;
        var DgDataEmpSalaries;
        var saDataEmpSalaries;
        var murDataEmpSalaries;
        google.load("visualization", "1", { packages: ["corechart"] });

        // Here We will fill chartData

        $(document).ready(function () {

            var brID = '<%=Session["BranchID"].ToString() %>';
            $('.searchbranchchange').val(brID);

            DptEmpSalariesFunc();
            RawalEmpSalariesFunc();
            GujranEmpSalariesFunc();
            FaisalEmpSalariesFunc();
            SarEmpSalariesFunc();
            MulEmpSalariesFunc();
            BulEmpSalariesFunc();
            DgEmpSalariesFunc();
            sahEmpSalariesFunc();
            murEmpSalariesFunc();
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



        //function DptEmpSalariesFunc() {
        //    $.ajax({
        //        url: "GLHome.aspx/GetChartEmpSalariesinPrevMonthData",
        //        data: "",
        //        dataType: "json",
        //        type: "POST",
        //        contentType: "application/json; chartset=utf-8",
        //        success: function (data) {
        //            chartDataEmpSalaries = data.d;


        //        },
        //        error: function () {
        //            alert("Error loading data! Please try again.");
        //        }
        //    }).done(function () {
        //        // after complete loading data
        //        google.setOnLoadCallback(drawDptEmpSalarisChart);
        //        drawDptEmpSalarisChart();
        //    });
        //}



        //function drawDptEmpSalarisChart() {

        //    var data = google.visualization.arrayToDataTable(chartDataEmpSalaries);

        //    var options = {

        //        pointSize: 5
        //    };
        //    var barChart = new google.visualization.ColumnChart(document.getElementById('deptEmpSalarischart_div'));

        //    barChart.draw(data, options);

        //}

        function DptEmpSalariesFunc() {
            $.ajax({
                url: "GLHomecopy.aspx/GetChartEmpSalariesinPrevMonthData",
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
            var barChart = new google.visualization.ColumnChart(document.getElementById('deptEmpSalarischart_div'));

            barChart.draw(data, options);

        }

        function RawalEmpSalariesFunc() {
            $.ajax({
                url: "GLHomecopy.aspx/RawalEmpSalariesinPrevMonthData",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {
                    RawalDataEmpSalaries = data.d;


                },
                error: function () {
                    alert("Error loading data! Please try again.");
                }
            }).done(function () {
                // after complete loading data
                google.setOnLoadCallback(RawalEmpSalarisChart);
                RawalEmpSalarisChart();
            });
        }



        function RawalEmpSalarisChart() {

            var data = google.visualization.arrayToDataTable(RawalDataEmpSalaries);

            var options = {

                pointSize: 5
            };
            var barChart = new google.visualization.ColumnChart(document.getElementById('RawalEmpSalarischart_div'));

            barChart.draw(data, options);

        }

        function GujranEmpSalariesFunc() {
            $.ajax({
                url: "GLHomecopy.aspx/GujranEmpSalariesinPrevMonthData",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {
                    GujDataEmpSalaries = data.d;


                },
                error: function () {
                    alert("Error loading data! Please try again.");
                }
            }).done(function () {
                // after complete loading data
                google.setOnLoadCallback(GujranEmpSalarisChart);
                GujranEmpSalarisChart();
            });
        }



        function GujranEmpSalarisChart() {

            var data = google.visualization.arrayToDataTable(GujDataEmpSalaries);

            var options = {

                pointSize: 5
            };
            var barChart = new google.visualization.ColumnChart(document.getElementById('GujranEmpSalarischart_div'));

            barChart.draw(data, options);

        }

        function FaisalEmpSalariesFunc() {
            $.ajax({
                url: "GLHomecopy.aspx/FaisEmpSalariesinPrevMonthData",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {
                    FaiDataEmpSalaries = data.d;


                },
                error: function () {
                    alert("Error loading data! Please try again.");
                }
            }).done(function () {
                // after complete loading data
                google.setOnLoadCallback(FaisEmpSalarisChart);
                FaisEmpSalarisChart();
            });
        }



        function FaisEmpSalarisChart() {

            var data = google.visualization.arrayToDataTable(FaiDataEmpSalaries);

            var options = {

                pointSize: 5
            };
            var barChart = new google.visualization.ColumnChart(document.getElementById('FaisEmpSalarischart_div'));

            barChart.draw(data, options);

        }

        function SarEmpSalariesFunc() {
            $.ajax({
                url: "GLHomecopy.aspx/SarEmpSalariesinPrevMonthData",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {
                    SarDataEmpSalaries = data.d;


                },
                error: function () {
                    alert("Error loading data! Please try again.");
                }
            }).done(function () {
                // after complete loading data
                google.setOnLoadCallback(SarEmpSalarisChart);
                SarEmpSalarisChart();
            });
        }



        function SarEmpSalarisChart() {

            var data = google.visualization.arrayToDataTable(SarDataEmpSalaries);

            var options = {

                pointSize: 5
            };
            var barChart = new google.visualization.ColumnChart(document.getElementById('SarEmpSalarischart_div'));

            barChart.draw(data, options);

        }

        function MulEmpSalariesFunc() {
            $.ajax({
                url: "GLHomecopy.aspx/MulEmpSalariesinPrevMonthData",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {
                    MulDataEmpSalaries = data.d;


                },
                error: function () {
                    alert("Error loading data! Please try again.");
                }
            }).done(function () {
                // after complete loading data
                google.setOnLoadCallback(MulEmpSalarisChart);
                MulEmpSalarisChart();
            });
        }

        function MulEmpSalarisChart() {

            var data = google.visualization.arrayToDataTable(MulDataEmpSalaries);

            var options = {

                pointSize: 5
            };
            var barChart = new google.visualization.ColumnChart(document.getElementById('MulEmpSalarischart_div'));

            barChart.draw(data, options);

        }

        function BulEmpSalariesFunc() {
            $.ajax({
                url: "GLHomecopy.aspx/BahEmpSalariesinPrevMonthData",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {
                    BulDataEmpSalaries = data.d;


                },
                error: function () {
                    alert("Error loading data! Please try again.");
                }
            }).done(function () {
                // after complete loading data
                google.setOnLoadCallback(BulEmpSalarisChart);
                BulEmpSalarisChart();
            });
        }



        function BulEmpSalarisChart() {

            var data = google.visualization.arrayToDataTable(BulDataEmpSalaries);

            var options = {

                pointSize: 3
            };
            var barChart = new google.visualization.ColumnChart(document.getElementById('BahEmpSalarischart_div'));

            barChart.draw(data, options);

        }

        function DgEmpSalariesFunc() {
            $.ajax({
                url: "GLHomecopy.aspx/DgEmpSalariesinPrevMonthData",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {
                    DgDataEmpSalaries = data.d;


                },
                error: function () {
                    alert("Error loading data! Please try again.");
                }
            }).done(function () {
                // after complete loading data
                google.setOnLoadCallback(DgEmpSalarisChart);
                DgEmpSalarisChart();
            });
        }



        function DgEmpSalarisChart() {

            var data = google.visualization.arrayToDataTable(DgDataEmpSalaries);

            var options = {

                pointSize: 5
            };
            var barChart = new google.visualization.ColumnChart(document.getElementById('DgEmpSalarischart_div'));

            barChart.draw(data, options);

        }

        function sahEmpSalariesFunc() {
            $.ajax({
                url: "GLHomecopy.aspx/SaEmpSalariesinPrevMonthData",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {
                    saDataEmpSalaries = data.d;


                },
                error: function () {
                    alert("Error loading data! Please try again.");
                }
            }).done(function () {
                // after complete loading data
                google.setOnLoadCallback(sahEmpSalarisChart);
                sahEmpSalarisChart();
            });
        }



        function sahEmpSalarisChart() {

            var data = google.visualization.arrayToDataTable(saDataEmpSalaries);

            var options = {

                pointSize: 5
            };
            var barChart = new google.visualization.ColumnChart(document.getElementById('SaEmpSalarischart_div'));

            barChart.draw(data, options);

        }

        function murEmpSalariesFunc() {
            $.ajax({
                url: "GLHomecopy.aspx/MurEmpSalariesinPrevMonthData",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; chartset=utf-8",
                success: function (data) {
                    murDataEmpSalaries = data.d;


                },
                error: function () {
                    alert("Error loading data! Please try again.");
                }
            }).done(function () {
                // after complete loading data
                google.setOnLoadCallback(murEmpSalarisChart);
                murEmpSalarisChart();
            });
        }



        function murEmpSalarisChart() {

            var data = google.visualization.arrayToDataTable(murDataEmpSalaries);

            var options = {

                pointSize: 5
            };
            var barChart = new google.visualization.ColumnChart(document.getElementById('MurEmpSalarischart_div'));

            barChart.draw(data, options);

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
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <label style="background-color:blue;color:white; font-size:20px">Pucar Head Office</label>
                            <asp:GridView ID="grdVoucherLHR" DataKeyNames="vrid" runat="server" 
                                AutoGenerateColumns="False" AllowPaging="true"
                                Width="100%" PageSize="20"  CssClass="table table-responsive-sm"
                                EmptyDataText="No voucher found.">
                                <Columns>
                                    <asp:BoundField DataField="Ref_no" HeaderText="Voucher#" ItemStyle-Width="70" />

                                    <asp:BoundField DataField="vr_dt" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false" HeaderText="Date">
                                        <ItemStyle Wrap="false" Width="80" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vr_nrtn" HeaderText="Narration" />
                                    <%--<asp:TemplateField HeaderText="Print" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstatus" runat="server" Visible="false" Text='<%# Bind("status") %>'>'></asp:Label>
                                            <asp:LinkButton ID="btnprint" runat="server" OnClick="lnkPrint_Click" Text="Print"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <%--<asp:CommandField ShowSelectButton="true" ButtonType="Link" ItemStyle-Width="70" />--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <label style="background-color:blue;color:white; font-size:20px">Rawalpindi Arts Council</label>
                            <asp:GridView ID="grdVoucherRWP" DataKeyNames="vrid" runat="server" 
                                AutoGenerateColumns="False" AllowPaging="true"
                                Width="100%" PageSize="20"  CssClass="table table-responsive-sm"
                                EmptyDataText="No voucher found.">
                                <Columns>
                                    <asp:BoundField DataField="Ref_no" HeaderText="Voucher#" ItemStyle-Width="70" />

                                    <asp:BoundField DataField="vr_dt" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false" HeaderText="Date">
                                        <ItemStyle Wrap="false" Width="80" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vr_nrtn" HeaderText="Narration" />
                                    <%--<asp:TemplateField HeaderText="Print" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstatus" runat="server" Visible="false" Text='<%# Bind("status") %>'>'></asp:Label>
                                            <asp:LinkButton ID="btnprint" runat="server" OnClick="lnkPrint_Click" Text="Print"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <%--<asp:CommandField ShowSelectButton="true" ButtonType="Link" ItemStyle-Width="70" />--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                        &nbsp;
                        <div class="row">
                            <div class="col-lg-6 col-md-6 col-sm-6">
                            <label style="background-color:blue;color:white; font-size:20px">Gujranwala Arts Council</label>
                            <asp:GridView ID="grdVoucherGuj" DataKeyNames="vrid" runat="server" 
                                AutoGenerateColumns="False" AllowPaging="true"
                                Width="100%"  PageSize="20"  CssClass="table table-responsive-sm"
                                EmptyDataText="No voucher found.">
                                <Columns>
                                    <asp:BoundField DataField="Ref_no" HeaderText="Voucher#" ItemStyle-Width="70" />

                                    <asp:BoundField DataField="vr_dt" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false" HeaderText="Date">
                                        <ItemStyle Wrap="false" Width="80" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vr_nrtn" HeaderText="Narration" />
                                    <%--<asp:TemplateField HeaderText="Print" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstatus" runat="server" Visible="false" Text='<%# Bind("status") %>'>'></asp:Label>
                                            <asp:LinkButton ID="btnprint" runat="server" OnClick="lnkPrint_Click" Text="Print"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <%--<asp:CommandField ShowSelectButton="true" ButtonType="Link" ItemStyle-Width="70" />--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <label style="background-color:blue;color:white; font-size:20px">Faisalabad Arts Council</label>
                            <asp:GridView ID="grdVoucherFai" DataKeyNames="vrid" runat="server" 
                                AutoGenerateColumns="False" AllowPaging="true"
                                Width="100%"  PageSize="20"  CssClass="table table-responsive-sm"
                                EmptyDataText="No voucher found.">
                                <Columns>
                                    <asp:BoundField DataField="Ref_no" HeaderText="Voucher#" ItemStyle-Width="70" />

                                    <asp:BoundField DataField="vr_dt" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false" HeaderText="Date">
                                        <ItemStyle Wrap="false" Width="80" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vr_nrtn" HeaderText="Narration" />
                                    <%--<asp:TemplateField HeaderText="Print" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstatus" runat="server" Visible="false" Text='<%# Bind("status") %>'>'></asp:Label>
                                            <asp:LinkButton ID="btnprint" runat="server" OnClick="lnkPrint_Click" Text="Print"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <%--<asp:CommandField ShowSelectButton="true" ButtonType="Link" ItemStyle-Width="70" />--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                        </div>
                    &nbsp;
                        <div class="row">
                            <div class="col-lg-6 col-md-6 col-sm-6">
                            <label style="background-color:blue;color:white; font-size:20px">Sargodha Arts Council</label>
                            <asp:GridView ID="grdVoucherSar" DataKeyNames="vrid" runat="server" 
                                AutoGenerateColumns="False" AllowPaging="true"
                                Width="100%"  PageSize="20"  CssClass="table table-responsive-sm"
                                EmptyDataText="No voucher found.">
                                <Columns>
                                    <asp:BoundField DataField="Ref_no" HeaderText="Voucher#" ItemStyle-Width="70" />

                                    <asp:BoundField DataField="vr_dt" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false" HeaderText="Date">
                                        <ItemStyle Wrap="false" Width="80" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vr_nrtn" HeaderText="Narration" />
                                    <%--<asp:TemplateField HeaderText="Print" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstatus" runat="server" Visible="false" Text='<%# Bind("status") %>'>'></asp:Label>
                                            <asp:LinkButton ID="btnprint" runat="server" OnClick="lnkPrint_Click" Text="Print"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <%--<asp:CommandField ShowSelectButton="true" ButtonType="Link" ItemStyle-Width="70" />--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <label style="background-color:blue;color:white; font-size:20px">Multan Arts Council</label>
                            <asp:GridView ID="grdVoucherMul" DataKeyNames="vrid" runat="server" 
                                AutoGenerateColumns="False" AllowPaging="true"
                                Width="100%"  PageSize="20"  CssClass="table table-responsive-sm"
                                EmptyDataText="No voucher found.">
                                <Columns>
                                    <asp:BoundField DataField="Ref_no" HeaderText="Voucher#" ItemStyle-Width="70" />

                                    <asp:BoundField DataField="vr_dt" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false" HeaderText="Date">
                                        <ItemStyle Wrap="false" Width="80" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vr_nrtn" HeaderText="Narration" />
                                    <%--<asp:TemplateField HeaderText="Print" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstatus" runat="server" Visible="false" Text='<%# Bind("status") %>'>'></asp:Label>
                                            <asp:LinkButton ID="btnprint" runat="server" OnClick="lnkPrint_Click" Text="Print"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <%--<asp:CommandField ShowSelectButton="true" ButtonType="Link" ItemStyle-Width="70" />--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                        </div>
                    &nbsp;
                        <div class="row">
                            <div class="col-lg-6 col-md-6 col-sm-6">
                            <label style="background-color:blue;color:white; font-size:20px">Bahawalpur Arts Council</label>
                            <asp:GridView ID="grdVoucherBaw" DataKeyNames="vrid" runat="server" 
                                AutoGenerateColumns="False" AllowPaging="true"
                                Width="100%"  PageSize="20"  CssClass="table table-responsive-sm"
                                EmptyDataText="No voucher found.">
                                <Columns>
                                    <asp:BoundField DataField="Ref_no" HeaderText="Voucher#" ItemStyle-Width="70" />

                                    <asp:BoundField DataField="vr_dt" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false" HeaderText="Date">
                                        <ItemStyle Wrap="false" Width="80" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vr_nrtn" HeaderText="Narration" />
                                    <%--<asp:TemplateField HeaderText="Print" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstatus" runat="server" Visible="false" Text='<%# Bind("status") %>'>'></asp:Label>
                                            <asp:LinkButton ID="btnprint" runat="server" OnClick="lnkPrint_Click" Text="Print"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <%--<asp:CommandField ShowSelectButton="true" ButtonType="Link" ItemStyle-Width="70" />--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <label style="background-color:blue;color:white; font-size:20px">DG Khan Arts Council</label>
                             <asp:GridView ID="grdVoucherDGK" DataKeyNames="vrid" runat="server" 
                                AutoGenerateColumns="False" AllowPaging="true"
                                Width="100%"  PageSize="20"  CssClass="table table-responsive-sm"
                                EmptyDataText="No voucher found.">
                                <Columns>
                                    <asp:BoundField DataField="Ref_no" HeaderText="Voucher#" ItemStyle-Width="70" />

                                    <asp:BoundField DataField="vr_dt" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false" HeaderText="Date">
                                        <ItemStyle Wrap="false" Width="80" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vr_nrtn" HeaderText="Narration" />
                                    <%--<asp:TemplateField HeaderText="Print" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstatus" runat="server" Visible="false" Text='<%# Bind("status") %>'>'></asp:Label>
                                            <asp:LinkButton ID="btnprint" runat="server" OnClick="lnkPrint_Click" Text="Print"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <%--<asp:CommandField ShowSelectButton="true" ButtonType="Link" ItemStyle-Width="70" />--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                        </div>
                    &nbsp;
                        <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <label style="background-color:blue;color:white; font-size:20px">Sahiwal Arts Council</label>
                            <asp:GridView ID="grdVoucherSah" DataKeyNames="vrid" runat="server" 
                                AutoGenerateColumns="False" AllowPaging="true"
                                Width="100%"  PageSize="20"  CssClass="table table-responsive-sm"
                                EmptyDataText="No voucher found.">
                                <Columns>
                                    <asp:BoundField DataField="Ref_no" HeaderText="Voucher#" ItemStyle-Width="70" />

                                    <asp:BoundField DataField="vr_dt" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false" HeaderText="Date">
                                        <ItemStyle Wrap="false" Width="80" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vr_nrtn" HeaderText="Narration" />
                                    <%--<asp:TemplateField HeaderText="Print" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstatus" runat="server" Visible="false" Text='<%# Bind("status") %>'>'></asp:Label>
                                            <asp:LinkButton ID="btnprint" runat="server" OnClick="lnkPrint_Click" Text="Print"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <%--<asp:CommandField ShowSelectButton="true" ButtonType="Link" ItemStyle-Width="70" />--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <label style="background-color:blue;color:white; font-size:20px">Murree Arts Council</label>
                            <asp:GridView ID="grdVoucherMur" DataKeyNames="vrid" runat="server" 
                                AutoGenerateColumns="False" AllowPaging="true"
                                Width="100%"  PageSize="20"  CssClass="table table-responsive-sm"
                                EmptyDataText="No voucher found.">
                                <Columns>
                                    <asp:BoundField DataField="Ref_no" HeaderText="Voucher#" ItemStyle-Width="70" />

                                    <asp:BoundField DataField="vr_dt" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false" HeaderText="Date">
                                        <ItemStyle Wrap="false" Width="80" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vr_nrtn" HeaderText="Narration" />
                                    <%--<asp:TemplateField HeaderText="Print" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstatus" runat="server" Visible="false" Text='<%# Bind("status") %>'>'></asp:Label>
                                            <asp:LinkButton ID="btnprint" runat="server" OnClick="lnkPrint_Click" Text="Print"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <%--<asp:CommandField ShowSelectButton="true" ButtonType="Link" ItemStyle-Width="70" />--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-4">
                            <%--<asp:Label ID="Label1" runat="server" Text="Divisions:"></asp:Label><br />
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" AppendDataBoundItems="True" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Division</asp:ListItem>
                            </asp:DropDownList>--%>
                        </div>
                        <div class="col-md-4"></div>
                        <div class="col-md-4"></div>
                    </div>
                    <%--<asp:UpdatePanel ID="Disupnl" runat="server">
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
                    </asp:UpdatePanel>--%>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
