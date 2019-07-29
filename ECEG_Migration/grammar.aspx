<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="grammar.aspx.cs" Inherits="ECEG_Migration.grammar" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p runat="server" id="grammar_title" class="h2"></p>
    <br />

    <p class="h3">Author</p>
    <div id="author_info">
        <p id="written_by" runat="server"></p>
        <p id="author_pob" runat="server"></p>
        <p class="h5">OCCUPATION</p>
        <asp:Table runat="server" ID="table_occupation" CssClass="table"></asp:Table>
        <p class="h5">BIO</p>
        <p id="author_bio" runat="server"></p>
    </div>
    <br />

    <p class="h3">Imprint</p>
    <div id="imprint_info">
        <p id="printed_in" runat="server"></p>
        <p class="h5">Printers</p>
        <p id="printed_by" runat="server"></p>
        <p class="h5">Booksellers</p>
        <p id="sold_by" runat="server"></p>
        <p class="h5">Price</p>
        <p id="price" runat="server"></p>
        <p class="h5">Physical description</p>
        <p id="imprint_description" runat="server"></p>
    </div>
    <br />

    <p class="h3">EDITIONS</p>
    <asp:Table runat="server" ID="table_editions" CssClass="table">
    </asp:Table>
    <p class="h3">REFERENCE SOURCES</p>
    <asp:Table runat="server" ID="table_references" CssClass="table"></asp:Table>
    <p class="h3">HOLDING LIBRARIES</p>
    <asp:Table runat="server" ID="table_libraries" CssClass="table"></asp:Table>
    <p class="h3">WORK TYPE AND DIVISION</p>
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
    <p class="h3">COMMENTS</p>
    <br />
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
