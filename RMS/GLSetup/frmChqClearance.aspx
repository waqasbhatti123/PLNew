<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="frmChqClearance.aspx.cs" Inherits="RMS.GLSetup.frmChqClearance"
    Culture="auto" UICulture="auto" EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
  <div class="row">
                        <div class="col-md-12">
                            <div class="card card-shadow mb-4 ">
                                <div class="card-body">

                                   
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td width="3%"></td>
                                            <td>
                                                <asp:Panel ID="pnlInfo" runat="server" Visible="false">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <b>Bank:</b>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:Label ID="lblBank" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <b>Voucher No:</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblAC" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <b>Cheque No:</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblChq" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <b>Cheque Date:</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblChqDt" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <b>Amount:</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblAmount" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">&nbsp;</td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="pnlMain">
                                                                                      <div class="row">
                                        <div class="col-md-4">
                                             <asp:Label ID="Label7" runat="server" Text="Clearance Date:"></asp:Label>
                                             <asp:TextBox ID="txtClrDate" runat="server" CssClass="RequiredField form-control"></asp:TextBox>
                                                                <ajaxToolkit:CalendarExtender ID="calClrDate" runat="server" Enabled="True"
                                                                    TargetControlID="txtClrDate">
                                                                </ajaxToolkit:CalendarExtender>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please select clearance date"
                                                                    ControlToValidate="txtClrDate" SetFocusOnError="true" ValidationGroup="main"
                                                                    Display="None"></asp:RequiredFieldValidator>
                                                                <span class="DteLtrl">
                                                                    <asp:Literal ID="Literal2" runat="server" Text="<%$ AppSettings: DateFormatPageText %>" /></span>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label ID="lblbasic" runat="server" Text="Clear:"></asp:Label>
                                              <asp:CheckBox ID="chkClear" runat="server" Checked="true" />
                                        </div>
                                    </div>
                                    
                                    <div class="row">
                                        <div class="col-md-4">
                                             <asp:Label ID="Label1" runat="server" Text="Divisions*"></asp:Label><br />
                                                                <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="searchbranchchange form-control" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged"
                                                                    AppendDataBoundItems="True" AutoPostBack="true">
                                                                    <asp:ListItem Value="0">Select Division</asp:ListItem>
                                                                </asp:DropDownList>
                                        </div>
                                        <div class="col-md-4">
                                             
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-4">
                                            <asp:Label ID="Label4" runat="server" Text="From Date*"></asp:Label><br />
                                                                <asp:TextBox runat="server" ID="txtFltFromDt" CssClass="form-control"></asp:TextBox>
                                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                                                    TargetControlID="txtFltFromDt">
                                                                </ajaxToolkit:CalendarExtender>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label ID="Label5" runat="server" Text="To Date*"></asp:Label><br />
                                                                <asp:TextBox runat="server" ID="txtFltToDt" CssClass="form-control"></asp:TextBox>
                                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True"
                                                                    TargetControlID="txtFltToDt">
                                                                </ajaxToolkit:CalendarExtender>
                                        </div>
                                    </div>

                                     <div class="row">
                                        <div class="col-md-4">
                                            <asp:Label ID="lblFltName" runat="server" Text="Bank"></asp:Label><br />
                                                                <asp:TextBox runat="server" ID="txtFltBank" MaxLength="20" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label ID="Label22" runat="server" Text="Voucher No."></asp:Label><br />
                                                                <asp:TextBox runat="server" ID="txtFltVch" MaxLength="20" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>


                                    
                                    <div class="row">
                                        <div class="col-md-4">
                                             <asp:Label ID="Label3" runat="server" Text="Cheque No"></asp:Label><br />
                                                                <asp:TextBox runat="server" ID="txtFltChq" MaxLength="20" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label ID="Label2" runat="server" Text="Type*"></asp:Label><br />
                                                                <asp:DropDownList ID="ddlFltType" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Value="0">Pending</asp:ListItem>
                                                                    <asp:ListItem Value="1">Cleared</asp:ListItem>
                                                                </asp:DropDownList>
                                              
                                        </div>
                                        <div class="col-md-4">
                                            <br />
                                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                                                    OnClick="btnSearch_Click" ToolTip="Search Emps" />
                                        </div>
                                    </div>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" valign="top" colspan="10">
                                               <br />
                                                <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" ValidationGroupName="main" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="3%"></td>
                                            <td valign="top">
                                               
                                                <asp:Panel ID="pnlFlter" runat="server" DefaultButton="btnSearch">
                                                    <table class="filterTable" width="100%">
                                                        <tr>
                                                            <td>
                                                               
                                                            </td>
                                                            <td>
                                                               
                                                            </td>
                                                            <td>
                                                                
                                                            </td>
                                                            <td>
                                                                
                                                            </td>
                                                            <td>
                                                               
                                                            </td>
                                                            <td>
                                                                
                                                            </td>
                                                            <td>
                                                                
                                                            </td>
                                                            <td>
                                                              
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:GridView ID="grdChq" runat="server" OnRowDataBound="grdChq_RowDataBound" PageSize="20"
                                                    AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No cheques are available"
                                                    OnSelectedIndexChanged="grdChq_SelectedIndexChanged" DataKeyNames="vrid"
                                                    Width="100%" OnPageIndexChanging="grdChq_PageIndexChanging">
                                                    <Columns>
                                                        <asp:BoundField DataField="bnkcd" HeaderText="Code" Visible="false" />
                                                        <asp:BoundField DataField="bnk" HeaderText="Bank" />
                                                        <asp:BoundField DataField="party" HeaderText="Party" />
                                                        <asp:BoundField DataField="Ref_No" HeaderText="Voucher No." />
                                                        <asp:BoundField DataField="chq" HeaderText="Cheque No." />
                                                        <asp:BoundField DataField="chqdt" HeaderText="Cheque Date" />
                                                        <asp:BoundField DataField="amount" HeaderText="Amount" />
                                                        <asp:BoundField DataField="Chq_clr_dt" HeaderText="Clearance Date" />
                                                        <asp:BoundField DataField="Clr_entryBy" HeaderText="Entered By" Visible="false" />
                                                        <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                                            <ControlStyle CssClass="lnk"></ControlStyle>
                                                        </asp:CommandField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="grid_hdr" />
                                                    <RowStyle CssClass="grid_row" />
                                                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                                                    <SelectedRowStyle CssClass="gridSelectedRow" />
                                                </asp:GridView>
                                                &nbsp;
                                            </td>
                                            <td width="3%"></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    
</asp:Content>
