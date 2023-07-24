<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"  AutoEventWireup="true"
 CodeBehind="frmReportParameter.aspx.cs" Culture="auto" UICulture="auto" 
 EnableEventValidation="true"Inherits="RMS.GL.Setup.frmReportParameter" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" 
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

    function pageLoad() {
        $(".classOnlyInt").keydown(function(event) {
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }
        });

        $("table[id*=grdView] input[type=text][id*=txtRecNo]").blur(function () {
            var Rec = $(this).val(); //$("table[id*=grdView] input[type=text][id*=txtRecNo]").val();
            var count = 0;
            $("table[id*=grdView] input[type=text][id*=txtRecNo]").each(function () {
                var RecValue = $(this).val();
                if (Rec == RecValue) {
                    count++;
                }

            });
            if (count > 1) {
                alert("RecNo already taken");
                $(this).val("");
                $(this).focus();

            }
        });

        $("table[id*=grdView] input[type=text][id*=txtBold]").blur(function() {

            var fs = $(this).val();
            if (fs != "") {
                if (fs.toUpperCase() !== 'Y' && fs.toUpperCase() !== 'N') {
                    alert("Value Should be Y or N");
                    $(this).focus();
                }
            }
        });

        $("table[id*=grdView] input[type=text][id*=txtUnderLine]").blur(function() {

            var fs = $(this).val();
            if (fs != "") {
                if (fs.toUpperCase() !== 'Y' && fs.toUpperCase() !== 'N') {
                    alert("Value Should be Y or N");
                    $(this).focus();
                }
            }
        });

        $("table[id*=grdView] input[type=text][id*=txtOverLine]").blur(function() {

            var fs = $(this).val();
            if (fs != "") {
                if (fs.toUpperCase() !== 'Y' && fs.toUpperCase() !== 'N') {
                    alert("Value Should be Y or N");
                    $(this).focus();
                }
            }
        });

        //    });

    }

      
        
