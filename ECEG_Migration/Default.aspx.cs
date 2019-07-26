using System;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using ECEG_Migration.Models;
using System.Data;
using System.Diagnostics;
using System.Data.Linq.SqlClient;
using System.Text.RegularExpressions;

namespace ECEG_Migration
{
    public partial class Default : System.Web.UI.Page
    {
        private Data dataInstance = Data.GetInstance();

        private int[] mainFilter = new int[] { 0, 0, 0 };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                SetYearDropdown();
                SetEditionDropdown();

                //title thing
                /*****/

                //mainFilter state initialization
                ViewState["mainFilter"] = mainFilter;

                filter();
            }
            else
            {
                mainFilter = ViewState["mainFilter"] as int[];
            }
        }

        protected void table_AllGrammars_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    //e.Row.Cells[2].Text = Truncate(e.Row.Cells[2].Text, 100);
                }
                catch
                {
                    // No hacemos nada
                }
            }
        }

        private string Truncate(string text, int length)
        {
            if (text.Length <= length) return text;
            return text.Substring(0, length) + "...";
        }

        protected void table_AllGrammars_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Select") return;

            int rowIndex = Convert.ToInt32(e.CommandArgument);
            int grammarIndex = Convert.ToInt32(table_allGrammars.Rows[rowIndex].Cells[0].Text);
            dataInstance.LastSelectedGrammar = (
                from Grammar grammar in dataInstance.LastSearchResults
                where grammar.GrammarId == grammarIndex
                select grammar)
                .First();
            Response.Redirect("grammar.aspx?grammar=" + grammarIndex.ToString());
        }

        protected void dropdown_year_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropdown_year.SelectedValue != "All") mainFilter[0] = 1;
            else mainFilter[0] = 0;
            ViewState["mainFilter"] = mainFilter;
            filter();
        }

        protected void table_allGrammars_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            table_allGrammars.DataSource = dataInstance.LastSearchResults;
            table_allGrammars.PageIndex = e.NewPageIndex;
            table_allGrammars.DataBind();
        }

        protected void dropdown_editions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropdown_editions.SelectedValue != "Any") mainFilter[1] = 1;
            else mainFilter[1] = 0;
            ViewState["mainFilter"] = mainFilter;
            filter();
        }

        protected void titleBtn_Click(object sender, EventArgs e)
        {
            if (input_title.Text != string.Empty) mainFilter[2] = 1;
            else mainFilter[2] = 0;
            ViewState["mainFilter"] = mainFilter;
            filter();
        }

        private void filter()
        {
            dataInstance.LastSearchResults = dataInstance.AllGrammars;

            if (mainFilter.Contains(1))
            {
                //any of the main filter has been triggered (year, edition or title)
                filterYear(dropdown_year.SelectedValue);
                filterEdition(dropdown_editions.SelectedValue);
                filterTitle(input_title.Text);
            }
            else
            {
                //if no filter is selected we just show all entries
                dataInstance.LastSearchResults = dataInstance.AllGrammars;
                table_allGrammars.DataSource = dataInstance.LastSearchResults;
                table_allGrammars.DataBind();
            }

            label_results.Text = String.Format("Showing {0} results out of {1} grammars", dataInstance.LastSearchResults.Count, dataInstance.AllGrammars.Count);
        }
        private void filterYear(string selectedValue)
        {
            if (mainFilter[0] == 1)
            {
                try
                {
                    string year = selectedValue;

                    dataInstance.LastSearchResults = (
                        from Grammar grammar in dataInstance.LastSearchResults
                        where grammar.GrammarPublicationYear == year
                        select grammar)
                        .ToList();
                    table_allGrammars.DataSource = dataInstance.LastSearchResults;
                    table_allGrammars.DataBind();

                    //SetEditionDropdown();
                    MarkAvailableEditions();
                }
                catch (Exception ex)
                {
                    string mensaje = ex.Message;
                }
            }
        }
        private void filterEdition(string selectedValue)
        {
            if (mainFilter[1] == 1)
            {
                try
                {
                    string edition = selectedValue;

                    if (edition == "N/A") edition = "-1";
                    //al final de esto nada xDD TENEMOS QUE FILTRAR POR LA PRIMERA EDICION, ES LA COLUMNA EDITION DE LA TABLA GRAMMARS
                    //if(mainFilter[0] == 1)
                    //    dataInstance.LastSearchResults = (
                    //        from Grammar g in dataInstance.LastSearchResults
                    //        where g.GrammarEditions.Select(ed => new { ed.Edition_number, ed.Edition_year }).Where(tuple => tuple.Edition_year == dropdown_year.SelectedValue).Select(tuple => Convert.ToInt32(tuple.Edition_number)).Contains(Convert.ToInt32(edition))
                    //        select g)
                    //        .ToList();
                    //else
                        dataInstance.LastSearchResults = (
                            from Grammar g in dataInstance.LastSearchResults
                            where g.GrammarEditions.Select(ed => ed.Edition_number).Contains(Convert.ToInt32(edition))
                            select g)
                            .ToList();
                    table_allGrammars.DataSource = dataInstance.LastSearchResults;
                    table_allGrammars.DataBind();

                    //SetYearDropdown();
                    MarkAvailableYears();
                }
                catch (Exception ex)
                {
                    string mensaje = ex.Message;
                }
            }
        }
        private void filterTitle(string writtenValue)
        {
            if(mainFilter[2] == 1)
            {
                try
                {
                    string title = writtenValue;
                    title = title.Replace("*", "");
                    string pattern = @"\b(\w*" + title + @"\w*)\b";
                    dataInstance.LastSearchResults = (
                        from Grammar g in dataInstance.LastSearchResults
                        where Regex.IsMatch(g.GrammarTitle, @title, RegexOptions.IgnoreCase)
                        select g)
                        .ToList();

                    //highlight searched words. in order to do that we need a deep copy of the list so we are not modifying the original
                    var formattedGrammars = new List<Grammar>();

                    foreach(Grammar g in dataInstance.LastSearchResults)
                    {
                        formattedGrammars.Add(g.CustomClone());
                    }

                    table_allGrammars.DataSource = formattedGrammars.Select(grammar =>
                    {
                        grammar.GrammarTitle = Regex.Replace(grammar.GrammarTitle, @pattern, "<b>$1:</b>", RegexOptions.IgnoreCase);
                        return grammar;
                    }).ToList();
                    table_allGrammars.DataBind();
                    
                    MarkAvailableYears();
                    MarkAvailableEditions();
                }
                catch (Exception ex)
                {
                    string mensaje = ex.Message;
                }
            }
        }



        /**********************DROPDOWN MANAGEMENT****************************/
        private void SetYearDropdown()
        {
            List<String> years = (
                    from Grammar grammar in dataInstance.AllGrammars
                    select grammar.GrammarPublicationYear)
                    .Distinct()
                    .ToList();
            years.Sort();

            dropdown_year.DataSource = years;
            dropdown_year.DataBind();
        }

        private void MarkAvailableEditions()
        {
            //edition is a list of lists since each grammar has a list of editions
            var editions = (from Grammar grammar in dataInstance.LastSearchResults
                            select grammar.GrammarEditions.Select(ed => ed.Edition_number))
                            .ToList();
            //merging all lists into one and clamping their values.
            List<int> allEditions = editions
                .SelectMany(list => list)
                .Distinct()
                .ToList();
            allEditions.Sort();

            List<String> allResultEditions = allEditions.Select(edN => edN == -1 ? "N/A" : edN.ToString()).ToList();

            foreach (ListItem item in dropdown_editions.Items)
            {
                if (item.Value == "Any") continue;
                if (allResultEditions.IndexOf(item.Value) == -1)
                    item.Attributes["disabled"] = "disabled";
                else
                    item.Attributes.Remove("disabled");
            }
        }

        private void MarkAvailableYears()
        {
            List<string> allResultYears = (from Grammar g in dataInstance.LastSearchResults select g.GrammarPublicationYear).ToList();

            foreach(ListItem item in dropdown_year.Items)
            {
                if (item.Value == "All") continue;
                if (allResultYears.IndexOf(item.Value) == -1)
                    item.Attributes["disabled"] = "disabled";
                else
                    item.Attributes.Remove("disabled");
            }
        }

        private void SetEditionDropdown()
        {
            //edition is a list of lists since each grammar has a list of editions
            var editions = (from Grammar grammar in dataInstance.AllGrammars
                            select grammar.GrammarEditions.Select(ed => ed.Edition_number))
                            .ToList();
            //merging all lists into one and clamping their values.
            List<int> allEditions = editions
                .SelectMany(list => list)
                .Distinct()
                .ToList();
            allEditions.Sort();

            List<String> allEditionsForDropdown = allEditions.Select(edN => edN == -1 ? "N/A" : edN.ToString()).ToList();
            dropdown_editions.DataSource = allEditionsForDropdown;
            dropdown_editions.DataBind();
        }
    }
}