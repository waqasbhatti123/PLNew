<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
Culture="auto" UICulture="auto" EnableEventValidation="false" 
CodeBehind="frmColorSetup.aspx.cs" Inherits="RMS.GL.Setup.frmColorSetup"  %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">

    $(document).ready(function() {

    });
    
       </script>
    
  <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
    
    <asp:UpdatePanel ID="UpnlTbox" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

<div style="width:50%; float:left; ">
    <table width="90%" border="0" cellspacing="0" cellpadding="0" >
        
              
             
              <tr>
                <td>
                    <asp:Label ID="lblColorCode" runat="server" Text="Color Code :"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtColorCode" runat="server" MaxLength="5" ></asp:TextBox>
                    
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="main" ErrorMessage="Please enter color code..." SetFocusOnError="true"
                    ControlToValidate="txtColorCode" Display="None" ></asp:RequiredFieldValidator>
                </td>
                
               </tr>
          
          
          <tr>
                <td>
                    <asp:Label ID="lblDescription" runat="server" Text="Description :"></asp:Label>
                </td>
          
                <td>
                    <asp:TextBox ID="txtDescription" runat="server" MaxLength="50"></asp:TextBox>
                    
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="main" ErrorMessage="Please enter description..." SetFocusOnError="true"
                    ControlToValidate="txtDescription" Display="None" ></asp:RequiredFieldValidator>
                </td>
          </tr>  
          
           <tr>
                <td>
                    <asp:Label ID="lblStatus" runat="server" Text="Status :"></asp:Label>
                </td>
          
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server" >
                    <asp:ListItem Text="Enable" Value="A"></asp:ListItem>
                    <asp:ListItem Text="Disable" Value="C"></asp:ListItem>
                    
                    </asp:DropDownList>
                </td>
          </tr>  
          <tr>
          <td>
          &nbsp
          </td>
          </tr>
          <tr>
        
         <%-- <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />--%>
        <td width="90px"> <asp:ImageButton ID="btnSave" runat="server" OnCommand="btnSave_Click" ImageUrl="~/images/btn_save.png"
            onMouseOver="this.src='../images/btn_save_m.png'" onMouseOut="this.src='../images/btn_save.png'" ValidationGroup="main" CausesValidation="true" />
          </td>
          <td><asp:ImageButton ID="btnClear" runat="server" OnCommand="btnClear_Click" ImageUrl="~/images/btn_clear.png" 
            onMouseOver="this.src='../images/btn_clear_m.png'" onMouseOut="this.src='../images/btn_clear.png'" /> </td>
          </tr>
                 
    </table>
     </div>       
     
   <%--</ContentTemplate>
   <Triggers>
   <asp:AsyncPostBackTrigger ControlID="ucButtons" />
   </Triggers>
        
   </asp:UpdatePanel> --%>   
   


   <%-- <div style="width:50%; float:left;">
    
    </div>--%>
  
     
   <%-- <asp:UpdatePanel ID="UpnlGrid"  runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
    
    
       
   <div style="width:50%; float:left;" >
    
   <asp:GridView ID="grdViewColor" runat="server" AutoGenerateColumns="false" OnRowDataBound ="grdViewColor_RowDataBound" Visible="true"
    Width="90%" OnSelectedIndexChanged="grdViewColor_SelectedIndexChanged" OnPageIndexChanging="grdViewColor_OnPageIndexChanging" PageSize="25" AllowPaging="true">
   <HeaderStyle CssClass="grid_hdr" />
   <RowStyle CssClass="grid_row" />
   <AlternatingRowStyle CssClass="gridAlternateRow" />
   <SelectedRowStyle CssClass ="gridSelectedRow" />
   
   <Columns>
   
   <asp:BoundField DataField="ColorCode" HeaderText="ColorCode"/>
   <asp:BoundField DataField="Description" HeaderText="Description"/>
   <asp:BoundField DataField="Status" HeaderText="Status" />
   <asp:CommandField ShowSelectButton="true" ControlStyle-CssClass="lnk"/>
   
   </Columns>
   
   </asp:GridView>
    </div>
    
</ContentTemplate>

    </asp:UpdatePanel>
    <br />

    
</asp:Content>
