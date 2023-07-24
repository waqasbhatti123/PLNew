<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/home/RMSMasterHome.Master"
 CodeBehind="frmSearchVoucher.aspx.cs" Culture="auto" UICulture="auto"
 Inherits="RMS.GLSetup.frmSearchVoucher" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function Ac_ItemSelected(sender, e) {
            var Ac = $get('<%= Ac.ClientID %>');
            Ac.value = e.get_value();
        }

        function pageLoad() {
        
            $('#<%= txtDebit.ClientID%>').keydown(function(event) {
                if (event.keyCode != 110 && event.keyCode != 190 && (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                    event.preventDefault();
                }
                var txt = $('#<%= txtDebit.ClientID%>').val();
                if (event.keyCode == 110 || event.keyCode == 190) {
                    if ((txt.split(".").pop().length) > 1) {
                        event.preventDefault();
                    }
                }
            });

            $('#<%= txtCredit.ClientID%>').keydown(function(event) {
                if (event.keyCode != 110 && event.keyCode != 190 && (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                    event.preventDefault();
                }
                var txt = $('#<%= txtCredit.ClientID%>').val();
                if (event.keyCode == 110 || event.keyCode == 190) {
                    if ((txt.split(".").pop().length) > 1) {
                        event.preventDefault();
                    }
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


      <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4 ">
                <div class="card-body">
                    
<div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
            <uc1:Messages ID="ucMessage" runat="server" />
</div>
     </div>
                    
<div class="form-group">
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                                 <label>Divisions:</label>
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged"
                                AppendDataBoundItems="True" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Division</asp:ListItem>
                            </asp:DropDownList>                          
                            </div>

                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Voucher Type</label>
                            <asp:DropDownList ID="ddlVoucher" runat="server" CssClass="form-control" >
                            <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                       
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                    </div>
				</div>
                    <div class="form-group">
                    <div class="row">
                         <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Voucher #</label>
                            <asp:TextBox ID="txtVoucherNo" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Narration</label>
                                                    <asp:TextBox ID="txtNarr" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                        </div>
                       
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                    </div>
				</div>
                    <div class="form-group">
                    <div class="row">
                         <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Title</label>
                            <asp:TextBox ID="txtTitle" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Cheque #</label>
                            <asp:TextBox ID="txtChqNo" runat="server" MaxLength="30" CssClass="form-control"></asp:TextBox>
                            
                        </div>
                   
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                    </div>
				</div>
                    <div class="form-group">
                    <div class="row">
                             <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Cost Center</label>
                            <asp:TextBox ID="txtCCntr" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
                            <ajaxToolkit:AutoCompleteExtender runat="server" ID="AutoComplete1" TargetControlID="txtCCntr" ServiceMethod="GetCostCenter" ServicePath="~/InvSetupSupport/AutoCompleteSearch.asmx" MinimumPrefixLength="1"
                            CompletionSetCount="5" CompletionInterval="100" FirstRowSelected="true" EnableCaching="true" OnClientItemSelected="Ac_ItemSelected" >
                            </ajaxToolkit:AutoCompleteExtender>
                            <asp:HiddenField runat="server" ID="Ac" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Debit</label>
                            <asp:TextBox ID="txtDebit" runat="server" MaxLength="12" CssClass="form-control"></asp:TextBox>

                        </div>
                       
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                    </div>
				</div>
                    <div class="form-group">
                    <div class="row">
                         <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Credit</label>
                            <asp:TextBox ID="txtCredit" runat="server" MaxLength="12" CssClass="form-control"></asp:TextBox>

                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Status</label>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                <asp:ListItem Value="-">All</asp:ListItem>
                                <asp:ListItem Value="A">Approved</asp:ListItem>
                                <asp:ListItem Value="P">Pending</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                       
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                    </div>
				</div>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                             <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click"/>
                            
                            <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" CssClass="btn btn-danger"/>
</div>
     </div>
                    <div class="row">
                        <br />
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:GridView ID="grdVoucher" runat="server" 
             AllowPaging="true" PageSize="30" AutoGenerateColumns="false" RowStyle-VerticalAlign="Top"
             EmptyDataText="No voucher found."  Width="100%" CssClass="table table-responsive-sm"
             OnRowDataBound="grdVoucher_OnRowDataBound" OnPageIndexChanging="grdVoucher_OnPageIndexChanging">
            
             <Columns>
                <asp:BoundField DataField="vr_dt" HeaderText="Date" ItemStyle-Width="70px"/>
                <asp:BoundField DataField="vr_no" HeaderText="Sr No" ItemStyle-Width="40px"/>
                <asp:BoundField DataField="Ref_no" HeaderText="Vr No" ItemStyle-Width="40px"/>
                <asp:BoundField DataField="gl_dsc" HeaderText="Title" ItemStyle-Width="150px"/>
                <asp:BoundField DataField="vr_nrtn" HeaderText="Narration"/>
                <asp:BoundField DataField="cc" HeaderText="Cost Center" ItemStyle-Width="100px"/>
                <asp:BoundField DataField="vr_chq" HeaderText="Chq No" ItemStyle-Width="60px"/>
                <asp:BoundField DataField="vrd_debit" HeaderText="Debit" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="vrd_credit" HeaderText="Credit" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right"/>
             </Columns>
             </asp:GridView>
</div>
     </div>
                </div>
            </div>
        </div>
    </div>





</asp:Content>