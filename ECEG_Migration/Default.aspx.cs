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
            Stopwatch sw = new Stopwatch(); // Creación del Stopwatch.
            sw.Start(); // Iniciar la medición.

            Grammar[] allGrammarsPartiallyCompleted = DbManager.GetAllGrammars_v2();
            Occupation[] allOccupations = DbManager.GetAllAuthorOccupations();
            Reference[] allReferences = DbManager.GetAllGrammarReferences();
            Library[] allLibraries = DbManager.GetAllGrammarHoldingLibraries();
            SubsidiaryContent[] allSubContents = DbManager.GetAllGrammarSubsidiaryContents();
            City[] allCities = DbManager.GetAllCities();
            County[] allCounties = DbManager.GetAllCounties();
            Country[] allCountries = DbManager.GetAllCountries();

            foreach (Grammar grammar in allGrammarsPartiallyCompleted)
            {
                //Rellenamos las listas
                grammar.GrammarAuthor.Occupations = (from Occupation occ in allOccupations where grammar.GrammarAuthor.Author_id == occ.Author_id select occ).ToArray();
                grammar.GrammarReferences = (from Reference reference in allReferences where grammar.GrammarId == reference.Grammar_id select reference).ToArray();
                grammar.GrammarHoldingLibraries = (from Library lib in allLibraries where grammar.GrammarId == lib.Grammar_id select lib).ToArray();
                grammar.GrammarSubsidiaryContents = (from SubsidiaryContent subc in allSubContents where grammar.GrammarId == subc.Grammar_id select subc).ToArray();

                //Nombramos los paises, ciudades y provincias
                grammar.GrammarAuthor.City_name = (from City city in allCities where grammar.GrammarAuthor.City_id == city.City_id select city.City_name).First();
                grammar.GrammarImprint.City_name = (from City city in allCities where grammar.GrammarImprint.City_id == city.City_id select city.City_name).First();

                grammar.GrammarAuthor.County_name = (from County county in allCounties where grammar.GrammarAuthor.County_id == county.County_id select county.County_name).First();
                grammar.GrammarImprint.County_name = (from County county in allCounties where grammar.GrammarImprint.County_id == county.County_id select county.County_name).First();

                grammar.GrammarAuthor.Country_name = (from Country country in allCountries where grammar.GrammarAuthor.Country_id == country.Country_id select country.Country_name).First();
                grammar.GrammarImprint.Country_name = (from Country country in allCountries where grammar.GrammarImprint.Country_id == country.Country_id select country.Country_name).First();
            }


            sw.Stop(); // Detener la medición.
            Debug.WriteLine("Time elapsed: {0}", sw.Elapsed.ToString("hh\\:mm\\:ss\\.fff")); 

            grammars = DbManager.GetAllGrammarsLite();

            sw.Restart(); // Iniciar la medición.

            ArrayList allGrammarsCompleted = DbManager.GetAllGrammars();

            sw.Stop(); // Detener la medición.
            Debug.WriteLine("Time elapsed: {0}", sw.Elapsed.ToString("hh\\:mm\\:ss\\.fff")); 

            if (Page.IsPostBack)
            {
                return;
            }

            DataSet yearSet = DbManager.GetAllYears();
            dropdown_year.DataSource = yearSet.Tables[0];
            dropdown_year.DataValueField = "YearP";
            dropdown_year.DataTextField = "YearP";
            dropdown_year.DataBind();
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
                string year = dropdown_year.SelectedItem.Value;
                query_AllGrammars.SelectCommand = "SELECT Grammar as id, YearP, Edition, Title from Grammars WHERE YearP = '" + year + "'";
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