<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"
    AutoEventWireup="true" CodeBehind="InterComminication.aspx.cs" Inherits="RMS.profile.InterComminication"
    Culture="auto" UICulture="auto" EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../Scripts/jquery-1.10.1.min.js"></script>
    <script>
        $(document).ready(function () {
            
            $(".fileup").change(function () {
                debugger
                if (this.files && this.files[0]) {
                    var filereader = new FileReader();
                    filereader.readAsDataURL(this.files[0]);
                    filereader.onload = function (a) {
                        $(".fileimg").attr('src', a.target.result);
                    }
                }
            });
            $(".fileuprep").change(function () {
                debugger
                if (this.files && this.files[0]) {
                    var filereader = new FileReader();
                    filereader.readAsDataURL(this.files[0]);
                    filereader.onload = function (a) {
                        $(".fileimgrep").attr('src', a.target.result);
                    }
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
                        <div class="col-lg-12 col-md-12 col-sm-12 bg-primary" style="text-align: center; height: 40px; padding: 10px;">
                            <h2 style="color: white !important">Request</h2>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-md-4 col-sm-4">
                            <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                                ValidationGroup="main" />
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Select Request Type*</label>
                            <asp:DropDownList runat="server" ID="ddlDocType" CssClass="form-control" AppendDataBoundItems="true">
                                <asp:ListItem Value="0">Select Doc. Type</asp:ListItem>
                                <asp:ListItem Value="Appointment of daily wages/staff">Appointment of daily wages/staff</asp:ListItem>
                                <asp:ListItem Value="CP Fund Advance">CP Fund Advance</asp:ListItem>
                                <asp:ListItem Value="NOC">NOC</asp:ListItem>
                                <asp:ListItem Value="Experience Certificate">Experience Certificate</asp:ListItem>
                                <asp:ListItem Value="Permotion">Permotion</asp:ListItem>
                                <asp:ListItem Value="Time Scale">Time Scale</asp:ListItem>
                                <asp:ListItem Value="Upgration">Upgration</asp:ListItem>
                                <asp:ListItem Value="Others">Others</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4 fromdivision">
                            <label>From*</label>
                            <asp:DropDownList runat="server" CssClass="form-control" AppendDataBoundItems="true" ID="ddlFromdivision">
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>To*</label>
                            <asp:DropDownList runat="server" CssClass="form-control" AppendDataBoundItems="true" ID="ddldivision">
                                <asp:ListItem Value="0">Select Division</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Submitted By*</label>
                            <asp:TextBox runat="server" ID="txtsubmitted" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Submitted Dated*</label>
                            <asp:TextBox runat="server" ID="txtDate" CssClass="form-control"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtDateCal" runat="server" TargetControlID="txtDate" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </div>

                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <label>Remarks</label>
                            <asp:TextBox runat="server" ID="txtRemarks" CssClass="form-control" TextMode="MultiLine" onkeyup="LimitText(this,5000);" onblur="LimitText(this,5000);" Height="100px"></asp:TextBox>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:FileUpload ID="fileUploader" CssClass="fileup" runat="server" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Image ImageUrl="" runat="server" CssClass="fileimg" Width="70px" Height="70px" ID="imageurl" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 bg-primary" runat="server" id="ReplyLable" style="text-align: center; height: 40px; padding: 10px;">
                            <h2 style="color: white !important">Reply</h2>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4" runat="server" id="ReplyDatee" > 
                            <label>Reply Date*</label>
                            <asp:TextBox runat="server" ID="ReplyDate" CssClass="form-control"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="ReplyDateCal" runat="server" TargetControlID="ReplyDate" Enabled="True">
                            </ajaxToolkit:CalendarExtender>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4" runat="server" id="ReplyBied">
                            <label>Replied By*</label>
                            <asp:TextBox runat="server" ID="txtReplBy" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4" runat="server" id="ReplyStatus">
                            <label>Status*</label>
                            <asp:DropDownList runat="server" ID="ddlRepStatus" CssClass="form-control" AppendDataBoundItems="true">
                                <asp:ListItem Value="0">Select Status</asp:ListItem>
                                <asp:ListItem Value="Pending" Selected="True">Pending</asp:ListItem>
                                <asp:ListItem Value="InProgress">InProgress</asp:ListItem>
                                <asp:ListItem Value="Approved">Approved</asp:ListItem>
                                <asp:ListItem Value="NFA">NFA</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12" runat="server" id="ReplyRemarks">
                            <label>Remarks*</label>
                            <asp:TextBox runat="server" ID="txtReplRemarks" CssClass="form-control" TextMode="MultiLine" onkeyup="LimitText(this,5000);" onblur="LimitText(this,5000);" Height="100px"></asp:TextBox>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4" runat="server" id="ReplyAttach"> 
                            <asp:FileUpload ID="RepliFile" CssClass="fileuprep" runat="server" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4" runat="server" id="ReplyImage">
                            <asp:Image ImageUrl="" runat="server" CssClass="fileimgrep" Width="70px" Height="70px" ID="repimageFile" />
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button runat="server" ID="Button2" OnClick="btn_Save" class="btn btn-primary" Text="Save" ValidationGroup=""></asp:Button>
                            <asp:Button runat="server" ID="Button4" OnClick="btn_Clear" class="btn btn-danger" Text="Clear"></asp:Button>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:GridView ID="grdIntercom" runat="server" CssClass="table table-responsive-sm" DataKeyNames="InterID" OnSelectedIndexChanged="grdInter_SelectedIndexChanged"
                                AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="grdInter_PageIndexChanging" OnRowDataBound="grdInter_RowDataBound"
                                EmptyDataText="No Addtional Allowances / Deduction" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="Doctype" HeaderText="Request Type" />
                                    <asp:BoundField DataField="from_br_nme" HeaderText="From" />
                                    <asp:BoundField DataField="br_nme" HeaderText="To" />
                                    <asp:BoundField DataField="SubBy" HeaderText="Submitted by" />
                                    <asp:BoundField DataField="ComDate" HeaderText="Submitted Dated" />
                                    <asp:BoundField DataField="RepBy" HeaderText="Replied By" />
                                    <asp:BoundField DataField="RepDate" HeaderText="Reply Date" />
                                    <asp:BoundField DataField="RepStatus" HeaderText="Reply Status" />
                                    <asp:TemplateField HeaderText="Submitted Attachment">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkPrint" runat="server" Text="See Picture" ToolTip="Print Employee Education Record" CommandArgument='<%#Eval("Attachment")%>' OnClick="lnkEduPrint_Click" CssClass="lnk">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Replied Attachment">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkPrintRep" runat="server" Text="See Picture" ToolTip="Print Employee Education Record" CommandArgument='<%#Eval("RepAttach")%>' OnClick="lnkEduApp_Click" CssClass="lnk">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
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
