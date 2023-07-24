<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" Culture="auto"
    UICulture="auto" EnableEventValidation="true" AutoEventWireup="true" 
    CodeBehind="frmChequePrint.aspx.cs" Inherits="RMS.GL.Setup.frmChequePrint" %>
<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--<script type="text/javascript">
    $(function() {

    //$('#<%= txtcashcode.ClientID %>').autocomplete({
     $('.tbt').autocomplete({
            source: function(request, response) {
                $.ajax({
                url: "frmPreferences.aspx/GetControlAccount",
                    data: "{ 'sname': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",

                    dataFilter: function(data) { return data; },
                    success: function(data) {
                        response($.map(data.d, function(item) {
                            return {
                                value: item,
                                result: item
                            }
                        }))
                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            },
            minLength: 1
        });
    });
    
    $(function() {
    $('.tbt1').autocomplete({
            source: function(request, response) {
                $.ajax({
                url: "frmPreferences.aspx/GetDetailAccount",
                    data: "{ 'sname': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",

                    dataFilter: function(data) { return data; },
                    success: function(data) {
                        response($.map(data.d, function(item) {
                            return {
                                value: item,
                                result: item
                            }
                        }))
                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            },
            minLength: 1
        });
    });
    
    </script>
<script type="text/javascript">

    $(document).ready(function() {
    $('.tbt').blur(function() {
        selectval = $(this).val().split(" ");
        $(this).val(selectval[0]);

        });

        $('.tbt1').blur(function() {
            selectval = $(this).val().split(" ");
            $(this).val(selectval[0]);

        });
        
        

    });


</script>--%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
  <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
  
     <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">   
    
    
    <Triggers > <asp:AsyncPostBackTrigger ControlID="grdpreference" /> </Triggers>
    <ContentTemplate>--%>
  <table>
    <tr>
        <td>
        <fieldset>
         <legend style="margin:5px"><b>Cheque Style</b></legend>
           <div>
            <table>
              
            <tr>
            <td>
            <asp:Label ID="lblcash" runat="server" Text="Font Size"></asp:Label>
            </td>
            <td>
            <asp:TextBox ID="txtFontSize" class="tbt" MaxLength="3" runat="server"></asp:TextBox>
            </td>
            <td>
                
            <asp:Label ID="lblbank"  runat="server" Text="Date Word Spacing"></asp:Label>
            </td>
            <td>
            <asp:TextBox ID="txtDateWordSpacing" class="tbt" MaxLength="3"  runat="server"></asp:TextBox>
            </td>
             </tr>
            <tr>
            <td>
            <asp:Label ID="lblvendor" runat="server" Text="Payee word Spacing"></asp:Label>
            </td>
            <td>
            <asp:TextBox ID="txtPayeeWordSpacing" MaxLength="3" class="tbt"  runat="server"></asp:TextBox>
            </td>
            <td>
            <asp:Label ID="lbldepartment" runat="server" Text="Payee Bold"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chkPayeeBold" runat="server" />
            </td>
            </tr>
            <tr>
            <td>
            <asp:Label ID="lblcustomer" runat="server" Text="Payee prefix"></asp:Label>
            </td>
            <td>
            <asp:TextBox ID="txtPayPrefix" class="tbt" MaxLength="3"  runat="server"></asp:TextBox>
            </td>
            <td>
            <asp:Label ID="lblbankaccount" runat="server" Text="Payee suffix"></asp:Label>
            </td>
            <td>
            <asp:TextBox ID="txtPaySuffix" class="tbt1" MaxLength="3" runat="server"></asp:TextBox>
            </td>
            </tr>
            <tr>
             <td>
            <asp:Label ID="lblcashaccount" runat="server" Text="Digits prefix"></asp:Label>
            </td>
            <td>
            <asp:TextBox ID="txtDigitsPrefix"  class="tbt1" MaxLength="3" runat="server"></asp:TextBox>
            </td>
            <td>
            <asp:Label ID="Label1" runat="server" Text="Digits Suffix"></asp:Label>
          
            </td>
            <td>
            <asp:TextBox ID="txtDigitsSuffix"  class="tbt1" MaxLength="3" runat="server"></asp:TextBox>
              
            </td>
           </tr>
                   <tr>
             <td>
            <asp:Label ID="Label2" runat="server" Text="Amount prefix"></asp:Label>
            </td>
            <td>
            <asp:TextBox ID="txtAmountPrefix"  class="tbt1" MaxLength="3" runat="server"></asp:TextBox>
            </td>
            <td>
            <asp:Label ID="Label3" runat="server" Text="Amount Suffix"></asp:Label>
          
            </td>
            <td>
            <asp:TextBox ID="txtAmountSuffix"  class="tbt1" MaxLength="3" runat="server"></asp:TextBox>
              
            </td>
           </tr>
           <tr>
            <td colspan="2">
                <asp:Label ID="lblapprove" runat="server" Text="Approve"  visible="false"></asp:Label>
                <asp:CheckBox ID="chkapprove" runat="server"  visible="false"/>
                <asp:Label ID="lblprint" runat="server" Text="Print" visible="false"></asp:Label>
                <asp:CheckBox ID="chkprint" runat="server" visible="false" />
            </td>
           </tr>
          
            
            </table>
            </div>
            </fieldset>
        </td>
      </tr>
      <tr> 
        <td>
            <fieldset>
                <legend style="margin:5px"> <b>Printer Orientation</b></legend>
                    <table>
                       
                        <tr>
                        <td>
                        <asp:Label ID="lblFreight" runat="server" Text="Cheque Feed"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlfield" runat="server">
                              <asp:ListItem Text="Portrait" Value="1" />
                              <asp:ListItem Text="Land" Value="2" />

                            </asp:DropDownList>
                            
                        </td>
                        <td>
                        <asp:Label ID="lblOtherCost" runat="server" Text="Pixel Shift Left"></asp:Label>
                        </td>
                        <td>
                        <asp:TextBox ID="txtPixelShiftLeft" class="tbt1" MaxLength="3" runat="server"></asp:TextBox>
                        </td>
                        </tr>
                        <tr>
                        <td>
                        <asp:Label ID="lblimpFreight" runat="server" Text="Pixels Right"></asp:Label>
                        </td>
                        <td>
                        <asp:TextBox ID="txtPixelsRight" class="tbt1" MaxLength="3"  runat="server"></asp:TextBox>
                        </td>
                              <td>
                        <asp:Label ID="Label4" runat="server" Text="Pixels Up"></asp:Label>
                        </td>
                        <td>
                        <asp:TextBox ID="txtPixelsUp" class="tbt1" MaxLength="3"  runat="server"></asp:TextBox>
                        </td>
                        </tr>
                         <tr>
                        <td>
                        <asp:Label ID="Label5" runat="server" Text="Pixels Down"></asp:Label>
                        </td>
                        <td>
                        <asp:TextBox ID="txtPixelsDown" class="tbt1" MaxLength="3"  runat="server"></asp:TextBox>
                        </td>
                              <td>
                         <asp:Label ID="labl1" runat="server" Text="LineBreak"></asp:Label>
                        </td>
                        <td>
                      <asp:TextBox ID="txtLineBreak" class="tbt1" MaxLength="3"  runat="server"></asp:TextBox> 
                        </td>
                        </tr>
                        
                      
                          <tr> 
            <td></td> 
            <td colspan="1" align="Right">
              <asp:ImageButton ID="btnSaveCode" runat="server"  ImageUrl="~/images/btn_save.png" OnCommand="btnSaveCodes_Click"
                onMouseOver="this.src='../images/btn_save.png'" onMouseOut="this.src='../images/btn_save.png'"   ValidationGroup="main"/>
            </td>
            
            </tr>
                          <tr>
                        <td></td> 
                        <td colspan="1" align="left">
                          <asp:ImageButton ID="btnSaveInv" runat="server" Visible="false"  ImageUrl="~/images/btn_save.png" OnCommand="btnSaveInvent_Click"
                            onMouseOver="this.src='../images/btn_save.png'" onMouseOut="this.src='../images/btn_save.png'"   ValidationGroup="main"/>
                        </td>
                        
                        </tr>
                    </table>
            </fieldset>
        </td>
      </tr>
   </table>
   <%--</ContentTemplate>    
    </asp:UpdatePanel>--%>

</asp:Content>