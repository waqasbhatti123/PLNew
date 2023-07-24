<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="BranchMgt.aspx.cs" Inherits="RMS.Setup.BranchMgt" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
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
                                   ValidationGroup="main"/>
                              <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                     <div class="col-lg-6 col-md-6 col-sm-6">
                    <asp:GridView ID="grdBranchs" runat="server" CssClass="table table-hover" DataKeyNames="br_id" OnSelectedIndexChanged="grdBranchs_SelectedIndexChanged"
                    AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdBranchs_PageIndexChanging"
                    EmptyDataText="There is no branch defined" Width="90%">
                    <Columns>
                        <asp:BoundField DataField="LoCode" HeaderText="DDO Code" />
                        <asp:BoundField DataField="br_nme" HeaderText="Division Name" />
                        <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True" >
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                    </Columns>
                    <HeaderStyle CssClass="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                    </asp:GridView>
                     </div>
                     <div class="col-lg-6 col-md-6 col-sm-6">
                         <asp:Panel runat="server" ID="pnlMain">
                         <div class="row">
                             <div class="col-lg-4 col-md-4 col-sm-4">
                                 <asp:Label ID="lblBrName" runat="server" Text="Division Name:" ></asp:Label>
                             </div>
                             <div class="col-lg-6 col-md-6 col-sm-6">
                                 <asp:TextBox ID="txtBrName" runat="server" MaxLength="60" 
                                   CssClass="RequiredField form-control"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtBrName"
                                   ErrorMessage="Please enter branch name" SetFocusOnError="true" ValidationGroup="main" Display="None" ></asp:RequiredFieldValidator>
                             </div>
                         </div>
                             &nbsp;
                          <div class="row">
                             <div class="col-lg-4 col-md-4 col-sm-4">
                                 <asp:Label ID="lblComp" runat="server" Text="Company:" ></asp:Label>
                             </div>
                             <div class="col-lg-6 col-md-6 col-sm-6">
                                 <asp:DropDownList ID="ddlCompany" runat="server" CssClass="RequiredField form-control" 
                                     AppendDataBoundItems = "True">
                                     <%--<asp:ListItem Value="0">Select Company</asp:ListItem>--%>
                                 </asp:DropDownList>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlCompany"
                                     ErrorMessage="Please select company" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                     InitialValue="0"></asp:RequiredFieldValidator>
                             </div>
                         </div>
                          &nbsp;
                        <div class="row">
                             <div class="col-lg-4 col-md-4 col-sm-4">
                                 <asp:Label ID="lbldDt" runat="server" Text="Division/District/Tehsil:" ></asp:Label>
                             </div>
                             <div class="col-lg-6 col-md-6 col-sm-6">
                                 <asp:DropDownList ID="DdlTehsil" runat="server" CssClass="RequiredField form-control" 
                                     AppendDataBoundItems = "True">
                                     <asp:ListItem Value="">Select</asp:ListItem>
                                     <asp:ListItem Value="0">Head Office</asp:ListItem>
                                     <asp:ListItem Value="1">Division</asp:ListItem>
                                     <asp:ListItem Value="2">District</asp:ListItem>
                                     <asp:ListItem Value="3">Tehsil</asp:ListItem>
                                 </asp:DropDownList>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlCompany"
                                     ErrorMessage="Please select company" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                     InitialValue="0"></asp:RequiredFieldValidator>
                             </div>
                         </div>
                             <div class="row">
                                 <div class="col-lg-4 col-md-4 col-sm-4">
                                     <asp:Label ID="lblCode" runat="server" Text="DDO Code:" ></asp:Label>
                                 </div>
                                 <div class="col-lg-6 col-md-6 col-sm-6">
                                     <asp:TextBox ID="txtLoCode" runat="server" MaxLength="30" CssClass="form-control"></asp:TextBox>
                                 </div>
                             </div>
                             &nbsp;
                             <div class="row">
                             <div class="col-lg-4 col-md-4 col-sm-4">
                                 <asp:Label ID="lblNtn" runat="server" Text="NTN #:" ></asp:Label>
                             </div>
                             <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:TextBox ID="txtNtn" runat="server" MaxLength="15" CssClass="form-control"></asp:TextBox>
                             </div>
                         </div>
                         &nbsp;
                             <div class="row">
                             <div class="col-lg-4 col-md-4 col-sm-4">
                                 <asp:Label ID="lbStx" runat="server" Text="Sales Tax #:" ></asp:Label>
                             </div>
                             <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:TextBox ID="txtSTx" runat="server" MaxLength="15" CssClass="form-control"></asp:TextBox>
                             </div>
                         </div>
                        &nbsp;
                             <div class="row">
                             <div class="col-lg-4 col-md-4 col-sm-4">
                                 <asp:Label ID="lblTel" runat="server" Text="Primary Telephone #:" ></asp:Label>
                             </div>
                             <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:TextBox ID="txtTel" runat="server" MaxLength="15" CssClass="form-control"></asp:TextBox>
                             </div>
                         </div>
                            &nbsp;
                             <div class="row">
                             <div class="col-lg-4 col-md-4 col-sm-4">
                                 <asp:Label ID="lblSec" runat="server" Text="Secondary Telephone #:" ></asp:Label>
                             </div>
                             <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:TextBox ID="txtSecTel" runat="server" MaxLength="15" CssClass="form-control"></asp:TextBox>
                             </div>
                         </div>
                             &nbsp;
                             <div class="row">
                             <div class="col-lg-4 col-md-4 col-sm-4">
                                 <asp:Label ID="lblFax" runat="server" Text="Fax #:" ></asp:Label>
                             </div>
                             <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:TextBox ID="txtFaxNo" runat="server" MaxLength="50" CssClass="form-control"></asp:TextBox>
                             </div>
                         </div>
                         &nbsp;
                        <div class="row">
                             <div class="col-lg-4 col-md-4 col-sm-4">
                                 <asp:Label ID="lblAdd" runat="server" Text="Address:" ></asp:Label>
                             </div>
                             <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:TextBox ID="txtAdd" runat="server" MaxLength="500" CssClass="form-control"></asp:TextBox>
                             </div>
                         </div>
                        &nbsp;
                        <div class="row">
                             <div class="col-lg-4 col-md-4 col-sm-4">
                                 <asp:Label ID="lblEnabled" runat="server" Text="Status:" ></asp:Label>
                             </div>
                             <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:RadioButtonList ID="rblStatus" runat="server" 
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Selected="True">Enable</asp:ListItem>
                                    <asp:ListItem Value="0">Disable</asp:ListItem>
                                </asp:RadioButtonList>
                             </div>
                         </div>
                        </asp:Panel> 
                         &nbsp;
                         <div class="row">
                             <div class="col-lg-4 col-md-4 col-sm-4">

                             </div>
                             <div class="col-lg-4 col-md-4 col-sm-4" >
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
