<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="EmpMgtLoan.aspx.cs" Inherits="RMS.Setup.EmpMgtLoan" Culture="auto"
    UICulture="auto" EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/EmpSearchUC.ascx" TagName="EmpSearchUC" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.1.min.js"></script>
<script type="text/javascript">

    $(document).ready(function () {

        

        pageLoad();
        $(".amount").change(function () {
            var amount = $(".koibe .amount").val();
            if (amount == "" || amount == null) {
                    amount = 1;
            }
            var size = $(".size").val();
            if (size == "" || size == null) {
                    size = 1;
            }
            var instal = amount / size;

            $(".dedamount").val(parseInt(instal));
        });

        $(".size").change(function () {
            debugger
            var size = $(".size").val();
            if (size == "" || size == null) {
                    size = 1;
            }
            var amount = $(".amount").val();
            if (amount == "" || amount == null) {
                    amount = 1;
            }
            var instal = amount / size;

            $(".dedamount").val(parseInt(instal));
        });

        });
  
        //$('table').on('change', '.income', function () {
        //            var $tr = $(this).closest('tr');
                
        //        var itemRate = $tr.find('.grant').val();
        //        if (itemRate == "" || itemRate == null) {
        //            itemRate = 0;
        //        }
        //            var itemQuantity = $tr.find('.income').val();

        //        var amount = parseInt(itemQuantity) + parseInt(itemRate);

        //            $tr.find('.perposed').val(amount);

        //    });
    
    
    function pageLoad() {

//        $('#<%= ddlAppStatus.ClientID %>').val('P');
        $('#<%= divChq.ClientID %>').hide();

         if ($('#<%= ddlAppStatus.ClientID %>').val() == 'P') {
                $('#<%= divChq.ClientID %>').hide();
                ValidatorEnable($('[id*=RequiredFieldValidator8]')[0], false);
                ValidatorEnable($('[id*=RequiredFieldValidator9]')[0], false);
                ValidatorEnable($('[id*=RequiredFieldValidator10]')[0], false);

            }
            else {
                if ($('#<%= ddlAppStatus.ClientID %>').val() == 'A' && $('#<%= ddlPayType.ClientID %>').val() == 'Cheque') {

                    $('#<%= lblChqDet.ClientID %>').html('Cheque Detail');
                    
                    $('#<%= divChq.ClientID %>').show();
                    $('#<%= lblChqNo.ClientID %>').show();
                    $('#<%= lblChqDate.ClientID %>').show();
                    $('#<%= txtChqNo.ClientID %>').show();
                    $('#<%= txtChqDate.ClientID %>').show();
                    
                    ValidatorEnable($('[id*=RequiredFieldValidator8]')[0], true);
                    ValidatorEnable($('[id*=RequiredFieldValidator9]')[0], true);
                    ValidatorEnable($('[id*=RequiredFieldValidator10]')[0], true);
                }
                else {
                    $('#<%= lblChqDet.ClientID %>').html('Account Detail');

                    $('#<%= divChq.ClientID %>').show();
                    $('#<%= lblChqNo.ClientID %>').hide();
                    $('#<%= lblChqDate.ClientID %>').hide();
                    $('#<%= txtChqNo.ClientID %>').hide();
                    $('#<%= txtChqDate.ClientID %>').hide();

                    ValidatorEnable($('[id*=RequiredFieldValidator8]')[0], true);
                    ValidatorEnable($('[id*=RequiredFieldValidator9]')[0], false);
                    ValidatorEnable($('[id*=RequiredFieldValidator10]')[0], false);
                }
            }

        ValidatorEnable($('[id*=RequiredFieldValidator8]')[0], false);
        ValidatorEnable($('[id*=RequiredFieldValidator9]')[0], false);
        ValidatorEnable($('[id*=RequiredFieldValidator10]')[0], false);

        $('#<%= ddlAppStatus.ClientID %>').change(function(event) {
            if ($('#<%= ddlAppStatus.ClientID %>').val() == 'P') {
                $('#<%= divChq.ClientID %>').hide();
                ValidatorEnable($('[id*=RequiredFieldValidator8]')[0], false);
                ValidatorEnable($('[id*=RequiredFieldValidator9]')[0], false);
                ValidatorEnable($('[id*=RequiredFieldValidator10]')[0], false);

            }
            else {
                if ($('#<%= ddlAppStatus.ClientID %>').val() == 'A' && $('#<%= ddlPayType.ClientID %>').val() == 'Cheque') {

                    $('#<%= lblChqDet.ClientID %>').html('Cheque Detail');
                    
                    $('#<%= divChq.ClientID %>').show();
                    $('#<%= lblChqNo.ClientID %>').show();
                    $('#<%= lblChqDate.ClientID %>').show();
                    $('#<%= txtChqNo.ClientID %>').show();
                    $('#<%= txtChqDate.ClientID %>').show();
                    
                    ValidatorEnable($('[id*=RequiredFieldValidator8]')[0], true);
                    ValidatorEnable($('[id*=RequiredFieldValidator9]')[0], true);
                    ValidatorEnable($('[id*=RequiredFieldValidator10]')[0], true);
                }
                else {
                    $('#<%= lblChqDet.ClientID %>').html('Account Detail');

                    $('#<%= divChq.ClientID %>').show();
                    $('#<%= lblChqNo.ClientID %>').hide();
                    $('#<%= lblChqDate.ClientID %>').hide();
                    $('#<%= txtChqNo.ClientID %>').hide();
                    $('#<%= txtChqDate.ClientID %>').hide();

                    ValidatorEnable($('[id*=RequiredFieldValidator8]')[0], true);
                    ValidatorEnable($('[id*=RequiredFieldValidator9]')[0], false);
                    ValidatorEnable($('[id*=RequiredFieldValidator10]')[0], false);
                }
            }

        });


        $('#<%= ddlPayType.ClientID %>').change(function(event) {
            if ($('#<%= ddlAppStatus.ClientID %>').val() == 'P') {
                $('#<%= divChq.ClientID %>').hide();
                ValidatorEnable($('[id*=RequiredFieldValidator8]')[0], false);
                ValidatorEnable($('[id*=RequiredFieldValidator9]')[0], false);
                ValidatorEnable($('[id*=RequiredFieldValidator10]')[0], false);

            }
            else {
                if ($('#<%= ddlAppStatus.ClientID %>').val() == 'A' && $('#<%= ddlPayType.ClientID %>').val() == 'Cheque') {
                    $('#<%= lblChqDet.ClientID %>').html('Cheque Detail');

                    $('#<%= divChq.ClientID %>').show();
                    $('#<%= lblChqNo.ClientID %>').show();
                    $('#<%= lblChqDate.ClientID %>').show();
                    $('#<%= txtChqNo.ClientID %>').show();
                    $('#<%= txtChqDate.ClientID %>').show();
                    
                    ValidatorEnable($('[id*=RequiredFieldValidator8]')[0], true);
                    ValidatorEnable($('[id*=RequiredFieldValidator9]')[0], true);
                    ValidatorEnable($('[id*=RequiredFieldValidator10]')[0], true);
                }
                else {
                    $('#<%= lblChqDet.ClientID %>').html('Account Detail');

                    $('#<%= divChq.ClientID %>').show();
                    $('#<%= lblChqNo.ClientID %>').hide();
                    $('#<%= lblChqDate.ClientID %>').hide();
                    $('#<%= txtChqNo.ClientID %>').hide();
                    $('#<%= txtChqDate.ClientID %>').hide();
                    
                    ValidatorEnable($('[id*=RequiredFieldValidator8]')[0], true);
                    ValidatorEnable($('[id*=RequiredFieldValidator9]')[0], false);
                    ValidatorEnable($('[id*=RequiredFieldValidator10]')[0], false);
                }
            }

        });

        $('#<%= txtChqBranch.ClientID %>').click(function(event) {
            this.select();
        });

        $('#<%= txtChqBranch.ClientID %>').autocomplete({

            source: function(request, response) {
                $.ajax({
                    url: "EmpMgtLoan.aspx/GetBranch",
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
                debugger;
                $('#<%= hdnGlCode.ClientID %>').val(ui.item.id);
                //alert($('#<%= hdnGlCode.ClientID %>').val());
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
                           <asp:ValidationSummary ID="main" CssClass="text-warning" runat="server" DisplayMode="List" ValidationGroup="main" />
                            <uc1:Messages ID="ucMessage" runat="server" />
                       </div>
                   </div>
                   <div class="row">
                       <div class="col-lg-4 col-md-4 col-sm-4">
                             <label>Divisions:</label>  
                            <asp:DropDownList ID="searchBranchDropDown" runat="server" CssClass="form-control searchbranchchange" OnSelectedIndexChanged="searchBranchDropDown_SelectedIndexChanged" AppendDataBoundItems="True" AutoPostBack="true">
                                <asp:ListItem Value="0">Select Division</asp:ListItem>
                            </asp:DropDownList>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Job Type*</label>
                                   <asp:DropDownList ID="ddlJobType" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlJobtype_SelectedIndexChanged" AppendDataBoundItems="True" AutoPostBack="true">
                                       <asp:ListItem Value="0" Selected="True">Select Job Type</asp:ListItem>
                                       <%--<asp:ListItem Value="Officer">Officer</asp:ListItem>
                                    <asp:ListItem Value="Permanent">Permanent</asp:ListItem>
                                    <asp:ListItem Value="Contract">Contract</asp:ListItem>
                                    <asp:ListItem Value="DailyWager">Daily Wager</asp:ListItem>
                                    <asp:ListItem Value="Consultant">Consultant</asp:ListItem>--%>
                                            </asp:DropDownList>

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlJobType"
                                                ErrorMessage="Please select job type" SetFocusOnError="true" ValidationGroup="main"
                                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Employees*</label>
                                <asp:UpdatePanel ID="Disupnl" runat="server">
                                    <ContentTemplate>
                                 <asp:DropDownList ID="ddlEmployeeSearch" runat="server" CssClass="form-control districselect" AppendDataBoundItems="false">
                                     <asp:ListItem Value="0">Select Employee</asp:ListItem>
                                </asp:DropDownList>
                                    </ContentTemplate>
                                   <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlJobType"  />
                                   </Triggers>
                                </asp:UpdatePanel>
                            </div>
                       <%--<div class="col-lg-12 col-md-12 col-sm-12">
                           <uc3:EmpSearchUC ID="EmpSrchUC" runat="server" />
                       </div>--%>
                   </div>
                   &nbsp;
                   <div class="row koibe">
                       <div class="col-lg-4 col-md-4 col-sm-4">
                           <label>Pay Type:</label>
                           <asp:DropDownList ID="ddlPayType"  runat="server" CssClass="RequiredField form-control" >
                            <asp:ListItem Value="Cheque" > Cheque</asp:ListItem >
                            </asp:DropDownList >
                       </div>
                       <div class="col-lg-4 col-md-4 col-sm-4">
                           <label>Type:</label>
                           <asp:DropDownList ID="ddlLoanType" CssClass="RequiredField form-control" runat="server" AppendDataBoundItems="true" >
                           </asp:DropDownList >
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlLoanType"
                           ErrorMessage="Please select deduction type" SetFocusOnError="true" ValidationGroup="main"
                           Display="None" InitialValue="0" > </asp:RequiredFieldValidator >
                       </div>
                       <div class="col-lg-4 col-md-4 col-sm-4">
                           <label>Advance Amount</label>
                           <asp:TextBox ID="txtClaimAmt" runat="server" CssClass="RequiredField form-control amount" Style="text-align: right"
                           MaxLength="7" > </asp:TextBox >
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" SetFocusOnError="true"
                           ErrorMessage="Please enter advance amount" ControlToValidate="txtClaimAmt" ValidationGroup="main"
                           Display="None" > </asp:RequiredFieldValidator >
                           <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="Invalid advance amount"
                           MinimumValue="1" MaximumValue="9999999" ControlToValidate="txtClaimAmt" SetFocusOnError="true" Type="Integer"
                           ValidationGroup="main" Display="None" > </asp:RangeValidator >
                       </div>
                   </div>
                   <div class="row">
                       <div class="col-lg-4 col-md-4 col-sm-4">
                           <label>Effective Date:</label> <span class="DteLtrl" > <asp:Literal ID="Literal2" runat="server" Text="<%$ AppSettings: DateFormatPageText %>" /></span >
                           <asp:TextBox ID="txtEffDate" runat="server" MaxLength="11" CssClass="RequiredField form-control" > </asp:TextBox >
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Please select effective date"
                           ControlToValidate="txtEffDate" SetFocusOnError="true" ValidationGroup="main"
                           Display="None" > </asp:RequiredFieldValidator >
                           <ajaxToolkit:CalendarExtender ID="txtEffDateCal" runat="server" Enabled="True" TargetControlID="txtEffDate" >
                           </ajaxToolkit:CalendarExtender >
                           <asp:TextBox ID="txtClaimYtd" runat="server" CssClass="RequiredField" Visible="false" > </asp:TextBox >
                       </div>
                       <div class="col-lg-4 col-md-4 col-sm-4">
                           <label>Installment Size:</label>
                           <asp:TextBox ID="txtLimYtd" runat="server" Style="text-align: left" CssClass="RequiredField form-control size"></asp:TextBox >
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please enter installment size"
                           ControlToValidate="txtLimYtd" SetFocusOnError="true" ValidationGroup="main" Display="None" > </asp:RequiredFieldValidator >
                           <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="Invalid Installment size "
                           MinimumValue="1" MaximumValue="254" Type="Integer" ControlToValidate="txtLimYtd" SetFocusOnError="true"
                           ValidationGroup="main" Display="None" > </asp:RangeValidator >
                       </div>
                       <div class="col-lg-4 col-md-4 col-sm-4">
                           <label>Deduction Amount:</label>
                           <asp:TextBox ID="txtOv" runat="server" Style="text-align: right" CssClass="RequiredField form-control dedamount" MaxLength="6"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please enter deduction amount"
                                ControlToValidate="txtOv" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="RangeValidator3" runat="server" ErrorMessage="Invalid deduction amount"
                                MinimumValue="1" MaximumValue="999999" ControlToValidate="txtLimYtd" SetFocusOnError="true" Type="Integer"
                                ValidationGroup="main" Display="None"></asp:RangeValidator>
                           
                       </div>
                       <div class="col-lg-4 col-md-4 col-sm-4">
                           <label>Deducted To-Date:</label>
                           <%--<asp:TextBox ID="txtAppBy" runat="server" CssClass="RequiredField form-control" Enabled="false" Visible="false" > </asp:TextBox >--%>
                           <asp:TextBox ID="txtOvLimApp" runat="server" CssClass="RequiredField form-control" ></asp:TextBox >
                           <ajaxToolkit:CalendarExtender ID="txtOvLimAppCal" runat="server" Enabled="True" TargetControlID="txtOvLimApp" >
                           </ajaxToolkit:CalendarExtender >
                       </div>
                       <div class="col-lg-4 col-md-4 col-sm-4">
                           <label>Status:</label>
                           <asp:DropDownList ID="ddlAppStatus" runat="server" CssClass="RequiredField form-control stat" >
                            <asp:ListItem Value="P" > Pending</asp:ListItem >
                            <asp:ListItem Value="A" > Approved</asp:ListItem >
                            </asp:DropDownList >
                       </div>
                       <div class="col-lg-4 col-md-4 col-sm-4" style="margin-top:40px">
                        <asp:CheckBox ID="checkIsActive" class="checkbox" runat="server" Checked="true" />&nbsp; <label id="per">Is Active</label>
                       </div>
                   </div>
                   &nbsp;
                   <div runat="server" id="divChq" class="DisplayNone">
                       <div class="row">
                           <div class="col-lg-4 col-md-4 col-sm-4 offset-lg-4" style="font-size:30px;">
                               <asp:Label ID="lblChqDet" runat="server" Text="Cheque Details" CssClass="text-capitalize"></asp:Label >
                           </div>
                       </div>
                       <div class="row">
                           
                           <div class="col-lg-4 col-md-4 col-sm-4">
                               <asp:Label ID="lblBranch" runat="server" Text="Branch"></asp:Label >
                               <asp:TextBox ID="txtChqBranch" runat="server" CssClass="RequiredField form-control" MaxLength="50"> </asp:TextBox >
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtChqBranch"
                                ErrorMessage="Please enter branch name" SetFocusOnError="true" ValidationGroup="main"
                                Display="None" > </asp:RequiredFieldValidator >
                                <asp:HiddenField ID="hdnGlCode" runat="server" />                                
                           </div>
                           <div class="col-lg-4 col-md-4 col-sm-4">
                               <asp:Label ID="lblAccount" runat="server" Text="Account No"></asp:Label >
                               <asp:TextBox ID="txtChqAcctNo" CssClass="form-control" runat="server" MaxLength="20"> </asp:TextBox >
                           </div>
                           <div class="col-lg-4 col-md-4 col-sm-4">
                               <asp:Label runat="server" Text="Cheque #" ID="lblChqNo"></asp:Label>
                               <asp:TextBox ID="txtChqNo" runat="server" CssClass="RequiredField form-control" MaxLength="20"> </asp:TextBox >
                               <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtChqNo"
                               ErrorMessage="Please enter cheque #" SetFocusOnError="true" ValidationGroup="main"
                               Display="None" > </asp:RequiredFieldValidator >
                           </div>
                           <div class="col-lg-4 col-md-4 col-sm-4">
                               <asp:Label ID="lblChqDate" runat="server" Text="Date"></asp:Label>
                               <ajaxToolkit:CalendarExtender ID="txtChqDateCal" runat="server" TargetControlID="txtChqDate"
                               Enabled="True" >
                               </ajaxToolkit:CalendarExtender >
                               <asp:TextBox ID="txtChqDate" runat="server" CssClass="RequiredFieldDate form-control" > </asp:TextBox > <br / >
                               <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtChqDate"
                               ErrorMessage="Please select date of check" SetFocusOnError="true" ValidationGroup="main"
                               Display="None" > </asp:RequiredFieldValidator >
                           </div>
                       </div>
                   </div>
                   &nbsp;
                   <div class="row">
                       <div class="col-lg-4 col-md-4 col-sm-4">
                           <asp:Button runat="server" ID="Button19" OnClick="Onclick_Save" class="btn btn-primary" Text="Save" ValidationGroup=""></asp:Button>
                            <asp:Button runat="server" ID="Button244" OnClick="Onclick_Clear" CssClass="btn btn-danger" Text="Clear" />
                           <%--<uc2:Buttons ID="ucButtons" OnButtonClick="ButtonCommand" runat="server" />
                           <asp:ImageButton ID="btnDelete" runat ="server" OnClick="btnDelete_Click" ImageUrl="~/images/btn_delete.png" onMouseOver="this.src='../images/btn_delete_m.png'" onMouseOut="this.src='../images/btn_delete.png'" Visible="false" />--%>
                       </div>
                   </div>
                   &nbsp;
                   <div class="row">
                       <div class="col-lg-12 col-md-12 col-sm-12">
                           <asp:GridView ID="grdLoans" CssClass="table table-hover" runat="server" DataKeyNames="CPFID,EmpID,LoanTypeID"
                           OnSelectedIndexChanged="grdLoans_SelectedIndexChanged" AutoGenerateColumns="False"
                           AllowPaging="True" OnPageIndexChanging="grdLoans_PageIndexChanging" OnRowDataBound="grdLoans_RowDataBound"
                           EmptyDataText="No CPF Advance">
                           <Columns >
                           <asp:BoundField DataField="FullName" HeaderText="Employee Name" />
                           <asp:BoundField DataField="AdvanceAmount" HeaderText="Advance Amount" />
                           <asp:BoundField DataField="AdvanceDate" HeaderText="Effective Date" />
                           <asp:BoundField DataField="NoOfInst" HeaderText="Installment Size" />
                           <asp:BoundField DataField="DedAmount" HeaderText="Deduction" />
                           <asp:BoundField DataField="BankBranch" HeaderText="Bank Branch" />
                           <asp:BoundField DataField="ChqNo" HeaderText="Chq No" />
                           <asp:BoundField DataField="Advstatus" HeaderText="Status" />
                           <asp:CommandField ControlStyle-CssClass="lnk" ShowSelectButton="True">
                           <ControlStyle CssClass="lnk select" ></ControlStyle>
                           </asp:CommandField>
                           </Columns>
                           <HeaderStyle CssClass="grid_hdr" />
                           <RowStyle CssClass="grid_row" />
                           <AlternatingRowStyle CssClass="gridAlternateRow" />
                           <SelectedRowStyle CssClass="gridSelectedRow" />
                           </asp:GridView>
                       </div>
                   </div>
               </div>
           </div>
       </div>
   </div>
</asp:Content>
