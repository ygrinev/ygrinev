<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pCtr1.aspx.cs" Inherits="nsDynControl.pCtr1"
    MasterPageFile="~/Site.Master" %>

<%@ Register TagPrefix="uc" TagName="DynCtlMgr" Src="UserControls/DynCtlMgr.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <b>Control Page One</b>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc:DynCtlMgr ID="ucDynCtlMgr" runat="server" ControlFileName="Ctr1.ascx" ControlClassName="Ctr1" ControlDeleteSpName="usp_DeleteContact" ControlGetSpName="usp_GetContact" ControlSaveSpName="usp_SaveContact"/>
</asp:Content>
