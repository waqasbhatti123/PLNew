<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="Employee.aspx.cs" Inherits="RMS.profile.Employee" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>

    <script src="http://code.jquery.com/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.16.js"></script>
    <link href="../Scripts/jquery-ui.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.jquery.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.min.css" rel="stylesheet" />

    <script>
        $(document).ready(function () {
            $('#<%= txtEmpSearch.ClientID %>').autocomplete({
                source: function (request, response) {
                    debugger
                    var param = { employee: $(".txtsearch").val() };
                    $.ajax({
                        url: "Employee.aspx/GetEmployee",
                        data: JSON.stringify(param),
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            debugger
                            response($.map(data.d, function (item) {
                                return {
                                    value: item.FullName,
                                    result: item.EmpID,
                                    id: item.EmpID
                                }
                            }))
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus);
                        }
                    });
                },
                <%--select: function (e, ui) {
                    //glCd = ui.item.id;
                    // getFirstRowCode();
                    if (ui.item.result != '') {
                        $('#<%= ddlEmployee.ClientID %>').val(ui.item.result);
                }
                else {
                    $('#<%= ddlEmployee.ClientID %>').val('0');
                    }

                },--%>
                minLength: 1
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4">
                <div class="card-body">
                    <div class="col-lg-12 col-md-12 col-sm-12">
                        <asp:ValidationSummary ID="ValidationSummary1" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                            ValidationGroup="main" />
                        <uc1:Messages ID="ucMessage" runat="server" />
                    </div>
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Personal File No*</label>
                            <asp:TextBox ID="txtEmpCode" runat="server" MaxLength="50" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEmpCode"
                                ErrorMessage="Please enter employee code." SetFocusOnError="true" ValidationGroup="main"
                                Display="None"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Name*</label>
                            <asp:TextBox ID="txtFullName" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqName" runat="server" ControlToValidate="txtFullName"
                                ErrorMessage="Please enter name" SetFocusOnError="true" ValidationGroup="main"
                                Display="None"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Father Name*</label>
                            <asp:TextBox ID="txtFatherName" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Sort Reference*</label>
                            <asp:TextBox ID="txtsortRefe" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Bank</label>
                            <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlBank_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Bank</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Bank Branch</label>
                            <asp:UpdatePanel ID="upnl" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtBankBranch" runat="server" MaxLength="50" CssClass="form-control" Enabled="false"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlBank" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Account#</label>
                            <asp:TextBox ID="txtBankAcct" runat="server" MaxLength="20" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button ID="Button1" CssClass="btn btn-primary" runat="server" Text="Save" OnClick="btnAdd_Click" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Select Employee</label>
                            <asp:TextBox runat="server" ID="txtEmpSearch" CssClass="form-control txtsearch"></asp:TextBox>
                            <div id="EmployeeList">
                            </div>
                        </div>
                        <%--<div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label ID="Label1" runat="server" Text="Personal File No:"></asp:Label><br />

                            <asp:DropDownList ID="ddlperson" runat="server" CssClass="form-control ddlEmpDrpdwnrt" OnSelectedIndexChanged="ddlPersonal_change" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Personal File Number</asp:ListItem>
                            </asp:DropDownList>
                        </div>--%>
                        <div class="col-lg-4 col-md-4 col-sm-4" style="margin-top: 30px">
                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                OnClick="btnSearch_Click" ToolTip="Search Emps" />
                        </div>
                    </div>
                    &nbsp;
                    <asp:GridView ID="grdEmps" runat="server" DataKeyNames="EmpId" CssClass="table table-responsive-sm" OnSelectedIndexChanged="grdEmps_SelectedIndexChanged"
                        AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdEmps_PageIndexChanging"
                        EmptyDataText="There is no employee defined" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="EmpCode" HeaderText="Personal File No" />
                            <asp:BoundField DataField="FullName" HeaderText="Name" />
                            <asp:BoundField DataField="Desig" HeaderText="Current Posting" />
                            <asp:BoundField DataField="Dept" HeaderText="Section" />
                            <asp:BoundField DataField="branch" HeaderText="Place of Posting" />
                            <%-- <asp:BoundField DataField="Reg" HeaderText="Region" />--%>
                            <asp:BoundField DataField="CityName" HeaderText="City" />
                            <%--<asp:BoundField DataField="LocName" HeaderText="Location" />--%>
                            <asp:BoundField DataField="Gender" HeaderText="Gender" />
                            <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                <ControlStyle CssClass="lnk lkk"></ControlStyle>
                            </asp:CommandField>
                            <%--<asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPrint" runat="server" Text="Print" ToolTip="Print Employee Profile" OnClick="lnkPrint_Click" CssClass="lnk">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>


    <script>
        <%--$('#<%=ddlEmpDrpdwn.ClientID%>').chosen();
        $('#<%=ddlperson.ClientID%>').chosen();--%>
    </script>

</asp:Content>


