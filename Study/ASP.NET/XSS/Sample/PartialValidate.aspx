<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PartialValidate.aspx.cs" Inherits="PartialValidate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
     <h2>
        Request Validation with ASP.NET 4.5!
    </h2>
   
    <div id="dvForm" runat="server">           <asp:TextBox ID="txtValidate" runat="server"  ValidateRequestMode="Disabled"></asp:TextBox><br />
        <asp:TextBox ID="txtunValidate" runat="server" ValidateRequestMode="Disabled"></asp:TextBox><br />
        <asp:Button ID="Button1" runat="server" Text="Submit" OnClick="Button1_Click" />
    </div>
</asp:Content>

