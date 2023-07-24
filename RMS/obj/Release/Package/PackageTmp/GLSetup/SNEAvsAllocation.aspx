﻿<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master"
    AutoEventWireup="true" CodeBehind="SNEAvsAllocation.aspx.cs" Inherits="RMS.GLSetup.SNEAvsAllocation"
    Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>
    <script>

        function PageReload() {
            location.reload(true);
        }

        $(function () {

            $(".click").click(function () {
                window.open("SNEvdAllocationReport.aspx");
                location.reload();
            })

            <%--var brID = '<%=Session["BranchID"].ToString() %>';
            $('.searchbranchchange').change(function () {
                debugger
                var Br_ID = $('.searchbranchchange').val();
                
                getBudget(Br_ID);
            });
            $('.searchbranchchange').val(brID);--%>
            var brID = $(".searchbranchchange").val();
            getBudget(brID);

             $('#btnSave').click(function () {
                debugger
                var accounts = [];

                var branchId = $('.searchbranchchange').val();
                $('.clAccount').each(function (index, item) {
                    var accountId = $(this).attr('accountId');
                    var $tr = $(this).closest('tr');
                    var grantid = $tr.find('.grant').val();
                    var push = accounts.push({ Account: accountId, Grant: grantid, br_id: branchId});
                    if (push) {
                    var $tr = $(this).closest('tr');
                    }
                })
                            

            $.ajax({
                url: "SNEAvsAllocation.aspx/SaveBudget",
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
                url: "SNEAvsAllocation.aspx/BindGridJquery",
                data: JSON.stringify({ brID:br_ID }),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger
                    var html = '';

                    $.each(data.d, function (index, item) {

                        html += '<tr><input type="hidden" class="clAccount" accountId="' + item.Account + '"/>';
                        //html += '<td>' + (index + 1) + '</td>';
                        html += '<td>' + item.GlDesc + '</td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right perposed" disabled="true"  id="txtPerposed" value="' + item.Aid + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right grant"  id="txtGrant"  value="' + item.ApGrant + '"/></td>';
                        html += '<td><input type="number" class="form-control form-control-sm text-right var" id="txtVaraiance" disabled="true" value="' + item.Variance + '"/></td>';
                        
                        html += '</tr>';
                        var perposed = $(".perposed").val();
                    
                        
                    });

                    $('#myBody').html(html);
                    onloadTotalGrants();
                    
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
             });

        }
        
        function onloadTotalGrants() {
            $('.clAccount').each(function (index, item) {
                var accountId = $(this).attr('accountId');
                var $tr = $(this).closest('tr');

                var q1 = $tr.find('.perposed').val();
                if (q1 == "" || q1 == null) {
                    q1 = 0;
                }
                var q2 = $tr.find('.grant').val();
                if (q2 == "" || q2 == null) {
                    q2 = 0;
                }
                var varieance = parseFloat(q1) - parseFloat(q2);
                $tr.find('.var').val(varieance);
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4">
                <div class="card-body">
               <div class="row">
                <div class="col-md-4">
                    <label>Divisions:</label>
                    <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" 
                        AppendDataBoundItems="True">
                        <asp:ListItem Value="0" Selected="True">Punjab Arts Council</asp:ListItem>
                    </asp:DropDownList>
                </div>
                   <div class="col-md-4">
                    <br />
                   
                    <input type="button" class="btn btn-lg btn-primary"  id="btnSave" value="Save" />
                </div>
                <div class="col-lg-2 col-md-2 col-sm-2 offset-lg-2" style="margin-top:20px">
                    <button class="btn btn-primary click">Report</button>
                    <%--<asp:Button Text="Report" CssClass="btn btn-primary click" OnClick="" runat="server" />--%>
                </div>
            </div>
              <div class="row">
                  <div class="col-lg-12 col-md-12 col-sm-12">
                      <div id="myGrid">
                <table id="myTable" class="table table-responsive table-sm">
                    <thead>
                        <tr>
                           <%-- <th>Serial
                        </th>--%>
                        <th>Account
                       
                        </th>
                            <th>
                                Demanded Through SNE
                            </th>
                            <th>
                                Allocation By F.D
                            </th>
                            <th>
                                Variance
                            </th>
                        </tr>
                    </thead>
                    <tbody id="myBody">
                    </tbody>
                </table>
            </div>
                  </div>
              </div>  
              </div>
            </div>
        </div>
    </div>
</asp:Content>
