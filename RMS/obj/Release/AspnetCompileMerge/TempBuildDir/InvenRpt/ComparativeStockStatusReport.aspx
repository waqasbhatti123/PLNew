<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="ComparativeStockStatusReport.aspx.cs" Inherits="RMS.InvenRpt.ComparativeStockStatusReport" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" 
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function promptFunc() {
        return confirm("Are your sure that you want to cancel this Material Note.");
    }
    
</script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
    ValidationGroup="main"/>
  <uc1:Messages ID="ucMessage" runat="server" />
  <fieldset class="fieldSet" >
  <legend >
 
  </legend>
  <br />
<div style="float:left;">


  
  <asp:UpdatePanel ID="uPanel" runat="server" UpdateMode="Conditional">
  <ContentTemplate>

  
<table cellpadding="4" cellspacing="4">

<tr>

<td>

</td>
<td>
<div style="float:left;">
<asp:Label ID="lblYear" runat="server" Text="Fin Year:" Width="60px">
</asp:Label>
<br /><br />
<%--<asp:Label ID="lblMonth" runat="server" Text="Month:" Width="60px">
</asp:Label>--%>
</div>
  <div style="float:left">
  <asp:DropDownList ID="ddlYear" runat="server" AppendDataBoundItems="true" Width="110px">
  <asp:ListItem Selected="True" Text="Select Year" Value="0">
  </asp:ListItem>
  </asp:DropDownList>

     <%--    <br />         
  
  <asp:DropDownList ID="ddlMonth" runat="server" Width="110px">
  <asp:ListItem Selected="True" Text="Select Month" Value="0">
  </asp:ListItem>
  <asp:ListItem Value="January">January</asp:ListItem>
  <asp:ListItem Value="February">February</asp:ListItem>
  <asp:ListItem Value="March">March</asp:ListItem>
  <asp:ListItem Value="April">April</asp:ListItem>
  <asp:ListItem Value="May">May</asp:ListItem>
  <asp:ListItem Value="June">June</asp:ListItem>
  <asp:ListItem Value="July">July</asp:ListItem>
  <asp:ListItem Value="August">August</asp:ListItem>
  <asp:ListItem Value="September">September</asp:ListItem>
  <asp:ListItem Value="October">October</asp:ListItem>
  <asp:ListItem Value="November">November</asp:ListItem>
  <asp:ListItem Value="December">December</asp:ListItem>
  </asp:DropDownList>
--%>
  
 

</div>
</td>
</tr>
</table>


  </ContentTemplate>
  </asp:UpdatePanel>
</div>
<div style="float:left;">


   <asp:LinkButton ID="linkBtnSearch" runat="server" Text="Generate Report" OnClick="btnSearch_Click" ValidationGroup="main" ForeColor="Black" ></asp:LinkButton>
  </div>


 <br />
 </fieldset>

 <%--<div>
    <rsweb:ReportViewer ID="reportViewer" runat="server" Width="98%" ShowRefreshButton="False" AsyncRendering="false">
          </rsweb:ReportViewer>
    </div>--%>
 
</asp:Content>
