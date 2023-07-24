<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="InvenDeptWiseConsumptionReport.aspx.cs" Inherits="RMS.InvenRpt.InvenDeptWiseConsumptionReport" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" 
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function pageLoad() {
        $("#<%= rdolistRptType.ClientID%>").change(function() {

            if ($("#<%= rdolistRptType.ClientID%>").find(":checked").val() == 'Monthly') {

                $("#<%= ddlMonth.ClientID%>").prop('disabled', false);
                ValidatorEnable($('[id*=reqval_month]')[0], true);
                $("#<%= ddlMonth.ClientID%>").val('0');
            }
            else {

                if ($("#<%= rdolistRptBy.ClientID%>").find(":checked").val() == 'Summary') {

                    $("#<%= ddlMonth.ClientID%>").prop('disabled', true);
                    ValidatorEnable($('[id*=reqval_month]')[0], false);
                    $("#<%= ddlMonth.ClientID%>").val('0');

                    $('#<%= rdolistRptBy.ClientID %> input[value="Detail"]').attr('checked', 'checked');
                }
                else {

                    $("#<%= ddlMonth.ClientID%>").prop('disabled', true);
                    ValidatorEnable($('[id*=reqval_month]')[0], false);
                    $("#<%= ddlMonth.ClientID%>").val('0');
                }
            }
        });

        $("#<%= rdolistRptBy.ClientID%>").change(function() {

            if ($("#<%= rdolistRptType.ClientID%>").find(":checked").val() == 'Summary') {

                if ($("#<%= rdolistRptType.ClientID%>").find(":checked").val() == 'Yearly') {

                    $("#<%= ddlMonth.ClientID%>").prop('disabled', false);
                    ValidatorEnable($('[id*=reqval_month]')[0], true);
                    $("#<%= ddlMonth.ClientID%>").val('0');
                    $('#<%= rdolistRptType.ClientID %> input[value="Monthly"]').attr('checked', 'checked');
                }
            }
            else {

                if ($("#<%= rdolistRptType.ClientID%>").find(":checked").val() == 'Yearly') {

                    $("#<%= ddlMonth.ClientID%>").prop('disabled', true);
                    ValidatorEnable($('[id*=reqval_month]')[0], false);
                    $("#<%= ddlMonth.ClientID%>").val('0');
                }
                else if ($("#<%= rdolistRptType.ClientID%>").find(":checked").val() == 'Monthly') {

                    $("#<%= ddlMonth.ClientID%>").prop('disabled', false);
                    ValidatorEnable($('[id*=reqval_month]')[0], true);
                    $("#<%= ddlMonth.ClientID%>").val('0');
                }
            }
        });
    }
</script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:ValidationSummary ID="main1" CssClass="ErrorSummary" runat="server" DisplayMode="List"
    ValidationGroup="main"/>
  <uc1:Messages ID="ucMessage" runat="server" />
  <fieldset class="fieldSet">
  <legend >
  </legend>
