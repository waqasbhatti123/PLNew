<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmpMgtCount.aspx.cs" Inherits="RMS.Setup.EmpMgtCount" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
   <script type="text/javascript">

       function pageLoad() {

           $(".classOnlyInt").keydown(function(event) {
               if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                   event.preventDefault();
               }
           });
       }
   
   </script>
   
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                            ValidationGroup="main" />
                        <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label ID="Label1" runat="server" Text="List Data"></asp:Label>
                        </div>
                        <div class="col-lg-8 col-md-8 col-sm-8">
                            <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                            <asp:ListItem Selected="True" Value="Region">Regional Data</asp:ListItem>
                            <asp:ListItem Value="Division">Divisional Data</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Medium" Text="Month*"></asp:Label>
                            <asp:TextBox ID="txtmonth" runat="server" CssClass="form-control" Class="classOnlyInt"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter Month"
                             ValidationGroup="main" ControlToValidate="txtmonth"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="Medium" Text="Year*"></asp:Label>
                            <asp:TextBox ID="txtYear" runat="server" CssClass="form-control" Class="classOnlyInt"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Year"
                             ValidationGroup="main" ControlToValidate="txtYear"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4" style="margin-top:25px">
                            <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="Generate" OnClick="Button1_Click" ValidationGroup="main" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row" id="Region" runat="server" visible="false">
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <asp:GridView ID="grdRegion" CssClass="table table-responsive-sm" runat="server" OnSelectedIndexChanged="grdEmps_SelectedIndexChanged"
                        AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdRegion_PageIndexChanging"
                        OnRowDataBound="grdEmps_RowDataBound" EmptyDataText="There is no data for selected criterion"
                        Width="760px" PageSize="20">
                        <Columns>
                            <asp:BoundField DataField="Region" HeaderText="Region" />
                            <asp:BoundField DataField="Devision" HeaderText="Division" />
                            <asp:BoundField DataField="Department" HeaderText="Department" />
                            <asp:BoundField DataField="Design" HeaderText="Designation" />
                            <asp:BoundField DataField="City" HeaderText="City" />
                            <asp:BoundField DataField="Leavers" HeaderText="Leavers" />
                            <asp:BoundField DataField="Joiners" HeaderText="Joiners" />
                            <asp:BoundField DataField="OnBoard" HeaderText="On Board" />
                        </Columns>
                        <HeaderStyle CssClass="grid_hdr" />
                        <RowStyle CssClass="grid_row" />
                        <AlternatingRowStyle CssClass="gridAlternateRow" />
                        <SelectedRowStyle CssClass="gridSelectedRow" />
                    </asp:GridView>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row" id="Division" runat="server" visible="false">
                         <asp:GridView ID="grdDivision" CssClass="table table-responsive-sm"  runat="server" OnSelectedIndexChanged="grdEmps_SelectedIndexChanged"
                        AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdDivision_PageIndexChanging"
                        OnRowDataBound="grdEmps_RowDataBound" EmptyDataText="There is no data for selected criterion"
                        Width="99%" PageSize="20">
                        <Columns>
                            <asp:BoundField DataField="Devision" HeaderText="Devision" />
                            <asp:BoundField DataField="Region" HeaderText="Region" />
                            <asp:BoundField DataField="Department" HeaderText="Department" />
                            <asp:BoundField DataField="Design" HeaderText="Design" />
                            <asp:BoundField DataField="City" HeaderText="City" />
                            <asp:BoundField DataField="Leavers" HeaderText="Leavers" />
                            <asp:BoundField DataField="Joiners" HeaderText="Joiners" />
                            <asp:BoundField DataField="OnBoard" HeaderText="On Board" />
                        </Columns>
                        <HeaderStyle CssClass="grid_hdr" />
                        <RowStyle CssClass="grid_row" />
                        <AlternatingRowStyle CssClass="gridAlternateRow" />
                        <SelectedRowStyle CssClass="gridSelectedRow" />
                    </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div> 
</asp:Content>
