<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="grammar.aspx.cs" Inherits="ECEG_Migration.grammar" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Table ID="table_item" runat="server" CssClass="table">
        <asp:TableRow>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <div class="row">
        <button runat="server" id="btn_prev" class="btn btn_direction" onserverclick="btn_prev_ServerClick">
            <i class="fas fa-chevron-left fa-9x"></i>
        </button>
        <div class="center">
            <p runat="server" id="grammar_page_counter"></p>
        </div>
        <button runat="server" id="btn_forw" class="btn btn_direction" onserverclick="btn_forw_ServerClick">
            <i class="fas fa-chevron-right fa-9x"></i>
        </button>
    </div>
    
</asp:Content>
