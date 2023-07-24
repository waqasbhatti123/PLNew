<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmpMgtReport.aspx.cs" Inherits="RMS.profile.EmpMgtReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Employee Detail Report</title>
    <%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
    <link href="../assets/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12">
                    <div class="card card-shedow mb-4">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <asp:ValidationSummary ID="main" CssClass="text-warning" runat="server" DisplayMode="List" ValidationGroup="main" />
                                    <asp:ValidationSummary ID="ValidationSummary1" CssClass="text-warning" runat="server" DisplayMode="List" ValidationGroup="main2" />
                                    <uc1:messages id="ucMessage" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <asp:Panel ID="Panel2" runat="server" Width="100%" Height="600">
                                        <rsweb:ReportViewer ID="reportViewer" runat="server" Width="100%" Height="580px">
                                        </rsweb:ReportViewer>
                                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
