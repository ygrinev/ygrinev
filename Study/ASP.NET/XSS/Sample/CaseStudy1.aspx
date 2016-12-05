<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CaseStudy1.aspx.cs" Inherits="CaseStudy1" %>

<%@ Register TagPrefix="myControl"  Namespace="CustomControl"  Assembly="CustomControl" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>
        Request Validation with ASP.NET 4.5 - Case Study 1!
    </h2>
     <myControl:CustomTextBox ID="TextBox1" runat="server" ValidateRequestMode="Enabled">
     </myControl:CustomTextBox><br /> 
     <asp:Button ID="Button1" runat="server" Text="Submit" OnClick="Button1_Click" />
</asp:Content>

