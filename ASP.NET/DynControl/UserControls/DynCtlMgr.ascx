<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DynCtlMgr.ascx.cs" Inherits="nsDynControl.DynCtlMgr" %>
<link href="<%# ResolveUrl("~/") %>CSS/CommonStyles.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnControl" runat="server" OnPreRender="pnControl_PreRender">
</asp:Panel>
