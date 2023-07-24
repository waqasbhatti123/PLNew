<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"
    AutoEventWireup="true" CodeBehind="frmGLCOA.aspx.cs" Inherits="RMS.GLSetup.frmGLCOA"
    Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4 ">
                <div class="card-body">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>GL Type*</label>
                                <asp:DropDownList ID="ddlgltype" runat="server" AppendDataBoundItems="True" CssClass="form-control"/>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Button ID="btnGenerat" runat="server" Text="Report" CssClass="btn btn-primary" OnClick="btnGenerat_Click" />
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>
                        </div>
                    </div>
                        <br />
    <asp:Panel ID="pnlMain" runat="server" Width="99%" Height="600">
        <rsweb:ReportViewer ID="viewer" runat="server" Width="100%" Height="580px">
        </rsweb:ReportViewer>
    </asp:Panel>

                </div>
            </div>
        </div>
    </div>
</asp:Content>

