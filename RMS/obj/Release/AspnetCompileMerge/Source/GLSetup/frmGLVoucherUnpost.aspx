<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" Culture="auto"
    UICulture="auto" EnableEventValidation="true" AutoEventWireup="true" 
    CodeBehind="frmGLVoucherUnpost.aspx.cs" Inherits="RMS.GLSetup.frmGLVoucherUnpost" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" 
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function prompt4Unpost() {
        return confirm("Are your sure, you want to unpost voucher?");
    }
</script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

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
                    <div class="row">
                          <div class="col-lg-3 col-md-3 col-sm-3">
                            <asp:Label ID="lblBranch" runat="server">Divisions:*</asp:Label>
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged"
                                AppendDataBoundItems="True" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Division</asp:ListItem>
                            </asp:DropDownList>

                            
                            </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <asp:Label ID="lblFromDt" runat="server">From Date</asp:Label>
                        <asp:TextBox ID="txtFromDt" runat="server" CssClass="form-control"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDt" EnableViewState="false"></ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <asp:Label ID="lblToDt" runat="server">To Date</asp:Label>
                        <asp:TextBox ID="txtToDt" runat="server" CssClass="form-control"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDt" EnableViewState="false"></ajaxToolkit:CalendarExtender>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <asp:Label ID="Label1" runat="server">Status</asp:Label>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                            <asp:ListItem Value="0">All</asp:ListItem>
                            <asp:ListItem Value="A"  Selected="True">Approved</asp:ListItem>
                            <asp:ListItem Value="P">Pending</asp:ListItem>
                            <asp:ListItem Value="D">Cancelled</asp:ListItem>
                        </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <asp:Label ID="Label2" runat="server">Voucher Type</asp:Label>
                        <asp:DropDownList ID="ddlVoucherType" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                            <asp:ListItem Value="0"  Selected="True">All</asp:ListItem>
                        </asp:DropDownList>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:ImageButton ID="btnShow" runat="server"  ImageUrl="~/images/btn_show.png" OnCommand="btnShow_Click"
                            onMouseOver="this.src='../images/btn_show_m.png'" onMouseOut="this.src='../images/btn_show.png'"   ValidationGroup="main"/>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:GridView ID="grdVoucher" DataKeyNames="vrid" CssClass="table table-responsive-sm" runat="server"
                                AutoGenerateColumns="False" OnRowDataBound="grdVoucher_RowDataBound" AllowPaging="true" 
                                Width="100%" PageSize="20" OnPageIndexChanging="grdVoucher_PageIndexChanging" RowStyle-VerticalAlign="Top"
                                EmptyDataText="">
                                        <HeaderStyle CssClass ="grid_hdr" />
                                        <RowStyle CssClass="grid_row" />
                                        <AlternatingRowStyle CssClass="gridAlternateRow" />
                                        <SelectedRowStyle CssClass="gridSelectedRow" />
                                <Columns>
                                    <asp:BoundField DataField="vrid"  ItemStyle-CssClass="DisplayNone" ControlStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" FooterStyle-CssClass="DisplayNone"/>
                                    <asp:BoundField DataField="vr_no" HeaderText="Sr#" />
                                    <asp:BoundField DataField="ref_no" HeaderText="Vouche#" ItemStyle-Width="70px"/>
                                    <asp:BoundField DataField="vr_dt" DataFormatString= "{0:dd-MMM-yyyy}" htmlencode="false" HeaderText="Date">
                                        <ItemStyle Wrap="false" Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vr_nrtn" HeaderText="Narration"/>
                                    <asp:BoundField DataField="status" HeaderText="Status" ItemStyle-Width="70px"/>
                                    <asp:TemplateField HeaderText="Options" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                        <ItemTemplate>
                                             <asp:Label ID="lblVoucherTypeID" runat="server"  Visible="false" Text='<%# Bind("vt_cd") %>'>'></asp:Label>
                                             <asp:Label ID="lbl" runat="server"  Visible="false" Text='<%# Bind("vrid") %>'>'></asp:Label>
                                             <asp:Label ID="lblstatus" runat="server"  Visible="false" Text='<%# Bind("status") %>'>'></asp:Label>
                                             <asp:Label ID="lblsr_no" runat="server"  Visible="false" Text='<%# Bind("vr_no") %>'>'></asp:Label>
                                             <asp:Label ID="lblvr_no" runat="server"  Visible="false" Text='<%# Bind("ref_no") %>'>'></asp:Label>
                                             <asp:Label ID="lblvrdt" runat="server"  Visible="false" Text='<%# Bind("vr_dt") %>'>'></asp:Label>
                                             <asp:LinkButton ID="lnkView" runat="server" OnClick="lnkUnpost_Click" Text="Unpost" CssClass="lnk" OnClientClick="return prompt4Unpost()"></asp:LinkButton>
                                             <asp:LinkButton ID="lnkPrint" runat="server" OnClick="lnkPrint_Click" Text="View" CssClass="lnk"></asp:LinkButton>   
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Image ID="imgRemarks" runat="server" ImageUrl="~/images/incomingmsg.jpg"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>   
                         </asp:GridView>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                             <asp:Panel ID="pnlMain" runat="server" Width="99%" Height="400px">
                               <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="95%">
                               </rsweb:ReportViewer>
                           </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
   
  
  
  
 
 

    
  
</asp:Content>