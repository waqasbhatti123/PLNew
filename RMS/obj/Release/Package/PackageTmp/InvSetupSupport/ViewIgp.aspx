<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewIgp.aspx.cs" Inherits="RMS.InvSetupSupport.ViewIgp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<link rel="Stylesheet" href="../cs/style.css" />
    <title>Lot Details</title>
</head>
<body style="background-color:#F2F2F2;">
    <form id="form1" runat="server">
    <br />
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
          
          <tr>
          <td>
          </td>
          <td>
            <b><asp:Label ID="lblIGP" runat="server" Text="Lot Details:">
            </asp:Label></b>
            <asp:Label ID="lblMsg" runat="server" Visible="false">
            </asp:Label>
          </td>
          <td>
          </td>
          <td></td>
          <td>
          </td>
          </tr>
          
          
          <tr>
          <td>
          </td>
          <td>
           &nbsp;
          </td>
          <td>
          </td>
          <td></td>
          <td>
          </td>
          </tr>
          
            
          <tr valign="top">
          <td width="3%">
          </td>
          <td width="50%">
          <b>Grading Selection Card</b>
            <asp:GridView ID="grdGrading" runat="server"  AutoGenerateColumns="false" 
                 Width="100%" OnRowDataBound="grdGrading_RowDataBound" ShowFooter="true" FooterStyle-HorizontalAlign="Right">
                  <HeaderStyle CssClass ="grid_hdr" />
                  <RowStyle CssClass="grid_row" />
                  <AlternatingRowStyle CssClass="gridAlternateRow" />
                  <SelectedRowStyle CssClass="gridSelectedRow" />
                  <Columns>
                      
                      <asp:BoundField DataField="LotNo" HeaderText ="Lot No"  HeaderStyle-Width="80px" ControlStyle-Width="80px" />
                      <asp:BoundField DataField="SizeGrade_Desc" HeaderText ="Selection"  HeaderStyle-Width="120px" ControlStyle-Width="120px" />
                      <asp:BoundField DataField="vr_qty" HeaderText="Pieces" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="60px" ControlStyle-Width="60px"/>
                      <asp:BoundField DataField="Feetage" HeaderText="Area" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="60px" ControlStyle-Width="60px"/>
                      <asp:CommandField NewText="avg" HeaderText="Average" ItemStyle-HorizontalAlign="Right"   HeaderStyle-Width="60px" ControlStyle-Width="60px"/>
                  </Columns>
              </asp:GridView>
          </td>
          <td width="1%"></td>
          <td width="35%">
          <b>Feetage Card</b>
                <asp:GridView ID="grdFeetage" CellPadding="0" CellSpacing="0" runat="server"
                        AutoGenerateColumns="false" Width="100%" OnRowDataBound="grdFeetage_RowDataBound" ShowFooter="true" FooterStyle-HorizontalAlign="Right" >
                      <HeaderStyle CssClass ="grid_hdr" />
                      <RowStyle CssClass="grid_row" />
                      <AlternatingRowStyle CssClass="gridAlternateRow" />
                        <SelectedRowStyle CssClass="gridSelectedRow" />
              
                  <Columns>
                      
                      <asp:BoundField DataField="LotNo" HeaderText ="Lot No"  HeaderStyle-Width="80px" ControlStyle-Width="80px" />
                      <asp:BoundField DataField="SizeGrade_Desc" HeaderText ="Size"   HeaderStyle-Width="120px" ControlStyle-Width="120px" />
                      <asp:BoundField DataField="vr_qty" HeaderText="Pieces" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="60px" ControlStyle-Width="60px" />
                      <asp:BoundField DataField="Feetage" HeaderText="Area" ItemStyle-HorizontalAlign="Right"   HeaderStyle-Width="60px" ControlStyle-Width="60px" />
                      <asp:BoundField DataField="avgArea" HeaderText="Average" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="60px" ControlStyle-Width="60px"/>  
                  
                  </Columns>
              
                </asp:GridView>
          </td>
          <td width="3%"></td>
          </tr>
          
          <tr>
           <td width="3%">&nbsp</td>
           <td width="90%"><b>IGP Detail</b>
                     <asp:GridView ID="GridView1" CellPadding="0" CellSpacing="0" runat="server" 
                        AutoGenerateColumns="false" Width="100%" OnRowDataBound="GridView1_RowDataBound"  ShowFooter="true" FooterStyle-HorizontalAlign="Right" >
                      <HeaderStyle CssClass ="grid_hdr" />
                      <RowStyle CssClass="grid_row" />
                      <AlternatingRowStyle CssClass="gridAlternateRow" />
                        <SelectedRowStyle CssClass="gridSelectedRow" />
              
                  <Columns>
        
                      <asp:BoundField DataField="IgpSr" HeaderText ="Sr"  HeaderStyle-Width="18px" ControlStyle-Width="18px" HeaderStyle-HorizontalAlign="Left" />
                      <asp:BoundField DataField="Sr" HeaderText ="Party Sr"   HeaderStyle-Width="18px" ControlStyle-Width="18px" />
                      <asp:BoundField DataField="vr_no" HeaderText="IGP No" ItemStyle-HorizontalAlign="Left"  HeaderStyle-Width="75px" ControlStyle-Width="75px" HeaderStyle-HorizontalAlign="Left" />
                      <asp:BoundField DataField="GPRef" HeaderText="GP Ref" ItemStyle-HorizontalAlign="Left"   HeaderStyle-Width="65px" ControlStyle-Width="65px" HeaderStyle-HorizontalAlign="Left" />
                      <asp:BoundField DataField="Party" HeaderText="Party" ItemStyle-HorizontalAlign="Left"  HeaderStyle-Width="325px" ControlStyle-Width="325px" HeaderStyle-HorizontalAlign="Left"/>  
                      <asp:BoundField DataField="vr_qty" HeaderText="Pieces" ItemStyle-HorizontalAlign="Right"   HeaderStyle-Width="65px" ControlStyle-Width="65px" />
                      <asp:BoundField DataField="Price" HeaderText="Prov.Amt" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="80px" ControlStyle-Width="80px"/>  
                     
                  </Columns>
              
                </asp:GridView>
           </td>
           <td>&nbsp</td>
           <td>&nbsp</td>
           <td>&nbsp</td>
           
           <td width="3%"></td>
          </tr>
       
          </table>
    </form>
</body>
</html>
