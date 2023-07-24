<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"
    AutoEventWireup="true" CodeBehind="ExcessSsurrendertwo.aspx.cs" Inherits="RMS.GLSetup.ExcessSsurrendertwo"
    Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>
    <script>
        $(document).ready(function () {
            $(".click").click(function () {
                window.open("BudgetReport.aspx");
                location.reload();
            })
           
            var brID = '<%=Session["BranchID"].ToString() %>';
            //if (brID == "1") {
            //   $('#btnSave').prop('disabled', false);
            //}
            //else {
            //     $('#btnSave').prop('disabled', true);
                
            //}
            getBudget(brID);
            $('.searchbranchchange').val(brID);

            $('.searchbranchchange').change(function () {

                var Br_ID = $('.searchbranchchange').val();
                getBudget(Br_ID);

            });
           
            $('#btnSave').click(function () {
                var accounts = [];
                debugger
                var branchId = $('.searchbranchchange').val();
                $('.clAccount').each(function (index, item) {
                    var accountId = $(this).attr('accountId');
                    var $tr = $(this).closest('tr');
                    var original = $tr.find('.txtGrant').val();
                    var supply = $tr.find('.txtsupl').val();
                    var approamount = $tr.find('.txtapproamount').val();
                    var modified = $tr.find('.modified').val();
                    var current = $tr.find('.txtcurrentfinancia').val();
                    var previous = $tr.find('.txtprevious').val();
                    var total = $tr.find('.txttotal').val();
                    var propertion = $tr.find('.txtpropertion').val();
                    var anticipated = $tr.find('.txtanticipated').val();
                    var excess = $tr.find('.txtexcess').val();
                    var surrender = $tr.find('.txtxurrender').val();
                    var code = $tr.find('.glcode').val();
                    var push = accounts.push({
                        HeadCode: code, HeadOfAccount: accountId, OriBudget: original, SupBudget: supply, AppAmoun: approamount, ModifiedBudget: modified,
                        CurrentYearExp: current, PreYearExp:previous, TotalActual:total, ProposedReapproperaition:propertion, AnticipatedRevisedExp:anticipated, Excess:excess,
                        Surrender:surrender, brId: branchId
                    });
                    
                })
                            

            $.ajax({
                url: "ExcessSsurrendertwo.aspx/SaveBudget",
                data: JSON.stringify({ excess: accounts }),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    window.alert(data.d);
                    
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
            });

            $('table').on('keyup', '.txtsupl', function () {
                debugger
                    var $tr = $(this).closest('tr');
                
                var itemRate = $tr.find('.txtGrant').val();
                if (itemRate == "" || itemRate == null) {
                    itemRate = 0;
                }
                

                var itemQuantity = $tr.find('.txtsupl').val();
                if (itemQuantity == "" || itemQuantity == null) {
                    itemQuantity = 0;
                }

                $txtsup = $tr.find(".txtapproamount").val();
                if ($txtsup == "" || $txtsup == null) {
                    $txtsup = 0;
                }

                var amount = parseInt(itemRate) + parseInt(itemQuantity) + parseInt($txtsup);

                $tr.find('.modified').val(amount);

                 //getGrandTotal();

            });

            $('table').on('keyup', '.txtapproamount', function () {
                var $tr = $(this).closest('tr');

                $Grant = $tr.find(".txtGrant").val();
                if ($Grant == "" || $Grant == null) {
                    $Grant = 0;
                }

                $txtsup = $tr.find(".txtsupl").val();
                if ($txtsup == "" || $txtsup == null) {
                    $txtsup = 0;
                }

                $txtapp = $tr.find(".txtapproamount").val();
                if ($txtapp == "" || $txtapp == null) {
                    $txtapp = 0;
                }

                var amount = parseInt($Grant) + parseInt($txtsup) + parseInt($txtapp);
                $tr.find('.modified').val(amount);

            });

            $('table').on('keyup', '.txtcurrentfinancia', function () {

                $tr = $(this).closest('tr');
                var $current = $tr.find('.txtcurrentfinancia').val();
                if ($current == "" || $current == null) {
                    $current = 0;
                }

                var $pre = $tr.find('.txtprevious').val();
                if ($pre == "" || $pre == null) {
                    $pre = 0;
                }

                var amount = parseInt($current) + parseInt($pre);
                $tr.find('.txttotal').val(amount);

            });

            $('table').on('keyup', '.txtprevious', function () {

                $tr = $(this).closest('tr');
                var $current = $tr.find('.txtcurrentfinancia').val();
                if ($current == "" || $current == null) {
                    $current = 0;
                }

                var $pre = $tr.find('.txtprevious').val();
                if ($pre == "" || $pre == null) {
                    $pre = 0;
                }

                var amount = parseInt($current) + parseInt($pre);
                $tr.find('.txttotal').val(amount);

            });
         
        });

          

        function getBudget(br_ID) {
            $.ajax({
                url: "ExcessSsurrendertwo.aspx/bindGridJquery",
                data: JSON.stringify({ brID:br_ID }),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    var html = '';

                    $.each(data.d, function (index, item) {
                        html += '<tr><input type="hidden" class="clAccount" accountId="' + item.Accounts + '"/>';
                        html += '<td class="glcode">' + item.code + '</td>';
                        html += '<td>' + item.Gldsc + '</td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtGrant" id="txtGrant" Disabled="true" style="width:120px" value="' + item.AppBudget + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtsupl" id="txtsupl" style="width:120px" value="' + item.SupplementryBudget + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtapproamount" id="txtapproamount" style="width:120px" value="' + item.ApprovedAmountReapp + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right modified" Disabled="true" id="modified" style="width:120px" value="' + item.ModifiedEstimateBudget + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtcurrentfinancia" id="txtcurrentfinancia" style="width:120px" value="' + item.eightmonthcurrentfinancialyear + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtprevious" id="txtprevious" style="width:120px" value="' + item.fourmonthpreviousfinancialyer + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txttotal" Disabled="true" id="txttotal" style="width:120px" value="' + item.TotalActual + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtpropertion" id="txtpropertion" style="width:120px" value="' + item.ProposedReappropriationbyBudgetAD + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtanticipated" id="txtanticipated" style="width:120px" value="' + item.AntiRevisedExpcurrentyear + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtexcess" id="txtexcess" style="width:120px" value="' + item.Excess + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right txtxurrender" id="txtxurrender" style="width:120px" value="' + item.Surrender + '"/></td>';
                        
                    });

                    $('#myBody').html(html);
                    //getIncomeTotal();
                    //getGrantTotal();
                    //getGrandTotal();
                    
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        }


        function OnloadPlusValue() {
            $('.clAccount').each(function (index, item) {
                var accouontId = $(this).attr('accountId');
                $tr = $(this).closest('tr');
                var q1 = $tr.find('.txtcurrentfinancia').val();
                if (q1 == "" || q1 == null) {
                    q1 = 0;
                }
                var q2 = $tr.find('.txtprevious').val();
                if (q2 == "" || q2 == null) {
                    q2 = 0;
                }
                var varieance = parseFloat(q1) + parseFloat(q2);
                $total = $tr.find('.txttotal').val();
                if ($total == 0 || $total == null || $total == "") {
                    $tr.find('.txttotal').val(varieance);
                }
                
            });
        }
          
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />


    
    &nbsp;
    <br />


    <div class="card">
        <div class="card-body">
            <div class="row">
        <div class="col-sm-12">
            <uc1:Messages ID="ucMessage" runat="server" />
        </div>
           </div>

            <div class="row">
                <div class="col-md-4">
                    <label>Divisions:</label>
                    <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" 
                        AppendDataBoundItems="True">
                        <asp:ListItem Value="0">Punjab Arts Council</asp:ListItem>
                    </asp:DropDownList>

                </div>
                <div class="col-md-2">
                    <br />
                    <input type="button" class="btn btn-lg btn-primary"  id="btnSave" value="Save" />
                </div>
                <%--<div class="col-lg-2 col-md-2 col-sm-2 offset-lg-4" style="margin-top:20px">
                    <asp:Button Text="Report" CssClass="btn btn-primary click" runat="server" />
                </div>--%>
            </div>

           


            <div class="succesmessage">

            </div>
            <div id="myGrid">
                <table id="myTable" class="table table-responsive table-sm">
                    <thead>
                        <tr>
                            <th>Code
                        </th>
                        <th>Account
                        </th>
                        <th>Original Budget
                        </th>
                            <th>
                                Supplementry Budget
                            </th>
                            <th>
                                Approved Amount of Reappropriation
                            </th>
                            <th>
                                Modified Budget Estimates
                            </th>
                            <th>
                                Actual Expenditure for first 8 month of Current Financial Year
                            </th>
                            <th>
                                Actual Expenditure for last 4 month of Previous Financial Year
                            </th>
                            <th>
                                Total Actual
                            </th>
                            <th>Proposed Reaproperation within budget by A.D</th>
                            <th>Anticipated/Revised expenditure for the current year</th>
                            <th>Excess(+)</th>
                            <th>Surrenders(-)</th>
                        </tr>
                    </thead>
                    <tbody id="myBody">
                    </tbody>
                </table>
            </div>



            <div class="row grandTotal">

                <%--<div class="col-md-6">
                    <div class="row">
                        <div class="col-md-6"></div>
                        <div class="col-md-6"><b>Total:</b>

                        </div>
                    </div>
                </div>--%>
                <%--<div class="col-md-6">
                    <div class="row">
                        <div class="col-md-4">
                            <input type="text" class=" text-right form-control grandTotalValGrant" style="margin-left: -25px;" readonly="readonly"/>
                        </div>
                        <div class="col-md-4">
                            <input type="text" class=" text-right form-control grandTotalIncome" style="margin-left: -45px;" readonly="readonly" />
                        </div>
                        
                        <div class="col-md-4">
                            <input type="text" class=" text-right form-control grandTotalVal" style="margin-left: -25px;" readonly="readonly"/>

                        </div>
                    </div>
                </div>--%>


            </div>
        </div>
    </div>













<%--
    <div class="row">
        <div class="col-sm-12">
            <uc1:Messages ID="ucMessage" runat="server" />
        </div>
    </div>

    <div class="row">
        <div class="col-sm-12">
            <asp:GridView ID="grdBudget" CssClass="table table-responsive-sm table-bordered font-weight-light bg-white" runat="server" DataKeyNames="Account" AutoGenerateColumns="False">
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="txtcode" Text='<%#Eval("Account") %>' Width="120px" CssClass="form-control form-control-sm"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Account">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="txtDsc" Text='<%#Eval("GlDesc") %>' CssClass="form-control form-control-sm"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Income">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtIncome" Text='<%#Eval("Income") %>' Width="100px" CssClass="form-control form-control-sm income" Style="text-align: right" TextMode="Number"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Grant">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtGrant" Text='<%#Eval("Grant") %>' Width="100px" CssClass="form-control form-control-sm grant" Style="text-align: right" TextMode="Number"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Approved">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtAid" Text='<%#Eval("Aid") %>' Width="100px" CssClass="form-control form-control-sm" Style="text-align: right" TextMode="Number"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12">
            <asp:Button runat="server" ID="btnSave" class="btn btn-primary" Text="Save" OnClick="BtnSave_Click"></asp:Button>
        </div>
    </div>--%>
</asp:Content>
