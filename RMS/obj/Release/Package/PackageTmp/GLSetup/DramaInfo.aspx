<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"
    AutoEventWireup="true" CodeBehind="DramaInfo.aspx.cs" Inherits="RMS.GLSetup.DramaInfo"
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
                              ValidationGroup="main"/>
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Select Theatre</label>
                            <asp:DropDownList ID="ddltheatreName" CssClass="form-control" runat="server" AppendDataBoundItems="False">
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Title</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtDramatitle"/>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Date From</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtDateFrom"/>
                            <ajaxToolkit:CalendarExtender ID="txtDateFromCal" runat="server" TargetControlID="txtDateFrom" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Date To</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtDateto"/>
                            <ajaxToolkit:CalendarExtender ID="txtDatetoCal" runat="server" TargetControlID="txtDateto" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Script Scrutiny Person Name</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtpersonName"/>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Script Scrutiny Date</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtSurintDate"/>
                            <ajaxToolkit:CalendarExtender ID="txtSurintDateCal" runat="server" TargetControlID="txtSurintDate" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Script Scrutiny Fee</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtFeePaid"/>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Monitoring Name</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtMoniName"/>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Monitoring Contact No</label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtMoniContact"/>
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
                 <asp:GridView ID="grdDrama" runat="server" CssClass="table table-responsive-sm" DataKeyNames="DrID" OnSelectedIndexChanged="grddrama_SelectedIndexChanged"
                  AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grddrama_PageIndexChanging" OnRowDataBound="grddrama_RowDataBound"
                  EmptyDataText="No Drama define" Width="100%">
                  <Columns>
                      <asp:BoundField DataField="thName" HeaderText="Theatre Name" />
                      <asp:BoundField DataField="title" HeaderText="Drama Title" />
                      <asp:BoundField DataField="DateFrom" HeaderText="Date From" />
                      <asp:BoundField DataField="DateTo" HeaderText="Date To" />
                      <asp:BoundField DataField="ScScruName" HeaderText="Person Name"/>
                      <asp:BoundField DataField="ScScruDate" HeaderText="Script Scrutiny Date"/>
                      <asp:BoundField DataField="ScScruFee" HeaderText="Script Scrutiny Fee"/>
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
