<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Ctr1.ascx.cs" Inherits="nsDynControl.Ctr1" %>

<table border="0" cellpadding="4">
    <tr>
        <td align="left" valign="top" colspan="2">            
            <asp:CheckBox ID="chkDel" runat="server" Text="Remove" CssClass="radiobttn" />
        </td>
    </tr>
    <tr>
        <td align="left">
            <b>First Name:</b>
        </td>
        <td align="left">
            <asp:TextBox ID="txtfname" runat="server" MaxLength="50"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align="left">
            <b>Last Name:</b>
        </td>
        <td align="left">
            <asp:TextBox ID="txtlname" runat="server" MaxLength="50"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <hr />
        </td>
    </tr>
</table>
