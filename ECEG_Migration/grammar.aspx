<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="grammar.aspx.cs" Inherits="ECEG_Migration.grammar" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p class="h1" runat="server" id="grammar_title"></p>
    <p class="h3">AUTHOR</p>
    <asp:Table ID="table_author" runat="server" CssClass="table">
        <asp:TableRow Width="100%">
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <p class="h3">OCCUPATION</p>
    <asp:Table runat="server" ID="table_occupation" CssClass="table"></asp:Table>
    <label>IMPRINT</label>
    <asp:Table runat="server" ID="table_imprint" CssClass="table">
        <asp:TableRow Width="100%">
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <p class="h3">REFERENCE SOURCES</p>
    <asp:Table runat="server" ID="table_references" CssClass="table"></asp:Table>
    <label>HOLDING LIBRARIES</label>
    <asp:Table runat="server" ID="table_libraries" CssClass="table"></asp:Table>
    <label>WORK TYPE AND DIVISION</label>
    <asp:Table runat="server" ID="table_work_types" CssClass="table">
        <asp:TableRow>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <p class="h3">EDITIONS</p>
    <asp:Table runat="server" ID="table_editions" CssClass="table"></asp:Table>
    <p class="h3">SUBSIDIARY CONTENT</p>
    <asp:Table runat="server" ID="table_sub_content" CssClass="table"></asp:Table>
    <p class="h3">AUDIENCE CRITERIA</p>
    <asp:Table runat="server" ID="table_audience" CssClass="table">
        <asp:TableRow>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <p class="h3">COMMENTS</p><br />
    <asp:Label runat="server" ID="label_comments"></asp:Label>
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
