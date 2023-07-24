<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewLedgerCard.aspx.cs" Inherits="RMS.GLSetup.ViewLedgerCard" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" 
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<link rel="Stylesheet" href="../cs/style.css" />
    <title>Ledger Card View</title>
</head>
<body style="background-color:#F2F2F2;">
    <form id="form1" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
       <uc1:Messages ID="ucMessage" runat="server" />
       <br />
       
       <asp:Panel ID="pnlMain" runat="server" Width="99%" Height="600">
           <rsweb:ReportViewer ID="viewer" runat="server" Width="100%" Height="580px">
           </rsweb:ReportViewer>
       </asp:Panel>
        
    </form>
</body>
</html>
