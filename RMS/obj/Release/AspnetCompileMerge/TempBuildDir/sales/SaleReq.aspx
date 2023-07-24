<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="SaleReq.aspx.cs" Inherits="RMS.sales.SaleReq" Culture="auto" UICulture="auto"
    EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

    function pageLoad() {

        $(".classDdlItem").change(function(e) {
            FillItemDetailFields($(this));

        });


        $('#<%= txtCustomer.ClientID %>').autocomplete({
            search: function(event, ui) {
            },
            source: function(request, response) {
                $.ajax({
                url: "SaleReq.aspx/GetCustomerDetail",
                    data: "{ 'custinfo': '" + request.term + "' }",
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
            $('#<%= txtCustomer.ClientID %>').val(ui.item.value);
                $("#<%= hdnCustomer.ClientID %>").val(ui.item.result)
                //alert($("#<%= hdnCustomer.ClientID %>").val());
                return false;
            },

            minLength: 1
        });




        
        function FillItemDetailFields(gRow) {
            var itemid = gRow.find('option:selected').val();
            $.ajax({
                url: "SaleReq.aspx/GetItemDetail",
                data: JSON.stringify({ itemid: itemid }),
                type: 'POST',
                contentType: 'application/json;',
                dataType: 'json',
                success: function(heads) {
                    var heads = heads.d;
                    if (heads.length > 0) {
                        gRow.closest('tr').find("input[type=text][id*=txtUomItem]").val(heads[0].uom_dsc);
                        gRow.closest('tr').find("input[type=text][id*=txtQOH]").val(heads[0].qoh);
                        ValidatorEnable(gRow.closest('tr').find('[id*=reqQty]')[0], true);
                        ValidatorEnable(gRow.closest('tr').find('[id*=reqDD]')[0], true);
                    }
                    else {
                        gRow.closest('tr').find("input[type=text][id*=txtUomItem]").val('');
                        gRow.closest('tr').find("input[type=text][id*=txtQOH]").val('');
                        ValidatorEnable(gRow.closest('tr').find('[id*=reqQty]')[0], false);
                        ValidatorEnable(gRow.closest('tr').find('[id*=reqDD]')[0], false);
                    }
                    gRow.focus();
                }
            });
        }


        $("[id*=GridView1][id*=ddlItem]").each(function() {
            if ($(this).val() != '0') {
                ValidatorEnable($(this).closest('tr').find('[id*=reqQty]')[0], true);
                ValidatorEnable($(this).closest('tr').find('[id*=reqDD]')[0], true);
                var gRow = $(this);
                var itemid = $(this).val();

                $.ajax({
                url: "SaleReq.aspx/GetItemDetail",
                    data: JSON.stringify({ itemid: itemid }),
                    type: 'POST',
                    contentType: 'application/json;',
                    dataType: 'json',
                    success: function(heads) {
                        var heads = heads.d;
                        if (heads.length > 0) {
                            gRow.closest('tr').find("input[type=text][id*=txtUomItem]").val(heads[0].uom_dsc);
                            gRow.closest('tr').find("input[type=text][id*=txtQOH]").val(heads[0].qoh);
                        }
                        else {
                            gRow.closest('tr').find("input[type=text][id*=txtUomItem]").val('');
                            gRow.closest('tr').find("input[type=text][id*=txtQOH]").val('');
                        }
                    }
                });
            }
            else {
                ValidatorEnable($(this).closest('tr').find('[id*=reqQty]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqDD]')[0], false);
            }
        });
       
    }
   
</script>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
    <asp:Panel ID="pnlTop" runat="server" Visible="false">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="1%">
            </td>
            <td>
                <table class="tblMain" cellspacing="0" cellpadding="0">
                    <asp:Panel runat="server" ID="Panel1">
                         <tr>
                            <td  class="tdlabel">
                                <asp:Label ID="Label7" runat="server" Text="Customer:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtCustomer" runat="server" Width="70%"></asp:TextBox>
                                <asp:HiddenField ID="hdnCustomer" runat="server" />
                            </td>
                         </tr>
                         <tr>
                            <td  class="tdlabel">
                                <asp:Label ID="Label9" runat="server" Text="User:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtUser" runat="server" Width="40%" ReadOnly="true" TabIndex="-1"></asp:TextBox>
                            </td>
                         </tr>
                         <tr>
                            <td  class="tdlabel">
                                <asp:Label ID="Label6" runat="server" Text="Doc No:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocNo" runat="server" MaxLength="8" Width="98" Enabled="false"></asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtDocNo"
                                    ErrorMessage="Please enter Doc No" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator> --%>
                            </td>
                            <td  class="tdlabel">
                                <asp:Label ID="Label2" runat="server" Text="Status:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="110px">
                                    <asp:ListItem Text="Select Status" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Confirmed" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="Pending" Value="P" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Cancelled" Value="C"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlStatus"
                                    ErrorMessage="Please select status" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator> 
                            </td>
                        </tr>
                        <tr>
                            <td  class="tdlabel">
                                <asp:Label ID="Lable15" runat="server" Text="Doc Ref #:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocRef" runat="server" MaxLength="20" Width="100"></asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDocRef"
                                    ErrorMessage="Please enter Doc Ref #" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator> --%>
                            </td>
                            <td  class="tdlabel">
                                <asp:Label ID="Label3" runat="server" Text="Date:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDte" runat="server" Width="80px">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtDte"
                                    ErrorMessage="Please select date" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator> 
                                <ajaxToolkit:CalendarExtender ID="txtDteCal" runat="server" TargetControlID="txtDte" PopupPosition="BottomLeft" >
                                  </ajaxToolkit:CalendarExtender>
                                <span class="tdlitrel">
                                    <asp:Literal ID="Literal2"  runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                        
                                <asp:UpdatePanel ID="uPnl" runat="server" UpdateMode="Conditional">
                                      <ContentTemplate>
                                  
                                  <table class="table">
                                  <tr>
                                  <td>

                                        <asp:GridView ID="GridView1" runat="server" CssClass="t_grd" Width="100%" AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound">
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
                                                      <asp:Label runat="server" ID="lblSr" Text='<%#Eval("Sr") %>' Width="20px">
                                                      </asp:Label>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="20px"/>
                                              </asp:TemplateField>
                                                 
                                              <asp:TemplateField HeaderText="Item*">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlItem" runat="server" AppendDataBoundItems="true" CssClass="classDdlItem">
                                                        <asp:ListItem Selected="True" Text="Select Item" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="UOM">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtUomItem" runat="server" Text='<%#Eval("UOM") %>'  Width="60px" ReadOnly="true" TabIndex="-1" />
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"  Width="60px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Qty Reqd.*">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtQuantity" runat="server" Text='<%#Eval("Quantity")%>' Width="80px" style="text-align:right"/>
                                                    <asp:RangeValidator runat="server" ID="rngQty" ValidationGroup="main" ErrorMessage="Invalid/Out of range Qty Reqd" SetFocusOnError="true" CssClass="validateGridView"
                                                        ControlToValidate="txtQuantity" Display="Dynamic" MinimumValue="000000000.00" MaximumValue="999999999.99" Type="Double" ></asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="reqQty" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtQuantity" ErrorMessage="Qty Reqd" Enabled="false"></asp:RequiredFieldValidator>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"  Width="80px"/>
                                              </asp:TemplateField>

                                              <asp:TemplateField HeaderText="Due Date*">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtDueDate" runat="server" Text='<%#Eval("DueDate")%>' Width="80px" />
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender23" runat="server" TargetControlID="txtDueDate" PopupPosition="BottomLeft" Format="dd-MMM-yy"></ajaxToolkit:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="reqDD" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtDueDate" ErrorMessage="Due Date Reqd" Enabled="false"></asp:RequiredFieldValidator>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"  Width="80px"/>
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
                </table>
             </td>
             <td width="1%">
            </td>
         </tr>
         <tr>
            <td width="1%">
            </td>
                <td align="center" valign="top" colspan="4">
                    <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
                </td>
             <td width="1%">
            </td>
         </tr>
     </table>
   </asp:Panel>
   
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="1%">
            </td>
            <td valign="top">
                <asp:Panel ID="pnlFlt" runat="server" DefaultButton="btnSearch">
                <table class="filterTable" width="80%">
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Customer:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltCust" MaxLength="12" Width="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblFltName" runat="server" Text="Doc No:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltDocNo" MaxLength="100" Width="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label22" runat="server" Text="Doc Ref #:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltDocRef" MaxLength="100" Width="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="From Date:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltFromDt" Width="100"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="calFltFromDt" runat="server" TargetControlID="txtFltFromDt" PopupPosition="BottomLeft" >
                            </ajaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="To Date:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltToDt" Width="100"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="calFltToDt" runat="server" TargetControlID="txtFltToDt" PopupPosition="BottomLeft" >
                            </ajaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Status:"></asp:Label><br />
                            <asp:DropDownList  ID="ddlFltStatus" runat="server">
                                <asp:ListItem Text="All" Value="0" />
                                <asp:ListItem Text="Confirmed" Value="A" />
                                <asp:ListItem Text="Pending" Value="P" />
                                <asp:ListItem Text="Cancelled" Value="C" />    
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                OnClick="btnSearch_Click" ToolTip="Search demand note" />
                        </td>
                    </tr>
                </table>
                </asp:Panel>
                <%--<br />--%>
                <asp:GridView ID="grdPurchReq" runat="server" DataKeyNames="vr_id" OnRowDataBound="grdPurchReq_RowDataBound"
                OnSelectedIndexChanged="grdPurchReq_SelectedIndexChanged" OnPageIndexChanging="grdPurchReq_PageIndexChanging"
                    AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No sale request found" Width="750px">
                    <Columns>
                        <asp:BoundField DataField="gl_dsc" HeaderText="Customer" />
                        <asp:BoundField DataField="vr_no" HeaderText="Doc No" />
                        <asp:BoundField DataField="DocRef" HeaderText="Doc Ref #" />
                        <asp:BoundField DataField="vr_dt" HeaderText="Date" />                    
                        <asp:BoundField DataField="vr_apr" HeaderText="Status" />
                        <%--<asp:BoundField DataField="cc_cd" HeaderText="Cost Center" /> --%>
                        <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkApprove" runat="server" Text="Approve" OnClick="lnkApprove_Click">
                                </asp:LinkButton>
                            </ItemTemplate>
                            <ControlStyle CssClass="lnk"/>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPrint" runat="server" Text="Print" OnClick="lnkPrint_Click">
                                </asp:LinkButton>
                            </ItemTemplate>
                            <ControlStyle CssClass="lnk"/>
                        </asp:TemplateField>
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
</asp:Content>
