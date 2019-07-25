<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ECEG_Migration.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:AccessDataSource ID="query_AllYears" runat="server" DataFile="~/App_Data/ECEG_2018.mdb"
        SelectCommand="SELECT DISTINCT YearP from Grammars ORDER BY YearP"></asp:AccessDataSource>

    <asp:UpdatePanel ID="update_year_dropdown_panel" runat="server">
        <ContentTemplate>
            <div class="p-2 form-inline">
                <asp:DropDownList ID="dropdown_year" runat="server" CssClass="form-control"
                    OnSelectedIndexChanged="dropdown_year_SelectedIndexChanged"
                    AutoPostBack="true" AppendDataBoundItems="true">
                    <asp:ListItem Text="All years" Value="All" Selected="True"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="dropdown_editions" runat="server" CssClass="form-control" 
                    OnSelectedIndexChanged="dropdown_editions_SelectedIndexChanged"
                    AutoPostBack="true" AppendDataBoundItems="true">
                    <asp:ListItem Text="Any edition" Value="Any" Selected="true"></asp:ListItem>
                </asp:DropDownList>
            </div>

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
                    <asp:BoundField DataField="grammarTitle" HeaderText="Title" />
                    <asp:CommandField ButtonType="Link" ShowSelectButton="true" SelectText="Seleccionar" ItemStyle-CssClass="btn" />
                </Columns>
            </asp:GridView>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
