<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="WetBlueTrnsfrHome.aspx.cs" Inherits="RMS.WetBlueTrnsfrHome" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function promptFunc() {
        return confirm("Are your sure that you want to cancel this note?");
    }
    function promptFunc1() {
        return confirm("Are your sure that you want to approve this note?");
    }
</script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
    ValidationGroup="fltr"/>
  <uc1:Messages ID="ucMessage" runat="server" />
  <fieldset class="fieldSet">
  <legend >
    Search
  </legend>
 
   <table width="70%" cellspacing="0" class="stats1" align="left">
  <tr>
  <td>
  <asp:Label ID="lblFromDt" runat="server"  Text="From:">
  </asp:Label>
  </td>
  <td>
  <asp:TextBox ID="txtfromDt" runat="server" Width="100px" ></asp:TextBox>
    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtfromDt" EnableViewState="false">
    </ajaxToolkit:CalendarExtender>
  </td>
  <td>
  <asp:Label ID="lblToDt" runat="server" Text="To:">
  </asp:Label>
  </td>
  <td>
  <asp:TextBox ID="txttoDt" runat="server" Width="100px"></asp:TextBox>
    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txttoDt" EnableViewState="false">
    </ajaxToolkit:CalendarExtender>
  </td>
  <td>
  <asp:Label ID="lblStatus" runat="server" Text="Status:">
  </asp:Label>
  </td>
  
  <td>
  <asp:DropDownList ID="ddlStatus" runat="server" Width="80px">
    <asp:ListItem Text="All" Selected="True" Value="M" ></asp:ListItem>
    <asp:ListItem Text="Approved" Value="A"></asp:ListItem>
    <asp:ListItem Text="Pending" Value="P"></asp:ListItem>
    <asp:ListItem Text="Cancelled" Value="C"></asp:ListItem>
    </asp:DropDownList>
   <asp:ImageButton ID="imgBtnSearch" runat="server" ToolTip="Search Note" OnClick="btnSearch_Click" ImageUrl="../images/search_icon.png"/>
  
  </td>
  </tr>

  </table> 
  
  
  <br />
  </fieldset>


    
 <div>
 <br />
 
 
 <%--<asp:Button ID="btnCreate"  runat="server" OnClick="btnCreate_Click" Text="Create Note"/>--%>
 <asp:ImageButton ID="btnCreate" runat ="server" OnCommand="btnCreate_Click" ImageUrl="~/images/btn_generate.png"
    onMouseOver="this.src='../images/btn_generate_m.png'" onMouseOut="this.src='../images/btn_generate.png'" />
 
 </div>
 <br />
 <br />
  <p>
         
        
        <asp:GridView ID="grdIGP" DataKeyNames="vr_no" runat="server"
         AutoGenerateColumns="False" OnRowDataBound="grdIGP_RowDataBound" AllowPaging="true" 
        Width="100%" PageSize="20" OnPageIndexChanging="grdIGP_PageIndexChanging" >
        <HeaderStyle CssClass ="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
            <Columns>
                <asp:BoundField DataField="vr_no" HeaderText="Note No" />
                <asp:BoundField DataField="vr_dt" DataFormatString= "{0:dd-MMM-yy}" htmlencode="false" HeaderText="Note Date" />
                <asp:BoundField DataField="product" HeaderText="Product" />
                <asp:BoundField DataField="design" HeaderText="Design" />
                <asp:BoundField DataField="color" HeaderText="Color" />
                <asp:BoundField DataField="thick" HeaderText="Thickness" />
                
                <asp:BoundField DataField="fromLoc" HeaderText="From Location" Visible="false"/>
                <asp:BoundField DataField="toLoc" HeaderText="To Location" Visible="false"/>
                <asp:BoundField DataField="status" HeaderText="Status" />
                <asp:TemplateField HeaderText="Options"  HeaderStyle-Width="150px">
                
                    <ItemTemplate>
                        <asp:Label ID="lbl" runat="server"  Visible="false" Text='<%# Bind("toLoc") %>'>'></asp:Label>
                        <asp:Label ID="Label1" runat="server"  Visible="false" Text='<%# Bind("fromLoc") %>'>'></asp:Label>
                         <asp:Label ID="lblstatus" runat="server"  Visible="false" Text='<%# Bind("status") %>'>'></asp:Label>
                         <asp:Label ID="lblvr_no" runat="server"  Visible="false" Text='<%# Bind("vr_no") %>'>'></asp:Label>
                         <asp:Label ID="lblvrdt" runat="server"  Visible="false" Text='<%# Bind("vr_dt") %>'>'></asp:Label>
                         
                         <asp:LinkButton ID="LinkApprove" runat="server"  Text="Approve" OnClick="lnkApprove_Click" OnClientClick="return promptFunc1()"></asp:LinkButton>
                         <asp:LinkButton ID="lnkView" runat="server"  Text="View" OnClick="lnkView_Click"></asp:LinkButton>
                         <asp:LinkButton ID="lnkEdit" runat="server"  Text="Edit" OnClick="lnkEdit_Click"></asp:LinkButton>
                         <asp:LinkButton ID="lnkCancel" runat="server"  Text="Cancel" OnClick="lnkCancel_Click" OnClientClick="return promptFunc()"  CssClass="promptDelete"></asp:LinkButton>
                         <%--<asp:LinkButton ID="lnkPrint" runat="server"  Text="Print"></asp:LinkButton>--%>
                         
                    </ItemTemplate>
                    
                     <ItemStyle />
                            <ControlStyle CssClass="lnk"></ControlStyle>
                            
                </asp:TemplateField>
               
                
            </Columns>
        
        </asp:GridView>
    </p>
</asp:Content>
