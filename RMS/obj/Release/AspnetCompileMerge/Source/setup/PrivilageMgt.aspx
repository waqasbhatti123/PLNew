<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="PrivilageMgt.aspx.cs" Inherits="RMS.PrivilageMgt" Title="tblAppPrivilage Management"
    Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function SelectAll() {
            var chk = "<%= chkSelectAll.ClientID %>";
            SelectCol(chk, 1);
            SelectCol(chk, 2);
            SelectCol(chk, 3);
            //SelectCol(chk, 4);
            // SelectCol(chk, 5);
        }

        function SelectCol(id, cellindex) {
            //get reference of GridView control
            var grid = document.getElementById("<%= grdGroups.ClientID %>");
            //variable to contain the cell of the grid
            var cell;

            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (i = 0; i < grid.rows.length; i++) {
                    //get the reference of first column
                    cell = grid.rows[i].cells[cellindex];

                    //loop according to the number of childNodes in the cell
                    for (j = 0; j < cell.childNodes.length; j++) {
                        //if childNode type is CheckBox                 
                        if (cell.childNodes[j].type == "checkbox") {
                            //assign the status of the Select All checkbox to the cell checkbox within the grid
                            cell.childNodes[j].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">




    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4 ">
                <div class="card-body">

                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List" ValidationGroup="privilage" />
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <asp:DropDownList ID="ddlGroup" runat="server" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged"
                                AutoPostBack="True" CssClass="form-control">
                                <asp:ListItem Value="0">Select group</asp:ListItem>
                            </asp:DropDownList>
                            
                            <asp:RequiredFieldValidator ID="reqGroup" runat="server" ControlToValidate="ddlGroup"
                                ErrorMessage="Please select group" SetFocusOnError="true" ValidationGroup="privilage" Display="None"
                                InitialValue="0"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="form-control" onclick="javascript:SelectAll()"
                                Text="Select All" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
</div>
     </div>

                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">

                            <asp:GridView ID="grdGroups" runat="server" AutoGenerateColumns="False" DataKeyNames="AmID,AmIDParent,ModuleID" CssClass="table table-responsive-sm" Width="100%"
                                OnRowDataBound="grdGroups_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="AmName" HeaderText="Menu Name">
                                        <ItemStyle Width="85%" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Menu Enable" ControlStyle-Width="60">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkEnabled" runat="server" Checked='<%# Bind("Enabled") %>' />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkEnabledAll" runat="server" Text="Enabled" Visible="false" />Enabled
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Add" ControlStyle-Width="60">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkAdd" runat="server" Checked='<%# Bind("CanAdd") %>' />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAddAll" runat="server" Text="Add" Visible="false" />
                                            Entry 
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit" ControlStyle-Width="60">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkEdit" runat="server" Checked='<%# Bind("CanEdit") %>' />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkEditAll" runat="server" Text="Edit" Visible="false" />Approval
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" ControlStyle-Width="60" Visible="false">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkDelete" runat="server" Checked='<%# Bind("CanDel") %>' />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkDeleteAll" runat="server" Text="Delete" />
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Print" ControlStyle-Width="60" Visible="false">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkPrint" runat="server" Visible="false" Checked='<%# Bind("CanPrint") %>' />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkPrintAll" Visible="false" runat="server" Text="Print" />
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:Button ID="btnSave"
                                runat="server" CssClass="btn btn-primary" OnClick="btnSave_Click" Text="Save"
                                ValidationGroup="privilage" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>





</asp:Content>
