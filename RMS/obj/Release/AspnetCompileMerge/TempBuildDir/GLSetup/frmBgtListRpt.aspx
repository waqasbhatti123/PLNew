<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"
 AutoEventWireup="true" CodeBehind="frmBgtListRpt.aspx.cs" Inherits="RMS.GLSetup.frmBgtListRpt"
 Culture="auto" UICulture="auto" %>
<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <p>
        <asp:Button ID="btnGenerat" runat="server" Text="Report" OnClick="btnGenerat_Click" /> 
    </p>
    <br />
   <asp:Panel ID="pnlMain" runat="server" Width="99%" Height="600">
       <rsweb:ReportViewer ID="viewer" runat="server" Width="100%" Height="580px">
       </rsweb:ReportViewer>
   </asp:Panel>

<%--       <td><asp:ImageButton ID="btnGenerat" runat="server"  ImageUrl="~/images/btn_generate.png"  OnClick="btnGenerat_Click"
       onMouseOver="this.src='../images/btn_generate_m.png'" onMouseOut="this.src='../images/btn_generate.png'" />
   
     </fieldset>
         <rsweb:ReportViewer ID="reportViewer" runat="server" Width="80%">
    </rsweb:ReportViewer>--%>
</asp:Content>
