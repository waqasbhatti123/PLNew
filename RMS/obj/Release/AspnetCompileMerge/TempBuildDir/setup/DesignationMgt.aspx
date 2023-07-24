<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="DesignationMgt.aspx.cs" Inherits="RMS.Setup.DesignationMgt" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
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
                    &nbsp;
                    <asp:Panel runat="server" ID="pnlMain">
                        <div class="row">
                            <div class="col-lg-8 col-md-8 col-sm-8">
                                <asp:GridView ID="grdDesignations" CssClass="table table-responsive-sm table-bordered" runat="server" DataKeyNames="CodeID" OnSelectedIndexChanged="grdDesignations_SelectedIndexChanged"
                                    AutoGenerateColumns="False" AllowPaging="True" PageSize="20" Width="98%" OnPageIndexChanging="grdDesignations_PageIndexChanging" OnRowDataBound="grdDesignations_RowDataBound">
                                    <HeaderStyle CssClass="grid_hdr" />
                                    <RowStyle CssClass="grid_row" />
                                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                                    <SelectedRowStyle CssClass="gridSelectedRow" />
                                    <Columns>

                                        <asp:BoundField DataField="CodeDesc" HeaderText="Designation Name" />
                                        <asp:BoundField DataField="Enabled" HeaderText="Status" />
                                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                                            <ItemStyle />
                                            <ControlStyle CssClass="lnk"></ControlStyle>
                                        </asp:CommandField>
                                    </Columns>
                                </asp:GridView>
                            </div>

                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Label ID="lblName" runat="server" Text="Designation Name*"></asp:Label>
                                <asp:TextBox ID="txtDesignation" CssClass="RequiredField form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDesignation"
                                    ErrorMessage="Please enter designation name" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                                <br />
                                <asp:Label ID="lblsort" runat="server" Text="Sort Refrence*"></asp:Label>
                                <asp:TextBox ID="txtsort" CssClass="RequiredField form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtsort"
                                    ErrorMessage="Please enter sort reference" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                                <br />
                                <asp:Label ID="lblEnable" runat="server" Text="Status:"></asp:Label>
                                <asp:RadioButtonList ID="rblStatus" runat="server"
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Selected="True">Enable</asp:ListItem>
                                    <asp:ListItem Value="0">Disable</asp:ListItem>
                                </asp:RadioButtonList>
                                <br />
                                <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
                                <!--img src="images/btn_new.jpg" width="60" height="20" /> <img src="images/btn_edit.jpg" width="60" height="20" /> <img src="images/btn_delete.jpg" alt="" width="60" height="20" /> <img src="images/btn_save.jpg" width="60" height="20" /> <img src="images/btn_cancel.jpg" width="60" height="20" /-->
                            </div>

                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>






</asp:Content>
