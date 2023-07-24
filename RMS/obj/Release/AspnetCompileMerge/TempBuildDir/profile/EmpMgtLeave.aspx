<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmpMgtLeave.aspx.cs" Inherits="RMS.Setup.EmpMgtLeave" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>
    <script src="../Scripts/jquery-ui-1.8.16.js"></script>
    <link href="../Scripts/jquery-ui.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.jquery.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.min.css" rel="stylesheet" />
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
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4 ">
                            <label>Employee*</label>
                            <asp:DropDownList ID="ddlEmployee" CssClass="form-control empl" runat="server" AppendDataBoundItems="False" OnSelectedIndexChanged="searchemps_changeIndex" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Employee</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Employee is required"
                                ControlToValidate="ddlEmployee" InitialValue="0" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>

                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Personal File No*</label>
                            <asp:DropDownList ID="ddlperson" runat="server" CssClass="form-control ddlEmpDrpdwnrt" OnSelectedIndexChanged="ddlPersonal_change" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Personal File Number</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2" style="margin-top: 30px">
                            <asp:Button Text="Search" CssClass="btn btn-info" runat="server" OnClick="Search_Grid" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <%-- <asp:GridView ID="grdLeaveStatus" CssClass="table table-responsive-sm" runat="server" DataKeyNames="EmpID"
                         OnRowDataBound="grdLeaveStatus_OnRowDataBound"
                         AutoGenerateColumns="False" Width="90%">
                         <Columns >
                         <asp:BoundField DataField="Avl_Casual" HeaderText="Avl_Casual" ItemStyle-HorizontalAlign="Right"/>
                         <asp:BoundField DataField="Avl_Medical" HeaderText="Avl_Medical" ItemStyle-HorizontalAlign="Right"/>
                         <asp:BoundField DataField="Avl_Annual" HeaderText="Avl_Annual" ItemStyle-HorizontalAlign="Right"/>

                         <asp:BoundField DataField="Avld_Casual" HeaderText="Avld_Casual" ItemStyle-HorizontalAlign="Right"/>
                         <asp:BoundField DataField="Avld_Medical" HeaderText="Avld_Medical" ItemStyle-HorizontalAlign="Right"/>
                         <asp:BoundField DataField="Avld_Annual" HeaderText="Avld_Annual" ItemStyle-HorizontalAlign="Right"/>

                         <asp:BoundField DataField="Bal_Casual" HeaderText="Bal_Casual" ItemStyle-HorizontalAlign="Right"/>
                         <asp:BoundField DataField="Bal_Medical" HeaderText="Bal_Medical" ItemStyle-HorizontalAlign="Right"/>
                         <asp:BoundField DataField="Bal_Annual" HeaderText="Bal_Annual" ItemStyle-HorizontalAlign="Right"/>
                         </Columns >
                         <HeaderStyle CssClass="grid_hdr" />
                         <RowStyle CssClass="grid_row" />
                         <%--<AlternatingRowStyle CssClass="gridAlternateRow" />
                         <SelectedRowStyle CssClass="gridSelectedRow" / > 
                         </asp:GridView >--%>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label id="lblbasic">Leave Type:</label>
                            <asp:DropDownList ID="ddlleaveType" runat="server" AppendDataBoundItems="true" CssClass="RequiredField form-control">
                                <asp:ListItem Value="0"> Select Leave Type</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlleaveType"
                                ErrorMessage="Please select leave type" SetFocusOnError="true" ValidationGroup="main"
                                Display="None" InitialValue="0"> </asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>From Date:</label><span class="DteLtrl">
                                <asp:Literal ID="Literal1" runat="server" Text="<%$ AppSettings: DateFormatPageText %>" /></span>
                            <ajaxToolkit:CalendarExtender ID="txtStartDateCal" runat="server" TargetControlID="txtStartDate"
                                Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                            <asp:TextBox ID="txtStartDate" runat="server" MaxLength="11" CssClass="RequiredField form-control"> </asp:TextBox>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please select start date"
                                ControlToValidate="txtStartDate" SetFocusOnError="true" ValidationGroup="main"
                                Display="None"> </asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>To Date:</label><span class="DteLtrl">
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ AppSettings: DateFormatPageText %>" /></span>
                            <ajaxToolkit:CalendarExtender ID="txtEndDateCal" runat="server" TargetControlID="txtEndDate"
                                Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                            <asp:TextBox ID="txtEndDate" runat="server" MaxLength="11" CssClass="RequiredField form-control"> </asp:TextBox>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please select end date"
                                ControlToValidate="txtEndDate" SetFocusOnError="true" ValidationGroup="main"
                                Display="None"> </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Remark:</label>s
                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine" onkeyup="LimitText(this,50);" onblur="LimitText(this,50);" Height="35px"> </asp:TextBox>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <%--<asp:CheckBox ID="checkIsactive" class="checkbox" runat="server" />&nbsp; <label id="per">Is Active</label>--%>
                            <asp:Label ID="lblStatus" Text="Status" runat="server"></asp:Label>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="RequiredField form-control">
                                <asp:ListItem Value="0" Text="Select Status"> </asp:ListItem>
                                <asp:ListItem Selected="True" Value="P" Text="Pending"> </asp:ListItem>
                                <asp:ListItem Value="A" Text="Approved"> </asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqVal_ddlStatus" runat="server" ErrorMessage="Please select status"
                                ControlToValidate="ddlStatus" SetFocusOnError="true" ValidationGroup="main" InitialValue="0"
                                Display="None"> </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:ImageButton ID="btnDelete" runat="server" Width="60" BorderStyle="Solid"
                                BackColor="#3399FF" Visible="false" OnClick="btnDelete_Click" Text="Delete" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">

                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button ID="Button1" OnClick="btnSave_Save" runat="server" CssClass="btn btn-primary" ValidationGroup="one" Text="Save" />
                            <%--<uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />--%>
                            <asp:ImageButton ID="ImageButton1" runat="server" OnClick="btnDelete_Click" ImageUrl="~/images/btn_delete.png" onMouseOver="this.src='../images/btn_delete_m.png'" onMouseOut="this.src='../images/btn_delete.png'" Visible="false" />
                            <!-- img src="images/btn_new.jpg" width="60" height="20" / > <img src="images/btn_edit.jpg" width="60" height="20" / > <img src="images/btn_delete.jpg" alt="" width="60" height="20" / > <img src="images/btn_save.jpg" width="60" height="20" / > <img src="images/btn_cancel.jpg" width="60" height="20" / -->
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:GridView ID="grdLeave" runat="server" CssClass="table table-responsive-sm" DataKeyNames="LeaveID" OnSelectedIndexChanged="grdLeave_SelectedIndexChanged"
                                AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdLeave_PageIndexChanging"
                                OnRowDataBound="grdLeave_RowDataBound" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="fName" HeaderText="Employee Name" />
                                    <asp:BoundField DataField="LeaveTypeID" HeaderText="Leave Type" />
                                    <asp:BoundField DataField="start" HeaderText=" From Date" />
                                    <asp:BoundField DataField="end" HeaderText=" To Date" />
                                    <asp:BoundField DataField="LeaveDays" HeaderText="Days" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
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

                    &nbsp;
                    <hr />
                    &nbsp;
                    <h1 class="bg-info" style="text-align: center; padding: 10px; color: white; font-size: 25px">Leave Balance</h1>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Leave Type:</label>
                            <asp:DropDownList ID="ddlLeaeveTYpe" runat="server" AppendDataBoundItems="true" CssClass="RequiredField form-control" OnSelectedIndexChanged="searchemp_changeIndex" AutoPostBack="true">
                                <asp:ListItem Value="0"> Select Leave Type</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlleaveType"
                                ErrorMessage="Please select leave type" SetFocusOnError="true" ValidationGroup="main"
                                Display="None" InitialValue="0"> </asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Entitled / Earned</label>
                            <asp:UpdatePanel ID="upnl" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtentirler" runat="server" MaxLength="11" CssClass="RequiredField form-control"> </asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlLeaeveTYpe" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label runat="server" visible="false">Availed</label>
                            <asp:TextBox ID="txtavailBlnc" runat="server" MaxLength="11" CssClass="RequiredField form-control" Visible="false"> </asp:TextBox>
                        </div>
                    </div>



                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 col-md-4">
                            <asp:Button runat="server" ID="Button5" OnClick="btnBalanceLeave_Save" class="btn btn-primary" Text="Save" ValidationGroup=""></asp:Button>

                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:GridView ID="grdBlnc" runat="server" CssClass="table table-responsive-sm" DataKeyNames="LeaID" OnSelectedIndexChanged="grdBlnc_SelectedIndexChanged"
                                AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdBlnc_PageIndexChanging"
                                OnRowDataBound="grdBlnc_RowDataBound" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="FullName" HeaderText="Employee Name" />
                                    <asp:BoundField DataField="LeaveTypeDesc" HeaderText="Leave Type" />
                                    <asp:BoundField DataField="LeaveBlnc" HeaderText="Balance" />
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

    <script>
        $(".empl").chosen();
        $('#<%=ddlperson.ClientID%>').chosen();
    </script>

</asp:Content>
