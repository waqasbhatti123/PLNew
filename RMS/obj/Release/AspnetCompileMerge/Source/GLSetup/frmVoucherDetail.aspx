<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/home/RMSMasterHome.Master"
CodeBehind="frmVoucherDetail.aspx.cs" Culture="auto" UICulture="auto" 
Inherits="RMS.GLSetup.frmVoucherDetail" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" 
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

    function OpenNewWindow() {
        window.open('../GLSetup/ViewLedgerCard.aspx?Code=', 'myPopup')
    }
    function pageLoad() {

        var totaldebit = calculatedebit();
        var totalcredit = calculatecredit();
        $("span[id*=lbldebit]").text(totaldebit);
        $("span[id*=lblcredit]").text(totalcredit);
        $("span[id*=lblbalance]").text(totaldebit - totalcredit);
        $('#<%= txtnarration.ClientID %>').focus();

        var glCd;
        $('#<%= txtChqBranch.ClientID %>').autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: "frmVoucherDetail.aspx/GetBranch",
                    data: "{ 'bank': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function(data) { return data; },
                    success: function(data) {
                        response($.map(data.d, function(item) {
                            return {
                                value: item.gl_cd + ' - ' + item.gl_dsc,
                                result: item.STN,
                                id: item.gl_cd
                            }
                        }))
                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            },
            select: function(e, ui) {
                glCd = ui.item.id;
                getFirstRowCode();
                if (ui.item.result != '') {
                    $('#<%= txtChqAcctNo.ClientID %>').val(ui.item.result);
                }
                else {
                    $('#<%= txtChqAcctNo.ClientID %>').val('');
                }
                $('#<%= txtChqNo.ClientID %>').focus();
            },
            minLength: 1
        });

        $('#<%= btnSave.ClientID %>').click(function(event) {
            if (glCd != null) {
                $('#<%= txtChqBranch.ClientID %>').val(glCd);
            }
        });


        $("[id*=grdView]input[type=text][id*=txtcode]").autocomplete({
            source: function(request, response) {
                $.ajax({
                    url: "frmPreferences.aspx/GetDetailAccount1",
                    data: "{ 'sname': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    async: false,
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function(data) { return data; },
                    success: function(data) {
                        response($.map(data.d, function(item) {
                            return {
                                value: item.gl_cd + ' - ' + item.gl_dsc,
                                result: item.gl_cd + ' - ' + item.gl_dsc,
                                id: item.gl_cd
                            }
                        }))
                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            },

            select: function(e, ui) {
                var codeItm = ui.item.result;
                codeItm = codeItm.split(" - ");
                $(e.target).closest('tr').find("input[type=text][id*=txtcode]").val(codeItm[0]);
                $(e.target).closest('tr').find("input[type=text][id*=txtdescription]").val(codeItm[1]);
                //$(e.target).closest('tr').find("input[type=text][id*=txtdebit]").focus();
                return false;
            },

            minLength: 1
        });

        $("[id*=grdView]input[type=text][id*=txtdebit]").keydown(function(event) {
            if (event.keyCode == 13) {
                $(event.target).closest('tr').find("input[type=text][id*=txtcredit]").focus();

            }
            var txtDbt = $(this).closest('tr').find("input[type=text][id*=txtdebit]").val();
            if (event.keyCode != 110 && event.keyCode != 190 && (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }
            if (event.keyCode == 110 || event.keyCode == 190) {
                if ((txtDbt.split(".").length) > 1) {
                    event.preventDefault();
                }
            }

        });
        
        $("[id*=grdView]input[type=text][id*=txtdebit]").keyup(function(e) {
            var debit = $(this).closest('tr').find("input[type=text][id*=txtdebit]").val();
            if (parseInt(debit) > 0) {
                $(e.target).closest('tr').find("input[type=text][id*=txtcredit]").val(0);
            }
            if ($(this).closest('tr').find("input[type=text][id*=txtcode]").val() == "") {
                $(this).closest('tr').find("input[type=text][id*=txtcode]").focus();
            }
            var totaldebit = calculatedebit();
            var totalcredit = calculatecredit();
            $("span[id*=lbldebit]").text(totaldebit);
            $("span[id*=lblcredit]").text(totalcredit);
            $("span[id*=lblbalance]").text(totaldebit - totalcredit);
        });
        
        $("[id*=grdView]input[type=text][id*=txtdebit]").change(function(e) {

            var debit = $(this).closest('tr').find("input[type=text][id*=txtdebit]").val();
            if (parseInt(debit) > 0) {
                $(e.target).closest('tr').find("input[type=text][id*=txtcredit]").val(0);
            }
            if ($(this).closest('tr').find("input[type=text][id*=txtcode]").val() == "") {
                $(this).closest('tr').find("input[type=text][id*=txtcode]").focus();
            }
            var totaldebit = calculatedebit();
            var totalcredit = calculatecredit();
            $("span[id*=lbldebit]").text(totaldebit);
            $("span[id*=lblcredit]").text(totalcredit);
            $("span[id*=lblbalance]").text(totaldebit - totalcredit);
        });


        $("[id*=grdView]input[type=text][id*=txtcredit]").keydown(function(event) {
            if (event.keyCode == 13) {
                $(event.target).closest('tr').find("input[type=text][id*=txtremark]").focus();

            }
            var txtCrdt = $(this).closest('tr').find("input[type=text][id*=txtcredit]").val();
            if (event.keyCode != 110 && event.keyCode != 190 && (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105) && (event.keyCode < 37 || event.keyCode > 40) && event.keyCode != 46 && event.keyCode != 8 && event.keyCode != 9) {
                event.preventDefault();
            }
            if (event.keyCode == 110 || event.keyCode == 190) {
                if ((txtCrdt.split(".").length) > 1) {
                    event.preventDefault();
                }
            }
        });
        
        $("[id*=grdView]input[type=text][id*=txtcredit]").keyup(function(e) {
            var credit = $(this).closest('tr').find("input[type=text][id*=txtcredit]").val();
            if (parseInt(credit) > 0) {
                $(e.target).closest('tr').find("input[type=text][id*=txtdebit]").val(0);
            }
            if ($(this).closest('tr').find("input[type=text][id*=txtcode]").val() == "") {
                $(this).closest('tr').find("input[type=text][id*=txtcode]").focus();
            }
            var totaldebit = calculatedebit();
            var totalcredit = calculatecredit();
            $("span[id*=lbldebit]").text(totaldebit);
            $("span[id*=lblcredit]").text(totalcredit);
            $("span[id*=lblbalance]").text(totaldebit - totalcredit);
        });
        
        $("[id*=grdView]input[type=text][id*=txtcredit]").change(function(e) {
            var credit = $(this).closest('tr').find("input[type=text][id*=txtcredit]").val();
            if (parseInt(credit) > 0) {
                $(e.target).closest('tr').find("input[type=text][id*=txtdebit]").val(0);
            }
            if ($(this).closest('tr').find("input[type=text][id*=txtcode]").val() == "") {
                $(this).closest('tr').find("input[type=text][id*=txtcode]").focus();
            }
            var totaldebit = calculatedebit();
            var totalcredit = calculatecredit();
            $("span[id*=lbldebit]").text(totaldebit);
            $("span[id*=lblcredit]").text(totalcredit);
            $("span[id*=lblbalance]").text(totaldebit - totalcredit);
        });

        function getFirstRowCode() {

            $.ajax({
                url: "frmVoucherDetail.aspx/GetCodeDesc",
                data: JSON.stringify({ glCd: glCd }),
                type: 'POST',
                contentType: 'application/json;',
                dataType: 'json',
                success: function(heads) {
                    var head = heads.d;
                    if (head.length > 0) {
                        $('#<%=grdView.ClientID%> tr:nth-child(2) ').find("input[type=text][id*=txtcode]").val(glCd);
                        $('#<%=grdView.ClientID%> tr:nth-child(2) ').find("input[type=text][id*=txtdescription]").val(head);
                    }
                }
            });
        }

        function calculatedebit() {
            var total = 0;
            $("[id*=grdView]input[type=text][id*=txtdebit]").each(function() {
                temp = parseFloat($(this).val());
                if (isNaN(temp)) temp = 0;
                total = total + temp;
            });
            return total;
        }

        function calculatecredit() {
            var total = 0;
            $("[id*=grdView]input[type=text][id*=txtcredit]").each(function() {
                temp = parseFloat($(this).val());
                if (isNaN(temp)) temp = 0;
                total = total + temp;
            });
            return total;
        }
      }
    
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<asp:ValidationSummary ID="one" CssClass="ErrorSummary" runat="server" DisplayMode="List" ValidationGroup="one" />
<uc1:Messages ID="ucMessage" runat="server" />

<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td></td>
        <td>
                <fieldset>
                    <legend style=" margin:5px;">
                           <b> 
                                <asp:Label ID="Label1" runat="server" Text="Enter"></asp:Label> 
                                <asp:Label ID="lblPgTilte" runat="server"></asp:Label> 
                           </b>
                    </legend>
                    <table width="100%" cellspacing="3">
                        <tr valign="top"">
                              <td>
                            
                              <label>Branch</label>
                        </td>
                        <td>
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged"
                                AppendDataBoundItems="True" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Branch</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                            <td rowspan="3">
                                <asp:Label ID="lblnarration" Text="Narration" runat="server"></asp:Label>
                            </td>
                            <td rowspan="3">
                                <asp:TextBox ID="txtnarration" runat="server" CssClass="RequiredField" TextMode="MultiLine"   onkeyup="LimitText(this,100);" onblur="LimitText(this,100);"  Width="422" Height="62"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="one" runat="server" ControlToValidate="txtnarration" ErrorMessage="Please Enter Voucher Narration" SetFocusOnError="true" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="lblstatus" Text="Status" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlstatus" runat="server" AppendDataBoundItems="True">
                                     <asp:ListItem Value="A">Approved</asp:ListItem>
                                     <asp:ListItem Value="P" Selected="True">Pending</asp:ListItem>
                                     <asp:ListItem Value="D">Cancelled</asp:ListItem>
                             </asp:DropDownList>
                            </td>
                            
                        </tr>
                        <tr valign="top">
                           <td>
                                <asp:Label ID="lbldate" Text="Date" runat="server"></asp:Label>
                            </td>
                            <td>
                                <ajaxToolkit:CalendarExtender ID="txtDate" runat="server" TargetControlID="txtdt" Enabled="True"/>
                                <asp:TextBox ID="txtdt" runat="server" Width="80"></asp:TextBox>
                            </td>
                        </tr>
                        <tr valign="top">
                         <td>
                                <asp:Label ID="lblRefSource" Text="" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRefSource" runat="server" Width="150" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        
                    </table>
                </fieldset>
        </td>
        <td></td>
    </tr>
    <tr>
        <td></td>
        <td>
            &nbsp;
        </td>
        <td></td>
    </tr>
    <tr  runat="server" id="divBankData" visible="false">
        <td></td>
        <td>
            <fieldset  class="fieldSet">
                    <legend style=" margin:5px;">
                           <b> 
                                Cheque Details
                           </b>
                    </legend>
                    
                    <div> 
                                     
                         <table  width="100%" cellspacing="0"> 
                            <tr valign="top">
                                <td>
                                    Branch
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChqBranch" runat="server" CssClass="RequiredField" MaxLength="50" Width="220"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtChqBranch"
                                        ErrorMessage="Please enter branch name" SetFocusOnError="true" ValidationGroup="one"
                                        Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    Account #
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChqAcctNo" runat="server" MaxLength="20" Width="100"></asp:TextBox>
                                </td>
                                <td>
                                    Cheque #
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChqNo" runat="server" CssClass="RequiredField" MaxLength="20" Width="100"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtChqNo"
                                        ErrorMessage="Please enter cheque #" SetFocusOnError="true" ValidationGroup="one"
                                        Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    Date
                                </td>
                                <td>
                                    
                                    <ajaxToolkit:CalendarExtender ID="txtChqDateCal" runat="server" TargetControlID="txtChqDate"
                                        Enabled="True">
                                    </ajaxToolkit:CalendarExtender>
                                    <asp:TextBox ID="txtChqDate" runat="server" CssClass="RequiredFieldDate"></asp:TextBox><br />
                                    <span class="DteLtrl">
                                        <asp:Literal ID="Literal1"  runat="server" Text="<%$ AppSettings: DateFormatPageText %>" />
                                    </span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtChqDate"
                                        ErrorMessage="Please select date of check" SetFocusOnError="true" ValidationGroup="one"
                                        Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                         </table>
                         
                         </div>
            </fieldset>
        </td>
        <td></td>
    </tr>
    <tr>
        <td></td>
        <td>
            &nbsp;
        </td>
        <td></td>
    </tr>
    <tr>
        <td colspan="3">
            <fieldset>
                <legend>
                        <b> Voucher Details </b>
                </legend>
                   
                    <table class="table">
                     <tr>
                      <td align="center">
                       <asp:UpdatePanel runat="server" ID="uPnl" UpdateMode="Conditional">
                        <ContentTemplate>
                         <asp:GridView runat="server" ID="grdView" CssClass="t_grd" ShowFooter="True" OnRowDataBound ="grdView_RowDataBound" AutoGenerateColumns="false">
                          <HeaderStyle CssClass="t_grd_hdr"></HeaderStyle>
                          <FooterStyle CssClass="t_grd_footer"></FooterStyle>
                          <RowStyle CssClass="t_grd_Row"></RowStyle>
                          <EditRowStyle CssClass="t_grd_Edit_Row" />
                          <SelectedRowStyle CssClass="t_grd_Selected_Row"  />
                          <AlternatingRowStyle CssClass="t_grd_Alter_Row"/>
                          <PagerStyle CssClass="t_grd_Pager"></PagerStyle>
                          <PagerSettings Mode="NumericFirstLast" />
                          <EmptyDataRowStyle CssClass="Label_Small_Bold" ForeColor="#C00000" HorizontalAlign="Center" />
                          <Columns>
                            <asp:TemplateField HeaderText="#">
                             <ItemTemplate>
                              <asp:TextBox runat="server" ID="txtseq"  Text='<%#Eval("seq") %>' ReadOnly="true" TabIndex="-1" Width="10px" Font-Size="10px"></asp:TextBox>
                             </ItemTemplate>
                             <ControlStyle />
                             <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                             <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="10px"/>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="GL Code">
                             <ItemTemplate>
                              <asp:TextBox runat="server" ID="txtcode" Text='<%#Eval("code") %>' Width="60px" Font-Size="10px" ></asp:TextBox>
                             </ItemTemplate>
                             <ControlStyle />
                             <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                             <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Description">
                             <ItemTemplate>
                              <asp:TextBox runat="server" ID="txtdescription" Text='<%#Eval("desc") %>' TabIndex="-1" Width="220px" Font-Size="10px"></asp:TextBox>
                             </ItemTemplate>
                             <ControlStyle />
                             <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                             <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="220px"/>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Debit" >
                             <ItemTemplate>
                              <asp:TextBox runat="server" ID="txtdebit" Text='<%#Eval("debit") %>' Width="60px" Font-Size="10px" style="text-align:right"></asp:TextBox>
                             </ItemTemplate>
                             <FooterTemplate> 
                              <asp:Label ID="lbldebit" runat="server" style="text-align:right" Font-Size="10px"></asp:Label>
                             </FooterTemplate>
                             <ControlStyle />
                             <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                             <FooterStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                             <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Credit">
                             <ItemTemplate>
                              <asp:TextBox runat="server" ID="txtcredit" Text='<%#Eval("credit") %>' Width="60px" Font-Size="10px" style="text-align:right"></asp:TextBox>
                             </ItemTemplate>
                             <FooterTemplate> 
                              <asp:Label ID="lblcredit" runat="server" style="text-align:right" Font-Size="10px"></asp:Label>
                             </FooterTemplate>
                             <ControlStyle />
                             <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                             <FooterStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                             <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="60px"/>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Remarks">
                             <ItemTemplate>
                              <asp:TextBox runat="server" ID="txtremarks" Text='<%#Eval("remark") %>' Width="100px" Font-Size="10px"></asp:TextBox>
                             </ItemTemplate>
                             <FooterTemplate> 
                              <asp:Label ID="lblbalance" runat="server"  style="text-align:center" Font-Size="10px"></asp:Label>
                             </FooterTemplate>
                             <ControlStyle />
                             <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                             <FooterStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                             <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="100px"/>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Cost Center">
                             <ItemTemplate>
                              <asp:DropDownList runat="server" ID="ddlcostcenter" AppendDataBoundItems="true" Width="110px" Height="28px" Font-Size="10px">
                               <asp:ListItem Text="" Value=""></asp:ListItem>
                              </asp:DropDownList>
                             </ItemTemplate>
                             <ControlStyle />
                             <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                             <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="110px"/>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                             <ItemTemplate>
                              <asp:LinkButton ID="lnkInfo" runat="server" Text="Info" CssClass="lnk" OnClientClick="OpenNewWindow();" OnClick="lnkInfo_Click" ToolTip="Ledger Card Info" Width="20px">
                              </asp:LinkButton>                                          
                             </ItemTemplate>
                             <ControlStyle />
                             <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                             <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="20px"/>
                            </asp:TemplateField>
                          </Columns>
                        </asp:GridView>
                     </ContentTemplate>
                     <Triggers>
                      <asp:AsyncPostBackTrigger ControlID="linkBtn"/>
                     </Triggers>  
                    </asp:UpdatePanel>
                  </td>
                 </tr>
                 <tr>
                  <td colspan="3">
                   <div style="float:right">
                    <asp:UpdatePanel runat="server" ID="updLinkButton" UpdateMode="Conditional">
                      <ContentTemplate>
                       <asp:LinkButton runat="server" ID="linkBtn" Text="Add Rows" OnClick="linkBtn_Click" CssClass="lnk"></asp:LinkButton>           
                    </ContentTemplate>  
                    </asp:UpdatePanel>
                   </div>
                  </td>
                 </tr>   
                 <tr>
                  <td colspan="3">
                   <div align="left"> 
                    <asp:ImageButton ID="btnSave" runat="server"  ImageUrl="~/images/btn_save.png" OnClick="btnSave_Click"
                     onMouseOver="this.src='../images/btn_save_m.png'" onMouseOut="this.src='../images/btn_save.png'"   ValidationGroup="one"/>
                    <asp:ImageButton ID="btnCancel" runat="server"  ImageUrl="~/images/btn_clear.png" OnClick="btnCancel_Click" 
                     onMouseOver="this.src='../images/btn_clear_m.png'" onMouseOut="this.src='../images/btn_clear.png'"/>
                   </div>
                  </td>
                </tr>
                    </table>
             </fieldset>
        </td>
    </tr>
    <tr>
        <td></td>
        <td>
            &nbsp;
        </td>
        <td></td>
    </tr>
    <tr>
        <td></td>
        <td>
            &nbsp;
        </td>
        <td></td>
    </tr>
    <tr>
        <td></td>
        <td>
       <div id="divRem" runat="server" style="float:right;">
         <table cellpadding="0" cellspacing="0" border="0" width="410" style="background-color:#e2e4e6;">
            <tr>
                <td>&nbsp;</td>
                <td>
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td colspan="2">
                                    <div class="model_popup_panel_hdr">Remarks</div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                   &nbsp; <uc1:Messages ID="Messages1" runat="server" />
                                </td>
                            </tr>
                        </table>
                    <div id="divRemEntry" runat="server">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr valign="top">
                                <td colspan="2">
                                    <asp:TextBox ID="txtRemarks" runat="server" Width="99.5%" Height="80" TextMode="MultiLine" onkeyup="LimitText(this,500);" onblur="LimitText(this,500);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ImageButton ID="ImageButton1" runat="server"  ImageUrl="~/images/btn_save.png" OnClick="btnSave_Click" onMouseOver="this.src='../images/btn_save_m.png'" onMouseOut="this.src='../images/btn_save.png'"/>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                        </table>
                     </div>
                     <div>
                            <asp:GridView ID="grdRemarks" runat="server" AutoGenerateColumns="False" EmptyDataText="No remarks found" Width="100%" AllowPaging="true" PageSize="3" OnPageIndexChanging="grdRemarks_PageIndexChanging" OnRowDataBound="grdRemarks_RowDataBound" RowStyle-VerticalAlign="Top">
                                <HeaderStyle CssClass ="grid_hdr" />
                                <FooterStyle CssClass="FooterStyle" />
                                <RowStyle CssClass="grid_row" />
                                <AlternatingRowStyle CssClass="gridAlternateRow" />
                                 <SelectedRowStyle CssClass="gridSelectedRow" />
                                    <Columns>
                                        <asp:BoundField DataField="Rmk_seq" HeaderText="Seq" ItemStyle-ForeColor="Red" ItemStyle-Width="20%" ItemStyle-CssClass="DisplayNone" ControlStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone"/>
                                        <asp:BoundField DataField="updateon" HeaderText="Date" ItemStyle-ForeColor="Red" ItemStyle-Width="20%" />
                                        <asp:BoundField DataField="Remark" HeaderText="Existing Remarks" />
                                    </Columns>
                            </asp:GridView>
                     </div>
                </td>
                <td>&nbsp;</td>
            </tr>
         </table>
     </div>
        </td>
        <td></td>
    </tr>
</table>

</asp:Content>


