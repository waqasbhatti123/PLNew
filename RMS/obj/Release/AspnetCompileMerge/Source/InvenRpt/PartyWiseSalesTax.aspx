<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="PartyWiseSalesTax.aspx.cs" Inherits="RMS.InvenRpt.PartyWiseSalesTax" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" 
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

    
</script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
    ValidationGroup="fltr"/>
  <uc1:Messages ID="ucMessage" runat="server" />
  <fieldset class="fieldSet">
  <legend >
    
  </legend>
  <br />
   <table style="text-align:center;">
    
  <tr>
   
    
    <td>
    Location:&nbsp;
        <asp:DropDownList ID="ddlLoc" runat="server" AppendDataBoundItems="true">
        <asp:ListItem Text="All Locations" Selected="True" Value="0"></asp:ListItem>
        </asp:DropDownList>
    </td>
    <td style="width:200px">
      From Date:&nbsp;
      <asp:TextBox ID="txtfromDt" runat="server" Width="80px"></asp:TextBox>
      </td>
      <td style="width:200px">
      To Date:&nbsp;
      <asp:TextBox ID="txttoDt" runat="server" Width="80px"></asp:TextBox>
     </td>
      <td>
      Export To:&nbsp;
        <asp:DropDownList ID ="ddlExtension" runat="server">
            <asp:ListItem Value="PDF" Selected="True">PDF</asp:ListItem>
            <asp:ListItem Value="Excel">Excel</asp:ListItem>
        </asp:DropDownList>
      </td>
  <td>
  &nbsp;  &nbsp;  
  <asp:LinkButton ID="linkBtnSearch" runat="server" Text="Generate Report" OnClick="btnSearch_Click" ForeColor="Black" ></asp:LinkButton>
  
  </td>
  </tr>
 </table>
 <br />
 </fieldset>
 <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtfromDt">
    </ajaxToolkit:CalendarExtender>
    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txttoDt">
    </ajaxToolkit:CalendarExtender>

 
 <div>
    <rsweb:ReportViewer ID="reportViewer" runat="server" Width="98%" ShowRefreshButton="False" AsyncRendering="false">
          </rsweb:ReportViewer>
    </div>
 
</asp:Content>
