<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="Seepicture.aspx.cs"  Inherits="RMS.profile.Seepicture"
   Culture="auto" UICulture="auto" EnableEventValidation="true" %>


<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>
    <script>
        <%--$(function () {
        var brID = '<%=Session["BranchID"].ToString() %>';
        $(".searchbranchchange").val(brID);
        })--%>
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
            <div class="row">
                <div class="col-gl-12 col-md-12 col-sm-12">
                    <div class="card card-shadow mb-4">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <asp:Image ImageUrl="" ID="ImgGet" runat="server" Height="700" Width="900" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>