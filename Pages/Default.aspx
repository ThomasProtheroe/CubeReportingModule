﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CubeReportingModule.Pages.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPane" runat="server">
    <div>
        Username:
        <asp:TextBox runat="server" required="true" />
        <br />
        Password:
        <asp:TextBox runat="server" type="password" required="true" />
        <br />
        <asp:Button runat="server" id="Login" text="Login" OnClick="Login_Click"/>
        <br />
        <asp:HyperLink runat="server" Text="Forgot your password?" />
    </div>
</asp:Content>
