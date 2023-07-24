<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmailMgt.aspx.cs" Inherits="RMS.inquiry.EmailMgt"%>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main"/>
    <uc1:Messages ID="ucMessage" runat="server" />
    

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="3%"></td>
            <td height="10" colspan="2"></td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td width="40%" valign="top">
            
             
             <asp:GridView ID="grdDesignations" runat="server" DataKeyNames="MailID" OnSelectedIndexChanged="grdDesignations_SelectedIndexChanged" 
                    AutoGenerateColumns="False"  AllowPaging="false" PageSize="20" Width="98%" OnPageIndexChanging="grdDesignations_PageIndexChanging" OnRowDataBound="grdDesignations_RowDataBound">
                    <HeaderStyle CssClass ="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                    <Columns>
                        
                        <asp:BoundField DataField="MailFrom" HeaderText="Mail From"/>
                        <asp:BoundField DataField="MailHead" HeaderText="Mail Head"/>
                        <asp:BoundField DataField="MailAddress" HeaderText="Mail Address"/>
                        <asp:BoundField DataField="Status" HeaderText="Status"/>
                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                            <ItemStyle />
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
				
             
             
            
            </td>
            <td width="60%" valign="top">
             
                
             <table class="stats1" align="left">
             <asp:Panel runat="server" ID="pnlMain">
              <tr>
                <td colspan="2" valign="top" class="bg_input_area"></td>
              </tr>
              <tr style="display:none">
                <td>
                   <asp:Label ID="lblName" runat="server" Text="Designation Name:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDesignation" CssClass="RequiredField" runat="server" MaxLength="80"></asp:TextBox>
                   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDesignation"
                        ErrorMessage="Please enter designation name" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>--%>
                </td>
                
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
             <tr>
                <td>
                   <asp:Label ID="Label1" runat="server" Text="Mail From:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMailFrom" CssClass="RequiredField" runat="server" MaxLength="100"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMailFrom"
                        ErrorMessage="Please enter Mail From" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                </td>
                
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
             <tr>
                <td>
                   <asp:Label ID="Label2" runat="server" Text="Mail Head:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMailHead" CssClass="RequiredField" runat="server" MaxLength="500"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtMailHead"
                        ErrorMessage="Please enter Mail Head" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                </td>
                
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
             <tr>
                <td>
                   <asp:Label ID="Label3" runat="server" Text="Mail Address:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMailAddress" CssClass="RequiredField" runat="server" MaxLength="100"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtMailAddress"
                        ErrorMessage="Please enter Mail Address" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2"
              runat="server" ErrorMessage="Please Enter Valid Email ID"
                  ValidationGroup="main" ControlToValidate="txtMailAddress"
                  CssClass="requiredFieldValidateStyle"
                  ForeColor="Red"
                  ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                  </asp:RegularExpressionValidator>
                </td>
                
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
             <tr>
                <td>
                   <asp:Label ID="Label4" runat="server" Text="Mail Password:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMailPswd" CssClass="RequiredField" runat="server" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtMailPswd"
                        ErrorMessage="Please enter Password" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                </td>
                
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
             <tr>
                <td>
                   <asp:Label ID="Label5" runat="server" Text="Port:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMailPort" CssClass="RequiredField" runat="server" MaxLength="4"></asp:TextBox>
                    <asp:RangeValidator ID="rangeValidator5" runat="server" ControlToValidate="txtMailPort" MaximumValue="9999" MinimumValue="0"
     ValidationGroup="main" ForeColor="Red" ErrorMessage="Port No is Out of Range" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtMailPort"
                        ErrorMessage="Please enter Port" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                </td>
                
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
             <tr>
                <td>
                   <asp:Label ID="Label6" runat="server" Text="Host:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMailHost" CssClass="RequiredField" runat="server" MaxLength="80"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtMailHost"
                        ErrorMessage="Please enter host" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                </td>
                
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
             <tr>
                <td>
                   <asp:Label ID="Label7" runat="server" Text="Enable SSL:"></asp:Label>
                </td>
                <td>
                     <asp:RadioButtonList ID="rblSSL" runat="server" 
                        RepeatDirection="Horizontal" Width="80%">
                        <asp:ListItem Value="1" Selected="True">Enable</asp:ListItem>
                        <asp:ListItem Value="0">Disable</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                
            </tr>
            
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
           <tr>
                <td>
                    <asp:Label ID="lblEnable" runat="server" Text="Status:"></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblStatus" runat="server" 
                        RepeatDirection="Horizontal" Width="80%">
                        <asp:ListItem Value="1" Selected="True">Enable</asp:ListItem>
                        <asp:ListItem Value="0">Disable</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>

            </asp:Panel>  
          
          <tr>
            <td valign="top">&nbsp;</td>
            <td valign="top">
        
            <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
            <!--img src="images/btn_new.jpg" width="60" height="20" /> <img src="images/btn_edit.jpg" width="60" height="20" /> <img src="images/btn_delete.jpg" alt="" width="60" height="20" /> <img src="images/btn_save.jpg" width="60" height="20" /> <img src="images/btn_cancel.jpg" width="60" height="20" /-->
            </td>
            
          </tr>
          
        </table>
        
        </td>
      </tr>
    </table>
        
</asp:Content>
