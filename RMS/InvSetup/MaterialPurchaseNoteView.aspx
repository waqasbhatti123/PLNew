<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="MaterialPurchaseNoteView.aspx.cs" Inherits="RMS.MaterialPurchaseNoteView" %>
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

    var totalPrice = calculatePrice();
    $("span[id*=lblPriceTotal]").text(totalPrice);
    $("[id*=GridView1]input[type=text][id*=txtPrice]").keyup(function(event) {
    var totalPrice = calculatePrice();
    $("span[id*=lblPriceTotal]").text(totaPrice);
    
});



        var totalAmount = calculateAmount();
        $("span[id*=lblAmountTotal]").text(totalAmount);
        $("[id*=GridView1]input[type=text][id*=txtAmount]").keyup(function(event) {
            var totalAmount = calculateAmount();
            $("span[id*=lblAmountTotal]").text(totalAmount);


////////////////////////////////////////////////////////////////////
            $("input[type = text][id *= txtFinalAmount]").val(totalAmount);
            var txtAmnt = $("input[type = text][id *= txtFinalAmount]").val();
            var txtArea = $('#<%=lblFTtlArea.ClientID%>').html();
            var txtQty = $('#<%=lblFTtlQty.ClientID%>').html();
            $("input[type = text][id *= txtFinalRate]").val(parseFloat(txtAmnt / txtArea).toFixed(5));
            $("input[type = text][id *= txtPerPieceCost]").val(parseFloat(txtAmnt / txtQty).toFixed(5));
            
            
        });
            
        $("[id*=GridView1]input[type=text][id*=txtQuantity]").keydown(function(event) {
        if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
            event.preventDefault();
            }
        });
        $("[id*=GridView1]input[type=text][id*=txtAmount]").keydown(function(event) {
        if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
            event.preventDefault();
        }
//            var txtCrdt = $(this).closest('tr').find("input[type=text][id*=txtAmount]").val();
//            if (event.keyCode == 110 || event.keyCode == 190) {
//                if ((txtCrdt.split(".").length) > 1) {
//                    event.preventDefault();
//                }
//            }
        });
        $(".classOnlyInt").keydown(function(event) {
        if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
            event.preventDefault();
        }
    });
    $(".classOnlyDecimal").keydown(function(event) {
        if (event.keyCode != 110 && event.keyCode != 190 && (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
            event.preventDefault();
        }
        var txt = $("input[type = text][id *= txtFinalRate]").val();
        if (event.keyCode == 110 || event.keyCode == 190) {
            if ((txt.split(".").pop().length) > 1) {
                event.preventDefault();
            }
        }
        
    });
    $(".classOnlyDecimal1").keydown(function(event) {
        if (event.keyCode != 110 && event.keyCode != 190 && (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
            event.preventDefault();
        }
        var txt = $("input[type = text][id *= txtFinalAmount]").val();
        if (event.keyCode == 110 || event.keyCode == 190) {
            
            if ((txt.split(".").pop().length) > 1) {
                event.preventDefault();
            }
        }

    });

    $(".classOnlyDecimal3").keydown(function(event) {


        event.preventDefault();
        //var txt = $("input[type = text][id *= txtFinalRate]").val();
        //alert(txt);
        //$("input[type = text][id *= txtFinalRate]").val(parseFloat(txt).toFixed(2));


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
    $("[id*=GridView1]input[type=text][id*=txtPrice]").each(function() {
        $(this).css('text-align', 'right');
    });
    $("[id*=GridView1]input[type=text][id*=txtAmount]").each(function() {
        $(this).css('text-align', 'right');
    });
    //////////////////////////////////////////////////////

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

    var txtAmnt = $("input[type = text][id *= txtFinalAmount]").val();
    var txtArea = $('#<%=lblFTtlArea.ClientID%>').html();
    var txtQty = $('#<%=lblFTtlQty.ClientID%>').html();
    $("input[type = text][id *= txtFinalRate]").val(parseFloat(txtAmnt / txtArea).toFixed(5));
    $("input[type = text][id *= txtPerPieceCost]").val(parseFloat(txtAmnt / txtQty).toFixed(5));
    
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
        //===========================
//        $("input[type = text][id *= txtFinalAmount]").keydown(function(event) {
//            var tx = $("input[type = text][id *= txtFinalAmount]").val();
//            //alert(tx);
//            if (tx.indexOf('.') != -1) {
//                var chk = tx.split(".").pop();
//                if (chk.length > 1) {
//                    event.preventDefault();
//                } // end of inner if
//            } //outer if

//        });
  //==============================      
    }
    function ace_ItemSelected(sender, e) {
        var aceValue = $get('<%= aceValue.ClientID %>');
        aceValue.value = e.get_value();
    }
</script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<table width="100%" border="0" cellspacing="0" cellpadding="0">
<tr>
    <td width="3%">
    </td>
    <td>
  <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
    ValidationGroup="main"/>
  <uc1:Messages ID="ucMessage" runat="server" />

 
 <asp:UpdatePanel ID="uPnel" runat="server" UpdateMode="Conditional">
 <ContentTemplate>
  <uc1:Messages ID="ucMsgPnl" runat="server" />
  </ContentTemplate>
  <Triggers>
  <asp:AsyncPostBackTrigger ControlID="imgFind" />
  </Triggers>
  </asp:UpdatePanel>
      
   <!-- search filter table -->
   
   <fieldset class="fieldSet">
  <legend>
  Search By Lot No
  </legend>
  <div  class="div">
  
    <asp:Label ID="lblLotNo" runat="server" CssClass="lbl" Width="80px" Text="Lot No:">
    </asp:Label>
    <asp:TextBox ID="txtLotNo" CssClass="RequiredFieldTxtSmall" runat="server" ReadOnly="true" >
    </asp:TextBox>
    
         <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="CompletionListCssClass"  TargetControlID="txtLotNo" OnClientItemSelected="ace_ItemSelected" EnableCaching="true" MinimumPrefixLength="1"  ServiceMethod="GetCompletionListMPNLot" ServicePath="~/InvSetupSupport/AutoCompleteSearch.asmx" FirstRowSelected="true" CompletionInterval="100" CompletionSetCount="8">
                                    </ajaxToolkit:AutoCompleteExtender>
                                    <asp:HiddenField runat="server" ID="aceValue" />
    
    
     <asp:ImageButton runat="server" ID="imgFind" OnClick="imgFind_Click" CssClass="Clk" ToolTip="Search" ImageUrl="../images/search_icon.png" />
    

   <%-- <asp:LinkButton ID="lnkFind" runat="server" Text="Search" OnClick="lnkFind_Click" ForeColor="Black" Font-Size="11px">
    </asp:LinkButton>
    --%>
    
    <br />
    
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    
    
        <asp:Label ID="lblIGPDate" runat="server" CssClass="lbl" Width="80px" Text="IGP Date:">

    </asp:Label>
    <asp:TextBox ID="txtIGPDate" runat="server" CssClass="RequiredFieldTxtSmall" ReadOnly="true">

    </asp:TextBox>
    
    
    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtIGPDate" >
    </ajaxToolkit:CalendarExtender>
    
    </ContentTemplate>
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="imgFind" />
    </Triggers>
    </asp:UpdatePanel>
    
    
    
  
    
    <asp:UpdatePanel ID="uPnlDDL" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    
    <asp:Label ID="lblVendor" runat="server" CssClass="lbl" Width="80px" Text="Vendor:">

    </asp:Label>
    <asp:DropDownList ID="ddlVendor" runat="server" CssClass="RequiredFieldDropDownVendor" AppendDataBoundItems="true" Enabled="false" >

    <asp:ListItem Text="Select Vendor" Selected="True" Value="0"></asp:ListItem>
    </asp:DropDownList>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlVendor"
        ErrorMessage="Please select vendor." SetFocusOnError="true" ValidationGroup="main" Display="None" 
        InitialValue="0">
        </asp:RequiredFieldValidator>
                        
                        
    
   <%-- <asp:Label ID="lblNewIGPNo" runat="server" CssClass="lbl" Width="80px">
    </asp:Label>
    <asp:TextBox ID="txtNewIGPNo" runat="server" CssClass="txtExtended" ReadOnly="true">
    </asp:TextBox>--%>
    

    
    <br />
    
    
    
    <asp:Label ID="lblFromLoc" runat="server" CssClass="lbl" Width="80px" Text="From Loc:">
    </asp:Label>
    <asp:DropDownList ID="ddlCity" runat="server" CssClass="RequiredFieldDropDown" AppendDataBoundItems="true" Enabled="false">

    <asp:ListItem Text="Select City" Selected="True" Value="0"></asp:ListItem>
    </asp:DropDownList>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlCity"
        ErrorMessage="Please select location." SetFocusOnError="true" ValidationGroup="main" Display="None" 
        InitialValue="0">
        </asp:RequiredFieldValidator>
                        
                        
  

    
    
    </ContentTemplate>
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="imgFind" />
    </Triggers>
    </asp:UpdatePanel> 
  <br />
  </div>
   
  
  <div  class="div">
  
  <asp:UpdatePanel ID="uPanel" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
  
  
            
          <asp:Label ID="lblFeetageCard" runat="server" Text="Feetage Card" Font-Size="15px" Visible="false">
          </asp:Label>
  
          <asp:GridView ID="grdFeetage" CellPadding="0" CellSpacing="0" runat="server"
          AutoGenerateColumns="false" Width="100%" OnRowDataBound="grdFeetage_RowDataBound" >
              <HeaderStyle CssClass ="grid_hdr" />
              <RowStyle CssClass="grid_row" />
              <AlternatingRowStyle CssClass="gridAlternateRow" />
              <SelectedRowStyle CssClass="gridSelectedRow" />
          
              <Columns>
                  <asp:BoundField DataField="IGPNo" HeaderText="Strt. IGP No" />
                  <asp:BoundField DataField="seq" HeaderText="Sr" />
                  <%--<asp:BoundField DataField="product" HeaderText="Product" />--%>
                  <asp:BoundField DataField="code" HeaderText ="Size" />
                  <asp:BoundField DataField="quantity" HeaderText="Pieces">
                    <ItemStyle HorizontalAlign="Right" />
                  </asp:BoundField>
                  <asp:BoundField DataField="area" HeaderText="Area" >
                    <ItemStyle HorizontalAlign="Right" />
                  </asp:BoundField>
                  <asp:BoundField DataField="remarks" HeaderText="Remarks" ItemStyle-Width="200" />
              </Columns>
          
         </asp:GridView>
          

          
          <br />
          
          <div style="float:right">
          <asp:Label ID="lblFeetTtlQty" runat="server" CssClass="lbl" Width="80px" Text="Total Qty:" Visible="false">
          </asp:Label>
          <asp:Label ID="lblFTtlQty" runat="server" CssClass="lbl" Width="80px" Visible="false">
          </asp:Label>
          <asp:Label ID="lblFeetTtlArea" runat="server" Text="Total Area:" Visible="false" CssClass="lbl" Width="80px">
          </asp:Label>
          <asp:Label ID="lblFTtlArea" runat="server" CssClass="lbl" Width="80px" Visible="false">
          </asp:Label>
          </div>
          
          
                    
          <asp:Label ID="lblGradingCard" runat="server" Text="Grading Selection Card" Font-Size="15px" Visible="false">
          </asp:Label>
          
          
          <asp:GridView ID="grdGrading" runat="server"  AutoGenerateColumns="false" 
             Width="100%" OnRowDataBound="grdGrading_RowDataBound">
              <HeaderStyle CssClass ="grid_hdr" />
              <RowStyle CssClass="grid_row" />
              <AlternatingRowStyle CssClass="gridAlternateRow" />
              <SelectedRowStyle CssClass="gridSelectedRow" />
              <Columns>
                  <asp:BoundField DataField="seq" HeaderText="Sr" />
                  <%--<asp:BoundField DataField="product" HeaderText="Product" />--%>
                  <asp:BoundField DataField="code" HeaderText ="Size"/>
                  <asp:BoundField DataField="quantity" HeaderText="Pieces" ItemStyle-HorizontalAlign="Right"/>
                  <asp:BoundField DataField="area" HeaderText="Area" ItemStyle-HorizontalAlign="Right"/>
                  <asp:BoundField DataField="PrcntSelection" HeaderText="Selection %age" ItemStyle-HorizontalAlign="Right"/>
                  <asp:BoundField DataField="PrcntSelectionFeetage" HeaderText="Selection Sqft" ItemStyle-HorizontalAlign="Right"/>
                 <%-- <asp:BoundField DataField="remarks" HeaderText="Remarks" ItemStyle-Width="200" />--%>
              </Columns>
          </asp:GridView>
          

          
          <br />
          
          <div style="float:right">
          <asp:Label ID="lblGradTtlQty" runat="server"  CssClass="lbl" Width="80px" Text="Total Qty:" Visible="false">
          </asp:Label>
          <asp:Label ID="lblGTtlQty" runat="server"  CssClass="lbl" Width="80px" Visible="false">
          </asp:Label>
          <asp:Label ID="lblGradTtlArea" runat="server"  CssClass="lbl" Width="80px" Text="Total Area:" Visible="false">
          </asp:Label>
          <asp:Label ID="lblGTtlArea" runat="server"  CssClass="lbl" Width="80px" Visible="false">
          </asp:Label>
          </div>
  
  
    </ContentTemplate>
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="imgFind" />
    </Triggers>
    </asp:UpdatePanel>
  </div>
 
  
  </fieldset>    
   
   
   <br />   
      

  <br />


                <table cellspacing="2" class="stats2" align="center" border="0" width="98%">
                    <asp:Panel runat="server" ID="pnlMain" Enabled="false">
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
                                <asp:Label ID="lblMPNNo" runat="server" Text="MPN No:"></asp:Label>
                            </td>
                            <td>
                                
                                <asp:TextBox ID="txtGPNo" runat="server" CssClass="RequiredFieldTxtSmall" ReadOnly="true">
                                </asp:TextBox>
                                
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtGPNo"
                                    ErrorMessage="Please enter MPN No" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator> 
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
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlStatus"
                                    ErrorMessage="Please select status" SetFocusOnError="true" ValidationGroup="main" Display="None" 
                                    InitialValue="0"></asp:RequiredFieldValidator> 
                                
                            </td>
                        </tr>
                          <tr>
                           
                            <td class="LblBgSetup">
                                <asp:Label ID="Label4" runat="server" Text="Date:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtGpDate" runat="server" CssClass="TxtDate">
                                </asp:TextBox>
                                <span class="DteLtrl">
                                    <asp:Literal ID="Literal2"  runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                                </span>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtGpDate" PopupPosition="BottomRight" >
                                  </ajaxToolkit:CalendarExtender>
                            </td>
                            
                             <td class="LblBgSetup">
                                <asp:Label ID="Label3" runat="server" Text="L. Frieght:"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="uPnl" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                
                                    <asp:TextBox ID="txtFrt" runat="server" ReadOnly="true" CssClass="TxtSmall">
                                    </asp:TextBox>
                                
                                </ContentTemplate>
                                <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="imgFind"/>
                                </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        
                        <tr>
                                <td  class="LblBgSetup">
                                        <asp:Label ID="lblDueDate" runat="server" Text="Due Date:"></asp:Label>
                                </td>
                                <td>
                                         <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="txtDueDate" runat="server" CssClass="RequiredFieldTxtSmall">
                                        </asp:TextBox>
                                        <span class="DteLtrl">
                                            <asp:Literal ID="Literal1"  runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                                        </span>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtDueDate" PopupPosition="BottomRight" >
                                         </ajaxToolkit:CalendarExtender>
                                         </ContentTemplate>
                                        <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="imgFind"/>
                                        </Triggers>
                                        </asp:UpdatePanel>
                                        
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDueDate"
                                          ErrorMessage="Please select due date." SetFocusOnError="true" ValidationGroup="main" Display="None" >
                                          </asp:RequiredFieldValidator> 
                                </td>
                                <td   class="LblBgSetup">
                                             <asp:Label ID="Label26" runat="server" Text="Amount:"></asp:Label>
                                </td>
                                <td>
                                
                                          <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                        
                                        
                                        <asp:TextBox ID="txtFinalAmount" runat="server" ReadOnly="true" width="74px" BackColor="#F2F2F2" BorderStyle="solid" BorderWidth="1" BorderColor="ActiveBorder"  CssClass="classIntegers">
                                        </asp:TextBox>
                                        
                                        
                                        
                                        </ContentTemplate>
                                            <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="imgFind"/>
                                            </Triggers>
                                            </asp:UpdatePanel>
                                        
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFinalAmount"
                                              ErrorMessage="Please enter amount." SetFocusOnError="true" ValidationGroup="main" Display="None" >
                                              </asp:RequiredFieldValidator> 
                                </td>
                         </tr>


    
                      
                        <tr>
                            <td class="LblBgSetup">
                            
                                  <asp:Label ID="Label25" runat="server" Text="Commission:"></asp:Label>
                                
                            </td>
                            <td>
                            
                               <asp:TextBox ID="txtCommission" runat="server" CssClass="RequiredFieldTxtSmall">
                                </asp:TextBox>
                           
                                
                                
                                
                               <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFinalRate"
                                  ErrorMessage="Please enter rate" SetFocusOnError="true" ValidationGroup="main" Display="None" >
                                  </asp:RequiredFieldValidator>--%>
                                
                                
                                <%--<asp:RangeValidator id="RangeValidator1" ControlToValidate="txtFrieght"
                                       MinimumValue="0" MaximumValue="999999999" Type="Double" SetFocusOnError="true"
                                       ErrorMessage="The frieght value must be numeric" ValidationGroup="main" Display="None" runat="server"/>--%>
                            </td>
                            <td class="LblBgSetup">
                                <asp:Label ID="Label8" runat="server" Text="Cost/Piece:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPerPieceCost" ReadOnly="true" runat="server" CssClass="TxtSmall">
                                </asp:TextBox>
                            <%--<asp:RequiredFieldValidator ID="regValidator" runat="server" SetFocusOnError="true" ControlToValidate="txtTotalQty"
                                 ErrorMessage="Please select total quantity" ValidationGroup="main" Display="None">
                                 </asp:RequiredFieldValidator>
                            
                              <asp:RangeValidator id="Range1txtTotalQty" ControlToValidate="txtTotalQty"
                                   MinimumValue="1" MaximumValue="999999999999999999999999" Type="Double" SetFocusOnError="true"
                                   ErrorMessage="The total quantity value must be numeric and greater than zero" ValidationGroup="main" Display="None" runat="server"/>--%>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td class="LblBgSetup">
                                <asp:Label ID="Label10" runat="server" Text="Remarks:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Height="67" Width="182"
                                 onkeyup="LimitText(this,100);" onblur="LimitText(this,100);" CssClass="txtRemz" ></asp:TextBox>
                            </td>
                            <td class="LblBgSetup">
                                    <asp:Label ID="Label7" runat="server" Text="Cost/Sqft:"></asp:Label>
                            </td>
                            <td>
                              
                                <asp:TextBox ID="txtFinalRate" runat="server" ReadOnly="true" CssClass="RequiredFieldTxtSmall" >
                                </asp:TextBox>
                                
                            </td>
                        </tr>
                        

                    </asp:Panel>
                </table>
              </td>
            <td width="3%">
            </td>
        </tr>
    </table>
          
          <div style="height:20px;"></div>
          
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr valign="top">
          <td width="3%">
          </td>
                  
                  <td width="90%">
                            
                            
                                    
                     <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                      
                        <asp:GridView ID="GridView1" runat="server" Enabled="false"  Width="100%" GridLines="None" AutoGenerateColumns="false" ShowFooter="true" >
                                        <HeaderStyle CssClass ="grid_hdr" />
                                        <RowStyle CssClass="grid_row" />
                                        <AlternatingRowStyle CssClass="gridAlternateRow" />
                                        <SelectedRowStyle CssClass="gridSelectedRow" />
                          <Columns>
                          
                          
                            <asp:TemplateField HeaderText="Sr." ItemStyle-HorizontalAlign="Center" ControlStyle-BorderWidth="1px" ControlStyle-Height="18px" HeaderStyle-Width="30px">
                            <ItemTemplate>
                            <asp:Label runat="server" ID="lblSr" Text='<%#Eval("Sr") %>' Width="30px" BackColor="White">
                            </asp:Label>
                            </ItemTemplate>
                            </asp:TemplateField>

                          
                          
                          
                            <asp:TemplateField HeaderText="IGP No"  FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" ControlStyle-Width="80px">
                            <ItemTemplate>
                            <asp:TextBox ID="txtIGP" runat="server" Text='<%#Eval("vr_no")%>' ReadOnly="true">
                            </asp:TextBox>                        
                            </ItemTemplate>
                            </asp:TemplateField>
                            
                            
                            <asp:TemplateField HeaderText="GP Ref"  FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" ControlStyle-Width="80px">
                            <ItemTemplate>
                            <asp:TextBox ID="txtGpRef" runat="server" Text='<%#Eval("GPRef")%>' ReadOnly="true">
                            </asp:TextBox>                        
                            </ItemTemplate>
                            </asp:TemplateField>


                            
                          
                           <asp:TemplateField HeaderText="Party"  FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="300px" ControlStyle-Width="300px">
                            <ItemTemplate>
                            <asp:TextBox ID="txtParty" runat="server" Text='<%#Eval("Party")%>' ReadOnly="true">
                            </asp:TextBox>                        
                            </ItemTemplate>
                            <FooterTemplate>
                            <asp:Label ID="lblTotal" runat="server" Text="Total"></asp:Label>
                            </FooterTemplate>
                            </asp:TemplateField>
                          
                          
                          
                            <asp:TemplateField HeaderText="Pieces"  FooterStyle-HorizontalAlign="Right"   ControlStyle-Width="80px" HeaderStyle-Width="80px" >
                            <ItemTemplate>
                            <asp:TextBox ID="txtQuantity" runat="server" Text='<%#Eval("vr_qty")%>' ReadOnly="true" CssClass="classAlign" />
                            </ItemTemplate>
                            <FooterTemplate>
                            <asp:Label ID="lblGrossQuantityTotal" runat="server" Text="0"></asp:Label>
                            </FooterTemplate>
                            </asp:TemplateField>
                            
                            
                            
                             <asp:TemplateField HeaderText="Prov. Amt"  FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" ControlStyle-Width="80px">
                              <ItemTemplate>
                                <asp:TextBox ID="txtPrice" runat="server" Text='<%#Eval("Price")%>' ReadOnly="true" CssClass="classAlign" />
                              </ItemTemplate>
                              <FooterTemplate>
                                <asp:Label ID="lblPriceTotal" runat="server" Text="0"></asp:Label>
                              </FooterTemplate>
                            </asp:TemplateField>
                          
                          
                          <asp:TemplateField HeaderText="Amount"  FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" ControlStyle-Width="80px">
                              <ItemTemplate>
                                <asp:TextBox ID="txtAmount" runat="server" Text='<%#Eval("Amount")%>' BorderColor="ActiveBorder" BorderWidth="1" BorderStyle="Solid" BackColor="#F2F2F2" CssClass="classAlign" />
                              </ItemTemplate>
                              <FooterTemplate>
                                <asp:Label ID="lblAmountTotal" runat="server" Text="0"></asp:Label>
                              </FooterTemplate>
                            </asp:TemplateField>
                          
                          
                          </Columns>
                          </asp:GridView>
                          
                          </ContentTemplate>
                          <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="imgFind" />
                          </Triggers>
                          </asp:UpdatePanel>
        
                            
                            
                  </td>
          <td width="3%">
          </td>
          </tr>
         
          <tr>
            <td width="3%">
          </td>
          <td width="60%">

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
          
          

</asp:Content>
