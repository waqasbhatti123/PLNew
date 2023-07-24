<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Practice.aspx.cs" Inherits="RMS.profile.Practice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
</head>
<body>
    <form id="form1" runat="server">
        <div>
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
                                <label>Employee Type*</label>
                                <asp:DropDownList ID="ddlEmpType" runat="server" CssClass="form-control employeeType" AppendDataBoundItems="True">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                    <asp:ListItem Value="Permanent">Permanent</asp:ListItem>
                                    <asp:ListItem Value="Contract">Contract</asp:ListItem>
                                    <asp:ListItem Value="DailyWager">Daily Wager</asp:ListItem>
                                    <asp:ListItem Value="Consultant">Consultant</asp:ListItem>
                                </asp:DropDownList>
                               <%-- <asp:RequiredFieldValidator ID="RequiredFieldEmployee" runat="server" ControlToValidate="ddlEmployee"
                                    ErrorMessage="Please select Employee" SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator>--%>
                            </div>


                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <label>Employee*</label>
                                <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                                    <asp:ListItem Value="0">Select Employee</asp:ListItem>
                                </asp:DropDownList>
                               <%-- <asp:RequiredFieldValidator ID="RequiredFieldEmployee" runat="server" ControlToValidate="ddlEmployee"
                                    ErrorMessage="Please select Employee" SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator>--%>
                            </div>

                                           <div class="col-lg-3 col-md-3 col-sm-3">
                                  <label>Effective Date*</label> <span class="DteLtrl"><asp:Literal ID="Literal1" runat="server" Text="<%$ AppSettings: DateFormatPageText %>" /></span>
                                  <asp:TextBox ID="txtEffDate"  runat="server" MaxLength="11" CssClass="RequiredField form-control"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="txtEffDateCal" runat="server" Enabled="True" TargetControlID="txtEffDate">
                                </ajaxToolkit:CalendarExtender>
                               <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEffDate"
                                    ErrorMessage="Please select effective date" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                </div>

                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <label id="basicpayLable">Basic Pay*</label>
                                <asp:TextBox ID="txtBasicPay" Style="text-align: left" runat="server" CssClass="txtBasicPay form-control" MaxLength="6"></asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBasicPay"
                                    ErrorMessage="Please Enter Basic Pay" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                            </div>

                        </div>

                        <br />
                        <div class="row gridallred">
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <h3>Allowances</h3>
                                <hr />
                                <div class="row" id="allowanceClass">
                                    <div class="col-md-8">
                                        <asp:Panel ID="EmployAllowanceLableGridDetail" Width="100%" runat="server">
                                        </asp:Panel>
                                        <%--  <input type="text" value="Total Allowances" class="form-control" />--%>
                                    </div>
                                    <div class="col-md-4">
                                        <div id="allowanceinputdiv">
                                            <asp:Panel ID="EmployAllowanceGridDetail" Width="100%" runat="server">
                                            </asp:Panel>
                                        </div>
                                        <%--    <asp:TextBox ID="totalAllowances" runat="server" CssClass="form-control totalAllowances"></asp:TextBox>--%>
                                    </div>
                                </div>
                            </div>


                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <h3>Deductions</h3>
                                <hr />
                                <div class="row" id="deductionClass">

                                    <div class="col-md-8">
                                        <asp:Panel ID="EmployDeductionLableGridDetail" Width="100%" runat="server">
                                        </asp:Panel>
                                        <%--    <input type="text" value="Total Deductions" class="form-control" />--%>
                                    </div>
                                    <div class="col-md-4">
                                        <div id="deductioninputdiv">
                                            <asp:Panel ID="EmployDeductionGridDetail" Width="100%" runat="server">
                                            </asp:Panel>
                                        </div>
                                        <%--   <asp:TextBox ID="totalDeductions" runat="server" CssClass="form-control totalDeductions"></asp:TextBox>--%>
                                    </div>

                                </div>
                            </div>
                        </div>


                        <%--        <div class="row" id="NetPayClass">
            <div class="col-md-4">
                <input type="text" value="Net Pay" class="form-control" />
            </div>
            <div class="col-md-4">
                <div id="netPayinputdiv">
                </div>
                <asp:TextBox ID="NetPay" runat="server" CssClass="form-control NetPay"></asp:TextBox>
            </div>

        </div>--%>

                        <br />
                        <div class="row btnDiv">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Button ID="Save" runat="server" OnClick="Button_Command" Text="Save" class="btn btn-primary"/>
                                <asp:Button ID="Clear" runat="server" OnClick="Clear_All" Text="Clear" class="btn btn-danger"/>

                            </div>
                        </div>

                        <br />

                              <asp:GridView ID="grdSalaryPackage" runat="server" CssClass="table table-responsive-sm table-bordered"  DataKeyNames="EmpID,CompID,EffDate" OnSelectedIndexChanged="grdSalaryPackage_PageIndexChanged"
                              AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdSalaryPackage_PageIndexChanging" OnRowDataBound="grdSalaryPackage_RowBound"
                              EmptyDataText="There is no package defined" Width="100%">
                              <Columns>
                                  <%--<asp:TemplateField>
                                      <ItemTemplate>
                                          <asp:Label runat="server" ></asp:Label>
                                      </ItemTemplate>
                                      
                                  </asp:TemplateField>--%>
                                  <asp:BoundField DataField="FullName" HeaderText="Name" />
                                  <asp:BoundField DataField="EffDate" HeaderText="Eff. Date" />
                                  <asp:BoundField DataField="Basic" HeaderText="Basic Pay" />
                                  <asp:BoundField DataField="IsActive" HeaderText="Is Active" />
<%--                                  <asp:BoundField DataField="OtrAlow" HeaderText="Total Allowances" />
                                  <asp:BoundField DataField="OtherDed" HeaderText="Total Deductions" />
                                  <asp:BoundField DataField="NSHA" HeaderText="Net Pay" />--%>

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
    </form>
</body>
</html>
