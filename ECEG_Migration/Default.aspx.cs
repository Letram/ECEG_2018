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
        ArrayList grammars = new ArrayList();
        protected void Page_Load(object sender, EventArgs e)
        {
            grammars = DbManager.GetAllGrammarsLite();

            if (Page.IsPostBack)
            {
                return;
            }

            DataSet yearSet = DbManager.GetAllYears();
            dropdown_year.DataSource = yearSet.Tables[0];
            dropdown_year.DataValueField = "YearP";
            dropdown_year.DataTextField = "YearP";
            dropdown_year.DataBind();

            Session["arraylist"] = grammars;
        }

        protected void table_AllGrammars_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    e.Row.Cells[3].Text = Truncate(e.Row.Cells[3].Text, 100);
                    //Grammar grammar = new Grammar(e.Row.Cells[0].Text);
                    //grammar.GrammarPublicationYear = e.Row.Cells[1].Text;
                    //grammars.Add(grammar);

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

            ArrayList pepe = (ArrayList)Session["arraylist"];

            int rowIndex = Convert.ToInt32(e.CommandArgument);
            int grammarIndex = Convert.ToInt32(table_AllGrammars.Rows[rowIndex].Cells[0].Text);
            string[] search_ids = (from Grammar grammar in grammars select grammar.GrammarId.ToString()).ToArray();
            Session["last_search_ids"] = search_ids;
            Response.Redirect("grammar.aspx?grammar=" + grammarIndex.ToString());
        }

        protected void dropdown_year_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //grammars = DbManager.GetAllGrammarsLite();
                string year = dropdown_year.SelectedItem.Value;
                //var query = from Grammar grammar in grammars where grammar.GrammarPublicationYear == year select grammar;
                query_AllGrammars.SelectCommand = "SELECT Grammar as id, YearP, Edition, Title from Grammars WHERE YearP = '" + year + "'";
                // query_AllGrammars.SelectCommand = "SELECT Grammar as id, YearP, Edition, Title from Grammars WHERE YearP = '" + year + "' ORDER BY Grammar, YearP, Edition";
                query_AllGrammars.DataBind();
                table_AllGrammars.DataBind();

                grammars = DbManager.GetAllGrammarsLite();
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
        }
    }
}