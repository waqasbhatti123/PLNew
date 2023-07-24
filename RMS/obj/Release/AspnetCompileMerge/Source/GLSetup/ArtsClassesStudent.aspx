<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"
    AutoEventWireup="true" CodeBehind="ArtsClassesStudent.aspx.cs" Inherits="RMS.GLSetup.ArtsClassesStudent"
    Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:ValidationSummary ID="ValidationSummary1" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                                ValidationGroup="main" />
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Division</label>
                            <asp:DropDownList ID="ddlDivisional" CssClass="form-control" runat="server" AppendDataBoundItems="False">
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Course Name</label>
                            <asp:TextBox runat="server" ID="txtCourceName" CssClass="form-control" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Teacher Name</label>
                            <asp:TextBox runat="server" ID="txtTeacherName" CssClass="form-control" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Total Enrolled student</label>
                            <asp:TextBox runat="server" ID="txtStudentNum" CssClass="form-control" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Course Fee</label>
                            <asp:TextBox runat="server" ID="txtCourceFee" CssClass="form-control" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Course Start</label>
                            <asp:TextBox runat="server" ID="txtcourceStart" CssClass="form-control" />
                            <ajaxToolkit:CalendarExtender ID="txtcourceStartCal" runat="server" TargetControlID="txtcourceStart" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Course End</label>
                            <asp:TextBox runat="server" ID="txtCourceEnd" CssClass="form-control" />
                            <ajaxToolkit:CalendarExtender ID="txtCourceEndCal" runat="server" TargetControlID="txtCourceEnd" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button Text="Save" CssClass="btn btn-primary" OnClick="StudentSave_Click" runat="server" />
                            <asp:Button Text="Save" CssClass="btn btn-success" OnClick="StudentClear_Click" runat="server" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:GridView ID="grdClasses" runat="server" CssClass="table table-responsive-sm" DataKeyNames="ArtID" OnSelectedIndexChanged="grdClasses_SelectedIndexChanged"
                                AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdClasses_PageIndexChanging" OnRowDataBound="grdClasses_RowDataBound"
                                EmptyDataText="No Scheme define" Width="100%">
                                <Columns>
                                   <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
                                   <asp:BoundField DataField="TeacherName" HeaderText="Teacher Name" />
                                   <asp:BoundField DataField="NumofStu" HeaderText="Num Of Student" />
                                   <asp:BoundField DataField="CourseFee" HeaderText="Course Fee" />
                                   <asp:BoundField DataField="CourseStart" HeaderText="Course Start" />
                                   <asp:BoundField DataField="CourseEnd" HeaderText="Course End" />
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
