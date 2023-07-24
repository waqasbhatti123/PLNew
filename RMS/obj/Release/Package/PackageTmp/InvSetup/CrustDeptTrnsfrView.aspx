<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="CrustDeptTrnsfrView.aspx.cs" Inherits="RMS.CrustDeptTrnsfrView" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/jquery-ui.min.js"></script> 

<script type="text/javascript">
    function pageLoad() {

        $(".classOnlyInt").keydown(function(event) {
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }
        });


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
        $("[id*=GridView1]input[type=text][id*=txtArea]").each(function() {
            $(this).css('text-align', 'right');
        });
        $("[id*=GridView1]input[type=text][id*=txtHalfQuantity]").each(function() {
            $(this).css('text-align', 'right');
        });

        
        

        $("[id*=GridView1]input[type=text][id*=txtHalfQuantity]").keydown(function(event) {
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }
        });


    $("[id*=GridView1]input[type=text][id*=txtQuantity]").keydown(function(event) {
        if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
            event.preventDefault();
        }
    });

    $("[id*=GridView1]input[type=text][id*=txtArea]").keydown(function(event) {
        if (event.keyCode != 110 && event.keyCode != 190 && (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
            event.preventDefault();
        }
        var txtCrdt = $(this).closest('tr').find("input[type=text][id*=txtArea]").val();
        if (event.keyCode == 110 || event.keyCode == 190) {
            if ((txtCrdt.split(".").length) > 1) {
                event.preventDefault();
            }
        }
    });




    var totalQuantity = calculatequantity();
    $("span[id*=lblGrossQuantityTotal]").text(totalQuantity);
    
    $("[id*=GridView1]input[type=text][id*=txtQuantity]").keyup(function(event) {
        var totalQuantity = calculatequantity();
        $("span[id*=lblGrossQuantityTotal]").text(totalQuantity);
    });
    $("[id*=GridView1]input[type=text][id*=txtQuantity]").change(function(event) {
        var totalQuantity = calculatequantity();
        $("span[id*=lblGrossQuantityTotal]").text(totalQuantity);
    });



    var totalhalfQuantity = calculatehalfquantity();
    $("span[id*=lblGrossQuantityHalfTotal]").text(totalhalfQuantity);

    $("[id*=GridView1]input[type=text][id*=txtHalfQuantity]").keyup(function(event) {
    var totalhalfQuantity = calculatehalfquantity();
    $("span[id*=lblGrossQuantityHalfTotal]").text(totalhalfQuantity);
    });
    $("[id*=GridView1]input[type=text][id*=txtHalfQuantity]").change(function(event) {
        var totalhalfQuantity = calculatehalfquantity();
        $("span[id*=lblGrossQuantityHalfTotal]").text(totalhalfQuantity);
    });


    function calculatequantity() {
        var totalQty = 0;
        $("[id*=GridView1]input[type=text][id*=txtQuantity]").each(function() {
            temp = parseFloat($(this).val());
            if (isNaN(temp)) temp = 0;
            totalQty = totalQty + temp;
        });
        return totalQty;
    }

    function calculatehalfquantity() {
        var totalHalfQty = 0;
        $("[id*=GridView1]input[type=text][id*=txtHalfQuantity]").each(function() {
            temp = parseFloat($(this).val());
            if (isNaN(temp)) temp = 0;
            totalHalfQty = totalHalfQty + temp;
        });
        return totalHalfQty;
    }

    var totalArea = calculateArea();
    $("span[id*=lblGrossArea]").text(totalArea);
    $("[id*=GridView1]input[type=text][id*=txtArea]").keyup(function(event) {
    var totalArea = calculateArea();
        $("span[id*=lblGrossArea]").text(totalArea);
    });
    $("[id*=GridView1]input[type=text][id*=txtArea]").change(function(event) {
        var totalArea = calculateArea();
        $("span[id*=lblGrossArea]").text(totalArea);
    });

    function calculateArea() {
        var totalAr = 0;
        $("[id*=GridView1]input[type=text][id*=txtArea]").each(function() {
            temp = parseFloat($(this).val());
            if (isNaN(temp)) temp = 0;
            totalAr = totalAr + temp;
        });
        return totalAr;
    }
    
    
    }

