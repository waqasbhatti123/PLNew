<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="AddtionalAllDdd.aspx.cs" Inherits="RMS.profile.AddtionalAllDdd" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" src="../js/empsaltotal.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12">
                <div class="card card-shadow mb-4">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:ValidationSummary ID="ValidationSummary1" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                              ValidationGroup="main"/>
                            <uc1:Messages ID="ucMessage" runat="server" />
                            </div>
                        </div>
                         <div class="row">
            <div class="col-lg-4 col-md-4 col-sm-4">
                 <label>Employees*</label>
    
           <asp:DropDownList ID="ddlEmployee" CssClass="form-control" runat="server" AppendDataBoundItems="False">
              <%-- <asp:ListItem Value="0">Select Employee</asp:ListItem>--%>
           </asp:DropDownList>
           <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Employee is required"
               ControlToValidate="ddlEmployee" InitialValue="0" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
            
                </div>
                <div class="col-lg-4 col-md-4 col-sm-4">
                 <label>Type*</label>
                 <asp:DropDownList ID="ddlAllDd" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                     <asp:ListItem Value="0">Select Allowance / Deduction</asp:ListItem>
                 </asp:DropDownList>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Allowance Deduction Type is Required"
                     ControlToValidate="ddlAllDd" InitialValue="0" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>  
              </div>
                <div class="col-lg-4 col-md-4 col-sm-4">
                <label>Select*</label>
                <asp:DropDownList ID="ddlallDed" CssClass="form-control" runat="server" AppendDataBoundItems="False">
              <%-- <asp:ListItem Value="0">Select Employee</asp:ListItem>--%>
           </asp:DropDownList>
           <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Employee is required"
               ControlToValidate="ddlEmployee" InitialValue="0" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                </div>
                
        </div>
        &nbsp;
        <div class="row">
            <div class="col-lg-4 col-md-4 col-sm-4">
                <label>From*</label><span class="DteLtrl" > <asp:Literal ID="Literal1" runat="server" Text="<%$ AppSettings: DateFormatPageText %>" /></span >
                <asp:TextBox runat="server" ID="txtfrom" CssClass="form-control"></asp:TextBox>
                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtfrom" Enabled="True">
                </ajaxToolkit:CalendarExtender>

            </div>
            <div class="col-lg-4 col-md-4 col-sm-4">
                <label>To*:</label><span class="DteLtrl" > <asp:Literal ID="Literal2" runat="server" Text="<%$ AppSettings: DateFormatPageText %>" /></span >
                <ajaxToolkit:CalendarExtender ID="txtToCal" runat="server" TargetControlID="txtTo"
                 Enabled="True" >
                </ajaxToolkit:CalendarExtender >
               <asp:TextBox ID="txtTo" runat="server" MaxLength="11"  CssClass="RequiredField form-control" > </asp:TextBox >
               
               <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please select end date"
               ControlToValidate="txtTo" SetFocusOnError="true" ValidationGroup="main"
               Display="None" > </asp:RequiredFieldValidator>
                <%--<label>To*</label>
                <asp:TextBox runat="server" ID="txtTo" CssClass="form-control"></asp:TextBox>
                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtTo" Enabled="True">
                </ajaxToolkit:CalendarExtender>--%>
            </div>
            <div class="col-lg-4 col-md-4 col-sm-4">
                <label>Size*</label>
                <asp:TextBox runat="server" ID="txtSize" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        &nbsp;
        <div class="row">
            <div class="col-lg-4 col-md-4 col-sm-4">
               <asp:CheckBox ID="checkIsPercen" class="checkbox" runat="server" />&nbsp; <label id="per">Is Percentage</label>
           </div>
           <div class="col-lg-4 col-md-4 col-sm-4">
               <asp:CheckBox ID="CheckIsActive" Checked="true" runat="server" />&nbsp;<label>Is Active</label>
           </div>
        </div>
        &nbsp;
         <div class="row">
          <div class="col-lg-4 col-md-4 col-sm-4">
          <asp:Button ID="Button1" CssClass="btn btn-primary" runat="server" Text="Save" OnClick="btnAdd_Click" />
              <asp:Button ID="Button2" CssClass="btn btn-danger" OnClick="btnClear_Click" runat="server" Text="Clear" />
         </div>
        </div>
        &nbsp;

                          <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                 <asp:Label ID="Label1" runat="server" Text="Branch:"></asp:Label><br />
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged" AppendDataBoundItems="True" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Division</asp:ListItem>
                            </asp:DropDownList>
                            </div>
                           
                        </div>
                                                   
                             <br />  
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12">
                 <asp:GridView ID="grdAddtional" runat="server" CssClass="table table-responsive-sm" DataKeyNames="AddID" OnSelectedIndexChanged="grdAdd_SelectedIndexChanged"
                  AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdAdd_PageIndexChanging" OnRowDataBound="grdAddtional_RowDataBound"
                  EmptyDataText="No Addtional Allowances / Deduction" Width="100%">
                  <Columns>
                      <asp:BoundField DataField="EmpName" HeaderText="Employee Name" />
                      <asp:BoundField DataField="salName" HeaderText="Type" />
                      <asp:BoundField DataField="Name" HeaderText=" Name" />
                      <asp:BoundField DataField="size" HeaderText="Size" />
                      <asp:BoundField DataField="fromd" HeaderText="From Date" />
                      <asp:BoundField DataField="tod" HeaderText="To Date" />
                      <asp:BoundField DataField="isActive" HeaderText="Is Active" />
                      <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                          <ControlStyle CssClass="lnk"></ControlStyle>
                      </asp:CommandField>
                  </Columns>
                 </asp:GridView>
            </div>
        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>



