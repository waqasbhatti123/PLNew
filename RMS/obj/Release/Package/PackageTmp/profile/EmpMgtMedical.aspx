<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmpMgtMedical.aspx.cs" Inherits="RMS.Setup.EmpMgtMedical" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script>

        $('document').ready(function() {
            $('#ctl00_ContentPlaceHolder1_txtClaimAmt').blur(function() {
                calcAmt();
            });

            $('#ctl00_ContentPlaceHolder1_txtPaidAmt').blur(function() {
                calcAmt();
            });

        });

        function calcAmt() {
            var amtclaim = parseFloat($('#ctl00_ContentPlaceHolder1_txtClaimAmt').val());
            var amtYTD = parseFloat($('#ctl00_ContentPlaceHolder1_txtClaimYtdOrg').val());
            var lmtYTD = parseFloat($('#ctl00_ContentPlaceHolder1_txtLimYtd').val());
            var paid = parseFloat($('#ctl00_ContentPlaceHolder1_txtPaidAmt').val());
            //var lmtOvrApp = parseFloat($('#ctl00_ContentPlaceHolder1_txtOvLimApp').val());

            var lmtOvr = (amtYTD + amtclaim) - lmtYTD > 0 ? (amtYTD + amtclaim) - lmtYTD : 0;
            var lmtOvrApp = (amtYTD + amtclaim) + lmtOvr - paid;
            //var paid = (amtYTD < lmtYTD? amtclaim + lmtOvrApp : lmtOvrApp);

            $('#ctl00_ContentPlaceHolder1_txtOv').val(lmtOvr);
            $('#ctl00_ContentPlaceHolder1_txtClaimYtd').val(amtYTD + amtclaim);
            $('#ctl00_ContentPlaceHolder1_txtOvLimApp').val(lmtOvrApp);
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
                        <asp:Panel runat="server" ID="pnlMain">
                            <div class="row">
                                <div class="col-lg-8 col-md-8 col-sm-8">
                                    <asp:Label ID="lblEmpSrch" Text="Emp. Name:" runat="server"></asp:Label>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtEmpSrch" MaxLength="150"></asp:TextBox>
                                        <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                                OnClick="btnSearch_Click" ToolTip="Search Employee" />
                                </div>
                            </div>
                            <div runat="server" id="divEmpInfo" visible="false">
                                <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <asp:Label ID="lblEmpName" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                   <asp:Label ID="lblEmpCode" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <asp:Label ID="lblEmpDesig" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <asp:Label ID="lblEmpDept" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6 col-md-6 col-sm-6">
                                    <asp:GridView runat="server" ID="grdEmpSrchUC" CssClass="table table-responsive-sm" DataKeyNames="EmpID" OnSelectedIndexChanged="grdEmpSrchUC_SelectedIndexChanged"
                                    AutoGenerateColumns="False" Visible="false">
                                    <HeaderStyle CssClass="grid_hdr" />
                                    <RowStyle CssClass="grid_row" />
                                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                                    <SelectedRowStyle CssClass="gridSelectedRow" />
                                    <Columns>
                                        <asp:BoundField DataField="EmpCode" HeaderText="Emp No" />
                                        <asp:BoundField DataField="FullName" HeaderText="Name" />
                                        <asp:BoundField DataField="Desig" HeaderText="Desig." />
                                        <asp:BoundField DataField="Dept" HeaderText="Dept." />
                                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                                            <ItemStyle />
                                            <ControlStyle CssClass="lnk"></ControlStyle>
                                        </asp:CommandField>
                                    </Columns>
                                </asp:GridView>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <asp:Label ID="lblbasic" runat="server" Text="Payment Type:"></asp:Label>
                                    <asp:DropDownList ID="ddlPayType" CssClass="form-control" runat="server" TabIndex="1" AppendDataBoundItems="true"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlPayType_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="0">Select Payment Type</asp:ListItem>
                            </asp:DropDownList>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label>Claim Ref No:</label>
                                    <asp:TextBox ID="txtClaimRefNo" CssClass="form-control" runat="server" TabIndex="2"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Claim Reference number required"
                                ControlToValidate="txtClaimRefNo" SetFocusOnError="true" ValidationGroup="main"
                                Display="None"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label>Refrence Date:</label><asp:Literal ID="Literal2" runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                                    <ajaxToolkit:CalendarExtender ID="txtEffDateCal" runat="server" TargetControlID="txtEffDate"
                                Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                            <asp:TextBox ID="txtEffDate" runat="server" MaxLength="11" CssClass="form-control"  OnTextChanged="txtEffDate_TextChanged"
                                TabIndex="3"></asp:TextBox>
                           
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Reference Date Require"
                                ControlToValidate="txtEffDate" SetFocusOnError="true" ValidationGroup="main"
                                Display="None"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            &nbsp;
                            <div class="row">
                                
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label>Claim Amount:</label>
                                    <asp:TextBox ID="txtClaimAmt" CssClass="form-control" runat="server" Style="text-align: left" TabIndex="4"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Claim Amount Require"
                                ControlToValidate="txtClaimAmt" SetFocusOnError="true" ValidationGroup="main"
                                Display="None"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <asp:Label ID="lblPaidAMount" runat="server" Text="Approved Amount:"></asp:Label>
                                    <asp:TextBox ID="txtAppBy" CssClass="form-control" runat="server" TabIndex="9"></asp:TextBox>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label>Approved Amount:</label>
                                    <asp:TextBox ID="txtPaidAmt" CssClass="form-control" runat="server" Style="text-align: left" TabIndex="8"
                                BorderStyle="Solid"></asp:TextBox>
                                </div>
                            </div>
                            &nbsp;
                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                     <label>Over Limit Approved:</label>
                                     <asp:TextBox ID="txtOvLimApp" CssClass="form-control" runat="server" Style="text-align: right" TabIndex="8"
                                ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            &nbsp;
                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label>Limit YTD:</label>
                                    <asp:TextBox ID="txtLimYtd" Enabled="false" CssClass="form-control" ReadOnly="true" runat="server" Style="text-align: left"
                                TabIndex="5" BorderStyle="None"></asp:TextBox>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label>Claim YTD</label>
                                    <asp:TextBox ID="txtClaimYtd" ReadOnly="true" CssClass="form-control" Enabled="false" runat="server" Style="text-align: right"
                                TabIndex="6" BorderStyle="None"></asp:TextBox>
                            <asp:TextBox ID="txtClaimYtdOrg" runat="server" Style="visibility: hidden"></asp:TextBox>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label>Over Limit:</label>
                                    <asp:TextBox ID="txtOv" CssClass="form-control" runat="server" Style="text-align: left" TabIndex="8"
                                ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
                <!--img src="images/btn_new.jpg" width="60" height="20" /> <img src="images/btn_edit.jpg" width="60" height="20" /> <img src="images/btn_delete.jpg" alt="" width="60" height="20" /> <img src="images/btn_save.jpg" width="60" height="20" /> <img src="images/btn_cancel.jpg" width="60" height="20" /-->
                                </div>
                            </div>
                            </asp:Panel>
                            
                            
                        <div runat="server" visible="false">
                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <asp:Label ID="lblFltName" runat="server" Text="Emp Name:"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtFltEmp" Width="100"></asp:TextBox>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <asp:Label ID="lblFltReg" runat="server" Text="Company:"></asp:Label>
                                    <asp:DropDownList runat="server" ID="ddlFltRegion" AppendDataBoundItems="true">
                                    <asp:ListItem Value="0">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <asp:Label ID="lblFltSegment" runat="server" Text="City:"></asp:Label><br />
                                    <asp:DropDownList runat="server" ID="ddlFltSegment" AppendDataBoundItems="true">
                                    <asp:ListItem Value="0">All</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            &nbsp;
                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <asp:ImageButton ID="btnSearch1" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                    OnClick="btnSearch_Click" ToolTip="Search Emps" />
                                </div>
                            </div>
                            &nbsp;
                            <div class="row">
                                <div class="col-lg-6 col-md-6 col-sm-6">
                                    <div runat="server" id="divgrdEmps">
                    <asp:GridView ID="grdEmps" runat="server" CssClass="table table-responsive-sm" DataKeyNames="EmpID,CompID,ExpTypeID,ExpYear,ExpRef,ExpAprovby"
                        OnSelectedIndexChanged="grdEmps_SelectedIndexChanged" AutoGenerateColumns="False"
                        AllowPaging="True" OnPageIndexChanging="grdEmps_PageIndexChanging" OnRowDataBound="grdEmps_RowDataBound"
                        EmptyDataText="No Expence Data is available" Width="600px">
                        <Columns>
                            <asp:BoundField DataField="expdate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}">
                            </asp:BoundField>
                            <asp:BoundField DataField="expref" HeaderText="Ref. #" />
                            <asp:BoundField DataField="type" HeaderText="Type" />
                            <asp:BoundField DataField="Amount" HeaderText="Claimed" />
                            <asp:BoundField DataField="LimitAprove" HeaderText="Approved" />
                            <asp:BoundField DataField="AproveBy" HeaderText="Aproved By" />
                            <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                <ControlStyle CssClass="lnk"></ControlStyle>
                            </asp:CommandField>
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
                </div>
            </div>
        </div>
    </div>






   <%-- <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="3%">
            </td>
            <td>
                <%--<asp:Panel runat="server" ID="pnlMain">--%>
               <%--- %> <table cellspacing="0" class="stats2" align="center">
                    <tr>
                        <td colspan="4" valign="top" class="bg_input_area">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div align="left">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblEmpSrch" Text="Emp. Name:" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtEmpSrch" MaxLength="150"></asp:TextBox>
                                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                                OnClick="btnSearch_Click" ToolTip="Search Employee" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <div id="divEmpInfo" runat="server" visible="false">
                                                <table cellspacing="3" cellpadding="2" class="DivTble" width="100%">
                                                    <tr>
                                                        <td class="style1">
                                                            <asp:Label ID="lblEmpName" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style1">
                                                            <asp:Label ID="lblEmpCode" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style1">
                                                            <asp:Label ID="lblEmpDesig" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style1">
                                                            <asp:Label ID="lblEmpDept" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView runat="server" ID="grdEmpSrchUC" DataKeyNames="EmpID" OnSelectedIndexChanged="grdEmpSrchUC_SelectedIndexChanged"
                                    AutoGenerateColumns="False" Visible="false">
                                    <HeaderStyle CssClass="grid_hdr" />
                                    <RowStyle CssClass="grid_row" />
                                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                                    <SelectedRowStyle CssClass="gridSelectedRow" />
                                    <Columns>
                                        <asp:BoundField DataField="EmpCode" HeaderText="Emp No" />
                                        <asp:BoundField DataField="FullName" HeaderText="Name" />
                                        <asp:BoundField DataField="Desig" HeaderText="Desig." />
                                        <asp:BoundField DataField="Dept" HeaderText="Dept." />
                                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                                            <ItemStyle />
                                            <ControlStyle CssClass="lnk"></ControlStyle>
                                        </asp:CommandField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div align="left">
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Limit YTD:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLimYtd" Enabled="false" ReadOnly="true" runat="server" Style="text-align: right"
                                TabIndex="5" BorderStyle="None"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblbasic" runat="server" Text="Payment Type:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPayType" runat="server" TabIndex="1" AppendDataBoundItems="true"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlPayType_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="0">Select Payment Type</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Claim YTD"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtClaimYtd" ReadOnly="true" Enabled="false" runat="server" Style="text-align: right"
                                TabIndex="6" BorderStyle="None"></asp:TextBox>
                            <asp:TextBox ID="txtClaimYtdOrg" runat="server" Style="visibility: hidden"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label7" runat="server" Text="Claim Ref. No.:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtClaimRefNo" runat="server" TabIndex="2"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Claim Reference number required"
                                ControlToValidate="txtClaimRefNo" SetFocusOnError="true" ValidationGroup="main"
                                Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Over Limit:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOv" ReadOnly="true" Enabled="false" runat="server" Style="text-align: right"
                                TabIndex="7" BorderStyle="None"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label13" runat="server" Text="Reference Date:"></asp:Label>
                        </td>
                        <td>
                            <ajaxToolkit:CalendarExtender ID="txtEffDateCal" runat="server" TargetControlID="txtEffDate"
                                Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                            <asp:TextBox ID="txtEffDate" runat="server" MaxLength="11" Width="80px" OnTextChanged="txtEffDate_TextChanged"
                                TabIndex="3"></asp:TextBox>
                            <asp:Literal ID="Literal1" runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Reference Date Require"
                                ControlToValidate="txtEffDate" SetFocusOnError="true" ValidationGroup="main"
                                Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label ID="lblPaidAMount" runat="server" Text="Approved Amount:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPaidAmt" runat="server" Style="text-align: right" TabIndex="8"
                                BorderStyle="Solid"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Claim Amount:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtClaimAmt" runat="server" Style="text-align: right" TabIndex="4"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Claim Amount Require"
                                ControlToValidate="txtClaimAmt" SetFocusOnError="true" ValidationGroup="main"
                                Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Over Limit Approved:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOvLimApp" runat="server" Style="text-align: right" TabIndex="8"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Approved By:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAppBy" runat="server" TabIndex="9"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <%-- </asp:Panel>  --%>
           <%---- </td>
        </tr>
        <tr>
            <td align="center" valign="top" colspan="6">
                <br />
                <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
                <!--img src="images/btn_new.jpg" width="60" height="20" /> <img src="images/btn_edit.jpg" width="60" height="20" /> <img src="images/btn_delete.jpg" alt="" width="60" height="20" /> <img src="images/btn_save.jpg" width="60" height="20" /> <img src="images/btn_cancel.jpg" width="60" height="20" /-->
            </td>
        </tr>
        <tr>
            <td width="3%">
            </td>
            <td valign="top">
                <br />
                <div runat="server" visible="false">
                    <table class="filterTable" width="100%">
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblFltName" runat="server" Text="Emp Name:"></asp:Label><br />
                                <asp:TextBox runat="server" ID="txtFltEmp" Width="100"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblFltReg" runat="server" Text="Company:"></asp:Label><br />
                                <asp:DropDownList runat="server" ID="ddlFltRegion" AppendDataBoundItems="true">
                                    <asp:ListItem Value="0">All</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblFltSegment" runat="server" Text="City:"></asp:Label><br />
                                <asp:DropDownList runat="server" ID="ddlFltSegment" AppendDataBoundItems="true">
                                    <asp:ListItem Value="0">All</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:ImageButton ID="btnSearch1" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                    OnClick="btnSearch_Click" ToolTip="Search Emps" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div runat="server" id="divgrdEmps">
                    <asp:GridView ID="grdEmps" runat="server" DataKeyNames="EmpID,CompID,ExpTypeID,ExpYear,ExpRef,ExpAprovby"
                        OnSelectedIndexChanged="grdEmps_SelectedIndexChanged" AutoGenerateColumns="False"
                        AllowPaging="True" OnPageIndexChanging="grdEmps_PageIndexChanging" OnRowDataBound="grdEmps_RowDataBound"
                        EmptyDataText="No Expence Data is available" Width="600px">
                        <Columns>
                            <asp:BoundField DataField="expdate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}">
                            </asp:BoundField>
                            <asp:BoundField DataField="expref" HeaderText="Ref. #" />
                            <asp:BoundField DataField="type" HeaderText="Type" />
                            <asp:BoundField DataField="Amount" HeaderText="Claimed" />
                            <asp:BoundField DataField="LimitAprove" HeaderText="Approved" />
                            <asp:BoundField DataField="AproveBy" HeaderText="Aproved By" />
                            <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                <ControlStyle CssClass="lnk"></ControlStyle>
                            </asp:CommandField>
                        </Columns>
                        <HeaderStyle CssClass="grid_hdr" />
                        <RowStyle CssClass="grid_row" />
                        <AlternatingRowStyle CssClass="gridAlternateRow" />
                        <SelectedRowStyle CssClass="gridSelectedRow" />
                    </asp:GridView>
                </div>
            </td>
            <td width="3%">
            </td>
        </tr>
    </table>--%>
</asp:Content>
