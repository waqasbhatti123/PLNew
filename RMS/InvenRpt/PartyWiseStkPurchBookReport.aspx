<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="PartyWiseStkPurchBookReport.aspx.cs" Inherits="RMS.InvenRpt.PartyWiseStkPurchBookReport" %>
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

<script type="text/javascript">
    $(function () {
        $('.code').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "InvenStockLedgerReport.aspx/GetDetailAccount",
                    data: "{ 'sname': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",

                    dataFilter: function (data) { return data; },
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                value: item,
                                result: item
                            }
                        }))
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            },
            minLength: 1
        });
    });
</script>

<script type="text/javascript">
    $(document).ready(function () {

        $('.code').blur(function () {
            selectval = $(this).val().split(" ");
            $(this).val(selectval[0]);

        });
    });
    
   </script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
    ValidationGroup="main"/>
  <uc1:Messages ID="ucMessage" runat="server" />
  <fieldset class="fieldSet" >
  <legend >
 
  </legend>

<div style="float:left;">


  
  <asp:UpdatePanel ID="uPanel" runat="server" UpdateMode="Conditional">
  <ContentTemplate>

  
<table cellpadding="4" cellspacing="4">
<tr>
    <td  colspan="5">
        <asp:Label ID="Label122" runat="server" Text="Party:"></asp:Label>

        <asp:DropDownList ID="ddlParty" runat="server" AppendDataBoundItems="true">
            <asp:ListItem Text="All" Selected="True" Value="">
            </asp:ListItem>
        </asp:DropDownList>
    </td>
</tr>
<tr>
    <td colspan="5">
     Item Group:
    <asp:DropDownList ID="ddlItemGroup" runat="server" AppendDataBoundItems="true">
        <asp:ListItem Text="All" Selected="True" Value="0">All</asp:ListItem>
    </asp:DropDownList>

    <asp:Label ID="lblstatus" Text="Code From" runat="server"/>
    &nbsp;
    <asp:TextBox ID="txtcodefrom" class="code" runat="server" Width="100px" />
    &nbsp; 
    <asp:Label ID="lbltype" Text="Code To" runat="server"/>
    &nbsp;
    <asp:TextBox ID="txtcodeto" class="code" runat="server" Width="100px"/>
    </td>
</tr>
<tr>

<td>
    Location:&nbsp;
        <asp:DropDownList ID="ddlLoc" runat="server" AppendDataBoundItems="true">
        <asp:ListItem Text="All Locations" Selected="True" Value="0"></asp:ListItem>
        </asp:DropDownList>
    </td>
    <td style = " display:none; ">
        <asp:Label ID="lblYear" runat="server" Text="Year:" Width="60px">
        </asp:Label> 
          <asp:DropDownList ID="ddlYear" runat="server" AppendDataBoundItems="true" Width="110px">
          <asp:ListItem Selected="True" Text="Select Year" Value="0">
          </asp:ListItem>
          </asp:DropDownList>
    
    </td>
    <td>
        From Date:&nbsp;
        <asp:TextBox ID="txtfromDt" runat="server" Width="80px"></asp:TextBox>
        </td>
        <td style="width:200px">
        To Date:&nbsp;
        <asp:TextBox ID="txttoDt" runat="server" Width="80px"></asp:TextBox>
    </td>
    <td style = " display:none; ">
        <asp:Label ID="lblMonth" runat="server" Text="Month:" Width="60px">
        </asp:Label> 
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

<ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtfromDt">
    </ajaxToolkit:CalendarExtender>
    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txttoDt">
    </ajaxToolkit:CalendarExtender>
  </ContentTemplate>
  </asp:UpdatePanel>
  
 </div>
  <div>
    Export To:&nbsp;
    <asp:DropDownList ID ="ddlExtension" runat="server">
        <asp:ListItem Value="PDF" Selected="True">PDF</asp:ListItem>
        <asp:ListItem Value="Excel">Excel</asp:ListItem>
    </asp:DropDownList>
  </div>
<div style="float:right;">

<asp:LinkButton ID="linkBtnSearch" runat="server" Text="Generate Report" OnClick="btnSearch_Click" ValidationGroup="main" ForeColor="Black" ></asp:LinkButton>
   
  </div>


 </fieldset>

 <%--<div>
    <rsweb:ReportViewer ID="reportViewer" runat="server" Width="98%" ShowRefreshButton="False" AsyncRendering="false">
          </rsweb:ReportViewer>
    </div>--%>
 
</asp:Content>
