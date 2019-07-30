<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="grammar.aspx.cs" Inherits="ECEG_Migration.grammar" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p runat="server" id="grammar_title" class="h2"></p>
    <br />
    <aside class="sidebar" id="sidebar">
        <div class="mt-5 mb-3 sticky-top" id="side">
            <ul class="nav flex-md-column flex-row justify-content-between" id="sidenav">

                <li class="nav-item">
                    <a href="#author_section" class="nav-link active pl-0">Author</a>
                    <ul class="nav flex-md-column ml-2">
                        <li class="nav-item"><a href="#author_occ" class="nav-link">Occupation</a></li>
                        <li class="nav-item"><a href="#author_bio" class="nav-link">Biography</a></li>
                    </ul>
                </li>

                <li class="nav-item">
                    <a href="#contents_section" class="nav-link pl-0">Contents</a>
                    <ul class="nav flex-md-column ml-2">
                        <li class="nav-item"><a href="#grammar_divisions_info" class="nav-link">Type of work and division</a></li>
                        <li class="nav-item"><a href="#subsidiary_content_info" class="nav-link">Subsidiary content</a></li>
                        <li class="nav-item"><a href="#audience_info" class="nav-link">Target audience</a></li>
                    </ul>
                </li>

                <li class="nav-item"><a href="#imprint_section" class="nav-link pl-0">Imprint</a></li>

                <li class="nav-item"><a href="#editions_section" class="nav-link pl-0">Editions</a></li>

                <li class="nav-item"><a href="#reference_section" class="nav-link pl-0">References</a></li>

                <li class="nav-item"><a href="#comment_section" class="nav-link pl-0">Comments</a></li>

            </ul>
        </div>
    </aside>

    <section id="author_section">
        <p class="h3">Author</p>
        <p id="written_by" runat="server"></p>
        <p id="author_pob" runat="server"></p>
        <p id="author_occ" class="h5">Occupation</p>
        <asp:Table runat="server" ID="table_occupation" CssClass="table"></asp:Table>
        <p id="author_bio" class="h5">Biography</p>
        <p id="author_bio" runat="server"></p>
    </section>
    <br />

    <section id="contents_section">
        <p class="h3">Contents</p>
        <section id="grammar_divisions_info">
            <p class="h5">Work type and divisions</p>
            <asp:Table runat="server" ID="table_work_types" CssClass="table">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>Type of Work</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Divisions of Grammar</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </section>
        <section id="subsidiary_content_info">
            <p class="h5">Subsidiary content</p>
            <asp:UpdatePanel ID="update_panel_subs" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="table_sub_content" runat="server"
                        ShowHeader="false"
                        AutoGenerateColumns="false"
                        AutoGenerateSelectButton="false"
                        AllowPaging="true"
                        PageSize="5"
                        GridLines="None"
                        Width="100%"
                        EmptyDataText="No references available..."
                        CssClass="table table-responsive"
                        OnPageIndexChanging="table_sub_content_PageIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="sub_content_name" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </section>
        <section id="audience_info">
            <p class="h5">Target audience</p>
            <asp:Table runat="server" ID="table_audience" CssClass="table">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>Age</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Gender</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Instruction</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Specific Purpose</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </section>
    </section>
    <br />

    <section id="imprint_section">
        <p class="h3">Imprint</p>
        <p id="printed_in" runat="server"></p>
        <p class="h5">Printers</p>
        <p id="printed_by" runat="server"></p>
        <p class="h5">Booksellers</p>
        <p id="sold_by" runat="server"></p>
        <p class="h5">Price</p>
        <p id="price" runat="server"></p>
        <p class="h5">Physical description</p>
        <p id="imprint_description" runat="server"></p>
    </section>
    <br />

    <section id="editions_section">
        <p class="h3">Editions</p>
        <asp:UpdatePanel ID="update_panel_editions" runat="server">
            <ContentTemplate>
                <asp:GridView ID="table_editions" runat="server"
                    ShowHeader="true"
                    AutoGenerateColumns="false"
                    AutoGenerateSelectButton="false"
                    AllowPaging="true"
                    PageSize="10"
                    GridLines="None"
                    Width="100%"
                    EmptyDataText="No results with that parameters..."
                    CssClass="table table-responsive"
                    OnPageIndexChanging="table_editions_PageIndexChanging"
                    OnRowDataBound="table_editions_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="edition_year" HeaderText="Year" />
                        <asp:BoundField DataField="edition_number" HeaderText="Edition number" />
                        <asp:BoundField DataField="printing_place" HeaderText="Place" />
                        <asp:BoundField DataField="description" HeaderText="Description" />
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </section>
    <br />

    <section id="reference_section">
        <p class="h3">References</p>
        <div class="row">
            <section id="references_info" class="col-6">
                <p class="h5">Reference sources</p>
                <asp:UpdatePanel ID="update_panel_references" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="table_references" runat="server"
                            ShowHeader="false"
                            AutoGenerateColumns="false"
                            AutoGenerateSelectButton="false"
                            AllowPaging="true"
                            PageSize="5"
                            GridLines="None"
                            Width="100%"
                            EmptyDataText="No references available..."
                            CssClass="table table-responsive"
                            OnPageIndexChanging="table_references_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="description" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </section>
            <section id="libraries_info" class="col-6">
                <p class="h5">Holding libraries</p>
                <asp:UpdatePanel ID="update_panel_libraries" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="table_libraries" runat="server"
                            ShowHeader="false"
                            AutoGenerateColumns="false"
                            AutoGenerateSelectButton="false"
                            AllowPaging="true"
                            PageSize="5"
                            GridLines="None"
                            Width="100%"
                            EmptyDataText="No references available..."
                            CssClass="table table-responsive"
                            OnPageIndexChanging="table_libraries_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="library_name" HtmlEncode="false" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </section>
        </div>
    </section>
    <br />

    <section id="comment_section">
        <p class="h3">Comments</p>
        <section id="comment_info">
            <asp:Label runat="server" ID="label_comments"></asp:Label>
        </section>
    </section>
    <br />

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
