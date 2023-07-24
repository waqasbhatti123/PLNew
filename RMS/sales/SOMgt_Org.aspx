<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="SOMgt_Org.aspx.cs" Inherits="RMS.sales.SOMgt_Org" Culture="auto" UICulture="auto"
    EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">

    function pageLoad() {


        $('#<%= txtSalesPerson.ClientID %>').autocomplete({
            search: function(event, ui) {
            },
            source: function(request, response) {
                $.ajax({
                    url: "SOMgt.aspx/GetSalesPersonDetail",
                    data: "{ 'salespersoninfo': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    async: false,
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function(data) { return data; },
                    success: function(data) {
                        response($.map(data.d, function(item) {
                            return {
                                value: item.info,
                                result: item.ID
                            }
                        }))
                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            },

            select: function(e, ui) {
                $('#<%= txtSalesPerson.ClientID %>').val(ui.item.value);
                $("#<%= hdnSalesPerson.ClientID %>").val(ui.item.result)
                //alert($("#<%= hdnSalesPerson.ClientID %>").val());
                return false;
            },

            minLength: 1
        });
    
        

        $("[id*=GridView1][id*=ddlItem]").each(function() {
            if ($(this).val() != '0') {
                ValidatorEnable($(this).closest('tr').find('[id*=reqOrderQty]')[0], true);
                ValidatorEnable($(this).closest('tr').find('[id*=reqRate]')[0], true);
                var gRow = $(this);
                var itemid = $(this).val();

                $.ajax({
                    url: "SOMgt.aspx/GetItemDetail",
                    data: JSON.stringify({ itemid: itemid }),
                    type: 'POST',
                    contentType: 'application/json;',
                    dataType: 'json',
                    success: function(heads) {
                        var heads = heads.d;
                        if (heads.length > 0) {
                            
                        }
                        else {
                           
                        }
                    }
                });
            }
            else {
                ValidatorEnable($(this).closest('tr').find('[id*=reqOrderQty]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqRate]')[0], false);
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
                    ValidatorEnable(gRow.closest('tr').find('[id*=reqOrderQty]')[0], true);
                    ValidatorEnable(gRow.closest('tr').find('[id*=reqRate]')[0], true);
                }
                else {
                    gRow.closest('tr').find("input[type=text][id*=txtUomItem]").val('');
                    ValidatorEnable(gRow.closest('tr').find('[id*=reqOrderQty]')[0], false);
                    ValidatorEnable(gRow.closest('tr').find('[id*=reqRate]')[0], false);
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
        <td width="1%"></td>
            <td>
                <table  cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="100%">
                            Search Document: &nbsp;
                            <asp:TextBox ID="txtSrchDoc" runat="server" Width="100px" ></asp:TextBox>
                            <asp:ImageButton ID="btnSrchDoc" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                OnClick="btnSrchDoc_Click" ToolTip="Search sale request"/>
                            <br />
                            <asp:GridView ID="grdSrchDoc" runat="server" DataKeyNames="vr_id" Width="100%" AutoGenerateColumns="false" CssClass="t_grd" 
                                          OnRowDataBound="grdSrchDoc_RowDataBound"
                                          OnSelectedIndexChanged="grdSrchDoc_SelectedIndexChanged">
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
                                  <asp:BoundField HeaderText="Document No" DataField="vr_no_formtd"/>
                                  <asp:BoundField HeaderText="Document Date" DataField="vr_dt"/>
                                  <asp:CommandField ShowSelectButton="true" SelectText="Select" ControlStyle-CssClass="lnk"/>
                                </Columns>
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
                <table class="tblMain" cellspacing="0" cellpadding="0">
                    <asp:Panel runat="server" ID="pnlMain">
                        <tr>
                            <td class="tdlabel">
                                <asp:Label ID="Label26" runat="server" Text="Order No:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderNo" runat="server" ReadOnly="true"  Width="80px" TabIndex="-1">
                                </asp:TextBox>
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
                                <asp:Label ID="Label1" runat="server" Text="Party:"></asp:Label>
                            </td>
                           <td>
                                <asp:DropDownList ID="ddlParty" runat="server" AppendDataBoundItems="true">
                                    <asp:ListItem Value="0">Select Party</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlParty"
                                    ErrorMessage="Please select Party" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator> 
                           </td> 
                            <td class="tdlabel">
                                <asp:Label ID="Label4" runat="server" Text="Order Date:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderDate" runat="server" Width="80px">
                                </asp:TextBox>
                                
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtOrderDate"
                                    ErrorMessage="Please select order date" SetFocusOnError="true" ValidationGroup="main" Display="None" ></asp:RequiredFieldValidator>
                                <span class="tdlitrel">
                                    <asp:Literal ID="Literal2"  runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                                </span>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtOrderDate" PopupPosition="BottomRight" >
                                  </ajaxToolkit:CalendarExtender>
                                
                            </td>
                        </tr>
                         <tr>
                            <td class="tdlabel">
                                <asp:Label ID="Label122" runat="server" Text="Sales Person:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSalesPerson" runat="server" Width="200px"></asp:TextBox>
                                <asp:HiddenField ID="hdnSalesPerson" runat="server" />
                            </td>
                           <td class="tdlabel">
                                <asp:Label ID="Label2" runat="server" Text="Ship To:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtShipTo" runat="server" Width="150px" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                         <tr> 
                           <td class="tdlabel">
                                <asp:Label ID="Label6" runat="server" Text="Delivery Period:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDeliveryPeriod" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
                                <span class="tdlitrel">
                                    <asp:Literal ID="Literal3"  runat="server" Text="Days" />
                                </span>
                                <asp:RangeValidator runat="server" ID="reqDP" ValidationGroup="main" ErrorMessage="Invalid/Out of range Delivery Period"
                                                        ControlToValidate="txtDeliveryPeriod" MinimumValue="1" MaximumValue="999" Type="Integer" Display="None" SetFocusOnError="true"></asp:RangeValidator>
                            </td>
                            <td class="tdlabel">
                                <asp:Label ID="Label8" runat="server" Text="Qauntity Variance:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtQtyVariance" runat="server" Width="50px" MaxLength="6"></asp:TextBox>
                                <span class="tdlitrel">
                                    <asp:Literal ID="Literal4"  runat="server" Text="%" />
                                </span>
                                <asp:RangeValidator runat="server" ID="rvQV" ValidationGroup="main" ErrorMessage="Invalid/Out of range Quantity Variance"
                                                        ControlToValidate="txtQtyVariance" MinimumValue="00.000" MaximumValue="99.999" Type="Double" Display="None" SetFocusOnError="true"></asp:RangeValidator>
                            </td>
                        </tr>
                         <tr>
                            <td class="tdlabel">
                                <asp:Label ID="Label7" runat="server" Text="Buyer Reference:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBuyerRef" runat="server" Width="100px" MaxLength="20"></asp:TextBox>
                            </td>
                           <td class="tdlabel">
                                <asp:Label ID="Label11" runat="server" Text="Buyer Ref. Date:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBuyerRefDate" runat="server" Width="80px"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtBuyerRefDate" PopupPosition="BottomRight" >
                                  </ajaxToolkit:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdlabel">
                                <asp:Label ID="Label3" runat="server" Text="Discount %:"></asp:Label>
                            </td>
                           <td>
                                <asp:TextBox ID="txtDiscountPercent" runat="server" Width="30px"></asp:TextBox>
                                <span class="tdlitrel">
                                    <asp:Literal ID="Literal1"  runat="server" Text="%" />
                                </span>
                                <asp:RangeValidator runat="server" ID="rvDP" ValidationGroup="main" ErrorMessage="Invalid/Out of range Discount Percent"
                                                        ControlToValidate="txtDiscountPercent" MinimumValue="00.00" MaximumValue="99.99" Type="Double" Display="None" SetFocusOnError="true"></asp:RangeValidator>
                           </td>
                           <td class="tdlabel">
                                <asp:Label ID="Label5" runat="server" Text="Paid Amount:"></asp:Label>
                            </td>
                           <td>
                                <asp:TextBox ID="txtPaidAmount" runat="server" Width="80px" style="text-align:right"></asp:TextBox>
                                <asp:RangeValidator runat="server" ID="rvPA" ValidationGroup="main" ErrorMessage="Invalid/Out of range Paid Amount"
                                                        ControlToValidate="txtPaidAmount" MinimumValue="0" MaximumValue="999999999" Type="Integer" Display="None" SetFocusOnError="true"></asp:RangeValidator>
                           </td>
                        </tr>
                        <tr>
                            <td class="tdlabel">
                                <asp:Label ID="Label12" runat="server" Text="Bill Terms:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBillTerms" runat="server" TextMode="MultiLine"
                                 onkeyup="LimitText(this,100);" onblur="LimitText(this,100);"></asp:TextBox>
                            </td>
                            <td class="tdlabel">
                                <asp:Label ID="Label13" runat="server" Text="Payment Terms:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentTerms" runat="server" TextMode="MultiLine"
                                 onkeyup="LimitText(this,100);" onblur="LimitText(this,100);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdlabel">
                                <asp:Label ID="Label14" runat="server" Text="Delivery Terms:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDeliveryTerms" runat="server" TextMode="MultiLine"
                                 onkeyup="LimitText(this,100);" onblur="LimitText(this,100);"></asp:TextBox>
                            </td>
                            <td class="tdlabel">
                                <asp:Label ID="Label15" runat="server" Text="Remarks:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"
                                 onkeyup="LimitText(this,100);" onblur="LimitText(this,100);"></asp:TextBox>
                            </td>
                        </tr>
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
                                                                  
                                              <asp:TemplateField HeaderText="UOM">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtUomItem" runat="server" Text='<%#Eval("UOMItem") %>' TabIndex="-1" Width="50px"/>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="50px"/>
                                              </asp:TemplateField>
                                              
                             
                                              <asp:TemplateField HeaderText="Order Quantity">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtOrderQty" runat="server" Text='<%#Eval("OrderQty") %>' Width="80px" style="text-align:right"/>
                                                    <asp:RequiredFieldValidator ID="reqOrderQty" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtOrderQty" ErrorMessage="Order Quantity required" Enabled="false"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator runat="server" ID="rvTQ" CssClass="validateGridView" ValidationGroup="main" ErrorMessage="Invalid/Out of range Order Quantity"
                                                        ControlToValidate="txtOrderQty" MinimumValue="1.00" MaximumValue="999999999.99" Type="Double" Display="Dynamic" SetFocusOnError="true"></asp:RangeValidator>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="80px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Rate">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtRate" runat="server" Text='<%#Eval("Rate") %>' Width="80px" style="text-align:right"/>
                                                    <asp:RequiredFieldValidator ID="reqRate" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtRate" ErrorMessage="Rate required" Enabled="false"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator runat="server" ID="rvRate" CssClass="validateGridView" ValidationGroup="main" ErrorMessage="Invalid/Out of range Rate"
                                                        ControlToValidate="txtRate" MinimumValue="1" MaximumValue="99999999" Type="Double" Display="Dynamic" SetFocusOnError="true"></asp:RangeValidator>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="80px"/>
                                              </asp:TemplateField>
                                                                     
                                              <asp:TemplateField HeaderText="Schedule  Date">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtSchDate" runat="server" Text='<%#Eval("SchDate") %>' Width="80px"/>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtSchDate" Format="dd-MMM-yyyy" PopupPosition="BottomRight" >
                                                    </ajaxToolkit:CalendarExtender>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="80px"/>
                                              </asp:TemplateField>
                                                                    
                                              <asp:TemplateField HeaderText="Remarks">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("Rem")%>' Width="150" MaxLength="70"/>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="150px"/>
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
                        </td>
                        <td>
                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                OnClick="btnSearch_Click" ToolTip="Search sales order" />
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="grd" runat="server" DataKeyNames="OrderID"
                OnSelectedIndexChanged="grd_SelectedIndexChanged"
                 OnPageIndexChanging="grd_PageIndexChanging" OnRowDataBound="grd_RowDataBound"
                    AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No sales order found" Width="100%">
                    <Columns>
                        <asp:BoundField DataField="OrderNo" HeaderText="Order No" />
                        <asp:BoundField DataField="OrderDate" HeaderText="Order Date" />
                        <asp:BoundField DataField="gl_dsc" HeaderText="Party" />
                        <asp:BoundField DataField="SalesPerson" HeaderText="Sales Person" />      
                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" />                  
                        <asp:TemplateField HeaderText="Sales Order">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSalesOrder" runat="server" Text="Print" OnClick="lnkSalesOrder_Click" CssClass="lnk"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
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
