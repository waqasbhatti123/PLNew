<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="InwardGatePassMgt.aspx.cs" Inherits="RMS.InvSetup.InwardGatePassMgt" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/jquery-ui.min.js"></script> 

<script type="text/javascript">

    function checkIfQtyEquals() {
        var v1 = $("[id*=txtTotalQty]").val();
        var f1 = $("span[id*=lblGrossQuantityTotal]").html();
        if (v1 == f1) {
            return true;
        }
        else {
            alert("Total quantity should match quantity in parts.");
            return false;
        }
    }
    
    function pageLoad() {
        var totalQuantity = calculatequantity();
        $("span[id*=lblGrossQuantityTotal]").text(totalQuantity);
        $("[id*=GridView1]input[type=text][id*=txtQuantity]").keyup(function(event) { 
        var totalQuantity = calculatequantity();
        $("span[id*=lblGrossQuantityTotal]").text(totalQuantity);
    });

    

    var totalWeight = calculateweight();
    $("span[id*=lblGrossWeightTotal]").text(totalWeight);
    $("[id*=GridView1]input[type=text][id*=txtWeight]").keyup(function(event) {
    var totalWeight = calculateweight();
    $("span[id*=lblGrossWeightTotal]").text(totalWeight);
    });
    
        $("[id*=GridView1]input[type=text][id*=txtQuantity]").keydown(function(event) {
        if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
            event.preventDefault();
            }
        });
        $("[id*=GridView1]input[type=text][id*=txtWeight]").keydown(function(event) {
            if (event.keyCode != 110 && event.keyCode != 190 && (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }
            var txtCrdt = $(this).closest('tr').find("input[type=text][id*=txtWeight]").val();
            if (event.keyCode == 110 || event.keyCode == 190) {
                if ((txtCrdt.split(".").length) > 1) {
                    event.preventDefault();
                }
            }

            //********************Restrict upto two Decimal palces
//            if (txtCrdt.indexOf('.') != -1) {
//                var amt = txtCrdt.split(".").pop();
//                if (amt.length > 1) {
//                    event.preventDefault();
//                }
//            }
            //********************




        });
        $(".classOnlyInt").keydown(function(event) {
        if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
            event.preventDefault();
        }
    });
    //////////////////////////////////////////////////////
//    $(".classAlign").focus(function() {

//        $(this).animate(
//    {
//        opacity: 0
//    }, "fast", "swing").animate(
//    {
//        opacity: 1
//    }, "fast", "swing");


