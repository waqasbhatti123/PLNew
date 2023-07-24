<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/home/RMSMasterHome.Master"
 CodeBehind="WetBlueStkLedgherReport.aspx.cs" Inherits="RMS.GLSetup.WetBlueStkLedgherReport" %>


<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    $(function() {
    $('.code').autocomplete({
            source: function(request, response) {
                $.ajax({
                url: "frmPreferences.aspx/GetDetailAccount",
                    data: "{ 'sname': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",

                    dataFilter: function(data) { return data; },
                    success: function(data) {
                        response($.map(data.d, function(item) {
                            return {
                                value: item,
                                result: item
                            }
                        }))
                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            },
            minLength: 1
        });
    });
</script>

<script type="text/javascript">
    $(document).ready(function() {
    
    $('.code').blur(function() {
        selectval = $(this).val().split(" ");
        $(this).val(selectval[0]);

        });
    });
    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <fieldset class="fieldSet">
  <legend></legend>
Report By,
<table>
        <tr><td>&nbsp</td></tr>
       <tr>
       <td><asp:RadioButton ID="rdoWetBlue" runat="server" Text="  Wet Blue Stock"  Width="190px" GroupName="rbo"/></td>
       </tr>
       <tr>
       <td><asp:RadioButton ID="rdoCrust" runat="server" Text="  Crust Stock" Width="190px" GroupName="rbo"/></td>
       </tr>
       <tr>
       <td><asp:RadioButton ID="rdoFinish" runat="server" Text="  Finish Goods Stock" Width="190px" GroupName="rbo"/></td>
       </tr>

</table>


    <br />
       <table style="text-align:center;">

       <tr>
       <td style="width:200px">
   From Date:&nbsp;
   <asp:TextBox ID="txtFrom" runat="server" Width="80px"/>
       </td>
       <td style="width:200px">
       To Date:&nbsp;
       <asp:TextBox ID="txtTo" runat="server" Width="80px"/>

       </td>
       <td>
  &nbsp;  &nbsp;  
  <asp:LinkButton ID="btnGenerat" runat="server" Text="Generate Report" OnClick="btnGenerat_Click" ForeColor="Black" ></asp:LinkButton>
  
  </td>
       </tr>
       </table>

    <br />
   
       </fieldset>    
    <ajaxToolkit:CalendarExtender ID="txtFromDate" runat="server" TargetControlID="txtFrom" Enabled="True"/>

     <ajaxToolkit:CalendarExtender ID="txtToDate" runat="server" TargetControlID="txtTo" Enabled="True"/>

  <%--
    <asp:Label ID="lblstatus" Text="Code From" runat="server"/>
&nbsp;<asp:TextBox ID="txtcodefrom" class="code" runat="server" Width="100px"/>
        &nbsp;
        
     <asp:Label ID="lbltype" Text="Code To" runat="server"/>
&nbsp;<asp:TextBox ID="txtcodeto" class="code" runat="server" Width="100px"/>
        &nbsp;--%>
   
     
        
     
   
       <%--
        <asp:ImageButton ID="btnGenerat" runat="server"  ImageUrl="~/images/btn_generate.png"  OnClick="btnGenerat_Click"
       onMouseOver="this.src='../images/btn_generate_m.png'" onMouseOut="this.src='../images/btn_generate.png'" />
             &nbsp;--%>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="80%">
    </rsweb:ReportViewer>
  
</asp:Content>
