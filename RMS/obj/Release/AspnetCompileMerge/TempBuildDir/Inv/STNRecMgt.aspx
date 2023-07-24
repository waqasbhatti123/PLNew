<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="STNRecMgt.aspx.cs" Inherits="RMS.Inv.STNRecMgt" Culture="auto" UICulture="auto"
    EnableEventValidation="true"%>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" language="javascript">
    //DISABLING DOUBLE CLICK
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    function BeginRequestHandler(sender, args) { var oControl = args.get_postBackElement(); oControl.disabled = true; }

</script>
<script type="text/javascript">

    function pageLoad() {

        $("[id*=GridView1][id*=ddlItem]").each(function() {
            if ($(this).val() != '0') {
                ValidatorEnable($(this).closest('tr').find('[id*=reqQtyIss]')[0], true);
                ValidatorEnable($(this).closest('tr').find('[id*=reqQtyAck]')[0], true);
            }
            else {
                ValidatorEnable($(this).closest('tr').find('[id*=reqQtyIss]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqQtyAck]')[0], false);
            }
        });

        $("[id*=GridView1]input[type=text][id*=txtUomItem]").each(function() {
            $(this).attr("readonly", true);
        });
 
        $(".classDdlItem").keyup(function(e) {
            FillItemDetailFields($(this));
        });
        $(".classDdlItem").change(function(e) {
            FillItemDetailFields($(this));
        });
    }

    function FillItemDetailFields(gRow) {
       var itemid = gRow.find('option:selected').val();
       $.ajax({
           url: "STNMgt.aspx/GetItemDetail",
           data: JSON.stringify({ itemid: itemid }),
           type: 'POST',
           contentType: 'application/json;',
           dataType: 'json',
           success: function(heads) {
               var heads = heads.d;
               if (heads.length > 0) {
                   gRow.closest('tr').find("input[type=text][id*=txtUomItem]").val(heads[0].uom_dsc);
                   ValidatorEnable(gRow.closest('tr').find('[id*=reqQtyIss]')[0], true);
                   ValidatorEnable(gRow.closest('tr').find('[id*=reqQtyAck]')[0], true);
               }
               else {
                   gRow.closest('tr').find("input[type=text][id*=txtUomItem]").val('');
                   ValidatorEnable(gRow.closest('tr').find('[id*=reqQtyIss]')[0], false);
                   ValidatorEnable(gRow.closest('tr').find('[id*=reqQtyAck]')[0], false);
               }
               gRow.focus();
           }
       });
    }
