<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="BankMgt.aspx.cs" Inherits="RMS.Setup.BankMgt" Title="Banks" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

    function pageLoad() {

        $('#<%= txtGlAccCode.ClientID %>').autocomplete({

            source: function(request, response) {
                $.ajax({
                    url: "BankMgt.aspx/GetBranch",
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

                $('#<%= hdnGlCode.ClientID %>').val(ui.item.id);

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
                             <asp:GridView ID="grdBanks" CssClass="table table-responsive-sm table-bordered" runat="server" DataKeyNames="BankCode"
                    OnSelectedIndexChanged="grdBanks_SelectedIndexChanged" AutoGenerateColumns="False"
                    AllowPaging="True" OnPageIndexChanging="grdBanks_PageIndexChanging">
                    <HeaderStyle CssClass ="grid_hdr" />
                    <RowStyle CssClass="grid_row" />
                    <AlternatingRowStyle CssClass="gridAlternateRow" />
                    <SelectedRowStyle CssClass="gridSelectedRow" />
                    <Columns>
                        <asp:BoundField DataField="BankCode" HeaderText="Bank Code" ItemStyle-Width="20%"/>
                        <asp:BoundField DataField="BankName" HeaderText="Branch" ItemStyle-Width="20%"/>
                        <asp:BoundField DataField="BankAbv" HeaderText="Bank Abbv." ItemStyle-Width="20%"/>
                        <asp:BoundField DataField="GlAccCd" HeaderText="GL A/C Code" ItemStyle-Width="40%"/>
                       
                        <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="lnk" ItemStyle-Width="10%">
                            <ItemStyle />
                            <ControlStyle CssClass="lnk"></ControlStyle>
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
                          </div>
                        
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Label ID="lblBankCode" runat="server" Text="Bank Code*"></asp:Label>
                                <asp:TextBox ID="txtBankCode" CssClass="RequiredField form-control" runat="server" MaxLength="4"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBankCode"
                         ErrorMessage="Please enter bank code" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                            <br />
                              <asp:Label ID="lblBankName" runat="server" Text="Branch*"></asp:Label>
                              <asp:TextBox ID="txtBankName" CssClass="RequiredField form-control" runat="server" MaxLength="100"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBankName"
                         ErrorMessage="Please enter branch" SetFocusOnError="true" ValidationGroup="main" Display="None"></asp:RequiredFieldValidator>
                                <br />
                            <asp:Label ID="lblAbbreviation" runat="server" Text="Bank Abbv*"></asp:Label>
                            <asp:TextBox ID="txtBankAbbreviation" CssClass="form-control" runat="server" MaxLength="100"></asp:TextBox>
                            <br />
                            <asp:Label ID="Label1" runat="server" Text="GL A/C Code*"></asp:Label>
                            <asp:TextBox ID="txtGlAccCode" CssClass="form-control" runat="server" MaxLength="12"></asp:TextBox> 
                            <asp:HiddenField ID="hdnGlCode" runat="server" />
                        
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
   
</asp:Content>
