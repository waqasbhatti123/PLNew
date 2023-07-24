<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"
 AutoEventWireup="true" CodeBehind="frmGLCashBook.aspx.cs" Inherits="RMS.GLSetup.frmGLCashBook"
 Culture="auto" UICulture="auto" %>
<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %><%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
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
    
    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main"/>
    <uc1:Messages ID="ucMessage" runat="server" />
    <br />
    
    <p>
    <%--<asp:Label ID="lblgltype" Text="GL Type" runat="server"/>
    &nbsp;
    <asp:DropDownList ID="ddlgltype" runat="server" AppendDataBoundItems="True" Width="150px"/>
    &nbsp;--%>
    <asp:Label ID="lbl" Text="From" runat="server"/>
    &nbsp;
    <asp:TextBox ID="txtFrom" runat="server" Width="80px"/>                            
    <ajaxToolkit:CalendarExtender ID="txtFromDate" runat="server" TargetControlID="txtFrom" Enabled="True"/>
    &nbsp;    
    <asp:Label ID="Label2" Text="To" runat="server"/>
    <ajaxToolkit:CalendarExtender ID="txtToDate" runat="server" TargetControlID="txtTo" Enabled="True"/>
    &nbsp;
    <asp:TextBox ID="txtTo" runat="server" Width="80px"/>
    &nbsp;<br />
    <asp:Label ID="lblstatus" Text="Code From" runat="server"/>
    &nbsp;
    <asp:TextBox ID="txtcodefrom" class="code" runat="server" Width="100px"/>
    <asp:RequiredFieldValidator ID="reqcodefrom" runat="server" SetFocusOnError="true" Display="None" 
    ErrorMessage="Please enter code from" ValidationGroup="main" ControlToValidate="txtcodefrom"></asp:RequiredFieldValidator>
    &nbsp;    
    <asp:Label ID="lbltype" Text="Code To" runat="server"/>
    &nbsp;
    <asp:TextBox ID="txtcodeto" class="code" runat="server" Width="100px"/>
    <asp:RequiredFieldValidator ID="reqcodeto" runat="server" SetFocusOnError="true" Display="None"
    ErrorMessage="Please enter code to" ValidationGroup="main" ControlToValidate="txtcodeto"></asp:RequiredFieldValidator>
    &nbsp;
    <asp:Button ID="btnGenerat" runat="server" Text="Report" OnClick="btnGenerat_Click" ValidationGroup="main" /> 
    </p>
    
    <br />
   <asp:Panel ID="pnlMain" runat="server" Width="99%" Height="600">
       <rsweb:ReportViewer ID="viewer" runat="server" Width="100%" Height="580px">
       </rsweb:ReportViewer>
   </asp:Panel> 
</asp:Content>
