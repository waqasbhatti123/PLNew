<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="ProvidentFund.aspx.cs" Inherits="RMS.profile.ProvidentFund" Culture="auto" UICulture="auto"
    EnableEventValidation="true" %>


<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>
    <script>
        $(document).ready(function () {
            $(".year").change(function () {
                
                var year = $(".year").val();
                var em = $(".employee").val();
                $.ajax({
                    
                url: "ProvidentFund.aspx/BindGridJquery",
                data: JSON.stringify({ id: year,emp: em }),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        $(".closing").empty();
                        $(".valueget").empty();
                        $(".opening").empty();
                        $(".valuelable").empty();
                        var value = data.d;

                        var vl = "";
                        vl += '<div class="row ser">';
                        vl += '<div class="col-lg-1 col-md-1 col-sm-1">';
                        vl += '<label>Serial</label>';
                        vl += '</div>';
                        vl += '<div class="col-lg-2 col-md-2 col-sm-2">';
                        vl += '<label>Month</label>';
                        vl += '</div>';
                        vl += '<div class="col-lg-2 col-md-2 col-sm-2">';
                        vl += '<label>Value</label>';
                        vl += '</div>';
                        vl += '</div>';

                        $(".valuelable").html(vl);

                        var cb = "";
                        cb += '<div class="row">';
                        cb += '<div class="col-lg-4 col-md-4 col-sm-4">';
                        cb += '<label>Opening Balance:</label>';
                        cb += '<input type="text" class="form-control" disabled="true" value='+ value[0].OpenBlnc + '>';
                        cb += '</div>';
                        cb += '</div>';

                        $(".opening").html(cb);
                         var html = '';
                        
                        $.each(data.d, function (index, item) {
                            html += '<br/>';
                        html += '<div class="row">'
                        html += '<div class="col-lg-1 col-md-1 col-sm-1">'
                        html += '<input type="text" class="form-control" disabled="true" value= ' + (index + 1) + '>'
                        html +='</div>';
                        html += '<div class="col-lg-2 col-md-2 col-sm-2">'
                        html += '<input type="text" class="form-control" disabled="true" value= ' + item.Month + '>'
                        html += '</div>';
                        html += '<div class="col-lg-2 col-md-2 col-sm-2">'
                        html += '<input type="text" class="form-control va" disabled="true" value= ' + item.value + '>'
                        html +='</div>';
                        html +='</div>';
                        });
                        $(".valueget").html(html);




                        debugger
                        var LastIndex = value.length - 1;
                        var First = value[LastIndex].closeBlnc;
                        var Closing = "";
                        Closing += '<div class="row">';
                        Closing += '<div class="col-lg-4 col-md-4 col-sm-4">';
                        Closing += '<label> Closing Balance</label>';
                        Closing += '<input type="text" class="form-control" disabled="true" value= ' + First + '>';
                        Closing += '</div>';
                        Closing += '</div>';
                        

                        $(".closing").html(Closing);



                    
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });


            });








           $("#myGrid").hide();
            // getProvident();
            $(".employee").change(function () {
                var emp = $(".employee").val();
                $.ajax({
                    url: "ProvidentFund.aspx/BindGrid",
                    data: JSON.stringify({ empId: emp }),
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        $('.ser').hide();
                        $(".closing").empty();
                        $(".valueget").empty();
                        $(".opening").empty();
                        $('.year').val(0);
                        if (data.d > 0) {

                            $(".openingBalance").hide();
                            $("#myGrid").show();
                        }
                        else
                        {
                            $(".openingBalance").show();
                            $("#myGrid").hide();
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            });
        });

    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:ValidationSummary ID="ValidationSummary1" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                              ValidationGroup="main"/>
                            <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Employees*</label>
                            <asp:DropDownList ID="ddlEmployeeSearch" CssClass="form-control employee" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Value="0">Select Employee</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Employee is required"
                                ControlToValidate="ddlEmployeeSearch" InitialValue="0" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                        </div>
                        
                    </div>
                    &nbsp;
                    <div class="row openingBalance">
                            <div class="col-lg-4 col-md-4 col-sm-4  ">
                                <div >
                                <label>Opening Balance</label>
                                <asp:TextBox ID="txtOpening" CssClass="RequiredField form-control " runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtOpening"
                         ErrorMessage="Please enter Opening Balance" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                                    </div>
                                <br />
                                <asp:Button ID="Button1"  runat="server" Text="Save" OnClick="Save_click" CssClass="btn btn-primary"/>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="margin-top:30px">
                                
                            </div>
                    </div>
                    &nbsp;
                    <div id="myGrid">
                    <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                            <label>Years*</label>
                            <asp:DropDownList ID="ddlYear" CssClass="form-control year" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Value="0">Select Year</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Year is Required"
                                ControlToValidate="ddlYear" InitialValue="0" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                         </div>
                         
                <%--<table id="myTable" class="table table-responsive table-sm">
                    <thead>
                        <tr>
                            <th>Serial
                        </th>
                        <th>Account
                        </th>
                        <th>Income
                        </th>
                            <th>
                                Grant
                            </th>
                            <th>
                                Perposed Budget
                            </th>
                        </tr>
                    </thead>
                    <tbody id="myBody">
                    </tbody>
                </table>--%>
            </div>
                        <br />
                         <div class="opening">
                         </div>
                        <br />
                          <div class="valuelable">
                              
                          </div>
                          <div class="valueget">

                          </div>
                            <br />
                            <div class="closing">

                          </div>
                         </div>
                </div>
            </div>
        </div>
    </div>
   
</asp:Content>
