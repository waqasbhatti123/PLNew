<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmpMgtSalary_x.aspx.cs" Inherits="RMS.Setup.EmpMgtSalary_x" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main"/>
    <uc1:Messages ID="ucMessage" runat="server" />
    
    
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="3%"></td>
            <td>
                 <asp:Panel runat="server" ID="pnlMain">
                <table cellspacing="0" class="stats2" align="center">
                  <tr>
                    <td colspan="4" valign="top" class="bg_input_area"></td>
                  </tr>
                  <tr>
                    <td colspan="4">
                        <uc3:EmpSearchUC ID="EmpSrchUC" runat="server" />
                    
                    </td>
                </tr>
                    <tr>
                        <td>
                            <div align="left">
                                <asp:Label ID="Label13" runat="server" Text="Effective Date:"></asp:Label>
                            </div>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEffDate" runat="server" MaxLength="11" Width="80px"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtEffDateCal" runat="server" Enabled="True" 
                                TargetControlID="txtEffDate">
                            </ajaxToolkit:CalendarExtender>
                            <asp:Literal ID="Literal1" runat="server" 
                                Text="<%$ AppSettings: DateFormatPageText %>" />
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                 <tr>
                    <td>
                        <asp:Label ID="lblbasic" runat="server" Text="Basic Pay:" ></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtBasicPay" runat="server" CssClass="RequiredField"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label14" runat="server" Text="Allowance:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAllounce" runat="server" CssClass="RequiredField"></asp:TextBox>
                     </td>
                    
                </tr>
                 <tr>
                     <td>
                        <asp:Label ID="Label1" runat="server" Text="House Rent:" ></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtHouseRent" runat="server" CssClass="RequiredField"></asp:TextBox>
                    </td>                   
                    
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Special Allowance:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSplAll" runat="server" CssClass="RequiredField"></asp:TextBox>
                    </td>
                    
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Utilities:" ></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUtilities" runat="server" CssClass="RequiredField"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="Tax Deduction:" ></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTaxDed" runat="server" CssClass="RequiredField"></asp:TextBox>
                    </td>
                    
                </tr>
                <tr>
                    <td>
                        
                        <asp:Label ID="Label3" runat="server" Text="Fuel Allowance:"></asp:Label>
                        
                    </td>
                    <td>
                        
                        <asp:TextBox ID="txtFuelAll" runat="server" CssClass="RequiredField"></asp:TextBox>
                        
                    </td>
                    <td>
                        <asp:Label ID="Label7" runat="server" Text="Other Deduction:" ></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtOtherDed" runat="server" CssClass="RequiredField"></asp:TextBox>
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
             
             
             <asp:GridView ID="grdEmps" runat="server" DataKeyNames="EmpID,CompID,EffDate" OnSelectedIndexChanged="grdEmps_SelectedIndexChanged"
                    AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdEmps_PageIndexChanging" OnRowDataBound="grdEmps_RowDataBound"
                    EmptyDataText="There is no employee defined" Width="760px">
                    <Columns>
                        <asp:BoundField DataField="EmpCode" HeaderText="Code" />
                        <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                        <asp:BoundField DataField="EffDate" HeaderText="Eff. Date" />
                        <asp:BoundField DataField="Basic" HeaderText="Basic Pay" > <ItemStyle HorizontalAlign="Right" /> </asp:BoundField>
                        <asp:BoundField DataField="HR" HeaderText="House Rent" ><ItemStyle HorizontalAlign="Right" /> </asp:BoundField>
                        <asp:BoundField DataField="Utilities" HeaderText="Utilities" ><ItemStyle HorizontalAlign="Right" /> </asp:BoundField>
                        
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
        </table>
        
</asp:Content>