//    });

    $("[id*=GridView1]input[type=text][id*=txtQuantity]").each(function() {
        $(this).css('text-align', 'right');
    });
    $("[id*=GridView1]input[type=text][id*=txtWeight]").each(function() {
        $(this).css('text-align', 'right');
    });
    //////////////////////////////////////////////////////

    $(function() {
        $('.tbt').autocomplete({
            source: function(request, response) {
                $.ajax({
                url: "inwardgatepassmgt.aspx/GetControlProduct",
                    data: "{ 'sname': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",

                    dataFilter: function(data) { return data; },
                    success: function(data) {
                        response($.map(data.d, function(item) {
                            return {
                                value: item,
                                result: item
                            }
                        }))
                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            },
            select: function(e, ui) {
                code = ui.item.value;
                code = code.split("-");
                $(e.target).closest('tr').find("input[type=text][id*=txtProduct]").val(code[1]);
                $(e.target).closest('tr').find("input[type=label][id*=lblCode]").val(code[0]);
                
               
            },
            minLength: 1
        });
    });

    //////////////////////////////////////////////////////
        
        
        function calculatequantity() {
            var totalQty = 0;
            $("[id*=GridView1]input[type=text][id*=txtQuantity]").each(function() {
                temp = parseFloat($(this).val());
                if (isNaN(temp)) temp = 0;
                totalQty = totalQty + temp;
            });
            return totalQty;
        }
        function calculateweight() {
            var totalvat = 0;
            $("[id*=GridView1]input[type=text][id*=txtWeight]").each(function() {
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
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="3%">
            </td>
            <td>
                <table cellspacing="2" class="stats2" align="center" border="0" width="98%">
                    <asp:Panel runat="server" ID="pnlMain">
                       <%-- <tr>
                            <td colspan="4">
                                <table width="100%">
                                  <tr>
                                    <td>Personal Info: <hr title="Personal Info"/><br /></td>
                                    <td valign="middle"></td>
                                  </tr>
                                </table>
                            </td>
                        </tr>--%>
                        <tr>
                            <td class="LblBgSetup">
                                <asp:Label ID="lblloc" runat="server" Text="To Loc:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLoc" runat="server" AppendDataBoundItems="true" CssClass="RequiredFieldDropDown">
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
                                <asp:Label ID="Label26" runat="server" Text="Start IGP #:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtGPNo" runat="server" CssClass="RequiredFieldTxtSmall" ReadOnly="true">
                                </asp:TextBox>
                            </td>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label25" runat="server" Text="Doc Ref #:" Visible="false"></asp:Label>
                                <asp:Label ID="Label4" runat="server" Text="IGP Date:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocRef" runat="server" CssClass="RequiredFieldTxtSmall" Visible="false" ReadOnly="true">
                                </asp:TextBox>
                                <asp:TextBox ID="txtGpDate" runat="server" CssClass="RequiredFieldDate">
                                </asp:TextBox>
                                <span class="DteLtrl">
                                    <asp:Literal ID="Literal2"  runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                                </span>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtGpDate" PopupPosition="BottomRight" >
                                  </ajaxToolkit:CalendarExtender>
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label1" runat="server" Text="Broker:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlVendor" runat="server" CssClass="RequiredFieldDropDownVendor" AppendDataBoundItems="true" Width="300">
                                    <asp:ListItem Text="Select Broker" Selected="True" Value="0">
                                    </asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlVendor"
                                    ErrorMessage="Please select broker" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label2" runat="server" Text="From Loc:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCity" runat="server" CssClass="RequiredFieldDropDown" AppendDataBoundItems="true" Width="110px">
                                    <asp:ListItem Text="Select City" Selected="True" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlCity"
                                    ErrorMessage="Please select city" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator> 
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label3" runat="server" Text="Product:"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlProduct" runat="server" CssClass="RequiredFieldDropDown" AppendDataBoundItems="true" >
                                    <asp:ListItem Text="Select Product" Value="0" Selected="True">
                                </asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlProduct"
                                    ErrorMessage="Please select product" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label6" runat="server" Text="Vehicle No:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVehicleNo" runat="server" CssClass="TxtNormal" MaxLength="20" Width="180">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label5" runat="server" Text="Bilty No:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBiltyNo" runat="server" CssClass="TxtNormal" MaxLength="20" Width="180">
                                </asp:TextBox>
                            </td>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label8" runat="server" Text="Total Qty:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalQty" runat="server"  CssClass="RequiredField" Width="180" MaxLength="6">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="regValidator" runat="server" SetFocusOnError="true" ControlToValidate="txtTotalQty"
                                 ErrorMessage="Please select total quantity" ValidationGroup="main" Display="None">
                                 </asp:RequiredFieldValidator>
                            
                              <asp:RangeValidator id="Range1txtTotalQty" ControlToValidate="txtTotalQty"
                                   MinimumValue="1" MaximumValue="999999999999999999999999" Type="Double" SetFocusOnError="true"
                                   ErrorMessage="The total quantity value must be numeric and greater than zero" ValidationGroup="main" Display="None" runat="server"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label7" runat="server" Text="Frieght:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFrieght" runat="server" CssClass="TxtNormal" MaxLength="9" Width="180"> 
                                </asp:TextBox>
                                
                                <asp:RangeValidator id="RangeValidator1" ControlToValidate="txtFrieght"
                                       MinimumValue="0" MaximumValue="999999999" Type="Double" SetFocusOnError="true"
                                       ErrorMessage="The frieght value must be numeric" ValidationGroup="main" Display="None" runat="server"/>
                            </td>
                            <td>
                                
                            </td>
                            <td>
                            
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
                </table>
              </td>
            <td width="3%">
            </td>
        </tr>
    </table>
    
    
    

    <asp:Label ID="lblDriver" runat="server" CssClass="lbl" Width="80px" Visible="false">
    </asp:Label>
    <asp:TextBox ID="txtDriver" runat="server"  cssClass="txt" Visible="false">
    </asp:TextBox>


    <div style="height:20px"></div>

        <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="3%">
            </td>
            <td>
                
                    <table cellspacing="2" class="stats2" align="left" border="0" width="60%">
                    <asp:Panel runat="server" ID="Panel1">
                        
                        <tr>
                            <td>
                            
                            
                                <asp:UpdatePanel ID="uPnl" runat="server" UpdateMode="Conditional">
                                      <ContentTemplate>
                                      
                                        <asp:GridView ID="GridView1" runat="server" Width="100%"  GridLines="None" EmptyDataRowStyle-ForeColor="Red" AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="GridView1_RowDataBound">
                                                        <HeaderStyle CssClass ="grid_hdr" />
                                                        <RowStyle CssClass="grid_row" />
                                                        <AlternatingRowStyle CssClass="gridAlternateRow" />
                                                        <SelectedRowStyle CssClass="gridSelectedRow" />
                                          
                                          
                                          <Columns>
                                          <asp:TemplateField HeaderText="Sr. " ControlStyle-BorderWidth=".9px" ControlStyle-Height="18px" HeaderStyle-Width="30px" ControlStyle-Width="30px">
                                          <ItemTemplate>
                                          <asp:Label runat="server" ID="lblSr" Text='<%#Eval("Sr") %>' Width="20px" BackColor="White">
                                          </asp:Label>
                                          </ItemTemplate>
                                          </asp:TemplateField>
                                             
                                             
                                         <asp:TemplateField HeaderText="Party*"  FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="200px" ControlStyle-Width="200px">
                                         <ItemTemplate>
                                         <asp:DropDownList ID="ddlVendor" runat="server" AppendDataBoundItems="true" >
                                         <asp:ListItem Selected="True" Text="--- Select Party ---" Value="0"  >
                                         </asp:ListItem>
                                         </asp:DropDownList>
                                         </ItemTemplate>
                                         </asp:TemplateField>
                                          

<asp:TemplateField></asp:TemplateField>


                                         <asp:TemplateField HeaderText="IGP Ref*"  FooterStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" ControlStyle-Width="100px">
                                          <ItemTemplate>
                                          <asp:TextBox ID="txtIGPRef" runat="server" Text='<%#Eval("IGPRef")%>' MaxLength="20"/>
                                          </ItemTemplate>
                                                   <FooterTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text="Total"></asp:Label>
                                                </FooterTemplate>
                                         </asp:TemplateField>
                                         

                                            
                                            
                                          <asp:TemplateField HeaderText="Quantity*"  FooterStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" ControlStyle-Width="100px">
                                          <ItemTemplate>
                                          <asp:TextBox ID="txtQuantity" runat="server" Text='<%#Eval("Quantity")%>' CssClass="classAlign" MaxLength="6"/>
                                          </ItemTemplate>
                                                <FooterTemplate>
                                                <asp:Label ID="lblGrossQuantityTotal" runat="server" Text="0"></asp:Label>
                                                </FooterTemplate>
                                          </asp:TemplateField>
                                            
                                            
                                            
                                            
                                          <asp:TemplateField HeaderText="Price*"  FooterStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" ControlStyle-Width="100px">
                                          <ItemTemplate>
                                          <asp:TextBox ID="txtWeight" runat="server" Text='<%#Eval("Weight")%>' CssClass="classAlign" MaxLength="9"/>
                                          </ItemTemplate>
                                                <FooterTemplate>
                                                <asp:Label ID="lblGrossWeightTotal" runat="server" Text="0"></asp:Label>
                                                </FooterTemplate>
                                          </asp:TemplateField>
                                      
                                          </Columns>
                                        </asp:GridView>
                                        
                        </ContentTemplate>
                        <Triggers>
                        <asp:AsyncPostBackTrigger  ControlID="addRow"/> 
                        </Triggers>
                        </asp:UpdatePanel>
                            
                            
                            </td>
                        </tr>
                        
                    </asp:Panel>
                    </table>
                
            </td>
            <td width="40%" style="text-align:left; padding-bottom:15px;" valign="bottom">
                <asp:UpdatePanel ID="updButPnl" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton runat="server" ID="addRow" Text="Add Row" OnClick="addRow_Click" CssClass="lnk"></asp:LinkButton>               
                    </ContentTemplate>  
                </asp:UpdatePanel>
            </td>
        </tr>
        </table>



<div style="height:20px"></div>

  
  <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="3%">
            </td>
            <td>
 
                    <asp:ImageButton ID="btnSave" runat="server"  ImageUrl="~/images/btn_save.png" OnCommand="btnSave_Click"
                        onMouseOver="this.src='../images/btn_save_m.png'" onMouseOut="this.src='../images/btn_save.png'"   ValidationGroup="main"/>
                        
                    
                    <asp:ImageButton ID="btnClear" runat="server"  ImageUrl="~/images/btn_clear.png" OnCommand="btnClear_Click"
                        onMouseOver="this.src='../images/btn_clear_m.png'" onMouseOut="this.src='../images/btn_clear.png'"/>
                       
                       
                    <asp:ImageButton ID="btnList" runat="server"  ImageUrl="~/images/btn_list.png" OnCommand="btnList_Click"
                       onMouseOver="this.src='../images/btn_list_m.png'" onMouseOut="this.src='../images/btn_list.png'"/>
                       

                    <asp:ImageButton ID="btnBack" runat="server"  ImageUrl="~/images/btn_list.png" OnCommand="btnBack_Click"
                        onMouseOver="this.src='../images/btn_list_m.png'" onMouseOut="this.src='../images/btn_list.png'"/>
      
                    
              </td>
              <td width="3%">
              </td>
              </tr>
              </table>
              
        
</asp:Content>
