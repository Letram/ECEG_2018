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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DataSet yearSet = DbManager.GetAllYears();
                dropdown_year.DataSource = yearSet.Tables[0];
                dropdown_year.DataValueField = "YearP";
                dropdown_year.DataTextField = "YearP";
                dropdown_year.DataBind();

                table_allGrammars.DataSource = dataInstance.AllGrammars;
                table_allGrammars.DataBind();
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
            dataInstance.LastSelectedGrammar = (from Grammar grammar in dataInstance.LastSearchResults where grammar.GrammarId == grammarIndex select grammar).First();
            Response.Redirect("grammar.aspx?grammar=" + grammarIndex.ToString());
        }

        protected void dropdown_year_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string year = dropdown_year.SelectedItem.Value;

                if (year != "All")
                    dataInstance.LastSearchResults = (from Grammar grammar in dataInstance.AllGrammars where grammar.GrammarPublicationYear == year select grammar).ToList();
                else
                    dataInstance.LastSearchResults = dataInstance.AllGrammars;
                table_allGrammars.DataSource = dataInstance.LastSearchResults;
                table_allGrammars.DataBind();
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
        }

        protected void table_allGrammars_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            table_allGrammars.DataSource = dataInstance.LastSearchResults;
            table_allGrammars.PageIndex = e.NewPageIndex;
            table_allGrammars.DataBind();
        }
    }
}