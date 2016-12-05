<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintPreview.aspx.cs" Inherits="nsDynControl.PrintPreview"
    MasterPageFile="~/Site.Master" %>

<%@ Register TagPrefix="uc" TagName="DynCtlMgr" Src="UserControls/DynCtlMgr.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <br/>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc:DynCtlMgr ID="ucDynCtlMgr" runat="server"/>
</asp:Content>
