using System;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ECEG_Migration
{
    public partial class Default : System.Web.UI.Page
    {
        //readonly string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = C:\Users\User\source\repos\ECEG_Migration\App_Data\ECEG_2018.mdb";
        readonly string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + HttpContext.Current.Server.MapPath("~/App_Data") + @"\ECEG_2018.mdb;";
        private int clickCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                dbConnection.Open();

                OleDbCommand query = new OleDbCommand("SELECT COUNT(*) FROM Grammars", dbConnection);

                int grammar_entries = (int) query.ExecuteScalar();

                //Label1.Text = grammar_entries.ToString();

            }

        }

        protected void table_AllGrammars_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    e.Row.Cells[3].Text = Truncate(e.Row.Cells[3].Text, 100);
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
        }

        protected void table_AllGrammars_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = table_AllGrammars.SelectedRow;
            Labelcrema.Text = "Row with ID " + row.Cells[0] + " was selected.";
        }
    }
}