</script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
      <uc1:Messages ID="ucMessages1" runat="server" />
   
    <%--<asp:UpdatePanel ID="upDpnl" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>--%>
    <uc1:Messages ID="ucMessage" runat="server" />
    <fieldset class="fieldSet">
    <legend></legend>
    <div>
    <table>
        <tr>
            <td><asp:Label ID="lblReportNo" runat="server" Text="Report No:"></asp:Label></td>
            <td><asp:TextBox ID="txtReportNo" runat="server" Width="100px" class="classOnlyInt"></asp:TextBox></td>
            
            <asp:RequiredFieldValidator id="rqf" runat="server" ControlToValidate="txtReportNo" SetFocusOnError="true" ErrorMessage="Please enter Report No" Display="None" ValidationGroup="main"></asp:RequiredFieldValidator>
            <td><asp:Label ID="lblNoteNo" runat="server" Text="Note No:"></asp:Label></td>
            <td><asp:TextBox ID="txtNoteNo" runat="server" class="classOnlyInt" MaxLength="5" Width="100px"></asp:TextBox></td>
            <%--<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ControlToValidate="txtNoteNo" SetFocusOnError="true" ErrorMessage="Please enter Note No" Display="None" ValidationGroup="main"></asp:RequiredFieldValidator>--%>
            
            <%--<td><asp:Label ID="lblDate" runat="server" Text="Date:"></asp:Label></td>
            <td><asp:TextBox ID="txtDate" runat="server" Width="95px"></asp:TextBox></td>--%>
            <td></td>
            <td></td>
            <%--<ajaxToolkit:CalendarExtender ID="ClExt1" runat="server" TargetControlID="txtDate" EnableViewState="false"></ajaxToolkit:CalendarExtender>--%>
        </tr>
        
        <tr>
            <td><asp:Label ID="lblName" runat="server" Text="Name:"></asp:Label></td>
            <td colspan="5"><asp:TextBox ID="txtName" runat="server" Width="450px" MaxLength="50"></asp:TextBox></td>
            <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ControlToValidate="txtName" SetFocusOnError="true" ErrorMessage="Please enter Report Name" Display="None" ValidationGroup="main"></asp:RequiredFieldValidator>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td><asp:Label ID="lblCompLevelRpt" runat="server" Text="Company Level:"></asp:Label></td>
            <td><asp:DropDownList ID="ddlCompLevel" runat="server" Width="100px">
                <asp:ListItem Text="Select" Value="0" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                <asp:ListItem Text="No" Value="N"></asp:ListItem>
            </asp:DropDownList></td>
            <asp:RequiredFieldValidator ID="rq" runat="server" InitialValue="0" ControlToValidate="ddlCompLevel" Display="None" ErrorMessage="Select Company Level" SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>
            <td><asp:Label ID="lblPrintPreYr" runat="server" Text="Print Prv Yr:"></asp:Label> </td>
            <td><asp:DropDownList ID="ddlPrintPreYr" runat="server" Width="100px">
            <asp:ListItem Text="Select" Value="0" Selected="True"></asp:ListItem> 
            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem> 
            <asp:ListItem Text="No" Value="N"></asp:ListItem> 
            </asp:DropDownList></td>
            <asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ControlToValidate="ddlPrintPreYr" SetFocusOnError="true" ErrorMessage="Please select PrintPrvYr" Display="None" ValidationGroup="main" InitialValue="0"></asp:RequiredFieldValidator>
            <td><asp:Label ID="lblPrintDrCr" runat="server" Text="Print Dr/Cr:"></asp:Label></td>
            <td>
            <asp:DropDownList ID="ddlPrintDrCr" runat="server" Width="100px">
            <asp:ListItem Text="Select" Value="0" Selected="True"></asp:ListItem> 
            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem> 
            <asp:ListItem Text="No" Value="N"></asp:ListItem> 
            </asp:DropDownList>
            <asp:RequiredFieldValidator id="RequiredFieldValidator4" runat="server" ControlToValidate="ddlPrintDrCr" SetFocusOnError="true" ErrorMessage="Please select Print Dr/Cr" Display="None" ValidationGroup="main" InitialValue="0"></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
    </div>
    
    </fieldset>
    
    <fieldset class="fieldSet">
    <legend></legend>
    <div>
        <asp:GridView ID="grdView" runat="server" AutoGenerateColumns="false" GridLines="None" OnRowDataBound="grdView_RowDataBound">
        <HeaderStyle CssClass ="grid_hdr" />
        <RowStyle CssClass="grid_row" />
        <AlternatingRowStyle CssClass="gridAlternateRow" />
        <SelectedRowStyle CssClass="gridSelectedRow" />
        
        <Columns>
        
        <asp:TemplateField HeaderText="RecNo" HeaderStyle-Width="35px" >
            <ItemTemplate>
                <asp:TextBox ID="txtRecNo" runat="server" Text='<%#Eval("RecNo") %>' Width="35px" class="classOnlyInt" MaxLength="4"  ></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Type" HeaderStyle-Width="65px" >
            <ItemTemplate>
                <asp:DropDownList ID="ddlType" runat="server" Width="65px" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Text="Select" Value="0" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Heading" Value="H" ></asp:ListItem>
                    <asp:ListItem Text="Detail" Value="D" ></asp:ListItem>
                    <asp:ListItem Text="Series" Value="E" ></asp:ListItem>
                    <%--<asp:ListItem Text="Cost" Value="C" ></asp:ListItem>--%>
                    <asp:ListItem Text="Total" Value="T" ></asp:ListItem>
                    <%--<asp:ListItem Text="Skip" Value="S"></asp:ListItem>--%>
                </asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Narration" HeaderStyle-Width="150px" >
            <ItemTemplate>
                <asp:TextBox ID="txtNarration" runat="server" Width="150px" Text='<%#Eval("Narration") %>'></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Ac From" HeaderStyle-Width="75px">
            <ItemTemplate>
                <asp:TextBox ID="txtAcFrom" runat="server"  Width="75px" OnTextChanged="txtAcFrom_TextChanged" AutoPostBack="true" Text='<%#Eval("AcFrom") %>' ></asp:TextBox>
                
                  <ajaxToolkit:AutoCompleteExtender runat="server" ID="AutoComplete1" TargetControlID="txtAcFrom" ServiceMethod="getAcRange" ServicePath="~/InvSetupSupport/AutoCompleteSearch.asmx" MinimumPrefixLength="1"
                        CompletionSetCount="5" CompletionInterval="100" FirstRowSelected="true" EnableCaching="true" ></ajaxToolkit:AutoCompleteExtender>
                        <asp:HiddenField runat="server" ID="AcFr" />
                        
                        
            </ItemTemplate>
        </asp:TemplateField>
       
        <asp:TemplateField HeaderText="Ac To" HeaderStyle-Width="75px">
            <ItemTemplate>
                <asp:TextBox ID="txtAcTo" runat="server" Width="75px" OnTextChanged="txtAcTo_TextChanged" AutoPostBack="true" Text='<%#Eval("AcTo") %>'></asp:TextBox>
                 <ajaxToolkit:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1" TargetControlID="txtAcTo" ServiceMethod="getAcRange" ServicePath="~/InvSetupSupport/AutoCompleteSearch.asmx" MinimumPrefixLength="1"
                        CompletionSetCount="5" CompletionInterval="100" FirstRowSelected="true" EnableCaching="true"  ></ajaxToolkit:AutoCompleteExtender>
                        <asp:HiddenField runat="server" ID="AcTo" />
            </ItemTemplate>
        </asp:TemplateField>
        
         <asp:TemplateField HeaderText="Total Level" HeaderStyle-Width="35px">
            <ItemTemplate>
                <asp:TextBox ID="txtTotalLevel" runat="server" Width="35px" class="classOnlyInt" Text='<%#Eval("TotalLevel") %>' MaxLength="1"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>
       
         <asp:TemplateField HeaderText="Arith-Op" HeaderStyle-Width="65px">
            <ItemTemplate>
                <asp:DropDownList ID="ddlArithOp" runat="server" Width="65px" >
                    <asp:ListItem Text="Select" Value="0" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Add" Value="Add" ></asp:ListItem>
                    <asp:ListItem Text="Subtract" Value="Sub" ></asp:ListItem>
                    <asp:ListItem Text="Multiply" Value="Mul" ></asp:ListItem>
                    <asp:ListItem Text="Divide" Value="Div" ></asp:ListItem>
                </asp:DropDownList>
            </ItemTemplate>
        </asp:TemplateField>
        
         <asp:TemplateField HeaderText="Print Level" HeaderStyle-Width="35px">
            <ItemTemplate>
                <asp:TextBox ID="txtPrintLevel" runat="server" Width="35px" class="classOnlyInt" Text='<%#Eval("PrintLevel") %>' MaxLength="1"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>
  
         <asp:TemplateField HeaderText="Init" HeaderStyle-Width="25px">
            <ItemTemplate>
                <asp:TextBox ID="txtInit" runat="server" Width="25px" class="classOnlyInt" Text='<%#Eval("Init") %>' MaxLength="1"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="SkipB" HeaderStyle-Width="35px">
            <ItemTemplate>
                <asp:TextBox ID="txtSkipB" runat="server" Width="35px" class="classOnlyInt" Text='<%#Eval("SkipB4") %>' MaxLength="1"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>
      
        <asp:TemplateField HeaderText="SkipA" HeaderStyle-Width="35px">
            <ItemTemplate>
                <asp:TextBox ID="txtSkipA" runat="server" Width="35px" class="classOnlyInt" Text='<%#Eval("SkipA4") %>' MaxLength="1"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="FSize" HeaderStyle-Width="35px">
            <ItemTemplate>
                <asp:TextBox ID="txtFontSize" runat="server" Width="35px" class="classOnlyInt" Text='<%#Eval("FSize") %>' MaxLength="2"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="B" HeaderStyle-Width="15px">
            <ItemTemplate>
                <asp:TextBox ID="txtBold" runat="server" Width="15px" Text='<%#Eval("Bold") %>' MaxLength="1"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>  
       
       <asp:TemplateField HeaderText="UL" HeaderStyle-Width="15px">
            <ItemTemplate>
                <asp:TextBox ID="txtUnderLine" runat="server" Width="15px" Text='<%#Eval("UL") %>' MaxLength="1"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>  
       
       <asp:TemplateField HeaderText="OL" HeaderStyle-Width="15px">
            <ItemTemplate>
                <asp:TextBox ID="txtOverLine" runat="server" Width="15px" Text='<%#Eval("OL") %>' MaxLength="1"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>  

        <asp:TemplateField HeaderText="CC Group" HeaderStyle-Width="75px">
            <ItemTemplate>
                <asp:TextBox ID="txtFromCC" runat="server" Width="75px" Text='<%#Eval("FromCC") %>' MaxLength="12" OnTextChanged="txtFromCC_TextChanged" AutoPostBack="true"></asp:TextBox>
                <ajaxToolkit:AutoCompleteExtender runat="server" ID="ACFromCC" TargetControlID="txtFromCC" ServiceMethod="GetCostCenterGroup" ServicePath="~/InvSetupSupport/AutoCompleteSearch.asmx" MinimumPrefixLength="1"
                        CompletionSetCount="5" CompletionInterval="100" FirstRowSelected="true" EnableCaching="true" ></ajaxToolkit:AutoCompleteExtender>
            </ItemTemplate>
        </asp:TemplateField>  

        <%--<asp:TemplateField HeaderText="To CC" HeaderStyle-Width="75px">
            <ItemTemplate>
                <asp:TextBox ID="txtToCC" runat="server" Width="75px" Text='<%#Eval("ToCC") %>' MaxLength="12" OnTextChanged="txtToCC_TextChanged" AutoPostBack="true"></asp:TextBox>
                <ajaxToolkit:AutoCompleteExtender runat="server" ID="ACToCC" TargetControlID="txtToCC" ServiceMethod="GetCostCenter" ServicePath="~/InvSetupSupport/AutoCompleteSearch.asmx" MinimumPrefixLength="1"
                        CompletionSetCount="5" CompletionInterval="100" FirstRowSelected="true" EnableCaching="true" ></ajaxToolkit:AutoCompleteExtender>
            </ItemTemplate>
        </asp:TemplateField> --%> 
       
       
        </Columns>
        
        </asp:GridView>
        <asp:LinkButton ID="lnkAddNewRow" runat="server" Text="Add New Row" OnClick="lnkAddNewRow_Click" ></asp:LinkButton>
    </div>
    </fieldset>
   <div>
        <asp:ImageButton ID="btnSave" runat="server"  ImageUrl="~/images/btn_save.png" OnCommand="btnSave_Click"
                        onMouseOver="this.src='../images/btn_save_m.png'" onMouseOut="this.src='../images/btn_save.png'"   ValidationGroup="main"/>
                        
                    
        <asp:ImageButton ID="btnClear" runat="server"  ImageUrl="~/images/btn_clear.png" OnCommand="btnClear_Click"
                        onMouseOver="this.src='../images/btn_clear_m.png'" onMouseOut="this.src='../images/btn_clear.png'"/>
                       
                       
        <asp:ImageButton ID="btnList" runat="server"  ImageUrl="~/images/btn_list.png" OnCommand="btnList_Click"
                       onMouseOver="this.src='../images/btn_list_m.png'" onMouseOut="this.src='../images/btn_list.png'"/>
    </div>
    
     <div>
    
        <%--<asp:Button ID="btnPrint" runat="server" OnClick="btnPrint_Click" Text="Print"  ValidationGroup="main"/>--%>
       <%--  <rsweb:ReportViewer ID="reportViewer" runat="server">
        </rsweb:ReportViewer>--%>
    </div>
    
<%--</ContentTemplate>--%>
<%--<Triggers><asp:AsyncPostBackTrigger ControlID="btnPrint" /></Triggers>
<Triggers><asp:PostBackTrigger ControlID="btnPrint" /></Triggers>--%>
<%--</asp:UpdatePanel>--%>
  
 
     
</asp:Content>
