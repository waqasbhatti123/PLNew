<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="PurchReqMgt.aspx.cs" Inherits="RMS.Inv.PurchReqMgt" Culture="auto" UICulture="auto"
    EnableEventValidation="true" %>

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
        $("[id*=GridView1]input[type=text][id*=txtItem]").each(function() {
            var itm = $(this).val();
            if (itm == "") {
                $(this).closest('tr').find("input[type=text][id*=txtItem]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtItemDesc]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtUomItem]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtQOH]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtQuantity]").val('');
                $(this).closest('tr').find("input[type=text][id*=txtDueDate]").val('');
                ValidatorEnable($(this).closest('tr').find('[id*=reqQty]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqDD]')[0], false);
            }
            else {
                ValidatorEnable($(this).closest('tr').find('[id*=reqQty]')[0], true);
                ValidatorEnable($(this).closest('tr').find('[id*=reqDD]')[0], true);
            }
        });
        $("[id*=GridView1]input[type=text][id*=txtItem]").change(function(e) {
            var itm = $(this).val();
            if (itm == "") {
                $(e.target).closest('tr').find("input[type=text][id*=txtItem]").val('');
                $(e.target).closest('tr').find("input[type=text][id*=txtItemDesc]").val('');
                $(e.target).closest('tr').find("input[type=text][id*=txtUomItem]").val('');
                $(e.target).closest('tr').find("input[type=text][id*=txtQOH]").val('');
                $(e.target).closest('tr').find("input[type=text][id*=txtQuantity]").val('');
                $(e.target).closest('tr').find("input[type=text][id*=txtDueDate]").val('');
                ValidatorEnable($(e.target).closest('tr').find('[id*=reqQty]')[0], false);
                ValidatorEnable($(e.target).closest('tr').find('[id*=reqDD]')[0], false);
            }
        });
        $("[id*=GridView1]input[type=text][id*=txtItem]").autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: "PurchReqMgt.aspx/GetItemDetail",
                    data: "{ 'itemid': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    async: false,
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function(data) { return data; },
                    success: function(data) {
                        response($.map(data.d, function(item) {
                            return {
                                value: item.itm_cd + ' <> ' + item.itm_dsc + ' (' + item.uom_dsc + ')',
                                result: item.itm_cd + ' <> ' + item.itm_dsc + ' <> ' + item.uom_dsc + ' <> ' + item.stk_qty,
                                id: item.itm_cd
                            }
                        }))
                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            },

            select: function(e, ui) {
                var codeItm = ui.item.result;
                codeItm = codeItm.split(" <> ");
                if (codeItm[1] != null && codeItm[1] != "") {
                    $(e.target).closest('tr').find("input[type=text][id*=txtItem]").val(codeItm[0]);
                    $(e.target).closest('tr').find("input[type=text][id*=txtItemDesc]").val(codeItm[1]);
                    $(e.target).closest('tr').find("input[type=text][id*=txtUomItem]").val(codeItm[2]);
                    $(e.target).closest('tr').find("input[type=text][id*=txtQOH]").val(codeItm[3]);
                    ValidatorEnable($(e.target).closest('tr').find('[id*=reqQty]')[0], true);
                    ValidatorEnable($(e.target).closest('tr').find('[id*=reqDD]')[0], true);
                }
                else {
                    $(e.target).closest('tr').find("input[type=text][id*=txtItem]").val('');
                    $(e.target).closest('tr').find("input[type=text][id*=txtItemDesc]").val('');
                    $(e.target).closest('tr').find("input[type=text][id*=txtUomItem]").val('');
                    $(e.target).closest('tr').find("input[type=text][id*=txtQOH]").val('');
                    ValidatorEnable($(e.target).closest('tr').find('[id*=reqQty]')[0], false);
                    ValidatorEnable($(e.target).closest('tr').find('[id*=reqDD]')[0], false);
                }
                return false;
            },

            minLength: 1
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
            <td width="3%">
            </td>
            <td>
                <table cellspacing="2" align="center" border="0" width="98%">
                    <asp:Panel runat="server" ID="pnlMain">
                        <tr>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label6" runat="server" Text="Doc No:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocNo" runat="server" MaxLength="8" Width="98" Enabled="false"></asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtDocNo"
                                    ErrorMessage="Please enter Doc No" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator> --%>
                            </td>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label2" runat="server" Text="Status:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="RequiredFieldDropDown" Width="110px">
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
                            <td class="LblBgSetup">
                                <asp:Label ID="Lable15" runat="server" Text="Doc Ref #:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocRef" runat="server" MaxLength="20" Width="100"></asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDocRef"
                                    ErrorMessage="Please enter Doc Ref #" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator> --%>
                            </td>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label3" runat="server" Text="Date:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDte" runat="server" CssClass="RequiredFieldDate">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtDte"
                                    ErrorMessage="Please select date" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator> 
                                <ajaxToolkit:CalendarExtender ID="txtDteCal" runat="server" TargetControlID="txtDte" PopupPosition="BottomLeft" >
                                  </ajaxToolkit:CalendarExtender>
                                <span class="DteLtrl">
                                    <asp:Literal ID="Literal2"  runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                                </span>
                            </td>
                            
                        </tr>
                       <tr>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label1" runat="server" Text="Department:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDept" runat="server" CssClass="RequiredFieldDropDown"  OnSelectedIndexChanged="ddlDept_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="true">
                                    <asp:ListItem Text="Select Department" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlDept"
                                    ErrorMessage="Please select department" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator> 
                            </td>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label4" runat="server" Text="Cost Center:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCostCenter" runat="server" CssClass="RequiredFieldDropDown" AppendDataBoundItems="true">
                                    <asp:ListItem Text="Select Cost Center" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlCostCenter"
                                    ErrorMessage="Please select cost center" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator> 
                            </td>
                        </tr>
                        <tr>
                        <td class="LblBgSetup">
                            <asp:Label ID="Label7" runat="server" Text="Issued To:"></asp:Label>
                           </td>
                           <td>
                                <asp:DropDownList ID="ddlIssTo" runat="server" CssClass="RequiredFieldDropDown" AppendDataBoundItems="true">
                                    <asp:ListItem Text="Select Employee" Selected="True" Value="0">
                                    </asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlIssTo"
                                    ErrorMessage="Please select employee" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator>
                           </td>
                           <td colspan="2"></td>
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
                                                    <asp:TextBox ID="txtItem" runat="server" Text='<%#Eval("Item") %>' Width="100px" />
                                                </ItemTemplate>
                                                <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="100px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtItemDesc" runat="server" Text='<%#Eval("ItemDesc") %>' Width="300px"  TabIndex="-1"/>
                                                </ItemTemplate>
                                                <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="300px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="UOM">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtUomItem" runat="server" Text='<%#Eval("UOM") %>'  Width="50px" TabIndex="-1" />
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"  Width="50px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Q-O-H">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtQOH" runat="server" Text='<%#Eval("QOH")%>' Width="80px" style="text-align:right" TabIndex="-1"/>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"  Width="80px"/>
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
                                <div width="50%" style="text-align:left; padding-bottom:15px;">
                                    <asp:UpdatePanel ID="updButPnl" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:LinkButton runat="server" ID="addRow" Text="Add Rows" OnClick="addRow_Click" CssClass="lnk"></asp:LinkButton>               
                                    </ContentTemplate>  
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="top" colspan="4">
                                <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
                            </td>
                        </tr>
                    </asp:Panel>
                </table>
            </td>
            <td width="3%">
            </td>
        </tr>
        <tr>
            <td width="3%">
            </td>
            <td valign="top">
                <br />
                <table class="filterTable" width="80%">
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblFltName" runat="server" Text="Doc No:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltDocNo" Width="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label22" runat="server" Text="Doc Ref #:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltDocRef" Width="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Department:"></asp:Label><br />
                            <asp:DropDownList ID="ddlFltDept" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Text="All" Value="0"></asp:ListItem>
                            </asp:DropDownList>
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
                                OnClick="btnSearch_Click" ToolTip="Search demand note" />
                        </td>
                    </tr>
                </table>
                <%--<br />--%>
                <asp:GridView ID="grdPurchReq" runat="server" DataKeyNames="vr_id" OnRowDataBound="grdPurchReq_RowDataBound"
                OnSelectedIndexChanged="grdPurchReq_SelectedIndexChanged" OnPageIndexChanging="grdPurchReq_PageIndexChanging"
                    AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No demand note found" Width="750px">
                    <Columns>
                        <asp:BoundField DataField="vr_no" HeaderText="Doc No" />
                        <asp:BoundField DataField="DocRef" HeaderText="Doc Ref #" />
                        <asp:BoundField DataField="vr_dt" HeaderText="Date" />   
                        <asp:BoundField DataField="DeptNme" HeaderText="Department" />                     
                        <asp:BoundField DataField="vr_apr" HeaderText="Status" />
                        <%--<asp:BoundField DataField="cc_cd" HeaderText="Cost Center" /> --%>
                        <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
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
            <td width="3%">
            </td>
        </tr>
    </table>
</asp:Content>
