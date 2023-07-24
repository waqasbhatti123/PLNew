<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="QualificationReport.aspx.cs" Inherits="RMS.report.QualificationReport" Culture="auto" UICulture="auto"
    EnableEventValidation="true" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" 
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>
    <script>
     //   var id = '';
     //   $(function () {
     //       debugger
     //       //id = $("[id*=grdEducation]").find("input[type=text][id*=txtEmpID]").val();
     //       id = $("[id*=grdEducation]input[type=text][id*=txtEmpID]").closest('tr').find("input[type=text][id*=txtEmpID]").val();
     //   });

     //   function OpenNewWindow() {
     //       debugger
     //       window.open('QualificationDetailReport.aspx?ID='+ id +'')
     //}
    </script>
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
                            <asp:Label Text="Division" ID="Label2" runat="server" />
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">All</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Degree Type*</label>
                             <asp:DropDownList ID="ddlDegreeType" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                                 <asp:ListItem Value="0">Select Degree</asp:ListItem>
                                 <asp:ListItem Value="Matric">Matric</asp:ListItem>
                                 <asp:ListItem Value="Intermediate">Intermediate</asp:ListItem>
                                 <asp:ListItem Value="Becholar(14 Years)">Becholar(14 Years)</asp:ListItem>
                                 <asp:ListItem Value="Becholar(16 Years)">Becholar(16 Years)</asp:ListItem>
                                 <asp:ListItem Value="Master(16 Years)">Master(16 Years)</asp:ListItem>
                                 <asp:ListItem Value="MS/M.Phil">MS/M.Phil</asp:ListItem>
                                 <asp:ListItem Value="PHD">PHD</asp:ListItem>
                             </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Report Type*</label>
                             <asp:DropDownList ID="ddlReportType" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                                 <asp:ListItem Value="0">All</asp:ListItem>
                                 <asp:ListItem Value="1">Serving Employee</asp:ListItem>
                                 <asp:ListItem Value="2">Relieved Employee</asp:ListItem>
                             </asp:DropDownList>
                        </div>
                        <%--<div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Marks Percentage(%)</label>
                            <asp:TextBox runat="server" ID="txtPercentage" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                             <label>Verified</label>
                             <asp:DropDownList ID="ddlEduVerified" runat="server" CssClass="form-control">
                                 <asp:ListItem Value="">Select</asp:ListItem>
                                 <asp:ListItem Value="True">Yes</asp:ListItem>
                                 <asp:ListItem Value="False">No</asp:ListItem>
                             </asp:DropDownList>
                         </div>--%>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button runat="server" CssClass="btn btn-primary" ID="btnSearch" OnClick="eduSearch_Click" Text="Search"/>
                        </div>
                    </div>
                    &nbsp;
                     <div class="col-lg-12 col-md-12 col-sm-12">
                         <asp:GridView ID="grdEducation" runat="server" CssClass="table table-responsive-sm" DataKeyNames="EmpEduID,EmpID" OnSelectedIndexChanged="grdEduEmps_SelectedIndexChanged"
                             AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdEduEmps_PageIndexChanging" OnRowDataBound="grdEdu_rowbound"
                             EmptyDataText="There is no employee defined" Width="100%">
                             <Columns>
                                 <asp:BoundField DataField="FullName" HeaderText="Employee Name" />
                                 <asp:BoundField DataField="CodeDesc" HeaderText="Current posting" />
                                 <asp:BoundField DataField="DegreeTitle" HeaderText="Degree Title" />
                                 <asp:BoundField DataField="UniversityBoard" HeaderText="Board/Uni" />
                                 <asp:BoundField DataField="Percente" HeaderText="Percentage" />
                                 <asp:BoundField DataField="Verified" HeaderText="Verified" />
                              <asp:TemplateField HeaderText="Print" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnprint" runat="server" OnClientClick="OpenNewWindow()" CommandArgument='<%#Eval("EmpID")%>' OnClick="lnkPrint_Click" Text="Print"></asp:LinkButton>
                                 </ItemTemplate>                             
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp runat="server" ID="txtEmpID"  Text='<%#Eval("EmpID")%>' Visible="false"></asp>
                                </ItemTemplate>
                            </asp:TemplateField>
                             </Columns>
                             <HeaderStyle CssClass="grid_hdr" />
                             <RowStyle CssClass="grid_row" />
                             <AlternatingRowStyle CssClass="gridAlternateRow" />
                             <SelectedRowStyle CssClass="gridSelectedRow" />
                         </asp:GridView>
                     </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                         <asp:GridView ID="GrdReli" runat="server" CssClass="table table-responsive-sm" DataKeyNames="EmpEduID,EmpID" OnSelectedIndexChanged="GrdReli_SelectedIndexChanged"
                             AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="GrdReli_PageIndexChanging" OnRowDataBound="GrdReli_rowbound"
                             EmptyDataText="There is no employee defined" Width="100%">
                             <Columns>
                                 <asp:BoundField DataField="FullName" HeaderText="Employee Name" />
                                 <asp:BoundField DataField="CodeDesc" HeaderText="Current posting" />
                                 <asp:BoundField DataField="DegreeTitle" HeaderText="Degree Title" />
                                 <asp:BoundField DataField="UniversityBoard" HeaderText="Board/Uni" />
                                 <asp:BoundField DataField="Percente" HeaderText="Percentage" />
                                 <asp:BoundField DataField="Verified" HeaderText="Verified" />
                              <asp:TemplateField HeaderText="Print" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnprint" runat="server" OnClientClick="OpenNewWindow()" CommandArgument='<%#Eval("EmpID")%>' OnClick="lnkPrint_Click" Text="Print"></asp:LinkButton>
                                 </ItemTemplate>                             
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp runat="server" ID="txtEmpID"  Text='<%#Eval("EmpID")%>' Visible="false"></asp>
                                </ItemTemplate>
                            </asp:TemplateField>
                             </Columns>
                             <HeaderStyle CssClass="grid_hdr" />
                             <RowStyle CssClass="grid_row" />
                             <AlternatingRowStyle CssClass="gridAlternateRow" />
                             <SelectedRowStyle CssClass="gridSelectedRow" />
                         </asp:GridView>
                     </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                         <asp:GridView ID="AllEmp" runat="server" CssClass="table table-responsive-sm" DataKeyNames="EmpEduID,EmpID" OnSelectedIndexChanged="AllEmp_SelectedIndexChanged"
                             AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="AllEmp_PageIndexChanging" OnRowDataBound="AllEmp_rowbound"
                             EmptyDataText="There is no employee defined" Width="100%">
                             <Columns>
                                 <asp:BoundField DataField="FullName" HeaderText="Employee Name" />
                                 <asp:BoundField DataField="CodeDesc" HeaderText="Current posting" />
                                 <asp:BoundField DataField="DegreeTitle" HeaderText="Degree Title" />
                                 <asp:BoundField DataField="UniversityBoard" HeaderText="Board/Uni" />
                                 <asp:BoundField DataField="Percente" HeaderText="Percentage" />
                                 <asp:BoundField DataField="Verified" HeaderText="Verified" />
                              <asp:TemplateField HeaderText="Print" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnprint" runat="server" OnClientClick="OpenNewWindow()" CommandArgument='<%#Eval("EmpID")%>' OnClick="lnkPrint_Click" Text="Print"></asp:LinkButton>
                                 </ItemTemplate>                             
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp runat="server" ID="txtEmpID"  Text='<%#Eval("EmpID")%>' Visible="false"></asp>
                                </ItemTemplate>
                            </asp:TemplateField>
                             </Columns>
                             <HeaderStyle CssClass="grid_hdr" />
                             <RowStyle CssClass="grid_row" />
                             <AlternatingRowStyle CssClass="gridAlternateRow" />
                             <SelectedRowStyle CssClass="gridSelectedRow" />
                         </asp:GridView>
                     </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:Panel ID="Panel1" runat="server" Width="780px" Height="600">
                            <rsweb:ReportViewer ID="viewer" runat="server" Width="100%" Height="580px">
                            </rsweb:ReportViewer>
                        </asp:Panel> 
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
