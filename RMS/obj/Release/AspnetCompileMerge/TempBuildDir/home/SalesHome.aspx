<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="SalesHome.aspx.cs" Inherits="RMS.home.SalesHome" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
    ValidationGroup="fltr"/>
  <uc1:Messages ID="ucMessage" runat="server" />
  
  <table align="center" cellpadding="4" cellspacing="4" width="98%" height="350px">
    <tr>
  
    <td valign="bottom">
        
       
  <%--<img src="../images/banner_hrms.jpg" id="imgBanner" alt="" width="100%" />     --%>
        
    </td>
    
    

    </tr>
  </table>
 
</asp:Content>
