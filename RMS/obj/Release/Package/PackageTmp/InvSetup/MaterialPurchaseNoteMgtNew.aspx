<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="MaterialPurchaseNoteMgtNew.aspx.cs" Inherits="RMS.MaterialPurchaseNoteMgtNew" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/jquery-ui.min.js"></script> 

<script type="text/javascript">

    function pageLoad() {


                    var totalQuantity = calculatequantity();
                    $("span[id*=lblGrossQuantityTotal]").text(totalQuantity);
                    $("[id*=GridView1]input[type=text][id*=txtQuantity]").keyup(function(event) { 
                        var totalQuantity = calculatequantity();
                        $("span[id*=lblGrossQuantityTotal]").text(totalQuantity);
                    });

                    var totalPrice = calculatePrice();
                    $("span[id*=lblPriceTotal]").text(totalPrice);
                    $("[id*=GridView1]input[type=text][id*=txtPrice]").keyup(function(event) {
                        var totalPrice = calculatePrice();
                        $("span[id*=lblPriceTotal]").text(totaPrice);
                    });

                    var totalAmount = calculateAmount();
                    $("span[id*=lblAmountTotal]").text(totalAmount);
                    $("input[type = text][id *= txtAmntComm]").val(totalAmount);
                    
                    $("[id*=GridView1]input[type=text][id*=txtAmount]").keyup(function(event) {
                        var totalAmount = calculateAmount();
                        $("span[id*=lblAmountTotal]").text(totalAmount);
                        $("input[type = text][id *= txtFinalAmount]").val(totalAmount);
                        $("input[type = text][id *= txtAmntComm]").val(totalAmount);
                        
                        var txtAmnt = $("input[type = text][id *= txtFinalAmount]").val();
                        var txtArea = $('#<%=lblFTtlArea.ClientID%>').html();
                        var txtQty = $('#<%=lblFTtlQty.ClientID%>').html();
                        $("input[type = text][id *= txtFinalRate]").val(parseFloat(txtAmnt / txtArea).toFixed(5));
                        $("input[type = text][id *= txtPerPieceCost]").val(parseFloat(txtAmnt / txtQty).toFixed(5));
                    });


                    $("[id*=GridView1]input[type=text][id*=txtAmount]").change(function(event) {
                        var totalAmount = calculateAmount();
                        $("span[id*=lblAmountTotal]").text(totalAmount);
                        $("input[type = text][id *= txtFinalAmount]").val(totalAmount);
                        $("input[type = text][id *= txtAmntComm]").val(totalAmount);

                        var txtAmnt = $("input[type = text][id *= txtFinalAmount]").val();
                        var txtArea = $('#<%=lblFTtlArea.ClientID%>').html();
                        var txtQty = $('#<%=lblFTtlQty.ClientID%>').html();
                        $("input[type = text][id *= txtFinalRate]").val(parseFloat(txtAmnt / txtArea).toFixed(5));
                        $("input[type = text][id *= txtPerPieceCost]").val(parseFloat(txtAmnt / txtQty).toFixed(5));
                    });


                    $("[id*=GridView1]input[type=text][id*=txtAmount]").keydown(function(event) {
                        if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                            event.preventDefault();
                        }
                    });


                    $(".classOnlyInt").keydown(function(event) {
                        if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                            event.preventDefault();
                        }
                    });



                    $(".classOnlyInt").keyup(function(event) {
                        var commission = $("input[type = text][id *= txtCommission]").val();
                        if (commission == "")
                            commission = 0;
                        var txtAmnt = parseInt(parseFloat($("input[type = text][id *= txtFinalAmount]").val()) + parseFloat(commission));
                        var txtArea = $('#<%=lblFTtlArea.ClientID%>').html();
                        var txtQty = $('#<%=lblFTtlQty.ClientID%>').html();
                        $("input[type = text][id *= txtAmntComm]").val(parseInt(txtAmnt));
                        $("input[type = text][id *= txtFinalRate]").val(parseFloat(txtAmnt / txtArea).toFixed(5));
                        $("input[type = text][id *= txtPerPieceCost]").val(parseFloat(txtAmnt / txtQty).toFixed(5));
                    });


                    $(".classOnlyInt").change(function(event) {
                        var commission = $("input[type = text][id *= txtCommission]").val();
                        if (commission == "")
                            commission = 0;
                        var txtAmnt = parseInt(parseFloat($("input[type = text][id *= txtFinalAmount]").val()) + parseFloat(commission));
                        var txtArea = $('#<%=lblFTtlArea.ClientID%>').html();
                        var txtQty = $('#<%=lblFTtlQty.ClientID%>').html();
                        $("input[type = text][id *= txtAmntComm]").val(parseInt(txtAmnt));
                        $("input[type = text][id *= txtFinalRate]").val(parseFloat(txtAmnt / txtArea).toFixed(5));
                        $("input[type = text][id *= txtPerPieceCost]").val(parseFloat(txtAmnt / txtQty).toFixed(5));
                    });
                    

                    $("[id*=GridView1]input[type=text][id*=txtQuantity]").each(function() {
                        $(this).css('text-align', 'right');
                    });
                    $("[id*=GridView1]input[type=text][id*=txtPrice]").each(function() {
                        $(this).css('text-align', 'right');
                    });
                    $("[id*=GridView1]input[type=text][id*=txtAmount]").each(function() {
                        $(this).css('text-align', 'right');
                    });


                    $(".classIntegers").keydown(function(event) {
                        if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                            event.preventDefault();
                        }
                    });


                    $(".classIntegers").keyup(function(event) {
                        var txtAmnt = $("input[type = text][id *= txtFinalAmount]").val();
                        var txtArea = $('#<%=lblFTtlArea.ClientID%>').html();
                        var txtQty = $('#<%=lblFTtlQty.ClientID%>').html();
                        //$("input[type = text][id *= txtFinalAmount]").val(parseInt( txtAmnt));
                        $("input[type = text][id *= txtFinalRate]").val(parseFloat(txtAmnt / txtArea).toFixed(5));
                        $("input[type = text][id *= txtPerPieceCost]").val(parseFloat(txtAmnt / txtQty).toFixed(5));
                    });

                    
                    $(".classIntegers").change(function(event) {

                        var txtAmnt = $("input[type = text][id *= txtFinalAmount]").val();
                        var txtArea = $('#<%=lblFTtlArea.ClientID%>').html();
                        var txtQty = $('#<%=lblFTtlQty.ClientID%>').html();
                        $("input[type = text][id *= txtFinalRate]").val(parseFloat(txtAmnt / txtArea).toFixed(5));
                        $("input[type = text][id *= txtPerPieceCost]").val(parseFloat(txtAmnt / txtQty).toFixed(5));
                    });
                    
                    
                    var commission = $("input[type = text][id *= txtCommission]").val();
                    if (commission == "")
                        commission = 0;
                    var txtAmnt = parseFloat($("input[type = text][id *= txtFinalAmount]").val()) + parseFloat(commission);
                    $("input[type = text][id *= txtAmntComm]").val(parseInt(txtAmnt));
                    var txtArea = $('#<%=lblFTtlArea.ClientID%>').html();
                    var txtQty = $('#<%=lblFTtlQty.ClientID%>').html();
                    $("input[type = text][id *= txtFinalRate]").val(parseFloat(txtAmnt / txtArea).toFixed(5));
                    $("input[type = text][id *= txtPerPieceCost]").val(parseFloat(txtAmnt / txtQty).toFixed(5));
                    
                    
                    

                    function calculatequantity() {
                        var totalQty = 0;
                        $("[id*=GridView1]input[type=text][id*=txtQuantity]").each(function() {
                            temp = parseFloat($(this).val());
                            if (isNaN(temp)) temp = 0;
                            totalQty = totalQty + temp;
                        });
                        return totalQty;
                    }
                    
                    function calculatePrice() {
                        var totalvat = 0;
                        $("[id*=GridView1]input[type=text][id*=txtPrice]").each(function() {
                            temp = parseFloat($(this).val());
                            if (isNaN(temp)) temp = 0;
                            totalvat = totalvat + temp;
                        });
                        return totalvat;
                    }

                    function calculateAmount() {
                        var totalvat = 0;
                        $("[id*=GridView1]input[type=text][id*=txtAmount]").each(function() {
                            temp = parseFloat($(this).val());
                            if (isNaN(temp)) temp = 0;
                            totalvat = totalvat + temp;
                        });
                        return totalvat;
                    }
                    
    }
