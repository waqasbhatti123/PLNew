<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="StoreRetMgt.aspx.cs" Inherits="RMS.Inv.StoreRetMgt" Culture="auto" UICulture="auto"
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
       
        $(".classOnlyInt").keydown(function(event) {
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }
        });
        //////////////////////////////////////////////////////
        $(".classAlign").focus(function() {

            $(this).animate(
    {
        opacity: 0
    }, "fast", "swing").animate(
    {
        opacity: 1
    }, "fast", "swing");


        });

        $("[id*=GridView1]input[type=text][id*=txtQtyRet]").each(function() {
            $(this).css('text-align', 'right');
        });
        $("[id*=GridView1]input[type=text][id*=txtQtyIss]").each(function() {
            $(this).css('text-align', 'right');
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
                        <tr valign="top">
                            <td colspan="4" align="center">
                                <table>
                                  <tr>
                                    <td class="LblBgSetup" align="right">
                                        <asp:Label ID="Label1" runat="server" Text="Select Mat. Issued Doc No:"></asp:Label>
                                    </td>
                                    <td class="LblBgSetup" align="center">
                                        <asp:TextBox ID="txtDocNoIss" runat="server" CssClass="RequiredField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDocNoIss"
                                            ErrorMessage="Please select material issued doc no" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator> 
                                    </td>
                                    <td class="LblBgSetup">
                                        <asp:ImageButton ID="btnSrchIGP" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                        OnClick="btnSearchIGP_Click" ToolTip="Search Issued Doc Nos" />
                                    </td>
                                 </tr>
                                 <tr>
                                    <td colspan="3">
                                        <asp:GridView ID="grdIssDocNos" runat="server" DataKeyNames="vr_id" Visible="false" 
                                        OnSelectedIndexChanged="grdIssDocNos_SelectedIndexChanged" OnRowDataBound="grdIssDocNos_RowDataBound"
                                            AutoGenerateColumns="False" EmptyDataText="No material issue note found" Width="100%">
                                            <Columns>
                                                <asp:BoundField DataField="vr_no" HeaderText="Doc No">
                                                <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="LocName" HeaderText="Location">
                                                <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DeptNme" HeaderText="Department">
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
                                    <td colspan="3">
                                       <div id="divSelectedIGPInfo" runat="server" visible="false">
                                            <asp:Label runat="server" ID="lblSelectedParty" Text=""/>
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
                        <tr>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label122" runat="server" Text="Department:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDept" runat="server" AppendDataBoundItems="true" Enabled="false" CssClass="RequiredFieldDropDown">
                                    <asp:ListItem Text="Select Department" Selected="True" Value="0">
                                    </asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlDept"
                                    ErrorMessage="Please select department" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                           <td></td>
                           <td></td>
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
                        <tr><td colspan="4">&nbsp;</td></tr>
                        <tr>
                        
                          <td colspan="4">
                            
                                <asp:UpdatePanel ID="uPnl" runat="server" UpdateMode="Conditional">
                                      <ContentTemplate>
                                        <div id="divGrid" runat="server" visible="false">
                                        
                                        <asp:GridView ID="GridView1" runat="server" Width="98%" AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound">
                                            <HeaderStyle CssClass ="grid_hdr" />
                                            <RowStyle CssClass="grid_row" />
                                            <AlternatingRowStyle CssClass="gridAlternateRow" />
                                            <SelectedRowStyle CssClass="gridSelectedRow" />
                                          
                                            <Columns>
                                              <asp:TemplateField HeaderText="Sr. " ControlStyle-BorderWidth=".9px" ControlStyle-Height="18px" HeaderStyle-Width="30px" ControlStyle-Width="30px">
                                                  <ItemTemplate>
                                                      <asp:Label runat="server" ID="lblSr" Text='<%#Eval("Sr") %>' Width="20px">
                                                      </asp:Label>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Item"  FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="160" ControlStyle-Width="160">
                                                <ItemTemplate>
                                                    <asp:Label ID="ddlItem" runat="server" Text='<%#Eval("Item") %>' CssClass="classAlign"  />
                                                    <asp:Label ID="ddlItemCode" runat="server" Text='<%#Eval("ItemCode") %>' CssClass="classAlign" Visible="false" />
                                                </ItemTemplate>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="UOM"  FooterStyle-HorizontalAlign="Right"  HeaderStyle-Width="50" ControlStyle-Width="50">
                                                  <ItemTemplate>
                                                    <asp:Label ID="txtUomItem" runat="server" Text='<%#Eval("UOMItem") %>' CssClass="classAlign"  />
                                                    <asp:Label ID="txtUomItemCode" runat="server" Text='<%#Eval("UOMItemCode") %>' CssClass="classAlign" Visible="false" />
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Qty Issued"  FooterStyle-HorizontalAlign="Right"  HeaderStyle-Width="80" ControlStyle-Width="80">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtQtyIss" runat="server" Text='<%#Eval("QtyIss") %>' CssClass="classAlign" Enabled="false"/>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Issue Balance"  FooterStyle-HorizontalAlign="Right"  HeaderStyle-Width="80" ControlStyle-Width="80">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtQtyIssBalance" runat="server" CssClass="classAlign" Enabled="false"/>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              
                                              <asp:TemplateField HeaderText="Qty Returned*"  FooterStyle-HorizontalAlign="Right"  HeaderStyle-Width="80" ControlStyle-Width="80">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtQtyRet" runat="server" Text='<%#Eval("QtyRet")%>' CssClass="classAlign"  />
                                                    <asp:RangeValidator runat="server" ID="rv4" ValidationGroup="main" ErrorMessage="Invalid/Out of range Qty Returned" SetFocusOnError="true"
                                                        ControlToValidate="txtQtyRet" Display="None" MinimumValue="0.000" MaximumValue="99999999999.99" Type="Double" ></asp:RangeValidator>
                                                    <asp:CompareValidator runat="server" ID="CompValidatorID" ControlToValidate="txtQtyRet" ControlToCompare="txtQtyIssBalance" Operator="LessThanEqual" 
                                                        type="Double" ErrorMessage="Qty Returned should be less than or equal to Issue Balance" SetFocusOnError="true" ValidationGroup="main" Display="None" />
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Cost Center"  FooterStyle-HorizontalAlign="Right"  HeaderStyle-Width="115" ControlStyle-Width="115">
                                                  <ItemTemplate>
                                                    <asp:Label ID="ddlCostCenter" runat="server" Text='<%#Eval("CostCenter") %>' CssClass="classAlign"  />
                                                    <asp:Label ID="ddlCostCenterCode" runat="server" Text='<%#Eval("CostCenterCode") %>' CssClass="classAlign" Visible="false" />
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Remarks"  FooterStyle-HorizontalAlign="Right"  HeaderStyle-Width="160" ControlStyle-Width="160">
                                                  <ItemTemplate>
                                                    <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("Rem")%>' CssClass="classAlign" MaxLength="99"/>
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                      
                                          </Columns>
                                        </asp:GridView>
                                     </div>
                                </ContentTemplate>
                            <Triggers>
                                
                            </Triggers>
                        </asp:UpdatePanel>
                        
                        </td>
                        <td style="text-align:left; padding-bottom:15px;" valign="bottom">
                            <asp:UpdatePanel ID="updButPnl" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                                  
                                </ContentTemplate>  
                            </asp:UpdatePanel>
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
            <td width="3%">
            </td>
        </tr>
        <tr>
            <td width="3%">
            </td>
            <td valign="top">
                <br />
                <table class="filterTable" width="60%">
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblFltdocno" runat="server" Text="Doc No:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltDocNo" Width="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label22" runat="server" Text="Issued Doc No:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltIssDocNo" Width="100"></asp:TextBox>
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
                                OnClick="btnSearch_Click" ToolTip="Search store returns" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="grdStRet" runat="server" DataKeyNames="vr_id" 
                OnSelectedIndexChanged="grdStRet_SelectedIndexChanged"
                 OnPageIndexChanging="grdStRet_PageIndexChanging" OnRowDataBound="grdStRet_RowDataBound"
                    AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="There is no store returns" Width="750px">
                    <Columns>
                        <asp:BoundField DataField="vr_no" HeaderText="Doc No" />
                        <asp:BoundField DataField="IGPNo" HeaderText="Mat. Issued Doc No" />
                        <asp:BoundField DataField="LocName" HeaderText="Location" />
                        <asp:BoundField DataField="vr_dt" HeaderText="Date" />                        
                        <asp:BoundField DataField="vr_apr" HeaderText="Status" />                       
                        <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                            <ItemStyle Width="50" />
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
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
