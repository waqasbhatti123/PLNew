<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Buttons.ascx.cs" Inherits="RMS.UserControl.Buttons" %>
<div>
    <asp:ImageButton ID="btnNew" ImageUrl="~/images/btn_new.jpg" runat="server"  CssClass="buttonNew" OnCommand="ButtonCommand"
        CommandName="New" Visible="false"/>

    <%--<asp:ImageButton ID="btnEdit" ImageUrl="~/images/btn_save.png" runat="server" OnCommand="ButtonCommand" CommandName="Save"
       onMouseOver="this.src='../images/btn_save_m.png'" onMouseOut="this.src='../images/btn_save.png'" CssClass="buttonEdit" Visible="false" />--%>

       <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-primary" CommandName="Save" Text="Save" OnCommand="ButtonCommand"/>


    <asp:ImageButton  ID="btnDelete" ImageUrl="~/images/btn_delete.jpg" runat="server" OnClientClick="<%$ Resources:MainResource, confirmation %>"
        CssClass="buttonDel" OnCommand="ButtonCommand"  
        CommandName="Delete" Visible="false"/>

    <%--<asp:ImageButton ID="btnSave" runat="server"  ImageUrl="~/images/btn_save.png" ValidationGroup="main" CssClass="buttonSave"
        onMouseOver="this.src='../images/btn_save_m.png'" onMouseOut="this.src='../images/btn_save.png'" CommandName="Save" OnCommand="ButtonCommand" />--%>

       <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" CommandName="Save" Text="Save" OnCommand="ButtonCommand"/>

       <asp:Button ID="btnPrint" runat="server" ImageUrl="~/images/btn_print.jpg" Visible="false"  CssClass="buttonPrint" CommandName="Print" OnCommand="ButtonCommand"/>

    <%--<asp:ImageButton ID="btnCancel" runat="server"  ImageUrl="~/images/btn_clear.png" OnCommand="ButtonCommand"
       onMouseOver="this.src='../images/btn_clear_m.png'" onMouseOut="this.src='../images/btn_clear.png'" CssClass="buttonCancel" CommandName="Cancel" />--%>

       <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" CommandName="Cancel" Text="Clear" OnCommand="ButtonCommand"/>

        
</div>
