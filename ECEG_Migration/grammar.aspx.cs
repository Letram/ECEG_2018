using System;
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
        int grammarId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                grammarId = Convert.ToInt32(Request.QueryString["grammar"]);
                query_grammar_id.SelectCommand = "SELECT * FROM grammars WHERE Grammar=" + grammarId.ToString();

                using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
                {
                    dbConnection.Open();

                    OleDbCommand query = new OleDbCommand("SELECT * FROM grammars WHERE Grammar=" + grammarId.ToString(), dbConnection);
                    OleDbDataReader reader = query.ExecuteReader();

                    if (reader.Read())
                    {
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

                }
            }
        }
    }
}