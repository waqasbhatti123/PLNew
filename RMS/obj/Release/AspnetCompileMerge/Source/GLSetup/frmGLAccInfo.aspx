<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"
 AutoEventWireup="true" CodeBehind="frmGLAccInfo.aspx.cs" Inherits="RMS.GLSetup.frmGLAccInfo"
 Culture="auto" UICulture="auto" %>
<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %><%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %><asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4 ">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>

                        <br />
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                             <asp:label id="Label1" text="Branch*" runat="server" />
                                &nbsp;
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged"
                                AppendDataBoundItems="True" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Branch</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                    
                                <asp:label id="lblgltype" text="GL Type*" runat="server" />
                                &nbsp;
    <asp:dropdownlist id="ddlgltype" runat="server" CssClass="form-control" appenddatabounditems="True" />
                                &nbsp;

                                <%--<asp:ImageButton ID="btnGenerat" runat="server"  ImageUrl="~/images/btn_generate.png"  
       OnClick="btnGenerat_Click"
       onMouseOver="this.src='../images/btn_generate_m.png'" 
       onMouseOut="this.src='../images/btn_generate.png'" />
     &nbsp;--%>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4"></div>
                    </div>
  
        <asp:button id="btnGenerat" runat="server" text="Report" onclick="btnGenerat_Click" CssClass="btn btn-primary" />
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
