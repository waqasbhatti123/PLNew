<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"
 AutoEventWireup="true" CodeBehind="Departments.aspx.cs" Inherits="RMS.Inv.Departments" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional">
<ContentTemplate>

    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main"/>
    <uc1:Messages ID="ucMessage" runat="server" />
   <div style="float:left; width:48%" >
   
   <table>
   <tr><td>
   
   <asp:Label ID="lblName" runat="server" Text="Department Name:"></asp:Label>
       </td>
       <td>         
       <asp:TextBox ID="txtName" CssClass="RequiredField" runat="server" MaxLength="150"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtName"
          ErrorMessage="Please enter department name" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
      </td>
      </tr>
   <tr><td>&nbsp</td></tr>
    <tr>
        <td colspan="2" valign="top">
        
            <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
                 </td>
    
    
    </tr>

   </table>
   </div>
</ContentTemplate>
<Triggers><asp:AsyncPostBackTrigger  ControlID="grdcc"/></Triggers>
</asp:UpdatePanel>
   
   <asp:UpdatePanel ID="up1" runat="server">
   <ContentTemplate>
   
   <div style="float:left; width:49%">
    <table  border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="3%"></td>
            <td height="10" colspan="2"></td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td width="34%" valign="top">
            
             
             <asp:GridView ID="grdcc" runat="server" DataKeyNames="DeptNme" OnSelectedIndexChanged="grdcc_SelectedIndexChanged" 
                    AutoGenerateColumns="False" AllowPaging="True" Width="100%" PageSize="20" OnPageIndexChanging="grdcc_PageIndexChanging" >
                    <HeaderStyle CssClass ="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                    <Columns>
                        <asp:BoundField DataField="DeptId" HeaderText="DeptID" />
                        <asp:BoundField DataField="DeptNme" HeaderText="Name" />
                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                            <ItemStyle />
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
                
	            </td>
            </tr>
    </table>
</div>

</ContentTemplate>
   <Triggers><asp:AsyncPostBackTrigger ControlID="ucButtons" /></Triggers>
   </asp:UpdatePanel>     
</asp:Content>
