<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Ctr2.ascx.cs" Inherits="nsDynControl.Ctr2" %>
<table border="0" cellpadding="4">
    <tr>
        <td align="left" valign="top" colspan="2">            
            <asp:CheckBox ID="chkDel" runat="server" Text="Remove" CssClass="radiobttn" />
        </td>
    </tr>
    <tr>
        <td align="left">
            <b>Category:</b>
        </td>
        <td align="left">
            <asp:DropDownList ID="dwCategory" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td align="left" valign="top">
            <b>Comment:</b>
        </td>
        <td align="left">
            <asp:TextBox ID="txtComment" runat="server" MaxLength="50"></asp:TextBox>
           
        </td>
    </tr>    
    <tr>
        <td colspan="2">
            <hr />
        </td>
    </tr>
</table>