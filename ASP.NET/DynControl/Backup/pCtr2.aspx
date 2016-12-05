<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pCtr2.aspx.cs" Inherits="nsDynControl.pCtr2"
    MasterPageFile="Site.Master" %>

<%@ Register TagPrefix="uc" TagName="DynCtlMgr" Src="UserControls/DynCtlMgr.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHead" runat="Server">
    <b>Control Page Two</b>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc:DynCtlMgr ID="ucDynCtlMgr" runat="server" ControlFileName="Ctr2.ascx" ControlClassName="Ctr2" ControlDeleteSpName="usp_DeleteCategory" ControlGetSpName="usp_GetCategory" ControlSaveSpName="usp_SaveCategory"/>
</asp:Content>
