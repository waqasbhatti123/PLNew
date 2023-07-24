<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchCostMgt.aspx.cs" Inherits="RMS.Inv.PurchCostMgt" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head runat="server">
<title></title>
    <link rel="Stylesheet" href="../cs/style.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/jquery-ui.min.js"></script> 
<script type="text/javascript" language="javascript">
    //DISABLING DOUBLE CLICK
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    function BeginRequestHandler(sender, args) { var oControl = args.get_postBackElement(); oControl.disabled = true; }

</script>    
    <script type="text/javascript">

        function pageLoad() {
        
            $("[id*=GridView1]input[type=text][id*=txtAmount]").keydown(function(event) {
                if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                    event.preventDefault();
                }
            });

            $("[id*=GridView1]input[type=text][id*=txtAmount]").each(function() {
                $(this).css('text-align', 'right');
            });

            var totalAmount = calculateAmount();
            $("span[id*=lblAmountTotal]").text(totalAmount);

            $("[id*=GridView1]input[type=text][id*=txtAmount]").keyup(function(event) {
                var totalAmount = calculateAmount();
                $("span[id*=lblAmountTotal]").text(totalAmount);
            });

            $("[id*=GridView1]input[type=text][id*=txtAmount]").change(function(event) {
                var totalAmount = calculateAmount();
                $("span[id*=lblAmountTotal]").text(totalAmount);
            });
            
            function calculateAmount() {
                var totalvat = 0;
                $("[id*=GridView1]input[type=text][id*=txtAmount]").each(function() {
                    temp = parseFloat($(this).val());
                    if (isNaN(temp)) temp = 0;
                    totalvat = totalvat + temp;
                });
                return totalvat;
            }
            function CloseWindow() {
                window.close();
            }
    }
    </script> 
</head>

