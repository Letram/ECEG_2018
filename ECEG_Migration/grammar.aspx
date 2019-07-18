<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="grammar.aspx.cs" Inherits="ECEG_Migration.grammar" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <label>AUTHOR</label>
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
    <label>OCCUPATION</label>
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
    <label>REFERENCE SOURCES</label>
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
    <label>SUBSIDIARY CONTENT</label>
    <asp:Table runat="server" ID="table_sub_content" CssClass="table"></asp:Table>
    <label>AUDIENCE CRITERIA</label>
    <asp:Table runat="server" ID="table_audience" CssClass="table">
        <asp:TableRow>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <label>COMMENTS</label><br />
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
