<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.ascx.cs" Inherits="RMS.UserControl.ReportViewer" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<rsweb:ReportViewer ID="rptViewer" runat="server" Width="98%" 
  ShowRefreshButton="False" AsyncRendering="False">
</rsweb:ReportViewer>
