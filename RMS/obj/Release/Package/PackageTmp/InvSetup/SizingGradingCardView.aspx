<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="SizingGradingCardView.aspx.cs" Inherits="RMS.SizingGradingCardView" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/jquery-ui.min.js"></script> 

<script type="text/javascript">
    function pageLoad() {
        //----------------Clicking the Link Button-----------
        $("#<%=txtSrcIGP.ClientID %>").keydown(function(event) {
            if (event.keyCode == 13) {
                //alert("hello...");
                $(".Clk").trigger('click');
                 //eval($(".Clk").attr('href'));
            }
        });
   
        //--------------------------
    
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
        });
        $(".classOnlyInt").keydown(function(event) {
        if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
            event.preventDefault();
        }

       
    });

    $(".classAlign").focusout(function() {
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
    /////////////////////////////////////////////////////////
    $(".classAlign").focus(function() {

        $(this).animate(
    {
        opacity: 0
    }, "fast", "swing").animate(
    {
        opacity: 1
    }, "fast", "swing");


    });

    $("[id*=GridView1]input[type=text][id*=txtQuantity]").each(function() {
        $(this).css('text-align', 'right');
    });
    $("[id*=GridView1]input[type=text][id*=txtWeight]").each(function() {
        $(this).css('text-align', 'right');
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


    function ace_ItemSelected(sender, e) {
        var aceValue = $get('<%= aceValue.ClientID %>');
        aceValue.value = e.get_value();
    }



</script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


  <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
    ValidationGroup="main"/>
    <asp:ValidationSummary ID="mainSrch" CssClass="ErrorSummary" runat="server" DisplayMode="List"
    ValidationGroup="mainSrch"/>
    <asp:UpdatePanel ID="uMsgPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
  <uc1:Messages ID="ucMessage" runat="server" />
  </ContentTemplate>
  <Triggers>
  <asp:AsyncPostBackTrigger ControlID="imgIGP" />
  </Triggers>
  </asp:UpdatePanel>
<br />

      <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtCardDate" >
      </ajaxToolkit:CalendarExtender>
      
  
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="3%">
            </td>
                <td>
                            
                            <fieldset class="fieldSet" >
                                
                                    &nbsp;
                                  <table><tr><td width="80px">
                                  <asp:Label ID="lblSrch" runat="server" Text="Strt IGP No:">
                                  </asp:Label>
                                  </td><td>
                                  <asp:TextBox ID="txtSrcIGP" runat="server" ReadOnly="true" CssClass="RequiredField" Width="400px" Height="18px">
                                  </asp:TextBox>
                                  
                                  
                                  <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="CompletionListCssClass"  TargetControlID="txtSrcIGP" OnClientItemSelected="ace_ItemSelected" EnableCaching="true" MinimumPrefixLength="1"  ServiceMethod="GetCompletionListIGP" ServicePath="~/InvSetupSupport/AutoCompleteSearch.asmx" FirstRowSelected="true" CompletionInterval="100" CompletionSetCount="8">
                                    </ajaxToolkit:AutoCompleteExtender>
                                    <asp:HiddenField runat="server" ID="aceValue" />
                                  
                                  <br />
                         
                                  
                                </td><td >
                                  <asp:ImageButton runat="server" ID="imgIGP" Visible="false" OnClick="srchIGP_Click" ValidationGroup="mainSrch" CssClass="Clk" ToolTip="Search by IGP No." ImageUrl="../images/search_icon.png" />
                                    </td>
                                   
                                    </tr>
                                    <tr><td></td><td>
                                    <span class="DteLtrl">
                                    <asp:Literal ID="Literal2"  runat="server" Text="<%$ AppSettings: IGPFormatText %>" />
                                    </span>
                                    </td><td></td></tr>
                                    <tr>
                                    <td>
                                    <asp:Label ID="lblCardDate" runat="server" Text="Card Date:" >
                                  </asp:Label>
                                    </td>
                                    <td>
                                             
                                  
                                  <asp:TextBox ID="txtCardDate" runat="server" CssClass="RequiredFieldTxtSmall" Enabled="false">
                                  </asp:TextBox>
                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtCardDate"
                                                    ErrorMessage="Please select card date." SetFocusOnError="true" ValidationGroup="mainSrch" Display="None" 
                                                    ></asp:RequiredFieldValidator>
                                  
                                    </td>
                                    <td></td>
                                    </tr>
                                    </table>
                                    <table>
                                    <tr align="right">
                           
                                    <td width="80px">
                                    
                                    </td>
                                    <td width="620px">
                                    
                                      <asp:UpdatePanel ID="uIGPPanel" runat="server" UpdateMode="Conditional">
                                      <ContentTemplate>
                                      <asp:Panel ID="srchPanel" HorizontalAlign="Center" Width="100%" runat="server" Visible="false">
                                      
                                            <asp:GridView ID="grdIGP" DataKeyNames="Doc_Ref" runat="server" OnSelectedIndexChanging="grdIGP_SelectedIndexChanging"
                                                            AutoGenerateColumns="False" OnRowDataBound="grdIGP_RowDataBound" AllowPaging="true" 
                                                            Width="100%" PageSize="5" OnPageIndexChanging="grdIGP_PageIndexChanging" >
                                                    <HeaderStyle CssClass ="grid_hdr" />
                                                    <RowStyle CssClass="grid_row" />
                                                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                                                    <SelectedRowStyle CssClass="gridSelectedRow" />
                                                    <Columns>
                                                        <asp:BoundField DataField="vr_no" HeaderText="IGP No" ControlStyle-Width="74px" ItemStyle-HorizontalAlign="Left"/>
                                                        <asp:BoundField DataField="vr_dt" HeaderText="IGP Date"  ControlStyle-Width="74px"  DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false" ItemStyle-HorizontalAlign="Left"/>
                                                        <asp:BoundField DataField="gl_dsc" HeaderText="Party"  ControlStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="vr_qty" HeaderText="Quantity"  ControlStyle-Width="40px" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="vr_apr" HeaderText="Status"  ControlStyle-Width="40px" Visible="false" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:CommandField  ShowSelectButton="true" HeaderText="Card"  ControlStyle-Width="20px" Visible="false" ItemStyle-HorizontalAlign="Left" />
                                                        
                                                     </Columns>
                                            </asp:GridView>  
                              
                                      </asp:Panel>
                                      </ContentTemplate>
                                      <Triggers>
                                      <asp:AsyncPostBackTrigger ControlID="imgIGP" />
                                      </Triggers>
                                      </asp:UpdatePanel>
                              
                                    
                                    
                                    
                                    
                                    
                                    </td>
                                    </tr>
                                    </table>
                           

                              
                        </fieldset>
                            
                </td>
            <td width="3%">
            </td>
        </tr>
        </table>
            
  
    <div style="height:20px"></div>
  
  
  
  
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="3%">
            </td>
                <td>
                        <table cellspacing="2" class="stats2" align="center" border="0" width="98%">
                        <asp:Panel runat="server" ID="pnlMain" Enabled="false">
                            <tr>
                            <td class="LblBgSetup">
                                <asp:Label ID="lblToStore" runat="server">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLoc" runat="server" CssClass="RequiredFieldDropDown" Enabled="false" AppendDataBoundItems="true">
                                <asp:ListItem Text="Select Store" Selected="True" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlLoc"
                                                    ErrorMessage="Please select location." SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                                    InitialValue="0"></asp:RequiredFieldValidator> 
                            </td>
                            <td class="LblBgSetup">
                                <asp:Label ID="lblStatus" runat="server">
                                </asp:Label>
                                
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="RequiredFieldDropDown">
                                <asp:ListItem Text="Select Status" Selected="True" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Approved" Value="A"></asp:ListItem>
                                <asp:ListItem Text="Pending" Value="P"></asp:ListItem>
                                <asp:ListItem Text="Cancelled" Value="C"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlStatus"
                                                    ErrorMessage="Please select status." SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                                    InitialValue="0"></asp:RequiredFieldValidator>
                                                        
                            </td>
                            </tr>
                            <tr>
                            <td  class="LblBgSetup">
                                <asp:Label ID="lblGPNo" runat="server">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtGPNo" runat="server" CssClass="RequiredFieldTxtSmall" ReadOnly="true">
                                </asp:TextBox>
                            </td>
                            <td   class="LblBgSetup">
                                <asp:Label ID="lblIgpDisp" runat="server" Text="Strt IGP No:">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                <asp:TextBox ID="txtIgpDisp" runat="server" CssClass="RequiredFieldTxtSmall" ReadOnly="true">
                                </asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                <asp:AsyncPostBackTrigger  ControlID="imgIGP"/>
                                </Triggers>
                                </asp:UpdatePanel>
                                
                            </td>
                            </tr>
                            <tr>
                            <td  class="LblBgSetup">
                                <asp:Label ID="lblGpDate" runat="server">
                                </asp:Label>
                            </td>
                            <td>
                               <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                <asp:TextBox ID="txtGpDate" runat="server" CssClass="RequiredFieldTxtSmall" Enabled="false">
                                </asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtGpDate">
                                     </ajaxToolkit:CalendarExtender>
                                    </ContentTemplate>
                                    <Triggers>
                                    <asp:AsyncPostBackTrigger  ControlID="imgIGP"/>
                                    </Triggers>
                                    </asp:UpdatePanel>  
                            </td>
                            <td   class="LblBgSetup">
                                <asp:Label ID="lblGpRef" runat="server" Text="Strt GP Ref:">
                                </asp:Label>
                            </td>
                            <td>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                    <asp:TextBox ID="txtGpRef" runat="server" CssClass="RequiredFieldTxtSmall" ReadOnly="true">
                                    </asp:TextBox>
                                    
                                    </ContentTemplate>
                                    <Triggers>
                                    <asp:AsyncPostBackTrigger  ControlID="imgIGP"/>
                                    </Triggers>
                                    </asp:UpdatePanel>      
                            </td>
                            </tr>
                            
                            
                             <tr>
                            <td  class="LblBgSetup">
                                <asp:Label ID="lblPrdct" runat="server" Text="Product:" >
                                </asp:Label>
                            </td>
                            <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                    <asp:DropDownList ID="ddlProduct" runat="server" CssClass="RequiredFieldDropDown" AppendDataBoundItems="true" Enabled="false">
                                    <asp:ListItem Text="--- Select Product ---" Value="0" Selected="True">
                                    </asp:ListItem>
                                    </asp:DropDownList>
                                    
                                    </ContentTemplate>
                                    <Triggers>
                                    <asp:AsyncPostBackTrigger  ControlID="imgIGP"/>
                                    </Triggers>
                                    </asp:UpdatePanel>
                                
                            </td>
                            <td  class="LblBgSetup">
                             <asp:Label ID="lblLotNo" runat="server">
                                </asp:Label>
                            </td>
                            <td>
                                    <asp:UpdatePanel ID="uFldPnl" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                <asp:TextBox ID="txtLotNo" runat="server" CssClass="RequiredFieldTxtSmall" ReadOnly="true">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLotNo"
                                                    ErrorMessage="Please enter lot no." SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                                    ></asp:RequiredFieldValidator>
                                </ContentTemplate>
                                <Triggers>
                                <asp:AsyncPostBackTrigger  ControlID="imgIGP"/>
                                </Triggers>
                                </asp:UpdatePanel>       
                            </td>
                            </tr>
                            
                            <tr>
                            
                                <td  class="LblBgSetup">
                                        <asp:Label ID="lblSizedQty" runat="server" Text="Sized Quantity:">
                                        </asp:Label>
                                </td>
                                <td>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                        
                                        <asp:TextBox ID="txtSizedQty" runat="server" CssClass="RequiredFieldTxtSmall" ReadOnly="true">
                                        </asp:TextBox>
                                                                                
                                        </ContentTemplate>
                                        <Triggers>
                                        <asp:AsyncPostBackTrigger  ControlID="imgIGP"/>
                                        </Triggers>
                                        </asp:UpdatePanel>
                                </td>
                                <td   class="LblBgSetup">
                                        <asp:Label ID="Label1" runat="server" Text="Lot Quantity:" >
                                        </asp:Label>
                                </td>
                                <td>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="txtLotQty" runat="server" CssClass="RequiredFieldTxtSmall" ReadOnly="true">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtLotQty"
                                                        ErrorMessage="Please enter lot quantity." SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                                        ></asp:RequiredFieldValidator>
                                        
                                        </ContentTemplate>
                                        <Triggers>
                                        <asp:AsyncPostBackTrigger  ControlID="imgIGP"/>
                                        </Triggers>
                                        </asp:UpdatePanel>
                                </td>
                                                 
                            </tr>
                            
                            
                             <tr valign="top">
                           
                            <td   class="LblBgSetup">
                                    <asp:Label ID="lblRemarks" runat="server" >
                                    </asp:Label>
                            </td>
                            <td>
                                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Height="67" Width="182"
                                 onkeyup="LimitText(this,100);" onblur="LimitText(this,100);" CssClass="txtRemz" ></asp:TextBox>
                            </td>
                             <td  class="LblBgSetup">
                                    <asp:Label ID="lblRemQty" runat="server" Text="Remaining Quantity:">
                                    </asp:Label>
                            </td>
                            <td>
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                        
                                        <asp:TextBox ID="txtRemQty" runat="server" CssClass="RequiredFieldTxtSmall" ReadOnly="true">
                                        </asp:TextBox>
                                                                                
                                        </ContentTemplate>
                                        <Triggers>
                                        <asp:AsyncPostBackTrigger  ControlID="imgIGP"/>
                                        </Triggers>
                                        </asp:UpdatePanel>
                            </td>
                            </tr>
                        </asp:Panel>
                        </table>
                                
                
                </td>
            <td width="3%">
            </td>
        </tr>
    </table>
  
  
    <div style="height:20px"></div>
  
  
  <table  width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
    <td width="3%">
    </td>
    <td>
                    <table cellspacing="2" class="stats2" align="left" border="0" width="60%">
                    <asp:Panel runat="server" ID="Panel1" Enabled="false">
                        
                        <tr>
                            <td>
                            
                                        <asp:UpdatePanel ID="uPnl" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
  
                                          <asp:GridView ID="GridView1" runat="server" GridLines="None" Width="100%" AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="GridView1_RowDataBound"> 
                                          <HeaderStyle CssClass ="grid_hdr" />
                                                        <RowStyle CssClass="grid_row" />
                                                        <AlternatingRowStyle CssClass="gridAlternateRow" />
                                                        <SelectedRowStyle CssClass="gridSelectedRow" />
      
                                          <Columns>
                                          <asp:TemplateField HeaderText="Sr." ControlStyle-BorderColor="Gray" ItemStyle-HorizontalAlign="Center" ControlStyle-BorderWidth="1px" ControlStyle-Height="18px" HeaderStyle-Width="30px" ControlStyle-Width="30px" >
                                          <ItemTemplate>
                                          <asp:Label runat="server" ID="lblSr" Text='<%#Eval("Sr") %>' Width="20px" BackColor="White">
                                          </asp:Label>
                                          </ItemTemplate>
                                          </asp:TemplateField>


                                            <asp:TemplateField HeaderText="Size/Grade Code" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="200px" ControlStyle-Width="200px">
                                            <ItemTemplate>
                                            
                                                <asp:TextBox ID="SizeCode" runat="server" ReadOnly="true" Text='<%#Eval("CodeDesc")%>'>  
                                                </asp:TextBox>
                                                <asp:HiddenField ID="hidSize" runat="server" Value='<%#Eval("Code")%>'/>
                                            
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text="Total">
                                                </asp:Label>
                                              </FooterTemplate>
                                            </asp:TemplateField>
                                            
                                            
                                              <asp:TemplateField></asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Quantity*"  FooterStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" ControlStyle-Width="100px" >
                                              <ItemTemplate>
                                                <asp:TextBox ID="txtQuantity" runat="server" Text='<%#Eval("Quantity")%>' CssClass="classAlign" />
                                              </ItemTemplate>
                                              <FooterTemplate>
                                                <asp:Label ID="lblGrossQuantityTotal" runat="server" Text="0"></asp:Label>
                                              </FooterTemplate>
                                            </asp:TemplateField>
                                            

                                            
                                            <asp:TemplateField HeaderText="Area*"  FooterStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px" ControlStyle-Width="100px">
                                              <ItemTemplate>
                                                <asp:TextBox ID="txtWeight" runat="server" Text='<%#Eval("Weight")%>' CssClass="classAlign"  />
                                              </ItemTemplate>
                                              <FooterTemplate>
                                                <asp:Label ID="lblGrossWeightTotal" runat="server" Text="0"></asp:Label>
                                              </FooterTemplate>
                                            </asp:TemplateField>

                                            
                                          </Columns>
                                        </asp:GridView>
                                        
                                        </ContentTemplate>
                                        <Triggers>
                                        <asp:AsyncPostBackTrigger  ControlID="imgIGP"/>
                                        </Triggers>
                                        </asp:UpdatePanel>
                            
                            
                            
                            </td>
                        </tr>
                    </asp:Panel>
                    </table>
    </td>
    <td width="3%">
                     <asp:LinkButton runat="server" ID="addRow" Visible="false" Text="Add Row" OnClick="addRow_Click"></asp:LinkButton>
            
    </td>
    </tr>
  </table>
  
  
    <div style="height:20px"></div>
  
<%--
  <asp:Button ID ="btnList" runat ="server" Text="List" OnClick="btnList_Click" />
  <asp:Button ID="btnBack" runat ="server" Text="List" OnClick="btnBack_Click" />
  <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
  <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="main" />
  --%>
 
 
 
   <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="3%">
            </td>
            <td>
 
                    <asp:ImageButton ID="btnList" runat="server"  ImageUrl="~/images/btn_list.png" OnCommand="btnList_Click"
                       onMouseOver="this.src='../images/btn_list_m.png'" onMouseOut="this.src='../images/btn_list.png'"/>
                       

                    <asp:ImageButton ID="btnBack" runat="server"  ImageUrl="~/images/btn_list.png" OnCommand="btnBack_Click"
                        onMouseOver="this.src='../images/btn_list_m.png'" onMouseOut="this.src='../images/btn_list.png'"/>
                 
                 
                    <asp:ImageButton ID="btnClear" runat="server"  ImageUrl="~/images/btn_clear.png" OnCommand="btnClear_Click"
                        onMouseOver="this.src='../images/btn_clear_m.png'" onMouseOut="this.src='../images/btn_clear.png'"/>
                        
                        
                    <asp:ImageButton ID="btnSave" runat="server"  ImageUrl="~/images/btn_save.png" OnCommand="btnSave_Click"
                        onMouseOver="this.src='../images/btn_save_m.png'" onMouseOut="this.src='../images/btn_save.png'"   ValidationGroup="main"/>
              </td>
              <td width="3%">
              </td>
              </tr>
              </table>
</asp:Content>