Report By ,
<br />
    <table width="100%" border="0">
        <tr>
            <td width="100%">
                <div style="float:left; margin-left:70px;">
                    <asp:RadioButtonList ID="rdolistRptType" runat="server">
                        <asp:ListItem Value="Monthly" Selected="True">Monthly</asp:ListItem>
                        <asp:ListItem Value="Yearly">Yearly</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div style="float:left; margin-left:40px;">
                    <asp:RadioButtonList ID="rdolistRptBy" runat="server">
                        <asp:ListItem Value="Summary" Selected="True">Summary</asp:ListItem>
                        <asp:ListItem Value="Detail">Detail</asp:ListItem>
                        <asp:ListItem Value="CostCenter">CostCenter</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </td>
        </tr>
    </table>
    
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
      <td width="0%">
      </td>
          <td>
                <table>
                <tr>
                    <td colspan="2" style="float:left;">
                     Item Group:
                                <asp:DropDownList ID="ddlItemGroup" runat="server" AppendDataBoundItems="true">
                                    <asp:ListItem Text="All" Selected="True" Value="0">All</asp:ListItem>
                                </asp:DropDownList>
                    </td>
                    <td style="float:left;">Department:</td>
                    <td style="float:left;">
                       <asp:DropDownList ID="ddlDept" runat="server" AppendDataBoundItems="true">
                       <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                       </asp:DropDownList>
                      
                    </td>
                </tr>
                <tr>
                <td style="float:left;">Location:</td>
                <td style="float:left;">
                    <asp:DropDownList ID="ddlLoc" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Text="All Locations" Selected="True" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="float:left;">Year:</td>
                <td style="float:left;">
                      <asp:DropDownList ID="ddlYear" runat="server" >
                      <asp:ListItem Selected="True" Text="Select Year" Value="0">
                      </asp:ListItem>
                      <asp:ListItem Value="2010">2010</asp:ListItem>
                      <asp:ListItem Value="2011">2011</asp:ListItem>
                      <asp:ListItem Value="2012">2012</asp:ListItem>
                      <asp:ListItem Value="2013">2013</asp:ListItem>
                      <asp:ListItem Value="2014">2014</asp:ListItem>
                      <asp:ListItem Value="2015">2015</asp:ListItem>
                      <asp:ListItem Value="2016">2016</asp:ListItem>
                      <asp:ListItem Value="2017">2017</asp:ListItem>
                      <asp:ListItem Value="2018">2018</asp:ListItem>
                      <asp:ListItem Value="2019">2019</asp:ListItem>
                      <asp:ListItem Value="2020">2020</asp:ListItem>
                      </asp:DropDownList>
                      <asp:RequiredFieldValidator ID="reqval_year" runat="server" ControlToValidate="ddlYear" ErrorMessage="Select year."
                       Display="None" SetFocusOnError="true" ValidationGroup="main" InitialValue="0">
                      </asp:RequiredFieldValidator>
                </td>
                <td style="float:left;">Month:</td>
                <td style="float:left;">

                      <asp:DropDownList ID="ddlMonth" runat="server" >
                      <asp:ListItem Selected="True" Text="Select Month" Value="0">
                      </asp:ListItem>
                      <asp:ListItem Value="1">January</asp:ListItem>
                      <asp:ListItem Value="2">February</asp:ListItem>
                      <asp:ListItem Value="3">March</asp:ListItem>
                      <asp:ListItem Value="4">April</asp:ListItem>
                      <asp:ListItem Value="5">May</asp:ListItem>
                      <asp:ListItem Value="6">June</asp:ListItem>
                      <asp:ListItem Value="7">July</asp:ListItem>
                      <asp:ListItem Value="8">August</asp:ListItem>
                      <asp:ListItem Value="9">September</asp:ListItem>
                      <asp:ListItem Value="10">October</asp:ListItem>
                      <asp:ListItem Value="11">November</asp:ListItem>
                      <asp:ListItem Value="12">December</asp:ListItem>
                      </asp:DropDownList>
                       <asp:RequiredFieldValidator ID="reqval_month" runat="server" ControlToValidate="ddlMonth" ErrorMessage="Select month."
                       Display="None" SetFocusOnError="true" ValidationGroup="main" InitialValue="0">
                       </asp:RequiredFieldValidator>

                </td>
                <td>
                  Export To:
                    <asp:DropDownList ID ="ddlExtension" runat="server">
                        <asp:ListItem Value="PDF" Selected="True">PDF</asp:ListItem>
                        <asp:ListItem Value="Excel">Excel</asp:ListItem>
                    </asp:DropDownList>
                 </td>
                <td style="float:right;">
                      <asp:LinkButton ID="linkBtnSearch" runat="server" Text="Generate Report" OnClick="btnSearch_Click" ForeColor="Black" ValidationGroup="main" >
                      </asp:LinkButton>
                </td>
                
                </tr>
                </table>
          </td>
      <td width="0%">
      </td>
      </tr>
      </table>
  
        
 </fieldset>
 
 <%--<div>
    <rsweb:ReportViewer ID="reportViewer" runat="server" Width="98%" ShowRefreshButton="False" AsyncRendering="false">
          </rsweb:ReportViewer>
    </div>--%>
 
</asp:Content>