</script>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="1%"></td>
            <td>
                <table>
                  <tr>
                    <td class="LblBgSetup" align="right">
                        <asp:Label ID="Label2" runat="server" Text="Select Store Transfer Doc No:"></asp:Label>
                    </td>
                    <td class="LblBgSetup" align="center">
                        <asp:TextBox ID="txtDocNoSTN" runat="server" CssClass="RequiredField"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtDocNoSTN"
                            ErrorMessage="Please select store transfer doc no" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator> 
                    </td>
                    <td class="LblBgSetup">
                        <asp:ImageButton ID="btnSrchSTN" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                        OnClick="btnSearchSTN_Click" ToolTip="Search Store Transfer Doc Nos" />
                    </td>
                 </tr>
                 <tr>
                    <td colspan="3">
                        <asp:GridView ID="grdSTN" runat="server" DataKeyNames="vr_id" Visible="false" 
                        OnSelectedIndexChanged="grdSTN_SelectedIndexChanged" OnRowDataBound="grdSTN_RowDataBound"
                            AutoGenerateColumns="False" EmptyDataText="No store transfer found" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="vr_no" HeaderText="Doc No">
                                <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="fromloc" HeaderText="From Location">
                                <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="toloc" HeaderText="To Location">
                                <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vr_dt" HeaderText="Date" />                       
                                <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                                    <ControlStyle CssClass="lnk"></ControlStyle>
                                </asp:CommandField>
                            </Columns>
                            <HeaderStyle CssClass="grid_hdr" />
                            <RowStyle CssClass="grid_row" />
                            <AlternatingRowStyle CssClass="gridAlternateRow" />
                            <SelectedRowStyle CssClass="gridSelectedRow" />
                        </asp:GridView>
                    </td>
                     </tr>
                 </table>
            </td>
            <td width="1%"></td>
        </tr>
        <tr>
            <td width="1%">
            </td>
            <td>
                <table class="tblMain"  cellspacing="0" cellpadding="0">
                    <asp:Panel runat="server" ID="pnlMain">
                        <tr>
                            <td class="tdlabel">
                                <asp:Label ID="lblloc" runat="server" Text="From Location:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLoc" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Text="Select Store Location" Selected="True" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlLoc"
                                    ErrorMessage="Please select 'from store location'" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator> 
                            </td>
                            <td class="tdlabel">
                                <asp:Label ID="Label9" runat="server" Text="Status:"></asp:Label>
                            </td>
                            <td >
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="120px">
                                    <asp:ListItem Text="Select Status" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Approved" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="Pending" Value="P" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Cancelled" Value="C"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlStatus"
                                    ErrorMessage="Please select status" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator> 
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="tdlabel">
                                <asp:Label ID="Label1" runat="server" Text="To Location:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlToLoc" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Text="Select Store Location" Selected="True" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlToLoc"
                                    ErrorMessage="Please select 'to store location'" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator> 
                            </td>
                            <td class="tdlabel">
                                <asp:Label ID="Label25" runat="server" Text="Doc Ref #:" Visible="false"></asp:Label>
                                <asp:Label ID="Label4" runat="server" Text="Document Date:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocDate" runat="server" Width="80px">
                                </asp:TextBox>
                                
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDocDate"
                                    ErrorMessage="Please select date" SetFocusOnError="true" ValidationGroup="main" Display="None" ></asp:RequiredFieldValidator>
                                <span class="tdlitrel">
                                    <asp:Literal ID="Literal2"  runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                                </span>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDocDate" PopupPosition="BottomRight" >
                                  </ajaxToolkit:CalendarExtender>
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="tdlabel">
                                <asp:Label ID="Label26" runat="server" Text="Doc No:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocNo" runat="server" ReadOnly="true" TabIndex="-1" Width="80px">
                                </asp:TextBox>
                            </td>
                             <td class="tdlabel">
                                <asp:Label ID="Label10" runat="server" Text="Remarks:"></asp:Label>
                            </td>
                            <td>
                           
                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"
                                 onkeyup="LimitText(this,100);" onblur="LimitText(this,100);"></asp:TextBox>
                            </td>
                        </tr>
                        <%--<tr><td colspan="4">&nbsp;</td></tr>--%>
                        <tr>
                        
                          <td colspan="4">
                        
                                <asp:UpdatePanel ID="uPnl" runat="server" UpdateMode="Conditional">
                                      <ContentTemplate>
                                        
                        <table class="table">
                        <tr>
                            <td>
                                <asp:GridView ID="GridView1" runat="server" CssClass="t_grd" AutoGenerateColumns="false"
                                    OnRowDataBound="GridView1_RowDataBound" Width="100%" Visible="true" >
                                <HeaderStyle CssClass="t_grd_hdr"></HeaderStyle>
                                <FooterStyle CssClass="t_grd_footer"></FooterStyle>
                                <RowStyle CssClass="t_grd_Row"></RowStyle>
                                <EditRowStyle CssClass="t_grd_Edit_Row" />
                                <SelectedRowStyle CssClass="t_grd_Selected_Row"  />
                                <AlternatingRowStyle CssClass="t_grd_Alter_Row"/>
                                <PagerStyle CssClass="t_grd_Pager"></PagerStyle>
                                <PagerSettings Mode="NumericFirstLast" />
                                <EmptyDataRowStyle CssClass="Label_Small_Bold" ForeColor="#C00000" HorizontalAlign="Center" />
                                <Columns>
                                    
                                    <asp:TemplateField HeaderText="Sr. ">
                                      <ItemTemplate>
                                          <asp:TextBox runat="server" ID="lblSr" Text='<%#Eval("Sr") %>' Width="20px" ReadOnly="true" TabIndex="-1"></asp:TextBox>
                                      </ItemTemplate>
                                      <ControlStyle />
                                      <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                      <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="20px"/>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Item">
                                      <ItemTemplate>
                                        <asp:DropDownList ID="ddlItem" runat="server" AppendDataBoundItems="true" CssClass="classDdlItem" Width="160px">
                                            <asp:ListItem Selected="True" Text="Select Item" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                      </ItemTemplate>
                                      <ControlStyle />
                                      <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                      <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="160px"/>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="UOM">
                                      <ItemTemplate>
                                        <asp:TextBox ID="txtUomItem" runat="server" Text='<%#Eval("UOMItem") %>' Width="60px" TabIndex="-1" />
                                      </ItemTemplate>
                                      <ControlStyle />
                                      <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                      <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                                    </asp:TemplateField>
                                   
                                    <asp:TemplateField HeaderText="Quantity Issued">
                                      <ItemTemplate>
                                         <asp:TextBox ID="txtQtyIss" runat="server" Text='<%#Eval("QtyIssued")%>' MaxLength="9" Width="60px"  style="text-align:right" ReadOnly="true" TabIndex="-1"/>
                                         <asp:RequiredFieldValidator ID="reqQtyIss" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                            ControlToValidate="txtQtyIss" ErrorMessage="Quantity Issued required" Enabled="false"></asp:RequiredFieldValidator>
                                         <asp:RangeValidator runat="server" ID="rvQI" CssClass="validateGridView" ValidationGroup="main" ErrorMessage="Invalid/Out of range Quantity Issued"
                                            ControlToValidate="txtQtyIss" MinimumValue="0.001" MaximumValue="999999999.99" Type="Double" Display="Dynamic" SetFocusOnError="true"></asp:RangeValidator>
                                      </ItemTemplate>
                                      <ControlStyle />
                                      <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center"/>
                                      <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                                    </asp:TemplateField>
                                      
                                    <asp:TemplateField HeaderText="Quantity ACK">
                                      <ItemTemplate>
                                        <asp:TextBox ID="txtQtyAck" runat="server" Text='<%#Eval("QtyACK") %>' Width="60px" style="text-align:right"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqQtyAck" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                            ControlToValidate="txtQtyAck" ErrorMessage="Quantity ACK required" Enabled="false"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator runat="server" ID="rvQA" CssClass="validateGridView" ValidationGroup="main" ErrorMessage="Invalid/Out of range Quantity ACK"
                                            ControlToValidate="txtQtyAck" MinimumValue="0.001" MaximumValue="999999999.99" Type="Double" Display="Dynamic" SetFocusOnError="true"></asp:RangeValidator>
                                        <asp:CompareValidator runat="server" ID="cvQA" CssClass="validateGridView" ValidationGroup="main" ErrorMessage="Quantity ACK must be equal to Quantity Issued"
                                            ControlToValidate="txtQtyAck" ControlToCompare="txtQtyIss"  Type="Double" Operator="LessThanEqual" Display="Dynamic" SetFocusOnError="true"></asp:CompareValidator>
                                      </ItemTemplate>
                                      <ControlStyle />
                                      <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                      <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                                    </asp:TemplateField>
                                  
                                  <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("Rem")%>' Width="230px" MaxLength="100"/>
                                    </ItemTemplate>
                                    <ControlStyle />
                                    <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="230px"/>
                                  </asp:TemplateField>
                                    
                                </Columns>
                            </asp:GridView>
                            </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="float:right">
                                        <asp:LinkButton runat="server" Visible="false" ID="addRow" Text="Add Rows" OnClick="addRow_Click" CssClass="lnk"></asp:LinkButton>               
                                    </div>
                                </td>
                            </tr>
                        </table>                                       
                        </ContentTemplate>
                    </asp:UpdatePanel>
                        
                        </td>
                        </tr>
    
                        </asp:Panel>
                        
                        <tr>
                            <td align="center" valign="top" colspan="4">
                                <br />
                                <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server"/>
                            </td>
                        </tr>
                    
                </table>
            </td>
            <td width="1%">
            </td>
        </tr>
        <tr>
            <td width="1%">
            </td>
            <td valign="top">
                <br />
                <table class="filterTable" width="70%">
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblFltDocno" runat="server" Text="Doc No:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltDocNo" Width="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Status:"></asp:Label><br />
                            <asp:DropDownList  ID="ddlFltStatus" runat="server">
                                <asp:ListItem Text="All" Value="0" />
                                <asp:ListItem Text="Approved" Value="A" />
                                <asp:ListItem Text="Pending" Value="P" />
                                <asp:ListItem Text="Cancelled" Value="C" />    
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                OnClick="btnSearch_Click" ToolTip="Search purchase requests" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="grd" runat="server" DataKeyNames="vr_id, LocId"
                OnSelectedIndexChanged="grd_SelectedIndexChanged"
                 OnPageIndexChanging="grd_PageIndexChanging" OnRowDataBound="grd_RowDataBound"
                    AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No store transfer note found" Width="750px">
                    <Columns>
                        <asp:BoundField DataField="vr_no" HeaderText="Doc No" />
                        <asp:BoundField DataField="LocName" HeaderText="Location" />
                        <asp:BoundField DataField="vr_dt" HeaderText="Date" />      
                        <asp:BoundField DataField="vr_nrtn" HeaderText="Remarks" />                  
                        <asp:BoundField DataField="vr_apr" HeaderText="Status" />                         
                        <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                    </Columns>
                    <HeaderStyle CssClass="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                </asp:GridView>
            </td>
            <td width="1%">
            </td>
        </tr>
    </table>
    
    <br />
    <br />
    
    
    
    
    
    
    


    
    
    
</asp:Content>
