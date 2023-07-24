<%@ Page Language="C#" MasterPageFile="~/home/RMSMasterHome.Master" AutoEventWireup="true"
    CodeBehind="PurchaseInquiry.aspx.cs" Inherits="RMS.sales.PurchaseInquiry" Culture="auto" UICulture="auto"
    EnableEventValidation="true" %>

<%@ Register Src="~/UserControl/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Buttons.ascx" TagName="Buttons" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">

    function pageLoad() {


    }

    function fn_AddFilterRow() {
        
        // 1. Get the <table> element corresponding to the gridview from the document
        var table = document.getElementById('myGridView');
        
        // 2. Insert a new row in the table fetched in step 1.
        var newRow = table.insertRow();
        alert('hello');
        //        // 3. Insert an empty cell for the first "Modify" column
        //                // Column 1 : An empty cell for the "Modify" column
        //                var newCell = newRow.insertCell();
        //                newCell.appendChild(btnDelete);

        //        // 4. Insert an empty cell for "WorkOrderID" in the row created in step 2. above.                                    
        //                // Column 1 : WorkOrderID
        //                newCell = newRow.insertCell();
        //        // In the cell created above, add a textbox in which the user will input the  workorderID value he wishes to insert.
        //            var newTextBox = document.createElement('input');
        //            newTextBox.type = 'text';
        //            newCell.appendChild(newTextBox);
        //    
        //        // 5. Insert an empty cell for ProductID in the row created above.
        //                // Column 2 : ProductID 
        //                newCell = newRow.insertCell();
        //        // In the cell created above, add a textbox in which the user will input the ProductID value he wishes to insert.
        //            var newTextBox = document.createElement('input');
        //            newTextBox.type = 'text';
        //            newCell.appendChild(newTextBox);

        //        // 6. Insert an empty cell for OrderQty in the row created above.                                
        //                // Column3 : OrderQty
        //                newCell = newRow.insertCell();
        //        // In the cell created above, add a textbox in which the user will input the OrderQty value he wishes to insert.
        //            var newTextBox = document.createElement('input');
        //            newTextBox.type = 'text';
        //            newCell.appendChild(newTextBox);
        // 
        //        // Do the same for the remaining columns.

    }

</script>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ValidationSummary ID="main" CssClass="ErrorSummary" runat="server" DisplayMode="List"
        ValidationGroup="main" />
    <uc1:Messages ID="ucMessage" runat="server" />
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="1%"></td>
            <td>
                <table  cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="100%">
                           





                           <input type="button" id="btnAddRow" value="Add" runat="server" />
                            <div>
                                <asp:GridView ID="myGridView" runat="server" DataSourceID="SqlDataSource1">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Modify
                                            </HeaderTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:RMS.BL.Properties.Settings.TSOFTConnectionString %>"
                                SelectCommand="SELECT top 10 AmID, AmName, AmURL FROM [dbo].[tblAppMenu]"></asp:SqlDataSource>
                            </div>





                        </td>
                    </tr>
                </table>
            </td>
            <td width="1%"></td>
        </tr>
    </table>
</asp:Content>
