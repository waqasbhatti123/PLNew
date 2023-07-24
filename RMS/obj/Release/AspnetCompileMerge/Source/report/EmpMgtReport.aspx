<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmpMgtReport.aspx.cs" Inherits="RMS.Setup.EmpMgtReport" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label ID="Label1" runat="server" Text="As Of Date*"></asp:Label> <asp:Literal ID="Literal1" runat="server" Text="<%$ AppSettings: DateFullYearFormatPageText %>"/>
                            <asp:TextBox ID="txtReportDate" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="please enter date" ControlToValidate="txtReportDate"></asp:RequiredFieldValidator>
                            <ajaxToolkit:CalendarExtender ID="txtReportDateCal" runat="server" 
                              TargetControlID="txtReportDate" Enabled="True">
                              </ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4" style="margin-top:25px;">
                            <asp:Button ID="btnReport" runat="server" CssClass="btn btn-primary" Text="Show" onclick="btnReport_Click"  />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <asp:GridView ID="grdlev" runat="server" DataKeyNames="EmpID" 
                    AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdEmps_PageIndexChanging"
                     OnRowDataBound="grdEmps_RowDataBound"
                    EmptyDataText="There is no Report" Width="99%">
                    <Columns>
                     
                       <asp:TemplateField HeaderText="Sr.#">
                    <ItemTemplate>
                        <asp:Label ID="lblSerial" runat="server"></asp:Label>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                   </asp:TemplateField> 
                        <asp:BoundField DataField="EmployeeId" HeaderText="Emp ID" />
                        <asp:BoundField DataField="EmpCode" HeaderText="Emp Ref" />
                        <asp:BoundField DataField="name" HeaderText="Employee Name" ><ItemStyle Width="310px"/></asp:BoundField>
                        <asp:BoundField DataField="desig" HeaderText="Desig" />
                        <asp:BoundField DataField="dpt" HeaderText="Dept" />
                        <asp:BoundField DataField="divis" HeaderText="Division" />
                        <asp:BoundField DataField="cty" HeaderText="City" />
                        <asp:BoundField DataField="DOJ" HeaderText="Joining Date" ><ItemStyle Width="80px"/></asp:BoundField>
                        <asp:BoundField DataField="DOL" HeaderText="Leaving Date" ><ItemStyle Width="80px"/></asp:BoundField>
                        <asp:BoundField DataField="regi" HeaderText="Region" />
                                              
                      
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
