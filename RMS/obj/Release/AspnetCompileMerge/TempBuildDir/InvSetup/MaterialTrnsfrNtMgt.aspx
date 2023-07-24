<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="MaterialTrnsfrNtMgt.aspx.cs" Inherits="RMS.MaterialTrnsfrNtMgt" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/jquery-ui.min.js"></script> 

<script type="text/javascript">
    function pageLoad() {

        //----------------Clicking the image Button-----------
        $("#<%=txtLotNo.ClientID %>").keydown(function(event) {
            if (event.keyCode == 13) {
             
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
//////////////////////////////////////////
    $("[id*=GridView1]input[type=text][id*=txtLotNo]").keydown(function(event) {
        if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
            event.preventDefault();
        }
    });

//    $(".classAlign").focus(function() {

//        $(this).animate(
//    {
//        opacity: 0
//    }, "slow", "swing").animate(
//    {
//        opacity: 1
//    }, "slow", "swing");


//    });

    $("[id*=GridView1]input[type=text][id*=txtQuantity]").each(function() {
        $(this).css('text-align', 'right');
    });
    $("[id*=GridView1]input[type=text][id*=txtWeight]").each(function() {
        $(this).css('text-align', 'right');
    });
    $("[id*=GridView1]input[type=text][id*=txtLotNo]").each(function() {
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


    function ace_ItemSelected(sender, e) {
        var aceValue = $get('<%= aceValue.ClientID %>');
        aceValue.value = e.get_value();
    }
</script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


  <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
    ValidationGroup="main"/>
  <uc1:Messages ID="ucMessage" runat="server" />


  <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtGpDate" >
      </ajaxToolkit:CalendarExtender>
 <asp:UpdatePanel ID="uPnlMsg" runat="server" UpdateMode="Conditional">
 <ContentTemplate>
 <uc1:Messages ID="ucMsgPnl" runat="server" />
 </ContentTemplate>
 <Triggers>
 <asp:AsyncPostBackTrigger ControlID="imgFind" />
 </Triggers>
 </asp:UpdatePanel>
  
  <br />
  
  <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="3%">
            </td>
            <td>
                
            
            
         <fieldset class="fieldSet">
                
                        <table>
                        <tr>
                        <td width="60px">
                            <asp:Label ID="lblLotNo" runat="server">
                            </asp:Label>
                        </td>
                        <td width="80px">
                            <asp:TextBox ID="txtLotNo" runat="server"  CssClass="RequiredFieldTxtSmall">
                            </asp:TextBox>
                            
                            <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="CompletionListCssClass"  TargetControlID="txtLotNo" OnClientItemSelected="ace_ItemSelected" EnableCaching="true" MinimumPrefixLength="1"  ServiceMethod="GetCompletionListMTNLot" ServicePath="~/InvSetupSupport/AutoCompleteSearch.asmx" FirstRowSelected="true" CompletionInterval="100" CompletionSetCount="8">
                                    </ajaxToolkit:AutoCompleteExtender>
                                    <asp:HiddenField runat="server" ID="aceValue" />
                            
                            
                            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtLotNo"
                             ErrorMessage="Please enter lot no." SetFocusOnError="true" ValidationGroup="main" Display="None" ></asp:RequiredFieldValidator>
                         
    
                        </td>
                        <td width="600px">
                             <asp:ImageButton runat="server" ID="imgFind" OnClick="imgFind_Click" CssClass="Clk" ToolTip="Search by Lot No." ImageUrl="../images/search_icon.png" />
                        </td>
                        </tr>
                        <tr>
                        <td>
                        </td>
                        <td>
                                    <span class="DteLtrl">
                                    <asp:Literal ID="Literal2"  runat="server" Text="<%$ AppSettings: LotFormatText %>" />
                                    </span>
                        </td>
                        
                        <td>
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
                        <asp:Panel runat="server" ID="pnlMain">
                            <tr>
                                <td class="LblBgSetup">
                                 <asp:Label ID="lblGPNo" runat="server">
                                     </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGPNo" runat="server" CssClass="RequiredFieldTxtSmall" ReadOnly="true">
                                       </asp:TextBox> 
                                </td>
                                <td   class="LblBgSetup">
                                    <asp:Label ID="lblStatus" runat="server">
                                    </asp:Label>
                                </td>
                                <td>
                                        
                                        <asp:DropDownList ID="ddlStatus" runat="server"  CssClass="ddl">
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
                                    <asp:Label ID="lblFromStore" runat="server">
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
                                <td   class="LblBgSetup">
                                     <asp:Label ID="lblToStore" runat="server">
                                    </asp:Label>
                                </td>
                                <td>
                                        <asp:DropDownList ID="ddlToLoc" runat="server" AppendDataBoundItems="true" CssClass="RequiredFieldDropDown" Enabled="false">
                                        <asp:ListItem Text="Select Store" Selected="True" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlLoc"
                                                            ErrorMessage="Please select location." SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                                            InitialValue="0"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            
                            <tr>
                                <td  class="LblBgSetup">
                                        <asp:Label ID="lblGpDate" runat="server">
                                        </asp:Label>
                                </td>
                                <td>
                                       <asp:TextBox ID="txtGpDate" runat="server" CssClass="RequiredFieldTxtSmall">
                                        </asp:TextBox>  
                                </td>
                                <td   class="LblBgSetup">
                                        <asp:Label ID="lblProduct" runat="server">
                                        </asp:Label>
                                </td>
                                <td>
                                            <asp:UpdatePanel ID="uFldPnl" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                            
                                            
                                            <asp:DropDownList ID="ddlProduct" runat="server" AppendDataBoundItems="true" CssClass="RequiredFieldDropDown" Enabled="false" >
                                            <asp:ListItem Selected="True" Value="0" Text="Select Product">
                                            </asp:ListItem>
                                            </asp:DropDownList>
                                            
                                            
                                            
                                            
                                            </ContentTemplate>
                                            <Triggers>
                                            <asp:AsyncPostBackTrigger  ControlID="imgFind"/>
                                            </Triggers>
                                            </asp:UpdatePanel>   
                                </td>
                            </tr>
                            <tr>
                                <td  class="LblBgSetup">
                                <asp:Label ID="prevQty" runat="server" Text="Transferred Qty:">
                                </asp:Label>
                                </td>
                                <td>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                            <asp:TextBox ID="txtPrevQty" runat="server" CssClass="RequiredFieldTxtSmall" ReadOnly="true">
                                            </asp:TextBox>
                                            
                                            </ContentTemplate>
                                            <Triggers>
                                            <asp:AsyncPostBackTrigger  ControlID="imgFind"/>
                                            </Triggers>
                                            </asp:UpdatePanel>
                                            
                                </td>
                                <td   class="LblBgSetup">
                                
                                       <asp:Label ID="lblQty" runat="server" Text="Lot Quantity:" >
                                            </asp:Label>
                                </td>
                                <td>
                                
                                               <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                            
                                
                                           <asp:TextBox ID="txtQty" runat="server" CssClass="RequiredFieldTxtSmall" ReadOnly="true">
                                            </asp:TextBox>
                                            
                                            </ContentTemplate>
                                            <Triggers>
                                            <asp:AsyncPostBackTrigger  ControlID="imgFind"/>
                                            </Triggers>
                                            </asp:UpdatePanel>
                                </td>
                            </tr>
                            
                            <tr valign="top">
                                <td  class="LblBgSetup">
                                    <asp:Label ID="lblRemarks" runat="server" >
                                                </asp:Label>
                                </td>
                                <td>
                                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Height="67" Width="182"
                                 onkeyup="LimitText(this,100);" onblur="LimitText(this,100);" CssClass="txtRemz" ></asp:TextBox>
                                </td>
                                <td  class="LblBgSetup">
                                <asp:Label ID="Label1" runat="server" Text="Remaining Qty:">
                                </asp:Label>
                                <td>
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                            
                                
                                           <asp:TextBox ID="txtRemQty" runat="server" CssClass="RequiredFieldTxtSmall" ReadOnly="true">
                                            </asp:TextBox>
                                            
                                            </ContentTemplate>
                                            <Triggers>
                                            <asp:AsyncPostBackTrigger  ControlID="imgFind"/>
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


  <asp:UpdatePanel ID="uIGPPanel" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
  <asp:Panel ID="srchPanel" HorizontalAlign="Center" Width="100%" runat="server" Visible="false">
  
    <asp:GridView ID="grdSGC" DataKeyNames="vr_apr" runat="server"
                    AutoGenerateColumns="False" OnRowDataBound="grdSGC_RowDataBound" AllowPaging="true" 
                    Width="100%" PageSize="10" OnPageIndexChanging="grdSGC_PageIndexChanging" >
            <HeaderStyle CssClass ="grid_hdr" />
            <RowStyle CssClass="grid_row" />
            <AlternatingRowStyle CssClass="gridAlternateRow" />
            <SelectedRowStyle CssClass="gridSelectedRow" />
            <Columns>
                <asp:BoundField DataField="vr_no" HeaderText="S/G Card No" />
                <asp:BoundField DataField="vr_dt" HeaderText="Date" />
                <asp:BoundField DataField="qty" HeaderText="Quantity" />
                <asp:BoundField DataField="feetage" HeaderText="Sqft" />
   
             </Columns>
    </asp:GridView>  
  
  </asp:Panel>
  </ContentTemplate>
  <Triggers>
  <asp:AsyncPostBackTrigger ControlID="imgFind" />
  </Triggers>
  </asp:UpdatePanel>

  <table  align="center" cellpadding="0" cellspacing="0" width="100%">
  <tr>
   <td width="3%">
            </td>
  <td>
   <asp:UpdatePanel ID="uPnl" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
  
    <asp:GridView ID="GridView1" runat="server"  Width="50%" GridLines="None" AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="GridView1_RowDataBound">
      <HeaderStyle CssClass ="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
      
      
      <Columns>
     
      
      <asp:TemplateField HeaderText="Sr." ControlStyle-BorderColor="Gray" ItemStyle-HorizontalAlign="Center" ControlStyle-BorderWidth="1px" ControlStyle-Height="18px" HeaderStyle-Width="30px">
      <ItemTemplate>
      <asp:Label runat="server" ID="lblSr" Text='<%#Eval("Sr") %>' Width="30px" BackColor="White">
      </asp:Label>
      </ItemTemplate>
          
      </asp:TemplateField>

  



        
        <asp:TemplateField HeaderText="Lot Ref"  ControlStyle-Width="60px" HeaderStyle-Width="60px">
        <ItemTemplate>
        <asp:TextBox ID="txtLotRef" runat="server"  Text='<%#Eval("LotRef") %>' CssClass="classOnlyInt" MaxLength="6">
        </asp:TextBox>
        </ItemTemplate>
        </asp:TemplateField>
        

<asp:TemplateField>
<ItemTemplate></ItemTemplate>
</asp:TemplateField>
        
        <asp:TemplateField HeaderText="Drum No"   FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="100px" ControlStyle-Width="100px">
        <ItemTemplate>
        <asp:TextBox ID="txtDrumNo" runat="server"  Text='<%#Eval("DrumNo") %>' CssClass="classOnlyInt" MaxLength="2">
        </asp:TextBox>
        </ItemTemplate>
        <FooterTemplate>
            <asp:Label ID="lblTotal" runat="server" Text="Total">
            </asp:Label>
          </FooterTemplate>
        </asp:TemplateField>

        


        
        <asp:TemplateField HeaderText="Quantity *"  FooterStyle-HorizontalAlign="Right"   ControlStyle-Width="100px" HeaderStyle-Width="100px" >
          <ItemTemplate>
            <asp:TextBox ID="txtQuantity" runat="server" Text='<%#Eval("Quantity")%>' CssClass="classAlign" MaxLength="6"/>
          </ItemTemplate>
          <FooterTemplate>
            <asp:Label ID="lblGrossQuantityTotal" runat="server" Text="0"></asp:Label>
          </FooterTemplate>
        </asp:TemplateField>
        
   
        
        
        <asp:TemplateField HeaderText="Weight"  FooterStyle-HorizontalAlign="Right"  ControlStyle-Width="100px" HeaderStyle-Width="100px">
          <ItemTemplate>
            <asp:TextBox ID="txtWeight" runat="server" Text='<%#Eval("Weight")%>' CssClass="classAlign" MaxLength="6"/>
          </ItemTemplate>
          <FooterTemplate>
            <asp:Label ID="lblGrossWeightTotal" runat="server" Text="0"></asp:Label>
          </FooterTemplate>
        </asp:TemplateField>

        
      </Columns>
    </asp:GridView>
    
    
  
    </ContentTemplate>
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="addRow" />
    </Triggers>
    </asp:UpdatePanel>
    </td>
     <td width="42%" style="text-align:left; padding-bottom:15px;" valign="bottom">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                            <asp:LinkButton runat="server" ID="addRow" Text="Add Row" CssClass="lnk" OnClick="addRow_Click"></asp:LinkButton>
                            </ContentTemplate>
                            </asp:UpdatePanel>
            </td>
    </tr>
  </table>


<%--  <asp:Button ID ="btnList" runat ="server" Text="List" OnClick="btnList_Click" />
  <asp:Button ID="btnBack" runat ="server" Text="List" OnClick="btnBack_Click" />
  <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
  <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="main" />--%>
  
 
 <div style="height:20px"></div>
 
 
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
