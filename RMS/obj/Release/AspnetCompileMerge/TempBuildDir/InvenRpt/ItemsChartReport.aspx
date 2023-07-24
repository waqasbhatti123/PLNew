<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="ItemsChartReport.aspx.cs" Inherits="RMS.InvenRpt.ItemsChartReport" %>
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
 
  
   <table>
  <tr>
   <td><asp:Label ID="Label1" runat="server" Text="Item Group: "></asp:Label></td>
    
    <td colspan="4">
    <%--<asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
            <asp:DropDownList ID="ddlItemGroup" runat="server" AppendDataBoundItems="true">
             <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
            </asp:DropDownList>
   <%--      </ContentTemplate>
    </asp:UpdatePanel>--%>
    </td>
  </tr>

 
   <%--for group--%>
  <tr>
     
 
    
    <td><asp:Label ID="lblGroup" runat="server" Text="Group Name: "></asp:Label></td>
    
    <td>
    <%--<asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
            <asp:DropDownList ID="ddlGroupName" runat="server" AppendDataBoundItems="true" AutoPostBack="true" 
             CssClass="RequiredFieldDropDown" Width="150px" OnSelectedIndexChanged="ddlGroupName_SelectedIndexChanged">
            </asp:DropDownList>
   <%--      </ContentTemplate>
    </asp:UpdatePanel>--%>
    </td>
    
    
 
  
  <%--for controls--%>
 <asp:Panel ID="pnlControl" runat="server">
    <td></td>
    <td>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <asp:Label ID="lblControlName" runat="server" Text="Control Name: "></asp:Label>
        <asp:DropDownList ID="ddlControlName" runat="server" AppendDataBoundItems="true" AutoPostBack="true" 
         CssClass="RequiredFieldDropDown"  Width="150px" OnSelectedIndexChanged="ddlControlName_SelectedIndexChanged">
        </asp:DropDownList>
    
          </ContentTemplate>
          <Triggers>
          <asp:AsyncPostBackTrigger ControlID="ddlGroupName" />
          </Triggers>
    </asp:UpdatePanel>
    </td>
    </asp:Panel>
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
 
  <div>
    <rsweb:ReportViewer ID="reportViewer" runat="server" Width="98%" ShowRefreshButton="False" AsyncRendering="true">
          </rsweb:ReportViewer>
    </div>
    
 
 
 <br />
 </fieldset>


 
 
</asp:Content>
