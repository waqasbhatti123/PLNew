<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="PostConsumption.aspx.cs" Inherits="RMS.Inv.PostConsumption" Culture="auto" UICulture="auto"
    EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" language="javascript">
    //DISABLING DOUBLE CLICK
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    function BeginRequestHandler(sender, args) { var oControl = args.get_postBackElement(); oControl.disabled = true; }

</script>
<script type="text/javascript">

    function pageLoad() {

        $("[src*=plus]").on("click", null, function () {
            if ($(this).attr("src") == '../images/plus.png') {
                $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
                $(this).attr("src", "../images/minus.png");
            }
            else if ($(this).attr("src") == '../images/minus.png') {
                $(this).attr("src", "../images/plus.png");
                $(this).closest("tr").next().remove();
            }
            else { }
        });


//        $("[src*=plus]").live("click", function () {
//            alert(1);
//            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
//            $(this).attr("src", "../images/minus.png");
//        });
//        $("[src*=minus]").live("click", function () {
//            alert(0);
//            $(this).attr("src", "../images/plus.png");
//            $(this).closest("tr").next().remove();
//        });

        }

        function CheckAll(chk) {
            all = document.getElementsByTagName("input");
            for (i = 0; i < all.length; i++) {
                if (all[i].type == "checkbox" && all[i].id.indexOf("gvConsumption") > -1) {
                    all[i].checked = chk.checked;
                }
            }
        }
        function prompt4Post() {
            return confirm("Are your sure, you want to post consumption?");
        }

</script> 
  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
    <b><asp:Label ID="lblTitle" runat="server"></asp:Label></b>
    <br />
        <div style=" width: 100%;">
            <div style="width:100%;">
                 <asp:GridView ID="gvConsumption" runat="server" AutoGenerateColumns="false" DataKeyNames="cc_cd, prnt_cd" 
                    OnRowDataBound="gvConsumption_RowDataBound" Width="100%">
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
                        <asp:TemplateField HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <img id="imgPlusMinus" alt = "" style="cursor: pointer" src="../images/plus.png" />
                                <asp:Panel ID="pnlConsumption" runat="server" style="display:none;" Width="100%" >
                                    <asp:GridView ID="gvConsumptnDet" runat="server" AutoGenerateColumns="false" DataKeyNames= "vr_id"
                                        OnRowDataBound="gvConsumptnDet_RowDataBound" Width="100%">
                                        <HeaderStyle CssClass="t_grd_hdr"></HeaderStyle>
                                        <FooterStyle CssClass="t_grd_footer"></FooterStyle>
                                        <RowStyle CssClass="t_grd_Row"></RowStyle>
                                        <EditRowStyle CssClass="t_grd_Edit_Row" />
                                        <SelectedRowStyle CssClass="t_grd_Selected_Row"  />
                                        <AlternatingRowStyle CssClass="t_grd_Alter_Row"/>
                                        <PagerStyle CssClass="t_grd_Pager"></PagerStyle>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <Columns>
                                            <asp:BoundField ItemStyle-Width="30px" ItemStyle-Font-Size="10px" HeaderStyle-Font-Size="10px" DataField="vr_no" HeaderText="Doc. No" />
                                            <asp:BoundField ItemStyle-Width="50px" ItemStyle-Font-Size="10px" HeaderStyle-Font-Size="10px" DataField="vr_dt" HeaderText="Doc. Date" />
                                            <asp:BoundField ItemStyle-Width="100px" ItemStyle-Font-Size="10px" HeaderStyle-Font-Size="10px" DataField="dept" HeaderText="Department" />
                                            <asp:BoundField ItemStyle-Width="150px" ItemStyle-Font-Size="10px" HeaderStyle-Font-Size="10px" DataField="locName" HeaderText="Location" />
                                            <asp:BoundField ItemStyle-Width="70px" ItemStyle-Font-Size="10px" HeaderStyle-Font-Size="10px" DataField="itm_cd" HeaderText="Item" />
                                            <asp:BoundField ItemStyle-Width="200px" ItemStyle-Font-Size="10px" HeaderStyle-Font-Size="10px" DataField="itm_dsc" HeaderText="Description" />
                                            <asp:BoundField ItemStyle-Width="30px"  ItemStyle-Font-Size="10px" HeaderStyle-Font-Size="10px" DataField="uom_dsc" HeaderText="UOM" />
                                            <asp:BoundField ItemStyle-Width="70px" ItemStyle-Font-Size="10px" HeaderStyle-Font-Size="10px" DataField="vr_qty" HeaderText="Qty Issued" />
                                            <asp:BoundField ItemStyle-Width="70px" ItemStyle-Font-Size="10px" HeaderStyle-Font-Size="10px" DataField="vr_val" HeaderText="Amount" />
                                             <%--<asp:TemplateField ItemStyle-Width="20px" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkChildSelect" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Font-Bold="true" DataField="cc_nme" HeaderText="Cost Center / Consumption Account" />
                        <asp:BoundField ItemStyle-Font-Bold="true" ItemStyle-Width="150px" DataField="vr_val" HeaderText="Consumption Amount" />
                        <%--OnClick="javascript:CheckAll(this);"--%>
                        <asp:TemplateField ItemStyle-Width="20px" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center"
                            ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkselectAll" runat="server"  AutoPostBack="true" OnCheckedChanged="chkSelectAll_Change"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_Change"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

            <div class="btnPost">
                <asp:ImageButton ID="btnPost" runat="server" OnClick="Post_Click" OnClientClick="return prompt4Post()" ImageUrl="~/images/btn_post.png"  onmouseover="this.src='../images/btn_post_m.png';" onmouseout="this.src='../images/btn_post.png';"
                                 ToolTip="Search purchase requests"  />
            </div>
        </div> 

</asp:Content>
