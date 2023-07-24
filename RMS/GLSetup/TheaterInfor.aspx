<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"
    AutoEventWireup="true" CodeBehind="TheaterInfor.aspx.cs" Inherits="RMS.GLSetup.TheaterInfor"
    Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>
    <script>
        $(document).ready(function () {
            $('#<%= ddlScurity.ClientID %>').change(function () {
                if ($('#<%= ddlScurity.ClientID%>').val() == "true") {
                    $(".systemCheck").show();
                }
                else {
                    $(".systemCheck").hide();
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                            <label>Division</label>
                            <asp:DropDownList ID="ddlDivisional" CssClass="form-control" runat="server" AppendDataBoundItems="False">
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Title</label>
                            <asp:TextBox runat="server" ID="txtTheaterTitle" CssClass="form-control"/>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Capacity</label>
                            <asp:TextBox runat="server" ID="txtCapacity" CssClass="form-control"/>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>License Number</label>
                            <asp:TextBox runat="server" ID="txtLicense" CssClass="form-control"/>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Valid Date</label>
                            <asp:TextBox runat="server" ID="txtvalid" CssClass="form-control"/>
                            <ajaxToolkit:CalendarExtender ID="txtvalidCal" runat="server" TargetControlID="txtvalid" Enabled="True">
                             </ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Contact Person Name</label>
                            <asp:TextBox runat="server" ID="txtconPer" CssClass="form-control"/>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Contact No</label>
                            <asp:TextBox runat="server" ID="txtconNo" CssClass="form-control"/>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Scurity System(CCTV)</label>
                            <asp:DropDownList ID="ddlScurity" CssClass="form-control" runat="server" AppendDataBoundItems="False">
                                <asp:ListItem Value="true">Yes</asp:ListItem>
                                <asp:ListItem Value="false">No</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    &nbsp;
                    <%--<div class="systemCheck">
                        <h2 style="text-align:center">Scurity System Information</h2>
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Cameras</label>
                                <asp:TextBox runat="server" ID="txt" />
                            </div>
                        </div>
                    </div>--%>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <label>Address</label>
                            <asp:TextBox ID="txtarearemaks" runat="server" CssClass="form-control" TextMode="MultiLine" onkeyup="LimitText(this,1000);" onblur="LimitText(this,1000);" Height="60px"> </asp:TextBox>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button runat="server" CssClass="btn btn-primary" ID="btnSave" OnClick="Save_click" Text="Save"/>
                            <asp:Button runat="server" CssClass="btn btn-success" ID="btnClear" OnClick="Clear_Click" Text="Clear"/>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12">
                 <asp:GridView ID="grdTheater" runat="server" CssClass="table table-responsive-sm" DataKeyNames="ThID" OnSelectedIndexChanged="grdtheater_SelectedIndexChanged"
                  AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdtheater_PageIndexChanging" OnRowDataBound="grdtheater_RowDataBound"
                  EmptyDataText="No Scheme define" Width="100%">
                  <Columns>
                      <asp:BoundField DataField="br_nme" HeaderText="Division" />
                      <asp:BoundField DataField="title" HeaderText="Title" />
                      <asp:BoundField DataField="Capacity" HeaderText="Capacity" />
                      <asp:BoundField DataField="ConPerson" HeaderText="Person Contact" />
                      <asp:BoundField DataField="Contactno" HeaderText="Contact No" />
                      <asp:BoundField DataField="Addresss" HeaderText="Address" />
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
