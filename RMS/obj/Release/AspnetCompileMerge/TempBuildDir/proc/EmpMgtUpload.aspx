<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmpMgtUpload.aspx.cs" Inherits="RMS.Setup.EmpMgtUpload" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Select Excel Sheet"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:RadioButtonList ID="rblSheetNames" runat="server" RepeatDirection="Vertical">
                            <%--<asp:ListItem Value="7">Company Parameters</asp:ListItem>--%>
                            <asp:ListItem Value="10">Arrear Payment</asp:ListItem>
                           <%-- <asp:ListItem Value="7">Medical Sheet</asp:ListItem>
                            <asp:ListItem Value="8">Advance Sheet</asp:ListItem>
                            <asp:ListItem Value="2" Selected="True">Attendance Sheet</asp:ListItem>
                            <asp:ListItem Value="6">Employees Sheet</asp:ListItem>--%>
                            <%--<asp:ListItem Value="3">PSM Incentive Sheet</asp:ListItem>
                            <asp:ListItem Value="5">Expense Claim Sheet</asp:ListItem>
                            <asp:ListItem Value="4">Mobile Bill Deduction Sheet</asp:ListItem>
                            <asp:ListItem Value="1">Extra Duty Allowance Sheet</asp:ListItem>--%>
                          </asp:RadioButtonList>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                         <div class="col-lg-4 col-md-4 col-sm-4">
                             <asp:FileUpload runat="server" ID="fileUploadImg" />
                          </div>
                          <div class="col-lg-4 col-md-4 col-sm-4">
                              <asp:ImageButton ID="btnUploadEmpData" runat="server"  ImageUrl="~/images/btn_upload.png" OnCommand="ButtonCommand"
                       onMouseOver="this.src='../images/btn_upload_m.png'" 
                       onMouseOut="this.src='../images/btn_upload.png'" CssClass="buttonCancel" CommandName="EmpData" />
                       
                       <asp:ImageButton ID="btnCancel" runat="server"  ImageUrl="~/images/btn_clear.png" OnCommand="ButtonCommand"
                        onMouseOver="this.src='../images/btn_clear_m.png'" onMouseOut="this.src='../images/btn_clear.png'" 
                        CssClass="buttonCancel" CommandName="Cancel" />
                          </div>
                    </div>
                </div>
            </div>
        </div>
    </div>  
    
   <%-- <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="3%"></td>
            <td>
                 <asp:Panel runat="server" ID="pnlMain">
                <table cellspacing="0" class="stats2" align="center">
                  <tr>
                    <td colspan="6" valign="top" class="bg_input_area"></td>
                  </tr>
                  <tr>
                    <td colspan="2">
                        <div align="left"><uc3:EmpSearchUC ID="EmpSrchUC" runat="server" /></div>
                    
                    </td>
                    <td>
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
                </asp:Panel>  
               </td>
               </tr>
                 <tr>
                    <td align="center" valign="top" colspan="6">
                    <br />
                    <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
                    <!--img src="images/btn_new.jpg" width="60" height="20" /> <img src="images/btn_edit.jpg" width="60" height="20" /> <img src="images/btn_delete.jpg" alt="" width="60" height="20" /> <img src="images/btn_save.jpg" width="60" height="20" /> <img src="images/btn_cancel.jpg" width="60" height="20" /-->
                    </td>                    
                  </tr>
          <tr>
            <td width="3%"></td>
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
             
             </div>
             <asp:GridView ID="grdlev" runat="server" DataKeyNames="EmpID,CompID,LeaveDate" OnSelectedIndexChanged="grdEmps_SelectedIndexChanged"
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
            <td width="3%"></td>
          </tr>
        </table>--%>
        
</asp:Content>
