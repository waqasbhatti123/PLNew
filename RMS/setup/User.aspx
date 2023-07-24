<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="User.aspx.cs" Inherits="RMS.Setup.User" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4 ">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:ValidationSummary ID="main" CssClass="text-warning" runat="server" DisplayMode="List" ValidationGroup="main" />
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>




                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Company*</label>
                                <asp:DropDownList ID="ddlComp" runat="server" CssClass="form-control" 
                                    AppendDataBoundItems="True">
                                    <asp:ListItem Value="0">Select Company</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlComp"
                                    ErrorMessage="Please select company" SetFocusOnError="true" ValidationGroup="main" Display="None"
                                    InitialValue="0"></asp:RequiredFieldValidator>
                                </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Divisions*</label>
                                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control"
                                    OnSelectedIndexChanged="BranchDropDown_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="True"> 
                                    <asp:ListItem Value="0">Select Division</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlBranch"
                                    ErrorMessage="Please select branch" SetFocusOnError="true" ValidationGroup="main" Display="None"
                                    InitialValue="0"></asp:RequiredFieldValidator>
                                </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                </div>
                        </div>
                    </div>







                    <div class="form-group">
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Emplyee *</label>
                            <asp:DropDownList ID="ddlEmployee" runat="server" AppendDataBoundItems="False" CssClass="form-control">
                                    <asp:ListItem Value="0">Select Employee</asp:ListItem>
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ddlEmployee"
                                    ErrorMessage="Please select Employee" SetFocusOnError="true" ValidationGroup="main" Display="None"
                                    InitialValue="0"></asp:RequiredFieldValidator>


                            <%--<asp:TextBox ID="txtName" runat="server" MaxLength="60"
                                    CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtName"
                                    ErrorMessage="Please enter name" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>--%>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>User Group*</label>
                            <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-control"
                                    AppendDataBoundItems="True">
                                    <asp:ListItem Value="0">Select User Group</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlGroup"
                                    ErrorMessage="Please select user group" SetFocusOnError="true" ValidationGroup="main" Display="None"
                                    InitialValue="0"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                    </div>
				</div>
                    <div class="form-group">
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Login Id*</label>
                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="200"
                                    CssClass="form-control" autocomplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEmail"
                                    Display="None" ErrorMessage="Please enter login name" SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>City*</label>
                            <asp:DropDownList ID="ddlCity" runat="server"
                                    AppendDataBoundItems="True" CssClass="form-control">
                                    <asp:ListItem Value="0">Select City</asp:ListItem>
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlCity"
                                    ErrorMessage="Please select a city" SetFocusOnError="true" ValidationGroup="main" Display="None"
                                    InitialValue="0"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                    </div>
				</div>
                    <div class="form-group">
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Password*</label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control"
                                    TextMode="Password" password="" AutoCompleteType="Disabled" autocomplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword"
                                    ErrorMessage="Please enter password" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Confirm Password*</label>
                            <asp:TextBox ID="txtConfPwd" runat="server" CssClass="form-control"
                                    TextMode="Password" password=""></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtConfPwd"
                                    ErrorMessage="Please enter confirm password" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator1" SetFocusOnError="true" runat="server" ControlToCompare="txtConfPwd"
                                    ControlToValidate="txtPassword" Display="None" ErrorMessage="Confirm password should be same as Password"
                                    ValidationGroup="main"></asp:CompareValidator>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                    </div>
				</div>
                    <div class="form-group">
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Gender*</label>
                             <asp:RadioButtonList ID="rblGender" runat="server" CssClass="form-control"
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Male" Selected="True">Male</asp:ListItem>
                                    <asp:ListItem Value="Female">Female</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="reqGender" runat="server" ControlToValidate="rblGender"
                                    ErrorMessage="Please select gender" SetFocusOnError="true" ValidationGroup="main" Display="None">*</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Status*</label>
                                    <asp:RadioButtonList ID="rblStatus" runat="server" CssClass="form-control"
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Selected="True">Enable</asp:ListItem>
                                    <asp:ListItem Value="0">Disable</asp:ListItem>
                                </asp:RadioButtonList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                    </div>
				</div>
                    <div class="form-group" style="display:none;">
                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-sm-8">
                            <label>Vendor*</label>
                            <asp:DropDownList ID="ddlParty" runat="server" AppendDataBoundItems="true" CssClass="form-control">
                                    <asp:ListItem Value="0">No Vendor</asp:ListItem>
                                </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                    </div>
				</div>
                    <div class="form-group">
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                    </div>
				</div>
                </div>
            </div>
        </div>
    </div>


       <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4 ">
                <div class="card-body">
                    <div class="form-group">
                    <div class="row">

                        <div class="col-lg-3 col-md-3 col-sm-3">
                                 <asp:Label ID="Label1" runat="server" Text="Branch:"></asp:Label><br />
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged" AppendDataBoundItems="True" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Branch</asp:ListItem>
                            </asp:DropDownList>
                            </div>

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>User Id*</label>
                            <asp:TextBox runat="server" ID="txtFltUser" CssClass="form-control" AutoCompleteType="Disabled"></asp:TextBox>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Group*</label>
                            <asp:DropDownList runat="server" ID="ddlFltGroup" AppendDataBoundItems="true" CssClass="form-control">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>City*</label>
                            <asp:DropDownList runat="server" ID="ddlFltCity" AppendDataBoundItems="true" CssClass="form-control">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
				</div>
                    <div class="form-group">
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search Users" CssClass="btn btn-info" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                    </div>
				</div>
                    
<div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:GridView ID="grdUsers" runat="server" DataKeyNames="UserId" OnSelectedIndexChanged="grdUsers_SelectedIndexChanged"
                    AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdUsers_PageIndexChanging" PageSize="20"
                    EmptyDataText="There is no user defined" Width="100%" CssClass="table table-responsive-sm">
                    <Columns>
                        <asp:BoundField DataField="UserName" HeaderText="Full Name" />
                        <asp:BoundField DataField="LoginID" HeaderText="Login" />
                        <asp:BoundField DataField="GroupName" HeaderText="Group" />
                        <asp:BoundField DataField="CityName" HeaderText="City" />
                        <asp:BoundField DataField="CompName" HeaderText="Company" />
                        <asp:BoundField DataField="Gender" HeaderText="Gender" />
                        <%--<asp:BoundField DataField="vendor" HeaderText="Vendor" />--%>
                        <asp:CommandField  ShowSelectButton="True">
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
</div>
                </div>
                </div>
            </div>
        </div>
    </div>




   
   
    <asp:Label ID="lblFltSegment" runat="server" Text="Company:" Visible="false"></asp:Label><br />
                            <asp:DropDownList runat="server" ID="ddlFltComp" AppendDataBoundItems="true" Visible="false">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>

</asp:Content>
