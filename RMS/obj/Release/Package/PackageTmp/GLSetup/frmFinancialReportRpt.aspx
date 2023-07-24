<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"  AutoEventWireup="true"
 CodeBehind="frmFinancialReportRpt.aspx.cs" Culture="auto" UICulture="auto" 
 EnableEventValidation="true"Inherits="RMS.GL.Setup.frmFinancialReportRpt" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">



        

   
        $(document).ready(function() {
            $(".classOnlyInt").keydown(function(event) {
                if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                    event.preventDefault();
                }
            });

            
            
            
        });
</script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    
   
    <uc1:Messages ID="ucMessage" runat="server" />
    <fieldset class="fieldSet">
    <legend></legend>
    <div>
    <table>
        <tr>
            <td><asp:Label ID="lblReportNo" runat="server" Text="Report No:" Visible="false"></asp:Label></td>
            <td><asp:TextBox ID="txtReportNo" runat="server" Width="100px" class="classOnlyInt" Visible="false"></asp:TextBox></td>
            
           
            <td><asp:Label ID="lblNoteNo" runat="server" Text="Note No:" Visible="false"></asp:Label></td>
            <td><asp:TextBox ID="txtNoteNo" runat="server" class="classOnlyInt" MaxLength="5" Width="100px" Visible="false"></asp:TextBox></td>
            <td><asp:Label ID="lblDate" runat="server" Text="To Date: " Width="50px"></asp:Label></td>
            <td><asp:TextBox ID="txtDate" runat="server" Width="70px"></asp:TextBox></td>
            
            <ajaxToolkit:CalendarExtender ID="ClExt1" runat="server" TargetControlID="txtDate" EnableViewState="false"></ajaxToolkit:CalendarExtender>
            &nbsp;&nbsp;
            <td><asp:Label ID="Label1" runat="server" Text="Print For the Month: " Width="110px"></asp:Label></td>
            <td>
                <asp:DropDownList ID="ddlPrint" runat="server">
                    <asp:ListItem Value="Y" >Yes</asp:ListItem>
                    <asp:ListItem Value="N" Selected="True">No</asp:ListItem>
                </asp:DropDownList>
            </td>
            &nbsp;&nbsp;
            <td><asp:Label ID="Label2" runat="server" Text="Status: " Width="60px"></asp:Label></td>
            <td>
                <asp:DropDownList ID="ddlStatus" runat="server">
                    <asp:ListItem Value="P" >Pending</asp:ListItem>
                    <asp:ListItem Value="A" Selected="True">Approved</asp:ListItem>
                    <asp:ListItem Value=" " >All</asp:ListItem>
                </asp:DropDownList>
            </td>
             <td>
              Export To:&nbsp;
                <asp:DropDownList ID ="ddlExtension" runat="server">
                    <asp:ListItem Value="PDF" Selected="True">PDF</asp:ListItem>
                    <asp:ListItem Value="Excel">Excel</asp:ListItem>
                </asp:DropDownList>
              </td>
        </tr>
        
        <tr>
            <td><asp:Label ID="lblName" runat="server" Text="Name:" Visible="false"></asp:Label></td>
            <td colspan="5"><asp:TextBox ID="txtName" runat="server" Width="400px" MaxLength="50" Visible="false"></asp:TextBox></td>
           
            <td><asp:LinkButton ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" Visible="false"></asp:LinkButton></td>
           
        </tr>
        
    </table>
    
    </div>
    </fieldset>
    
  <%-- <asp:ImageButton ID="btnGenerate" runat="server"  ImageUrl="~/images/btn_generate.png"
            onMouseOver="this.src='../images/btn_generate_m.png'" onMouseOut="this.src='../images/btn_generate.png'" OnClick="btnGenerate_Click"  />
                    --%>    
    
    
    <fieldset class="fieldSet">
    <legend></legend>
  
    <div>
        <asp:GridView ID="grdView" runat="server"  AutoGenerateColumns="false" DataKeyNames="ReportNo" Width="99%" OnRowDataBound="grdView_RowDataBound">
        <HeaderStyle CssClass ="grid_hdr" />
        <RowStyle CssClass="grid_row" />
        <AlternatingRowStyle CssClass="gridAlternateRow" />
        <SelectedRowStyle CssClass="gridSelectedRow" />
        
        <Columns>
        <asp:BoundField HeaderText="Report No" DataField="ReportNo"  ItemStyle-Width="100px"/>
        <asp:BoundField HeaderText="Report Name" DataField="ReportName" ItemStyle-Width="300px"/>
        <asp:BoundField HeaderText="Note No" DataField="NoteNo" ItemStyle-Width="100px"/>
       
       <asp:TemplateField >
        <ItemTemplate>
          <%--  <asp:LinkButton ID="lnkEdit" Text="Edit" runat="server" OnClick="lnkEdit_Click"></asp:LinkButton>
            <asp:LinkButton ID="lnkView" Text="View" runat="server" OnClick="lnkView_Click"></asp:LinkButton>--%>
            <asp:LinkButton ID="lnkPrint" Text="Print" runat="server" OnClick="lnkPrint_Click"></asp:LinkButton>
        </ItemTemplate>
       </asp:TemplateField>
       
        </Columns>
        
        </asp:GridView>
    </div>

    </fieldset>
   


</asp:Content>
