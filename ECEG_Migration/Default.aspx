<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ECEG_Migration.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:AccessDataSource ID="query_AllYears" runat="server" DataFile="~/App_Data/ECEG_2018.mdb"
        SelectCommand="SELECT DISTINCT YearP from Grammars ORDER BY YearP"></asp:AccessDataSource>

    <asp:UpdatePanel ID="update_year_dropdown_panel" runat="server">
        <ContentTemplate>
            <div class="p-2 form-inline" id="main_filter">
                <%--                <asp:DropDownList ID="dropdown_year" runat="server" CssClass="form-control m-lateral"
                    OnSelectedIndexChanged="dropdown_year_SelectedIndexChanged"
                    AutoPostBack="true" AppendDataBoundItems="true">
                    <asp:ListItem Text="All years" Value="All" Selected="True"></asp:ListItem>
                </asp:DropDownList>--%>
                <asp:ListBox ID="dropdown_year" runat="server" CssClass="form-control m-lateral multi"
                    SelectionMode="Multiple"
                    AutoPostBack="true"
                    AppendDataBoundItems="true"
                    OnSelectedIndexChanged="dropdown_year_SelectedIndexChanged"
                    title="All years"
                    data-actions-box="true"
                    data-selected-text-format="count > 3"></asp:ListBox>
                <%--                <asp:DropDownList ID="dropdown_editions" runat="server" CssClass="form-control m-lateral multi"
                    OnSelectedIndexChanged="dropdown_editions_SelectedIndexChanged"
                    AutoPostBack="true" AppendDataBoundItems="true">
                    <asp:ListItem Text="Any edition" Value="Any" Selected="true"></asp:ListItem>
                </asp:DropDownList>--%>
                <asp:ListBox ID="dropdown_editions" runat="server" CssClass="form-control m-lateral multi"
                    OnSelectedIndexChanged="dropdown_editions_SelectedIndexChanged"
                    AutoPostBack="true" AppendDataBoundItems="true"
                    SelectionMode="Multiple"
                    title="All editions"
                    data-actions-box="true"
                    data-selected-text-format="count > 3"></asp:ListBox>
                <div class="m-lateral">
                    <asp:TextBox runat="server" ID="input_title" CssClass="form-control" placeholder="Grammar title..."></asp:TextBox>
                    <asp:Button runat="server" ID="bnt_title" CssClass="btn btn-outline-secondary fix-size" OnClick="titleBtn_Click" Text="Search" />
                </div>
            </div>

            <ul class="nav nav-pills nav-justified" id="pills-tab" role="tablist">
                <li class="nav-item">
                    <a class="nav-link active" id="pills-author-tab" data-toggle="pill" href="#pills-author" role="tab" aria-controls="pills-author" aria-selected="true">Author</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="pills-contents-tab" data-toggle="pill" href="#pills-contents" role="tab" aria-controls="pills-contents" aria-selected="false">Contents</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="pills-imprint-tab" data-toggle="pill" href="#pills-imprint" role="tab" aria-controls="pills-imprint" aria-selected="false">Imprint</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="pills-editions-tab" data-toggle="pill" href="#pills-editions" role="tab" aria-controls="pills-editions" aria-selected="false">Editions</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="pills-references-tab" data-toggle="pill" href="#pills-references" role="tab" aria-controls="pills-references" aria-selected="false">References</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="pills-comments-tab" data-toggle="pill" href="#pills-comments" role="tab" aria-controls="pills-comments" aria-selected="false">Comments</a>
                </li>
            </ul>
            <div class="tab-content" id="pills-tabContent">
                <div class="tab-pane fade show active" id="pills-author" role="tabpanel" aria-labelledby="pills-author-tab">Author filter</div>
                <div class="tab-pane fade" id="pills-contents" role="tabpanel" aria-labelledby="pills-contents-tab">Contents filter</div>
                <div class="tab-pane fade" id="pills-imprint" role="tabpanel" aria-labelledby="pills-imprint-tab">Imprint filter</div>
                <div class="tab-pane fade" id="pills-editions" role="tabpanel" aria-labelledby="pills-editions-tab">Editions filter</div>
                <div class="tab-pane fade" id="pills-references" role="tabpanel" aria-labelledby="pills-references-tab">References filter</div>
                <div class="tab-pane fade" id="pills-comments" role="tabpanel" aria-labelledby="pills-comments-tab">Comments filter</div>
            </div>

            <asp:Label ID="label_results" runat="server" CssClass="float-right" />
            <asp:GridView ID="table_allGrammars" runat="server"
                ShowHeader="true"
                AutoGenerateColumns="false"
                AutoGenerateSelectButton="false"
                AllowPaging="true"
                PageSize="10"
                GridLines="None"
                Width="100%"
                EmptyDataText="No results with that parameters..."
                CssClass="table table-responsive all_grammar_table"
                OnRowDataBound="table_AllGrammars_RowDataBound"
                OnRowCommand="table_AllGrammars_RowCommand"
                OnPageIndexChanging="table_allGrammars_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="grammarId" HeaderText="Grammar" />
                    <asp:BoundField DataField="grammarPublicationYear" HeaderText="Publication year" />
                    <asp:BoundField DataField="grammarTitle" HeaderText="Title" HtmlEncode="false" />
                    <asp:CommandField ButtonType="Link" ShowSelectButton="true" SelectText="Seleccionar" ItemStyle-CssClass="btn" />
                </Columns>
            </asp:GridView>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