</script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      
      <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main"/>

      <uc1:Messages ID="ucMessage" runat="server" />

      

      
      
     <%-- <asp:UpdatePanel ID="upnlIGPs" runat="server" UpdateMode="Conditional">
      <ContentTemplate>--%>
      <uc1:Messages ID="uMsg" runat="server" />
      
      
      
      <asp:Panel ID="pnlSrchIGPs" runat="server">
      <br />
              <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                  <td width="3%">
                  </td>
                  <td>
             
                        <asp:Label ID="lblSelectItem" runat="server" Text="Select Product:" Width="100px">
                        </asp:Label>
                        &nbsp;
                        <asp:DropDownList ID="ddlSelectItem" runat="server" Width="20%">
                        </asp:DropDownList>
                        <br />
                        
                        <asp:Label ID="lblSelectVendor" runat="server" Text="Select Vendor:" Width="100px">
                        </asp:Label>
                        &nbsp;
                        <asp:DropDownList ID="ddlSelectVendor" runat="server" Width="60%">
                        </asp:DropDownList>
                        <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddlSelectVendor"
                        PromptText="Search Vendor" PromptPosition="Top" QueryPattern="Contains" PromptCssClass="ListSearchExtenderPrompt">
                        </ajaxToolkit:ListSearchExtender>
                        &nbsp;
                        <asp:LinkButton ID="lnkSelectVendor" runat="server" Text="Select" CssClass="lnk" ToolTip="Select vendor." OnClick="LnkSelectVendor_Click">
                        </asp:LinkButton>
            
            
                  </td>
                  <td width="3%">
                  </td>
                  </tr>
              </table>
        <br />
      </asp:Panel>
      
      
      <asp:Panel ID="pnlGetIGPs" runat="server">
           <br />
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
              <td width="3%">
              </td>
              <td>
                 
                                           
                            <asp:GridView ID="grdIGPs" runat="server" 
                                            AutoGenerateColumns="False"
                                            Width="100%" ShowFooter="true" FooterStyle-HorizontalAlign="Center" OnRowDataBound="GrdIGPs_OnRowDataBound">
                                    <HeaderStyle CssClass ="grid_hdr" />
                                    <RowStyle CssClass="grid_row" />
                                    <AlternatingRowStyle CssClass="gridAlternateRow"  />
                                    <SelectedRowStyle CssClass="gridSelectedRow" />
                                    
                                    <Columns>
                                        <asp:BoundField DataField="Sr" HeaderText="Sr." ControlStyle-Width="15px" HeaderStyle-Width="15px"  />
                                        <asp:BoundField DataField="StrtIGP" HeaderText="Strt IGP"  ControlStyle-Width="50px" HeaderStyle-Width="50px"  />
                                        <asp:BoundField DataField="Date" HeaderText="Date"  ControlStyle-Width="80px" HeaderStyle-Width="80px"  />
                                        <%--<asp:BoundField DataField="GPRef" HeaderText="GP Ref"   ControlStyle-Width="50px" HeaderStyle-Width="50px"   />--%>
                                        <asp:BoundField DataField="Broker" HeaderText="Broker"  ControlStyle-Width="230px" HeaderStyle-Width="230px"  />
                                        <asp:BoundField DataField="Item" HeaderText="Product"   ControlStyle-Width="70px" HeaderStyle-Width="70px" />
                                        <asp:BoundField DataField="LotQty" HeaderText="Pieces"   ControlStyle-Width="50px" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px"  />
                                        
                                        <asp:TemplateField HeaderText="Select" ControlStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" >
                                        <ItemTemplate>
                                        <asp:CheckBox ID="cbxSelectIGP" runat="server" ToolTip="Select to prepare MPN." />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        <asp:LinkButton ID="lnkMergIgps" runat="server" Text="Prepare" OnClick="LnkMergIgps_Click" ToolTip="Prepare MPN." Font-Bold="true" Font-Underline="false" CssClass="lnk">
                                        </asp:LinkButton>
                                        </FooterTemplate>
                                        </asp:TemplateField>
                                                                              
                                     </Columns>
                                    
                            </asp:GridView>  
                
                     
           
            </td>
          <td width="3%">
          </td>
          </tr>
          </table>
        <br />
      </asp:Panel>
      
      
      <asp:Panel ID="pnlVendor" runat="server" Visible="false">
      <br />
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                      <td width="3%">
                      </td>
                      <td>
              <table cellspacing="2" class="stats2" align="center" border="0" width="98%">
             <tr>
                <td class="LblBgSetup" width="96px">
                <asp:Label ID="lblProduct" runat="server" Text="Product :" ></asp:Label>
                </td>
                <td>
                <asp:TextBox ID="txtProduct" runat="server" CssClass="RequiredField" Enabled="false"></asp:TextBox>
                </td>
             </tr>
             <tr>
                <td class="LblBgSetup">
                <asp:Label ID="lblVendor" runat="server"  Text="Vendor:">
                </asp:Label>
                </td>
                <td >
                <asp:DropDownList ID="ddlVendor" runat="server" CssClass="RequiredFieldDropDown" AppendDataBoundItems="true" Enabled="false" >
                    <asp:ListItem Text="Select Vendor" Selected="True" Value="0"></asp:ListItem>
                </asp:DropDownList>
                                
                </td>
            </tr>
            <tr>
                <td class="LblBgSetup">
                    <asp:Label ID="lblFromLoc" runat="server"  Text="From Loc:">
                    </asp:Label>
                </td>
                
                <td>
                    <asp:DropDownList ID="ddlCity" runat="server" CssClass="RequiredFieldDropDown" AppendDataBoundItems="true" Enabled="false">
                    <asp:ListItem Text="Select City" Selected="True" Value="0"></asp:ListItem>
                    </asp:DropDownList>
             
                </td>
            </tr>
             
              </table>
                </td>
                      <td width="3%">
                      </td>
                      </tr>
                      </table>
                      <br />
      </asp:Panel>
      
      
      <asp:Panel ID="pnlFields" runat="server">
      
             <table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
              <td width="3%">
              </td>
              <td>
              
              <table  cellspacing="2" class="stats2" align="center" border="0" width="98%">
                    <tr>
                                <td  class="LblBgSetup">
                                    <asp:Label ID="lblMPNNo" runat="server" Text="MPN No:"></asp:Label>
                                </td>
                                <td>
                                     <asp:TextBox ID="txtGPNo" runat="server" CssClass="RequiredFieldTxtSmall" ReadOnly="true">
                                     </asp:TextBox>
                                
                                    
                                </td>
                                <td   class="LblBgSetup">
                                    <asp:Label ID="Label4" runat="server" Text="MPN Date:"></asp:Label>
                                </td>
                                <td>
                                         <asp:TextBox ID="txtGpDate" runat="server" CssClass="RequiredFieldDate">
                                        </asp:TextBox>
                                        <span class="DteLtrl">
                                            <asp:Literal ID="Literal2"  runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                                        </span>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtGpDate" PopupPosition="BottomRight" >
                                          </ajaxToolkit:CalendarExtender>
                                        
                                </td>
                                <td   class="LblBgSetup">
                                        <asp:Label ID="Label9" runat="server" Text="Status:"></asp:Label>
                                </td>
                                <td>
                                         <asp:DropDownList ID="ddlStatus" runat="server" CssClass="RequiredFieldDropDown" Width="110px">
                                        <asp:ListItem Text="Select Status" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Approved" Value="A"></asp:ListItem>
                                        <asp:ListItem Text="Pending" Value="P" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Cancelled" Value="C"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlStatus"
                                        ErrorMessage="Please select status" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                        InitialValue="0"></asp:RequiredFieldValidator> 
                                </td>
                    </tr>
                    
                      <tr>
                                <td  class="LblBgSetup">
                                    <asp:Label ID="lblIGPDate" runat="server" Text="IGP Date:">
                                        </asp:Label>
                                </td>
                                <td>
                                               
                                        <asp:TextBox ID="txtIGPDate" runat="server" CssClass="RequiredFieldDate" Enabled="false">
                                        </asp:TextBox>
                                        
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtIGPDate" PopupPosition="BottomRight" >
                                        </ajaxToolkit:CalendarExtender>
                         
                                </td>
                                <td   class="LblBgSetup">
                                        <asp:Label ID="lblDueDate" runat="server" Text="Due Date:"></asp:Label>
                                </td>
                                <td>
                               
                                        <asp:TextBox ID="txtDueDate" runat="server" CssClass="RequiredFieldDate">
                                        </asp:TextBox>
                                        
                                        <span class="DteLtrl">
                                            <asp:Literal ID="Literal1"  runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                                        </span>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtDueDate" PopupPosition="BottomRight" >
                                         </ajaxToolkit:CalendarExtender>
                                        
                                </td>
                                <td   class="LblBgSetup">
                                        <asp:Label ID="Label3" runat="server" Text="L. Frieght:"></asp:Label>
                                </td>
                                <td>
                                                          
                                            <asp:TextBox ID="txtFrt" runat="server" ReadOnly="true" CssClass="TxtSmall">
                                            </asp:TextBox>
                               </td>
                    </tr>
                    
                      <tr>
                                <td  class="LblBgSetup">
                                        <asp:Label ID="Label26" runat="server" Text="Amount:"></asp:Label>
                                </td>
                                <td>
                                                  
                                        <asp:TextBox ID="txtFinalAmount" runat="server" ReadOnly="true" width="74px" BackColor="#F2F2F2" BorderStyle="solid" BorderWidth="1" BorderColor="ActiveBorder"  CssClass="classIntegers">
                                        </asp:TextBox>
                                       
                                </td>
                                <td   class="LblBgSetup">
                                        <asp:Label ID="Label8" runat="server" Text="Cost/Piece:"></asp:Label>
                                </td>
                                <td>
                                          <asp:TextBox ID="txtPerPieceCost" ReadOnly="true" runat="server" CssClass="RequiredFieldTxtSmall">
                                            </asp:TextBox>
                                       
                                </td>
                                <td   class="LblBgSetup">
                                            <asp:Label ID="Label7" runat="server" Text="Cost/Sqft:"></asp:Label>
                                </td>
                                <td>
                                            <asp:TextBox ID="txtFinalRate" runat="server" ReadOnly="true" CssClass="RequiredFieldTxtSmall" >
                                            </asp:TextBox>
                                </td>
                    </tr>
                    
                      <tr valign="top">
                                <td  class="LblBgSetup">
                                            <asp:Label ID="Label25" runat="server" Text="Commission:"></asp:Label>
                                </td>
                                <td>
                                             <asp:TextBox ID="txtCommission" runat="server" CssClass="classOnlyInt" Width="74px" MaxLength="7">
                                            </asp:TextBox>
                                </td>
                                
                                <td class="LblBgSetup">
                                            <asp:Label ID="Label1" runat="server" Text="Total Amount:"></asp:Label>
                                </td>
                                <td >
                                            <asp:TextBox ID="txtAmntComm" runat="server" ReadOnly="true" width="74px" BackColor="#F2F2F2" BorderStyle="solid" BorderWidth="1" BorderColor="ActiveBorder"  CssClass="classIntegers1">
                                            </asp:TextBox>
                                </td>
                                
                                <td   class="LblBgSetup">
                                            <asp:Label ID="Label10" runat="server" Text="Remarks:"></asp:Label>
                                </td>
                                <td>
                                             <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Height="67" Width="182"
                                        onkeyup="LimitText(this,100);" onblur="LimitText(this,100);" CssClass="txtRemz" ></asp:TextBox>
                                </td>
                                
                                
                    </tr>


            </table>
              
              </td>
              <td width="3%">
              </td>
              </tr>
              </table>
      <br />
      </asp:Panel>
      
      
      <table width="100%" border="0" cellspacing="0" cellpadding="0" runat="server" id="tblLine" visible="false">
      <tr>
      <td width="3%"></td>
      <td>
      <hr style="height:.5; color:Gray"/>
      </td>
      <td width="3%"></td>
      </tr>
      <tr>
      <td width="3%"></td>
      <td>
      &nbsp;
      </td>
      <td width="3%"></td>
      </tr>
      </table>
      
      
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
      
       <tr>
      <td>
      </td>
      <td>
      <b><asp:Label ID="lblGradingCard" runat="server" Text="Grading Selection Card:" Font-Size="12px" Visible="false">
        </asp:Label></b>
      </td>
      <td>
      </td>
      <td>
      <b> <asp:Label ID="lblFeetageCard" runat="server" Text="Feetage Card:" Font-Size="12px" Visible="false">
            </asp:Label></b>
          <asp:Label ID="lblFTtlQty" runat="server" Style="display:none;" Font-Size="10px">
          </asp:Label>
          <asp:Label ID="lblFTtlArea" runat="server" Style="display:none;" Font-Size="10px">
          </asp:Label>
      </td>
      <td>
      </td>
      </tr>
       <tr>
      <td>
      &nbsp;
      </td>
      <td>
      &nbsp;
      </td>
      <td>
      &nbsp;
      </td>
      <td>
      &nbsp;
      </td>
      <td>
      &nbsp;
      </td>
      </tr>
                  
      <tr valign="top">
      <td width="3%">
      </td>
      
      <td>
      
          <asp:Panel ID="pnlGradingCard" runat="server">
          
          <asp:GridView ID="grdGrading" runat="server"  AutoGenerateColumns="false" 
                 Width="100%" OnRowDataBound="grdGrading_RowDataBound" ShowFooter="true" FooterStyle-HorizontalAlign="Right">
                  <HeaderStyle CssClass ="grid_hdr" />
                  <RowStyle CssClass="grid_row" />
                  <AlternatingRowStyle CssClass="gridAlternateRow" />
                  <SelectedRowStyle CssClass="gridSelectedRow" />
                  <Columns>
                      <asp:BoundField DataField="seq" HeaderText="Sr"  HeaderStyle-Width="25px" ControlStyle-Width="25px"/>
                      <asp:BoundField DataField="code" HeaderText ="Size"  HeaderStyle-Width="120px" ControlStyle-Width="120px" />
                      <asp:BoundField DataField="quantity" HeaderText="Pieces" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="60px" ControlStyle-Width="60px"/>
                      <asp:BoundField DataField="area" HeaderText="Area" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="60px" ControlStyle-Width="60px"/>
                      <asp:BoundField DataField="PrcntSelection" HeaderText="Sel. %age" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" ControlStyle-Width="60px"/>
                      <asp:BoundField DataField="PrcntSelectionFeetage" HeaderText="Sel. Sqft" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" ControlStyle-Width="60px"/>
                  </Columns>
              </asp:GridView>

          </asp:Panel>
      
      </td>
      <td width="1%">
      
      </td>
      
      <td>
      
   
       <asp:Panel ID="pnlFeetageCard" runat="server">
          
                <asp:GridView ID="grdFeetage" CellPadding="0" CellSpacing="0" runat="server"
                        AutoGenerateColumns="false" Width="100%" OnRowDataBound="grdFeetage_RowDataBound" ShowFooter="true" FooterStyle-HorizontalAlign="Right" >
                      <HeaderStyle CssClass ="grid_hdr" />
                      <RowStyle CssClass="grid_row" />
                      <AlternatingRowStyle CssClass="gridAlternateRow" />
                  <SelectedRowStyle CssClass="gridSelectedRow" />
              
                  <Columns>
                      <asp:BoundField DataField="seq" HeaderText="Sr"   HeaderStyle-Width="25px" ControlStyle-Width="25px"/>
                      <asp:BoundField DataField="code" HeaderText ="Size"   HeaderStyle-Width="120px" ControlStyle-Width="120px" />
                      <asp:BoundField DataField="quantity" HeaderText="Pieces" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="60px" ControlStyle-Width="60px" />
                      <asp:BoundField DataField="area" HeaderText="Area" ItemStyle-HorizontalAlign="Right"   HeaderStyle-Width="60px" ControlStyle-Width="60px" />
                  </Columns>
              
             </asp:GridView>
          
          </asp:Panel>
   
   
          </td>
          <td width="3%">
          </td>
          </tr>
          <tr>
      <td>
      &nbsp;
      </td>
      <td>
      &nbsp;
      </td>
      <td>
      &nbsp;
      </td>
      <td>
      &nbsp;
      </td>
      <td>
      &nbsp;
      </td>
      </tr>
          </table>
      
      
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
      <td width="3%">
      </td>
      <td>
      <b><asp:Label ID="lblIGPsDet" runat="server" Text="IGPs Info:" Font-Size="12px" Visible="false">
        </asp:Label></b>
            <asp:LinkButton ID="lblVvDet" runat="server" Text="(View Detail)" CssClass="lnk" ToolTip="Lots Info." Visible="false">
            </asp:LinkButton>
      <asp:Panel ID="pnlIGPs" runat="server">

        <br />
                      
                        <asp:GridView ID="GridView1" runat="server"  Width="100%" GridLines="None" OnRowDataBound="GridView1_RowDataBound" AutoGenerateColumns="false" ShowFooter="true" >
                                        <HeaderStyle CssClass ="grid_hdr" />
                                        <RowStyle CssClass="grid_row" />
                                        <AlternatingRowStyle CssClass="gridAlternateRow" />
                                        <SelectedRowStyle CssClass="gridSelectedRow" />
                          <Columns>
                          
                          
                            <asp:TemplateField HeaderText="Sr" ItemStyle-HorizontalAlign="Center" ControlStyle-BorderWidth="1px" ControlStyle-Height="18px" HeaderStyle-Width="25px" ControlStyle-Width="25px">
                            <ItemTemplate>
                            <asp:Label runat="server" ID="lblSr" Text='<%#Eval("Sr") %>'  BackColor="White">
                            </asp:Label>
                            </ItemTemplate>
                            </asp:TemplateField>

                          
                          
                          
                            <asp:TemplateField HeaderText="IGP No"  FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="75px" ControlStyle-Width="75px">
                            <ItemTemplate>
                            <asp:TextBox ID="txtIGP" runat="server" Text='<%#Eval("vr_no")%>' Enabled="false">
                            </asp:TextBox>                        
                            </ItemTemplate>
                            </asp:TemplateField>
                            
                            
                            <asp:TemplateField HeaderText="GP Ref"  FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="65px" ControlStyle-Width="65px">
                            <ItemTemplate>
                            <asp:TextBox ID="txtGpRef" runat="server" Text='<%#Eval("GPRef")%>' Enabled="false">
                            </asp:TextBox>                        
                            </ItemTemplate>
                            </asp:TemplateField>
                                 
                          
                           <asp:TemplateField HeaderText="Party"  FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="325px" ControlStyle-Width="325px">
                            <ItemTemplate>
                            <asp:TextBox ID="txtParty" runat="server" Text='<%#Eval("Party")%>' Enabled="false">
                            </asp:TextBox>                        
                            </ItemTemplate>
                            <FooterTemplate>
                            <asp:Label ID="lblTotal" runat="server" Text="Total"></asp:Label>
                            </FooterTemplate>
                            </asp:TemplateField>
                          
                          
                          
                            <asp:TemplateField HeaderText="Pieces"  FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"   ControlStyle-Width="65px" HeaderStyle-Width="65px" >
                            <ItemTemplate>
                            <asp:TextBox ID="txtQuantity" runat="server" Text='<%#Eval("vr_qty")%>' Enabled="false" />
                            </ItemTemplate>
                            <FooterTemplate>
                            <asp:Label ID="lblGrossQuantityTotal" runat="server" Text="0"></asp:Label>
                            </FooterTemplate>
                            </asp:TemplateField>
                            
                            
                            
                             <asp:TemplateField HeaderText="Prov. Amt"  FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="80px"  ItemStyle-HorizontalAlign="Right"  ControlStyle-Width="80px">
                              <ItemTemplate>
                                <asp:TextBox ID="txtPrice" runat="server" Text='<%#Eval("Price")%>' Enabled="false"  />
                              </ItemTemplate>
                              <FooterTemplate>
                                <asp:Label ID="lblPriceTotal" runat="server" Text="0"></asp:Label>
                              </FooterTemplate>
                            </asp:TemplateField>
                          
                          
                          <asp:TemplateField HeaderText="Amount"  FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"  ControlStyle-Width="80px">
                              <ItemTemplate>
                                <asp:TextBox ID="txtAmount" runat="server" Text='<%#Eval("Amount")%>' BorderColor="ActiveBorder" BorderWidth="1" BorderStyle="Solid" BackColor="#F2F2F2" MaxLength="9" />
                              </ItemTemplate>
                              <FooterTemplate>
                                <asp:Label ID="lblAmountTotal" runat="server" Text="0"></asp:Label>
                              </FooterTemplate>
                            </asp:TemplateField>
                          
                                                   
                          </Columns>
                          </asp:GridView>
           
      <br />
      </asp:Panel>
      
      </td>
      <td width="3%">
      </td>
      </tr>
      </table>
      
      
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
      <td width="3%">
      </td>
      <td> 
            <asp:ImageButton ID="btnSave" runat="server"  ImageUrl="~/images/btn_save.png" OnClick="btnSave_Click"
                onMouseOver="this.src='../images/btn_save_m.png'" onMouseOut="this.src='../images/btn_save.png'" ValidationGroup="main" />
            <asp:ImageButton ID="btnClear" runat="server"  ImageUrl="~/images/btn_clear.png" OnClick="btnClear_Click"
                onMouseOver="this.src='../images/btn_clear_m.png'" onMouseOut="this.src='../images/btn_clear.png'" />
            <asp:ImageButton ID="btnList" runat="server"  ImageUrl="~/images/btn_list.png" OnClick="btnList_Click"
                onMouseOver="this.src='../images/btn_list_m.png'" onMouseOut="this.src='../images/btn_list.png'"/>
             <asp:ImageButton ID="btnBack" runat="server"  ImageUrl="~/images/btn_back.png" OnClick="btnBack_Click"
               onMouseOver="this.src='../images/btn_back_m.png'" onMouseOut="this.src='../images/btn_back.png'"/>
     </td>
      <td width="3%">
      </td>
      </tr>
      </table>
      
      
      
   
     <%--      
       </ContentTemplate>
      </asp:UpdatePanel>
           --%>
        

      
           

</asp:Content>
