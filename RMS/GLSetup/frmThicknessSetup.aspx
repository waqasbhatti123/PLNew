<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
Culture="auto" UICulture="auto" EnableEventValidation="false" 
CodeBehind="frmThicknessSetup.aspx.cs" Inherits="RMS.GL.Setup.frmThicknessSetup"  %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">

    $(document).ready(function() {
    $(".classOnlyInt").keydown(function(event) {
        if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
            event.preventDefault();
        }
    });
 $(".classOnlyDeci").keydown(function(event) {
    if (event.keyCode != 110 && event.keyCode != 190 && (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
        event.preventDefault();
    }
    var txtCrdt = $(this).val();
    if (event.keyCode == 110 || event.keyCode == 190) {
        if ((txtCrdt.split(".").length) > 1) {
            event.preventDefault();
        }
    }

});
    
    
    
    
    });
    
       </script>
    
  <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
    
    <%--<asp:UpdatePanel ID="Upnl" runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
    
    <div style="width:55%; float:left;">
    <table width="98%" border="0" cellspacing="0" cellpadding="0" >
        
              
              <%--<tr>
                <td>
                    <asp:Label ID="lblItemCode" runat="server" Text="Item Code :"></asp:Label>
                </td>
                <td>
                     <asp:DropDownList runat="server" ID="ddlItemCode" OnSelectedIndexChanged="ddlItemCode_SelectedIndexChanged">
                     <asp:ListItem Text="--- Select Item Code ---" Value="0"></asp:ListItem>
                     </asp:DropDownList>
                </td>
                
               </tr>--%>
                <tr>
                <td>
                    <asp:Label ID="lblThicknessCode" runat="server" Text="Thickness Code :"></asp:Label>
                </td>
            
                <td>
                    <asp:TextBox ID="txtThicknessCode" runat="server" CssClass="classOnlyInt" Width="60px" MaxLength="4"></asp:TextBox>
                    
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="main" ErrorMessage="Please enter thickness code..." SetFocusOnError="true"
                    ControlToValidate="txtThicknessCode" Display="None" ></asp:RequiredFieldValidator>
                </td>
          
          </tr>
          
          <tr>
                <td>
                    <asp:Label ID="lblThickness" runat="server" Text="Thickness :"></asp:Label>
                </td>
            
                <td>
                    <asp:TextBox ID="txtThickness" runat="server"  CssClass="classOnlyDeci"  Width="60px" MaxLength="8"></asp:TextBox>
                    
                    <asp:RequiredFieldValidator runat="server" ID="rfv1" ValidationGroup="main" ErrorMessage="Please enter thickness..." SetFocusOnError="true"
                    ControlToValidate="txtThickness" Display="None" ></asp:RequiredFieldValidator>
                    <asp:RangeValidator runat="server" ID="rv1" ValidationGroup="main" ErrorMessage="Thickness out of range..." SetFocusOnError="true"
                     ControlToValidate="txtThickness" Display="None" MinimumValue="00000.00" MaximumValue="99999.99" Type="Double" ></asp:RangeValidator>
                </td>
          
          </tr>
          
          <tr>
                <td>
                    <asp:Label ID="lblDescription" runat="server" Text="Description :"></asp:Label>
                </td>
          
                <td>
                    <asp:TextBox ID="txtDescription" runat="server" MaxLength="50"></asp:TextBox>
                    
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="main" ErrorMessage="Please enter description..." SetFocusOnError="true"
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
                <%--<td colspan="2" valign="top">
                    <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
                </td>--%>
                <td width="90px"> <asp:ImageButton ID="btnSave" runat="server" OnCommand="btnSave_click" ImageUrl="~/images/btn_save.png"
                 onMouseOver="this.src='../images/btn_save_m.png'" onMouseOut="this.src='../images/btn_save.png'" ValidationGroup="main" CausesValidation="true" /></td>
                
                <td> <asp:ImageButton ID="btnClear" runat="server" OnCommand="btnClear_click" ImageUrl="~/images/btn_clear.png"
                    onMouseOver="this.src='../images/btn_clear_m.png'" onMouseOut="this.src='../images/btn_clear.png'"/></td>
            </tr>

       
    
    
    </table>
    
    </div>
  
  <div style="width:40%; float:left;">
  <asp:GridView ID="grdViewItemD" runat="server" Visible="true" AutoGenerateColumns="false" OnRowDataBound="grdViewItemD_RowDataBound"
                OnSelectedIndexChanged="grdViewItemD_SelectedIndexChanged" Width="98%">
   <HeaderStyle CssClass="grid_hdr" />
   <RowStyle CssClass="grid_row" />
   <AlternatingRowStyle CssClass="gridAlternateRow" />
   <SelectedRowStyle CssClass="gridSelectedRow" />
   
   <Columns>
   
   <asp:BoundField DataField="ThicknessCode" HeaderText="ThickCode" />
   <asp:BoundField DataField="Thickness" HeaderText="Thickness" />
    <asp:BoundField DataField ="Description" HeaderText="Description" />

   <asp:BoundField DataField="Status" HeaderText="Status" />
   <asp:CommandField ShowSelectButton="true" ControlStyle-CssClass="lnk" />
   
   
   
   
   </Columns>
   
   
   
   </asp:GridView>
    </div>

    <%--</ContentTemplate>
    </asp:UpdatePanel>
--%>

</asp:Content>
