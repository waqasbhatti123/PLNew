<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="Groups.aspx.cs" Inherits="RMS.Setup.Groups" Title="Groups" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
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
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <asp:GridView ID="grdGroups" runat="server" DataKeyNames="GroupID" Width="100%" CssClass="table table-responsive-sm"
                                OnSelectedIndexChanged="grdGroups_SelectedIndexChanged" AutoGenerateColumns="False"
                                AllowPaging="True" PageSize="13" OnPageIndexChanging="grdGroups_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="GroupName" HeaderText="Group Name" />
                                    <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                                        <ItemStyle />
                                        <ControlStyle></ControlStyle>
                                    </asp:CommandField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <div class="row">
                                <div class="col-lg-6 col-md-6 col-sm-6">
                                    <label>Group*</label>
                                    <asp:TextBox ID="txtGroup" CssClass="form-control" runat="server" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtGroup"
                                        ErrorMessage="Please enter group name" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                             <div class="row">
                                <div class="col-lg-6 col-md-6 col-sm-6">
                                    &nbsp;
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6 col-md-6 col-sm-6">
                                    <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
