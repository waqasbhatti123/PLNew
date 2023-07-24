<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"
    AutoEventWireup="true" CodeBehind="DistGrantAidCoucil.aspx.cs" Inherits="RMS.GLSetup.DistGrantAidCoucil"
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
           
            var brID = '<%=Session["BranchID"].ToString() %>';
            if (brID == "1") {
                $('#btnSave').prop('disabled', false);
                $(".click").show();
            }
            else {
                $('#btnSave').prop('disabled', true);
                $(".click").hide();
            }
            getBudget(brID);
            $('.searchbranchchange').val(brID);

            $('.searchbranchchange').change(function () {

                var Br_ID = $('.searchbranchchange').val();
                getBudget(Br_ID);

            });


            $(".click").click(function () {
                window.open("DistGrntAmounAllCouReport.aspx");
                location.reload(true);
            });


            $('table').on('keyup', '.grant', function () {
                var $tr = $(this).closest('tr');
                var grantVal = $tr.find('.grant').val();

                if (grantVal == "" || grantVal == null) {
                    grantVal = 0;
                }

                var amount = parseFloat(grantVal / 2);

                $tr.find('.Half').val(amount);
                getGrantTotal();
                getHalfTotal();
            });

            $('table').on('keyup', '.grant', function () {
                var $tr = $(this).closest('tr');
                var grantVal1 = $tr.find('.Half').val();

                if (grantVal1 == "" || grantVal1 == null) {
                    grantVal1 = 0;
                }

                var amount1 = parseFloat(grantVal1 / 2);

                $tr.find('.Quater').val(amount1);
                getQuaterTotal();

            });

            $('table').on('change', '.grant', function () {
                getGrantTotal();
            });

            $('table').on('change', '.Half', function () {
                getHalfTotal();
            });

            $('table').on('change', '.Quater', function () {
                getQuaterTotal();
            });

            $('#btnSave').click(function () {
                debugger
                var accounts = [];

                var branchId = $('.searchbranchchange').val();
                $('.clAccount').each(function (index, item) {
                    var accountId = $(this).attr('accountId');
                    var $tr = $(this).closest('tr');
                    var grantid = $tr.find('.grant').val();
                    var incomeid = $tr.find('.income').val();
                    var Aidd = $tr.find(".aid").val();
                    var push = accounts.push({ Account: accountId, Grant: grantid, br_id: branchId, Income: incomeid });
                    if (push) {
                    var $tr = $(this).closest('tr');
                    }
                })
                            

            $.ajax({
                url: "DistGrantAidCoucil.aspx/SaveBudget",
                data: JSON.stringify({ budget: accounts }),
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


            
        
        });

          

        function getBudget(br_ID) {
            $.ajax({
                url: "DistGrantAidCoucil.aspx/bindGridJquery",
                data: JSON.stringify({ brID:br_ID }),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    var html = '';

                    $.each(data.d, function (index, item) {
                       
                        html += '<tr><input type="hidden" class="clAccount" accountId="' + item.Account + '"/>';
                        html += '<td>' + (index + 1) + '</td>';
                        html += '<td>' + item.GlDesc + '</td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right grant" id="txtGrant" value="' + item.Grant + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right income" hidden="true" id="txtIncome" value="' + item.Income + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right Half" readonly="readonly"   id="txtHalf" value=""/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right Quater" readonly="readonly"  id="txtQuater" value=""/></td>';
                        html += '</tr>';
                        
                    });

                    $('#myBody').html(html);
                    onloadTotalGrants();
                    onloadTotalHalf();
                    getGrantTotal();
                    getHalfTotal();
                    getQuaterTotal();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });

          
        }

        
        //function getGrandTotal() {

        //        var val1 = 0;
        //                $("#myBody [id^=txtGrant]").each(function () {
                            
        //                     if ($(this).val() == null || $(this).val() == "") {
        //                    $(this).val(0);
        //                }

        //                //Add values multiple Values of a single column
        //                val1 += parseFloat($(this).val());

        //                if ($(this).val() == 0) {
        //                    $(this).val("");
        //                }
        //        });
        //        $(".grandTotalVal").val(val1);
                        
        //}
        function getGrantTotal() {
            debugger
                var val1 = 0;
                        $("#myBody [id^=txtGrant]").each(function () {
                            
                             if ($(this).val() == null || $(this).val() == "") {
                            $(this).val(0);
                        }

                            //Add values multiple Values of a single column
                            val1 += parseFloat($(this).val());

                        if ($(this).val() == 0) {
                            $(this).val("");
                        }
                });
                $(".grandTotalVal").val(val1);
                        
        }

         function getHalfTotal() {

                var val2 = 0;
                        $("#myBody [id^=txtHalf]").each(function () {
                            
                             if ($(this).val() == null || $(this).val() == "") {
                            $(this).val(0);
                        }

                            //Add values multiple Values of a single column
                            val2 += parseFloat($(this).val());

                        if ($(this).val() == 0) {
                            $(this).val("");
                        }
                });
                $(".grandHalfVal").val(val2);
                        
        }

        function getQuaterTotal() {

                var val3 = 0;
                        $("#myBody [id^=txtQuater]").each(function () {
                            
                             if ($(this).val() == null || $(this).val() == "") {
                            $(this).val(0);
                        }

                            //Add values multiple Values of a single column
                            val3 += parseFloat($(this).val());

                        if ($(this).val() == 0) {
                            $(this).val("");
                        }
                });
                $(".grandQuartVal").val(val3);
                        
        }
        function onloadTotalGrants() {

            $('.clAccount').each(function (index, item) {

                var accountId = $(this).attr('accountId');
                var $tr = $(this).closest('tr');

                //var q1 = $tr.find('.Half').val();
                //if (q1 == "" || q1 == null) {
                //    q1 = 0;
                //}
                var q2 = $tr.find('.grant').val();
                if (q2 == "" || q2 == null) {
                    q2 = 0;
                }
                var varieance = parseInt(q2 / 2); 
                $tr.find('.Half').val(varieance);
            });
        }

        function onloadTotalHalf() {

            $('.clAccount').each(function (index, item) {

                var accountId = $(this).attr('accountId');
                var $tr = $(this).closest('tr');

                //var q1 = $tr.find('.Half').val();
                //if (q1 == "" || q1 == null) {
                //    q1 = 0;
                //}
                var q2 = $tr.find('.Half').val();
                if (q2 == "" || q2 == null) {
                    q2 = 0;
                }
                var varieance = parseInt(q2 / 2); 
                $tr.find('.Quater').val(varieance);
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
                <div class="col-md-4">
                    <br />
                    <input type="button" class="btn btn-lg btn-primary"  id="btnSave" value="Save Budget" />
                </div>
                <div class="col-lg-2 col-md-2 col-sm-2 offset-lg-2" style="margin-top:25px">
                    <asp:Button Text="Report" CssClass="btn btn-primary click" runat="server" />
                </div>
            </div>

           


            <div class="succesmessage">

            </div>
            <div id="myGrid">
                <table id="myTable" class="table table-responsive table-sm">
                    <thead>
                        <tr>
                            <th>Serial
                        </th>
                        <th>Account
                        </th>
                            
                            <th>
                               Total Yearly Grant
                            </th>
                            <th></th>
                            <th>
                               Half Yearly Grant
                            </th>
                            <th>
                               Quarterly Grant
                            </th>
                        </tr>
                    </thead>
                    <tbody id="myBody">
                    </tbody>
                </table>
            </div>


            <div class="row grandTotal">
                <div class="col-md-2 offset-md-5">
                    <label>Total Yealy Grant</label>
                    <input type="text" class=" text-right form-control grandTotalVal" readonly="readonly"/>
                </div>
                <div class="col-md-2 offset-md-1">
                    <label>Half Yealy Grant</label>
                    <input type="text" class=" text-right form-control grandHalfVal" readonly="readonly"/>
                </div>
                <div class="col-md-2">
                    <label>Quarterly Grant</label>
                    <input type="text" class=" text-right form-control grandQuartVal" readonly="readonly"/>
                </div>
                
                                    
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