<body style="background-color:#F2F2F2;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="36000" runat="server">
        </asp:ScriptManager>
         
        <div style="width:700px;" class="bodyText">
              <div style="height:15px;"></div>
              <table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
              <td width="3%">
              </td>
              <td>
                <asp:Label ID="lblTitle" CssClass="mainHeading" runat="server"></asp:Label>
              </td>
              <td width="3%">
              </td>
              </tr>
              </table>
              
              
              <div style="height:10px;"></div>  
              <table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
              <td width="3%">
              </td>
              <td>
                    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List" ValidationGroup="main"/>
                    <uc1:Messages ID="ucMessage" runat="server" />
              </td>
              <td width="3%">
              </td>
              </tr>
              </table>
              <div style="height:5px;"></div>
        </div>
            
        <div style="width:700px;" class="bodyText">
   
              <table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
              <td width="3%"> &nbsp; </td>
              <td>
                    <asp:Panel ID="pnlFields" runat="server" Enabled="true">
                    <table>
                     <tr>
                                <td> &nbsp; </td>
                                <td> &nbsp; </td>
                                <td> &nbsp; </td>
                                <td> &nbsp; </td>
                      </tr>
                      <%--<tr>
                                <td  class="LblBgSetup">
                                    <asp:Label ID="lblDocNo" runat="server" Text="Doc. No:">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDocNo" runat="server" CssClass="RequiredFieldTxtSmall" Enabled="false">
                                    </asp:TextBox>
                                </td>
                                <td> &nbsp; </td>
                                <td> &nbsp; </td>
                    </tr>
                    <tr>
                                <td   class="LblBgSetup">
                                    <asp:Label ID="lblDocDate" runat="server" Text="Doc. Date:">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDocDate" runat="server" CssClass="RequiredFieldTxtSmall" Enabled="false">
                                    </asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="C1" runat="server" TargetControlID="txtDocDate" PopupPosition="BottomRight">
                                    </ajaxToolkit:CalendarExtender>
                                </td>
                                <td> &nbsp; </td>
                                <td> &nbsp; </td>
                    </tr>--%>
                     <tr>
                                <td   class="LblBgSetup">
                                    <asp:Label ID="lblVendor" runat="server" Text="Vendor:">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlVendor" runat="server" AppendDataBoundItems="true" CssClass="RequiredFieldDropDownVendor" Enabled="false">
                                    </asp:DropDownList>
                                </td>
                               <td> &nbsp; </td>
                                <td> &nbsp; </td>
                    </tr>
                    </table>
              <div style="height:15px;"></div>  
              </asp:Panel>
              </td>
              <td width="3%"> &nbsp; </td>
              </tr>
              <tr>
              <td width="3%"> &nbsp; </td>
              <td> 
                    <asp:Panel ID="pnlCost" runat="server">
                        <asp:GridView ID="GridView1" runat="server"  Width="100%" OnRowDataBound="GridView1_RowDataBound" GridLines="None" AutoGenerateColumns="false" ShowFooter="true" >
                                        <HeaderStyle CssClass ="grid_hdr" />
                                        <RowStyle CssClass="grid_row" />
                                        <AlternatingRowStyle CssClass="gridAlternateRow" />
                                        <SelectedRowStyle CssClass="gridSelectedRow" />
                          <Columns>
                        
                          
                          
                            <asp:TemplateField HeaderText="ID"  FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" ControlStyle-Width="30px">
                            <ItemTemplate>
                            <asp:TextBox ID="txtCostId" runat="server" Text='<%#Eval("CostId")%>' Enabled="false">
                            </asp:TextBox>                        
                            </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Temp ID"  FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" ControlStyle-Width="60px">
                            <ItemTemplate>
                            <asp:TextBox ID="txtTempId" runat="server" Text='<%#Eval("TempId")%>' Enabled="false">
                            </asp:TextBox>                        
                            </ItemTemplate>
                            </asp:TemplateField>
                            
                            
                            <asp:TemplateField HeaderText="Description"  FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="150px" ControlStyle-Width="150px">
                            <ItemTemplate>
                            <asp:TextBox ID="txtDesc" runat="server" Text='<%#Eval("Desc")%>' Enabled="false">
                            </asp:TextBox>                        
                            </ItemTemplate>
                            </asp:TemplateField>
                                 
                          
                           <asp:TemplateField HeaderText="Date" HeaderStyle-Width="70px" ControlStyle-Width="70px">
                            <ItemTemplate>
                            <asp:TextBox ID="txtDate" runat="server" Text='<%#Eval("Date")%>' >
                            </asp:TextBox> 
                            <ajaxToolkit:CalendarExtender ID="C2" runat="server" TargetControlID="txtDate" PopupPosition="BottomRight">
                            </ajaxToolkit:CalendarExtender>                     
                            </ItemTemplate>
                            </asp:TemplateField>
                          
                          
                          
                            <asp:TemplateField HeaderText="Doc Ref" FooterStyle-HorizontalAlign="Right"   ControlStyle-Width="70px" HeaderStyle-Width="70px" >
                            <ItemTemplate>
                            <asp:TextBox ID="txtDocRef" runat="server" Text='<%#Eval("DocRef")%>' MaxLength="9"/>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblTotal" runat="server" Text="Total"></asp:Label>
                              </FooterTemplate>
                            </asp:TemplateField>

                          
                            <asp:TemplateField HeaderText="Amount*"  FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"  ControlStyle-Width="80px">
                              <ItemTemplate>
                                <asp:TextBox ID="txtAmount" runat="server" Text='<%#Eval("Amount")%>' MaxLength="9"/>
                              </ItemTemplate>
                              <FooterTemplate>
                                <asp:Label ID="lblAmountTotal" runat="server" Text="0"></asp:Label>
                              </FooterTemplate>
                            </asp:TemplateField>
                          
                             <asp:TemplateField HeaderText="Remarks"  HeaderStyle-Width="175px" ControlStyle-Width="175px">
                                  <ItemTemplate>
                                    <asp:TextBox ID="txtRem" runat="server" Text='<%#Eval("Remarks")%>' MaxLength="100"/>
                                  </ItemTemplate>
                                </asp:TemplateField>
                              
                          </Columns>
                          </asp:GridView>
                          </asp:Panel>  
              </td>
              <td width="3%"> &nbsp; </td>
              </tr>
              <tr>
              <td width="3%"> &nbsp; </td>
              <td>
                         <asp:ImageButton ID="btnAdd" runat="server"  ImageUrl="~/images/btn_Add.png" OnClick="btnAdd_Click"
                         onMouseOver="this.src='../images/btn_Add_m.png'" onMouseOut="this.src='../images/btn_Add.png'" ValidationGroup="main" />
                         <asp:ImageButton ID="btnBack" runat="server"  ImageUrl="~/images/btn_Back.png" OnClick="btnBack_Click"
                         onMouseOver="this.src='../images/btn_Back_m.png'" onMouseOut="this.src='../images/btn_Back.png'" />
              </td>
              <td width="3%"> &nbsp; </td>
              </tr>
              <tr>
              <td width="3%"> &nbsp; </td>
              <td>            &nbsp; </td>
              <td width="3%"> &nbsp; </td>
              </tr>
              </table>
              
        </div>

       
    </form>
</body>
</html>
