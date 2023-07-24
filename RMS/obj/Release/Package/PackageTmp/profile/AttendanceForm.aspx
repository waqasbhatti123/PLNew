<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="AttendanceForm.aspx.cs"  Inherits="RMS.profile.AttendanceForm"
   Culture="auto" UICulture="auto" EnableEventValidation="true" %>


<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>
    <script>
        <%--$(function () {
        var brID = '<%=Session["BranchID"].ToString() %>';
        $(".searchbranchchange").val(brID);
        })--%>
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4">
                <div class="card-body">
                    <div class="col-lg-12 col-md-12 col-sm-12">
                        <asp:ValidationSummary ID="ValidationSummary1" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                          ValidationGroup="main"/>
                        <uc1:Messages ID="ucMessage" runat="server" />
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                             <label>Posting Station*</label>  
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged" AppendDataBoundItems="True" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Division</asp:ListItem>
                            </asp:DropDownList>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Job Type*</label>
                                   <asp:DropDownList ID="ddlJobType" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlJobtype_SelectedIndexChanged" AppendDataBoundItems="True" AutoPostBack="true">
                                       <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                       <%--<asp:ListItem Value="Officer">Officer</asp:ListItem>
                                    <asp:ListItem Value="Permanent">Permanent</asp:ListItem>
                                    <asp:ListItem Value="Contract">Contract</asp:ListItem>
                                    <asp:ListItem Value="DailyWager">Daily Wager</asp:ListItem>
                                    <asp:ListItem Value="Consultant">Consultant</asp:ListItem>--%>
                                            </asp:DropDownList>

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlJobType"
                                                ErrorMessage="Please select job type" SetFocusOnError="true" ValidationGroup="main"
                                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Employees*</label>
                                <asp:UpdatePanel ID="Disupnl" runat="server">
                                    <ContentTemplate>
                                 <asp:DropDownList ID="ddlEmployeeSearch" runat="server" CssClass="form-control districselect" AppendDataBoundItems="false">
                                     <asp:ListItem Value="0">Select Employee</asp:ListItem>
                                </asp:DropDownList>
                                    </ContentTemplate>
                                   <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlJobType"  />
                                   </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Select Month*</label>
                            <asp:TextBox ID="txtMonth" TextMode="Month"  runat="server" MaxLength="15" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Total Days*</label>
                            <asp:TextBox ID="txtPrestDays" TextMode="Number"  runat="server" MaxLength="15" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Leaves Days*</label>
                            <asp:TextBox ID="txtLeaveDays" TextMode="Number"  runat="server" MaxLength="15" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button runat="server" ID="Button19" OnClick="Onclick_Save" class="btn btn-primary" Text="Save" ValidationGroup=""></asp:Button>
                            <asp:Button runat="server" ID="Button244" OnClick="Onclick_Clear" CssClass="btn btn-danger" Text="Clear" />
                            <%--<button runat="server" class="btn btn-primary" onclick="Onclick_Save">Save</button>
                            <button runat="server" class="btn btn-danger" onclick="Onclick_Clear">Clear</button>--%>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                        <asp:GridView ID="grdAttendance" runat="server" CssClass="table table-responsive-sm" DataKeyNames="AttID,EmpID" OnSelectedIndexChanged="grdEduEmps_SelectedIndexChanged"
                            AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdEduEmps_PageIndexChanging"
                            EmptyDataText="There is no employee defined" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="br_nme" HeaderText="Branch Name" />
                                <asp:BoundField DataField="FullName" HeaderText="Employee Name" />
                                <asp:BoundField DataField="month" HeaderText="Month" />
                                <asp:BoundField DataField="PresentDays" HeaderText="Present Days" />
                                <asp:BoundField DataField="LeaveDays" HeaderText="Leaves Days" />
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
