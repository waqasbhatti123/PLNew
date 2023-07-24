<%@  Language="C#" AutoEventWireup="true" CodeBehind="HomeSelection.aspx.cs" Title="Home Page" Inherits="RMS.HomeSelection" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>



<!DOCTYPE html>
<html lang="en">

<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Home Page</title>
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico">
    <!-- google font -->
    <link href="https://fonts.googleapis.com/css?family=Poppins:300,400,500,600,700" rel="stylesheet" type="text/css" />
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" type="text/css">
    <link href="assets/css/font-awesome.min.css" rel="stylesheet" type="text/css">
    <link href="assets/css/ionicons.css" rel="stylesheet" type="text/css">
    <link href="assets/css/simple-line-icons.css" rel="stylesheet" type="text/css">
    <link href="assets/css/jquery.mCustomScrollbar.css" rel="stylesheet">
    <link href="assets/css/weather-icons.min.css" rel="stylesheet">
    <link href="assets/css/style.css" rel="stylesheet">
    <link href="assets/css/responsive.css" rel="stylesheet">

    <style>
        #headerPic{
            background-image: url(./empix/pucarlogo.jpeg);
            background-repeat: no-repeat;
            background-size:100% 100%;
            
            
        }
    </style>
</head>

<body>
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>



        <div class="row ">
            <div class="col-lg-12 col-md-12 col-sm-12">
                <div class="card  mb-4">
                    <div class="card-header text-white   border-0" id="headerPic" style="height: 300px;">
                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12">
                                <div style="float: right;">
                                    <em>
                                        <asp:LinkButton ID="btnLogout" runat="server" OnClick="btnLogout_Click" ForeColor="White">Logout</asp:LinkButton>
                                    </em>
                                    <asp:Label ID="Label1" runat="server" Text="  |  "></asp:Label>
                                    <em>
                                        <asp:LinkButton ID="btnChangePassword" runat="server" OnClick="btnChangePassword_Click" ForeColor="White">Change Password</asp:LinkButton>
                                    </em>
                                    <asp:Label ID="lblNameWelcome" runat="server" Visible="false" />
                                    <asp:DropDownList ID="ddlBranch" runat="server" AppendDataBoundItems="True" Visible="false"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-1 col-md-1 col-sm-1">
                                
                            </div>
                            <div class="col-lg-8 col-md-8 col-sm-8">
                                <div class="media p-4">
                                    <div class="media-body">
                                        <h1 style="font-size: 30px; color: white;">
                                            <%--<asp:Label ID="lblCompName" runat="server" Text="" Visible="true"></asp:Label>--%>
                                        </h1>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>



        <div class="row pl-2 pr-2">
            <div class="col-lg-4 col-md-4 col-sm-4">
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <i class="icon-layers text-primary f30"></i>
                            </div>
                            <div class="col-lg-10 col-md-10 col-sm-10">
                                <h5 class="card-title m-0">Admin Panel </h5>
                                <p class="f12 mb-0">
                                    <asp:LinkButton ID="btnAdmin" runat="server" OnClick="btnAdmin_Click" CssClass="boxes">Go to admin panel module</asp:LinkButton>
                                </p>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12">
                                <p class="card-text">
                                    The Administrator application, also known as the Back-end, Admin Panel or Control Panel, is the interface where administrators and other site officials with appropriate privileges can manipulate the user functionality.
                                </p>
                                <p class="card-text">
                                    <small class="text-muted">Last updated 10 mins ago</small>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-4 col-md-4 col-sm-4">
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <i class="icon-notebook text-primary f30"></i>
                            </div>
                            <div class="col-lg-10 col-md-10 col-sm-10">
                                <h5 class="card-title m-0">Account & Finance</h5>
                                <p class="f12 mb-0">
                                    <asp:LinkButton ID="btnGL" runat="server" OnClick="btnGL_Click" CssClass="boxes">Go to general ledger module</asp:LinkButton>
                                </p>
                            </div>
                        </div>
                        <br />
                        <div class="row">

                            <div class="col-lg-12 col-md-12 col-sm-12">
                                <p class="card-text">
                                    A general ledger represents the record-keeping system for a company's financial data with debit and credit account records validated by a trial balance. The general ledger provides a record of each financial transaction during the life of an operating company.
                                </p>
                                <p class="card-text">
                                    <small class="text-muted">Last updated 15 mins ago</small>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-4 col-md-4 col-sm-4">
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <i class="icon-people text-primary f30"></i>
                            </div>
                            <div class="col-lg-10 col-md-10 col-sm-10">
                                <h5 class="card-title m-0">Admin & HR Module </h5>
                                <p class="f12 mb-0">
                                    <asp:LinkButton ID="btnPayroll" runat="server" OnClick="btnPayroll_Click" CssClass="boxes">Go to admin & HR module</asp:LinkButton>
                                </p>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12">
                                <p class="card-text">
                                    An admin and hr system calculates the amount you owe your employees based on factors such as the time they worked, their hourly wages or salaries, and whether they took vacation or holiday time during the pay period.
                                </p>
                                <p class="card-text">
                                    <small class="text-muted">Last updated 3 mins ago</small>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


        </div>

















        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12">
                <div class="container-fluid">

                    <!--state widget start-->

                    <div class="row" style="display: none;">
                        <div class="col-xl-3 col-sm-10 mb-4">
                            <div class="card ">
                                <div class="card-body" style="height: 175px;">
                                    <div class="row">
                                        <div class="col-3">
                                            <i class=" icon-bag text-success f30"></i>
                                        </div>
                                        <div class="col-9">
                                            <h6 class="m-0">Inventory</h6>
                                            <p class="f12 mb-0">
                                                <asp:LinkButton ID="btnPay" runat="server" OnClick="btnPayable_Click" CssClass="boxes">Go to inventory module</asp:LinkButton>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xl-3 col-sm-6 mb-4">
                            <div class="card">
                                <div class="card-body" style="height: 75px;">
                                    <div class="row">
                                        <div class="col-3">
                                            <i class="icon-login text-primary f30"></i>
                                        </div>
                                        <div class="col-9">
                                            <h6 class="m-0">Sales</h6>
                                            <p class="f12 mb-0">
                                                <asp:LinkButton ID="btnSales" runat="server" OnClick="btnSales_Click" CssClass="boxes">Go to sales module</asp:LinkButton>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xl-3 col-sm-6 mb-4">
                            <div class="card">
                                <div class="card-body" style="height: 75px;">
                                    <div class="row">
                                        <div class="col-3">
                                            <i class="icon-logout text-info f30"></i>
                                        </div>
                                        <div class="col-9">
                                            <h6 class="m-0">Purchase</h6>
                                            <p class="f12 mb-0">
                                                <asp:LinkButton ID="btnPruchase" runat="server" OnClick="btnPruchase_Click" CssClass="boxes">Go to purchase module</asp:LinkButton>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xl-3 col-sm-6 mb-4">
                            <div class="card">
                                <div class="card-body" style="height: 75px;">
                                    <div class="row">
                                        <asp:LinkButton ID="btnInquiry" runat="server" CssClass="boxes"></asp:LinkButton><%--OnClick="btnInquiry_Click"--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xl-3 col-sm-6 mb-4">
                            <div class="card ">
                                <div class="card-body" style="height: 75px;">
                                    <div class="row">
                                        <asp:LinkButton ID="LinkButton2" runat="server" CssClass="boxes"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!--state widget end-->
                </div>
            </div>
        </div>
    </form>

    <script type="text/javascript" src="assets/js/jquery.min.js"></script>
    <script type="text/javascript" src="assets/js/popper.min.js"></script>
    <script type="text/javascript" src="assets/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="assets/js/jquery.mCustomScrollbar.concat.min.js"></script>
    <script type="text/javascript" src="assets/js/jquery.dcjqaccordion.2.7.js"></script>
    <script src="assets/js/custom.js" type="text/javascript"></script>
</body>
</html>

