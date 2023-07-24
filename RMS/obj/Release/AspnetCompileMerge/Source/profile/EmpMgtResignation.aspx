<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmpMgtResignation.aspx.cs" Inherits="RMS.Setup.EmpMgtResignation"
    Culture="auto" UICulture="auto" EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                            <div class="col-lg-12 col-md-12 col-sm-12">
                                <uc3:EmpSearchUC ID="EmpSrchUC" runat="server" />
                            </div>
                        </div>
                        &nbsp;
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Leaving Date:</label><span class="DteLtrl"><asp:Literal ID="Literal2" runat="server" Text="<%$ AppSettings: DateFormatPageText %>" /></span>
                                <ajaxToolkit:CalendarExtender ID="txtLeavingdatecal" runat="server" Enabled="True"
                                    TargetControlID="txtLeavingdate">
                                </ajaxToolkit:CalendarExtender>
                                <asp:TextBox ID="txtLeavingdate"  runat="server" MaxLength="11" CssClass="RequiredField form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please select leaving date"
                                    ControlToValidate="txtLeavingdate" SetFocusOnError="true" ValidationGroup="main"
                                    Display="None"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="margin-top:8px;">
                                <asp:Label ID="lblbasic" runat="server" Text="Reason Type:"></asp:Label>
                                <asp:DropDownList ID="ddlLeaveReason"  runat="server" AppendDataBoundItems="true" CssClass="RequiredField form-control">
                                    <asp:ListItem Value="0">Select Reason</asp:ListItem>
                                </asp:DropDownList>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlLeaveReason"
                                ErrorMessage="Please select reason type" SetFocusOnError="true" ValidationGroup="main"
                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Reason of Leave:</label>
                                <asp:TextBox ID="txtReason" runat="server" CssClass="RequiredField form-control" MaxLength="50" Width="250"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter reason of leaving"
                                    ControlToValidate="txtReason" SetFocusOnError="true" ValidationGroup="main"
                                    Display="None"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        &nbsp;
                        <div class="row">
                          <div class="col-lg-4 col-md-4 col-sm-4">
                              <asp:CheckBox ID="CheckIsRes" Checked="true" runat="server" />&nbsp;<label>Is Resigned</label>
                          </div>
                       </div>
                        <%--<div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                             <asp:Label ID="lblStatus" runat="server" Text="Status:"></asp:Label>
                             <asp:DropDownList ID="ddlStatus" runat="server" CssClass="RequiredField form-control">
                             <asp:ListItem Value="0">Select Status</asp:ListItem>
                             <asp:ListItem Value="P">Pending</asp:ListItem>
                             <asp:ListItem Value="A">Approved</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqVal_ddlStatus" runat="server" ErrorMessage="Please select status"
                                ControlToValidate="ddlStatus" SetFocusOnError="true" ValidationGroup="main" InitialValue="0"
                                Display="None"></asp:RequiredFieldValidator>
                            </div>
                        </div>--%>
                        &nbsp;
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" ValidationGroupName="main" />
                                <!--img src="images/btn_new.jpg" width="60" height="20" /> <img src="images/btn_edit.jpg" width="60" height="20" /> <img src="images/btn_delete.jpg" alt="" width="60" height="20" /> <img src="images/btn_save.jpg" width="60" height="20" /> <img src="images/btn_cancel.jpg" width="60" height="20" /-->
                            </div>
                        </div>
                        &nbsp;
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Label ID="lblFltName" runat="server" Text="Emp Name:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltEmp" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Label ID="Label22" runat="server" Text="Emp No:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltEmpCode" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="margin-top:30px;">
                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                OnClick="btnSearch_Click" ToolTip="Search Emps" />
                            </div>
                        </div>
                        &nbsp;
                        <div class="row">
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:GridView ID="grdResign" runat="server" CssClass="table table-responsive-sm" OnRowDataBound="grdEmps_RowDataBound" PageSize="20"
                    AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No Resignations are available"
                    OnSelectedIndexChanged="grdResign_SelectedIndexChanged" DataKeyNames="EmpID"
                    Width="98%"  OnPageIndexChanging="grdResign_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="emp_Id" HeaderText="Emp ID" />
                        <asp:BoundField DataField="Code" HeaderText="Emp Ref" />
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:BoundField DataField="Date" HeaderText="Leaving Date" />
                        <asp:BoundField DataField="type" HeaderText="Reason Type" />
                        <asp:BoundField DataField="reason" HeaderText="Reason of Leaving" />
                        <asp:BoundField DataField="IsResigned" HeaderText="Resigned" />
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
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
