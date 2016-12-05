<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DynCtlMgr.ascx.cs" Inherits="nsDynControl.DynCtlMgr" %>
<asp:Panel ID="pnControl" runat="server">
</asp:Panel>
<table cellpadding="2" cellspacing="2">
    <tr>
        <td nowrap>
            <asp:LinkButton ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" />
        </td>
        <td>
            <asp:LinkButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
        </td>
    </tr>
</table>
