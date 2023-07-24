<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
Culture="auto" UICulture="auto" EnableEventValidation="false" 
CodeBehind="frmGl_Code.aspx.cs" Inherits="RMS.GL.Setup.frmGl_Code"  %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >

<script type="text/javascript">
//Disabling default submit behaiour
    function disableEnterKey(e) {
        var key;
        if (window.event)
            key = window.event.keyCode; //IE
        else
            key = e.which; //firefox
        return (key != 13);
    }
    document.onkeypress = disableEnterKey;
////////////////////////////////////////////
    

    $(document).ready(function() {
        //function pageLoad() {


        $(".filter").keyup(function(event) {
            if (event.keyCode == 13) {
                $(".filterclick").click();
            }
        });



        var selectedTypeId = $('#<%= ddlcodetype.ClientID %>').val();
        $('#<%= txtdescription.ClientID %>').attr('maxlength', 100);
        $('#<%= ddlglheadcode.ClientID %>').attr('disabled', true);
        $.ajax
                ({
                    url: "frmGl_Code.aspx/GLCodeHeads",
                    data: JSON.stringify({ selectedTypeId1: selectedTypeId }),
                    type: 'POST',
                    contentType: 'application/json;',
                    dataType: 'json',
                    success: function(glheads) {
                        var glheads = glheads.d;
                        if (glheads.length > 0) {
                            $('#<%= txtglcode.ClientID %>').attr('maxlength', glheads[0].ct_len);
                        }
                    }
                });


        $("#<%= ddlcodetype.ClientID %>").change(function() {

            var selectedTypeId = $(this).val();
            if ($(this).val() == "A") {

                $('#<%= ddlglheadcode.ClientID %>').attr('disabled', true);
                $('#<%= ddlgltype.ClientID %>').attr('disabled', true);
                $('#<%= txtglcode.ClientID %>').val('');

                $.ajax
                ({
                    url: "frmGl_Code.aspx/GLCodeHeads",
                    data: JSON.stringify({ selectedTypeId1: selectedTypeId }),
                    type: 'POST',
                    contentType: 'application/json;',
                    dataType: 'json',
                    success: function(glheads) {
                        var glheads = glheads.d;
                        if (glheads.length > 0) {
                            $('#<%= txtglcode.ClientID %>').attr('maxlength', glheads[0].ct_len);
                        }
                    }
                });
            }

            if ($(this).val() == "C" || $(this).val() == "D") {
                $('#<%= ddlglheadcode.ClientID %>').attr('disabled', false);
                $('#<%= ddlgltype.ClientID %>').attr('disabled', true);
                $.ajax
                ({
                    url: "frmGl_Code.aspx/GLCodeHeads",
                    data: JSON.stringify({ selectedTypeId1: selectedTypeId }),
                    type: 'POST',
                    contentType: 'application/json;',
                    dataType: 'json',
                    success: function(glheads) {
                        var glheads = glheads.d;
                        if (glheads.length > 0) {
                            var ddlglheadcode = $('#<%= ddlglheadcode.ClientID %>');
                            ddlglheadcode.empty();
                            $.each(glheads, function(index, item) {
                                $('#<%= ddlglheadcode.ClientID %>').append
                        ($('<option/>').attr('value', item.gl_cd).text(item.gl_cd + '-' + item.gl_dsc));
                            });
                            $('#<%= txtglcode.ClientID %>').attr('maxlength', glheads[0].ct_len);
                            $('#<%= txtglcode.ClientID %>').val(glheads[0].gl_cd);
                            $('#<%= ddlgltype.ClientID %>').val(glheads[0].gt_cd);

                        }
                    }

                });
            }
            else {
                $('#<%= ddlgltype.ClientID %>').attr('disabled', false);
                $('#<%= ddlgltype.ClientID %>')[0].selectedIndex = 0;
                $('#<%= txtglcode.ClientID %>').val('');
                $('#<%= txtdescription.ClientID %>').val('');
                $('#<%= ddlglheadcode.ClientID %>')[0].selectedIndex = -1;
            }
        });



        $("#<%= txtglcode.ClientID %>").keyup
            (function() {
                var selectedTypeId1 = $("#<%= ddlcodetype.ClientID %>").val();
                var selectedTypeId2 = $("#<%= ddlglheadcode.ClientID %>").val();
                if ($("#<%= ddlcodetype.ClientID %>").val() == "C" || $("#<%= ddlcodetype.ClientID %>").val() == "D") {
                    $.ajax
                ({
                    url: "frmGl_Code.aspx/GLCodeVal",
                    data: JSON.stringify({ selectedTypeId1: selectedTypeId1, selectedTypeId2: selectedTypeId2 }),
                    type: 'POST',
                    contentType: 'application/json;',
                    dataType: 'json',
                    success: function(glheads) {
                        var glheads = glheads.d;
                        if (glheads.length > 0) {
                            // alert(glheads[0].gl_cd);
                            if (glheads[0].ct_len / 2 >= $("#<%= txtglcode.ClientID %>").val().length) {
                                $("#<%= txtglcode.ClientID %>").val(glheads[0].gl_cd);
                            }
                        }
                    }

                });
                }
            });




        $("#<%= ddlglheadcode.ClientID %>").change(function() {
            var selectedTypeId = $(this).val();
            $.ajax
            ({
                url: "frmGl_Code.aspx/GLCodeHeadsType",
                data: JSON.stringify({ selectedTypeId: selectedTypeId }),
                type: 'POST',
                contentType: 'application/json;',
                dataType: 'json',
                success: function(glheads) {
                    var glheads = glheads.d;
                    if (glheads.length > 0) {
                        $("#<%= txtglcode.ClientID %>").val(glheads[0].gl_cd);
                        $("#<%= ddlgltype.ClientID %>").val(glheads[0].gt_cd);
                    }
                }

            });

            var v = $('#<%= ddlglheadcode.ClientID %>').val();
            $('#<%= hidglHeadCode.ClientID %>').val(v);

        });
        /////////////////////////////////////////////////////////////////
        $("#<%= txtglcode.ClientID %>").keydown(function(event) {
            var v = $('#<%= ddlglheadcode.ClientID %>').val();
            $('#<%= hidglHeadCode.ClientID %>').val(v);


            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }
        });
        $("#<%= txtglcode.ClientID %>").change(function(event) {
            var v = $('#<%= ddlglheadcode.ClientID %>').val();
            $('#<%= hidglHeadCode.ClientID %>').val(v);
        });
        /////////////////////////////////////////////////////////////////

    });
    
       </script>
    
  <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="3%"></td>
            <td height="10"></td>
            <td width="3%"></td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td valign="top">
             <table class="stats1">
             <asp:Panel runat="server" ID="pnlMain">
              <tr>
                <td colspan="2" valign="top" class="bg_input_area"></td>
              </tr>
             
              
              <tr>
                <td>
                    <asp:Label ID="lblCodeType" runat="server" Text="Code Type:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlcodetype"  OnSelectedIndexChanged="ddlcodetype_SelectedIndexChanged"  runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlcodetype"
                        ErrorMessage="Please Select Code Type" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                </td>
                
            </tr>
              <tr>
                <td>
                    <asp:Label ID="lblGlHeadCode" runat="server" Text="GL Head Code:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlglheadcode"   OnSelectedIndexChanged="ddlglheadcode_SelectedIndexChanged"  runat="server"></asp:DropDownList>
                    <asp:HiddenField ID="hidglHeadCode" runat="server" />
                </td>
                
            </tr>
            
            <tr>
                <td>
                   <asp:Label ID="lblglcode" runat="server" Text="GL Code:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtglcode" CssClass="RequiredField"  runat="server" MaxLength="150"></asp:TextBox>
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtglcode"
                        ErrorMessage="Please enter GL Code" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                </td>
                
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbldescription" runat="server" Text="Description:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtdescription" CssClass="RequiredField" runat="server" MaxLength="150" Width="400"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtdescription"
                        ErrorMessage="Please enter GL Description" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                </td>
                
            </tr>
         
            <tr>
                <td>
                    <asp:Label ID="lblgltype" runat="server" Text="GL Type:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlgltype" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlgltype"
                        ErrorMessage="Please select GL Type" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                        
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
                <td colspan="2" valign="top">
                    <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server"/>
                </td>
            </tr>

            </asp:Panel>  
          
          <tr>
            <%--<td valign="top">&nbsp;</td>--%>
            <td valign="top">
        
            <%--i did (buttons)--%>
            
            
            <!--img src="images/btn_new.jpg" width="60" height="20" /> <img src="images/btn_edit.jpg" width="60" height="20" /> <img src="images/btn_delete.jpg" alt="" width="60" height="20" /> <img src="images/btn_save.jpg" width="60" height="20" /> <img src="images/btn_cancel.jpg" width="60" height="20" /-->
            </td>
            
          </tr>

        </table>
        
        </td>
        <td></td>
      </tr>
      <tr><td colspan="3">&nbsp;</td></tr>
      <tr>
        <td></td>
        <td>
            <table class="filterTable" cellpadding="1" cellspacing="2" width="100%"><tr><td colspan="9">&nbsp;</td></tr>
              <tr>
                <td>&nbsp;</td>
                <td>GL Code:</td>
                <td>
                    <asp:TextBox runat="server" ID="txtFltGlCode" Width="80" class="filter"></asp:TextBox>
                </td>
                <td>Description:</td>
                <td>
                    <asp:TextBox runat="server" ID="txtFltDesc" class="filter"></asp:TextBox>
                </td>
                <td>GL Type:</td>
                <td>
                    <asp:DropDownList ID="ddlFltGlType" runat="server" AppendDataBoundItems="true" class="filter">
                        <asp:ListItem Value="0">All</asp:ListItem>    
                   </asp:DropDownList>
                </td>
                <td>Code Type:</td>
                <td>
                    <asp:DropDownList ID="ddlFltCodeType" runat="server" AppendDataBoundItems="true" class="filter">
                        <asp:ListItem Value="0">All</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/search-icon-blue.gif" class="filterclick"
                          OnClick="btnsearch_Click" ToolTip="Search Code" Visible="true"/>
                 </td>
                <td>&nbsp;</td>
            </tr>
            <tr><td colspan="9">&nbsp;</td></tr></table>
        </td>
        <td></td>
      </tr>
      <tr><td colspan="3">&nbsp;</td></tr>
      <tr>
          <td></td>
          <td>
          <asp:GridView ID="grdcode" runat="server" DataKeyNames="gl_cd" OnSelectedIndexChanged="grdcode_SelectedIndexChanged" OnDataBound="grdcode_OnDataBound"
                    AutoGenerateColumns="False" AllowPaging="True" Width="100%" PageSize="25" OnPageIndexChanging="grdcode_PageIndexChanging" OnRowDataBound="grdcode_RowDataBound" > 
                    <HeaderStyle CssClass ="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <%--<SelectedRowStyle CssClass="gridSelectedRow" />--%>
                    <Columns>
                        <asp:BoundField DataField="gl_cd" HeaderText="Code"  />
                        <asp:BoundField DataField="gl_dsc" HeaderText="Description" />
                        <asp:BoundField DataField="codetype" HeaderText="Code Type" />
                        <asp:BoundField DataField="headgl_dsc" HeaderText="GL Code Head" />
                        <asp:BoundField DataField="gl_type" HeaderText="GL Type"  />
                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                            <ItemStyle />
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
          
          </td>
          <td></td>
          </tr>
    </table>

            
             
    

</asp:Content>
