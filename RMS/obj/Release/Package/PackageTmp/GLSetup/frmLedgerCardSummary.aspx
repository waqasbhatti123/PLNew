<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/home/RMSMasterHome.Master"
 CodeBehind="frmLedgerCardSummary.aspx.cs" Inherits="RMS.GLSetup.frmLedgerCardSummary" %>


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
   <br />
   <uc1:Messages ID="ucMessage" runat="server"/>
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4 ">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <uc1:Messages ID="Messages1" runat="server" />
                        </div>
                    </div>

                    <br />

                    <div class="row">
                        <div class="col-md-4">
                            Branch*
                             <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="searchbranchchange form-control" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged"
                                        AppendDataBoundItems="True" AutoPostBack="true">
                                        <asp:ListItem Value="0">Select Branch</asp:ListItem>
                                    </asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            Status*
                             <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="P">Pending</asp:ListItem>
                                        <asp:ListItem Value="A" Selected="True">Approved</asp:ListItem>
                                        <asp:ListItem Value=" ">All</asp:ListItem>
                                    </asp:DropDownList>
                        </div>
                    </div>
                    <br />
                     <div class="row">
                        <div class="col-md-4">
                            From*
                            <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" />
                                    <ajaxToolkit:CalendarExtender ID="txtFromDate" runat="server" TargetControlID="txtFrom" Enabled="True" />
                                
                        </div>
                        <div class="col-md-4">
                            To*
                             <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" />
                                    <ajaxToolkit:CalendarExtender ID="txtToDate" runat="server" TargetControlID="txtTo" Enabled="True" />
                                
                        </div>
                    </div>
                    <br />
                     <div class="row">
                        <div class="col-md-4">
                            Account Type*
                             <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="D">Detail</asp:ListItem>
                                        <asp:ListItem Value="C2">Control - 2</asp:ListItem>
                                        <asp:ListItem Value="C1">Control - 1</asp:ListItem>
                                        <asp:ListItem Value="G">Group</asp:ListItem>
                                    </asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                             Account Heads
                                <asp:DropDownList ID="codeDropDown" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                    <asp:ListItem Value="">All</asp:ListItem>
                                </asp:DropDownList>
                        </div>
                    </div>
                

                    <br />
                        <asp:Button ID="Button1" runat="server" Text="Report" OnClick="btnGenerat_Click" CssClass="btn btn-primary"/>
                    <br />
                    <asp:Panel ID="pnlMain" runat="server" Width="99%" Height="600">
                        <rsweb:ReportViewer ID="viewer" runat="server" Width="100%" Height="580px">
                        </rsweb:ReportViewer>
                    </asp:Panel>

                    <%--        <p>
         &nbsp;
         <br />&nbsp;
        <asp:ImageButton ID="btnGenerat" runat="server"  ImageUrl="~/images/btn_generate.png"  OnClick="btnGenerat_Click"
       onMouseOver="this.src='../images/btn_generate_m.png'" onMouseOut="this.src='../images/btn_generate.png'" />
             &nbsp;

    </p>--%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
