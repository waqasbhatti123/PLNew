<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="SalaryContent.aspx.cs" Inherits="RMS.Setup.SalaryContent" Title="UOM" Culture="auto" UICulture="auto"
    EnableEventValidation="true"%>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>
    <script type="text/javascript" language="javascript">
        
        <%--$(function () {
            $('#<%=checkIsPercen.ClientID%>').change(function () {
              
                debugger
                if ($('[id*=checkIsPercen]').is(':checked')) {
                    
                    var textSizeVal = $('[id*=txtSize]').val();
                    //$('#txtSize').val("");
                    var txtNameVal = $('[id*=txtName]').val();
                    $('[id*=txtName]').val("");
                    $('[id*=txtName]').val(txtNameVal + " @ " + textSizeVal + "%");
                }
                else if ($(this).is(":not(:checked)")) {
                    var txtNameVal = $('[id*=txtName]').val();
                    if (txtNameVal.indexOf('%') > -1) {
                        $('[id*=txtName]').val("");
                        var generalValSplit = txtNameVal.split('@');
                        var genralVal = generalValSplit[0];
                        $('[id*=txtName]').val(genralVal);
                    }
                }
            });
        });--%>

    </script>
    <script>
        $(document).ready(function () {
            $('#txtNameError').fadeIn('slow', function () {
                $('#txtNameError').delay(5000).fadeOut();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    
    
    

    <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12">
                <div class="card card-shadow mb-4">
                    <div class="card-body">
                        <div class="col-lg-12 col-md-12 sol-sm-12">
                            <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                              ValidationGroup="main"/>
                          <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Allowance / Deduction*</label>
                                <asp:DropDownList ID="ddlAllDd" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="This Field is required"
                                    ControlToValidate="ddlAllDd" InitialValue="0" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        &nbsp;
                       
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Name*</label>
                                <asp:TextBox ID="txtName" runat="server" class="txtName" CssClass="form-control "></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtName"
                                                ErrorMessage="Please select Name" SetFocusOnError="true" ValidationGroup="main"
                                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-3" style="margin-top:35px">
                                <asp:CheckBox ID="CheckIsActive" Checked="true" runat="server" />&nbsp;<label>Is Active</label>
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <%--<label>Size*</label>
                                <asp:TextBox ID="txtSize" runat="server" class="txtsize" CssClass="form-control "></asp:TextBox>--%>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Sort Reference*</label>
                                <asp:TextBox ID="txtSortReg" runat="server" class="txtName" CssClass="form-control "></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSortReg"
                                                ErrorMessage="Please Set Sort Refrence" SetFocusOnError="true" ValidationGroup="main"
                                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        &nbsp;
                        <div class="row">
                            <%--<div class="col-lg-3 col-md-3 col-sm-3">
                                <asp:CheckBox ID="checkIsPercen" class="checkbox" runat="server" />&nbsp; <label id="per">Is Percentage</label>
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <asp:CheckBox ID="CheckIsActive" Checked="true" runat="server" />&nbsp;<label>Is Active</label>
                            </div>--%>
                        </div>
                        &nbsp;
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Button ID="btnAddAll" CssClass="btn btn-primary" runat="server" Text="Save" OnClick="btnAll_Ddu" />
                            </div>
                        </div>
                        &nbsp;
                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12">
                                <asp:GridView ID="grdAll" runat="server" DataKeyNames="SalaryContentID" CssClass="table table-responsive-sm" OnSelectedIndexChanged="grdAll_SelectedIndexChanged"
                                    AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdAll_PageIndexChanging" OnRowDataBound="grdAll_RowDataBound"
                                    EmptyDataText="There is no Record" Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="SalaryContentID" HeaderText="Sr #" />
                                        <asp:BoundField DataField="sao" HeaderText="Allowance / Deduction" />
                                        <asp:BoundField DataField="Name" HeaderText="Name" />
                                        <asp:BoundField DataField="Sort" HeaderText="Sort Order" />
                                        <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                            <ControlStyle CssClass="lnk"></ControlStyle>
                                        </asp:CommandField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <%-- <asp:LinkButton ID="lnkPrint" runat="server" Text="Print" ToolTip="Print Employee Profile" OnClick="lnkPrint_Click" CssClass="lnk">
                                                            </asp:LinkButton>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
</asp:Content>
