<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmpMgtSalary.aspx.cs" Inherits="RMS.profile.EmpMgtSalary" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" src="../js/empsaltotal.js"></script>
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
                    <asp:Panel runat="server" ID="pnlMain">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 center">
                            <uc3:EmpSearchUC ID="EmpSrchUC" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-6">
                            &nbsp; &nbsp; &nbsp; &nbsp;
                        </div>
                    </div>
                    
                        <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                  <label>Effective Date:</label> <span class="DteLtrl"><asp:Literal ID="Literal1" runat="server" Text="<%$ AppSettings: DateFormatPageText %>" /></span>
                                  <asp:TextBox ID="txtEffDate"  runat="server" MaxLength="11" CssClass="RequiredField form-control"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="txtEffDateCal" runat="server" Enabled="True" TargetControlID="txtEffDate">
                                </ajaxToolkit:CalendarExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEffDate"
                                    ErrorMessage="Please select effective date" SetFocusOnError="true" ValidationGroup="main"
                                    Display="None"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label>Basic Pay:</label>
                                    <asp:TextBox ID="txtBasicPay" Style="text-align: left" runat="server"  CssClass="txtBasicPay form-control" MaxLength="6"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBasicPay"
                                    ErrorMessage="Please enter basic pay" SetFocusOnError="true" ValidationGroup="main"
                                    Display="None"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label>House Rent:</label>
                                  <asp:TextBox ID="txtHouseRent" runat="server" Style="text-align: left" CssClass="txtHouseRent form-control" MaxLength="6"></asp:TextBox>

                                </div>
                            </div>
                            &nbsp;
                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label>Utililties:</label>
                                     <asp:TextBox ID="txtUtilities" runat="server" Style="text-align: left" CssClass="txtUtilities form-control" MaxLength="5"></asp:TextBox>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label>Misc. Deduction:</label>
                                    <asp:TextBox ID="txtMessDed" runat="server" Style="text-align: left" CssClass="txtMessDed form-control" MaxLength="5"></asp:TextBox> 
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label>Medical Allownce:</label>
                                    <asp:TextBox ID="txtAllounce" runat="server" Style="text-align: left" CssClass="txtAllounce form-control" MaxLength="6"></asp:TextBox>    

                                </div>
                            </div>
                           &nbsp;
                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label>Text Deduction:</label>
                                     <asp:TextBox ID="txtTaxDed" runat="server" Style="text-align: left" CssClass="txtTaxDed form-control" MaxLength="6"></asp:TextBox>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label>Special Allownce:</label>
                                     <asp:TextBox ID="txtSplAll" runat="server"  Style="text-align: left" CssClass="txtSplAll form-control" MaxLength="5"></asp:TextBox> 
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label>Other Deduction:</label>
                                     <asp:TextBox ID="txtOtherDed" runat="server" Style="text-align: left" CssClass="txtOtherDed form-control" MaxLength="6"></asp:TextBox>    

                                </div>
                            </div>
                            &nbsp;
                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label>Conveyance Allownce:</label>
                                     <asp:TextBox ID="txtFuelAll" Text="0" runat="server"  Style="text-align: right" CssClass="txtFuelAll form-control" MaxLength="5"></asp:TextBox>
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <label>Net Pay:</label>
                                     <asp:TextBox ID="txtTotalPay" runat="server" ReadOnly="true" Style="text-align: left" CssClass="RequiredFieldFinal form-control"></asp:TextBox> 
                                </div>
                            </div>
                        </asp:Panel>
                            &nbsp;
                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <asp:Button id="btnDelete" runat="server"  Width="60" BorderStyle="Solid" 
                    BackColor="#3399FF" Visible="false" onclick="btnDelete_Click" Text="Delete" />
                               <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
                               <asp:ImageButton ID="ImageButton1" runat ="server"  OnClick="btnDelete_Click" ImageUrl="~/images/btn_delete.png" onMouseOver="this.src='../images/btn_delete_m.png'" onMouseOut="this.src='../images/btn_delete.png'" Visible="false" />

                                </div>
                            </div>
                            &nbsp;
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <asp:GridView ID="grdEmps" runat="server" CssClass="table table-responsive-sm" DataKeyNames="EmpID,CompID,EffDate" OnSelectedIndexChanged="grdEmps_SelectedIndexChanged"
                    AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdEmps_PageIndexChanging"
                    OnRowDataBound="grdEmps_RowDataBound" EmptyDataText="There is no salary defined"
                    Width="720px">
                    <Columns>
                        <%--<asp:BoundField DataField="emp_Id" HeaderText="Emp ID" ItemStyle-Width="80px" />
                        <asp:BoundField DataField="EmpCode" HeaderText="Emp Ref" ItemStyle-Width="80px" />--%>
                        <asp:BoundField DataField="FullName" HeaderText="Full Name" ItemStyle-Width="240px" />
                        <asp:BoundField DataField="EffDate" HeaderText="Eff. Date" ItemStyle-Width="95px" />
                        <asp:BoundField DataField="Basic" HeaderText="Basic Pay" ItemStyle-Width="80px">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HR" HeaderText="House Rent" ItemStyle-Width="80px">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Utilities" HeaderText="Utilities" ItemStyle-Width="80px">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NSHA" HeaderText="Med. Allow." ItemStyle-Width="80px">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SplAlow" HeaderText="Spl. Allow." ItemStyle-Width="80px">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CA" HeaderText="Con. Allow." ItemStyle-Width="80px">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MessDed" HeaderText="Misc. Ded." ItemStyle-Width="80px">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TaxDed" HeaderText="Tax Ded." ItemStyle-Width="80px">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="OtherDed" HeaderText="Other Ded." ItemStyle-Width="80px">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        
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
</asp:Content>

   <%-- <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
    <div class="col-lg-12 col-md-6 col-sm-6 col-xs-4">
    <table class="dd">
        <tr>
            <td>
            </td>
            <td align="center">
                
            </td>
        </tr>
    </table>
    <table width="100%" style="background-color:white;">
        <tr>
            <td width="3%">
            </td>
            <td height="10" colspan="2"></td>
            <td>
                <asp:Panel runat="server" ID="pnlMain">
                    <table width="100%" class="stats1">
                        <tr>
                            <td colspan="4" valign="top">
                                <uc3:EmpSearchUC ID="EmpSrchUC" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" valign="top">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <div align="left">
                                    <asp:Label ID="Label13" runat="server" Text="Effective Date:"></asp:Label>
                                </div>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEffDate" runat="server" MaxLength="11" Width="80px" CssClass="RequiredField"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="txtEffDateCal" runat="server" Enabled="True" TargetControlID="txtEffDate">
                                </ajaxToolkit:CalendarExtender>
                                <span class="DteLtrl"><asp:Literal ID="Literal1" runat="server" Text="<%$ AppSettings: DateFormatPageText %>" /></span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEffDate"
                                    ErrorMessage="Please select effective date" SetFocusOnError="true" ValidationGroup="main"
                                    Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td>
                               <asp:Label ID="lblbasic" runat="server" Text="Basic Pay:"></asp:Label>
                            </td>
                            <td >
                                <asp:TextBox ID="txtBasicPay" Style="text-align: right" runat="server"  CssClass="txtBasicPay" MaxLength="6"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBasicPay"
                                    ErrorMessage="Please enter basic pay" SetFocusOnError="true" ValidationGroup="main"
                                    Display="None"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                         <td>
                               <asp:Label ID="Label1" runat="server" Text="House Rent:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtHouseRent" runat="server" Style="text-align: right" CssClass="txtHouseRent" MaxLength="6"></asp:TextBox>
                            </td>
                          
                        </tr>
                        <tr>
                         <td>
                              <asp:Label ID="Label2" runat="server" Text="Utilities:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUtilities" runat="server" Style="text-align: right" CssClass="txtUtilities" MaxLength="5"></asp:TextBox>
                            </td>
                          <td>
                            <asp:Label ID="Label8" runat="server" Text="Misc. Deduction:"></asp:Label>
                          </td>
                          <td>
                                <asp:TextBox ID="txtMessDed" runat="server" Style="text-align: right" CssClass="txtMessDed" MaxLength="5"></asp:TextBox>
                          </td>
                        </tr>
                        <tr>
                          <td>
                             <asp:Label ID="Label14" runat="server" Text="Medical Allowance:"></asp:Label>
                          </td>
                          <td>
                                <asp:TextBox ID="txtAllounce" runat="server" Style="text-align: right" CssClass="txtAllounce" MaxLength="6"></asp:TextBox>
                                
                          </td>
                           <td>
                                <asp:Label ID="Label5" runat="server" Text="Tax Deduction:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTaxDed" runat="server" Style="text-align: right" CssClass="txtTaxDed" MaxLength="6"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                         <td>
                             <asp:Label ID="Label4" runat="server" Text="Special Allowance:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSplAll" runat="server"  Style="text-align: right" CssClass="txtSplAll" MaxLength="5"></asp:TextBox>
                            </td>
                             <td>
                                <asp:Label ID="Label7" runat="server"  Text="Other Deduction:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOtherDed" runat="server" Style="text-align: right" CssClass="txtOtherDed" MaxLength="6"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Conveyance Allowance:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFuelAll" Text="0" runat="server"  Style="text-align: right" CssClass="txtFuelAll" MaxLength="5"></asp:TextBox>
                            </td>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                         <td align="right">
                          <asp:Label ID="Label6" runat="server"  Text="<b>Net Pay:</b>&nbsp;&nbsp;"></asp:Label>
                            
                         </td>  
                        <td>
                            <asp:TextBox ID="txtTotalPay" runat="server" ReadOnly="true" Style="text-align: right" CssClass="RequiredFieldFinal"></asp:TextBox>
                            
                        </td>
                        </tr>
                        
                    </table>
                </asp:Panel>
            </td>
            <td width="3%">
            </td>
        </tr>
        <tr>
            <td align="center" valign="top" colspan="6">
                <br />
                <%--<asp:Button id="btnDelete" runat="server"  Width="60" BorderStyle="Solid" 
                    BackColor="#3399FF" Visible="false" onclick="btnDelete_Click" Text="Delete" />--%>
            
            

