<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="RawHideStockReport.aspx.cs" Inherits="RMS.RawHideStockReport" %>
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
    ValidationGroup="fltr"/>
  <uc1:Messages ID="ucMessage" runat="server" />
  <fieldset class="fieldSet">
 
  <legend >
    
  </legend>
  <br />
<div style="float:left; width:52%">
 <asp:UpdatePanel ID="uPnl" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
  
   <table style="text-align:left; width:98%;">
  <tr>
    <tr>
        <td >
        Report By,
        </td>
    </tr>
    <tr>
    <td>
    &nbsp
    </td>
    
   <td>
  <asp:RadioButton ID="rdbStockAll" GroupName="sort" runat="server" Text="  All" AutoPostBack="true" OnCheckedChanged="rdbStockAll_Changed" Width="100px" />
 <br />
 <asp:RadioButton ID="rdbStockYes" GroupName="sort" runat="server" Text="  Transferred" AutoPostBack ="true" OnCheckedChanged="rdbStockYes_Changed" Width="100px" />
 <br /> 
 <asp:RadioButton ID="rdbStockNo" GroupName="sort" runat ="server" Text ="  Stock In Hand" AutoPostBack="true" OnCheckedChanged="rdbStockNo_Changed" Width="130px" />
  </td>
  </tr>
  
  
  <tr>
    <td>
    &nbsp
    </td>
    <td>
    &nbsp
    </td>
    </tr>
  
  <tr>
  <td>
  &nbsp
  </td>
  <td >
  From Date:&nbsp;
  <asp:TextBox ID="txtfromDt" runat="server" Width="80px"></asp:TextBox>

  To Date:&nbsp;
  <asp:TextBox ID="txttoDt" runat="server" Width="80px"></asp:TextBox>
  </td>
       
  </tr>
 </table>


    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtfromDt">
    </ajaxToolkit:CalendarExtender>
    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txttoDt">
    </ajaxToolkit:CalendarExtender>
 
 


 
 </ContentTemplate>
  </asp:UpdatePanel>
 </div>  
<div style="float:left; margin-top:11.2%;">

<asp:LinkButton ID="linkBtnSearch" runat="server" Text="Generate Report" OnClick="btnSearch_Click" ForeColor="Black" ></asp:LinkButton>

</div>

 <br />
 </fieldset>
 
 
 <%--<div>
    <rsweb:ReportViewer ID="reportViewer" runat="server" Width="98%" ShowRefreshButton="False" AsyncRendering="false">
          </rsweb:ReportViewer>
    </div>
 --%>
</asp:Content>
