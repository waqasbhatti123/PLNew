<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="MonthlyDemandReport.aspx.cs" Inherits="RMS.InvenRpt.MonthlyDemandReport" %>
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

  <fieldset class="fieldSet" >
  <legend >
 
  </legend>
  <br />
  
  <asp:UpdatePanel ID="upnl" runat="server" UpdateMode="Conditional">
<ContentTemplate>
  <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
    ValidationGroup="main"/>
  <uc1:Messages ID="ucMessage" runat="server" />
  

  Report By,
<table style="margin-left:60px">
<tr>
<td>
&nbsp;
</td>
<td>
   <asp:RadioButton ID="rdbDeptWise" runat="server" GroupName="sort" Text=" Department-Wise" Width="200px"/>
<br />
<asp:RadioButton ID="rdbItemWise" runat="server" GroupName="sort" Text=" Item-Wise" Width="200px"/>
    
</td>
</tr>
</table>
<div style="float:left;">
<table>
<tr><td>&nbsp;</td><td>&nbsp;</td></tr>
<tr>
    <td colspan="4">
     Item Group:
                    <asp:DropDownList ID="ddlItemGroup" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="All" Selected="True" Value="0">All</asp:ListItem>
                    </asp:DropDownList>
    </td>
    <td>
  Export To:&nbsp;
    <asp:DropDownList ID ="ddlExtension" runat="server">
        <asp:ListItem Value="PDF" Selected="True">PDF</asp:ListItem>
        <asp:ListItem Value="Excel">Excel</asp:ListItem>
    </asp:DropDownList>
  </td>
   </tr>
<tr>

<td>
Year: &nbsp;
 <asp:DropDownList ID="ddlYear" runat="server" AppendDataBoundItems="true" Width="110px">
  <asp:ListItem Selected="True" Text="Select Year" Value="0">
  </asp:ListItem>
  </asp:DropDownList>
</td>

<td>

Month: &nbsp;
  
  
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

</td>

</tr>
</table>



</div>

 </ContentTemplate>

</asp:UpdatePanel>

<div style="float:left; margin-top:20px;">


   <asp:LinkButton ID="linkBtnSearch" runat="server" Text="Generate Report" OnClick="btnSearch_Click" ValidationGroup="main" ForeColor="Black" ></asp:LinkButton>
  </div>


 <br />
 </fieldset>


</asp:Content>
