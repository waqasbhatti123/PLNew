<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="LoanReport.aspx.cs" Inherits="RMS.Setup.LoanReport" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" 
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main"/>
    <uc1:Messages ID="ucMessage" runat="server" />
    
     
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
           
           
           
          <tr>
      
            <td width="3%"></td>
            <td valign="top">
             <br />
             <div runat="server" visible="false">
             </div>
             <table cellspacing="0" cellpadding="0">
             
             <tr>
             <td>
            <asp:Label ID="cpnyid" runat="server" Text="Company Name:"></asp:Label>
           <br />
            <br />
            <br />
            
             </td>
              
              <td>
           <asp:Label ID="cpnyidv" runat="server" Text="Company Name"></asp:Label>
          <br />
            <br />
            <br />
            
          </td>
             </tr>
             
             
             <tr>
             <td>
          <asp:Label ID="regid" runat="server" Text="Region" > </asp:Label>
             
             </td>
             
             <td>
              <asp:Label ID="regidv" runat="server" Text="Region" > </asp:Label>
             </td>
            
             </tr>
             
             <tr>
              <td>
                 
                 <asp:Label ID="divid" runat="server" Text="Division"></asp:Label>
                 <br />
          </td>
          
          <td>
           <asp:Label ID="dividv" runat="server" Text="Division"></asp:Label>
          
          </td>
          
             </tr>
             
             </table>
             <asp:GridView ID="grdlev" runat="server" DataKeyNames="EmpID" 
                    AutoGenerateColumns="False" AllowPaging="true" OnPageIndexChanging="grdEmps_PageIndexChanging"
                     OnRowDataBound="grdEmps_RowDataBound"
                    EmptyDataText="There is no Report" Width="760px">
                   
                   <Columns>
                     
                       <asp:TemplateField HeaderText="Sr.#">
                    <ItemTemplate>
                        <asp:Label ID="lblSerial" runat="server"></asp:Label>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                   
                   </asp:TemplateField> 
                        <asp:BoundField DataField="name" HeaderText="Serial Employee No/Name" />
                        <asp:BoundField DataField="dept" HeaderText="Department" />
                        <asp:BoundField DataField="desig" HeaderText="Designation" />
                        <asp:BoundField DataField="loantype" HeaderText="Loan Type" />
                        <asp:BoundField DataField="paymentref" HeaderText="Voucher Refrence" />
                        <asp:BoundField DataField="dat" HeaderText="Loan Date" ><ItemStyle Width="70px" /></asp:BoundField > 
                        <asp:BoundField DataField="lonamt" HeaderText="Amount Paid" ><ItemStyle HorizontalAlign="Right"  /></asp:BoundField >
                        <asp:BoundField DataField="nos" HeaderText="Inst NOS" ><ItemStyle HorizontalAlign="Center"  /></asp:BoundField >
                        <asp:BoundField DataField="instamt" HeaderText="Inst Amount" ><ItemStyle HorizontalAlign="Right"  /></asp:BoundField >
                       <%-- <asp:BoundField DataField="regi1" HeaderText="Last Payment Date" />                      
                       <asp:BoundField  DataField="regi2" HeaderText="Last Payment Amount" />
                       <asp:BoundField  DataField="regi3" HeaderText="Unpaid Balance" />
                       <asp:BoundField  DataField="regi4" HeaderText="Current Due" />
                      --%>
                    </Columns>
                    <HeaderStyle CssClass="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                </asp:GridView>
             
            </td>
            <td width="3%"></td>
          </tr>
        </table>
        
</asp:Content>
