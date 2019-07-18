﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ECEG_Migration.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:AccessDataSource ID="query_AllYears" runat="server" DataFile="~/App_Data/ECEG_2018.mdb" 
    SelectCommand = "SELECT DISTINCT YearP from Grammars ORDER BY YearP"></asp:AccessDataSource>
    <div>
        <asp:DropDownList ID="dropdown_year" runat="server" CssClass="form-control"></asp:DropDownList>
    </div>

    <asp:AccessDataSource ID="query_AllGrammars" runat="server" DataFile="~/App_Data/ECEG_2018.mdb" 
    SelectCommand = "SELECT Grammar as id, YearP, Edition, Title from Grammars ORDER BY Grammar, YearP, Edition"></asp:AccessDataSource>

    <asp:GridView ID="table_AllGrammars" runat="server"
        DataSourceID="query_AllGrammars" 
        ShowHeader="true" 
        AllowPaging="true" 
        PageSize="10"
        AutoGenerateColumns="false"  
        AutoGenerateSelectButton="false"
        GridLines="None"
        Width="100%" 
        EmptyDataText="Data not available..."
        CssClass="table table-responsive all_grammar_table"
        OnRowDataBound="table_AllGrammars_RowDataBound"
        OnRowCommand="table_AllGrammars_RowCommand">
        <Columns>
            <asp:BoundField DataField="id"        HeaderText="ID"                  ItemStyle-Width="5%"  />
            <asp:BoundField DataField="yearP"     HeaderText="Publication year"    ItemStyle-Width="15%" />
            <asp:BoundField DataField="Edition"   HeaderText="Edition"             ItemStyle-Width="10%" />
            <asp:BoundField DataField="Title"     HeaderText="Title"               ItemStyle-Width="70%" />
            <asp:CommandField ButtonType="Link"   ShowSelectButton="true"          SelectText="Seleccionar" ItemStyle-CssClass="btn"/>
        </Columns>
    </asp:GridView>


</asp:Content>