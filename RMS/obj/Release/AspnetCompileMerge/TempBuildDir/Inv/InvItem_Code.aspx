<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
Culture="auto" UICulture="auto" EnableEventValidation="false" 
CodeBehind="InvItem_Code.aspx.cs" Inherits="RMS.Inv.InvItem_Code"  %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" language="javascript">
    //DISABLING DOUBLE CLICK
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    function BeginRequestHandler(sender, args) { var oControl = args.get_postBackElement(); oControl.disabled = true; }

</script>
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
    
    
    //$(document).ready(function() {
    function pageLoad() {
        $(".filter").keyup(function(event) {
            if (event.keyCode == 13) {
                $(".filterclick").click();
            }
        });
    }
    //});
    
       </script>
    
    
  
  
    <asp:UpdatePanel ID="upnlMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    
    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
        
    <br />
    
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
          <td width="3%">
          </td>
          <td>
                    <table>
                        <tr>
                            <td class="LblBgSetup">
                                <asp:Label ID="lblType" runat="server" Text="Item Type:" Width="110px">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlType" runat="server" OnSelectedIndexChanged="DdlType_SelectedIndexChanged" AutoPostBack="true" CssClass="RequiredFieldDropDown">
                                <asp:ListItem Text="Select Type" Value="0">
                                </asp:ListItem>
                                <asp:ListItem Text="Group" Value="A">
                                </asp:ListItem>
                                <asp:ListItem Text="Control" Value="C">
                                </asp:ListItem>
                                <asp:ListItem Text="Detail" Value="D" Selected="True">
                                </asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <%--For Group--%>
                         <tr id="trGroup" runat="server" visible="false">
                            <td class="LblBgSetup">
                                <asp:Label ID="lblGroupCode" runat="server" Text="Group Code:" Width="110px">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtGroupCode" runat="server" CssClass="RequiredFieldTxtSmall" AutoPostBack="true" OnTextChanged="txtGroupCode_TexChanged" Width="20px">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqFv2" runat="server" ControlToValidate="txtGroupCode" Display="None" ErrorMessage="Enter Group code" SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>
                                
                            </td>
                        </tr>
                        
                        <tr id="trGroup1" runat="server" visible="false" valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="lblGroup" runat="server" Text="Group Dsc:" Width="110px">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtGroup" runat="server" CssClass="RequiredField" OnTextChanged="txtGroup_TextChanged" TextMode="MultiLine" onkeyup="LimitText(this,50);" AutoPostBack="true">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtGroup" Display="None" ErrorMessage="Enter Group Description" SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>   
                            </td>
                        </tr>
                        
                        <%--For Control--%>
                        <tr   id="trControl1" runat="server" visible="false">
                            <td class="LblBgSetup">
                                <asp:Label ID="lblGroupName" runat="server" Text="Group Name:" Width="110px">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlGroup" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" CssClass="RequiredFieldDropDown">
                                <asp:ListItem Selected="True" Text="Select Group" Value="0">
                                </asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr   id="trControl" runat="server" visible="false">
                            <td class="LblBgSetup">
                                <asp:Label ID="lblControlCode" runat="server" Text="Control Code:" Width="110px">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtControlHead" runat="server" Enabled="false" CssClass="RequiredField" Width="20px">
                                </asp:TextBox>
                                <asp:TextBox ID="txtControlCode" runat="server" CssClass="RequiredFieldTxtSmall" AutoPostBack="true" OnTextChanged="txtControlCode_TextChanged" Width="60px">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtControlCode" Display="None" ErrorMessage="Enter Control Code" SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>   
                            
                                
                            </td>
                        </tr>
                        <tr   id="trControl2" runat="server" visible="false" valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="lblControlName" runat="server" Text="Control Dsc:" Width="110px">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContolName" runat="server" CssClass="RequiredField" OnTextChanged="txtContolName_TextChanged" AutoPostBack="true" TextMode="MultiLine"  onkeyup="LimitText(this,50);" onblur="LimitText(this,50);">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtContolName" Display="None" ErrorMessage="Enter Control Name" SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>   
                            
                            </td>
                        </tr>
                        <%--For Detail--%>
                        <tr   id="trDetail" runat="server" visible="false">
                            <td class="LblBgSetup">
                                <asp:Label ID="lblControl" runat="server" Text="Control Name:" Width="110px">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlControl" runat="server" AppendDataBoundItems="true" AutoPostBack="true"  CssClass="RequiredFieldDropDown" OnSelectedIndexChanged="ddlControl_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Text="Select Control" Value="0">
                                </asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr   id="trDetail2" runat="server" visible="false">
                            <td class="LblBgSetup">
                                <asp:Label ID="lblItemCode" runat="server" Text="Item Code:" Width="110px">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemHead" runat="server" CssClass="RequiredFieldTxtSmall" Enabled="false" Width="30px">
                                </asp:TextBox>
                                <asp:TextBox ID="txtItemCode" runat="server" CssClass="RequiredFieldTxtSmall" AutoPostBack="true" OnTextChanged="txtItemCode_TextChanged" Width="60px">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtItemCode" Display="None" ErrorMessage="Enter Item Code" SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>   
                            
                            </td>
                        </tr>
                         <tr   id="trDetail4" runat="server" visible="false">
                            <td class="LblBgSetup">
                                <asp:Label ID="lblAltrCode" runat="server" Text="Altr. Code:" Width="110px">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAltrCode" runat="server" AutoPostBack="true" OnTextChanged="txtAltrCode_TextChanged" Width="60px">
                                </asp:TextBox>
                            </td>
                            
                        </tr>
                        <tr   id="trDetail3" runat="server" visible="false" valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="lblItemName" runat="server" Text="Item Dsc:" Width="110px">
                                </asp:Label>
                            </td>
                             <td>
                                <asp:TextBox ID="txtItemName" runat="server" CssClass="RequiredField" OnTextChanged="txtItemName_TextChanged" AutoPostBack="true" TextMode="MultiLine"  onkeyup="LimitText(this,50);" onblur="LimitText(this,50);">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqFv1" runat="server" ControlToValidate="txtItemName" Display="None" ErrorMessage="Enter Item Description"  SetFocusOnError="true" ValidationGroup="main"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr   id="trDetail7" runat="server" visible="false" valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="Label1" runat="server" Text="UOM:" Width="110px">
                                </asp:Label>
                            </td>
                             <td>
                                <asp:DropDownList ID="ddlUOM" runat="server" AppendDataBoundItems="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="trDetail6" runat="server" visible="false" valign="top">
                             <td   class="LblBgSetup">
                            <asp:Label ID="lblSpecs" runat="server" Text="Specification:">
                            </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSpecs" runat="server" TextMode="MultiLine"  onkeyup="LimitText(this,99);" onblur="LimitText(this,99);">
                                </asp:TextBox>
                            </td>
                            
                            <td   class="LblBgSetup">
                                <asp:Label ID="lblItemType" runat="server" Text="Item Type:">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:RadioButton ID="rdbTypeImp" runat="server"   GroupName="Type" Text="Imported"/>
                                &nbsp;
                                <asp:RadioButton ID="rdbTypeLocal" runat="server"  GroupName="Type" Text="Local"/>
                            </td>
                        </tr>
                        <tr id="trDetail5" runat="server" visible="false" valign="top">
                            <td   class="LblBgSetup">
                                <asp:Label ID="lblBarCode" runat="server" Text="Bar Code:">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBarCode" runat="server" TextMode="MultiLine"  onkeyup="LimitText(this,50);" onblur="LimitText(this,50);">
                                </asp:TextBox>
                            </td>
                            <td   class="LblBgSetup">
                                <asp:Label ID="lblDrawingNo" runat="server" Text="Drawing No:">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDrawingNo" runat="server" TextMode="MultiLine"  onkeyup="LimitText(this,99);" onblur="LimitText(this,99);">
                                </asp:TextBox>
                            </td>
                           
                           
                        </tr>
                        
                        
                    </table>
 
          </td>
          <td width="3%">
          </td>
          </tr>
          <tr>
          <td width="3%">
          </td>
          <td>
            &nbsp;
          </td>
          <td width="3%">
          </td>
          </tr>
          </table>
  
    
    </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" />
            <asp:AsyncPostBackTrigger ControlID="grdcode" />
        </Triggers>
    </asp:UpdatePanel>
      
       
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
       <tr>
      <td width="3%">
      </td>
      <td>

               <asp:ImageButton ID="btnSave" runat ="server"  OnClick="btnSave_Click" ImageUrl="~/images/btn_save.png"
                    onMouseOver="this.src='../images/btn_save_m.png'" onMouseOut="this.src='../images/btn_save.png'"  ValidationGroup="main" />
                    
               <asp:ImageButton ID="btnClear" runat ="server"  OnClick="btnClear_Click" ImageUrl="~/images/btn_clear.png"
                    onMouseOver="this.src='../images/btn_clear_m.png'" onMouseOut="this.src='../images/btn_clear.png'" />
                    
      </td>
      <td width="3%">
      </td>
      </tr>
      </table>
      
      <br />
      
      <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
      <ContentTemplate>
     
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
      <td width="3%">
      </td>
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
                        <asp:TextBox runat="server" ID="txtFltDesc" Width="350px" MaxLength="50" class="filter"></asp:TextBox>
                    </td>
                    <td>Code Type:</td>
                    <td>
                        <asp:DropDownList ID="ddlFltCodeType" runat="server" class="filter">
                            <asp:ListItem Value="M">All
                            </asp:ListItem>
                            <asp:ListItem Text="Group" Value="A">
                            </asp:ListItem>
                            <asp:ListItem Text="Control" Value="C">
                            </asp:ListItem>
                            <asp:ListItem Text="Detail" Value="D">
                            </asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                              OnClick="btnsearch_Click" ToolTip="Search Code" Visible="true" class="filterclick"/>
                     </td>
                    <td>&nbsp;</td>
                </tr>
                <tr><td colspan="9">&nbsp;</td></tr></table>
                
                
          </td>
      <td width="3%">
      </td>
      </tr>
      <tr>
      <td width="3%">
      </td>
          <td>
                <asp:GridView ID="grdcode" runat="server" DataKeyNames="itm_cd, ct_id" OnSelectedIndexChanged="grdcode_SelectedIndexChanged" 
                    AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No record found." Width="100%" PageSize="25" OnPageIndexChanging="grdcode_PageIndexChanging"
                     OnRowDataBound="grdcode_RowDataBound" OnDataBound="grdcode_DataBound" >
                    <HeaderStyle CssClass ="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <%--<SelectedRowStyle CssClass="gridSelectedRow" />--%>
                    <Columns>
                        <asp:BoundField DataField="itm_cd" HeaderText="Code"  />
                        <asp:BoundField DataField="itm_dsc" HeaderText="Description" />
                        <asp:BoundField DataField="codetype" HeaderText="Code Type" />
                        <asp:BoundField DataField="prntDsc" HeaderText="Code Head" />
                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                            <ItemStyle />
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
          </td>
      <td width="3%">
      </td>
      </tr>
      </table>
      
      </ContentTemplate>
      <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" />
      </Triggers>
      </asp:UpdatePanel>

</asp:Content>
