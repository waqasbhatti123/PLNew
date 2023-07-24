<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="PTNMgt_Org.aspx.cs" Inherits="RMS.sales.PTNMgt_Org" Culture="auto" UICulture="auto"
    EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">

    function pageLoad() {

        $("[id*=GridView1][id*=ddlItem]").each(function() {
            if ($(this).val() != '0') {
                ValidatorEnable($(this).closest('tr').find('[id*=reqTrnsferQty]')[0], true);
                ValidatorEnable($(this).closest('tr').find('[id*=reqRcvdQty]')[0], true);
                var gRow = $(this);
                var itemid = $(this).val();

                $.ajax({
                    url: "PTNMgt.aspx/GetItemDetail",
                    data: JSON.stringify({ itemid: itemid }),
                    type: 'POST',
                    contentType: 'application/json;',
                    dataType: 'json',
                    success: function(heads) {
                        var heads = heads.d;
                        if (heads.length > 0) {
                            if (heads[0].Batch == false) {
                                ValidatorEnable(gRow.closest('tr').find('[id*=reqBatch]')[0], false);
                            }
                            else {
                                ValidatorEnable(gRow.closest('tr').find('[id*=reqBatch]')[0], true);
                            }
                        }
                        else {
                            ValidatorEnable(gRow.closest('tr').find('[id*=reqBatch]')[0], false);
                        }
                    }
                });
            }
            else {
                ValidatorEnable($(this).closest('tr').find('[id*=reqTrnsferQty]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqRcvdQty]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqBatch]')[0], false);
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
            url: "PTNMgt.aspx/GetItemDetail",
            data: JSON.stringify({ itemid: itemid }),
            type: 'POST',
            contentType: 'application/json;',
            dataType: 'json',
            success: function(heads) {
                var heads = heads.d;
                if (heads.length > 0) {
                    gRow.closest('tr').find("input[type=text][id*=txtUomItem]").val(heads[0].uom_dsc);
                    ValidatorEnable(gRow.closest('tr').find('[id*=reqTrnsferQty]')[0], true);
                    ValidatorEnable(gRow.closest('tr').find('[id*=reqRcvdQty]')[0], true);
                    if (heads[0].Batch == false) {
                        ValidatorEnable(gRow.closest('tr').find('[id*=reqBatch]')[0], false);
                    }
                    else {
                        ValidatorEnable(gRow.closest('tr').find('[id*=reqBatch]')[0], true);
                    }
                }
                else {
                    gRow.closest('tr').find("input[type=text][id*=txtUomItem]").val('');
                    ValidatorEnable(gRow.closest('tr').find('[id*=reqTrnsferQty]')[0], false);
                    ValidatorEnable(gRow.closest('tr').find('[id*=reqRcvdQty]')[0], false);
                    ValidatorEnable(gRow.closest('tr').find('[id*=reqBatch]')[0], false);
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
            <td width="1%">
            </td>
            <td>
                <table class="tblMain" cellspacing="0" cellpadding="0">
                    <asp:Panel runat="server" ID="pnlMain">
                        <tr>
                            <td class="tdlabel">
                                <asp:Label ID="lblloc" runat="server" Text="To Location:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLoc" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Text="Select Store Location" Selected="True" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlLoc"
                                    ErrorMessage="Please select store location" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator> 
                            </td>
                            <td class="tdlabel">
                                <asp:Label ID="Label9" runat="server" Text="Status:"></asp:Label>
                            </td>
                            <td>
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
                                <asp:Label ID="Label26" runat="server" Text="Document No:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocNo" runat="server" ReadOnly="true"  Width="80px" TabIndex="-1">
                                </asp:TextBox>
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
                                <asp:Label ID="Label10" runat="server" Text="Remarks:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"
                                 onkeyup="LimitText(this,100);" onblur="LimitText(this,100);"></asp:TextBox>
                            </td>
                            <td>
                                
                            </td>
                            <td>
                            
                            </td>
                        </tr>
                        <%--<tr><td colspan="4">&nbsp;</td></tr>--%>
                        <tr>
                        
                          <td colspan="4">
                        
                                <asp:UpdatePanel ID="uPnl" runat="server" UpdateMode="Conditional">
                                      <ContentTemplate>
                                      
                                      <table class="table">
                        <tr>
                            <td align="center">
                                        <asp:GridView ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="false" CssClass="t_grd" OnRowDataBound="GridView1_RowDataBound">
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
                                                      <asp:Label runat="server" ID="lblSr" Text='<%#Eval("Sr") %>' Width="20px" TabIndex="-1">
                                                      </asp:Label>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="20px"/>
                                              </asp:TemplateField>
                                                                                                        
                                              <asp:TemplateField HeaderText="Item">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlItem" runat="server" AppendDataBoundItems="true" CssClass="classDdlItem" Width="200px">
                                                        <asp:ListItem Selected="True" Text="Select Item" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ControlStyle />
                                                <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="200px"/>
                                              </asp:TemplateField>
                                                                                            
                                              <asp:TemplateField HeaderText="Batch">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtBatch" runat="server" Text='<%#Eval("Batch") %>' MaxLength="20" Width="60px"/>
                                                    <asp:RequiredFieldValidator ID="reqBatch" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtBatch" ErrorMessage="Batch required" Enabled="false"></asp:RequiredFieldValidator>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="UOM">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtUomItem" runat="server" Text='<%#Eval("UOMItem") %>' TabIndex="-1" Width="50px"/>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="50px"/>
                                              </asp:TemplateField>
                                                                                 
                                              <asp:TemplateField HeaderText="Trnsfr Qty">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtTrnsfrQty" runat="server" Text='<%#Eval("TrnsfrQty") %>' Width="60px" style="text-align:right"/>
                                                    <asp:RequiredFieldValidator ID="reqTrnsferQty" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtTrnsfrQty" ErrorMessage="Transfer Quantity required" Enabled="false"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator runat="server" ID="rvTQ" CssClass="validateGridView" ValidationGroup="main" ErrorMessage="Invalid/Out of range Transfer Quantity"
                                                        ControlToValidate="txtTrnsfrQty" MinimumValue="1.00" MaximumValue="999999999.99" Type="Double" Display="Dynamic" SetFocusOnError="true"></asp:RangeValidator>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Rcvd Qty">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtRcvdQty" runat="server" Text='<%#Eval("RcvdQty") %>' Width="60px" style="text-align:right"/>
                                                    <asp:RequiredFieldValidator ID="reqRcvdQty" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtRcvdQty" ErrorMessage="Received Quantity required" Enabled="false"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator runat="server" ID="rvRQ" CssClass="validateGridView" ValidationGroup="main" ErrorMessage="Invalid/Out of range Received Quantity"
                                                        ControlToValidate="txtRcvdQty" MinimumValue="1.00" MaximumValue="999999999.99" Type="Double" Display="Dynamic" SetFocusOnError="true"></asp:RangeValidator>
                                                    <asp:CompareValidator runat="server" ID="cvRQ" CssClass="validateGridView" ValidationGroup="main" ErrorMessage="Received Quantity must be less than equal to Transfer Quantity"
                                                        ControlToValidate="txtRcvdQty" ControlToCompare="txtTrnsfrQty" Type="Double" Operator="LessThanEqual" Display="Dynamic" SetFocusOnError="true"></asp:CompareValidator>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Remarks">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("Rem")%>' Width="210" MaxLength="100"/>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="210px"/>
                                              </asp:TemplateField>
                                          </Columns>
                                        </asp:GridView>
                                        </td>
                                        </tr>
                                        </table>
                                </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger  ControlID="addRow"/> 
                            </Triggers>
                        </asp:UpdatePanel>
                         </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <div style="float:right">
                                        <asp:UpdatePanel ID="updButPnl" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:LinkButton runat="server" ID="addRow" Text="Add Rows" OnClick="addRow_Click" CssClass="lnk"></asp:LinkButton>               
                                            </ContentTemplate>  
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                                 
                        </asp:Panel>
                        
                        <tr>
                            <td align="center" valign="top" colspan="4">
                                <br />
                                <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
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
                <table class="filterTable" width="60%">
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblFltDocno" runat="server" Text="Document No:"></asp:Label>
                            <asp:TextBox runat="server" ID="txtFltDocNo" Width="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Status:"></asp:Label>
                            <asp:DropDownList  ID="ddlFltStatus" runat="server">
                                <asp:ListItem Text="All" Value="0" />
                                <asp:ListItem Text="Approved" Value="A" />
                                <asp:ListItem Text="Pending" Value="P" />
                                <asp:ListItem Text="Cancelled" Value="C" />    
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                OnClick="btnSearch_Click" ToolTip="Search production transfer note" />
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="grd" runat="server" DataKeyNames="vr_id, LocId"
                OnSelectedIndexChanged="grd_SelectedIndexChanged"
                 OnPageIndexChanging="grd_PageIndexChanging" OnRowDataBound="grd_RowDataBound"
                    AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No production transfer note found" Width="100%">
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
