<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="RawHideLotReport.aspx.cs" Inherits="RMS.RawHideLotReport" %>
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
  
   <table>
        <tr>
            <td width="80px">
                Status:
            </td>
            <td width="120px">
              <asp:RadioButton ID="rdbApproved" GroupName="sort" runat="server" Text="  Approved Lots"/>
            </td>
            <td width="120px">
                <asp:RadioButton ID="rdbPending" GroupName="sort" runat="server" Text="  Pending Lots"/>
            </td>
            <td width="120px">
                &nbsp
            </td>
            <td width="120px">
                &nbsp
            </td>
        </tr>
        <tr>
            <td>
                Sort By:
            </td>
            <td>
                <asp:DropDownList ID="ddlSortBy" runat="server" >
                    <asp:ListItem Selected="True" Value="Broker">Broker</asp:ListItem>
                    <asp:ListItem Value="Lot">Lot</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                &nbsp
            </td>
            <td>
                &nbsp
            </td>
            <td>
                &nbsp
            </td>
        </tr>
        <tr>
            <td>
                From Date:
            </td>
            <td>
                <asp:TextBox ID="txtfromDt" runat="server" Width="100px"></asp:TextBox>
            </td>
            <td >
                To Date:
            </td>
            <td>
                <asp:TextBox ID="txttoDt" runat="server" Width="100px"></asp:TextBox>
            </td>
            <td>
                <asp:LinkButton ID="linkBtnSearch" runat="server" Text="Generate Report" OnClick="btnSearch_Click" ForeColor="Black" ></asp:LinkButton>
            </td>
        </tr> 
     </table>

    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtfromDt">
    </ajaxToolkit:CalendarExtender>
    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txttoDt">
    </ajaxToolkit:CalendarExtender>

 </fieldset>

 <%--<div>
    <rsweb:ReportViewer ID="reportViewer" runat="server" Width="98%" ShowRefreshButton="False" AsyncRendering="false">
          </rsweb:ReportViewer>
    </div>
 --%>
</asp:Content>
