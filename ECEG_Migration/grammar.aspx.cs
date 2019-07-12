using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ECEG_Migration
{
    public partial class grammar : System.Web.UI.Page
    {
        readonly string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + HttpContext.Current.Server.MapPath("~/App_Data") + @"\ECEG_2018.mdb;";
        String queryString = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["grammar_id"] = "";
                Session["grammar_ids"] = "";
                queryString = "";

                Session["grammar_id"] = Request.QueryString["grammar"];

                String grammarId = Session["grammar_id"].ToString();
                using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
                {
                    dbConnection.Open();
                   
                    /*
                    OleDbCommand query = new OleDbCommand("SELECT * FROM grammars WHERE Grammar=" + grammarId, dbConnection);
                    OleDbDataReader reader = query.ExecuteReader();

                    if (reader.Read())
                    {
                        //Informacion sin hacer los joins
                        table_item.Rows[0].Cells[0].Text = reader["BD"].ToString();
                        table_item.Rows[0].Cells[1].Text = reader["Grammar"].ToString();
                        table_item.Rows[0].Cells[2].Text = reader["YearP"].ToString();
                        table_item.Rows[0].Cells[3].Text = reader["Edition"].ToString();
                        table_item.Rows[0].Cells[4].Text = reader["Author_id"].ToString();
                        table_item.Rows[0].Cells[5].Text = reader["Title"].ToString();
                        table_item.Rows[0].Cells[6].Text = reader["Type_work"].ToString();
                        table_item.Rows[0].Cells[7].Text = reader["Comments"].ToString();
                        table_item.Rows[0].Cells[8].Text = reader["Division_Grammar"].ToString();
                        table_item.Rows[0].Cells[9].Text = reader["Printers"].ToString();
                        table_item.Rows[0].Cells[10].Text = reader["BookSellers"].ToString();
                        table_item.Rows[0].Cells[11].Text = reader["Price"].ToString();
                        table_item.Rows[0].Cells[12].Text = reader["Target_Audience_Age"].ToString();
                        table_item.Rows[0].Cells[13].Text = reader["Target_Audience_Gender"].ToString();
                        table_item.Rows[0].Cells[14].Text = reader["Target_Audience_Instruction"].ToString();
                        table_item.Rows[0].Cells[15].Text = reader["Target_Audience_SP"].ToString();
                        table_item.Rows[0].Cells[16].Text = reader["Physical_Description"].ToString();
                        table_item.Rows[0].Cells[17].Text = reader["Bibliographical_References"].ToString();
                    }
                    */

                    queryString= "SELECT author_name, gender_text, bio, City, County, Country FROM((((authors INNER JOIN Grammars ON Grammars.Author_id = authors.author_id) INNER JOIN authors_gender ON authors.gender = Authors_Gender.gender_id) INNER JOIN Cities ON authors.PoB_city = Cities.City_Id) INNER JOIN Counties ON authors.PoB_county = Counties.County_Id) INNER JOIN Country ON authors.PoB_country = Country.Country_Id WHERE Grammars.Grammar = " + grammarId;
                    OleDbCommand query = new OleDbCommand(queryString, dbConnection);
                    OleDbDataReader reader = query.ExecuteReader();

                    if(reader.Read()){
                        
                        table_item.Rows[0].Cells[0].Text = reader["author_name"].ToString();
                        table_item.Rows[0].Cells[1].Text = reader["gender_text"].ToString();
                        table_item.Rows[0].Cells[2].Text = reader["bio"].ToString();
                        table_item.Rows[0].Cells[3].Text = reader["City"].ToString();
                        table_item.Rows[0].Cells[4].Text = reader["County"].ToString();
                        table_item.Rows[0].Cells[5].Text = reader["Country"].ToString();                        
                    }

                    query = new OleDbCommand("SELECT grammar FROM grammars ORDER BY grammar ASC", dbConnection);
                    reader = query.ExecuteReader();

                    while (reader.Read())
                    {
                        int readIndex = Convert.ToInt32(reader["grammar"].ToString());
                        Session["grammar_ids"] += reader["grammar"].ToString() + ",";
                    }
                    Session["grammar_ids"] = Session["grammar_ids"].ToString().Remove(Session["grammar_ids"].ToString().Length - 1);
                    String[] grammarArr = Session["grammar_ids"].ToString().Split(',');

                    grammar_page_counter.InnerText = (Array.IndexOf(grammarArr, grammarId) + 1) + " of " + grammarArr.Length;

                    chech_position(Array.IndexOf(grammarArr, grammarId), grammarArr);
                }
            }
        }

        private void chech_position(int v, String[] arr)
        {
            if(v == 0)
                btn_prev.Visible = false;
            else
                btn_prev.Visible = true;

            if(v == arr.Length - 1)
                btn_forw.Visible = false;
            else
                btn_forw.Visible = true;
        }

        protected void btn_prev_ServerClick(object sender, EventArgs e)
        {
            String[] grammarIdsArr = Session["grammar_ids"].ToString().Split(',');
            String grammarId = Session["grammar_id"].ToString();
            int currentIndex = Array.IndexOf(grammarIdsArr, grammarId);
            if (currentIndex != -1 && currentIndex > 0)
                Response.Redirect("grammar?grammar=" + grammarIdsArr[currentIndex - 1]);
            else
                Response.Redirect("grammar?grammar=" + grammarIdsArr[0]);
        }

        protected void btn_forw_ServerClick(object sender, EventArgs e)
        {
            String[] grammarIdsArr = Session["grammar_ids"].ToString().Split(',');
            String grammarId = Session["grammar_id"].ToString();
            int currentIndex = Array.IndexOf(grammarIdsArr, grammarId);
            if (currentIndex != -1 && currentIndex < grammarIdsArr.Length)
                Response.Redirect("grammar?grammar=" + grammarIdsArr[currentIndex + 1]);
            else
                Response.Redirect("grammar?grammar=" + grammarIdsArr[grammarIdsArr.Length - 1]);
        }
    }
}