<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="AdjNote.aspx.cs" Inherits="RMS.Inv.AdjNote" Culture="auto" UICulture="auto"
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

        $(".classDdlItem").change(function(e) {
            FillItemDetailFields($(this));

        });

        function FillItemDetailFields(gRow) {
            var itemid = gRow.find('option:selected').val();
            if (itemid != '0') {
                ValidatorEnable(gRow.closest('tr').find('[id*=reqAdjQty]')[0], true);
                ValidatorEnable(gRow.closest('tr').find('[id*=reqAdjVal]')[0], true);
            }
            else {
                ValidatorEnable(gRow.closest('tr').find('[id*=reqAdjQty]')[0], false);
                ValidatorEnable(gRow.closest('tr').find('[id*=reqAdjVal]')[0], false);
            }
        }


        $("[id*=GridView1][id*=ddlItem]").each(function() {
            if ($(this).val() != '0') {
                ValidatorEnable($(this).closest('tr').find('[id*=reqAdjQty]')[0], true);
                ValidatorEnable($(this).closest('tr').find('[id*=reqAdjVal]')[0], true);
            }
            else {
                ValidatorEnable($(this).closest('tr').find('[id*=reqAdjQty]')[0], false);
                ValidatorEnable($(this).closest('tr').find('[id*=reqAdjVal]')[0], false);
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
            <td width="3%">
            </td>
            <td>
                <table cellspacing="2" align="center" border="0" width="98%">
                    <asp:Panel runat="server" ID="pnlMain">
                        <tr>
                            <td class="LblBgSetup">
                                <asp:Label ID="lblloc" runat="server" Text="Loc:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLoc" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlLoc_SelectedIndexChanged" CssClass="RequiredFieldDropDown">
                                <asp:ListItem Text="Select Store Location" Selected="True" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlLoc"
                                    ErrorMessage="Please select store location" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator> 
                            </td>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label9" runat="server" Text="Status:"></asp:Label>
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
                                <asp:Label ID="Label26" runat="server" Text="Doc No:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocNo" runat="server" Width="80" Enabled="false">
                                </asp:TextBox>
                            </td>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label25" runat="server" Text="Doc Ref #:" Visible="false"></asp:Label>
                                <asp:Label ID="Label4" runat="server" Text="Doc Date:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocDate" runat="server" CssClass="RequiredFieldDate">
                                </asp:TextBox>
                                
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDocDate"
                                    ErrorMessage="Please select date" SetFocusOnError="true" ValidationGroup="main" Display="None" ></asp:RequiredFieldValidator>
                                <span class="DteLtrl">
                                    <asp:Literal ID="Literal2"  runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                                </span>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDocDate" PopupPosition="BottomRight" >
                                  </ajaxToolkit:CalendarExtender>
                                
                            </td>
                        </tr>
                        <tr valign="top">
                             <td class="LblBgSetup">
                                <asp:Label ID="Label10" runat="server" Text="Remarks:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" width="182" height="67" 
                                 onkeyup="LimitText(this,100);" onblur="LimitText(this,100);" CssClass="txtRemz"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr><td colspan="4">&nbsp;</td></tr>
                        <tr>
                        
                          <td colspan="4">
                        
                                <asp:UpdatePanel ID="uPnl" runat="server" UpdateMode="Conditional">
                                      <ContentTemplate>
                                      
                                      <table class="table">
                                      <tr>
                                      <td>
                                      
                                        <asp:GridView ID="GridView1" runat="server"  CssClass="t_grd" Width="100%" AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound">
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
                                                    <asp:DropDownList ID="ddlItem" runat="server" AppendDataBoundItems="true" Width="250px" CssClass="classDdlItem" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Selected="True" Text="Select Item" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"/>
                                              </asp:TemplateField>
                                              
                                              
                                              
                                              <asp:TemplateField HeaderText="UOM">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtUomItem" runat="server" Text='<%#Eval("UOMItem") %>' Width="50px" ReadOnly="true" TabIndex="-1"/>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="50px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Qty on Hand">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtQtyHand" runat="server" Text='<%#Eval("QtyHand") %>' Width="80px" style="text-align:right;" ReadOnly="true" TabIndex="-1"  />
                                                    <%--<asp:RangeValidator runat="server" ID="rv5" ValidationGroup="main" ErrorMessage="Invalid/Out of range Qty on Hand"
                                                        ControlToValidate="txtQtyHand" Display="Dynamic" CssClass="validateGridView" MinimumValue="1.00" MaximumValue="99999999999.99" Type="Double"></asp:RangeValidator>--%>
                                                    <%--<asp:CompareValidator runat="server" ID="cmpNumbers"  Display="Dynamic" CssClass="validateGridView" ControlToValidate="txtQtyIss" ControlToCompare="txtQtyHand" Operator="LessThanEqual" 
                                                        type="Double" ErrorMessage="Qty Issued should be less than or equal to Qty on Hand" SetFocusOnError="true" ValidationGroup="main"/>--%>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="80px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Value on Hand">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtValHand" runat="server" Text='<%#Eval("ValHand") %>' Width="80px" style="text-align:right;" ReadOnly="true" TabIndex="-1"  />
                                                    <%--<asp:RangeValidator runat="server" ID="rv5" ValidationGroup="main" ErrorMessage="Invalid/Out of range Qty on Hand"
                                                        ControlToValidate="txtValHand" Display="Dynamic" CssClass="validateGridView" MinimumValue="1.00" MaximumValue="99999999999.99" Type="Double"></asp:RangeValidator>--%>
                                                    <%--<asp:CompareValidator runat="server" ID="cmpNumbers"  Display="Dynamic" CssClass="validateGridView" ControlToValidate="txtQtyIss" ControlToCompare="txtValHand" Operator="LessThanEqual" 
                                                        type="Double" ErrorMessage="Qty Issued should be less than or equal to Qty on Hand" SetFocusOnError="true" ValidationGroup="main"/>--%>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="80px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Adjustment Qty">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtAdjQty" runat="server" Text='<%#Eval("AdjQty")%>' Width="80px" style="text-align:right;"/>
                                                    <asp:RangeValidator runat="server" ID="rv4" ValidationGroup="main" ErrorMessage="Invalid/Out of range Adjustment Qty" SetFocusOnError="true"
                                                        ControlToValidate="txtAdjQty" Display="Dynamic" CssClass="validateGridView" MinimumValue="-99999999999.999" MaximumValue="99999999999.999" Type="Double" ></asp:RangeValidator>
                                                    <%--<asp:CompareValidator runat="server" ID="cmpAdjQty"  Display="Dynamic" CssClass="validateGridView" ControlToValidate="txtAdjQty" ControlToCompare="txtQtyHand" Operator="LessThanEqual" 
                                                        type="Double" ErrorMessage="Adjustment Qty should be less than or equal to Qty on Hand" SetFocusOnError="true" ValidationGroup="main"/>--%>
                                                    <asp:RequiredFieldValidator ID="reqAdjQty" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtAdjQty" ErrorMessage="Qty Reqd" Enabled="false"></asp:RequiredFieldValidator>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="80px"/>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Adjustment Value">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtAdjVal" runat="server" Text='<%#Eval("AdjVal")%>' Width="80px" style="text-align:right;"/>
                                                    <asp:RangeValidator runat="server" ID="rv10" ValidationGroup="main" ErrorMessage="Invalid/Out of range Adjustment Value" SetFocusOnError="true"
                                                        ControlToValidate="txtAdjVal" Display="Dynamic" CssClass="validateGridView" MinimumValue="-99999999999.999" MaximumValue="99999999999.999" Type="Double" ></asp:RangeValidator>
                                                    <%--<asp:CompareValidator runat="server" ID="cmpAdjVal"  Display="Dynamic" CssClass="validateGridView" ControlToValidate="txtAdjVal" ControlToCompare="txtValHand" Operator="LessThanEqual" 
                                                        type="Double" ErrorMessage="Adjustment Value should be less than or equal to Value on Hand" SetFocusOnError="true" ValidationGroup="main"/>--%>
                                                    <asp:RequiredFieldValidator ID="reqAdjVal" runat="server" Display="Dynamic" ValidationGroup="main" CssClass="validateGridView"
                                                        ControlToValidate="txtAdjVal" ErrorMessage="Value Reqd" Enabled="false"></asp:RequiredFieldValidator>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="80px"/>
                                              </asp:TemplateField>

                                              <asp:TemplateField HeaderText="Remarks">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("Rem")%>' MaxLength="99" Width="100px"/>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="100px"/>
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
                        
                        </asp:Panel>
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
                                <br />
                                <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
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
                <asp:GridView ID="grdAdj" runat="server" DataKeyNames="vr_id, LocId"
                OnSelectedIndexChanged="grdAdj_SelectedIndexChanged"
                 OnPageIndexChanging="grdAdj_PageIndexChanging" OnRowDataBound="grdAdj_RowDataBound"
                    AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No adjustment note found" Width="750px">
                    <Columns>
                        <asp:BoundField DataField="vr_no" HeaderText="Doc No" />
                        <asp:BoundField DataField="LocName" HeaderText="Location" />
                        <asp:BoundField DataField="vr_dt" HeaderText="Date" />                        
                        <asp:BoundField DataField="vr_apr" HeaderText="Status" />                       
                        <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                        <%--<asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPrint" runat="server" Text="Print" CssClass="lnk" OnClick="lnkPrint_Click"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
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
    
    <br />
    <br />
</asp:Content>