</script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


  <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
    ValidationGroup="main"/>
    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <uc1:Messages ID="ucMessage" runat="server" />
    </ContentTemplate>
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="btnSave" />
    </Triggers>
    </asp:UpdatePanel>
  

    <br />
  
  
  <asp:Panel ID="pnlHead" runat="server" Width="92%" Enabled="false">
  <div style="float:right;">
    <asp:Label ID="lblStatus" runat="server" Text="Status:">
    </asp:Label>
    <asp:RadioButton ID="rbdApprove" runat="server" GroupName="groupSelect" Text="Approved"/>
    <asp:RadioButton ID="rbdPending" runat="server" GroupName="groupSelect" Text="Pending" />
    <asp:RadioButton ID="rbdCancelled" runat="server" GroupName="groupSelect" Text="Cancelled"/>
     <br />
      <br />
  </div>
  </asp:Panel>  
  <br />

 
 
  <br />
 
 
 
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
              <td width="3%">
              </td>
                  <td>
                  
                                <table cellspacing="2" class="stats2" align="center" border="0" width="98%">
                                <asp:Panel runat="server" ID="Panel1">
                                
                                    <tr>
                                        <td  class="LblBgSetup">
                                            <asp:Label ID="lblTrnsfrNo" runat="server" Text="Transfer No:">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTrnsfrNo" runat="server" CssClass="RequiredFieldTxtSmall"  Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                        <td   class="LblBgSetup">
                                            <asp:Label ID="lblItem" runat="server" Text="Product:">
                                            </asp:Label> 
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlItem" runat="server" AppendDataBoundItems="true" AutoPostBack="true" CausesValidation="true" CssClass="RequiredFieldDropDown" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged" Enabled="false">
                                              <asp:ListItem Text="Select Product" Value="0" Selected="True">
                                              </asp:ListItem>
                                              </asp:DropDownList>
                                              
                                              
                                              <asp:RequiredFieldValidator ID="regVal1" runat="server" ControlToValidate="ddlItem" ValidationGroup="main"
                                              Display="None" SetFocusOnError="true" ErrorMessage="Please select product." InitialValue="0">
                                              </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td  class="LblBgSetup">
                                                <asp:Label ID="lblDate" runat="server" Text="Date:">
                                                </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="RequiredFieldTxtSmall" Enabled="false">
                                              </asp:TextBox>
                                              <ajaxToolkit:CalendarExtender ID="dateExtender" runat="server" TargetControlID="txtDate"
                                              PopupPosition="BottomRight">
                                              </ajaxToolkit:CalendarExtender>
                                        </td>
                                        <td   class="LblBgSetup">
                                                <asp:Label ID="lblDesign" runat="server" Text="Design:">
                                                </asp:Label>
                                        </td>
                                        <td>
                                                 <asp:DropDownList ID="ddlDesign" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlDesign_SelectedIndexChanged" CssClass="RequiredFieldDropDown" CausesValidation="true"  ValidationGroup="main" Enabled="false" >
                                                  <asp:ListItem Text="Select Design" Value="0" Selected="True">
                                                  </asp:ListItem>
                                                  </asp:DropDownList>
                                                  
                                                  
                                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlDesign" ValidationGroup="main"
                                                  Display="None" SetFocusOnError="true" ErrorMessage="Please select design." InitialValue="0">
                                                  </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    
                                     <tr>
                                     <td class="LblBgSetup">
                                            <asp:Label ID="lblStkHalf" runat="server" Text="Half Piece (In Stock):"></asp:Label>
                                    </td>
                                    <td>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                    
                                            <asp:TextBox ID="txtStkHalf" runat="server" width="74px" BackColor="#F2F2F2" BorderStyle="solid" BorderWidth="1" BorderColor="ActiveBorder" Text="0" CssClass="classOnlyInt" Enabled="false">
                                            </asp:TextBox>
                                            
                                            </ContentTemplate>
                                            <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlDesign" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlItem" />
                                            </Triggers>
                                            </asp:UpdatePanel>
                                    </td>
                                   
                                       
                                        <td   class="LblBgSetup">
                                                <asp:Label ID="lblColor" runat="server" Text="Color:">
                                                </asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upnlDDL1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                            
                                                <asp:DropDownList ID="ddlColor" runat="server" AppendDataBoundItems="true" Enabled="false">
                                                <asp:ListItem Text="" Selected="True" Value="0">
                                                </asp:ListItem>
                                                </asp:DropDownList>

                                            </ContentTemplate>
                                            <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlDesign" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlItem" />
                                            </Triggers>
                                            </asp:UpdatePanel>
                                        
                                        </td>
                                    </tr>
                                       <tr>
                                        <td class="LblBgSetup">
                                            <asp:Label ID="lblStkFull" runat="server" Text="Full Piece (In Stock):"></asp:Label>
                                    </td>
                                    <td>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                    
                                            <asp:TextBox ID="txtStkFull" runat="server" width="74px" BackColor="#F2F2F2" BorderStyle="solid" BorderWidth="1" BorderColor="ActiveBorder" Text="0" CssClass="classOnlyInt" Enabled="false">
                                            </asp:TextBox>
                                            
                                            </ContentTemplate>
                                            <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlDesign" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlItem" />
                                            </Triggers>
                                            </asp:UpdatePanel>
                                    </td>
                                        <td  class="LblBgSetup">
                                                <asp:Label ID="lblThick" runat="server" Text="Thickness:">
                                                </asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                            
                                                <asp:DropDownList ID="ddlThickness" runat="server" AppendDataBoundItems="true" Enabled="false">
                                                <asp:ListItem Text="" Selected="True" Value="0">
                                                </asp:ListItem>
                                                </asp:DropDownList>
                                                
                                            </ContentTemplate>
                                            <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlDesign" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlItem" />
                                            </Triggers>
                                            </asp:UpdatePanel> 
                                        </td>
                                    </tr>
                     
                                </asp:Panel>
                                </table>
                  
                  
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
              
              
              <tr>
              <td width="3%"></td>
              <td>
                    <asp:UpdatePanel ID="uPnl" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                    <asp:Panel ID="pnlGrd" runat="server">

                    <asp:GridView ID="GridView1" runat="server"  Width="50%" GridLines="None" AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="GridView1_RowDataBound" EmptyDataText="No record found.">
                                    <HeaderStyle CssClass ="grid_hdr" />
                                    <RowStyle CssClass="grid_row" />
                                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                                    <SelectedRowStyle CssClass="gridSelectedRow" />
                      <Columns>

                        
                        <asp:TemplateField HeaderText="Grade" HeaderStyle-Width="150px" ControlStyle-Width="150px">
                        <ItemTemplate>
                        <asp:TextBox ID="txtGrade" runat="server" Text='<%#Eval("Grade")%>' Enabled="false">
                        </asp:TextBox>
                        <asp:HiddenField ID="hdnGrade" runat="server" Value='<%#Eval("HdnGrade")%>'/>
                        <asp:HiddenField ID="hdnSelection" runat="server" Value='<%#Eval("HdnSelection")%>'/>
                        </ItemTemplate>
                        </asp:TemplateField>


                                            
                        <asp:TemplateField HeaderText="Full Piece*"  FooterStyle-HorizontalAlign="Right"   ControlStyle-Width="90px" HeaderStyle-Width="90px" >
                        <ItemTemplate>
                        <asp:TextBox ID="txtQuantity" runat="server" Text='<%#Eval("FullQty")%>' CssClass="classAlign" MaxLength="9"/>
                        </ItemTemplate>
                        <FooterTemplate>
                        <asp:Label ID="lblGrossQuantityTotal" runat="server" Text="0"></asp:Label>
                        </FooterTemplate>
                        </asp:TemplateField>
                        
                        
                        <asp:TemplateField HeaderText="Half Piece*"  FooterStyle-HorizontalAlign="Right"   ControlStyle-Width="90px" HeaderStyle-Width="90px" >
                        <ItemTemplate>
                        <asp:TextBox ID="txtHalfQuantity" runat="server" Text='<%#Eval("HalfQty")%>' CssClass="classAlign" MaxLength="9"/>
                        </ItemTemplate>
                        <FooterTemplate>
                        <asp:Label ID="lblGrossQuantityHalfTotal" runat="server" Text="0"></asp:Label>
                        </FooterTemplate>
                        </asp:TemplateField>
                        
                        
                        
                         <asp:TemplateField HeaderText="Area*"  FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="90px" ControlStyle-Width="90px">
                          <ItemTemplate>
                            <asp:TextBox ID="txtArea" runat="server" Text='<%#Eval("Area")%>' CssClass="classAlign" MaxLength="12"/>
                          </ItemTemplate>
                          <FooterTemplate>
                            <asp:Label ID="lblGrossArea" runat="server"  Text="0">
                            </asp:Label>
                          </FooterTemplate>
                        </asp:TemplateField>
                      
                      
                      
                      </Columns>
                      </asp:GridView>
                      </asp:Panel>
                      
                      
                      </ContentTemplate>
                      <Triggers>
                      <asp:AsyncPostBackTrigger ControlID="ddlDesign" />
                      <asp:AsyncPostBackTrigger ControlID="ddlItem" />
                      </Triggers>
                      </asp:UpdatePanel>    
              </td>
              <td width="3%"></td>
              </tr>
              
          </table>
 
  <table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
  <td width="3%"></td>
  <td>
       &nbsp;     
  </td>
  <td width="3%"></td>
  </tr>
  
  <tr>
  <td width="3%">
  </td>
  <td>
            <table cellspacing="2" class="stats2" align="center" border="0" width="98%">
            <tr>
            <td valign="top">
            
            <div style="float:left">
                <asp:ImageButton ID="btnList" runat ="server"  OnCommand="btnList_Click" ImageUrl="~/images/btn_list.png" onMouseOver="this.src='../images/btn_list_m.png'" onMouseOut="this.src='../images/btn_list.png'" />
                <asp:ImageButton ID="btnBack" runat ="server"  OnCommand="btnBack_Click" ImageUrl="~/images/btn_back.png" onMouseOver="this.src='../images/btn_back_m.png'" onMouseOut="this.src='../images/btn_back.png'" />
                <asp:ImageButton ID="btnClear" runat ="server"  OnCommand="btnClear_Click" ImageUrl="~/images/btn_clear.png" onMouseOver="this.src='../images/btn_clear_m.png'" onMouseOut="this.src='../images/btn_clear.png'" />
            </div>
         
            <div style="float:left; margin-left:4px;">
           
            <asp:UpdatePanel ID="subPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:ImageButton ID="btnSave" runat ="server"  OnCommand="btnSave_Click" ImageUrl="~/images/btn_save.png" onMouseOver="this.src='../images/btn_save_m.png'" onMouseOut="this.src='../images/btn_save.png'" ValidationGroup="main" />
            </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlDesign" />
            <asp:AsyncPostBackTrigger ControlID="ddlItem" />
            </Triggers>
            </asp:UpdatePanel>
            </div>
        </td>
        <td align="right"></td>
        </tr>
        </table>
  </td>
  <td width="3%">
  </td>
  </tr>
  </table>

  
</asp:Content>
