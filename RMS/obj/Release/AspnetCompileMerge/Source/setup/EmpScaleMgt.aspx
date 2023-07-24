<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmpScaleMgt.aspx.cs" Inherits="RMS.setup.EmpScaleMgt" %>--%>


<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmpScaleMgt.aspx.cs" Inherits="RMS.setup.EmpScaleMgt"%>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
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
            <div class="col-lg-4 col-md-4 col-sm-4">
                <label>Scale Name *</label>
               <asp:TextBox ID="txtScaleName" runat="server" CssClass="form-control"></asp:TextBox>                                           
            </div>
            <div class="col-lg-4 col-md-4 col-sm-4">
                 <label>Description *</label>
               <asp:TextBox ID="txtScaleDescription" runat="server" CssClass="form-control"></asp:TextBox> 
            </div>
        </div>
        <div class="row">
            <div class="col-lg- col-md-4 col-sm-4">
                <label>Basic(Min)</label>
                <asp:TextBox runat="server" ID="txtBasicMin" CssClass="form-control" />
            </div>
            <div class="col-lg- col-md-4 col-sm-4">
                <label>Basic(Max)</label>
                <asp:TextBox runat="server" ID="txtBasicMax" CssClass="form-control" />
            </div>
            <div class="col-lg- col-md-4 col-sm-4">
                <label>Basic(Increment Rate)</label>
                <asp:TextBox runat="server" ID="txtIncre" CssClass="form-control" />
            </div>
        </div>

<br />
        <div class="row btnDiv">
            <div class="col-lg-4 col-md-4 col-sm-4">
                <asp:Button ID="Save" runat="server" OnClick="Button_Command" Text="Save" class="btn btn-primary" />
                <asp:Button ID="Clear" runat="server" OnClick="Clear_All" Text="Clear" class="btn btn-danger" />

            </div>
        </div>
<br />

        <asp:GridView ID="grdEmpScale" runat="server" CssClass="table table-responsive-sm table-bordered" DataKeyNames="ScaleID" OnSelectedIndexChanged="grdEmpScale_PageIndexChanged"
            AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdEmpScale_PageIndexChanging"
            EmptyDataText="There is no Scale defined" Width="100%">
            <Columns>

                <asp:BoundField DataField="ScaleName" HeaderText="Name" />
                <asp:BoundField DataField="ScaleDescription" HeaderText="Description" />
                <asp:BoundField DataField="Minimum" HeaderText="Basic(Min)" />
                <asp:BoundField DataField="Maxminmum" HeaderText="Basic(Max)" />
                <asp:BoundField DataField="IncRate" HeaderText="Increment" />

                <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                    <ControlStyle CssClass="lnk"></ControlStyle>
                </asp:CommandField>


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
    
    
    
    
   
</asp:Content>




