<%@ Page Language="C#"  MasterPageFile="~/home/RMSMasterHome.Master"
 AutoEventWireup="true" CodeBehind="frmVoucherHome.aspx.cs" Culture="auto"
 UICulture="auto" Inherits="RMS.GLSetup.frmVoucherHome" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

	<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js" type="text/javascript"></script>
	<script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.7.2/jquery-ui.min.js" type="text/javascript"></script>
	<link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.7.1/themes/blitzer/jquery-ui.css" type="text/css" />
    <script src="../js/jquery.easy-confirm-dialog.js" type="text/javascript"></script>
<script type="text/javascript">
    function prompt4Approval() {
        return confirm("Are your sure, you want to approve?");
    }
    function prompt4Cancel() {
        return confirm("Are your sure, you want to cancel?");
    }
    $(document).ready(function() {
        //         function pageLoad() {

//                        $(".promptDelete").click(function() {
//                            if (confirm("Are you sure, you want to cancel?") == true)
//                                return true;
//                            else
//                                return false;
//                                        });
        ///////////////////////////////////
        //        $(".promptDelete").easyconfirm({ locale: { title: 'Select Yes or No', button: ['No', 'Yes']} });

        //        $(".promptDelete").click(function() {
        //            //       return true;
        //            
        //            alert("Yes clicked");
        //        });
        /////////////////////////////////////
            $(".promptDelete").easyconfirm({ locale: { title: 'Select Yes or No', button: ['No', 'Yes']} });

            $(".doDelete").click(function() {
                       return true;
            });

        ////////////////////////////////////
        //}
    });
                            

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td width="1%"></td>
        <td>
            <uc1:Messages ID="ucMessage" runat="server" />

            <fieldset class="fieldSet">
               
                <legend style=" margin:5px;">
                        <b> Search</b>
                </legend>
                    <table width="80%" cellspacing="0">
                      <tr>
                          <td>
                               <label>Branch</label>
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged"
                                AppendDataBoundItems="True" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Branch</asp:ListItem>
                            </asp:DropDownList>
                          </td>
                          <td>
                                <asp:Label ID="lbl" Text="From" runat="server"/>
                          </td>
                          <td>
                                <ajaxToolkit:CalendarExtender ID="txtFromDate" runat="server" TargetControlID="txtFrom" Enabled="True"/>
                                <asp:TextBox ID="txtFrom" runat="server" Width="80px"/>
                          </td>
                          <td>
                                <asp:Label ID="Label2" Text="To" runat="server"/>
                            </td>
                            <td>
                                <ajaxToolkit:CalendarExtender ID="txtToDate" runat="server" TargetControlID="txtTo" Enabled="True"/>
                                <asp:TextBox ID="txtTo" runat="server" Width="80px"/>
                            </td>
                           <td>
                                <asp:Label ID="lblstatus" Text="Status" runat="server"/>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlstatus" runat="server" AppendDataBoundItems="True">
                                    <asp:ListItem Value="0">All</asp:ListItem>
                                    <asp:ListItem Value="A">Approved</asp:ListItem>
                                    <asp:ListItem Value="P">Pending</asp:ListItem>
                                    <asp:ListItem Value="D">Cancelled</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:ImageButton ID="btnSearch" ToolTip="Click here to search" runat="server" ImageUrl="~/images/search_icon.png" OnClick="btnSearch_Click" />
                            </td>
                       </tr>
                    </table>
              </fieldset>
        </td>
        <td width="1%"></td>
    </tr>
    <tr>
         <td width="1%"></td>
         <td>
              &nbsp;
         </td>
         <td width="1%"></td>
    </tr>
    <tr>
         <td width="1%"></td>
         <td>
              <asp:ImageButton ID="btnNew" runat="server"  ImageUrl="~/images/btn_generate.png" OnClick="btnNew_Click" onMouseOver="this.src='../images/btn_generate_m.png'" onMouseOut="this.src='../images/btn_generate.png'" />
              <asp:Button runat="server" ID="btnDelete" OnClick="lnkDelete_Click" Visible="false" CssClass="doDelete"/> 
         </td>
         <td width="1%"></td>
    </tr>
    <tr>
         <td width="1%"></td>
         <td>
              &nbsp;
         </td>
         <td width="1%"></td>
    </tr>
    <tr>
         <td width="1%"></td>
         <td>
              <asp:GridView ID="grdVoucher" DataKeyNames="vrid" runat="server"
                     AutoGenerateColumns="False" OnRowDataBound="grdVoucher_RowDataBound" AllowPaging="true" 
                    Width="100%" PageSize="20" OnPageIndexChanging="grdVoucher_PageIndexChanging" RowStyle-VerticalAlign="Top"
                     EmptyDataText="No voucher found.">
                                <HeaderStyle CssClass ="grid_hdr" />
                                <RowStyle CssClass="grid_row" />
                                <AlternatingRowStyle CssClass="gridAlternateRow" />
                                <SelectedRowStyle CssClass="gridSelectedRow" />
                        <Columns>
                                <asp:BoundField DataField="vrid"  ItemStyle-CssClass="DisplayNone" ControlStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone"/>
                                <asp:BoundField DataField="vr_no" HeaderText="Sr#" ItemStyle-Width="70" />
                                <asp:BoundField DataField="ref_no" HeaderText="Voucher#" ItemStyle-Width="70" />
                                <asp:BoundField DataField="vr_dt" DataFormatString= "{0:dd-MMM-yyyy}" htmlencode="false" HeaderText="Date">
                                    <ItemStyle Wrap="false" Width="80" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vr_nrtn" HeaderText="Narration" ItemStyle-Width="300" />
                                <asp:BoundField DataField="status" HeaderText="Status" ItemStyle-Width="80" />
                                
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkstatus" runat="server" />
                                    </ItemTemplate>
                                    <HeaderTemplate> 
                                        <asp:LinkButton ID="btnApprove" runat="server"  OnClick="btnApprove_Click" ToolTip="Click here to approve following checked vouchers" CssClass="ApproveLink" OnClientClick="return prompt4Approval()">Approve</asp:LinkButton>
                                    </HeaderTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="70" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Options" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <asp:Label ID="lbl" runat="server"  Visible="false" Text='<%# Bind("vrid") %>'>'></asp:Label>
                                     <asp:Label ID="lblstatus" runat="server"  Visible="false" Text='<%# Bind("status") %>'>'></asp:Label>
                                     <asp:Label ID="lblsr_no" runat="server"  Visible="false" Text='<%# Bind("vr_no") %>'>'></asp:Label>
                                     <asp:Label ID="lblvr_no" runat="server"  Visible="false" Text='<%# Bind("ref_no") %>'>'></asp:Label>
                                     <asp:Label ID="lblvrdt" runat="server"  Visible="false" Text='<%# Bind("vr_dt") %>'>'></asp:Label>
                                     <asp:LinkButton ID="lnkView" runat="server" OnClick="lnkView_Click" Text="View"></asp:LinkButton>
                                     <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click" Text="Edit"></asp:LinkButton>
                                     <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click" Text="Cancel"  OnClientClick="return prompt4Cancel()"></asp:LinkButton>
                                     <asp:LinkButton ID="lnkPrint" runat="server" OnClick="lnkPrint_Click" Text="Print"></asp:LinkButton>   
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Remarks" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10">
                                <ItemTemplate>
                                    <asp:Image ID="imgRemarks" runat="server" ImageUrl="~/images/incomingmsg.jpg"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                        </Columns>   
                 </asp:GridView>
         </td>
         <td width="1%"></td>
    </tr>
</table>

    
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="99%">
    </rsweb:ReportViewer>
</asp:Content>
