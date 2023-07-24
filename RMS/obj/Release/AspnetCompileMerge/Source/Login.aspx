<%@  Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Title="Login" Inherits="RMS.Login" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>


<!DOCTYPE html>
<html lang="en">

<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Login</title>
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico">
    <!-- google font -->
    <link href="https://fonts.googleapis.com/css?family=Poppins:300,400,500,600,700" rel="stylesheet" type="text/css" />
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" type="text/css">
    <link href="assets/css/font-awesome.min.css" rel="stylesheet" type="text/css">
    <link href="assets/css/ionicons.css" rel="stylesheet" type="text/css">
    <link href="assets/css/simple-line-icons.css" rel="stylesheet" type="text/css">
    <link href="assets/css/jquery.mCustomScrollbar.css" rel="stylesheet">
    <link href="assets/css/style.css" rel="stylesheet">
    <link href="assets/css/responsive.css" rel="stylesheet">
</head>

<body style="background-image: url('assets/images/background-login.png'); overflow: hidden; background-repeat: no-repeat; background-size: cover; background-position: center;">
    <div class="sufee-login d-flex align-content-center flex-wrap">
        <div class="container">
            <div class="login-content">

                <div class="login-form" style="background-color: white">

                    <div class="row mb-45">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                             <div class="logo">
                        <a href="#">
                            <strong class="logo_icon">
                                <img alt="" src="images/logo.png" width="120px" height="120px">
                            </strong>
                            <span class="logo-default">
                                <img alt="" src="images/logo.png"  width="120px" height="120px">
                            </span>
                        </a>
                    </div>
</div>
     </div>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                             <form runat="server">
                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                        <div class="form-group">
                            <label>User Id</label>
                            <asp:TextBox ID="txtUSERID" runat="server" CssClass="form-control" Placeholder="User Id">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUSERID" ErrorMessage="User Id is required" Display="None" Font-Bold="False" ValidationGroup="submit">
                            </asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <label>Password</label>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" class="form-control" placeholder="Password">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required" Display="None" Font-Bold="False" ValidationGroup="submit">
                            </asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <asp:ValidationSummary ID="vs1" runat="server" ValidationGroup="submit" CssClass="text-warning" />
                            <span>
                                <uc1:Messages Visible="false" ID="ucMessage" runat="server" />
                            </span>
                        </div>
                        <div class="checkbox">
                            <label>
                                <input type="checkbox">
                                Remember Me
                           
                            </label>
                            <%--<label class="pull-right">
                                <a href="#">Forgotten Password?</a>
                            </label>--%>
                        </div>
                        <asp:Button runat="server" ID="btnLogin" OnClick="btnLogin_Click" class="btn btn-info btn-flat m-b-30 m-t-30" ValidationGroup="submit" Text="Sign In"></asp:Button>
                    </form>
</div>
     </div>
                   

                   

                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript" src="assets/js/jquery.min.js"></script>
    <script type="text/javascript" src="assets/js/popper.min.js"></script>
    <script type="text/javascript" src="assets/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="assets/js/jquery.dcjqaccordion.2.7.js"></script>
    <script src="assets/js/custom.js" type="text/javascript"></script>
</body>

</html>

