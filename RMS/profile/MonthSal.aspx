<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="MonthSal.aspx.cs" Inherits="RMS.Setup.MonthSal" Culture="auto" UICulture="auto"
    EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
    <%--<table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
            <td width="3%"></td>
            <td>
               <asp:Panel runat="server" ID="pnlMain">
                <table cellspacing="0" class="stats2" align="center">
                  <tr>
                    <td colspan="6" valign="top" class="bg_input_area"></td>
                  </tr>
                  <tr>
                  <%--  <td colspan="2">
                        <div align="left"><uc3:EmpSearchUC ID="EmpSrchUC" runat="server" /></div>
                    
                    </td>--%>
    <%--<td>
                        <asp:Label ID="Label7" runat="server" Text="Start Date:"></asp:Label>
                    </td>
                    <td>
                        <ajaxToolkit:CalendarExtender ID="txtStartDateCal" runat="server" 
                            TargetControlID="txtStartDate" Enabled="True">
                        </ajaxToolkit:CalendarExtender>
                        <asp:TextBox ID="txtStartDate" runat="server" MaxLength="11" Width="80px"></asp:TextBox> <asp:Literal ID="Literal2" runat="server" Text="<%$ AppSettings: DateFormatPageText %>"/>
                    </td>
                    
                </tr>
                 <tr>
                    <td>
                        <asp:Label ID="lblbasic" runat="server" Text="Leave Type:" ></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlleaveType" runat="server" AppendDataBoundItems="true">
                            <asp:ListItem Value="0">Select Leave Type</asp:ListItem>
                        
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label13" runat="server" Text="End Date:"></asp:Label>
                    </td>
                    <td>
                        <ajaxToolkit:CalendarExtender ID="txtEndDateCal" runat="server" 
                            TargetControlID="txtEndDate" Enabled="True">
                        </ajaxToolkit:CalendarExtender>
                        <asp:TextBox ID="txtEndDate" runat="server" MaxLength="11" Width="80px"></asp:TextBox> <asp:Literal ID="Literal1" runat="server" Text="<%$ AppSettings: DateFormatPageText %>"/>
                    </td>
                </tr>
                 

              </table>
                </asp:Panel>--%>
    <asp:Label ID="Label1" runat="server" Text="Emp Code">
    </asp:Label><asp:TextBox ID="txtempcode" runat="server" Style="text-align: right"
        TabIndex="1"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Employee Code Require"
        ControlToValidate="txtempcode" SetFocusOnError="true" ValidationGroup="main"
        Display="None"></asp:RequiredFieldValidator>
    <asp:DropDownList ID="ddlPayPred" runat="server" AppendDataBoundItems="true" TabIndex="2">
    </asp:DropDownList>
    <asp:Button ID="Button1" runat="server" Text="Show" OnClick="Button1_Click" />
    <br />
    <div id="main_area" visible="false" runat="server">
        <table class="style1">
            <tr>
                <td>
                    <asp:Label ID="LblName" runat="server" Text="Name"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LblDispName" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LblMonthDays" runat="server" Text="Month Days" Font-Bold="False" Font-Names="Times New Roman"
                        Font-Size="Larger"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LblDispMonthDays" runat="server" Font-Bold="False" Font-Names="Times New Roman"
                        Font-Size="Larger"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LblCode" runat="server" Text="Code"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LblDispCode" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LblWorkedDays" runat="server" Text="Worked Days" Font-Bold="False"
                        Font-Names="Times New Roman" Font-Size="Larger"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LblDispWorkedDays" runat="server" Font-Bold="False" Font-Names="Times New Roman"
                        Font-Size="Larger"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LblDesignation" runat="server" Text="Designation"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LblDispDesignation" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LblDepartment" runat="server" Text="Department"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LblDispDepartment" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LblNetPay" runat="server" Text="Net Pay" Font-Bold="True" Font-Names="Times New Roman"
                        Font-Size="Larger"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LbldispNetPay" runat="server" Font-Bold="True" Font-Names="Times New Roman"
                        Font-Size="Larger"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <table class="style1" border="1">
            <tr>
                <td>
                    <asp:Label ID="LblbasicPay0" runat="server" Text="Pay &amp; Allowence" Font-Bold="True"
                        Font-Names="Times New Roman" Font-Size="Larger"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LblbasicPay1" runat="server" Text="Amount" Font-Bold="True" Font-Names="Times New Roman"
                        Font-Size="Larger"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LblbasicPay2" runat="server" Text="Deduction" Font-Bold="True" Font-Names="Times New Roman"
                        Font-Size="Larger"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LblbasicPay3" runat="server" Text="Amount" Font-Bold="True" Font-Names="Times New Roman"
                        Font-Size="Larger"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LblbasicPay" runat="server" Text="Basic Pay"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="LbldispbasicPay" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Lbllwop" runat="server" Text="Leave W/o Pay"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="Lbldisplwop" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LblHR" runat="server" Text="House Rent"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="LbldispHR" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LblMob_exces_limit" runat="server" Text="Mobile Excess Limit"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="LbldispMob_exces_limit" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LblUtilities" runat="server" Text="Utilities"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="LbldispUtilities" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LblMOb_Device" runat="server" Text="Mobile Device"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="LbldispMOb_Device" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblHardShipAll" runat="server" Text="Hard Ship Allowance"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="lbldispHardShipAll" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LblEOBI" runat="server" Text="E.O.B.I."></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="LbldispEOBI" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSpecialAll" runat="server" Text="Special Allowance"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="lbldispSpecialAll" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LblOther" runat="server" Text="Other"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="LbldispOther" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Lblmed_bill" runat="server" Text="Medical Reimbursement"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="Lbldispmed_bill" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LblIncome_Tax" runat="server" Text="Income Tax"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="LbldispIncome_Tax" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Lblsal_Arr" runat="server" Text="Salary Arrears"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="Lbldispsal_Arr" runat="server"></asp:Label>
                </td>
                <td>
                </td>
                <td align="right">
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LblEDA" runat="server" Text="EDA"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="LbldispEDA" runat="server"></asp:Label>
                </td>
                <td>
                </td>
                <td align="right">
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LblPSM" runat="server" Text="PSM"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="LbldispPSM" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LblMOB_Alownce" runat="server" Text="Mobile Allowance"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="LbldispMOB_Alownce" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LblFeul_Alownce" runat="server" Text="Fuel Allowance"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="LbldispFeul_Alownce" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Lblexp_claim" runat="server" Text="Expence Claim"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="Lbldispexp_claim" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Lblsales_Incentives" runat="server" Text="Sales Incentives"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="Lbldispsales_Incentives" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LblOthers" runat="server" Text="Others"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="LbldispOthers" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="LblsimpTotal" runat="server" Text="Total"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="LbldispsimpTotal" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LbldedTotal" runat="server" Text="Total"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="LbldispdedTotal" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <%--</td> </tr>
    <tr>
                    <td align="center" valign="top" colspan="6">
                    <br />
                    <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
                    <!--img src="images/btn_new.jpg" width="60" height="20" /> <img src="images/btn_edit.jpg" width="60" height="20" /> <img src="images/btn_delete.jpg" alt="" width="60" height="20" /> <img src="images/btn_save.jpg" width="60" height="20" /> <img src="images/btn_cancel.jpg" width="60" height="20" /-->
                    </td>                    
                  </tr>--%>
    <%--<tr>
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
                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                   OnClick="btnSearch_Click"  ToolTip="Search Emps"/>
                </td>
             </tr>
            </table>
             
             </div>--%>
    <%--<asp:GridView ID="grdlev" runat="server" DataKeyNames="EmpID,CompID,LeaveDate" OnSelectedIndexChanged="grdEmps_SelectedIndexChanged"
                    AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdEmps_PageIndexChanging" OnRowDataBound="grdEmps_RowDataBound"
                    EmptyDataText="There is no Leave" Width="800px">
                    <Columns>
                        <asp:BoundField DataField="CompID" HeaderText="CompID" />
                        <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                        <asp:BoundField DataField="LeaveDate" HeaderText="Date" />
                        <asp:BoundField DataField="LeaveDays" HeaderText="Duration(Days)" />
                        <asp:BoundField DataField="LeaveTypeID" HeaderText="Leave Type" />
                                                
                        <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True" >
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                    </Columns>
                    <HeaderStyle CssClass="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                </asp:GridView>
             
        </td>
        <td width="3%">
        </td>
    </tr>
    </table>--%>
    <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" />
</asp:Content>
