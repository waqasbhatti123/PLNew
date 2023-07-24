<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="SalarySummary.aspx.cs" Inherits="RMS.Setup.SalarySummary" Culture="auto" UICulture="auto"
    EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .style1 {
            width: 100%;
        }
    </style>
    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />


    <div class="row">
        <div class="col-sm-12">
            <h3 id="hTitle" class="text-primary" runat="server"></h3>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-6">
            <asp:GridView ID="grdAllowances" CssClass="table table-responsive-sm table-bordered font-weight-light" runat="server" DataKeyNames="gl_cd" AutoGenerateColumns="False">
                <Columns>
                       <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                    <asp:Label runat="server" ID="txtcode" Text='<%#Eval("gl_cd") %>' Width="100px" CssClass="form-control form-control-sm"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                    <asp:TemplateField HeaderText="Allowances">
                    <ItemTemplate>
                    <asp:Label runat="server" ID="txtDsc" Text='<%#Eval("gl_dsc") %>' CssClass="form-control form-control-sm"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                    <asp:TemplateField HeaderText="Amount">
                        <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtAmount" Text='<%#Eval("amount") %>' Width="100px" CssClass="form-control form-control-sm" style="text-align:right" TextMode="Number"></asp:TextBox>
                        </ItemTemplate>
                     </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="col-sm-6">
            <asp:GridView ID="grdDeductions" CssClass="table table-responsive-sm table-bordered font-weight-light" runat="server" DataKeyNames="gl_cd" AutoGenerateColumns="False">
                <Columns>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                    <asp:Label runat="server" ID="txtcode" Text='<%#Eval("gl_cd") %>' Width="120px" CssClass="form-control form-control-sm"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                   <asp:TemplateField HeaderText="Deductions">
                    <ItemTemplate>
                    <asp:Label runat="server" ID="txtDsc" Text='<%#Eval("gl_dsc") %>' CssClass="form-control form-control-sm"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                    <asp:TemplateField HeaderText="Amount">
                        <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtAmount" Text='<%#Eval("amount") %>' Width="100px" CssClass="form-control form-control-sm" style="text-align:right" TextMode="Number"></asp:TextBox>
                        </ItemTemplate>
                     </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12">
            <asp:Button runat="server" ID="btnSave" class="btn btn-primary" Text="Save"  OnClick="BtnSave_Click">
				</asp:Button>
        </div>
    </div>
</asp:Content>
