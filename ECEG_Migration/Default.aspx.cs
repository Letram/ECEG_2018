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
                //year dropdown
                //List<String> years = (
                //    from Grammar grammar in dataInstance.LastSearchResults
                //    select grammar.GrammarPublicationYear)
                //    .Distinct()
                //    .ToList();
                //years.Sort();
                //dropdown_year.DataSource = years;
                //dropdown_year.DataBind();

                //edition dropdown
                //edition is a list of lists since each grammar has a list of editions
                //var editions = (from Grammar grammar in dataInstance.LastSearchResults
                //                select grammar.GrammarEditions.Select(ed => ed.Edition_number))
                //                .ToList();
                ////merging all lists into one and clamping their values.
                //List<int> allEditions = editions
                //    .SelectMany(list => list)
                //    .Distinct()
                //    .ToList();
                //allEditions.Sort();

                //List<String> allEditionsForDropdown = allEditions.Select(edN => edN == -1 ? "N/A" : edN.ToString()).ToList();
                //dropdown_editions.DataSource = allEditionsForDropdown;
                //dropdown_editions.DataBind();

                SetYearDropdown();
                SetEditionDropdown();

                //title thing
                /*****/

                //mainFilter state initialization
                ViewState["mainFilter"] = mainFilter;

                filter();
                //table_allGrammars.DataSource = dataInstance.AllGrammars;
                //table_allGrammars.DataBind();


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
                    e.Row.Cells[2].Text = Truncate(e.Row.Cells[2].Text, 100);

                    Debug.WriteLine(e.Row.Cells[0].Text);
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
            //try
            //{
            //    string year = dropdown_year.SelectedItem.Value;

            //    if (year != "All")
            //        dataInstance.LastSearchResults = (
            //            from Grammar grammar in dataInstance.AllGrammars
            //            where grammar.GrammarPublicationYear == year
            //            select grammar)
            //            .ToList();
            //    else
            //        dataInstance.LastSearchResults = dataInstance.AllGrammars;
            //    table_allGrammars.DataSource = dataInstance.LastSearchResults;
            //    table_allGrammars.DataBind();
            //}
            //catch (Exception ex)
            //{
            //    string mensaje = ex.Message;
            //}

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
            //try
            //{
            //    string edition = dropdown_editions.SelectedItem.Value;

            //    if (edition != "Any")
            //    {
            //        if (edition == "N/A") edition = "-1";
            //        dataInstance.LastSearchResults = (
            //            from Grammar g in dataInstance.AllGrammars
            //            where g.GrammarEditions.Select(ed => ed.Edition_number).Contains(Convert.ToInt32(edition))
            //            select g)
            //            .ToList();
            //    }
            //    else
            //    {

            //        dataInstance.LastSearchResults = dataInstance.AllGrammars;
            //    }
            //    table_allGrammars.DataSource = dataInstance.LastSearchResults;
            //    table_allGrammars.DataBind();
            //}
            //catch (Exception ex)
            //{
            //    string mensaje = ex.Message;
            //}
            if (dropdown_editions.SelectedValue != "Any") mainFilter[1] = 1;
            else mainFilter[1] = 0;
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
                filterTitle();
            }
            else
            {
                //if no filter is selected we just show all entries
                SetYearDropdown();
                SetEditionDropdown();

                dataInstance.LastSearchResults = dataInstance.AllGrammars;
                table_allGrammars.DataSource = dataInstance.LastSearchResults;
                table_allGrammars.DataBind();
            }
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

                    SetEditionDropdown();
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
                    dataInstance.LastSearchResults = (
                        from Grammar g in dataInstance.LastSearchResults
                        where g.GrammarEditions.Select(ed => ed.Edition_number).Contains(Convert.ToInt32(edition))
                        select g)
                        .ToList();
                    table_allGrammars.DataSource = dataInstance.LastSearchResults;
                    table_allGrammars.DataBind();

                    SetYearDropdown();
                }
                catch (Exception ex)
                {
                    string mensaje = ex.Message;
                }
            }
        }

        private void filterTitle()
        {
        }

        /**********************DROPDOWN MANAGEMENT****************************/
        private void SetYearDropdown()
        {
            List<String> years = (
                    from Grammar grammar in dataInstance.LastSearchResults
                    select grammar.GrammarPublicationYear)
                    .Distinct()
                    .ToList();
            years.Sort();

            dropdown_year.DataSource = years;
            dropdown_year.DataBind();
        }

        private void SetEditionDropdown()
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

            List<String> allEditionsForDropdown = allEditions.Select(edN => edN == -1 ? "N/A" : edN.ToString()).ToList();
            dropdown_editions.DataSource = allEditionsForDropdown;
            dropdown_editions.DataBind();
        }
    }
}