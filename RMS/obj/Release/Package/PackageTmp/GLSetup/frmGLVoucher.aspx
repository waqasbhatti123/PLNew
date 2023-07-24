<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" Culture="auto"
    UICulture="auto" EnableEventValidation="true" AutoEventWireup="true" 
    CodeBehind="frmGLVoucher.aspx.cs" Inherits="RMS.GLSetup.frmGLVoucher" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" 
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function prompt4Approval() {
        return confirm("Are your sure, you want to approve?");
    }
    function prompt4Cancel() {
        return confirm("Are your sure, you want to cancel?");
    }
</script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
  <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />

    <div class="row">
        <div class="col-md-12">
            <div class="card card-shadow mb-4 ">
                <div class="card-body">


                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="1%"></td>
                            <td>
                                <table width="100%" cellpadding="0" cellspacing="0" class="HeaderBar">
                                    <tr>
                                        <td>
                                            <label>Branch</label>
                                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged"
                                                AppendDataBoundItems="True" AutoPostBack="true">
                                                <asp:ListItem Value="0">Select Branch</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblVoucher" runat="server">Voucher Type</asp:Label>
                                            <asp:DropDownList ID="ddlVoucher" runat="server">
                                                <asp:ListItem Value="0" Selected="True">All</asp:ListItem>

                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" runat="server">Status</asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" Width="150px">
                                                <asp:ListItem Value="0">All</asp:ListItem>
                                                <asp:ListItem Value="A">Approved</asp:ListItem>
                                                <asp:ListItem Value="P" Selected="True">Pending</asp:ListItem>
                                                <asp:ListItem Value="D">Cancelled</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                       
                                    </tr>
                                    <tr>

                                        <td>
                                            <asp:Label ID="lblFromDt" runat="server">From Date</asp:Label>
                                            <asp:TextBox ID="txtFromDt" runat="server" Width="100px"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDt" EnableViewState="false"></ajaxToolkit:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblToDt" runat="server">To Date</asp:Label>
                                            <asp:TextBox ID="txtToDt" runat="server" Width="100px"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDt" EnableViewState="false"></ajaxToolkit:CalendarExtender>
                                        </td>
                                         <td>
                                            <asp:ImageButton ID="btnShow" runat="server" ImageUrl="~/images/btn_show.png" OnCommand="btnShow_Click"
                                                onMouseOver="this.src='../images/btn_show_m.png'" onMouseOut="this.src='../images/btn_show.png'" ValidationGroup="main" />
                                            <asp:ImageButton ID="btnClear" runat="server" ImageUrl="~/images/btn_clear.png" OnCommand="btnClear_Click"
                                                onMouseOver="this.src='../images/btn_clear_m.png'" onMouseOut="this.src='../images/btn_clear.png'" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td width="1%"></td>
                        </tr>
                        <tr>
                            <td width="1%"></td>
                            <td>&nbsp;
                            </td>
                            <td width="1%"></td>
                        </tr>
                        <%--<tr>
        <td width="1%"></td>
        <td>
            <table width="100%" border="1px" cellpadding="0" cellspacing="0" class="HeaderBar">
                <tr>
                    <th width="70px">Vr No</th>
                    <th width="80px">Date</th>
                    <th>Narration</th>
                    <th width="70px">Status</th>
                    <th width="120px">Options</th>
                    <th width="20px"></th>
                </tr>
            </table>
        </td>
        <td width="1%"></td>
     </tr>--%>
                        <%--  <tr>
        <td width="1%"></td>
        <td>
        &nbsp;
        </td>
        <td width="1%"></td>
     </tr>--%>
                        <%--JOURNAL VOUCHER--%>
                       <%-- <tr>
                            <td width="1%"></td>
                            <td>--%>
                                <%--<table cellpadding="0" cellspacing="0" width="100%" class="HeaderBar">
                <tr>
                    <td>
                          <asp:Label ID="lblJV" runat="server">Journal Voucher</asp:Label>
                    </td>
                </tr>
            </table>--%>
                        <tr>
                            <td width="1%"></td>
                            <td>
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:GridView ID="grdVoucher" DataKeyNames="vrid" runat="server"
                                                AutoGenerateColumns="False" OnRowDataBound="grdVoucher_RowDataBound" AllowPaging="true"
                                                Width="100%" PageSize="15" OnPageIndexChanging="grdVoucher_PageIndexChanging" RowStyle-VerticalAlign="Top"
                                                EmptyDataText="">
                                                <HeaderStyle CssClass="grid_hdr" />
                                                <RowStyle CssClass="grid_row" />
                                                <AlternatingRowStyle CssClass="gridAlternateRow" />
                                                <SelectedRowStyle CssClass="gridSelectedRow" />
                                                <Columns>
                                                    <asp:BoundField DataField="vrid" ItemStyle-CssClass="DisplayNone" ControlStyle-CssClass="DisplayNone" FooterStyle-CssClass="DisplayNone" />
                                                    <asp:BoundField DataField="vr_no" HeaderText="Sr#" />
                                                    <asp:BoundField DataField="ref_no" HeaderText="Voucher#" ItemStyle-Width="70px" />
                                                    <asp:BoundField DataField="vr_dt" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false" HeaderText="Date" HeaderStyle-CssClass="DisplayNone">
                                                        <ItemStyle Wrap="false" Width="80px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vr_nrtn" HeaderText="Narration" />
                                                    <asp:BoundField DataField="status" HeaderText="Status" ItemStyle-Width="70px" />
                                                    <asp:TemplateField HeaderText="Options" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                        <ItemTemplate>
                                                            <%--<asp:Label ID="lblVoucherTypeID" runat="server"  Visible="false" Text="1"></asp:Label>--%>
                                                            <asp:Label ID="lbl" runat="server" Visible="false" Text='<%# Bind("vrid") %>'>'></asp:Label>
                                                            <asp:Label ID="lblstatus" runat="server" Visible="false" Text='<%# Bind("status") %>'>'></asp:Label>
                                                            <asp:Label ID="lblsr_no" runat="server" Visible="false" Text='<%# Bind("vr_no") %>'>'></asp:Label>
                                                            <asp:Label ID="lblvr_no" runat="server" Visible="false" Text='<%# Bind("ref_no") %>'>'></asp:Label>
                                                            <asp:Label ID="lblvrdt" runat="server" Visible="false" Text='<%# Bind("vr_dt") %>'>'></asp:Label>
                                                            <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click" Text="Edit" CssClass="lnk"></asp:LinkButton>
                                                            <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click" Text="Cancel" CssClass="lnk" OnClientClick="return prompt4Cancel()"></asp:LinkButton>
                                                            <asp:LinkButton ID="lnkPrint" runat="server" OnClick="lnkPrint_Click" Text="Print" CssClass="lnk"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgRemarks" runat="server" ImageUrl="~/images/incomingmsg.jpg" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td width="1%"></td>
                        </tr>
                    </table>

                </div>

            </div>
        </div>
    </div>
                                    
 <br />
 
 <asp:Panel ID="pnlMain" runat="server" Width="99%" Height="400px">
     <rsweb:ReportViewer ID="viewer" runat="server" Width="100%" Height="95%">
     </rsweb:ReportViewer>
 </asp:Panel>
    
  
    
  
</asp:Content>