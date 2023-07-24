<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/home/RMSMasterHome.Master"
 CodeBehind="frmViewVoucher.aspx.cs" Culture="auto" UICulture="auto" 
 Inherits="RMS.GLSetup.frmViewVoucher" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function prompt4Approval() {
        return confirm("Are your sure, you want to approve?");
    } 
    function prompt4Cancel() {
        return confirm("Are your sure, you want to cancel?");
    }
    function prompt4AUnpost() {
        return confirm("Are your sure, you want to unpost voucher?");
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">





<table width="90%">
<tr>
<td width="100px">
Voucher #:
</td>
<td width="550px">
<asp:Label ID="lblvoucher" runat="server"></asp:Label>
</td>
<tr>
<td>
Date:
</td>
<td>
<asp:Label ID="lbldate" runat="server"></asp:Label>
</td>
</tr>
<tr>
<td>
Status:
</td>
<td>
<asp:Label ID="lblstatus" runat="server"></asp:Label>
</td>
</tr>
<tr>
<td>
Narration:
</td>
<td>
<asp:Label ID="lblnarration" runat="server"></asp:Label>
</td>
</tr>
<tr>
<td>
<asp:Label ID="lblRefSourceHdr" runat="server"></asp:Label>
</td>
<td>
<asp:Label ID="lblRefSource" runat="server"></asp:Label>
</td>
</tr>
</table>
<br />
<br />

    <asp:GridView ID="grdVoucher" runat="server" AutoGenerateColumns="False" 
    Width="100%" PageSize="20" OnRowDataBound ="grdVoucher_RowDataBound"
    OnPageIndexChanging="grdVoucher_PageIndexChanging" ShowFooter="True" RowStyle-VerticalAlign="Top">
    <HeaderStyle CssClass ="grid_hdr" />
    <FooterStyle CssClass="FooterStyle" />
    <RowStyle CssClass="grid_row" />
    <AlternatingRowStyle CssClass="gridAlternateRow" />
     <SelectedRowStyle CssClass="gridSelectedRow" />
        <Columns>
            <asp:BoundField DataField="vr_seq" HeaderText="Seq" />
            <asp:BoundField DataField="gl_cd" HeaderText="GL Code" />
            <asp:BoundField DataField="gl_dsc" HeaderText="Description" />
            <asp:BoundField datafield="vrd_debit" HeaderText="Debit" />
            <asp:BoundField datafield="vrd_credit" HeaderText="Credit" />
            <asp:BoundField datafield="vrd_nrtn" HeaderText="Remarks" />
            <asp:BoundField datafield="cc_nme" HeaderText="Cost Center" />
            <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Label ID="lbldebit" runat="server"  Visible="false" Text='<%# Bind("vrd_debit") %>'>'></asp:Label>
                         <asp:Label ID="lblcredit" runat="server"  Visible="false" Text='<%# Bind("vrd_credit") %>'>'></asp:Label>
                     </ItemTemplate>
               </asp:TemplateField>
                         
                
        </Columns>
    </asp:GridView>
     <br />
     
     <div runat="server" id="divBankData" visible="false"> 
     
     <table> 
        <tr>
            <td colspan="3"><br />
                <b>Cheque Details:</b>
            </td>
        </tr>
        <tr><td>&nbsp;</td></tr>
        <tr valign="top">
            <td>
                <b>Branch:</b>
            </td>
            <td style="width:70">&nbsp;</td>
            <td>
                <asp:Label ID="lblChqBranch" runat="server"></asp:Label>
                
            </td>
            </tr>
            <tr>
            <td>
                <b>Cheque #:</b>
            </td>
            <td style="width:70">&nbsp;</td>
            <td>
                <asp:Label ID="lblChqNo" runat="server" ></asp:Label>
                
            </td>
            </tr>
            <tr>
            <td>
                <b>Date:</b>
            </td>
            <td style="width:70">&nbsp;</td>
            <td>
                
                <asp:Label ID="lblChqDate" runat="server"></asp:Label>
                
            </td>
            </tr>
            <tr>
            <td>
                <b>Account #:</b>
            </td>
            <td style="width:70">&nbsp;</td>
            <td>
                <asp:Label ID="lblChqAcctNo" runat="server"></asp:Label>
            </td>
        </tr>
        <tr><td>&nbsp;</td></tr>
     </table>
     
     </div>
     
     
     
     <br />
     <div id="divRem" runat="server" style="float:right;">
         <table cellpadding="0" cellspacing="0" border="0" width="410" style="background-color:#e2e4e6;">
            <tr>
                <td width="1%">&nbsp;</td>
                <td>
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td colspan="2">
                                    <div class="model_popup_panel_hdr">Remarks</div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                   &nbsp; <uc1:Messages ID="ucMessage" runat="server" />
                                </td>
                            </tr>
                        </table>
                    <div id="divRemEntry" runat="server">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr valign="top">
                                <td colspan="2">
                                    <asp:TextBox ID="txtRemarks" runat="server" Width="99.5%" Height="80" TextMode="MultiLine" onkeyup="LimitText(this,500);" onblur="LimitText(this,500);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ImageButton ID="btnSave" runat="server"  ImageUrl="~/images/btn_save.png" OnClick="btnSave_Click" onMouseOver="this.src='../images/btn_save_m.png'" onMouseOut="this.src='../images/btn_save.png'"/>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                        </table>
                     </div>
                     <div>
                            <asp:GridView ID="grdRemarks" runat="server" AutoGenerateColumns="False" EmptyDataText="No remarks found" Width="100%" PageSize="3" OnPageIndexChanging="grdRemarks_PageIndexChanging" OnRowDataBound="grdRemarks_RowDataBound" RowStyle-VerticalAlign="Top">
                                <HeaderStyle CssClass ="grid_hdr" />
                                <FooterStyle CssClass="FooterStyle" />
                                <RowStyle CssClass="grid_row" />
                                <AlternatingRowStyle CssClass="gridAlternateRow" />
                                 <SelectedRowStyle CssClass="gridSelectedRow" />
                                    <Columns>
                                        <asp:BoundField DataField="Rmk_seq" HeaderText="Seq" ItemStyle-ForeColor="Red" ItemStyle-Width="20%" ItemStyle-CssClass="DisplayNone" ControlStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone"/>
                                        <asp:BoundField DataField="updateon" HeaderText="Date" ItemStyle-ForeColor="Red" ItemStyle-Width="20%" />
                                        <asp:BoundField DataField="Remark" HeaderText="Existing Remarks" />
                                    </Columns>
                            </asp:GridView>
                     </div>
                </td>
                <td width="1%">&nbsp;</td>
            </tr>
         </table>
     </div>
     
     <br />
     
     
     
    <asp:ImageButton ID="btnUnpost" runat="server"  ImageUrl="~/images/btn_unpost.png" OnClick="btnUnpost_Click"
       onMouseOver="this.src='../images/btn_unpost_m.png'" onMouseOut="this.src='../images/btn_unpost.png'" OnClientClick="return prompt4AUnpost()" Visible="false" /> 
    <asp:ImageButton ID="btnApprove" runat="server"  ImageUrl="~/images/btn_approve.png" OnClick="btnApprove_Click"
       onMouseOver="this.src='../images/btn_approve_m.png'" onMouseOut="this.src='../images/btn_approve.png'" OnClientClick="return prompt4Approval()" Visible="false" />
       <asp:ImageButton ID="btnCancel" runat="server"  ImageUrl="~/images/btn_cancel.png" OnClick="btnCancel_Click"
       onMouseOver="this.src='../images/btn_cancel_m.png'" onMouseOut="this.src='../images/btn_cancel.png'" OnClientClick="return prompt4Cancel()" Visible="false"/>
       <asp:ImageButton ID="btnPrint" runat="server"  ImageUrl="~/images/btn_print.png" OnClick="btnPrint_Click"
       onMouseOver="this.src='../images/btn_print_m.png'" onMouseOut="this.src='../images/btn_print.png'" />
    <asp:ImageButton ID="btnBack" runat="server"  ImageUrl="~/images/btn_back.png" OnClick="btnback_Click" 
       onMouseOver="this.src='../images/btn_back_m.png'" onMouseOut="this.src='../images/btn_back.png'" />
       <%--OnClientClick="javascript:history.back(); return false;"--%>
        
    
    
    
    
    
    
    
    
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="99%">
    </rsweb:ReportViewer>
    </asp:Content>