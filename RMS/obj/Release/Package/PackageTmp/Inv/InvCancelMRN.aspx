<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="InvCancelMRN.aspx.cs" Inherits="RMS.Inv.InvCancelMRN" Culture="auto" UICulture="auto"
    EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" language="javascript">
    //DISABLING DOUBLE CLICK
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    function BeginRequestHandler(sender, args) { var oControl = args.get_postBackElement(); oControl.disabled = true; }
    function prompt4Cancel() {
        return confirm("Are your sure, you want to cancel MRN?");
    }
</script>
<script type="text/javascript">

    function pageLoad()
    {
    
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
                    <asp:Panel runat ="server" ID="pnlloc" Enabled ="false" >
                        <tr valign="top">
                            <td colspan="4" align="center">
                                <table width="80%">
                                  <tr style = "display:none;">
                                    <td>
                                     <table>
                                     <tr><td class="LblBgSetup">
                                        <asp:Label ID="Label1" runat="server" Text="Select IGP:"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtIGP" runat="server" CssClass="RequiredField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtIGP"
                                            ErrorMessage="Please select IGP" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator> 
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="btnSrchIGP" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                        OnClick="btnSearchIGP_Click" ToolTip="Search IGPs" />
                                    </td>
                                    </tr></table>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td>
                                        <asp:GridView ID="grdSrchIgp" runat="server" DataKeyNames="locid,br_id,vt_cd,vr_no" Visible="false" 
                                        OnSelectedIndexChanged="grdSrchIgp_SelectedIndexChanged" OnRowDataBound="grdSrchIgp_RowDataBound" Width="100%"
                                            AutoGenerateColumns="False" EmptyDataText="No IGP found">
                                            <Columns>
                                                <asp:BoundField DataField="vr_no" HeaderText="IGP #">
                                                <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="LocName" HeaderText="Location">
                                                <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="gl_dsc" HeaderText="Party">
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
                                 <tr>
                                    <td colspan="3" align="left">
                                       <div id="divSelectedIGPInfo" runat="server" visible="false">
                                            <asp:Label runat="server" ID="lblSelectedParty" Text="Party: "/>
                                       </div> 
                                    </td>
                                 </tr>
                               </table>
                            </td>
                        </tr>
                        <tr><td>&nbsp;</td></tr>
                        <tr>
                            <td class="LblBgSetup">
                                <asp:Label ID="lblloc" runat="server" Text="To Loc:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLoc" runat="server" AppendDataBoundItems="true" CssClass="RequiredFieldDropDown" Enabled="false">
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
                                
                            </td>
                            <td>
                            
                            </td>
                        </tr>
                        </asp:Panel>
                        <tr><td colspan="4">&nbsp;</td></tr>
                        <tr>
                        
                          <td colspan="4">
                            
                                <asp:UpdatePanel ID="uPnl" runat="server" UpdateMode="Conditional">
                                      <ContentTemplate>
                                        <div id="divGrid" runat="server" visible="false">
                                        
                                        <asp:GridView ID="GridView1" runat="server" Width="98%" EmptyDataRowStyle-ForeColor="Red" AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound">
                                            <HeaderStyle CssClass ="grid_hdr" />
                                            <RowStyle CssClass="grid_row" />
                                            <AlternatingRowStyle CssClass="gridAlternateRow" />
                                            <SelectedRowStyle CssClass="gridSelectedRow" />
                                          
                                            <Columns>
                                              <asp:TemplateField HeaderText="Sr." ControlStyle-BorderWidth=".9px" ControlStyle-Height="18px" HeaderStyle-Width="30px">
                                                  <ItemTemplate>
                                                      <asp:Label runat="server" ID="lblSr" Text='<%#Eval("Sr") %>' Width="20px">
                                                      </asp:Label>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="PO Ref#"  FooterStyle-HorizontalAlign="Right"  HeaderStyle-Width="70" ControlStyle-Width="70">
                                                  <ItemTemplate>
                                                    <asp:Label ID="txtPoref" runat="server" Text='<%#Eval("PoRef")%>'  />
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Item"  FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="145" ControlStyle-Width="145">
                                                <ItemTemplate>
                                                    <asp:Label ID="ddlItem" runat="server" Text='<%#Eval("Item")%>' />
                                                    <asp:Label ID="ddlItemCode" runat="server" Text='<%#Eval("ItemCode")%>'  Visible="false" />
                                                </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Packs"  FooterStyle-HorizontalAlign="Right"  HeaderStyle-Width="50" ControlStyle-Width="50"  ItemStyle-HorizontalAlign="Right">
                                                  <ItemTemplate>
                                                    <asp:Label ID="txtPacks" runat="server" Text='<%#Eval("Packs")%>'  />
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <%--<asp:TemplateField HeaderText="UOM"  FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="50" ControlStyle-Width="50">
                                                <ItemTemplate>
                                                    <asp:Label ID="ddlUom" runat="server" Text='<%#Eval("UOM")%>'  />
                                                    <asp:Label ID="ddlUomCode" runat="server" Text='<%#Eval("UOMCode")%>'  Visible="false" />
                                                </ItemTemplate>
                                              </asp:TemplateField>--%>
                                              <asp:TemplateField HeaderText="Unit Size"  FooterStyle-HorizontalAlign="Right"  HeaderStyle-Width="70" ControlStyle-Width="70" ItemStyle-HorizontalAlign="Right">
                                                  <ItemTemplate>
                                                    <asp:Label ID="txtUnitSize" runat="server" Text='<%#Eval("UnitSize")%>' />
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="UOM"  FooterStyle-HorizontalAlign="Right"  HeaderStyle-Width="50" ControlStyle-Width="50" ItemStyle-HorizontalAlign="Center">
                                                  <ItemTemplate>
                                                    <asp:Label ID="ddlUomQty" runat="server" Text='<%#Eval("UomQty")%>'  />
                                                    <asp:Label ID="ddlUomQtyCode" runat="server" Text='<%#Eval("UomQtyCode")%>'  Visible="false" />
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Qty">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtQuantity" runat="server" Text='<%#Eval("Quantity")%>' Width="80px" style="text-align:right" TabIndex="-1" ReadOnly="true"/>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"  Width="80px"/>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Short Qty">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtQtyShort" runat="server" Text='<%#Eval("QtyShort")%>' Width="80px" style="text-align:right"  />
                                                    <asp:RangeValidator runat="server" ID="rngShortQty" ValidationGroup="main" ErrorMessage="Invalid/Out of range Qty Short" SetFocusOnError="true"
                                                        ControlToValidate="txtQtyShort" Display="Dynamic" MinimumValue="000000000.00" MaximumValue="999999999.99" Type="Double" ></asp:RangeValidator>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"  Width="80px"/>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Rejected Qty">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtQtyRej" runat="server" Text='<%#Eval("QtyRej")%>' Width="80px" style="text-align:right"  />
                                                    <asp:RangeValidator runat="server" ID="rngRejQty" ValidationGroup="main" ErrorMessage="Invalid/Out of range Qty Rejected" SetFocusOnError="true"
                                                        ControlToValidate="txtQtyRej" Display="Dynamic" MinimumValue="000000000.00" MaximumValue="999999999.99" Type="Double" ></asp:RangeValidator>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"  Width="80px"/>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Remarks">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("Rem")%>' Width="100px" MaxLength="99"/>
                                                  </ItemTemplate>
                                                  <ControlStyle />
                                                  <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                  <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"  Width="100px"/>
                                              </asp:TemplateField>
                                          </Columns>
                                        </asp:GridView>
                                     </div>
                                </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger  ControlID="addRow"/> 
                            </Triggers>
                        </asp:UpdatePanel>
                        
                        </td>
                        <td style="text-align:left; padding-bottom:15px;" valign="bottom">
                            <asp:UpdatePanel ID="updButPnl" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:LinkButton runat="server" ID="addRow" Text="Add Row" OnClick="addRow_Click" CssClass="lnk" Visible="false"></asp:LinkButton>               
                                </ContentTemplate>  
                            </asp:UpdatePanel>
                        </td>
                        
                        </tr>
                        <tr>
                            <td align="center" valign="top" colspan="4">
                                <br />
                                <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" Visible="false" />
                                 <asp:ImageButton ID="btnCancelVendor" runat ="server" OnClick="btnCancel_Click" OnClientClick="return prompt4Cancel()" ImageUrl="~/images/btn_cancel.png" onMouseOver="this.src='../images/btn_cancel_m.png'" onMouseOut="this.src='../images/btn_cancel.png'" />
                                <asp:ImageButton ID="btnClear" runat ="server"  OnClick="btnClear_Click" ImageUrl="~/images/btn_clear.png" onMouseOver="this.src='../images/btn_clear_m.png'" onMouseOut="this.src='../images/btn_clear.png'" />
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
                <table class="filterTable" width="90%">
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblFltName" runat="server" Text="Doc No:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltDocNo" Width="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label22" runat="server" Text="IGP #:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltIgpNo" Width="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Party:"></asp:Label><br />
                            <asp:DropDownList ID="ddlFltVendor" runat="server"  AppendDataBoundItems="true" Width="300">
                                <asp:ListItem Text="All" Value="0">
                                </asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Status:"></asp:Label><br />
                            <asp:DropDownList  ID="ddlFltStatus" runat="server" Enabled ="false" >
                                <asp:ListItem Text="All" Value="0" />
                                <asp:ListItem Text="Approved" Value="A" Selected="True" />
                                <asp:ListItem Text="Pending" Value="P" />
                                <asp:ListItem Text="Cancelled" Value="C" />    
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                OnClick="btnSearch_Click" ToolTip="Search material receivings" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="grdMatRec" runat="server" DataKeyNames="vr_id" 
                OnSelectedIndexChanged="grdMatRec_SelectedIndexChanged"
                 OnPageIndexChanging="grdMatRec_PageIndexChanging" OnRowDataBound="grdMatRec_RowDataBound"
                    AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No material receiving found" Width="750px">
                    <Columns>
                        <asp:BoundField DataField="vr_no" HeaderText="Doc No" />
                        <asp:BoundField DataField="IGPNo" HeaderText="IGP #" />
                        <asp:BoundField DataField="LocName" HeaderText="Location" />
                        <asp:BoundField DataField="vendorid" HeaderText="Party" />
                        <asp:BoundField DataField="vr_dt" HeaderText="Date" />                        
                        <asp:BoundField DataField="vr_apr" HeaderText="Status" />                       
                        <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                            <ItemStyle Width="50" />
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPrint" runat="server" Text="Print" CssClass="lnk" OnClick="lnkPrint_Click"></asp:LinkButton>
                            </ItemTemplate>
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
    
    <br />
    <br />
</asp:Content>
