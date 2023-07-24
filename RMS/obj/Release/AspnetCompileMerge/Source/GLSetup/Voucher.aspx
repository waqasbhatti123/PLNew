<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/home/RMSMasterHome.Master"
CodeBehind="Voucher.aspx.cs" Culture="auto" UICulture="auto" 
Inherits="RMS.GLSetup.Voucher" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" 
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--<script type="text/javascript" language="javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    function BeginRequestHandler(sender, args) { var oControl = args.get_postBackElement(); oControl.disabled = true; }
</script>--%>
<script type="text/javascript">
        function CheckForCostCenters() {
            var reqGlCd = '';
            //****************************************************************************
            var status = 'go';
            $("[id*=grdView]input[type=text][id*=txtcode]").each(function () {
                var glcd = $(this).val();
                var cc = $(this).closest('tr').find("[id*=ddlcostcenter]").val();
                if (glcd != "") {
                    $.ajax({
                        url: "Vouchers.aspx/GetGlCodeRec",
                        data: JSON.stringify({ glcd: glcd }),
                        type: 'POST',
                        async: false,
                        contentType: 'application/json;',
                        dataType: 'json',
                        success: function (data) {
                            var data = data.d;
                            if ((data[0].gt_cd == 'I' || data[0].gt_cd == 'E' || data[0].gt_cd == 'S') && cc == "") {
                                if (status == 'go') {
                                    reqGlCd = data[0].gl_cd;
                                    status = 'done';
                                }
                            }
                        }
                    });
                }
            });
            //****************************************************************************
            if (status == 'done') {
                if (confirm('Cost center expected in ' + reqGlCd + '. ' + 'Are you sure you want to continue?')) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else
                return true;
        }


    function OpenNewWindow() {
        window.open('../GLSetup/ViewLedgerCard.aspx?Code=', 'myPopup')
    }
    function pageLoad() {


        var totaldebit = calculatedebit();
        var totalcredit = calculatecredit();
        $("span[id*=lbldebit]").text(totaldebit);
        $("span[id*=lblcredit]").text(totalcredit);
        $("span[id*=lblbalance]").text(parseFloat(totaldebit - totalcredit).toFixed(2));
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
                    url: "Voucher.aspx/GetDetailAccountForTemplates",
                    data: "{ 'sname': '" + request.term + "' }",
                    dataType: "json",
                    type: "POST",
                    async: false,
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataFilter: function(data) { return data; },
                    success: function(data) {
                        response($.map(data.d, function (item) {
                            debugger
                            return {
                                value: item.code + ' <> ' + item.gl_dsc,
                                result: item.code + ' <> ' + item.gl_dsc,
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
                codeItm = codeItm.split(" <> ");
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
            $("span[id*=lblbalance]").text(parseFloat(totaldebit - totalcredit).toFixed(2));
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
            $("span[id*=lblbalance]").text(parseFloat(totaldebit - totalcredit).toFixed(2));
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
            $("span[id*=lblbalance]").text(parseFloat(totaldebit - totalcredit).toFixed(2));
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
            $("span[id*=lblbalance]").text(parseFloat(totaldebit - totalcredit).toFixed(2));
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
            return total.toFixed(2);
        }

        function calculatecredit() {
            var total = 0;
            $("[id*=grdView]input[type=text][id*=txtcredit]").each(function() {
                temp = parseFloat($(this).val());
                if (isNaN(temp)) temp = 0;
                total = total + temp;
            });
            return total.toFixed(2);
        }
      }
    
</script>

    <style type="text/css">
        .style1
        {
            height: 84px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<asp:ValidationSummary ID="onesumm" CssClass="ErrorSummary" runat="server" DisplayMode="List" ValidationGroup="one" />
<uc1:Messages ID="ucMessage" runat="server" />


<div class="row">
    <div class="col-md-12">
        <div class="card card-shadow mb-4 ">
            <div class="card-body"> 

                <div class="form-group">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <label>Narration*</label>
                            <asp:TextBox ID="txtnarration" runat="server" CssClass="form-control" TextMode="MultiLine"/>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="one" runat="server" ControlToValidate="txtnarration" ErrorMessage="Please Enter Voucher Narration" SetFocusOnError="true" Display="None"></asp:RequiredFieldValidator>
                        </div>
                    </div>
				</div>

                <div class="form-group">
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Voucher# *</label>
                            <asp:TextBox ID="txtVoucherNo" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Date*</label>
                            <asp:TextBox ID="txtdt" runat="server" CssClass="form-control"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtDate" runat="server" TargetControlID="txtdt" Enabled="True"/>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Status*</label>
                            <asp:DropDownList ID="ddlstatus" runat="server" CssClass="form-control" AppendDataBoundItems="True">
                             </asp:DropDownList>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label style="display:none">Source*</label>
                            <asp:DropDownList ID="ddlSource" runat="server" CssClass="form-control" AppendDataBoundItems="True" Visible="false">
                             </asp:DropDownList>
                        </div>
                    </div>
				</div>
                
                <div id="divBankData" runat="server" visible="false">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4" style="display:none">
                                <label>Bank*</label>
                                <asp:TextBox ID="txtChqBranch" runat="server" CssClass="form-control"></asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtChqBranch"
                                        ErrorMessage="Please enter branch name" SetFocusOnError="true" ValidationGroup="one"
                                        Display="None"></asp:RequiredFieldValidator>--%>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="display:none">
                                <label>Account# *</label>
                                <asp:TextBox ID="txtChqAcctNo" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Cheque# *</label>
                                <asp:TextBox ID="txtChqNo" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtChqNo"
                                        ErrorMessage="Please enter cheque #" SetFocusOnError="true" ValidationGroup="one"
                                        Display="None"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Cheque Date*</label>
                                <asp:TextBox ID="txtChqDate" runat="server" CssClass="form-control"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="txtChqDateCal" runat="server" TargetControlID="txtChqDate" Enabled="True">
                                </ajaxToolkit:CalendarExtender>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtChqDate"
                                        ErrorMessage="Please select date of check" SetFocusOnError="true" ValidationGroup="one"
                                        Display="None"></asp:RequiredFieldValidator>
                            </div>
                        </div>
				    </div>
                    <%--<div class="form-group" runat="server" id="divTax">
                        <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Income Tax:</label>
                            <asp:TextBox ID="txtIncomeTax" runat="server" CssClass="form-control"/>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>GST Tax:</label>
                            <asp:TextBox ID="txtGST"  runat="server" CssClass="form-control"/>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>PRA Tax:</label>
                            <asp:TextBox ID="txtPRA" runat="server" CssClass="form-control"/>
                        </div>
                    </div>
                    </div>--%>
                    <div class="form-group">
                        <div class="row">
                            
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Label ID="lblRefSource" Text="" runat="server"></asp:Label>
                                <asp:TextBox ID="txtRefSource" runat="server" Width="150" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">

                            </div>
                        </div>
				    </div>
                </div>

                <div class="form-group">
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label ID="Label1" Text="" runat="server">Account*</asp:Label>
                            <asp:DropDownList ID="ddlSingleAccount" CssClass="form-control form-control-sm" runat="server" AppendDataBoundItems="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlSingleAccount"
                                        ErrorMessage="Select account" SetFocusOnError="true" ValidationGroup="one" InitialValue="0"
                                        Display="None"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                    </div>
				</div>

                <div class="form-group">
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                    </div>
				</div>

                <div class="form-group">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">

                       <asp:UpdatePanel runat="server" ID="uPnl" UpdateMode="Conditional">
                        <ContentTemplate>
                         <asp:GridView runat="server" ID="grdView" ShowFooter="True" OnRowDataBound ="grdView_RowDataBound" AutoGenerateColumns="false" class="table table-responsive-sm" Width="100%"  >
                          <PagerSettings Mode="NumericFirstLast" />
                          <EmptyDataRowStyle HorizontalAlign="Center" />
                          <Columns>

                            <asp:TemplateField HeaderText="#">
                             <ItemTemplate>
                              <asp:TextBox runat="server" ID="txtseq"  Text='<%#Eval("seq") %>' ReadOnly="true" TabIndex="-1" Width="40px" CssClass="form-control form-control-sm"></asp:TextBox>
                             </ItemTemplate>
                             <ControlStyle />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Heads of Accounts">
                             <ItemTemplate>
                              <asp:TextBox runat="server" ID="txtcode" Text='<%#Eval("code") %>' Width="120px" CssClass="form-control form-control-sm"></asp:TextBox>
                             </ItemTemplate>
                             <ControlStyle />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Description">
                             <ItemTemplate>
                              <asp:TextBox runat="server" ID="txtdescription" Text='<%#Eval("desc") %>' TabIndex="-1" Width="200px" CssClass="form-control form-control-sm"></asp:TextBox>
                             </ItemTemplate>
                             <ControlStyle />
                             <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                             <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="260px"/>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Amount" >
                             <ItemTemplate>
                              <asp:TextBox runat="server" ID="txtdebit" Text='<%#Eval("debit") %>'  Width="100px" CssClass="form-control form-control-sm" style="text-align:right"></asp:TextBox>
                             </ItemTemplate>
                             <FooterTemplate> 
                              <asp:Label ID="lbldebit" runat="server" style="text-align:right" Font-Size="10px"></asp:Label>
                             </FooterTemplate>
                             <ControlStyle />
                             <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                             <FooterStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                             <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="70px"/>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Credit" Visible="false">
                             <ItemTemplate>
                              <asp:TextBox runat="server" ID="txtcredit" Text='<%#Eval("credit") %>'  Width="70px" CssClass="form-control form-control-sm" style="text-align:right"></asp:TextBox>
                             </ItemTemplate>
                             <FooterTemplate> 
                              <asp:Label ID="lblcredit" runat="server" style="text-align:right;" Font-Size="10px" ></asp:Label>
                             </FooterTemplate>
                             <ControlStyle />
                             <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                             <FooterStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                             <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="70px"/>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Remarks">
                             <ItemTemplate>
                              <asp:TextBox runat="server" ID="txtremarks" Text='<%#Eval("remark") %>'  Width="90px" CssClass="form-control form-control-sm"></asp:TextBox>
                             </ItemTemplate>
                             <FooterTemplate> 
                              <asp:Label ID="lblbalance" runat="server"  style="text-align:center" Font-Size="10px" Visible="false"></asp:Label>
                             </FooterTemplate>
                             <ControlStyle />
                             <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                             <FooterStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                             <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="90px"/>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Cost Center">
                             <ItemTemplate>
                              <asp:DropDownList runat="server" ID="ddlcostcenter" AppendDataBoundItems="true"  Width="180px" CssClass="form-control form-control-sm">
                               <asp:ListItem Text="" Value=""></asp:ListItem>
                              </asp:DropDownList>
                             </ItemTemplate>
                             <ControlStyle />
                             <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                             <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="220px"/>
                            </asp:TemplateField>
                            
                            <asp:TemplateField Visible="false">
                             <ItemTemplate>
                              <asp:LinkButton ID="lnkInfo" runat="server" Text="Info" CssClass="lnk" OnClientClick="OpenNewWindow();" OnClick="lnkInfo_Click" ToolTip="Ledger Card Info" Width="20px" Font-Size="10px">
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
 
                   <div style="float:right; margin-top:10px;">
                    <asp:UpdatePanel runat="server" ID="updLinkButton" UpdateMode="Conditional">
                      <ContentTemplate>
                       <asp:LinkButton runat="server" ID="linkBtn" Text="Add More Rows" OnClick="linkBtn_Click" CssClass="btn btn-info"></asp:LinkButton>           
                    </ContentTemplate>  
                    </asp:UpdatePanel>
                   </div>

                        </div>
                    </div>
				</div>


				<asp:Button runat="server" ID="btnSave" class="btn btn-primary" Text="Save" 
                            OnClick="btnSave_Click" OnClientClick="if(!CheckForCostCenters()) {return false;} else {return true;}"
                            ValidationGroup="one">
				</asp:Button>
                <asp:Button runat="server" ID="btnCancel" class="btn btn-danger" Text="Clear" OnClick="btnCancel_Click" >
				</asp:Button>
            </div>
        </div>
    </div>
</div>
                   

    <div class="row">
    <div class="col-md-12">
        <div class="card card-shadow mb-4 ">
            <div class="card-body"> 
                <div class="form-group">
                    <div class="row">
                          <div class="col-lg-3 col-md-3 col-sm-3">
                                 <label>Divisions</label>
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged"
                                AppendDataBoundItems="True" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Division</asp:ListItem>
                            </asp:DropDownList>

                            
                            </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>From Date*</label>
                          <asp:TextBox runat="server" ID="txtFltCode" class="form-control" Visible="false"></asp:TextBox>
                            <asp:TextBox runat="server" ID="txtFrom" MaxLength="50" class="form-control"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtFromDate" runat="server" TargetControlID="txtFrom" Enabled="True"/>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>To Date*</label>
                            <asp:TextBox runat="server" ID="txtTo" MaxLength="50" class="form-control"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtToDate" runat="server" TargetControlID="txtTo" Enabled="True"/>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <label>Status*</label>
                            <asp:DropDownList ID="ddlFltrStatus" runat="server" AppendDataBoundItems="true" class="form-control">
                             </asp:DropDownList> 
                        </div>
                    </div>
				</div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                        <asp:Button runat="server" id="btnSearch" OnClick="btnSearch_Click" class="btn btn-info" Text="Search Voucher"></asp:Button>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">

                        </div>
                    </div>
				</div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                               <asp:GridView ID="grdVoucher" DataKeyNames="vrid" runat="server" OnSelectedIndexChanged="grdVoucher_SelectedIndexChanging"
                     AutoGenerateColumns="False" OnRowDataBound="grdVoucher_RowDataBound" AllowPaging="true"
                    Width="100%" PageSize="20" OnPageIndexChanging="grdVoucher_PageIndexChanging" CssClass="table table-responsive-sm"
                     EmptyDataText="No voucher found.">
                        <Columns>
                                <asp:BoundField DataField="vrid"  ItemStyle-CssClass="DisplayNone" ControlStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" Visible="false"/>
                                <asp:BoundField DataField="vr_no" HeaderText="Sr#" ItemStyle-Width="80" />
                                <asp:BoundField DataField="ref_no" HeaderText="Voucher#" ItemStyle-Width="70" />
                                 <asp:BoundField DataField="headsInvolved" HeaderText="Heads"  />

                                <asp:BoundField DataField="vr_dt" DataFormatString= "{0:dd-MMM-yyyy}" htmlencode="false" HeaderText="Date">
                                    <ItemStyle Wrap="false" Width="80" />
                                </asp:BoundField>
                                <%--<asp:BoundField DataField="source" HeaderText="Source" ItemStyle-Width="70" />--%>
                                <asp:BoundField DataField="vr_nrtn" HeaderText="Narration"/>
                                <asp:BoundField DataField="status" HeaderText="Status" ItemStyle-Width="80" />
                                
   
                            <%--<asp:TemplateField HeaderText="Remarks" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10">
                                <ItemTemplate>
                                    <asp:Image ID="imgRemarks" runat="server" ImageUrl="~/images/incomingmsg.jpg"/>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Print" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                     <asp:Label ID="lblstatus" runat="server"  Visible="false" Text='<%# Bind("status") %>'>'></asp:Label>
                                    <asp:LinkButton ID="btnprint" runat="server" OnClick="lnkPrint_Click" Text="Print"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowSelectButton="true" ButtonType="Link" ItemStyle-Width="70" />
                        </Columns>   
                 </asp:GridView>
                        </div>
                    </div>
				</div>
                </div>
            </div>
        </div>
        </div>
    






</asp:Content>


