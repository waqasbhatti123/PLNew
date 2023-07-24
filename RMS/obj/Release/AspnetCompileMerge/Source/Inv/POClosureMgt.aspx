<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="POClosureMgt.aspx.cs" Inherits="RMS.Inv.POClosureMgt" Culture="auto" UICulture="auto"
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
            <td width="1%">
            </td>
            <td>
                <table cellspacing="2" align="center" border="0" width="100%">
                    <asp:Panel runat="server" ID="pnlMain" Enabled="false">
                        <tr>
                        <td class="LblBgSetup">
                                <asp:Label ID="Label3" runat="server" Text="PO No:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPoNo" runat="server" width = "80" MaxLength="10" Enabled="false">
                                </asp:TextBox>
                                &nbsp;
                                <asp:Label ID="Label17" runat="server" Text="Revision:"></asp:Label>
                                &nbsp;
                                <asp:TextBox ID="txtPoRev" runat="server" width = "60" Enabled="false">
                                </asp:TextBox>
                                  <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtPoNo"
                                    ErrorMessage="Please enter PO No" SetFocusOnError="true" ValidationGroup="main" Display="None" ></asp:RequiredFieldValidator> --%>
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
                                <asp:Label ID="lblloc" runat="server" Text="PO Type:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPoType" runat="server" AppendDataBoundItems="true" CssClass="RequiredFieldDropDown">
                                <asp:ListItem Text="Select PO Type" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Local" Selected="True" Value="L"></asp:ListItem>
                                <asp:ListItem Text="Foreign" Value="F"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlPoType"
                                    ErrorMessage="Please select PO type" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator> 
                            </td>
                            
                            <td class="LblBgSetup">
                                <asp:Label ID="Label4" runat="server" Text="PO Date:"></asp:Label>
                            </td>
                            <td>
                                
                                <asp:TextBox ID="txtPoDate" runat="server" CssClass="RequiredFieldDate">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPoDate"
                                    ErrorMessage="Please select PO date" SetFocusOnError="true" ValidationGroup="main" Display="None" ></asp:RequiredFieldValidator> 
                                <span class="DteLtrl">
                                    <asp:Literal ID="Literal2"  runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                                </span>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtPoDate" PopupPosition="BottomRight" >
                                  </ajaxToolkit:CalendarExtender>
                                
                            </td>
                        </tr>
                         <tr>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label26" runat="server" Text="Supply Type:"></asp:Label>
                            </td>
                            <td>
                               <asp:DropDownList ID="ddlSupType" runat="server" AppendDataBoundItems="true" CssClass="RequiredFieldDropDown">
                                <asp:ListItem Text="Select supply type" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Goods" Selected="True" Value="G"></asp:ListItem>
                                <asp:ListItem Text="Services" Value="S"></asp:ListItem>
                                <asp:ListItem Text="Asset" Value="A"></asp:ListItem>
                                <asp:ListItem Text="Consumable Expense" Value="C"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlSupType"
                                    ErrorMessage="Please select supply type" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator> 
                            </td>
                             <td class="LblBgSetup">
                                <asp:Label ID="Label11" runat="server" Text="PO Currency:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCurrency" runat="server" AppendDataBoundItems="true" Width="120">
                                <asp:ListItem Text="Select currency" Value="0"  Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label122" runat="server" Text="Vendor:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlVendor" runat="server" CssClass="RequiredFieldDropDownVendor" AppendDataBoundItems="true" Width="220">
                                    <asp:ListItem Text="Select Vendor" Selected="True" Value="0">
                                    </asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlVendor"
                                    ErrorMessage="Please select vendor" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label2" runat="server" Text="Shipment:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlShipment" runat="server" AppendDataBoundItems="true" Width="110px">
                                    <asp:ListItem Text="Select Shipment"  Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Road" Selected="True" Value="t"></asp:ListItem>
                                    <asp:ListItem Text="Air" Value="a"></asp:ListItem>
                                    <asp:ListItem Text="Sea"  Value="s"></asp:ListItem>
                                </asp:DropDownList>
                                
                            </td>
                        </tr>
                        
                        <tr>
                         <td class="LblBgSetup">
                                <asp:Label ID="Label15" runat="server" Text="Vendor Doc Ref:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID ="txtVendorDocRef" runat="server" Width="100px" MaxLength="20"></asp:TextBox>
                            </td>
                            
                            <td class="LblBgSetup">
                                <asp:Label ID="Label16" runat="server" Text="Ref Date:"></asp:Label>
                            </td>
                            <td>
                                
                                <asp:TextBox ID="txtRefDate" runat="server" Width="80px">
                                </asp:TextBox>
                                <span class="DteLtrl">
                                    <asp:Literal ID="Literal1"  runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                                </span>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtRefDate" PopupPosition="BottomRight" >
                                  </ajaxToolkit:CalendarExtender>
                                
                            </td>
                        </tr>
                        
                        <tr>
                         <td class="LblBgSetup">
                                <asp:Label ID="Label6" runat="server" Text="Delivery Location:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDelLoc" runat="server" CssClass="TxtNormal" MaxLength="50">
                                </asp:TextBox>
                            </td>
                             
                            
                            <td class="LblBgSetup">
                                <asp:Label ID="Label18" runat="server" Text="Withholding Tax:"></asp:Label>
                            </td>
                            <td>
                                
                                <asp:DropDownList ID="ddlWHT" runat="server" AppendDataBoundItems="true">
                                    <asp:ListItem Value="0" Selected="True">No WHT</asp:ListItem>
                                </asp:DropDownList>
                                                                
                            </td>
                        </tr>
                        
                        <tr class="DisplayNone">
                           <td class="LblBgSetup">
                                
                            </td>
                            <td>
                                
                            </td>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label7" runat="server" Text="Delivery Period:"></asp:Label>
                            </td>
                         
                            <td>
                                <asp:TextBox ID="txtDelPeriod" runat="server" MaxLength="3" Width="40" />
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtDelPeriod"
                                    ErrorMessage="Please enter delivery period" SetFocusOnError="true" ValidationGroup="main" Display="None" ></asp:RequiredFieldValidator> --%>
                                <asp:RangeValidator runat="server" ID="RangeValidator3" ValidationGroup="main" ErrorMessage="Invalid/Out of range delivery period, cannot be greater than 255" SetFocusOnError="true"
                                    ControlToValidate="txtDelPeriod" Display="None" MinimumValue="1" MaximumValue="255" Type="Integer" ></asp:RangeValidator>
                                
                            </td>
                        </tr>
                        <tr>
                           <td class="LblBgSetup">
                                <asp:Label ID="Label1" runat="server" Text="Qty Variation %:"></asp:Label>
                            </td>
                            <td>
                               <asp:TextBox ID="txtQtyVar" runat="server" MaxLength="2"  Width="40"/>
                               <asp:RangeValidator runat="server" ID="RangeValidator1" ValidationGroup="main" ErrorMessage="Invalid/Out of range Qty Variation %" SetFocusOnError="true"
                                    ControlToValidate="txtQtyVar" Display="None" MinimumValue="1" MaximumValue="99" Type="Integer" ></asp:RangeValidator>
                            </td>
                             <td class="LblBgSetup">
                                <asp:Label ID="Label5" runat="server" Text="Payment Days:" Visible="false"></asp:Label>
                                <asp:Label ID="Label19" runat="server" Text="Overall Discount:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPayDays" runat="server" CssClass="TxtNormal" MaxLength="3"  Width="40" Visible="false"/>
                                <asp:RangeValidator runat="server" ID="RangeValidator2" ValidationGroup="main" ErrorMessage="Invalid/Out of range payment days, cannot be greater than 255" SetFocusOnError="true"
                                    ControlToValidate="txtPayDays" Display="None" MinimumValue="1" MaximumValue="255" Type="Integer" ></asp:RangeValidator>

                                <asp:TextBox ID="txtOverAllDisc" runat="server" CssClass="TxtNormal" MaxLength="12" style="text-align:right;" />
                                <asp:RangeValidator runat="server" ID="RangeValidator4" ValidationGroup="main" ErrorMessage="Invalid/Out of range overall discount, it must be an integer value" SetFocusOnError="true"
                                    ControlToValidate="txtOverAllDisc" Display="None" MinimumValue="0.00" MaximumValue="999999999.00" Type="Double" ></asp:RangeValidator>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="Label10" runat="server" Text="Payment Terms:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtPayTerms" runat="server" TextMode="MultiLine" width="500" height="50" 
                                 onkeyup="LimitText(this,200);" onblur="LimitText(this,200);"></asp:TextBox>
                            </td>
                            
                        </tr>
                        <tr valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="Label12" runat="server" Text="Terms:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtTerms" runat="server" TextMode="MultiLine" width="500" height="50" 
                                 onkeyup="LimitText(this,100);" onblur="LimitText(this,100);"></asp:TextBox>
                            </td>
                            
                        </tr>
                        <tr valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="Label13" runat="server" Text="Instructions:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtInst" runat="server" TextMode="MultiLine" width="500" height="50" 
                                 onkeyup="LimitText(this,200);" onblur="LimitText(this,200);"></asp:TextBox>
                            </td>
                            
                        </tr>
                            </asp:Panel>
                        <tr>
                            <td align="center" valign="top" colspan="4">
                                <asp:GridView ID="grdPODet" runat="server" DataKeyNames="vr_id" 
                                    AutoGenerateColumns="False" AllowPaging="false" EmptyDataText="No PO found" Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="itm_cd" HeaderText="Item Code" />
                                        <asp:BoundField DataField="itm_dsc" HeaderText="Description" />
                                        <asp:BoundField DataField="uom_dsc" HeaderText="UOM" />
                                        <asp:BoundField DataField="vr_qty" HeaderText="PO Qty" />                        
                                        <asp:BoundField DataField="rec_qty" HeaderText="Rec. Qty" />
                                        <asp:BoundField DataField="balance" HeaderText="Balance" />                       
                                        
                                    </Columns>
                                    <HeaderStyle CssClass="grid_hdr" />
                                    <RowStyle CssClass="grid_row" />
                                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                                    <SelectedRowStyle CssClass="gridSelectedRow" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="top" colspan="4">
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
                <table class="filterTable" width="70%">
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblFltName" runat="server" Text="PO No:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltPoNo" Width="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label22" runat="server" Text="Party:"></asp:Label><br />
                            <asp:TextBox runat="server" ID="txtFltParty" Width="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/search-icon-blue.gif"
                                OnClick="btnSearch_Click" ToolTip="Search purchase requests" />
                        </td>
                    </tr>
                </table>
                
                <asp:GridView ID="grdPo" runat="server" DataKeyNames="vr_id" 
                OnSelectedIndexChanged="grdPo_SelectedIndexChanged" OnPageIndexChanging="grdPo_PageIndexChanging"
                OnRowDataBound="grdPo_RowDataBound"
                    AutoGenerateColumns="False" AllowPaging="True" EmptyDataText="No PO found" Width="750px">
                    <Columns>
                        <asp:BoundField DataField="vr_no" HeaderText="PO No" />
                        <asp:BoundField DataField="RevSeq" HeaderText="Revision" />
                        <asp:BoundField DataField="vendorNme" HeaderText="Vendor" />
                        <asp:BoundField DataField="vr_dt" HeaderText="PO Date" />                        
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
</asp:Content>
