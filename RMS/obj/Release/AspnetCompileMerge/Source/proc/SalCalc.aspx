<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="SalCalc.aspx.cs" Inherits="RMS.Setup.SalCalc" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-sm-8">
                            <asp:Label ID="lblMonthSal" runat="server" Font-Bold="True" Text="Salary Calculation for month "></asp:Label>
                            <asp:Label ID="lblMonthSalAgain" runat="server" Font-Bold="True" Text="Salary is already calculated for month " Visible="false"></asp:Label>
                        </div>
                    </div>
                    &nbsp;
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button ID="btnUploadEmpData" runat="server" 
                            Text="&nbsp;&nbsp;OK&nbsp;&nbsp;" OnCommand="ButtonCommand"
                       CssClass="buttonCancel btn btn-primary" CommandName="EmpData" 
                         />
                       
                       <asp:Button ID="btnCancel" runat="server"  Text=" Cancel " OnCommand="ButtonCommand"
                       CssClass="buttonCancel btn btn-success" CommandName="Cancel" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    
    <table>
        <tr>
            <td>
               
            </td>
        </tr>
        <tr>
            <td>
            
            </td>
        </tr>
       
       
        <tr>
            <td>
                <table cellpadding="2" cellspacing="2">
                <tr>
                    <td>  </td>
                    <td>&nbsp;
                        <b> </b>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        
                    </td>
                    <td>
                        
                    </td>
                </tr>
                </table>

            </td>
        </tr>
    </table>

</asp:Content>
