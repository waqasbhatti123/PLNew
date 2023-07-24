<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="DepartmentMgt.aspx.cs" Inherits="RMS.Setup.DepartmentMgt"%>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            $('#<%= txtStaffSal.ClientID %>').click(function(event) {
                this.select();
            });
            $('#<%= txtSalPayable.ClientID %>').click(function(event) {
                this.select();
            });
            $('#<%= txtITax.ClientID %>').click(function(event) {
                this.select();
            });
            $('#<%= txtEOBI.ClientID %>').click(function(event) {
                this.select();
            });
            $('#<%= txtLoanAdv.ClientID %>').click(function(event) {
                this.select();
            });
            $('#<%= txtMisc.ClientID %>').click(function(event) {
                this.select();
            });
            $('#<%= txtOtrDed.ClientID %>').click(function (event) {
                this.select();
            });
            $('#<%= txtMiscDed.ClientID %>').click(function (event) {
                this.select();
            });


            $('#<%= txtStaffSal.ClientID %>').autocomplete({

                source: function(request, response) {
                    $.ajax({
                        url: "DepartmentMgt.aspx/GetBranch",
                        data: "{ 'code': '" + request.term + "' }",
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
                    //alert(ui.item.id);
                    $('#<%= hdnStaffSal.ClientID %>').val(ui.item.id);
                },

                minLength: 1
            });

            $('#<%= txtSalPayable.ClientID %>').autocomplete({

                source: function(request, response) {
                    $.ajax({
                        url: "DepartmentMgt.aspx/GetBranch",
                        data: "{ 'code': '" + request.term + "' }",
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
                    //alert(ui.item.id);
                    $('#<%= hdnSalPayable.ClientID %>').val(ui.item.id);
                },

                minLength: 1
            });

            $('#<%= txtITax.ClientID %>').autocomplete({

                source: function(request, response) {
                    $.ajax({
                        url: "DepartmentMgt.aspx/GetBranch",
                        data: "{ 'code': '" + request.term + "' }",
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
                    //alert(ui.item.id);
                    $('#<%= hdnITax.ClientID %>').val(ui.item.id);
                },

                minLength: 1
            });

            $('#<%= txtEOBI.ClientID %>').autocomplete({

                source: function(request, response) {
                    $.ajax({
                        url: "DepartmentMgt.aspx/GetBranch",
                        data: "{ 'code': '" + request.term + "' }",
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
                    //alert(ui.item.id);
                    $('#<%= hdnEOBI.ClientID %>').val(ui.item.id);
                },

                minLength: 1
            });

            $('#<%= txtLoanAdv.ClientID %>').autocomplete({

                source: function(request, response) {
                    $.ajax({
                        url: "DepartmentMgt.aspx/GetBranch",
                        data: "{ 'code': '" + request.term + "' }",
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
                    //alert(ui.item.id);
                    $('#<%= hdnLoanAdv.ClientID %>').val(ui.item.id);
                },

                minLength: 1
            });

//            $('#<%= txtMisc.ClientID %>').autocomplete({

//                source: function(request, response) {
//                    $.ajax({
//                        url: "DepartmentMgt.aspx/GetBranch",
//                        data: "{ 'code': '" + request.term + "' }",
//                        dataType: "json",
//                        type: "POST",
//                        contentType: "application/json; charset=utf-8",

//                        dataFilter: function(data) { return data; },
//                        success: function(data) {
//                            response($.map(data.d, function(item) {
//                                return {
//                                    value: item.gl_cd + ' - ' + item.gl_dsc,
//                                    result: item.STN,
//                                    id: item.gl_cd
//                                }
//                            }))
//                        },
//                        error: function(XMLHttpRequest, textStatus, errorThrown) {
//                            alert(textStatus);
//                        }
//                    });
//                },
//                select: function(e, ui) {
//                    //alert(ui.item.id);
//                    $('#<%= hdnMisc.ClientID %>').val(ui.item.id);
//                },

//                minLength: 1
//            });




            $('#<%= txtMisc.ClientID %>').autocomplete({

                source: function(request, response) {
                    $.ajax({
                    url: "DepartmentMgt.aspx/GetControlAccount",
                        data: "{ 'code': '" + request.term + "' }",
                        dataType: "json",
                        type: "POST",
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
                    //alert(ui.item.id);
                $('#<%= hdnMisc.ClientID %>').val(ui.item.id);
                },

                minLength: 1
            });





            $('#<%= txtOtrDed.ClientID %>').autocomplete({

                source: function (request, response) {
                    $.ajax({
                        url: "DepartmentMgt.aspx/GetControlAccount",
                        data: "{ 'code': '" + request.term + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",

                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    value: item.gl_cd + ' - ' + item.gl_dsc,
                                    result: item.gl_cd + ' - ' + item.gl_dsc,
                                    id: item.gl_cd
                                }
                            }))
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus);
                        }
                    });
                },
                select: function (e, ui) {
                    //alert(ui.item.id);
                    $('#<%= hdnOtrDed.ClientID %>').val(ui.item.id);
                },

                minLength: 1
            });



            $('#<%= txtMiscDed.ClientID %>').autocomplete({

                source: function (request, response) {
                    $.ajax({
                        url: "DepartmentMgt.aspx/GetControlAccount",
                        data: "{ 'code': '" + request.term + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",

                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    value: item.gl_cd + ' - ' + item.gl_dsc,
                                    result: item.gl_cd + ' - ' + item.gl_dsc,
                                    id: item.gl_cd
                                }
                            }))
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus);
                        }
                    });
                },
                select: function (e, ui) {
                    //alert(ui.item.id);
                    $('#<%= hdnMiscDed.ClientID %>').val(ui.item.id);
                },

                minLength: 1
            });


        }
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="card card-shadow mb-4">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
                               ValidationGroup="main"/>
                             <uc1:Messages ID="ucMessage" runat="server" />
                        </div>
                    </div>
                    &nbsp;
                    <asp:Panel runat="server" ID="pnlMain">
                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-sm-8">
                            <asp:GridView ID="grdDepartments" CssClass="table table-responsive-sm table-bordered" runat="server" DataKeyNames="CodeID" OnSelectedIndexChanged="grdDepartments_SelectedIndexChanged" 
                    AutoGenerateColumns="False" AllowPaging="True" Width="98%" PageSize="20" OnPageIndexChanging="grdDepartments_PageIndexChanging" OnRowDataBound="grdDepartments_RowDataBound">
                    <HeaderStyle CssClass ="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                    <Columns>
                        
                        <asp:BoundField DataField="CodeDesc" HeaderText="Section Name"/>
                        <asp:BoundField DataField="Enabled" HeaderText="Status"/>
                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk">
                            <ItemStyle />
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label ID="lblName" runat="server"  Font-Size="small" Text="Section Name*"></asp:Label>
                            <asp:TextBox ID="txtDepartment" CssClass="RequiredField form-control" runat="server" MaxLength="100"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDepartment"
                        ErrorMessage="Please enter department name" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                            <br />
                            <asp:Panel ID="pnlFields" runat="server">
                                <asp:Label ID="lblStaffSal" runat="server" Font-Size="small" Text="Staff Salaries A/C*"></asp:Label>
                                <asp:TextBox ID="txtStaffSal" CssClass="form-control" runat="server" Width="300px" style="font-size:12px;"></asp:TextBox>
                                <asp:HiddenField ID="hdnStaffSal" runat="server" />
                                <br />
                                <asp:Label ID="lblSalPayable" runat="server" Font-Size="Small" Text="Salary Payable A/C*"></asp:Label>
                                <asp:TextBox ID="txtSalPayable" CssClass="form-control" runat="server" Width="300px" style="font-size:12px;"></asp:TextBox>
                                <asp:HiddenField ID="hdnSalPayable" runat="server" />
                                <br />
                                 <asp:Label ID="lblITax" runat="server" Font-Size="Small" Text="I/Tax A/C*"></asp:Label>
                                <asp:TextBox ID="txtITax" runat="server" CssClass="form-control" Width="300px" style="font-size:12px;"></asp:TextBox>
                                <asp:HiddenField ID="hdnITax" runat="server" />
                                <br />
                                <asp:Label ID="lblEOBI" runat="server" Font-Size="Small" Text="EOBI Payble A/C*"></asp:Label>
                                <asp:TextBox ID="txtEOBI" CssClass="form-control" runat="server" Width="300px" style="font-size:12px;"></asp:TextBox>
                                <asp:HiddenField ID="hdnEOBI" runat="server" />
                                <br />
                                <asp:Label ID="lblLoanAdv" runat="server" Font-Size="Small" Text="Loan/Advances A/C*"></asp:Label>
                                <asp:TextBox ID="txtLoanAdv" CssClass="form-control" runat="server" Width="300px" style="font-size:12px;"></asp:TextBox>
                               <asp:HiddenField ID="hdnLoanAdv" runat="server" />
                                <br />
                                <asp:Label ID="lblMisc" runat="server" Font-Size="Small" Text="Ctrl Employee A/C*"></asp:Label>
                                <asp:TextBox ID="txtMisc" CssClass="form-control" runat="server" Width="300px" style="font-size:12px;"></asp:TextBox>
                                <asp:HiddenField ID="hdnMisc" runat="server" />
                                <br />
                                <asp:Label ID="Label1" runat="server" Font-Size="Small" Text="Ctrl Other Ded. A/C*"></asp:Label>
                                 <asp:TextBox ID="txtOtrDed" CssClass="form-control" runat="server" Width="300px" style="font-size:12px;"></asp:TextBox>
                                 <asp:HiddenField ID="hdnOtrDed" runat="server" />
                                <br />
                                <asp:Label ID="Label2" runat="server" Font-Size="Small" Text="Ctrl Misc. Ded. A/C*"></asp:Label>
                                <asp:TextBox ID="txtMiscDed" CssClass="form-control" runat="server" Width="300px" style="font-size:12px;"></asp:TextBox>
                                <asp:HiddenField ID="hdnMiscDed" runat="server" />
                                <br />
                            </asp:Panel>
                            <asp:Label ID="lblEnable" runat="server" Font-Size="Small" Text="Status*"></asp:Label>
                            <asp:RadioButtonList ID="rblStatus" runat="server" 
                        RepeatDirection="Horizontal">
                        <asp:ListItem Value="1" Selected="True">Enable</asp:ListItem>
                        <asp:ListItem Value="0">Disable</asp:ListItem>
                    </asp:RadioButtonList>
                            <br />
                            <uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
            <!--img src="images/btn_new.jpg" width="60" height="20" /> <img src="images/btn_edit.jpg" width="60" height="20" /> <img src="images/btn_delete.jpg" alt="" width="60" height="20" /> <img src="images/btn_save.jpg" width="60" height="20" /> <img src="images/btn_cancel.jpg" width="60" height="20" /-->
                        </div>
                    </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    
    
    
    
    <
        
</asp:Content>
