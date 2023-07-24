﻿<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
Culture="auto" UICulture="auto" EnableEventValidation="false" 
CodeBehind="frmGlLvlCode.aspx.cs" Inherits="RMS.GLSetup.frmGlLvlCode"  %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

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

    function pageLoad() {
        $(".filter").keyup(function(event) {
            if (event.keyCode == 13) {
                $(".filterclick").click();
            }
        });
    }

    function pageLoad() {
        
        $(".filter").keyup(function(event) {
            if (event.keyCode == 13) {
                $(".filterclick").click();
            }
        });

        $("#<%= txtCode.ClientID %>").keyup(function(event) {

            var selectedTypeId1 = $("#<%= ddlCodeType.ClientID %>").val();
            var selectedTypeId2 = $("#<%= ddlCodeHead.ClientID %>").val();

            if ($("#<%= ddlCodeType.ClientID %>").val() != "") {
                
                $.ajax({
                    url: "frmGlLvlCode.aspx/CodeVal",
                    data: JSON.stringify({ selectedTypeId1: selectedTypeId1, selectedTypeId2: selectedTypeId2 }),
                    type: 'POST',
                    contentType: 'application/json;',
                    dataType: 'json',
                    success: function(heads) {
                        var heads = heads.d;
                        if (heads.length > 0) {
//                            alert(heads[0].ct_len);
//                            alert(heads[0].gl_cd);
//                            alert($("#<%= txtCode.ClientID %>").val().substring(0, heads[0].p_ct_len));
//                            alert(heads[0].p_ct_len);
                            if ($("#<%= txtCode.ClientID %>").val().substring(0, heads[0].p_ct_len) != heads[0].gl_cd) {
                                $("#<%= txtCode.ClientID %>").val(heads[0].gl_cd);
                            }
                        }
                    }

                });
            }
        });

        $("#<%= txtCode.ClientID %>").keydown(function(event) {
          
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }
        });
    }
</script>
    
    
  
  
    <asp:UpdatePanel ID="upnlMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        
        <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List" ValidationGroup="main" />
        <uc1:Messages ID="ucMessage" runat="server" />
            
        <br />
        
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                          <td width="1%"></td>
                          <td>
                                    <table>
                                        <tr valign="top">
                                            <td class="LblBgSetup">
                                                <asp:Label ID="Label2" runat="server" Text="Code Type:" Width="110"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCodeType" runat="server" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCodeType_SelectedIndexChanged"></asp:DropDownList>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr valign="top">
                                            <td class="LblBgSetup">
                                                <asp:Label ID="Label1" runat="server" Text="Code Head:" Width="110"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCodeHead" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCodeHead_SelectedIndexChanged"></asp:DropDownList>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr valign="top">
                                            <td class="LblBgSetup">
                                                <asp:Label ID="Label3" runat="server" Text="Code:" Width="110"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCode" runat="server" CssClass="RequiredField" Width="120"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCode" Display="None" ErrorMessage="Enter code." SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>   
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr valign="top">
                                            <td class="LblBgSetup">
                                                <asp:Label ID="Label4" runat="server" Text="Description:" Width="110"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDescription" runat="server" CssClass="RequiredField" TextMode="MultiLine"  onkeyup="LimitText(this,100);" onblur="LimitText(this,100);" Width="200" Height="40"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDescription" Display="None" ErrorMessage="Enter description." SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>   
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                         <tr>
                                            <td  class="LblBgSetup">
                                                <asp:Label ID="lblgltype" runat="server" Text="GL Type:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlgltype" runat="server" AppendDataBoundItems="true">
                                                    <asp:ListItem Selected="True" Value="0">Select GL Type</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlgltype" InitialValue="0"
                                                    ErrorMessage="Select GL Type" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                                                    
                                            </td>
                                            
                                        </tr>
                                      </table>
                            </td>
                            <td width="1%"></td>
                         </tr>
                 </table>
          </ContentTemplate>
      </asp:UpdatePanel>
      
      <br />
      
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                  <td width="1%"></td>
                  <td>
                            <table width="100%">
                                <tr>
                                    <td>
                                            <asp:ImageButton ID="btnSave" runat ="server"  OnClick="btnSave_Click" ImageUrl="~/images/btn_save.png" onMouseOver="this.src='../images/btn_save_m.png'" onMouseOut="this.src='../images/btn_save.png'"  ValidationGroup="main" />
                                            <asp:ImageButton ID="btnClear" runat ="server"  OnClick="btnClear_Click" ImageUrl="~/images/btn_clear.png" onMouseOver="this.src='../images/btn_clear_m.png'" onMouseOut="this.src='../images/btn_clear.png'" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                               <table class="filterTable" cellpadding="1" cellspacing="2" width="100%"><tr><td colspan="9">&nbsp;</td></tr>
                                                      <tr>
                                                        <td>&nbsp;</td>
                                                        <td>Code:</td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtFltCode" Width="80" class="filter"></asp:TextBox>
                                                        </td>
                                                        <td>Description:</td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtFltDesc" Width="300px" MaxLength="50" class="filter"></asp:TextBox>
                                                        </td>
                                                        <td>Code Type:</td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlFltCodeType" runat="server" AppendDataBoundItems="true" class="filter">
                                                                <asp:ListItem Value="All">All</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>GL Type:</td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlFltGlType" runat="server" AppendDataBoundItems="true" class="filter">
                                                                <asp:ListItem Value="-">All</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/search-icon-blue.gif" OnClick="btnsearch_Click" ToolTip="Search Code" Visible="true" class="filterclick"/>
                                                         </td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                    <td colspan="9">&nbsp;</td>
                                                    </tr>
                                                </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                              <asp:GridView ID="grdcode" runat="server" DataKeyNames="gl_cd, ct_id,codetype, headgl_cd, gt_cd" OnSelectedIndexChanged="grdcode_SelectedIndexChanged" AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No record found." Width="100%" PageSize="25" OnPageIndexChanging="grdcode_PageIndexChanging" OnRowDataBound="grdcode_RowDataBound" OnDataBound="grdcode_DataBound" >
                                                    
                                                    <Columns>
                                                        <asp:BoundField DataField="gl_cd" HeaderText="Code"  />
                                                        <asp:BoundField DataField="gl_dsc" HeaderText="Description" />
                                                        <asp:BoundField DataField="codetype" HeaderText="Code Type" />
                                                        <asp:BoundField DataField="gl_type" HeaderText="GL Type" />
                                                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                                                            <ItemStyle />
                                                            <ControlStyle CssClass="lnk"></ControlStyle>
                                                        </asp:CommandField>
                                                    </Columns>
                                                    
                                                    <HeaderStyle CssClass ="grid_hdr" />
                                                    <RowStyle CssClass="grid_row" />
                                                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                                                    <%--<SelectedRowStyle CssClass="gridSelectedRow" />--%>
                                                    
                                               </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                  </td>
                  <td width="1%"></td>
               </tr>
      </table>

</asp:Content>
